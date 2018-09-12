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
	/// OpenCV 行列オブジェクト構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxCvMat :
		IEquatable<TxCvMat>
	{
		#region プロパティ:

		/// <summary>
		/// 
		/// </summary>
		public int type;

		/// <summary>
		/// 
		/// </summary>
		public int step;

		/// <summary>
		/// 
		/// </summary>
		IntPtr refcount;

		/// <summary>
		/// 
		/// </summary>
		int hdr_refcount;

		/// <summary>
		/// 
		/// </summary>
		public IntPtr ptr;

		/// <summary>
		/// 
		/// </summary>
		public int rows;

		/// <summary>
		/// 
		/// </summary>
		public int cols;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr"></param>
		/// <param name="_rows"></param>
		/// <param name="_cols"></param>
		/// <param name="_type"></param>
		/// <param name="_step"></param>
		/// <param name="_refcount"></param>
		/// <param name="_hdr_refcount"></param>
		public TxCvMat(IntPtr addr, int _rows, int _cols, int _type, int _step, IntPtr _refcount, int _hdr_refcount)
		{
			ptr = addr;
			rows = _rows;
			cols = _cols;
			type = _type;
			step = _step;
			refcount = _refcount;
			hdr_refcount = _hdr_refcount;
		}

		#endregion

		#region コンストラクタ: (TxIplImage ← TxImage)

		/// <summary>
		/// コンストラクタ (TxCvMat ← TxImage)
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ch">チャネル指標 [0~(src.Channels-1)]</param>
		public TxCvMat(TxImage src, int ch)
		{
			// ---------------------------------------------
			int dst_depth = 0;
			int dst_channels = src.Model.Pack;
			switch (src.Model.Type)
			{
				default:
					throw new CxException(ExStatus.Unsupported);
				case ExType.U8: dst_depth = CVDefs.CV_8U; break;
				case ExType.U16: dst_depth = CVDefs.CV_16U; break;
				case ExType.S8: dst_depth = CVDefs.CV_8S; break;
				case ExType.S16: dst_depth = CVDefs.CV_16S; break;
				case ExType.S32: dst_depth = CVDefs.CV_32S; break;
				case ExType.F32: dst_depth = CVDefs.CV_32F; break;
				case ExType.F64: dst_depth = CVDefs.CV_64F; break;
			}

			type = CVDefs.CV_MAKETYPE(dst_depth, dst_channels);
			step = src.Stride;
			refcount = IntPtr.Zero;
			hdr_refcount = 0;
			ptr = src.Layer[ch];
			rows = src.Height;
			cols = src.Width;
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
		public bool Equals(TxCvMat other)
		{
			if (this.ptr != other.ptr) return false;
			if (this.rows != other.rows) return false;
			if (this.cols != other.cols) return false;
			if (this.type != other.type) return false;
			if (this.step != other.step) return false;
			if (this.refcount != other.refcount) return false;
			if (this.hdr_refcount != other.hdr_refcount) return false;
			return true;
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
			if (!(obj is TxCvMat)) return false;
			return this.Equals((TxCvMat)obj);
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
		public static bool operator ==(TxCvMat left, TxCvMat right)
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
		public static bool operator !=(TxCvMat left, TxCvMat right)
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
		public static implicit operator TxImage(TxCvMat src)
		{
			TxImage dst = new TxImage();

			TxModel dst_model = TxModel.Default;
			int src_type	= CVDefs.CV_MAT_DEPTH(src.type);
			int src_pack	= CVDefs.CV_MAT_CN(src.type);
			switch(src_type)
			{
			case CVDefs.CV_8U	: dst_model = TxModel.U8(src_pack); break;
			case CVDefs.CV_16U	: dst_model = TxModel.U16(src_pack); break;
			case CVDefs.CV_32F	: dst_model = TxModel.F32(src_pack); break;
			case CVDefs.CV_64F	: dst_model = TxModel.F64(src_pack); break;
			case CVDefs.CV_8S	: dst_model = TxModel.S8(src_pack); break;
			case CVDefs.CV_16S	: dst_model = TxModel.S16(src_pack); break;
			case CVDefs.CV_32S	: dst_model = TxModel.S32(src_pack); break;
			default:
				break;
			}

			dst.Layer[0]	= src.ptr;
			dst.Width		= src.cols;
			dst.Height		= src.rows;
			dst.Model		= dst_model;
			dst.Channels	= 1;
			dst.Stride		= src.step;
			dst.Depth		= 0;

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
		public static explicit operator CxImage(TxCvMat src)
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
			get { return (ExBoolean.True == xie_core.fnXIE_Core_CvMat_IsValid(this)); }
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
				ExStatus status = xie_core.fnXIE_Core_CvMat_CopyTo(this, hdst);
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
				ExStatus status = xie_core.fnXIE_Core_CvMat_CopyFrom(this, hsrc);
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
