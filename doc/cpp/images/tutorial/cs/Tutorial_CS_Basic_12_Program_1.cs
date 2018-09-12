using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace tutorial
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			XIE.Axi.Setup();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());

			XIE.Axi.TearDown();
		}
	}
}
