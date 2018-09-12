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
using System.Reflection;
using System.IO;

namespace XIEstudio
{
	/// <summary>
	/// 画像サムネイルフォーム
	/// </summary>
	public partial class CxImageThumbnailForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageThumbnailForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="nodes">画像ノードのコレクション</param>
		public CxImageThumbnailForm(List<CxImageNode> nodes)
		{
			InitializeComponent();

			foreach (var node in nodes)
				this.ImageNodes[node] = ExActionType.None;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像ノードのコレクション
		/// </summary>
		public Dictionary<CxImageNode, ExActionType> ImageNodes
		{
			get { return m_ImageNodes; }
			set { m_ImageNodes = value; }
		}
		private Dictionary<CxImageNode, ExActionType> m_ImageNodes = new Dictionary<CxImageNode, ExActionType>();

		/// <summary>
		/// アクション種別
		/// </summary>
		public enum ExActionType
		{
			/// <summary>
			/// なし
			/// </summary>
			None = 0x0,

			/// <summary>
			/// 閉じる
			/// </summary>
			Close = 0x1,

			/// <summary>
			/// 保存する
			/// </summary>
			Save = 0x2,

			/// <summary>
			/// 保存して閉じる
			/// </summary>
			SaveAndClose = 0x3,
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageThumbnailForm_Load(object sender, EventArgs e)
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

			listThumbnail.View = View.Details;
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
				int small_denom = 4;
				int dst_width = 256;
				int dst_height = 256;

				listThumbnail.Items.Clear();

				imageListThumbnailL.Images.Clear();
				imageListThumbnailL.ColorDepth = ColorDepth.Depth24Bit;
				imageListThumbnailL.ImageSize = new Size(dst_width, dst_height);

				imageListThumbnailS.Images.Clear();
				imageListThumbnailS.ColorDepth = ColorDepth.Depth24Bit;
				imageListThumbnailS.ImageSize = new Size(dst_width / small_denom, dst_height / small_denom);

				var failures = new List<CxImageNode>();
				int imageIndex = 0;
				foreach (var item in this.ImageNodes)
				{
					try
					{
						var node = item.Key;
						var type = item.Value;
						var image = node.Data;

						System.Drawing.Bitmap bitmapL = null;
						System.Drawing.Bitmap bitmapS = null;

						#region 表示倍率の調整:
						if (image == null || image.IsValid == false)
						{
							bitmapL = new Bitmap(dst_width, dst_height);
							bitmapS = new Bitmap(dst_width / small_denom, dst_height / small_denom);
						}
						else
						{
							using (var src = new XIE.CxImage())
							using (var dst = new XIE.CxImage())
							using (var act = new XIE.CxImage())
							{
								float mag_x = (float)dst_width / image.Width;
								float mag_y = (float)dst_height / image.Height;
								float mag = System.Math.Min(mag_x, mag_y);

								var src_width = image.Width;
								var src_height = image.Height;
								var src_model = XIE.TxModel.U8(image.Model.Pack);
								var src_channels = System.Math.Min(3, image.Channels);

								#region 入力画像の準備:
								{
									var compatible = false;
									var range = XIE.Axi.CalcRange(image.Model.Type, image.Depth);
									if (range.Lower == 0 && range.Upper == 255)
									{
										if (image.Model == XIE.TxModel.U8(1))
										{
											if (image.Channels == 1 || image.Channels == 3)
												compatible = true;
										}
										else if (image.Model == XIE.TxModel.U8(3) || image.Model == XIE.TxModel.U8(4))
										{
											if (image.Channels == 1)
												compatible = true;
										}
									}

									if (compatible)
										src.Attach(image);
									else
									{
										src.Resize(src_width, src_height, src_model, src_channels);
										var scale = XIE.Axi.CalcScale(image.Model.Type, image.Depth, src.Model.Type, src.Depth);
										src.Filter().Copy(image, scale);
									}
								}
								#endregion

								#region サムネイル画像の生成:
								{
									dst.Resize(dst_width, dst_height, src_model, src_channels);
									dst.Reset();
									int act_width = (int)(src_width * mag);
									int act_height = (int)(src_height * mag);
									var window = new XIE.TxRectangleI((dst_width - act_width) / 2, (dst_height - act_height) / 2, act_width, act_height);
									act.Attach(dst, window);
									act.Filter().Scale(src, mag, mag, (mag < 1) ? 0 : 1);
									int depth = act.CalcDepth(-1);
									if (!(2 <= depth && depth <= 8))
										dst.Depth = depth;
									bitmapL = dst.ToBitmap();

									act.Dispose();
									act.Filter().Scale(dst, 1.0 / small_denom, 1.0 / small_denom, 2);
									bitmapS = act.ToBitmap();
								}
								#endregion
							}
						}
						#endregion

						#region リストビューへの追加:
						if (bitmapL != null && bitmapS != null)
						{
							imageListThumbnailL.Images.Add(bitmapL);
							imageListThumbnailS.Images.Add(bitmapS);

							var lvitem = new ListViewItem(node.Name, imageIndex);
							lvitem.SubItems.Add(type.ToString());
							if (image == null)
							{
								lvitem.SubItems.Add("(null)");
							}
							else
							{
								lvitem.SubItems.Add(
									string.Format("{0}x{1} {2}ch {3}x{4} {5}",
										image.Width,
										image.Height,
										image.Channels,
										image.Model.Type,
										image.Model.Pack,
										((image.Depth == 0) ? "" : string.Format("({0}bits)", image.Depth))
										)
									);
							}
							lvitem.SubItems.Add(node.Info.FileName);

							listThumbnail.Items.Add(lvitem);
						}
						else
						{
							failures.Add(item.Key);
						}
						#endregion

						imageIndex++;
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.Message);

						failures.Add(item.Key);
					}
				}

				// リストビューに表示されていない項目を取り除く.
				foreach(var item in failures)
					this.ImageNodes.Remove(item);
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message);
			}
			#endregion
		}

		#endregion

		#region toolbar: (OK/Cancel)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region toolbar: (アクション関連)

		/// <summary>
		/// None が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolActionNone_Click(object sender, EventArgs e)
		{
			int count = 0;
			var nodes = ImageNodes.Keys.ToList<CxImageNode>();
			for (int i = 0; i < listThumbnail.Items.Count; i++)
			{
				if (listThumbnail.Items[i].Selected)
				{
					var node = nodes[i];
					var type = ExActionType.None;
					ImageNodes[node] = type;
					listThumbnail.Items[i].SubItems[1].Text = type.ToString();
					count++;
				}
			}
			if (count == 0)
			{
				MessageBox.Show(this, "Node is not selected.\nPlease select one or more nodes.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>
		/// Save が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolActionSave_Click(object sender, EventArgs e)
		{
			int count = 0;
			var nodes = ImageNodes.Keys.ToList<CxImageNode>();
			for (int i = 0; i < listThumbnail.Items.Count; i++)
			{
				if (listThumbnail.Items[i].Selected)
				{
					var node = nodes[i];
					var type = ExActionType.Save | ImageNodes[node];
					ImageNodes[node] = type;
					listThumbnail.Items[i].SubItems[1].Text = type.ToString();
					count++;
				}
			}
			if (count == 0)
			{
				MessageBox.Show(this, "Node is not selected.\nPlease select one or more nodes.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>
		/// Close が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolActionClose_Click(object sender, EventArgs e)
		{
			int count = 0;
			var nodes = ImageNodes.Keys.ToList<CxImageNode>();
			for (int i = 0; i < listThumbnail.Items.Count; i++)
			{
				if (listThumbnail.Items[i].Selected)
				{
					var node = nodes[i];
					var type = ExActionType.Close | ImageNodes[node];
					ImageNodes[node] = type;
					listThumbnail.Items[i].SubItems[1].Text = type.ToString();
					count++;
				}
			}
			if (count == 0)
			{
				MessageBox.Show(this, "Node is not selected.\nPlease select one or more nodes.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		#endregion

		#region toolbar: (リストビュー関連)

		/// <summary>
		/// リストビューの表示種別切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewType_Click(object sender, EventArgs e)
		{
			if (listThumbnail.View != View.LargeIcon)
				listThumbnail.View = View.LargeIcon;
			else
				listThumbnail.View = View.Details;
		}

		/// <summary>
		/// リストビュー項目幅の調整
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAdjustWidth_Click(object sender, EventArgs e)
		{
			foreach (ColumnHeader header in listThumbnail.Columns)
				header.Width = -2;
		}

		#endregion

		#region キーボードイベント:

		/// <summary>
		/// キーボードが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listThumbnail_KeyDown(object sender, KeyEventArgs e)
		{
			#region [CTRL+A]: 全選択
			if (e.KeyCode == Keys.A && !e.Alt && e.Control && !e.Shift)
			{
				for (int i = 0; i < listThumbnail.Items.Count; i++)
				{
					listThumbnail.Items[i].Selected = true;
				}
			}
			#endregion

			#region [CTRL+R]: 選択された項目のアクション (None)
			if (e.KeyCode == Keys.R && !e.Alt && e.Control && !e.Shift)
			{
				toolActionNone_Click(sender, e);
			}
			#endregion

			#region [CTRL+S]: 選択された項目のアクション (Save)
			if (e.KeyCode == Keys.S && !e.Alt && e.Control && !e.Shift)
			{
				toolActionSave_Click(sender, e);
			}
			#endregion

			#region [CTRL+D]: 選択された項目のアクション (Close)
			if (e.KeyCode == Keys.D && !e.Alt && e.Control && !e.Shift)
			{
				toolActionClose_Click(sender, e);
			}
			#endregion
		}

		#endregion
	}
}
