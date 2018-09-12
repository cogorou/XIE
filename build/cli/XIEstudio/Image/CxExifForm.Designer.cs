namespace XIEstudio
{
	partial class CxExifForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxExifForm));
			this.listInfo = new System.Windows.Forms.ListView();
			this.headerTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolFileOpen = new System.Windows.Forms.ToolStripButton();
			this.toolFileSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolItemNameMode = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolItemNameMode0 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolItemNameMode1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolEditCut = new System.Windows.Forms.ToolStripButton();
			this.toolEditCopy = new System.Windows.Forms.ToolStripButton();
			this.toolEditPaste = new System.Windows.Forms.ToolStripButton();
			this.toolEditClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.toolDock = new System.Windows.Forms.ToolStripButton();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// listInfo
			// 
			this.listInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerTag,
            this.headerType,
            this.headerName,
            this.headerValue});
			this.listInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listInfo.FullRowSelect = true;
			this.listInfo.Location = new System.Drawing.Point(0, 35);
			this.listInfo.Name = "listInfo";
			this.listInfo.Size = new System.Drawing.Size(434, 238);
			this.listInfo.TabIndex = 0;
			this.listInfo.UseCompatibleStateImageBehavior = false;
			this.listInfo.View = System.Windows.Forms.View.Details;
			// 
			// headerTag
			// 
			this.headerTag.Text = "Tag";
			// 
			// headerType
			// 
			this.headerType.Text = "Type";
			this.headerType.Width = 50;
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			this.headerName.Width = 150;
			// 
			// headerValue
			// 
			this.headerValue.Text = "Value";
			this.headerValue.Width = 146;
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFileOpen,
            this.toolFileSave,
            this.toolStripSeparator3,
            this.toolItemNameMode,
            this.toolStripSeparator1,
            this.toolEditCut,
            this.toolEditCopy,
            this.toolEditPaste,
            this.toolEditClear,
            this.toolStripSeparator2,
            this.toolDock});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(434, 35);
			this.toolbar.TabIndex = 1;
			this.toolbar.Text = "toolbar";
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
			this.toolFileOpen.Text = "Load";
			this.toolFileOpen.ToolTipText = "Load from Exif Data file";
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
			this.toolFileSave.ToolTipText = "Save to Exif Data file";
			this.toolFileSave.Click += new System.EventHandler(this.toolFileSave_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
			// 
			// toolItemNameMode
			// 
			this.toolItemNameMode.AutoSize = false;
			this.toolItemNameMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolItemNameMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolItemNameMode0,
            this.toolItemNameMode1});
			this.toolItemNameMode.Image = ((System.Drawing.Image)(resources.GetObject("toolItemNameMode.Image")));
			this.toolItemNameMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolItemNameMode.Name = "toolItemNameMode";
			this.toolItemNameMode.Size = new System.Drawing.Size(40, 32);
			this.toolItemNameMode.Text = "Item Name Mode";
			this.toolItemNameMode.ToolTipText = "Item Name Mode";
			this.toolItemNameMode.DropDownOpened += new System.EventHandler(this.toolItemNameMode_DropDownOpened);
			// 
			// toolItemNameMode0
			// 
			this.toolItemNameMode0.Name = "toolItemNameMode0";
			this.toolItemNameMode0.Size = new System.Drawing.Size(150, 22);
			this.toolItemNameMode0.Tag = "0";
			this.toolItemNameMode0.Text = "English (&0)";
			this.toolItemNameMode0.Click += new System.EventHandler(this.toolItemNameMode_Click);
			// 
			// toolItemNameMode1
			// 
			this.toolItemNameMode1.Name = "toolItemNameMode1";
			this.toolItemNameMode1.Size = new System.Drawing.Size(150, 22);
			this.toolItemNameMode1.Tag = "1";
			this.toolItemNameMode1.Text = "Japanese (&1)";
			this.toolItemNameMode1.Click += new System.EventHandler(this.toolItemNameMode_Click);
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
			this.toolEditCut.ToolTipText = "Cut to Clipboard";
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
			this.toolEditCopy.ToolTipText = "Copy to Clipboard";
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
			this.toolEditPaste.ToolTipText = "Paste from Clipboard";
			this.toolEditPaste.Click += new System.EventHandler(this.toolEditPaste_Click);
			// 
			// toolEditClear
			// 
			this.toolEditClear.AutoSize = false;
			this.toolEditClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolEditClear.Image = ((System.Drawing.Image)(resources.GetObject("toolEditClear.Image")));
			this.toolEditClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolEditClear.Name = "toolEditClear";
			this.toolEditClear.Size = new System.Drawing.Size(32, 32);
			this.toolEditClear.Text = "Clear";
			this.toolEditClear.ToolTipText = "Clear from Clipboard";
			this.toolEditClear.Click += new System.EventHandler(this.toolEditClear_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
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
			// CxExifForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 273);
			this.Controls.Add(this.listInfo);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxExifForm";
			this.Opacity = 0.9D;
			this.ShowInTaskbar = false;
			this.Text = "Exif";
			this.Activated += new System.EventHandler(this.CxExifForm_Activated);
			this.Deactivate += new System.EventHandler(this.CxExifForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxExifForm_FormClosing);
			this.Load += new System.EventHandler(this.CxExifForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CxExifForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CxExifForm_DragEnter);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listInfo;
		private System.Windows.Forms.ColumnHeader headerTag;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.ColumnHeader headerValue;
		private System.Windows.Forms.ColumnHeader headerType;
		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.ToolStripDropDownButton toolItemNameMode;
		private System.Windows.Forms.ToolStripMenuItem toolItemNameMode0;
		private System.Windows.Forms.ToolStripMenuItem toolItemNameMode1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolEditCopy;
		private System.Windows.Forms.ToolStripButton toolEditCut;
		private System.Windows.Forms.ToolStripButton toolEditPaste;
		private System.Windows.Forms.ToolStripButton toolEditClear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripButton toolFileOpen;
		private System.Windows.Forms.ToolStripButton toolFileSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolDock;

	}
}