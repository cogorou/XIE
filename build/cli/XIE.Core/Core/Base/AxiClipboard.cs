/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using XIE;
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// クリップボード補助ツール
	/// </summary>
	public static unsafe class AxiClipboard
	{
		/// <summary>
		/// 指定された画像オブジェクトをクリップボードへコピーします。
		/// </summary>
		/// <param name="src">元画像</param>
		public static unsafe void CopyFrom(CxImage src)
		{
			if (src.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			#region 互換性の検査:
			bool is_compatible = false;
			if (src.Model.Type == ExType.U8)
			{
				switch (src.Model.Pack)
				{
					default:
						break;
					case 1:
						switch (src.Channels)
						{
							default:
								break;
							case 1:
							case 3:
							case 4:
								is_compatible = true;
								break;
						}
						break;
					case 3:
					case 4:
						switch (src.Channels)
						{
							default:
								break;
							case 1:
								is_compatible = true;
								break;
						}
						break;
				}
			}
			#endregion

			#region クリップボードへのコピー:
			if (is_compatible)
			{
#if true
				using (var dib = AllocateDib(src))
				{
					Clipboard.SetData(DataFormats.Dib, dib);
				}
#else
				switch (System.Environment.OSVersion.Platform)
				{
					case PlatformID.Unix:
						// Linux では Bitmap を使用する.
						{
							Clipboard.SetData(DataFormats.Bitmap, src);
						}
						break;
					default:
						// Windows では Dib を使用する.
						using (var dib = AllocateDib(src))
						{
							Clipboard.SetData(DataFormats.Dib, dib);
						}
						break;
				}
#endif
			}
			else
			{
				Clipboard.SetData(typeof(XIE.CxImage).FullName, src);
			}
			#endregion
		}

		/// <summary>
		/// 指定された画像オブジェクトの DIB を生成します。
		/// </summary>
		/// <param name="src">元画像</param>
		/// <returns>
		///		生成した DIB を返します。
		/// </returns>
		private static MemoryStream AllocateDib(CxImage src)
		{
			IntPtr pvdib = IntPtr.Zero;
			uint bmpInfoSize = 0;
			uint imageSize = 0;

			try
			{
				ExStatus status = xie_core.fnXIE_Core_DIB_Save(((IxModule)src).GetHandle(), ref pvdib, ref bmpInfoSize, ref imageSize);
				if (status != ExStatus.Success)
					throw new CxException(status);

				var dib_size = bmpInfoSize + imageSize;
				var dib_array = new byte[dib_size];
				Marshal.Copy(pvdib, dib_array, 0, (int)dib_size);

				return new MemoryStream(dib_array);
			}
			finally
			{
				if (pvdib != IntPtr.Zero)
					Axi.MemoryFree(pvdib);
				pvdib = IntPtr.Zero;
			}
		}

		/// <summary>
		/// クリップボードから画像データを取得します。
		/// </summary>
		/// <returns>
		///		画像データをコピーした画像オブジェクトを返します。
		///		データがなければ null を返します。
		/// </returns>
		public static unsafe CxImage ToImage()
		{
			var idata = Clipboard.GetDataObject();
			if (idata == null)
				return null;

			#region クリップボードからの取得:
			if (idata.GetDataPresent(DataFormats.Dib))
			{
				using (var stream = (MemoryStream)idata.GetData(DataFormats.Dib))
				{
					var dib_array = stream.ToArray();
					var dib_size = dib_array.Length;
					var dib_addr = Axi.MemoryAlloc(dib_size);
					if (dib_addr == IntPtr.Zero)
						throw new CxException(ExStatus.MemoryError);
					var dst = new CxImage();

					try
					{
						Marshal.Copy(dib_array, 0, dib_addr, dib_size);
						ExStatus status = xie_core.fnXIE_Core_DIB_Load(((IxModule)dst).GetHandle(), dib_addr, ExBoolean.False);
						if (status != ExStatus.Success)
							throw new CxException(status);
					}
					finally
					{
						if (dib_addr != IntPtr.Zero)
							Axi.MemoryFree(dib_addr);
						dib_addr = IntPtr.Zero;
					}
					return dst;
				}
			}
			if (idata.GetDataPresent(DataFormats.Bitmap))
			{
				using (var bitmap = (Bitmap)idata.GetData(DataFormats.Bitmap))
				{
					var dst = (XIE.CxImage)bitmap;
					return dst;
				}
			}
			if (idata.GetDataPresent(typeof(XIE.CxImage).FullName))
			{
				var dst = (XIE.CxImage)idata.GetData(typeof(XIE.CxImage).FullName);
				return dst;
			}
			#endregion

			return null;
		}
	}
}
