namespace XIE.Log
{
	partial class CxLogForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxLogForm));
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.tabLogs = new System.Windows.Forms.TabControl();
			this.tabPage0 = new System.Windows.Forms.TabPage();
			this.logView0 = new XIE.Log.CxLogView();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.logView1 = new XIE.Log.CxLogView();
			this.tabLogs.SuspendLayout();
			this.tabPage0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logView0)).BeginInit();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logView1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonClose.Location = new System.Drawing.Point(723, 531);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(96, 32);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
			this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonSave.Location = new System.Drawing.Point(621, 531);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(96, 32);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// tabLogs
			// 
			this.tabLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabLogs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabLogs.Controls.Add(this.tabPage0);
			this.tabLogs.Controls.Add(this.tabPage1);
			this.tabLogs.Location = new System.Drawing.Point(0, 0);
			this.tabLogs.Name = "tabLogs";
			this.tabLogs.SelectedIndex = 0;
			this.tabLogs.Size = new System.Drawing.Size(826, 525);
			this.tabLogs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabLogs.TabIndex = 0;
			// 
			// tabPage0
			// 
			this.tabPage0.Controls.Add(this.logView0);
			this.tabPage0.Location = new System.Drawing.Point(4, 25);
			this.tabPage0.Name = "tabPage0";
			this.tabPage0.Size = new System.Drawing.Size(818, 496);
			this.tabPage0.TabIndex = 0;
			this.tabPage0.Text = "Trace";
			this.tabPage0.UseVisualStyleBackColor = true;
			// 
			// logView0
			// 
			this.logView0.AllowUserToAddRows = false;
			this.logView0.AllowUserToDeleteRows = false;
			this.logView0.AllowUserToResizeRows = false;
			this.logView0.AutoScroll = false;
			this.logView0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.logView0.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logView0.Interval = 200;
			this.logView0.Level = XIE.Log.ExLogLevel.Trace;
			this.logView0.Location = new System.Drawing.Point(0, 0);
			this.logView0.Logs = null;
			this.logView0.Name = "logView0";
			this.logView0.ReadOnly = true;
			this.logView0.RowHeadersVisible = false;
			this.logView0.RowTemplate.Height = 21;
			this.logView0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.logView0.Size = new System.Drawing.Size(818, 496);
			this.logView0.TabIndex = 0;
			this.logView0.VirtualMode = true;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.logView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(818, 496);
			this.tabPage1.TabIndex = 1;
			this.tabPage1.Text = "Error";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// logView1
			// 
			this.logView1.AllowUserToAddRows = false;
			this.logView1.AllowUserToDeleteRows = false;
			this.logView1.AllowUserToResizeRows = false;
			this.logView1.AutoScroll = false;
			this.logView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logView1.Interval = 200;
			this.logView1.Level = XIE.Log.ExLogLevel.Trace;
			this.logView1.Location = new System.Drawing.Point(0, 0);
			this.logView1.Logs = null;
			this.logView1.Name = "logView1";
			this.logView1.ReadOnly = true;
			this.logView1.RowHeadersVisible = false;
			this.logView1.RowTemplate.Height = 21;
			this.logView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.logView1.Size = new System.Drawing.Size(818, 496);
			this.logView1.TabIndex = 1;
			this.logView1.VirtualMode = true;
			// 
			// CxLogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(825, 571);
			this.Controls.Add(this.tabLogs);
			this.Controls.Add(this.buttonSave);
			this.Controls.Add(this.buttonClose);
			this.Name = "CxLogForm";
			this.Text = "CxLogForm";
			this.Load += new System.EventHandler(this.CxLogForm_Load);
			this.tabLogs.ResumeLayout(false);
			this.tabPage0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.logView0)).EndInit();
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.logView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.TabControl tabLogs;
		private System.Windows.Forms.TabPage tabPage0;
		private XIE.Log.CxLogView logView0;
		private System.Windows.Forms.TabPage tabPage1;
		private XIE.Log.CxLogView logView1;
	}
}