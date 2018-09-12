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
	/// ２値化クラス (単一閾値)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxBinarize1 : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		double m_Threshold;
		bool m_UseAbs;
		TxRangeD m_Value;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 閾値 (threshold≦src を真とします。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxBinarize1.Threshold")]
		public unsafe double Threshold
		{
			get { return m_Threshold; }
			set { m_Threshold = value; }
		}

		/// <summary>
		/// 入力値を絶対値として扱うか否か
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxBinarize1.UseAbs")]
		public unsafe bool UseAbs
		{
			get { return m_UseAbs; }
			set { m_UseAbs = value; }
		}

		/// <summary>
		/// 出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxBinarize1.Value")]
		public unsafe TxRangeD Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			m_Threshold = 128;
			m_UseAbs = false;
			m_Value = new TxRangeD(0, 1);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxBinarize1()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="threshold">閾値 (threshold≦src を真とします。)</param>
		/// <param name="use_abs">入力値を絶対値として扱うか否か</param>
		/// <param name="value">出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)</param>
		public CxBinarize1(double threshold, bool use_abs, TxRangeD value)
		{
			_Constructor();
			this.Threshold = threshold;
			this.UseAbs = use_abs;
			this.Value = value;
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
			var clone = new CxBinarize1();
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
				var _src = (CxBinarize1)src;
				_dst.Threshold = _src.Threshold;
				_dst.UseAbs = _src.UseAbs;
				_dst.Value = _src.Value;
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
				var _src = (CxBinarize1)src;
				if (_dst.Threshold != _src.Threshold) return false;
				if (_dst.UseAbs != _src.UseAbs) return false;
				if (_dst.Value != _src.Value) return false;
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
			ExStatus status = xie_core.fnXIE_Effectors_Binarize1(hsrc, hdst, hmask, Threshold, UseAbs, Value);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
