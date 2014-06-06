using System.IO;
using CodeParsing;

namespace ParsingTest
{
	class Program
	{
		static void Main( string[] args )
		{
			var builder = new CompilationBuilder();
			foreach ( var file in Directory.GetFiles( @"D:\docs\Dropbox\projects\kp.Algo\kp.Algo", "*.cs", SearchOption.AllDirectories ) )
			{
				builder.AddSourceFile( file );
			}

			var compilation = builder.Build();

			var importer = new CodeImporter( compilation, location => File.ReadAllText( location.FileName ).Substring( location.Span.Start, location.Span.Length ) );
			var result = importer.ImportCode( @"
using System;
using kp.Algo;
using kp.Algo.Misc;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

class Program{
	public static void Main(){
		var ttt = NumTheoryUtils.SquareRoot(12, 12);
		var m = new Matrix[12];
		var ff = new Matrix(10,10);


		var c1 = new Circle();
		var c2 = new Circle();
		var crosses = Geometry.CrossCircleAndCircle(c1, c2);
		var tt = new Testing();
	}
}

namespace kp.Algo{

class Testing{

}

}
" );


			var errors = importer.Validate( result );

			/*
			Compilation compilation;
			List<StaticClassSource> staticClasses;
			Dictionary<CodeLocation, StaticMethodSource> methodsMap;
			List<ClassSource> classes;
			Dictionary<CodeLocation, ClassSource> classesMap;
			var errors = LibraryHelper.CreateLibrary( @"D:\docs\Dropbox\projects\kp.Algo\kp.Algo", out compilation,
				out staticClasses, out methodsMap, out classes, out classesMap );
			var sourceCode = @"
using System;
using kp.Algo;

class Program{
	public static void Main(){
		var tt = NumTheoryUtils.SquareRoot(12, 12);
	}
}

namespace kp.Algo{}
";
			var newCode = LibraryHelper.ImportStaticMethodsAndClasses( compilation, sourceCode, methodsMap );
			*/
		}
	}
}
