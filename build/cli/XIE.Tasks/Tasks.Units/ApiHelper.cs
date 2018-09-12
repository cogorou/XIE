/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Diagnostics;
using System.CodeDom;

namespace XIE.Tasks
{
	/// <summary>
	/// 補助関数
	/// </summary>
	static class ApiHelper
	{
		#region OutputImage:

		/// <summary>
		/// 画像ビューに画像を表示します。
		/// </summary>
		/// <param name="view">表示先の画像ビュー</param>
		/// <param name="dst">表示用画像オブジェクト</param>
		/// <param name="data">表示対象データ</param>
		/// <param name="depth">既定のビット深度</param>
		public static void OutputImage(XIE.GDI.CxImageView view, CxImage dst, object data, int depth = 8)
		{
			if (view == null) return;
			if (dst == null) return;

			if (data is CxImage)
			{
				#region CxImage を複製して表示する.
				try
				{
					var src = (CxImage)data;
					if (src.IsValid == false)
					{
						dst.Dispose();
						view.Image = null;
						return;
					}

					dst.CopyFrom(src);
					dst.Depth = dst.CalcDepth(-1);
					if (1 < dst.Depth && dst.Depth < depth)
						dst.Depth = depth;
					view.Image = dst;
				}
				catch (Exception)
				{
					dst.Dispose();
					view.Image = null;
				}
				return;
				#endregion
			}
			else
			{
				dst.Dispose();
				view.Image = null;
				return;
			}
		}

		/// <summary>
		/// 依存関係を辿り、画像ビューに画像を表示します。
		/// </summary>
		/// <param name="port">対象の入力ポート</param>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		/// <returns>
		///		画像ビューへの表示を実行すると true を返します。それ以外は false を返します。
		/// </returns>
		public static bool OutputImageRecursive(XIE.Tasks.CxTaskPortIn port, XIE.GDI.CxImageView view, bool update)
		{
			if (port.IsConnected)
			{
				if (port.ReferenceTask is IxTaskOutputImage)
				{
					var task = (IxTaskOutputImage)port.ReferenceTask;
					task.OutputImage(view, update);
					return true;
				}
				else
				{
					foreach (var item in port.ReferenceTask.DataIn)
					{
						if (OutputImageRecursive(item, view, update))
							return true;
					}
					foreach (var item in port.ReferenceTask.DataParam)
					{
						if (OutputImageRecursive(item, view, update))
							return true;
					}
				}
			}
			return false;
		}

		#endregion

		#region スクリーンサイズ.

		/// <summary>
		/// スクリーンサイズ(最大値)を取得します。
		/// </summary>
		/// <returns>
		///		スクリーンサイズを返します。
		/// </returns>
		public static System.Drawing.Size GetScreenSize()
		{
			System.Drawing.Size max_size = Screen.PrimaryScreen.Bounds.Size;
			foreach (Screen item in Screen.AllScreens)
			{
				if (max_size.Width < item.Bounds.Size.Width)
					max_size.Width = item.Bounds.Size.Width;
				if (max_size.Height < item.Bounds.Size.Height)
					max_size.Height = item.Bounds.Size.Height;
			}
			return max_size;
		}

		#endregion

		#region マウス付近の座標.

		/// <summary>
		/// ウィンドウがスクリーンに収まるように考慮したマウス付近の座標を計算します。
		/// </summary>
		/// <param name="size">ウィンドウのサイズ</param>
		/// <param name="mode">モード [0:TopLeft, 1:Center]</param>
		/// <returns>
		///		計算した結果を返します。
		/// </returns>
		public static System.Drawing.Point GetNeighborPosition(System.Drawing.Size size, int mode = 0)
		{
			return GetNeighborPosition(size.Width, size.Height, mode);
		}

		/// <summary>
		/// ウィンドウがスクリーンに収まるように考慮したマウス付近の座標を計算します。
		/// </summary>
		/// <param name="width">ウィンドウ幅</param>
		/// <param name="height">ウィンドウ高さ</param>
		/// <param name="mode">モード [0:TopLeft, 1:Center]</param>
		/// <returns>
		///		計算した結果を返します。
		/// </returns>
		public static System.Drawing.Point GetNeighborPosition(int width, int height, int mode = 0)
		{
			var pos = Control.MousePosition;
			var screen = Screen.FromPoint(Control.MousePosition).Bounds;
			switch (mode)
			{
				default:
				case 0:
					break;
				case 1:
					pos.X -= (width / 2);
					pos.Y -= (height / 2);
					break;
			}
			if (pos.X > (screen.Right - 72) - width)
				pos.X = (screen.Right - 72) - width;
			if (pos.Y > (screen.Bottom - 72) - height)
				pos.Y = (screen.Bottom - 72) - height;
			if (pos.X < screen.Left)
				pos.X = screen.Left;
			if (pos.Y < screen.Top)
				pos.Y = screen.Top;
			return pos;
		}

		#endregion

		#region ファイル名関連:

		/// <summary>
		/// クラス名のサフィックス(年月日_時分秒)の作成
		/// </summary>
		/// <param name="timestamp">ファイル名のサフィックスにするタイムスタンプ</param>
		/// <param name="use_msec">ミリ秒も付加するか否か</param>
		/// <returns>
		///		生成したサフィックスを返します。
		/// </returns>
		public static string MakeClassNameSuffix(DateTime timestamp, bool use_msec)
		{
			string suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second);
			if (use_msec)
				suffix += "_" + timestamp.Millisecond.ToString("000");
			return suffix;
		}

