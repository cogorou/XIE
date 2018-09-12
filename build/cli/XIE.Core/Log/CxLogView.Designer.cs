namespace XIE.Log
{
	partial class CxLogView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.viewTimer = new System.Windows.Forms.Timer();
			this.SuspendLayout();
			// 
			// viewTimer
			// 
			this.viewTimer.Tick += new System.EventHandler(this.viewTimer_Tick);
			// 
			// CxLogView
			// 
			this.AllowUserToAddRows = false;
			this.AllowUserToDeleteRows = false;
			this.AllowUserToResizeRows = false;
			this.ReadOnly = true;
			this.RowHeadersVisible = false;
			this.RowTemplate.Height = 21;
			this.Size = new System.Drawing.Size(499, 351);
			this.VirtualMode = true;
			this.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.CxLogView_CellValueNeeded);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer viewTimer;
	}
}
