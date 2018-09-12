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

namespace XIEstudio
{
	/// <summary>
	/// タスクフローサムネイルフォーム
	/// </summary>
	public partial class CxTaskflowThumbnailForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflowThumbnailForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="nodes">タスクノードのコレクション</param>
		public CxTaskflowThumbnailForm(List<CxTaskNode> nodes)
		{
			InitializeComponent();
			this.TaskNodes = nodes;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タスクノードのコレクション
		/// </summary>
		public List<CxTaskNode> TaskNodes
		{
			get { return m_TaskNodes; }
			set { m_TaskNodes = value; }
		}
		private List<CxTaskNode> m_TaskNodes = new List<CxTaskNode>();

		/// <summary>
		/// 選択された項目の指標 [-1:非選択、0~:選択]
		/// </summary>
		public int SelectedIndex
		{
			get { return m_SelectedIndex; }
			set { m_SelectedIndex = value; }
		}
		private int m_SelectedIndex = -1;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowThumbnailForm_Load(object sender, EventArgs e)
		{
			#region アイコン設定.
			// 
			// 詳細は XIEstudio に記載するコメントを参照されたい.
			// 現象)
			//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
			//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
			// 
			switch (System.Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					// 
					// 上記理由によりアイコンの設定は行わない.
					// 
					break;
				default:
					{
						var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
						var image = (Bitmap)aux.ImageList.Images["View-Thumbnail"];
						this.Icon = Icon.FromHandle(image.GetHicon());
					}
					break;
			}
			#endregion

			MakeThumbnail();
		}

		#endregion

		#region CxTaskflowThumbnailForm: (コントロールイベント)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowThumbnailForm_Resize(object sender, EventArgs e)
		{
		}

		#endregion

		#region listThumbnail: (コントロールイベント)

		/// <summary>
		/// listThumbnail: 項目がダブルクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listThumbnail_DoubleClick(object sender, EventArgs e)
		{
			for (int i = 0; i < listThumbnail.Items.Count; i++)
			{
				if (listThumbnail.Items[i].Selected)
				{
					this.SelectedIndex = i;
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}

		#endregion

		#region listThumbnail: (内部関数)

		/// <summary>
		/// サムネイル生成
		/// </summary>
		private void MakeThumbnail()
		{
			#region 描画:
			try
			{

				int width = 256;
				int height = width * 166 / 234;

				listThumbnail.Items.Clear();
				imageListThumbnail.ColorDepth = ColorDepth.Depth24Bit;
				imageListThumbnail.ImageSize = new Size(width, height);

				for (int i = 0; i < this.TaskNodes.Count; i++)
				{
					var rect = new Rectangle(0, 0, width, height);
					var bitmap = new System.Drawing.Bitmap(width, height);
					var graphics = Graphics.FromImage(bitmap);

					try
					{
						var node = this.TaskNodes[i];
						var taskflow = node.Taskflow;
						var scope = (XIE.Tasks.CxTaskflow)node.CurrentTaskflow;

						#region 表示倍率の調整:
						if (scope != null)
						{
							var sheet = node.Visualizer.GetSheetSize();
							rect.Size = sheet;

							float mag_x = (float)bitmap.Width / sheet.Width;
							float mag_y = (float)bitmap.Height / sheet.Height;
							float mag = System.Math.Min(mag_x, mag_y);

							graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
							graphics.ResetTransform();
							graphics.ScaleTransform(mag, mag);
						}
						#endregion

						#region 背景:
						using (var brush = new SolidBrush(node.Visualizer.SheetColor))
						{
							graphics.FillRectangle(brush, rect);
						}
						#endregion

						#region タスクユニットの描画:
						if (scope != null)
						{
							foreach (var task in scope.TaskUnits)
							{
								try
								{
									node.Visualizer.Render(graphics, task);
								}
								catch (Exception ex)
								{
									XIE.Log.Api.Error(ex.StackTrace);
								}
							}
						}
						#endregion

						#region リストビューへの追加:
						{
							imageListThumbnail.Images.Add(bitmap);

							var lvitem = new ListViewItem(node.Name, i);

							listThumbnail.Items.Add(lvitem);
						}
						#endregion
					}
					finally
					{
						graphics.Dispose();
					}
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message);
			}
			#endregion
		}

		#endregion
	}
}
