using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ContestToolsAddIn.Forms
{
	public partial class NewProblemForm : Form
	{
		private readonly SettingsXml _settings;

		public NewProblemForm( SettingsXml settings )
		{
			if ( settings == null )
				throw new ArgumentNullException( "settings" );
			_settings = settings;

			InitializeComponent();
		}

		private void NewProblemForm_Load( object sender, EventArgs e )
		{
			LoadData();
		}

		private void LoadData()
		{
			txtContestName.Text = _settings.LatestContestName;
			txtContestName.AutoCompleteCustomSource = GetContestNames();
			LoadTemplates();
			var selectedItem =
				cbTemplate.Items.Cast<ProblemTemplate>().FirstOrDefault( t => t.Name == _settings.LatestTemplateName );
			if ( selectedItem != null )
				cbTemplate.SelectedItem = selectedItem;
			if ( _settings.LatestBasedOnExistingFile )
			{
				rbExistingFile.Checked = true;
				NewProblemForm_RadioButtonChange( this, null );
			}
		}

		private AutoCompleteStringCollection GetContestNames()
		{
			var result = new AutoCompleteStringCollection();

			if ( !string.IsNullOrEmpty( _settings.ProblemsRootDirectory ) && Directory.Exists( _settings.ProblemsRootDirectory ) )
			{
				result.AddRange( Directory.GetDirectories( _settings.ProblemsRootDirectory, "*", SearchOption.TopDirectoryOnly )
					.Select( Path.GetFileName ).ToArray() );
			}

			return result;
		}

		private void LoadTemplates()
		{
			if ( _settings.Templates == null
				|| _settings.Templates.Length == 0 )
			{
				_settings.Templates = new[] { new ProblemTemplate { Name = "-Empty-", Code = "" } };
			}
			cbTemplate.DataSource = _settings.Templates;
		}

		private void NewProblemForm_RadioButtonChange( object sender, EventArgs e )
		{
			if ( rbNewFile.Checked )
			{
				panelExistingFile.Visible = false;
				panelNewFile.Visible = true;
			}
			else
			{
				panelExistingFile.Visible = true;
				panelNewFile.Visible = false;
			}
		}

		private void btnBrowse_Click( object sender, EventArgs e )
		{
			var dialog = new OpenFileDialog
										{
											CheckFileExists = true,
											Filter = "CSharp source files (*.cs)|*.cs",
											Multiselect = false
										};
			if ( !string.IsNullOrEmpty( _settings.LatestExistingFilePathName ) )
				dialog.InitialDirectory = _settings.LatestExistingFilePathName;
			if ( dialog.ShowDialog() != DialogResult.OK
				|| String.IsNullOrEmpty( dialog.FileName ) ) return;
			_settings.LatestExistingFilePathName = Path.GetDirectoryName( dialog.FileName );

			txtFileName.Text = dialog.FileName;
		}

		private void btnOk_Click( object sender, EventArgs e )
		{
			var errors = Validate();
			if ( errors != null && errors.Count > 0 )
			{
				MessageBox.Show( string.Format( "There are errors:\r\n{0}", string.Join( Environment.NewLine, errors.ToArray() ) ) );
				DialogResult = DialogResult.None;
				return;
			}
			SaveData();
			CreateSourceFile();
		}

		public string SourceFileName { get; private set; }

		public string SubmitFileName { get; private set; }

		private void CreateSourceFile()
		{
			var dir = Path.Combine( _settings.ProblemsRootDirectory, txtContestName.Text );
			if ( !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );
			if ( rbExistingFile.Checked )
			{
				SubmitFileName = txtFileName.Text;
				SourceFileName = Path.Combine( dir, Path.GetFileName( txtFileName.Text ) );
				File.Copy( txtFileName.Text, SourceFileName );
			}
			else
			{
				SourceFileName = Path.Combine( dir, txtProblemName.Text + ".cs" );
				var template = (ProblemTemplate)cbTemplate.SelectedItem;
				SubmitFileName = Path.Combine( Path.Combine( _settings.ProblemsRootDirectory, "_submit" ), txtContestName.Text );
				if ( !Directory.Exists( SubmitFileName ) )
					Directory.CreateDirectory( SubmitFileName );
				SubmitFileName = Path.Combine( SubmitFileName, txtProblemName.Text + ".cs" );

				using ( var stream = File.Create( SourceFileName ) )
				using ( var writer = new StreamWriter( stream ) )
					writer.Write( MakeAllReplacements( template.Code ) );
			}
			AppendPreamble();
		}

		private string MakeAllReplacements( string code )
		{
			const string OUT_FILE_MARKER = "%CONTEST_TOOL_SUBMIT_FILE%";

			code = code.Replace( OUT_FILE_MARKER, SubmitFileName );

			return code;
		}

		private void AppendPreamble()
		{
			File.WriteAllText( SourceFileName, string.Format( "//CONTEST_TOOL_SUBMIT:{0}", SubmitFileName )
				+ Environment.NewLine + File.ReadAllText( SourceFileName ) );
		}

		private new List<string> Validate()
		{
			var result = new List<string>();
			if ( string.IsNullOrEmpty( txtContestName.Text ) )
				result.Add( "Contest name is empty" );
			if ( rbNewFile.Checked )
			{
				if ( string.IsNullOrEmpty( txtProblemName.Text ) )
					result.Add( "Problem name is empty" );
			}
			else
			{
				if ( string.IsNullOrEmpty( txtFileName.Text ) )
					result.Add( "File name is empty" );
				else if ( !File.Exists( txtFileName.Text ) )
					result.Add( "File " + txtFileName.Text + " doesn't exist" );
			}
			return result;
		}

		private void SaveData()
		{
			_settings.LatestContestName = txtContestName.Text;
			_settings.LatestTemplateName = ( (ProblemTemplate)cbTemplate.SelectedItem ).Name;
			_settings.LatestBasedOnExistingFile = rbExistingFile.Checked;
			_settings.Save();
		}

		private void btnManageTemplates_Click( object sender, EventArgs e )
		{
			new TemplatesForm( _settings ).ShowDialog();
			LoadTemplates();
		}
	}
}
