namespace XIEstudio
{
	partial class CxScaleForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxScaleForm));
			this.checkInterpolation = new System.Windows.Forms.CheckBox();
			this.radioMode1 = new System.Windows.Forms.RadioButton();
			this.radioMode2 = new System.Windows.Forms.RadioButton();
			this.numericPercentage = new System.Windows.Forms.NumericUpDown();
			this.groupSize = new System.Windows.Forms.GroupBox();
			this.labelHeight = new System.Windows.Forms.Label();
			this.labelWidth = new System.Windows.Forms.Label();
			this.numericHeight = new System.Windows.Forms.NumericUpDown();
			this.numericWidth = new System.Windows.Forms.NumericUpDown();
			this.checkKeepRatio = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericPercentage)).BeginInit();
			this.groupSize.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// checkInterpolation
			// 
			this.checkInterpolation.AutoSize = true;
			this.checkInterpolation.Location = new System.Drawing.Point(12, 195);
			this.checkInterpolation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.checkInterpolation.Name = "checkInterpolation";
			this.checkInterpolation.Size = new System.Drawing.Size(102, 22);
			this.checkInterpolation.TabIndex = 4;
			this.checkInterpolation.Text = "Interpolation";
			this.checkInterpolation.UseVisualStyleBackColor = true;
			// 
			// radioMode1
			// 
			this.radioMode1.AutoSize = true;
			this.radioMode1.Location = new System.Drawing.Point(12, 13);
			this.radioMode1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.radioMode1.Name = "radioMode1";
			this.radioMode1.Size = new System.Drawing.Size(117, 22);
			this.radioMode1.TabIndex = 0;
			this.radioMode1.TabStop = true;
			this.radioMode1.Text = "Percentage (%)";
			this.radioMode1.UseVisualStyleBackColor = true;
			this.radioMode1.CheckedChanged += new System.EventHandler(this.radioMode1_CheckedChanged);
			// 
			// radioMode2
			// 
			this.radioMode2.AutoSize = true;
			this.radioMode2.Location = new System.Drawing.Point(12, 43);
			this.radioMode2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.radioMode2.Name = "radioMode2";
			this.radioMode2.Size = new System.Drawing.Size(76, 22);
			this.radioMode2.TabIndex = 1;
			this.radioMode2.TabStop = true;
			this.radioMode2.Text = "Absolute";
			this.radioMode2.UseVisualStyleBackColor = true;
			this.radioMode2.CheckedChanged += new System.EventHandler(this.radioMode2_CheckedChanged);
			// 
			// numericPercentage
			// 
			this.numericPercentage.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericPercentage.Location = new System.Drawing.Point(172, 13);
			this.numericPercentage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericPercentage.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.numericPercentage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericPercentage.Name = "numericPercentage";
			this.numericPercentage.Size = new System.Drawing.Size(140, 25);
			this.numericPercentage.TabIndex = 2;
			this.numericPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericPercentage.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericPercentage.ValueChanged += new System.EventHandler(this.numericPercentage_ValueChanged);
			this.numericPercentage.Enter += new System.EventHandler(this.numericPercentage_Enter);
			// 
			// groupSize
			// 
			this.groupSize.Controls.Add(this.labelHeight);
			this.groupSize.Controls.Add(this.labelWidth);
			this.groupSize.Controls.Add(this.numericHeight);
			this.groupSize.Controls.Add(this.numericWidth);
			this.groupSize.Location = new System.Drawing.Point(12, 72);
			this.groupSize.Name = "groupSize";
			this.groupSize.Size = new System.Drawing.Size(307, 116);
			this.groupSize.TabIndex = 3;
			this.groupSize.TabStop = false;
			this.groupSize.Text = "Destination size (pixels)";
			// 
			// labelHeight
			// 
			this.labelHeight.Location = new System.Drawing.Point(15, 69);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(114, 25);
			this.labelHeight.TabIndex = 2;
			this.labelHeight.Text = "Height";
			// 
			// labelWidth
			// 
			this.labelWidth.Location = new System.Drawing.Point(15, 36);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(114, 25);
			this.labelWidth.TabIndex = 0;
			this.labelWidth.Text = "Width";
			// 
			// numericHeight
			// 
			this.numericHeight.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericHeight.Location = new System.Drawing.Point(160, 69);
			this.numericHeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.numericHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericHeight.Name = "numericHeight";
			this.numericHeight.Size = new System.Drawing.Size(140, 25);
			this.numericHeight.TabIndex = 3;
			this.numericHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericHeight.ValueChanged += new System.EventHandler(this.numericHeight_ValueChanged);
			this.numericHeight.Enter += new System.EventHandler(this.numericHeight_Enter);
			// 
			// numericWidth
			// 
			this.numericWidth.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericWidth.Location = new System.Drawing.Point(160, 36);
			this.numericWidth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.numericWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericWidth.Name = "numericWidth";
			this.numericWidth.Size = new System.Drawing.Size(140, 25);
			this.numericWidth.TabIndex = 1;
			this.numericWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericWidth.ValueChanged += new System.EventHandler(this.numericWidth_ValueChanged);
			this.numericWidth.Enter += new System.EventHandler(this.numericWidth_Enter);
			// 
			// checkKeepRatio
			// 
			this.checkKeepRatio.AutoSize = true;
			this.checkKeepRatio.Location = new System.Drawing.Point(12, 225);
			this.checkKeepRatio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.checkKeepRatio.Name = "checkKeepRatio";
			this.checkKeepRatio.Size = new System.Drawing.Size(129, 22);
			this.checkKeepRatio.TabIndex = 5;
			this.checkKeepRatio.Text = "Keep aspect ratio";
			this.checkKeepRatio.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(121, 286);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 6;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonCancel.Location = new System.Drawing.Point(223, 286);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// CxScaleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 330);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupSize);
			this.Controls.Add(this.numericPercentage);
			this.Controls.Add(this.radioMode2);
			this.Controls.Add(this.radioMode1);
			this.Controls.Add(this.checkKeepRatio);
			this.Controls.Add(this.checkInterpolation);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxScaleForm";
			this.ShowInTaskbar = false;
			this.Text = "Scale";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxScaleForm_FormClosing);
			this.Load += new System.EventHandler(this.CxScaleForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericPercentage)).EndInit();
			this.groupSize.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkInterpolation;
		private System.Windows.Forms.RadioButton radioMode1;
		private System.Windows.Forms.RadioButton radioMode2;
		private System.Windows.Forms.NumericUpDown numericPercentage;
		private System.Windows.Forms.GroupBox groupSize;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.NumericUpDown numericHeight;
		private System.Windows.Forms.NumericUpDown numericWidth;
		private System.Windows.Forms.CheckBox checkKeepRatio;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
	}
}