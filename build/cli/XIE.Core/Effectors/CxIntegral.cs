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
	/// 積分画像生成クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxIntegral : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		int m_Mode;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 処理モード [1:総和、2:2乗の総和]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxIntegral.Mode")]
		public unsafe int Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			Mode = 1;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxIntegral()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="mode">処理モード [1:総和、2:2乗の総和]</param>
		public CxIntegral(int mode)
		{
			_Constructor();
			this.Mode = mode;
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
			var clone = new CxIntegral();
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
				var _src = (CxIntegral)src;
				_dst.Mode = _src.Mode;
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
				var _src = (CxIntegral)src;
				if (_dst.Mode != _src.Mode) return false;
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
			ExStatus status = xie_core.fnXIE_Effectors_Integral(hsrc, hdst, hmask, Mode);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
