namespace XIE.Tasks
{
	partial class CxTaskPortEditForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskPortEditForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.toolAdd = new System.Windows.Forms.ToolStripButton();
			this.toolRemove = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolUp = new System.Windows.Forms.ToolStripButton();
			this.toolDown = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusbarMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.treePorts = new System.Windows.Forms.TreeView();
			this.panelPort = new System.Windows.Forms.Panel();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.labelDescription = new System.Windows.Forms.Label();
			this.buttonType = new System.Windows.Forms.Button();
			this.textType = new System.Windows.Forms.TextBox();
			this.labelType = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.labelName = new System.Windows.Forms.Label();
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.panelPort.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCancel,
            this.toolOK,
            this.toolAdd,
            this.toolRemove,
            this.toolStripSeparator1,
            this.toolUp,
            this.toolDown});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(624, 35);
			this.toolbar.TabIndex = 0;
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
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
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(96, 32);
			this.toolOK.Text = "OK";
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// toolAdd
			// 
			this.toolAdd.AutoSize = false;
			this.toolAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolAdd.Image")));
			this.toolAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAdd.Name = "toolAdd";
			this.toolAdd.Size = new System.Drawing.Size(32, 32);
			this.toolAdd.Text = "Add";
			this.toolAdd.Click += new System.EventHandler(this.toolAdd_Click);
			// 
			// toolRemove
			// 
			this.toolRemove.AutoSize = false;
			this.toolRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolRemove.Image = ((System.Drawing.Image)(resources.GetObject("toolRemove.Image")));
			this.toolRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolRemove.Name = "toolRemove";
			this.toolRemove.Size = new System.Drawing.Size(32, 32);
			this.toolRemove.Text = "Remove";
			this.toolRemove.Click += new System.EventHandler(this.toolRemove_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
			// 
			// toolUp
			// 
			this.toolUp.AutoSize = false;
			this.toolUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolUp.Image = ((System.Drawing.Image)(resources.GetObject("toolUp.Image")));
			this.toolUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolUp.Name = "toolUp";
			this.toolUp.Size = new System.Drawing.Size(32, 32);
			this.toolUp.Text = "Up";
			this.toolUp.Click += new System.EventHandler(this.toolUp_Click);
			// 
			// toolDown
			// 
			this.toolDown.AutoSize = false;
			this.toolDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolDown.Image = ((System.Drawing.Image)(resources.GetObject("toolDown.Image")));
			this.toolDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolDown.Name = "toolDown";
			this.toolDown.Size = new System.Drawing.Size(32, 32);
			this.toolDown.Text = "Down";
			this.toolDown.Click += new System.EventHandler(this.toolDown_Click);
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarMessage});
			this.statusbar.Location = new System.Drawing.Point(0, 430);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(624, 23);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusbarMessage
			// 
			this.statusbarMessage.Name = "statusbarMessage";
			this.statusbarMessage.Size = new System.Drawing.Size(13, 18);
			this.statusbarMessage.Text = "-";
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitter.Location = new System.Drawing.Point(0, 35);
			this.splitter.Name = "splitter";
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.treePorts);
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2.Controls.Add(this.panelPort);
			this.splitter.Size = new System.Drawing.Size(624, 395);
			this.splitter.SplitterDistance = 200;
			this.splitter.TabIndex = 2;
			// 
			// treePorts
			// 
			this.treePorts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treePorts.Location = new System.Drawing.Point(0, 0);
			this.treePorts.Name = "treePorts";
			this.treePorts.Size = new System.Drawing.Size(200, 395);
			this.treePorts.TabIndex = 0;
			this.treePorts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePorts_AfterSelect);
			// 
			// panelPort
			// 
			this.panelPort.Controls.Add(this.textDescription);
			this.panelPort.Controls.Add(this.labelDescription);
			this.panelPort.Controls.Add(this.buttonType);
			this.panelPort.Controls.Add(this.textType);
			this.panelPort.Controls.Add(this.labelType);
			this.panelPort.Controls.Add(this.textName);
			this.panelPort.Controls.Add(this.labelName);
			this.panelPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelPort.Location = new System.Drawing.Point(0, 0);
			this.panelPort.Name = "panelPort";
			this.panelPort.Size = new System.Drawing.Size(420, 395);
			this.panelPort.TabIndex = 0;
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(15, 103);
			this.textDescription.Multiline = true;
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(392, 277);
			this.textDescription.TabIndex = 6;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// labelDescription
			// 
			this.labelDescription.AutoSize = true;
			this.labelDescription.Location = new System.Drawing.Point(12, 82);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(73, 18);
			this.labelDescription.TabIndex = 5;
			this.labelDescription.Text = "Description";
			// 
			// buttonType
			// 
			this.buttonType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonType.Location = new System.Drawing.Point(363, 43);
			this.buttonType.Name = "buttonType";
			this.buttonType.Size = new System.Drawing.Size(44, 25);
			this.buttonType.TabIndex = 4;
			this.buttonType.Text = "...";
			this.buttonType.UseVisualStyleBackColor = true;
			this.buttonType.Click += new System.EventHandler(this.buttonType_Click);
			// 
			// textType
			// 
			this.textType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textType.Location = new System.Drawing.Point(61, 43);
			this.textType.Name = "textType";
			this.textType.ReadOnly = true;
			this.textType.Size = new System.Drawing.Size(296, 25);
			this.textType.TabIndex = 3;
			// 
			// labelType
			// 
			this.labelType.AutoSize = true;
			this.labelType.Location = new System.Drawing.Point(12, 46);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(36, 18);
			this.labelType.TabIndex = 2;
			this.labelType.Text = "Type";
			// 
			// textName
			// 
			this.textName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textName.Location = new System.Drawing.Point(60, 8);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(347, 25);
			this.textName.TabIndex = 1;
			this.textName.TextChanged += new System.EventHandler(this.textName_TextChanged);
			this.textName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textName_KeyPress);
			// 
			// labelName
			// 
			this.labelName.AutoSize = true;
			this.labelName.Location = new System.Drawing.Point(12, 11);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(43, 18);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			// 
			// imageList16
			// 
			this.imageList16.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList16.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// CxTaskPortEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 453);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxTaskPortEditForm";
			this.Text = "Task Property";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxTaskPortEditForm_FormClosing);
			this.Load += new System.EventHandler(this.CxTaskPortEditForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.panelPort.ResumeLayout(false);
			this.panelPort.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.ToolStripStatusLabel statusbarMessage;
		private System.Windows.Forms.SplitContainer splitter;
		private System.Windows.Forms.TreeView treePorts;
		private System.Windows.Forms.ImageList imageList16;
		private System.Windows.Forms.Panel panelPort;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Button buttonType;
		private System.Windows.Forms.TextBox textType;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.ToolStripButton toolAdd;
		private System.Windows.Forms.ToolStripButton toolRemove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolUp;
		private System.Windows.Forms.ToolStripButton toolDown;
	}
}