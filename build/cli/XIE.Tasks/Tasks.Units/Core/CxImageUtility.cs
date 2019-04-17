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
	#region Split

	/// <summary>
	/// カラー画像の各カラーコンポーネントを抽出して複数枚の画像に分割します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageUtility_Split : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageUtility_Split()
			: base()
		{
			this.Category = "XIE.CxImage.Utility";
			this.Name = "Split";
			this.IconKey = "Parser-ChannelUnpack";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst0", new Type[] {typeof(CxImage)}),
				new CxTaskPortOut("Dst1", new Type[] {typeof(CxImage)}),
				new CxTaskPortOut("Dst2", new Type[] {typeof(CxImage)}),
				new CxTaskPortOut("Dst3", new Type[] {typeof(CxImage)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力画像の 0 次元目（0 チャネルまたは 0 フィールド）を返します。
			/// </summary>
			DataOut0,

			/// <summary>
			/// 入力画像の 1 次元目を返します。無ければ未確保の画像オブジェクトを返します。
			/// </summary>
			DataOut1,

			/// <summary>
			/// 入力画像の 2 次元目を返します。無ければ未確保の画像オブジェクトを返します。
			/// </summary>
			DataOut2,

			/// <summary>
			/// 入力画像の 3 次元目を返します。無ければ未確保の画像オブジェクトを返します。
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
			if (src is CxImageUtility_Split)
			{
				base.CopyFrom(src);

				var _src = (CxImageUtility_Split)src;

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
				var _src = (CxImageUtility_Split)src;
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
			#endregion

			// 実行.
			if (src.IsValid)
			{
				var dsts = new XIE.CxImage[this.DataOut.Length];

				for (int i = 0; i < this.DataOut.Length; i++)
				{
					dsts[i] = new XIE.CxImage();
					this.DataOut[i].Data = dsts[i];
				}

				if (src.Model.Pack == 1)
				{
					int channels = System.Math.Min(this.DataOut.Length, src.Channels);
					for (int ch = 0; ch < channels; ch++)
					{
						using (var child = src.Child(ch))
						{
							dsts[ch].CopyFrom(child);
						}
					}
				}
				else
				{
					int channels = System.Math.Min(this.DataOut.Length, src.Model.Pack);
					for (int ch = 0; ch < channels; ch++)
					{
						dsts[ch].Filter().CopyEx(src, ch, 1);
					}
				}
			}
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
				case 1:
				case 2:
				case 3:
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
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// task#_Dst
						var dst0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
						var dst1 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[1]));
						var dst2 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[2]));
						var dst3 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[3]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst0.Assign(CodeLiteral.From<XIE.CxImage>()));
						brace.TryStatements.Add(dst1.Assign(CodeLiteral.From<XIE.CxImage>()));
						brace.TryStatements.Add(dst2.Assign(CodeLiteral.From<XIE.CxImage>()));
						brace.TryStatements.Add(dst3.Assign(CodeLiteral.From<XIE.CxImage>()));

						// var dsts = new XIE.CxImage[4];
						var dsts = new CodeExtraVariable("dsts", typeof(XIE.CxImage[]));
						brace.TryStatements.Add(dsts.Declare(new CodeArrayCreateExpression("XIE.CxImage", 4)));

						// dsts[0] = dst0;
						brace.TryStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dsts, CodeLiteral.From(0)), dst0));
						brace.TryStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dsts, CodeLiteral.From(1)), dst1));
						brace.TryStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dsts, CodeLiteral.From(2)), dst2));
						brace.TryStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dsts, CodeLiteral.From(3)), dst3));

						#region if (src.Model.Pack == 1)
						{
							var if1 = new CodeConditionStatement();
							brace.TryStatements.Add(if1);

							// if (src.Model.Pack == 1)
							if1.Condition = src.Ref("Model").Ref("Pack").Equal(CodeLiteral.From(1));
							{
								// int channels = System.Math.Min(this.DataOut.Length, src.Channels);
								var channels = new CodeExtraVariable("channels", typeof(int));
								if1.TrueStatements.Add(
									channels.Declare(
										new CodeExtraType(typeof(System.Math)).Call("Min", CodeLiteral.From(4), src.Ref("Channels"))
								));

								// int ch;
								var ch = new CodeExtraVariable("ch", typeof(int));
								if1.TrueStatements.Add(ch.Declare());

								// for(int ch=0 ; ch<channels ; ch++)
								var for1 = new CodeIterationStatement();
								if1.TrueStatements.Add(for1);

								for1.InitStatement = ch.Assign(CodeLiteral.From(0));
								for1.TestExpression = ch.LessThan(channels);
								for1.IncrementStatement = ch.Assign(ch.Add(CodeLiteral.From(1)));

								{
									var copyfrom = new CodeMethodInvokeExpression(new CodeArrayIndexerExpression(dsts, ch), "CopyFrom", src.Call("Child", ch));
									for1.Statements.Add(copyfrom);
								}
							}
							// else
							{
								// int channels = System.Math.Min(this.DataOut.Length, src.Model.Pack);
								var channels = new CodeExtraVariable("channels", typeof(int));
								if1.FalseStatements.Add(
									channels.Declare(
										new CodeExtraType(typeof(System.Math)).Call("Min", CodeLiteral.From(4), src.Ref("Model").Ref("Pack"))
								));

								// int ch;
								var ch = new CodeExtraVariable("ch", typeof(int));
								if1.FalseStatements.Add(ch.Declare());

								// for(int ch=0 ; ch<channels ; ch++)
								var for1 = new CodeIterationStatement();
								if1.FalseStatements.Add(for1);

								for1.InitStatement = ch.Assign(CodeLiteral.From(0));
								for1.TestExpression = ch.LessThan(channels);
								for1.IncrementStatement = ch.Assign(ch.Add(CodeLiteral.From(1)));

								{
									var filter = new CodeMethodInvokeExpression(new CodeArrayIndexerExpression(dsts, ch), "Filter");
									var copyex = new CodeMethodInvokeExpression(filter, "CopyEx", src, ch, CodeLiteral.From(1));
									for1.Statements.Add(copyex);
								}
							}
						}
						#endregion
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

	#region Combine

	/// <summary>
	/// 複数枚の濃淡画像をチャネル結合して１つの画像オブジェクトを生成します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxImageUtility_Combine : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageUtility_Combine()
			: base()
		{
			this.Category = "XIE.CxImage.Utility";
			this.Name = "Combine";
			this.IconKey = "Parser-ChannelUnpack";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src0", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src1", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src2", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("Src3", new Type[] {typeof(CxImage)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像 0 (未接続または未確保の場合は無視します。)
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力画像 1 (未接続または未確保の場合は無視します。)
			/// </summary>
			DataIn1,

			/// <summary>
			/// 入力画像 2 (未接続または未確保の場合は無視します。)
			/// </summary>
			DataIn2,

			/// <summary>
			/// 入力画像 3 (未接続または未確保の場合は無視します。)
			/// </summary>
			DataIn3,

			/// <summary>
			/// 出力画像。有効な入力画像をチャネル結合した画像を生成して返します。
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
			if (src is CxImageUtility_Combine)
			{
				base.CopyFrom(src);

				var _src = (CxImageUtility_Combine)src;

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
				var _src = (CxImageUtility_Combine)src;
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

			var srcs = new List<CxImage>();

			#region 引数の取得:
			for (int i = 0; i < this.DataIn.Length; i++)
			{
				if (this.DataIn[i].CheckValidity())
				{
					var src = (CxImage)this.DataIn[i].Data;
					srcs.Add(src);
				}
			}
			#endregion

			// 実行.
			if (srcs.Count > 0)
			{
				var dst = new CxImage();

				dst.Resize(srcs[0].Width, srcs[0].Height, srcs[0].Model, srcs.Count);
				for (int ch = 0; ch < dst.Channels; ch++)
				{
					using (var dst_child = dst.Child(ch))
					{
						if (srcs[ch].IsValid)
						{
							dst_child.Filter().Copy(srcs[ch]);
						}
					}
				}

				this.DataOut[0].Data = dst;
			}
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

				var connected = Array.Exists(this.DataIn, (item) => { return (item.IsConnected); });
				if (connected)
				{
					var brace = new CodeTryCatchFinallyStatement();
					scope.Add(brace);
					{
						// src
						var srcs = new List<CodeExtraVariable>();
						for(int i=0 ; i<this.DataIn.Length ; i++)
						{
							if (this.DataIn[i].CheckValidity())
							{
								srcs.Add(new CodeExtraVariable(e.GetVariable(this.DataIn[i])));
							}
						}

						// task#_Dst
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// ref) task#_Dst = task$_Xxxx;
						// new) task#_Dst = new XIE.CxImage();
						brace.TryStatements.Add(dst.Assign(CodeLiteral.From<XIE.CxImage>()));

						if (srcs.Count > 0)
						{
							brace.TryStatements.Add(
								dst.Call("Resize", srcs[0].Ref("Width"), srcs[0].Ref("Height"), srcs[0].Ref("Model"), CodeLiteral.From(srcs.Count))
								);

							for (int i = 0; i < srcs.Count; i++)
							{
								brace.TryStatements.Add(
									dst.Call("Child", CodeLiteral.From(i)).Call("Filter").Call("Copy", srcs[i])
									);
							}
						}
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
