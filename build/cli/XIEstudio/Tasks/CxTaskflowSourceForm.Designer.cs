namespace XIEstudio
{
	partial class CxTaskflowSourceForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskflowSourceForm));
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolOutput = new System.Windows.Forms.ToolStripDropDownButton();
			this.splitView = new System.Windows.Forms.SplitContainer();
			this.richOutput = new System.Windows.Forms.RichTextBox();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolFileSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolFind = new System.Windows.Forms.ToolStripButton();
			this.toolGoto = new System.Windows.Forms.ToolStripButton();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitView)).BeginInit();
			this.splitView.Panel2.SuspendLayout();
			this.splitView.SuspendLayout();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusSpacer,
            this.toolOutput});
			this.statusbar.Location = new System.Drawing.Point(0, 530);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(944, 32);
			this.statusbar.TabIndex = 2;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(20, 27);
			this.statusMessage.Text = "...";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(835, 27);
			this.statusSpacer.Spring = true;
			// 
			// toolOutput
			// 
			this.toolOutput.AutoSize = false;
			this.toolOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOutput.Image = ((System.Drawing.Image)(resources.GetObject("toolOutput.Image")));
			this.toolOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOutput.Name = "toolOutput";
			this.toolOutput.Size = new System.Drawing.Size(43, 30);
			this.toolOutput.Text = "Collapse";
			this.toolOutput.Click += new System.EventHandler(this.toolOutput_Click);
			// 
			// splitView
			// 
			this.splitView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitView.Location = new System.Drawing.Point(0, 35);
			this.splitView.Name = "splitView";
			this.splitView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitView.Panel2
			// 
			this.splitView.Panel2.Controls.Add(this.richOutput);
			this.splitView.Size = new System.Drawing.Size(944, 495);
			this.splitView.SplitterDistance = 317;
			this.splitView.TabIndex = 2;
			// 
			// richOutput
			// 
			this.richOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richOutput.Location = new System.Drawing.Point(0, 0);
			this.richOutput.Name = "richOutput";
			this.richOutput.Size = new System.Drawing.Size(944, 174);
			this.richOutput.TabIndex = 0;
			this.richOutput.Text = "";
			this.richOutput.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richOutput_MouseDoubleClick);
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// imageList16
			// 
			this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
			this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList16.Images.SetKeyName(0, "XML");
			this.imageList16.Images.SetKeyName(1, "CPlusPlus");
			this.imageList16.Images.SetKeyName(2, "CSharp");
			this.imageList16.Images.SetKeyName(3, "VisualBasic");
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFileSave,
            this.toolStripSeparator1,
            this.toolFind,
            this.toolGoto});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(944, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolFileSave
			// 
			this.toolFileSave.AutoSize = false;
			this.toolFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFileSave.Image = ((System.Drawing.Image)(resources.GetObject("toolFileSave.Image")));
			this.toolFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFileSave.Name = "toolFileSave";
			this.toolFileSave.Size = new System.Drawing.Size(32, 32);
			this.toolFileSave.Text = "Save";
			this.toolFileSave.Click += new System.EventHandler(this.toolFileSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolFind
			// 
			this.toolFind.AutoSize = false;
			this.toolFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFind.Image = ((System.Drawing.Image)(resources.GetObject("toolFind.Image")));
			this.toolFind.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFind.Name = "toolFind";
			this.toolFind.Size = new System.Drawing.Size(32, 32);
			this.toolFind.Text = "Find";
			this.toolFind.ToolTipText = "Find [Ctrl + F]";
			this.toolFind.Click += new System.EventHandler(this.toolFind_Click);
			// 
			// toolGoto
			// 
			this.toolGoto.AutoSize = false;
			this.toolGoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolGoto.Image = ((System.Drawing.Image)(resources.GetObject("toolGoto.Image")));
			this.toolGoto.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolGoto.Name = "toolGoto";
			this.toolGoto.Size = new System.Drawing.Size(32, 32);
			this.toolGoto.Text = "Goto";
			this.toolGoto.ToolTipText = "Goto [Ctrl + G]";
			this.toolGoto.Click += new System.EventHandler(this.toolGoto_Click);
			// 
			// CxTaskflowSourceForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(944, 562);
			this.Controls.Add(this.splitView);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.KeyPreview = true;
			this.Name = "CxTaskflowSourceForm";
			this.Text = "Script";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxTaskflowSourceForm_FormClosing);
			this.Load += new System.EventHandler(this.CxTaskflowSourceForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditorForm_KeyDown);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitView.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitView)).EndInit();
			this.splitView.ResumeLayout(false);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.SplitContainer splitView;
		private System.Windows.Forms.RichTextBox richOutput;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripDropDownButton toolOutput;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ImageList imageList16;
		private System.Windows.Forms.ToolStripButton toolFind;
		private System.Windows.Forms.ToolStripButton toolGoto;
		private System.Windows.Forms.ToolStripButton toolFileSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}