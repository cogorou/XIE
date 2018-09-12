/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE.Tasks
{
	#region ExLanguageType

	/// <summary>
	/// 言語種別
	/// </summary>
	public enum ExLanguageType
	{
		/// <summary>
		/// なし
		/// </summary>
		None, 

		/// <summary>
		/// C#
		/// </summary>
		CSharp,

		/// <summary>
		/// Visual Basic
		/// </summary>
		VisualBasic,
	}

	#endregion

	#region ExTaskOrder

	/// <summary>
	/// タスク要求
	/// </summary>
	public enum ExTaskOrder
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,

		/// <summary>
		/// 再描画
		/// </summary>
		Redraw = 1,

		/// <summary>
		/// 実行
		/// </summary>
		Execute = 2,
	}

	#endregion

	#region ExControlSyntax

	/// <summary>
	/// 制御構文種別
	/// </summary>
	public enum ExControlSyntax
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,

		/// <summary>
		/// 反復処理の末尾まで処理をスキップします。
		/// </summary>
		Continue = 1,

		/// <summary>
		/// 反復処理を中断します。
		/// </summary>
		Break = 2,

		/// <summary>
		/// 関数を終了します。
		/// </summary>
		Return = 3,
	}

	#endregion

	#region ExDataStoreType

	/// <summary>
	/// データ格納種別
	/// </summary>
	public enum ExDataStoreType
	{
		/// <summary>
		/// ビットフィールド
		/// </summary>
		BitFields,

		/// <summary>
		/// 数値
		/// </summary>
		Number,
	}

	#endregion

	#region ExTaskUnitPortType

	/// <summary>
	/// 出力ポート種別
	/// </summary>
	public enum ExOutputPortType
	{
		/// <summary>
		/// コントロール出力ポート
		/// </summary>
		ControlOut,

		/// <summary>
		/// データ出力ポート
		/// </summary>
		DataOut,
	}

	#endregion

	#region ExComparisonOperatorType

	/// <summary>
	/// 比較演算子の種類
	/// </summary>
	public enum ExComparisonOperatorType
	{
		/// <summary>
		/// 等価（２つの値の比較演算を行い真偽 [true:等価、false:不等価] を返します。コード例:[C#: left == right]）
		/// </summary>
		Equal,
		/// <summary>
		/// 不等価（２つの値の比較演算を行い真偽 [true:不等価、false:等価] を返します。コード例:[C#: left != right]）
		/// </summary>
		NotEqual,
		/// <summary>
		/// 小なり（２つの値の比較演算を行い真偽 [true:未満、false:それ以外] を返します。式:[left ＜ right]）
		/// </summary>
		LessThan,
		/// <summary>
		/// 小なりまたは等価（２つの値の比較演算を行い真偽 [true:以下、false:それ以外] を返します。式:[left ≦ right]）
		/// </summary>
		LessThanOrEqual,
		/// <summary>
		/// 大なり（２つの値の比較演算を行い真偽 [true:超過、false:それ以外] を返します。式:[left ＞ right]）
		/// </summary>
		GreaterThan,
		/// <summary>
		/// 大なりまたは等価（２つの値の比較演算を行い真偽 [true:以上、false:それ以外] を返します。式:[left ≧ right]）
		/// </summary>
		GreaterThanOrEqual,
	}

	#endregion

	#region ExBinaryOperatorType

	/// <summary>
	/// ２項演算子の種類
	/// </summary>
	public enum ExBinaryOperatorType
	{
		/// <summary>
		/// 加算（２つの値の加算を行います。コード例:[C#: left + right]）
		/// </summary>
		Add,
		/// <summary>
		/// 減算（２つの値の減算を行います。コード例:[C#: left - right]）
		/// </summary>
		Subtract,
		/// <summary>
		/// 乗算（２つの値の乗算を行います。コード例:[C#: left * right]）
		/// </summary>
		Multiply,
		/// <summary>
		/// 除算（２つの値の除算を行います。コード例:[C#: left / right]）
		/// </summary>
		Divide,
		/// <summary>
		/// 剰余（２つの値の剰余を求めます。コード例:[C#: left % right]）
		/// </summary>
		Modulus,
		/// <summary>
		/// 論理積（２つの値の論理積を行います。コード例:[C#: left ＆ right、left ＆＆ right]）
		/// </summary>
		And,
		/// <summary>
		/// 論理和（２つの値の論理和を行います。コード例:[C#: left | right、left || right]）
		/// </summary>
		Or,
		/// <summary>
		/// 排他的論理和（２つの値の排他的論理和を行います。コード例:[C#: left ^ right]）
		/// </summary>
		Xor,
		/// <summary>
		/// 否定論理積（２つの値の否定論理積を行います。コード例:[C#: ~(left ＆ right)、!(left ＆＆ right)]）
		/// </summary>
		Nand,
	}

	#endregion

	#region ExUnaryOperatorType

	/// <summary>
	/// 単項演算子の種類
	/// </summary>
	public enum ExUnaryOperatorType
	{
		/// <summary>
		/// 否定（１つのブール値の真偽を反転します。コード例:[C#: !value]）
		/// </summary>
		BooleanNot,

		/// <summary>
		/// ビット反転（１つの整数値の各ビットを反転します。コード例:[C#: ~value]）
		/// </summary>
		BitwiseNot,
	}

	#endregion

	#region VideoFileType

	/// <summary>
	/// 動画ファイルの種別
	/// </summary>
	public enum ExVideoFileType
	{
		/// <summary>
		/// 自動
		/// </summary>
		Auto,
		/// <summary>
		/// AVI フォーマット
		/// </summary>
		Avi,
		/// <summary>
		/// ASF フォーマット
		/// </summary>
		Asf,
		/// <summary>
		/// WMV フォーマット
		/// </summary>
		Wmv,
	}

	#endregion
}
