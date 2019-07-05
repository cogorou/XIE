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

namespace XIEstudio
{
	/// <summary>
	/// 画像編集フォーム設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxImageEditorSettings : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEditorSettings()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// オーバレイ図形 (点) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.Point")]
		public virtual XIE.GDI.CxGdiPoint Point
		{
			get { return m_Point; }
			set { m_Point = value; }
		}
		private XIE.GDI.CxGdiPoint m_Point = new XIE.GDI.CxGdiPoint(0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (線分) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.LineSegment")]
		public virtual XIE.GDI.CxGdiLineSegment LineSegment
		{
			get { return m_LineSegment; }
			set { m_LineSegment = value; }
		}
		private XIE.GDI.CxGdiLineSegment m_LineSegment = new XIE.GDI.CxGdiLineSegment(0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (真円) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.Circle")]
		public virtual XIE.GDI.CxGdiCircle Circle
		{
			get { return m_Circle; }
			set { m_Circle = value; }
		}
		private XIE.GDI.CxGdiCircle m_Circle = new XIE.GDI.CxGdiCircle(0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (真円の円弧) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.CircleArc")]
		public virtual XIE.GDI.CxGdiCircleArc CircleArc
		{
			get { return m_CircleArc; }
			set { m_CircleArc = value; }
		}
		private XIE.GDI.CxGdiCircleArc m_CircleArc = new XIE.GDI.CxGdiCircleArc(0, 0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (楕円) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.Ellipse")]
		public virtual XIE.GDI.CxGdiEllipse Ellipse
		{
			get { return m_Ellipse; }
			set { m_Ellipse = value; }
		}
		private XIE.GDI.CxGdiEllipse m_Ellipse = new XIE.GDI.CxGdiEllipse(0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (楕円の円弧) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.EllipseArc")]
		public virtual XIE.GDI.CxGdiEllipseArc EllipseArc
		{
			get { return m_EllipseArc; }
			set { m_EllipseArc = value; }
		}
		private XIE.GDI.CxGdiEllipseArc m_EllipseArc = new XIE.GDI.CxGdiEllipseArc(0, 0, 0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (矩形) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.Rectangle")]
		public virtual XIE.GDI.CxGdiRectangle Rectangle
		{
			get { return m_Rectangle; }
			set { m_Rectangle = value; }
		}
		private XIE.GDI.CxGdiRectangle m_Rectangle = new XIE.GDI.CxGdiRectangle(0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (台形) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.Trapezoid")]
		public virtual XIE.GDI.CxGdiTrapezoid Trapezoid
		{
			get { return m_Trapezoid; }
			set { m_Trapezoid = value; }
		}
		private XIE.GDI.CxGdiTrapezoid m_Trapezoid = new XIE.GDI.CxGdiTrapezoid(0, 0, 0, 0, 0, 0, 0, 0) { PenColor = Color.Red };

		/// <summary>
		/// オーバレイ図形 (文字列) の既定値
		/// </summary>
		[XIE.CxCategory("Figures")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.String")]
		public virtual XIE.GDI.CxGdiString String
		{
			get { return m_String; }
			set { m_String = value; }
		}
		private XIE.GDI.CxGdiString m_String = new XIE.GDI.CxGdiString() { PenColor = Color.Red };

		#endregion

		#region プロパティ: (ツール)

		/// <summary>
		/// ペイントブラシツールの既定値
		/// </summary>
		[XIE.CxCategory("Tools")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.PaintBrush")]
		public virtual XIEstudio.CxPaintBrush PaintBrush
		{
			get { return m_PaintBrush; }
			set { m_PaintBrush = value; }
		}
		private XIEstudio.CxPaintBrush m_PaintBrush = new XIEstudio.CxPaintBrush();

		/// <summary>
		/// ペイント水滴ツールの既定値
		/// </summary>
		[XIE.CxCategory("Tools")]
		[XIE.CxDescription("P:XIEstudio.CxImageEditorSettings.PaintDrop")]
		public virtual XIEstudio.CxPaintDrop PaintDrop
		{
			get { return m_PaintDrop; }
			set { m_PaintDrop = value; }
		}
		private XIEstudio.CxPaintDrop m_PaintDrop = new XIEstudio.CxPaintDrop();
			
		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
		}
		#endregion

		// ////////////////////////////////////////////////////////////
		// インターフェースの実装.
		//

		#region ICloneable の実装:

		/// <summary>
		/// 自身のクローン生成 (ICloneable)
		/// </summary>
		/// <returns>
		///		自身のクローンを返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxImageEditorSettings();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの複製 (XIE.IxEquatable)
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			CxImageEditorSettings _src = (CxImageEditorSettings)src;

			this.Point = _src.Point;
			this.LineSegment = _src.LineSegment;
			this.Circle = _src.Circle;
			this.CircleArc = _src.CircleArc;
			this.Ellipse = _src.Ellipse;
			this.EllipseArc = _src.EllipseArc;
			this.Rectangle = _src.Rectangle;
			this.Trapezoid = _src.Trapezoid;

			if (_src.String == null)
				this.String = null;
			else
				this.String = (XIE.GDI.CxGdiString)_src.String.Clone();

			if (_src.PaintBrush == null)
				this.PaintBrush = null;
			else
				this.PaintBrush = (XIEstudio.CxPaintBrush)_src.PaintBrush.Clone();

			if (_src.PaintDrop == null)
				this.PaintDrop = null;
			else
				this.PaintDrop = (XIEstudio.CxPaintDrop)_src.PaintDrop.Clone();

			return;
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				CxImageEditorSettings _src = (CxImageEditorSettings)src;

				if (this.Point != _src.Point) return false;
				if (this.LineSegment != _src.LineSegment) return false;
				if (this.Circle != _src.Circle) return false;
				if (this.CircleArc != _src.CircleArc) return false;
				if (this.Ellipse != _src.Ellipse) return false;
				if (this.EllipseArc != _src.EllipseArc) return false;
				if (this.Rectangle != _src.Rectangle) return false;
				if (this.Trapezoid != _src.Trapezoid) return false;

				if (this.String != null && _src.String == null) return false;
				if (this.String == null && _src.String != null) return false;
				if (this.String != null && _src.String != null)
				{
					if (this.String.ContentEquals(_src.String) == false) return false;
				}

				if (this.PaintBrush != null && _src.PaintBrush == null) return false;
				if (this.PaintBrush == null && _src.PaintBrush != null) return false;
				if (this.PaintBrush != null && _src.PaintBrush != null)
				{
					if (this.PaintBrush.ContentEquals(_src.PaintBrush) == false) return false;
				}

				if (this.PaintDrop != null && _src.PaintDrop == null) return false;
				if (this.PaintDrop == null && _src.PaintDrop != null) return false;
				if (this.PaintDrop != null && _src.PaintDrop != null)
				{
					if (this.PaintDrop.ContentEquals(_src.PaintDrop) == false) return false;
				}

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region IxFileAccess の実装:

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public static CxImageEditorSettings LoadFrom(string filename, params object[] options)
		{
			return (CxImageEditorSettings)XIE.Axi.ReadAsXml(filename, typeof(CxImageEditorSettings));
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public virtual void Load(string filename, params object[] options)
		{
			this.CopyFrom(XIE.Axi.ReadAsXml(filename, typeof(CxImageEditorSettings)));
		}

		/// <summary>
		/// ファイル保存
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public virtual void Save(string filename, params object[] options)
		{
			XIE.Axi.WriteAsXml(filename, this);
		}

		#endregion

		#region SelfConverter

		/// <summary>
		/// 型変換クラス
		/// </summary>
		internal class SelfConverter : ExpandableObjectConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		オブジェクトの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のオブジェクトを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
