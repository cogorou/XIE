/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace VideoEncode
{
	class Program
	{
		const string AppName = "VideoEncode";
		static string ResultDir = "Results";

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				// Results
				const string result_dir = "Results";
				System.IO.Directory.CreateDirectory(result_dir);

				// Results/AppName
				ResultDir = System.IO.Path.Combine(result_dir, AppName);
				System.IO.Directory.CreateDirectory(ResultDir);

				// Execute
				Encode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				XIE.Axi.TearDown();
			}
		}
		/// <summary>
		/// Encode
		/// </summary>
		static void Encode()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.Multiselect = true;
			ofd.Filter = "Video files |*.avi;*.wmv;*.mp4";
			ofd.Filter += "|All files (*.*)|*.*";

			if (ofd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				var audio_input = new XIE.Media.CxDeviceParam();
				{
					var devices = new XIE.Media.CxDeviceList();
					devices.Setup(XIE.Media.ExMediaType.Audio, XIE.Media.ExMediaDir.Input);
					int length = devices.Length;
					if (length > 0)
					{
						audio_input.Name = devices[0].Name;
						audio_input.Index = devices[0].Index;
					}
				}

				for (int sss = 0; sss < ofd.FileNames.Length; sss++)
				{
					var source_name = System.IO.Path.GetFileNameWithoutExtension(ofd.FileNames[sss]);
					var output_name = string.Format("{0}.wmv", source_name);
					Console.WriteLine("{0}: {1}={2}", __FUNCTION__, sss, source_name);

					// --------------------------------------------------
					try
					{
						var watch = new XIE.CxStopwatch();

						// 1) 初期化.
						var controller = new XIE.Media.CxMediaPlayer();
						var source_file = new XIE.CxStringA(ofd.FileNames[sss]);
						var output_file = new XIE.CxStringA(System.IO.Path.Combine(ResultDir, output_name));
						controller.Setup(source_file, null, output_file);

						// 2) 周期処理.
						watch.Start();
						{
							controller.Start();							// 再生開始.
							controller.WaitForCompletion(30 * 1000);	// 待機.
							controller.Stop();							// 再生停止.
						}
						watch.Stop();
						Console.WriteLine("{0}: Elapsed={1} msec", __FUNCTION__, watch.Elapsed);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.StackTrace);
					}
				}
			}
		}
	}
}
