/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace demo
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			ImageView = new XIE.GDI.CxImageView();
			ImageView.Dock = DockStyle.Fill;
			ImageView.Rendering += new XIE.GDI.CxRenderingEventHandler(ImageView_Rendering);

			panelView.Controls.Add(ImageView);
		}

		XIE.GDI.CxImageView ImageView = null;

		void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var rect1 = new XIE.GDI.CxGdiRectangle(10, 20, 100, 50);
			var rect2 = new XIE.GDI.CxGdiRectangle(100, 60, 100, 50);
			var rect3 = new XIE.GDI.CxGdiRectangle(10, 100, 100, 50);

			rect1.PenColor = Color.Red;
			rect1.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
			rect1.PenWidth = 1;

			rect2.PenColor = Color.Lime;
			rect2.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
			rect2.PenWidth = 3;

			rect3.PenColor = Color.Blue;
			rect3.PenStyle = XIE.GDI.ExGdiPenStyle.Dash;
			rect3.PenWidth = 5;

			XIE.GDI.IxGdi2d[] figures = { rect1, rect2, rect3 };

			e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.TopLeft);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			ImageView.Image = new XIE.CxImage("TestFiles/cube.png");
		}
	}
}
