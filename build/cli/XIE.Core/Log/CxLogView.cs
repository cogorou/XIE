/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace XIE.Log
{
	/// <summary>
	/// ���O�r���[�N���X
	/// </summary>
	public partial class CxLogView : DataGridView
	{
		#region �R���X�g���N�^

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxLogView()
		{
			InitializeComponent();

			if (!IsInDesignMode())
			{
				this.Columns.Add("Number", "Number");
				this.Columns.Add("TimeStamp", "TimeStamp");
				this.Columns.Add("Message", "Message");

				this.Columns[0].Width = 80;
				this.Columns[1].Width = 180;
				this.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}

			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			Interval = 200;
		}

		#endregion

		#region �v���p�e�B:

		/// <summary>
		/// ���O�R���N�V����
		/// </summary>
		public CxLogCollection Logs
		{
			get { return m_Logs; }
			set
			{
				m_Logs = value;

				if (!ReferenceEquals(m_Logs, null))
				{
					viewTimer.Start();
					PreviousNo = m_Logs.SequenceNo;
				}
				else
				{
					viewTimer.Stop();
				}
			}
		}
		private CxLogCollection m_Logs = null;

		/// <summary>
		/// �O�񕪂̗v�f��
		/// </summary>
		private uint PreviousNo = 0;

		/// <summary>
		/// �\���Ώۂ̃��O���x��
		/// </summary>
		public ExLogLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}
		private ExLogLevel m_Level = ExLogLevel.Trace;

		/// <summary>
		/// �����X�N���[���̎w�� [����:false]
		/// </summary>
		public bool AutoScroll
		{
			get { return m_AutoScroll; }
			set { m_AutoScroll = value; }
		}
		private bool m_AutoScroll = false;

		/// <summary>
		/// �X�V���� (ms) [����: 200]
		/// </summary>
		public int Interval
		{
			get { return viewTimer.Interval; }
			set { viewTimer.Interval = value; }
		}

		#endregion

		#region �����֐�.

		/// <summary>
		/// �f�U�C�����[�h���ۂ��̔���
		/// </summary>
		/// <returns></returns>
		private bool IsInDesignMode()
		{
			if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
				return true;
			else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
				return true;

			return false;
		}

		#endregion

		#region ��������.

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void viewTimer_Tick(object sender, EventArgs e)
		{
			if (ReferenceEquals(m_Logs, null))
				return;

			try
			{
				if (this.RowCount != m_Logs.CountInGroup(Level))
					this.RowCount = m_Logs.CountInGroup(Level);

				if (this.RowCount > 0)
				{
					if (AutoScroll)
					{
						if (PreviousNo != m_Logs.SequenceNo)
						{
							int count = this.DisplayedRowCount(false);
							int row = this.RowCount - count;
							if (row < 0)
								row = 0;

							if (this.FirstDisplayedScrollingRowIndex >= 0)
								this.FirstDisplayedScrollingRowIndex = row;

							PreviousNo = m_Logs.SequenceNo;
						}
					}
				}

				this.Invalidate();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		#endregion

		#region �R���g���[���C�x���g.

		/// <summary>
		/// DataGridView �R���g���[���� VirtualMode �v���p�e�B�� true�ŁA�Z���������ݒ肵�ĕ\�����邽�߂� DataGridView ���Z���̒l��K�v�Ƃ���ꍇ�ɔ������܂��B
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxLogView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			CxLogElement log = m_Logs.Fetch(Level, e.RowIndex);

			switch (e.ColumnIndex)
			{
				case 0:
					e.Value = log.Number.ToString();
					break;
				case 1:
					e.Value = log.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss.fff");
					break;
				case 2:
					e.Value = log.Message;
					break;
			}
		}

		#endregion
	}
}
