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
	/// ランレングスクラスタ
	/// </summary>
	class CxRunCluster
	{
		#region フィールド:

		/// <summary>
		/// 状態 [0:通常、+：結合した物、-：消滅した物]
		/// </summary>
		public int Status;

		/// <summary>
		/// ランレングスのコレクション
		/// </summary>
		public List<TxRunLength> Runs;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRunCluster()
		{
			Status = 0;
			Runs = new List<TxRunLength>();
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// ランレングスの追加
		/// </summary>
		/// <param name="src">追加するランレングス</param>
		public void Add(TxRunLength src)
		{
			Runs.Add(src);
		}

		/// <summary>
		/// ランレングスの追加
		/// </summary>
		/// <param name="src">追加するランレングス</param>
		public void Add(IEnumerable<TxRunLength> src)
		{
			Runs.AddRange(src);
		}

		/// <summary>
		/// 結合可能か否かの検査
		/// </summary>
		/// <param name="src">検査対象のランレングス</param>
		/// <returns>
		///		結合可能な場合は true 、それ以外は false を返します。
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
		/// 指定座標を含むか否かの検査
		/// </summary>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <returns>
		///		指定座標を含む場合は true を返します。
		///		それ以外は false を返します。
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

		#region 解析:

		/// <summary>
		/// 画像解析
		/// </summary>
		/// <param name="src">２値画像 [U8(1)x1ch]</param>
		/// <returns>
		///		指定された２値画像を解析してランレングスクラスタのコレクションを返します。
		/// </returns>
		public static List<CxRunCluster> Parse(XIE.CxImage src)
		{
			var watch = new XIE.CxStopwatch();
			var runs = new List<TxRunLength>();
			var clusters = new List<CxRunCluster>();

			#region 単純走査:
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

			#region クラスタリング:
			{
				watch.Start();

				foreach (var run in runs)
				{
					var chains = new List<CxRunCluster>();

					// 連結可能なクラスタを探す.
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
						// 連結する.
						chains[0].Add(run);
					}
					else if (chains.Count > 1)
					{
						// 連結する.
						var merged_cluster = new CxRunCluster();
						merged_cluster.Status = chains.Count + 1;	// 連結フラグ.
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
						// 種を追加する.
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
