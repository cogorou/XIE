namespace XIEprompt
{
	partial class CxPromptForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxPromptForm));
			this.textPrompt = new System.Windows.Forms.RichTextBox();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.toolbarMain = new XIE.Forms.CxToolStripEx();
			this.toolLogForm = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolFileOpen = new System.Windows.Forms.ToolStripButton();
			this.toolFileSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolBuild = new System.Windows.Forms.ToolStripButton();
			this.toolTaskStart = new System.Windows.Forms.ToolStripButton();
			this.toolTaskStop = new System.Windows.Forms.ToolStripButton();
			this.toolPromptClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEditUndo = new System.Windows.Forms.ToolStripButton();
			this.toolEditRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEditCut = new System.Windows.Forms.ToolStripButton();
			this.toolEditCopy = new System.Windows.Forms.ToolStripButton();
			this.toolEditPaste = new System.Windows.Forms.ToolStripButton();
			this.toolEditDel = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolFind = new System.Windows.Forms.ToolStripButton();
			this.toolGoto = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolVersion = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolOutput = new System.Windows.Forms.ToolStripDropDownButton();
			this.splitView = new System.Windows.Forms.SplitContainer();
			this.tabSource = new System.Windows.Forms.TabControl();
			this.tabPageSource1 = new System.Windows.Forms.TabPage();
			this.richOutput = new System.Windows.Forms.RichTextBox();
			this.imageListTab = new System.Windows.Forms.ImageList(this.components);
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.panelArgs = new System.Windows.Forms.Panel();
			this.labelArgs = new System.Windows.Forms.Label();
			this.textArgs = new System.Windows.Forms.TextBox();
			this.toolbarMain.SuspendLayout();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitView)).BeginInit();
			this.splitView.Panel1.SuspendLayout();
			this.splitView.Panel2.SuspendLayout();
			this.splitView.SuspendLayout();
			this.tabSource.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.panelArgs.SuspendLayout();
			this.SuspendLayout();
			// 
			// textPrompt
			// 
			this.textPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.textPrompt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textPrompt.ForeColor = System.Drawing.Color.LimeGreen;
			this.textPrompt.Location = new System.Drawing.Point(0, 0);
			this.textPrompt.Name = "textPrompt";
			this.textPrompt.Size = new System.Drawing.Size(944, 120);
			this.textPrompt.TabIndex = 1;
			this.textPrompt.Text = "";
			this.textPrompt.TextChanged += new System.EventHandler(this.textPrompt_TextChanged);
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// toolbarMain
			// 
			this.toolbarMain.BackColor = System.Drawing.SystemColors.Control;
			this.toolbarMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLogForm,
            this.toolStripSeparator6,
            this.toolFileOpen,
            this.toolFileSave,
            this.toolStripSeparator3,
            this.toolBuild,
            this.toolTaskStart,
            this.toolTaskStop,
            this.toolPromptClear,
            this.toolStripSeparator7,
            this.toolEditUndo,
            this.toolEditRedo,
            this.toolStripSeparator2,
            this.toolEditCut,
            this.toolEditCopy,
            this.toolEditPaste,
            this.toolEditDel,
            this.toolStripSeparator5,
            this.toolFind,
            this.toolGoto,
            this.toolStripSeparator1,
            this.toolVersion});
			this.toolbarMain.Location = new System.Drawing.Point(0, 0);
			this.toolbarMain.Name = "toolbarMain";
			this.toolbarMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbarMain.Size = new System.Drawing.Size(944, 35);
			this.toolbarMain.TabIndex = 2;
			this.toolbarMain.Text = "toolbar";
			// 
			// toolLogForm
			// 
			this.toolLogForm.AutoSize = false;
			this.toolLogForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLogForm.Image = ((System.Drawing.Image)(resources.GetObject("toolLogForm.Image")));
			this.toolLogForm.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLogForm.Name = "toolLogForm";
			this.toolLogForm.Size = new System.Drawing.Size(32, 32);
			this.toolLogForm.Text = "Log";
			this.toolLogForm.ToolTipText = "Log";
			this.toolLogForm.Click += new System.EventHandler(this.toolLogForm_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 35);
			// 
			// toolFileOpen
			// 
			this.toolFileOpen.AutoSize = false;
			this.toolFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolFileOpen.Image")));
			this.toolFileOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFileOpen.Name = "toolFileOpen";
			this.toolFileOpen.Size = new System.Drawing.Size(32, 32);
			this.toolFileOpen.Text = "Open";
			this.toolFileOpen.Click += new System.EventHandler(this.toolFileOpen_Click);
			// 
			// toolFileSave
			// 
			this.toolFileSave.AutoSize = false;
			this.toolFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFileSave.Image = ((System.Drawing.Image)(resources.GetObject("toolFileSave.Image")));
			this.toolFileSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFileSave.Name = "toolFileSave";
			this.toolFileSave.Size = new System.Drawing.Size(32, 32);
			this.toolFileSave.Text = "Save";
			this.toolFileSave.ToolTipText = "Save [Ctrl+S]";
			this.toolFileSave.Click += new System.EventHandler(this.toolFileSave_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
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
			// toolTaskStart
			// 
			this.toolTaskStart.AutoSize = false;
			this.toolTaskStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolTaskStart.Image = ((System.Drawing.Image)(resources.GetObject("toolTaskStart.Image")));
			this.toolTaskStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolTaskStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolTaskStart.Name = "toolTaskStart";
			this.toolTaskStart.Size = new System.Drawing.Size(32, 32);
			this.toolTaskStart.Text = "Start";
			this.toolTaskStart.ToolTipText = "Start [F5]";
			this.toolTaskStart.Click += new System.EventHandler(this.toolTaskStart_Click);
			// 
			// toolTaskStop
			// 
			this.toolTaskStop.AutoSize = false;
			this.toolTaskStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolTaskStop.Image = ((System.Drawing.Image)(resources.GetObject("toolTaskStop.Image")));
			this.toolTaskStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolTaskStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolTaskStop.Name = "toolTaskStop";
			this.toolTaskStop.Size = new System.Drawing.Size(32, 32);
			this.toolTaskStop.Text = "Stop";
			this.toolTaskStop.Click += new System.EventHandler(this.toolTaskStop_Click);
			// 
			// toolPromptClear
			// 
			this.toolPromptClear.AutoSize = false;
			this.toolPromptClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPromptClear.Image = ((System.Drawing.Image)(resources.GetObject("toolPromptClear.Image")));
			this.toolPromptClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPromptClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPromptClear.Name = "toolPromptClear";
			this.toolPromptClear.Size = new System.Drawing.Size(32, 32);
			this.toolPromptClear.Text = "Clear";
			this.toolPromptClear.Click += new System.EventHandler(this.toolPromptClear_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 35);
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
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 35);
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolVersion
			// 
			this.toolVersion.AutoSize = false;
			this.toolVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolVersion.Image = ((System.Drawing.Image)(resources.GetObject("toolVersion.Image")));
			this.toolVersion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolVersion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolVersion.Name = "toolVersion";
			this.toolVersion.Size = new System.Drawing.Size(32, 32);
			this.toolVersion.Text = "Version";
			this.toolVersion.Click += new System.EventHandler(this.toolVersion_Click);
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusSpacer,
            this.toolOutput});
			this.statusbar.Location = new System.Drawing.Point(0, 659);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(944, 23);
			this.statusbar.TabIndex = 3;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(13, 18);
			this.statusMessage.Text = "-";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(873, 18);
			this.statusSpacer.Spring = true;
			// 
			// toolOutput
			// 
			this.toolOutput.AutoSize = false;
			this.toolOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolOutput.Image = ((System.Drawing.Image)(resources.GetObject("toolOutput.Image")));
			this.toolOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOutput.Name = "toolOutput";
			this.toolOutput.Size = new System.Drawing.Size(43, 21);
			this.toolOutput.Text = "Collapse";
			this.toolOutput.Click += new System.EventHandler(this.toolOutput_Click);
			// 
			// splitView
			// 
			this.splitView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitView.Location = new System.Drawing.Point(0, 0);
			this.splitView.Name = "splitView";
			this.splitView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitView.Panel1
			// 
			this.splitView.Panel1.Controls.Add(this.tabSource);
			// 
			// splitView.Panel2
			// 
			this.splitView.Panel2.Controls.Add(this.richOutput);
			this.splitView.Size = new System.Drawing.Size(944, 465);
			this.splitView.SplitterDistance = 352;
			this.splitView.TabIndex = 0;
			// 
			// tabSource
			// 
			this.tabSource.Controls.Add(this.tabPageSource1);
			this.tabSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSource.ItemSize = new System.Drawing.Size(32, 24);
			this.tabSource.Location = new System.Drawing.Point(0, 0);
			this.tabSource.Name = "tabSource";
			this.tabSource.SelectedIndex = 0;
			this.tabSource.Size = new System.Drawing.Size(944, 352);
			this.tabSource.TabIndex = 0;
			// 
			// tabPageSource1
			// 
			this.tabPageSource1.Location = new System.Drawing.Point(4, 28);
			this.tabPageSource1.Name = "tabPageSource1";
			this.tabPageSource1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSource1.Size = new System.Drawing.Size(936, 320);
			this.tabPageSource1.TabIndex = 0;
			this.tabPageSource1.Text = "1";
			this.tabPageSource1.UseVisualStyleBackColor = true;
			// 
			// richOutput
			// 
			this.richOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richOutput.Location = new System.Drawing.Point(0, 0);
			this.richOutput.Name = "richOutput";
			this.richOutput.Size = new System.Drawing.Size(944, 109);
			this.richOutput.TabIndex = 1;
			this.richOutput.Text = "";
			this.richOutput.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richOutput_MouseDoubleClick);
			// 
			// imageListTab
			// 
			this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
			this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTab.Images.SetKeyName(0, "Prompt");
			this.imageListTab.Images.SetKeyName(1, "Source");
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitMain.Location = new System.Drawing.Point(0, 70);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.splitView);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.textPrompt);
			this.splitMain.Size = new System.Drawing.Size(944, 589);
			this.splitMain.SplitterDistance = 465;
			this.splitMain.TabIndex = 4;
			// 
			// panelArgs
			// 
			this.panelArgs.Controls.Add(this.textArgs);
			this.panelArgs.Controls.Add(this.labelArgs);
			this.panelArgs.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelArgs.Location = new System.Drawing.Point(0, 35);
			this.panelArgs.Name = "panelArgs";
			this.panelArgs.Size = new System.Drawing.Size(944, 35);
			this.panelArgs.TabIndex = 0;
			// 
			// labelArgs
			// 
			this.labelArgs.AutoSize = true;
			this.labelArgs.Location = new System.Drawing.Point(12, 12);
			this.labelArgs.Name = "labelArgs";
			this.labelArgs.Size = new System.Drawing.Size(29, 12);
			this.labelArgs.TabIndex = 0;
			this.labelArgs.Text = "args";
			// 
			// textArgs
			// 
			this.textArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textArgs.Location = new System.Drawing.Point(48, 8);
			this.textArgs.Name = "textArgs";
			this.textArgs.Size = new System.Drawing.Size(884, 19);
			this.textArgs.TabIndex = 1;
			// 
			// CxPromptForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(944, 682);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.panelArgs);
			this.Controls.Add(this.toolbarMain);
			this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.KeyPreview = true;
			this.Name = "CxPromptForm";
			this.Text = "Prompt";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxPromptForm_FormClosing);
			this.Load += new System.EventHandler(this.CxPromptForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CxPromptForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CxPromptForm_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CxPromptForm_KeyDown);
			this.toolbarMain.ResumeLayout(false);
			this.toolbarMain.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitView.Panel1.ResumeLayout(false);
			this.splitView.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitView)).EndInit();
			this.splitView.ResumeLayout(false);
			this.tabSource.ResumeLayout(false);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.panelArgs.ResumeLayout(false);
			this.panelArgs.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox textPrompt;
		private System.Windows.Forms.Timer timerUpdateUI;
		private XIE.Forms.CxToolStripEx toolbarMain;
		private System.Windows.Forms.ToolStripButton toolTaskStart;
		private System.Windows.Forms.ToolStripButton toolTaskStop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripButton toolFileOpen;
		private System.Windows.Forms.ToolStripButton toolLogForm;
		private System.Windows.Forms.ImageList imageListTab;
		private System.Windows.Forms.SplitContainer splitView;
		private System.Windows.Forms.RichTextBox richOutput;
		private System.Windows.Forms.TabControl tabSource;
		private System.Windows.Forms.TabPage tabPageSource1;
		private System.Windows.Forms.ToolStripDropDownButton toolOutput;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private System.Windows.Forms.ToolStripButton toolFileSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton toolVersion;
		private System.Windows.Forms.ToolStripButton toolPromptClear;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ToolStripButton toolBuild;
		private System.Windows.Forms.ToolStripButton toolEditUndo;
		private System.Windows.Forms.ToolStripButton toolEditRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolEditCut;
		private System.Windows.Forms.ToolStripButton toolEditCopy;
		private System.Windows.Forms.ToolStripButton toolEditPaste;
		private System.Windows.Forms.ToolStripButton toolEditDel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton toolFind;
		private System.Windows.Forms.ToolStripButton toolGoto;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Panel panelArgs;
		private System.Windows.Forms.TextBox textArgs;
		private System.Windows.Forms.Label labelArgs;
	}
}