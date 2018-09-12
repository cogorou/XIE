namespace XIE.Tasks
{
	partial class CxScriptEditorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxScriptEditorForm));
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolOutput = new System.Windows.Forms.ToolStripDropDownButton();
			this.splitView = new System.Windows.Forms.SplitContainer();
			this.richOutput = new System.Windows.Forms.RichTextBox();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.toolProperty = new System.Windows.Forms.ToolStripButton();
			this.toolImports = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolBuild = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEditCut = new System.Windows.Forms.ToolStripButton();
			this.toolEditCopy = new System.Windows.Forms.ToolStripButton();
			this.toolEditPaste = new System.Windows.Forms.ToolStripButton();
			this.toolEditDel = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEditUndo = new System.Windows.Forms.ToolStripButton();
			this.toolEditRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
			this.statusbar.Size = new System.Drawing.Size(784, 32);
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
			this.statusSpacer.Size = new System.Drawing.Size(706, 27);
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
			this.toolOutput.Text = "toolStripDropDownButton1";
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
			this.splitView.Size = new System.Drawing.Size(784, 495);
			this.splitView.SplitterDistance = 289;
			this.splitView.SplitterWidth = 8;
			this.splitView.TabIndex = 2;
			// 
			// richOutput
			// 
			this.richOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richOutput.Location = new System.Drawing.Point(0, 0);
			this.richOutput.Name = "richOutput";
			this.richOutput.Size = new System.Drawing.Size(784, 198);
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
            this.toolCancel,
            this.toolOK,
            this.toolProperty,
            this.toolImports,
            this.toolStripSeparator4,
            this.toolBuild,
            this.toolStripSeparator1,
            this.toolEditCut,
            this.toolEditCopy,
            this.toolEditPaste,
            this.toolEditDel,
            this.toolStripSeparator3,
            this.toolEditUndo,
            this.toolEditRedo,
            this.toolStripSeparator2,
            this.toolFind,
            this.toolGoto});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(784, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
			this.toolCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCancel.Name = "toolCancel";
			this.toolCancel.Size = new System.Drawing.Size(72, 32);
			this.toolCancel.Text = "Cancel";
			this.toolCancel.Click += new System.EventHandler(this.toolCancel_Click);
			// 
			// toolOK
			// 
			this.toolOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOK.AutoSize = false;
			this.toolOK.Image = ((System.Drawing.Image)(resources.GetObject("toolOK.Image")));
			this.toolOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(72, 32);
			this.toolOK.Text = "OK";
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// toolProperty
			// 
			this.toolProperty.AutoSize = false;
			this.toolProperty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolProperty.Image")));
			this.toolProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolProperty.Name = "toolProperty";
			this.toolProperty.Size = new System.Drawing.Size(32, 32);
			this.toolProperty.Text = "Property";
			this.toolProperty.Click += new System.EventHandler(this.toolProperty_Click);
			// 
			// toolImports
			// 
			this.toolImports.AutoSize = false;
			this.toolImports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolImports.Image = ((System.Drawing.Image)(resources.GetObject("toolImports.Image")));
			this.toolImports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolImports.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolImports.Name = "toolImports";
			this.toolImports.Size = new System.Drawing.Size(32, 32);
			this.toolImports.Text = "Imports";
			this.toolImports.ToolTipText = "Imports";
			this.toolImports.Click += new System.EventHandler(this.toolImports_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
			// 
			// toolBuild
			// 
			this.toolBuild.AutoSize = false;
			this.toolBuild.Image = ((System.Drawing.Image)(resources.GetObject("toolBuild.Image")));
			this.toolBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolBuild.Name = "toolBuild";
			this.toolBuild.Size = new System.Drawing.Size(72, 32);
			this.toolBuild.Text = "Build";
			this.toolBuild.ToolTipText = "Build [F7]";
			this.toolBuild.Click += new System.EventHandler(this.toolBuild_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolEditCut
			// 
			this.toolEditCut.AutoSize = false;
			this.toolEditCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditCut.Image = ((System.Drawing.Image)(resources.GetObject("toolEditCut.Image")));
			this.toolEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditCut.Name = "toolEditCut";
			this.toolEditCut.Size = new System.Drawing.Size(32, 32);
			this.toolEditCut.Text = "Cut";
			this.toolEditCut.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditCut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditCut.ToolTipText = "Cut [Ctrl + X]";
			this.toolEditCut.Click += new System.EventHandler(this.toolEditCut_Click);
			// 
			// toolEditCopy
			// 
			this.toolEditCopy.AutoSize = false;
			this.toolEditCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolEditCopy.Image")));
			this.toolEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditCopy.Name = "toolEditCopy";
			this.toolEditCopy.Size = new System.Drawing.Size(32, 32);
			this.toolEditCopy.Text = "Copy";
			this.toolEditCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditCopy.ToolTipText = "Copy [Ctrl + C]";
			this.toolEditCopy.Click += new System.EventHandler(this.toolEditCopy_Click);
			// 
			// toolEditPaste
			// 
			this.toolEditPaste.AutoSize = false;
			this.toolEditPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolEditPaste.Image")));
			this.toolEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditPaste.Name = "toolEditPaste";
			this.toolEditPaste.Size = new System.Drawing.Size(32, 32);
			this.toolEditPaste.Text = "Paste";
			this.toolEditPaste.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditPaste.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditPaste.ToolTipText = "Paste [Ctrl + V]";
			this.toolEditPaste.Click += new System.EventHandler(this.toolEditPaste_Click);
			// 
			// toolEditDel
			// 
			this.toolEditDel.AutoSize = false;
			this.toolEditDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditDel.Image = ((System.Drawing.Image)(resources.GetObject("toolEditDel.Image")));
			this.toolEditDel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditDel.Name = "toolEditDel";
			this.toolEditDel.Size = new System.Drawing.Size(32, 32);
			this.toolEditDel.Text = "Delete";
			this.toolEditDel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditDel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditDel.Click += new System.EventHandler(this.toolEditDel_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
			// 
			// toolEditUndo
			// 
			this.toolEditUndo.AutoSize = false;
			this.toolEditUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolEditUndo.Image")));
			this.toolEditUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditUndo.Name = "toolEditUndo";
			this.toolEditUndo.Size = new System.Drawing.Size(32, 32);
			this.toolEditUndo.Text = "Undo";
			this.toolEditUndo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditUndo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditUndo.ToolTipText = "Undo [Ctrl + Z]";
			this.toolEditUndo.Click += new System.EventHandler(this.toolEditUndo_Click);
			// 
			// toolEditRedo
			// 
			this.toolEditRedo.AutoSize = false;
			this.toolEditRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolEditRedo.Image")));
			this.toolEditRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditRedo.Name = "toolEditRedo";
			this.toolEditRedo.Size = new System.Drawing.Size(32, 32);
			this.toolEditRedo.Text = "Redo";
			this.toolEditRedo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.toolEditRedo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolEditRedo.ToolTipText = "Redo [Ctrl + Y]";
			this.toolEditRedo.Click += new System.EventHandler(this.toolEditRedo_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
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
			// CxScriptEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.splitView);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.KeyPreview = true;
			this.Name = "CxScriptEditorForm";
			this.Text = "Script";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxScriptEditorForm_FormClosing);
			this.Load += new System.EventHandler(this.CxScriptEditorForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CxScriptEditorForm_KeyDown);
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
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.SplitContainer splitView;
		private System.Windows.Forms.RichTextBox richOutput;
		private System.Windows.Forms.ToolStripButton toolBuild;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripDropDownButton toolOutput;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolEditCut;
		private System.Windows.Forms.ToolStripButton toolEditCopy;
		private System.Windows.Forms.ToolStripButton toolEditPaste;
		private System.Windows.Forms.ToolStripButton toolEditDel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolEditUndo;
		private System.Windows.Forms.ToolStripButton toolEditRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ImageList imageList16;
		private System.Windows.Forms.ToolStripButton toolFind;
		private System.Windows.Forms.ToolStripButton toolGoto;
		private System.Windows.Forms.ToolStripButton toolImports;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolProperty;
	}
}