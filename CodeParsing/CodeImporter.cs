using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeParsing
{
	public class CodeImporter
	{
		private readonly CSharpCompilation _compilation;
		private readonly Func<CodeLocation, string> _sourceGetter;

		private readonly Dictionary<CodeLocation, CodeImportNodeBase> _globalNodeCache =
			new Dictionary<CodeLocation, CodeImportNodeBase>();

		public CodeImporter( CSharpCompilation compilation, Func<CodeLocation, string> sourceGetter )
		{
			if ( compilation == null )
				throw new ArgumentNullException( "compilation" );
			if ( sourceGetter == null )
				throw new ArgumentNullException( "sourceGetter" );
			_compilation = compilation;
			_sourceGetter = sourceGetter;
		}

		public string ImportCode( string sourceCode )
		{
			var nodes = GetCodeImportNodes( sourceCode );
			if ( nodes.Count == 0 )
				return sourceCode;
			var queue = new Queue<CodeImportNodeBase>();
			var visited = new HashSet<CodeImportNodeBase>();
			foreach ( var node in nodes )
			{
				queue.Enqueue( node );
				visited.Add( node );
			}

			while ( queue.Any() )
			{
				var curNode = queue.Dequeue();
				foreach ( var child in curNode.Children )
				{
					if ( !visited.Contains( child ) )
					{
						queue.Enqueue( child );
						visited.Add( child );
					}
				}
			}

			return sourceCode + Environment.NewLine + CombineNodesToSource( new List<CodeImportNodeBase>( visited ) );
		}

		public List<string> Validate( string sourceCode )
		{
			var builder = new CompilationBuilder();
			builder.AddSourceText( sourceCode );
			return builder.ValidateCompilation( builder.Build() );
		}

		private string CombineNodesToSource( List<CodeImportNodeBase> nodes )
		{
			var builder = new StringBuilder();
			foreach ( var group in nodes.OfType<StaticClassMemberNode>().GroupBy( n => n.ClassNamespace ) )
			{
				builder.AppendLine( string.Format( "namespace {0}\r\n{{", group.Key ) );

				foreach ( var classGroup in group.GroupBy( m => m.ClassNameWithoutNamespace ) )
				{
					builder.AppendLine( string.Format( "\tstatic class {0}\r\n\t{{", classGroup.Key ) );

					foreach ( var methodSource in classGroup )
					{
						builder.AppendLine( "\t\t" + _sourceGetter( methodSource.Location ) );
					}

					builder.AppendLine( "\t}" );
				}

				builder.AppendLine( "}" );
			}

			foreach ( var group in nodes.OfType<NonStaticClassNode>().GroupBy( n => n.ClassNamespace ) )
			{
				builder.AppendLine( string.Format( "namespace {0}\r\n{{", group.Key ) );

				foreach ( var item in group )
				{
					builder.AppendLine( _sourceGetter( item.Location ) );
				}

				builder.AppendLine( "}" );
			}

			return builder.ToString();
		}

		private List<CodeImportNodeBase> GetChildrenNodes( SyntaxNode root, CSharpCompilation compilation,
			CodeImportNodeBase importNode )
		{
			if ( importNode != null && _globalNodeCache.ContainsKey( importNode.Location ) )
				return importNode.Children;
			var result = new List<CodeImportNodeBase>();
			if ( importNode != null )
			{
				_globalNodeCache.Add( importNode.Location, importNode );
				importNode.Children = result;
			}
			Traverse( root, node =>
			{
				var model = compilation.GetSemanticModel( node.SyntaxTree );
				var info = model.GetSymbolInfo( node );
				if ( info.Symbol != null &&
						info.Symbol.DeclaringSyntaxReferences.Length == 1 )
				{
					var symbolLocation = new CodeLocation( info.Symbol.DeclaringSyntaxReferences[0].SyntaxTree.FilePath,
						info.Symbol.DeclaringSyntaxReferences[0].Span );
					var needImport = info.Symbol.ContainingAssembly.Identity.Name == Constants.DynamicAssemblyName
						&& info.Symbol.DeclaringSyntaxReferences[0].SyntaxTree.FilePath != _thisPath;
					if ( needImport )
					{
						CodeImportNodeBase child = null;
						SyntaxNode childNode = null;
						if ( info.Symbol.IsStatic && info.Symbol.Kind == SymbolKind.Method &&
							info.Symbol.ContainingType.IsStatic )
						{
							child = new StaticClassMemberNode
							{
								Location = symbolLocation,
								Name = info.Symbol.Name,
								ClassName = info.Symbol.ContainingType.ContainingNamespace + "." +
														info.Symbol.ContainingType.Name
							};
							childNode = info.Symbol.DeclaringSyntaxReferences[0].GetSyntax();
						}
						else if ( info.Symbol.IsStatic && info.Symbol.Kind == SymbolKind.Field &&
							info.Symbol.ContainingType.IsStatic )
						{

							child = new StaticClassMemberNode
							{
								Location = new CodeLocation( info.Symbol.DeclaringSyntaxReferences[0].SyntaxTree.FilePath,
									info.Symbol.DeclaringSyntaxReferences[0].GetSyntax().Parent.Parent.Span ),
								Name = info.Symbol.Name,
								ClassName = info.Symbol.ContainingType.ContainingNamespace + "." +
														info.Symbol.ContainingType.Name
							};
							childNode = info.Symbol.DeclaringSyntaxReferences[0].GetSyntax();
						}
						else if ( info.Symbol.ContainingType != null && info.Symbol.Kind == SymbolKind.Method )
						{
							child = new NonStaticClassNode
							{
								Location = new CodeLocation( info.Symbol.DeclaringSyntaxReferences[0].SyntaxTree.FilePath,
									info.Symbol.DeclaringSyntaxReferences[0].GetSyntax().Parent.Span ),
								Name = info.Symbol.ContainingType.ContainingNamespace + "." +
														info.Symbol.ContainingType.Name
							};
							childNode = info.Symbol.DeclaringSyntaxReferences[0].GetSyntax().Parent;
						}
						else if ( info.Symbol.Kind == SymbolKind.NamedType && !info.Symbol.IsStatic )
						{
							child = new NonStaticClassNode
							{
								Location = symbolLocation,
								Name = info.Symbol.ContainingNamespace + "." + info.Symbol.Name
							};
							childNode = info.Symbol.DeclaringSyntaxReferences[0].GetSyntax();
						}
						if ( child != null )
						{
							if ( _globalNodeCache.ContainsKey( child.Location ) )
								result.Add( _globalNodeCache[child.Location] );
							else
							{
								GetChildrenNodes( childNode, compilation, child );
								result.Add( child );
							}
						}
					}
				}

				return false;
			} );

			return result;
		}

		private string _thisPath;
		private List<CodeImportNodeBase> GetCodeImportNodes( string sourceCode )
		{
			_thisPath = Guid.NewGuid() + ".cs";
			var tree = CSharpSyntaxTree.ParseText( sourceCode, _thisPath );
			var compilation = _compilation.AddSyntaxTrees( tree );

			return GetChildrenNodes( tree.GetRoot(), compilation, null );
		}

		static void Traverse( SyntaxNode node, Func<SyntaxNode, bool> visitor )
		{
			if ( visitor( node ) )
				return;
			foreach ( var childNode in node.ChildNodes() )
			{
				Traverse( childNode, visitor );
			}
		}
	}
}
