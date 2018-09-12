namespace XIE.Tasks
{
	partial class TxImageSizeForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TxImageSizeForm));
			this.labelWidth = new System.Windows.Forms.Label();
			this.numericWidth = new System.Windows.Forms.NumericUpDown();
			this.labelHeight = new System.Windows.Forms.Label();
			this.numericHeight = new System.Windows.Forms.NumericUpDown();
			this.labelModel = new System.Windows.Forms.Label();
			this.labelChannels = new System.Windows.Forms.Label();
			this.numericPack = new System.Windows.Forms.NumericUpDown();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.numericChannels = new System.Windows.Forms.NumericUpDown();
			this.labelDepth = new System.Windows.Forms.Label();
			this.numericDepth = new System.Windows.Forms.NumericUpDown();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericChannels)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericDepth)).BeginInit();
			this.SuspendLayout();
			// 
			// labelWidth
			// 
			this.labelWidth.AutoSize = true;
			this.labelWidth.Location = new System.Drawing.Point(14, 22);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(88, 15);
			this.labelWidth.TabIndex = 0;
			this.labelWidth.Text = "Width (pixels)";
			// 
			// numericWidth
			// 
			this.numericWidth.Location = new System.Drawing.Point(177, 20);
			this.numericWidth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericWidth.Name = "numericWidth";
			this.numericWidth.Size = new System.Drawing.Size(140, 23);
			this.numericWidth.TabIndex = 1;
			this.numericWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericWidth.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			this.numericWidth.Enter += new System.EventHandler(this.numericUpDown_Enter);
			// 
			// labelHeight
			// 
			this.labelHeight.AutoSize = true;
			this.labelHeight.Location = new System.Drawing.Point(14, 54);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(92, 15);
			this.labelHeight.TabIndex = 2;
			this.labelHeight.Text = "Height (pixels)";
			// 
			// numericHeight
			// 
			this.numericHeight.Location = new System.Drawing.Point(177, 52);
			this.numericHeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericHeight.Name = "numericHeight";
			this.numericHeight.Size = new System.Drawing.Size(140, 23);
			this.numericHeight.TabIndex = 3;
			this.numericHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericHeight.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			this.numericHeight.Enter += new System.EventHandler(this.numericUpDown_Enter);
			// 
			// labelModel
			// 
			this.labelModel.AutoSize = true;
			this.labelModel.Location = new System.Drawing.Point(15, 86);
			this.labelModel.Name = "labelModel";
			this.labelModel.Size = new System.Drawing.Size(41, 15);
			this.labelModel.TabIndex = 4;
			this.labelModel.Text = "Model";
			// 
			// labelChannels
			// 
			this.labelChannels.AutoSize = true;
			this.labelChannels.Location = new System.Drawing.Point(15, 118);
			this.labelChannels.Name = "labelChannels";
			this.labelChannels.Size = new System.Drawing.Size(59, 15);
			this.labelChannels.TabIndex = 7;
			this.labelChannels.Text = "Channels";
			// 
			// numericPack
			// 
			this.numericPack.Location = new System.Drawing.Point(257, 84);
			this.numericPack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericPack.Name = "numericPack";
			this.numericPack.Size = new System.Drawing.Size(61, 23);
			this.numericPack.TabIndex = 6;
			this.numericPack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericPack.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			this.numericPack.Enter += new System.EventHandler(this.numericUpDown_Enter);
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboType.FormattingEnabled = true;
			this.comboType.Location = new System.Drawing.Point(178, 84);
			this.comboType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboType.MaxDropDownItems = 12;
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(70, 23);
			this.comboType.TabIndex = 5;
			this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
			// 
			// numericChannels
			// 
			this.numericChannels.Location = new System.Drawing.Point(177, 116);
			this.numericChannels.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericChannels.Name = "numericChannels";
			this.numericChannels.Size = new System.Drawing.Size(140, 23);
			this.numericChannels.TabIndex = 8;
			this.numericChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericChannels.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			this.numericChannels.Enter += new System.EventHandler(this.numericUpDown_Enter);
			// 
			// labelDepth
			// 
			this.labelDepth.AutoSize = true;
			this.labelDepth.Location = new System.Drawing.Point(15, 150);
			this.labelDepth.Name = "labelDepth";
			this.labelDepth.Size = new System.Drawing.Size(77, 15);
			this.labelDepth.TabIndex = 9;
			this.labelDepth.Text = "Depth (bits)";
			// 
			// numericDepth
			// 
			this.numericDepth.Location = new System.Drawing.Point(177, 148);
			this.numericDepth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericDepth.Name = "numericDepth";
			this.numericDepth.Size = new System.Drawing.Size(140, 23);
			this.numericDepth.TabIndex = 10;
			this.numericDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericDepth.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
			this.numericDepth.Enter += new System.EventHandler(this.numericUpDown_Enter);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
			this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonOK.Location = new System.Drawing.Point(121, 196);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 11;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonCancel.Location = new System.Drawing.Point(223, 196);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// TxImageSizeForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(331, 240);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.comboType);
			this.Controls.Add(this.numericPack);
			this.Controls.Add(this.numericDepth);
			this.Controls.Add(this.numericChannels);
			this.Controls.Add(this.numericHeight);
			this.Controls.Add(this.numericWidth);
			this.Controls.Add(this.labelDepth);
			this.Controls.Add(this.labelChannels);
			this.Controls.Add(this.labelModel);
			this.Controls.Add(this.labelHeight);
			this.Controls.Add(this.labelWidth);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "TxImageSizeForm";
			this.Text = "Image Size";
			this.Load += new System.EventHandler(this.TxImageSizeForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericChannels)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericDepth)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.NumericUpDown numericWidth;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.NumericUpDown numericHeight;
		private System.Windows.Forms.Label labelModel;
		private System.Windows.Forms.Label labelChannels;
		private System.Windows.Forms.NumericUpDown numericPack;
		private System.Windows.Forms.ComboBox comboType;
		private System.Windows.Forms.NumericUpDown numericChannels;
		private System.Windows.Forms.Label labelDepth;
		private System.Windows.Forms.NumericUpDown numericDepth;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
	}
}