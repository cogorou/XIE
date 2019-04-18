/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Diagnostics;
using System.CodeDom;

namespace XIE.Tasks
{
	#region TxImageSize.ctor

	/// <summary>
	/// 画像サイズ構造体を生成します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxImageSize_ctor : CxTaskUnit
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxImageSize_ctor()
			: base()
		{
			this.Category = "XIE.TxImageSize";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Width", new Type[] { typeof(int) }),
				new CxTaskPortIn("Height", new Type[] { typeof(int) }),
				new CxTaskPortIn("Model", new Type[] { typeof(TxModel) }),
				new CxTaskPortIn("Channels", new Type[] { typeof(int) }),
				new CxTaskPortIn("Depth", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxImageSize) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 幅 (pixel) [1~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 高さ (pixel) [1~]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 要素モデル
			/// </summary>
			DataParam2,

			/// <summary>
			/// チャネル数 [1~16]
			/// </summary>
			DataParam3,

			/// <summary>
			/// ビット深度 (bit) [0,1~64]
			/// </summary>
			DataParam4,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxImageSize_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxImageSize_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxImageSize_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.Width")]
		public int Width
		{
			get { return m_This.Width; }
			set { m_This.Width = value; }
		}

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.Height")]
		public int Height
		{
			get { return m_This.Height; }
			set { m_This.Height = value; }
		}

		/// <summary>
		/// 要素モデル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.Model")]
		public TxModel Model
		{
			get { return m_This.Model; }
			set { m_This.Model = value; }
		}

		/// <summary>
		/// チャネル数 [範囲:1~16]
		/// </summary>
		[TypeConverter(typeof(ChannelsConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.Channels")]
		public int Channels
		{
			get { return m_This.Channels; }
			set { m_This.Channels = value; }
		}

		#region ChannelsConverter

		class ChannelsConverter : Int32Converter
		{
			public ChannelsConverter()
			{
			}
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}
			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				try
				{
					var items = new List<int>();
					for (int i = 0; i < XIE.Defs.XIE_IMAGE_CHANNELS_MAX; i++)
						items.Add(i + 1);
					return new StandardValuesCollection(items);
				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}

		#endregion

		/// <summary>
		/// ビット深度 [範囲:0=最大値、1~=指定値]
		/// </summary>
		[TypeConverter(typeof(DepthConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.Depth")]
		public int Depth
		{
			get { return m_This.Depth; }
			set { m_This.Depth = value; }
		}

		#region DepthConverter

		class DepthConverter : Int32Converter
		{
			public DepthConverter()
			{
			}
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}
			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				try
				{
					var items = new List<int>();
					for (int i = 0; i <= 64; i++)
						items.Add(i);
					return new StandardValuesCollection(items);
				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}

		#endregion

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 画像サイズ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxImageSize_ctor.This")]
		public TxImageSize This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxImageSize m_This = new TxImageSize();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Width = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Height = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Model = (TxModel)this.DataParam[iii].Data; break;
						case 3: this.Channels = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.Depth = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxImageSize)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Width));
					var height = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Height));
					var model = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Model));
					var channels = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Channels));
					var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Depth));

					// task#_This = new XIE.TxImageSize(width, height, model, channels, depth);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxImageSize), width, height, model, channels, depth)
						));
				}
			}
		}

		#endregion

		#region IxTaskControlPanel の実装:

		/// <summary>
		/// コントロールパネルの生成
		/// </summary>
		/// <param name="args">実行時引数</param>
		/// <param name="options">オプション (使用しません。)</param>
		/// <returns>
		///		モードレスダイアログの場合は生成したコントロールパネルを返します。
		///		モーダルダイアログの場合は内部で表示して完了後に null を返します。
		/// </returns>
		Form IxTaskControlPanel.Create(CxTaskExecuteEventArgs args, params object[] options)
		{
			var dlg = new TxImageSizeForm();
			dlg.ImageSize = this.This;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.This = dlg.ImageSize;
			}
			return null;
		}

		/// <summary>
		/// コントロールパネルの表示状態
		/// </summary>
		/// <remarks>
		///		モードレスダイアログの表示中は true 、未表示の場合は false を返します。
		///		モーダルダイアログの場合は常に false を返します。
		/// </remarks>
		bool IxTaskControlPanel.IsOpened
		{
			get { return false; }
		}

		#endregion
	}

	#endregion

	#region TxImageSize.Properties

	/// <summary>
	/// 画像サイズ構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxImageSize_Properties : CxTaskUnit
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxImageSize_Properties()
			: base()
		{
			this.Category = "XIE.TxImageSize";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxImageSize) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Width", new Type[] { typeof(int) }),
				new CxTaskPortOut("Height", new Type[] { typeof(int) }),
				new CxTaskPortOut("Model", new Type[] { typeof(TxModel) }),
				new CxTaskPortOut("Channels", new Type[] { typeof(int) }),
				new CxTaskPortOut("Depth", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 幅 (pixel) [1~]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 高さ (pixel) [1~]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 要素モデル
			/// </summary>
			DataOut2,

			/// <summary>
			/// チャネル数 [1~16]
			/// </summary>
			DataOut3,

			/// <summary>
			/// ビット深度 (bit) [0,1~64]
			/// </summary>
			DataOut4,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxImageSize_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxImageSize_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxImageSize_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 画像サイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxImageSize_Properties.This")]
		[Editor(typeof(ImageSizeEditor), typeof(UITypeEditor))]
		public TxImageSize This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxImageSize m_This = new TxImageSize();

		#region ImageSizeEditor

		private class ImageSizeEditor : UITypeEditor
		{
			public ImageSizeEditor()
			{
			}
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				try
				{
					var fes = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
					if (fes == null)
						return null;

					var dlg = new TxImageSizeForm();
					dlg.ImageSize = (TxImageSize)value;
					dlg.StartPosition = FormStartPosition.Manual;
					dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
					if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
						return dlg.ImageSize;
					return value;
				}
				catch (Exception)
				{
					return base.EditValue(context, provider, value);
				}
			}
		}

		#endregion

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxImageSize)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Width;
			this.DataOut[1].Data = this.This.Height;
			this.DataOut[2].Data = this.This.Model;
			this.DataOut[3].Data = this.This.Channels;
			this.DataOut[4].Data = this.This.Depth;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
				case 1:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
				case 2:
					{
						target_port.Data = (TxModel)value;
						return;
					}
				case 3:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
				case 4:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Width = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Width))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Height))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Model))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Channels))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.Depth))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Width = task$.Width;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Width"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Height"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Model"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Channels"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("Depth"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskControlPanel の実装:

		/// <summary>
		/// コントロールパネルの生成
		/// </summary>
		/// <param name="args">実行時引数</param>
		/// <param name="options">オプション (使用しません。)</param>
		/// <returns>
		///		モードレスダイアログの場合は生成したコントロールパネルを返します。
		///		モーダルダイアログの場合は内部で表示して完了後に null を返します。
		/// </returns>
		Form IxTaskControlPanel.Create(CxTaskExecuteEventArgs args, params object[] options)
		{
			var dlg = new TxImageSizeForm();
			dlg.ImageSize = this.This;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.This = dlg.ImageSize;
			}
			return null;
		}

		/// <summary>
		/// コントロールパネルの表示状態
		/// </summary>
		/// <remarks>
		///		モードレスダイアログの表示中は true 、未表示の場合は false を返します。
		///		モーダルダイアログの場合は常に false を返します。
		/// </remarks>
		bool IxTaskControlPanel.IsOpened
		{
			get { return false; }
		}

		#endregion
	}

	#endregion

	#region TxModel.ctor

	/// <summary>
	/// 要素モデル構造体を生成します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxModel_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxModel_ctor()
			: base()
		{
			this.Category = "XIE.TxModel";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Type", new Type[] { typeof(ExType), typeof(int) }),
				new CxTaskPortIn("Pack", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxModel) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 型
			/// </summary>
			DataParam0,

			/// <summary>
			/// パック数 [1~]
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxModel_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxModel_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxModel_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxModel_ctor.Type")]
		public ExType Type
		{
			get { return m_This.Type; }
			set { m_This.Type = value; }
		}

		/// <summary>
		/// パック数 [範囲:0,1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxModel_ctor.Pack")]
		public int Pack
		{
			get { return m_This.Pack; }
			set { m_This.Pack = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 要素モデル
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxModel_ctor.This")]
		public TxModel This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxModel m_This = new TxModel();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Type = (ExType)(this.DataParam[iii].Data); break;
						case 1: this.Pack = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxModel)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var type = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Type));
					var pack = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Pack));

					// task#_This = new XIE.TxModel(type, pack);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxModel), type, pack)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxModel.Properties

	/// <summary>
	/// 要素モデル構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxModel_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxModel_Properties()
			: base()
		{
			this.Category = "XIE.TxModel";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxModel) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Type", new Type[] { typeof(ExType) }),
				new CxTaskPortOut("Pack", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 型
			/// </summary>
			DataOut0,

			/// <summary>
			/// パック数 [1~]
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxModel_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxModel_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxModel_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素モデル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxModel_Properties.This")]
		public TxModel This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxModel m_This = new TxModel();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxModel)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Type;
			this.DataOut[1].Data = this.This.Pack;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = (ExType)value;
						return;
					}
				case 1:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Type = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Type))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Pack))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Type = task$.Type;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Type"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Pack"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRGB8x3.ctor

	/// <summary>
	/// RGB 構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRGB8x3_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRGB8x3_ctor()
			: base()
		{
			this.Category = "XIE.TxRGB8x3";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("R", new Type[] { typeof(byte) }),
				new CxTaskPortIn("G", new Type[] { typeof(byte) }),
				new CxTaskPortIn("B", new Type[] { typeof(byte) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRGB8x3) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRGB8x3_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRGB8x3_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRGB8x3_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x3_ctor.R")]
		public byte R
		{
			get { return m_This.R; }
			set { m_This.R = value; }
		}

		/// <summary>
		/// 緑成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x3_ctor.G")]
		public byte G
		{
			get { return m_This.G; }
			set { m_This.G = value; }
		}

		/// <summary>
		/// 青成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x3_ctor.B")]
		public byte B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// RGB 値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRGB8x3_ctor.This")]
		public TxRGB8x3 This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRGB8x3 m_This = new TxRGB8x3();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.R = Convert.ToByte(this.DataParam[iii].Data); break;
						case 1: this.G = Convert.ToByte(this.DataParam[iii].Data); break;
						case 2: this.B = Convert.ToByte(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRGB8x3)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var r = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.R));
					var g = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.G));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.B));

					// task#_This = new XIE.TxRGB8x3(r, g, b);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRGB8x3), r, g, b)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRGB8x3.Properties

	/// <summary>
	/// RGB 構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRGB8x3_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRGB8x3_Properties()
			: base()
		{
			this.Category = "XIE.TxRGB8x3";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRGB8x3) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("R", new Type[] { typeof(byte) }),
				new CxTaskPortOut("G", new Type[] { typeof(byte) }),
				new CxTaskPortOut("B", new Type[] { typeof(byte) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRGB8x3_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRGB8x3_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRGB8x3_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// RGB 値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x3_Properties.This")]
		public TxRGB8x3 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRGB8x3 m_This = new TxRGB8x3();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRGB8x3)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.R;
			this.DataOut[1].Data = this.This.G;
			this.DataOut[2].Data = this.This.B;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToByte(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (byte i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.R))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.G))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (byte i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_R = task$.R;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("R"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("G"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("B"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRGB8x4.ctor

	/// <summary>
	/// RGB 構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRGB8x4_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRGB8x4_ctor()
			: base()
		{
			this.Category = "XIE.TxRGB8x4";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("R", new Type[] { typeof(byte) }),
				new CxTaskPortIn("G", new Type[] { typeof(byte) }),
				new CxTaskPortIn("B", new Type[] { typeof(byte) }),
				new CxTaskPortIn("A", new Type[] { typeof(byte) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRGB8x4) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataParam2,

			/// <summary>
			/// アルファ [0~255]
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRGB8x4_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRGB8x4_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRGB8x4_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_ctor.R")]
		public byte R
		{
			get { return m_This.R; }
			set { m_This.R = value; }
		}

		/// <summary>
		/// 緑成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_ctor.G")]
		public byte G
		{
			get { return m_This.G; }
			set { m_This.G = value; }
		}

		/// <summary>
		/// 青成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_ctor.B")]
		public byte B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		/// <summary>
		/// アルファ [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_ctor.A")]
		public byte A
		{
			get { return m_This.A; }
			set { m_This.A = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// RGB 値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_ctor.This")]
		public TxRGB8x4 This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRGB8x4 m_This = new TxRGB8x4();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.R = Convert.ToByte(this.DataParam[iii].Data); break;
						case 1: this.G = Convert.ToByte(this.DataParam[iii].Data); break;
						case 2: this.B = Convert.ToByte(this.DataParam[iii].Data); break;
						case 3: this.A = Convert.ToByte(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRGB8x4)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var r = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.R));
					var g = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.G));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.B));
					var a = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.A));

					// task#_This = new XIE.TxRGB8x4(r, g, b, a);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRGB8x4), r, g, b, a)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRGB8x4.Properties

	/// <summary>
	/// RGB 構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRGB8x4_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRGB8x4_Properties()
			: base()
		{
			this.Category = "XIE.TxRGB8x4";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRGB8x4) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("R", new Type[] { typeof(byte) }),
				new CxTaskPortOut("G", new Type[] { typeof(byte) }),
				new CxTaskPortOut("B", new Type[] { typeof(byte) }),
				new CxTaskPortOut("A", new Type[] { typeof(byte) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataOut2,

			/// <summary>
			/// アルファ [0~255]
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRGB8x4_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRGB8x4_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRGB8x4_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// RGB 値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRGB8x4_Properties.This")]
		public TxRGB8x4 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRGB8x4 m_This = new TxRGB8x4();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRGB8x4)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.R;
			this.DataOut[1].Data = this.This.G;
			this.DataOut[2].Data = this.This.B;
			this.DataOut[3].Data = this.This.A;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToByte(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (byte i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.R))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.G))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.A))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (byte i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_R = task$.R;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("R"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("G"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("B"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("A"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxBGR8x3.ctor

	/// <summary>
	/// BGR 構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxBGR8x3_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxBGR8x3_ctor()
			: base()
		{
			this.Category = "XIE.TxBGR8x3";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("R", new Type[] { typeof(byte) }),
				new CxTaskPortIn("G", new Type[] { typeof(byte) }),
				new CxTaskPortIn("B", new Type[] { typeof(byte) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxBGR8x3) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxBGR8x3_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxBGR8x3_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxBGR8x3_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x3_ctor.R")]
		public byte R
		{
			get { return m_This.R; }
			set { m_This.R = value; }
		}

		/// <summary>
		/// 緑成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x3_ctor.G")]
		public byte G
		{
			get { return m_This.G; }
			set { m_This.G = value; }
		}

		/// <summary>
		/// 青成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x3_ctor.B")]
		public byte B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// RGB 値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxBGR8x3_ctor.This")]
		public TxBGR8x3 This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxBGR8x3 m_This = new TxBGR8x3();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.R = Convert.ToByte(this.DataParam[iii].Data); break;
						case 1: this.G = Convert.ToByte(this.DataParam[iii].Data); break;
						case 2: this.B = Convert.ToByte(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxBGR8x3)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var r = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.R));
					var g = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.G));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.B));

					// task#_This = new XIE.TxBGR8x3(r, g, b);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxBGR8x3), r, g, b)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxBGR8x3.Properties

	/// <summary>
	/// BGR 構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxBGR8x3_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxBGR8x3_Properties()
			: base()
		{
			this.Category = "XIE.TxBGR8x3";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxBGR8x3) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("R", new Type[] { typeof(byte) }),
				new CxTaskPortOut("G", new Type[] { typeof(byte) }),
				new CxTaskPortOut("B", new Type[] { typeof(byte) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxBGR8x3_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxBGR8x3_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxBGR8x3_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// RGB 値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x3_Properties.This")]
		public TxBGR8x3 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxBGR8x3 m_This = new TxBGR8x3();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxBGR8x3)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.R;
			this.DataOut[1].Data = this.This.G;
			this.DataOut[2].Data = this.This.B;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToByte(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (byte i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.R))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.G))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (byte i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_R = task$.R;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("R"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("G"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("B"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxBGR8x4.ctor

	/// <summary>
	/// BGR 構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxBGR8x4_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxBGR8x4_ctor()
			: base()
		{
			this.Category = "XIE.TxBGR8x4";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("R", new Type[] { typeof(byte) }),
				new CxTaskPortIn("G", new Type[] { typeof(byte) }),
				new CxTaskPortIn("B", new Type[] { typeof(byte) }),
				new CxTaskPortIn("A", new Type[] { typeof(byte) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxBGR8x4) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataParam2,

			/// <summary>
			/// アルファ [0~255]
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxBGR8x4_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxBGR8x4_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxBGR8x4_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_ctor.R")]
		public byte R
		{
			get { return m_This.R; }
			set { m_This.R = value; }
		}

		/// <summary>
		/// 緑成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_ctor.G")]
		public byte G
		{
			get { return m_This.G; }
			set { m_This.G = value; }
		}

		/// <summary>
		/// 青成分 [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_ctor.B")]
		public byte B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		/// <summary>
		/// アルファ [0~255]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_ctor.A")]
		public byte A
		{
			get { return m_This.A; }
			set { m_This.A = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// BGR 値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_ctor.This")]
		public TxBGR8x4 This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxBGR8x4 m_This = new TxBGR8x4();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.R = Convert.ToByte(this.DataParam[iii].Data); break;
						case 1: this.G = Convert.ToByte(this.DataParam[iii].Data); break;
						case 2: this.B = Convert.ToByte(this.DataParam[iii].Data); break;
						case 3: this.A = Convert.ToByte(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxBGR8x4)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var r = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.R));
					var g = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.G));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.B));
					var a = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.A));

					// task#_This = new XIE.TxBGR8x4(r, g, b, a);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxBGR8x4), r, g, b, a)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxBGR8x4.Properties

	/// <summary>
	/// BGR 構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxBGR8x4_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxBGR8x4_Properties()
			: base()
		{
			this.Category = "XIE.TxBGR8x4";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxBGR8x4) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("R", new Type[] { typeof(byte) }),
				new CxTaskPortOut("G", new Type[] { typeof(byte) }),
				new CxTaskPortOut("B", new Type[] { typeof(byte) }),
				new CxTaskPortOut("A", new Type[] { typeof(byte) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataOut2,

			/// <summary>
			/// アルファ [0~255]
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxBGR8x4_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxBGR8x4_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxBGR8x4_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// BGR 値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxBGR8x4_Properties.This")]
		public TxBGR8x4 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxBGR8x4 m_This = new TxBGR8x4();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxBGR8x4)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.R;
			this.DataOut[1].Data = this.This.G;
			this.DataOut[2].Data = this.This.B;
			this.DataOut[3].Data = this.This.A;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToByte(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (byte i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.R))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.G))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.A))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (byte i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_R = task$.R;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("R"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("G"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("B"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("A"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxPointD.ctor

	/// <summary>
	/// 点構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxPointD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxPointD_ctor()
			: base()
		{
			this.Category = "XIE.TxPointD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxPointD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxPointD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxPointD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxPointD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 点座標
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxPointD_ctor.This")]
		public TxPointD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxPointD m_This = new TxPointD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data);  break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxPointD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));

					// task#_This = new XIE.TxPointD(x, y);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxPointD), x, y)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiPoint(This);

			var w = e.Canvas.ImageSize.Width / 50;
			var h = e.Canvas.ImageSize.Height / 50;
			var max_size = System.Math.Max(w, h);

			figure.PenColor = Color.Blue;
			figure.AnchorSize = new TxSizeD(max_size, max_size);
			figure.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Cross;

			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxPointD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxPointD.Properties

	/// <summary>
	/// 点構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxPointD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxPointD_Properties()
			: base()
		{
			this.Category = "XIE.TxPointD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxPointD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxPointD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxPointD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxPointD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 点座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointD_Properties.This")]
		public TxPointD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxPointD m_This = new TxPointD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxPointD)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiPoint(This);

			var w = e.Canvas.ImageSize.Width / 50;
			var h = e.Canvas.ImageSize.Height / 50;
			var max_size = System.Math.Max(w, h);

			figure.PenColor = Color.Blue;
			figure.AnchorSize = new TxSizeD(max_size, max_size);
			figure.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Cross;

			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxPointD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxPointI.ctor

	/// <summary>
	/// 点構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxPointI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxPointI_ctor()
			: base()
		{
			this.Category = "XIE.TxPointI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxPointI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxPointI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxPointI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxPointI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 点座標
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxPointI_ctor.This")]
		public TxPointI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxPointI m_This = new TxPointI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxPointI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));

					// task#_This = new XIE.TxPointI(x, y);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxPointI), x, y)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiPoint(This);

			var w = e.Canvas.ImageSize.Width / 50;
			var h = e.Canvas.ImageSize.Height / 50;
			var max_size = System.Math.Max(w, h);

			figure.PenColor = Color.Blue;
			figure.AnchorSize = new TxSizeD(max_size, max_size);
			figure.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Cross;

			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxPointI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxPointI.Properties

	/// <summary>
	/// 点構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxPointI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxPointI_Properties()
			: base()
		{
			this.Category = "XIE.TxPointI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxPointI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxPointI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxPointI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxPointI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 点座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxPointI_Properties.This")]
		public TxPointI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxPointI m_This = new TxPointI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxPointI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiPoint(This);

			var w = e.Canvas.ImageSize.Width / 50;
			var h = e.Canvas.ImageSize.Height / 50;
			var max_size = System.Math.Max(w, h);

			figure.PenColor = Color.Blue;
			figure.AnchorSize = new TxSizeD(max_size, max_size);
			figure.AnchorStyle = XIE.GDI.ExGdiAnchorStyle.Cross;

			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxPointI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxSizeD.ctor

	/// <summary>
	/// サイズ構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSizeD_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSizeD_ctor()
			: base()
		{
			this.Category = "XIE.TxSizeD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Width", new Type[] { typeof(double) }),
				new CxTaskPortIn("Height", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxSizeD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 幅
			/// </summary>
			DataParam0,

			/// <summary>
			/// 高さ
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxSizeD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxSizeD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxSizeD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeD_ctor.Width")]
		public double Width
		{
			get { return m_This.Width; }
			set { m_This.Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeD_ctor.Height")]
		public double Height
		{
			get { return m_This.Height; }
			set { m_This.Height = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// サイズ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxSizeD_ctor.This")]
		public TxSizeD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxSizeD m_This = new TxSizeD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Width = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Height = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxSizeD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Width));
					var height = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Height));

					// task#_This = new XIE.TxSizeD(width, height);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxSizeD), width, height)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxSizeD.Properties

	/// <summary>
	/// サイズ構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSizeD_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSizeD_Properties()
			: base()
		{
			this.Category = "XIE.TxSizeD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxSizeD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Width", new Type[] { typeof(double) }),
				new CxTaskPortOut("Height", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 幅
			/// </summary>
			DataOut0,

			/// <summary>
			/// 高さ
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxSizeD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxSizeD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxSizeD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// サイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeD_Properties.This")]
		public TxSizeD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxSizeD m_This = new TxSizeD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxSizeD)(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Width;
			this.DataOut[1].Data = this.This.Height;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Width = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Width))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Height))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Width = task$.Width;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Width"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Height"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxSizeI.ctor

	/// <summary>
	/// サイズ構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSizeI_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSizeI_ctor()
			: base()
		{
			this.Category = "XIE.TxSizeI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Width", new Type[] { typeof(int) }),
				new CxTaskPortIn("Height", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxSizeI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 幅
			/// </summary>
			DataParam0,

			/// <summary>
			/// 高さ
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxSizeI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxSizeI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxSizeI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeI_ctor.Width")]
		public int Width
		{
			get { return m_This.Width; }
			set { m_This.Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeI_ctor.Height")]
		public int Height
		{
			get { return m_This.Height; }
			set { m_This.Height = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// サイズ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxSizeI_ctor.This")]
		public TxSizeI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxSizeI m_This = new TxSizeI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Width = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Height = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxSizeI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					// width
					var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Width));

					// height
					var height = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Height));

					// task#_This = new XIE.TxSizeI(width, height);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxSizeI), width, height)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxSizeI.Properties

	/// <summary>
	/// サイズ構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSizeI_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSizeI_Properties()
			: base()
		{
			this.Category = "XIE.TxSizeI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxSizeI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Width", new Type[] { typeof(int) }),
				new CxTaskPortOut("Height", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 幅
			/// </summary>
			DataOut0,

			/// <summary>
			/// 高さ
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxSizeI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxSizeI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxSizeI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// サイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSizeI_Properties.This")]
		public TxSizeI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxSizeI m_This = new TxSizeI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxSizeI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Width;
			this.DataOut[1].Data = this.This.Height;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Width = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Width))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Height))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Width = task$.Width;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Width"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Height"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRangeD.ctor

	/// <summary>
	/// レンジ構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRangeD_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRangeD_ctor()
			: base()
		{
			this.Category = "XIE.TxRangeD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Lower", new Type[] { typeof(double) }),
				new CxTaskPortIn("Upper", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRangeD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 下限値
			/// </summary>
			DataParam0,

			/// <summary>
			/// 上限値
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRangeD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRangeD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRangeD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 下限値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeD_ctor.Lower")]
		public double Lower
		{
			get { return m_This.Lower; }
			set { m_This.Lower = value; }
		}

		/// <summary>
		/// 上限値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeD_ctor.Upper")]
		public double Upper
		{
			get { return m_This.Upper; }
			set { m_This.Upper = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// レンジ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRangeD_ctor.This")]
		public TxRangeD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRangeD m_This = new TxRangeD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Lower = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Upper = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRangeD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var lower = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Lower));
					var upper = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Upper));

					// task#_This = new XIE.TxRangeD(lower, upper);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRangeD), lower, upper)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRangeD.Properties

	/// <summary>
	/// レンジ構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRangeD_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRangeD_Properties()
			: base()
		{
			this.Category = "XIE.TxRangeD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRangeD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Lower", new Type[] { typeof(double) }),
				new CxTaskPortOut("Upper", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 下限値
			/// </summary>
			DataOut0,

			/// <summary>
			/// 上限値
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRangeD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRangeD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRangeD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// レンジ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeD_Properties.This")]
		public TxRangeD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRangeD m_This = new TxRangeD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRangeD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Lower;
			this.DataOut[1].Data = this.This.Upper;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Lower = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Lower))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Upper))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Lower = task$.Lower;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Lower"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Upper"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRangeI.ctor

	/// <summary>
	/// レンジ構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRangeI_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRangeI_ctor()
			: base()
		{
			this.Category = "XIE.TxRangeI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Lower", new Type[] { typeof(int) }),
				new CxTaskPortIn("Upper", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRangeI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 下限値
			/// </summary>
			DataParam0,

			/// <summary>
			/// 上限値
			/// </summary>
			DataParam1,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRangeI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRangeI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRangeI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 下限値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeI_ctor.Lower")]
		public int Lower
		{
			get { return m_This.Lower; }
			set { m_This.Lower = value; }
		}

		/// <summary>
		/// 上限値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeI_ctor.Upper")]
		public int Upper
		{
			get { return m_This.Upper; }
			set { m_This.Upper = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// レンジ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRangeI_ctor.This")]
		public TxRangeI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRangeI m_This = new TxRangeI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Lower = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Upper = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRangeI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var lower = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Lower));
					var upper = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Upper));

					// task#_This = new XIE.TxRangeI(lower, upper);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRangeI), lower, upper)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRangeI.Properties

	/// <summary>
	/// レンジ構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRangeI_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRangeI_Properties()
			: base()
		{
			this.Category = "XIE.TxRangeI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRangeI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Lower", new Type[] { typeof(int) }),
				new CxTaskPortOut("Upper", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 下限値
			/// </summary>
			DataOut0,

			/// <summary>
			/// 上限値
			/// </summary>
			DataOut1,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRangeI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRangeI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRangeI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// レンジ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRangeI_Properties.This")]
		public TxRangeI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRangeI m_This = new TxRangeI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRangeI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Lower;
			this.DataOut[1].Data = this.This.Upper;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Lower = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Lower))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Upper))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Lower = task$.Lower;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Lower"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Upper"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxRectangleD.ctor

	/// <summary>
	/// 矩形構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRectangleD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRectangleD_ctor()
			: base()
		{
			this.Category = "XIE.TxRectangleD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
				new CxTaskPortIn("Width", new Type[] { typeof(double) }),
				new CxTaskPortIn("Height", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRectangleD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 幅
			/// </summary>
			DataParam2,

			/// <summary>
			/// 高さ
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRectangleD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRectangleD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRectangleD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.Width")]
		public double Width
		{
			get { return m_This.Width; }
			set { m_This.Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.Height")]
		public double Height
		{
			get { return m_This.Height; }
			set { m_This.Height = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 矩形
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.This")]
		public TxRectangleD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRectangleD m_This = new TxRectangleD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.Width = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.Height = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRectangleD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Width));
					var height = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Height));

					// task#_This = new XIE.TxRectangleD(x, y, width, height);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRectangleD), x, y, width, height)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiRectangle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxRectangleD.Properties

	/// <summary>
	/// 矩形構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRectangleD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRectangleD_Properties()
			: base()
		{
			this.Category = "XIE.TxRectangleD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRectangleD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
				new CxTaskPortOut("Width", new Type[] { typeof(double) }),
				new CxTaskPortOut("Height", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 幅
			/// </summary>
			DataOut2,

			/// <summary>
			/// 高さ
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRectangleD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRectangleD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRectangleD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 矩形
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_Properties.This")]
		public TxRectangleD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRectangleD m_This = new TxRectangleD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRectangleD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Width;
			this.DataOut[3].Data = this.This.Height;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Width))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Height))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Width = task$.Width;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Width"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Height"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiRectangle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxRectangleD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxRectangleI.ctor

	/// <summary>
	/// 矩形構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRectangleI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRectangleI_ctor()
			: base()
		{
			this.Category = "XIE.TxRectangleI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
				new CxTaskPortIn("Width", new Type[] { typeof(int) }),
				new CxTaskPortIn("Height", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxRectangleI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 幅
			/// </summary>
			DataParam2,

			/// <summary>
			/// 高さ
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRectangleI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxRectangleI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRectangleI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.Width")]
		public int Width
		{
			get { return m_This.Width; }
			set { m_This.Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.Height")]
		public int Height
		{
			get { return m_This.Height; }
			set { m_This.Height = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 矩形
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.This")]
		public TxRectangleI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxRectangleI m_This = new TxRectangleI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Width = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Height = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxRectangleI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Width));
					var height = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Height));

					// task#_This = new XIE.TxRectangleI(x, y, width, height);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxRectangleI), x, y, width, height)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiRectangle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxRectangleI.Properties

	/// <summary>
	/// 矩形構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxRectangleI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxRectangleI_Properties()
			: base()
		{
			this.Category = "XIE.TxRectangleI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxRectangleI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
				new CxTaskPortOut("Width", new Type[] { typeof(int) }),
				new CxTaskPortOut("Height", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 幅
			/// </summary>
			DataOut2,

			/// <summary>
			/// 高さ
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxRectangleI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxRectangleI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxRectangleI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 矩形
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_Properties.This")]
		public TxRectangleI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxRectangleI m_This = new TxRectangleI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxRectangleI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Width;
			this.DataOut[3].Data = this.This.Height;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Width))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Height))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Width = task$.Width;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Width"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Height"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiRectangle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxRectangleI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティページを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxCircleD.ctor

	/// <summary>
	/// 真円構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleD_ctor()
			: base()
		{
			this.Category = "XIE.TxCircleD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
				new CxTaskPortIn("Radius", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxCircleD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxCircleD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleD_ctor.Radius")]
		public double Radius
		{
			get { return m_This.Radius; }
			set { m_This.Radius = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 真円
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxCircleD_ctor.This")]
		public TxCircleD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxCircleD m_This = new TxCircleD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.Radius = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxCircleD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Radius));

					// task#_This = new XIE.TxCircleD(x, y, radius);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxCircleD), x, y, radius)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxCircleD.Properties

	/// <summary>
	/// 真円構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleD_Properties()
			: base()
		{
			this.Category = "XIE.TxCircleD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxCircleD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
				new CxTaskPortOut("Radius", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 半径
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxCircleD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 真円
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleD_Properties.This")]
		public TxCircleD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxCircleD m_This = new TxCircleD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxCircleD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Radius;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Radius))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Radius"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxCircleI.ctor

	/// <summary>
	/// 真円構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleI_ctor()
			: base()
		{
			this.Category = "XIE.TxCircleI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
				new CxTaskPortIn("Radius", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxCircleI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxCircleI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleI_ctor.Radius")]
		public int Radius
		{
			get { return m_This.Radius; }
			set { m_This.Radius = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 真円
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxCircleI_ctor.This")]
		public TxCircleI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxCircleI m_This = new TxCircleI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Radius = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxCircleI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Radius));

					// task#_This = new XIE.TxCircleI(x, y, radius);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxCircleI), x, y, radius)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxCircleI.Properties

	/// <summary>
	/// 真円構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleI_Properties()
			: base()
		{
			this.Category = "XIE.TxCircleI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxCircleI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
				new CxTaskPortOut("Radius", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 半径
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxCircleI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 真円
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleI_Properties.This")]
		public TxCircleI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxCircleI m_This = new TxCircleI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxCircleI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Radius;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Radius))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Radius"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircle(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxCircleArcD.ctor

	/// <summary>
	/// 真円の円弧構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleArcD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleArcD_ctor()
			: base()
		{
			this.Category = "XIE.TxCircleArcD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
				new CxTaskPortIn("Radius", new Type[] { typeof(double) }),
				new CxTaskPortIn("StartAngle", new Type[] { typeof(double) }),
				new CxTaskPortIn("SweepAngle", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxCircleArcD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleArcD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxCircleArcD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleArcD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.Radius")]
		public double Radius
		{
			get { return m_This.Radius; }
			set { m_This.Radius = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.StartAngle")]
		public double StartAngle
		{
			get { return m_This.StartAngle; }
			set { m_This.StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.SweepAngle")]
		public double SweepAngle
		{
			get { return m_This.SweepAngle; }
			set { m_This.SweepAngle = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 真円の円弧
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.This")]
		public TxCircleArcD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxCircleArcD m_This = new TxCircleArcD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.Radius = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.StartAngle = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 4: this.SweepAngle = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxCircleArcD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Radius));
					var start_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.StartAngle));
					var sweep_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.SweepAngle));

					// task#_This = new XIE.TxCircleArcD(x, y, radius, start_angle, sweep_angle);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxCircleArcD), x, y, radius, start_angle, sweep_angle)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircleArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_ctor.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxCircleArcD.Properties

	/// <summary>
	/// 真円の円弧構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleArcD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleArcD_Properties()
			: base()
		{
			this.Category = "XIE.TxCircleArcD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxCircleArcD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
				new CxTaskPortOut("Radius", new Type[] { typeof(double) }),
				new CxTaskPortOut("StartAngle", new Type[] { typeof(double) }),
				new CxTaskPortOut("SweepAngle", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataOut3,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataOut4,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleArcD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxCircleArcD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleArcD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 円弧
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_Properties.This")]
		public TxCircleArcD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxCircleArcD m_This = new TxCircleArcD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxCircleArcD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Radius;
			this.DataOut[3].Data = this.This.StartAngle;
			this.DataOut[4].Data = this.This.SweepAngle;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Radius))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.StartAngle))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.SweepAngle))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Radius"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("StartAngle"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("SweepAngle"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircleArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcD_Properties.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxCircleArcI.ctor

	/// <summary>
	/// 真円の円弧構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleArcI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleArcI_ctor()
			: base()
		{
			this.Category = "XIE.TxCircleArcI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
				new CxTaskPortIn("Radius", new Type[] { typeof(int) }),
				new CxTaskPortIn("StartAngle", new Type[] { typeof(int) }),
				new CxTaskPortIn("SweepAngle", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxCircleArcI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// 半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleArcI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxCircleArcI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleArcI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.Radius")]
		public int Radius
		{
			get { return m_This.Radius; }
			set { m_This.Radius = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.StartAngle")]
		public int StartAngle
		{
			get { return m_This.StartAngle; }
			set { m_This.StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.SweepAngle")]
		public int SweepAngle
		{
			get { return m_This.SweepAngle; }
			set { m_This.SweepAngle = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 円弧
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.This")]
		public TxCircleArcI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxCircleArcI m_This = new TxCircleArcI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Radius = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.StartAngle = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.SweepAngle = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxCircleArcI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Radius));
					var start_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.StartAngle));
					var sweep_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.SweepAngle));

					// task#_This = new XIE.TxCircleArcI(x, y, radius, start_angle, sweep_angle);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxCircleArcI), x, y, radius, start_angle, sweep_angle)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircleArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_ctor.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxCircleArcI.Properties

	/// <summary>
	/// 真円の円弧構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxCircleArcI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxCircleArcI_Properties()
			: base()
		{
			this.Category = "XIE.TxCircleArcI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxCircleArcI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
				new CxTaskPortOut("Radius", new Type[] { typeof(int) }),
				new CxTaskPortOut("StartAngle", new Type[] { typeof(int) }),
				new CxTaskPortOut("SweepAngle", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// 半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataOut3,

			/// <summary>
			/// 円弧範囲(degree) [0~±360]
			/// </summary>
			DataOut4,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxCircleArcI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxCircleArcI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxCircleArcI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 円弧
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_Properties.This")]
		public TxCircleArcI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxCircleArcI m_This = new TxCircleArcI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxCircleArcI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.Radius;
			this.DataOut[3].Data = this.This.StartAngle;
			this.DataOut[4].Data = this.This.SweepAngle;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Radius))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.StartAngle))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.SweepAngle))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Radius"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("StartAngle"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("SweepAngle"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiCircleArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxCircleArcI_Properties.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxEllipseD.ctor

	/// <summary>
	/// 楕円
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseD_ctor()
			: base()
		{
			this.Category = "XIE.TxEllipseD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
				new CxTaskPortIn("RadiusX", new Type[] { typeof(double) }),
				new CxTaskPortIn("RadiusY", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxEllipseD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataParam3,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径(X軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.RadiusX")]
		public double RadiusX
		{
			get { return m_This.RadiusX; }
			set { m_This.RadiusX = value; }
		}

		/// <summary>
		/// 半径(Y軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.RadiusY")]
		public double RadiusY
		{
			get { return m_This.RadiusY; }
			set { m_This.RadiusY = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 楕円
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.This")]
		public TxEllipseD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxEllipseD m_This = new TxEllipseD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.RadiusX = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.RadiusY = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxEllipseD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius_x = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RadiusX));
					var radius_y = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.RadiusX));

					// task#_This = new XIE.TxEllipseD(x, y, radius_x, radius_y);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxEllipseD), x, y, radius_x, radius_y)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipse(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxEllipseD.Properties

	/// <summary>
	/// 楕円
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseD_Properties()
			: base()
		{
			this.Category = "XIE.TxEllipseD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxEllipseD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
				new CxTaskPortOut("RadiusX", new Type[] { typeof(double) }),
				new CxTaskPortOut("RadiusY", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// ｙ 軸半径
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 楕円
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_Properties.This")]
		public TxEllipseD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxEllipseD m_This = new TxEllipseD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxEllipseD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.RadiusX;
			this.DataOut[3].Data = this.This.RadiusY;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusX))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusY))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("RadiusX"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("RadiusY"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipse(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxEllipseI.ctor

	/// <summary>
	/// 楕円
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseI_ctor()
			: base()
		{
			this.Category = "XIE.TxEllipseI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
				new CxTaskPortIn("RadiusX", new Type[] { typeof(int) }),
				new CxTaskPortIn("RadiusY", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxEllipseI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataParam3,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径(X軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.RadiusX")]
		public int RadiusX
		{
			get { return m_This.RadiusX; }
			set { m_This.RadiusX = value; }
		}

		/// <summary>
		/// 半径(Y軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.RadiusY")]
		public int RadiusY
		{
			get { return m_This.RadiusY; }
			set { m_This.RadiusY = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 楕円
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.This")]
		public TxEllipseI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxEllipseI m_This = new TxEllipseI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.RadiusX = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.RadiusY = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxEllipseI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius_x = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RadiusX));
					var radius_y = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.RadiusY));

					// task#_This = new XIE.TxEllipseI(x, y, radius_x, radius_y);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxEllipseI), x, y, radius_x, radius_y)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipse(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxEllipseI.Properties

	/// <summary>
	/// 楕円
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseI_Properties()
			: base()
		{
			this.Category = "XIE.TxEllipseI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxEllipseI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
				new CxTaskPortOut("RadiusX", new Type[] { typeof(int) }),
				new CxTaskPortOut("RadiusY", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// ｙ 軸半径
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 楕円
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_Properties.This")]
		public TxEllipseI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxEllipseI m_This = new TxEllipseI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxEllipseI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.RadiusX;
			this.DataOut[3].Data = this.This.RadiusY;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusX))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusY))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("RadiusX"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("RadiusY"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipse(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxEllipseArcD.ctor

	/// <summary>
	/// 楕円の円弧構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseArcD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseArcD_ctor()
			: base()
		{
			this.Category = "XIE.TxEllipseArcD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y", new Type[] { typeof(double) }),
				new CxTaskPortIn("RadiusX", new Type[] { typeof(double) }),
				new CxTaskPortIn("RadiusY", new Type[] { typeof(double) }),
				new CxTaskPortIn("StartAngle", new Type[] { typeof(double) }),
				new CxTaskPortIn("SweepAngle", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxEllipseArcD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataParam3,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataParam5,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseArcD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseArcD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseArcD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.X")]
		public double X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.Y")]
		public double Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径(X軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.RadiusX")]
		public double RadiusX
		{
			get { return m_This.RadiusX; }
			set { m_This.RadiusX = value; }
		}

		/// <summary>
		/// 半径(Y軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.RadiusY")]
		public double RadiusY
		{
			get { return m_This.RadiusY; }
			set { m_This.RadiusY = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.StartAngle")]
		public double StartAngle
		{
			get { return m_This.StartAngle; }
			set { m_This.StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.SweepAngle")]
		public double SweepAngle
		{
			get { return m_This.SweepAngle; }
			set { m_This.SweepAngle = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 楕円の円弧
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.This")]
		public TxEllipseArcD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxEllipseArcD m_This = new TxEllipseArcD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.RadiusX = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.RadiusY = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 4: this.StartAngle = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 5: this.SweepAngle = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxEllipseArcD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius_x = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RadiusX));
					var radius_y = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.RadiusX));
					var start_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.StartAngle));
					var sweep_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.SweepAngle));

					// task#_This = new XIE.TxEllipseArcD(x, y, radius_x, radius_y, start_angle, sweep_angle);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxEllipseArcD), x, y, radius_x, radius_y, start_angle, sweep_angle)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipseArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_ctor.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxEllipseArcD.Properties

	/// <summary>
	/// 楕円の円弧構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseArcD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseArcD_Properties()
			: base()
		{
			this.Category = "XIE.TxEllipseArcD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxEllipseArcD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y", new Type[] { typeof(double) }),
				new CxTaskPortOut("RadiusX", new Type[] { typeof(double) }),
				new CxTaskPortOut("RadiusY", new Type[] { typeof(double) }),
				new CxTaskPortOut("StartAngle", new Type[] { typeof(double) }),
				new CxTaskPortOut("SweepAngle", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataOut3,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataOut4,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataOut5,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseArcD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseArcD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseArcD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 円弧
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_Properties.This")]
		public TxEllipseArcD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxEllipseArcD m_This = new TxEllipseArcD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxEllipseArcD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.RadiusX;
			this.DataOut[3].Data = this.This.RadiusY;
			this.DataOut[4].Data = this.This.StartAngle;
			this.DataOut[5].Data = this.This.SweepAngle;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusX))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusY))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.StartAngle))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.This.SweepAngle))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("RadiusX"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("RadiusY"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("StartAngle"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("SweepAngle"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipseArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcD_Properties.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxEllipseArcI.ctor

	/// <summary>
	/// 楕円の円弧構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseArcI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseArcI_ctor()
			: base()
		{
			this.Category = "XIE.TxEllipseArcI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y", new Type[] { typeof(int) }),
				new CxTaskPortIn("RadiusX", new Type[] { typeof(int) }),
				new CxTaskPortIn("RadiusY", new Type[] { typeof(int) }),
				new CxTaskPortIn("StartAngle", new Type[] { typeof(int) }),
				new CxTaskPortIn("SweepAngle", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxEllipseArcI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// X 座標
			/// </summary>
			DataParam0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataParam1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataParam2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataParam3,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 円弧範囲 (degree) [0~±360]
			/// </summary>
			DataParam5,

			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseArcI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseArcI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseArcI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.X")]
		public int X
		{
			get { return m_This.X; }
			set { m_This.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.Y")]
		public int Y
		{
			get { return m_This.Y; }
			set { m_This.Y = value; }
		}

		/// <summary>
		/// 半径(X軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.RadiusX")]
		public int RadiusX
		{
			get { return m_This.RadiusX; }
			set { m_This.RadiusX = value; }
		}

		/// <summary>
		/// 半径(Y軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.RadiusY")]
		public int RadiusY
		{
			get { return m_This.RadiusY; }
			set { m_This.RadiusY = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.StartAngle")]
		public int StartAngle
		{
			get { return m_This.StartAngle; }
			set { m_This.StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.SweepAngle")]
		public int SweepAngle
		{
			get { return m_This.SweepAngle; }
			set { m_This.SweepAngle = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 円弧
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.This")]
		public TxEllipseArcI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxEllipseArcI m_This = new TxEllipseArcI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.RadiusX = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.RadiusY = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.StartAngle = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 5: this.SweepAngle = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxEllipseArcI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X));
					var y = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y));
					var radius_x = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RadiusX));
					var radius_y = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.RadiusY));
					var start_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.StartAngle));
					var sweep_angle = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.SweepAngle));

					// task#_This = new XIE.TxEllipseArcI(x, y, radius_x, radius_y, start_angle, sweep_angle);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxEllipseArcI), x, y, radius_x, radius_y, start_angle, sweep_angle)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipseArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_ctor.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxEllipseArcI.Properties

	/// <summary>
	/// 楕円の円弧構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxEllipseArcI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxEllipseArcI_Properties()
			: base()
		{
			this.Category = "XIE.TxEllipseArcI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxEllipseArcI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y", new Type[] { typeof(int) }),
				new CxTaskPortOut("RadiusX", new Type[] { typeof(int) }),
				new CxTaskPortOut("RadiusY", new Type[] { typeof(int) }),
				new CxTaskPortOut("StartAngle", new Type[] { typeof(int) }),
				new CxTaskPortOut("SweepAngle", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// X 座標
			/// </summary>
			DataOut0,

			/// <summary>
			/// Y 座標
			/// </summary>
			DataOut1,

			/// <summary>
			/// X 軸半径
			/// </summary>
			DataOut2,

			/// <summary>
			/// Y 軸半径
			/// </summary>
			DataOut3,

			/// <summary>
			/// 開始角 (degree) [0~360]
			/// </summary>
			DataOut4,

			/// <summary>
			/// 円弧範囲(degree) [0~±360]
			/// </summary>
			DataOut5,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxEllipseArcI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxEllipseArcI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxEllipseArcI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 円弧
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_Properties.This")]
		public TxEllipseArcI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxEllipseArcI m_This = new TxEllipseArcI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxEllipseArcI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X;
			this.DataOut[1].Data = this.This.Y;
			this.DataOut[2].Data = this.This.RadiusX;
			this.DataOut[3].Data = this.This.RadiusY;
			this.DataOut[4].Data = this.This.StartAngle;
			this.DataOut[5].Data = this.This.SweepAngle;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusX))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.RadiusY))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.StartAngle))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.This.SweepAngle))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("RadiusX"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("RadiusY"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("StartAngle"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("SweepAngle"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiEllipseArc(This);
			figure.PenColor = Color.Blue;
			figure.Closed = this.Closed;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxEllipseArcI_Properties.Closed")]
		public bool Closed
		{
			get { return m_Closed; }
			set { m_Closed = value; }
		}
		private bool m_Closed = true;

		#endregion
	}

	#endregion

	#region TxLineD.ctor

	/// <summary>
	/// 直線構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineD_ctor()
			: base()
		{
			this.Category = "XIE.TxLineD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("A", new Type[] { typeof(double) }),
				new CxTaskPortIn("B", new Type[] { typeof(double) }),
				new CxTaskPortIn("C", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxLineD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 a
			/// </summary>
			DataParam0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 b
			/// </summary>
			DataParam1,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 c
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxLineD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 直線式の係数 A
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineD_ctor.A")]
		public double A
		{
			get { return m_This.A; }
			set { m_This.A = value; }
		}

		/// <summary>
		/// 直線式の係数 B
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineD_ctor.B")]
		public double B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		/// <summary>
		/// 直線式の係数 C
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineD_ctor.C")]
		public double C
		{
			get { return m_This.C; }
			set { m_This.C = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 直線式 (ax+by+c=0)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxLineD_ctor.This")]
		public TxLineD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxLineD m_This = new TxLineD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.A = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.B = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.C = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxLineD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var a = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.A));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.B));
					var c = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.C));

					// task#_This = new XIE.TxLineD(a, b, c);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxLineD), a, b, c)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLine(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineD.Properties

	/// <summary>
	/// 直線構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineD_Properties()
			: base()
		{
			this.Category = "XIE.TxLineD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxLineD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("A", new Type[] { typeof(double) }),
				new CxTaskPortOut("B", new Type[] { typeof(double) }),
				new CxTaskPortOut("C", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 a
			/// </summary>
			DataOut0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 b
			/// </summary>
			DataOut1,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 c
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxLineD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 直線式 (ax+by+c=0)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineD_Properties.This")]
		public TxLineD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxLineD m_This = new TxLineD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxLineD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.A;
			this.DataOut[1].Data = this.This.B;
			this.DataOut[2].Data = this.This.C;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.A))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.C))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_A = task$.A;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("A"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("B"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("C"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLine(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineI.ctor

	/// <summary>
	/// 直線構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineI_ctor()
			: base()
		{
			this.Category = "XIE.TxLineI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("A", new Type[] { typeof(int) }),
				new CxTaskPortIn("B", new Type[] { typeof(int) }),
				new CxTaskPortIn("C", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxLineI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 a
			/// </summary>
			DataParam0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 b
			/// </summary>
			DataParam1,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 c
			/// </summary>
			DataParam2,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxLineI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 直線式の係数 A
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineI_ctor.A")]
		public int A
		{
			get { return m_This.A; }
			set { m_This.A = value; }
		}

		/// <summary>
		/// 直線式の係数 B
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineI_ctor.B")]
		public int B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		/// <summary>
		/// 直線式の係数 C
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineI_ctor.C")]
		public int C
		{
			get { return m_This.C; }
			set { m_This.C = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 直線式 (ax+by+c=0)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxLineI_ctor.This")]
		public TxLineI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxLineI m_This = new TxLineI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.A = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.B = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.C = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxLineI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var a = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.A));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.B));
					var c = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.C));

					// task#_This = new XIE.TxLineI(a, b, c);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxLineI), a, b, c)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLine(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineI.Properties

	/// <summary>
	/// 直線構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineI_Properties()
			: base()
		{
			this.Category = "XIE.TxLineI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxLineI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("A", new Type[] { typeof(int) }),
				new CxTaskPortOut("B", new Type[] { typeof(int) }),
				new CxTaskPortOut("C", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 a
			/// </summary>
			DataOut0,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 b
			/// </summary>
			DataOut1,

			/// <summary>
			/// 直線式 (ax + by + c = 0) の 係数 c
			/// </summary>
			DataOut2,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxLineI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 直線式 (ax+by+c=0)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineI_Properties.This")]
		public TxLineI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxLineI m_This = new TxLineI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxLineI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.A;
			this.DataOut[1].Data = this.This.B;
			this.DataOut[2].Data = this.This.C;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_A = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.A))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.B))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.C))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_A = task$.A;
						switch(i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("A"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("B"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("C"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLine(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineSegmentD.ctor

	/// <summary>
	/// 線分構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineSegmentD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineSegmentD_ctor()
			: base()
		{
			this.Category = "XIE.TxLineSegmentD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X1", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y1", new Type[] { typeof(double) }),
				new CxTaskPortIn("X2", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y2", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxLineSegmentD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 始点 (X)
			/// </summary>
			DataParam0,

			/// <summary>
			/// 始点 (Y)
			/// </summary>
			DataParam1,

			/// <summary>
			/// 終点 (X)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 終点 (Y)
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineSegmentD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxLineSegmentD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineSegmentD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.X1")]
		public double X1
		{
			get { return m_This.X1; }
			set { m_This.X1 = value; }
		}

		/// <summary>
		/// Y1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.Y1")]
		public double Y1
		{
			get { return m_This.Y1; }
			set { m_This.Y1 = value; }
		}

		/// <summary>
		/// X2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.X2")]
		public double X2
		{
			get { return m_This.X2; }
			set { m_This.X2 = value; }
		}

		/// <summary>
		/// Y2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.Y2")]
		public double Y2
		{
			get { return m_This.Y2; }
			set { m_This.Y2 = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 線分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.This")]
		public TxLineSegmentD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxLineSegmentD m_This = new TxLineSegmentD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X1 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y1 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.X2 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.Y2 = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxLineSegmentD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X1));
					var y1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y1));
					var x2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.X2));
					var y2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Y2));

					// task#_This = new XIE.TxLineSegmentD(x1, y1, x2, y2);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxLineSegmentD), x1, y1, x2, y2)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLineSegment(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineSegmentD.Properties

	/// <summary>
	/// 線分構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineSegmentD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineSegmentD_Properties()
			: base()
		{
			this.Category = "XIE.TxLineSegmentD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxLineSegmentD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X1", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y1", new Type[] { typeof(double) }),
				new CxTaskPortOut("X2", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y2", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 始点 (X)
			/// </summary>
			DataOut0,

			/// <summary>
			/// 始点 (Y)
			/// </summary>
			DataOut1,

			/// <summary>
			/// 終点 (X)
			/// </summary>
			DataOut2,

			/// <summary>
			/// 終点 (Y)
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineSegmentD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxLineSegmentD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineSegmentD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 線分
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_Properties.This")]
		public TxLineSegmentD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxLineSegmentD m_This = new TxLineSegmentD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxLineSegmentD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X1;
			this.DataOut[1].Data = this.This.Y1;
			this.DataOut[2].Data = this.This.X2;
			this.DataOut[3].Data = this.This.Y2;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X1))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y1))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.X2))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y2))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X1 = task$.X1;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X1"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y1"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("X2"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Y2"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLineSegment(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineSegmentI.ctor

	/// <summary>
	/// 線分構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineSegmentI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineSegmentI_ctor()
			: base()
		{
			this.Category = "XIE.TxLineSegmentI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X1", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y1", new Type[] { typeof(int) }),
				new CxTaskPortIn("X2", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y2", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxLineSegmentI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 始点 (X)
			/// </summary>
			DataParam0,

			/// <summary>
			/// 始点 (Y)
			/// </summary>
			DataParam1,

			/// <summary>
			/// 終点 (X)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 終点 (Y)
			/// </summary>
			DataParam3,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineSegmentI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxLineSegmentI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineSegmentI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.X1")]
		public int X1
		{
			get { return m_This.X1; }
			set { m_This.X1 = value; }
		}

		/// <summary>
		/// Y1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.Y1")]
		public int Y1
		{
			get { return m_This.Y1; }
			set { m_This.Y1 = value; }
		}

		/// <summary>
		/// X2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.X2")]
		public int X2
		{
			get { return m_This.X2; }
			set { m_This.X2 = value; }
		}

		/// <summary>
		/// Y2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.Y2")]
		public int Y2
		{
			get { return m_This.Y2; }
			set { m_This.Y2 = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 線分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.This")]
		public TxLineSegmentI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxLineSegmentI m_This = new TxLineSegmentI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X1 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y1 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.X2 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Y2 = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxLineSegmentI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X1));
					var y1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y1));
					var x2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.X2));
					var y2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Y2));

					// task#_This = new XIE.TxLineSegmentI(x1, y1, x2, y2);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxLineSegmentI), x1, y1, x2, y2)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLineSegment(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxLineSegmentI.Properties

	/// <summary>
	/// 線分構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxLineSegmentI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxLineSegmentI_Properties()
			: base()
		{
			this.Category = "XIE.TxLineSegmentI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxLineSegmentI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X1", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y1", new Type[] { typeof(int) }),
				new CxTaskPortOut("X2", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y2", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 始点 (X)
			/// </summary>
			DataOut0,

			/// <summary>
			/// 始点 (Y)
			/// </summary>
			DataOut1,

			/// <summary>
			/// 終点 (X)
			/// </summary>
			DataOut2,

			/// <summary>
			/// 終点 (Y)
			/// </summary>
			DataOut3,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxLineSegmentI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxLineSegmentI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxLineSegmentI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 線分
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_Properties.This")]
		public TxLineSegmentI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxLineSegmentI m_This = new TxLineSegmentI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxLineSegmentI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X1;
			this.DataOut[1].Data = this.This.Y1;
			this.DataOut[2].Data = this.This.X2;
			this.DataOut[3].Data = this.This.Y2;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X1))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y1))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.X2))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y2))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X1 = task$.X1;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X1"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y1"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("X2"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Y2"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiLineSegment(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxLineSegmentI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティページを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxTrapezoidD.ctor

	/// <summary>
	/// 台形構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxTrapezoidD_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxTrapezoidD_ctor()
			: base()
		{
			this.Category = "XIE.TxTrapezoidD";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X1", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y1", new Type[] { typeof(double) }),
				new CxTaskPortIn("X2", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y2", new Type[] { typeof(double) }),
				new CxTaskPortIn("X3", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y3", new Type[] { typeof(double) }),
				new CxTaskPortIn("X4", new Type[] { typeof(double) }),
				new CxTaskPortIn("Y4", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxTrapezoidD) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 左上 (X 座標)
			/// </summary>
			DataParam0,

			/// <summary>
			/// 左上 (Y 座標)
			/// </summary>
			DataParam1,

			/// <summary>
			/// 右上 (X 座標)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 右上 (Y 座標)
			/// </summary>
			DataParam3,

			/// <summary>
			/// 右下 (X 座標)
			/// </summary>
			DataParam4,

			/// <summary>
			/// 右下 (Y 座標)
			/// </summary>
			DataParam5,

			/// <summary>
			/// 左下 (X 座標)
			/// </summary>
			DataParam6,

			/// <summary>
			/// 左下 (Y 座標)
			/// </summary>
			DataParam7,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxTrapezoidD_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxTrapezoidD_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxTrapezoidD_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.X1")]
		public double X1
		{
			get { return m_This.X1; }
			set { m_This.X1 = value; }
		}

		/// <summary>
		/// Y1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.Y1")]
		public double Y1
		{
			get { return m_This.Y1; }
			set { m_This.Y1 = value; }
		}

		/// <summary>
		/// X2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.X2")]
		public double X2
		{
			get { return m_This.X2; }
			set { m_This.X2 = value; }
		}

		/// <summary>
		/// Y2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.Y2")]
		public double Y2
		{
			get { return m_This.Y2; }
			set { m_This.Y2 = value; }
		}

		/// <summary>
		/// X3 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.X3")]
		public double X3
		{
			get { return m_This.X3; }
			set { m_This.X3 = value; }
		}

		/// <summary>
		/// Y3 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.Y3")]
		public double Y3
		{
			get { return m_This.Y3; }
			set { m_This.Y3 = value; }
		}

		/// <summary>
		/// X4 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.X4")]
		public double X4
		{
			get { return m_This.X4; }
			set { m_This.X4 = value; }
		}

		/// <summary>
		/// Y4 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.Y4")]
		public double Y4
		{
			get { return m_This.Y4; }
			set { m_This.Y4 = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 台形
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.This")]
		public TxTrapezoidD This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxTrapezoidD m_This = new TxTrapezoidD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X1 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 1: this.Y1 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 2: this.X2 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 3: this.Y2 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 4: this.X3 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 5: this.Y3 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 6: this.X4 = Convert.ToDouble(this.DataParam[iii].Data); break;
						case 7: this.Y4 = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxTrapezoidD)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X1));
					var y1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y1));
					var x2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.X2));
					var y2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Y2));
					var x3 = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.X3));
					var y3 = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.Y3));
					var x4 = ApiHelper.CodeOptionalExpression(e, this.DataParam[6], CodeLiteral.From(this.X4));
					var y4 = ApiHelper.CodeOptionalExpression(e, this.DataParam[7], CodeLiteral.From(this.Y4));

					// task#_This = new XIE.TxTrapezoidD(x1, y1, x2, y2, x3, y3, x4, y4);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxTrapezoidD), x1, y1, x2, y2, x3, y3, x4, y4)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiTrapezoid(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxTrapezoidD.Properties

	/// <summary>
	/// 台形構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxTrapezoidD_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxTrapezoidD_Properties()
			: base()
		{
			this.Category = "XIE.TxTrapezoidD";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxTrapezoidD) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X1", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y1", new Type[] { typeof(double) }),
				new CxTaskPortOut("X2", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y2", new Type[] { typeof(double) }),
				new CxTaskPortOut("X3", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y3", new Type[] { typeof(double) }),
				new CxTaskPortOut("X4", new Type[] { typeof(double) }),
				new CxTaskPortOut("Y4", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 左上 (X 座標)
			/// </summary>
			DataOut0,

			/// <summary>
			/// 左上 (Y 座標)
			/// </summary>
			DataOut1,

			/// <summary>
			/// 右上 (X 座標)
			/// </summary>
			DataOut2,

			/// <summary>
			/// 右上 (Y 座標)
			/// </summary>
			DataOut3,

			/// <summary>
			/// 右下 (X 座標)
			/// </summary>
			DataOut4,

			/// <summary>
			/// 右下 (Y 座標)
			/// </summary>
			DataOut5,

			/// <summary>
			/// 左下 (X 座標)
			/// </summary>
			DataOut6,

			/// <summary>
			/// 左下 (Y 座標)
			/// </summary>
			DataOut7,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxTrapezoidD_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxTrapezoidD_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxTrapezoidD_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 台形
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_Properties.This")]
		public TxTrapezoidD This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxTrapezoidD m_This = new TxTrapezoidD();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxTrapezoidD)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X1;
			this.DataOut[1].Data = this.This.Y1;
			this.DataOut[2].Data = this.This.X2;
			this.DataOut[3].Data = this.This.Y2;
			this.DataOut[4].Data = this.This.X3;
			this.DataOut[5].Data = this.This.Y3;
			this.DataOut[6].Data = this.This.X4;
			this.DataOut[7].Data = this.This.Y4;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X1))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y1))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.X2))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y2))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.X3))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y3))); break;
							case 6: scope.Add(variable.Declare(CodeLiteral.From(this.This.X4))); break;
							case 7: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y4))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X1 = task$.X1;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X1"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y1"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("X2"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Y2"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("X3"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("Y3"))); break;
							case 6: scope.Add(dst.Assign(src.Ref("X4"))); break;
							case 7: scope.Add(dst.Assign(src.Ref("Y4"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiTrapezoid(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidD_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxTrapezoidI.ctor

	/// <summary>
	/// 台形構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxTrapezoidI_ctor : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxTrapezoidI_ctor()
			: base()
		{
			this.Category = "XIE.TxTrapezoidI";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("X1", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y1", new Type[] { typeof(int) }),
				new CxTaskPortIn("X2", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y2", new Type[] { typeof(int) }),
				new CxTaskPortIn("X3", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y3", new Type[] { typeof(int) }),
				new CxTaskPortIn("X4", new Type[] { typeof(int) }),
				new CxTaskPortIn("Y4", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxTrapezoidI) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 左上 (X 座標)
			/// </summary>
			DataParam0,

			/// <summary>
			/// 左上 (Y 座標)
			/// </summary>
			DataParam1,

			/// <summary>
			/// 右上 (X 座標)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 右上 (Y 座標)
			/// </summary>
			DataParam3,

			/// <summary>
			/// 右下 (X 座標)
			/// </summary>
			DataParam4,

			/// <summary>
			/// 右下 (Y 座標)
			/// </summary>
			DataParam5,

			/// <summary>
			/// 左下 (X 座標)
			/// </summary>
			DataParam6,

			/// <summary>
			/// 左下 (Y 座標)
			/// </summary>
			DataParam7,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxTrapezoidI_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxTrapezoidI_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxTrapezoidI_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// X1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.X1")]
		public int X1
		{
			get { return m_This.X1; }
			set { m_This.X1 = value; }
		}

		/// <summary>
		/// Y1 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.Y1")]
		public int Y1
		{
			get { return m_This.Y1; }
			set { m_This.Y1 = value; }
		}

		/// <summary>
		/// X2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.X2")]
		public int X2
		{
			get { return m_This.X2; }
			set { m_This.X2 = value; }
		}

		/// <summary>
		/// Y2 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.Y2")]
		public int Y2
		{
			get { return m_This.Y2; }
			set { m_This.Y2 = value; }
		}

		/// <summary>
		/// X3 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.X3")]
		public int X3
		{
			get { return m_This.X3; }
			set { m_This.X3 = value; }
		}

		/// <summary>
		/// Y3 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.Y3")]
		public int Y3
		{
			get { return m_This.Y3; }
			set { m_This.Y3 = value; }
		}

		/// <summary>
		/// X4 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.X4")]
		public int X4
		{
			get { return m_This.X4; }
			set { m_This.X4 = value; }
		}

		/// <summary>
		/// Y4 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.Y4")]
		public int Y4
		{
			get { return m_This.Y4; }
			set { m_This.Y4 = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 台形
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.This")]
		public TxTrapezoidI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxTrapezoidI m_This = new TxTrapezoidI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.X1 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Y1 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.X2 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Y2 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.X3 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 5: this.Y3 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 6: this.X4 = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 7: this.Y4 = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxTrapezoidI)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var x1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.X1));
					var y1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Y1));
					var x2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.X2));
					var y2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Y2));
					var x3 = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.X3));
					var y3 = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.Y3));
					var x4 = ApiHelper.CodeOptionalExpression(e, this.DataParam[6], CodeLiteral.From(this.X4));
					var y4 = ApiHelper.CodeOptionalExpression(e, this.DataParam[7], CodeLiteral.From(this.Y4));

					// task#_This = new XIE.TxTrapezoidI(x1, y1, x2, x2, x3, y3, x4, x4);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxTrapezoidI), x1, y1, x2, x2, x3, y3, x4, x4)
						));
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiTrapezoid(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_ctor.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxTrapezoidI.Properties

	/// <summary>
	/// 台形構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxTrapezoidI_Properties : CxTaskUnit
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxTrapezoidI_Properties()
			: base()
		{
			this.Category = "XIE.TxTrapezoidI";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxTrapezoidI) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("X1", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y1", new Type[] { typeof(int) }),
				new CxTaskPortOut("X2", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y2", new Type[] { typeof(int) }),
				new CxTaskPortOut("X3", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y3", new Type[] { typeof(int) }),
				new CxTaskPortOut("X4", new Type[] { typeof(int) }),
				new CxTaskPortOut("Y4", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 左上 (X 座標)
			/// </summary>
			DataOut0,

			/// <summary>
			/// 左上 (Y 座標)
			/// </summary>
			DataOut1,

			/// <summary>
			/// 右上 (X 座標)
			/// </summary>
			DataOut2,

			/// <summary>
			/// 右上 (Y 座標)
			/// </summary>
			DataOut3,

			/// <summary>
			/// 右下 (X 座標)
			/// </summary>
			DataOut4,

			/// <summary>
			/// 右下 (Y 座標)
			/// </summary>
			DataOut5,

			/// <summary>
			/// 左下 (X 座標)
			/// </summary>
			DataOut6,

			/// <summary>
			/// 左下 (Y 座標)
			/// </summary>
			DataOut7,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxTrapezoidI_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxTrapezoidI_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxTrapezoidI_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 台形
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_Properties.This")]
		public TxTrapezoidI This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxTrapezoidI m_This = new TxTrapezoidI();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxTrapezoidI)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.X1;
			this.DataOut[1].Data = this.This.Y1;
			this.DataOut[2].Data = this.This.X2;
			this.DataOut[3].Data = this.This.Y2;
			this.DataOut[4].Data = this.This.X3;
			this.DataOut[5].Data = this.This.Y3;
			this.DataOut[6].Data = this.This.X4;
			this.DataOut[7].Data = this.This.Y4;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_X = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.X1))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y1))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.X2))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y2))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.X3))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y3))); break;
							case 6: scope.Add(variable.Declare(CodeLiteral.From(this.This.X4))); break;
							case 7: scope.Add(variable.Declare(CodeLiteral.From(this.This.Y4))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X1 = task$.X1;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("X1"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Y1"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("X2"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Y2"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("X3"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("Y3"))); break;
							case 6: scope.Add(dst.Assign(src.Ref("X4"))); break;
							case 7: scope.Add(dst.Assign(src.Ref("Y4"))); break;
						}
					}
				}
			}
		}

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var figure = new XIE.GDI.CxGdiTrapezoid(This);
			figure.PenColor = Color.Blue;
			e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.Center);
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		[Browsable(false)]
		[CxCategory("Visualization")]
		[CxDescription("P:XIE.Tasks.TxTrapezoidI_Properties.EnableRendering")]
		public bool EnableRendering
		{
			get { return m_EnableRendering; }
			set { m_EnableRendering = value; }
		}
		private bool m_EnableRendering = true;

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool IxTaskOverlayRendering.HasRenderingPropertyForm
		{
			get { return false; }
		}

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティページを返します。
		/// </returns>
		Form IxTaskOverlayRendering.OpenRenderingPropertyForm(params object[] options)
		{
			return null;
		}

		#endregion
	}

	#endregion

	#region TxStatistics.ctor

	/// <summary>
	/// 統計構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxStatistics_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxStatistics_ctor()
			: base()
		{
			this.Category = "XIE.TxStatistics";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Count", new Type[] { typeof(double) }),
				new CxTaskPortIn("Max", new Type[] { typeof(double) }),
				new CxTaskPortIn("Min", new Type[] { typeof(double) }),
				new CxTaskPortIn("Sum1", new Type[] { typeof(double) }),
				new CxTaskPortIn("Sum2", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxStatistics) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 要素数
			/// </summary>
			DataParam0,

			/// <summary>
			/// 最大
			/// </summary>
			DataParam1,

			/// <summary>
			/// 最小
			/// </summary>
			DataParam2,

			/// <summary>
			/// 総和
			/// </summary>
			DataParam3,

			/// <summary>
			/// ２乗の総和
			/// </summary>
			DataParam4,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxStatistics_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxStatistics_ctor)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxStatistics_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.Count")]
		public double Count
		{
			get { return m_This.Count; }
			set { m_This.Count = value; }
		}

		/// <summary>
		/// 最大
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.Max")]
		public double Max
		{
			get { return m_This.Max; }
			set { m_This.Max = value; }
		}

		/// <summary>
		/// 最小
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.Min")]
		public double Min
		{
			get { return m_This.Min; }
			set { m_This.Min = value; }
		}

		/// <summary>
		/// 総和
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.Sum1")]
		public double Sum1
		{
			get { return m_This.Sum1; }
			set { m_This.Sum1 = value; }
		}

		/// <summary>
		/// ２乗の総和
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.Sum2")]
		public double Sum2
		{
			get { return m_This.Sum2; }
			set { m_This.Sum2 = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 統計データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_ctor.This")]
		public TxStatistics This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxStatistics m_This = new TxStatistics();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Count = Convert.ToDouble(this.DataParam[0].Data); break;
						case 1: this.Max = Convert.ToDouble(this.DataParam[1].Data); break;
						case 2: this.Min = Convert.ToDouble(this.DataParam[2].Data); break;
						case 3: this.Sum1 = Convert.ToDouble(this.DataParam[3].Data); break;
						case 4: this.Sum2 = Convert.ToDouble(this.DataParam[4].Data); break;
					}
				}
			}

			this.DataOut[0].Data = this.This;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = this.This = (TxStatistics)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var count = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Count));
					var maxval = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Max));
					var minval = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Min));
					var sum1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Sum1));
					var sum2 = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Sum2));

					// task#_This = new XIE.TxStatistics(count, maxval, minval, sum1, sum2);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxStatistics), count, maxval, minval, sum1, sum2)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxStatistics.Properties

	/// <summary>
	/// 統計構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxStatistics_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxStatistics_Properties()
			: base()
		{
			this.Category = "XIE.TxStatistics";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxStatistics) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Count", new Type[] { typeof(double) }),
				new CxTaskPortOut("Max", new Type[] { typeof(double) }),
				new CxTaskPortOut("Min", new Type[] { typeof(double) }),
				new CxTaskPortOut("Sum1", new Type[] { typeof(double) }),
				new CxTaskPortOut("Sum2", new Type[] { typeof(double) }),
				new CxTaskPortOut("Mean", new Type[] { typeof(double) }),
				new CxTaskPortOut("Sigma", new Type[] { typeof(double) }),
				new CxTaskPortOut("Variance", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 要素数
			/// </summary>
			DataOut0,

			/// <summary>
			/// 最大
			/// </summary>
			DataOut1,

			/// <summary>
			/// 最小
			/// </summary>
			DataOut2,

			/// <summary>
			/// 総和
			/// </summary>
			DataOut3,

			/// <summary>
			/// ２乗の総和
			/// </summary>
			DataOut4,

			/// <summary>
			/// 平均
			/// </summary>
			DataOut5,

			/// <summary>
			/// 標準偏差
			/// </summary>
			DataOut6,

			/// <summary>
			/// 分散
			/// </summary>
			DataOut7,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxStatistics_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxStatistics_Properties)src;

				this.This = _src.This;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxStatistics_Properties)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 統計データ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.This")]
		public TxStatistics This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxStatistics m_This = new TxStatistics();

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 要素数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Count")]
		public double Count
		{
			get { return This.Count; }
		}

		/// <summary>
		/// 最大
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Max")]
		public double Max
		{
			get { return This.Max; }
		}

		/// <summary>
		/// 最小
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Min")]
		public double Min
		{
			get { return This.Min; }
		}

		/// <summary>
		/// 総和
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Sum1")]
		public double Sum1
		{
			get { return This.Sum1; }
		}

		/// <summary>
		/// ２乗の総和
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Sum2")]
		public double Sum2
		{
			get { return This.Sum2; }
		}

		#endregion

		#region プロパティ: (拡張)

		/// <summary>
		/// 平均
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs (Ex)")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Mean")]
		public double Mean
		{
			get { return This.Mean; }
		}

		/// <summary>
		/// 標準偏差
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs (Ex)")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Sigma")]
		public double Sigma
		{
			get { return This.Sigma; }
		}

		/// <summary>
		/// 分散
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs (Ex)")]
		[CxDescription("P:XIE.Tasks.TxStatistics_Properties.Variance")]
		public double Variance
		{
			get { return This.Variance; }
		}

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.This = (TxStatistics)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.This.Count;
			this.DataOut[1].Data = this.This.Max;
			this.DataOut[2].Data = this.This.Min;
			this.DataOut[3].Data = this.This.Sum1;
			this.DataOut[4].Data = this.This.Sum2;
			this.DataOut[5].Data = this.This.Mean;
			this.DataOut[6].Data = this.This.Sigma;
			this.DataOut[7].Data = this.This.Variance;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					{
						target_port.Data = Convert.ToDouble(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_Count = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.This.Count))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.This.Max))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.This.Min))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.This.Sum1))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.This.Sum2))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.This.Mean))); break;
							case 6: scope.Add(variable.Declare(CodeLiteral.From(this.This.Sigma))); break;
							case 7: scope.Add(variable.Declare(CodeLiteral.From(this.This.Variance))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_Count = task$.Count;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Count"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Max"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Min"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Sum1"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("Sum2"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("Mean"))); break;
							case 6: scope.Add(dst.Assign(src.Ref("Sigma"))); break;
							case 7: scope.Add(dst.Assign(src.Ref("Variance"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxDateTime.ctor

	/// <summary>
	/// 日時構造体を生成します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxDateTime_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxDateTime_ctor()
			: base()
		{
			this.Category = "XIE.TxDateTime";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Year", new Type[] { typeof(int) }),
				new CxTaskPortIn("Month", new Type[] { typeof(int) }),
				new CxTaskPortIn("Day", new Type[] { typeof(int) }),
				new CxTaskPortIn("Hour", new Type[] { typeof(int) }),
				new CxTaskPortIn("Minute", new Type[] { typeof(int) }),
				new CxTaskPortIn("Second", new Type[] { typeof(int) }),
				new CxTaskPortIn("Millisecond", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TxDateTime) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 年 (西暦) [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 月 [1~12]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 日 [1~31]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 時 [0~23]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 分 [0~59]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 秒 [0~59,60]
			/// </summary>
			DataParam5,

			/// <summary>
			/// ミリ秒 [0~999]
			/// </summary>
			DataParam6,

			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxDateTime_ctor)
			{
				base.CopyFrom(src);

				var _src = (TxDateTime_ctor)src;

				this.Year = _src.Year;
				this.Month = _src.Month;
				this.Day = _src.Day;
				this.Hour = _src.Hour;
				this.Minute = _src.Minute;
				this.Second = _src.Second;
				this.Millisecond = _src.Millisecond;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxDateTime_ctor)src;

				if (this.Year != _src.Year) return false;
				if (this.Month != _src.Month) return false;
				if (this.Day != _src.Day) return false;
				if (this.Hour != _src.Hour) return false;
				if (this.Minute != _src.Minute) return false;
				if (this.Second != _src.Second) return false;
				if (this.Millisecond != _src.Millisecond) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 年 (西暦) [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Year")]
		public int Year
		{
			get { return m_Year; }
			set { m_Year = value; }
		}
		private int m_Year = 1;

		/// <summary>
		/// 月 [1~12]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Month")]
		public int Month
		{
			get { return m_Month; }
			set { m_Month = value; }
		}
		private int m_Month = 1;

		/// <summary>
		/// 日 [1~31]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Day")]
		public int Day
		{
			get { return m_Day; }
			set { m_Day = value; }
		}
		private int m_Day = 1;

		/// <summary>
		/// 時 [0~23]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Hour")]
		public int Hour
		{
			get { return m_Hour; }
			set { m_Hour = value; }
		}
		private int m_Hour = 0;

		/// <summary>
		/// 分 [0~59]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Minute")]
		public int Minute
		{
			get { return m_Minute; }
			set { m_Minute = value; }
		}
		private int m_Minute = 0;

		/// <summary>
		/// 秒 [0~59,60]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Second")]
		public int Second
		{
			get { return m_Second; }
			set { m_Second = value; }
		}
		private int m_Second = 0;

		/// <summary>
		/// ミリ秒 [0~999]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.Millisecond")]
		public int Millisecond
		{
			get { return m_Millisecond; }
			set { m_Millisecond = value; }
		}
		private int m_Millisecond = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 日時
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxDateTime_ctor.This")]
		public TxDateTime This
		{
			get
			{
				if (this.DataOut == null) return default(TxDateTime);
				if (this.DataOut.Length == 0) return default(TxDateTime);
				object data = this.DataOut[0].Data;
				if (data == null) return default(TxDateTime);
				return (TxDateTime)data;
			}
		}

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Year = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Month = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Day = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Hour = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.Minute = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 5: this.Second = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 6: this.Millisecond = Convert.ToInt32(this.DataParam[iii].Data);break;
					}
				}
			}

			var dt = new TxDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);

			this.DataOut[0].Data = dt;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
					{
						target_port.Data = (TxDateTime)value;
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var year = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Year));
					var month = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Month));
					var day = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Day));
					var hour = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Hour));
					var minute = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Minute));
					var second = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.Second));
					var millisecond = ApiHelper.CodeOptionalExpression(e, this.DataParam[6], CodeLiteral.From(this.Millisecond));

					// task#_This = new XIE.TxDateTime(year, month, day, hour, minute, second, millisecond);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.TxDateTime), year, month, day, hour, minute, second, millisecond)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxDateTime.Properties

	/// <summary>
	/// 日時構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxDateTime_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxDateTime_Properties()
			: base()
		{
			this.Category = "XIE.TxDateTime";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TxDateTime) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Year", new Type[] { typeof(int) }),
				new CxTaskPortOut("Month", new Type[] { typeof(int) }),
				new CxTaskPortOut("Day", new Type[] { typeof(int) }),
				new CxTaskPortOut("Hour", new Type[] { typeof(int) }),
				new CxTaskPortOut("Minute", new Type[] { typeof(int) }),
				new CxTaskPortOut("Second", new Type[] { typeof(int) }),
				new CxTaskPortOut("Millisecond", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 年 (西暦) [0~]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 月 [1~12]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 日 [1~31]
			/// </summary>
			DataOut2,

			/// <summary>
			/// 時 [0~23]
			/// </summary>
			DataOut3,

			/// <summary>
			/// 分 [0~59]
			/// </summary>
			DataOut4,

			/// <summary>
			/// 秒 [0~59,60]
			/// </summary>
			DataOut5,

			/// <summary>
			/// ミリ秒 [0~999]
			/// </summary>
			DataOut6,
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is TxDateTime_Properties)
			{
				base.CopyFrom(src);

				var _src = (TxDateTime_Properties)src;

				this.Body = _src.Body;

				return;
			}
			#endregion

			#region XIE.IxConvertible
			if (src is XIE.IxConvertible)
			{
				((XIE.IxConvertible)src).CopyTo(this);
				return;
			}
			#endregion

			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public override bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			{
				var _src = (TxDateTime_Properties)src;

				if (this.Body != _src.Body) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 日時
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxDateTime_Properties.Body")]
		public TxDateTime Body
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private TxDateTime m_This = new TxDateTime();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Body = (TxDateTime)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.Body.Year;
			this.DataOut[1].Data = this.Body.Month;
			this.DataOut[2].Data = this.Body.Day;
			this.DataOut[3].Data = this.Body.Hour;
			this.DataOut[4].Data = this.Body.Minute;
			this.DataOut[5].Data = this.Body.Second;
			this.DataOut[6].Data = this.Body.Millisecond;
		}

		#endregion

		#region メソッド: (代入)

		/// <summary>
		/// 代入 (指定のデータ出力ポートのデータに値を代入します。)
		/// </summary>
		/// <param name="target_port">代入先のデータ出力ポート</param>
		/// <param name="value">代入する値</param>
		public override void Assign(CxTaskPortOut target_port, object value)
		{
			int dst_index = Array.IndexOf(this.DataOut, target_port);
			switch (dst_index)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
					{
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					if (this.DataParam[0].IsConnected)
					{
						scope.Add(variable.Declare());
					}
					else
					{
						// var task#_xxx = xxx;
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Year))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Month))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Day))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Hour))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Minute))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Second))); break;
							case 6: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Millisecond))); break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				if (this.DataParam[0].IsConnected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_xxx = task$.xxx;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Year"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Month"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Day"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Hour"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("Minute"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("Second"))); break;
							case 6: scope.Add(dst.Assign(src.Ref("Millisecond"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
