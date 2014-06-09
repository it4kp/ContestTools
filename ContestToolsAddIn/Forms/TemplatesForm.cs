using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ContestToolsAddIn.Forms
{
	public partial class TemplatesForm : Form
	{
		private readonly SettingsXml _settings;
		private readonly ScintillaNET.Scintilla _rtbTemplateSource;

		public TemplatesForm( SettingsXml settings )
		{
			if ( settings == null )
				throw new ArgumentNullException( "settings" );

			_settings = settings;
			InitializeComponent();

			_rtbTemplateSource = new ScintillaNET.Scintilla
			{
				Parent = splitContainer1.Panel2,
				ConfigurationManager = { Language = "cs" },
				Dock = DockStyle.Fill
			};
		}

		private void TemplatesForm_Load( object sender, EventArgs e )
		{
			LoadData();
		}

		private void LoadData()
		{
			lvTemplates.Items.Clear();
			foreach ( var template in _settings.Templates )
			{
				lvTemplates.Items.Add( new ListViewItem( template.Name ) { Tag = template.Code } );
			}
		}

		private void lvTemplates_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( lvTemplates.SelectedItems.Count == 0 )
			{
				_rtbTemplateSource.Text = string.Empty;
			}
			else
			{
				_rtbTemplateSource.Text = (string)lvTemplates.SelectedItems[0].Tag;
			}
		}

		private void tbNew_Click( object sender, EventArgs e )
		{
			if ( lvTemplates.SelectedItems.Count == 0 )
			{
				return;
			}
			lvTemplates.Items.Add( new ListViewItem( "New Template" ) { Tag = string.Empty } );
		}

		private void tbDelete_Click( object sender, EventArgs e )
		{
			if ( lvTemplates.SelectedItems.Count == 0 )
			{
				return;
			}
			lvTemplates.Items.Remove( lvTemplates.SelectedItems[0] );
		}

		private void tbSave_Click( object sender, EventArgs e )
		{
			if ( lvTemplates.SelectedItems.Count > 0 )
			{
				lvTemplates.SelectedItems[0].Tag = _rtbTemplateSource.Text;
			}
			var templates = new List<ProblemTemplate>();
			foreach ( ListViewItem item in lvTemplates.Items )
			{
				templates.Add( new ProblemTemplate { Name = item.SubItems[0].Text, Code = (string)item.Tag } );
			}
			_settings.Templates = templates.ToArray();
			_settings.Save();
		}
	}
}
