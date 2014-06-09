namespace ContestToolsAddIn.Forms
{
	partial class NewProblemForm
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
			this.txtContestName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gbProblemSource = new System.Windows.Forms.GroupBox();
			this.rbExistingFile = new System.Windows.Forms.RadioButton();
			this.rbNewFile = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panelNewFile = new System.Windows.Forms.Panel();
			this.btnManageTemplates = new System.Windows.Forms.Button();
			this.cbTemplate = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtProblemName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panelExistingFile = new System.Windows.Forms.Panel();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.gbProblemSource.SuspendLayout();
			this.panelNewFile.SuspendLayout();
			this.panelExistingFile.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtContestName
			// 
			this.txtContestName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtContestName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txtContestName.Location = new System.Drawing.Point(12, 25);
			this.txtContestName.Name = "txtContestName";
			this.txtContestName.Size = new System.Drawing.Size(306, 20);
			this.txtContestName.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Contest:";
			// 
			// gbProblemSource
			// 
			this.gbProblemSource.Controls.Add(this.rbExistingFile);
			this.gbProblemSource.Controls.Add(this.rbNewFile);
			this.gbProblemSource.Location = new System.Drawing.Point(15, 63);
			this.gbProblemSource.Name = "gbProblemSource";
			this.gbProblemSource.Size = new System.Drawing.Size(303, 90);
			this.gbProblemSource.TabIndex = 2;
			this.gbProblemSource.TabStop = false;
			this.gbProblemSource.Text = "Problem based on:";
			// 
			// rbExistingFile
			// 
			this.rbExistingFile.AutoSize = true;
			this.rbExistingFile.Location = new System.Drawing.Point(13, 56);
			this.rbExistingFile.Name = "rbExistingFile";
			this.rbExistingFile.Size = new System.Drawing.Size(77, 17);
			this.rbExistingFile.TabIndex = 1;
			this.rbExistingFile.Text = "Existing file";
			this.rbExistingFile.UseVisualStyleBackColor = true;
			this.rbExistingFile.CheckedChanged += new System.EventHandler(this.NewProblemForm_RadioButtonChange);
			// 
			// rbNewFile
			// 
			this.rbNewFile.AutoSize = true;
			this.rbNewFile.Checked = true;
			this.rbNewFile.Location = new System.Drawing.Point(13, 22);
			this.rbNewFile.Name = "rbNewFile";
			this.rbNewFile.Size = new System.Drawing.Size(63, 17);
			this.rbNewFile.TabIndex = 0;
			this.rbNewFile.TabStop = true;
			this.rbNewFile.Text = "New file";
			this.rbNewFile.UseVisualStyleBackColor = true;
			this.rbNewFile.CheckedChanged += new System.EventHandler(this.NewProblemForm_RadioButtonChange);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(433, 225);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(525, 225);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// panelNewFile
			// 
			this.panelNewFile.Controls.Add(this.btnManageTemplates);
			this.panelNewFile.Controls.Add(this.cbTemplate);
			this.panelNewFile.Controls.Add(this.label3);
			this.panelNewFile.Controls.Add(this.txtProblemName);
			this.panelNewFile.Controls.Add(this.label2);
			this.panelNewFile.Location = new System.Drawing.Point(338, 25);
			this.panelNewFile.Name = "panelNewFile";
			this.panelNewFile.Size = new System.Drawing.Size(261, 161);
			this.panelNewFile.TabIndex = 5;
			// 
			// btnManageTemplates
			// 
			this.btnManageTemplates.Location = new System.Drawing.Point(19, 122);
			this.btnManageTemplates.Name = "btnManageTemplates";
			this.btnManageTemplates.Size = new System.Drawing.Size(193, 24);
			this.btnManageTemplates.TabIndex = 3;
			this.btnManageTemplates.Text = "Manage Templates...";
			this.btnManageTemplates.UseVisualStyleBackColor = true;
			this.btnManageTemplates.Click += new System.EventHandler(this.btnManageTemplates_Click);
			// 
			// cbTemplate
			// 
			this.cbTemplate.DisplayMember = "Name";
			this.cbTemplate.FormattingEnabled = true;
			this.cbTemplate.Location = new System.Drawing.Point(17, 90);
			this.cbTemplate.Name = "cbTemplate";
			this.cbTemplate.Size = new System.Drawing.Size(195, 21);
			this.cbTemplate.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Template:";
			// 
			// txtProblemName
			// 
			this.txtProblemName.Location = new System.Drawing.Point(17, 32);
			this.txtProblemName.Name = "txtProblemName";
			this.txtProblemName.Size = new System.Drawing.Size(195, 20);
			this.txtProblemName.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Problem Name:";
			// 
			// panelExistingFile
			// 
			this.panelExistingFile.Controls.Add(this.btnBrowse);
			this.panelExistingFile.Controls.Add(this.txtFileName);
			this.panelExistingFile.Controls.Add(this.label4);
			this.panelExistingFile.Location = new System.Drawing.Point(338, 25);
			this.panelExistingFile.Name = "panelExistingFile";
			this.panelExistingFile.Size = new System.Drawing.Size(261, 161);
			this.panelExistingFile.TabIndex = 6;
			this.panelExistingFile.Visible = false;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(223, 29);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(25, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(17, 32);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(200, 20);
			this.txtFileName.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(26, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "File:";
			// 
			// NewProblemForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(621, 260);
			this.Controls.Add(this.panelExistingFile);
			this.Controls.Add(this.panelNewFile);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbProblemSource);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtContestName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "NewProblemForm";
			this.Text = "New Problem";
			this.Load += new System.EventHandler(this.NewProblemForm_Load);
			this.gbProblemSource.ResumeLayout(false);
			this.gbProblemSource.PerformLayout();
			this.panelNewFile.ResumeLayout(false);
			this.panelNewFile.PerformLayout();
			this.panelExistingFile.ResumeLayout(false);
			this.panelExistingFile.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtContestName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gbProblemSource;
		private System.Windows.Forms.RadioButton rbExistingFile;
		private System.Windows.Forms.RadioButton rbNewFile;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panelNewFile;
		private System.Windows.Forms.ComboBox cbTemplate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtProblemName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelExistingFile;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnManageTemplates;
	}
}