/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using HWND = System.IntPtr;
using HDC = System.IntPtr;

namespace XIEcapture
{
	/// <summary>
	/// スクリーンキャプチャフォーム
	/// </summary>
	public partial class CxCaptureForm : Form
	{
		#region 共有データ:

		/// <summary>
		/// アプリケーション設定
		/// </summary>
		public static CxAppSettings AppSettings = null;

		/// <summary>
		/// アプリケーション設定ファイルパス
		/// </summary>
		public static string AppSettingsFileName = "";

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxCaptureForm()
		{
			InitializeComponent();

			ImageView = new XIE.GDI.CxImageView();
			ImageView.Image = new XIE.CxImage();
			ImageView.Dock = DockStyle.Fill;
			ImageView.EnableMouseGrip = false;
			ImageView.EnableWheelScaling = false;
			ImageView.EnableWheelScroll = false;
			ImageView.Rendering += ImageView_Rendering;
			splitMain.Panel1.Controls.Add(ImageView);

			ScreenList = new XIE.Media.CxScreenList();

			ScreenCapture = new XIE.Media.CxScreenCapture();
			ScreenCaptureGrabber = ScreenCapture.CreateGrabber();
			ScreenCaptureGrabber.Notify += ScreenCaptureGrabber_Notify;
			if (0 <= AppSettings.FrameRate && AppSettings.FrameRate < FrameRates.Length)
				ScreenCapture.FrameRate = FrameRates[AppSettings.FrameRate];
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxCaptureForm_Load(object sender, EventArgs e)
		{
			#region comboFps 初期化:
			int index = 0;
			comboFps.Items.Clear();
			for (int i=0 ; i<FrameRates.Length ; i++)
			{
				comboFps.Items.Add(string.Format("{0} fps", FrameRates[i]));
				if (ScreenCapture.FrameRate == FrameRates[i])
					index = i;
			}
			comboFps.SelectedIndex = index;
			#endregion

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられる時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxCaptureForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (ScreenCapture != null && ScreenCapture.IsRunning)
			{
				e.Cancel = true;
				return;
			}

			timerUpdateUI.Stop();

			if (ScreenCaptureGrabber != null)
				ScreenCaptureGrabber.Dispose();
			ScreenCaptureGrabber = null;

			if (ScreenCapture != null)
				ScreenCapture.Dispose();
			ScreenCapture = null;

			if (ScreenList != null)
				ScreenList.Dispose();
			ScreenList = null;
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region 表示更新:
			var is_running = ScreenCapture.IsRunning;
			var selected = (listScreenList.SelectedItems.Count > 0);

			// toolbar:
			toolRefresh.Enabled = (is_running == false);
			toolStart.Enabled = (is_running == false && selected == true);
			toolStop.Enabled = (is_running == true);

			// listScreenList:
			listScreenList.Enabled = (is_running == false);

			// statusbar:
			if (is_running)
				statusMessage.Text = "During capture ...";
			else if (selected)
				statusMessage.Text = "Ready";
			else if (listScreenList.Items.Count > 0)
				statusMessage.Text = "Please select a screen.";
			else
				statusMessage.Text = "Please refresh screen list.";

			if (is_running)
			{
				var timespan = DateTime.Now - StartTime;
				statusTimespan.Text = timespan.ToString(@"hh\:mm\:ss");

				var mean = ScreenCaptureGrabberStat.Mean;
				if (mean == 0)
					statusFps.Text = "[ 0 fps ]";
				else
					statusFps.Text = string.Format("[ {0:F0} fps ]", 1000/mean);
			}
			#endregion
		}

		#region オブジェクト:

		/// <summary>
		/// 画像ビュー
		/// </summary>
		private XIE.GDI.CxImageView ImageView = null;

		/// <summary>
		/// フレームレート配列
		/// </summary>
		private int[] FrameRates = 
			{
				1, 5, 10, 15, 30
			};

		/// <summary>
		/// スクリーンリスト
		/// </summary>
		private XIE.Media.CxScreenList ScreenList = null;

		/// <summary>
		/// スクリーンキャプチャ
		/// </summary>
		private XIE.Media.CxScreenCapture ScreenCapture = null;

		/// <summary>
		/// スクリーンキャプチャのグラバー
		/// </summary>
		private XIE.Media.CxGrabber ScreenCaptureGrabber = null;

		#endregion

		#region toolbar: (表示更新)

		/// <summary>
		/// 表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolRefresh_Click(object sender, EventArgs e)
		{
			ScreenList.Setup();

			#region リストの生成と表示:
			int seqno = 0;
			listScreenList.Items.Clear();
			foreach (var item in ScreenList)
			{
				var lvitem = new ListViewItem();
				lvitem.Name = item.Handle.ToString("X8");
				lvitem.Text = seqno.ToString();
				lvitem.SubItems.Add(item.Name);
				lvitem.SubItems.Add(item.Bounds.ToString());
				listScreenList.Items.Add(lvitem);
				seqno++;
			}
			listScreenList.Refresh();
			#endregion

			#region プレビュー画像の生成と表示:
			var stat_x = new XIE.TxStatistics();
			var stat_y = new XIE.TxStatistics();
			foreach (var item in ScreenList)
			{
				var trapezoid = item.Bounds.ToTrapezoid();
				stat_x += trapezoid.X1;
				stat_x += trapezoid.X3;
				stat_y += trapezoid.Y1;
				stat_y += trapezoid.Y3;
			}
			var sx = (int)stat_x.Min;
			var sy = (int)stat_y.Min;
			var ex = (int)stat_x.Max;
			var ey = (int)stat_y.Max;
			var width = ex - sx;
			var height = ey - sy;

			using (var bitmap = new Bitmap(width, height))
			using (var graphics = Graphics.FromImage(bitmap))
			{
				HWND hwnd = GetDesktopWindow();
				HDC hdcSrc = GetDC(hwnd);
				HDC hdcDst = graphics.GetHdc();
				BitBlt(hdcDst, 0, 0, width, height, hdcSrc, sx, sy, SRCCOPY);
				graphics.ReleaseHdc();
				ReleaseDC(hwnd, hdcSrc);

				ImageView.Image.CopyFrom(bitmap);
			}

			ImageView.FitImageSize(0);
			ImageView.Refresh();
			#endregion
		}

		[DllImport("user32")]
		static extern HWND GetDesktopWindow();

		[DllImport("user32")]
		static extern HDC GetDC(HWND hWnd);

		[DllImport("user32")]
		static extern int ReleaseDC(HWND hWnd, HDC hDC);

		[DllImport("gdi32")]
		static extern bool BitBlt(HDC hdc, int x, int y, int cx, int cy, HDC hdcSrc, int x1, int y1, uint rop);

		const uint SRCCOPY = (uint)0x00CC0020;

		#endregion

		#region toolbar: (開始/停止)

		/// <summary>
		/// キャプチャ開始時間
		/// </summary>
		private DateTime StartTime = DateTime.Now;

		/// <summary>
		/// キャプチャ開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStart_Click(object sender, EventArgs e)
		{
			if (ScreenCapture.IsRunning == true) return;

			if (listScreenList.SelectedItems.Count > 0)
			{
				var lvitem = listScreenList.SelectedItems[0];
				var index = lvitem.Index;
				if (0 <= index && index < ScreenList.Length)
				{
					try
					{
						var item = ScreenList[index];
						var ts = DateTime.Now;
						var filename = string.Format(
							"ScreenCapture-{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}.avi",
							ts.Year, ts.Month, ts.Day, ts.Hour, ts.Minute, ts.Second);
						var filepath = System.IO.Path.Combine(XIEcapture.SharedData.ProjectDir, filename);
						var audio_input = AppSettings.AudioInputInfo;
						using (var output_file = new XIE.CxStringA(filepath))
						{
							ScreenCapture.Setup(item, audio_input, output_file);
						}

						StartTime = DateTime.Now;
						ScreenCaptureGrabber_Start();
						ScreenCapture.Start();
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		/// <summary>
		/// キャプチャ停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStop_Click(object sender, EventArgs e)
		{
			if (ScreenCapture.IsRunning == false) return;

			try
			{
				ScreenCapture.Stop();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		#endregion

		#region toolbar: (音声入力選択)

		/// <summary>
		/// 音声入力選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAudioInput_Click(object sender, EventArgs e)
		{
			var param = (XIE.Media.CxDeviceParam)AppSettings.AudioInputInfo.Clone();
			var dlg = new XIE.Media.CxDeviceSelectionForm(
				XIE.Media.ExMediaType.Audio,
				XIE.Media.ExMediaDir.Input,
				param);
			dlg.StartPosition = FormStartPosition.CenterParent;
			dlg.Text = string.Format("Audio Input");
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				if (string.IsNullOrEmpty(param.Name))
				{
					AppSettings.AudioInputInfo = new XIE.Media.CxDeviceParam();
				}
				else
				{
					AppSettings.AudioInputInfo = (XIE.Media.CxDeviceParam)dlg.Param.Clone();
				}
			}
		}

		#endregion

		#region toolbar: (フレームレート選択)

		/// <summary>
		/// フレームレート選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboFps_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = comboFps.SelectedIndex;
			ScreenCapture.FrameRate = FrameRates[index];
			AppSettings.FrameRate = index;
		}

		#endregion

		#region ImageView:

		/// <summary>
		/// オーバレイ図形の表示処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (listScreenList.SelectedItems.Count > 0)
			{
				var lvitem = listScreenList.SelectedItems[0];
				var index = lvitem.Index;
				if (0 <= index && index < ScreenList.Length)
				{
					var item = ScreenList[index];
					var figure = new XIE.GDI.CxGdiRectangle(item.Bounds);
					figure.PenColor = new XIE.TxRGB8x4(0xFF, 0x00, 0x00);
					figure.BrushColor = new XIE.TxRGB8x4(0xFF, 0x7F, 0x00, 0x3F);
					figure.BrushStyle = XIE.GDI.ExGdiBrushStyle.Solid;
					e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.TopLeft);
				}
			}
		}

		#endregion

		#region listScreenList:

		/// <summary>
		/// 項目の選択状態が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listScreenList_SelectedIndexChanged(object sender, EventArgs e)
		{
			ImageView.Refresh();
		}

		#endregion

		#region ScreenCapture:

		/// <summary>
		/// スクリーンキャプチャのグラバーの時間計測用
		/// </summary>
		private XIE.CxStopwatch ScreenCaptureGrabberWatch = new XIE.CxStopwatch();

		/// <summary>
		/// スクリーンキャプチャのグラバーの時間計測用
		/// </summary>
		private XIE.TxStatistics ScreenCaptureGrabberStat = new XIE.TxStatistics();

		/// <summary>
		/// スクリーンキャプチャのグラバーの時間計測開始
		/// </summary>
		private void ScreenCaptureGrabber_Start()
		{
			if (ScreenCaptureGrabber != null)
			{
				ScreenCaptureGrabberWatch.Reset();
				ScreenCaptureGrabberWatch.Start();
				ScreenCaptureGrabber.Start();
			}
		}

		/// <summary>
		/// スクリーンキャプチャのグラバーの通知イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ScreenCaptureGrabber_Notify(object sender, XIE.Media.CxGrabberArgs e)
		{
			ScreenCaptureGrabberWatch.Stop();
			ScreenCaptureGrabberStat += ScreenCaptureGrabberWatch.Lap;
		}

		#endregion
	}
}
