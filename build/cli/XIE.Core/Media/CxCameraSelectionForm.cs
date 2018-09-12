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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XIE.Media
{
	/// <summary>
	/// カメラ選択フォーム
	/// </summary>
	public partial class CxCameraSelectionForm : Form
	{
		#region 初期化と解放:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxCameraSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="param">デバイスパラメータ</param>
		public CxCameraSelectionForm(CxDeviceParam param)
		{
			InitializeComponent();
			if (param != null)
				this.Param = param;
		}

		#endregion

		#region フォームの初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxCameraSelectionForm_Load(object sender, EventArgs e)
		{
			try
			{
				if (this.Param != null)
					this.DeviceParam.CopyFrom(this.Param);

				this.propertyParam.SelectedObject = this.DeviceParam;

				panelView_Setup();
				Grabber_Setup();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxCameraSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();

			panelView_Dispose();
			Grabber_Dispose();
		}

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			if (this.Param == null)
				this.Param = new CxDeviceParam();
			this.Param.CopyFrom(this.DeviceParam);

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region 画像取り込み関連の初期化と解放:

		/// <summary>
		/// 画像取り込み関連の初期化
		/// </summary>
		void Grabber_Setup()
		{
			for (int i = 0; i < this.Images.Length; i++)
			{
				if (this.Images[i] == null)
					this.Images[i] = new CxImage();
			}

			this.DeviceList.Dispose();
			this.DeviceParam.Dispose();
			this.Camera.Dispose();
			this.Grabber.Dispose();

			comboName.Items.Clear();
			comboPin.Items.Clear();
			comboSize.Items.Clear();

			// デバイスリストの初期化.
			this.DeviceList.Setup(ExMediaType.Video, ExMediaDir.Input);
			foreach (var item in this.DeviceList)
			{
				comboName.Items.Add(item.GetProductName());
			}

			// 初期状態でデバイス名称リストを選択するか否か.
			if (this.Param != null && this.DeviceList.IsValid)
			{
				int index = this.DeviceList.FindIndex(this.Param.Name, this.Param.Index);
				if (0 <= index && index < comboName.Items.Count)
					comboName.SelectedIndex = index;
			}
		}

		/// <summary>
		/// 画像取り込み関連の解放
		/// </summary>
		void Grabber_Dispose()
		{
			this.Grabber.Dispose();

			this.Camera.Abort();
			this.Camera.Dispose();

			for (int i = 0; i < this.Images.Length; i++)
			{
				if (this.Images[i] != null)
					this.Images[i].Dispose();
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// デバイスパラメータ
		/// </summary>
		public CxDeviceParam Param
		{
			get { return m_Param; }
			set { m_Param = value; }
		}
		private CxDeviceParam m_Param = null;

		#endregion

		#region 画像取り込み関連:

		/// <summary>
		/// ビデオデバイスリスト
		/// </summary>
		private CxDeviceList DeviceList = new CxDeviceList();

		/// <summary>
		/// デバイスパラメータ (バックアップ)
		/// </summary>
		private CxDeviceParam DeviceParam = new CxDeviceParam();

		/// <summary>
		/// カメラデバイス
		/// </summary>
		private CxCamera Camera = new CxCamera();

		/// <summary>
		/// 画像取り込み用イベントレシーバ
		/// </summary>
		private CxGrabber Grabber = new CxGrabber();

		/// <summary>
		/// 画像取り込み用バッファの現在位置を示す指標
		/// </summary>
		private int BufferIndex = 0;

		/// <summary>
		/// 画像取り込み用バッファ
		/// </summary>
		private CxImage[] Images = new CxImage[5];

		/// <summary>
		/// 画像更新フラグ
		/// </summary>
		private bool ImageUpdated = false;

		/// <summary>
		/// タイムスタンプ (最終)
		/// </summary>
		private DateTime LastTimeStamp = DateTime.Now;

		/// <summary>
		/// タイムスタンプのコレクション
		/// </summary>
		private List<DateTime> TimeStamps = new List<DateTime>();

		/// <summary>
		/// フレームレート (fps)
		/// </summary>
		private double FrameRate = 0.0;

		/// <summary>
		/// 画像取り込み通知
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Grabber_Notify(object sender, CxGrabberArgs e)
		{
			try
			{
				this.BufferIndex = e.Index % this.Images.Length;
				e.CopyTo(this.Images[this.BufferIndex]);

				var dt = TxDateTime.FromBinary(e.TimeStamp, true);
				this.LastTimeStamp = dt;
				this.TimeStamps.Add(dt);
				int count = this.TimeStamps.Count;
				if (count > 30)
				{
					double sum = 0.0;
					for (int i = 1; i < count; i++)
					{
						TimeSpan diff = this.TimeStamps[i].Subtract(this.TimeStamps[i - 1]);
						sum += diff.TotalSeconds;
					}
					if (sum != 0)
						this.FrameRate = count / sum;

					this.TimeStamps.Clear();
				}

				this.ImageUpdated = true;
			}
			catch (System.Exception)
			{
			}
		}

		#endregion

		#region 画像表示:

		/// <summary>
		/// 画像表示用パネルの初期化
		/// </summary>
		private void panelView_Setup()
		{
		}

		/// <summary>
		/// 画像表示用パネルの解放
		/// </summary>
		private void panelView_Dispose()
		{
		}

		/// <summary>
		/// 画像表示用パネルのサイズ変更が発生したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelView_Resize(object sender, EventArgs e)
		{
		}

		#endregion

		#region 定期的な表示更新処理:

		/// <summary>
		/// 定期的な表示更新処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region ツールバーの表示更新.
			toolStart.Enabled = (this.Camera.IsValid && !this.Camera.IsRunning);
			toolPause.Enabled = (this.Camera.IsRunning);
			toolPause.Checked = (this.Camera.IsRunning && !this.Grabber.IsRunning);
			toolStop.Enabled = (this.Camera.IsRunning);
			buttonProperty.Enabled = (this.Camera.IsValid);
			#endregion

			var image = this.Images[this.BufferIndex];

			#region 画像表示:
			if (this.ImageUpdated)
			{
				this.ImageUpdated = false;
				using (var graphics = this.panelView.CreateGraphics())
				using (var canvas = new XIE.GDI.CxCanvas()) 
				{
					canvas.Attach(graphics, this.panelView.Width, this.panelView.Height);

					double mag_x = (double)this.panelView.Width / image.Width;
					double mag_y = (double)this.panelView.Height / image.Height;
					double mag = System.Math.Min(mag_x, mag_y);
					canvas.Magnification = mag;
					canvas.DrawImage(image);

					statusMag.Text = string.Format("{0:F0} %", mag * 100);
				}
			}
			#endregion

			#region ステータスバーの表示更新:
			{
				statusFps.Text = string.Format("{0:F2} fps", this.FrameRate);
				statusTimeStamp.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
					this.LastTimeStamp.Hour,
					this.LastTimeStamp.Minute,
					this.LastTimeStamp.Second,
					this.LastTimeStamp.Millisecond);
			}
			#endregion
		}

		#endregion

		#region コントロールイベント: (コンボボックス)

		/// <summary>
		/// デバイス名称リストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.DeviceList.IsValid == false) return;
				this.Grabber.Dispose();

				comboPin.Items.Clear();
				var video = this.DeviceList[comboName.SelectedIndex];
				var names = video.GetPinNames();
				foreach (var item in names)
				{
					comboPin.Items.Add(item);
				}
				if (comboPin.Items.Count > 0)
					comboPin.SelectedIndex = 0;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// ピンリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPin_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.DeviceList.IsValid == false) return;
				this.Grabber.Dispose();

				comboSize.Items.Clear();
				var video = this.DeviceList[comboName.SelectedIndex];
				var pin = comboPin.SelectedIndex;
				var sizes = video.GetFrameSizes(pin);
				foreach (var item in sizes)
				{
					comboSize.Items.Add(string.Format("{0} x {1}", item.Width, item.Height));
				}
				if (comboSize.Items.Count > 0)
				{
					int index = 0;
					if (this.Param != null)
					{
						var prev_size = this.Param.Size;
						index = Array.FindIndex<XIE.TxSizeI>(sizes, 0,
							delegate(XIE.TxSizeI item)
							{
								return (prev_size == item);
							});
						if (index < 0)
							index = 0;
					}

					comboSize.SelectedIndex = index;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// フレームサイズリストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.DeviceList.IsValid == false) return;
				this.Grabber.Dispose();

				var video = this.DeviceList[comboName.SelectedIndex];
				var pin = comboPin.SelectedIndex;
				var sizes = video.GetFrameSizes(pin);
				var size = sizes[comboSize.SelectedIndex];

				var param = this.DeviceParam;
				param.Name = video.Name;
				param.Index = video.Index;
				param.Pin = pin;
				param.Size = size;
				this.Camera.Setup(param, null, null);
				this.Grabber = this.Camera.CreateGrabber();
				this.Grabber.Notify += Grabber_Notify;
				this.Grabber.Start();
				this.Camera.Start();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region コントロールイベント: (プロパティページ)

		/// <summary>
		/// プロパティページ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonProperty_Click(object sender, EventArgs e)
		{
			this.Camera.OpenPropertyDialog(this);
		}

		#endregion

		#region コントロールイベント: (開始/停止)

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStart_Click(object sender, EventArgs e)
		{
			try
			{
				this.Camera.Start();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPause_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.Grabber.IsRunning)
					this.Grabber.Stop();
				else
					this.Grabber.Start();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStop_Click(object sender, EventArgs e)
		{
			try
			{
				this.Camera.Stop();
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		#endregion
	}
}
