namespace XIEstudio
{
	partial class CxTaskflowHelpForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskflowHelpForm));
			this.labelName = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.listPorts = new System.Windows.Forms.ListView();
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.checkDock = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelName.Location = new System.Drawing.Point(2, 3);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(608, 32);
			this.labelName.TabIndex = 1;
			this.labelName.Text = "Name";
			this.labelName.Click += new System.EventHandler(this.labelName_Click);
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(2, 39);
			this.textDescription.Multiline = true;
			this.textDescription.Name = "textDescription";
			this.textDescription.ReadOnly = true;
			this.textDescription.Size = new System.Drawing.Size(647, 57);
			this.textDescription.TabIndex = 2;
			this.textDescription.TabStop = false;
			// 
			// listPorts
			// 
			this.listPorts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerTypes,
            this.headerDescription});
			this.listPorts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listPorts.FullRowSelect = true;
			this.listPorts.GridLines = true;
			this.listPorts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listPorts.Location = new System.Drawing.Point(0, 0);
			this.listPorts.Name = "listPorts";
			this.listPorts.Size = new System.Drawing.Size(651, 258);
			this.listPorts.TabIndex = 3;
			this.listPorts.UseCompatibleStateImageBehavior = false;
			this.listPorts.View = System.Windows.Forms.View.Details;
			this.listPorts.SelectedIndexChanged += new System.EventHandler(this.listPorts_SelectedIndexChanged);
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			this.headerName.Width = 150;
			// 
			// headerTypes
			// 
			this.headerTypes.Text = "Types";
			this.headerTypes.Width = 200;
			// 
			// headerDescription
			// 
			this.headerDescription.Text = "Description";
			this.headerDescription.Width = 280;
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.checkDock);
			this.splitMain.Panel1.Controls.Add(this.textDescription);
			this.splitMain.Panel1.Controls.Add(this.labelName);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.listPorts);
			this.splitMain.Size = new System.Drawing.Size(651, 362);
			this.splitMain.SplitterDistance = 100;
			this.splitMain.TabIndex = 4;
			// 
			// checkDock
			// 
			this.checkDock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkDock.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkDock.FlatAppearance.BorderSize = 0;
			this.checkDock.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
			this.checkDock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkDock.Image = ((System.Drawing.Image)(resources.GetObject("checkDock.Image")));
			this.checkDock.Location = new System.Drawing.Point(616, 3);
			this.checkDock.Name = "checkDock";
			this.checkDock.Size = new System.Drawing.Size(32, 32);
			this.checkDock.TabIndex = 3;
			this.checkDock.UseVisualStyleBackColor = true;
			this.checkDock.CheckedChanged += new System.EventHandler(this.checkDock_CheckedChanged);
			// 
			// CxTaskflowHelpForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(651, 362);
			this.Controls.Add(this.splitMain);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CxTaskflowHelpForm";
			this.ShowInTaskbar = false;
			this.Text = "Help";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxTaskflowHelpForm_FormClosing);
			this.Load += new System.EventHandler(this.CxTaskflowHelpForm_Load);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel1.PerformLayout();
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.ListView listPorts;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.ColumnHeader headerTypes;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ColumnHeader headerDescription;
		private System.Windows.Forms.CheckBox checkDock;
	}
}