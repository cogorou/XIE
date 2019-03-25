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
	// //////////////////////////////////////////////////
	// Constructors
	// 

	#region CxImage

	/// <summary>
	/// コンストラクタ
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_ctor : CxTaskUnit
		, IxTaskOutputImage
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_ctor()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Constructor";
			this.IconKey = "Data-Image";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("ImageSize", new Type[] { typeof(TxImageSize) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(CxImage) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像サイズ。幅または高さが 0 の場合は未確保になります。
			/// </summary>
			DataParam0,

			/// <summary>
			/// 画像オブジェクトを構築して返します。
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
			if (src is CxImage_ctor)
			{
				base.CopyFrom(src);

				var _src = (CxImage_ctor)src;

				this.ImageSize = _src.ImageSize;

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
				var _src = (CxImage_ctor)src;

				if (this.ImageSize != _src.ImageSize) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像サイズ。幅または高さが 0 の場合は未確保になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_ctor.ImageSize")]
		[Editor(typeof(ImageSizeEditor), typeof(UITypeEditor))]
		public TxImageSize ImageSize
		{
			get { return m_ImageSize; }
			set { m_ImageSize = value; }
		}
		private TxImageSize m_ImageSize = new TxImageSize();

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

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_ctor.This")]
		public CxImage This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private CxImage m_This = new CxImage();

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			if (this.This == null)
				this.This = new CxImage(this.ImageSize);
			else
				this.This.Resize(this.ImageSize);

			this.DataOut[0].Data = this.This;
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
						case 0: this.ImageSize = (TxImageSize)this.DataParam[iii].Data;  break;
					}
				}
			}

			if (this.This == null)
				this.This = new CxImage(this.ImageSize);
			else
				this.This.Resize(this.ImageSize);

			// 出力.
			this.DataOut[0].Data = this.This;
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

				// task#_This = new XIE.CxImage(image_size);
				{
					// image_size
					var image_size = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.ImageSize));

					// task#_This = new XIE.CxImage(image_size);
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeObjectCreateExpression(typeof(XIE.CxImage), image_size)
						));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

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
			dlg.ImageSize = this.ImageSize;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.ImageSize = dlg.ImageSize;
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

	// //////////////////////////////////////////////////
	// Properties
	// 

	#region ImageSize

	/// <summary>
	/// ImageSize プロパティ
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_ImageSize : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_ImageSize()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "ImageSize";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(TxImageSize)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された画像オブジェクトの画像サイズ情報を取得して返します。
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
			if (src is CxImage_ImageSize)
			{
				base.CopyFrom(src);

				var _src = (CxImage_ImageSize)src;

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
				var _src = (CxImage_ImageSize)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_ImageSize.This")]
		public TxImageSize This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private TxImageSize m_This = new TxImageSize();

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			// 実行.
			this.This = src.ImageSize;

			// 出力.
			this.DataOut[0].Data = this.This;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_This = task$.ImageSize;
					scope.Add(dst.Assign(src.Ref("ImageSize")));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Size

	/// <summary>
	/// Size プロパティ
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Size : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Size()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Size";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(TxSizeI)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された画像オブジェクトのサイズ（画素単位の幅と高さ）を取得して返します。
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
			if (src is CxImage_Size)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Size)src;

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
				var _src = (CxImage_Size)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Size.This")]
		public TxSizeI This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private TxSizeI m_This = new TxSizeI();

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			// 実行.
			this.This = src.Size;

			// 出力.
			this.DataOut[0].Data = this.This;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_This = task$.Size;
					scope.Add(dst.Assign(src.Ref("Size")));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Depth.get

	/// <summary>
	/// Depth プロパティ（取得）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Depth_get : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Depth_get()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Depth";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された画像オブジェクトのビット深度を取得して返します。
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
			if (src is CxImage_Depth_get)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Depth_get)src;

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
				var _src = (CxImage_Depth_get)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Depth_get.This")]
		public int This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private int m_This = 0;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			// 実行.
			this.This = src.Depth;

			// 出力.
			this.DataOut[0].Data = this.This;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_This = task$.Depth;
					scope.Add(dst.Assign(src.Ref("Depth")));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Depth.set

	/// <summary>
	/// Depth プロパティ（設定）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Depth_set : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Depth_set()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Depth";
			this.IconKey = "Unit-PropertySet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// ビット深度 [範囲:0=最大値, 1~64:指定値] [既定値:0]
			/// </summary>
			DataParam0,
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
			if (src is CxImage_Depth_set)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Depth_set)src;

				this.Value = _src.Value;

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
				var _src = (CxImage_Depth_set)src;

				if (this.Value != _src.Value) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// ビット深度 [範囲:0=最大値, 1~64:指定値] [既定値:0]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Depth_set.Value")]
		public int Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}
		private int m_Value = 0;

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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Value = Convert.ToInt32(this.DataParam[iii].Data);  break;
					}
				}
			}

			// 実行.
			src.Depth = this.Value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Value
					var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Value));

					// task$.Depth = task#_Value;
					scope.Add(new CodeAssignStatement(src.Ref("Depth"), value));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// Methods
	// 

	#region Load

	/// <summary>
	/// Load メソッド。画像ファイルを読み込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Load : CxTaskUnit
		, IxTaskOutputImage
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Load()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Load";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("FileName", new Type[] {typeof(string)}),
				new CxTaskPortIn("Unpacking", new Type[] {typeof(bool)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Body", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 書き込み先の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// ファイル名 [拡張子: bmp,dib,png,jpg,jpeg,tif,tiff,raw]
			/// </summary>
			DataParam0,

			/// <summary>
			/// アンパッキングの指示 [既定値:true]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 指定された画像オブジェクトに画像ファイルを読み込んで返します。
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
			if (src is CxImage_Load)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Load)src;

				this.FileName = _src.FileName;
				this.Unpacking = _src.Unpacking;

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
				var _src = (CxImage_Load)src;

				if (this.FileName != _src.FileName) return false;
				if (this.Unpacking != _src.Unpacking) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// ファイル名 [拡張子: bmp,dib,png,jpg,jpeg,tif,tiff,raw]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Load.FileName")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}
		private string m_FileName = "";

		#region FileNameEditor

		private class FileNameEditor : UITypeEditor
		{
			public FileNameEditor()
			{
			}
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				// 現在のディレクトリ.
				string base_dir = "";
				var parent = context.Instance as CxImage_Load;
				if (parent != null)
				{
					if (!string.IsNullOrWhiteSpace(parent.FileName))
					{
						string dir = System.IO.Path.GetDirectoryName(parent.FileName);
						if (!string.IsNullOrWhiteSpace(dir))
						{
							dir = System.IO.Path.GetFullPath(dir);
							if (System.IO.Directory.Exists(dir))
							{
								base_dir = dir;
							}
						}
					}
				}

				// ダイアログ表示.
				var dlg = new OpenFileDialog();
				dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				dlg.Filter += "|All files (*.*)|*.*";
				dlg.Multiselect = false;
				if (string.IsNullOrWhiteSpace(base_dir) == false)
					dlg.InitialDirectory = base_dir;
				if (string.IsNullOrWhiteSpace(parent.FileName) == false)
					dlg.FileName = System.IO.Path.GetFileName(parent.FileName);

				if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					return dlg.FileName;
				}

				return base.EditValue(context, provider, value);
			}
		}

		#endregion

		/// <summary>
		/// アンパッキングの指示 [既定値:true]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Load.Unpacking")]
		public bool Unpacking
		{
			get { return m_Unpacking; }
			set { m_Unpacking = value; }
		}
		private bool m_Unpacking = true;

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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.FileName = (string)this.DataParam[iii].Data;  break;
						case 1: this.Unpacking = Convert.ToBoolean(this.DataParam[iii].Data); break;
					}
				}
			}

			// ファイルパス.
			var filename = this.FileName;
			// 相対パスの場合は、基準ディレクトリを付加する.
			if (string.IsNullOrWhiteSpace(this.FileName) == false &&
				string.IsNullOrWhiteSpace(e.BaseDir) == false)
			{
				if (System.IO.Path.IsPathRooted(this.FileName) == false)
				{
					filename = System.IO.Path.Combine(e.BaseDir, this.FileName);
				}
			}
			
			// 実行.
			src.Load(filename, this.Unpacking);

			// 出力.
			this.DataOut[0].Data = src;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dataout0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var filename = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.FileName));
					var unpacking = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Unpacking));

					// task$.Load(filename, unpacking);
					scope.Add(datain0.Call("Load", filename, unpacking));

					// task#_Src = task$;
					scope.Add(dataout0.Assign(datain0));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

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
			// 現在のディレクトリ.
			string base_dir = "";
			var parent = this;
			if (parent != null)
			{
				if (!string.IsNullOrWhiteSpace(parent.FileName))
				{
					string dir = System.IO.Path.GetDirectoryName(parent.FileName);
					if (!string.IsNullOrWhiteSpace(dir))
					{
						dir = System.IO.Path.GetFullPath(dir);
						if (System.IO.Directory.Exists(dir))
						{
							base_dir = dir;
						}
					}
				}
			}

			// ダイアログ表示.
			var dlg = new OpenFileDialog();
			dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
			dlg.Filter += "|All files (*.*)|*.*";
			dlg.Multiselect = false;
			if (string.IsNullOrWhiteSpace(base_dir) == false)
				dlg.InitialDirectory = base_dir;
			if (string.IsNullOrWhiteSpace(parent.FileName) == false)
				dlg.FileName = System.IO.Path.GetFileName(parent.FileName);

			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.FileName = dlg.FileName;
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

	#region Save

	/// <summary>
	/// Save メソッド。画像ファイルへ保存します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Save : CxTaskUnit
		, IxTaskOutputImage
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Save()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Save";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("FileName", new Type[] {typeof(string)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 保存対象の画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 保存先のファイル名。[拡張子: bmp,dib,png,jpg,jpeg,tif,tiff,raw] 空文字を指定した場合は処理を行いません。
			/// </summary>
			DataParam0,
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
			if (src is CxImage_Save)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Save)src;

				this.FileName = _src.FileName;

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
				var _src = (CxImage_Save)src;

				if (this.FileName != _src.FileName) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// ファイル名 [拡張子: bmp,dib,png,jpg,jpeg,tif,tiff,raw] 空文字を指定した場合は処理を行いません。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Save.FileName")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}
		private string m_FileName = "";

		#region FileNameEditor

		private class FileNameEditor : UITypeEditor
		{
			public FileNameEditor()
			{
			}
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				// 現在のディレクトリ.
				string base_dir = "";
				var parent = context.Instance as CxImage_Save;
				if (parent != null)
				{
					if (!string.IsNullOrWhiteSpace(parent.FileName))
					{
						string dir = System.IO.Path.GetDirectoryName(parent.FileName);
						if (!string.IsNullOrWhiteSpace(dir))
						{
							dir = System.IO.Path.GetFullPath(dir);
							if (System.IO.Directory.Exists(dir))
							{
								base_dir = dir;
							}
						}
					}
				}

				// ダイアログ表示.
				var dlg = new SaveFileDialog();
				dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				dlg.Filter += "|All files (*.*)|*.*";
				if (string.IsNullOrWhiteSpace(base_dir) == false)
					dlg.InitialDirectory = base_dir;
				if (string.IsNullOrWhiteSpace(parent.FileName) == false)
					dlg.FileName = System.IO.Path.GetFileName(parent.FileName);

				if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					return dlg.FileName;
				}

				return base.EditValue(context, provider, value);
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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.FileName = (string)this.DataParam[iii].Data;  break;
					}
				}
			}

			// 実行.
			if (string.IsNullOrWhiteSpace(this.FileName) == false)
				src.Save(this.FileName);

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// parameters
					var filename = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.FileName));

					// datain0.Save(filename);
					scope.Add(datain0.Call("Save", filename));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

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
			// 現在のディレクトリ.
			string base_dir = "";
			var parent = this;
			if (parent != null)
			{
				if (!string.IsNullOrWhiteSpace(parent.FileName))
				{
					string dir = System.IO.Path.GetDirectoryName(parent.FileName);
					if (!string.IsNullOrWhiteSpace(dir))
					{
						dir = System.IO.Path.GetFullPath(dir);
						if (System.IO.Directory.Exists(dir))
						{
							base_dir = dir;
						}
					}
				}
			}

			// ダイアログ表示.
			var dlg = new SaveFileDialog();
			dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
			dlg.Filter += "|All files (*.*)|*.*";
			if (string.IsNullOrWhiteSpace(base_dir) == false)
				dlg.InitialDirectory = base_dir;
			if (string.IsNullOrWhiteSpace(parent.FileName) == false)
				dlg.FileName = System.IO.Path.GetFileName(parent.FileName);

			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.FileName = dlg.FileName;
			}
			return null;	// modal の場合は null を返す。modeless の場合は生成したフォームを返す。
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

	#region Resize

	/// <summary>
	/// Resize メソッド。画像オブジェクトの再確保を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Resize : CxTaskUnit
		, IxTaskOutputImage
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Resize()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Resize";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("ImageSize", new Type[] {typeof(TxImageSize)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Body", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像サイズ。幅または高さが 0 の場合は未確保になります。
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定されたオブジェクトの再確保を行って返します。
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
			if (src is CxImage_Resize)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Resize)src;

				this.ImageSize = _src.ImageSize;

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
				var _src = (CxImage_Resize)src;

				if (this.ImageSize != _src.ImageSize) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像サイズ。幅または高さが 0 の場合は未確保になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Resize.ImageSize")]
		[Editor(typeof(ImageSizeEditor), typeof(UITypeEditor))]
		public TxImageSize ImageSize
		{
			get { return m_ImageSize; }
			set { m_ImageSize = value; }
		}
		private TxImageSize m_ImageSize = new TxImageSize();

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
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.ImageSize = (TxImageSize)this.DataParam[iii].Data;  break;
					}
				}
			}

			// 実行.
			src.Resize(this.ImageSize);

			// 出力.
			this.DataOut[0].Data = src;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dataout0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// image_size
					var image_size = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.ImageSize));

					// task$.Resize(image_size);
					scope.Add(datain0.Call("Resize", image_size));

					// task#_Src = task$;
					scope.Add(dataout0.Assign(datain0));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

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
			dlg.ImageSize = this.ImageSize;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.ImageSize = dlg.ImageSize;
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

	#region Reset

	/// <summary>
	/// Reset メソッド。全ての要素を 0 初期化します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Reset : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Reset()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Reset";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Body", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 処理を実行後に指定された処理対象のオブジェクトをそのまま返します。
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
			if (src is CxImage_Reset)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Reset)src;

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
				var _src = (CxImage_Reset)src;
			}
			#endregion

			return true;
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			// 実行.
			src.Reset();

			// 出力.
			this.DataOut[0].Data = src;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dataout0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// task$.Reset();
					scope.Add(datain0.Call("Reset"));

					// task#_Src = task$;
					scope.Add(dataout0.Assign(datain0));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Clear

	/// <summary>
	/// Clear メソッド。画素値を任意の値でクリアします。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Clear : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Clear()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Clear";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Value", new Type[] {typeof(object)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Body", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// クリアに使用する値。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 指定されたオブジェクトの再確保を行って返します。
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
			if (src is CxImage_Clear)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Clear)src;

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
				var _src = (CxImage_Clear)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// クリアに使用する値。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Clear.Value")]
		public object Value
		{
			get
			{
				if (this.DataIn == null) return null;
				if (this.DataIn.Length <= 1) return null;
				if (this.DataIn[1].IsConnected)
				{
					return this.DataIn[1].Data;
				}
				else
				{
					return null;
				}
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

			// 引数の取得.
			for(int i=0 ; i<this.DataIn.Length ; i++)
				this.DataIn[i].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;
			var value = this.DataIn[1].Data;

			// 実行.
			src.Clear(value);

			// 出力.
			this.DataOut[0].Data = src;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var value = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// task$.Clear(value);
					scope.Add(src.Call("Clear", value));

					// task#_Src = task$;
					scope.Add(dst.Assign(src));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Clone

	/// <summary>
	/// Clone メソッド。指定されたオブジェクトのクローンを生成します。複製時の型変換も可能です。例えば濃淡化やカラー化としても利用できます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Clone : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Clone()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Clone";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Model", new Type[] {typeof(TxModel)}),
				new CxTaskPortIn("Channels", new Type[] {typeof(int)}),
				new CxTaskPortIn("Scale", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 複製元
			/// </summary>
			DataIn0,

			/// <summary>
			/// 複製先の型 [Type: None=同一型、その他=指定型] [Pack: 0=同一パック数、1~=指定パック数]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 複製先のチャネル数 [範囲:0=同一チャネル数、1~16=指定チャネル数]
			/// </summary>
			DataParam1,

			/// <summary>
			/// スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する] 
			/// </summary>
			DataParam2,

			/// <summary>
			/// 指定されたオブジェクトのクローンを生成して返します。
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
			if (src is CxImage_Clone)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Clone)src;

				this.Model = _src.Model;
				this.Channels = _src.Channels;
				this.Scale = _src.Scale;

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
				var _src = (CxImage_Clone)src;

				if (this.Model != _src.Model) return false;
				if (this.Channels != _src.Channels) return false;
				if (this.Scale != _src.Scale) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 複製先の型 [Type: None=同一型、その他=指定型] [Pack: 0=同一パック数、1~=指定パック数]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Clone.Model")]
		public TxModel Model
		{
			get { return m_Model; }
			set { m_Model = value; }
		}
		private TxModel m_Model = TxModel.Default;

		/// <summary>
		/// 複製先のチャネル数 [範囲:0=同一チャネル数、1~16=指定チャネル数]
		/// </summary>
		[TypeConverter(typeof(ChannelsConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Clone.Channels")]
		public int Channels
		{
			get { return m_Channels; }
			set { m_Channels = value; }
		}
		private int m_Channels = 0;

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
					for (int i = 0; i <= XIE.Defs.XIE_IMAGE_CHANNELS_MAX; i++)
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

		/// <summary>
		/// スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する] 
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Clone.Scale")]
		public double Scale
		{
			get { return m_Scale; }
			set { m_Scale = value; }
		}
		private double m_Scale = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Clone.This")]
		public CxImage This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private CxImage m_This = new CxImage();

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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Model = (TxModel)this.DataParam[iii].Data;  break;
						case 1: this.Channels = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Scale = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			this.This = src.Clone(this.Model, this.Channels, this.Scale);

			// 出力.
			this.DataOut[0].Data = this.This;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					var model = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Model));
					var channels = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Channels));
					var scale = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Scale));

					// task#_This = task$.Clone(model, channels, scale);
					scope.Add(dst.Assign(src.Call("Clone", model, channels, scale)));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Child

	/// <summary>
	/// Child メソッド。指定されたオブジェクトの画像領域にアタッチした新しいオブジェクトを生成します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Child : CxTaskUnit
		, IxTaskOutputImage
		, IxTaskOverlayRendering
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Child()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Child";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("ChannelNo", new Type[] {typeof(int)}),
				new CxTaskPortIn("Bounds", new Type[] {typeof(TxRectangleI)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// アタッチ先の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// アタッチするチャネル指標 [範囲:-1=全チャネル、0~=指定チャネル]
			/// </summary>
			DataParam0,

			/// <summary>
			/// アタッチする範囲 (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)
			/// </summary>
			DataParam1,

			/// <summary>
			/// 指定されたオブジェクトのクローンを生成して返します。
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
			if (src is CxImage_Child)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Child)src;

				this.ChannelNo = _src.ChannelNo;
				this.Bounds = _src.Bounds;

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
				var _src = (CxImage_Child)src;

				if (this.ChannelNo != _src.ChannelNo) return false;
				if (this.Bounds != _src.Bounds) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// アタッチするチャネル指標 [範囲:-1=全チャネル、0~=指定チャネル]
		/// </summary>
		[TypeConverter(typeof(ChannelNoConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Child.ChannelNo")]
		public int ChannelNo
		{
			get { return m_ChannelNo; }
			set { m_ChannelNo = value; }
		}
		private int m_ChannelNo = -1;

		#region ChannelNoConverter

		class ChannelNoConverter : Int32Converter
		{
			public ChannelNoConverter()
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
					items.Add(-1);
					for (int i = 0; i < XIE.Defs.XIE_IMAGE_CHANNELS_MAX; i++)
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

		/// <summary>
		/// アタッチする範囲 (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Child.Bounds")]
		public TxRectangleI Bounds
		{
			get { return m_Bounds; }
			set { m_Bounds = value; }
		}
		private TxRectangleI m_Bounds = new TxRectangleI();

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Child.This")]
		public CxImage This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private CxImage m_This = new CxImage();

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
			if (this.This != null)
				this.This.Dispose();

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.ChannelNo = Convert.ToInt32(this.DataParam[iii].Data);  break;
						case 1: this.Bounds = (TxRectangleI)this.DataParam[iii].Data;  break;
					}
				}
			}

			// 実行.
			this.This = src.Child(this.ChannelNo, this.Bounds);

			// 出力.
			this.DataOut[0].Data = this.This;

			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					var channelno = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.ChannelNo));
					var bounds = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Bounds));

					// task#_This = task$.Child(channelno, bounds);
					scope.Add(dst.Assign(src.Call("Child", channelno, bounds)));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
			{
				var src = this.DataIn[0].Data as CxImage;
				if (src == null || src.IsValid == false)
				{
					if (this.m_OutputImage != null)
						this.m_OutputImage.Dispose();
					view.Image = null;
				}
				else if (this.This.IsValid == false)
				{
					if (this.m_OutputImage != null)
						this.m_OutputImage.Dispose();
					view.Image = null;
				}
				else
				{
					using (var act = src.Child(this.ChannelNo))
					{
						ApiHelper.OutputImage(view, this.m_OutputImage, act);
					}
				}
			}
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion

		#region IxTaskOverlayRendering の実装:

		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void IxTaskOverlayRendering.Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.DataIn[0].Data is CxImage)
			{
				var src = (CxImage)this.DataIn[0].Data;
				var size1 = src.Size;
				var size2 = e.Canvas.ImageSize;

				if (this.This.IsValid && size1 == size2)
				{
					var figure = new XIE.GDI.CxGdiRectangle(0, 0, this.This.Width, this.This.Height);
					figure.PenColor = Color.Blue;
					e.Canvas.DrawOverlay(figure, XIE.GDI.ExGdiScalingMode.TopLeft);
				}
			}
		}

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		bool IxTaskOverlayRendering.EnableRendering
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

	#region Statistics

	/// <summary>
	/// Statistics メソッド。画像の濃度統計を算出します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Statistics : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Statistics()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Statistics";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Channel", new Type[] {typeof(int)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(TxStatistics)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// チャネル指標 [範囲:0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。入力画像のサイズと一致しており、チャネル数が 1 または入力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 指定された画像の濃度統計を算出して返します。
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
			if (src is CxImage_Statistics)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Statistics)src;

				this.Channel = _src.Channel;

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
				var _src = (CxImage_Statistics)src;

				if (this.Channel != _src.Channel) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// チャネル指標 [範囲:0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_Statistics.Channel")]
		public int Channel
		{
			get { return m_Channel; }
			set { m_Channel = value; }
		}
		private int m_Channel = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 統計データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxImage_Statistics.This")]
		public TxStatistics This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private TxStatistics m_This = new TxStatistics();

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			if (this.DataParam[0].CheckValidity())
				this.Channel = Convert.ToInt32(this.DataParam[0].Data);
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;

			// 実行.
			this.This = src.Statistics(this.Channel, mask);

			// 出力.
			this.DataOut[0].Data = this.This;
	
			return;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dataout0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// channel
					var channel = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Channel));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_This = task$.Statistics(channel, mask);
					scope.Add(dataout0.Assign(datain0.Call("Statistics", channel, mask)));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// Methods (Exif)
	// 

	#region Exif.get

	/// <summary>
	/// Exif の取得
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Exif_get : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Exif_get()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Exif";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(TxExif)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された画像オブジェクトの Exif 構造体を取得して返します。
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
			if (src is CxImage_Exif_get)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Exif_get)src;

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
				var _src = (CxImage_Exif_get)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
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
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			// 出力.
			this.DataOut[0].Data = src.Exif();
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_This = task$.Exif();
					scope.Add(dst.Assign(src.Call("Exif")));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region Exif.set

	/// <summary>
	/// Exif の設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_Exif_set : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_Exif_set()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "Exif";
			this.IconKey = "Unit-PropertySet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Value", new Type[] {typeof(TxExif)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 設定する Exif 構造体
			/// </summary>
			DataIn1,
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
			if (src is CxImage_Exif_set)
			{
				base.CopyFrom(src);

				var _src = (CxImage_Exif_set)src;

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
				var _src = (CxImage_Exif_set)src;
			}
			#endregion

			return true;
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
			for(int i=0 ; i<this.DataIn.Length ; i++)
				this.DataIn[i].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;
			var exif = (TxExif)this.DataIn[1].Data;

			// 実行.
			src.Exif(exif);
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var exif = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// src.Exif( exif );
					scope.Add(src.Call("Exif", exif));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region ExifCopy

	/// <summary>
	/// Exif の加工・複製
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImage_ExifCopy : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage_ExifCopy()
			: base()
		{
			this.Category = "XIE.CxImage";
			this.Name = "ExifCopy";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Exif", new Type[] {typeof(TxExif), typeof(CxExif), typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("LTC", new Type[] {typeof(bool)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 処理対象の画像オブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 複製元の Exif 構造体 (※ ダイアグラムの簡略化の為、TxExif の他、CxExif や CxImage も許容します。)
			/// </summary>
			DataIn1,

			/// <summary>
			/// 複製後の Exif のタイムゾーン [true=ローカル時刻、false=協定世界時]
			/// </summary>
			DataParam0,
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 複製後の Exif のタイムゾーン [true=ローカル時刻、false=協定世界時]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImage_ExifCopy.LTC")]
		public bool LTC
		{
			get { return m_LTC; }
			set { m_LTC = value; }
		}
		private bool m_LTC = true;

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
			if (src is CxImage_ExifCopy)
			{
				base.CopyFrom(src);

				var _src = (CxImage_ExifCopy)src;

				this.LTC = _src.LTC;

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
				var _src = (CxImage_ExifCopy)src;

				if (this.LTC != _src.LTC) return false;
			}
			#endregion

			return true;
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
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.LTC = Convert.ToBoolean(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			var data = this.DataIn[1].Data;
			if (data is TxExif)
			{
				var exif = (TxExif)this.DataIn[1].Data;
				src.ExifCopy(exif, this.LTC);
			}
			else if (data is CxExif)
			{
				var exif = (CxExif)this.DataIn[1].Data;
				src.ExifCopy(exif.Tag(), this.LTC);
			}
			else if (data is CxImage)
			{
				var image = (CxImage)this.DataIn[1].Data;
				src.ExifCopy(image.Exif(), this.LTC);
			}
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// parameters
					var ltc = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.LTC));

					int mode = -1;

					#region 接続された型の確認:
					var data = this.DataIn[1].Data;
					if (data is TxExif) mode = 0;
					else if (data is CxExif) mode = 1;
					else if (data is CxImage) mode = 2;
					else if (this.DataIn[1].IsConnected)
					{
						if (this.DataIn[1].ReferencePort.Types.Length > 0)
						{
							var type = this.DataIn[1].ReferencePort.Types[0];
							if (type == typeof(TxExif)) mode = 0;
							else if (type == typeof(CxExif)) mode = 1;
							else if (type == typeof(CxImage)) mode = 2;
						}
					}
					#endregion

					switch (mode)
					{
					case 0:
						{
							var exif = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
							// src.ExifCopy( exif, ltc );
							scope.Add(src.Call("ExifCopy", exif, ltc));
						}
						break;
					case 1:
						{
							var exif = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
							// src.ExifCopy( exif.Tag(), ltc );
							scope.Add(src.Call("ExifCopy", exif.Call("Tag"), ltc));
						}
						break;
					case 2:
						{
							var image = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
							// src.ExifCopy( image.Exif(), ltc );
							scope.Add(src.Call("ExifCopy", image.Call("Exif"), ltc));
						}
						break;
					}
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataIn[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion
}
