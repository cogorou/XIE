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
	/// データ入出力ポートスレッド基本クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxDataPortThread : System.Object
		, IDisposable
		, IxRunnable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDataPortThread()
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
		public virtual event CxDataPortEventHandler Notify;

		/// <summary>
		/// 通知イベント送信関数
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">イベント引数</param>
		protected virtual void SendNotify(object sender, CxDataPortEventArgs e)
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
	/// データ入力ポートインターフェース
	/// </summary>
	public interface IxDataPortIn
	{
		#region プロパティ:

		/// <summary>
		/// データ長
		/// </summary>
		int Length { get; }

		/// <summary>
		/// データ型
		/// </summary>
		Type DataType { get; }

		/// <summary>
		/// データ格納種別
		/// </summary>
		ExDataStoreType StoreType { get; }

		#endregion
	}

	/// <summary>
	/// データ入力ポートインターフェース (データの取得と設定)
	/// </summary>
	public interface IxDataPortIn<TE> : IxDataPortIn
	{
		#region メソッド:

		/// <summary>
		/// 入力データの取得
		/// </summary>
		/// <param name="index">指標 [0~]</param>
		/// <returns>
		///		入力データを返します。
		/// </returns>
		TE GetData(int index);

		/// <summary>
		/// 入力データの設定
		/// </summary>
		/// <param name="index">指標 [0~]</param>
		/// <param name="value">設定値</param>
		void SetData(int index, TE value);

		#endregion
	}

	/// <summary>
	/// データ出力ポートインターフェース
	/// </summary>
	public interface IxDataPortOut
	{
		#region プロパティ:

		/// <summary>
		/// データ長
		/// </summary>
		int Length { get; }

		/// <summary>
		/// データ型
		/// </summary>
		Type DataType { get; }

		/// <summary>
		/// データ格納種別
		/// </summary>
		ExDataStoreType StoreType { get; }

		#endregion
	}

	/// <summary>
	/// データ出力ポートインターフェース (データの取得と設定)
	/// </summary>
	public interface IxDataPortOut<TE> : IxDataPortOut
	{
		#region メソッド:

		/// <summary>
		/// 出力データの取得
		/// </summary>
		/// <param name="index">指標 [0~]</param>
		/// <returns>
		///		出力データを返します。
		/// </returns>
		TE GetData(int index);

		/// <summary>
		/// 出力データの設定
		/// </summary>
		/// <param name="index">指標 [0~]</param>
		/// <param name="value">設定値</param>
		void SetData(int index, TE value);

		#endregion
	}

	/// <summary>
	/// データ入出力ポートイベントデリゲート
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void CxDataPortEventHandler(object sender, CxDataPortEventArgs e);

	/// <summary>
	/// データ入出力ポートイベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxDataPortEventArgs : System.EventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDataPortEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="timestamp">タイムスタンプ (LCT)</param>
		/// <param name="index">データ指標 [0~]</param>
		/// <param name="count">データ個数 [1~]</param>
		public CxDataPortEventArgs(DateTime timestamp, int index, int count)
		{
			this.TimeStamp = timestamp;
			this.Index = index;
			this.Count = count;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムスタンプ (LCT)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortEventArgs.TimeStamp")]
		public virtual DateTime TimeStamp
		{
			get { return m_TimeStamp; }
			set { m_TimeStamp = value; }
		}
		private DateTime m_TimeStamp = DateTime.Now;

		/// <summary>
		/// データ指標 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortEventArgs.Index")]
		public virtual int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		/// <summary>
		/// データ個数 [1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortEventArgs.Count")]
		public virtual int Count
		{
			get { return m_Count; }
			set { m_Count = value; }
		}
		private int m_Count = 0;

		/// <summary>
		/// 処理中断の応答
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortEventArgs.Cancellation")]
		public virtual bool Cancellation
		{
			get { return m_Cancellation; }
			set { m_Cancellation = value; }
		}
		private bool m_Cancellation = false;

		#endregion
	}
}