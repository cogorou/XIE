using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace tutorial
{
	static class Program
	{
		/// <summary>
		/// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
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
