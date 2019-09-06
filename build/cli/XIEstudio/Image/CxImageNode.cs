/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.CodeDom;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// AUX ツリーノード (画像オブジェクト)
	/// </summary>
	public class CxImageNode : TreeNode
		, IDisposable
		, XIE.Tasks.IxAuxNode
		, XIE.Tasks.IxAuxNodeImageOut
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ補助関数
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageNode()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">データ情報</param>
		/// <param name="data">データオブジェクト</param>
		public CxImageNode(XIE.Tasks.CxImageInfo info, XIE.CxImage data)
		{
			InitializeComponent();
			this.Info = info;
			this.Data = data;

			if (string.IsNullOrEmpty(info.FileName) == false &&
				System.IO.File.Exists(info.FileName))
			{
				this.FileTime = System.IO.File.GetLastWriteTime(info.FileName);
			}
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxImageNode()
		{
			Dispose();
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// 画像オブジェクト: 画像オブジェクトの接続.
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxImageNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			var aux = (XIE.Tasks.IxAuxInfoImages)config;
			int count = aux.Infos.Length;
			var nodes = new CxImageNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					var node = new CxImageNode(aux.Infos[i], aux.Datas[i]);
					nodes[i] = node;
					node.AuxInfo = config;
					node.Setup();
				}
				catch (System.Exception ex)
				{
					Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion

			return nodes;
		}

		/// <summary>
		/// 切断
		/// </summary>
		/// <param name="config">外部機器情報</param>
		public virtual void Disconnect(XIE.Tasks.CxAuxInfo config)
		{
			this.Dispose();
			this.Remove();

			// AuxInfo からの除去.
			{
				var aux = (XIE.Tasks.IxAuxInfoImages)config;
				aux.Remove(this.Info);
			}
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			if (this.IsSetuped == true) return;
			this.IsSetuped = true;

			#region 表示名:
			{
				string name = (string.IsNullOrEmpty(this.Info.FileName) == false)
					? System.IO.Path.GetFileName(this.Info.FileName)
					: ApiHelper.MakeFileNameSuffix(DateTime.Now, true);

				this.Name = name;
				this.Text = name;
				this.ToolTipText = this.Info.FileName;
			}
			#endregion

			#region ビット深度調整:
			if (this.Data != null && this.Data.IsValid == true)
			{
				var depth = this.Data.CalcDepth(-1);
				if (1 < depth && depth < 8)
					depth = 8;
				if (depth == XIE.Axi.CalcDepth(this.Data.Model.Type))
					depth = 0;
				this.Data.Depth = depth;
			}
			#endregion

			this.Tick(this, EventArgs.Empty);
		}

		/// <summary>
		/// 初期化済みフラグ
		/// </summary>
		public virtual bool IsSetuped
		{
			get { return m_IsSetuped; }
			protected set { m_IsSetuped = value; }
		}
		private bool m_IsSetuped = false;

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			if (PropertyDialog != null)
				PropertyDialog.Close();
			PropertyDialog = null;
			IsSetuped = false;
		}

		#endregion

		#region IxAuxNode の実装:

		/// <summary>
		/// 外部機器情報 (通常は CxAuxInfo のインスタンスが設定されます)
		/// </summary>
		public virtual XIE.Tasks.CxAuxInfo AuxInfo
		{
			get;
			set;
		}

		/// <summary>
		/// ダブルクリックされたとき
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void DoubleClick(object sender, EventArgs e)
		{
			this.OpenPropertyDialog(null);
		}

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void Tick(object sender, EventArgs e)
		{
			#region アイコンの更新.
			string key = "Color-Black";

			if (this.Data != null)
			{
				switch (this.Data.Model.Type)
				{
					case XIE.ExType.U8:
					case XIE.ExType.U16:
					case XIE.ExType.U32:
					case XIE.ExType.U64:
						#region unsigned:
						if (this.Data.Channels == 1)
						{
							if (this.Data.Depth == 1)
								key = "Image-Bin";
							else if (this.Data.Model.Pack == 1)
								key = "Image-Gray";
							else if (this.Data.Model.Pack == 3 || this.Data.Model.Pack == 4)
								key = "Image-RGB";
						}
						else if (this.Data.Channels == 3 || this.Data.Channels == 4)
						{
							if (this.Data.Model.Pack == 1)
								key = "Image-Unpacked";
						}
						#endregion
						break;
					case XIE.ExType.S8:
					case XIE.ExType.S16:
					case XIE.ExType.S32:
					case XIE.ExType.S64:
					case XIE.ExType.F32:
					case XIE.ExType.F64:
						key = "Image-Signed";
						break;
					default:
						break;
				}
			}

			if (this.ImageKey != key)
				this.ImageKey = key;
			if (this.SelectedImageKey != key)
				this.SelectedImageKey = key;
			#endregion
		}

		#endregion

		#region IxAuxNodeImageOut の実装:

		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Draw(XIE.GDI.CxImageView view)
		{
			if (view.Image != this.Data)
			{
				view.Image = this.Data;
				view.Refresh();
			}
		}

		/// <summary>
		/// 描画イメージの解除
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Reset(XIE.GDI.CxImageView view)
		{
			if (ReferenceEquals(view.Image, this.Data))
				view.Image = null;
		}

		/// <summary>
		/// 描画処理 (描画中の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
		}

		/// <summary>
		/// 描画処理 (描画完了時の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
		}

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
		}

		/// <summary>
		/// 処理範囲
		/// </summary>
		public virtual XIE.TxRectangleI ROI
		{
			get { return m_ROI; }
			set { m_ROI = value; }
		}
		private XIE.TxRectangleI m_ROI = new XIE.TxRectangleI();

		#endregion

		#region プロパティ:

		/// <summary>
		/// データ情報
		/// </summary>
		public virtual XIE.Tasks.CxImageInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxImageInfo m_Info = new XIE.Tasks.CxImageInfo();

		/// <summary>
		/// データオブジェクト
		/// </summary>
		public virtual XIE.CxImage Data
		{
			get { return m_Data; }
			set { m_Data = value; }
		}
		private XIE.CxImage m_Data = null;

		/// <summary>
		/// 図形操作オーバレイ
		/// </summary>
		public virtual CxFigureOverlay Overlay
		{
			get { return m_Overlay; }
			set { m_Overlay = value; }
		}
		private CxFigureOverlay m_Overlay = new CxFigureOverlay();

		/// <summary>
		/// ペイントブラシツール
		/// </summary>
		public virtual CxPaintBrush PaintBrush
		{
			get { return m_PaintBrush; }
			set { m_PaintBrush = value; }
		}
		private CxPaintBrush m_PaintBrush = (CxPaintBrush)CxImageEditorForm.ImageEditorSettings.PaintBrush.Clone();

		/// <summary>
		/// ペイント水滴ツール
		/// </summary>
		public virtual CxPaintDrop PaintDrop
		{
			get { return m_PaintDrop; }
			set { m_PaintDrop = value; }
		}
		private CxPaintDrop m_PaintDrop = (CxPaintDrop)CxImageEditorForm.ImageEditorSettings.PaintDrop.Clone();

		/// <summary>
		/// ペイント平滑化ツール
		/// </summary>
		public virtual CxPaintSmoothing PaintSmoothing
		{
			get { return m_PaintSmoothing; }
			set { m_PaintSmoothing = value; }
		}
		private CxPaintSmoothing m_PaintSmoothing = (CxPaintSmoothing)CxImageEditorForm.ImageEditorSettings.PaintSmoothing.Clone();

		#endregion

		#region プロパティ: (TimeStamp)

		/// <summary>
		/// ファイル日時
		/// </summary>
		public DateTime FileTime
		{
			get;
			set;
		}

		/// <summary>
		/// 更新日時
		/// </summary>
		public DateTime UpdatedTime
		{
			get;
			set;
		}

		/// <summary>
		/// 未保存のデータの存在の有無を取得します。
		/// </summary>
		public virtual bool UnsavedDataExists
		{
			get
			{
				if (this.Data == null) return false;
				if (string.IsNullOrEmpty(this.Info.FileName) == true) return true;
				if (System.IO.File.Exists(this.Info.FileName) == false) return true;
				DateTime filetime = System.IO.File.GetLastWriteTime(this.Info.FileName);
				return (filetime < this.UpdatedTime);
			}
		}

		#endregion

		// 
		// UI 関連
		// 

		#region 選択ダイアログ:

		/// <summary>
		/// 選択ダイアログの表示
		/// </summary>
		/// <param name="owner">ダイアログのオーナーフォーム</param>
		/// <param name="aux_info">外部機器情報</param>
		/// <param name="options">追加の引数 (通常は指定不要です。)</param>
		/// <returns>
		///		OK または Cancel を返します。
		/// </returns>
		public virtual DialogResult OpenSelectDialog(Form owner, XIE.Tasks.CxAuxInfo aux_info, params object[] options)
		{
			DialogResult result = DialogResult.Cancel;

			return result;
		}

		#endregion

		#region プロパティダイアログ:

		/// <summary>
		/// プロパティダイアログ
		/// </summary>
		public virtual Form PropertyDialog
		{
			get { return m_PropertyDialog; }
			set { m_PropertyDialog = value; }
		}
		private Form m_PropertyDialog = null;

		/// <summary>
		/// プロパティダイアログの生成
		/// </summary>
		/// <param name="owner">ダイアログのオーナーフォーム</param>
		/// <param name="options">追加の引数 (通常は指定不要です。)</param>
		/// <returns>
		///		生成されたフォームを返します。
		/// </returns>
		public virtual Form OpenPropertyDialog(Form owner, params object[] options)
		{
			if (PropertyDialog == null)
			{
				if (this.Data == null)
				{
					MessageBox.Show(Form.ActiveForm, "Data is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				}

				PropertyDialog = new CxImageEditorForm(this);
				PropertyDialog.Owner = owner;
				PropertyDialog.StartPosition = FormStartPosition.Manual;
				PropertyDialog.Location = ApiHelper.GetNeighborPosition(PropertyDialog.Size);
				PropertyDialog.FormClosed += delegate(object _sender, FormClosedEventArgs _e)
				{
					PropertyDialog = null;
				};
				PropertyDialog.Show();
			}
			else if (PropertyDialog.WindowState == FormWindowState.Minimized)
			{
				PropertyDialog.WindowState = FormWindowState.Normal;
			}
			else
			{
				PropertyDialog.Focus();
			}
			return PropertyDialog;
		}

		#endregion

		#region ファイルアクセス:

		/// <summary>
		/// ダイアログを表示して保存処理を行います。
		/// </summary>
		/// <param name="owner">オーナーフォーム</param>
		/// <returns>
		///		ユーザーの応答を返します。
		/// </returns>
		public DialogResult OpenSaveDialog(Form owner)
		{
			var node = this;

			#region SaveFileDialog
			var dlg = new SaveFileDialog();
			{
				string filename = node.Name;
				{
					bool support_all = false;
					bool support_png = false;
					bool support_tif = false;
					switch (node.Data.Model.Type)
					{
						case XIE.ExType.U8:
							switch (node.Data.Model.Pack)
							{
								case 1:
									support_all = (node.Data.Channels == 1 || node.Data.Channels == 3 || node.Data.Channels == 4);
									break;
								case 3:
								case 4:
									support_all = (node.Data.Channels == 1);
									break;
							}
							break;
						case XIE.ExType.U16:
							switch (node.Data.Model.Pack)
							{
								case 1:
									support_png = (node.Data.Channels == 1 || node.Data.Channels == 3 || node.Data.Channels == 4);
									break;
								case 3:
								case 4:
									support_png = (node.Data.Channels == 1);
									break;
							}
							break;
						case XIE.ExType.S16:
						case XIE.ExType.F32:
						case XIE.ExType.F64:
							switch (node.Data.Model.Pack)
							{
								case 1:
									support_tif = (node.Data.Channels == 1 || node.Data.Channels == 3 || node.Data.Channels == 4);
									break;
								case 3:
								case 4:
									support_tif = (node.Data.Channels == 1);
									break;
							}
							break;
						default:
							break;
					}

					if (support_all)
					{
						if (node.ExifIsValid())
							filename += ".jpg";
						else
							filename += ".png";
						dlg.Filter = "Image files (*.bmp;*.dib;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.raw)|*.bmp;*.dib;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.raw";
						dlg.Filter += "|Bitmap files (*.bmp;*.dib)|*.bmp;*.dib";
						dlg.Filter += "|Jpeg files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
						dlg.Filter += "|Png files (*.png)|*.png";
						dlg.Filter += "|Tiff files (*.tif;*.tiff)|*.tif;*.tiff";
						dlg.Filter += "|Raw files (*.raw)|*.raw";
					}
					else if (support_png)
					{
						filename += ".png";
						dlg.Filter = "Image files (*.png;*.tif;*.tiff;*.raw)|*.png;*.tif;*.tiff;*.raw";
						dlg.Filter += "|Png files (*.png)|*.png";
						dlg.Filter += "|Tiff files (*.tif;*.tiff)|*.tif;*.tiff";
						dlg.Filter += "|Raw files (*.raw)|*.raw";
					}
					else if (support_tif)
					{
						filename += ".png";
						dlg.Filter = "Image files (*.tif;*.tiff;*.raw)|*.tif;*.tiff;*.raw";
						dlg.Filter += "|Tiff files (*.tif;*.tiff)|*.tif;*.tiff";
						dlg.Filter += "|Raw files (*.raw)|*.raw";
					}
					else
					{
						filename += ".raw";
						dlg.Filter = "Raw files (*.raw)|*.raw";
					}
				}

				if (string.IsNullOrEmpty(node.Info.FileName) == false)
					filename = node.Info.FileName;

				dlg.OverwritePrompt = true;
				dlg.AddExtension = true;
				dlg.FileName = System.IO.Path.GetFileName(filename);

				var dir = System.IO.Path.GetDirectoryName(filename);
				if (string.IsNullOrWhiteSpace(dir) == false)
					dlg.InitialDirectory = dir;
				else if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.SaveDataDirectory) == false)
					dlg.InitialDirectory = CxAuxInfoForm.AppSettings.SaveDataDirectory.Trim();

				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			var ans = dlg.ShowDialog(owner);
			if (ans == DialogResult.OK)
			{
				#region 保存先ディレクトリ記録:
				if (node.Info.FileName != dlg.FileName)
				{
					CxAuxInfoForm.AppSettings.SaveDataDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
				}
				#endregion

				#region ファイル保存.
				try
				{
					string filename = dlg.FileName;

					node.Data.Save(filename);

					string name = System.IO.Path.GetFileName(filename);
					node.Name = name;
					node.Text = name;
					node.ToolTipText = dlg.FileName;
					node.Info.FileName = dlg.FileName;
					node.FileTime = System.IO.File.GetLastWriteTime(dlg.FileName);
					node.UpdatedTime = node.FileTime;
				}
				catch (System.Exception)
				{
				}
				#endregion
			}

			return ans;
		}

		#endregion

		#region Exif 関連:

		/// <summary>
		/// 画像オブジェクトの Exif データの有効性
		/// </summary>
		/// <returns>
		///		有効な場合は true 、それ以外は false を返します。
		/// </returns>
		public unsafe bool ExifIsValid()
		{
			if (this.Data == null) return false;
			var exif = this.Data.Exif();
			return exif.IsValid;
		}

		#endregion

		#region 編集履歴:

		/// <summary>
		/// 編集履歴: Undo Buffer
		/// </summary>
		private List<object> HistoryUndos = new List<object>();

		/// <summary>
		/// 編集履歴: Redo Buffer
		/// </summary>
		private List<object> HistoryRedos = new List<object>();

		/// <summary>
		/// 編集履歴: 追加
		/// </summary>
		/// <param name="data_type">データ種別 [0:画像、1:オーバレイ]</param>
		/// <param name="update_time">タイムスタンプを更新するか否か</param>
		public void AddHistory(int data_type, bool update_time)
		{
			switch (data_type)
			{
				default:
					return;
				case 0:
					this.HistoryUndos.Add(this.Data.Clone());
					break;
				case 1:
					this.HistoryUndos.Add(this.Overlay.Clone());
					break;
			}
			foreach (var item in this.HistoryRedos)
				((IDisposable)item).Dispose();
			this.HistoryRedos.Clear();
			if (update_time)
				this.UpdatedTime = DateTime.Now;
		}

		/// <summary>
		/// 編集履歴: 戻す事が可能か否か
		/// </summary>
		public bool CanUndo()
		{
			return (0 < this.HistoryUndos.Count);
		}

		/// <summary>
		/// 編集履歴: 進む事が可能か否か
		/// </summary>
		public bool CanRedo()
		{
			return (0 < this.HistoryRedos.Count);
		}

		/// <summary>
		/// 編集履歴: 戻す
		/// </summary>
		public void Undo()
		{
			if (this.HistoryUndos.Count == 0) return;
			using (var data = (IDisposable)this.HistoryUndos[this.HistoryUndos.Count - 1])
			{
				if (data is XIE.CxImage)
				{
					this.HistoryRedos.Add(this.Data.Clone());
					this.Data.CopyFrom(data);
				}
				else if (data is CxFigureOverlay)
				{
					this.HistoryRedos.Add(this.Overlay.Clone());
					this.Overlay.CopyFrom(data);
				}
				this.HistoryUndos.Remove(data);
			}
		}

		/// <summary>
		/// 編集履歴: 進む
		/// </summary>
		public void Redo()
		{
			if (this.HistoryRedos.Count == 0) return;
			using (var data = (IDisposable)this.HistoryRedos[this.HistoryRedos.Count - 1])
			{
				if (data is XIE.CxImage)
				{
					this.HistoryUndos.Add(this.Data.Clone());
					this.Data.CopyFrom(data);
				}
				else if (data is CxFigureOverlay)
				{
					this.HistoryUndos.Add(this.Overlay.Clone());
					this.Overlay.CopyFrom(data);
				}
				this.HistoryRedos.Remove(data);
			}
		}

		#endregion
	}

	/// <summary>
	/// 図形操作オーバレイ
	/// </summary>
	[Serializable]
	public class CxFigureOverlay : System.Object
		, IDisposable
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxFigureOverlay()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 可視属性
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.Visible")]
		public bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}
		private bool m_Visible = true;

		/// <summary>
		/// 可視属性 (選択されているオーバレイ図形の選択マーク)
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.VisibleSelectionMark")]
		public bool VisibleSelectionMark
		{
			get { return m_VisibleSelectionMark; }
			set { m_VisibleSelectionMark = value; }
		}
		private bool m_VisibleSelectionMark = true;

		/// <summary>
		/// オーバレイ図形のスケーリングモード
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.ScalingMode")]
		public XIE.GDI.ExGdiScalingMode ScalingMode
		{
			get { return m_ScalingMode; }
			set { m_ScalingMode = value; }
		}
		private XIE.GDI.ExGdiScalingMode m_ScalingMode = XIE.GDI.ExGdiScalingMode.TopLeft;

		/// <summary>
		/// オーバレイ図形のコレクション
		/// </summary>
		[XmlIgnore]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.Figures")]
		public List<XIE.GDI.IxGdi2d> Figures
		{
			get { return m_Figuers; }
			set { m_Figuers = value; }
		}
		[NonSerialized]
		private List<XIE.GDI.IxGdi2d> m_Figuers = new List<XIE.GDI.IxGdi2d>();

		/// <summary>
		/// オーバレイ図形のコレクション (選択されているもの)
		/// </summary>
		[XmlIgnore]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.SelectedFigures")]
		public CxSelectedFigures SelectedFigures
		{
			get { return m_SelectedFigures; }
			set { m_SelectedFigures = value; }
		}
		[NonSerialized]
		private CxSelectedFigures m_SelectedFigures = new CxSelectedFigures();

		/// <summary>
		/// 最後に選択されたオーバレイ図形
		/// </summary>
		[XmlIgnore]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxFigureOverlay.SelectedFigure")]
		public XIE.GDI.IxGdi2d SelectedFigure
		{
			get { return m_SelectedFigure; }
			set { m_SelectedFigure = value; }
		}
		[NonSerialized]
		private XIE.GDI.IxGdi2d m_SelectedFigure = null;

		#endregion

		#region IDisposable の実装:

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			foreach(var figure in Figures)
			{
				if (figure is IDisposable)
					((IDisposable)figure).Dispose();
			}
			Figures.Clear();
			SelectedFigures.Dispose();
			SelectedFigure = null;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			object clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			this.Dispose();

			if (src is CxFigureOverlay)
			{
				#region 同一型:
				var _src = (CxFigureOverlay)src;

				this.Visible = _src.Visible;
				this.VisibleSelectionMark = _src.VisibleSelectionMark;
				this.ScalingMode = _src.ScalingMode;

				// prepare selected figures
				var indexes = new List<int>();
				foreach (var figure in this.SelectedFigures.Figures)
				{
					if (figure.Key is ICloneable)
					{
						int index = this.Figures.FindIndex((item) => { return ReferenceEquals(item, figure.Key); });
						if (index >= 0)
							indexes.Add(index);
					}
				}

				// clear
				this.Figures.Clear();
				this.SelectedFigures.Dispose();
				this.SelectedFigure = null;

				// deep copy
				foreach (var figure in _src.Figures)
				{
					if (figure is ICloneable)
					{
						var clone = (XIE.GDI.IxGdi2d)((ICloneable)figure).Clone();
						this.Figures.Add(clone);
					}
				}

				// restore selected figures
				foreach (var index in indexes)
				{
					if (index < _src.Figures.Count)
					{
						var figure = _src.Figures[index];
						this.SelectedFigures.Add(figure);
					}
				}

				return;
				#endregion
			}

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			return false;
		}

		#endregion

		#region 描画:

		/// <summary>
		/// 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.Visible == false) return;

			e.Canvas.DrawOverlay(this.Figures, this.ScalingMode);

			#region 選択マークの描画:
			if (this.VisibleSelectionMark && !this.SelectedFigures.IsEmpty)
			{
				var figures = new List<XIE.GDI.IxGdi2d>();

				foreach (var item in this.SelectedFigures.Figures)
				{
					var bounds = item.Key.Bounds;
					var angle = item.Key.Angle;
					var axis = item.Key.Location + item.Key.Axis - bounds.Location;

					{
						var figure = new XIE.GDI.CxGdiRectangle(bounds);
						figure.Angle = angle;
						figure.Axis = axis;
						figure.PenColor = Color.Black;
						figure.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
						figure.PenWidth = 0;
						figures.Add(figure);
					}
					{
						var figure = new XIE.GDI.CxGdiRectangle(bounds);
						figure.Angle = angle;
						figure.Axis = axis;
						figure.PenColor = Color.White;
						figure.PenStyle = XIE.GDI.ExGdiPenStyle.Dot;
						figure.PenWidth = 0;
						figures.Add(figure);
					}
				}

				e.Canvas.DrawOverlay(figures, this.ScalingMode);

				foreach (var figure in figures)
				{
					if (figure is IDisposable)
						((IDisposable)figure).Dispose();
				}
			}
			#endregion
		}

		#endregion

		#region 操作:

		/// <summary>
		/// 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			Keys keys = e.Keys;
			bool ctrl = (keys & Keys.Control) == Keys.Control;
			bool shift = (keys & Keys.Shift) == Keys.Shift;
			bool alt = (keys & Keys.Alt) == Keys.Alt;

			double margin = 8 / e.Canvas.Magnification;

			switch(e.Reason)
			{
				default:
					break;
				case XIE.GDI.ExHandlingEventReason.MouseDown:
					if (e.IsGripped == false)
					{
						var mouse = (TxPointD)e.MouseEventArgs.Location;
						if (this.ScalingMode != XIE.GDI.ExGdiScalingMode.None)
							mouse = e.Canvas.DPtoIP(mouse, this.ScalingMode);

						#region 図形の選択:
						if (e.MouseEventArgs.Button == MouseButtons.Left)
						{
							if (this.HitFigure != null)
							{
								var figure = this.HitFigure;

								if (!ctrl && !shift && !alt)
								{
									if (this.BeginHandling != null)
										this.BeginHandling(sender, e);

									this.HandlingEventArgs = e;
									if (this.SelectedFigures.Exists(figure) == false)
									{
										this.SelectedFigures.Dispose();
										this.SelectedFigures.Add(figure);
										this.SelectedFigure = figure;
									}
									e.IsGripped = true;
									e.IsUpdated = true;
									break;
								}
								else if (ctrl && !shift && !alt)
								{
									if (this.BeginHandling != null)
										this.BeginHandling(sender, e);

									this.HandlingEventArgs = e;
									if (this.SelectedFigures.Exists(figure))
									{
										this.SelectedFigures.Remove(figure);
										this.SelectedFigure = null;
									}
									else
									{
										this.SelectedFigures.Add(figure);
										this.SelectedFigure = figure;
									}
									e.IsGripped = true;
									e.IsUpdated = true;
									break;
								}
							}
							if (e.IsGripped == false)
							{
								this.HandlingEventArgs = null;
								this.SelectedFigures.Dispose();
								this.SelectedFigure = null;
								e.IsUpdated = true;
							}
						}
						#endregion
					}
					break;
				case XIE.GDI.ExHandlingEventReason.MouseMove:
					this.HitEventArgs = null;
					this.HitFigure = null;
					if (e.IsGripped == false)
					{
						var mouse = (TxPointD)e.MouseEventArgs.Location;
						if (this.ScalingMode != XIE.GDI.ExGdiScalingMode.None)
							mouse = e.Canvas.DPtoIP(mouse, this.ScalingMode);

						if (this.HandlingEventArgs != null)
						{
							#region 選択されている図形の操作（移動または編集）:
							var prev_position = (TxPointD)this.HandlingEventArgs.MouseEventArgs.Location;
							if (this.ScalingMode != XIE.GDI.ExGdiScalingMode.None)
								prev_position = e.Canvas.DPtoIP(prev_position, this.ScalingMode);
							var move_position = mouse;

							if (this.SelectedFigures.Figures.Count == 1)
							{
								switch (this.HitPosition.Mode)
								{
									case 1: e.Cursor = Cursors.SizeAll; break;
									case 2: e.Cursor = Cursors.Cross; break;
								}

								foreach (var item in this.SelectedFigures.Figures)
								{
									item.Key.Modify(item.Value, prev_position, move_position, margin);
									break;
								}

								e.IsGripped = true;
								e.IsUpdated = true;
							}
							else if (this.SelectedFigures.Figures.Count > 1)
							{
								e.Cursor = Cursors.Hand;

								var mv = move_position - prev_position;
								foreach (var item in this.SelectedFigures.Figures)
								{
									item.Key.Location = item.Value.Location + mv;
								}

								e.IsGripped = true;
								e.IsUpdated = true;
							}
							#endregion
						}
						else
						{
							#region 図形の位置判定とカーソルの変更:
							double min_area = double.MaxValue;
							var figures = new List<XIE.GDI.IxGdi2d>(this.Figures);

							#region 選択された図形の判定:
							if (this.SelectedFigures.Figures.Count > 0)
							{
								foreach (var item in this.SelectedFigures.Figures)
								{
									var figure = item.Key;
									figures.Remove(figure);
								}
								foreach (var item in this.SelectedFigures.Figures)
								{
									var figure = item.Key;
									var hit = figure.HitTest(mouse, margin);
									if (hit.Mode != 0)
									{
										double area = 0;
										if (figure is XIE.GDI.CxGdiLine)
										{
											var vis = e.Canvas.VisibleRect();
											area = vis.Width * vis.Height;
										}
										else
										{
											var bounds = figure.Bounds;
											area = bounds.Width * bounds.Height;
										}
										if (min_area >= area)
										{
											min_area = area;
											this.HitEventArgs = e;
											this.HitFigure = figure;
											this.HitPosition = hit;
										}
									}
								}
							}
							#endregion

							#region 選択されていない図形の判定:
							if (this.HitFigure == null)
							{
								foreach (var figure in figures)
								{
									var hit = figure.HitTest(mouse, margin);
									if (hit.Mode != 0)
									{
										double area = 0;
										if (figure is XIE.GDI.CxGdiLine)
										{
											var vis = e.Canvas.VisibleRect();
											area = vis.Width * vis.Height;
										}
										else
										{
											var bounds = figure.Bounds;
											area = bounds.Width * bounds.Height;
										}
										if (min_area >= area)
										{
											min_area = area;
											this.HitEventArgs = e;
											this.HitFigure = figure;
											this.HitPosition = hit;
										}
									}
								}
							}
							#endregion

							if (this.HitFigure != null)
							{
								switch(this.HitPosition.Mode)
								{
									case 1: e.Cursor = Cursors.SizeAll; break;
									case 2: e.Cursor = Cursors.Cross; break;
								}
							}
							#endregion
						}
					}
					break;
				case XIE.GDI.ExHandlingEventReason.MouseUp:
					if (this.HandlingEventArgs != null)
					{
						if (e.IsGripped == false)
						{
							if (!this.SelectedFigures.IsEmpty)
								this.SelectedFigures.Update();
						}

						this.HandlingEventArgs = null;
						this.HitFigure = null;
						this.HitEventArgs = null;
					}
					break;
				case XIE.GDI.ExHandlingEventReason.MouseEnter:
					this.HandlingEventArgs = null;
					break;
				case XIE.GDI.ExHandlingEventReason.MouseLeave:
					this.HandlingEventArgs = null;
					break;
			}
		}

		/// <summary>
		/// マウス操作開始時のイベント引数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private XIE.GDI.CxHandlingEventArgs HandlingEventArgs = null;

		/// <summary>
		/// マウス位置判定時のイベント引数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private XIE.GDI.CxHandlingEventArgs HitEventArgs = null;

		/// <summary>
		/// マウス位置にある図形
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private XIE.GDI.IxGdi2d HitFigure = null;

		/// <summary>
		/// マウスが図形上のどの位置にあるかを示す位置情報
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private XIE.GDI.TxHitPosition HitPosition = new XIE.GDI.TxHitPosition();

		#endregion

		#region 操作前にコールバックされる関数:

		/// <summary>
		/// 操作前にコールバックされる関数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public XIE.GDI.CxHandlingEventHandler BeginHandling = null;

		#endregion
	}

	/// <summary>
	/// 選択されたオーバレイ図形のコレクション
	/// </summary>
	public class CxSelectedFigures : System.Object
		, IDisposable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSelectedFigures()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// オーバレイ図形のコレクション (Key=参照、Value=複製)
		/// </summary>
		[XmlIgnore]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxSelectedFigures.Figures")]
		public Dictionary<XIE.GDI.IxGdi2d, XIE.GDI.IxGdi2d> Figures
		{
			get { return m_Figuers; }
			set { m_Figuers = value; }
		}
		private Dictionary<XIE.GDI.IxGdi2d, XIE.GDI.IxGdi2d> m_Figuers = new Dictionary<XIE.GDI.IxGdi2d, XIE.GDI.IxGdi2d>();

		/// <summary>
		/// コレクションが空か否かの検査
		/// </summary>
		/// <returns>
		///		コレクションが空ならば true, それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		[Browsable(false)]
		public virtual bool IsEmpty
		{
			get { return (Figures.Count == 0); }
		}

		#endregion

		#region IDisposable の実装:

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			foreach (var figure in Figures)
			{
				if (figure.Value is IDisposable)
					((IDisposable)figure.Value).Dispose();
			}
			Figures.Clear();
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 存在するか否かの検査
		/// </summary>
		/// <param name="figure">対象のオーバレイ図形</param>
		/// <returns>
		///		指定された図形がコレクション内に存在すれば true, それ以外は false を返します。
		/// </returns>
		public virtual bool Exists(XIE.GDI.IxGdi2d figure)
		{
			foreach (var item in Figures.Keys)
			{
				if (ReferenceEquals(item, figure)) return true;
			}
			return false;
		}

		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="figure">対象のオーバレイ図形</param>
		public virtual void Add(XIE.GDI.IxGdi2d figure)
		{
			if (figure is ICloneable)
			{
				var value = (XIE.GDI.IxGdi2d)((ICloneable)figure).Clone();
				Figures.Add(figure, value);
			}
		}

		/// <summary>
		/// 除去
		/// </summary>
		/// <param name="figure">対象のオーバレイ図形</param>
		public virtual void Remove(XIE.GDI.IxGdi2d figure)
		{
			foreach (var item in Figures)
			{
				if (ReferenceEquals(item.Key, figure))
				{
					if (item.Value is IDisposable)
						((IDisposable)item.Value).Dispose();
					Figures.Remove(figure);
					return;
				}
			}
		}

		/// <summary>
		/// 更新
		/// </summary>
		public virtual void Update()
		{
			var figures = new List<XIE.GDI.IxGdi2d>();
			foreach (var item in this.Figures)
				figures.Add(item.Key);
			this.Dispose();
			foreach(var item in figures)
				this.Add(item);
		}

		#endregion
	}

	/// <summary>
	/// ペイントブラシツール
	/// </summary>
	[Serializable]
	public class CxPaintBrush : System.Object
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPaintBrush()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 描画色。画像を Shift+左クリック すると色を抽出します。
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintBrush.Color")]
		public TxRGB8x4 Color
		{
			get { return m_Color; }
			set { m_Color = value; }
		}
		private TxRGB8x4 m_Color = new TxRGB8x4(255, 0, 0);

		/// <summary>
		/// ブラシサイズ
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintBrush.BrushSize")]
		public TxSizeI BrushSize
		{
			get { return m_BrushSize; }
			set { m_BrushSize = value; }
		}
		private TxSizeI m_BrushSize = new TxSizeI(11, 11);

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			object clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			if (src is CxPaintBrush)
			{
				#region 同一型:
				var _src = (CxPaintBrush)src;

				this.Color = _src.Color;
				this.BrushSize = _src.BrushSize;

				return;
				#endregion
			}

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxPaintBrush)src;
				if (this.Color != _src.Color) return false;
				if (this.BrushSize != _src.BrushSize) return false;

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region 描画:

		/// <summary>
		/// 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			#region カーソル描画:
			{
				TxSizeD size = this.BrushSize;
				TxSizeD half = this.BrushSize / 2;
				var rect = new TxRectangleD(this.MousePosition - half, size);

				var cursor_fg = new XIE.GDI.CxGdiRectangle(rect);
				cursor_fg.PenColor = this.Color;
				cursor_fg.PenStyle = XIE.GDI.ExGdiPenStyle.Dash;

				var cursor_bg = new XIE.GDI.CxGdiRectangle(rect);
				cursor_bg.PenColor = new XIE.TxRGB8x4(
					(byte)(255 - this.Color.R),
					(byte)(255 - this.Color.G),
					(byte)(255 - this.Color.B)
					);

				e.Canvas.DrawOverlay(new XIE.GDI.IxGdi2d[] {cursor_bg, cursor_fg}, XIE.GDI.ExGdiScalingMode.TopLeft);
			}
			#endregion
		}
		[XmlIgnore]
		[NonSerialized]
		private TxPointD MousePosition = new TxPointD();

		#endregion

		#region 操作:

		/// <summary>
		/// 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (e.IsGripped == false)
			{
				// ※ TopLeft の場合は切り捨て、Center の場合は四捨五入する.
				var scaling_mode = XIE.GDI.ExGdiScalingMode.TopLeft;
				var mouse_position = e.Canvas.DPtoIP(e.MouseEventArgs.Location, scaling_mode);
				switch (scaling_mode)
				{
					case XIE.GDI.ExGdiScalingMode.TopLeft:
						mouse_position.X = System.Math.Floor(mouse_position.X);
						mouse_position.Y = System.Math.Floor(mouse_position.Y);
						break;
					case XIE.GDI.ExGdiScalingMode.Center:
						mouse_position.X = System.Math.Round(mouse_position.X);
						mouse_position.Y = System.Math.Round(mouse_position.Y);
						break;
				}
				this.MousePosition = mouse_position;

				Keys keys = e.Keys;
				bool ctrl = (keys & Keys.Control) == Keys.Control;
				bool shift = (keys & Keys.Shift) == Keys.Shift;
				bool alt = (keys & Keys.Alt) == Keys.Alt;

				{
					int mode = 0;

					#region 判定:
					switch (e.Reason)
					{
						case XIE.GDI.ExHandlingEventReason.MouseDown:
							switch (Control.MouseButtons)
							{
								case MouseButtons.Left:
									if (!ctrl && !shift && !alt)	// Left
									{
										mode = 1;
										this.IsPressed = true;
									}
									if (!ctrl && shift && !alt)		// Shift+Left
									{
										mode = 11;
									}
									break;
							}
							break;
						case XIE.GDI.ExHandlingEventReason.MouseUp:
							{
								this.IsPressed = false;
							}
							break;
						case XIE.GDI.ExHandlingEventReason.MouseMove:
							if (this.IsPressed)
							{
								switch (Control.MouseButtons)
								{
									case MouseButtons.Left:
										mode = 2;
										break;
									default:
										mode = 99;
										this.IsPressed = false;
										break;
								}
							}
							else
							{
								mode = 99;
							}
							break;
						default:
							break;
					}
					#endregion

					switch (mode)
					{
						case 1:
						case 2:
							#region 描画:
							if (e.Image != null && e.Image.IsValid)
							{
								TxSizeD size = this.BrushSize;
								TxSizeD half = this.BrushSize / 2;
								var rect = new TxRectangleD(this.MousePosition - half, size);

								#region 範囲チェック:
								int sx = (int)System.Math.Round(rect.X);
								int sy = (int)System.Math.Round(rect.Y);
								int ex = sx + this.BrushSize.Width;
								int ey = sy + this.BrushSize.Height;

								if (sx < 0)
									sx = 0;
								if (sy < 0)
									sy = 0;
								if (ex > e.Image.Width)
									ex = e.Image.Width;
								if (ey > e.Image.Height)
									ey = e.Image.Height;
								#endregion

								var roi = new TxRectangleI(sx, sy, ex - sx, ey - sy);
								if (roi.Width > 0 && roi.Height > 0)
								{
									if (mode == 1)
									{
										if (this.BeginHandling != null)
											this.BeginHandling(sender, e);
									}

									#region 塗り潰し:
									for (int ch = 0; ch < e.Image.Channels; ch++)
									{
										using (var child = e.Image.Child(ch, roi))
										{
											switch (child.Model.Pack)
											{
												case 1:
													if (e.Image.Channels == 1)
													{
														var value = this.Color.R * 0.299 + this.Color.G * 0.587 + this.Color.B * 0.114;
														child.Clear(value);
													}
													else
													{
														double value = 0;
														switch (ch)
														{
															case 0: value = this.Color.R; break;
															case 1: value = this.Color.G; break;
															case 2: value = this.Color.B; break;
															case 3: value = this.Color.A; break;
														}
														child.Clear(value);
													}
													break;
												case 3:
													{
														var value = (TxRGB8x3)this.Color;
														child.Clear(value);
													}
													break;
												case 4:
													{
														var value = (TxRGB8x3)this.Color;
														child.Clear(value);
													}
													break;
											}
										}
									}
									#endregion

									e.IsGripped = true;
									e.IsUpdated = true;	// 上位側へ表示更新を要求する.
								}
							}
							#endregion
							break;
						case 11:
							#region 色抽出:
							if (e.Image != null && e.Image.IsValid)
							{
								#region 範囲チェック:
								int mx = (int)System.Math.Round(this.MousePosition.X);
								int my = (int)System.Math.Round(this.MousePosition.Y);

								if (mx < 0)
									mx = 0;
								if (my < 0)
									my = 0;
								if (mx > e.Image.Width - 1)
									mx = e.Image.Width - 1;
								if (my > e.Image.Height - 1)
									my = e.Image.Height - 1;
								#endregion

								var scale = XIE.Axi.CalcScale(e.Image.Model.Type, e.Image.Depth, ExType.U8, 0);
								var current_color = this.Color;
								var picked_colors = new List<byte>();

								var channels = 0;
								var pack = 0;

								#region チャネル数とパック数の設定:
								switch (e.Image.Channels)
								{
									case 3:
									case 4:
										channels = 3;
										pack = 1;
										break;
									default:
										switch (e.Image.Model.Pack)
										{
											case 3:
											case 4:
												channels = 1;
												pack = 3;
												break;
											default:
												channels = 1;
												pack = 1;
												break;
										}
										break;
								}
								#endregion

								for (int ch = 0; ch < channels; ch++)
								{
									var scan = e.Image.Scanner(ch);
									var pixel = scan[my, mx];

									#region 画素の型による分岐:
									switch (e.Image.Model.Type)
									{
										case ExType.U8:
											unsafe
											{
												var addr = (byte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U16:
											unsafe
											{
												var addr = (ushort*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U32:
											unsafe
											{
												var addr = (uint*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U64:
											unsafe
											{
												var addr = (ulong*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S8:
											unsafe
											{
												var addr = (sbyte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S16:
											unsafe
											{
												var addr = (short*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S32:
											unsafe
											{
												var addr = (int*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S64:
											unsafe
											{
												var addr = (long*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F32:
											unsafe
											{
												var addr = (float*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F64:
											unsafe
											{
												var addr = (double*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
									}
									#endregion
								}

								#region 抽出した色の設定:
								if (picked_colors.Count > 0)
								{
									switch (picked_colors.Count)
									{
										case 3:
										case 4:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[1];
											current_color.B = picked_colors[2];
											break;
										default:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[0];
											current_color.B = picked_colors[0];
											break;
									}
									this.Color = current_color;
								}
								#endregion

								#region ImageEditorSettings への反映:
								{
									CxImageEditorForm.ImageEditorSettings.PaintBrush = (XIEstudio.CxPaintBrush)((ICloneable)this).Clone();
								}
								#endregion
							}
							#endregion
							break;
						case 99:
							e.IsUpdated = true;	// 上位側へ表示更新を要求する.
							break;
					}
				}
			}
		}

		/// <summary>
		/// マウスが押下されているか否か
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private bool IsPressed = false;

		#endregion

		#region 操作前にコールバックされる関数:

		/// <summary>
		/// 操作前にコールバックされる関数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public XIE.GDI.CxHandlingEventHandler BeginHandling = null;

		#endregion
	}

	/// <summary>
	/// ペイント水滴ツール
	/// </summary>
	[Serializable]
	public class CxPaintDrop : System.Object
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPaintDrop()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 描画色。画像を Shift+左クリック すると色を抽出します。
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintDrop.Color")]
		public TxRGB8x4 Color
		{
			get { return m_Color; }
			set { m_Color = value; }
		}
		private TxRGB8x4 m_Color = new TxRGB8x4(255, 0, 0);

		/// <summary>
		/// 水滴の浸透許容範囲 (%) [0~100] ※ ±(注目画素の輝度値×N％) の画素値を対象とします。Ctrl＋左クリックの場合は注目画素に連結する画素のみ対象とします。
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintDrop.ErrorRange")]
		public int ErrorRange
		{
			get { return m_ErrorRange; }
			set { m_ErrorRange = value; }
		}
		private int m_ErrorRange = 25;

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			object clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			if (src is CxPaintDrop)
			{
				#region 同一型:
				var _src = (CxPaintDrop)src;

				this.Color = _src.Color;
				this.ErrorRange = _src.ErrorRange;

				return;
				#endregion
			}

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxPaintDrop)src;
				if (this.Color != _src.Color) return false;
				if (this.ErrorRange != _src.ErrorRange) return false;

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region 描画:

		/// <summary>
		/// 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			#region カーソル描画:
			{
				var cursor_fg = new XIE.GDI.CxGdiPoint(this.MousePosition);
				cursor_fg.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Rectangle;
				cursor_fg.AnchorSize = new TxSizeD(0.5, 0.5);
				cursor_fg.PenColor = this.Color;

				var cursor_bg = new XIE.GDI.CxGdiPoint(this.MousePosition);
				cursor_bg.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Rectangle;
				cursor_bg.AnchorSize = new TxSizeD(0.5, 0.5);
				cursor_bg.PenColor = new XIE.TxRGB8x4(
					(byte)(255 - this.Color.R),
					(byte)(255 - this.Color.G),
					(byte)(255 - this.Color.B)
					);

				var around_fg = new XIE.GDI.CxGdiCircle(this.MousePosition, 1);
				around_fg.PenColor = this.Color;
				around_fg.PenStyle = XIE.GDI.ExGdiPenStyle.Dash;

				var around_bg = new XIE.GDI.CxGdiCircle(this.MousePosition, 1);
				around_bg.PenColor = new XIE.TxRGB8x4(
					(byte)(255 - this.Color.R),
					(byte)(255 - this.Color.G),
					(byte)(255 - this.Color.B)
					);

				if (e.Canvas.Magnification < double.Epsilon)
				{
					// ignore
				}
				else if (e.Canvas.Magnification < 8)
				{
					around_fg.Radius = 25 / e.Canvas.Magnification;
					around_bg.Radius = 25 / e.Canvas.Magnification;
					var figures = new XIE.GDI.IxGdi2d[] { cursor_bg, cursor_fg, around_bg, around_fg };
					e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.Center);
				}
				else
				{
					var figures = new XIE.GDI.IxGdi2d[] { cursor_bg, cursor_fg };
					e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.Center);
				}
			}
			#endregion
		}
		[XmlIgnore]
		[NonSerialized]
		private TxPointD MousePosition = new TxPointD();

		#endregion

		#region 操作:

		/// <summary>
		/// 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (e.IsGripped == false)
			{
				// ※ TopLeft の場合は切り捨て、Center の場合は四捨五入する.
				var scaling_mode = XIE.GDI.ExGdiScalingMode.Center;
				var mouse_position = e.Canvas.DPtoIP(e.MouseEventArgs.Location, scaling_mode);
				switch(scaling_mode)
				{
					case XIE.GDI.ExGdiScalingMode.TopLeft:
						mouse_position.X = System.Math.Floor(mouse_position.X);
						mouse_position.Y = System.Math.Floor(mouse_position.Y);
						break;
					case XIE.GDI.ExGdiScalingMode.Center:
						mouse_position.X = System.Math.Round(mouse_position.X);
						mouse_position.Y = System.Math.Round(mouse_position.Y);
						break;
				}
				this.MousePosition = mouse_position;

				Keys keys = e.Keys;
				bool ctrl = (keys & Keys.Control) == Keys.Control;
				bool shift = (keys & Keys.Shift) == Keys.Shift;
				bool alt = (keys & Keys.Alt) == Keys.Alt;

				{
					int mode = 0;

					#region 判定:
					switch (e.Reason)
					{
						case XIE.GDI.ExHandlingEventReason.MouseDown:
							switch (Control.MouseButtons)
							{
								case MouseButtons.Left:
									if (!shift && !alt)				// Left / Ctrl+Left
									{
										mode = 1;
									}
									if (!ctrl && shift && !alt)		// Shift+Left
									{
										mode = 11;
									}
									break;
							}
							break;
						case XIE.GDI.ExHandlingEventReason.MouseMove:
							mode = 99;
							break;
						default:
							break;
					}
					#endregion

					switch (mode)
					{
						case 1:
						case 2:
							#region 描画:
							if (e.Image != null && e.Image.IsValid)
							{
								var pos = (TxPointI)this.MousePosition;
								if (0 <= pos.X && pos.X < e.Image.Width &&
									0 <= pos.Y && pos.Y < e.Image.Height)
								{
									if (mode == 1)
									{
										if (this.BeginHandling != null)
											this.BeginHandling(sender, e);
									}

									#region 塗り潰し:
									try
									{
										var watch = new XIE.CxStopwatch();

										using (var mask = new CxImage())
										{
											#region マスク生成: (単純な２値化)
											using (var src = new CxImage())
											{
												watch.Start();

												#region アンパック.
												if (e.Image.Channels * e.Image.Model.Pack == 1)
												{
													src.Attach(e.Image);
												}
												else if (e.Image.Channels > 1 && e.Image.Model.Pack == 1)
												{
													src.Attach(e.Image);
												}
												else if (e.Image.Channels == 1 && e.Image.Model.Pack > 1)
												{
													var model = new XIE.TxModel(e.Image.Model.Type, 1);
													var channels = e.Image.Model.Pack;
													src.Resize(e.Image.Width, e.Image.Height, model, channels);
													src.Filter().Copy(e.Image);
												}
												#endregion

												#region ２値化:
												if (src.IsValid)
												{
													mask.Resize(src.Width, src.Height, TxModel.U8(1), 1);
													mask.Clear(1);

													for (int ch = 0; ch < src.Channels; ch++)
													{
														using (var child = src.Child(ch))
														{
															double src_value = 0;
															unsafe
															{
																var src_pixel = child[0, pos.Y, pos.X];
																switch (child.Model.Type)
																{
																	case ExType.U8: src_value = *(byte*)src_pixel; break;
																	case ExType.U16: src_value = *(ushort*)src_pixel; break;
																	case ExType.U32: src_value = *(uint*)src_pixel; break;
																	case ExType.U64: src_value = *(ulong*)src_pixel; break;
																	case ExType.S8: src_value = *(sbyte*)src_pixel; break;
																	case ExType.S16: src_value = *(short*)src_pixel; break;
																	case ExType.S32: src_value = *(int*)src_pixel; break;
																	case ExType.S64: src_value = *(long*)src_pixel; break;
																	case ExType.F32: src_value = *(float*)src_pixel; break;
																	case ExType.F64: src_value = *(double*)src_pixel; break;
																	default: break;
																}
															}
															var threshold = new TxRangeD(
																	src_value - this.ErrorRange,
																	src_value + this.ErrorRange
																);
															if (threshold.Lower > threshold.Upper)
															{
																var tmp = threshold.Lower;
																threshold.Upper = threshold.Lower;
																threshold.Lower = tmp;
															}

															// ２値化:
															using (var bin = new CxImage(child.Width, child.Height, TxModel.U8(1), 1))
															{
																var binarize = new XIE.Effectors.CxBinarize2(threshold, false, new TxRangeD(0, 1));
																binarize.Execute(child, bin);
																mask.Filter().And(mask, bin);

																//DEBUG
																//bin.Depth = 1;
																//bin.Save(string.Format("test.bin{0}.png", ch));
															}
														}
													}

													//DEBUG
													//mask.Depth = 1;
													//mask.Save("test.mask.png");
												}
												#endregion

												watch.Stop();
												//XIE.Log.Api.Trace("CxPaintDrop::Handling: {0,-15} = {1:F3} msec", "Binarize", watch.Lap);
											}
											#endregion

											#region マスク生成: (クラスタリング) [Ctrl+Left]
											if (ctrl)
											{
												watch.Start();
												if (mask.IsValid)
												{
													var clusters = CxRunCluster.Parse(mask);
													mask.Clear(0);

													foreach (var cluster in clusters)
													{
														if (cluster.Status < 0) continue;
														if (cluster.HitTest(pos.X, pos.Y))
														{
															unsafe
															{
																var scan = mask.Scanner(0);
																foreach (var run in cluster.Runs)
																{
																	var addr = (byte*)scan[run.Y, 0];
																	for (int x = run.X1; x <= run.X2; x++)
																	{
																		addr[x] = 1;
																	}
																}
															}
															break;
														}
													}
												}
												watch.Stop();
												//XIE.Log.Api.Trace("CxPaintDrop::Handling: {0,-15} = {1:F3} msec", "Clustering", watch.Lap);
											}
											#endregion

											#region マスク指定の塗り潰し:
											if (mask.IsValid)
											{
												watch.Start();

												if (e.Image.Channels * e.Image.Model.Pack == 1)
												{
													var value = this.Color.R * 0.299 + this.Color.G * 0.587 + this.Color.B * 0.114;
													e.Image.Clear(value, mask);
												}
												else if (e.Image.Channels > 1 && e.Image.Model.Pack == 1)
												{
													for (int ch = 0; ch < e.Image.Channels; ch++)
													{
														double value = 0;
														switch (ch)
														{
															case 0: value = this.Color.R; break;
															case 1: value = this.Color.G; break;
															case 2: value = this.Color.B; break;
															case 3: value = this.Color.A; break;
														}

														using (var child = e.Image.Child(ch))
														{
															child.Clear(value, mask);
														}
													}
												}
												else if (e.Image.Channels == 1 && e.Image.Model.Pack > 1)
												{
													var pack = e.Image.Model.Pack;
													for (int k = 0; k < pack; k++)
													{
														double value = 0;
														switch (k)
														{
															case 0: value = this.Color.R; break;
															case 1: value = this.Color.G; break;
															case 2: value = this.Color.B; break;
															case 3: value = this.Color.A; break;
														}

														e.Image.ClearEx(value, k, 1, mask);
													}
												}

												watch.Stop();
												//XIE.Log.Api.Trace("CxPaintDrop::Handling: {0,-15} = {1:F3} msec", "Painting", watch.Lap);
											}
											#endregion
										}
									}
									catch (System.Exception ex)
									{
										XIE.Log.Api.Error(ex.Message);
									}
									#endregion

									e.IsGripped = true;
									e.IsUpdated = true;	// 上位側へ表示更新を要求する.
								}
							}
							#endregion
							break;
						case 11:
							#region 色抽出:
							if (e.Image != null && e.Image.IsValid)
							{
								#region 範囲チェック:
								int mx = (int)System.Math.Round(this.MousePosition.X);
								int my = (int)System.Math.Round(this.MousePosition.Y);

								if (mx < 0)
									mx = 0;
								if (my < 0)
									my = 0;
								if (mx > e.Image.Width - 1)
									mx = e.Image.Width - 1;
								if (my > e.Image.Height - 1)
									my = e.Image.Height - 1;
								#endregion

								var scale = XIE.Axi.CalcScale(e.Image.Model.Type, e.Image.Depth, ExType.U8, 0);
								var current_color = this.Color;
								var picked_colors = new List<byte>();

								var channels = 0;
								var pack = 0;

								#region チャネル数とパック数の設定:
								switch (e.Image.Channels)
								{
									case 3:
									case 4:
										channels = 3;
										pack = 1;
										break;
									default:
										switch (e.Image.Model.Pack)
										{
											case 3:
											case 4:
												channels = 1;
												pack = 3;
												break;
											default:
												channels = 1;
												pack = 1;
												break;
										}
										break;
								}
								#endregion

								for (int ch = 0; ch < channels; ch++)
								{
									var pixel = e.Image[ch, my, mx];

									#region 画素の型による分岐:
									switch (e.Image.Model.Type)
									{
										case ExType.U8:
											unsafe
											{
												var addr = (byte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U16:
											unsafe
											{
												var addr = (ushort*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U32:
											unsafe
											{
												var addr = (uint*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U64:
											unsafe
											{
												var addr = (ulong*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S8:
											unsafe
											{
												var addr = (sbyte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S16:
											unsafe
											{
												var addr = (short*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S32:
											unsafe
											{
												var addr = (int*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S64:
											unsafe
											{
												var addr = (long*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F32:
											unsafe
											{
												var addr = (float*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F64:
											unsafe
											{
												var addr = (double*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
									}
									#endregion
								}

								#region 抽出した色の設定:
								if (picked_colors.Count > 0)
								{
									switch (picked_colors.Count)
									{
										case 3:
										case 4:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[1];
											current_color.B = picked_colors[2];
											break;
										default:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[0];
											current_color.B = picked_colors[0];
											break;
									}
									this.Color = current_color;
								}
								#endregion

								#region ImageEditorSettings への反映:
								{
									CxImageEditorForm.ImageEditorSettings.PaintDrop = (XIEstudio.CxPaintDrop)((ICloneable)this).Clone();
								}
								#endregion
							}
							#endregion
							break;
						case 99:
							e.IsUpdated = true;	// 上位側へ表示更新を要求する.
							break;
					}
				}
			}
		}

		#endregion

		#region 操作前にコールバックされる関数:

		/// <summary>
		/// 操作前にコールバックされる関数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public XIE.GDI.CxHandlingEventHandler BeginHandling = null;

		#endregion
	}

	/// <summary>
	/// ペイント平滑化ツール
	/// </summary>
	[Serializable]
	public class CxPaintSmoothing : System.Object
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPaintSmoothing()
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// フィルタサイズ [3 以上の奇数]
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintSmoothing.m_FilterSize")]
		public int FilterSize
		{
			get { return m_FilterSize; }
			set { m_FilterSize = value; }
		}
		private int m_FilterSize = 3;

		/// <summary>
		/// ブラシサイズ
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxPaintSmoothing.BrushSize")]
		public TxSizeI BrushSize
		{
			get { return m_BrushSize; }
			set { m_BrushSize = value; }
		}
		private TxSizeI m_BrushSize = new TxSizeI(11, 11);

		/// <summary>
		/// 描画色。画像を Shift+左クリック すると色を抽出します。
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxPaintSmoothing.Color")]
		public TxRGB8x4 Color
		{
			get { return m_Color; }
			set { m_Color = value; }
		}
		private TxRGB8x4 m_Color = new TxRGB8x4(255, 0, 0);

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			object clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			if (src is CxPaintSmoothing)
			{
				#region 同一型:
				var _src = (CxPaintSmoothing)src;

				this.FilterSize = _src.FilterSize;
				this.BrushSize = _src.BrushSize;
				this.Color = _src.Color;

				return;
				#endregion
			}

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxPaintSmoothing)src;
				if (this.FilterSize != _src.FilterSize) return false;
				if (this.BrushSize != _src.BrushSize) return false;
				if (this.Color != _src.Color) return false;

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region 描画:

		/// <summary>
		/// 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			#region カーソル描画:
			{
				TxSizeD size = this.BrushSize;
				TxSizeD half = this.BrushSize / 2;
				var rect = new TxRectangleD(this.MousePosition - half, size);

				var cursor_fg = new XIE.GDI.CxGdiRectangle(rect);
				cursor_fg.PenColor = this.Color;
				cursor_fg.PenStyle = XIE.GDI.ExGdiPenStyle.Dash;

				var cursor_bg = new XIE.GDI.CxGdiRectangle(rect);
				cursor_bg.PenColor = new XIE.TxRGB8x4(
					(byte)(255 - this.Color.R),
					(byte)(255 - this.Color.G),
					(byte)(255 - this.Color.B)
					);

				e.Canvas.DrawOverlay(new XIE.GDI.IxGdi2d[] { cursor_bg, cursor_fg }, XIE.GDI.ExGdiScalingMode.TopLeft);
			}
			#endregion
		}
		[XmlIgnore]
		[NonSerialized]
		private TxPointD MousePosition = new TxPointD();

		#endregion

		#region 操作:

		/// <summary>
		/// 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (e.IsGripped == false)
			{
				// ※ TopLeft の場合は切り捨て、Center の場合は四捨五入する.
				var scaling_mode = XIE.GDI.ExGdiScalingMode.TopLeft;
				var mouse_position = e.Canvas.DPtoIP(e.MouseEventArgs.Location, scaling_mode);
				switch (scaling_mode)
				{
					case XIE.GDI.ExGdiScalingMode.TopLeft:
						mouse_position.X = System.Math.Floor(mouse_position.X);
						mouse_position.Y = System.Math.Floor(mouse_position.Y);
						break;
					case XIE.GDI.ExGdiScalingMode.Center:
						mouse_position.X = System.Math.Round(mouse_position.X);
						mouse_position.Y = System.Math.Round(mouse_position.Y);
						break;
				}
				this.MousePosition = mouse_position;

				Keys keys = e.Keys;
				bool ctrl = (keys & Keys.Control) == Keys.Control;
				bool shift = (keys & Keys.Shift) == Keys.Shift;
				bool alt = (keys & Keys.Alt) == Keys.Alt;

				{
					int mode = 0;

					#region 判定:
					switch (e.Reason)
					{
						case XIE.GDI.ExHandlingEventReason.MouseDown:
							switch (Control.MouseButtons)
							{
								case MouseButtons.Left:
									if (!ctrl && !shift && !alt)	// Left
									{
										mode = 1;
										this.IsPressed = true;
									}
									if (!ctrl && shift && !alt)		// Shift+Left
									{
										mode = 11;
									}
									break;
							}
							break;
						case XIE.GDI.ExHandlingEventReason.MouseUp:
							{
								this.IsPressed = false;
							}
							break;
						case XIE.GDI.ExHandlingEventReason.MouseMove:
							if (this.IsPressed)
							{
								switch (Control.MouseButtons)
								{
									case MouseButtons.Left:
										mode = 2;
										break;
									default:
										mode = 99;
										this.IsPressed = false;
										break;
								}
							}
							else
							{
								mode = 99;
							}
							break;
						default:
							break;
					}
					#endregion

					switch (mode)
					{
						case 1:
						case 2:
							#region 描画:
							if (e.Image != null && e.Image.IsValid)
							{
								TxSizeD size = this.BrushSize;
								TxSizeD half = this.BrushSize / 2;
								var rect = new TxRectangleD(this.MousePosition - half, size);

								#region 範囲チェック:
								int sx = (int)System.Math.Round(rect.X);
								int sy = (int)System.Math.Round(rect.Y);
								int ex = sx + this.BrushSize.Width;
								int ey = sy + this.BrushSize.Height;

								if (sx < 0)
									sx = 0;
								if (sy < 0)
									sy = 0;
								if (ex > e.Image.Width)
									ex = e.Image.Width;
								if (ey > e.Image.Height)
									ey = e.Image.Height;
								#endregion

								var roi = new TxRectangleI(sx, sy, ex - sx, ey - sy);
								if (roi.Width > 0 && roi.Height > 0)
								{
									if (mode == 1)
									{
										if (this.BeginHandling != null)
											this.BeginHandling(sender, e);
									}

									int filter_size = System.Math.Abs(this.FilterSize / 2) * 2 + 1;
									if (filter_size < 3)
										filter_size = 3;

									#region 平滑化:
									// 
									// 　　┏━━━┓←── e.Image
									// 　　┃　　　┃
									// 　　┃　　　┃
									// 　　┗━━━┛
									// 　　↓ each channel
									// 　　┌───┐←── child
									// 　　│　　　│　　
									// 　　│　　　│　　
									// 　　└───┘　　
									// 　　│ unpack
									// ┏━━━━━━━┓← work (unpacked)
									// ┃　↓　　　　　┃
									// ┃　┏━━━┓←┃─ work_child (処理範囲部分)
									// ┃　┃　　　┃　┃
									// ┃　┃　　　┃　┃
									// ┃　┗━━━┛　┃
									// ┃　　　　　　　┃
									// ┗━━━━━━━┛
									// 　　↓ each channel
									// ┌───────┐← work_sum1
									// │　　　　　　　│
									// │　┌───┐←│─ (局所領域の注目画素が移動する範囲)
									// │　│　　　│　│
									// │　│　　　│　│
									// │　└───┘　│
									// │　│　　　　　│
									// └───────┘
									// 　　↓ 局所領域平均
									// 　　┏━━━┓←── work_buff
									// 　　┃　　　┃
									// 　　┃　　　┃
									// 　　┗━━━┛
									// 
									for (int ch = 0; ch < e.Image.Channels; ch++)
									{
										using (var child = e.Image.Child(ch, roi))
										using (var work = new CxImage())
										{
											// 作業用画像:
											work.Resize(
												child.Width + filter_size,
												child.Height + filter_size,
												new TxModel(child.Model.Type, 1),
												child.Model.Pack
												);
											// 処理範囲:
											var work_roi = new TxRectangleI(
												filter_size / 2 + 1,
												filter_size / 2 + 1,
												child.Width,
												child.Height
												);
											// 元画像を作業用画像に反映する.
											{
												// 1: 作業用画像の処理範囲部分に元画像を複製する.
												using (var work_child = work.Child(work_roi))
												{
													work_child.Filter().Copy(child);
												}
												// 2: 作業用画像のボーダー部分を塗り潰す.
												for (var k = 0; k < work.Channels; k++)
												{
													// 上辺.
													using (var work_edge = work.Child(new TxRectangleI(work_roi.X, work_roi.Y, work_roi.Width, 1)))
													{
														for (int y = 0; y < work_roi.Y; y++)
														{
															using (var work_border = work.Child(new TxRectangleI(work_roi.X, y, work_roi.Width, 1)))
															{
																work_border.Filter().Copy(work_edge);
															}
														}
													}
													// 下辺.
													using (var work_edge = work.Child(new TxRectangleI(work_roi.X, work_roi.Y + work_roi.Height - 1, work_roi.Width, 1)))
													{
														for (int y = work_roi.Y + work_roi.Height; y < work.Height; y++)
														{
															using (var work_border = work.Child(new TxRectangleI(work_roi.X, y, work_roi.Width, 1)))
															{
																work_border.Filter().Copy(work_edge);
															}
														}
													}
													// 左辺.
													using (var work_edge = work.Child(new TxRectangleI(work_roi.X, 0, 1, work.Height)))
													{
														for (int x = 0; x < work_roi.X; x++)
														{
															using (var work_border = work.Child(new TxRectangleI(x, 0, 1, work.Height)))
															{
																work_border.Filter().Copy(work_edge);
															}
														}
													}
													// 右辺.
													using (var work_edge = work.Child(new TxRectangleI(work_roi.X + work_roi.Width - 1, 0, 1, work.Height)))
													{
														for (int x = work_roi.X + work_roi.Width; x < work.Width; x++)
														{
															using (var work_border = work.Child(new TxRectangleI(x, 0, 1, work.Height)))
															{
																work_border.Filter().Copy(work_edge);
															}
														}
													}
												}
											}
											// 局所領域平均により平滑化した結果を元画像へ反映する.
											using (var work_buff = new CxImage(work_roi.Width, work_roi.Height, TxModel.F64(1), work.Channels))
											{
												#region 局所領域平均による平滑化:
												for (var k = 0; k < work_buff.Channels; k++)
												{
													using (var work_each = work.Child(k))
													using (var work_sum1 = new CxImage(work.Width, work.Height, XIE.TxModel.F64(1), 1))
													{
														// 積分画像:
														var effector = new XIE.Effectors.CxIntegral(1);	// [1=総和、2=２乗の総和]
														effector.Execute(work_each, work_sum1);

														// 局所領域平均:
														// 例)
														// ・注目画素 … [0,0] ※ 下図×部分
														// ・局所領域 … 始点=-1,-1、サイズ=3x3 (9画素)
														// ・平均 … ((11-10)-(01-00)) / 9
														// 
														// ┌─┬─┬─┬─┬───┐
														// │00│　│　│01│
														// ├─┌─┬─┬─┐
														// │　│　│　│　│
														// ├─├─┼─┼─┤← 元画像のＹ端
														// │　│　│×│　│
														// ├─├─┼─┼─┤
														// │10│　│　│11│
														// ├─└─┴─┴─┘
														// │　　　↑　　　　　　
														// │　　　└ 元画像のＸ端　　　　　　
														// └───────────┘
														// 
														var back_size = filter_size / 2 + 1;
														var next_size = filter_size / 2;
														var filter_pixels = filter_size * filter_size;
														var sum_scan = work_sum1.Scanner(0);
														var dst_scan = work_buff.Scanner(k);
														for (int y = 0; y < work_roi.Height; y++)
														{
															for (int x = 0; x < work_roi.Width; x++)
															{
																unsafe
																{
																	var sum00 = *((double*)sum_scan[work_roi.Y + y - back_size, work_roi.X + x - back_size].ToPointer());
																	var sum01 = *((double*)sum_scan[work_roi.Y + y - back_size, work_roi.X + x + next_size].ToPointer());
																	var sum10 = *((double*)sum_scan[work_roi.Y + y + next_size, work_roi.X + x - back_size].ToPointer());
																	var sum11 = *((double*)sum_scan[work_roi.Y + y + next_size, work_roi.X + x + next_size].ToPointer());
																	var mean = ((sum11 - sum10) - (sum01 - sum00)) / filter_pixels;
																	var dst_addr = (double*)dst_scan[y, x];
																	*dst_addr = mean;
																}
															}
														}
													}
												}
												#endregion

												// 元画像への反映:
												child.Filter().Copy(work_buff);
											}
										}
									}
									#endregion

									e.IsGripped = true;
									e.IsUpdated = true;	// 上位側へ表示更新を要求する.
								}
							}
							#endregion
							break;
						case 11:
							#region 色抽出:
							if (e.Image != null && e.Image.IsValid)
							{
								#region 範囲チェック:
								int mx = (int)System.Math.Round(this.MousePosition.X);
								int my = (int)System.Math.Round(this.MousePosition.Y);

								if (mx < 0)
									mx = 0;
								if (my < 0)
									my = 0;
								if (mx > e.Image.Width - 1)
									mx = e.Image.Width - 1;
								if (my > e.Image.Height - 1)
									my = e.Image.Height - 1;
								#endregion

								var scale = XIE.Axi.CalcScale(e.Image.Model.Type, e.Image.Depth, ExType.U8, 0);
								var current_color = this.Color;
								var picked_colors = new List<byte>();

								var channels = 0;
								var pack = 0;

								#region チャネル数とパック数の設定:
								switch (e.Image.Channels)
								{
									case 3:
									case 4:
										channels = 3;
										pack = 1;
										break;
									default:
										switch (e.Image.Model.Pack)
										{
											case 3:
											case 4:
												channels = 1;
												pack = 3;
												break;
											default:
												channels = 1;
												pack = 1;
												break;
										}
										break;
								}
								#endregion

								for (int ch = 0; ch < channels; ch++)
								{
									var scan = e.Image.Scanner(ch);
									var pixel = scan[my, mx];

									#region 画素の型による分岐:
									switch (e.Image.Model.Type)
									{
										case ExType.U8:
											unsafe
											{
												var addr = (byte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U16:
											unsafe
											{
												var addr = (ushort*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U32:
											unsafe
											{
												var addr = (uint*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.U64:
											unsafe
											{
												var addr = (ulong*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S8:
											unsafe
											{
												var addr = (sbyte*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S16:
											unsafe
											{
												var addr = (short*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S32:
											unsafe
											{
												var addr = (int*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.S64:
											unsafe
											{
												var addr = (long*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F32:
											unsafe
											{
												var addr = (float*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
										case ExType.F64:
											unsafe
											{
												var addr = (double*)pixel;
												for (int k = 0; k < pack; k++)
												{
													picked_colors.Add((byte)System.Math.Round(addr[k] * scale));
												}
											}
											break;
									}
									#endregion
								}

								#region 抽出した色の設定:
								if (picked_colors.Count > 0)
								{
									switch (picked_colors.Count)
									{
										case 3:
										case 4:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[1];
											current_color.B = picked_colors[2];
											break;
										default:
											current_color.R = picked_colors[0];
											current_color.G = picked_colors[0];
											current_color.B = picked_colors[0];
											break;
									}
									this.Color = current_color;
								}
								#endregion

								#region ImageEditorSettings への反映:
								{
									CxImageEditorForm.ImageEditorSettings.PaintSmoothing = (XIEstudio.CxPaintSmoothing)((ICloneable)this).Clone();
								}
								#endregion
							}
							#endregion
							break;
						case 99:
							e.IsUpdated = true;	// 上位側へ表示更新を要求する.
							break;
					}
				}
			}
		}

		/// <summary>
		/// マウスが押下されているか否か
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		private bool IsPressed = false;

		#endregion

		#region 操作前にコールバックされる関数:

		/// <summary>
		/// 操作前にコールバックされる関数
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public XIE.GDI.CxHandlingEventHandler BeginHandling = null;

		#endregion
	}
}
