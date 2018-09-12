/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace XIE.Media
{
	/// <summary>
	/// グラバークラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxGrabber : System.Object
		, IDisposable
		, IxRunnable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabber()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="receiver">イベントレシーバ</param>
		internal CxGrabber(CxGrabberInternal receiver)
		{
			_Constructor();
			this.Receiver = receiver;
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxGrabber()
		{
			this.Receiver = null;
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			this.Stop();
			this.Notify = null;
		}

		/// <summary>
		/// 有効性 [初期値:false]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsValid
		{
			get
			{
				return (this.Receiver != null);
			}
		}

		#endregion

		#region IxRunnable の実装:

		/// <summary>
		/// リセット
		/// </summary>
		public void Reset()
		{
			this.Index = -1;
		}

		/// <summary>
		/// 開始
		/// </summary>
		public void Start()
		{
			this.Enable = true;
		}

		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
			this.Enable = false;
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
			var watch = new XIE.CxStopwatch();
			watch.Start();
			while(IsRunning)
			{
				watch.Stop();
				if (0 <= timeout && timeout <= watch.Elapsed)
					return false;
				System.Threading.Thread.Sleep(1);
			}
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
			get { return this.Enable; }
		}
		private bool Enable = false;

		/// <summary>
		/// フレーム指標 [-1,0~]
		/// </summary>
		public virtual int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = -1;

		#endregion

		#region イベント:

		/// <summary>
		/// 通知イベント  
		/// </summary>
		public virtual event CxGrabberHandler Notify;

		/// <summary>
		/// イベント受信オブジェクト  
		/// </summary>
		internal CxGrabberInternal Receiver
		{
			get { return m_Receiver; }
			set
			{
				if (m_Receiver != null)
					m_Receiver.Notify -= Receiver_Notify;

				m_Receiver = value;

				if (m_Receiver != null)
					m_Receiver.Notify += Receiver_Notify;
			}
		}
		private CxGrabberInternal m_Receiver = null;

		/// <summary>
		/// イベント受信関数
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント引数</param>
		private void Receiver_Notify(object sender, CxGrabberArgs e)
		{
			if (this.Enable)
			{
				if (Notify != null)
				{
					var args = (CxGrabberArgs)e.Clone();
					this.Index++;
					if (this.Index < 0)
						this.Index = 0;
					args.Index = this.Index;

					Notify(this, args);

					if (args.Cancellation)
						this.Enable = false;
				}
			}
		}

		#endregion
	}
}
