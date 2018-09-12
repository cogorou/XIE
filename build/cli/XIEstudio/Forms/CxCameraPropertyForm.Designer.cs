namespace XIEstudio
{
	partial class CxCameraPropertyForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxCameraPropertyForm));
			this.toolbar = new XIE.Forms.CxToolStripEx();
			this.toolRun = new System.Windows.Forms.ToolStripButton();
			this.toolPause = new System.Windows.Forms.ToolStripButton();
			this.toolStop = new System.Windows.Forms.ToolStripButton();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.propertyParam = new System.Windows.Forms.PropertyGrid();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolRun,
            this.toolPause,
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
			// toolPause
			// 
			this.toolPause.AutoSize = false;
			this.toolPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPause.Image = ((System.Drawing.Image)(resources.GetObject("toolPause.Image")));
			this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPause.Name = "toolPause";
			this.toolPause.Size = new System.Drawing.Size(32, 32);
			this.toolPause.Text = "Pause";
			this.toolPause.Click += new System.EventHandler(this.toolPause_Click);
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
			this.statusbar.Location = new System.Drawing.Point(0, 340);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(448, 22);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusStrip1";
			// 
			// propertyParam
			// 
			this.propertyParam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyParam.Location = new System.Drawing.Point(0, 35);
			this.propertyParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.propertyParam.Name = "propertyParam";
			this.propertyParam.Size = new System.Drawing.Size(448, 305);
			this.propertyParam.TabIndex = 2;
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Interval = 10;
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// CxCameraPropertyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(448, 362);
			this.Controls.Add(this.propertyParam);
			this.Controls.Add(this.statusbar);
			this.Controls.Add(this.toolbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxCameraPropertyForm";
			this.Text = "Camera Property";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxCameraPropertyForm_FormClosing);
			this.Load += new System.EventHandler(this.CxCameraPropertyForm_Load);
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public XIE.Forms.CxToolStripEx toolbar;
		public System.Windows.Forms.StatusStrip statusbar;
		public System.Windows.Forms.PropertyGrid propertyParam;
		private System.Windows.Forms.ToolStripButton toolRun;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripButton toolPause;
		private System.Windows.Forms.ToolStripButton toolStop;

	}
}