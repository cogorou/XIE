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
	/// 濃度投影クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxProjection : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		ExScanDir m_ScanDir;
		int m_Channel;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 投影方向
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxProjection.ScanDir")]
		public ExScanDir ScanDir
		{
			get { return m_ScanDir; }
			set { m_ScanDir = value; }
		}

		/// <summary>
		/// 処理対象のチャネル指標(またはフィールド指標) [0~] ※ 処理対象画像の Model.Pack * Channels 未満である必要があります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Effectors.CxProjection.Channel")]
		public int Channel
		{
			get { return m_Channel; }
			set { m_Channel = value; }
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			ScanDir = ExScanDir.X;
			Channel = 0;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxProjection()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="dir">投影方向</param>
		/// <param name="ch">処理対象のチャネル指標(またはフィールド指標) [0~] ※ 処理対象画像の Model.Pack * Channels 未満である必要があります。</param>
		public CxProjection(ExScanDir dir, int ch)
		{
			_Constructor();
			ScanDir = dir;
			Channel = ch;
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
			var clone = new CxProjection();
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
				var _src = (CxProjection)src;
				_dst.ScanDir = _src.ScanDir;
				_dst.Channel = _src.Channel;
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
				var _src = (CxProjection)src;
				if (_dst.ScanDir != _src.ScanDir) return false;
				if (_dst.Channel != _src.Channel) return false;
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
		/// <param name="src">入力元 [CxImage]</param>
		/// <param name="dst">出力先 [CxArray]</param>
		/// <param name="mask">マスク [CxImage] (省略する場合は null を指定してください。)</param>
		public virtual void Execute(IxModule src, IxModule dst, IxModule mask = null)
		{
			HxModule hsrc = src.GetHandle();
			HxModule hdst = dst.GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : mask.GetHandle();
			ExStatus status = xie_core.fnXIE_Effectors_Projection(hsrc, hdst, hmask, this.ScanDir, this.Channel);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