		/// <summary>
		/// ファイル名のサフィックス(年月日_時分秒)の作成
		/// </summary>
		/// <param name="timestamp">ファイル名のサフィックスにするタイムスタンプ</param>
		/// <param name="use_msec">ミリ秒も付加するか否か</param>
		/// <returns>
		///		生成したサフィックスを返します。
		/// </returns>
		public static string MakeFileNameSuffix(DateTime timestamp, bool use_msec)
		{
			string suffix = string.Format("{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second);
			if (use_msec)
				suffix += "-" + timestamp.Millisecond.ToString("000");
			return suffix;
		}

		/// <summary>
		/// 指定されたファイル名に無効な文字があれば指定文字に置き換えます。
		/// </summary>
		/// <param name="filename">検査対象のファイル名</param>
		/// <param name="substitute">代用する文字</param>
		/// <returns>
		///		検査後のファイル名を返します。
		/// </returns>
		public static string MakeValidFileName(string filename, string substitute)
		{
			char[] invalids = System.IO.Path.GetInvalidFileNameChars();
			foreach (char item in invalids)
			{
				string invalid = string.Format("{0}", item);
				if (filename.Contains(invalid))
					filename = filename.Replace(invalid, substitute);
			}
			return filename;
		}

		#endregion

		#region パラメータポートの参照表現の生成:

		/// <summary>
		/// パラメータポートの参照表現の生成
		/// </summary>
		/// <param name="e">引数</param>
		/// <param name="port">パラメータポート</param>
		/// <param name="value">リテラル表現</param>
		/// <returns>
		///		指定された port の IsConnected が true の時は参照表現を返します。
		///		それ以外は指定されたリテラル表現を返します。
		/// </returns>
		public static CodeExpression CodeOptionalExpression(CxGenerateCodeArgs e, CxTaskPortIn port, CodeExpression value)
		{
			if (port.IsConnected)
				return new CodeExtraVariable(e.GetVariable(port));
			else
				return value;
		}

		#endregion

		#region Exif 関連:

		/// <summary>
		/// 指定された画像オブジェクト間で Exif を複製します。
		/// </summary>
		/// <param name="dst">出力側</param>
		/// <param name="src">入力側</param>
		public static void CopyExif(XIE.CxImage dst, XIE.CxImage src)
		{
			using (var src_exif = XIE.CxExif.FromTag(src.Exif()))
			using (var dst_exif = src_exif.GetPurgedExif())
			{
				dst.Exif(dst_exif.Tag());
			}
		}

		/// <summary>
		/// 指定された画像オブジェクトの Exif を更新します。
		/// </summary>
		/// <param name="src">画像オブジェクト</param>
		public static void UpdateExif(XIE.CxImage src)
		{
			using (var exif = XIE.CxExif.FromTag(src.Exif()))
			{
				if (exif.IsValid)
				{
					var size = src.Size;
					var items = exif.GetItems();

					foreach (var item in items)
					{
						switch (item.ID)
						{
							case 0x0132:	// File change date and time
								if (item.Type == 2 && item.Count >= 19)
								{
									var now = DateTime.Now;
									var timestamp = string.Format(
										"{0:0000}:{1:00}:{2:00} {3:00}:{4:00}:{5:00}",
										now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
									var value = new XIE.CxStringA(timestamp);
									exif.SetValue(item, value);
								}
								break;
							case 0xA002:	// Valid image width
								if (item.Type == 3 && item.Count == 1)
								{
									var value = new XIE.CxArray(1, XIE.TxModel.U16(1));
									var ptr = (XIE.Ptr.UInt16Ptr)value.Address();
									ptr[0] = (ushort)size.Width;
									exif.SetValue(item, value);
								}
								else if (item.Type == 4 && item.Count == 1)
								{
									var value = new XIE.CxArray(1, XIE.TxModel.U32(1));
									var ptr = (XIE.Ptr.UInt32Ptr)value.Address();
									ptr[0] = (uint)size.Width;
									exif.SetValue(item, value);
								}
								break;
							case 0xA003:	// Valid image height
								if (item.Type == 3 && item.Count == 1)
								{
									var value = new XIE.CxArray(1, XIE.TxModel.U16(1));
									var ptr = (XIE.Ptr.UInt16Ptr)value.Address();
									ptr[0] = (ushort)size.Height;
									exif.SetValue(item, value);
								}
								else if (item.Type == 4 && item.Count == 1)
								{
									var value = new XIE.CxArray(1, XIE.TxModel.U32(1));
									var ptr = (XIE.Ptr.UInt32Ptr)value.Address();
									ptr[0] = (uint)size.Height;
									exif.SetValue(item, value);
								}
								break;
						}
					}
				}
			}
		}

		#endregion

		#region コンパイラ関連:

		/// <summary>
		/// 参照設定の生成
		/// </summary>
		/// <returns>
		///		参照設定を初期化して返します。
		/// </returns>
		public static CxReferencedAssembly[] CreateReferences()
		{
			var result = new List<CxReferencedAssembly>();
			foreach(var item in SharedData.References)
			{
				result.Add((CxReferencedAssembly)item.Value.Clone());
			}
			return result.ToArray();
		}

		/// <summary>
		/// 名前空間設定の生成
		/// </summary>
		/// <returns>
		///		名前空間設定を初期化して返します。
		/// </returns>
		public static string[] CreateImports()
		{
			return new string[]
			{
				"System",
				"System.Collections.Generic",
				"System.ComponentModel",
				"System.Drawing",
				"System.Text",
				"System.Windows.Forms",
				"System.Reflection",
				"System.IO",
				"System.Xml",
				"System.Threading",
				"System.Diagnostics",
			};
		}

		#endregion
	}
}
