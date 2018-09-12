/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using XIE.Ptr;

namespace XIE.Effectors
{
	/// <summary>
	/// パートカラー変換クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxPartColor : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		int m_Depth;
		int m_HueDir;
		int m_HueRange;
		double m_RedRatio;
		double m_GreenRatio;
		double m_BlueRatio;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力画像のビット深度 [0,1~64]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.Depth")]
		public unsafe int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}

		/// <summary>
		/// 抽出する色相の方向 [0~±180 または 0~360] ※ HueDir±HueRange が抽出対象になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.HueDir")]
		public unsafe int HueDir
		{
			get { return m_HueDir; }
			set { m_HueDir = value; }
		}

		/// <summary>
		/// 抽出する色相の範囲 [0~180] ※ HueDir±HueRange が抽出対象になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.HueRange")]
		public unsafe int HueRange
		{
			get { return m_HueRange; }
			set { m_HueRange = value; }
		}

		/// <summary>
		/// 赤成分の変換係数 [0~1] [既定値:0.299]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.RedRatio")]
		public double RedRatio
		{
			get { return m_RedRatio; }
			set { m_RedRatio = value; }
		}

		/// <summary>
		/// 緑成分の変換係数 [0~1] [既定値:0.587]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.GreenRatio")]
		public double GreenRatio
		{
			get { return m_GreenRatio; }
			set { m_GreenRatio = value; }
		}

		/// <summary>
		/// 青成分の変換係数 [0~1] [既定値:0.114]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxPartColor.BlueRatio")]
		public double BlueRatio
		{
			get { return m_BlueRatio; }
			set { m_BlueRatio = value; }
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			m_Depth = 0;
			m_HueDir = 0;
			m_HueRange = 30;
			m_RedRatio = 0.299;
			m_GreenRatio = 0.587;
			m_BlueRatio = 0.114;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPartColor()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <param name="hue_dir">抽出する色相の方向 [0~±180 または 0~360] ※ hue_dir±hue_range が抽出対象になります。</param>
		/// <param name="hue_range">抽出する色相の範囲 [0~180] ※ hue_dir±hue_range が抽出対象になります。</param>
		/// <param name="red_ratio">赤成分の変換係数 [0~1] [既定値:0.299]</param>
		/// <param name="green_ratio">緑成分の変換係数 [0~1] [既定値:0.587]</param>
		/// <param name="blue_ratio">青成分の変換係数 [0~1] [既定値:0.114]</param>
		public CxPartColor(int depth, int hue_dir, int hue_range, double red_ratio = 0.299, double green_ratio = 0.587, double blue_ratio = 0.114)
		{
			_Constructor();
			this.Depth = depth;
			this.HueDir = hue_dir;
			this.HueRange = hue_range;
			this.RedRatio = red_ratio;
			this.GreenRatio = green_ratio;
			this.BlueRatio = blue_ratio;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxPartColor();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;
			if (src is IxModule)
			{
				var _dst = this;
				var _src = (CxPartColor)src;
				_dst.Depth = _src.Depth;
				_dst.HueDir = _src.HueDir;
				_dst.HueRange = _src.HueRange;
				_dst.RedRatio = _src.RedRatio;
				_dst.GreenRatio = _src.GreenRatio;
				_dst.BlueRatio = _src.BlueRatio;
				return;
			}
			if (src is IxConvertible)
			{
				((IxConvertible)src).CopyTo(this);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
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
				var _dst = this;
				var _src = (CxPartColor)src;
				if (_dst.Depth != _src.Depth) return false;
				if (_dst.HueDir != _src.HueDir) return false;
				if (_dst.HueRange != _src.HueRange) return false;
				if (_dst.RedRatio != _src.RedRatio) return false;
				if (_dst.GreenRatio != _src.GreenRatio) return false;
				if (_dst.BlueRatio != _src.BlueRatio) return false;
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="src">入力元</param>
		/// <param name="dst">出力先</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		public virtual void Execute(IxModule src, IxModule dst, IxModule mask = null)
		{
			HxModule hsrc = src.GetHandle();
			HxModule hdst = dst.GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Effectors_PartColor(hsrc, hdst, hmask, Depth, HueDir, HueRange, RedRatio, GreenRatio, BlueRatio);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
