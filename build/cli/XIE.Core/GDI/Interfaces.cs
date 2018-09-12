/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;

namespace XIE.GDI
{
	#region IxGdi2d

	/// <summary>
	/// オーバレイ図形インターフェース (２次元用)
	/// </summary>
	public interface IxGdi2d
		: IxGdi2dRendering
		, IxGdi2dHandling
		, IxGdi2dVisualizing
	{
	}

	#endregion

	#region IxGdi2dRendering

	/// <summary>
	/// オーバレイ図形描画インターフェース (２次元用)
	/// </summary>
	public interface IxGdi2dRendering
	{
		#region メソッド:

		/// <summary>
		/// 図形をグラフィクスに描画します。
		/// </summary>
		/// <param name="graphics">描画先のグラフィクス</param>
		/// <param name="info">キャンバス情報</param>
		/// <param name="mode">スケーリングモード</param>
		void Render(Graphics graphics, CxCanvasInfo info, ExGdiScalingMode mode);
		
		#endregion
	}

	#endregion

	#region IxGdi2dHandling

	/// <summary>
	/// オーバレイ図形操作インターフェース (２次元用)
	/// </summary>
	public interface IxGdi2dHandling : IxGdi2dRendering
	{
		#region プロパティ:

		/// <summary>
		/// 基準位置
		/// </summary>
		TxPointD Location { get; set; }

		/// <summary>
		/// 外接矩形
		/// </summary>
		TxRectangleD Bounds { get; }

		/// <summary>
		/// 回転角 (degree) [±180]
		/// </summary>
		double Angle { get; set; }

		/// <summary>
		/// 回転の機軸 (基準位置からの相対値(±))
		/// </summary>
		TxPointD Axis { get; set; }

		#endregion

		#region メソッド:

		/// <summary>
		/// 指定座標が図形のどの位置にあるかを判定します。
		/// </summary>
		/// <param name="position">指定座標</param>
		/// <param name="margin">判定の許容範囲 [0~] ※±margin の範囲内で判定します。</param>
		/// <returns>
		///		指定座標が図形外にあれば 0 を返します。
		///		指定座標が図形内にあれば 0 以外の値を返します。
		///		返す値の範囲は図形によって異なります。
		/// </returns>
		TxHitPosition HitTest(TxPointD position, double margin);

		/// <summary>
		/// 図形の編集（位置移動または形状変更）を行います。
		/// </summary>
		/// <param name="prev_figure">編集前の図形</param>
		/// <param name="prev_position">移動前の座標</param>
		/// <param name="move_position">移動後の座標</param>
		/// <param name="margin">判定の許容範囲 [0~] ※±margin の範囲内で判定します。</param>
		void Modify(object prev_figure, TxPointD prev_position, TxPointD move_position, double margin);

		#endregion
	}

	#endregion

	#region IxGdi2dVisualizing

	/// <summary>
	/// オーバレイ図形視覚情報インターフェース (２次元用)
	/// </summary>
	public interface IxGdi2dVisualizing : IxGdi2dHandling
	{
		#region プロパティ:

		/// <summary>
		/// ペン色
		/// </summary>
		TxRGB8x4 PenColor { get; set; }

		/// <summary>
		/// ペン幅
		/// </summary>
		float PenWidth { get; set; }

		/// <summary>
		/// ペン形状
		/// </summary>
		ExGdiPenStyle PenStyle { get; set; }

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		TxRGB8x4 BrushColor { get; set; }

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		TxRGB8x4 BrushShadow { get; set; }

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		ExGdiBrushStyle BrushStyle { get; set; }

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		System.Drawing.Drawing2D.HatchStyle HatchStyle { get; set; }

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode { get; set; }

		#endregion
	}

	#endregion
}
