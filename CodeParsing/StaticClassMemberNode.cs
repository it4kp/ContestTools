using System;

namespace CodeParsing
{
	public class StaticClassMemberNode : CodeImportNodeBase
	{
		public string ClassName { get; set; }

		public override string ClassNamespace
		{
			get
			{
				if ( string.IsNullOrEmpty( ClassName ) || ClassName.LastIndexOf( ".", StringComparison.InvariantCulture ) == -1 )
					return string.Empty;
				return ClassName.Substring( 0, ClassName.LastIndexOf( ".", StringComparison.InvariantCulture ) );
			}
		}

		public override string ClassNameWithoutNamespace
		{
			get
			{
				if ( string.IsNullOrEmpty( ClassName ) || ClassName.LastIndexOf( ".", StringComparison.InvariantCulture ) == -1 )
					return ClassName;
				return ClassName.Substring( ClassName.LastIndexOf( ".", StringComparison.InvariantCulture ) + 1 );
			}
		}
	}
}
