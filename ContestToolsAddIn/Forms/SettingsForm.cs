using System;
using System.Windows.Forms;

namespace ContestToolsAddIn.Forms
{
	public partial class SettingsForm : Form
	{
		private readonly SettingsXml _settings;

		public SettingsForm( SettingsXml settings )
		{
			if ( settings == null )
				throw new ArgumentNullException( "settings" );
			_settings = settings;
			InitializeComponent();
		}

		private void btnOk_Click( object sender, EventArgs e )
		{
			SaveData();
		}

		private void SaveData()
		{
			_settings.ProblemsRootDirectory = txtProblemRootDirectory.Text;
			_settings.LibraryRootDirectory = txtLibraryRootDirectory.Text;

			_settings.Save();
		}

		private void SettingsForm_Load( object sender, EventArgs e )
		{
			LoadData();
		}

		private void LoadData()
		{
			txtProblemRootDirectory.Text = _settings.ProblemsRootDirectory;
			txtLibraryRootDirectory.Text = _settings.LibraryRootDirectory;
		}

		private void btnSelectRootDirectory_Click( object sender, EventArgs e )
		{
			var dialog = new FolderBrowserDialog();
			if ( dialog.ShowDialog() == DialogResult.OK )
			{
				txtProblemRootDirectory.Text = dialog.SelectedPath;
			}
		}

		private void btnSelectLibraryDirectory_Click( object sender, EventArgs e )
		{
			var dialog = new FolderBrowserDialog();
			if ( dialog.ShowDialog() == DialogResult.OK )
			{
				txtLibraryRootDirectory.Text = dialog.SelectedPath;
			}
		}
	}
}
