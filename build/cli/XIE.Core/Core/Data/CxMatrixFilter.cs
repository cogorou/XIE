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
	/// 行列オブジェクトフィルタクラス
	/// </summary>
	public struct CxMatrixFilter
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="dst">出力先</param>
		public CxMatrixFilter(IxModule dst)
		{
			Dst = dst;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 出力先
		/// </summary>
		public IxModule Dst;

		#endregion

		#region メソッド: (Copy)

		/// <summary>
		/// 要素のコピー (キャスト)
		/// </summary>
		/// <param name="src">入力</param>
		/// <exception cref="T:XIE.CxException"/>
		public void Cast(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Cast(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 要素のコピー
		/// </summary>
		/// <param name="src">入力</param>
		/// <exception cref="T:XIE.CxException"/>
		public void Copy(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Copy(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Segmentation)

		/// <summary>
		/// 要素の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <param name="cmp">比較対象</param>
		/// <param name="error_range">誤差範囲</param>
		/// <remarks>
		///		各要素の比較結果を格納したオブジェクトを返します。
		///		値が一致する要素は 0、一致しない要素は 1 以上が格納されます。
		/// </remarks>
		/// <exception cref="T:XIE.CxException"/>
		public void Compare(IxModule src, IxModule cmp, double error_range)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hcmp = ((IxModule)cmp).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Compare(hdst, hsrc, hcmp, error_range);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (GeoTrans)

		/// <summary>
		/// ミラー反転
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="mode">モード [1:X方向、2:Y方向、3:XY方向]</param>
		public void Mirror(IxModule src, int mode)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Mirror(hdst, hsrc, mode);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="mode">モード [-1:-90度、+1:+90度、+2:180度]</param>
		public void Rotate(IxModule src, int mode)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Rotate(hdst, hsrc, mode);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 転置
		/// </summary>
		/// <param name="src">入力</param>
		public void Transpose(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Transpose(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Linear)

		/// <summary>
		/// 行列の積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="val">右辺値</param>
		public void Product(IxModule src, IxModule val)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)val).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Product(hdst, hsrc, hval);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 逆行列
		/// </summary>
		/// <param name="src">入力</param>
		public void Invert(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Invert(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 小行列
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="row">除去対象の行指標 [0~]</param>
		/// <param name="col">除去対象の列指標 [0~]</param>
		public void Submatrix(IxModule src, int row, int col)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Submatrix(hdst, hsrc, row, col);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Mathematic)

		/// <summary>
		/// 数理関数
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="type">種別</param>
		public void Math(IxModule src, ExMath type)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Math(hdst, hsrc, type);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Not)

		/// <summary>
		/// 符号反転
		/// </summary>
		/// <param name="src">入力</param>
		public void Not(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Not(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Add))

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Add(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Add);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Add(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Add);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Mul))

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Mul(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Mul);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Mul(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Mul);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Sub))

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Sub(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Sub);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 減算 (pixel - value)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Sub(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Sub);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 減算 (value - pixel)
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Sub(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.SubInv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Div))

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Div(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Div);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 除算 (pixel / value)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Div(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Div);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 除算 (value / pixel)
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Div(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.DivInv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Mod))

		/// <summary>
		/// 剰余
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Mod(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Mod);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 剰余 (pixel / value)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Mod(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Mod);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 剰余 (value / pixel)
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Mod(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.ModInv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Pow))

		/// <summary>
		/// 累乗
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Pow(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Pow);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 累乗 (Pow(pixel, value))
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Pow(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Pow);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 累乗 (Pow(value, pixel))
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Pow(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.PowInv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Atan2))

		/// <summary>
		/// 正接
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Atan2(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, ExOpe2A.Atan2);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 正接 (Atan2(pixel, value))
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Atan2(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Atan2);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 正接 (Atan2(value, pixel))
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Atan2(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, ExOpe1A.Atan2Inv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}

	internal static partial class Filter
	{
		#region メソッド: (Linear)

		/// <summary>
		/// 上三角成分の抽出
		/// </summary>
		/// <param name="dst">出力</param>
		/// <param name="src">入力</param>
		public static void Triu(IxModule dst, IxModule src)
		{
			HxModule hsrc = src.GetHandle();
			HxModule hdst = ((IxModule)dst).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Triu(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 下三角成分の抽出
		/// </summary>
		/// <param name="dst">出力</param>
		/// <param name="src">入力</param>
		public static void Tril(IxModule dst, IxModule src)
		{
			HxModule hsrc = src.GetHandle();
			HxModule hdst = ((IxModule)dst).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Tril(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
