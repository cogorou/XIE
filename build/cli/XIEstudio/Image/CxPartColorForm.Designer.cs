namespace XIEstudio
{
	partial class CxPartColorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxPartColorForm));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupHueDir = new System.Windows.Forms.GroupBox();
			this.trackHueDir = new System.Windows.Forms.TrackBar();
			this.numericHueDir = new System.Windows.Forms.NumericUpDown();
			this.groupHueRange = new System.Windows.Forms.GroupBox();
			this.trackHueRange = new System.Windows.Forms.TrackBar();
			this.numericHueRange = new System.Windows.Forms.NumericUpDown();
			this.panelView = new System.Windows.Forms.Panel();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.workerExecute = new System.ComponentModel.BackgroundWorker();
			this.groupBlueRatio = new System.Windows.Forms.GroupBox();
			this.trackBlueRatio = new System.Windows.Forms.TrackBar();
			this.numericBlueRatio = new System.Windows.Forms.NumericUpDown();
			this.groupGreenRatio = new System.Windows.Forms.GroupBox();
			this.trackGreenRatio = new System.Windows.Forms.TrackBar();
			this.numericGreenRatio = new System.Windows.Forms.NumericUpDown();
			this.groupRedRatio = new System.Windows.Forms.GroupBox();
			this.trackRedRatio = new System.Windows.Forms.TrackBar();
			this.numericRedRatio = new System.Windows.Forms.NumericUpDown();
			this.groupHueDir.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackHueDir)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHueDir)).BeginInit();
			this.groupHueRange.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackHueRange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHueRange)).BeginInit();
			this.groupBlueRatio.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBlueRatio)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericBlueRatio)).BeginInit();
			this.groupGreenRatio.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackGreenRatio)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGreenRatio)).BeginInit();
			this.groupRedRatio.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackRedRatio)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericRedRatio)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(647, 441);
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
			this.buttonCancel.Location = new System.Drawing.Point(749, 441);
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
			// trackHueDir
			// 
			this.trackHueDir.LargeChange = 15;
			this.trackHueDir.Location = new System.Drawing.Point(108, 26);
			this.trackHueDir.Maximum = 360;
			this.trackHueDir.Name = "trackHueDir";
			this.trackHueDir.Size = new System.Drawing.Size(294, 45);
			this.trackHueDir.TabIndex = 1;
			this.trackHueDir.TickFrequency = 15;
			this.trackHueDir.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackHueDir.Scroll += new System.EventHandler(this.trackHueDir_Scroll);
			// 
			// numericHueDir
			// 
			this.numericHueDir.Location = new System.Drawing.Point(6, 33);
			this.numericHueDir.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.numericHueDir.Name = "numericHueDir";
			this.numericHueDir.Size = new System.Drawing.Size(96, 25);
			this.numericHueDir.TabIndex = 0;
			this.numericHueDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericHueDir.ValueChanged += new System.EventHandler(this.numericHueDir_ValueChanged);
			this.numericHueDir.Enter += new System.EventHandler(this.numericHueDir_Enter);
			// 
			// groupHueRange
			// 
			this.groupHueRange.Controls.Add(this.trackHueRange);
			this.groupHueRange.Controls.Add(this.numericHueRange);
			this.groupHueRange.Location = new System.Drawing.Point(12, 95);
			this.groupHueRange.Name = "groupHueRange";
			this.groupHueRange.Size = new System.Drawing.Size(408, 77);
			this.groupHueRange.TabIndex = 1;
			this.groupHueRange.TabStop = false;
			this.groupHueRange.Text = "Hue Range";
			// 
			// trackHueRange
			// 
			this.trackHueRange.Location = new System.Drawing.Point(108, 24);
			this.trackHueRange.Maximum = 180;
			this.trackHueRange.Name = "trackHueRange";
			this.trackHueRange.Size = new System.Drawing.Size(294, 45);
			this.trackHueRange.TabIndex = 1;
			this.trackHueRange.TickFrequency = 5;
			this.trackHueRange.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackHueRange.Value = 30;
			this.trackHueRange.Scroll += new System.EventHandler(this.trackHueRange_Scroll);
			// 
			// numericHueRange
			// 
			this.numericHueRange.Location = new System.Drawing.Point(6, 36);
			this.numericHueRange.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numericHueRange.Name = "numericHueRange";
			this.numericHueRange.Size = new System.Drawing.Size(96, 25);
			this.numericHueRange.TabIndex = 0;
			this.numericHueRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericHueRange.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericHueRange.ValueChanged += new System.EventHandler(this.numericHueRange_ValueChanged);
			this.numericHueRange.Enter += new System.EventHandler(this.numericHueRange_Enter);
			// 
			// panelView
			// 
			this.panelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelView.Location = new System.Drawing.Point(426, 12);
			this.panelView.Name = "panelView";
			this.panelView.Size = new System.Drawing.Size(419, 409);
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
			// groupBlueRatio
			// 
			this.groupBlueRatio.Controls.Add(this.trackBlueRatio);
			this.groupBlueRatio.Controls.Add(this.numericBlueRatio);
			this.groupBlueRatio.Location = new System.Drawing.Point(12, 344);
			this.groupBlueRatio.Name = "groupBlueRatio";
			this.groupBlueRatio.Size = new System.Drawing.Size(408, 77);
			this.groupBlueRatio.TabIndex = 8;
			this.groupBlueRatio.TabStop = false;
			this.groupBlueRatio.Text = "Blue Ratio";
			// 
			// trackBlueRatio
			// 
			this.trackBlueRatio.Location = new System.Drawing.Point(108, 26);
			this.trackBlueRatio.Maximum = 1000;
			this.trackBlueRatio.Name = "trackBlueRatio";
			this.trackBlueRatio.Size = new System.Drawing.Size(294, 45);
			this.trackBlueRatio.TabIndex = 1;
			this.trackBlueRatio.TickFrequency = 5;
			this.trackBlueRatio.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackBlueRatio.Value = 114;
			this.trackBlueRatio.Scroll += new System.EventHandler(this.trackBlueRatio_Scroll);
			// 
			// numericBlueRatio
			// 
			this.numericBlueRatio.Location = new System.Drawing.Point(6, 35);
			this.numericBlueRatio.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericBlueRatio.Name = "numericBlueRatio";
			this.numericBlueRatio.Size = new System.Drawing.Size(96, 25);
			this.numericBlueRatio.TabIndex = 0;
			this.numericBlueRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericBlueRatio.Value = new decimal(new int[] {
            114,
            0,
            0,
            0});
			this.numericBlueRatio.ValueChanged += new System.EventHandler(this.numericGreenRatio_ValueChanged);
			this.numericBlueRatio.Enter += new System.EventHandler(this.numericBlueRatio_Enter);
			// 
			// groupGreenRatio
			// 
			this.groupGreenRatio.Controls.Add(this.trackGreenRatio);
			this.groupGreenRatio.Controls.Add(this.numericGreenRatio);
			this.groupGreenRatio.Location = new System.Drawing.Point(12, 261);
			this.groupGreenRatio.Name = "groupGreenRatio";
			this.groupGreenRatio.Size = new System.Drawing.Size(408, 77);
			this.groupGreenRatio.TabIndex = 7;
			this.groupGreenRatio.TabStop = false;
			this.groupGreenRatio.Text = "Green Ratio";
			// 
			// trackGreenRatio
			// 
			this.trackGreenRatio.Location = new System.Drawing.Point(108, 24);
			this.trackGreenRatio.Maximum = 1000;
			this.trackGreenRatio.Name = "trackGreenRatio";
			this.trackGreenRatio.Size = new System.Drawing.Size(294, 45);
			this.trackGreenRatio.TabIndex = 1;
			this.trackGreenRatio.TickFrequency = 5;
			this.trackGreenRatio.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackGreenRatio.Value = 587;
			this.trackGreenRatio.Scroll += new System.EventHandler(this.trackGreenRatio_Scroll);
			// 
			// numericGreenRatio
			// 
			this.numericGreenRatio.Location = new System.Drawing.Point(6, 36);
			this.numericGreenRatio.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericGreenRatio.Name = "numericGreenRatio";
			this.numericGreenRatio.Size = new System.Drawing.Size(96, 25);
			this.numericGreenRatio.TabIndex = 0;
			this.numericGreenRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericGreenRatio.Value = new decimal(new int[] {
            587,
            0,
            0,
            0});
			this.numericGreenRatio.ValueChanged += new System.EventHandler(this.numericGreenRatio_ValueChanged);
			this.numericGreenRatio.Enter += new System.EventHandler(this.numericGreenRatio_Enter);
			// 
			// groupRedRatio
			// 
			this.groupRedRatio.Controls.Add(this.trackRedRatio);
			this.groupRedRatio.Controls.Add(this.numericRedRatio);
			this.groupRedRatio.Location = new System.Drawing.Point(12, 178);
			this.groupRedRatio.Name = "groupRedRatio";
			this.groupRedRatio.Size = new System.Drawing.Size(408, 77);
			this.groupRedRatio.TabIndex = 6;
			this.groupRedRatio.TabStop = false;
			this.groupRedRatio.Text = "Red Ratio";
			// 
			// trackRedRatio
			// 
			this.trackRedRatio.Location = new System.Drawing.Point(108, 26);
			this.trackRedRatio.Maximum = 1000;
			this.trackRedRatio.Name = "trackRedRatio";
			this.trackRedRatio.Size = new System.Drawing.Size(294, 45);
			this.trackRedRatio.TabIndex = 1;
			this.trackRedRatio.TickFrequency = 5;
			this.trackRedRatio.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackRedRatio.Value = 100;
			this.trackRedRatio.Scroll += new System.EventHandler(this.trackRedRatio_Scroll);
			// 
			// numericRedRatio
			// 
			this.numericRedRatio.Location = new System.Drawing.Point(6, 33);
			this.numericRedRatio.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericRedRatio.Name = "numericRedRatio";
			this.numericRedRatio.Size = new System.Drawing.Size(96, 25);
			this.numericRedRatio.TabIndex = 0;
			this.numericRedRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericRedRatio.Value = new decimal(new int[] {
            299,
            0,
            0,
            0});
			this.numericRedRatio.ValueChanged += new System.EventHandler(this.numericRedRatio_ValueChanged);
			this.numericRedRatio.Enter += new System.EventHandler(this.numericRedRatio_Enter);
			// 
			// CxPartColorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(857, 485);
			this.Controls.Add(this.groupBlueRatio);
			this.Controls.Add(this.groupGreenRatio);
			this.Controls.Add(this.groupRedRatio);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.groupHueRange);
			this.Controls.Add(this.groupHueDir);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxPartColorForm";
			this.Text = "Part Color";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxPartColorForm_FormClosing);
			this.Load += new System.EventHandler(this.CxPartColorForm_Load);
			this.groupHueDir.ResumeLayout(false);
			this.groupHueDir.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackHueDir)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHueDir)).EndInit();
			this.groupHueRange.ResumeLayout(false);
			this.groupHueRange.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackHueRange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHueRange)).EndInit();
			this.groupBlueRatio.ResumeLayout(false);
			this.groupBlueRatio.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBlueRatio)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericBlueRatio)).EndInit();
			this.groupGreenRatio.ResumeLayout(false);
			this.groupGreenRatio.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackGreenRatio)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGreenRatio)).EndInit();
			this.groupRedRatio.ResumeLayout(false);
			this.groupRedRatio.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackRedRatio)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericRedRatio)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupHueDir;
		private System.Windows.Forms.GroupBox groupHueRange;
		private System.Windows.Forms.NumericUpDown numericHueDir;
		private System.Windows.Forms.NumericUpDown numericHueRange;
		private System.Windows.Forms.TrackBar trackHueDir;
		private System.Windows.Forms.TrackBar trackHueRange;
		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.ComponentModel.BackgroundWorker workerExecute;
		private System.Windows.Forms.GroupBox groupBlueRatio;
		private System.Windows.Forms.TrackBar trackBlueRatio;
		private System.Windows.Forms.NumericUpDown numericBlueRatio;
		private System.Windows.Forms.GroupBox groupGreenRatio;
		private System.Windows.Forms.TrackBar trackGreenRatio;
		private System.Windows.Forms.NumericUpDown numericGreenRatio;
		private System.Windows.Forms.GroupBox groupRedRatio;
		private System.Windows.Forms.TrackBar trackRedRatio;
		private System.Windows.Forms.NumericUpDown numericRedRatio;
	}
}