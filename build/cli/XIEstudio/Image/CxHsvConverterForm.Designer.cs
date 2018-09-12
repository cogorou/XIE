namespace XIEstudio
{
	partial class CxHsvConverterForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxHsvConverterForm));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupHueDir = new System.Windows.Forms.GroupBox();
			this.groupSaturationFactor = new System.Windows.Forms.GroupBox();
			this.groupValueFactor = new System.Windows.Forms.GroupBox();
			this.numericHueDir = new System.Windows.Forms.NumericUpDown();
			this.numericSaturationFactor = new System.Windows.Forms.NumericUpDown();
			this.numericValueFactor = new System.Windows.Forms.NumericUpDown();
			this.trackHueDir = new System.Windows.Forms.TrackBar();
			this.trackSaturationFactor = new System.Windows.Forms.TrackBar();
			this.trackValueFactor = new System.Windows.Forms.TrackBar();
			this.panelView = new System.Windows.Forms.Panel();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.workerExecute = new System.ComponentModel.BackgroundWorker();
			this.groupHueDir.SuspendLayout();
			this.groupSaturationFactor.SuspendLayout();
			this.groupValueFactor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericHueDir)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericSaturationFactor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericValueFactor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackHueDir)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackSaturationFactor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackValueFactor)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(647, 275);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonCancel.Location = new System.Drawing.Point(749, 275);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// groupHueDir
			// 
			this.groupHueDir.Controls.Add(this.trackHueDir);
			this.groupHueDir.Controls.Add(this.numericHueDir);
			this.groupHueDir.Location = new System.Drawing.Point(12, 12);
			this.groupHueDir.Name = "groupHueDir";
			this.groupHueDir.Size = new System.Drawing.Size(408, 77);
			this.groupHueDir.TabIndex = 0;
			this.groupHueDir.TabStop = false;
			this.groupHueDir.Text = "Hue Direction";
			// 
			// groupSaturationFactor
			// 
			this.groupSaturationFactor.Controls.Add(this.trackSaturationFactor);
			this.groupSaturationFactor.Controls.Add(this.numericSaturationFactor);
			this.groupSaturationFactor.Location = new System.Drawing.Point(12, 95);
			this.groupSaturationFactor.Name = "groupSaturationFactor";
			this.groupSaturationFactor.Size = new System.Drawing.Size(408, 77);
			this.groupSaturationFactor.TabIndex = 1;
			this.groupSaturationFactor.TabStop = false;
			this.groupSaturationFactor.Text = "Saturation Factor";
			// 
			// groupValueFactor
			// 
			this.groupValueFactor.Controls.Add(this.trackValueFactor);
			this.groupValueFactor.Controls.Add(this.numericValueFactor);
			this.groupValueFactor.Location = new System.Drawing.Point(12, 178);
			this.groupValueFactor.Name = "groupValueFactor";
			this.groupValueFactor.Size = new System.Drawing.Size(408, 77);
			this.groupValueFactor.TabIndex = 2;
			this.groupValueFactor.TabStop = false;
			this.groupValueFactor.Text = "Value Factor";
			// 
			// numericHueDir
			// 
			this.numericHueDir.Location = new System.Drawing.Point(6, 33);
			this.numericHueDir.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numericHueDir.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.numericHueDir.Name = "numericHueDir";
			this.numericHueDir.Size = new System.Drawing.Size(96, 25);
			this.numericHueDir.TabIndex = 0;
			this.numericHueDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericHueDir.ValueChanged += new System.EventHandler(this.numericHueDir_ValueChanged);
			this.numericHueDir.Enter += new System.EventHandler(this.numericHueDir_Enter);
			// 
			// numericSaturationFactor
			// 
			this.numericSaturationFactor.Location = new System.Drawing.Point(6, 36);
			this.numericSaturationFactor.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericSaturationFactor.Name = "numericSaturationFactor";
			this.numericSaturationFactor.Size = new System.Drawing.Size(96, 25);
			this.numericSaturationFactor.TabIndex = 0;
			this.numericSaturationFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericSaturationFactor.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericSaturationFactor.ValueChanged += new System.EventHandler(this.numericSaturationFactor_ValueChanged);
			this.numericSaturationFactor.Enter += new System.EventHandler(this.numericSaturationFactor_Enter);
			// 
			// numericValueFactor
			// 
			this.numericValueFactor.Location = new System.Drawing.Point(6, 35);
			this.numericValueFactor.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericValueFactor.Name = "numericValueFactor";
			this.numericValueFactor.Size = new System.Drawing.Size(96, 25);
			this.numericValueFactor.TabIndex = 0;
			this.numericValueFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericValueFactor.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericValueFactor.ValueChanged += new System.EventHandler(this.numericValueFactor_ValueChanged);
			this.numericValueFactor.Enter += new System.EventHandler(this.numericValueFactor_Enter);
			// 
			// trackHueDir
			// 
			this.trackHueDir.LargeChange = 15;
			this.trackHueDir.Location = new System.Drawing.Point(108, 26);
			this.trackHueDir.Maximum = 180;
			this.trackHueDir.Minimum = -180;
			this.trackHueDir.Name = "trackHueDir";
			this.trackHueDir.Size = new System.Drawing.Size(294, 45);
			this.trackHueDir.TabIndex = 1;
			this.trackHueDir.TickFrequency = 15;
			this.trackHueDir.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackHueDir.Scroll += new System.EventHandler(this.trackHueDir_Scroll);
			// 
			// trackSaturationFactor
			// 
			this.trackSaturationFactor.Location = new System.Drawing.Point(108, 24);
			this.trackSaturationFactor.Maximum = 200;
			this.trackSaturationFactor.Name = "trackSaturationFactor";
			this.trackSaturationFactor.Size = new System.Drawing.Size(294, 45);
			this.trackSaturationFactor.TabIndex = 1;
			this.trackSaturationFactor.TickFrequency = 5;
			this.trackSaturationFactor.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackSaturationFactor.Value = 100;
			this.trackSaturationFactor.Scroll += new System.EventHandler(this.trackSaturationFactor_Scroll);
			// 
			// trackValueFactor
			// 
			this.trackValueFactor.Location = new System.Drawing.Point(108, 26);
			this.trackValueFactor.Maximum = 200;
			this.trackValueFactor.Name = "trackValueFactor";
			this.trackValueFactor.Size = new System.Drawing.Size(294, 45);
			this.trackValueFactor.TabIndex = 1;
			this.trackValueFactor.TickFrequency = 5;
			this.trackValueFactor.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackValueFactor.Value = 100;
			this.trackValueFactor.Scroll += new System.EventHandler(this.trackValueFactor_Scroll);
			// 
			// panelView
			// 
			this.panelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelView.Location = new System.Drawing.Point(426, 12);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(419, 243);
			this.panelView.TabIndex = 5;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// workerExecute
			// 
			this.workerExecute.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerExecute_DoWork);
			this.workerExecute.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerExecute_RunWorkerCompleted);
			// 
			// CxHsvConverterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(857, 319);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.groupValueFactor);
			this.Controls.Add(this.groupSaturationFactor);
			this.Controls.Add(this.groupHueDir);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxHsvConverterForm";
			this.Text = "HSV Converter";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxHsvConverterForm_FormClosing);
			this.Load += new System.EventHandler(this.CxHsvConverterForm_Load);
			this.groupHueDir.ResumeLayout(false);
			this.groupHueDir.PerformLayout();
			this.groupSaturationFactor.ResumeLayout(false);
			this.groupSaturationFactor.PerformLayout();
			this.groupValueFactor.ResumeLayout(false);
			this.groupValueFactor.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericHueDir)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericSaturationFactor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericValueFactor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackHueDir)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackSaturationFactor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackValueFactor)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupHueDir;
		private System.Windows.Forms.GroupBox groupSaturationFactor;
		private System.Windows.Forms.GroupBox groupValueFactor;
		private System.Windows.Forms.NumericUpDown numericHueDir;
		private System.Windows.Forms.NumericUpDown numericSaturationFactor;
		private System.Windows.Forms.NumericUpDown numericValueFactor;
		private System.Windows.Forms.TrackBar trackHueDir;
		private System.Windows.Forms.TrackBar trackSaturationFactor;
		private System.Windows.Forms.TrackBar trackValueFactor;
		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.ComponentModel.BackgroundWorker workerExecute;
	}
}