namespace XIEstudio
{
	partial class CxGammaConverterForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxGammaConverterForm));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupGamma = new System.Windows.Forms.GroupBox();
			this.trackGamma = new System.Windows.Forms.TrackBar();
			this.numericGamma = new System.Windows.Forms.NumericUpDown();
			this.panelView = new System.Windows.Forms.Panel();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.workerExecute = new System.ComponentModel.BackgroundWorker();
			this.groupGamma.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackGamma)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGamma)).BeginInit();
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
			// groupGamma
			// 
			this.groupGamma.Controls.Add(this.trackGamma);
			this.groupGamma.Controls.Add(this.numericGamma);
			this.groupGamma.Location = new System.Drawing.Point(12, 12);
			this.groupGamma.Name = "groupGamma";
			this.groupGamma.Size = new System.Drawing.Size(408, 77);
			this.groupGamma.TabIndex = 0;
			this.groupGamma.TabStop = false;
			this.groupGamma.Text = "Gamma";
			// 
			// trackGamma
			// 
			this.trackGamma.LargeChange = 10;
			this.trackGamma.Location = new System.Drawing.Point(108, 26);
			this.trackGamma.Maximum = 200;
			this.trackGamma.Name = "trackGamma";
			this.trackGamma.Size = new System.Drawing.Size(294, 45);
			this.trackGamma.TabIndex = 1;
			this.trackGamma.TickFrequency = 10;
			this.trackGamma.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackGamma.Value = 100;
			this.trackGamma.Scroll += new System.EventHandler(this.trackGamma_Scroll);
			// 
			// numericGamma
			// 
			this.numericGamma.Location = new System.Drawing.Point(6, 33);
			this.numericGamma.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericGamma.Name = "numericGamma";
			this.numericGamma.Size = new System.Drawing.Size(96, 25);
			this.numericGamma.TabIndex = 0;
			this.numericGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numericGamma.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericGamma.ValueChanged += new System.EventHandler(this.numericGamma_ValueChanged);
			this.numericGamma.Enter += new System.EventHandler(this.numericGamma_Enter);
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
			// CxGammaConverterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(857, 319);
			this.Controls.Add(this.panelView);
			this.Controls.Add(this.groupGamma);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxGammaConverterForm";
			this.Text = "Gamma Converter";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxGammaConverterForm_FormClosing);
			this.Load += new System.EventHandler(this.CxGammaConverterForm_Load);
			this.groupGamma.ResumeLayout(false);
			this.groupGamma.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackGamma)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericGamma)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupGamma;
		private System.Windows.Forms.NumericUpDown numericGamma;
		private System.Windows.Forms.TrackBar trackGamma;
		private System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.ComponentModel.BackgroundWorker workerExecute;
	}
}