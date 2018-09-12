namespace XIEcapture
{
	partial class CxCaptureForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxCaptureForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStart = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.comboFps = new System.Windows.Forms.ToolStripComboBox();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusFps = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusTimespan = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.listScreenList = new System.Windows.Forms.ListView();
			this.headerSeqno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerBounds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.toolAudioInput = new System.Windows.Forms.ToolStripButton();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolRefresh,
            this.toolStripSeparator1,
            this.toolStart,
            this.toolStop,
            this.toolStripSeparator2,
            this.toolAudioInput,
            this.comboFps});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(720, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolRefresh
			// 
			this.toolRefresh.AutoSize = false;
			this.toolRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolRefresh.Image")));
			this.toolRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolRefresh.Name = "toolRefresh";
			this.toolRefresh.Size = new System.Drawing.Size(32, 32);
			this.toolRefresh.Text = "Refresh Screen List";
			this.toolRefresh.Click += new System.EventHandler(this.toolRefresh_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolStart
			// 
			this.toolStart.AutoSize = false;
			this.toolStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStart.Image")));
			this.toolStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStart.Name = "toolStart";
			this.toolStart.Size = new System.Drawing.Size(32, 32);
			this.toolStart.Text = "Start Capture";
			this.toolStart.Click += new System.EventHandler(this.toolStart_Click);
			// 
			// toolStop
			// 
			this.toolStop.AutoSize = false;
			this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(32, 32);
			this.toolStop.Text = "Stop Capture";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// comboFps
			// 
			this.comboFps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFps.Name = "comboFps";
			this.comboFps.Size = new System.Drawing.Size(75, 35);
			this.comboFps.SelectedIndexChanged += new System.EventHandler(this.comboFps_SelectedIndexChanged);
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusSpacer,
            this.statusFps,
            this.statusTimespan});
			this.statusbar.Location = new System.Drawing.Point(0, 579);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(720, 23);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusbar";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(44, 18);
			this.statusMessage.Text = "Ready";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(547, 18);
			this.statusSpacer.Spring = true;
			// 
			// statusFps
			// 
			this.statusFps.Name = "statusFps";
			this.statusFps.Size = new System.Drawing.Size(54, 18);
			this.statusFps.Text = "[ 0 fps ]";
			// 
			// statusTimespan
			// 
			this.statusTimespan.Name = "statusTimespan";
			this.statusTimespan.Size = new System.Drawing.Size(60, 18);
			this.statusTimespan.Text = "00:00:00";
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.IsSplitterFixed = true;
			this.splitMain.Location = new System.Drawing.Point(0, 35);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.listScreenList);
			this.splitMain.Size = new System.Drawing.Size(720, 544);
			this.splitMain.SplitterDistance = 335;
			this.splitMain.TabIndex = 2;
			// 
			// listScreenList
			// 
			this.listScreenList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerSeqno,
            this.headerName,
            this.headerBounds});
			this.listScreenList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listScreenList.FullRowSelect = true;
			this.listScreenList.GridLines = true;
			this.listScreenList.Location = new System.Drawing.Point(0, 0);
			this.listScreenList.MultiSelect = false;
			this.listScreenList.Name = "listScreenList";
			this.listScreenList.Size = new System.Drawing.Size(720, 205);
			this.listScreenList.TabIndex = 1;
			this.listScreenList.UseCompatibleStateImageBehavior = false;
			this.listScreenList.View = System.Windows.Forms.View.Details;
			this.listScreenList.SelectedIndexChanged += new System.EventHandler(this.listScreenList_SelectedIndexChanged);
			// 
			// headerSeqno
			// 
			this.headerSeqno.Text = "No";
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			this.headerName.Width = 438;
			// 
			// headerBounds
			// 
			this.headerBounds.Text = "Bounds";
			this.headerBounds.Width = 200;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// toolAudioInput
			// 
			this.toolAudioInput.AutoSize = false;
			this.toolAudioInput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAudioInput.Image = ((System.Drawing.Image)(resources.GetObject("toolAudioInput.Image")));
			this.toolAudioInput.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolAudioInput.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAudioInput.Name = "toolAudioInput";
			this.toolAudioInput.Size = new System.Drawing.Size(32, 32);
			this.toolAudioInput.Text = "Audio Input";
			this.toolAudioInput.Click += new System.EventHandler(this.toolAudioInput_Click);
			// 
			// CxCaptureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(720, 602);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.Name = "CxCaptureForm";
			this.Text = "XIE-Capture";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxCaptureForm_FormClosing);
			this.Load += new System.EventHandler(this.CxCaptureForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.ToolStripButton toolRefresh;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ListView listScreenList;
		private System.Windows.Forms.ColumnHeader headerSeqno;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.ColumnHeader headerBounds;
		private System.Windows.Forms.ToolStripButton toolStart;
		private System.Windows.Forms.ToolStripButton toolStop;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripStatusLabel statusTimespan;
		private System.Windows.Forms.ToolStripStatusLabel statusFps;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripComboBox comboFps;
		private System.Windows.Forms.ToolStripButton toolAudioInput;
	}
}

