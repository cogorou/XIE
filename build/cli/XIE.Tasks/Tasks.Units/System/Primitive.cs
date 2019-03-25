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
	// Primitive
	// 

	#region Boolean.ctor

	/// <summary>
	/// ブーリアン
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Boolean_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Boolean_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Boolean";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Boolean) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Boolean_ctor)
			{
				base.CopyFrom(src);

				var _src = (Boolean_ctor)src;

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
				var _src = (Boolean_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Boolean_ctor.This")]
		public Boolean This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Boolean m_This = default(Boolean);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Byte.ctor

	/// <summary>
	/// 8bits 整数（符号なし）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Byte_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Byte_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Byte";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Byte) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Byte_ctor)
			{
				base.CopyFrom(src);

				var _src = (Byte_ctor)src;

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
				var _src = (Byte_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Byte_ctor.This")]
		public Byte This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Byte m_This = default(Byte);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region SByte.ctor

	/// <summary>
	/// 8bits 整数（符号付き）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class SByte_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SByte_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "SByte";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(SByte) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is SByte_ctor)
			{
				base.CopyFrom(src);

				var _src = (SByte_ctor)src;

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
				var _src = (SByte_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.SByte_ctor.This")]
		public SByte This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private SByte m_This = default(SByte);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Char.ctor

	/// <summary>
	/// Unicode 文字
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Char_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Char_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Char";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Char) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Char_ctor)
			{
				base.CopyFrom(src);

				var _src = (Char_ctor)src;

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
				var _src = (Char_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Char_ctor.This")]
		public Char This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Char m_This = default(Char);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region UInt16.ctor

	/// <summary>
	/// 16bits 整数（符号なし）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class UInt16_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public UInt16_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "UInt16";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(UInt16) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is UInt16_ctor)
			{
				base.CopyFrom(src);

				var _src = (UInt16_ctor)src;

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
				var _src = (UInt16_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.UInt16_ctor.This")]
		public UInt16 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private UInt16 m_This = default(UInt16);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Int16.ctor

	/// <summary>
	/// 16bits 整数（符号付き）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Int16_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Int16_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Int16";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Int16) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Int16_ctor)
			{
				base.CopyFrom(src);

				var _src = (Int16_ctor)src;

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
				var _src = (Int16_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Int16_ctor.This")]
		public Int16 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Int16 m_This = default(Int16);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region UInt32.ctor

	/// <summary>
	/// 32bits 整数（符号なし）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class UInt32_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public UInt32_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "UInt32";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(UInt32) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is UInt32_ctor)
			{
				base.CopyFrom(src);

				var _src = (UInt32_ctor)src;

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
				var _src = (UInt32_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.UInt32_ctor.This")]
		public UInt32 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private UInt32 m_This = default(UInt32);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Int32.ctor

	/// <summary>
	/// 32bits 整数（符号付き）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Int32_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Int32_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Int32";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Int32) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Int32_ctor)
			{
				base.CopyFrom(src);

				var _src = (Int32_ctor)src;

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
				var _src = (Int32_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Int32_ctor.This")]
		public Int32 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Int32 m_This = default(Int32);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region UInt64.ctor

	/// <summary>
	/// 64bits 整数（符号なし）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class UInt64_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public UInt64_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "UInt64";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(UInt64) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is UInt64_ctor)
			{
				base.CopyFrom(src);

				var _src = (UInt64_ctor)src;

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
				var _src = (UInt64_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.UInt64_ctor.This")]
		public UInt64 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private UInt64 m_This = default(UInt64);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Int64.ctor

	/// <summary>
	/// 64bits 整数（符号付き）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Int64_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Int64_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Int64";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Int64) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Int64_ctor)
			{
				base.CopyFrom(src);

				var _src = (Int64_ctor)src;

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
				var _src = (Int64_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Int64_ctor.This")]
		public Int64 This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Int64 m_This = default(Int64);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Single.ctor

	/// <summary>
	/// 単精度浮動小数点
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Single_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Single_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Single";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Single) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Single_ctor)
			{
				base.CopyFrom(src);

				var _src = (Single_ctor)src;

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
				var _src = (Single_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Single_ctor.This")]
		public Single This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Single m_This = default(Single);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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

	#region Double.ctor

	/// <summary>
	/// 倍精度浮動小数点
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Double_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Double_ctor()
			: base()
		{
			this.Category = "Primitive";
			this.Name = "Double";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Double) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// プロパティで設定された値を返します。
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
			if (src is Double_ctor)
			{
				base.CopyFrom(src);

				var _src = (Double_ctor)src;

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
				var _src = (Double_ctor)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 値
		/// </summary>
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Double_ctor.This")]
		public Double This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private Double m_This = default(Double);

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			var args = new CxTaskExecuteEventArgs();
			args.CopyFrom(e);
			this.Execute(sender, args);
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
			this.DataOut[0].Data = this.This;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
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
