/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE.Log
{
	/// <summary>
	/// ���O�v�f
	/// </summary>
	public class CxLogElement
	{
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="number">�V�[�P���X�ԍ� [�͈�:0~]</param>
		/// <param name="level">���O�o�̓��x�� [����l:Trace]</param>
		/// <param name="message">���b�Z�[�W</param>
		public CxLogElement(uint number, ExLogLevel level, string message)
		{
			this.m_Number = number;
			this.m_Level = level;
			this.m_Message = message;
			this.m_TimeStamp = DateTime.Now;
		}

		#region �v���p�e�B:

		/// <summary>
		/// �V�[�P���X�ԍ� [�͈�:0~]
		/// </summary>
		public uint Number
		{
			get { return m_Number; }
		}
		private UInt32 m_Number = 0;

		/// <summary>
		/// ���O�o�̓��x�� [����l:Trace]
		/// </summary>
		public ExLogLevel Level
		{
			get { return m_Level; }
		}
		private ExLogLevel m_Level = ExLogLevel.Trace;

		/// <summary>
		/// ���b�Z�[�W
		/// </summary>
		public string Message
		{
			get { return m_Message; }
		}
		string m_Message;

		/// <summary>
		/// �^�C���X�^���v
		/// </summary>
		public DateTime TimeStamp
		{
			get { return m_TimeStamp; }
		}
		DateTime m_TimeStamp;

		#endregion
	}
}
