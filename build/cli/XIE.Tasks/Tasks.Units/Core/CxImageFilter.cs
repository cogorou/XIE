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
	// //////////////////////////////////////////////////
	// Basic
	// 

	#region RgbToBgr

	/// <summary>
	/// RGB カラー画像の R,B を入れ替えます。入力が RGB 配列の場合は BGR 配列に変換します。逆に入力が BGR 配列の場合は RGB 配列に変換します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_RgbToBgr : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_RgbToBgr()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "RgbToBgr";
			this.IconKey = "Image-RGB";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
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
			if (src is CxImageFilter_RgbToBgr)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_RgbToBgr)src;

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
				var _src = (CxImageFilter_RgbToBgr)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_RgbToBgr.Scale")]
		[DefaultValue(0)]
		public double Scale
		{
			get { return m_Scale; }
			set { m_Scale = value; }
		}
		private double m_Scale = 0;

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
				this.Scale = Convert.ToInt32(this.DataParam[2].Data);
			#endregion

			// 実行.
			dst.Filter(mask).RgbToBgr(src, this.Scale);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);
					var scale = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Scale));

					// task#_Dst.Filter(mask).RgbToBgr(src, scale);
					scope.Add(dst.Call("Filter", mask).Call("RgbToBgr", src, scale));
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

	#region Compare

	/// <summary>
	/// ２つの画像の各画素の比較（差分が許容範囲内にあるか否か）を行い、出力画像の各画素に結果 [1:範囲内、0:範囲外] を書き込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Compare : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Compare()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Compare";
			this.IconKey = "Image-Bin";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("ErrorRange", new Type[] {typeof(double)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力画像2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:U8] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Compare)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Compare)src;

				this.ErrorRange = _src.ErrorRange;

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
				var _src = (CxImageFilter_Compare)src;

				if (this.ErrorRange != _src.ErrorRange) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 許容範囲
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Compare.ErrorRange")]
		[DefaultValue(0.0)]
		public double ErrorRange
		{
			get { return m_ErrorRange; }
			set { m_ErrorRange = value; }
		}
		private double m_ErrorRange = 0;

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
			var src1 = (CxImage)this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			var src2 = (CxImage)this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;

			if (this.DataParam[2].CheckValidity())
				this.ErrorRange = Convert.ToDouble(this.DataParam[2].Data);
			#endregion

			// 実行.
			dst.Filter(mask).Compare(src1, src2, this.ErrorRange);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// errorRange
					var errorRange = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.ErrorRange));

					// task#_Dst.Filter(mask).Compare(src1, src2, errorRange);
					scope.Add(dst.Call("Filter", mask).Call("Compare", src1, src2, errorRange));
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

	// //////////////////////////////////////////////////
	// GeoTrans
	// 

	#region Mirror

	/// <summary>
	/// Mirror メソッド。画像のミラー反転を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Mirror : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Mirror()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Mirror";
			this.IconKey = "Parser-GeoTransMirror1";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mode", new Type[] {typeof(int)}),
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
			/// マスク画像。入力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// モード [0:反転なし、1:X方向反転、2:Y方向反転、3:XY方向反転]
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
			if (src is CxImageFilter_Mirror)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Mirror)src;

				this.Mode = _src.Mode;

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
				var _src = (CxImageFilter_Mirror)src;

				if (this.Mode != _src.Mode) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// モード [0:反転なし、1:X方向反転、2:Y方向反転、3:XY方向反転]
		/// </summary>
		[TypeConverter(typeof(ModeConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Mirror.Mode")]
		public int Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}
		private int m_Mode = 0;

		#region ModeConverter

		class ModeConverter : Int32Converter
		{
			public ModeConverter()
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
				return new StandardValuesCollection(new int[] { 0, 1, 2, 3 });
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
				this.Mode = Convert.ToInt32(this.DataParam[2].Data);
			#endregion

			// 実行.
			dst.Filter(mask).Mirror(src, this.Mode);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// mode
					var mode = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Mode));

					// task#_Dst.Filter().Mirror(src, mode);
					scope.Add(dst.Call("Filter", mask).Call("Mirror", src, mode));
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

	#region Rotate

	/// <summary>
	/// Rotate メソッド。画像の回転を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Rotate : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Rotate()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Rotate";
			this.IconKey = "Parser-GeoTransRotate90";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mode", new Type[] {typeof(int)}),
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
			/// 出力画像。[Type:入力と同一] 入力画像のサイズ（※注）と一致している必要があります。（※注：モードによって幅と高さの対応が入れ替わりますのでご注意ください。）次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。入力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// モード [0:0度、-1:-90度、+1:+90度、+2:180度]
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
			if (src is CxImageFilter_Rotate)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Rotate)src;

				this.Mode = _src.Mode;

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
				var _src = (CxImageFilter_Rotate)src;

				if (this.Mode != _src.Mode) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// モード [0:0度、-1:-90度、+1:+90度、+2:180度]
		/// </summary>
		[TypeConverter(typeof(ModeConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Rotate.Mode")]
		public int Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}
		private int m_Mode = 0;

		#region ModeConverter

		class ModeConverter : Int32Converter
		{
			public ModeConverter()
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
				return new StandardValuesCollection(new int[] { 0, -1, +1, +2 });
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
				this.Mode = Convert.ToInt32(this.DataParam[2].Data);
			#endregion

			// 実行.
			dst.Filter(mask).Rotate(src, this.Mode);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// mode
					var mode = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Mode));

					// task#_Dst.Filter().Rotate(src, mode);
					scope.Add(dst.Call("Filter", mask).Call("Rotate", src, mode));
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

	#region Transpose

	/// <summary>
	/// Transpose メソッド。画像の転置を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Transpose : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Transpose()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Transpose";
			this.IconKey = "Parser-GeoTransTranspose";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
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
			/// 出力画像。[Type:入力と同一] 入力の幅と出力の高さ、入力の高さと出力の幅が一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。入力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Transpose)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Transpose)src;

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
				var _src = (CxImageFilter_Transpose)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var src = (CxImage)this.DataIn[0].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			// 実行.
			dst.Filter(mask).Transpose(src);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Transpose(src);
					scope.Add(dst.Call("Filter", mask).Call("Transpose", src));
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

	#region Scale

	/// <summary>
	/// Scale メソッド。画像のサイズ変更（拡大・縮小）を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Scale : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Scale()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Scale";
			this.IconKey = "Parser-GeoTransScale2";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("SX", new Type[] {typeof(double)}),
				new CxTaskPortIn("SY", new Type[] {typeof(double)}),
				new CxTaskPortIn("Interpolation", new Type[] {typeof(int)}),
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
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと異なっていても構いません。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。入力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

			/// <summary>
			/// X方向の倍率 (1.0 が等倍です。)
			/// </summary>
			DataParam2,

			/// <summary>
			/// Y方向の倍率 (1.0 が等倍です。)
			/// </summary>
			DataParam3,

			/// <summary>
			/// 濃度補間モード [0:最近傍、1:双方向、2:平均値] ※一般的に 1 は拡大時に、2 は縮小時に使用します。0 は補間を行いません。
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
			if (src is CxImageFilter_Scale)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Scale)src;

				this.SX = _src.SX;
				this.SY = _src.SY;
				this.Interpolation = _src.Interpolation;

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
				var _src = (CxImageFilter_Scale)src;

				if (this.SX != _src.SX) return false;
				if (this.SY != _src.SY) return false;
				if (this.Interpolation != _src.Interpolation) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// X方向の倍率 (1.0 が等倍です。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Scale.SX")]
		public double SX
		{
			get { return m_SX; }
			set { m_SX = value; }
		}
		private double m_SX = 1;

		/// <summary>
		/// Y方向の倍率 (1.0 が等倍です。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Scale.SY")]
		public double SY
		{
			get { return m_SY; }
			set { m_SY = value; }
		}
		private double m_SY = 1;

		/// <summary>
		/// 濃度補間モード [0:最近傍、1:双方向、2:平均値] ※一般的に 1 は拡大時に、2 は縮小時に使用します。0 は補間を行いません。
		/// </summary>
		[TypeConverter(typeof(InterpolationConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Scale.Interpolation")]
		public int Interpolation
		{
			get { return m_Interpolation; }
			set { m_Interpolation = value; }
		}
		private int m_Interpolation = 0;

		#region InterpolationConverter

		class InterpolationConverter : Int32Converter
		{
			public InterpolationConverter()
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
				return new StandardValuesCollection(new int[] { 0, 1, 2 });
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
				this.SX = Convert.ToDouble(this.DataParam[2].Data);
			if (this.DataParam[3].CheckValidity())
				this.SY = Convert.ToDouble(this.DataParam[3].Data);
			if (this.DataParam[4].CheckValidity())
				this.Interpolation = Convert.ToInt32(this.DataParam[4].Data);
			#endregion

			// 実行.
			dst.Filter(mask).Scale(src, this.SX, this.SY, this.Interpolation);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// sx
					var sx = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.SX));

					// sy
					var sy = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.SY));

					// interpolation
					var interpolation = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Interpolation));

					// task#_Dst.Filter().Scale(src, sx, sy, interpolation);
					scope.Add(dst.Call("Filter", mask).Call("Scale", src, sx, sy, interpolation));
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

	// //////////////////////////////////////////////////
	// Mathematic Function
	// 

	#region Math

	/// <summary>
	/// 数理計算。入力画像の各画素値を数理計算関数で計算した結果を出力画像の各画素に書き込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Math : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Math()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Math";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
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
			if (src is CxImageFilter_Math)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Math)src;

				this.Mode = _src.Mode;

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
				var _src = (CxImageFilter_Math)src;

				if (this.Mode != _src.Mode) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 数理関数種別
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxImageFilter_Math.Mode")]
		public XIE.ExMath Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}
		private XIE.ExMath m_Mode = XIE.ExMath.Abs;

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
			#endregion

			// 実行.
			dst.Filter(mask).Math(src, this.Mode);

			// 出力.
			this.DataOut[0].Data = dst;
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
					// src
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Abs(src);
					scope.Add(dst.Call("Filter", mask).Call(this.Mode.ToString(), src));
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

	// //////////////////////////////////////////////////
	// Arithmetic Operation
	// 

	#region Add

	/// <summary>
	/// Add メソッド。２つの画像の画素間または画素値と定数の加算を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Add : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Add()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Add";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Add)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Add)src;

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
				var _src = (CxImageFilter_Add)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Add(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Add(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				CxImage src = (CxImage)data_in1;
				dst.Filter(mask).Add(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Add(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Add", src1, src2));
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

	#region Sub

	/// <summary>
	/// Sub メソッド。２つの画像の画素間または画素値と定数の減算を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Sub : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Sub()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Sub";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage), typeof(double)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Sub)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Sub)src;

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
				var _src = (CxImageFilter_Sub)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Sub(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Sub(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Sub(value, src);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Sub(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Sub", src1, src2));
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

	#region Mul

	/// <summary>
	/// Mul メソッド。２つの画像の画素間または画素値と定数の乗算を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Mul : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Mul()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Mul";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Mul)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Mul)src;

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
				var _src = (CxImageFilter_Mul)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Mul(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Mul(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Mul(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Mul(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Mul", src1, src2));
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

	#region Div

	/// <summary>
	/// Div メソッド。２つの画像の画素間または画素値と定数の除算を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Div : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Div()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Div";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage), typeof(double)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Div)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Div)src;

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
				var _src = (CxImageFilter_Div)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Div(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Div(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Div(value, src);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Div(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Div", src1, src2));
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

	#region Mod

	/// <summary>
	/// Mod メソッド。２つの画像の画素間または画素値と定数の剰余演算を行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Mod : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Mod()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Mod";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage), typeof(double)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Mod)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Mod)src;

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
				var _src = (CxImageFilter_Mod)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Mod(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Mod(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Mod(value, src);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Mod(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Mod", src1, src2));
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

	#region Min

	/// <summary>
	/// Min メソッド。２つの画像の画素間または画素値と定数の比較を行い小さい方を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Min : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Min()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Min";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Min)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Min)src;

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
				var _src = (CxImageFilter_Min)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Min(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Min(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				CxImage src = (CxImage)data_in1;
				dst.Filter(mask).Min(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Min(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Min", src1, src2));
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

	#region Max

	/// <summary>
	/// Max メソッド。２つの画像の画素間または画素値と定数の比較を行い大きい方を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Max : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Max()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Max";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Max)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Max)src;

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
				var _src = (CxImageFilter_Max)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Max(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Max(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				CxImage src = (CxImage)data_in1;
				dst.Filter(mask).Max(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Max(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Max", src1, src2));
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

	#region Diff

	/// <summary>
	/// Diff メソッド。２つの画像の画素間または画素値と定数の差分（差の絶対値）を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Diff : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Diff()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Diff";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Diff)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Diff)src;

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
				var _src = (CxImageFilter_Diff)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Diff(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Diff(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				CxImage src = (CxImage)data_in1;
				dst.Filter(mask).Diff(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Diff(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Diff", src1, src2));
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

	#region Pow

	/// <summary>
	/// Pow メソッド。２つの画像の画素間または画素値と定数の累乗を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Pow : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Pow()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Pow";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage), typeof(double)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Pow)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Pow)src;

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
				var _src = (CxImageFilter_Pow)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Pow(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Pow(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Pow(value, src);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Pow(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Pow", src1, src2));
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

	#region Atan2

	/// <summary>
	/// Atan2 メソッド。２つの画像の画素間または画素値と定数の逆正接を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Atan2 : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Atan2()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Atan2";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage), typeof(double)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(double)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Atan2)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Atan2)src;

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
				var _src = (CxImageFilter_Atan2)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Atan2(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToDouble(data_in1);
				dst.Filter(mask).Atan2(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToDouble(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Atan2(value, src);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Atan2(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Atan2", src1, src2));
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

	// //////////////////////////////////////////////////
	// Logical Operation
	// 

	#region And

	/// <summary>
	/// And メソッド。２つの画像の画素間または画素値と定数の論理積を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_And : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_And()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "And";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(uint)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_And)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_And)src;

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
				var _src = (CxImageFilter_And)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).And(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToUInt32(data_in1);
				dst.Filter(mask).And(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToUInt32(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).And(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).And(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("And", src1, src2));
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

	#region Nand

	/// <summary>
	/// Nand メソッド。２つの画像の画素間または画素値と定数の否定論理積を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Nand : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Nand()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Nand";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(uint)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Nand)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Nand)src;

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
				var _src = (CxImageFilter_Nand)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Nand(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToUInt32(data_in1);
				dst.Filter(mask).Nand(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToUInt32(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Nand(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Nand(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Nand", src1, src2));
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

	#region Or

	/// <summary>
	/// Or メソッド。２つの画像の画素間または画素値と定数の論理和を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Or : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Or()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Or";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(uint)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Or)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Or)src;

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
				var _src = (CxImageFilter_Or)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Or(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToUInt32(data_in1);
				dst.Filter(mask).Or(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToUInt32(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Or(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Or(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Or", src1, src2));
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

	#region Xor

	/// <summary>
	/// Xor メソッド。２つの画像の画素間または画素値と定数の排他的論理和を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageFilter_Xor : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageFilter_Xor()
			: base()
		{
			this.Category = "XIE.CxImageFilter";
			this.Name = "Xor";
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage), typeof(uint)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Mask", new Type[] {typeof(CxImage)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn0,

			/// <summary>
			/// 画像または定数。
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力画像。[Type:入力と同一] 入力画像のサイズと一致している必要があります。次元数(Channels×Model.Pack)が異なる場合は少ない方を適用します。型(Type)が許容値と異なる場合は作業用バッファを介しますので一時的にメモリを消費します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// マスク画像。出力画像のサイズと一致しており、チャネル数が 1 または出力画像と同一、要素モデルが TxModel.U8(1) である必要があります。
			/// </summary>
			DataParam1,

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
			if (src is CxImageFilter_Xor)
			{
				base.CopyFrom(src);

				var _src = (CxImageFilter_Xor)src;

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
				var _src = (CxImageFilter_Xor)src;
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

			#region 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object data_in0 = this.DataIn[0].Data;

			this.DataIn[1].CheckValidity(true);
			object data_in1 = this.DataIn[1].Data;

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			var mask = (this.DataParam[1].CheckValidity())
				? (CxImage)this.DataParam[1].Data
				: (CxImage)null;
			#endregion

			#region 実行.
			if (data_in0 is CxImage && data_in1 is CxImage)
			{
				var src = (CxImage)data_in0;
				var val = (CxImage)data_in1;
				dst.Filter(mask).Xor(src, val);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in0 is CxImage && data_in1.GetType().IsPrimitive)
			{
				var src = (CxImage)data_in0;
				var value = Convert.ToUInt32(data_in1);
				dst.Filter(mask).Xor(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			else if (data_in1 is CxImage && data_in0.GetType().IsPrimitive)
			{
				var value = Convert.ToUInt32(data_in0);
				var src = (CxImage)data_in1;
				dst.Filter(mask).Xor(src, value);
				// 出力.
				this.DataOut[0].Data = dst;
				return;
			}
			#endregion

			throw new System.NotSupportedException();
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
					// src1
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// src2
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// ref) task#_Dst = task$_Xxxx;
					// new) task#_Dst = new XIE.CxImage();
					scope.Add(dst.Assign(
						ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
						));

					// mask
					var mask = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.Null);

					// task#_Dst.Filter(mask).Xor(src1, src2);
					scope.Add(dst.Call("Filter", mask).Call("Xor", src1, src2));
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
