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
using XIE;

namespace XIEstudio
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
				if (port.ReferenceTask is XIE.Tasks.IxTaskOutputImage)
				{
					var task = (XIE.Tasks.IxTaskOutputImage)port.ReferenceTask;
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
		/// 指定されたファイル名称から既知の拡張子を除去します。(画像関連)
		/// </summary>
		/// <param name="filename">元のファイル名</param>
		/// <returns>
		///		拡張子を除去したファイル名を返します。
		/// </returns>
		public static string GetFileNameWithoutExtensionForImage(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename))
				return filename;
			if (filename.EndsWith(".bmp", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".dib", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".tif", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".tiff", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".raw", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".asf", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
				filename.EndsWith(".wmv", StringComparison.CurrentCultureIgnoreCase)
				)
			{
				return System.IO.Path.GetFileNameWithoutExtension(filename);
			}
			return System.IO.Path.GetFileName(filename);
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
		public static CodeExpression CodeOptionalExpression(XIE.Tasks.CxGenerateCodeArgs e, XIE.Tasks.CxTaskPortIn port, CodeExpression value)
		{
			if (port.IsConnected)
				return new CodeExtraVariable(e.GetVariable(port));
			else
				return value;
		}

		#endregion

		#region コンパイラ関連:

		/// <summary>
		/// 参照設定の生成
		/// </summary>
		/// <returns>
		///		参照設定を初期化して返します。
		/// </returns>
		public static XIE.Tasks.CxReferencedAssembly[] CreateReferences()
		{
			var result = new List<XIE.Tasks.CxReferencedAssembly>();
			var assemblies = new List<Assembly>();
			assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
			assemblies.Sort((item1, item2) => { return item1.FullName.CompareTo(item2.FullName); });
			foreach (var asm in assemblies)
			{
				var name = asm.FullName.Split(',')[0];
				switch (name)
				{
					case "System":
					case "System.Core":
					case "System.Drawing":
					case "System.Windows.Forms":
					case "System.Xml":
					case "System.Xml.Linq":
					case "XIE.Core":
					case "XIE.Tasks":
						result.Add(new XIE.Tasks.CxReferencedAssembly(asm.FullName, ""));
						break;
				}
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
