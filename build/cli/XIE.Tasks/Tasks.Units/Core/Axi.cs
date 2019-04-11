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
using System.Security.Permissions;
using System.Threading;
using System.Diagnostics;
using System.CodeDom;
using System.Linq;

using XIE.Ptr;

namespace XIE.Tasks
{
	// //////////////////////////////////////////////////
	// Axi (計算)
	// 

	#region Axi_CalcBpp

	/// <summary>
	/// 型のサイズ（bits）の計算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_CalcBpp : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_CalcBpp()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "CalcBpp";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Type", new Type[] { typeof(ExType), typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 要素の型
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された型のサイズ (bits) を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcBpp.Type")]
		public ExType Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}
		private ExType m_Type = ExType.None;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 指定された型のサイズ (bits) を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_CalcBpp.Result")]
		public int Result
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is int)
					return (int)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_CalcBpp)
			{
				base.CopyFrom(src);

				var _src = (Axi_CalcBpp)src;

				this.Type = _src.Type;

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
				var _src = (Axi_CalcBpp)src;

				if (this.Type != _src.Type) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Type = (ExType)Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.CalcBpp(this.Type);
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
					{
						var type = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Type));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.CalcBpp(type);
							scope.Add(result.Assign(
									axi.Call("CalcBpp", type)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_CalcDepth

	/// <summary>
	/// ビット深度の計算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_CalcDepth : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_CalcDepth()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "CalcDepth";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Type", new Type[] { typeof(ExType), typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 要素の型
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された型が表わすことができる最大のビット深度を計算します。対応しない型が指定された場合は 0 を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcDepth.Type")]
		public ExType Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}
		private ExType m_Type = ExType.None;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 指定された型が表わすことができる最大のビット深度を計算します。対応しない型が指定された場合は 0 を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_CalcDepth.Result")]
		public int Result
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is int)
					return (int)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_CalcDepth)
			{
				base.CopyFrom(src);

				var _src = (Axi_CalcDepth)src;

				this.Type = _src.Type;

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
				var _src = (Axi_CalcDepth)src;

				if (this.Type != _src.Type) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Type = (ExType)Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.CalcDepth(this.Type);
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
					{
						var type = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Type));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.CalcDepth(type);
							scope.Add(result.Assign(
									axi.Call("CalcDepth", type)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_CalcRange

	/// <summary>
	/// 型のレンジの計算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_CalcRange : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_CalcRange()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "CalcRange";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Type", new Type[] { typeof(ExType), typeof(int) }),
				new CxTaskPortIn("Depth", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] { typeof(TxRangeD) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 要素の型
			/// </summary>
			DataParam0,

			/// <summary>
			/// ビット深度 (bits) [0=最大値、1~=指定値]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 指定された型が表わすことができる値の範囲を計算します。Depth に指定された値が範囲外の場合は最大値に補正します。対応しない型が指定された場合は上下限共に 0 になります。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcRange.Type")]
		public ExType Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}
		private ExType m_Type = ExType.None;

		/// <summary>
		/// ビット深度 (bits) [0=最大値、1~=指定値]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcRange.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		private int m_Depth = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 指定された型が表わすことができる値の範囲を計算します。Depth に指定された値が範囲外の場合は最大値に補正します。対応しない型が指定された場合は上下限共に 0 になります。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_CalcRange.Result")]
		public TxRangeD Result
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is TxRangeD)
					return (TxRangeD)this.DataOut[index].Data;
				return new TxRangeD();
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_CalcRange)
			{
				base.CopyFrom(src);

				var _src = (Axi_CalcRange)src;

				this.Type = _src.Type;
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
				var _src = (Axi_CalcRange)src;

				if (this.Type != _src.Type) return false;
				if (this.Depth != _src.Depth) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Type = (ExType)Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Depth = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.CalcRange(this.Type, this.Depth);
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
					{
						var type = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Type));
						var depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Depth));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.CalcRange(type, depth);
							scope.Add(result.Assign(
									axi.Call("CalcRange", type, depth)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_CalcScale

	/// <summary>
	/// 濃度値のスケーリング値の計算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_CalcScale : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_CalcScale()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "CalcScale";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("SrcType", new Type[] { typeof(ExType), typeof(int) }),
				new CxTaskPortIn("SrcDepth", new Type[] { typeof(int) }),
				new CxTaskPortIn("DstType", new Type[] { typeof(ExType), typeof(int) }),
				new CxTaskPortIn("DstDepth", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力画像の要素の型
			/// </summary>
			DataParam0,

			/// <summary>
			/// 入力画像のビット深度 (bits) [0=最大値、1~=指定値]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 出力画像の要素の型
			/// </summary>
			DataParam2,

			/// <summary>
			/// 出力画像のビット深度 (bits) [0=最大値、1~=指定値]
			/// </summary>
			DataParam3,

			/// <summary>
			/// ビット深度が異なる画像間で濃度値をスケーリングする際の倍率を計算して返します。範囲外の値が指定された場合は最大のビット深度に補正します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 入力画像の要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcScale.SrcType")]
		public ExType SrcType
		{
			get { return m_SrcType; }
			set { m_SrcType = value; }
		}
		private ExType m_SrcType = ExType.None;

		/// <summary>
		/// 入力画像のビット深度 (bits) [0=最大値、1~=指定値]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcScale.SrcDepth")]
		public int SrcDepth
		{
			get { return m_SrcDepth; }
			set { m_SrcDepth = value; }
		}
		private int m_SrcDepth = 0;

		/// <summary>
		/// 出力画像の要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcScale.DstType")]
		public ExType DstType
		{
			get { return m_DstType; }
			set { m_DstType = value; }
		}
		private ExType m_DstType = ExType.None;

		/// <summary>
		/// 出力画像のビット深度 (bits) [0=最大値、1~=指定値]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcScale.DstDepth")]
		public int DstDepth
		{
			get { return m_DstDepth; }
			set { m_DstDepth = value; }
		}
		private int m_DstDepth = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// ビット深度が異なる画像間で濃度値をスケーリングする際の倍率を計算して返します。範囲外の値が指定された場合は最大のビット深度に補正します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_CalcScale.Result")]
		public double Result
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is double)
					return (double)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_CalcScale)
			{
				base.CopyFrom(src);

				var _src = (Axi_CalcScale)src;

				this.SrcType = _src.SrcType;
				this.SrcDepth = _src.SrcDepth;
				this.DstType = _src.DstType;
				this.DstDepth = _src.DstDepth;

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
				var _src = (Axi_CalcScale)src;

				if (this.SrcType != _src.SrcType) return false;
				if (this.SrcDepth != _src.SrcDepth) return false;
				if (this.DstType != _src.DstType) return false;
				if (this.DstDepth != _src.DstDepth) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.SrcType = (ExType)Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.SrcDepth = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.DstType = (ExType)Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.DstDepth = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.CalcScale(this.SrcType, this.SrcDepth, this.DstType, this.DstDepth);
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
					{
						var src_type = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.SrcType));
						var src_depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.SrcDepth));
						var dst_type = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.DstType));
						var dst_depth = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.DstDepth));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.CalcScale(src_type, src_depth, dst_type, dst_depth);
							scope.Add(result.Assign(
									axi.Call("CalcScale", src_type, src_depth, dst_type, dst_depth)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_CalcStride

	/// <summary>
	/// ２次元領域の水平方向サイズ (pixels) の計算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_CalcStride : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_CalcStride()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "CalcStride";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Model", new Type[] { typeof(TxModel) }),
				new CxTaskPortIn("Width", new Type[] { typeof(int) }),
				new CxTaskPortIn("PackingSize", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 要素モデル
			/// </summary>
			DataParam0,

			/// <summary>
			/// 幅 (pixels)
			/// </summary>
			DataParam1,

			/// <summary>
			/// パッキングサイズ (bytes) [1,2,4,8,16]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 計算した２次元領域の水平方向サイズ (pixels) を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素モデル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcStride.Model")]
		public TxModel Model
		{
			get { return m_Model; }
			set { m_Model = value; }
		}
		private TxModel m_Model = new TxModel();

		/// <summary>
		/// 幅 (pixels)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcStride.Width")]
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}
		private int m_Width = 0;

		/// <summary>
		/// パッキングサイズ (bytes) [1,2,4,8,16]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_CalcStride.PackingSize")]
		[TypeConverter(typeof(PackingSizeConverter))]
		public int PackingSize
		{
			get { return m_PackingSize; }
			set { m_PackingSize = value; }
		}
		private int m_PackingSize = 4;

		#region PackingSizeConverter

		class PackingSizeConverter : Int32Converter
		{
			public PackingSizeConverter()
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
					var items = new int[] { 1, 2, 4, 8, 16 };
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
		/// 計算した２次元領域の水平方向サイズ (pixels) を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_CalcStride.Result")]
		public int Result
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is int)
					return (int)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_CalcStride)
			{
				base.CopyFrom(src);

				var _src = (Axi_CalcStride)src;

				this.Model = _src.Model;
				this.Width = _src.Width;
				this.PackingSize = _src.PackingSize;

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
				var _src = (Axi_CalcStride)src;

				if (this.Model != _src.Model) return false;
				if (this.Width != _src.Width) return false;
				if (this.PackingSize != _src.PackingSize) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Model = (TxModel)this.DataParam[iii].Data; break;
						case 1: this.Width = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.PackingSize = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.CalcStride(this.Model, this.Width, this.PackingSize);
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
					{
						var model = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Model));
						var width = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Width));
						var packing_size = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.PackingSize));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.CalcStride(model, width, packing_size);
							scope.Add(result.Assign(
									axi.Call("CalcStride", model, width, packing_size)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_DegToRad

	/// <summary>
	/// Degree から Radian に変換します。 
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_DegToRad : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_DegToRad()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "DegToRad";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Degree", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Radian", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// Degree 単位の角度
			/// </summary>
			DataParam0,

			/// <summary>
			/// Radian 単位に変換した結果を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// Degree 単位の角度
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_DegToRad.Degree")]
		public double Degree
		{
			get { return m_Degree; }
			set { m_Degree = value; }
		}
		private double m_Degree = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// Radian 単位に変換した結果を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_DegToRad.Radian")]
		public double Radian
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is double)
					return (double)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_DegToRad)
			{
				base.CopyFrom(src);

				var _src = (Axi_DegToRad)src;

				this.Degree = _src.Degree;

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
				var _src = (Axi_DegToRad)src;

				if (this.Degree != _src.Degree) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Degree = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.DegToRad(this.Degree);
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
					{
						var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Degree));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.DegToRad(value);
							scope.Add(result.Assign(
									axi.Call("DegToRad", value)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Axi_RadToDeg

	/// <summary>
	/// Radian から Degree に変換します。 
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Axi_RadToDeg : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Axi_RadToDeg()
			: base()
		{
			this.Category = "XIE.Axi";
			this.Name = "RadToDeg";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Radian", new Type[] { typeof(double) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Degree", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// Radian 単位の角度
			/// </summary>
			DataParam0,

			/// <summary>
			/// Degree 単位に変換した結果を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// Radian 単位の角度
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Axi_RadToDeg.Radian")]
		public double Radian
		{
			get { return m_Radian; }
			set { m_Radian = value; }
		}
		private double m_Radian = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// Degree 単位に変換した結果を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Axi_RadToDeg.Degree")]
		public double Degree
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is double)
					return (double)this.DataOut[index].Data;
				return 0;
			}
		}

		#endregion

		#region XIE.IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Axi_RadToDeg)
			{
				base.CopyFrom(src);

				var _src = (Axi_RadToDeg)src;

				this.Radian = _src.Radian;

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
				var _src = (Axi_RadToDeg)src;

				if (this.Radian != _src.Radian) return false;
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

			foreach (var item in this.DataIn)
				item.CheckValidity(true);

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Radian = Convert.ToDouble(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = XIE.Axi.RadToDeg(this.Radian);
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
					{
						var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Radian));

						var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

						// -----
						{
							var axi = new CodeExtraType(typeof(XIE.Axi));

							// result = XIE.Axi.RadToDeg(value);
							scope.Add(result.Assign(
									axi.Call("RadToDeg", value)
								));
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
