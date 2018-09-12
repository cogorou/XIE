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
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;

namespace XIEstudio
{
	/// <summary>
	/// タスクユニット描画クラス
	/// </summary>
	public class CxTaskVisualizer : System.Object
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskVisualizer()
		{
			this.Setup();
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		public void Setup()
		{
			this.FontName = "Meiryo UI";
			this.EnableFill = true;

			const int alpha = 224;
			int block = (this.BlockType == 0) ? 32 : 16;
			int fontsize1 = (this.BlockType == 0) ? 8 : 8;
			int fontsize2 = (this.BlockType == 0) ? 9 : 8;

			switch (this.ColorType)
			{
				default:
				case 0:
					#region Dark モード:
					{
						SheetColor = Color.FromArgb(32, 32, 32);			// 黒:
						GridColor = Color.FromArgb(128, 128, 128);
						TextColor = Color.FromArgb(192, 192, 192);			// 白:
						FrameColor = Color.FromArgb(96, 96, 96);			// 灰:
					//	BackColor1 = Color.FromArgb(alpha, 63, 73, 127);	// 青:	(見づらい)
					//	BackColor1 = Color.FromArgb(alpha, 127, 146, 200);	// 青:	(まぁ良い)
						BackColor1 = Color.FromArgb(alpha, 137, 156, 210);	// 青:	(ちょうど良い)
					//	BackColor1 = Color.FromArgb(alpha, 127, 192, 224);	// 青:	(軽薄)
					//	BackColor1 = Color.FromArgb(alpha, 127, 200, 146);	// 緑:	(まぁまぁ)
						BackColor2 = Color.FromArgb(alpha, 32, 32, 32);		// 黒:
						ControlInColor = Color.FromArgb(alpha, 255, 216, 0);
						ControlOutColor = Color.FromArgb(alpha, 255, 216, 0);
						DataInColor = Color.FromArgb(alpha, 255, 0, 0);
						DataOutColor = Color.FromArgb(alpha, 0, 255, 0);
						DataParamColor = Color.FromArgb(alpha, 0, 255, 255);
						SelectColor = Color.FromArgb(alpha, 255, 128, 0);	// 橙:
						LineWidth = 1;
						CategoryFont = new Font(this.FontName, fontsize1, FontStyle.Regular);
						NameFont = new Font(this.FontName, fontsize2, FontStyle.Bold);
						TitleHeight = 32;
						FrameBold = 2;
						BlockSize = new Size(block, block);
						PortFont = new Font(this.FontName, fontsize2, FontStyle.Regular);
						PortSize = new Size(block / 2, block / 2);
					}
					#endregion
					break;
				case 1:
					#region Light モード:
					{
						SheetColor = Color.FromArgb(250, 250, 250);
						GridColor = Color.FromArgb(128, 128, 128);
						TextColor = Color.FromArgb(16, 16, 16);				// 黒:
						FrameColor = Color.FromArgb(160, 160, 160);			// 灰:
					//	BackColor1 = Color.FromArgb(alpha, 127, 146, 255);	// 青:	(見づらい)
						BackColor1 = Color.FromArgb(alpha, 137, 156, 210);	// 青:	(ちょうど良い)
					//	BackColor1 = Color.FromArgb(alpha, 127, 200, 146);	// 緑:	(まぁまぁ)
						BackColor2 = Color.FromArgb(alpha, 32, 32, 32);		// 黒:
						ControlInColor = Color.FromArgb(alpha, 178, 148, 0);
						ControlOutColor = Color.FromArgb(alpha, 178, 148, 0);
						DataInColor = Color.FromArgb(alpha, 255, 0, 0);
						DataOutColor = Color.FromArgb(alpha, 0, 128, 0);
						DataParamColor = Color.FromArgb(alpha, 0, 0, 255);
						SelectColor = Color.FromArgb(alpha, 255, 128, 0);	// 橙:
						LineWidth = 1;
						CategoryFont = new Font(this.FontName, fontsize1, FontStyle.Regular);
						NameFont = new Font(this.FontName, fontsize2, FontStyle.Bold);
						TitleHeight = 32;
						FrameBold = 2;
						BlockSize = new Size(block, block);
						PortFont = new Font(this.FontName, fontsize2, FontStyle.Regular);
						PortSize = new Size(block / 2, block / 2);
					}
					#endregion
					break;
			}
		}

		#endregion

		#region 初期化: (印刷用設定)

		/// <summary>
		/// 初期化 (印刷用設定)
		/// </summary>
		/// <param name="src">元となる画面表示設定</param>
		public void SetupForPrinting(CxTaskVisualizer src)
		{
			this.Scale = 100;
			this.SheetType = src.SheetType;
			this.ColorType = 1;	// 1:Light
			this.LineType = src.LineType;
			this.BlockType = src.BlockType;
			this.Setup();

			this.EnableFill = false;
			this.LineWidth = src.FrameBold;
			this.FrameColor = Color.Black;
			this.TextColor = Color.Black;
		}

		#endregion

		#region プロパティ: (描画属性)

		/// <summary>
		/// 表示倍率 [10~100 (10 刻み)]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.Scale")]
		public int Scale
		{
			get { return m_Scale; }
			set
			{
				var scale = value / 10 * 10;
				if (scale < 10)
					scale = 10;
				if (scale > 100)
					scale = 100;
				m_Scale = value;
			}
		}
		private int m_Scale = 100;

		/// <summary>
		/// シートタイプ [1~9]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.SheetType")]
		public int SheetType
		{
			get { return m_SheetType; }
			set
			{
				if (1 <= value && value <= 9)
					m_SheetType = value;
			}
		}
		private int m_SheetType = 1;

		/// <summary>
		/// 背景色タイプ [0:Dark, 1:Light]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.ColorType")]
		public int ColorType
		{
			get { return m_ColorType; }
			set
			{
				switch (value)
				{
					case 0:
					case 1:
						m_ColorType = value;
						break;
				}
			}
		}
		private int m_ColorType = 0;

		/// <summary>
		/// 接続線タイプ [0:Polyine, 1:Spline]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.LineType")]
		public int LineType
		{
			get { return m_LineType; }
			set
			{
				switch (value)
				{
					case 0:
					case 1:
						m_LineType = value;
						break;
				}
			}
		}
		private int m_LineType = 0;

		/// <summary>
		/// ブロックタイプ [0:Large, 1:Small]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.BlockType")]
		public int BlockType
		{
			get { return m_BlockType; }
			set
			{
				switch (value)
				{
					case 0:
					case 1:
						m_BlockType = value;
						break;
				}
			}
		}
		private int m_BlockType = 0;

		#endregion

		#region プロパティ: (操作関連)

		/// <summary>
		/// マウス操作情報
		/// </summary>
		[Browsable(false)]
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskVisualizer.HandlingInfo")]
		public CxTaskHandlingInfo HandlingInfo
		{
			get { return m_HandlingInfo; }
			private set { m_HandlingInfo = value; }
		}
		private CxTaskHandlingInfo m_HandlingInfo = new CxTaskHandlingInfo();

		#endregion

		#region フィールド:

		/// <summary>
		/// フォント名
		/// </summary>
		public string FontName;

		/// <summary>
		/// 塗り潰し属性
		/// </summary>
		public bool EnableFill;

		/// <summary>
		/// シートの色
		/// </summary>
		public Color SheetColor;

		/// <summary>
		/// グリッドの色
		/// </summary>
		public Color GridColor;

		/// <summary>
		/// テキストの色
		/// </summary>
		public Color TextColor;

		/// <summary>
		/// フレームの前景色
		/// </summary>
		public Color FrameColor;

		/// <summary>
		/// フレームの背景色 (始点)
		/// </summary>
		public Color BackColor1;

		/// <summary>
		/// フレームの背景色 (終点)
		/// </summary>
		public Color BackColor2;

		/// <summary>
		/// コントロール入力ポートの描画色
		/// </summary>
		public Color ControlInColor;

		/// <summary>
		/// コントロール出力ポートの描画色
		/// </summary>
		public Color ControlOutColor;

		/// <summary>
		/// データ入力ポートの描画色
		/// </summary>
		public Color DataInColor;

		/// <summary>
		/// データ出力ポートの描画色
		/// </summary>
		public Color DataOutColor;

		/// <summary>
		/// パラメータポートの描画色
		/// </summary>
		public Color DataParamColor;

		/// <summary>
		/// 選択枠の色
		/// </summary>
		public Color SelectColor;

		/// <summary>
		/// 接続線の太さ
		/// </summary>
		public int LineWidth;

		/// <summary>
		/// カテゴリのフォント
		/// </summary>
		public Font CategoryFont;

		/// <summary>
		/// 名称のフォント
		/// </summary>
		public Font NameFont;

		/// <summary>
		/// タイトルの高さ
		/// </summary>
		public int TitleHeight;

		/// <summary>
		/// フレーム線幅
		/// </summary>
		public int FrameBold;

		/// <summary>
		/// ブロックサイズ
		/// </summary>
		public Size BlockSize;

		/// <summary>
		/// データポートのフォント
		/// </summary>
		public Font PortFont;

		/// <summary>
		/// データポートのサイズ
		/// </summary>
		public Size PortSize;

		#endregion

		#region メソッド: (位置とサイズの計算)

		/// <summary>
		/// シートサイズの取得
		/// </summary>
		/// <param name="scaling">スケーリングするか否か</param>
		/// <returns>
		///		現在のタスクユニットがタスクフローの場合は、
		///		シートタイプに対応するサイズを返します。
		///		それ以外は既定のシートサイズ(2339,1654)を返します。
		/// </returns>
		public Size GetSheetSize(bool scaling = true)
		{
			var size = GetSheetSize(this.SheetType);
			if (scaling)
			{
				var scale = this.Scale / 10 * 10;
				if (scale < 10)
					scale = 10;
				if (scale > 100)
					scale = 100;

				size.Width = size.Width * scale / 100;
				size.Height = size.Height * scale / 100;
			}
			return size;
		}

		/// <summary>
		/// シートサイズの取得
		/// </summary>
		/// <param name="type">シートタイプ[1~9]</param>
		/// <returns>
		///		指定されたシートタイプに対応するサイズを返します。
		/// </returns>
		public Size GetSheetSize(int type)
		{
			if (type < 1)
				type = 1;
			if (type > 9)
				type = 9;

			int w, h;
			switch (type)
			{
				case 1: w = 1169; h = 826; break;	// A4
				case 2: w = 1390; h = 985; break;	// B4
				case 3: w = 1654; h = 1169; break;	// A3
				case 4: w = 1968; h = 1390; break;	// B3
				default:
				case 5: w = 2339; h = 1654; break;	// A2
				case 6: w = 2783; h = 1968; break;	// B2
				case 7: w = 3311; h = 2338; break;	// A1
				case 8: w = 3937; h = 2783; break;	// B1
				case 9: w = 4680; h = 3311; break;	// A0
			}
			return new Size(w, h);
		}

		/// <summary>
		/// シート規格の取得
		/// </summary>
		/// <returns>
		///		現在のタスクユニットがタスクフローの場合は、
		///		シートタイプに対応する規格(A#,B#)を返します。
		///		それ以外は既定のシート規格(A2)を返します。
		/// </returns>
		public string GetSheetSpec()
		{
			return GetSheetSpec(this.SheetType);
		}

		/// <summary>
		/// シート規格の取得
		/// </summary>
		/// <param name="type">シートタイプ[1~10]</param>
		/// <returns>
		///		指定されたシートタイプに対応する規格(A#,B#)を返します。
		/// </returns>
		public string GetSheetSpec(int type)
		{
			if (type < 1)
				type = 1;
			if (type > 9)
				type = 9;

			string result;
			switch (type)
			{
				case 1: result = "A4"; break;
				case 2: result = "B4"; break;
				case 3: result = "A3"; break;
				case 4: result = "B3"; break;
				default:
				case 5: result = "A2"; break;
				case 6: result = "B2"; break;
				case 7: result = "A1"; break;
				case 8: result = "B1"; break;
				case 9: result = "A0"; break;
			}
			return result;
		}

		/// <summary>
		/// 本体の最小外接矩形の取得
		/// </summary>
		/// <param name="unit">対象のタスクユニット</param>
		/// <returns>
		///		現在の状態から計算された矩形を返します。
		/// </returns>
		public Rectangle GetBounds(XIE.Tasks.CxTaskUnit unit)
		{
			//   title
			//   +---------+
			//   |[]       |
			// i0|         |o0
			// i1|         |o1
			// p0|         |o2
			// p1|         |
			//   +---------+

			{
				Rectangle result = new Rectangle();
				result.Location = unit.Location;

				int port_count = System.Math.Max(
					unit.DataIn.Length + unit.DataParam.Length,
					unit.DataOut.Length);
				int margin_x = this.BlockSize.Width * (2 + 1);
				int height = this.BlockSize.Height * port_count + this.TitleHeight;
				int width = this.BlockSize.Width;
				if (this.BlockSize.Width > 0)
					width = margin_x + ((width + (this.BlockSize.Width - 1)) / this.BlockSize.Width * this.BlockSize.Width);
				result.Size = new Size(width, height);
				return result;
			}
		}

		/// <summary>
		/// ポートの矩形の取得
		/// </summary>
		/// <param name="unit">対象のタスクユニット</param>
		/// <param name="type">種別</param>
		/// <returns>
		///		各ポートの矩形を返します。
		/// </returns>
		public Rectangle[] GetPortRects(XIE.Tasks.CxTaskUnit unit, ExTaskUnitPositionType type)
		{
			//   +---------+
			//   |[] title |
			// i0|         |o0
			// i1|         |o1
			// p0|         |o2
			// p1|         |
			//   +---------+

			var result = new List<Rectangle>();
			switch (type)
			{
				default:
				case ExTaskUnitPositionType.Body:
					break;
				case ExTaskUnitPositionType.ControlIn:
					#region コントロール入力ポート:
					{
						{
							Rectangle rect = new Rectangle(
								unit.Location.X - this.PortSize.Width - (this.FrameBold + 4),
								unit.Location.Y,
								this.PortSize.Width / 2,
								this.PortSize.Height / 2);
							result.Add(rect);
						}
					}
					#endregion
					break;
				case ExTaskUnitPositionType.ControlOut:
					#region コントロール出力ポート:
					{
						var bounds = GetBounds(unit);
						{
							Rectangle rect = new Rectangle(
								unit.Location.X + bounds.Width + (this.FrameBold + 4),
								unit.Location.Y,
								this.PortSize.Width / 2,
								this.PortSize.Height / 2);
							result.Add(rect);
						}
					}
					#endregion
					break;
				case ExTaskUnitPositionType.DataIn:
					#region データ入力ポート:
					{
						int offset = this.TitleHeight;
						for (int i = 0; i < unit.DataIn.Length; i++)
						{
							Rectangle rect = new Rectangle(
								unit.Location.X - this.PortSize.Width - (this.FrameBold + 4),
								unit.Location.Y + offset + (this.BlockSize.Height * i),
								this.PortSize.Width,
								this.PortSize.Height);
							result.Add(rect);
						}
					}
					#endregion
					break;
				case ExTaskUnitPositionType.DataParam:
					#region パラメータポート:
					{
						int offset = this.TitleHeight + this.BlockSize.Height * unit.DataIn.Length;
						for (int i = 0; i < unit.DataParam.Length; i++)
						{
							Rectangle rect = new Rectangle(
								unit.Location.X - this.PortSize.Width - (this.FrameBold + 4),
								unit.Location.Y + offset + (this.BlockSize.Height * i),
								this.PortSize.Width,
								this.PortSize.Height);
							result.Add(rect);
						}
					}
					#endregion
					break;
				case ExTaskUnitPositionType.DataOut:
					#region データ出力ポート:
					{
						var bounds = GetBounds(unit);
						int offset = this.TitleHeight;
						for (int i = 0; i < unit.DataOut.Length; i++)
						{
							Rectangle rect = new Rectangle(
								unit.Location.X + bounds.Width + (this.FrameBold + 4),
								unit.Location.Y + offset + (this.BlockSize.Height * i),
								this.PortSize.Width,
								this.PortSize.Height);
							result.Add(rect);
						}
					}
					#endregion
					break;
			}
			return result.ToArray();
		}

		/// <summary>
		/// 文字列の外接矩形サイズの取得
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns>
		///		計算されたサイズを返します。
		/// </returns>
		public SizeF MeasureString(Graphics graphics, string text, Font font)
		{
			SizeF size = graphics.MeasureString(text, font);
			return size;
		}

		/// <summary>
		/// 文字列の外接矩形サイズの取得
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="position"></param>
		/// <returns>
		///		計算されたサイズを返します。
		/// </returns>
		public SizeF MeasureString(Graphics graphics, string text, Font font, PointF position)
		{
			// こちらの方が正確に算出できるらしい.
			SizeF size = graphics.MeasureString(text, font);
			StringFormat format = new StringFormat();
			format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, text.Length) });
			Region[] regions = graphics.MeasureCharacterRanges(text, font, new RectangleF(position, size), format);
			RectangleF rect = regions[0].GetBounds(graphics);
			return rect.Size;
		}

		#endregion

		#region メソッド: (描画)

		/// <summary>
		/// 描画
		/// </summary>
		/// <param name="graphics">描画先のグラフィクス</param>
		/// <param name="unit">描画対象のタスクユニット</param>
		public void Render(Graphics graphics, XIE.Tasks.CxTaskUnit unit)
		{
			//  ReferenceNode
			//  │     ReferenceData
			//  ↓     ↓
			// +---+       st     ed       +---+
			// |src|o0:[rect]-----[rect]:i0|dst|
			// +---+                    :i1|   |
			// +---+                    :p0|   |
			// |src|o0:[rect]-----[rect]:p1|   |
			// |   |o1:                 :p2|   |
			// +---+                       +---+

			var bounds = this.GetBounds(unit);

			using (var selectPen = new Pen(this.SelectColor, this.FrameBold))
			using (var bodyPen = new Pen(this.FrameColor, this.FrameBold))
			using (var textBrush = new SolidBrush(this.TextColor))
			{
				if (unit is XIE.Tasks.CxTaskReference)
					bodyPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

				#region カテゴリと名称:
				{
					// カテゴリ:
					{
						var stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Near;		// Left
						stringFormat.LineAlignment = StringAlignment.Far;	// bottom
						var position = unit.Location;
						var size = MeasureString(graphics, "1", this.CategoryFont);
						position.Y -= (FrameBold + (int)size.Height);
						graphics.DrawString(unit.Category, this.CategoryFont, textBrush, position, stringFormat);
					}
					// 名称:
					{
						var stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Near;		// Left
						stringFormat.LineAlignment = StringAlignment.Far;	// bottom
						var position = unit.Location;
						position.Y -= FrameBold;
						graphics.DrawString(unit.Name, this.NameFont, textBrush, position, stringFormat);
					}
				}
				#endregion

				#region 本体:
				{
					// 枠:
					graphics.DrawRectangle(bodyPen, bounds);

					Color bg1 = this.BackColor1;
					Color bg2 = this.BackColor2;

					if (ReferenceEquals(unit, this.HandlingInfo.FocusedUnit))
					{
						bg1 = Color.FromArgb(bg1.A, bg1.B, bg1.G, bg1.R);	// B,R の入れ替え.
						bg2 = Color.FromArgb(bg2.A, bg2.B, bg2.G, bg2.R);	// B,R の入れ替え.
					}

					// 背景:
					if (this.EnableFill)
					{
						var point1 = new Point((bounds.Left + bounds.Right) / 2, bounds.Top);
						var point2 = new Point((bounds.Left + bounds.Right) / 2, bounds.Bottom);
						using (var brush = new LinearGradientBrush(point1, point2, bg1, bg2))
						{
							graphics.FillRectangle(brush, bounds);
						}
					}

					// アイコンの背景:
					if (this.EnableFill)
					{
						using (var brush = new SolidBrush(bg1))
						{
							var rect = new Rectangle(
								unit.Location.X + 8,
								unit.Location.Y + 4,
								24,
								24
								);
							graphics.FillEllipse(brush, rect);
						}
					}

					// アイコン:
					if (unit.IconImage != null)
					{
						var position = new Point(
							unit.Location.X + 8 + ((24 - 16) / 2),
							unit.Location.Y + 4 + ((24 - 16) / 2)
							);
						graphics.DrawImage(unit.IconImage, position);
					}

					// ブレークポイント:
					if (unit.Breakpoint)
					{
						using (var pen = new Pen(Color.White))
						using (var brush = new SolidBrush(Color.Red))
						{
							var center = new Point(
								bounds.X + bounds.Width / 2,
								bounds.Y + bounds.Height / 2
 								);
							var size = new Size(16, 16);
							var rect = new Rectangle(
								center.X - size.Width / 2,
								center.Y - size.Height / 2,
								size.Width,
								size.Height
								);
							graphics.FillEllipse(brush, rect);
							graphics.DrawEllipse(pen, rect);
						}
					}

					// 選択枠:
					if (unit.Selected)
					{
						graphics.DrawRectangle(selectPen, bounds);
					}
				}
				#endregion

				#region コントロール入力ポート:
				{
					var port_rects = this.GetPortRects(unit, ExTaskUnitPositionType.ControlIn);
					for (int i = 0; i < port_rects.Length; i++)
					{
						// ポートの矩形:
						var rect = port_rects[i];
						if (this.EnableFill)
						{
							using (var portBrush = new SolidBrush(this.ControlInColor))
							using (var portPen = new Pen(this.ControlInColor, 1))
							{
								graphics.FillEllipse(portBrush, rect);
								graphics.DrawEllipse(portPen, rect);
							}
						}
						else
						{
							using (var portPen = new Pen(this.ControlInColor, this.FrameBold))
							{
								graphics.DrawEllipse(portPen, rect);
							}
						}
					}
				}
				#endregion

				#region コントロール出力ポート:
				{
					var port_rects = this.GetPortRects(unit, ExTaskUnitPositionType.ControlOut);
					for (int i = 0; i < port_rects.Length; i++)
					{
						// ポートの矩形:
						var rect = port_rects[i];
						if (this.EnableFill)
						{
							using (var portBrush = new SolidBrush(this.ControlOutColor))
							using (var portPen = new Pen(this.ControlOutColor, 1))
							{
								graphics.FillEllipse(portBrush, rect);
								graphics.DrawEllipse(portPen, rect);
							}
						}
						else
						{
							using (var portPen = new Pen(this.ControlOutColor, this.FrameBold))
							{
								graphics.DrawEllipse(portPen, rect);
							}
						}
					}
				}
				#endregion

				#region データ入力ポート:
				{
					var port_rects = this.GetPortRects(unit, ExTaskUnitPositionType.DataIn);
					for (int i = 0; i < unit.DataIn.Length; i++)
					{
						// ポートの矩形:
						var rect = port_rects[i];
						if (this.EnableFill)
						{
							using (var portBrush = new SolidBrush(this.DataInColor))
							{
								graphics.FillRectangle(portBrush, rect);
								graphics.DrawRectangle(bodyPen, rect);
							}
						}
						else
						{
							using (var portPen = new Pen(this.DataInColor, this.FrameBold))
							{
								if (unit is XIE.Tasks.CxTaskReference)
									portPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								graphics.DrawRectangle(portPen, rect);
							}
						}

						// ポートの名称:
						var stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Far;		// right
						stringFormat.LineAlignment = StringAlignment.Far;	// bottom
						var portPoint = new PointF(
							-this.FrameBold + rect.Left,
							+this.FrameBold + (rect.Top + rect.Bottom) / 2
							);
						graphics.DrawString(unit.DataIn[i].Name, this.PortFont, textBrush, portPoint, stringFormat);
					}
				}
				#endregion

				#region パラメータポート:
				{
					var port_rects = this.GetPortRects(unit, ExTaskUnitPositionType.DataParam);
					for (int i = 0; i < unit.DataParam.Length; i++)
					{
						// ポートの矩形:
						var rect = port_rects[i];
						if (this.EnableFill)
						{
							using (var portBrush = new SolidBrush(this.DataParamColor))
							{
								graphics.FillRectangle(portBrush, rect);
								graphics.DrawRectangle(bodyPen, rect);
							}
						}
						else
						{
							using (var portPen = new Pen(this.DataParamColor, this.FrameBold))
							{
								if (unit is XIE.Tasks.CxTaskReference)
									portPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								graphics.DrawRectangle(portPen, rect);
							}
						}

						// ポートの名称:
						var stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Far;		// right
						stringFormat.LineAlignment = StringAlignment.Far;	// bottom

						var portPoint = new PointF(
							-this.FrameBold + rect.Left,
							+this.FrameBold + (rect.Top + rect.Bottom) / 2
							);
						graphics.DrawString(unit.DataParam[i].Name, this.PortFont, textBrush, portPoint, stringFormat);
					}
				}
				#endregion

				#region データ出力ポート:
				{
					var port_rects = this.GetPortRects(unit, ExTaskUnitPositionType.DataOut);
					for (int i = 0; i < unit.DataOut.Length; i++)
					{
						// ポートの矩形:
						var rect = port_rects[i];
						if (this.EnableFill)
						{
							using (var portBrush = new SolidBrush(this.DataOutColor))
							{
								graphics.FillRectangle(portBrush, rect);
								graphics.DrawRectangle(bodyPen, rect);
							}
						}
						else
						{
							using (var portPen = new Pen(this.DataOutColor, this.FrameBold))
							{
								if (unit is XIE.Tasks.CxTaskReference)
									portPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								graphics.DrawRectangle(portPen, rect);
							}
						}

						// ポートの名称:
						var stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Near;		// left
						stringFormat.LineAlignment = StringAlignment.Far;	// bottom

						var portPoint = new PointF(
							+this.FrameBold + rect.Right,
							+this.FrameBold + (rect.Top + rect.Bottom) / 2
							);
						graphics.DrawString(unit.DataOut[i].Name, this.PortFont, textBrush, portPoint, stringFormat);
					}
				}
				#endregion

				#region コントロール入力ポート: (接続線)
				using (var pen = new Pen(this.ControlInColor, this.LineWidth))
				{
					try
					{
						DrawConnectionLine(graphics, pen, unit, ExTaskUnitPositionType.ControlIn);
					}
					catch (Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion

				#region データ入力ポート: (接続線)
				using (var pen = new Pen(this.DataInColor, this.LineWidth))
				{
					try
					{
						DrawConnectionLine(graphics, pen, unit, ExTaskUnitPositionType.DataIn);
					}
					catch (Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion

				#region パラメータポート: (接続線)
				using (var pen = new Pen(this.DataParamColor, this.LineWidth))
				{
					try
					{
						DrawConnectionLine(graphics, pen, unit, ExTaskUnitPositionType.DataParam);
					}
					catch (Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion
			}
		}

		/// <summary>
		/// 指定されたポートを接続する折れ線を描画します。
		/// </summary>
		/// <param name="graphics">グラフィクス</param>
		/// <param name="pen">線のペン</param>
		/// <param name="unit">対象のタスクユニット</param>
		/// <param name="type">ポート種別 [DataIn, DataParam]</param>
		private void DrawConnectionLine(Graphics graphics, Pen pen, XIE.Tasks.CxTaskUnit unit, ExTaskUnitPositionType type)
		{
			var dst_rects = this.GetPortRects(unit, type);
			for (int i = 0; i < dst_rects.Length; i++)
			{
				XIE.Tasks.CxTaskUnit src_unit = null;
				XIE.Tasks.CxTaskPortOut src_port = null;
				XIE.Tasks.CxTaskUnit dst_unit = null;
				XIE.Tasks.CxTaskPortIn dst_port = null;
				int src_index = -1;
				var src_type = ExTaskUnitPositionType.None;

				switch(type)
				{
					default:
						break;
					case ExTaskUnitPositionType.ControlIn:
						dst_unit = unit;
						dst_port = unit.ControlIn;
						if (dst_port != null && dst_port.IsConnected)
						{
							src_unit = dst_port.ReferenceTask;
							src_port = dst_port.ReferencePort;
							src_index = (src_unit.ControlOut == src_port) ? 0 : -1;
							src_type = ExTaskUnitPositionType.ControlOut;
						}
						break;
					case ExTaskUnitPositionType.DataIn:
						dst_unit = unit;
						dst_port = unit.DataIn[i];
						if (dst_port != null && dst_port.IsConnected)
						{
							src_unit = dst_port.ReferenceTask;
							src_port = dst_port.ReferencePort;
							src_index = Array.IndexOf(src_unit.DataOut, src_port);
							src_type = ExTaskUnitPositionType.DataOut;
						}
						break;
					case ExTaskUnitPositionType.DataParam:
						dst_unit = unit;
						dst_port = unit.DataParam[i];
						if (dst_port != null && dst_port.IsConnected)
						{
							src_unit = dst_port.ReferenceTask;
							src_port = dst_port.ReferencePort;
							src_index = Array.IndexOf(src_unit.DataOut, src_port);
							src_type = ExTaskUnitPositionType.DataOut;
						}
						break;
				}

				if (src_index >= 0)
				{
					var src_rects = this.GetPortRects(src_unit, src_type);
					var src_rect = src_rects[src_index];
					var src_size = graphics.MeasureString(src_port.Name, this.PortFont);
					var src_width = (int)System.Math.Ceiling(src_size.Width);

					var dst_rect = dst_rects[i];
					var dst_size = graphics.MeasureString(dst_port.Name, this.PortFont);
					var dst_width = (int)System.Math.Ceiling(dst_size.Width);

					var src = new Point(src_rect.Right + this.FrameBold, (src_rect.Top + src_rect.Bottom) / 2);
					var dst = new Point(dst_rect.Left - this.FrameBold, (dst_rect.Top + dst_rect.Bottom) / 2);
					var src_bounds = GetBounds(src_unit);
					var dst_bounds = GetBounds(dst_unit);
					var points = CreateConnectionLine(src, src_width, src_bounds, dst, dst_width, dst_bounds);

					switch (this.LineType)
					{
						default:
						case 0:
							graphics.DrawLines(pen, points);
							break;
						case 1:
							graphics.DrawCurve(pen, points);
							break;
					}
				}
			}
		}

		/// <summary>
		/// 指定された２点を接続する折れ線を生成します。
		/// </summary>
		/// <param name="src">始点</param>
		/// <param name="src_width">始点の文字幅</param>
		/// <param name="src_bounds">始点の本体の外接矩形</param>
		/// <param name="dst">終点</param>
		/// <param name="dst_width">終点の文字幅</param>
		/// <param name="dst_bounds">終点の本体の外接矩形</param>
		/// <returns>
		///		接続線を返します。
		/// </returns>
		private Point[] CreateConnectionLine(Point src, int src_width, Rectangle src_bounds, Point dst, int dst_width, Rectangle dst_bounds)
		{
			var st = new Point(src.X + src_width, src.Y);
			var ed = new Point(dst.X - dst_width, dst.Y);
			var center = new Point((src.X + dst.X) / 2, (src.Y + dst.Y) / 2);
			var diff = new Point(ed.X - st.X, ed.Y - st.Y);
			if (st.X < ed.X)
			{
				#region 始点 → 終点: (文字列が重ならない)
				var points = new Point[] {
						src,
						new Point(st.X, src.Y),
						new Point(ed.X, dst.Y),
						dst,
					};
				return points;
				#endregion
			}
			else if (src.X < dst.X)
			{
				#region 始点 → 終点: (文字列が重なるが、ポートが重ならない)
				var points = new Point[] {
							src,
							new Point(st.X, src.Y),
							new Point(st.X, center.Y),
							new Point(ed.X, center.Y),
							new Point(ed.X, dst.Y),
							dst,
						};
				return points;
				#endregion
			}
			else
			{
				#region 終点 ← 始点:
				if (src_bounds.Bottom < dst_bounds.Top)
				{
					#region 本体が重ならない: (始点が上に有る)
					var middle = (src_bounds.Bottom + dst_bounds.Top) / 2;
					var points = new Point[] {
							src,
							new Point(st.X, src.Y),
							new Point(st.X, middle),
							new Point(ed.X, middle),
							new Point(ed.X, dst.Y),
							dst,
						};
					return points;
					#endregion
				}
				else if (src_bounds.Top > dst_bounds.Bottom)
				{
					#region 本体が重ならない: (終点が上に有る)
					var middle = (src_bounds.Top + dst_bounds.Bottom) / 2;
					var points = new Point[] {
							src,
							new Point(st.X, src.Y),
							new Point(st.X, middle),
							new Point(ed.X, middle),
							new Point(ed.X, dst.Y),
							dst,
						};
					return points;
					#endregion
				}
				else if (src_bounds.Top <= dst_bounds.Bottom)
				{
					#region 本体が重なる: (始点の上辺に重なる)
					var top = System.Math.Min(src_bounds.Top, dst_bounds.Top) - 16;
					var points = new Point[] {
							src,
							new Point(st.X, src.Y),
							new Point(st.X, top),
							new Point(ed.X, top),
							new Point(ed.X, dst.Y),
							dst,
						};
					return points;
					#endregion
				}
				else
				{
					#region 本体が重なる: (終点の上辺に重なる)
					var bottom = System.Math.Min(src_bounds.Bottom, dst_bounds.Bottom) + 16;
					var points = new Point[] {
							src,
							new Point(st.X, src.Y),
							new Point(st.X, bottom),
							new Point(ed.X, bottom),
							new Point(ed.X, dst.Y),
							dst,
						};
					return points;
					#endregion
				}
				#endregion
			}
		}

		#endregion

		#region メソッド: (操作)

		/// <summary>
		/// ヒット判定
		/// </summary>
		/// <param name="unit">対象のタスクユニット</param>
		/// <param name="click_position">クリック位置</param>
		/// <returns>
		///		クリック位置情報を返します。
		/// </returns>
		public TxTaskUnitPosition HitTest(XIE.Tasks.CxTaskUnit unit, Point click_position)
		{
			var bounds = this.GetBounds(unit);
			click_position = GetActualPoint(click_position);

			var error_range = System.Math.Sqrt(
					this.BlockSize.Width * this.BlockSize.Width +
					this.BlockSize.Height * this.BlockSize.Height
					);
			var results = new List<TxTaskUnitPosition>();

			#region 本体:
			{
				var type = ExTaskUnitPositionType.Body;
				if (bounds.Left <= click_position.X && click_position.X < bounds.Right &&
					bounds.Top <= click_position.Y && click_position.Y < bounds.Bottom)
				{
					var center = new XIE.TxPointD(
						(bounds.Right + bounds.Left) * 0.5,
						(bounds.Bottom + bounds.Top) * 0.5
						);
					var diff_x = center.X - click_position.X;
					var diff_y = center.Y - click_position.Y;
					var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
					var result = new TxTaskUnitPosition(type, 0, distance);
					return result;
				}
			}
			#endregion

			#region コントロール入力:
			{
				var type = ExTaskUnitPositionType.ControlIn;
				var port_rects = this.GetPortRects(unit, type);
				for (int i = 0; i < port_rects.Length; i++)
				{
					var rect = port_rects[i];
					{
						var center = new XIE.TxPointD(
							(rect.Right + rect.Left) * 0.5,
							(rect.Bottom + rect.Top) * 0.5
							);
						var diff_x = center.X - click_position.X;
						var diff_y = center.Y - click_position.Y;
						var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
						if (distance <= error_range)
							results.Add(new TxTaskUnitPosition(type, i, distance));
					}
				}
			}
			#endregion

			#region コントロール出力:
			{
				var type = ExTaskUnitPositionType.ControlOut;
				var port_rects = this.GetPortRects(unit, type);
				for (int i = 0; i < port_rects.Length; i++)
				{
					var rect = port_rects[i];
					{
						var center = new XIE.TxPointD(
							(rect.Right + rect.Left) * 0.5,
							(rect.Bottom + rect.Top) * 0.5
							);
						var diff_x = center.X - click_position.X;
						var diff_y = center.Y - click_position.Y;
						var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
						if (distance <= error_range)
							results.Add(new TxTaskUnitPosition(type, i, distance));
					}
				}
			}
			#endregion

			#region データ入力:
			{
				var type = ExTaskUnitPositionType.DataIn;
				var port_rects = this.GetPortRects(unit, type);
				for (int i = 0; i < port_rects.Length; i++)
				{
					var rect = port_rects[i];
					{
						var center = new XIE.TxPointD(
							(rect.Right + rect.Left) * 0.5,
							(rect.Bottom + rect.Top) * 0.5
							);
						var diff_x = center.X - click_position.X;
						var diff_y = center.Y - click_position.Y;
						var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
						if (distance <= error_range)
							results.Add(new TxTaskUnitPosition(type, i, distance));
					}
				}
			}
			#endregion

			#region パラメータ:
			{
				var type = ExTaskUnitPositionType.DataParam;
				var port_rects = this.GetPortRects(unit, type);
				for (int i = 0; i < port_rects.Length; i++)
				{
					var rect = port_rects[i];
					{
						var center = new XIE.TxPointD(
							(rect.Right + rect.Left) * 0.5,
							(rect.Bottom + rect.Top) * 0.5
							);
						var diff_x = center.X - click_position.X;
						var diff_y = center.Y - click_position.Y;
						var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
						if (distance <= error_range)
							results.Add(new TxTaskUnitPosition(type, i, distance));
					}
				}
			}
			#endregion

			#region データ出力:
			{
				var type = ExTaskUnitPositionType.DataOut;
				var port_rects = this.GetPortRects(unit, type);
				for (int i = 0; i < port_rects.Length; i++)
				{
					var rect = port_rects[i];
					{
						var center = new XIE.TxPointD(
							(rect.Right + rect.Left) * 0.5,
							(rect.Bottom + rect.Top) * 0.5
							);
						var diff_x = center.X - click_position.X;
						var diff_y = center.Y - click_position.Y;
						var distance = System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
						if (distance <= error_range)
							results.Add(new TxTaskUnitPosition(type, i, distance));
					}
				}
			}
			#endregion

			#region 最小距離のポートの検索:
			{
				var distances = new List<double>();
				foreach (var item in results)
					distances.Add(item.Distance);
				var stat = XIE.TxStatistics.From(distances);
				int index = results.FindIndex(
					delegate(TxTaskUnitPosition item)
					{
						return (item.Distance == stat.Min);
					});
				if (index >= 0)
					return results[index];
			}
			#endregion

			return new TxTaskUnitPosition();
		}

		/// <summary>
		/// 現在の表示倍率でスケーリング(拡大)して等倍表示の点座標を取得します。
		/// </summary>
		/// <param name="location">変換対象の座標 (スケーリング(縮小)された座標)</param>
		/// <returns>
		///		計算結果を返します。
		/// </returns>
		public Point GetActualPoint(Point location)
		{
			var mag = (double)this.Scale / 100;
			var point = location;
			point.X = (int)(point.X / mag);
			point.Y = (int)(point.Y / mag);
			return point;
		}

		#endregion
	}

	#region CxTaskHandlingInfo

	/// <summary>
	/// タスク操作情報
	/// </summary>
	public class CxTaskHandlingInfo
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskHandlingInfo()
		{
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// グリップ解除
		/// </summary>
		public void Reset()
		{
			this.TimeStamp = DateTime.Now;
			this.GripPosition.Type = ExTaskUnitPositionType.None;
			this.Button = MouseButtons.None;
			this.Location = new Point();
			this.FocusedUnit = null;
			this.SelectedPrev = false;
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="grip_pos"></param>
		/// <param name="button"></param>
		/// <param name="location"></param>
		public void Update(XIE.Tasks.CxTaskUnit unit, TxTaskUnitPosition grip_pos, MouseButtons button, Point location)
		{
			this.TimeStamp = DateTime.Now;
			this.GripPosition = grip_pos;
			this.ModifierKeys = Control.ModifierKeys;
			this.Button = button;
			this.Location = location;
			this.FocusedUnit = unit;
			this.SelectedPrev = (unit != null) ? unit.Selected : false;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// グリップしたときの時刻
		/// </summary>
		public DateTime TimeStamp = DateTime.Now;

		/// <summary>
		/// グリップしている箇所
		/// </summary>
		public TxTaskUnitPosition GripPosition = new TxTaskUnitPosition();

		/// <summary>
		/// グリップした時のキーボードの押下状態
		/// </summary>
		public Keys ModifierKeys = Keys.None;

		/// <summary>
		/// グリップした時のマウスのボタン
		/// </summary>
		public MouseButtons Button = MouseButtons.None;

		/// <summary>
		/// グリップした時のマウスの位置 (Client 座標)
		/// </summary>
		public Point Location = new Point();

		/// <summary>
		/// 現在フォーカスを持つタスクユニット
		/// </summary>
		public XIE.Tasks.CxTaskUnit FocusedUnit = null;

		/// <summary>
		/// グリップする前の選択状態
		/// </summary>
		public bool SelectedPrev = false;

		#endregion
	}

	#endregion

	#region ExTaskUnitPositionType

	/// <summary>
	/// タスクユニットのクリック位置種別
	/// </summary>
	public enum ExTaskUnitPositionType
	{
		/// <summary>
		/// なし
		/// </summary>
		None,

		/// <summary>
		/// 本体
		/// </summary>
		Body,

		/// <summary>
		/// コントロール入力ポート
		/// </summary>
		ControlIn,

		/// <summary>
		/// コントロール出力ポート
		/// </summary>
		ControlOut,

		/// <summary>
		/// データ入力ポート
		/// </summary>
		DataIn,

		/// <summary>
		/// パラメータポート
		/// </summary>
		DataParam,

		/// <summary>
		/// データ出力ポート
		/// </summary>
		DataOut,
	}

	#endregion

	#region TxTaskUnitPosition

	/// <summary>
	/// タスクユニットのクリック位置情報
	/// </summary>
	public struct TxTaskUnitPosition
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">種別</param>
		/// <param name="index">指標 [0~]</param>
		/// <param name="distance">中心からの距離 [0~]</param>
		public TxTaskUnitPosition(ExTaskUnitPositionType type = ExTaskUnitPositionType.None, int index = 0, double distance = 0)
		{
			Type = type;
			Index = index;
			Distance = distance;
		}

		/// <summary>
		/// 種別
		/// </summary>
		public ExTaskUnitPositionType Type;

		/// <summary>
		/// 指標 [0~]
		/// </summary>
		public int Index;

		/// <summary>
		/// 中心からの距離 [0~]
		/// </summary>
		public double Distance;
	}

	#endregion
}
