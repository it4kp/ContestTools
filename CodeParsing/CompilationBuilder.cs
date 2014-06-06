using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeParsing
{
	public class CompilationBuilder
	{
		public CompilationBuilder()
		{
			FrameworkVersion = "v4.0.30319";
		}

		public string FrameworkVersion { get; set; }

		private readonly List<SourceFileDescription> _files = new List<SourceFileDescription>();

		public void AddSourceFile( string path )
		{
			_files.Add( new SourceFileDescription
			{
				FileName = Path.GetFullPath( path ),
				Content = File.ReadAllText( path )
			} );
		}

		public void AddSourceText( string source )
		{
			_files.Add( new SourceFileDescription
			{
				FileName = string.Format( "{0}.cs", Guid.NewGuid() ),
				Content = source
			} );
		}

		public CSharpCompilation Build()
		{
			var assemblyPath = Path.GetDirectoryName( typeof( object ).Assembly.Location );
			assemblyPath = Path.Combine( assemblyPath.Substring( 0, assemblyPath.LastIndexOf( Path.DirectorySeparatorChar ) ),
				FrameworkVersion );

			var compilation = CSharpCompilation.Create( Constants.DynamicAssemblyName,
				options: new CSharpCompilationOptions( OutputKind.DynamicallyLinkedLibrary ) )
					.AddReferences( new MetadataFileReference( Path.Combine( assemblyPath, "mscorlib.dll" ) ) )
					.AddReferences( new MetadataFileReference( Path.Combine( assemblyPath, "System.dll" ) ) )
					.AddReferences( new MetadataFileReference( Path.Combine( assemblyPath, "System.Numerics.dll" ) ) );

			if ( FrameworkVersion == "v4.0.30319" )
			{
				compilation = compilation.AddReferences( new MetadataFileReference( Path.Combine( assemblyPath, "System.Core.dll" ) ) );
			}

			foreach ( var file in _files )
			{
				var src = CSharpSyntaxTree.ParseText( file.Content, file.FileName );
				compilation = compilation.AddSyntaxTrees( src );
			}

			return compilation;
		}

		public List<string> ValidateCompilation( CSharpCompilation compilation )
		{
			var errors = compilation.GetDiagnostics().Where( d => d.Severity == DiagnosticSeverity.Error )
				.Select( d => d.GetMessage() ).ToList();
			return errors;
		}
	}
}
