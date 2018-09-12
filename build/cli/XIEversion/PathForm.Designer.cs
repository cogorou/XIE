namespace XIEversion
{
	partial class PathForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathForm));
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusbarMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.textPath = new System.Windows.Forms.TextBox();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolbar.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCancel,
            this.toolOK});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(596, 35);
			this.toolbar.TabIndex = 0;
			// 
			// statusbar
			// 
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarMessage});
			this.statusbar.Location = new System.Drawing.Point(0, 408);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(596, 23);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusbarMessage
			// 
			this.statusbarMessage.Name = "statusbarMessage";
			this.statusbarMessage.Size = new System.Drawing.Size(13, 18);
			this.statusbarMessage.Text = "-";
			// 
			// textPath
			// 
			this.textPath.AcceptsReturn = true;
			this.textPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textPath.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textPath.Location = new System.Drawing.Point(0, 35);
			this.textPath.Multiline = true;
			this.textPath.Name = "textPath";
			this.textPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textPath.Size = new System.Drawing.Size(596, 373);
			this.textPath.TabIndex = 2;
			this.textPath.TextChanged += new System.EventHandler(this.textPath_TextChanged);
			// 
			// toolOK
			// 
			this.toolOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOK.AutoSize = false;
			this.toolOK.Image = ((System.Drawing.Image)(resources.GetObject("toolOK.Image")));
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(72, 32);
			this.toolOK.Text = "OK";
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
			this.toolCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCancel.Name = "toolCancel";
			this.toolCancel.Size = new System.Drawing.Size(72, 32);
			this.toolCancel.Text = "Cancel";
			this.toolCancel.Click += new System.EventHandler(this.toolCancel_Click);
			// 
			// PathForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(596, 431);
			this.Controls.Add(this.textPath);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Name = "PathForm";
			this.Text = "PathForm";
			this.Load += new System.EventHandler(this.PathForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.ToolStripStatusLabel statusbarMessage;
		private System.Windows.Forms.TextBox textPath;
	}
}