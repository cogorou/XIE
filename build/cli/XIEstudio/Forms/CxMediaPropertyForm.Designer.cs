namespace XIEstudio
{
	partial class CxMediaPropertyForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxMediaPropertyForm));
			this.propertyParam = new System.Windows.Forms.PropertyGrid();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolRun = new System.Windows.Forms.ToolStripButton();
			this.toolPause = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolReset = new System.Windows.Forms.ToolStripButton();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.MainTab = new System.Windows.Forms.TabControl();
			this.tabPageSeeking = new System.Windows.Forms.TabPage();
			this.groupStopPosition = new System.Windows.Forms.GroupBox();
			this.trackStopPosition = new System.Windows.Forms.TrackBar();
			this.numericStopPosition = new System.Windows.Forms.NumericUpDown();
			this.groupStartPosition = new System.Windows.Forms.GroupBox();
			this.trackStartPosition = new System.Windows.Forms.TrackBar();
			this.numericStartPosition = new System.Windows.Forms.NumericUpDown();
			this.tabPageProperty = new System.Windows.Forms.TabPage();
			this.groupCurrentPosition = new System.Windows.Forms.GroupBox();
			this.textCurrentPosition = new System.Windows.Forms.TextBox();
			this.trackCurrentPosition = new System.Windows.Forms.TrackBar();
			this.toolbar.SuspendLayout();
			this.MainTab.SuspendLayout();
			this.tabPageSeeking.SuspendLayout();
			this.groupStopPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackStopPosition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericStopPosition)).BeginInit();
			this.groupStartPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackStartPosition)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericStartPosition)).BeginInit();
			this.tabPageProperty.SuspendLayout();
			this.groupCurrentPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackCurrentPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// propertyParam
			// 
			this.propertyParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyParam.Location = new System.Drawing.Point(3, 3);
			this.propertyParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.propertyParam.Name = "propertyParam";
			this.propertyParam.Size = new System.Drawing.Size(610, 267);
			this.propertyParam.TabIndex = 5;
			// 
			// statusbar
			// 
			this.statusbar.Location = new System.Drawing.Point(0, 340);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(624, 22);
			this.statusbar.TabIndex = 4;
			this.statusbar.Text = "statusStrip1";
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolRun,
            this.toolPause,
            this.toolStop,
            this.toolStripSeparator1,
            this.toolReset});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(624, 35);
			this.toolbar.TabIndex = 3;
			this.toolbar.Text = "toolbar";
			// 
			// toolRun
			// 
			this.toolRun.AutoSize = false;
			this.toolRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolRun.Image = ((System.Drawing.Image)(resources.GetObject("toolRun.Image")));
			this.toolRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolRun.Name = "toolRun";
			this.toolRun.Size = new System.Drawing.Size(32, 32);
			this.toolRun.Text = "Run";
			this.toolRun.Click += new System.EventHandler(this.toolRun_Click);
			// 
			// toolPause
			// 
			this.toolPause.AutoSize = false;
			this.toolPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPause.Image = ((System.Drawing.Image)(resources.GetObject("toolPause.Image")));
			this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPause.Name = "toolPause";
			this.toolPause.Size = new System.Drawing.Size(32, 32);
			this.toolPause.Text = "Pause";
			this.toolPause.Click += new System.EventHandler(this.toolPause_Click);
			// 
			// toolStop
			// 
			this.toolStop.AutoSize = false;
			this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(32, 32);
			this.toolStop.Text = "Stop";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolReset
			// 
			this.toolReset.AutoSize = false;
			this.toolReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolReset.Image = ((System.Drawing.Image)(resources.GetObject("toolReset.Image")));
			this.toolReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolReset.Name = "toolReset";
			this.toolReset.Size = new System.Drawing.Size(32, 32);
			this.toolReset.Text = "Reset";
			this.toolReset.Click += new System.EventHandler(this.toolReset_Click);
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// MainTab
			// 
			this.MainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.MainTab.Controls.Add(this.tabPageSeeking);
			this.MainTab.Controls.Add(this.tabPageProperty);
			this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTab.ItemSize = new System.Drawing.Size(96, 24);
			this.MainTab.Location = new System.Drawing.Point(0, 35);
			this.MainTab.Name = "MainTab";
			this.MainTab.SelectedIndex = 0;
			this.MainTab.Size = new System.Drawing.Size(624, 305);
			this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.MainTab.TabIndex = 6;
			// 
			// tabPageSeeking
			// 
			this.tabPageSeeking.Controls.Add(this.groupCurrentPosition);
			this.tabPageSeeking.Controls.Add(this.groupStopPosition);
			this.tabPageSeeking.Controls.Add(this.groupStartPosition);
			this.tabPageSeeking.Location = new System.Drawing.Point(4, 28);
			this.tabPageSeeking.Name = "tabPageSeeking";
			this.tabPageSeeking.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSeeking.Size = new System.Drawing.Size(616, 273);
			this.tabPageSeeking.TabIndex = 0;
			this.tabPageSeeking.Text = "Seeking";
			this.tabPageSeeking.UseVisualStyleBackColor = true;
			// 
			// groupStopPosition
			// 
			this.groupStopPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupStopPosition.Controls.Add(this.trackStopPosition);
			this.groupStopPosition.Controls.Add(this.numericStopPosition);
			this.groupStopPosition.Location = new System.Drawing.Point(8, 146);
			this.groupStopPosition.Name = "groupStopPosition";
			this.groupStopPosition.Size = new System.Drawing.Size(600, 64);
			this.groupStopPosition.TabIndex = 2;
			this.groupStopPosition.TabStop = false;
			this.groupStopPosition.Text = "Stop Position";
			// 
			// trackStopPosition
			// 
			this.trackStopPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackStopPosition.LargeChange = 10;
			this.trackStopPosition.Location = new System.Drawing.Point(141, 13);
			this.trackStopPosition.Maximum = 100;
			this.trackStopPosition.Name = "trackStopPosition";
			this.trackStopPosition.Size = new System.Drawing.Size(453, 45);
			this.trackStopPosition.TabIndex = 1;
			this.trackStopPosition.TickFrequency = 10;
			this.trackStopPosition.Scroll += new System.EventHandler(this.trackStopPosition_Scroll);
			// 
			// numericStopPosition
			// 
			this.numericStopPosition.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericStopPosition.Location = new System.Drawing.Point(15, 24);
			this.numericStopPosition.Name = "numericStopPosition";
			this.numericStopPosition.Size = new System.Drawing.Size(120, 23);
			this.numericStopPosition.TabIndex = 0;
			this.numericStopPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericStopPosition.ValueChanged += new System.EventHandler(this.numericStopPosition_ValueChanged);
			// 
			// groupStartPosition
			// 
			this.groupStartPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupStartPosition.Controls.Add(this.trackStartPosition);
			this.groupStartPosition.Controls.Add(this.numericStartPosition);
			this.groupStartPosition.Location = new System.Drawing.Point(8, 76);
			this.groupStartPosition.Name = "groupStartPosition";
			this.groupStartPosition.Size = new System.Drawing.Size(600, 64);
			this.groupStartPosition.TabIndex = 1;
			this.groupStartPosition.TabStop = false;
			this.groupStartPosition.Text = "Start Position";
			// 
			// trackStartPosition
			// 
			this.trackStartPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackStartPosition.LargeChange = 10;
			this.trackStartPosition.Location = new System.Drawing.Point(141, 13);
			this.trackStartPosition.Maximum = 100;
			this.trackStartPosition.Name = "trackStartPosition";
			this.trackStartPosition.Size = new System.Drawing.Size(453, 45);
			this.trackStartPosition.TabIndex = 1;
			this.trackStartPosition.TickFrequency = 10;
			this.trackStartPosition.Value = 5;
			this.trackStartPosition.Scroll += new System.EventHandler(this.trackStartPosition_Scroll);
			// 
			// numericStartPosition
			// 
			this.numericStartPosition.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericStartPosition.Location = new System.Drawing.Point(15, 24);
			this.numericStartPosition.Name = "numericStartPosition";
			this.numericStartPosition.Size = new System.Drawing.Size(120, 23);
			this.numericStartPosition.TabIndex = 0;
			this.numericStartPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericStartPosition.ValueChanged += new System.EventHandler(this.numericStartPosition_ValueChanged);
			// 
			// tabPageProperty
			// 
			this.tabPageProperty.Controls.Add(this.propertyParam);
			this.tabPageProperty.Location = new System.Drawing.Point(4, 28);
			this.tabPageProperty.Name = "tabPageProperty";
			this.tabPageProperty.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageProperty.Size = new System.Drawing.Size(616, 273);
			this.tabPageProperty.TabIndex = 1;
			this.tabPageProperty.Text = "Property";
			this.tabPageProperty.UseVisualStyleBackColor = true;
			// 
			// groupCurrentPosition
			// 
			this.groupCurrentPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCurrentPosition.Controls.Add(this.trackCurrentPosition);
			this.groupCurrentPosition.Controls.Add(this.textCurrentPosition);
			this.groupCurrentPosition.Location = new System.Drawing.Point(8, 6);
			this.groupCurrentPosition.Name = "groupCurrentPosition";
			this.groupCurrentPosition.Size = new System.Drawing.Size(600, 64);
			this.groupCurrentPosition.TabIndex = 0;
			this.groupCurrentPosition.TabStop = false;
			this.groupCurrentPosition.Text = "Current Position";
			// 
			// textCurrentPosition
			// 
			this.textCurrentPosition.Location = new System.Drawing.Point(15, 26);
			this.textCurrentPosition.Name = "textCurrentPosition";
			this.textCurrentPosition.ReadOnly = true;
			this.textCurrentPosition.Size = new System.Drawing.Size(120, 19);
			this.textCurrentPosition.TabIndex = 0;
			this.textCurrentPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// trackCurrentPosition
			// 
			this.trackCurrentPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackCurrentPosition.Enabled = false;
			this.trackCurrentPosition.Location = new System.Drawing.Point(141, 13);
			this.trackCurrentPosition.Name = "trackCurrentPosition";
			this.trackCurrentPosition.Size = new System.Drawing.Size(453, 45);
			this.trackCurrentPosition.TabIndex = 1;
			// 
			// CxMediaPropertyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 362);
			this.Controls.Add(this.MainTab);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Name = "CxMediaPropertyForm";
			this.Text = "Media Property";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxMediaPropertyForm_FormClosing);
			this.Load += new System.EventHandler(this.CxMediaPropertyForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.MainTab.ResumeLayout(false);
			this.tabPageSeeking.ResumeLayout(false);
			this.groupStopPosition.ResumeLayout(false);
			this.groupStopPosition.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackStopPosition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericStopPosition)).EndInit();
			this.groupStartPosition.ResumeLayout(false);
			this.groupStartPosition.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackStartPosition)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericStartPosition)).EndInit();
			this.tabPageProperty.ResumeLayout(false);
			this.groupCurrentPosition.ResumeLayout(false);
			this.groupCurrentPosition.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackCurrentPosition)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PropertyGrid propertyParam;
		private System.Windows.Forms.StatusStrip statusbar;
		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.ToolStripButton toolRun;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripButton toolPause;
		private System.Windows.Forms.ToolStripButton toolStop;
		private System.Windows.Forms.TabControl MainTab;
		private System.Windows.Forms.TabPage tabPageSeeking;
		private System.Windows.Forms.GroupBox groupStopPosition;
		private System.Windows.Forms.TrackBar trackStopPosition;
		private System.Windows.Forms.NumericUpDown numericStopPosition;
		private System.Windows.Forms.GroupBox groupStartPosition;
		private System.Windows.Forms.TrackBar trackStartPosition;
		private System.Windows.Forms.NumericUpDown numericStartPosition;
		private System.Windows.Forms.TabPage tabPageProperty;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolReset;
		private System.Windows.Forms.GroupBox groupCurrentPosition;
		private System.Windows.Forms.TextBox textCurrentPosition;
		private System.Windows.Forms.TrackBar trackCurrentPosition;
	}
}