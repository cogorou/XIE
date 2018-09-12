/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XIE
{
	#region エラーコード.

	/// <summary>
	/// エラーコード
	/// </summary>
	public enum ExStatus
	{
		/// <summary>
		/// 正常
		/// </summary>
		Success = 0,

		/// <summary>
		/// パラメータが不正です。
		/// </summary>
		InvalidParam,

		/// <summary>
		/// オブジェクトが不正です。
		/// </summary>
		InvalidObject,

		/// <summary>
		/// メモリの確保に失敗しました。
		/// </summary>
		MemoryError,

		/// <summary>
		/// 見つかりません。
		/// </summary>
		NotFound,

		/// <summary>
		/// 処理できない条件です。
		/// </summary>
		Impossible,

		/// <summary>
		/// 処理が中断しました。
		/// </summary>
		Interrupted,

		/// <summary>
		/// アクセスに失敗しました。
		/// </summary>
		IOError,

		/// <summary>
		/// 待機時間を超過しました。
		/// </summary>
		Timeout,

		/// <summary>
		/// サポートしていません。
		/// </summary>
		Unsupported,
	};

	#endregion

	#region 要素の型.

	/// <summary>
	/// 要素の型
	/// </summary>
	public enum ExType
	{
		/// <summary>
		/// 不明
		/// </summary>
		None = 0,

		/// <summary>
		/// ポインタ
		/// </summary>
		Ptr,

		/// <summary>
		/// 整数 (8 bit) (符号なし) System.Byte
		/// </summary>
		U8,

		/// <summary>
		/// 整数 (16 bit) (符号なし) System.UInt16
		/// </summary>
		U16,

		/// <summary>
		/// 整数 (32 bit) (符号なし) System.UInt32
		/// </summary>
		U32,

		/// <summary>
		/// 整数 (64 bit) (符号なし) System.UInt64
		/// </summary>
		U64,

		/// <summary>
		/// 整数 (8 bit) (符号付き) System.SByte
		/// </summary>
		S8,

		/// <summary>
		/// 整数 (16 bit) (符号付き) System.Int16
		/// </summary>
		S16,

		/// <summary>
		/// 整数 (32bit) (符号付き) System.Int32
		/// </summary>
		S32,

		/// <summary>
		/// 整数 (64 bit) (符号付き) System.Int64
		/// </summary>
		S64,

		/// <summary>
		/// 実数 (32 bit) (単精度浮動小数点型) System.Float
		/// </summary>
		F32,

		/// <summary>
		/// 実数 (64 bit) (倍精度浮動小数点型) System.Double
		/// </summary>
		F64,
	};

	#endregion

	#region ブーリアン.

	/// <summary>
	/// ブーリアン
	/// </summary>
	public enum ExBoolean
	{
		/// <summary>
		/// 偽
		/// </summary>
		False = 0,

		/// <summary>
		/// 真
		/// </summary>
		True = 1,
	}

	#endregion

	#region スキャン方向.

	/// <summary>
	/// スキャン方向
	/// </summary>
	public enum ExScanDir
	{
		/// <summary>
		/// X 方向
		/// </summary>
		X = 0,

		/// <summary>
		/// Y 方向
		/// </summary>
		Y = 1,
	}

	#endregion

	#region エンディアンタイプ:

	/// <summary>
	/// エンディアンタイプ
	/// </summary>
	public enum ExEndianType
	{
		/// <summary>
		/// 不明
		/// </summary>
		None = 0,

		/// <summary>
		/// リトルエンディアン
		/// </summary>
		LE = 1,

		/// <summary>
		/// ビッグエンディアン
		/// </summary>
		BE = 2,
	}

	#endregion

	#region 数理関数種別:

	/// <summary>
	/// 数理関数種別
	/// </summary>
	public enum ExMath
	{
		/// <summary>
		/// 絶対値
		/// </summary>
		Abs,

		/// <summary>
		/// 符号化
		/// </summary>
		Sign,

		/// <summary>
		/// 平方根
		/// </summary>
		Sqrt,

		/// <summary>
		/// 指数関数
		/// </summary>
		Exp,

		/// <summary>
		/// 自然対数（底 e の対数）
		/// </summary>
		Log,

		/// <summary>
		/// 常用対数（底 10 の対数）
		/// </summary>
		Log10,

		/// <summary>
		/// 正弦
		/// </summary>
		Sin,

		/// <summary>
		/// 余弦
		/// </summary>
		Cos,

		/// <summary>
		/// 正接
		/// </summary>
		Tan,

		/// <summary>
		/// 双曲線正弦（ハイパボリック サイン）
		/// </summary>
		Sinh,

		/// <summary>
		/// 双曲線余弦（ハイパボリック コサイン）
		/// </summary>
		Cosh,

		/// <summary>
		/// 双曲線正接（ハイパボリック タンジェント）
		/// </summary>
		Tanh,

		/// <summary>
		/// 逆正弦（アーク サイン）
		/// </summary>
		Asin,

		/// <summary>
		/// 逆余弦（アーク コサイン）
		/// </summary>
		Acos,

		/// <summary>
		/// 逆正接（アーク タンジェント）
		/// </summary>
		Atan,

		/// <summary>
		/// 天井関数
		/// </summary>
		Ceiling,

		/// <summary>
		/// 床関数
		/// </summary>
		Floor,

		/// <summary>
		/// 四捨五入
		/// </summary>
		Round,

		/// <summary>
		/// 小数部の切捨て
		/// </summary>
		Truncate,

		/// <summary>
		/// 小数部の抽出
		/// </summary>
		Modf,
	}

	#endregion

	#region internal - 画像演算関連.

	/// <summary>
	/// 算術演算 (pixel : value)
	/// </summary>
	public enum ExOpe1A
	{
		Add,
		Mul,
		Sub,
		SubInv,
		Div,
		DivInv,
		Mod,
		ModInv,
		Pow,
		PowInv,
		Atan2,
		Atan2Inv,
		Diff,
		Min,
		Max,
	}

	/// <summary>
	/// 論理演算 (pixel : value)
	/// </summary>
	public enum ExOpe1L
	{
		And,
		Nand,
		Or,
		Xor,
	}

	/// <summary>
	/// 算術演算 (pixel : pixel)
	/// </summary>
	public enum ExOpe2A
	{
		Add,
		Mul,
		Sub,
		Div,
		Mod,
		Pow,
		Atan2,
		Diff,
		Min,
		Max,
	}

	/// <summary>
	/// 論理演算 (pixel : pixel)
	/// </summary>
	public enum ExOpe2L
	{
		And,
		Nand,
		Or,
		Xor,
	}

	#endregion
}
