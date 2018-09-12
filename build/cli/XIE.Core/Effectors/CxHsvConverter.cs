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
	/// HSV 変換クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxHsvConverter : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		int m_Depth;
		int m_HueDir;
		double m_SaturationFactor;
		double m_ValueFactor;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力画像のビット深度 [0,1~64]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxHsvConverter.Depth")]
		public unsafe int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}

		/// <summary>
		/// 色相の回転方向 [0~±180 または 0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxHsvConverter.HueDir")]
		public unsafe int HueDir
		{
			get { return m_HueDir; }
			set { m_HueDir = value; }
		}

		/// <summary>
		/// 彩度の変換係数 [0.0~1.0]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxHsvConverter.SaturationFactor")]
		public unsafe double SaturationFactor
		{
			get { return m_SaturationFactor; }
			set { m_SaturationFactor = value; }
		}

		/// <summary>
		/// 明度の変換係数 [0.0~1.0]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxHsvConverter.ValueFactor")]
		public unsafe double ValueFactor
		{
			get { return m_ValueFactor; }
			set { m_ValueFactor = value; }
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
			m_SaturationFactor = 1;
			m_ValueFactor = 1;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxHsvConverter()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <param name="hue_dir">色相の回転方向 [0~±180 または 0~360]</param>
		/// <param name="saturation_factor">彩度の変換係数 [0.0~]</param>
		/// <param name="value_factor">明度の変換係数 [0.0~]</param>
		public CxHsvConverter(int depth, int hue_dir, double saturation_factor, double value_factor)
		{
			_Constructor();
			this.Depth = depth;
			this.HueDir = hue_dir;
			this.SaturationFactor = saturation_factor;
			this.ValueFactor = value_factor;
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
			var clone = new CxHsvConverter();
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
				var _src = (CxHsvConverter)src;
				_dst.Depth = _src.Depth;
				_dst.HueDir = _src.HueDir;
				_dst.SaturationFactor = _src.SaturationFactor;
				_dst.ValueFactor = _src.ValueFactor;
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
				var _src = (CxHsvConverter)src;
				if (_dst.Depth != _src.Depth) return false;
				if (_dst.HueDir != _src.HueDir) return false;
				if (_dst.SaturationFactor != _src.SaturationFactor) return false;
				if (_dst.ValueFactor != _src.ValueFactor) return false;
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
			ExStatus status = xie_core.fnXIE_Effectors_HsvConverter(hsrc, hdst, hmask, Depth, HueDir, SaturationFactor, ValueFactor);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
