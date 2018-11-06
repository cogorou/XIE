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
using System.Threading.Tasks;
using System.IO;

namespace DumpExif
{
	class Program
	{
		const string AppName = "DumpExif";

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				var commands = ParseArguments(args);
				string value = "";

				int output_mode = 0;	// 0:feature
				if (commands.TryGetValue("d", out value))
					output_mode = 1;	// 1:detail

				if (Parameters.Count == 0)
				{
					// ダイアログでファイル選択する.
					var filenames = SelectFiles();
					Execute(filenames, output_mode);
				}
				else
				{
					// 指定されたディレクトリの配下を再帰的に検索する.
					var filenames = SearchFilesRecursively(Parameters);
					Execute(filenames, output_mode);
				}

				Console.WriteLine("Completed.");
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

		#region 引数解析:

		/// <summary>
		/// パラメータ
		/// </summary>
		static List<string> Parameters = new List<string>();

		/// <summary>
		/// 引数解析
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		/// <returns>
		///		解析結果を返します。
		/// </returns>
		private static Dictionary<string, string> ParseArguments(string[] args)
		{
			var commands = new Dictionary<string, string>();

			foreach (string arg in args)
			{
				if (arg.StartsWith("/"))
				{
					int colon = arg.IndexOf(':');
					if (colon > 1)
					{
						string key = arg.Substring(1, colon - 1);
						string val = arg.Substring(colon + 1);
						commands[key] = val;
					}
					else
					{
						string key = arg.Substring(1);
						commands[key] = "";
					}
				}
				else
				{
					Parameters.Add(arg);
				}
			}

			return commands;
		}

		#endregion

		#region 入力ファイル関連:

		/// <summary>
		/// ファイルを選択する
		/// </summary>
		static List<string> SelectFiles()
		{
			var result = new List<string>();

			var ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.Multiselect = true;
			ofd.Filter = "Image files |*.jpg;*.jpeg;*.raw;";
			ofd.Filter += "|All files (*.*)|*.*";

			if (ofd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				result.AddRange(ofd.FileNames);
			}

			return result;
		}

		/// <summary>
		/// 再帰的に検索する
		/// </summary>
		/// <param name="dirs">対象ディレクトリのリスト</param>
		static List<string> SearchFilesRecursively(IEnumerable<string> dirs)
		{
			var result = new List<string>();

			foreach (var dir in dirs)
			{
				if (System.IO.Directory.Exists(dir))
				{
					var filenames1 = System.IO.Directory.GetFiles(dir);
					foreach (var filename in filenames1)
					{
						var ext = System.IO.Path.GetExtension(filename).ToLower();
						if (ext == ".jpg" || ext == ".jpeg")
						{
							result.Add(filename);
						}
					}

					var subdirs = System.IO.Directory.GetDirectories(dir);
					var filenames2 = SearchFilesRecursively(subdirs);
					result.AddRange(filenames2);
				}
			}

			return result;
		}

		#endregion

		/// <summary>
		/// 指定されたファイルを処理する
		/// </summary>
		/// <param name="filenames">処理対象ファイルのリスト</param>
		/// <param name="output_mode">出力モード [0:要点、1:詳細]</param>
		static void Execute(IEnumerable<string> filenames, int output_mode)
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var sfd = new SaveFileDialog();

			#region ファイル保存ダイアログ設定:
			{
				var now = DateTime.Now;
				var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
					now.Year, now.Month, now.Day,
					now.Hour, now.Minute, now.Second);

				sfd.AddExtension = true;
				sfd.DefaultExt = "txt";
				sfd.Filter = "Text files |*.txt;";
				sfd.Filter += "|All files |*.*";
				sfd.FileName = string.Format("{0}_{1}.txt", AppName, suffix);
			}
			#endregion

			if (sfd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				using (var stream = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
				{
					var watch = new XIE.CxStopwatch();
					foreach (var filename in filenames)
					{
						#region Exif 出力:
						var ext = System.IO.Path.GetExtension(filename).ToLower();
						if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
						{
							watch.Start();
							using (var image = new XIE.CxImage(filename))
							using (var exif = XIE.CxExif.FromTag(image.Exif()))
							{
								stream.WriteLine("");
								stream.WriteLine("{0}	{1}",
									System.IO.Path.GetFileName(filename),
									System.IO.Path.GetDirectoryName(filename));
								if (exif.IsValid)
								{
									switch (output_mode)
									{
										default:
										case 0:
											DumpFeature(stream, exif);
											break;
										case 1:
											DumpDetail(stream, exif);
											break;
									}
								}
								else
								{
									stream.WriteLine("	Empty");
								}
							}
							watch.Stop();

							Console.WriteLine("{0,9:F3} msec : {1}", watch.Lap, filename);
						}
						#endregion
					}
				}
			}
		}

