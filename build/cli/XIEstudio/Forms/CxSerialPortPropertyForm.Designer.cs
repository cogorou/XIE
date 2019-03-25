namespace XIEstudio
{
	partial class CxSerialPortPropertyForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxSerialPortPropertyForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolConnect = new System.Windows.Forms.ToolStripButton();
			this.buttonCancel = new System.Windows.Forms.ToolStripButton();
			this.buttonOK = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEncoding = new System.Windows.Forms.ToolStripComboBox();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.labelCTS = new System.Windows.Forms.ToolStripStatusLabel();
			this.labelDSR = new System.Windows.Forms.ToolStripStatusLabel();
			this.labelCD = new System.Windows.Forms.ToolStripStatusLabel();
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.textRecv = new System.Windows.Forms.RichTextBox();
			this.buttonSend = new System.Windows.Forms.Button();
			this.textSend = new System.Windows.Forms.TextBox();
			this.propertyParam = new System.Windows.Forms.PropertyGrid();
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolConnect,
            this.buttonCancel,
            this.buttonOK,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.toolClear,
            this.toolStripSeparator3,
            this.toolEncoding});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(681, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolConnect
			// 
			this.toolConnect.AutoSize = false;
			this.toolConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolConnect.Image")));
			this.toolConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolConnect.Name = "toolConnect";
			this.toolConnect.Size = new System.Drawing.Size(32, 32);
			this.toolConnect.Text = "Connect";
			this.toolConnect.Click += new System.EventHandler(this.toolConnect_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonCancel.AutoSize = false;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(72, 32);
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.buttonOK.AutoSize = false;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 32);
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// toolClear
			// 
			this.toolClear.AutoSize = false;
			this.toolClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolClear.Image = ((System.Drawing.Image)(resources.GetObject("toolClear.Image")));
			this.toolClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClear.Name = "toolClear";
			this.toolClear.Size = new System.Drawing.Size(32, 32);
			this.toolClear.Text = "Clear";
			this.toolClear.Click += new System.EventHandler(this.toolClear_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
			// 
			// toolEncoding
			// 
			this.toolEncoding.Items.AddRange(new object[] {
            "Default",
            "ASCII",
            "UTF8",
            "UTF7",
            "UTF32",
            "Unicode",
            "BigEndianUnicode"});
			this.toolEncoding.Name = "toolEncoding";
			this.toolEncoding.Size = new System.Drawing.Size(121, 35);
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusInfo,
            this.labelCTS,
            this.labelDSR,
            this.labelCD});
			this.statusbar.Location = new System.Drawing.Point(0, 549);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(681, 29);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusInfo
			// 
			this.statusInfo.Name = "statusInfo";
			this.statusInfo.Size = new System.Drawing.Size(441, 24);
			this.statusInfo.Spring = true;
			this.statusInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCTS
			// 
			this.labelCTS.AutoSize = false;
			this.labelCTS.Name = "labelCTS";
			this.labelCTS.Size = new System.Drawing.Size(64, 24);
			this.labelCTS.Text = "CTS";
			// 
			// labelDSR
			// 
			this.labelDSR.AutoSize = false;
			this.labelDSR.Name = "labelDSR";
			this.labelDSR.Size = new System.Drawing.Size(64, 24);
			this.labelDSR.Text = "DSR";
			// 
			// labelCD
			// 
			this.labelCD.AutoSize = false;
			this.labelCD.Name = "labelCD";
			this.labelCD.Size = new System.Drawing.Size(64, 24);
			this.labelCD.Text = "CD";
			// 
			// imageList16
			// 
			this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
			this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList16.Images.SetKeyName(0, "Property");
			this.imageList16.Images.SetKeyName(1, "Test");
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// textRecv
			// 
			this.textRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textRecv.Location = new System.Drawing.Point(3, 50);
			this.textRecv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.textRecv.Name = "textRecv";
			this.textRecv.Size = new System.Drawing.Size(402, 459);
			this.textRecv.TabIndex = 6;
			this.textRecv.Text = "";
			// 
			// buttonSend
			// 
			this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSend.Location = new System.Drawing.Point(322, 12);
			this.buttonSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonSend.Name = "buttonSend";
			this.buttonSend.Size = new System.Drawing.Size(84, 29);
			this.buttonSend.TabIndex = 4;
			this.buttonSend.Text = "Send";
			this.buttonSend.UseVisualStyleBackColor = true;
			this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
			// 
			// textSend
			// 
			this.textSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSend.Location = new System.Drawing.Point(3, 16);
			this.textSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.textSend.Name = "textSend";
			this.textSend.Size = new System.Drawing.Size(311, 23);
			this.textSend.TabIndex = 3;
			this.textSend.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textSend_PreviewKeyDown);
			// 
			// propertyParam
			// 
			this.propertyParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyParam.Location = new System.Drawing.Point(0, 0);
			this.propertyParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.propertyParam.Name = "propertyParam";
			this.propertyParam.Size = new System.Drawing.Size(256, 514);
			this.propertyParam.TabIndex = 7;
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitter.Location = new System.Drawing.Point(0, 35);
			this.splitter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitter.Name = "splitter";
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.propertyParam);
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2.Controls.Add(this.textSend);
			this.splitter.Panel2.Controls.Add(this.textRecv);
			this.splitter.Panel2.Controls.Add(this.buttonSend);
			this.splitter.Size = new System.Drawing.Size(681, 514);
			this.splitter.SplitterDistance = 256;
			this.splitter.SplitterWidth = 5;
			this.splitter.TabIndex = 8;
			// 
			// CxSerialPortPropertyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(681, 578);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxSerialPortPropertyForm";
			this.Text = "SerialPort";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortSelectForm_FormClosing);
			this.Load += new System.EventHandler(this.SerialPortSelectForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel2.ResumeLayout(false);
			this.splitter.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripStatusLabel statusInfo;
		private System.Windows.Forms.ImageList imageList16;
		private System.Windows.Forms.ToolStripButton toolConnect;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		public System.Windows.Forms.ToolStripButton buttonCancel;
		public System.Windows.Forms.ToolStripButton buttonOK;
		private System.Windows.Forms.RichTextBox textRecv;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox textSend;
		private System.Windows.Forms.PropertyGrid propertyParam;
		private System.Windows.Forms.SplitContainer splitter;
		private System.Windows.Forms.ToolStripButton toolClear;
		private System.Windows.Forms.ToolStripStatusLabel labelCTS;
		private System.Windows.Forms.ToolStripStatusLabel labelDSR;
		private System.Windows.Forms.ToolStripStatusLabel labelCD;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripComboBox toolEncoding;
	}
}