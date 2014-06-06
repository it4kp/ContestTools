namespace ContestToolsAddIn.Forms
{
	partial class TemplatesForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lvTemplates = new System.Windows.Forms.ListView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tbNew = new System.Windows.Forms.ToolStripButton();
			this.tbDelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbSave = new System.Windows.Forms.ToolStripButton();
			this.rtbTemplateSource = new System.Windows.Forms.RichTextBox();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer1.Panel1.Controls.Add(this.lvTemplates);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.rtbTemplateSource);
			this.splitContainer1.Size = new System.Drawing.Size(980, 589);
			this.splitContainer1.SplitterDistance = 326;
			this.splitContainer1.TabIndex = 1;
			// 
			// lvTemplates
			// 
			this.lvTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvTemplates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvTemplates.FullRowSelect = true;
			this.lvTemplates.GridLines = true;
			this.lvTemplates.HideSelection = false;
			this.lvTemplates.LabelEdit = true;
			this.lvTemplates.Location = new System.Drawing.Point(3, 28);
			this.lvTemplates.MultiSelect = false;
			this.lvTemplates.Name = "lvTemplates";
			this.lvTemplates.Size = new System.Drawing.Size(320, 558);
			this.lvTemplates.TabIndex = 1;
			this.lvTemplates.UseCompatibleStateImageBehavior = false;
			this.lvTemplates.View = System.Windows.Forms.View.Details;
			this.lvTemplates.SelectedIndexChanged += new System.EventHandler(this.lvTemplates_SelectedIndexChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNew,
            this.tbDelete,
            this.toolStripSeparator1,
            this.tbSave});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(326, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tbNew
			// 
			this.tbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbNew.Image = global::ContestToolsAddIn.Properties.Resources.plus;
			this.tbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbNew.Name = "tbNew";
			this.tbNew.Size = new System.Drawing.Size(23, 22);
			this.tbNew.Text = "toolStripButton1";
			this.tbNew.Click += new System.EventHandler(this.tbNew_Click);
			// 
			// tbDelete
			// 
			this.tbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbDelete.Image = global::ContestToolsAddIn.Properties.Resources.delete;
			this.tbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbDelete.Name = "tbDelete";
			this.tbDelete.Size = new System.Drawing.Size(23, 22);
			this.tbDelete.Text = "toolStripButton3";
			this.tbDelete.Click += new System.EventHandler(this.tbDelete_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tbSave
			// 
			this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbSave.Image = global::ContestToolsAddIn.Properties.Resources.accept;
			this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbSave.Name = "tbSave";
			this.tbSave.Size = new System.Drawing.Size(23, 22);
			this.tbSave.Text = "toolStripButton4";
			this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
			// 
			// rtbTemplateSource
			// 
			this.rtbTemplateSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbTemplateSource.Location = new System.Drawing.Point(0, 0);
			this.rtbTemplateSource.Name = "rtbTemplateSource";
			this.rtbTemplateSource.Size = new System.Drawing.Size(650, 589);
			this.rtbTemplateSource.TabIndex = 0;
			this.rtbTemplateSource.Text = "";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 300;
			// 
			// TemplatesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(980, 589);
			this.Controls.Add(this.splitContainer1);
			this.Name = "TemplatesForm";
			this.Text = "Templates";
			this.Load += new System.EventHandler(this.TemplatesForm_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tbNew;
		private System.Windows.Forms.ToolStripButton tbDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tbSave;
		private System.Windows.Forms.ListView lvTemplates;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.RichTextBox rtbTemplateSource;
	}
}