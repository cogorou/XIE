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
	// Operations
	// 

	#region Primitive.Comparison

	/// <summary>
	/// 比較演算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Primitive_Comparison : CxScript
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_Comparison()
			: base(ExLanguageType.CSharp)
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_Comparison(ExComparisonOperatorType operator_type)
			: base(ExLanguageType.CSharp)
		{
			this.Operator = operator_type;
			_Constructor();
		}

		/// <summary>
		/// コンストラク補助関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "Primitive";
			this.Name = this.Operator.ToString();
			this.IconKey = "Notify-Question";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Left"),
				new CxTaskPortIn("Right"),
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
			/// 左辺値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 右辺値
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
			if (src is Primitive_Comparison)
			{
				base.CopyFrom(src);

				var _src = (Primitive_Comparison)src;

				this.Operator = _src.Operator;

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
				var _src = (Primitive_Comparison)src;

				if (this.Operator != _src.Operator) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 左辺値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.Primitive_Comparison.Left")]
		public string Left
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				if (this.DataIn[0].IsConnected)
				{
					return this.DataIn[0].Data.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// 右辺値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.Primitive_Comparison.Right")]
		public string Right
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				if (this.DataIn[1].IsConnected)
				{
					return this.DataIn[1].Data.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 演算子
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Primitive_Comparison.Operator")]
		public ExComparisonOperatorType Operator
		{
			get { return m_Operator; }
			set { m_Operator = value; }
		}
		private ExComparisonOperatorType m_Operator = ExComparisonOperatorType.Equal;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_Comparison.This")]
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

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			base.Setup(sender, e);
		}

		#endregion

		#region メソッド: (実行)

		[NonSerialized]
		Type DataInType0 = null;

		[NonSerialized]
		Type DataInType1 = null;

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

			// 演算子:
			string ope = "";
			switch (this.Operator)
			{
				case ExComparisonOperatorType.Equal: ope = "=="; break;
				case ExComparisonOperatorType.NotEqual: ope = "!="; break;
				case ExComparisonOperatorType.LessThan: ope = "<"; break;
				case ExComparisonOperatorType.LessThanOrEqual: ope = "<="; break;
				case ExComparisonOperatorType.GreaterThan: ope = ">"; break;
				case ExComparisonOperatorType.GreaterThanOrEqual: ope = ">="; break;
				default:
					throw new System.NotSupportedException();
			}

			// 準備:
			Type type0 = this.DataIn[0].Data.GetType();
			Type type1 = this.DataIn[1].Data.GetType();
			if (DataInType0 != type0 ||
				DataInType1 != type1)
			{
				string statement = "";
				statement += string.Format("var left = ({0})DataIn[0].Data;", type0.FullName);
				statement += string.Format("var right = ({0})DataIn[1].Data;", type1.FullName);
				statement += string.Format("DataOut[0].Data = (left {0} right);", ope);
				this.Statement = statement;
				if (this.Build() == false)
				{
					string errors = "";

					#region エラーメッセージ:
					foreach (CompilerError item in this.Results.Errors)
					{
						int line_no = item.Line - this.LineOffset;
						string msg = string.Format(
							"({0},{1}): {2} {3} {4} ",
							line_no,
							item.Column,
							(item.IsWarning) ? "Warning" : "Error",
							item.ErrorNumber,
							item.ErrorText
							);
						errors += msg;
					}
					#endregion

					throw new InvalidOperationException(errors);
				}

				DataInType0 = type0;
				DataInType1 = type1;
			}

			base.Execute(sender, e);
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

					switch (this.Operator)
					{
						default:
							break;
						case ExComparisonOperatorType.Equal:
							// dst = (src1 == src2);
							scope.Add(dst.Assign(src1.Equal(src2)));
							break;
						case ExComparisonOperatorType.NotEqual:
							// dst = (src1 != src2);
							scope.Add(dst.Assign(src1.NotEqual(src2)));
							break;
						case ExComparisonOperatorType.GreaterThan:
							// dst = (src1 > src2);
							scope.Add(dst.Assign(src1.GreaterThan(src2)));
							break;
						case ExComparisonOperatorType.GreaterThanOrEqual:
							// dst = (src1 >= src2);
							scope.Add(dst.Assign(src1.GreaterThanOrEqual(src2)));
							break;
						case ExComparisonOperatorType.LessThan:
							// dst = (src1 < src2);
							scope.Add(dst.Assign(src1.LessThan(src2)));
							break;
						case ExComparisonOperatorType.LessThanOrEqual:
							// dst = (src1 <= src2);
							scope.Add(dst.Assign(src1.LessThanOrEqual(src2)));
							break;
					}
				}
			}
		}

		#endregion

		#region メソッド: (説明文)

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <param name="key">リソースキー。(※ 省略時（null や Empty が指定された場合）は DescriptionKey プロパティを参照します。)</param>
		/// <returns>
		///		指定されたキーに対応する説明文を返します。
		///		見つからなければ Empty を返します。
		/// </returns>
		public override string GetDescription(string key)
		{
			if (0 == string.Compare(key, string.Format("T:{0}", this.GetType().FullName)))
			{
				string reskey;
				switch (this.Operator)
				{
					case ExComparisonOperatorType.Equal:
					case ExComparisonOperatorType.NotEqual:
					case ExComparisonOperatorType.LessThan:
					case ExComparisonOperatorType.LessThanOrEqual:
					case ExComparisonOperatorType.GreaterThan:
					case ExComparisonOperatorType.GreaterThanOrEqual:
						reskey = string.Format("F:{0}.{1}", this.Operator.GetType().FullName, this.Operator);
						break;
					default:
						reskey = this.DescriptionKey;
						break;
				}
				var value = XIE.AxiTextStorage.GetValue(reskey);
				return value;
			}
			else
			{
				return base.GetDescription(key);
			}
		}

		#endregion
	}

	#endregion

	#region Primitive.BinaryOperation

	/// <summary>
	/// ２項演算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Primitive_BinaryOperation : CxScript
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_BinaryOperation()
			: base(ExLanguageType.CSharp)
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_BinaryOperation(ExBinaryOperatorType operator_type)
			: base(ExLanguageType.CSharp)
		{
			this.Operator = operator_type;
			_Constructor();
		}

		/// <summary>
		/// コンストラク補助関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "Primitive";
			this.Name = this.Operator.ToString();
			this.IconKey = "Parser-Operation";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Left"),
				new CxTaskPortIn("Right"),
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
			/// 左辺値
			/// </summary>
			DataIn0,

			/// <summary>
			/// 右辺値
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
			if (src is Primitive_BinaryOperation)
			{
				base.CopyFrom(src);

				var _src = (Primitive_BinaryOperation)src;

				this.Operator = _src.Operator;

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
				var _src = (Primitive_BinaryOperation)src;

				if (this.Operator != _src.Operator) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Inputs)

		/// <summary>
		/// 左辺値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.Primitive_BinaryOperation.Left")]
		public string Left
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				if (this.DataIn[0].IsConnected)
				{
					return this.DataIn[0].Data.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// 右辺値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.Primitive_BinaryOperation.Right")]
		public string Right
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 1) return "";
				if (this.DataIn[1].IsConnected)
				{
					return this.DataIn[1].Data.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 演算子
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Primitive_BinaryOperation.Operator")]
		public ExBinaryOperatorType Operator
		{
			get { return m_Operator; }
			set { m_Operator = value; }
		}
		private ExBinaryOperatorType m_Operator = ExBinaryOperatorType.Add;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_BinaryOperation.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				if (data is byte ||
					data is sbyte ||
					data is short ||
					data is ushort ||
					data is int ||
					data is uint ||
					data is long ||
					data is ulong)
					return string.Format("0x{0:X} ({0})", data);
				else
					return string.Format("{0}", data);
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_BinaryOperation.Type")]
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
			base.Setup(sender, e);
		}

		#endregion

		#region メソッド: (実行)

		[NonSerialized]
		Type DataInType0 = null;

		[NonSerialized]
		Type DataInType1 = null;

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

			// 準備:
			Type type0 = this.DataIn[0].Data.GetType();
			Type type1 = this.DataIn[1].Data.GetType();
			if (DataInType0 != type0 ||
				DataInType1 != type1)
			{
				string statement = "";
				statement += string.Format("var left = ({0})DataIn[0].Data;", type0.FullName);
				statement += string.Format("var right = ({0})DataIn[1].Data;", type1.FullName);

				#region 式:
				switch (this.Operator)
				{
					case ExBinaryOperatorType.Add:
						{
							statement += "DataOut[0].Data = (left + right);";
						}
						break;
					case ExBinaryOperatorType.Subtract:
						{
							statement += "DataOut[0].Data = (left - right);";
						}
						break;
					case ExBinaryOperatorType.Multiply:
						{
							statement += "DataOut[0].Data = (left * right);";
						}
						break;
					case ExBinaryOperatorType.Divide:
						{
							statement += "DataOut[0].Data = (left / right);";
						}
						break;
					case ExBinaryOperatorType.Modulus:
						{
							statement += "DataOut[0].Data = (left % right);";
						}
						break;
					case ExBinaryOperatorType.And:
						{
							var ope = "";	// 式:
							if (type0 == typeof(bool) && type1 == typeof(bool))
								ope = "(left && right)";
							else
								ope = "(left & right)";

							if (type0 == type1)
								statement += string.Format("var ans = ({0})({1});", type0.FullName, ope);
							else
								statement += string.Format("var ans = ({0});", ope);

							statement += "DataOut[0].Data = ans;";
						}
						break;
					case ExBinaryOperatorType.Or:
						{
							var ope = "";	// 式:
							if (type0 == typeof(bool) && type1 == typeof(bool))
								ope = "(left || right)";
							else
								ope = "(left | right)";

							if (type0 == type1)
								statement += string.Format("var ans = ({0})({1});", type0.FullName, ope);
							else
								statement += string.Format("var ans = ({0});", ope);

							statement += "DataOut[0].Data = ans;";
						}
						break;
					case ExBinaryOperatorType.Xor:
						{
							var ope = "";	// 式:
							if (type0 == typeof(bool) && type1 == typeof(bool))
								ope = "(left ^ right)";
							else
								ope = "(left ^ right)";

							if (type0 == type1)
								statement += string.Format("var ans = ({0})({1});", type0.FullName, ope);
							else
								statement += string.Format("var ans = ({0});", ope);

							statement += "DataOut[0].Data = ans;";
						}
						break;
					case ExBinaryOperatorType.Nand:
						{
							var ope = "";	// 式:
							if (type0 == typeof(bool) && type1 == typeof(bool))
								ope = "!(left && right)";
							else
								ope = "~(left & right)";

							if (type0 == type1)
								statement += string.Format("var ans = ({0})({1});", type0.FullName, ope);
							else
								statement += string.Format("var ans = ({0});", ope);

							statement += "DataOut[0].Data = ans;";
						}
						break;
					default:
						throw new System.NotSupportedException();
				}
				#endregion

				this.Statement = statement;
				if (this.Build() == false)
				{
					string errors = "";

					#region エラーメッセージ:
					foreach (CompilerError item in this.Results.Errors)
					{
						int line_no = item.Line - this.LineOffset;
						string msg = string.Format(
							"({0},{1}): {2} {3} {4} ",
							line_no,
							item.Column,
							(item.IsWarning) ? "Warning" : "Error",
							item.ErrorNumber,
							item.ErrorText
							);
						errors += msg;
					}
					#endregion

					throw new InvalidOperationException(errors);
				}

				DataInType0 = type0;
				DataInType1 = type1;
			}

			base.Execute(sender, e);
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

					switch (this.Operator)
					{
						case ExBinaryOperatorType.Add:
							// dst = src1 + src2;
							scope.Add(dst.Assign(src1.Add(src2)));
							break;
						case ExBinaryOperatorType.Subtract:
							// dst = src1 - src2;
							scope.Add(dst.Assign(src1.Subtract(src2)));
							break;
						case ExBinaryOperatorType.Multiply:
							// dst = src1 * src2;
							scope.Add(dst.Assign(src1.Multiply(src2)));
							break;
						case ExBinaryOperatorType.Divide:
							// dst = src1 / src2;
							scope.Add(dst.Assign(src1.Divide(src2)));
							break;
						case ExBinaryOperatorType.Modulus:
							// dst = src1 % src2;
							scope.Add(dst.Assign(src1.Modulus(src2)));
							break;
						case ExBinaryOperatorType.And:
							// dst = src1 && src2;
							// dst = src1 & src2;
							if (dst.Type == typeof(bool))
								scope.Add(dst.Assign(new CodeCastExpression(dst.Type, src1.BooleanAnd(src2))));
							else
								scope.Add(dst.Assign(new CodeCastExpression(dst.Type, src1.BitwiseAnd(src2))));
							break;
						case ExBinaryOperatorType.Or:
							// dst = src1 || src2;
							// dst = src1 | src2;
							if (dst.Type == typeof(bool))
								scope.Add(dst.Assign(new CodeCastExpression(dst.Type, src1.BooleanOr(src2))));
							else
								scope.Add(dst.Assign(new CodeCastExpression(dst.Type, src1.BitwiseOr(src2))));
							break;
						case ExBinaryOperatorType.Xor:
							// dst = src1 ^ src2;
							// CodeDom には Xor は存在しないので CodeSnnipet を使用しています.
							if (e.LanguageType == ExLanguageType.CSharp)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = ({1})({2} ^ {3});",
									dst.VariableName,
									dst.Type.FullName,
									src1.VariableName,
									src2.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else if (e.LanguageType == ExLanguageType.VisualBasic)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = CType(({2} Xor {3}), {1})",
									dst.VariableName,
									dst.Type.FullName,
									src1.VariableName,
									src2.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else
							{
								scope.Add(new CodeCommentStatement(string.Format("{0} is not supported.", this.Operator)));
								scope.Add(new CodeCommentStatement(string.Format("{1} = {2} {0} {3}"
									, this.Operator
									, dst.VariableName
									, src1.VariableName
									, src2.VariableName
									)));
							}
							break;
						case ExBinaryOperatorType.Nand:
							// dst = !(src1 && src2);
							// dst = ~(src1 & src2);
							// CodeDom には Nand は存在しないので CodeSnnipet を使用しています.
							if (e.LanguageType == ExLanguageType.CSharp)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = "";
								if (dst.Type == typeof(bool))
								{
									statement = string.Format("{0} = !(({1})({2} && {3}));"
										 , dst.VariableName
										 , dst.Type.FullName
										 , src1.VariableName
										 , src2.VariableName);
								}
								else
								{
									statement = string.Format("{0} = ~(({1})({2} & {3}));"
										 , dst.VariableName
										 , dst.Type.FullName
										 , src1.VariableName
										 , src2.VariableName);
								}
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else if (e.LanguageType == ExLanguageType.VisualBasic)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = CType(Not ({2} And {3}), {1})",
									dst.VariableName,
									dst.Type.FullName,
									src1.VariableName,
									src2.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else
							{
								scope.Add(new CodeCommentStatement(string.Format("{0} is not supported.", this.Operator)));
								scope.Add(new CodeCommentStatement(string.Format("{1} = {2} {0} {3}"
									, this.Operator
									, dst.VariableName
									, src1.VariableName
									, src2.VariableName
									)));
							}
							break;
						default:
							break;
					}

				}
			}
		}

		#endregion

		#region メソッド: (説明文)

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <param name="key">リソースキー。(※ 省略時（null や Empty が指定された場合）は DescriptionKey プロパティを参照します。)</param>
		/// <returns>
		///		指定されたキーに対応する説明文を返します。
		///		見つからなければ Empty を返します。
		/// </returns>
		public override string GetDescription(string key)
		{
			if (0 == string.Compare(key, string.Format("T:{0}", this.GetType().FullName)))
			{
				string reskey;
				switch (this.Operator)
				{
					case ExBinaryOperatorType.Add:
					case ExBinaryOperatorType.Subtract:
					case ExBinaryOperatorType.Multiply:
					case ExBinaryOperatorType.Divide:
					case ExBinaryOperatorType.Modulus:
					case ExBinaryOperatorType.And:
					case ExBinaryOperatorType.Or:
					case ExBinaryOperatorType.Xor:
					case ExBinaryOperatorType.Nand:
						reskey = string.Format("F:{0}.{1}", this.Operator.GetType().FullName, this.Operator);
						break;
					default:
						reskey = this.DescriptionKey;
						break;
				}
				var value = XIE.AxiTextStorage.GetValue(reskey);
				return value;
			}
			else
			{
				return base.GetDescription(key);
			}
		}

		#endregion
	}

	#endregion

	#region Primitive.UnaryOperation

	/// <summary>
	/// 単項演算
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Primitive_UnaryOperation : CxScript
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_UnaryOperation()
			: base(ExLanguageType.CSharp)
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_UnaryOperation(ExUnaryOperatorType operator_type)
			: base(ExLanguageType.CSharp)
		{
			this.Operator = operator_type;
			_Constructor();
		}

		/// <summary>
		/// コンストラク補助関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "Primitive";
			this.Name = this.Operator.ToString();
			this.IconKey = "Parser-Operation";

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
			if (src is Primitive_UnaryOperation)
			{
				base.CopyFrom(src);

				var _src = (Primitive_UnaryOperation)src;

				this.Operator = _src.Operator;

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
				var _src = (Primitive_UnaryOperation)src;

				if (this.Operator != _src.Operator) return false;
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
		[CxDescription("P:XIE.Tasks.Primitive_UnaryOperation.Value")]
		public string Value
		{
			get
			{
				if (this.DataIn == null) return "";
				if (this.DataIn.Length <= 0) return "";
				if (this.DataIn[0].IsConnected)
				{
					return this.DataIn[0].Data.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 演算子
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Primitive_UnaryOperation.Operator")]
		public ExUnaryOperatorType Operator
		{
			get { return m_Operator; }
			set { m_Operator = value; }
		}
		private ExUnaryOperatorType m_Operator = ExUnaryOperatorType.BooleanNot;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_UnaryOperation.This")]
		public string This
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				if (data is byte ||
					data is sbyte ||
					data is short ||
					data is ushort ||
					data is int ||
					data is uint ||
					data is long ||
					data is ulong)
					return string.Format("0x{0:X} ({0})", data);
				else
					return string.Format("{0}", data);
			}
		}

		/// <summary>
		/// 結果の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_UnaryOperation.Type")]
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
			base.Setup(sender, e);
		}

		#endregion

		#region メソッド: (実行)

		[NonSerialized]
		Type DataInType0 = null;

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

			// 演算子:
			string ope = "";
			switch (this.Operator)
			{
				case ExUnaryOperatorType.BooleanNot: ope = "!"; break;
				case ExUnaryOperatorType.BitwiseNot: ope = "~"; break;
				default:
					throw new System.NotSupportedException();
			}

			// 準備:
			Type type0 = this.DataIn[0].Data.GetType();
			if (DataInType0 != type0)
			{
				string statement = "";
				statement += string.Format("{0} value = ({0})DataIn[0].Data;", type0.FullName);
				statement += string.Format("{0} ans = ({0})({1}value);", type0.FullName, ope);
				statement += "DataOut[0].Data = ans;";
				this.Statement = statement;
				if (this.Build() == false)
				{
					string errors = "";

					#region エラーメッセージ:
					foreach (CompilerError item in this.Results.Errors)
					{
						int line_no = item.Line - this.LineOffset;
						string msg = string.Format(
							"({0},{1}): {2} {3} {4} ",
							line_no,
							item.Column,
							(item.IsWarning) ? "Warning" : "Error",
							item.ErrorNumber,
							item.ErrorText
							);
						errors += msg;
					}
					#endregion

					throw new InvalidOperationException(errors);
				}

				DataInType0 = type0;
			}

			base.Execute(sender, e);
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

					switch (this.Operator)
					{
						case ExUnaryOperatorType.BooleanNot:
							// dst = !src1;
							// CodeDom には ! (論理否定) は存在しないので CodeSnippet を使用しています.
							if (e.LanguageType == ExLanguageType.CSharp)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = !(({1}){2});"
										 , dst.VariableName
										 , dst.Type.FullName
										 , src1.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else if (e.LanguageType == ExLanguageType.VisualBasic)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = CType(Not {2}, {1})"
										, dst.VariableName
										, dst.Type.FullName
										, src1.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else
							{
								scope.Add(new CodeCommentStatement(string.Format("{0} is not supported.", this.Operator)));
								scope.Add(new CodeCommentStatement(string.Format("{1} = {0} {2}"
										, this.Operator
										, dst.VariableName
										, src1.VariableName
									)));
							}
							break;
						case ExUnaryOperatorType.BitwiseNot:
							// dst = ~src1;
							// CodeDom には ~ (ビット反転) は存在しないので CodeSnippet を使用しています.
							if (e.LanguageType == ExLanguageType.CSharp)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = ~(({1}){2});"
										 , dst.VariableName
										 , dst.Type.FullName
										 , src1.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else if (e.LanguageType == ExLanguageType.VisualBasic)
							{
								var indent = new string('	', e.IndentLevel);
								var statement = string.Format("{0} = CType(Not {2}, {1})"
										, dst.VariableName
										, dst.Type.FullName
										, src1.VariableName);
								scope.Add(new CodeSnippetStatement(indent + statement));
							}
							else
							{
								scope.Add(new CodeCommentStatement(string.Format("{0} is not supported.", this.Operator)));
								scope.Add(new CodeCommentStatement(string.Format("{1} = {0} {2}"
										, this.Operator
										, dst.VariableName
										, src1.VariableName
									)));
							}
							break;
						default:
							break;
					}

				}
			}
		}

		#endregion

		#region メソッド: (説明文)

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <param name="key">リソースキー。(※ 省略時（null や Empty が指定された場合）は DescriptionKey プロパティを参照します。)</param>
		/// <returns>
		///		指定されたキーに対応する説明文を返します。
		///		見つからなければ Empty を返します。
		/// </returns>
		public override string GetDescription(string key)
		{
			if (0 == string.Compare(key, string.Format("T:{0}", this.GetType().FullName)))
			{
				string reskey;
				switch (this.Operator)
				{
					case ExUnaryOperatorType.BooleanNot:
					case ExUnaryOperatorType.BitwiseNot:
						reskey = string.Format("F:{0}.{1}", this.Operator.GetType().FullName, this.Operator);
						break;
					default:
						reskey = this.DescriptionKey;
						break;
				}
				var value = XIE.AxiTextStorage.GetValue(reskey);
				return value;
			}
			else
			{
				return base.GetDescription(key);
			}
		}

		#endregion
	}

	#endregion

	#region Primitive.Pass

	/// <summary>
	/// プロパティに指定された任意の値を出力します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Primitive_Pass : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Primitive_Pass()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">設定値</param>
		public Primitive_Pass(object value)
			: base()
		{
			_Constructor();
			this.Value = value;
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "Primitive";
			this.Name = "Pass";
			this.IconKey = "Node-DataOut";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Value", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 指定された値
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
			if (src is Primitive_Pass)
			{
				base.CopyFrom(src);

				var _src = (Primitive_Pass)src;

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
				var _src = (Primitive_Pass)src;

				if (this.Value != _src.Value) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 出力値
		/// </summary>
		[XmlIgnore]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Primitive_Pass.Value")]
		public object Value
		{
			get { return this.DataOut[0].Data; }
			set { this.DataOut[0].Data = value; }
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
				//scope.Add(new CodeSnippetStatement());
				//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[0];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), typeof(object));

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_Value = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.Value)));
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
		}

		#endregion
	}

	#endregion
}
