/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XIEstudio
{
	/// <summary>
	/// Exif フォーム
	/// </summary>
	partial class CxExifForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="image">画像オブジェクト</param>
		public CxExifForm(XIE.CxImage image)
		{
			InitializeComponent();
			this.Image = image;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像オブジェクト
		/// </summary>
		public XIE.CxImage Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}
		private XIE.CxImage m_Image = null;

		/// <summary>
		/// オーナーフォームへの追従モード
		/// </summary>
		public bool IsDocking
		{
			get { return m_IsDocking; }
			set { m_IsDocking = value; }
		}
		private bool m_IsDocking = true;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_Load(object sender, EventArgs e)
		{
			UpdateInfo();
			toolDock.Checked = IsDocking;
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			var enable = (this.Image != null);
			var exists = (listInfo.Items.Count > 0);
			var stocked = ExifStock.IsValid;

			toolFileOpen.Enabled = enable;
			toolFileSave.Enabled = enable && exists;

			toolEditCut.Enabled = enable && exists;
			toolEditCopy.Enabled = enable && exists;
			toolEditPaste.Enabled = enable && stocked;
			toolEditClear.Enabled = enable && stocked;
		}

		#region メソッド:

		/// <summary>
		/// Exif の表示消去
		/// </summary>
		public void ClearInfo()
		{
			listInfo.Items.Clear();
		}

		/// <summary>
		/// Exif の表示更新
		/// </summary>
		public unsafe void UpdateInfo()
		{
			listInfo.Items.Clear();
			if (this.Image != null)
			{
				using (var exif = XIE.CxExif.FromTag(this.Image.Exif()))
				{
					#region リストビューの初期化:
					if (exif.IsValid)
					{
						var groupNames = new string[]
							{
								"TimeStamp",
								"Camera",
								"Exposure",
								"Image Settings",
								"Image Information",
								"Software",
								"Etc",
							};
						var groups = new Dictionary<string, ListViewGroup>();
						foreach (var name in groupNames)
						{
							var group = new ListViewGroup(name);
							groups[name] = group;
							listInfo.Groups.Add(group);
						}

						var exifitems = exif.GetItems();
						foreach (var item in exifitems)
						{
							var id = item.ID.ToString("X4");
							var type = item.Type.ToString();
							var name = GetNickName(item.ID, CxAuxInfoForm.AppSettings.ExifItemNameMode);
							var val = GetValueAsString(exif, item);

							var lvitem = new ListViewItem(id);
							lvitem.SubItems.Add(new ListViewItem.ListViewSubItem(lvitem, type));
							lvitem.SubItems.Add(new ListViewItem.ListViewSubItem(lvitem, name));
							lvitem.SubItems.Add(new ListViewItem.ListViewSubItem(lvitem, val));

							#region グルーピング:
							switch (item.ID)
							{
								case 0x132: // "変更日時"
								case 0x9003: // "撮影日時"
								case 0x9004: // "作成日時"
									groups["TimeStamp"].Items.Add(lvitem);
									break;
								case 0x10F: // "カメラのメーカー"
								case 0x110: // "カメラのモデル"
								case 0xA430: // "カメラ所有者"
								case 0xA431: // "カメラ製造番号"
								case 0xA432: // "レンズ仕様"
								case 0xA433: // "レンズメーカー"
								case 0xA434: // "レンズモデル"
								case 0xA435: // "レンズ製造番号"
								case 0x920A: // "焦点距離"
								case 0x9206: // "被写体距離"
								case 0x9214: // "被写体面積"
								case 0xA214: // "被写体位置"
								case 0xA404: // "デジタルズーム"
								case 0xA405: // "焦点距離（35mm 換算）"
									groups["Camera"].Items.Add(lvitem);
									break;
								case 0x829A: // "露光時間"
								case 0x829D: // "F 値"
								case 0x8822: // "露出モード"
								case 0x8824: // "分光感度"
								case 0x8827: // "ISO 感度設定"
								case 0x8828: // "光電変換係数"
								case 0x8830: // "感度種別"
								case 0x8831: // "標準出力感度"
								case 0x8832: // "推奨露光指数"
								case 0x8833: // "ISO 感度"
								case 0x8834: // "ISO 感度 緯度 yyy"
								case 0x8835: // "ISO 感度 緯度 zzz"
								case 0x9201: // "シャッタースピード"
								case 0x9202: // "絞り値"
								case 0x9203: // "明るさ"
								case 0x9204: // "露出補正"
								case 0x9205: // "最大絞り"
								case 0x9207: // "測光モード"
								case 0x9208: // "光源"
								case 0x9209: // "フラッシュ"
								case 0xA215: // "露光指標"
								case 0xA217: // "センサー方式"
								case 0xA402: // "露光モード"
									groups["Exposure"].Items.Add(lvitem);
									break;
								case 0xA001: // "色空間情報"
								case 0xA301: // "シーン種別"
								case 0xA401: // "カスタムイメージ処理"
								case 0xA403: // "ホワイトバランス"
								case 0xA406: // "シーン撮影モード"
								case 0xA407: // "ゲイン調整"
								case 0xA408: // "コントラスト"
								case 0xA409: // "彩度"
								case 0xA40A: // "明瞭度"
								case 0xA500: // "ガンマ値"
									groups["Image Settings"].Items.Add(lvitem);
									break;
								case 0x100: // "画像の幅"
								case 0x101: // "画像の高さ"
								case 0x102: // "ビットの深さ"
								case 0x103: // "圧縮の種類"
								case 0x106: // "画素合成"
								case 0x11A: // "水平方向の解像度"
								case 0x11B: // "垂直方向の解像度"
								case 0x11C: // "画像データの配置"
								case 0x111: // "ロケーション"
								case 0x112: // "画像の方向"
								case 0x115: // "コンポーネント数"
								case 0x116: // "ストリップ毎の行数"
								case 0x117: // "圧縮ストリップ毎のバイト数"
								case 0x128: // "解像度の単位"
								case 0xA002: // "実効画像幅"
								case 0xA003: // "実効画像高さ"
									groups["Image Information"].Items.Add(lvitem);
									break;
								case 0x10E: // "タイトル"
								case 0x131: // "ソフトウェア名"
								case 0x13B: // "作成者"
								case 0x8298: // "著作者"
								case 0x9000: // "Exif バージョン"
								case 0xA000: // "対応 FlashPix バージョン"
									groups["Software"].Items.Add(lvitem);
									break;
								case 0x12D: // "Transfer function"
								case 0x13E: // "白色点の色度"
								case 0x13F: // "原色の色度"
								case 0x201: // "JPEG SOI オフセット"
								case 0x202: // "JPEG データオフセット"
								case 0x211: // "色空間変換行列係数"
								case 0x212: // "Subsampling ration of Y to C"
								case 0x213: // "Y and C positioning"
								case 0x214: // "黒白点の基準値"
								case 0x9102: // "画像圧縮モード"
								case 0x927C: // "メーカーノート"
								case 0x9286: // "コメント"
								case 0x8769: // "Exif IFD ポインタ"
								case 0x8825: // "GPS IFD ポインタ"
								case 0x9101: // "各コンポーネントの意味"
								case 0x9290: // "作成日時サブ秒"
								case 0x9291: // "作成日時サブ秒 (original)"
								case 0x9292: // "作成日時サブ秒 (digitized)"
								case 0xA004: // "音声ファイル"
								case 0xA005: // "互換性 IFD ポインタ"
								case 0xA20B: // "フラッシュモード"
								case 0xA20C: // "空間周波数応答"
								case 0xA20E: // "焦点面 X 解像度"
								case 0xA20F: // "焦点面 Y 解像度"
								case 0xA210: // "焦点面解像度単位"
								case 0xA300: // "ファイルソース"
								case 0xA302: // "CFA pattern"
								case 0xA40B: // "デバイス設定説明"
								case 0xA40C: // "被写体距離範囲"
								case 0xA420: // "Unique image ID"
									groups["Etc"].Items.Add(lvitem);
									break;
								default:
									groups["Etc"].Items.Add(lvitem);
									break;
							}
							#endregion

							listInfo.Items.Add(lvitem);
						}
					}
					#endregion
				}
			}
		}

		/// <summary>
		/// 指定された ID に対応する名称を取得します。
		/// </summary>
		/// <param name="id">Exif ID</param>
		/// <param name="mode">言語モード [0:英語, 1:日本語]</param>
		/// <returns>
		///		変換後の名称を返します。未対応の場合は空文字を返します。
		/// </returns>
		static string GetNickName(ushort id, int mode = 1)
		{
			string name = "";

			#region 変換:
			if (mode == 1)
			{
				#region 変換: (日本語)
				switch (id)
				{
					// 4.6.8 Tag Support Levels
					// Table 17
					case 0x100: name = "画像の幅"; break;
					case 0x101: name = "画像の高さ"; break;
					case 0x102: name = "ビットの深さ"; break;
					case 0x103: name = "圧縮の種類"; break;
					case 0x106: name = "画素合成"; break;
					case 0x10E: name = "タイトル"; break;
					case 0x10F: name = "カメラのメーカー"; break;
					case 0x110: name = "カメラのモデル"; break;
					case 0x111: name = "ロケーション"; break;
					case 0x112: name = "画像の方向"; break;
					case 0x115: name = "コンポーネント数"; break;
					case 0x116: name = "ストリップ毎の行数"; break;
					case 0x117: name = "圧縮ストリップ毎のバイト数"; break;
					case 0x11A: name = "水平方向の解像度"; break;
					case 0x11B: name = "垂直方向の解像度"; break;
					case 0x11C: name = "画像データの配置"; break;
					case 0x128: name = "解像度の単位"; break;
					case 0x12D: name = "Transfer function"; break;
					case 0x131: name = "ソフトウェア名"; break;
					case 0x132: name = "変更日時"; break;
					case 0x13B: name = "作成者"; break;
					case 0x13E: name = "白色点の色度"; break;
					case 0x13F: name = "原色の色度"; break;
					case 0x201: name = "JPEG SOI オフセット"; break;
					case 0x202: name = "JPEG データオフセット"; break;
					case 0x211: name = "色空間変換行列係数"; break;
					case 0x212: name = "Subsampling ration of Y to C"; break;
					case 0x213: name = "Y and C positioning"; break;
					case 0x214: name = "黒白点の基準値"; break;
					case 0x8298: name = "著作者"; break;
					case 0x8769: name = "Exif IFD ポインタ"; break;
					case 0x8825: name = "GPS IFD ポインタ"; break;
					// Table 18
					case 0x829A: name = "露光時間"; break;
					case 0x829D: name = "F 値"; break;
					case 0x8822: name = "露出モード"; break;
					case 0x8824: name = "分光感度"; break;
					case 0x8827: name = "ISO 感度設定"; break;
					case 0x8828: name = "光電変換係数"; break;
					case 0x8830: name = "感度種別"; break;
					case 0x8831: name = "標準出力感度"; break;
					case 0x8832: name = "推奨露光指数"; break;
					case 0x8833: name = "ISO 感度"; break;
					case 0x8834: name = "ISO 感度 緯度 yyy"; break;
					case 0x8835: name = "ISO 感度 緯度 zzz"; break;
					case 0x9000: name = "Exif バージョン"; break;
					case 0x9003: name = "撮影日時"; break;
					case 0x9004: name = "作成日時"; break;
					case 0x9101: name = "各コンポーネントの意味"; break;
					case 0x9102: name = "画像圧縮モード"; break;
					case 0x9201: name = "シャッタースピード"; break;
					case 0x9202: name = "絞り値"; break;
					case 0x9203: name = "明るさ"; break;
					case 0x9204: name = "露出補正"; break;
					case 0x9205: name = "最大絞り"; break;
					case 0x9206: name = "被写体距離"; break;
					case 0x9207: name = "測光モード"; break;
					case 0x9208: name = "光源"; break;
					case 0x9209: name = "フラッシュ"; break;
					case 0x920A: name = "焦点距離"; break;
					case 0x9214: name = "被写体面積"; break;
					case 0x927C: name = "メーカーノート"; break;
					case 0x9286: name = "コメント"; break;
					case 0x9290: name = "作成日時サブ秒"; break;
					case 0x9291: name = "作成日時サブ秒 (original)"; break;
					case 0x9292: name = "作成日時サブ秒 (digitized)"; break;
					case 0xA000: name = "対応 FlashPix バージョン"; break;
					case 0xA001: name = "色空間情報"; break;
					case 0xA002: name = "実効画像幅"; break;
					case 0xA003: name = "実効画像高さ"; break;
					case 0xA004: name = "音声ファイル"; break;
					case 0xA005: name = "互換性 IFD ポインタ"; break;
					case 0xA20B: name = "フラッシュモード"; break;
					case 0xA20C: name = "空間周波数応答"; break;
					case 0xA20E: name = "焦点面 X 解像度"; break;
					case 0xA20F: name = "焦点面 Y 解像度"; break;
					case 0xA210: name = "焦点面解像度単位"; break;
					case 0xA214: name = "被写体位置"; break;
					case 0xA215: name = "露光指標"; break;
					case 0xA217: name = "センサー方式"; break;
					case 0xA300: name = "ファイルソース"; break;
					case 0xA301: name = "シーン種別"; break;
					case 0xA302: name = "CFA pattern"; break;
					case 0xA401: name = "カスタムイメージ処理"; break;
					case 0xA402: name = "露光モード"; break;
					case 0xA403: name = "ホワイトバランス"; break;
					case 0xA404: name = "デジタルズーム"; break;
					case 0xA405: name = "焦点距離（35mm 換算）"; break;
					case 0xA406: name = "シーン撮影モード"; break;
					case 0xA407: name = "ゲイン調整"; break;
					case 0xA408: name = "コントラスト"; break;
					case 0xA409: name = "彩度"; break;
					case 0xA40A: name = "明瞭度"; break;
					case 0xA40B: name = "デバイス設定説明"; break;
					case 0xA40C: name = "被写体距離範囲"; break;
					case 0xA420: name = "Unique image ID"; break;
					case 0xA430: name = "カメラ所有者"; break;
					case 0xA431: name = "カメラ製造番号"; break;
					case 0xA432: name = "レンズ仕様"; break;
					case 0xA433: name = "レンズメーカー"; break;
					case 0xA434: name = "レンズモデル"; break;
					case 0xA435: name = "レンズ製造番号"; break;
					case 0xA500: name = "ガンマ値"; break;
				}
				#endregion
			}
			else
			{
				#region 変換: (英語)
				switch (id)
				{
					// 4.6.8 Tag Support Levels
					// Table 17
					case 0x100: name = "Image width"; break;
					case 0x101: name = "Image height"; break;
					case 0x102: name = "Number of bits per component"; break;
					case 0x103: name = "Compression scheme"; break;
					case 0x106: name = "Pixel composition"; break;
					case 0x10E: name = "Image title"; break;
					case 0x10F: name = "Manufacturer of image input equipment"; break;
					case 0x110: name = "Model of image input equipment"; break;
					case 0x111: name = "Image data location"; break;
					case 0x112: name = "Orientation of image"; break;
					case 0x115: name = "Number of components"; break;
					case 0x116: name = "Number of rows per strip"; break;
					case 0x117: name = "Bytes per compressed strip"; break;
					case 0x11A: name = "Image resolution in width direction"; break;
					case 0x11B: name = "Image resolution in height direction"; break;
					case 0x11C: name = "Image data arrangement"; break;
					case 0x128: name = "Unit of X and Y resolution"; break;
					case 0x12D: name = "Transfer function"; break;
					case 0x131: name = "Software used"; break;
					case 0x132: name = "File change date and time"; break;
					case 0x13B: name = "Person who created the image"; break;
					case 0x13E: name = "White point chromaticity"; break;
					case 0x13F: name = "Chromaticities of primaries"; break;
					case 0x201: name = "Offset to JPEG SOI"; break;
					case 0x202: name = "Offset to JPEG data"; break;
					case 0x211: name = "Color space transformation matrix coefficients"; break;
					case 0x212: name = "Subsampling ration of Y to C"; break;
					case 0x213: name = "Y and C positioning"; break;
					case 0x214: name = "Pair of black and white reference values"; break;
					case 0x8298: name = "Copyright holder"; break;
					case 0x8769: name = "Exif IFD pointer"; break;
					case 0x8825: name = "GPS IFD pointer"; break;
					// Table 18
					case 0x829A: name = "Exposure time"; break;
					case 0x829D: name = "F number"; break;
					case 0x8822: name = "Exposure program"; break;
					case 0x8824: name = "Spectral sensitivity"; break;
					case 0x8827: name = "ISO speed ratings"; break;
					case 0x8828: name = "Optoelectric conversion factor"; break;
					case 0x8830: name = "Sensitivity Type"; break;
					case 0x8831: name = "Standard Output Sensitivity"; break;
					case 0x8832: name = "Recommended Exposure Index"; break;
					case 0x8833: name = "ISO Speed"; break;
					case 0x8834: name = "ISO Speed Latitude yyy"; break;
					case 0x8835: name = "ISO Speed Latitude zzz"; break;
					case 0x9000: name = "Exif version"; break;
					case 0x9003: name = "Date and time of original data generation"; break;
					case 0x9004: name = "Date and time of digital data generation"; break;
					case 0x9101: name = "Meaning of each component"; break;
					case 0x9102: name = "Image compression mode"; break;
					case 0x9201: name = "Shutter speed"; break;
					case 0x9202: name = "Aperture"; break;
					case 0x9203: name = "Brightness"; break;
					case 0x9204: name = "Exposure bias"; break;
					case 0x9205: name = "Maximul lens aperture"; break;
					case 0x9206: name = "Subject distance"; break;
					case 0x9207: name = "Metering mode"; break;
					case 0x9208: name = "Light source"; break;
					case 0x9209: name = "Flash"; break;
					case 0x920A: name = "Lens focal length"; break;
					case 0x9214: name = "Subject area"; break;
					case 0x927C: name = "Manufacturer note"; break;
					case 0x9286: name = "User comments"; break;
					case 0x9290: name = "Date time subseconds"; break;
					case 0x9291: name = "Date time original subseconds"; break;
					case 0x9292: name = "Date time digitized subseconds"; break;
					case 0xA000: name = "Supported Flashpix version"; break;
					case 0xA001: name = "Color space information"; break;
					case 0xA002: name = "Valid image width"; break;
					case 0xA003: name = "Valid image height"; break;
					case 0xA004: name = "Related audio file"; break;
					case 0xA005: name = "Interoperatibility IFD pointer"; break;
					case 0xA20B: name = "Flash energy"; break;
					case 0xA20C: name = "Spatial frequency response"; break;
					case 0xA20E: name = "Focal plane X resolution"; break;
					case 0xA20F: name = "Focal plane Y resolution"; break;
					case 0xA210: name = "Focal plane resolution unit"; break;
					case 0xA214: name = "Subject location"; break;
					case 0xA215: name = "Exposure index"; break;
					case 0xA217: name = "Sensing method"; break;
					case 0xA300: name = "File source"; break;
					case 0xA301: name = "Scene type"; break;
					case 0xA302: name = "CFA pattern"; break;
					case 0xA401: name = "Custom image processing"; break;
					case 0xA402: name = "Exposure mode"; break;
					case 0xA403: name = "White balance"; break;
					case 0xA404: name = "Digital zoom ratio"; break;
					case 0xA405: name = "Focal length in 35mm film"; break;
					case 0xA406: name = "Scene capture type"; break;
					case 0xA407: name = "Gain control"; break;
					case 0xA408: name = "Contrast"; break;
					case 0xA409: name = "Saturation"; break;
					case 0xA40A: name = "Sharpness"; break;
					case 0xA40B: name = "Device settings description"; break;
					case 0xA40C: name = "Subject distance range"; break;
					case 0xA420: name = "Unique image ID"; break;
					case 0xA430: name = "Camera Owner Name"; break;
					case 0xA431: name = "Body Serial Number"; break;
					case 0xA432: name = "Lens Specification"; break;
					case 0xA433: name = "Lens Make"; break;
					case 0xA434: name = "Lens Model"; break;
					case 0xA435: name = "Lens Serial Number"; break;
					case 0xA500: name = "Gamma"; break;
				}
				#endregion
			}
			#endregion

			return name;
		}

		/// <summary>
		/// 指定された項目に関連する値を文字列として取得します。
		/// </summary>
		/// <param name="exif">Exif オブジェクト</param>
		/// <param name="item">対象の項目</param>
		/// <returns>
		///		変換後の文字列を返します。未対応の場合は空文字を返します。
		/// </returns>
		static string GetValueAsString(XIE.CxExif exif, XIE.TxExifItem item)
		{
			string strValue = "";

			#region 変換:
			switch (item.Type)
			{
				case 1:		// BYTE
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.U8(1))
						{
							scan.ForEach((int x, IntPtr addr) =>
							{
								var ptr = (XIE.Ptr.BytePtr)addr;
								if (x == 0)
									strValue += string.Format("{0}", ptr[0]);
								else
									strValue += string.Format(",{0}", ptr[0]);
							});
						}
					}
					break;
				case 7:		// UNDEFINED
					switch (item.ID)
					{
						case 0x9000:	// Exif version
						case 0xA000:	// Supported FlashPix version
							{
								var value = new XIE.CxArray();
								exif.GetValue(item, value);
								if (value.Length == 4)
								{
									var addr = (XIE.Ptr.BytePtr)value.Address();
									var data = addr.ToArray(value.Length);
									strValue = System.Text.Encoding.ASCII.GetString(data);
								}
							}
							break;
					}
					break;
				case 2:		// ASCII
					{
						var value = new XIE.CxStringA();
						exif.GetValue(item, value);
						if (value.IsValid)
						{
							strValue = value.ToString();
						}
					}
					break;
				case 3:		// SHORT
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.U16(1))
						{
							scan.ForEach((int x, IntPtr addr) =>
							{
								var ptr = (XIE.Ptr.UInt16Ptr)addr;
								if (x == 0)
									strValue += string.Format("{0}", ptr[0]);
								else
									strValue += string.Format(",{0}", ptr[0]);
							});
						}
					}
					break;
				case 4:		// LONG
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.U32(1))
						{
							scan.ForEach((int x, IntPtr addr) =>
							{
								var ptr = (XIE.Ptr.UInt32Ptr)addr;
								if (x == 0)
									strValue += string.Format("{0}", ptr[0]);
								else
									strValue += string.Format(",{0}", ptr[0]);
							});
						}
					}
					break;
				case 9:		// SLONG 32bit
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.S32(1))
						{
							scan.ForEach((int x, IntPtr addr) =>
							{
								var ptr = (XIE.Ptr.Int32Ptr)addr;
								if (x == 0)
									strValue += string.Format("{0}", ptr[0]);
								else
									strValue += string.Format(",{0}", ptr[0]);
							});
						}
					}
					break;
				case 5:		// RATIONAL
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.U32(2))
						{
							switch (item.ID)
							{
								case 0x829D: // F number
								case 0x9202: // Aperture
								case 0x920A: // Lens focal length
									{
										var ptr = (XIE.Ptr.UInt32Ptr)scan.Address;
										if (ptr[1] == 0)
											strValue = string.Format("{0}/{1}", ptr[0], ptr[1]);
										else
										{
											var va = (double)ptr[0] / ptr[1];
											var v2 = System.Math.Round(va, 2);
											var v1 = System.Math.Round(va, 1);
											var v0 = System.Math.Round(va);
											if (v0 == v2)
												strValue = string.Format("{0:F0}", v0);
											else if (v1 == v2)
												strValue = string.Format("{0:F1}", v1);
											else
												strValue = string.Format("{0:F2}", v2);
										}
									}
									break;
								default:
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.UInt32Ptr)addr;
										if (x == 0)
											strValue += string.Format("{0}/{1}", ptr[0], ptr[1]);
										else
											strValue += string.Format(",{0}/{1}", ptr[0], ptr[1]);
									});
									break;
							}
						}
					}
					break;
				case 10:	// SRATIONAL
					{
						var value = new XIE.CxArray();
						exif.GetValue(item, value);
						var scan = value.Scanner();
						if (value.Model == XIE.TxModel.S32(2))
						{
							switch (item.ID)
							{
								case 0x9204: // Exposure bias
									{
										var ptr = (XIE.Ptr.Int32Ptr)scan.Address;
										if (ptr[1] == 0)
											strValue = string.Format("{0}/{1}", ptr[0], ptr[1]);
										else
										{
											var va = (double)ptr[0] / ptr[1];
											var v2 = System.Math.Round(va, 2);
											var v1 = System.Math.Round(va, 1);
											var v0 = System.Math.Round(va);
											if (v0 == v2)
												strValue = string.Format("{0:F0}", v0);
											else if (v1 == v2)
												strValue = string.Format("{0:F1}", v1);
											else
												strValue = string.Format("{0:F2}", v2);
										}
									}
									break;
								default:
									scan.ForEach((int x, IntPtr addr) =>
									{
										var ptr = (XIE.Ptr.Int32Ptr)addr;
										if (x == 0)
											strValue += string.Format("{0}/{1}", ptr[0], ptr[1]);
										else
											strValue += string.Format(",{0}/{1}", ptr[0], ptr[1]);
									});
									break;
							}
						}
					}
					break;
			}
			#endregion

			return strValue;
		}

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// フォームがアクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_Activated(object sender, EventArgs e)
		{
			this.Opacity = 1.0;
		}

		/// <summary>
		/// フォームが非アクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_Deactivate(object sender, EventArgs e)
		{
			this.Opacity = 0.9;
		}

		#endregion

		#region コントロールイベント: (ドラッグ＆ドロップ)

		/// <summary>
		/// ドラッグされた項目がコントロール内に入ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
				return;
			}
		}

		/// <summary>
		/// ドラッグされた項目がコントロール上にドロップされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxExifForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				#region ファイルがドロップされた時.
				try
				{
					var drops = (string[])e.Data.GetData(DataFormats.FileDrop, false);
					foreach (var filename in drops)
					{
						if (filename.EndsWith(".raw", StringComparison.CurrentCultureIgnoreCase))
						{
							var header = XIE.Axi.CheckRaw(filename);
							var clsname = Encoding.ASCII.GetString(header.ClassName).Trim('\0');
							if (clsname == "CxExif")
							{
								var src = new XIE.CxExif(filename);
								var exif = src.Tag();
								if (exif.IsValid)
								{
									this.Image.Exif(exif);
									this.UpdateInfo();
									break;
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
		}

		#endregion

		#region toolbar: (Exif ファイル)

		/// <summary>
		/// Exif ファイルの読み込み
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileOpen_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			#region OpenFileDialog
			var dlg = new OpenFileDialog();
			{
				dlg.AddExtension = true;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = "Exif files |*.raw";
				dlg.Filter += "|All files (*.*)|*.*";
				if (XIE.Tasks.SharedData.ProjectDir != "")
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					using (var src_exif = new XIE.CxExif(dlg.FileName))
					{
						if (src_exif.IsValid)
						{
							this.Image.ExifCopy(src_exif.Tag());
							this.UpdateInfo();
						}
						else
						{
							MessageBox.Show(this, "Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// Exif ファイルへの保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileSave_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			var exif = this.Image.Exif();
			if (exif.IsValid)
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					var now = DateTime.Now;
					var timestamp = ApiHelper.MakeFileNameSuffix(now, false);
					var filename = string.Format("{0}-{1}.raw", "Exif", timestamp);

					dlg.Filter = "Exif files |*.raw";
					dlg.Filter += "|All files (*.*)|*.*";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;
					if (XIE.Tasks.SharedData.ProjectDir != "")
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
				}
				#endregion

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						using (var dst = XIE.CxExif.FromTag(exif))
						{
							dst.Save(dlg.FileName);
						}
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		#endregion

		#region toolbar: (ItemNameMode)

		/// <summary>
		/// ItemNameMode: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolItemNameMode_DropDownOpened(object sender, EventArgs e)
		{
			var ctrls = new ToolStripMenuItem[]
			{
				toolItemNameMode0,
				toolItemNameMode1,
			};
			foreach (var ctrl in ctrls)
			{
				if (ctrl.Tag != null)
				{
					int mode = Convert.ToInt32(ctrl.Tag);
					ctrl.Checked = (mode == CxAuxInfoForm.AppSettings.ExifItemNameMode);
				}
			}
		}

		/// <summary>
		/// ItemNameMode: メニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolItemNameMode_Click(object sender, EventArgs e)
		{
			var ctrl = sender as ToolStripMenuItem;
			if (ctrl != null && ctrl.Tag != null)
			{
				var mode = Convert.ToInt32(ctrl.Tag);
				CxAuxInfoForm.AppSettings.ExifItemNameMode = mode;
				UpdateInfo();
			}
		}

		#endregion

		#region toolbar: (Exif 編集)

		/// <summary>
		/// Exif ストック
		/// </summary>
		private static XIE.CxExif ExifStock = new XIE.CxExif();

		/// <summary>
		/// Exif 編集: Exif の切り取り
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCut_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			toolEditCopy_Click(sender, e);

			this.Image.Exif(new XIE.TxExif());
			this.UpdateInfo();
		}

		/// <summary>
		/// Exif 編集: Exif のコピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCopy_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			var exif = this.Image.Exif();
			if (exif.IsValid)
			{
				// 配列の複製:
				using (var src = XIE.CxExif.FromTag(exif))
				{
					ExifStock.CopyFrom(src);
				}

				// リストビュー項目の収集:
				{
					var sb = new StringBuilder();
					foreach (ListViewGroup group in listInfo.Groups)
					{
						foreach (ListViewItem lvitem in group.Items)
						{
							var str = lvitem.Text;
							for (int i = 1; i < lvitem.SubItems.Count; i++)
							{
								ListViewItem.ListViewSubItem subitem = lvitem.SubItems[i];
								str += "\t" + subitem.Text;
							}
							str += "\n";
							sb.Append(str);
						}
					}
					Clipboard.SetText(sb.ToString());
					sb.Length = 0;
				}
			}
		}

		/// <summary>
		/// Exif 編集: Exif の貼り付け
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditPaste_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			if (ExifStock.IsValid)
			{
				this.Image.ExifCopy(ExifStock.Tag());
				this.UpdateInfo();
			}
		}

		/// <summary>
		/// Exif 編集: Exif ストックの消去
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditClear_Click(object sender, EventArgs e)
		{
			if (this.Image == null) return;

			ExifStock.Dispose();
		}

		#endregion

		#region toolbar: (Dock)

		/// <summary>
		/// フォームの追従モードの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolDock_Click(object sender, EventArgs e)
		{
			IsDocking = toolDock.Checked;
		}

		#endregion
	}
}
