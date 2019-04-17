/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Xml.Serialization;

namespace XIE.Tasks
{
	/// <summary>
	/// イメージグラバースレッド基本クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxGrabberThread : System.Object
		, IDisposable
		, IxRunnable
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabberThread()
		{
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// プロパティ:
		//

		// ////////////////////////////////////////////////////////////
		// 関数.
		//

		#region 初期化と解放:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
		}

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
		}

		#endregion

		#region ノード関連:

		/// <summary>
		/// 名称の取得
		/// </summary>
		/// <returns>
		///		名称を返します。
		/// </returns>
		public virtual string GetName()
		{
			return this.GetType().Name;
		}

		#endregion

		#region イベント:

		/// <summary>
		/// 通知イベント  
		/// </summary>
		public virtual event CxGrabberEventHandler Notify;

		/// <summary>
		/// 通知イベント送信関数
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント引数</param>
		protected virtual void SendNotify(object sender, CxGrabberEventArgs e)
		{
			if (Notify != null)
			{
				Notify(this, e);
			}
		}

		#endregion

		#region IxRunnable の実装:

		/// <summary>
		/// リセット
		/// </summary>
		public virtual void Reset()
		{
		}

		/// <summary>
		/// 開始
		/// </summary>
		public virtual void Start()
		{
		}

		/// <summary>
		/// 停止
		/// </summary>
		public virtual void Stop()
		{
		}

		/// <summary>
		/// 待機
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <returns>
		///		停止を検知すると true を返します。
		///		タイムアウトすると false を返します。
		/// </returns>
		public virtual bool Wait(int timeout)
		{
			return true;
		}

		/// <summary>
		/// 動作状態
		/// </summary>
		/// <returns>
		///		非同期処理が動作中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsRunning
		{
			get
			{
				return false;
			}
		}

		#endregion
	}

	/// <summary>
	/// イメージグラバーイベントデリゲート
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void CxGrabberEventHandler(object sender, CxGrabberEventArgs e);

	/// <summary>
	/// イメージグラバーイベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxGrabberEventArgs : System.EventArgs
		, IxConvertible
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabberEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="timestamp">タイムスタンプ (LCT)</param>
		/// <param name="data">入力データ</param>
		public CxGrabberEventArgs(DateTime timestamp, TxImage data)
		{
			this.TimeStamp = timestamp;
			this.Data = data;
		}

		#endregion

		#region IxConvertible の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="dst">複製先</param>
		public virtual void CopyTo(object dst)
		{
			if (dst is CxImage)
			{
				var _dst = (CxImage)dst;
				using (var _src = CxImage.FromTag(this.Data))
				{
					_dst.CopyFrom(_src);
				}
			}
			else
			{
				throw new CxException(ExStatus.Unsupported);
			}
		}

		/// <summary>
		/// CxImage への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxImage(CxGrabberEventArgs src)
		{
			var dst = new CxImage();
			src.CopyTo(dst);
			return dst;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// タイムスタンプ (LCT)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxGrabberEventArgs.TimeStamp")]
		public virtual DateTime TimeStamp
		{
			get { return m_TimeStamp;  }
			set { m_TimeStamp = value; }
		}
		private DateTime m_TimeStamp = DateTime.Now;

		/// <summary>
		/// 入力データ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxGrabberEventArgs.Data")]
		public virtual TxImage Data
		{
			get { return m_Data; }
			set { m_Data = value; }
		}
		private TxImage m_Data = new TxImage();

		/// <summary>
		/// 処理中断の応答
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxGrabberEventArgs.Cancellation")]
		public virtual bool Cancellation
		{
			get { return m_Cancellation; }
			set { m_Cancellation = value; }
		}
		private bool m_Cancellation = false;

		#endregion
	}
}