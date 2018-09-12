using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tutorial
{
	public partial class Form1 : Form
	{
		private XIE.GDI.CxImageView ImageView = null;		// (1)
		private XIE.CxImage Image = new XIE.CxImage();		// (2)

		public Form1()
		{
			InitializeComponent();

			// (3)
			this.ImageView = new XIE.GDI.CxImageView();
			this.ImageView.Image = this.Image;				// 画像オブジェクトのインスタンスを参照する.
			this.ImageView.Dock = DockStyle.Fill;
			this.Controls.Add(this.ImageView);				// フォームへ追加する.
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// (4)
			this.Image.Load("TestFiles/cube.png");
		}
	}
}
