namespace XIEstudio
{
	partial class CxImageThumbnailForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxImageThumbnailForm));
			this.listThumbnail = new System.Windows.Forms.ListView();
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imageListThumbnailL = new System.Windows.Forms.ImageList(this.components);
			this.imageListThumbnailS = new System.Windows.Forms.ImageList(this.components);
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolViewType = new System.Windows.Forms.ToolStripButton();
			this.toolAdjustWidth = new System.Windows.Forms.ToolStripButton();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolActionNone = new System.Windows.Forms.ToolStripButton();
			this.toolActionSave = new System.Windows.Forms.ToolStripButton();
			this.toolActionClose = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// listThumbnail
			// 
			this.listThumbnail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerAction,
            this.headerSize,
            this.headerFilename});
			this.listThumbnail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listThumbnail.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listThumbnail.FullRowSelect = true;
			this.listThumbnail.GridLines = true;
			this.listThumbnail.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listThumbnail.LargeImageList = this.imageListThumbnailL;
			this.listThumbnail.Location = new System.Drawing.Point(0, 35);
			this.listThumbnail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.listThumbnail.Name = "listThumbnail";
			this.listThumbnail.Size = new System.Drawing.Size(1008, 477);
			this.listThumbnail.SmallImageList = this.imageListThumbnailS;
			this.listThumbnail.TabIndex = 0;
			this.listThumbnail.UseCompatibleStateImageBehavior = false;
			this.listThumbnail.View = System.Windows.Forms.View.Details;
			this.listThumbnail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listThumbnail_KeyDown);
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			this.headerName.Width = 282;
			// 
			// headerAction
			// 
			this.headerAction.Text = "Action";
			this.headerAction.Width = 100;
			// 
			// headerSize
			// 
			this.headerSize.Text = "ImageSize";
			this.headerSize.Width = 250;
			// 
			// headerFilename
			// 
			this.headerFilename.Text = "Filename";
			this.headerFilename.Width = 350;
			// 
			// imageListThumbnailL
			// 
			this.imageListThumbnailL.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListThumbnailL.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListThumbnailL.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// imageListThumbnailS
			// 
			this.imageListThumbnailS.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListThumbnailS.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListThumbnailS.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolViewType,
            this.toolAdjustWidth,
            this.toolCancel,
            this.toolOK,
            this.toolStripSeparator1,
            this.toolActionNone,
            this.toolActionSave,
            this.toolActionClose,
            this.toolStripSeparator2});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(1008, 35);
			this.toolbar.TabIndex = 1;
			this.toolbar.Text = "toolbar";
			// 
			// toolViewType
			// 
			this.toolViewType.AutoSize = false;
			this.toolViewType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewType.Image = ((System.Drawing.Image)(resources.GetObject("toolViewType.Image")));
			this.toolViewType.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewType.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewType.Name = "toolViewType";
			this.toolViewType.Size = new System.Drawing.Size(32, 32);
			this.toolViewType.Text = "View Type";
			this.toolViewType.Click += new System.EventHandler(this.toolViewType_Click);
			// 
			// toolAdjustWidth
			// 
			this.toolAdjustWidth.AutoSize = false;
			this.toolAdjustWidth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAdjustWidth.Image = ((System.Drawing.Image)(resources.GetObject("toolAdjustWidth.Image")));
			this.toolAdjustWidth.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolAdjustWidth.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAdjustWidth.Name = "toolAdjustWidth";
			this.toolAdjustWidth.Size = new System.Drawing.Size(32, 32);
			this.toolAdjustWidth.Text = "Adjust Width";
			this.toolAdjustWidth.Click += new System.EventHandler(this.toolAdjustWidth_Click);
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
			this.toolCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCancel.Name = "toolCancel";
			this.toolCancel.Size = new System.Drawing.Size(96, 32);
			this.toolCancel.Text = "Cancel";
			this.toolCancel.Click += new System.EventHandler(this.toolCancel_Click);
			// 
			// toolOK
			// 
			this.toolOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOK.AutoSize = false;
			this.toolOK.Image = ((System.Drawing.Image)(resources.GetObject("toolOK.Image")));
			this.toolOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(96, 32);
			this.toolOK.Text = "OK";
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolActionNone
			// 
			this.toolActionNone.AutoSize = false;
			this.toolActionNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolActionNone.Image = ((System.Drawing.Image)(resources.GetObject("toolActionNone.Image")));
			this.toolActionNone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolActionNone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolActionNone.Name = "toolActionNone";
			this.toolActionNone.Size = new System.Drawing.Size(32, 32);
			this.toolActionNone.Text = "None";
			this.toolActionNone.Click += new System.EventHandler(this.toolActionNone_Click);
			// 
			// toolActionSave
			// 
			this.toolActionSave.AutoSize = false;
			this.toolActionSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolActionSave.Image = ((System.Drawing.Image)(resources.GetObject("toolActionSave.Image")));
			this.toolActionSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolActionSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolActionSave.Name = "toolActionSave";
			this.toolActionSave.Size = new System.Drawing.Size(32, 32);
			this.toolActionSave.Text = "Save";
			this.toolActionSave.ToolTipText = "Set the action to save.";
			this.toolActionSave.Click += new System.EventHandler(this.toolActionSave_Click);
			// 
			// toolActionClose
			// 
			this.toolActionClose.AutoSize = false;
			this.toolActionClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolActionClose.Image = ((System.Drawing.Image)(resources.GetObject("toolActionClose.Image")));
			this.toolActionClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolActionClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolActionClose.Name = "toolActionClose";
			this.toolActionClose.Size = new System.Drawing.Size(32, 32);
			this.toolActionClose.Text = "Close";
			this.toolActionClose.ToolTipText = "Set the action to close.";
			this.toolActionClose.Click += new System.EventHandler(this.toolActionClose_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// CxImageThumbnailForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 512);
			this.Controls.Add(this.listThumbnail);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxImageThumbnailForm";
			this.ShowInTaskbar = false;
			this.Text = "Thumbnail";
			this.Load += new System.EventHandler(this.CxImageThumbnailForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listThumbnail;
		private System.Windows.Forms.ImageList imageListThumbnailL;
		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.ImageList imageListThumbnailS;
		private System.Windows.Forms.ToolStripButton toolViewType;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.ColumnHeader headerSize;
		private System.Windows.Forms.ColumnHeader headerFilename;
		private System.Windows.Forms.ToolStripButton toolAdjustWidth;
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolActionSave;
		private System.Windows.Forms.ToolStripButton toolActionClose;
		private System.Windows.Forms.ColumnHeader headerAction;
		private System.Windows.Forms.ToolStripButton toolActionNone;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}