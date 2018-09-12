namespace XIEstudio
{
	partial class CxTaskflowImageForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskflowImageForm));
			this.Toolbar = new XIE.Forms.CxToolStripEx();
			this.toolDock = new System.Windows.Forms.ToolStripButton();
			this.toolViewHalftone = new System.Windows.Forms.ToolStripButton();
			this.Statusbar = new System.Windows.Forms.StatusStrip();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusCursorInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.panelView = new System.Windows.Forms.Panel();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.Toolbar.SuspendLayout();
			this.Statusbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// Toolbar
			// 
			this.Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolDock,
            this.toolViewHalftone});
			this.Toolbar.Location = new System.Drawing.Point(0, 0);
			this.Toolbar.Name = "Toolbar";
			this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.Toolbar.Size = new System.Drawing.Size(304, 35);
			this.Toolbar.TabIndex = 0;
			this.Toolbar.Text = "Dock";
			// 
			// toolDock
			// 
			this.toolDock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolDock.AutoSize = false;
			this.toolDock.CheckOnClick = true;
			this.toolDock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDock.Image = ((System.Drawing.Image)(resources.GetObject("toolDock.Image")));
			this.toolDock.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolDock.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDock.Name = "toolDock";
			this.toolDock.Size = new System.Drawing.Size(32, 32);
			this.toolDock.Text = "toolStripButton1";
			this.toolDock.Click += new System.EventHandler(this.toolDock_Click);
			// 
			// toolViewHalftone
			// 
			this.toolViewHalftone.AutoSize = false;
			this.toolViewHalftone.CheckOnClick = true;
			this.toolViewHalftone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewHalftone.Image = ((System.Drawing.Image)(resources.GetObject("toolViewHalftone.Image")));
			this.toolViewHalftone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewHalftone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewHalftone.Name = "toolViewHalftone";
			this.toolViewHalftone.Size = new System.Drawing.Size(32, 32);
			this.toolViewHalftone.Text = "Halftone";
			this.toolViewHalftone.Click += new System.EventHandler(this.toolViewHalftone_Click);
			// 
			// Statusbar
			// 
			this.Statusbar.AutoSize = false;
			this.Statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusSpacer,
            this.statusCursorInfo});
			this.Statusbar.Location = new System.Drawing.Point(0, 247);
			this.Statusbar.Name = "Statusbar";
			this.Statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.Statusbar.Size = new System.Drawing.Size(304, 35);
			this.Statusbar.TabIndex = 1;
			this.Statusbar.Text = "statusStrip1";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(87, 30);
			this.statusSpacer.Spring = true;
			// 
			// statusCursorInfo
			// 
			this.statusCursorInfo.AutoSize = false;
			this.statusCursorInfo.Name = "statusCursorInfo";
			this.statusCursorInfo.Size = new System.Drawing.Size(200, 30);
			this.statusCursorInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelView
			// 
			this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelView.Location = new System.Drawing.Point(0, 35);
			this.panelView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(304, 212);
			this.panelView.TabIndex = 2;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// CxTaskflowImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(304, 282);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.Statusbar);
			this.Controls.Add(this.Toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxTaskflowImageForm";
			this.ShowInTaskbar = false;
			this.Text = "Preview";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxTaskflowImageForm_FormClosing);
			this.Load += new System.EventHandler(this.CxTaskflowImageForm_Load);
			this.Resize += new System.EventHandler(this.CxTaskflowImageForm_Resize);
			this.Toolbar.ResumeLayout(false);
			this.Toolbar.PerformLayout();
			this.Statusbar.ResumeLayout(false);
			this.Statusbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx Toolbar;
		private System.Windows.Forms.ToolStripButton toolDock;
		private System.Windows.Forms.StatusStrip Statusbar;
		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.ToolStripButton toolViewHalftone;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripStatusLabel statusCursorInfo;
		private System.Windows.Forms.Timer timerUpdateUI;

	}
}