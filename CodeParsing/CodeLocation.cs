using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeParsing
{
	public struct CodeLocation : IEquatable<CodeLocation>
	{
		public readonly string FileName;
		public readonly TextSpan Span;

		public CodeLocation( string fileName, TextSpan span )
		{
			FileName = fileName;
			Span = span;
		}

		public CodeLocation( SyntaxNode node )
		{
			FileName = node.SyntaxTree.FilePath;
			Span = node.Span;
		}

		public static bool operator ==( CodeLocation location1, CodeLocation location2 )
		{
			return location1.Equals( location2 );
		}

		public static bool operator !=( CodeLocation location1, CodeLocation location2 )
		{
			return !location1.Equals( location2 );
		}

		public bool Equals( CodeLocation other )
		{
			return FileName == other.FileName && Span == other.Span;
		}

		public override bool Equals( object obj )
		{
			if ( obj is CodeLocation )
				return Equals( (CodeLocation)obj );
			else
				return false;
		}

		public override int GetHashCode()
		{
			return FileName.GetHashCode() * 3137 + Span.GetHashCode();
		}
	}
}
