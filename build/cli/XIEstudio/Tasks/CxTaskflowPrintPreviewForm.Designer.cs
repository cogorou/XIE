namespace XIEstudio
{
	partial class CxTaskflowPrintPreviewForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskflowPrintPreviewForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolPrint = new System.Windows.Forms.ToolStripButton();
			this.toolPageSetup = new System.Windows.Forms.ToolStripButton();
			this.toolPageNoPrev = new System.Windows.Forms.ToolStripButton();
			this.toolPageNo = new System.Windows.Forms.ToolStripLabel();
			this.toolPageNoNext = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.printPreview = new System.Windows.Forms.PrintPreviewControl();
			this.pageSetup = new System.Windows.Forms.PageSetupDialog();
			this.printDlg = new System.Windows.Forms.PrintDialog();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPrint,
            this.toolPageSetup,
            this.toolPageNoPrev,
            this.toolPageNo,
            this.toolPageNoNext});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(624, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolStrip1";
			// 
			// toolPrint
			// 
			this.toolPrint.AutoSize = false;
			this.toolPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolPrint.Image")));
			this.toolPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPrint.Name = "toolPrint";
			this.toolPrint.Size = new System.Drawing.Size(32, 32);
			this.toolPrint.Text = "Print";
			this.toolPrint.Click += new System.EventHandler(this.toolPrint_Click);
			// 
			// toolPageSetup
			// 
			this.toolPageSetup.AutoSize = false;
			this.toolPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPageSetup.Image = ((System.Drawing.Image)(resources.GetObject("toolPageSetup.Image")));
			this.toolPageSetup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPageSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPageSetup.Name = "toolPageSetup";
			this.toolPageSetup.Size = new System.Drawing.Size(32, 32);
			this.toolPageSetup.Text = "Page Setup";
			this.toolPageSetup.Click += new System.EventHandler(this.toolPageSetup_Click);
			// 
			// toolPageNoPrev
			// 
			this.toolPageNoPrev.AutoSize = false;
			this.toolPageNoPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPageNoPrev.Image = ((System.Drawing.Image)(resources.GetObject("toolPageNoPrev.Image")));
			this.toolPageNoPrev.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPageNoPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPageNoPrev.Name = "toolPageNoPrev";
			this.toolPageNoPrev.Size = new System.Drawing.Size(32, 32);
			this.toolPageNoPrev.Text = "Previous";
			this.toolPageNoPrev.ToolTipText = "Previous";
			this.toolPageNoPrev.Click += new System.EventHandler(this.toolPageNoPrev_Click);
			// 
			// toolPageNo
			// 
			this.toolPageNo.AutoSize = false;
			this.toolPageNo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPageNo.Name = "toolPageNo";
			this.toolPageNo.Size = new System.Drawing.Size(72, 32);
			this.toolPageNo.Text = "00/00";
			// 
			// toolPageNoNext
			// 
			this.toolPageNoNext.AutoSize = false;
			this.toolPageNoNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPageNoNext.Image = ((System.Drawing.Image)(resources.GetObject("toolPageNoNext.Image")));
			this.toolPageNoNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPageNoNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPageNoNext.Name = "toolPageNoNext";
			this.toolPageNoNext.Size = new System.Drawing.Size(32, 32);
			this.toolPageNoNext.Text = "Next";
			this.toolPageNoNext.ToolTipText = "Next";
			this.toolPageNoNext.Click += new System.EventHandler(this.toolPageNoNext_Click);
			// 
			// statusbar
			// 
			this.statusbar.Location = new System.Drawing.Point(0, 420);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(624, 22);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// printPreview
			// 
			this.printPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.printPreview.Location = new System.Drawing.Point(0, 35);
			this.printPreview.Name = "printPreview";
			this.printPreview.Size = new System.Drawing.Size(624, 385);
			this.printPreview.TabIndex = 2;
			// 
			// printDlg
			// 
			this.printDlg.UseEXDialog = true;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// CxTaskflowPrintPreviewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.printPreview);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CxTaskflowPrintPreviewForm";
			this.Text = "Print Preview";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxTaskflowPrintPreviewForm_FormClosing);
			this.Load += new System.EventHandler(this.CxTaskflowPrintPreviewForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.PrintPreviewControl printPreview;
		private System.Windows.Forms.ToolStripButton toolPrint;
		private System.Windows.Forms.ToolStripButton toolPageSetup;
		private System.Windows.Forms.PageSetupDialog pageSetup;
		private System.Windows.Forms.PrintDialog printDlg;
		private System.Windows.Forms.ToolStripButton toolPageNoPrev;
		private System.Windows.Forms.ToolStripLabel toolPageNo;
		private System.Windows.Forms.ToolStripButton toolPageNoNext;
		private System.Windows.Forms.Timer timerUpdateUI;
	}
}