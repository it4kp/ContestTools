using System;
using System.Collections.Generic;

namespace CodeParsing
{
	public abstract class CodeImportNodeBase : IEquatable<CodeImportNodeBase>
	{
		public CodeLocation Location { get; set; }

		public List<CodeImportNodeBase> Children { get; set; }

		public string Name { get; set; }

		public virtual string ClassNamespace
		{
			get
			{
				if ( string.IsNullOrEmpty( Name ) || Name.LastIndexOf( ".", StringComparison.InvariantCulture ) == -1 )
					return string.Empty;
				return Name.Substring( 0, Name.LastIndexOf( ".", StringComparison.InvariantCulture ) );
			}
		}

		public virtual string ClassNameWithoutNamespace
		{
			get
			{
				if ( string.IsNullOrEmpty( Name ) || Name.LastIndexOf( ".", StringComparison.InvariantCulture ) == -1 )
					return Name;
				return Name.Substring( Name.LastIndexOf( ".", StringComparison.InvariantCulture ) + 1 );
			}
		}

		public bool Equals( CodeImportNodeBase other )
		{
			if ( (object)other == null )
				return false;
			return Location == other.Location;
		}

		public override bool Equals( object obj )
		{
			var a = obj as CodeImportNodeBase;
			if ( a != null )
				return Equals( a );
			return false;
		}

		public override int GetHashCode()
		{
			return Location.GetHashCode();
		}

		public static bool operator ==( CodeImportNodeBase node1, CodeImportNodeBase node2 )
		{
			if ( (object)node1 == null )
				return (object)node2 == null;
			return node1.Equals( node2 );
		}

		public static bool operator !=( CodeImportNodeBase node1, CodeImportNodeBase node2 )
		{
			if ( (object)node1 == null )
				return (object)node2 != null;
			return !node1.Equals( node2 );
		}
	}
}
