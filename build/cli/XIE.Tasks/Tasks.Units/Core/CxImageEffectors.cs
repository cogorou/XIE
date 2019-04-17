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
	#region CxBinarize1

	/// <summary>
	/// ２値化（単一閾値）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_Binarize1 : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_Binarize1()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxBinarize1";
			this.IconKey = "Image-Bin";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Threshold", new Type[] {typeof(double)}),
				new CxTaskPortIn("UseAbs", new Type[] {typeof(bool)}),
				new CxTaskPortIn("Value", new Type[] {typeof(TxRangeD)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:U8] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 閾値 (Threshold≦src を真とします。)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 入力値を絶対値として扱うか否か
			/// </summary>
			DataParam3,

			/// <summary>
			/// 出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)
			/// </summary>
			DataParam4,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_Binarize1)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_Binarize1)src;

				this.Threshold = _src.Threshold;
				this.UseAbs = _src.UseAbs;
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
				var _src = (CxImageEffectors_Binarize1)src;

				if (this.Threshold != _src.Threshold) return false;
				if (this.UseAbs != _src.UseAbs) return false;
				if (this.Value != _src.Value) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 閾値 (Threshold≦src を真とします。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize1.Threshold")]
		public double Threshold
		{
			get { return m_Threshold; }
			set { m_Threshold = value; }
		}
		private double m_Threshold = 128;

		/// <summary>
		/// 入力値を絶対値として扱うか否か
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize1.UseAbs")]
		public bool UseAbs
		{
			get { return m_UseAbs; }
			set { m_UseAbs = value; }
		}
		private bool m_UseAbs = false;

		/// <summary>
		/// 出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize1.Value")]
		public TxRangeD Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}
		private TxRangeD m_Value = new TxRangeD(0, 1);

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Threshold = Convert.ToDouble(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.UseAbs = Convert.ToBoolean(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.Value = (TxRangeD)this.DataParam[4].Data;
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxBinarize1(this.Threshold, this.UseAbs, this.Value);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var threshold = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Threshold));
						var useAbs = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.UseAbs));
						var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Value));

						// var effector = new XIE.Effectors.CxBinarize1(threshold, useAbs, value);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxBinarize1));
						brace.TryStatements.Add(effector.Declare(effector.New(threshold, useAbs, value)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data, 1);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxBinarize2

	/// <summary>
	/// ２値化（バンド閾値）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_Binarize2 : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_Binarize2()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxBinarize2";
			this.IconKey = "Image-Bin";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Threshold", new Type[] {typeof(TxRangeD)}),
				new CxTaskPortIn("UseAbs", new Type[] {typeof(bool)}),
				new CxTaskPortIn("Value", new Type[] {typeof(TxRangeD)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:U8] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 閾値 (Threshold.Lower≦src≦Threshold.Upper を真とします。)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 入力値を絶対値として扱うか否か
			/// </summary>
			DataParam3,

			/// <summary>
			/// 出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)
			/// </summary>
			DataParam4,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_Binarize2)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_Binarize2)src;

				this.Threshold = _src.Threshold;
				this.UseAbs = _src.UseAbs;
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
				var _src = (CxImageEffectors_Binarize2)src;

				if (this.Threshold != _src.Threshold) return false;
				if (this.UseAbs != _src.UseAbs) return false;
				if (this.Value != _src.Value) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 閾値 (Threshold.Lower≦src≦Threshold.Upper を真とします。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize2.Threshold")]
		public TxRangeD Threshold
		{
			get { return m_Threshold; }
			set { m_Threshold = value; }
		}
		private TxRangeD m_Threshold = new TxRangeD(64, 192);

		/// <summary>
		/// 入力値を絶対値として扱うか否か
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize2.UseAbs")]
		public bool UseAbs
		{
			get { return m_UseAbs; }
			set { m_UseAbs = value; }
		}
		private bool m_UseAbs = false;

		/// <summary>
		/// 出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Binarize2.Value")]
		public TxRangeD Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}
		private TxRangeD m_Value = new TxRangeD(0, 1);

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Threshold = (TxRangeD)this.DataParam[2].Data;
			if (this.DataParam[3].CheckValidity())
				this.UseAbs = Convert.ToBoolean(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.Value = (TxRangeD)this.DataParam[4].Data;
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxBinarize2(this.Threshold, this.UseAbs, this.Value);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var threshold = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Threshold));
						var useAbs = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.UseAbs));
						var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Value));

						// var effector = new XIE.Effectors.CxBinarize2(threshold, useAbs, value);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxBinarize2));
						brace.TryStatements.Add(effector.Declare(effector.New(threshold, useAbs, value)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data, 1);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxGammaConverter

	/// <summary>
	/// ガンマ変換 (入力画像をガンマ変換して出力画像に格納します。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_GammaConverter : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_GammaConverter()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxGammaConverter";
			this.IconKey = "Parser-EnhanceDark";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Depth", new Type[] {typeof(int)}),
				new CxTaskPortIn("Gamma", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
			/// </summary>
			DataParam2,

			/// <summary>
			/// ガンマ値 [0 以外] ※ 1.0 が無変換を意味します。
			/// </summary>
			DataParam3,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_GammaConverter)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_GammaConverter)src;

				this.Depth = _src.Depth;
				this.Gamma = _src.Gamma;

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
				var _src = (CxImageEffectors_GammaConverter)src;

				if (this.Depth != _src.Depth) return false;
				if (this.Gamma != _src.Gamma) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_GammaConverter.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

		/// <summary>
		/// ガンマ値 [0.0 以外] ※ 1.0 が無変換を意味します。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_GammaConverter.Gamma")]
		public double Gamma
		{
			get { return m_Gamma; }
			set { m_Gamma = value; }
		}
		private double m_Gamma = 1.0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Depth = Convert.ToInt32(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.Gamma = Convert.ToDouble(this.DataParam[3].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxGammaConverter(this.Depth, this.Gamma);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Depth));
						var gamma = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Gamma));

						// var effector = new XIE.Effectors.CxGammaConverter(depth, gamma);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxGammaConverter));
						brace.TryStatements.Add(effector.Declare(effector.New(depth, gamma)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxRgbToGray

	/// <summary>
	/// RGB 変換 (入力の RGB 画像の各画素に指定の係数を乗算して変換します。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_RgbToGray : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_RgbToGray()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxRgbToGray";
			this.IconKey = "Image-Gray";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Scale", new Type[] {typeof(double)}),
				new CxTaskPortIn("RedRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("GreenRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("BlueRatio", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 赤成分の変換係数 [0~] [既定値:0.299]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 緑成分の変換係数 [0~] [既定値:0.587]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 青成分の変換係数 [0~] [既定値:0.114]
			/// </summary>
			DataParam5,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_RgbToGray)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_RgbToGray)src;

				this.Scale = _src.Scale;
				this.RedRatio = _src.RedRatio;
				this.GreenRatio = _src.GreenRatio;
				this.BlueRatio = _src.BlueRatio;

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
				var _src = (CxImageEffectors_RgbToGray)src;

				if (this.Scale != _src.Scale) return false;
				if (this.RedRatio != _src.RedRatio) return false;
				if (this.GreenRatio != _src.GreenRatio) return false;
				if (this.BlueRatio != _src.BlueRatio) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbToGray.Scale")]
		public double Scale
		{
			get { return m_Scale; }
			set { m_Scale = value; }
		}
		private double m_Scale = 1.0;

		/// <summary>
		/// 赤成分の変換係数 [0~1] [既定値:0.299]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbToGray.RedRatio")]
		public double RedRatio
		{
			get { return m_RedRatio; }
			set { m_RedRatio = value; }
		}
		private double m_RedRatio = 0.299;

		/// <summary>
		/// 緑成分の変換係数 [0~1] [既定値:0.587]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbToGray.GreenRatio")]
		public double GreenRatio
		{
			get { return m_GreenRatio; }
			set { m_GreenRatio = value; }
		}
		private double m_GreenRatio = 0.587;

		/// <summary>
		/// 青成分の変換係数 [0~1] [既定値:0.114]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbToGray.BlueRatio")]
		public double BlueRatio
		{
			get { return m_BlueRatio; }
			set { m_BlueRatio = value; }
		}
		private double m_BlueRatio = 0.114;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Scale = Convert.ToDouble(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.RedRatio = Convert.ToDouble(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.GreenRatio = Convert.ToDouble(this.DataParam[4].Data);
			if (this.DataParam[5].CheckValidity())
				this.BlueRatio = Convert.ToDouble(this.DataParam[5].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxRgbToGray(this.Scale, this.RedRatio, this.GreenRatio, this.BlueRatio);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var scale = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Scale));
						var ratioR = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.RedRatio));
						var ratioG = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.GreenRatio));
						var ratioB = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.BlueRatio));

						// var effector = new XIE.Effectors.CxRgbToGray(scale, ratioR, ratioG, ratioB);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxRgbToGray));
						brace.TryStatements.Add(effector.Declare(effector.New(scale, ratioR, ratioG, ratioB)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxRgbToHsv

	/// <summary>
	/// 色空間変換 (RGB から HSV への変換) 入力の RGB 画像を HSV に変換します。H,S,V を 0,1,2 チャネル（または 0,1,2 フィールド）に格納します。各レンジは 0.0~1.0 です。Hue は 0~360 (度) を 0.0~1.0 にレンジ変換しています。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_RgbToHsv : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_RgbToHsv()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxRgbToHsv";
			this.IconKey = "Parser-ColorConvert";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Depth", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:F64] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
			/// </summary>
			DataParam2,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_RgbToHsv)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_RgbToHsv)src;

				this.Depth = _src.Depth;

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
				var _src = (CxImageEffectors_RgbToHsv)src;

				if (this.Depth != _src.Depth) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbToHsv.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Depth = Convert.ToInt32(this.DataParam[2].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxRgbToHsv(this.Depth);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Depth));

						// var effector = new XIE.Effectors.CxRgbToHsv(depth);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxRgbToHsv));
						brace.TryStatements.Add(effector.Declare(effector.New(depth)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxHsvToRgb

	/// <summary>
	/// 色空間変換 (HSV から RGB への変換) 入力の HSV 画像を RGB に変換します。H,S,V の各レンジは 0.0~1.0 です。Hue は 0.0~1.0 を 0~360 (度) にレンジ変換して計算します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_HsvToRgb : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_HsvToRgb()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxHsvToRgb";
			this.IconKey = "Parser-ColorConvert";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Depth", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。Channels や Pack が異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 出力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
			/// </summary>
			DataParam2,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_HsvToRgb)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_HsvToRgb)src;

				this.Depth = _src.Depth;

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
				var _src = (CxImageEffectors_HsvToRgb)src;

				if (this.Depth != _src.Depth) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 出力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_HsvToRgb.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Depth = Convert.ToInt32(this.DataParam[2].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxHsvToRgb(this.Depth);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Depth));

						// var effector = new XIE.Effectors.CxHsvToRgb(depth);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxHsvToRgb));
						brace.TryStatements.Add(effector.Declare(effector.New(depth)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxHsvConverter

	/// <summary>
	/// HSV 変換 (入力の RGB 画像を HSV に変換して指定のパラメータで調整した後、RGB に再変換します。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_HsvConverter : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_HsvConverter()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxHsvConverter";
			this.IconKey = "Parser-ColorConvert";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Depth", new Type[] {typeof(int)}),
				new CxTaskPortIn("HueDir", new Type[] {typeof(int)}),
				new CxTaskPortIn("SaturationFactor", new Type[] {typeof(double)}),
				new CxTaskPortIn("ValueFactor", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
			/// </summary>
			DataParam2,

			/// <summary>
			/// 色相の回転方向 [0~±180 または 0~360] ※ 0 が変換なしを意味します。
			/// </summary>
			DataParam3,

			/// <summary>
			/// 彩度の変換係数 [0.0~] ※ 1.0 が変換なしを意味します。
			/// </summary>
			DataParam4,

			/// <summary>
			/// 明度の変換係数 [0.0~] ※ 1.0 が変換なしを意味します。
			/// </summary>
			DataParam5,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_HsvConverter)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_HsvConverter)src;

				this.Depth = _src.Depth;
				this.HueDir = _src.HueDir;
				this.SaturationFactor = _src.SaturationFactor;
				this.ValueFactor = _src.ValueFactor;

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
				var _src = (CxImageEffectors_HsvConverter)src;

				if (this.Depth != _src.Depth) return false;
				if (this.HueDir != _src.HueDir) return false;
				if (this.SaturationFactor != _src.SaturationFactor) return false;
				if (this.ValueFactor != _src.ValueFactor) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_HsvConverter.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

		/// <summary>
		/// 色相の回転方向 [0~±180 または 0~360] ※ 0 が変換なしを意味します。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_HsvConverter.HueDir")]
		public int HueDir
		{
			get { return m_HueDir; }
			set { m_HueDir = value; }
		}
		private int m_HueDir = 0;

		/// <summary>
		/// 彩度の変換係数 [0.0~] ※ 1.0 が変換なしを意味します。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_HsvConverter.SaturationFactor")]
		public double SaturationFactor
		{
			get { return m_SaturationFactor; }
			set { m_SaturationFactor = value; }
		}
		private double m_SaturationFactor = 1.0;

		/// <summary>
		/// 明度の変換係数 [0.0~] ※ 1.0 が変換なしを意味します。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_HsvConverter.ValueFactor")]
		public double ValueFactor
		{
			get { return m_ValueFactor; }
			set { m_ValueFactor = value; }
		}
		private double m_ValueFactor = 1.0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Depth = Convert.ToInt32(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.HueDir = Convert.ToInt32(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.SaturationFactor = Convert.ToDouble(this.DataParam[4].Data);
			if (this.DataParam[5].CheckValidity())
				this.ValueFactor = Convert.ToDouble(this.DataParam[5].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxHsvConverter(this.Depth, this.HueDir, this.SaturationFactor, this.ValueFactor);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Depth));
						var hue_dir = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.HueDir));
						var saturation_factor = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.SaturationFactor));
						var value_factor = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.ValueFactor));

						// var effector = new XIE.Effectors.CxHsvConverter(depth, hue_dir, saturation_factor, value_factor);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxHsvConverter));
						brace.TryStatements.Add(effector.Declare(effector.New(depth, hue_dir, saturation_factor, value_factor)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxRgbConverter

	/// <summary>
	/// RGB 変換 (入力の RGB 画像の各画素に指定の係数を乗算して変換します。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_RgbConverter : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_RgbConverter()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxRgbConverter";
			this.IconKey = "Image-RGB";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("RedRatio", new Type[] {typeof(int)}),
				new CxTaskPortIn("GreenRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("BlueRatio", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 赤成分の変換係数 (%) [0~]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 緑成分の変換係数 (%) [0~]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 青成分の変換係数 (%) [0~]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_RgbConverter)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_RgbConverter)src;

				this.RedRatio = _src.RedRatio;
				this.GreenRatio = _src.GreenRatio;
				this.BlueRatio = _src.BlueRatio;

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
				var _src = (CxImageEffectors_RgbConverter)src;

				if (this.RedRatio != _src.RedRatio) return false;
				if (this.GreenRatio != _src.GreenRatio) return false;
				if (this.BlueRatio != _src.BlueRatio) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbConverter.RedRatio")]
		public double RedRatio
		{
			get { return m_RedRatio; }
			set { m_RedRatio = value; }
		}
		private double m_RedRatio = 1.0;

		/// <summary>
		/// 緑成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbConverter.GreenRatio")]
		public double GreenRatio
		{
			get { return m_GreenRatio; }
			set { m_GreenRatio = value; }
		}
		private double m_GreenRatio = 1.0;

		/// <summary>
		/// 青成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_RgbConverter.BlueRatio")]
		public double BlueRatio
		{
			get { return m_BlueRatio; }
			set { m_BlueRatio = value; }
		}
		private double m_BlueRatio = 1.0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.RedRatio = Convert.ToDouble(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.GreenRatio = Convert.ToDouble(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.BlueRatio = Convert.ToDouble(this.DataParam[4].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxRgbConverter(this.RedRatio, this.GreenRatio, this.BlueRatio);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var ratioR = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RedRatio));
						var ratioG = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.GreenRatio));
						var ratioB = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.BlueRatio));

						// var effector = new XIE.Effectors.CxRgbConverter(ratioR, ratioG, ratioB);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxRgbConverter));
						brace.TryStatements.Add(effector.Declare(effector.New(ratioR, ratioG, ratioB)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxMonochrome

	/// <summary>
	/// モノクローム変換 (入力の RGB 画像を濃淡化します。濃淡化係数は指定の係数によって割合を調整できます。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_Monochrome : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_Monochrome()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxMonochrome";
			this.IconKey = "Image-Gray";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("RedRatio", new Type[] {typeof(int)}),
				new CxTaskPortIn("GreenRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("BlueRatio", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 赤成分の変換係数 (%) [0~]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 緑成分の変換係数 (%) [0~]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 青成分の変換係数 (%) [0~]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_Monochrome)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_Monochrome)src;

				this.RedRatio = _src.RedRatio;
				this.GreenRatio = _src.GreenRatio;
				this.BlueRatio = _src.BlueRatio;

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
				var _src = (CxImageEffectors_Monochrome)src;

				if (this.RedRatio != _src.RedRatio) return false;
				if (this.GreenRatio != _src.GreenRatio) return false;
				if (this.BlueRatio != _src.BlueRatio) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Monochrome.RedRatio")]
		public double RedRatio
		{
			get { return m_RedRatio; }
			set { m_RedRatio = value; }
		}
		private double m_RedRatio = 1.0;

		/// <summary>
		/// 緑成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Monochrome.GreenRatio")]
		public double GreenRatio
		{
			get { return m_GreenRatio; }
			set { m_GreenRatio = value; }
		}
		private double m_GreenRatio = 1.0;

		/// <summary>
		/// 青成分の変換係数 (%) [0~1]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_Monochrome.BlueRatio")]
		public double BlueRatio
		{
			get { return m_BlueRatio; }
			set { m_BlueRatio = value; }
		}
		private double m_BlueRatio = 1.0;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.RedRatio = Convert.ToDouble(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.GreenRatio = Convert.ToDouble(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.BlueRatio = Convert.ToDouble(this.DataParam[4].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxMonochrome(this.RedRatio, this.GreenRatio, this.BlueRatio);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var ratioR = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.RedRatio));
						var ratioG = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.GreenRatio));
						var ratioB = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.BlueRatio));

						// var effector = new XIE.Effectors.CxMonochrome(ratioR, ratioG, ratioB);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxMonochrome));
						brace.TryStatements.Add(effector.Declare(effector.New(ratioR, ratioG, ratioB)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	#region CxPartColor

	/// <summary>
	/// パートカラー変換 (RGB 色空間のカラー画像の各画素が特定の色相範囲内 (HueDir±HueRange) にあるか否かを判定し、範囲内であれば入力元の値をそのまま出力し、範囲外であれば濃淡化して出力します。)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageEffectors_PartColor : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEffectors_PartColor()
			: base()
		{
			this.Category = "XIE.Effectors";
			this.Name = "CxPartColor";
			this.IconKey = "Parser-PartColor";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Depth", new Type[] {typeof(int)}),
				new CxTaskPortIn("HueDir", new Type[] {typeof(int)}),
				new CxTaskPortIn("HueRange", new Type[] {typeof(int)}),
				new CxTaskPortIn("RedRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("GreenRatio", new Type[] {typeof(double)}),
				new CxTaskPortIn("BlueRatio", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
			/// </summary>
			DataParam2,

			/// <summary>
			/// 抽出する色相の方向 [0~±180 または 0~360] ※ HueDir±HueRange が抽出対象になります。
			/// </summary>
			DataParam3,

			/// <summary>
			/// 抽出する色相の範囲 [0~180] ※ HueDir±HueRange が抽出対象になります。
			/// </summary>
			DataParam4,

			/// <summary>
			/// 赤成分の変換係数 [0~] [既定値:0.299]
			/// </summary>
			DataParam5,

			/// <summary>
			/// 緑成分の変換係数 [0~] [既定値:0.587]
			/// </summary>
			DataParam6,

			/// <summary>
			/// 青成分の変換係数 [0~] [既定値:0.114]
			/// </summary>
			DataParam7,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxImageEffectors_PartColor)
			{
				base.CopyFrom(src);

				var _src = (CxImageEffectors_PartColor)src;

				this.Depth = _src.Depth;
				this.HueDir = _src.HueDir;
				this.HueRange = _src.HueRange;
				this.RedRatio = _src.RedRatio;
				this.GreenRatio = _src.GreenRatio;
				this.BlueRatio = _src.BlueRatio;

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
				var _src = (CxImageEffectors_PartColor)src;

				if (this.Depth != _src.Depth) return false;
				if (this.HueDir != _src.HueDir) return false;
				if (this.HueRange != _src.HueRange) return false;
				if (this.RedRatio != _src.RedRatio) return false;
				if (this.GreenRatio != _src.GreenRatio) return false;
				if (this.BlueRatio != _src.BlueRatio) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 入力画像のビット深度 [0,1~64] ※ 0 は型の最大値、1~64 は (2 の Depth 乗 - 1) を最大とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

		/// <summary>
		/// 抽出する色相の方向 [0~±180 または 0~360] ※ HueDir±HueRange が抽出対象になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.HueDir")]
		public int HueDir
		{
			get { return m_HueDir; }
			set { m_HueDir = value; }
		}
		private int m_HueDir = 0;

		/// <summary>
		/// 抽出する色相の範囲 [0~180] ※ HueDir±HueRange が抽出対象になります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.HueRange")]
		public int HueRange
		{
			get { return m_HueRange; }
			set { m_HueRange = value; }
		}
		private int m_HueRange = 30;

		/// <summary>
		/// 赤成分の変換係数 [0~1] [既定値:0.299]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.RedRatio")]
		public double RedRatio
		{
			get { return m_RedRatio; }
			set { m_RedRatio = value; }
		}
		private double m_RedRatio = 0.299;

		/// <summary>
		/// 緑成分の変換係数 [0~1] [既定値:0.587]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.GreenRatio")]
		public double GreenRatio
		{
			get { return m_GreenRatio; }
			set { m_GreenRatio = value; }
		}
		private double m_GreenRatio = 0.587;

		/// <summary>
		/// 青成分の変換係数 [0~1] [既定値:0.114]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageEffectors_PartColor.BlueRatio")]
		public double BlueRatio
		{
			get { return m_BlueRatio; }
			set { m_BlueRatio = value; }
		}
		private double m_BlueRatio = 0.114;

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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			if (this.DataParam[2].CheckValidity())
				this.Depth = Convert.ToInt32(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.HueDir = Convert.ToInt32(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.HueRange = Convert.ToInt32(this.DataParam[4].Data);
			if (this.DataParam[5].CheckValidity())
				this.RedRatio = Convert.ToDouble(this.DataParam[5].Data);
			if (this.DataParam[6].CheckValidity())
				this.GreenRatio = Convert.ToDouble(this.DataParam[6].Data);
			if (this.DataParam[7].CheckValidity())
				this.BlueRatio = Convert.ToDouble(this.DataParam[7].Data);
			#endregion

			// 実行.
			{
				var effector = new XIE.Effectors.CxPartColor(this.Depth, this.HueDir, this.HueRange, this.RedRatio, this.GreenRatio, this.BlueRatio);
				effector.Execute(src, dst, mask);
			}

			// 出力.
			this.DataOut[0].Data = dst;
			return;
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
						target_port.Data = (CxImage)value;
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					// try
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(
							ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
							));

						// mask
						var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

						// parameters
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Depth));
						var hue_dir = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.HueDir));
						var hue_range = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.HueRange));
						var ratioR = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.RedRatio));
						var ratioG = ApiHelper.CodeOptionalExpression(e, this.DataParam[6], CodeLiteral.From(this.GreenRatio));
						var ratioB = ApiHelper.CodeOptionalExpression(e, this.DataParam[7], CodeLiteral.From(this.BlueRatio));

						// var effector = new XIE.Effectors.CxPartColor(depth, hue_dir, hue_range, ratioR, ratioG, ratioB);
						var effector = new CodeExtraVariable("effector", typeof(XIE.Effectors.CxPartColor));
						brace.TryStatements.Add(effector.Declare(effector.New(depth, hue_dir, hue_range, ratioR, ratioG, ratioB)));

						// effector.Execute(src, task#_Dst, mask);
						brace.TryStatements.Add(effector.Call("Execute", src, dst, mask));
					}
					// finally
					{
						brace.FinallyStatements.Add(new CodeSnippetStatement());
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
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion
}
