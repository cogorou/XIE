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
using System.Security.Permissions;
using System.Threading;
using System.Diagnostics;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;

namespace XIE.Tasks
{
	// //////////////////////////////////////////////////
	// Math
	// 

	#region System.Math.Abs

	/// <summary>
	/// 指定された値の絶対値を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Abs : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Abs()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Abs";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Abs)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Abs)src;

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
				var _src = (System_Math_Abs)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Abs.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Abs.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Abs.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Abs((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Abs((double)value);
			}
			if (value is float)
			{
				return System.Math.Abs((float)value);
			}
			if (value is long)
			{
				return System.Math.Abs((long)value);
			}
			if (value is int)
			{
				return System.Math.Abs((int)value);
			}
			if (value is short)
			{
				return System.Math.Abs((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Abs((sbyte)value);
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				return System.Math.Abs((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Abs((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Abs((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Abs(src);
					scope.Add(dst.Assign(api.Call("Abs", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Sign

	/// <summary>
	/// 指定された値の符号を示す整数を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Sign : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Sign()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Sign";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Sign)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Sign)src;

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
				var _src = (System_Math_Sign)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sign.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sign.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sign.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Sign((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Sign((double)value);
			}
			if (value is float)
			{
				return System.Math.Sign((float)value);
			}
			if (value is long)
			{
				return System.Math.Sign((long)value);
			}
			if (value is int)
			{
				return System.Math.Sign((int)value);
			}
			if (value is short)
			{
				return System.Math.Sign((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Sign((sbyte)value);
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				return System.Math.Sign((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Sign((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Sign((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Sign(src);
					scope.Add(dst.Assign(api.Call("Sign", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Truncate

	/// <summary>
	/// 指定された実数の整数部を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Truncate : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Truncate()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Truncate";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Truncate)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Truncate)src;

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
				var _src = (System_Math_Truncate)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Truncate.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Truncate.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Truncate.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Truncate((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Truncate((double)value);
			}
			if (value is float)
			{
				return System.Math.Truncate((float)value);
			}
			if (value is long)
			{
				throw new System.NotSupportedException();
			}
			if (value is int)
			{
				throw new System.NotSupportedException();
			}
			if (value is short)
			{
				throw new System.NotSupportedException();
			}
			if (value is sbyte)
			{
				throw new System.NotSupportedException();
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				throw new System.NotSupportedException();
			}
			if (value is ushort)
			{
				throw new System.NotSupportedException();
			}
			if (value is byte)
			{
				throw new System.NotSupportedException();
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Truncate(src);
					scope.Add(dst.Assign(api.Call("Truncate", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Ceiling

	/// <summary>
	/// 指定された実数の小数部を切り捨てて返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Ceiling : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Ceiling()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Ceiling";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Ceiling)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Ceiling)src;

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
				var _src = (System_Math_Ceiling)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Ceiling.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Ceiling.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Ceiling.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Ceiling((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Ceiling((double)value);
			}
			if (value is float)
			{
				return System.Math.Ceiling((float)value);
			}
			if (value is long)
			{
				throw new System.NotSupportedException();
			}
			if (value is int)
			{
				throw new System.NotSupportedException();
			}
			if (value is short)
			{
				throw new System.NotSupportedException();
			}
			if (value is sbyte)
			{
				throw new System.NotSupportedException();
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				throw new System.NotSupportedException();
			}
			if (value is ushort)
			{
				throw new System.NotSupportedException();
			}
			if (value is byte)
			{
				throw new System.NotSupportedException();
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Ceiling(src);
					scope.Add(dst.Assign(api.Call("Ceiling", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Floor

	/// <summary>
	/// 指定された実数の小数部を切り上げて返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Floor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Floor()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Floor";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Floor)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Floor)src;

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
				var _src = (System_Math_Floor)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Floor.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Floor.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Floor.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Floor((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Floor((double)value);
			}
			if (value is float)
			{
				return System.Math.Floor((float)value);
			}
			if (value is long)
			{
				throw new System.NotSupportedException();
			}
			if (value is int)
			{
				throw new System.NotSupportedException();
			}
			if (value is short)
			{
				throw new System.NotSupportedException();
			}
			if (value is sbyte)
			{
				throw new System.NotSupportedException();
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				throw new System.NotSupportedException();
			}
			if (value is ushort)
			{
				throw new System.NotSupportedException();
			}
			if (value is byte)
			{
				throw new System.NotSupportedException();
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Floor(src);
					scope.Add(dst.Assign(api.Call("Floor", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Round

	/// <summary>
	/// 指定された実数の小数部を四捨五入して返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Round : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Round()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Round";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Round)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Round)src;

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
				var _src = (System_Math_Round)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Round.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Round.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Round.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				return System.Math.Round((Decimal)value);
			}
			if (value is double)
			{
				return System.Math.Round((double)value);
			}
			if (value is float)
			{
				return System.Math.Round((float)value);
			}
			if (value is long)
			{
				throw new System.NotSupportedException();
			}
			if (value is int)
			{
				throw new System.NotSupportedException();
			}
			if (value is short)
			{
				throw new System.NotSupportedException();
			}
			if (value is sbyte)
			{
				throw new System.NotSupportedException();
			}
			if (value is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value is uint)
			{
				throw new System.NotSupportedException();
			}
			if (value is ushort)
			{
				throw new System.NotSupportedException();
			}
			if (value is byte)
			{
				throw new System.NotSupportedException();
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Round(src);
					scope.Add(dst.Assign(api.Call("Round", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Exp

	/// <summary>
	/// 指定した値で e を累乗した値を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Exp : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Exp()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Exp";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Exp)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Exp)src;

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
				var _src = (System_Math_Exp)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Exp.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Exp.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Exp.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Exp((double)value);
			}
			if (value is float)
			{
				return System.Math.Exp((float)value);
			}
			if (value is long)
			{
				return System.Math.Exp((long)value);
			}
			if (value is int)
			{
				return System.Math.Exp((int)value);
			}
			if (value is short)
			{
				return System.Math.Exp((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Exp((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Exp((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Exp((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Exp((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Exp((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Exp(src);
					scope.Add(dst.Assign(api.Call("Exp", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Log

	/// <summary>
	/// 指定した値の自然 (底 e) 対数を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Log : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Log()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Log";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Log)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Log)src;

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
				var _src = (System_Math_Log)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Log((double)value);
			}
			if (value is float)
			{
				return System.Math.Log((float)value);
			}
			if (value is long)
			{
				return System.Math.Log((long)value);
			}
			if (value is int)
			{
				return System.Math.Log((int)value);
			}
			if (value is short)
			{
				return System.Math.Log((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Log((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Log((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Log((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Log((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Log((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Log(src);
					scope.Add(dst.Assign(api.Call("Log", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Log10

	/// <summary>
	/// 指定した値の底 10 の対数を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Log10 : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Log10()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Log10";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Log10)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Log10)src;

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
				var _src = (System_Math_Log10)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log10.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log10.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Log10.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Log10((double)value);
			}
			if (value is float)
			{
				return System.Math.Log10((float)value);
			}
			if (value is long)
			{
				return System.Math.Log10((long)value);
			}
			if (value is int)
			{
				return System.Math.Log10((int)value);
			}
			if (value is short)
			{
				return System.Math.Log10((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Log10((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Log10((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Log10((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Log10((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Log10((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Log10(src);
					scope.Add(dst.Assign(api.Call("Log10", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Sqrt

	/// <summary>
	/// 指定した値の平方根を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Sqrt : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Sqrt()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Sqrt";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Sqrt)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Sqrt)src;

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
				var _src = (System_Math_Sqrt)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sqrt.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sqrt.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sqrt.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Sqrt((double)value);
			}
			if (value is float)
			{
				return System.Math.Sqrt((float)value);
			}
			if (value is long)
			{
				return System.Math.Sqrt((long)value);
			}
			if (value is int)
			{
				return System.Math.Sqrt((int)value);
			}
			if (value is short)
			{
				return System.Math.Sqrt((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Sqrt((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Sqrt((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Sqrt((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Sqrt((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Sqrt((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Sqrt(src);
					scope.Add(dst.Assign(api.Call("Sqrt", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Sin

	/// <summary>
	/// 指定した角度（radian）の正弦を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Sin : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Sin()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Sin";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Sin)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Sin)src;

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
				var _src = (System_Math_Sin)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sin.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sin.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sin.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Sin((double)value);
			}
			if (value is float)
			{
				return System.Math.Sin((float)value);
			}
			if (value is long)
			{
				return System.Math.Sin((long)value);
			}
			if (value is int)
			{
				return System.Math.Sin((int)value);
			}
			if (value is short)
			{
				return System.Math.Sin((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Sin((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Sin((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Sin((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Sin((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Sin((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Sin(src);
					scope.Add(dst.Assign(api.Call("Sin", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Cos

	/// <summary>
	/// 指定した角度（radian）の余弦を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Cos : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Cos()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Cos";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Cos)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Cos)src;

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
				var _src = (System_Math_Cos)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cos.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cos.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cos.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Cos((double)value);
			}
			if (value is float)
			{
				return System.Math.Cos((float)value);
			}
			if (value is long)
			{
				return System.Math.Cos((long)value);
			}
			if (value is int)
			{
				return System.Math.Cos((int)value);
			}
			if (value is short)
			{
				return System.Math.Cos((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Cos((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Cos((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Cos((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Cos((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Cos((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Cos(src);
					scope.Add(dst.Assign(api.Call("Cos", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Tan

	/// <summary>
	/// 指定した角度（radian）の正接を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Tan : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Tan()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Tan";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Tan)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Tan)src;

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
				var _src = (System_Math_Tan)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tan.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tan.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tan.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Tan((double)value);
			}
			if (value is float)
			{
				return System.Math.Tan((float)value);
			}
			if (value is long)
			{
				return System.Math.Tan((long)value);
			}
			if (value is int)
			{
				return System.Math.Tan((int)value);
			}
			if (value is short)
			{
				return System.Math.Tan((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Tan((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Tan((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Tan((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Tan((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Tan((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Tan(src);
					scope.Add(dst.Assign(api.Call("Tan", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Sinh

	/// <summary>
	/// 指定した角度（radian）の双曲線正弦（ハイパボイックサイン）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Sinh : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Sinh()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Sinh";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Sinh)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Sinh)src;

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
				var _src = (System_Math_Sinh)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sinh.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sinh.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Sinh.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Sinh((double)value);
			}
			if (value is float)
			{
				return System.Math.Sinh((float)value);
			}
			if (value is long)
			{
				return System.Math.Sinh((long)value);
			}
			if (value is int)
			{
				return System.Math.Sinh((int)value);
			}
			if (value is short)
			{
				return System.Math.Sinh((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Sinh((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Sinh((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Sinh((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Sinh((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Sinh((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Sinh(src);
					scope.Add(dst.Assign(api.Call("Sinh", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Cosh

	/// <summary>
	/// 指定した角度（radian）の双曲線余弦（ハイパボイックコサイン）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Cosh : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Cosh()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Cosh";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Cosh)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Cosh)src;

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
				var _src = (System_Math_Cosh)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cosh.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cosh.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Cosh.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Cosh((double)value);
			}
			if (value is float)
			{
				return System.Math.Cosh((float)value);
			}
			if (value is long)
			{
				return System.Math.Cosh((long)value);
			}
			if (value is int)
			{
				return System.Math.Cosh((int)value);
			}
			if (value is short)
			{
				return System.Math.Cosh((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Cosh((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Cosh((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Cosh((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Cosh((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Cosh((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Cosh(src);
					scope.Add(dst.Assign(api.Call("Cosh", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Tanh

	/// <summary>
	/// 指定した角度（radian）の双曲線正接（ハイパボイックタンジェント）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Tanh : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Tanh()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Tanh";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Tanh)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Tanh)src;

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
				var _src = (System_Math_Tanh)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tanh.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tanh.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Tanh.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Tanh((double)value);
			}
			if (value is float)
			{
				return System.Math.Tanh((float)value);
			}
			if (value is long)
			{
				return System.Math.Tanh((long)value);
			}
			if (value is int)
			{
				return System.Math.Tanh((int)value);
			}
			if (value is short)
			{
				return System.Math.Tanh((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Tanh((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Tanh((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Tanh((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Tanh((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Tanh((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Tanh(src);
					scope.Add(dst.Assign(api.Call("Tanh", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Asin

	/// <summary>
	/// 指定した値の逆正弦（アークサイン）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Asin : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Asin()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Asin";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Asin)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Asin)src;

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
				var _src = (System_Math_Asin)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Asin.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Asin.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Asin.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Asin((double)value);
			}
			if (value is float)
			{
				return System.Math.Asin((float)value);
			}
			if (value is long)
			{
				return System.Math.Asin((long)value);
			}
			if (value is int)
			{
				return System.Math.Asin((int)value);
			}
			if (value is short)
			{
				return System.Math.Asin((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Asin((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Asin((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Asin((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Asin((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Asin((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Asin(src);
					scope.Add(dst.Assign(api.Call("Asin", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Acos

	/// <summary>
	/// 指定した値の逆余弦（アークコサイン）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Acos : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Acos()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Acos";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Acos)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Acos)src;

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
				var _src = (System_Math_Acos)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Acos.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Acos.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Acos.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Acos((double)value);
			}
			if (value is float)
			{
				return System.Math.Acos((float)value);
			}
			if (value is long)
			{
				return System.Math.Acos((long)value);
			}
			if (value is int)
			{
				return System.Math.Acos((int)value);
			}
			if (value is short)
			{
				return System.Math.Acos((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Acos((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Acos((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Acos((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Acos((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Acos((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Acos(src);
					scope.Add(dst.Assign(api.Call("Acos", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Atan

	/// <summary>
	/// 指定した値の逆正接（アークタンジェント）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Atan : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Atan()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Atan";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Atan)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Atan)src;

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
				var _src = (System_Math_Atan)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			object value = this.DataIn[0].Data;

			this.DataOut[0].Data = Compute(value);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value">入力値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value)
		{
			if (value is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value is double)
			{
				return System.Math.Atan((double)value);
			}
			if (value is float)
			{
				return System.Math.Atan((float)value);
			}
			if (value is long)
			{
				return System.Math.Atan((long)value);
			}
			if (value is int)
			{
				return System.Math.Atan((int)value);
			}
			if (value is short)
			{
				return System.Math.Atan((short)value);
			}
			if (value is sbyte)
			{
				return System.Math.Atan((sbyte)value);
			}
			if (value is ulong)
			{
				return System.Math.Atan((ulong)value);
			}
			if (value is uint)
			{
				return System.Math.Atan((uint)value);
			}
			if (value is ushort)
			{
				return System.Math.Atan((ushort)value);
			}
			if (value is byte)
			{
				return System.Math.Atan((byte)value);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Atan(src);
					scope.Add(dst.Assign(api.Call("Atan", src)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Atan2

	/// <summary>
	/// tanθ=value1/value2 となる角度（θ）を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Atan2 : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Atan2()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Atan2";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Atan2)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Atan2)src;

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
				var _src = (System_Math_Atan2)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan2.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan2.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan2.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Atan2.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			this.DataOut[0].Data = Compute(value1, value2);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is double && value2 is double)
			{
				return System.Math.Atan2((double)value1, (double)value2);
			}
			if (value1 is float && value2 is float)
			{
				return System.Math.Atan2((float)value1, (float)value2);
			}
			if (value1 is long && value2 is long)
			{
				return System.Math.Atan2((long)value1, (long)value2);
			}
			if (value1 is int && value2 is int)
			{
				return System.Math.Atan2((int)value1, (int)value2);
			}
			if (value1 is short && value2 is short)
			{
				return System.Math.Atan2((short)value1, (short)value2);
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				return System.Math.Atan2((sbyte)value1, (sbyte)value2);
			}
			if (value1 is ulong && value2 is ulong)
			{
				return System.Math.Atan2((ulong)value1, (ulong)value2);
			}
			if (value1 is uint && value2 is uint)
			{
				return System.Math.Atan2((uint)value1, (uint)value2);
			}
			if (value1 is ushort && value2 is ushort)
			{
				return System.Math.Atan2((ushort)value1, (ushort)value2);
			}
			if (value1 is byte && value2 is byte)
			{
				return System.Math.Atan2((byte)value1, (byte)value2);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Atan2(src1, src2);
					scope.Add(dst.Assign(api.Call("Atan2", src1, src2)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Pow

	/// <summary>
	/// 値1 を 値2 で累乗した値を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Pow : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Pow()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Pow";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Pow)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Pow)src;

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
				var _src = (System_Math_Pow)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Pow.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Pow.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Pow.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Pow.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			this.DataOut[0].Data = Compute(value1, value2);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is double && value2 is double)
			{
				return System.Math.Pow((double)value1, (double)value2);
			}
			if (value1 is float && value2 is float)
			{
				return System.Math.Pow((float)value1, (float)value2);
			}
			if (value1 is long && value2 is long)
			{
				return System.Math.Pow((long)value1, (long)value2);
			}
			if (value1 is int && value2 is int)
			{
				return System.Math.Pow((int)value1, (int)value2);
			}
			if (value1 is short && value2 is short)
			{
				return System.Math.Pow((short)value1, (short)value2);
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				return System.Math.Pow((sbyte)value1, (sbyte)value2);
			}
			if (value1 is ulong && value2 is ulong)
			{
				return System.Math.Pow((ulong)value1, (ulong)value2);
			}
			if (value1 is uint && value2 is uint)
			{
				return System.Math.Pow((uint)value1, (uint)value2);
			}
			if (value1 is ushort && value2 is ushort)
			{
				return System.Math.Pow((ushort)value1, (ushort)value2);
			}
			if (value1 is byte && value2 is byte)
			{
				return System.Math.Pow((byte)value1, (byte)value2);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Pow(src1, src2);
					scope.Add(dst.Assign(api.Call("Pow", src1, src2)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Max

	/// <summary>
	/// ２つの値の内、大きい方を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Max : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Max()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Max";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Max)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Max)src;

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
				var _src = (System_Math_Max)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Max.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Max.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Max.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Max.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			this.DataOut[0].Data = Compute(value1, value2);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				return System.Math.Max((Decimal)value1, (Decimal)value2);
			}
			if (value1 is double && value2 is double)
			{
				return System.Math.Max((double)value1, (double)value2);
			}
			if (value1 is float && value2 is float)
			{
				return System.Math.Max((float)value1, (float)value2);
			}
			if (value1 is long && value2 is long)
			{
				return System.Math.Max((long)value1, (long)value2);
			}
			if (value1 is int && value2 is int)
			{
				return System.Math.Max((int)value1, (int)value2);
			}
			if (value1 is short && value2 is short)
			{
				return System.Math.Max((short)value1, (short)value2);
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				return System.Math.Max((sbyte)value1, (sbyte)value2);
			}
			if (value1 is ulong && value2 is ulong)
			{
				return System.Math.Max((ulong)value1, (ulong)value2);
			}
			if (value1 is uint && value2 is uint)
			{
				return System.Math.Max((uint)value1, (uint)value2);
			}
			if (value1 is ushort && value2 is ushort)
			{
				return System.Math.Max((ushort)value1, (ushort)value2);
			}
			if (value1 is byte && value2 is byte)
			{
				return System.Math.Max((byte)value1, (byte)value2);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Max(src1, src2);
					scope.Add(dst.Assign(api.Call("Max", src1, src2)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.Min

	/// <summary>
	/// ２つの値の内、小さい方を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_Min : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_Min()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "Min";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_Min)
			{
				base.CopyFrom(src);

				var _src = (System_Math_Min)src;

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
				var _src = (System_Math_Min)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Min.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Min.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Min.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_Min.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			this.DataOut[0].Data = Compute(value1, value2);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				return System.Math.Min((Decimal)value1, (Decimal)value2);
			}
			if (value1 is double && value2 is double)
			{
				return System.Math.Min((double)value1, (double)value2);
			}
			if (value1 is float && value2 is float)
			{
				return System.Math.Min((float)value1, (float)value2);
			}
			if (value1 is long && value2 is long)
			{
				return System.Math.Min((long)value1, (long)value2);
			}
			if (value1 is int && value2 is int)
			{
				return System.Math.Min((int)value1, (int)value2);
			}
			if (value1 is short && value2 is short)
			{
				return System.Math.Min((short)value1, (short)value2);
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				return System.Math.Min((sbyte)value1, (sbyte)value2);
			}
			if (value1 is ulong && value2 is ulong)
			{
				return System.Math.Min((ulong)value1, (ulong)value2);
			}
			if (value1 is uint && value2 is uint)
			{
				return System.Math.Min((uint)value1, (uint)value2);
			}
			if (value1 is ushort && value2 is ushort)
			{
				return System.Math.Min((ushort)value1, (ushort)value2);
			}
			if (value1 is byte && value2 is byte)
			{
				return System.Math.Min((byte)value1, (byte)value2);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.Min(src1, src2);
					scope.Add(dst.Assign(api.Call("Min", src1, src2)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.BigMul

	/// <summary>
	/// ２つの整数値の積を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_BigMul : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_BigMul()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "BigMul";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値
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
			if (src is System_Math_BigMul)
			{
				base.CopyFrom(src);

				var _src = (System_Math_BigMul)src;

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
				var _src = (System_Math_BigMul)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_BigMul.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_BigMul.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_BigMul.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_BigMul.Type")]
		public string Type
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			this.DataOut[0].Data = Compute(value1, value2);
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public static object Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is double && value2 is double)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is float && value2 is float)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is long && value2 is long)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is int && value2 is int)
			{
				return System.Math.BigMul((int)value1, (int)value2);
			}
			if (value1 is short && value2 is short)
			{
				return System.Math.BigMul((short)value1, (short)value2);
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				return System.Math.BigMul((sbyte)value1, (sbyte)value2);
			}
			if (value1 is ulong && value2 is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is uint && value2 is uint)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is ushort && value2 is ushort)
			{
				return System.Math.BigMul((ushort)value1, (ushort)value2);
			}
			if (value1 is byte && value2 is byte)
			{
				return System.Math.BigMul((byte)value1, (byte)value2);
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst = System.Math.BigMul(src1, src2);
					scope.Add(dst.Assign(api.Call("BigMul", src1, src2)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region System.Math.DivRem

	/// <summary>
	/// ２つの整数の商と剰余を返します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class System_Math_DivRem : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public System_Math_DivRem()
			: base()
		{
			this.Category = "System.Math";
			this.Name = "DivRem";
			this.IconKey = "Parser-Function";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Value1"),
				new CxTaskPortIn("Value2"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Quotient", new Type[] { typeof(object) }),
				new CxTaskPortOut("Remainder", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 入力値1
			/// </summary>
			DataIn0,

			/// <summary>
			/// 入力値2
			/// </summary>
			DataIn1,

			/// <summary>
			/// 出力値1 (商)
			/// </summary>
			DataOut0,

			/// <summary>
			/// 出力値2 (剰余)
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
			if (src is System_Math_DivRem)
			{
				base.CopyFrom(src);

				var _src = (System_Math_DivRem)src;

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
				var _src = (System_Math_DivRem)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 値1
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.Value1")]
		public string Value1
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				object data = this.DataIn[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// 値2
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.Value2")]
		public string Value2
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				object data = this.DataIn[1].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// ２つの値の商
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.Quotient")]
		public string Quotient
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// ２つの値の商の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.QuotientType")]
		public string QuotientType
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
		}

		/// <summary>
		/// ２つの値の剰余
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.Remainder")]
		public string Remainder
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.ToString();
			}
		}

		/// <summary>
		/// ２つの値の剰余の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.System_Math_DivRem.RemainderType")]
		public string RemainderType
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return data.GetType().Name.ToString();
			}
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			this.DataIn[1].CheckValidity(true);
			object value1 = this.DataIn[0].Data;
			object value2 = this.DataIn[1].Data;

			Compute(value1, value2);
		}

		#endregion

		#region 補助関数:

		/// <summary>
		/// 指定された値をキャストして関数を呼び出します。
		/// </summary>
		/// <param name="value1">左辺値</param>
		/// <param name="value2">右辺値</param>
		/// <returns>
		///		計算結果を返します。
		///		未対応の型が指定された場合は例外を発行します。
		/// </returns>
		public void Compute(object value1, object value2)
		{
			if (value1 is Decimal && value2 is Decimal)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is double && value2 is double)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is float && value2 is float)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is long && value2 is long)
			{
				long remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((long)value1, (long)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is int && value2 is int)
			{
				int remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((int)value1, (int)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is short && value2 is short)
			{
				int remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((short)value1, (short)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is sbyte && value2 is sbyte)
			{
				int remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((sbyte)value1, (sbyte)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is ulong && value2 is ulong)
			{
				throw new System.NotSupportedException();
			}
			if (value1 is uint && value2 is uint)
			{
				long remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((uint)value1, (uint)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is ushort && value2 is ushort)
			{
				int remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((ushort)value1, (ushort)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}
			if (value1 is byte && value2 is byte)
			{
				int remainder = 0;
				this.DataOut[0].Data = System.Math.DivRem((byte)value1, (byte)value2, out remainder);
				this.DataOut[1].Data = remainder;
				return;
			}

			throw new System.NotSupportedException();
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
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];
					var type = (this.DataOut[0].Data == null)
						? typeof(object)
						: this.DataOut[0].Data.GetType();

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), type);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					scope.Add(variable.Declare());
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					var dst1 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var dst2 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src1 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var src2 = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var api = new CodeExtraType(typeof(System.Math));

					// dst1 = System.Math.DivRem(src1, src2, out dst2);
					scope.Add(dst1.Assign(api.Call("DivRem", src1, src2, dst2.Out(1))));
				}
			}
		}

		#endregion
	}

	#endregion
}
