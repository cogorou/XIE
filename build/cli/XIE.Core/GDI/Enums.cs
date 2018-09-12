/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XIE.GDI
{
	#region スケーリングモード

	/// <summary>
	/// スケーリングモード
	/// </summary>
	public enum ExGdiScalingMode
	{
		/// <summary>
		/// オーバレイ図形を画像の伸縮に追従させません。
		/// </summary>
		None = 0,

		/// <summary>
		/// オーバレイ図形を画像の伸縮に追従させます。画素の中心を 0,0 とします。画素の左上が -0.5,-0.5、右下が 0.5,0.5 になります。
		/// </summary>
		Center,

		/// <summary>
		/// オーバレイ図形を画像の伸縮に追従させます。画素の左上を 0,0 とします。画素の右下が 1.0,1.0 になります。
		/// </summary>
		TopLeft,
	};

	#endregion

	#region アンカースタイル

	/// <summary>
	/// アンカースタイル
	/// </summary>
	public enum ExGdiAnchorStyle
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// 矢印
		/// </summary>
		Arrow,
		/// <summary>
		/// 十字
		/// </summary>
		Cross,
		/// <summary>
		/// 交差 (斜め)
		/// </summary>
		Diagcross,
		/// <summary>
		/// 菱形
		/// </summary>
		Diamond,
		/// <summary>
		/// 矩形
		/// </summary>
		Rectangle,
		/// <summary>
		/// 三角形
		/// </summary>
		Triangle,
	};

	#endregion

	#region ペンのスタイル

	/// <summary>
	/// ペンのスタイル
	/// </summary>
	public enum ExGdiPenStyle
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// 実線
		/// </summary>
		Solid,
		/// <summary>
		/// 点線
		/// </summary>
		Dot,
		/// <summary>
		/// 破線
		/// </summary>
		Dash,
		/// <summary>
		/// １点破線
		/// </summary>
		DashDot,
		/// <summary>
		/// ２点破線
		/// </summary>
		DashDotDot,
	};

	#endregion

	#region ブラシのスタイル

	/// <summary>
	/// ブラシのスタイル
	/// </summary>
	public enum ExGdiBrushStyle
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// 塗りつぶし
		/// </summary>
		Solid,
		/// <summary>
		/// ハッチ
		/// </summary>
		Hatch,
		/// <summary>
		/// 線形グラデーション
		/// </summary>
		LinearGradient,
	};

	#endregion

	#region テキストの配置

	/// <summary>
	/// テキストの配置
	/// </summary>
	public enum ExGdiTextAlign
	{
		/// <summary>
		/// 上辺の左端を基準座標とします。
		/// </summary>
		TopLeft = 0,
		/// <summary>
		/// 上辺の右端を基準座標とします。
		/// </summary>
		TopRight = 2,
		/// <summary>
		/// 上辺の中央を基準座標とします。
		/// </summary>
		TopCenter = 6,
		/// <summary>
		/// 下辺の左端を基準座標とします。
		/// </summary>
		BottomLeft = 8,
		/// <summary>
		/// 下辺の右端を基準座標とします。
		/// </summary>
		BottomRight = 10,
		/// <summary>
		/// 下辺の中央を基準座標とします。
		/// </summary>
		BottomCenter = 14,
		/// <summary>
		/// 文字のベースラインの左端を基準座標とします。
		/// </summary>
		BaselineLeft = 24,
		/// <summary>
		/// 文字のベースラインの右端を基準座標とします。
		/// </summary>
		BaselineRight = 22,
		/// <summary>
		/// 文字のベースラインの中央を基準座標とします。
		/// </summary>
		BaselineCenter = 30,
	};

	#endregion

	#region 矩形の位置を示す定数.

	/// <summary>
	/// 矩形の位置を示す定数
	/// </summary>
	public enum ExGdiRectPosition
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// 左辺
		/// </summary>
		Left = 0x01,
		/// <summary>
		/// 上辺
		/// </summary>
		Top = 0x02,
		/// <summary>
		/// 右辺
		/// </summary>
		Right = 0x04,
		/// <summary>
		/// 下辺
		/// </summary>
		Bottom = 0x08,
		/// <summary>
		/// 左上
		/// </summary>
		TopLeft = 0x03,
		/// <summary>
		/// 右上
		/// </summary>
		TopRight = 0x06,
		/// <summary>
		/// 左下
		/// </summary>
		BottomLeft = 0x09,
		/// <summary>
		/// 右下
		/// </summary>
		BottomRight = 0x0C,
		/// <summary>
		/// 範囲内の何れか
		/// </summary>
		All = 0x0F,
	};

	#endregion
}
