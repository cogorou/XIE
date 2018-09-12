namespace XIEstudio
{
	partial class CxTaskflowSourceToolboxFind
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTaskflowSourceToolboxFind));
			this.textKeyword = new System.Windows.Forms.TextBox();
			this.buttonPrev = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.checkCase = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// textKeyword
			// 
			this.textKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textKeyword.Location = new System.Drawing.Point(12, 9);
			this.textKeyword.Name = "textKeyword";
			this.textKeyword.Size = new System.Drawing.Size(276, 19);
			this.textKeyword.TabIndex = 0;
			this.textKeyword.Enter += new System.EventHandler(this.textKeyword_Enter);
			this.textKeyword.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textKeyword_PreviewKeyDown);
			// 
			// buttonPrev
			// 
			this.buttonPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPrev.Image = ((System.Drawing.Image)(resources.GetObject("buttonPrev.Image")));
			this.buttonPrev.Location = new System.Drawing.Point(355, 6);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new System.Drawing.Size(32, 24);
			this.buttonPrev.TabIndex = 2;
			this.buttonPrev.UseVisualStyleBackColor = true;
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonNext.Image")));
			this.buttonNext.Location = new System.Drawing.Point(393, 6);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(32, 24);
			this.buttonNext.TabIndex = 3;
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// checkCase
			// 
			this.checkCase.Location = new System.Drawing.Point(294, 6);
			this.checkCase.Name = "checkCase";
			this.checkCase.Size = new System.Drawing.Size(50, 24);
			this.checkCase.TabIndex = 1;
			this.checkCase.Text = "&Case";
			this.checkCase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkCase.UseVisualStyleBackColor = false;
			// 
			// CxTaskflowSourceToolboxFind
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 37);
			this.Controls.Add(this.checkCase);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonPrev);
			this.Controls.Add(this.textKeyword);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.Name = "CxTaskflowSourceToolboxFind";
			this.ShowInTaskbar = false;
			this.Text = "Find";
			this.Load += new System.EventHandler(this.EditorToolboxFind_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CxTaskflowSourceToolboxFind_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textKeyword;
		private System.Windows.Forms.Button buttonPrev;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.CheckBox checkCase;
	}
}