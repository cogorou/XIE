namespace XIEstudio
{
	partial class CxDataPortSelectionForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxDataPortSelectionForm));
			this.buttonConfigFile = new System.Windows.Forms.Button();
			this.textConfigFile = new System.Windows.Forms.TextBox();
			this.labelConfigFile = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonPluginFile = new System.Windows.Forms.Button();
			this.comboPluginClass = new System.Windows.Forms.ComboBox();
			this.labelPluginClass = new System.Windows.Forms.Label();
			this.labelPluginFile = new System.Windows.Forms.Label();
			this.comboPluginFile = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonConfigFile
			// 
			this.buttonConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonConfigFile.Location = new System.Drawing.Point(502, 80);
			this.buttonConfigFile.Name = "buttonConfigFile";
			this.buttonConfigFile.Size = new System.Drawing.Size(30, 23);
			this.buttonConfigFile.TabIndex = 7;
			this.buttonConfigFile.Text = "...";
			this.buttonConfigFile.UseVisualStyleBackColor = false;
			this.buttonConfigFile.Click += new System.EventHandler(this.buttonConfigFile_Click);
			// 
			// textConfigFile
			// 
			this.textConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textConfigFile.Location = new System.Drawing.Point(106, 84);
			this.textConfigFile.Name = "textConfigFile";
			this.textConfigFile.Size = new System.Drawing.Size(390, 19);
			this.textConfigFile.TabIndex = 6;
			// 
			// labelConfigFile
			// 
			this.labelConfigFile.Location = new System.Drawing.Point(12, 82);
			this.labelConfigFile.Name = "labelConfigFile";
			this.labelConfigFile.Size = new System.Drawing.Size(88, 23);
			this.labelConfigFile.TabIndex = 5;
			this.labelConfigFile.Text = "ConfigFile";
			this.labelConfigFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(334, 132);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonCancel.Location = new System.Drawing.Point(436, 132);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonPluginFile
			// 
			this.buttonPluginFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPluginFile.Location = new System.Drawing.Point(502, 10);
			this.buttonPluginFile.Name = "buttonPluginFile";
			this.buttonPluginFile.Size = new System.Drawing.Size(30, 23);
			this.buttonPluginFile.TabIndex = 2;
			this.buttonPluginFile.Text = "...";
			this.buttonPluginFile.UseVisualStyleBackColor = false;
			this.buttonPluginFile.Click += new System.EventHandler(this.buttonPluginFile_Click);
			// 
			// comboPluginClass
			// 
			this.comboPluginClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPluginClass.FormattingEnabled = true;
			this.comboPluginClass.Location = new System.Drawing.Point(106, 48);
			this.comboPluginClass.Name = "comboPluginClass";
			this.comboPluginClass.Size = new System.Drawing.Size(426, 20);
			this.comboPluginClass.TabIndex = 4;
			this.comboPluginClass.Enter += new System.EventHandler(this.comboPluginClass_Enter);
			// 
			// labelPluginClass
			// 
			this.labelPluginClass.Location = new System.Drawing.Point(12, 46);
			this.labelPluginClass.Name = "labelPluginClass";
			this.labelPluginClass.Size = new System.Drawing.Size(88, 23);
			this.labelPluginClass.TabIndex = 3;
			this.labelPluginClass.Text = "ClassName";
			this.labelPluginClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPluginFile
			// 
			this.labelPluginFile.Location = new System.Drawing.Point(12, 12);
			this.labelPluginFile.Name = "labelPluginFile";
			this.labelPluginFile.Size = new System.Drawing.Size(88, 23);
			this.labelPluginFile.TabIndex = 0;
			this.labelPluginFile.Text = "AssemblyName";
			this.labelPluginFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPluginFile
			// 
			this.comboPluginFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPluginFile.FormattingEnabled = true;
			this.comboPluginFile.Location = new System.Drawing.Point(106, 13);
			this.comboPluginFile.Name = "comboPluginFile";
			this.comboPluginFile.Size = new System.Drawing.Size(390, 20);
			this.comboPluginFile.TabIndex = 1;
			this.comboPluginFile.SelectionChangeCommitted += new System.EventHandler(this.comboPluginFile_SelectionChangeCommitted);
			// 
			// CxDataPortSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(544, 176);
			this.Controls.Add(this.buttonConfigFile);
			this.Controls.Add(this.comboPluginFile);
			this.Controls.Add(this.buttonPluginFile);
			this.Controls.Add(this.textConfigFile);
			this.Controls.Add(this.labelConfigFile);
			this.Controls.Add(this.comboPluginClass);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelPluginFile);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.labelPluginClass);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CxDataPortSelectionForm";
			this.Text = "Plugin Selection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxGrabberSelectForm_FormClosing);
			this.Load += new System.EventHandler(this.CxGrabberSelectForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelPluginFile;
		private System.Windows.Forms.ComboBox comboPluginClass;
		private System.Windows.Forms.Label labelPluginClass;
		private System.Windows.Forms.Button buttonPluginFile;
		private System.Windows.Forms.Button buttonConfigFile;
		private System.Windows.Forms.TextBox textConfigFile;
		private System.Windows.Forms.Label labelConfigFile;
		private System.Windows.Forms.ComboBox comboPluginFile;
	}
}