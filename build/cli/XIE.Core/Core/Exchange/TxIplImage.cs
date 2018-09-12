/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XIE
{
	/// <summary>
	/// IPL 画像オブジェクト構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxIplImage :
		IEquatable<TxIplImage>
	{
		#region プロパティ:

		/// <summary>
		/// 構造体のサイズ (sizeof(IplImage))
		/// </summary>
		public int nSize;

		/// <summary>
		/// バージョン (常に 0 です。)
		/// </summary>
		public int ID;

		/// <summary>
		/// チャネル数 [1,3,4]
		/// </summary>
		public int nChannels;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public int alphaChannel;

		/// <summary>
		/// ピクセルサイズ [IPL_DEPTH]
		/// </summary>
		public int depth;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte colorModel0;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte colorModel1;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte colorModel2;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte colorModel3;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte channelSeq0;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte channelSeq1;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte channelSeq2;

		/// <summary>
		/// (Ignored) 
		/// </summary>
		public byte channelSeq3;

		/// <summary>
		/// IPL_DATA_ORDER [0=Pixel / 1=Plane] (常に 0 です。)
		/// </summary>
		public int dataOrder;

		/// <summary>
		/// IPL_ORIGIN [0=TL / 1=BL] (常に 0 です。)
		/// </summary>
		public int origin;

		/// <summary>
		/// (Ignored) 水平方向のパッキングサイズ(bytes) [4/8] (常に 4 です。)
		/// </summary>
		public int align;

		/// <summary>
		/// 画像の幅 (pixels)
		/// </summary>
		public int width;

		/// <summary>
		/// 画像の高さ (pixels)
		/// </summary>
		public int height;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public IntPtr roi;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public IntPtr maskROI;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public IntPtr imageId;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public IntPtr tileInfo;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int imageSize;

		/// <summary>
		/// 画像データ領域の先頭アドレス
		/// </summary>
		public IntPtr imageData;

		/// <summary>
		/// 画像データ領域の水平方向サイズ (bytes)
		/// </summary>
		public int widthStep;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderMode0;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderMode1;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderMode2;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderMode3;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderConst0;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderConst1;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderConst2;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public int BorderConst3;

		/// <summary>
		/// (Ignored)
		/// </summary>
		public IntPtr imageDataOrigin;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">画像データ領域の先頭アドレス</param>
		/// <param name="_width">画像の幅 (pixels)</param>
		/// <param name="_height">画像の高さ (pixels)</param>
		/// <param name="_depth">ピクセルサイズ [IPL_DEPTH]</param>
		/// <param name="channels">チャネル数 [1,3,4]</param>
		/// <param name="step">画像データ領域の水平方向サイズ (bytes)</param>
		public TxIplImage(IntPtr addr, int _width, int _height, int _depth, int channels, int step)
		{
			nSize = Marshal.SizeOf(typeof(TxIplImage));
			ID = 0;
			// ---
			imageData = addr;
			width = _width;
			height = _height;
			depth = _depth;
			nChannels = channels;
			widthStep = step;
			// ---
			dataOrder = IPLDefs.IPL_DATA_ORDER_PIXEL;
			origin = IPLDefs.IPL_ORIGIN_TL;
			align = XIE.Defs.XIE_IMAGE_PACKING_SIZE;
			// ---
			alphaChannel = 0;				// [-] Ignored.
			colorModel0 = 0;				// [-] Ignored.
			colorModel1 = 0;				// [-] Ignored.
			colorModel2 = 0;				// [-] Ignored.
			colorModel3 = 0;				// [-] Ignored.
			channelSeq0 = 0;				// [-] Ignored.
			channelSeq1 = 0;				// [-] Ignored.
			channelSeq2 = 0;				// [-] Ignored.
			channelSeq3 = 0;				// [-] Ignored.
			roi = IntPtr.Zero;				// [-] Ignored.
			maskROI = IntPtr.Zero;			// [-] Ignored.
			imageId = IntPtr.Zero;			// [-] Ignored.
			tileInfo = IntPtr.Zero;			// [-] Ignored.
			imageSize = 0;					// [-] Ignored.
			BorderMode0 = 0;				// [-] Ignored.
			BorderMode1 = 0;				// [-] Ignored.
			BorderMode2 = 0;				// [-] Ignored.
			BorderMode3 = 0;				// [-] Ignored.
			BorderConst0 = 0;				// [-] Ignored.
			BorderConst1 = 0;				// [-] Ignored.
			BorderConst2 = 0;				// [-] Ignored.
			BorderConst3 = 0;				// [-] Ignored.
			imageDataOrigin = IntPtr.Zero;	// [-] Ignored.
		}

		#endregion

		#region コンストラクタ: (TxIplImage ← TxImage)

		/// <summary>
		/// コンストラクタ (TxIplImage ← TxImage)
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ch">チャネル指標 [0~(src.Channels-1)]</param>
		public unsafe TxIplImage(TxImage src, int ch)
		{
			nSize = Marshal.SizeOf(typeof(TxIplImage));
			ID = 0;
			// ---
			dataOrder = IPLDefs.IPL_DATA_ORDER_PIXEL;
			origin = IPLDefs.IPL_ORIGIN_TL;
			align = XIE.Defs.XIE_IMAGE_PACKING_SIZE;
			// ---
			alphaChannel = 0;				// [-] Ignored.
			colorModel0 = 0;				// [-] Ignored.
			colorModel1 = 0;				// [-] Ignored.
			colorModel2 = 0;				// [-] Ignored.
			colorModel3 = 0;				// [-] Ignored.
			channelSeq0 = 0;				// [-] Ignored.
			channelSeq1 = 0;				// [-] Ignored.
			channelSeq2 = 0;				// [-] Ignored.
			channelSeq3 = 0;				// [-] Ignored.
			roi = IntPtr.Zero;				// [-] Ignored.
			maskROI = IntPtr.Zero;			// [-] Ignored.
			imageId = IntPtr.Zero;			// [-] Ignored.
			tileInfo = IntPtr.Zero;			// [-] Ignored.
			imageSize = 0;					// [-] Ignored.
			BorderMode0 = 0;				// [-] Ignored.
			BorderMode1 = 0;				// [-] Ignored.
			BorderMode2 = 0;				// [-] Ignored.
			BorderMode3 = 0;				// [-] Ignored.
			BorderConst0 = 0;				// [-] Ignored.
			BorderConst1 = 0;				// [-] Ignored.
			BorderConst2 = 0;				// [-] Ignored.
			BorderConst3 = 0;				// [-] Ignored.
			imageDataOrigin = IntPtr.Zero;	// [-] Ignored.

			// ---------------------------------------------
			int dst_depth = 0;
			int dst_channels = src.Model.Pack;
			switch (src.Model.Type)
			{
				case ExType.U8: dst_depth = IPLDefs.IPL_DEPTH_8U; break;
				case ExType.U16: dst_depth = IPLDefs.IPL_DEPTH_16U; break;
				case ExType.F32: dst_depth = IPLDefs.IPL_DEPTH_32F; break;
				case ExType.F64: dst_depth = IPLDefs.IPL_DEPTH_64F; break;
				case ExType.S8: dst_depth = IPLDefs.IPL_DEPTH_8S; break;
				case ExType.S16: dst_depth = IPLDefs.IPL_DEPTH_16S; break;
				case ExType.S32: dst_depth = IPLDefs.IPL_DEPTH_32S; break;
				default:
					throw new CxException(ExStatus.Unsupported);
			}

			imageData = src.Layer[ch];
			width = src.Width;
			height = src.Height;
			depth = dst_depth;
			nChannels = dst_channels;
			widthStep = src.Stride;
		}

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxIplImage other)
		{
			return (ExBoolean.True == xie_core.fnXIE_Core_IplImage_Equals(this, other));
		}

		/// <summary>
		/// 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="obj">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (!(obj is TxIplImage)) return false;
			return this.Equals((TxIplImage)obj);
		}

		/// <summary>
		/// ハッシュ値の取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		/// </returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region 比較オペレータ:

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator ==(TxIplImage left, TxIplImage right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は false、それ以外は true を返します。
		/// </returns>
		public static bool operator !=(TxIplImage left, TxIplImage right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (TxImage ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxImage ← 自身)
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxImage(TxIplImage src)
		{
			TxImage dst = new TxImage();

			TxModel dst_model;
			switch (src.depth)
			{
				case IPLDefs.IPL_DEPTH_8U: dst_model = TxModel.U8(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_16U: dst_model = TxModel.U16(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_32F: dst_model = TxModel.F32(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_64F: dst_model = TxModel.F64(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_8S: dst_model = TxModel.S8(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_16S: dst_model = TxModel.S16(src.nChannels); break;
				case IPLDefs.IPL_DEPTH_32S: dst_model = TxModel.S32(src.nChannels); break;
				default: dst_model = TxModel.Default; break;
			}

			dst.Layer.Address0	= src.imageData;
			dst.Width = src.width;
			dst.Height = src.height;
			dst.Model = dst_model;
			dst.Channels = 1;
			dst.Stride = src.widthStep;
			dst.Depth = 0;

			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (CxImage ← 自身)

		/// <summary>
		/// CxImage への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxImage(TxIplImage src)
		{
			var dst = new CxImage();
			src.CopyTo(dst);
			return dst;
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 構造体に設定された情報が有効か否かを検査します。
		/// </summary>
		/// <returns>
		///		有効な場合は true を、それ以外は false を返します。
		/// </returns>
		public bool IsValid
		{
			get { return (ExBoolean.True == xie_core.fnXIE_Core_IplImage_IsValid(this)); }
		}

		/// <summary>
		/// 画像データを FIE 仕様から XIE 仕様へ変換します。
		/// </summary>
		/// <param name="dst">複製先</param>
		public void CopyTo(object dst)
		{
			if (dst is IxModule)
			{
				var hdst = ((IxModule)dst).GetHandle();
				ExStatus status = xie_core.fnXIE_Core_IplImage_CopyTo(this, hdst);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// 画像データを XIE 仕様から FIE 仕様へ変換します。
		/// </summary>
		/// <param name="src">複製元</param>
		public void CopyFrom(object src)
		{
			if (src is IxModule)
			{
				var hsrc = ((IxModule)src).GetHandle();
				ExStatus status = xie_core.fnXIE_Core_IplImage_CopyFrom(this, hsrc);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}
			if (src is IxConvertible)
			{
				((IxConvertible)src).CopyTo(this);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		#endregion
	}
}
