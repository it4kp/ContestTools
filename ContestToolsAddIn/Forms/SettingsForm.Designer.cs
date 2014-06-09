namespace ContestToolsAddIn.Forms
{
	partial class SettingsForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtProblemRootDirectory = new System.Windows.Forms.TextBox();
			this.btnSelectRootDirectory = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSelectLibraryDirectory = new System.Windows.Forms.Button();
			this.txtLibraryRootDirectory = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Problem root directory:";
			// 
			// txtProblemRootDirectory
			// 
			this.txtProblemRootDirectory.Location = new System.Drawing.Point(15, 27);
			this.txtProblemRootDirectory.Name = "txtProblemRootDirectory";
			this.txtProblemRootDirectory.Size = new System.Drawing.Size(496, 20);
			this.txtProblemRootDirectory.TabIndex = 1;
			// 
			// btnSelectRootDirectory
			// 
			this.btnSelectRootDirectory.Location = new System.Drawing.Point(520, 25);
			this.btnSelectRootDirectory.Name = "btnSelectRootDirectory";
			this.btnSelectRootDirectory.Size = new System.Drawing.Size(38, 23);
			this.btnSelectRootDirectory.TabIndex = 2;
			this.btnSelectRootDirectory.Text = "...";
			this.btnSelectRootDirectory.UseVisualStyleBackColor = true;
			this.btnSelectRootDirectory.Click += new System.EventHandler(this.btnSelectRootDirectory_Click);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(400, 240);
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
			this.btnCancel.Location = new System.Drawing.Point(483, 240);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSelectLibraryDirectory
			// 
			this.btnSelectLibraryDirectory.Location = new System.Drawing.Point(520, 75);
			this.btnSelectLibraryDirectory.Name = "btnSelectLibraryDirectory";
			this.btnSelectLibraryDirectory.Size = new System.Drawing.Size(38, 23);
			this.btnSelectLibraryDirectory.TabIndex = 7;
			this.btnSelectLibraryDirectory.Text = "...";
			this.btnSelectLibraryDirectory.UseVisualStyleBackColor = true;
			this.btnSelectLibraryDirectory.Click += new System.EventHandler(this.btnSelectLibraryDirectory_Click);
			// 
			// txtLibraryRootDirectory
			// 
			this.txtLibraryRootDirectory.Location = new System.Drawing.Point(15, 77);
			this.txtLibraryRootDirectory.Name = "txtLibraryRootDirectory";
			this.txtLibraryRootDirectory.Size = new System.Drawing.Size(496, 20);
			this.txtLibraryRootDirectory.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Library code directory:";
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(576, 276);
			this.Controls.Add(this.btnSelectLibraryDirectory);
			this.Controls.Add(this.txtLibraryRootDirectory);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnSelectRootDirectory);
			this.Controls.Add(this.txtProblemRootDirectory);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "SettingsForm";
			this.Text = "Contest Tools Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtProblemRootDirectory;
		private System.Windows.Forms.Button btnSelectRootDirectory;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSelectLibraryDirectory;
		private System.Windows.Forms.TextBox txtLibraryRootDirectory;
		private System.Windows.Forms.Label label2;
	}
}