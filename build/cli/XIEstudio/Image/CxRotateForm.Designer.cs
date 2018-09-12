namespace XIEstudio
{
	partial class CxRotateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxRotateForm));
			this.checkInterpolation = new System.Windows.Forms.CheckBox();
			this.radioMode1 = new System.Windows.Forms.RadioButton();
			this.radioMode2 = new System.Windows.Forms.RadioButton();
			this.numericAngle = new System.Windows.Forms.NumericUpDown();
			this.groupSize = new System.Windows.Forms.GroupBox();
			this.labelAxisY = new System.Windows.Forms.Label();
			this.labelAxisX = new System.Windows.Forms.Label();
			this.numericAxisY = new System.Windows.Forms.NumericUpDown();
			this.numericAxisX = new System.Windows.Forms.NumericUpDown();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelAngle = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericAngle)).BeginInit();
			this.groupSize.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericAxisY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericAxisX)).BeginInit();
			this.SuspendLayout();
			// 
			// checkInterpolation
			// 
			this.checkInterpolation.AutoSize = true;
			this.checkInterpolation.Location = new System.Drawing.Point(12, 225);
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
			this.radioMode1.Location = new System.Drawing.Point(12, 43);
			this.radioMode1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.radioMode1.Name = "radioMode1";
			this.radioMode1.Size = new System.Drawing.Size(65, 22);
			this.radioMode1.TabIndex = 0;
			this.radioMode1.TabStop = true;
			this.radioMode1.Text = "Center";
			this.radioMode1.UseVisualStyleBackColor = true;
			this.radioMode1.CheckedChanged += new System.EventHandler(this.radioMode1_CheckedChanged);
			// 
			// radioMode2
			// 
			this.radioMode2.AutoSize = true;
			this.radioMode2.Location = new System.Drawing.Point(12, 73);
			this.radioMode2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.radioMode2.Name = "radioMode2";
			this.radioMode2.Size = new System.Drawing.Size(76, 22);
			this.radioMode2.TabIndex = 1;
			this.radioMode2.TabStop = true;
			this.radioMode2.Text = "Absolute";
			this.radioMode2.UseVisualStyleBackColor = true;
			this.radioMode2.CheckedChanged += new System.EventHandler(this.radioMode2_CheckedChanged);
			// 
			// numericAngle
			// 
			this.numericAngle.Location = new System.Drawing.Point(172, 13);
			this.numericAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numericAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.numericAngle.Name = "numericAngle";
			this.numericAngle.Size = new System.Drawing.Size(140, 25);
			this.numericAngle.TabIndex = 2;
			this.numericAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericAngle.ValueChanged += new System.EventHandler(this.numericAngle_ValueChanged);
			this.numericAngle.Enter += new System.EventHandler(this.numericAngle_Enter);
			// 
			// groupSize
			// 
			this.groupSize.Controls.Add(this.labelAxisY);
			this.groupSize.Controls.Add(this.labelAxisX);
			this.groupSize.Controls.Add(this.numericAxisY);
			this.groupSize.Controls.Add(this.numericAxisX);
			this.groupSize.Location = new System.Drawing.Point(12, 102);
			this.groupSize.Name = "groupSize";
			this.groupSize.Size = new System.Drawing.Size(307, 116);
			this.groupSize.TabIndex = 3;
			this.groupSize.TabStop = false;
			this.groupSize.Text = "Axis (image coordinates)";
			// 
			// labelAxisY
			// 
			this.labelAxisY.Location = new System.Drawing.Point(15, 69);
			this.labelAxisY.Name = "labelAxisY";
			this.labelAxisY.Size = new System.Drawing.Size(114, 25);
			this.labelAxisY.TabIndex = 2;
			this.labelAxisY.Text = "Y";
			// 
			// labelAxisX
			// 
			this.labelAxisX.Location = new System.Drawing.Point(15, 36);
			this.labelAxisX.Name = "labelAxisX";
			this.labelAxisX.Size = new System.Drawing.Size(114, 25);
			this.labelAxisX.TabIndex = 0;
			this.labelAxisX.Text = "X";
			// 
			// numericAxisY
			// 
			this.numericAxisY.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericAxisY.Location = new System.Drawing.Point(160, 69);
			this.numericAxisY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericAxisY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericAxisY.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.numericAxisY.Name = "numericAxisY";
			this.numericAxisY.Size = new System.Drawing.Size(140, 25);
			this.numericAxisY.TabIndex = 3;
			this.numericAxisY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericAxisY.ValueChanged += new System.EventHandler(this.numericAxisY_ValueChanged);
			this.numericAxisY.Enter += new System.EventHandler(this.numericAxisY_Enter);
			// 
			// numericAxisX
			// 
			this.numericAxisX.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericAxisX.Location = new System.Drawing.Point(160, 36);
			this.numericAxisX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericAxisX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericAxisX.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.numericAxisX.Name = "numericAxisX";
			this.numericAxisX.Size = new System.Drawing.Size(140, 25);
			this.numericAxisX.TabIndex = 1;
			this.numericAxisX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericAxisX.ValueChanged += new System.EventHandler(this.numericAxisX_ValueChanged);
			this.numericAxisX.Enter += new System.EventHandler(this.numericAxisX_Enter);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(121, 294);
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
			this.buttonCancel.Location = new System.Drawing.Point(223, 294);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labelAngle
			// 
			this.labelAngle.AutoSize = true;
			this.labelAngle.Location = new System.Drawing.Point(13, 13);
			this.labelAngle.Name = "labelAngle";
			this.labelAngle.Size = new System.Drawing.Size(96, 18);
			this.labelAngle.TabIndex = 8;
			this.labelAngle.Text = "Angle (Degree)";
			// 
			// CxRotateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 338);
			this.Controls.Add(this.labelAngle);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupSize);
			this.Controls.Add(this.numericAngle);
			this.Controls.Add(this.radioMode2);
			this.Controls.Add(this.radioMode1);
			this.Controls.Add(this.checkInterpolation);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxRotateForm";
			this.ShowInTaskbar = false;
			this.Text = "Rotate";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxRotateForm_FormClosing);
			this.Load += new System.EventHandler(this.CxRotateForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericAngle)).EndInit();
			this.groupSize.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericAxisY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericAxisX)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkInterpolation;
		private System.Windows.Forms.RadioButton radioMode1;
		private System.Windows.Forms.RadioButton radioMode2;
		private System.Windows.Forms.NumericUpDown numericAngle;
		private System.Windows.Forms.GroupBox groupSize;
		private System.Windows.Forms.Label labelAxisY;
		private System.Windows.Forms.Label labelAxisX;
		private System.Windows.Forms.NumericUpDown numericAxisY;
		private System.Windows.Forms.NumericUpDown numericAxisX;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelAngle;
	}
}