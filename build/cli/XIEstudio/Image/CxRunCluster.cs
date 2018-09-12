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
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// ���������O�X�N���X�^
	/// </summary>
	class CxRunCluster
	{
		#region �t�B�[���h:

		/// <summary>
		/// ��� [0:�ʏ�A+�F�����������A-�F���ł�����]
		/// </summary>
		public int Status;

		/// <summary>
		/// ���������O�X�̃R���N�V����
		/// </summary>
		public List<TxRunLength> Runs;

		#endregion

		#region �R���X�g���N�^:

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxRunCluster()
		{
			Status = 0;
			Runs = new List<TxRunLength>();
		}

		#endregion

		#region ���\�b�h:

		/// <summary>
		/// ���������O�X�̒ǉ�
		/// </summary>
		/// <param name="src">�ǉ����郉�������O�X</param>
		public void Add(TxRunLength src)
		{
			Runs.Add(src);
		}

		/// <summary>
		/// ���������O�X�̒ǉ�
		/// </summary>
		/// <param name="src">�ǉ����郉�������O�X</param>
		public void Add(IEnumerable<TxRunLength> src)
		{
			Runs.AddRange(src);
		}

		/// <summary>
		/// �����\���ۂ��̌���
		/// </summary>
		/// <param name="src">�����Ώۂ̃��������O�X</param>
		/// <returns>
		///		�����\�ȏꍇ�� true �A����ȊO�� false ��Ԃ��܂��B
		/// </returns>
		public bool CanChain(TxRunLength src)
		{
			if (Runs.Count == 0) return false;
			int st = 0;
			int ed = Runs.Count - 1;
			{
				var item1 = Runs[st];
				var item2 = Runs[ed];
				if (!(item1.Y <= (src.Y - 1) && (src.Y - 1) <= item2.Y)) return false;
			}
			for (int i = ed; i >= st; i--)
			{
				var item = Runs[i];
				if (item.Y == (src.Y - 1))
				{
					if (item.X1 <= src.X1 && src.X1 <= item.X2) return true;
					if (item.X1 <= src.X2 && src.X2 <= item.X2) return true;
					if (item.X1 > src.X1 && src.X2 > item.X2) return true;
				}
				else if (item.Y < (src.Y - 1)) break;
			}
			return false;
		}

		/// <summary>
		/// �w����W���܂ނ��ۂ��̌���
		/// </summary>
		/// <param name="x">X���W</param>
		/// <param name="y">Y���W</param>
		/// <returns>
		///		�w����W���܂ޏꍇ�� true ��Ԃ��܂��B
		///		����ȊO�� false ��Ԃ��܂��B
		/// </returns>
		public bool HitTest(int x, int y)
		{
			foreach (var run in Runs)
			{
				if (run.Y == y && (run.X1 <= x && x <= run.X2)) return true;
			}
			return false;
		}

		#endregion

		#region ���:

		/// <summary>
		/// �摜���
		/// </summary>
		/// <param name="src">�Q�l�摜 [U8(1)x1ch]</param>
		/// <returns>
		///		�w�肳�ꂽ�Q�l�摜����͂��ă��������O�X�N���X�^�̃R���N�V������Ԃ��܂��B
		/// </returns>
		public static List<CxRunCluster> Parse(XIE.CxImage src)
		{
			var watch = new XIE.CxStopwatch();
			var runs = new List<TxRunLength>();
			var clusters = new List<CxRunCluster>();

			#region �P������:
			{
				watch.Start();
				unsafe
				{
					var scan = src.Scanner(0);
					for (int y = 0; y < scan.Height; y++)
					{
						var addr = (byte*)scan[y, 0];
						var rised = false;
						var run = new TxRunLength();

						for (int x = 0; x < scan.Width; x++)
						{
							if (rised)
							{
								if (addr[x] != 0)
								{
									run.X2 = x;
								}
								else
								{
									runs.Add(run);
									rised = false;
									run.X1 = 0;
									run.X2 = 0;
									run.Y = 0;
								}
							}
							else
							{
								if (addr[x] != 0)
								{
									rised = true;
									run.X1 = x;
									run.X2 = x;
									run.Y = y;
								}
							}
						}
						if (rised)
						{
							runs.Add(run);
						}
					}
				}
				watch.Stop();

				Console.WriteLine("{0,-20} : {1:F3} msec", "scanning", watch.Lap);
			}
			#endregion

			#region �N���X�^�����O:
			{
				watch.Start();

				foreach (var run in runs)
				{
					var chains = new List<CxRunCluster>();

					// �A���\�ȃN���X�^��T��.
					foreach (var cluster in clusters)
					{
						if (cluster.Status < 0) continue;
						if (cluster.CanChain(run))
						{
							chains.Add(cluster);
						}
					}

					if (chains.Count == 1)
					{
						// �A������.
						chains[0].Add(run);
					}
					else if (chains.Count > 1)
					{
						// �A������.
						var merged_cluster = new CxRunCluster();
						merged_cluster.Status = chains.Count + 1;	// �A���t���O.
						foreach (var cluster in chains)
						{
							merged_cluster.Add(cluster.Runs);
							cluster.Status = -1;
							cluster.Runs.Clear();
						}
						merged_cluster.Add(run);
						merged_cluster.Runs.Sort();
						clusters.Add(merged_cluster);
					}
					else
					{
						// ���ǉ�����.
						var cluster = new CxRunCluster();
						cluster.Add(run);
						clusters.Add(cluster);
					}
				}

				watch.Stop();

				Console.WriteLine("{0,-20} : {1:F3} msec", "clustering", watch.Lap);
			}
			#endregion

			return clusters;
		}

		#endregion
	}
}
