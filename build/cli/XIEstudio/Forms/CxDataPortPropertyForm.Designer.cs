namespace XIEstudio
{
	partial class CxDataPortPropertyForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
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

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxDataPortPropertyForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolRun = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.propertyParam = new System.Windows.Forms.PropertyGrid();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.MainTab = new System.Windows.Forms.TabControl();
			this.tabDIO = new System.Windows.Forms.TabPage();
			this.tabProperty = new System.Windows.Forms.TabPage();
			this.groupDioBits = new System.Windows.Forms.GroupBox();
			this.textDI = new System.Windows.Forms.TextBox();
			this.textDO = new System.Windows.Forms.TextBox();
			this.toolbar.SuspendLayout();
			this.MainTab.SuspendLayout();
			this.tabDIO.SuspendLayout();
			this.tabProperty.SuspendLayout();
			this.groupDioBits.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolRun,
            this.toolStop});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbar.Size = new System.Drawing.Size(448, 35);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolbar";
			// 
			// toolRun
			// 
			this.toolRun.AutoSize = false;
			this.toolRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolRun.Image = ((System.Drawing.Image)(resources.GetObject("toolRun.Image")));
			this.toolRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolRun.Name = "toolRun";
			this.toolRun.Size = new System.Drawing.Size(32, 32);
			this.toolRun.Text = "Run";
			this.toolRun.Click += new System.EventHandler(this.toolRun_Click);
			// 
			// toolStop
			// 
			this.toolStop.AutoSize = false;
			this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
			this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStop.Name = "toolStop";
			this.toolStop.Size = new System.Drawing.Size(32, 32);
			this.toolStop.Text = "Stop";
			this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
			// 
			// statusbar
			// 
			this.statusbar.Location = new System.Drawing.Point(0, 503);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(448, 22);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// propertyParam
			// 
			this.propertyParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyParam.Location = new System.Drawing.Point(3, 3);
			this.propertyParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.propertyParam.Name = "propertyParam";
			this.propertyParam.Size = new System.Drawing.Size(434, 431);
			this.propertyParam.TabIndex = 0;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// MainTab
			// 
			this.MainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.MainTab.Controls.Add(this.tabDIO);
			this.MainTab.Controls.Add(this.tabProperty);
			this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainTab.Location = new System.Drawing.Point(0, 35);
			this.MainTab.Name = "MainTab";
			this.MainTab.SelectedIndex = 0;
			this.MainTab.Size = new System.Drawing.Size(448, 468);
			this.MainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.MainTab.TabIndex = 2;
			// 
			// tabDIO
			// 
			this.tabDIO.Controls.Add(this.groupDioBits);
			this.tabDIO.Location = new System.Drawing.Point(4, 27);
			this.tabDIO.Name = "tabDIO";
			this.tabDIO.Padding = new System.Windows.Forms.Padding(3);
			this.tabDIO.Size = new System.Drawing.Size(440, 437);
			this.tabDIO.TabIndex = 0;
			this.tabDIO.Text = "DIO";
			this.tabDIO.UseVisualStyleBackColor = true;
			// 
			// tabProperty
			// 
			this.tabProperty.Controls.Add(this.propertyParam);
			this.tabProperty.Location = new System.Drawing.Point(4, 27);
			this.tabProperty.Name = "tabProperty";
			this.tabProperty.Padding = new System.Windows.Forms.Padding(3);
			this.tabProperty.Size = new System.Drawing.Size(440, 437);
			this.tabProperty.TabIndex = 1;
			this.tabProperty.Text = "Property";
			this.tabProperty.UseVisualStyleBackColor = true;
			// 
			// groupDioBits
			// 
			this.groupDioBits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDioBits.Controls.Add(this.textDO);
			this.groupDioBits.Controls.Add(this.textDI);
			this.groupDioBits.Location = new System.Drawing.Point(9, 7);
			this.groupDioBits.Name = "groupDioBits";
			this.groupDioBits.Size = new System.Drawing.Size(423, 91);
			this.groupDioBits.TabIndex = 0;
			this.groupDioBits.TabStop = false;
			this.groupDioBits.Text = "DIO Bits";
			// 
			// textDI
			// 
			this.textDI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDI.BackColor = System.Drawing.Color.White;
			this.textDI.ForeColor = System.Drawing.Color.Red;
			this.textDI.Location = new System.Drawing.Point(6, 23);
			this.textDI.Name = "textDI";
			this.textDI.ReadOnly = true;
			this.textDI.Size = new System.Drawing.Size(411, 23);
			this.textDI.TabIndex = 0;
			// 
			// textDO
			// 
			this.textDO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDO.BackColor = System.Drawing.Color.White;
			this.textDO.ForeColor = System.Drawing.Color.Blue;
			this.textDO.Location = new System.Drawing.Point(6, 52);
			this.textDO.Name = "textDO";
			this.textDO.ReadOnly = true;
			this.textDO.Size = new System.Drawing.Size(411, 23);
			this.textDO.TabIndex = 0;
			// 
			// CxDataPortPropertyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(448, 525);
			this.Controls.Add(this.MainTab);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxDataPortPropertyForm";
			this.Text = "DataPort Property";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxDataPortPropertyForm_FormClosing);
			this.Load += new System.EventHandler(this.CxDataPortPropertyForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.MainTab.ResumeLayout(false);
			this.tabDIO.ResumeLayout(false);
			this.tabProperty.ResumeLayout(false);
			this.groupDioBits.ResumeLayout(false);
			this.groupDioBits.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public XIE.Forms.CxToolStripEx toolbar;
		public System.Windows.Forms.StatusStrip statusbar;
		public System.Windows.Forms.PropertyGrid propertyParam;
		private System.Windows.Forms.ToolStripButton toolRun;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripButton toolStop;
		private System.Windows.Forms.TabControl MainTab;
		private System.Windows.Forms.TabPage tabDIO;
		private System.Windows.Forms.GroupBox groupDioBits;
		private System.Windows.Forms.TextBox textDO;
		private System.Windows.Forms.TextBox textDI;
		private System.Windows.Forms.TabPage tabProperty;

	}
}