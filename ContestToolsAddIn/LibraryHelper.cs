using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace ContestToolsAddIn
{
	public static class LibraryHelper
	{
		public static List<string> CreateLibrary( string path, out Compilation compilation,
			out List<StaticClassSource> staticClasses, out Dictionary<CodeLocation, StaticMethodSource> methodsMap,
			out List<ClassSource> classes, out Dictionary<CodeLocation, ClassSource> classesMap )
		{
			var sourceFiles = Directory.GetFiles( path, "*.cs", SearchOption.AllDirectories );

			var sources = new List<SyntaxTree>();
			compilation = Compilation.Create( "DynamicLibraryAssembly", new CompilationOptions( OutputKind.DynamicallyLinkedLibrary ) )
															 .AddReferences( MetadataReference.CreateAssemblyReference( "mscorlib" ) )
															 .AddReferences( MetadataReference.CreateAssemblyReference( "System" ) )
															 .AddReferences( MetadataReference.CreateAssemblyReference( "System.Numerics" ) );

			foreach ( var sourceFile in sourceFiles )
			{
				var src = SyntaxTree.ParseFile( sourceFile );
				compilation = compilation.AddSyntaxTrees( src );
				sources.Add( src );
			}

			var errors = compilation.GetDiagnostics().Where( d => d.Info.Severity == DiagnosticSeverity.Error )
				.Select( d => d.Info.GetMessage() ).ToList();
			staticClasses = new List<StaticClassSource>();
			methodsMap = new Dictionary<CodeLocation, StaticMethodSource>();
			classes = new List<ClassSource>();
			classesMap = new Dictionary<CodeLocation, ClassSource>();

			if ( errors.Any() )
				return errors;

			foreach ( var syntaxTree in sources )
			{
				ParseSourceFile( compilation, syntaxTree.GetRoot(), staticClasses, methodsMap, classes, classesMap );
			}

			return null;
		}

		public static List<StaticMethodSource> GetStaticMethods( Compilation libraryCompilation, string sourceCode,
			Dictionary<CodeLocation, StaticMethodSource> methodsMap )
		{
			var tree = SyntaxTree.ParseText( sourceCode );
			var compilation = libraryCompilation.AddSyntaxTrees( tree );
			var queue = new Queue<StaticMethodSource>();
			var visitedMethods = new HashSet<CodeLocation>();

			Traverse( tree.GetRoot(), node =>
																{
																	if ( node is MemberAccessExpressionSyntax )
																	{
																		var model = compilation.GetSemanticModel( node.SyntaxTree );
																		var info = model.GetSymbolInfo( (ExpressionSyntax)node );
																		if ( info.Symbol != null && info.Symbol.Kind == SymbolKind.Method &&
																				info.Symbol.DeclaringSyntaxNodes.Count == 1 )
																		{
																			var methodLocation = new CodeLocation( info.Symbol.DeclaringSyntaxNodes[0] );
																			if ( methodsMap.ContainsKey( methodLocation ) )
																			{
																				if ( !visitedMethods.Contains( methodLocation ) )
																				{
																					visitedMethods.Add( methodLocation );
																					queue.Enqueue( methodsMap[methodLocation] );
																				}
																			}
																		}
																	}
																	return false;
																} );

			var result = new List<StaticMethodSource>();

			while ( queue.Any() )
			{
				var method = queue.Dequeue();
				result.Add( method );
				if ( method.MethodReferences != null )
					foreach ( var methodReference in method.MethodReferences )
					{
						if ( !visitedMethods.Contains( methodReference ) )
						{
							visitedMethods.Add( methodReference );
							queue.Enqueue( methodsMap[methodReference] );
						}
					}
			}

			return result;
		}

		public static string ImportStaticMethodsAndClasses( Compilation libraryCompilation, string sourceCode,
			Dictionary<CodeLocation, StaticMethodSource> methodsMap )
		{
			var methods = GetStaticMethods( libraryCompilation, sourceCode, methodsMap );
			if ( !methods.Any() )
				return sourceCode;
			var builder = new StringBuilder();
			foreach ( var group in methods.GroupBy( m => m.ParentClass.Namespace ) )
			{
				builder.AppendLine( string.Format( "namespace {0}\r\n{{", group.Key ) );

				foreach ( var classGroup in group.GroupBy( m => m.ParentClass.NameWithoutNamespace ) )
				{
					builder.AppendLine( string.Format( "\tstatic class {0}\r\n\t{{", classGroup.Key ) );

					foreach ( var methodSource in classGroup )
					{
						builder.AppendLine( "\t\t" + methodSource.SourceCode );
					}

					builder.AppendLine( "\t}" );
				}

				builder.AppendLine( "}" );
			}

			return sourceCode + Environment.NewLine + builder;
		}

		private static void ParseSourceFile( Compilation compilation, SyntaxNode root, List<StaticClassSource> statics,
			Dictionary<CodeLocation, StaticMethodSource> methodsMap, List<ClassSource> classes, Dictionary<CodeLocation, ClassSource> classesMap )
		{
			if ( root is ClassDeclarationSyntax )
			{
				var r = root as ClassDeclarationSyntax;

				if ( r.Modifiers.Any( t => t.ValueText == "static" ) )
				{
					statics.Add( ParseStaticClass( compilation, r, methodsMap, classes, classesMap ) );
				}
				else
				{
					classes.Add( ParseClass( compilation, r, methodsMap, classes, classesMap ) );
				}

				return;
			}

			foreach ( var child in root.ChildNodes() )
			{
				ParseSourceFile( compilation, child, statics, methodsMap, classes, classesMap );
			}
		}

		private static ClassSource ParseClass( Compilation compilation, ClassDeclarationSyntax r, Dictionary<CodeLocation, StaticMethodSource> methodsMap,
			List<ClassSource> classes, Dictionary<CodeLocation, ClassSource> classesMap )
		{
			var result = new ClassSource
			{
				Name = r.Identifier.ValueText,
				Location = new CodeLocation( r ),
				SourceCode = r.SyntaxTree.GetText().GetSubText( r.Span ).ToString(),
				StaticMethodReferences = ParseStaticMethodReferences( compilation, r ),
				ClassReferences = ParseClassReferences( compilation, r )
			};

			classesMap.Add( result.Location, result );

			if ( r.Parent is NamespaceDeclarationSyntax )
			{
				result.Name = ( r.Parent as NamespaceDeclarationSyntax ).Name + "." + result.Name;
			}

			Traverse( r, node =>
				{

					return false;
				} );

			return result;
		}

		private static StaticClassSource ParseStaticClass( Compilation compilation, ClassDeclarationSyntax r,
			Dictionary<CodeLocation, StaticMethodSource> methodsMap, List<ClassSource> classes, Dictionary<CodeLocation, ClassSource> classesMap )
		{
			var result = new StaticClassSource
			{
				Name = r.Identifier.ValueText,
				Methods = new List<StaticMethodSource>()
			};
			if ( r.Parent is NamespaceDeclarationSyntax )
			{
				result.Name = ( r.Parent as NamespaceDeclarationSyntax ).Name + "." + result.Name;
			}

			foreach ( var member in r.Members )
			{
				if ( member is MethodDeclarationSyntax )
				{
					var method = member as MethodDeclarationSyntax;
					var md = new StaticMethodSource
					{
						Name = method.Identifier.ValueText,
						ParentClass = result,
						SourceCode = method.SyntaxTree.GetText().GetSubText( method.Span ).ToString(),
						MethodReferences = ParseStaticMethodReferences( compilation, method ),
						Location = new CodeLocation( method ),
						ClassReferences = ParseClassReferences( compilation, method )
					};
					methodsMap.Add( md.Location, md );
					result.Methods.Add( md );
				}
			}

			return result;
		}

		private static HashSet<CodeLocation> ParseStaticMethodReferences( Compilation compilation, SyntaxNode root )
		{
			var result = new HashSet<CodeLocation>();

			var model = compilation.GetSemanticModel( root.SyntaxTree );

			Traverse( root, node =>
			{

				if ( node is InvocationExpressionSyntax )
				{
					var member = node as InvocationExpressionSyntax;
					try
					{
						var info = model.GetSymbolInfo( member );
						if ( info.Symbol != null && info.Symbol.DeclaringSyntaxNodes.Count == 1 && info.Symbol.IsStatic )
						{
							result.Add( new CodeLocation( info.Symbol.DeclaringSyntaxNodes[0] ) );
						}
					}
					catch { }
				}
				return false;
			} );

			return result;
		}

		private static HashSet<CodeLocation> ParseClassReferences( Compilation compilation, SyntaxNode root )
		{
			var result = new HashSet<CodeLocation>();

			var model = compilation.GetSemanticModel( root.SyntaxTree );

			Traverse( root, node =>
			{
				try
				{
					var expr = node as ExpressionSyntax;
					if ( expr != null )
					{
						var info = model.GetSymbolInfo( expr );
						if ( info.Symbol != null && info.Symbol.DeclaringSyntaxNodes.Count == 1 )
						{
							if ( info.Symbol.ContainingType != null && !info.Symbol.ContainingType.IsStatic
								&& info.Symbol.ContainingAssembly.BaseName == "DynamicLibraryAssembly" )
								result.Add( new CodeLocation( info.Symbol.DeclaringSyntaxNodes[0] ) );
						}
					}
					else
					{
						var ctor = node as ConstructorInitializerSyntax;
						if ( ctor != null )
						{
							var info = model.GetSymbolInfo( expr );
							if ( info.Symbol != null && info.Symbol.DeclaringSyntaxNodes.Count == 1 )
							{
								result.Add( new CodeLocation( info.Symbol.DeclaringSyntaxNodes[0] ) );
							}
						}
					}
				}
				catch { }
				return false;
			} );

			return result;
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
