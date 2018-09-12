namespace XIE.Media
{
	partial class CxDeviceSelectionForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxDeviceSelectionForm));
			this.Toolbar = new XIE.Forms.CxToolStripEx();
			this.toolCancel = new System.Windows.Forms.ToolStripButton();
			this.toolOK = new System.Windows.Forms.ToolStripButton();
			this.Statusbar = new System.Windows.Forms.StatusStrip();
			this.comboPin = new System.Windows.Forms.ComboBox();
			this.comboName = new System.Windows.Forms.ComboBox();
			this.labelPin = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.Toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// Toolbar
			// 
			this.Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolCancel,
            this.toolOK});
			this.Toolbar.Location = new System.Drawing.Point(0, 0);
			this.Toolbar.Name = "Toolbar";
			this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.Toolbar.Size = new System.Drawing.Size(484, 35);
			this.Toolbar.TabIndex = 1;
			this.Toolbar.Text = "toolbar";
			// 
			// toolCancel
			// 
			this.toolCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolCancel.AutoSize = false;
			this.toolCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolCancel.Image")));
			this.toolCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCancel.Name = "toolCancel";
			this.toolCancel.Size = new System.Drawing.Size(96, 32);
			this.toolCancel.Text = "Cancel";
			this.toolCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.toolCancel.Click += new System.EventHandler(this.toolCancel_Click);
			// 
			// toolOK
			// 
			this.toolOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolOK.AutoSize = false;
			this.toolOK.Image = ((System.Drawing.Image)(resources.GetObject("toolOK.Image")));
			this.toolOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolOK.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolOK.Name = "toolOK";
			this.toolOK.Size = new System.Drawing.Size(96, 32);
			this.toolOK.Text = "OK";
			this.toolOK.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.toolOK.Click += new System.EventHandler(this.toolOK_Click);
			// 
			// Statusbar
			// 
			this.Statusbar.AutoSize = false;
			this.Statusbar.Location = new System.Drawing.Point(0, 141);
			this.Statusbar.Name = "Statusbar";
			this.Statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.Statusbar.Size = new System.Drawing.Size(484, 24);
			this.Statusbar.TabIndex = 2;
			this.Statusbar.Text = "statusbar";
			// 
			// comboPin
			// 
			this.comboPin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPin.BackColor = System.Drawing.SystemColors.Control;
			this.comboPin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboPin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboPin.FormattingEnabled = true;
			this.comboPin.Location = new System.Drawing.Point(84, 93);
			this.comboPin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboPin.Name = "comboPin";
			this.comboPin.Size = new System.Drawing.Size(378, 23);
			this.comboPin.TabIndex = 3;
			this.comboPin.SelectedIndexChanged += new System.EventHandler(this.comboPin_SelectedIndexChanged);
			// 
			// comboName
			// 
			this.comboName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboName.BackColor = System.Drawing.SystemColors.Control;
			this.comboName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboName.FormattingEnabled = true;
			this.comboName.Location = new System.Drawing.Point(84, 57);
			this.comboName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.comboName.Name = "comboName";
			this.comboName.Size = new System.Drawing.Size(378, 23);
			this.comboName.TabIndex = 1;
			this.comboName.SelectedIndexChanged += new System.EventHandler(this.comboName_SelectedIndexChanged);
			// 
			// labelPin
			// 
			this.labelPin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPin.Location = new System.Drawing.Point(12, 93);
			this.labelPin.Name = "labelPin";
			this.labelPin.Size = new System.Drawing.Size(64, 23);
			this.labelPin.TabIndex = 2;
			this.labelPin.Text = "Pin";
			this.labelPin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelName
			// 
			this.labelName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelName.Location = new System.Drawing.Point(12, 56);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(64, 23);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// CxDeviceSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 165);
			this.Controls.Add(this.comboPin);
			this.Controls.Add(this.comboName);
			this.Controls.Add(this.labelPin);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.Statusbar);
			this.Controls.Add(this.Toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxDeviceSelectionForm";
			this.Text = "Device Selection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxDeviceSelectionForm_FormClosing);
			this.Load += new System.EventHandler(this.CxDeviceSelectionForm_Load);
			this.Toolbar.ResumeLayout(false);
			this.Toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private XIE.Forms.CxToolStripEx Toolbar;
		private System.Windows.Forms.StatusStrip Statusbar;
		private System.Windows.Forms.ComboBox comboName;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.ComboBox comboPin;
		private System.Windows.Forms.Label labelPin;
		private System.Windows.Forms.ToolStripButton toolCancel;
		private System.Windows.Forms.ToolStripButton toolOK;
		private System.Windows.Forms.Timer timerUpdateUI;
	}
}
