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
	// Array
	// 

	#region Array.ctor

	/// <summary>
	/// 配列。固定長配列を生成します。要素の型は System.Object になっています。変更する場合は Control Panel を使用してください。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Array_ctor : CxTaskUnit
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Array_ctor()
			: base()
		{
			this.Category = "System";
			this.Name = "Array";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 確保する要素数
			/// </summary>
			DataParam0,

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
			if (src is Array_ctor)
			{
				base.CopyFrom(src);

				var _src = (Array_ctor)src;

				this.Length = _src.Length;
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
				var _src = (Array_ctor)src;

				if (this.Length != _src.Length) return false;
				if (this.Type != _src.Type) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素数 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Array_ctor.Length")]
		public virtual int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

		/// <summary>
		/// 要素の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Array_ctor.Type")]
		public virtual Type Type
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.TypeForSerialize))
					return typeof(object);
				else
					return Type.GetType(this.TypeForSerialize);
			}
			set
			{
				if (value == null)
					m_TypeForSerialize = "";
				else
					m_TypeForSerialize = value.FullName;
			}
		}

		/// <summary>
		/// 要素の型 (シリアライズ用)
		/// </summary>
		[Browsable(false)]
		public virtual string TypeForSerialize
		{
			get { return m_TypeForSerialize; }
			set { m_TypeForSerialize = value; }
		}
		private string m_TypeForSerialize = "";

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

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Length = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			if (this.Type == null)
				this.Type = typeof(object);

			var item_type = this.Type;
			var list_type = typeof(System.Collections.Generic.List<>);
			var body_type = list_type.MakeGenericType(item_type);
			var body_ctor = body_type.GetConstructor(Type.EmptyTypes);

			var body = body_ctor.Invoke(null);
			var body_Add = body_type.GetMethod("Add");
			var body_ToArray = body_type.GetMethod("ToArray");

			for (int i = 0; i < this.Length; i++)
			{
				body_Add.Invoke(body, new object[] { null });
			}

			this.DataOut[0].Data = body_ToArray.Invoke(body, null);
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
				var name = e.TaskNames[this];
				var port = this.DataOut[0];

				var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Data.GetType());

				var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
				var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
				e.Variables[key] = value;

				if (this.DataParam[0].IsConnected)
				{
					// xxxx[] task#_This;
					scope.Add(variable.Declare());
				}
				else
				{
					// xxxx[] task#_This = new xxxx[##];
					scope.Add(variable.Declare(new CodeArrayCreateExpression(this.Type, this.Length)));
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
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				if (this.DataParam[0].IsConnected)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var length = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

					// task#_This = new xxxx[length];
					scope.Add(dst.Assign(new CodeArrayCreateExpression(this.Type, length)));
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
			var dlg = new XIE.Tasks.CxTaskPortTypeForm();
			dlg.Type = this.Type;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				if (dlg.Type != null)
					this.Type = dlg.Type;
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

	#region Array.Length

	/// <summary>
	/// 要素数の取得
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Array_Length : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Array_Length()
			: base()
		{
			this.Category = "System.Array";
			this.Name = "Length";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(System.Array)}),
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
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 要素数を返します。
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
			if (src is Array_Length)
			{
				base.CopyFrom(src);

				var _src = (Array_Length)src;

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
				var _src = (Array_Length)src;
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
		[CxDescription("P:XIE.Tasks.Array_Length.This")]
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
			var body = (System.Collections.IEnumerable)this.DataIn[0].Data;

			// 実行.
			this.This = (int)body.GetType().GetProperty("Length").GetValue(body, null);

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

					// task#_This = task$.Length;
					scope.Add(dst.Assign(src.Ref("Length")));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Array.Item_get

	/// <summary>
	/// 要素の取得
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Array_Item_get : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Array_Item_get()
			: base()
		{
			this.Category = "System.Array";
			this.Name = "Item";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Array)),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 配列指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当する要素を返します。
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
			if (src is Array_Item_get)
			{
				base.CopyFrom(src);

				var _src = (Array_Item_get)src;

				this.Index = _src.Index;

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
				var _src = (Array_Item_get)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Array_Item_get.Index")]
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		#region IndexConverter

		class IndexConverter : Int32Converter
		{
			public IndexConverter()
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
					var body = ((Array_Item_get)context.Instance).DataIn[0].Data as System.Collections.IEnumerable;
					if (body != null)
					{
						int index = 0;
						foreach (var item in body)
							items.Add(index++);
					}
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
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Array_Item_get.This")]
		public object This
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length == 0) return null;
				object data = this.DataOut[0].Data;
				if (data == null) return null;
				if (data.GetType().IsClass)
					return data;
				else
					return data.ToString();
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var body = (System.Collections.IEnumerable)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 準備.
			if (this.Pass == null)
				this.Pass = new Primitive_Pass();

			if (this.Invoker == null)
			{
				this.Invoker = new CxScript(ExLanguageType.CSharp);
				this.Invoker.DataIn = this.DataIn;
				this.Invoker.DataParam = new CxTaskPortIn[]
				{
					new CxTaskPortIn("Index", new Type[] { typeof(int) })
				};
				this.Invoker.DataOut = this.DataOut;
			}

			if (this.ArrayType != body.GetType())
			{
				string statement = "";
				statement += string.Format("var body = ({0})DataIn[0].Data;", body.GetType().FullName);
				statement += "var index = Convert.ToInt32(DataParam[0].Data);";
				statement += "DataOut[0].Data = body[index];";
				this.Invoker.Statement = statement;
				if (this.Invoker.Build() == false)
					throw new InvalidOperationException();
				this.ArrayType = body.GetType();
			}

			// Pass は、このクラスの Index プロパティの値を Invoker の DataParam[0] へ渡す為に使用します。
			this.Pass.Value = this.Index;
			this.Pass.Execute(sender, e);

			// 実行.
			this.Invoker.DataParam[0].ReferenceTask = this.Pass;
			this.Invoker.DataParam[0].ReferencePort = this.Pass.DataOut[0];
			this.Invoker.Execute(sender, e);
		}

		[NonSerialized]
		Type ArrayType = null;

		[NonSerialized]
		Primitive_Pass Pass = null;

		[NonSerialized]
		CxScript Invoker = null;

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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// task#_Item = body[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(body, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Array.Item_set

	/// <summary>
	/// 要素の設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Array_Item_set : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Array_Item_set()
			: base()
		{
			this.Category = "System.Array";
			this.Name = "Item";
			this.IconKey = "Unit-PropertySet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Array)),
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された指標の位置に代入する要素
			/// </summary>
			DataIn1,

			/// <summary>
			/// 配列指標 [0~]
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
			if (src is Array_Item_set)
			{
				base.CopyFrom(src);

				var _src = (Array_Item_set)src;

				this.Index = _src.Index;

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
				var _src = (Array_Item_set)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Array_Item_set.Index")]
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		#region IndexConverter

		class IndexConverter : Int32Converter
		{
			public IndexConverter()
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
					var body = ((Array_Item_set)context.Instance).DataIn[0].Data as System.Collections.IEnumerable;
					if (body != null)
					{
						int index = 0;
						foreach (var item in body)
							items.Add(index++);
					}
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
			var body = (System.Collections.IEnumerable)this.DataIn[0].Data;
			var value = this.DataIn[1].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 準備.
			if (this.Pass == null)
				this.Pass = new Primitive_Pass();

			if (this.Invoker == null)
			{
				this.Invoker = new CxScript(ExLanguageType.CSharp);
				this.Invoker.DataIn = this.DataIn;
				this.Invoker.DataParam = new CxTaskPortIn[]
				{
					new CxTaskPortIn("Index", new Type[] { typeof(int) })
				};
				this.Invoker.DataOut = this.DataOut;
			}

			if (this.ArrayType != body.GetType())
			{
				string statement = "";
				statement += string.Format("var body = ({0})DataIn[0].Data;", body.GetType().FullName);
				statement += string.Format("var value = ({0})DataIn[1].Data;", value.GetType().FullName);
				statement += "var index = Convert.ToInt32(DataParam[0].Data);";
				statement += "body[index] = value;";
				this.Invoker.Statement = statement;
				this.Invoker.Build();
				if (this.Invoker.Build() == false)
					throw new InvalidOperationException();
				this.ArrayType = body.GetType();
			}

			// Pass は、このクラスの Index プロパティの値を Invoker の DataParam[0] へ渡す為に使用します。
			this.Pass.Value = this.Index;
			this.Pass.Execute(sender, e);

			// 実行.
			this.Invoker.DataParam[0].ReferenceTask = this.Pass;
			this.Invoker.DataParam[0].ReferencePort = this.Pass.DataOut[0];
			this.Invoker.Execute(sender, e);
		}

		[NonSerialized]
		Type ArrayType = null;

		[NonSerialized]
		Primitive_Pass Pass = null;

		[NonSerialized]
		CxScript Invoker = null;

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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var value = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// body[index] = value;
					scope.Add(new CodeAssignStatement(new CodeIndexerExpression(body, index), value));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Array.Copy1

	/// <summary>
	/// 指定範囲のコピー。先頭から指定の数の要素を複製します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Array_Copy1 : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Array_Copy1()
			: base()
		{
			this.Category = "System.Array";
			this.Name = "Copy1";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Src", new Type[] { typeof(System.Array) }),
				new CxTaskPortIn("Dst", new Type[] { typeof(System.Array) }),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 複製元
			/// </summary>
			DataIn0,

			/// <summary>
			/// 複製先
			/// </summary>
			DataIn1,

			/// <summary>
			/// 複製する要素数
			/// </summary>
			DataParam0,

			/// <summary>
			/// 複製先のオブジェクトを返します。
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
			if (src is Array_Copy1)
			{
				base.CopyFrom(src);

				var _src = (Array_Copy1)src;

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
				var _src = (Array_Copy1)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 複製する要素数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Array_Copy1.Length")]
		public int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

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
			for(int i=0 ; i<this.DataIn.Length ; i++)
				this.DataIn[i].CheckValidity(true);
			var src = (System.Array)this.DataIn[0].Data;
			var dst = (System.Array)this.DataIn[1].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Length = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			System.Array.Copy(src, dst, this.Length);

			this.DataOut[0].Data = dst;
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
					var dst = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var res = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var length = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Length));

					// System.Array.Copy(src, dst, length);
					var arr = new CodeExtraType(typeof(System.Array));
					scope.Add(arr.Call("Copy", src, dst, length));

					// res = dst;
					scope.Add(res.Assign(dst));
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// List
	// 

	#region List.ctor

	/// <summary>
	/// コレクション。System.Collections.Generic.List を生成します。要素の型は System.Object になっています。変更する場合は Control Panel を使用してください。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_ctor : CxTaskUnit
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_ctor()
			: base()
		{
			this.Category = "System.Collections.Generic";
			this.Name = "List";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
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
			if (src is List_ctor)
			{
				base.CopyFrom(src);

				var _src = (List_ctor)src;

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
				var _src = (List_ctor)src;

				if (this.Type != _src.Type) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 要素の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.List_ctor.Type")]
		public virtual Type Type
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.TypeForSerialize))
					return typeof(object);
				else
					return Type.GetType(this.TypeForSerialize);
			}
			set
			{
				if (value == null)
					m_TypeForSerialize = "";
				else
					m_TypeForSerialize = value.FullName;
			}
		}

		/// <summary>
		/// 要素の型 (シリアライズ用)
		/// </summary>
		[Browsable(false)]
		public virtual string TypeForSerialize
		{
			get { return m_TypeForSerialize; }
			set { m_TypeForSerialize = value; }
		}
		private string m_TypeForSerialize = "";

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 本体
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.List_ctor.This")]
		public System.Collections.IList This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private System.Collections.IList m_This = null;

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

			if (this.Type == null)
				this.Type = typeof(object);

			var item_type = this.Type;
			var list_type = typeof(System.Collections.Generic.List<>);
			var body_type = list_type.MakeGenericType(item_type);
			var body_ctor = body_type.GetConstructor(Type.EmptyTypes);

			this.This = (System.Collections.IList)body_ctor.Invoke(null);

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
			var dlg = new XIE.Tasks.CxTaskPortTypeForm();
			dlg.Type = this.Type;
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				if (dlg.Type != null)
					this.Type = dlg.Type;
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

	#region List.Add

	/// <summary>
	/// 要素の追加
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_Add : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_Add()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "Add";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Collections.IList)),
				new CxTaskPortIn("Value"),
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
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 追加する要素
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
			if (src is List_Add)
			{
				base.CopyFrom(src);

				var _src = (List_Add)src;

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
				var _src = (List_Add)src;
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
			this.DataIn[1].CheckValidity(true);
			var body = (System.Collections.IList)this.DataIn[0].Data;
			object value = this.DataIn[1].Data;

			// 実行.
			body.Add(value);
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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var item = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// body.Add(item);
					scope.Add(body.Call("Add", item));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.AddRange

	/// <summary>
	/// 要素の追加
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_AddRange : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_AddRange()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "AddRange";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Collections.IList)),
				new CxTaskPortIn("Items", typeof(System.Collections.IEnumerable)),
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
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 追加する要素のコレクション
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
			if (src is List_AddRange)
			{
				base.CopyFrom(src);

				var _src = (List_AddRange)src;

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
				var _src = (List_AddRange)src;
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
			this.DataIn[1].CheckValidity(true);
			var body = (System.Collections.IList)this.DataIn[0].Data;
			var items = (System.Collections.IEnumerable)this.DataIn[1].Data;

			// 実行.
			var method = body.GetType().GetMethod("AddRange");
			method.Invoke(body, new object[] { items });
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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var items = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// body.AddRange(items);
					scope.Add(body.Call("AddRange", items));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.Clear

	/// <summary>
	/// 要素の消去
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_Clear : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_Clear()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "Clear";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Collections.IList)),
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
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,
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
			if (src is List_Clear)
			{
				base.CopyFrom(src);

				var _src = (List_Clear)src;

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
				var _src = (List_Clear)src;
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
			var body = (System.Collections.IList)this.DataIn[0].Data;

			// 実行.
			body.Clear();
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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// body.Clear();
					scope.Add(body.Call("Clear"));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.Count

	/// <summary>
	/// 要素数の取得
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_Count : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_Count()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "Count";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(System.Collections.IList)}),
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
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 要素数を返します。
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
			if (src is List_Count)
			{
				base.CopyFrom(src);

				var _src = (List_Count)src;

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
				var _src = (List_Count)src;
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
		[CxDescription("P:XIE.Tasks.List_Count.This")]
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
			var src = (System.Collections.IList)this.DataIn[0].Data;

			// 実行.
			this.This = src.Count;

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

					// task#_This = task$.Count;
					scope.Add(dst.Assign(src.Ref("Count")));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.Item_get

	/// <summary>
	/// 要素の取得
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_Item_get : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_Item_get()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "Item";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Collections.IList)),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 配列指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当する要素を返します。
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
			if (src is List_Item_get)
			{
				base.CopyFrom(src);

				var _src = (List_Item_get)src;

				this.Index = _src.Index;

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
				var _src = (List_Item_get)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.List_Item_get.Index")]
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		#region IndexConverter

		class IndexConverter : Int32Converter
		{
			public IndexConverter()
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
					var body = ((List_Item_get)context.Instance).DataIn[0].Data as System.Collections.IList;
					if (body != null)
					{
						for (int i = 0; i < body.Count; i++)
							items.Add(i);
					}
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
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.List_Item_get.This")]
		public object This
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length == 0) return null;
				object data = this.DataOut[0].Data;
				if (data == null) return null;
				if (data.GetType().IsClass)
					return data;
				else
					return data.ToString();
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

			// 引数の取得:
			this.DataIn[0].CheckValidity(true);
			var body = (System.Collections.IList)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			this.DataOut[0].Data = body[this.Index];
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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

					// index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// task#_Item = body[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(body, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.Item_set

	/// <summary>
	/// 要素の設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_Item_set : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_Item_set()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "Item";
			this.IconKey = "Unit-PropertySet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", typeof(System.Collections.IList)),
				new CxTaskPortIn("Value"),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 指定された指標の位置に代入する要素
			/// </summary>
			DataIn1,

			/// <summary>
			/// 配列指標 [0~]
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
			if (src is List_Item_set)
			{
				base.CopyFrom(src);

				var _src = (List_Item_set)src;

				this.Index = _src.Index;

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
				var _src = (List_Item_set)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.List_Item_set.Index")]
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

		#region IndexConverter

		class IndexConverter : Int32Converter
		{
			public IndexConverter()
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
					var body = ((List_Item_set)context.Instance).DataIn[0].Data as System.Collections.IList;
					if (body != null)
					{
						for (int i = 0; i < body.Count; i++)
							items.Add(i);
					}
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
			var body = (System.Collections.IList)this.DataIn[0].Data;
			var value = this.DataIn[1].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			body[this.Index] = value;
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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var value = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));

					// index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// body[index] = value;
					scope.Add(new CodeAssignStatement(new CodeIndexerExpression(body, index), value));
				}
			}
		}

		#endregion
	}

	#endregion

	#region List.ToArray

	/// <summary>
	/// 配列への変換
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class List_ToArray : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public List_ToArray()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "ToArray";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(System.Collections.IList)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 配列に変換して返します。
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
			if (src is List_ToArray)
			{
				base.CopyFrom(src);

				var _src = (List_ToArray)src;

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
				var _src = (List_ToArray)src;
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
			this.DataIn[0].CheckValidity(true);
			var body = (System.Collections.IList)this.DataIn[0].Data;

			// 実行.
			this.DataOut[0].Data = body.GetType().GetMethod("ToArray").Invoke(body, null);
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

					// task#_This = task$.ToArray();
					scope.Add(dst.Assign(src.Call("ToArray")));
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// List<string>
	// 

	#region FileNameList

	/// <summary>
	/// ファイル名の配列
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class FileNameList : CxTaskUnit
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FileNameList()
			: base()
		{
			this.Category = "System.Collections.Generic.List";
			this.Name = "FileNameList";
			this.IconKey = "List.Number";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(List<string>) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ファイル名の配列を返します。
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
			if (src is FileNameList)
			{
				base.CopyFrom(src);

				var _src = (FileNameList)src;

				if (this.FileNames.Length != _src.FileNames.Length)
					this.FileNames = new string[_src.FileNames.Length];
				for (int i = 0; i < this.FileNames.Length; i++)
					this.FileNames[i] = _src.FileNames[i];

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
				var _src = (FileNameList)src;

				if (this.FileNames.Length != _src.FileNames.Length) return false;
				for (int i = 0; i < this.FileNames.Length; i++)
					if (this.FileNames[i] != _src.FileNames[i]) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// ファイル名のコレクション
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.FileNameList.FileNames")]
		[XmlArrayItem(typeof(string))]
		//[Editor(typeof(FileNamesEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public virtual string[] FileNames
		{
			get { return m_FileNames; }
			set { m_FileNames = value; }
		}
		private string[] m_FileNames = new string[0];

		#region FileNamesEditor

		private class FileNamesEditor : UITypeEditor
		{
			public FileNamesEditor()
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
				var parent = context.Instance as FileNameList;
				if (parent != null)
				{
					foreach (var filename in parent.FileNames)
					{
						if (!string.IsNullOrWhiteSpace(filename))
						{
							string dir = System.IO.Path.GetDirectoryName(filename);
							if (System.IO.Directory.Exists(dir))
							{
								base_dir = dir;
								break;
							}
						}
					}
				}

				// ダイアログ表示.
				var dlg = new OpenFileDialog();
				dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				dlg.Filter += "|All files (*.*)|*.*";
				dlg.Multiselect = true;
				if (string.IsNullOrWhiteSpace(base_dir) == false)
					dlg.InitialDirectory = base_dir;


				if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					return dlg.FileNames;
				}

				return base.EditValue(context, provider, value);
			}
		}

		#endregion

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
			this.DataOut[0].Data = new List<string>(this.FileNames);
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From<List<string>>()));
					for (int i = 0; i < this.FileNames.Length; i++)
					{
						var filename = this.FileNames[i];
						scope.Add(variable.Call("Add", CodeLiteral.From(filename)));
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
			// 現在のディレクトリ.
			string base_dir = "";
			var parent = this;
			if (parent != null)
			{
				foreach (var filename in parent.FileNames)
				{
					if (!string.IsNullOrWhiteSpace(filename))
					{
						string dir = System.IO.Path.GetDirectoryName(filename);
						if (System.IO.Directory.Exists(dir))
						{
							base_dir = dir;
							break;
						}
					}
				}
			}

			// ダイアログ表示.
			var dlg = new OpenFileDialog();
			dlg.Filter = "Image files|*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
			dlg.Filter += "|All files (*.*)|*.*";
			dlg.Multiselect = true;
			if (string.IsNullOrWhiteSpace(base_dir) == false)
				dlg.InitialDirectory = base_dir;

			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				parent.FileNames = dlg.FileNames;
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
}
