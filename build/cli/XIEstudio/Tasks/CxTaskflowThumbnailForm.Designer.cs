namespace XIEstudio
{
	partial class CxTaskflowThumbnailForm
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
			this.listThumbnail = new System.Windows.Forms.ListView();
			this.imageListThumbnail = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// listThumbnail
			// 
			this.listThumbnail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listThumbnail.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listThumbnail.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listThumbnail.LargeImageList = this.imageListThumbnail;
			this.listThumbnail.Location = new System.Drawing.Point(0, 0);
			this.listThumbnail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.listThumbnail.MultiSelect = false;
			this.listThumbnail.Name = "listThumbnail";
			this.listThumbnail.Size = new System.Drawing.Size(704, 442);
			this.listThumbnail.TabIndex = 0;
			this.listThumbnail.UseCompatibleStateImageBehavior = false;
			this.listThumbnail.DoubleClick += new System.EventHandler(this.listThumbnail_DoubleClick);
			// 
			// imageListThumbnail
			// 
			this.imageListThumbnail.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListThumbnail.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListThumbnail.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// CxTaskflowThumbnailForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(704, 442);
			this.Controls.Add(this.listThumbnail);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxTaskflowThumbnailForm";
			this.ShowInTaskbar = false;
			this.Text = "Thumbnail";
			this.Load += new System.EventHandler(this.CxTaskflowThumbnailForm_Load);
			this.Resize += new System.EventHandler(this.CxTaskflowThumbnailForm_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listThumbnail;
		private System.Windows.Forms.ImageList imageListThumbnail;
	}
}