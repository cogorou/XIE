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
		private XIE.GDI.CxImageView ImageView = null;
		private XIE.CxImage Image = new XIE.CxImage();

		public Form1()
		{
			InitializeComponent();

			this.ImageView = new XIE.GDI.CxImageView();
			this.ImageView.Image = this.Image;
			this.ImageView.Dock = DockStyle.Fill;
			this.Controls.Add(this.ImageView);

			// (1)
			this.ImageView.Rendering += ImageView_Rendering;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.Image.Load("TestFiles/cube.png");
		}

		// (2)
		void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure1 = new XIE.GDI.CxGdiEllipse(30, 40, 100, 50);
			var figure2 = new XIE.GDI.CxGdiLineSegment(100, 60, 300, 200);
			var figure3 = new XIE.GDI.CxGdiRectangle(350, 250, 200, 150);

			// ellipse
			figure1.PenColor = Color.Red;
			figure1.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
			figure1.PenWidth = 1;

			// linesegment
			figure2.PenColor = Color.Lime;
			figure2.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
			figure2.PenWidth = 3;

			// rectangle
			figure3.PenColor = Color.Blue;
			figure3.PenStyle = XIE.GDI.ExGdiPenStyle.Dash;
			figure3.PenWidth = 5;
			figure3.BkColor = new XIE.TxRGB8x4(0, 0, 255, 64);	// 00,00,FF (blue) opacity 25% (64/255)
			figure3.BkEnable = true;

			XIE.GDI.IxGdi2d[] figures = { figure1, figure2, figure3 };

			e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.TopLeft);
		}
	}
}
