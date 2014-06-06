using System.Collections.Generic;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace ContestToolsAddIn
{
	public class StaticClassSource
	{
		public string Name { get; set; }

		public List<StaticMethodSource> Methods { get; set; }

		public string Namespace
		{
			get
			{
				if ( string.IsNullOrEmpty( Name ) || Name.LastIndexOf( "." ) == -1 )
					return string.Empty;
				return Name.Substring( 0, Name.LastIndexOf( "." ) );
			}
		}

		public string NameWithoutNamespace
		{
			get
			{
				if ( string.IsNullOrEmpty( Name ) || Name.LastIndexOf( "." ) == -1 )
					return Name;
				return Name.Substring( Name.LastIndexOf( "." ) + 1 );
			}
		}
	}

	public class StaticMethodSource
	{
		public StaticClassSource ParentClass { get; set; }
		public string Name { get; set; }
		public HashSet<CodeLocation> MethodReferences { get; set; }
		public string SourceCode { get; set; }
		public CodeLocation Location { get; set; }
		public HashSet<CodeLocation> ClassReferences { get; set; }
	}

	public struct CodeLocation
	{
		public readonly string FileName;
		public readonly TextSpan Span;

		public CodeLocation( SyntaxNode node )
		{
			FileName = node.SyntaxTree.FilePath;
			Span = node.Span;
		}
	}

	public class ClassSource
	{
		public string Name { get; set; }
		public HashSet<CodeLocation> StaticMethodReferences { get; set; }
		public string SourceCode { get; set; }
		public CodeLocation Location { get; set; }
		public HashSet<CodeLocation> ClassReferences { get; set; }
	}
}
