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
	/// 画像オブジェクトフィルタクラス
	/// </summary>
	public struct CxImageFilter
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="dst">出力先</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		public CxImageFilter(IxModule dst, IxModule mask = null)
		{
			Dst = dst;
			Mask = mask;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 出力先
		/// </summary>
		public IxModule Dst;

		/// <summary>
		/// マスク
		/// </summary>
		public IxModule Mask;

		#endregion

		#region メソッド: (Basic)

		/// <summary>
		/// 要素のコピー (キャスト)
		/// </summary>
		/// <param name="src">入力</param>
		/// <exception cref="T:XIE.CxException"/>
		public void Cast(IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Cast(hdst, hmask, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 要素のコピー
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <exception cref="T:XIE.CxException"/>
		public void Copy(IxModule src, double scale = 0)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Copy(hdst, hmask, hsrc, scale);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 要素のコピー (フィールド指定)
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <exception cref="T:XIE.CxException"/>
		public void CopyEx(IxModule src, int index, int count)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_CopyEx(hdst, hmask, hsrc, index, count);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// RGB/BGR の相互変換
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <exception cref="T:XIE.CxException"/>
		public void RgbToBgr(IxModule src, double scale = 0)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_RgbToBgr(hdst, hmask, hsrc, scale);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 要素の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <param name="cmp">比較対象</param>
		/// <param name="error_range">誤差範囲</param>
		/// <remarks>
		///		各要素の比較結果を出力先に格納します。
		///		値が一致する要素は 0、一致しない要素は 1 以上が格納されます。
		/// </remarks>
		/// <exception cref="T:XIE.CxException"/>
		public void Compare(IxModule src, IxModule cmp, double error_range)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hcmp = ((IxModule)cmp).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Compare(hdst, hmask, hsrc, hcmp, error_range);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Filter)

		/// <summary>
		/// カラー行列フィルタ
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="matrix">カラー行列 [3x3, Model=F64(1)]</param>
		public void ColorMatrix(IxModule src, CxMatrix matrix)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			HxModule hmatrix = ((IxModule)matrix).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_ColorMatrix(hdst, hmask, hsrc, hmatrix);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (GeoTrans)

		/// <summary>
		/// アフィン変換
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="matrix">変換行列</param>
		/// <param name="interpolation">濃度補間モード [0:最近傍、1:双方向、2:平均値]</param>
		public void Affine(IxModule src, CxMatrix matrix, int interpolation)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			HxModule hmatrix = ((IxModule)matrix).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Affine(hdst, hmask, hsrc, hmatrix, interpolation);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// ミラー反転
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="mode">モード [0:反転なし、1:X方向、2:Y方向、3:XY方向]</param>
		public void Mirror(IxModule src, int mode)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Mirror(hdst, hmask, hsrc, mode);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="mode">モード [0:0度、-1:-90度、+1:+90度、+2:180度]</param>
		public void Rotate(IxModule src, int mode)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Rotate(hdst, hmask, hsrc, mode);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Transpose(hdst, hmask, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// サイズ変更
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="sx">X方向の倍率 (1.0 が等倍です。)</param>
		/// <param name="sy">Y方向の倍率 (1.0 が等倍です。)</param>
		/// <param name="interpolation">濃度補間モード [0:最近傍、1:双方向、2:平均値]</param>
		public void Scale(IxModule src, double sx, double sy, int interpolation)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Scale(hdst, hmask, hsrc, sx, sy, interpolation);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Mathematic)

		/// <summary>
		/// 絶対値
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="type">種別</param>
		public void Math(IxModule src, ExMath type)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Math(hdst, hmask, hsrc, type);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Not))

		/// <summary>
		/// 濃度値の反転
		/// </summary>
		/// <param name="src">入力</param>
		/// <param name="mask">マスク</param>
		public void Not(IxModule src, IxModule mask = null)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Not(hdst, hmask, hsrc);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Add);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Add);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Mul);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Mul);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Sub);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Sub);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.SubInv);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Div);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Div);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.DivInv);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Mod);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 剰余 (pixel % value)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Mod(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Mod);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 剰余 (value % pixel)
		/// </summary>
		/// <param name="value">左辺値</param>
		/// <param name="src">右辺値</param>
		public void Mod(double value, IxModule src)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.ModInv);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Pow);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Pow);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.PowInv);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Atan2);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Atan2);
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
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Atan2Inv);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Diff))

		/// <summary>
		/// 差分
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Diff(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Diff);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 差分
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Diff(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Diff);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Max))

		/// <summary>
		/// 比較演算 (最大値)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Max(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Max);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 比較演算 (最大値)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Max(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Max);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Min))

		/// <summary>
		/// 比較演算 (最小値)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Min(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, ExOpe2A.Min);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 比較演算 (最小値)
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Min(IxModule src, double value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, ExOpe1A.Min);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (And))

		/// <summary>
		/// 論理積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void And(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, ExOpe2L.And);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 論理積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void And(IxModule src, uint value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, ExOpe1L.And);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Nand))

		/// <summary>
		/// 否定論理積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Nand(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, ExOpe2L.Nand);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 否定論理積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Nand(IxModule src, uint value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, ExOpe1L.Nand);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Or))

		/// <summary>
		/// 論理和
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Or(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, ExOpe2L.Or);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 論理和
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Or(IxModule src, uint value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, ExOpe1L.Or);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Operation (Xor))

		/// <summary>
		/// 排他的論理和
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Xor(IxModule src, IxModule value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hval = ((IxModule)value).GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, ExOpe2L.Xor);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 排他的論理和
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="value">右辺値</param>
		public void Xor(IxModule src, uint value)
		{
			HxModule hdst = Dst.GetHandle();
			HxModule hsrc = src.GetHandle();
			HxModule hmask = (Mask == null) ? HxModule.Zero : Mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, ExOpe1L.Xor);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
