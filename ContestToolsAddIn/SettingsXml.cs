using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ContestToolsAddIn
{
	[XmlRoot]
	public class SettingsXml
	{
		public static SettingsXml Load( string fileName )
		{
			using ( var stream = File.OpenRead( fileName ) )
			{
				var result = (SettingsXml)new XmlSerializer( typeof( SettingsXml ) ).Deserialize( stream );
				result._fileName = fileName;
				return result;
			}
		}

		public void Save()
		{
			Save( _fileName );
		}

		public void Save( string fileName )
		{
			using ( var stream = File.Create( fileName ) )
				new XmlSerializer( typeof( SettingsXml ) ).Serialize( stream, this );
		}

		public List<string> Validate()
		{
			var result = new List<string>();

			if ( string.IsNullOrEmpty( ProblemsRootDirectory ) )
			{
				result.Add( "Problem root directory not set" );
			}

			return result;
		}

		[XmlIgnore]
		private string _fileName;

		[XmlElement]
		public string LibraryRootDirectory { get; set; }

		[XmlElement]
		public string ProblemsRootDirectory { get; set; }

		[XmlElement]
		public string LatestContestName { get; set; }

		[XmlElement]
		public string LatestTemplateName { get; set; }

		[XmlElement]
		public bool LatestBasedOnExistingFile { get; set; }

		[XmlElement]
		public string LatestExistingFilePathName { get; set; }

		[XmlArray]
		public ProblemTemplate[] Templates { get; set; }

		[XmlArray]
		public List<string> RecentFiles { get; set; }
	}

	public class ProblemTemplate
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlElement]
		public string Code { get; set; }
	}
}
