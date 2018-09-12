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
	/// ガンマ変換クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxGammaConverter : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		int m_Depth;
		double m_Gamma;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力画像のビット深度 [0,1~64]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxGammaConverter.Depth")]
		public unsafe int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}

		/// <summary>
		/// ガンマ値 [0.0 以外] ※ 1.0 が無変換を意味します。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxGammaConverter.Gamma")]
		public unsafe double Gamma
		{
			get { return m_Gamma; }
			set { m_Gamma = value; }
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			m_Depth = 0;
			m_Gamma = 1;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGammaConverter()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <param name="gamma">ガンマ値 [0.0 以外] ※ 1.0 が無変換を意味します。</param>
		public CxGammaConverter(int depth, double gamma)
		{
			_Constructor();
			this.Depth = depth;
			this.Gamma = gamma;
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
			var clone = new CxGammaConverter();
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
				var _src = (CxGammaConverter)src;
				_dst.Depth = _src.Depth;
				_dst.Gamma = _src.Gamma;
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
				var _src = (CxGammaConverter)src;
				if (_dst.Depth != _src.Depth) return false;
				if (_dst.Gamma != _src.Gamma) return false;
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
			ExStatus status = xie_core.fnXIE_Effectors_GammaConverter(hsrc, hdst, hmask, Depth, Gamma);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
