using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContestToolsAddIn.Forms
{
	public partial class RecentFilesForm : Form
	{
		private readonly SettingsXml _settings;

		public RecentFilesForm( SettingsXml settings )
		{
			if ( settings == null )
				throw new ArgumentNullException( "settings" );
			_settings = settings;
			InitializeComponent();
		}

		private void RecentFilesForm_Load( object sender, EventArgs e )
		{
			LoadData();
		}

		private void LoadData()
		{
			lvRecentFiles.Items.Clear();
			if ( _settings.RecentFiles != null )
				foreach ( var file in _settings.RecentFiles )
				{
					lvRecentFiles.Items.Add( new ListViewItem( GetDisplayName( file ) ) { Tag = file } );
				}
		}

		private string GetDisplayName( string file )
		{
			int p = file.LastIndexOf( "\\" );
			if ( p < 1 ) return file;
			p = file.LastIndexOf( "\\", p - 1 );
			if ( p == -1 ) return file;
			return file.Substring( p + 1 );
		}

		private void btnOk_Click( object sender, EventArgs e )
		{
			if ( lvRecentFiles.SelectedItems.Count == 1 )
				SelectedFile = (string)lvRecentFiles.SelectedItems[0].Tag;
		}

		public string SelectedFile { get; set; }

		private void lvRecentFiles_MouseDoubleClick( object sender, MouseEventArgs e )
		{
			if ( lvRecentFiles.SelectedItems.Count == 1 )
			{
				SelectedFile = (string)lvRecentFiles.SelectedItems[0].Tag;
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}