		#region DumpFeature:

		/// <summary>
		/// Exif 出力 (要点)
		/// </summary>
		/// <param name="stream">出力先のストリーム</param>
		/// <param name="exif"></param>
		static void DumpFeature(StreamWriter stream, XIE.CxExif exif)
		{
			var items = exif.GetItems();

			string camera_model = "";
			string lens_model = "";
			string focal_length_real = "";
			string focal_length_35mm = "";
			string f_number = "";
			string exposure_time = "";
			string iso_speed = "";

			#region 抽出:
			foreach (var item in items)
			{
				switch (item.ID)
				{
					case 0x0110:	//  2: カメラのモデル:
						if (item.Type == 2)
						{
							using (var value = new XIE.CxStringA())
							{
								exif.GetValue(item, value);
								if (value.IsValid)
								{
									camera_model = value.ToString().Trim();
								}
							}
						}
						break;
					case 0xA434:	//  2: レンズモデル:
						if (item.Type == 2)
						{
							using (var value = new XIE.CxStringA())
							{
								exif.GetValue(item, value);
								if (value.IsValid)
								{
									lens_model = value.ToString().Trim();
								}
							}
						}
						break;
					case 0x920A:	//  5: 焦点距離:
						if (item.Type == 5)
						{
							using (var value = new XIE.CxArray())
							{
								exif.GetValue(item, value);
								if (value.Model == XIE.TxModel.U32(2))
								{
									var scan = value.Scanner();
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt32Ptr)addr;
										if (x == 0)
										{
											var value0 = (double)ptr[0];
											var value1 = (double)ptr[1];
											if (value0 > 0 && value1 > 0)
											{
												var ans = value0 / value1;
												if (ans == System.Math.Floor(ans))
												{
													focal_length_real = ans.ToString("F0");
												}
												else
												{
													focal_length_real = ans.ToString("F2");
												}
											}
										}
									});
								}
							}
						}
						break;
					case 0xA405:	//  3: 焦点距離（35mm 換算）:
						if (item.Type == 3)
						{
							using (var value = new XIE.CxArray())
							{
								exif.GetValue(item, value);
								if (value.Model == XIE.TxModel.U16(1))
								{
									var scan = value.Scanner();
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt16Ptr)addr;
										if (x == 0)
										{
											focal_length_35mm = ptr[0].ToString();
										}
									});
								}
							}
						}
						break;
					case 0x829D:	//  5: F 値:
						if (item.Type == 5)
						{
							using (var value = new XIE.CxArray())
							{
								exif.GetValue(item, value);
								if (value.Model == XIE.TxModel.U32(2))
								{
									var scan = value.Scanner();
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt32Ptr)addr;
										if (x == 0)
										{
											var value0 = (double)ptr[0];
											var value1 = (double)ptr[1];
											if (value0 > 0 && value1 > 0)
											{
												var ans = value0 / value1;
												if (ans == System.Math.Floor(ans))
												{
													f_number = ans.ToString("F0");
												}
												else if (ans >= 1)
												{
													f_number = ans.ToString("F1");
												}
												else
												{
													f_number = ans.ToString("F2");
												}
											}
										}
									});
								}
							}
						}
						break;
					case 0x829A:	//  5: 露光時間:
						if (item.Type == 5)
						{
							using (var value = new XIE.CxArray())
							{
								exif.GetValue(item, value);
								if (value.Model == XIE.TxModel.U32(2))
								{
									var scan = value.Scanner();
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt32Ptr)addr;
										if (x == 0)
										{
											var value0 = (double)ptr[0];
											var value1 = (double)ptr[1];
											if (value0 > 0 && value1 > 0)
											{
												var ans = value0 / value1;
												if (ans < 1)
												{
													if (value0 > 1)
													{
														exposure_time = string.Format("1/{0:F0}", value1/value0);
													}
													else
													{
														exposure_time = string.Format("{0}/{1}", value0, value1);
													}
												}
												else
												{
													exposure_time = ans.ToString("F0");
												}
											}
										}
									});
								}
							}
						}
						break;
					case 0x8827:	//  3: ISO 感度設定:
						if (item.Type == 3)
						{
							using (var value = new XIE.CxArray())
							{
								exif.GetValue(item, value);
								if (value.Model == XIE.TxModel.U16(1))
								{
									var scan = value.Scanner();
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt16Ptr)addr;
										if (x == 0)
										{
											iso_speed = ptr[0].ToString();
										}
									});
								}
							}
						}
						break;
				}
			}
			#endregion

			string result = "	";

			#region 連結:

			// 焦点距離:
			if (string.IsNullOrWhiteSpace(focal_length_real) == false)
			{
				if (string.IsNullOrWhiteSpace(focal_length_35mm) == false)
				{
					result += string.Format("{0}mm (equiv. {1}mm)", focal_length_real, focal_length_35mm);
				}
				else
				{
					result += string.Format("{0}mm", focal_length_real);
				}
			}

			// F 値:
			if (string.IsNullOrWhiteSpace(f_number) == false)
			{
				result += string.Format(" : F{0}", f_number);
			}

			// 露光時間:
			if (string.IsNullOrWhiteSpace(exposure_time) == false)
			{
				result += string.Format(" : {0} sec.", exposure_time);
			}

			// ISO 感度:
			if (string.IsNullOrWhiteSpace(iso_speed) == false)
			{
				result += string.Format(" : ISO {0}", iso_speed);
			}

			// モデル名:
			if (string.IsNullOrWhiteSpace(lens_model) == false)
			{
				result += string.Format(" : {0}", lens_model);
			}
			else if (string.IsNullOrWhiteSpace(camera_model) == false)
			{
				result += string.Format(" : {0}", camera_model);
			}

			#endregion

			stream.WriteLine("	{0}", result);
		}

		#endregion

		#region DumpDetail:

		/// <summary>
		/// Exif 出力 (詳細)
		/// </summary>
		/// <param name="stream">出力先のストリーム</param>
		/// <param name="exif"></param>
		static void DumpDetail(StreamWriter stream, XIE.CxExif exif)
		{
			var items = exif.GetItems();

			foreach (var item in items)
			{
				var nickname = GetNickName(item.ID);
				string strValue = "";

				switch (item.Type)
				{
					case 1:		// BYTE
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.U8(1))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.BytePtr)addr;
									if (x == 0)
										strValue += string.Format("{0}", ptr[0]);
									else
										strValue += string.Format(",{0}", ptr[0]);
								});
							}
						}
						break;
					case 7:		// UNDEFINED
						switch (item.ID)
						{
							case 0x9000:	// Exif version
							case 0xA000:	// Supported FlashPix version
								{
									var value = new XIE.CxArray();
									exif.GetValue(item, value);
									if (value.Length == 4)
									{
										var addr = (XIE.Ptr.BytePtr)value.Address();
										var data = addr.ToArray(value.Length);
										strValue = System.Text.Encoding.ASCII.GetString(data);
									}
								}
								break;
						}
						break;
					case 2:		// ASCII
						{
							var value = new XIE.CxStringA();
							exif.GetValue(item, value);
							if (value.IsValid)
							{
								strValue = value.ToString();
							}
						}
						break;
					case 3:		// SHORT
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.U16(1))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.UInt16Ptr)addr;
									if (x == 0)
										strValue += string.Format("{0}", ptr[0]);
									else
										strValue += string.Format(",{0}", ptr[0]);
								});
							}
						}
						break;
					case 4:		// LONG
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.U32(1))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.UInt32Ptr)addr;
									if (x == 0)
										strValue += string.Format("{0}", ptr[0]);
									else
										strValue += string.Format(",{0}", ptr[0]);
								});
							}
						}
						break;
					case 9:		// SLONG 32bit
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.S32(1))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.Int32Ptr)addr;
									if (x == 0)
										strValue += string.Format("{0}", ptr[0]);
									else
										strValue += string.Format(",{0}", ptr[0]);
								});
							}
						}
						break;
					case 5:		// RATIONAL
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.U32(2))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.UInt32Ptr)addr;
									if (x == 0)
										strValue += string.Format("{0}/{1}", ptr[0], ptr[1]);
									else
										strValue += string.Format(",{0}/{1}", ptr[0], ptr[1]);
								});
							}
						}
						break;
					case 10:	// SRATIONAL
						{
							var value = new XIE.CxArray();
							exif.GetValue(item, value);
							var scan = value.Scanner();
							if (value.Model == XIE.TxModel.S32(2))
							{
								scan.ForEach((int x, IntPtr addr) =>
								{
									var ptr = (XIE.Ptr.Int32Ptr)addr;
									if (x == 0)
										strValue += string.Format("{0}/{1}", ptr[0], ptr[1]);
									else
										strValue += string.Format(",{0}/{1}", ptr[0], ptr[1]);
								});
							}
						}
						break;
				}


				stream.WriteLine("	[{0:X8}] {1:X4} {2,2} count={3,6} value={4,12} ({4:X8}) [{5,-32}] {6}", item.Offset, item.ID, item.Type, item.Count, item.ValueOrIndex, strValue, nickname);
			}
		}

		#endregion

		#region GetNickName:

		/// <summary>
		/// NickName
		/// </summary>
		/// <param name="id">Exif ID</param>
		/// <param name="mode">language mode [0:English, 1:Japanese]</param>
		/// <returns>
		///		return NickName
		/// </returns>
		static string GetNickName(ushort id, int mode = 1)
		{
			string name = "";

			#region 変換:
			if (mode == 1)
			{
				#region 変換: (日本語)
				switch (id)
				{
					// 4.6.8 Tag Support Levels
					// Table 17
					case 0x100: name = "画像の幅"; break;
					case 0x101: name = "画像の高さ"; break;
					case 0x102: name = "ビットの深さ"; break;
					case 0x103: name = "圧縮の種類"; break;
					case 0x106: name = "画素合成"; break;
					case 0x10E: name = "タイトル"; break;
					case 0x10F: name = "カメラのメーカー"; break;
					case 0x110: name = "カメラのモデル"; break;
					case 0x111: name = "ロケーション"; break;
					case 0x112: name = "画像の方向"; break;
					case 0x115: name = "コンポーネント数"; break;
					case 0x116: name = "ストリップ毎の行数"; break;
					case 0x117: name = "圧縮ストリップ毎のバイト数"; break;
					case 0x11A: name = "水平方向の解像度"; break;
					case 0x11B: name = "垂直方向の解像度"; break;
					case 0x11C: name = "画像データの配置"; break;
					case 0x128: name = "解像度の単位"; break;
					case 0x12D: name = "Transfer function"; break;
					case 0x131: name = "ソフトウェア名"; break;
					case 0x132: name = "変更日時"; break;
					case 0x13B: name = "作成者"; break;
					case 0x13E: name = "白色点の色度"; break;
					case 0x13F: name = "原色の色度"; break;
					case 0x201: name = "JPEG SOI オフセット"; break;
					case 0x202: name = "JPEG データオフセット"; break;
					case 0x211: name = "色空間変換行列係数"; break;
					case 0x212: name = "Subsampling ration of Y to C"; break;
					case 0x213: name = "Y and C positioning"; break;
					case 0x214: name = "黒白点の基準値"; break;
					case 0x8298: name = "著作者"; break;
					case 0x8769: name = "Exif IFD ポインタ"; break;
					case 0x8825: name = "GPS IFD ポインタ"; break;
					// Table 18
					case 0x829A: name = "露光時間"; break;
					case 0x829D: name = "F 値"; break;
					case 0x8822: name = "露出モード"; break;
					case 0x8824: name = "分光感度"; break;
					case 0x8827: name = "ISO 感度設定"; break;
					case 0x8828: name = "光電変換係数"; break;
					case 0x8830: name = "感度種別"; break;
					case 0x8831: name = "標準出力感度"; break;
					case 0x8832: name = "推奨露光指数"; break;
					case 0x8833: name = "ISO 感度"; break;
					case 0x8834: name = "ISO 感度 緯度 yyy"; break;
					case 0x8835: name = "ISO 感度 緯度 zzz"; break;
					case 0x9000: name = "Exif バージョン"; break;
					case 0x9003: name = "撮影日時"; break;
					case 0x9004: name = "作成日時"; break;
					case 0x9101: name = "各コンポーネントの意味"; break;
					case 0x9102: name = "画像圧縮モード"; break;
					case 0x9201: name = "シャッタースピード"; break;
					case 0x9202: name = "絞り値"; break;
					case 0x9203: name = "明るさ"; break;
					case 0x9204: name = "露出補正"; break;
					case 0x9205: name = "最大絞り"; break;
					case 0x9206: name = "被写体距離"; break;
					case 0x9207: name = "測光モード"; break;
					case 0x9208: name = "光源"; break;
					case 0x9209: name = "フラッシュ"; break;
					case 0x920A: name = "焦点距離"; break;
					case 0x9214: name = "被写体面積"; break;
					case 0x927C: name = "メーカーノート"; break;
					case 0x9286: name = "コメント"; break;
					case 0x9290: name = "作成日時サブ秒"; break;
					case 0x9291: name = "作成日時サブ秒 (original)"; break;
					case 0x9292: name = "作成日時サブ秒 (digitized)"; break;
					case 0xA000: name = "対応 FlashPix バージョン"; break;
					case 0xA001: name = "色空間情報"; break;
					case 0xA002: name = "実効画像幅"; break;
					case 0xA003: name = "実効画像高さ"; break;
					case 0xA004: name = "音声ファイル"; break;
					case 0xA005: name = "互換性 IFD ポインタ"; break;
					case 0xA20B: name = "フラッシュモード"; break;
					case 0xA20C: name = "空間周波数応答"; break;
					case 0xA20E: name = "焦点面 X 解像度"; break;
					case 0xA20F: name = "焦点面 Y 解像度"; break;
					case 0xA210: name = "焦点面解像度単位"; break;
					case 0xA214: name = "被写体位置"; break;
					case 0xA215: name = "露光指標"; break;
					case 0xA217: name = "センサー方式"; break;
					case 0xA300: name = "ファイルソース"; break;
					case 0xA301: name = "シーン種別"; break;
					case 0xA302: name = "CFA pattern"; break;
					case 0xA401: name = "カスタムイメージ処理"; break;
					case 0xA402: name = "露光モード"; break;
					case 0xA403: name = "ホワイトバランス"; break;
					case 0xA404: name = "デジタルズーム"; break;
					case 0xA405: name = "焦点距離（35mm 換算）"; break;
					case 0xA406: name = "シーン撮影モード"; break;
					case 0xA407: name = "ゲイン調整"; break;
					case 0xA408: name = "コントラスト"; break;
					case 0xA409: name = "彩度"; break;
					case 0xA40A: name = "明瞭度"; break;
					case 0xA40B: name = "デバイス設定説明"; break;
					case 0xA40C: name = "被写体距離範囲"; break;
					case 0xA420: name = "Unique image ID"; break;
					case 0xA430: name = "カメラ所有者"; break;
					case 0xA431: name = "カメラ製造番号"; break;
					case 0xA432: name = "レンズ仕様"; break;
					case 0xA433: name = "レンズメーカー"; break;
					case 0xA434: name = "レンズモデル"; break;
					case 0xA435: name = "レンズ製造番号"; break;
					case 0xA500: name = "ガンマ値"; break;
				}
				#endregion
			}
			else
			{
				#region 変換: (英語)
				switch (id)
				{
					// 4.6.8 Tag Support Levels
					// Table 17
					case 0x100: name = "Image width"; break;
					case 0x101: name = "Image height"; break;
					case 0x102: name = "Number of bits per component"; break;
					case 0x103: name = "Compression scheme"; break;
					case 0x106: name = "Pixel composition"; break;
					case 0x10E: name = "Image title"; break;
					case 0x10F: name = "Manufacturer of image input equipment"; break;
					case 0x110: name = "Model of image input equipment"; break;
					case 0x111: name = "Image data location"; break;
					case 0x112: name = "Orientation of image"; break;
					case 0x115: name = "Number of components"; break;
					case 0x116: name = "Number of rows per strip"; break;
					case 0x117: name = "Bytes per compressed strip"; break;
					case 0x11A: name = "Image resolution in width direction"; break;
					case 0x11B: name = "Image resolution in height direction"; break;
					case 0x11C: name = "Image data arrangement"; break;
					case 0x128: name = "Unit of X and Y resolution"; break;
					case 0x12D: name = "Transfer function"; break;
					case 0x131: name = "Software used"; break;
					case 0x132: name = "File change date and time"; break;
					case 0x13B: name = "Person who created the image"; break;
					case 0x13E: name = "White point chromaticity"; break;
					case 0x13F: name = "Chromaticities of primaries"; break;
					case 0x201: name = "Offset to JPEG SOI"; break;
					case 0x202: name = "Offset to JPEG data"; break;
					case 0x211: name = "Color space transformation matrix coefficients"; break;
					case 0x212: name = "Subsampling ration of Y to C"; break;
					case 0x213: name = "Y and C positioning"; break;
					case 0x214: name = "Pair of black and white reference values"; break;
					case 0x8298: name = "Copyright holder"; break;
					case 0x8769: name = "Exif IFD pointer"; break;
					case 0x8825: name = "GPS IFD pointer"; break;
					// Table 18
					case 0x829A: name = "Exposure time"; break;
					case 0x829D: name = "F number"; break;
					case 0x8822: name = "Exposure program"; break;
					case 0x8824: name = "Spectral sensitivity"; break;
					case 0x8827: name = "ISO speed ratings"; break;
					case 0x8828: name = "Optoelectric conversion factor"; break;
					case 0x8830: name = "Sensitivity Type"; break;
					case 0x8831: name = "Standard Output Sensitivity"; break;
					case 0x8832: name = "Recommended Exposure Index"; break;
					case 0x8833: name = "ISO Speed"; break;
					case 0x8834: name = "ISO Speed Latitude yyy"; break;
					case 0x8835: name = "ISO Speed Latitude zzz"; break;
					case 0x9000: name = "Exif version"; break;
					case 0x9003: name = "Date and time of original data generation"; break;
					case 0x9004: name = "Date and time of digital data generation"; break;
					case 0x9101: name = "Meaning of each component"; break;
					case 0x9102: name = "Image compression mode"; break;
					case 0x9201: name = "Shutter speed"; break;
					case 0x9202: name = "Aperture"; break;
					case 0x9203: name = "Brightness"; break;
					case 0x9204: name = "Exposure bias"; break;
					case 0x9205: name = "Maximul lens aperture"; break;
					case 0x9206: name = "Subject distance"; break;
					case 0x9207: name = "Metering mode"; break;
					case 0x9208: name = "Light source"; break;
					case 0x9209: name = "Flash"; break;
					case 0x920A: name = "Lens focal length"; break;
					case 0x9214: name = "Subject area"; break;
					case 0x927C: name = "Manufacturer note"; break;
					case 0x9286: name = "User comments"; break;
					case 0x9290: name = "Date time subseconds"; break;
					case 0x9291: name = "Date time original subseconds"; break;
					case 0x9292: name = "Date time digitized subseconds"; break;
					case 0xA000: name = "Supported Flashpix version"; break;
					case 0xA001: name = "Color space information"; break;
					case 0xA002: name = "Valid image width"; break;
					case 0xA003: name = "Valid image height"; break;
					case 0xA004: name = "Related audio file"; break;
					case 0xA005: name = "Interoperatibility IFD pointer"; break;
					case 0xA20B: name = "Flash energy"; break;
					case 0xA20C: name = "Spatial frequency response"; break;
					case 0xA20E: name = "Focal plane X resolution"; break;
					case 0xA20F: name = "Focal plane Y resolution"; break;
					case 0xA210: name = "Focal plane resolution unit"; break;
					case 0xA214: name = "Subject location"; break;
					case 0xA215: name = "Exposure index"; break;
					case 0xA217: name = "Sensing method"; break;
					case 0xA300: name = "File source"; break;
					case 0xA301: name = "Scene type"; break;
					case 0xA302: name = "CFA pattern"; break;
					case 0xA401: name = "Custom image processing"; break;
					case 0xA402: name = "Exposure mode"; break;
					case 0xA403: name = "White balance"; break;
					case 0xA404: name = "Digital zoom ratio"; break;
					case 0xA405: name = "Focal length in 35mm film"; break;
					case 0xA406: name = "Scene capture type"; break;
					case 0xA407: name = "Gain control"; break;
					case 0xA408: name = "Contrast"; break;
					case 0xA409: name = "Saturation"; break;
					case 0xA40A: name = "Sharpness"; break;
					case 0xA40B: name = "Device settings description"; break;
					case 0xA40C: name = "Subject distance range"; break;
					case 0xA420: name = "Unique image ID"; break;
					case 0xA430: name = "Camera Owner Name"; break;
					case 0xA431: name = "Body Serial Number"; break;
					case 0xA432: name = "Lens Specification"; break;
					case 0xA433: name = "Lens Make"; break;
					case 0xA434: name = "Lens Model"; break;
					case 0xA435: name = "Lens Serial Number"; break;
					case 0xA500: name = "Gamma"; break;
				}
				#endregion
			}
			#endregion

			return name;
		}

		#endregion
	}
}
