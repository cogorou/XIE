namespace XIEstudio
{
	partial class CxTcpClientSelectForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTcpClientSelectForm));
			this.checkResolve = new System.Windows.Forms.CheckBox();
			this.groupResolve = new System.Windows.Forms.GroupBox();
			this.numericIndex = new System.Windows.Forms.NumericUpDown();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelHostname = new System.Windows.Forms.Label();
			this.comboHostname = new System.Windows.Forms.ComboBox();
			this.groupIP = new System.Windows.Forms.GroupBox();
			this.numericPort = new System.Windows.Forms.NumericUpDown();
			this.labelPort = new System.Windows.Forms.Label();
			this.groupResolve.SuspendLayout();
			this.groupIP.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkResolve
			// 
			this.checkResolve.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkResolve.Location = new System.Drawing.Point(22, 25);
			this.checkResolve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.checkResolve.Name = "checkResolve";
			this.checkResolve.Size = new System.Drawing.Size(84, 40);
			this.checkResolve.TabIndex = 0;
			this.checkResolve.Text = "Auto";
			this.checkResolve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkResolve.UseVisualStyleBackColor = false;
			this.checkResolve.CheckedChanged += new System.EventHandler(this.checkResolve_CheckedChanged);
			// 
			// groupResolve
			// 
			this.groupResolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupResolve.Controls.Add(this.numericIndex);
			this.groupResolve.Controls.Add(this.checkResolve);
			this.groupResolve.Location = new System.Drawing.Point(17, 126);
			this.groupResolve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.groupResolve.Name = "groupResolve";
			this.groupResolve.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.groupResolve.Size = new System.Drawing.Size(255, 79);
			this.groupResolve.TabIndex = 1;
			this.groupResolve.TabStop = false;
			this.groupResolve.Text = "Resolve";
			// 
			// numericIndex
			// 
			this.numericIndex.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericIndex.Location = new System.Drawing.Point(118, 30);
			this.numericIndex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericIndex.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numericIndex.Name = "numericIndex";
			this.numericIndex.Size = new System.Drawing.Size(121, 23);
			this.numericIndex.TabIndex = 1;
			this.numericIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(285, 237);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(112, 40);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonCancel.Location = new System.Drawing.Point(404, 237);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(112, 40);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelHostname
			// 
			this.labelHostname.Location = new System.Drawing.Point(20, 24);
			this.labelHostname.Name = "labelHostname";
			this.labelHostname.Size = new System.Drawing.Size(86, 29);
			this.labelHostname.TabIndex = 0;
			this.labelHostname.Text = "Hostname";
			this.labelHostname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboHostname
			// 
			this.comboHostname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboHostname.FormattingEnabled = true;
			this.comboHostname.Location = new System.Drawing.Point(118, 26);
			this.comboHostname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboHostname.Name = "comboHostname";
			this.comboHostname.Size = new System.Drawing.Size(361, 23);
			this.comboHostname.TabIndex = 1;
			this.comboHostname.SelectedIndexChanged += new System.EventHandler(this.comboHostname_SelectedIndexChanged);
			// 
			// groupIP
			// 
			this.groupIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupIP.Controls.Add(this.numericPort);
			this.groupIP.Controls.Add(this.comboHostname);
			this.groupIP.Controls.Add(this.labelPort);
			this.groupIP.Controls.Add(this.labelHostname);
			this.groupIP.Location = new System.Drawing.Point(17, 15);
			this.groupIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.groupIP.Name = "groupIP";
			this.groupIP.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.groupIP.Size = new System.Drawing.Size(498, 103);
			this.groupIP.TabIndex = 0;
			this.groupIP.TabStop = false;
			this.groupIP.Text = "IP";
			// 
			// numericPort
			// 
			this.numericPort.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericPort.Location = new System.Drawing.Point(118, 61);
			this.numericPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericPort.Name = "numericPort";
			this.numericPort.Size = new System.Drawing.Size(121, 23);
			this.numericPort.TabIndex = 3;
			this.numericPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelPort
			// 
			this.labelPort.Location = new System.Drawing.Point(20, 62);
			this.labelPort.Name = "labelPort";
			this.labelPort.Size = new System.Drawing.Size(86, 29);
			this.labelPort.TabIndex = 2;
			this.labelPort.Text = "Port";
			this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CxTcpClientSelectForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 292);
			this.Controls.Add(this.groupIP);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupResolve);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CxTcpClientSelectForm";
			this.Text = "IP Selection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IPSelectForm_FormClosing);
			this.Load += new System.EventHandler(this.IPSelectForm_Load);
			this.groupResolve.ResumeLayout(false);
			this.groupIP.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox checkResolve;
		private System.Windows.Forms.GroupBox groupResolve;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelHostname;
		private System.Windows.Forms.ComboBox comboHostname;
		private System.Windows.Forms.GroupBox groupIP;
		private System.Windows.Forms.NumericUpDown numericPort;
		private System.Windows.Forms.Label labelPort;
		private System.Windows.Forms.NumericUpDown numericIndex;
	}
}