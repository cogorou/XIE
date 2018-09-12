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
	// String
	// 

	#region String.ctor

	/// <summary>
	/// 文字列
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class String_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public String_ctor()
			: base()
		{
			this.Category = "System";
			this.Name = "String";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(String) })
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
			if (src is String_ctor)
			{
				base.CopyFrom(src);

				var _src = (String_ctor)src;

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
				var _src = (String_ctor)src;

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
		[CxDescription("P:XIE.Tasks.String_ctor.This")]
		public String This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private String m_This = "";

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

	#region String.GetProperty

	/// <summary>
	/// 文字列のプロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class String_GetProperty : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public String_GetProperty()
			: base()
		{
			_Construtor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="item_name">取得するプロパティの名称</param>
		public String_GetProperty(string item_name)
			: base()
		{
			this.ItemName = item_name;
			_Construtor();
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Construtor()
		{
			this.Category = "System.String";
			this.Name = string.IsNullOrWhiteSpace(this.ItemName) ? "GetProperty" : this.ItemName;
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(String) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Value", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 取得した値
			/// </summary>
			DataOut0,
		}

		private enum ItemNames
		{
			/// <summary>
			/// 文字列の長さを取得します。
			/// </summary>
			Length,
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
			if (src is String_GetProperty)
			{
				base.CopyFrom(src);

				var _src = (String_GetProperty)src;

				this.Body = _src.Body;
				this.ItemName = _src.ItemName;

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
				var _src = (String_GetProperty)src;

				if (this.Body != _src.Body) return false;
				if (this.ItemName != _src.ItemName) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 文字列
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.String_GetProperty.Body")]
		public String Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private String m_Body = "";

		/// <summary>
		/// 取得するプロパティの名称
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.String_GetProperty.ItemName")]
		public string ItemName
		{
			get { return m_ItemName; }
			set { m_ItemName = value; }
		}
		private string m_ItemName = "";

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 取得した値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.String_GetProperty.Value")]
		public string Value
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[0].Data;
				if (data == null) return "";
				return string.Format("{0}", data);
			}
		}

		/// <summary>
		/// 取得した値の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.String_GetProperty.Type")]
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

			// 引数の取得.
			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Body = (String)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			if (string.IsNullOrWhiteSpace(this.ItemName) == false)
			{
				var info = this.Body.GetType().GetProperty(this.ItemName);
				if (info != null)
					this.DataOut[0].Data = info.GetValue(this.Body, null);
			}
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), (port.Data == null) ? typeof(object) : port.Data.GetType());

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
						scope.Add(variable.Declare(CodeLiteral.From(this.DataOut[i].Data)));
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

					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

					if (string.IsNullOrWhiteSpace(this.ItemName) == false)
					{
						// task#_X = task$.X;
						scope.Add(dst.Assign(src.Ref(this.ItemName)));
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
				string reskey = (string.IsNullOrWhiteSpace(this.ItemName))
					? this.DescriptionKey
					: string.Format("F:{0}.{1}.{2}", this.GetType().FullName, "ItemNames", this.ItemName);
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

	// //////////////////////////////////////////////////
	// Encoding
	// 

	#region Encoding.ctor

	/// <summary>
	/// 指定されたエンコーディングを構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Encoding_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Encoding_ctor()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "System.Text";
			this.Name = "Encoding";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Encoding) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// オブジェクトを構築して返します。
			/// </summary>
			DataOut0,
		}

		public enum CodePageNames
		{
			/// <summary>
			/// 現在の ANSI コード ページのエンコーディングを構築します。
			/// </summary>
			Default,

			/// <summary>
			/// ASCII (7 ビット) 文字セットのエンコーディングを構築します。
			/// </summary>
			ASCII,

			/// <summary>
			/// UTF-8 形式のエンコーディングを構築します。
			/// </summary>
			UTF8,

			/// <summary>
			/// UTF-7 形式のエンコーディングを構築します。
			/// </summary>
			UTF7,

			/// <summary>
			/// リトル エンディアン バイト順を使用する UTF-32 形式のエンコーディングを構築します。
			/// </summary>
			UTF32,

			/// <summary>
			/// リトル エンディアン バイト順を使用する UTF-16 形式のエンコーディングを構築します。
			/// </summary>
			Unicode,

			/// <summary>
			/// ビッグ エンディアンのバイト順を使用する UTF-16 形式のエンコーディングを構築します。
			/// </summary>
			BigEndianUnicode,
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
			if (src is Encoding_ctor)
			{
				base.CopyFrom(src);

				var _src = (Encoding_ctor)src;

				this.CodePageName = _src.CodePageName;

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
				var _src = (Encoding_ctor)src;

				if (this.CodePageName != _src.CodePageName) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// コードページを示す名称
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Encoding_ctor.CodePageName")]
		public CodePageNames CodePageName
		{
			get { return m_CodePageName; }
			set { m_CodePageName = value; }
		}
		private CodePageNames m_CodePageName = CodePageNames.Default;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 結果
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Encoding_ctor.This")]
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
		[CxDescription("P:XIE.Tasks.Encoding_ctor.Type")]
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
			var info = typeof(Encoding).GetProperty(this.CodePageName.ToString(), BindingFlags.Public | BindingFlags.Static);
			this.DataOut[0].Data = info.GetValue(null, null);
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

				var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

				// task#_This = Encoding.Xxxx;
				var encoding = new CodeExtraType(typeof(Encoding));
				scope.Add(dst.Assign(encoding.Ref(this.CodePageName.ToString())));
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
				var value = "";
				{
					value += XIE.AxiTextStorage.GetValue(key);
				}
				//{
				//	var reskey = string.Format("F:{0}.{1}.{2}", this.GetType().FullName, "CodePageNames", this.CodePageName);
				//	value += "\n";
				//	value += string.Format("{0}: {1}", this.CodePageName, XIE.AxiTextStorage.GetValue(reskey));
				//}
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

	#region Encoding.GetBytes

	/// <summary>
	/// GetBytes メソッド。指定した文字列に含まれるすべての文字をバイトシーケンスにエンコードします。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Encoding_GetBytes : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Encoding_GetBytes()
			: base()
		{
			this.Category = "System.Text.Encoding";
			this.Name = "GetBytes";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(Encoding)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Text", new Type[] {typeof(string)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(byte[])})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// エンコーディング
			/// </summary>
			DataIn0,

			/// <summary>
			/// 変換元の文字列
			/// </summary>
			DataParam0,

			/// <summary>
			/// 変換後のバイト配列
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
			if (src is Encoding_GetBytes)
			{
				base.CopyFrom(src);

				var _src = (Encoding_GetBytes)src;

				this.Text = _src.Text;

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
				var _src = (Encoding_GetBytes)src;

				if (this.Text != _src.Text) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 変換元の文字列
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Encoding_GetBytes.Text")]
		public string Text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}
		private string m_Text = "";

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 変換後のバイト配列
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Encoding_GetBytes.This")]
		public byte[] This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private byte[] m_This = new byte[0];

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
			var src = (Encoding)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Text = (string)this.DataParam[iii].Data; break;
					}
				}
			}

			// 実行.
			this.This = src.GetBytes(this.Text);

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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var text = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Text));

					// task#_This = task$.GetBytes(text);
					scope.Add(result.Assign(body.Call("GetBytes", text)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Encoding.GetString

	/// <summary>
	/// GetString メソッド。指定したバイト配列に格納されているすべてのバイトを文字列にデコードします。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Encoding_GetString : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Encoding_GetString()
			: base()
		{
			this.Category = "System.Text.Encoding";
			this.Name = "GetString";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(Encoding)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Bytes", new Type[] {typeof(byte[])}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] {typeof(string)})
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// エンコーディング
			/// </summary>
			DataIn0,

			/// <summary>
			/// 変換元のバイト配列
			/// </summary>
			DataParam0,

			/// <summary>
			/// 変換後の文字列
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
			if (src is Encoding_GetString)
			{
				base.CopyFrom(src);

				var _src = (Encoding_GetString)src;

				this.Bytes = new byte[_src.Bytes.Length];
				for (int i = 0; i < this.Bytes.Length; i++)
					this.Bytes[i] = _src.Bytes[i];

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
				var _src = (Encoding_GetString)src;

				if (this.Bytes.Length != _src.Bytes.Length) return false;
				for (int i = 0; i < this.Bytes.Length; i++)
					if (this.Bytes[i] != _src.Bytes[i]) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 変換元のバイト配列
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Encoding_GetString.Bytes")]
		[XmlArrayItem(typeof(byte))]
		public byte[] Bytes
		{
			get { return m_Bytes; }
			set { m_Bytes = value; }
		}
		private byte[] m_Bytes = new byte[0];

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 変換後の文字列
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Encoding_GetString.This")]
		public string This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		[NonSerialized]
		private string m_This = "";

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
			var src = (Encoding)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Bytes = (byte[])this.DataParam[iii].Data; break;
					}
				}
			}

			// 実行.
			this.This = src.GetString(this.Bytes);

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
					var body = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					if (this.DataParam[0].IsConnected)
					{
						// parameters
						var bytes = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_This = task$.GetString(bytes);
						scope.Add(result.Assign(body.Call("GetString", bytes)));
					}
					else
					{
						// new byte[##] { 11, 22,33 };
						var items = new CodeExpression[this.Bytes.Length];
						for (int i = 0; i < items.Length; i++)
							items[i] = CodeLiteral.From(this.Bytes[i]);
						var bytes = new CodeArrayCreateExpression(typeof(byte), items);

						// task#_This = task$.GetString(bytes);
						scope.Add(result.Assign(body.Call("GetString", bytes)));
					}
				}
			}
		}

		#endregion
	}

	#endregion
}
