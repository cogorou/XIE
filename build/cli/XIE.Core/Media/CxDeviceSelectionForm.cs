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
	/// デバイス選択フォーム
	/// </summary>
	public partial class CxDeviceSelectionForm : Form
	{
		#region 初期化と解放:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		public CxDeviceSelectionForm(ExMediaType type, ExMediaDir dir)
		{
			InitializeComponent();
			this.MediaType = type;
			this.MediaDir = dir;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		/// <param name="param">デバイスパラメータ</param>
		public CxDeviceSelectionForm(ExMediaType type, ExMediaDir dir, CxDeviceParam param)
		{
			InitializeComponent();
			this.MediaType = type;
			this.MediaDir = dir;
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
		private void CxDeviceSelectionForm_Load(object sender, EventArgs e)
		{
			try
			{
				if (this.Param != null)
					this.DeviceParam.CopyFrom(this.Param);

				// デバイスリストの初期化.
				this.DeviceList.Setup(this.MediaType, this.MediaDir);
				this.comboName.Items.Add("(None)");
				foreach (var item in this.DeviceList)
					this.comboName.Items.Add(item.GetProductName());

				{
					int selected_index = 0;

					// 初期状態でデバイス名称リストを選択するか否か.
					if (this.Param != null &&
						this.Param.Name != null &&
						this.DeviceList.IsValid)
					{
						int index = this.DeviceList.FindIndex(this.Param.Name, this.Param.Index);
						if (0 <= index && index < comboName.Items.Count - 1)
							selected_index = index + 1;
					}

					comboName.SelectedIndex = selected_index;
				}
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
		private void CxDeviceSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
			DeviceList.Dispose();
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

		#region プロパティ:

		/// <summary>
		/// メディア種別
		/// </summary>
		public ExMediaType MediaType
		{
			get { return m_MediaType; }
			set { m_MediaType = value; }
		}
		private ExMediaType m_MediaType = ExMediaType.None;

		/// <summary>
		/// メディア方向
		/// </summary>
		public ExMediaDir MediaDir
		{
			get { return m_MediaDir; }
			set { m_MediaDir = value; }
		}
		private ExMediaDir m_MediaDir = ExMediaDir.None;

		/// <summary>
		/// デバイスパラメータ
		/// </summary>
		public CxDeviceParam Param
		{
			get { return m_Param; }
			set { m_Param = value; }
		}
		private CxDeviceParam m_Param = null;

		/// <summary>
		/// デバイスリスト
		/// </summary>
		private CxDeviceList DeviceList = new CxDeviceList();

		/// <summary>
		/// デバイスパラメータ (バックアップ)
		/// </summary>
		private CxDeviceParam DeviceParam = new CxDeviceParam();

		#endregion

		#region 定期的な表示更新処理:

		/// <summary>
		/// 定期的な表示更新処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
		}

		#endregion

		#region コントロールイベント: (コンボボックス)

		/// <summary>
		///デバイス名称リストの指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.DeviceList.IsValid == false) return;

				this.DeviceParam.Name = null;
				this.DeviceParam.Index = 0;

				comboPin.Items.Clear();
				comboPin.Enabled = false;

				int index = this.comboName.SelectedIndex - 1;
				if (0 <= index && index < this.DeviceList.Length)
				{
					var device = this.DeviceList[index];

					{
						this.DeviceParam.Name = device.Name;
						this.DeviceParam.Index = device.Index;
					}

					if (this.MediaDir == ExMediaDir.Input)
					{
						var names = device.GetPinNames();
						foreach (var item in names)
						{
							comboPin.Items.Add(item);
						}

						int pin = this.DeviceParam.Pin;
						if (0 <= pin && pin < comboPin.Items.Count)
							comboPin.SelectedIndex = pin;
						comboPin.Enabled = true;
					}
				}
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
				int index = this.comboPin.SelectedIndex;
				if (0 <= index)
				{
					this.DeviceParam.Pin = index;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
	}
}
