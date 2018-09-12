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

namespace XIE
{
	/// <summary>
	/// フレーム指標構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxFrameIndex :
		IEquatable<TxFrameIndex>
	{
		#region フィールド:

		int m_Track;
		int m_Frame;
		int m_Flag;
		double m_Progress;
		ulong m_TimeStamp;

		#endregion

		#region プロパティ:

		/// <summary>
		/// トラック指標 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxFrameIndex.Track")]
		public int Track
		{
			get { return m_Track; }
			set { m_Track = value; }
		}

		/// <summary>
		/// フレーム指標 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxFrameIndex.Frame")]
		public int Frame
		{
			get { return m_Frame; }
			set { m_Frame = value; }
		}

		/// <summary>
		/// 取り込み状態 [0=継続、1=到達]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxFrameIndex.Flag")]
		public int Flag
		{
			get { return m_Flag; }
			set { m_Flag = value; }
		}

		/// <summary>
		/// 経過時間
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxFrameIndex.Progress")]
		public double Progress
		{
			get { return m_Progress; }
			set { m_Progress = value; }
		}

		/// <summary>
		/// タイムスタンプ (UTC)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxFrameIndex.TimeStamp")]
		public ulong TimeStamp
		{
			get { return m_TimeStamp; }
			set { m_TimeStamp = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="track">トラック指標 [0~]</param>
		/// <param name="frame">フレーム指標 [0~]</param>
		/// <param name="flag">取り込み状態 [0=継続、1=到達]</param>
		/// <param name="progress">経過時間</param>
		/// <param name="timestamp">タイムスタンプ (UTC)</param>
		public TxFrameIndex(int track, int frame, int flag, double progress, ulong timestamp)
		{
			m_Track = track;
			m_Frame = frame;
			m_Flag = flag;
			m_Progress = progress;
			m_TimeStamp = timestamp;
		}

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxFrameIndex other)
		{
			if (this.Track != other.Track) return false;
			if (this.Frame != other.Frame) return false;
			if (this.Flag != other.Flag) return false;
			if (this.Progress != other.Progress) return false;
			if (this.TimeStamp != other.TimeStamp) return false;
			return true;
		}

		/// <summary>
		/// 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="obj">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (!(obj is TxFrameIndex)) return false;
			return this.Equals((TxFrameIndex)obj);
		}

		/// <summary>
		/// ハッシュ値の取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		/// </returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region 比較オペレータ:

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator ==(TxFrameIndex left, TxFrameIndex right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は false、それ以外は true を返します。
		/// </returns>
		public static bool operator !=(TxFrameIndex left, TxFrameIndex right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
