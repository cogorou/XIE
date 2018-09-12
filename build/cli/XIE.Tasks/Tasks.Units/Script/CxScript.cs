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
using System.Drawing.Design;
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
using System.CodeDom.Compiler;

namespace XIE.Tasks
{
	/// <summary>
	/// スクリプト基本クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxScript : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScript()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">言語種別</param>
		public CxScript(ExLanguageType type)
			: base()
		{
			this.LanguageType = type;
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Imports = ApiHelper.CreateImports();
			this.Category = "";
			this.Name = "Script";
			switch (this.LanguageType)
			{
				default:
				case ExLanguageType.CSharp:
					this.IconKey = "Lang-CS";
					break;
				case ExLanguageType.VisualBasic:
					this.IconKey = "Lang-VB";
					break;
			}

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxScript(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "LanguageType":
							this.LanguageType = this.GetValue<ExLanguageType>(info, entry.Name);
							break;
						case "Imports":
							this.Imports = this.GetValue<string[]>(info, entry.Name);
							break;
						case "Statement":
							this.Statement = this.GetValue<string>(info, entry.Name);
							break;
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
			}

			if (this.Imports == null)
				this.Imports = ApiHelper.CreateImports();
			if (this.Statement == null)
				this.Statement = "";
		}

		/// <summary>
		/// シリアライズ
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">シリアライズ先</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("LanguageType", this.LanguageType);
			info.AddValue("Imports", this.Imports);
			info.AddValue("Statement", this.Statement);
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			this.Dispose();
			base.CopyFrom(src);

			if (src is CxScript)
			{
				#region 同一型:
				var _src = (CxScript)src;

				this.LanguageType = _src.LanguageType;
				this.Statement = _src.Statement;

				this.Imports = new string[_src.Imports.Length];
				for (int i = 0; i < this.Imports.Length; i++)
					this.Imports[i] = _src.Imports[i];

				this.Results = null;
				this.Task = null;
				this.IsUpdated = true;

				return;
				#endregion
			}

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
			if (base.ContentEquals(src) == false) return false;

			if (src is CxScript)
			{
				var _src = (CxScript)src;

				if (this.LanguageType != _src.LanguageType) return false;
				if (this.Statement != _src.Statement) return false;

				if (this.Imports == null && _src.Imports != null) return false;
				if (this.Imports != null && _src.Imports == null) return false;
				if (this.Imports != null && _src.Imports != null)
				{
					for(int i=0 ; i<this.Imports.Length ; i++)
						if (this.Imports[i] != _src.Imports[i]) return false;
				}
				return true;
			}

			return false;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 言語種別
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxScript.LanguageType")]
		public ExLanguageType LanguageType
		{
			get { return m_LanguageType; }
			set { m_LanguageType = value; }
		}
		private ExLanguageType m_LanguageType = ExLanguageType.CSharp;

		/// <summary>
		/// 名前空間
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxScript.Imports")]
		[XmlArrayItem(typeof(string))]
		public virtual string[] Imports
		{
			get { return m_Imports; }
			set { m_Imports = value; }
		}
		private string[] m_Imports = new string[0];

		/// <summary>
		/// 構文
		/// </summary>
		[Browsable(false)]
		[Editor(typeof(StatementEditor), typeof(UITypeEditor))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxScript.Statement")]
		public string Statement
		{
			get { return m_Statement; }
			set { m_Statement = value; }
		}
		private string m_Statement = "";

		#region UITypeEditor (StatementEditor)

		/// <summary>
		/// 構文エディタ (プロパティグリッド用)
		/// </summary>
		class StatementEditor : UITypeEditor
		{
			/// <summary>
			/// コンストラクタ
			/// </summary>
			public StatementEditor()
			{
			}

			/// <summary>
			/// エディタが使用するエディタ スタイルの型をプロパティ ウィンドウに通知します。
			/// </summary>
			/// <param name="context"></param>
			/// <returns></returns>
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}

			/// <summary>
			/// ユーザー インターフェイス、ユーザーによる入力、および値の代入を処理します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="provider"></param>
			/// <param name="value"></param>
			/// <returns></returns>
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				var owner = (CxScriptEx)context.Instance;
				var dlg = new CxScriptEditorForm(owner);
				dlg.StartPosition = FormStartPosition.Manual;
				dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
				if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					try
					{
						owner.Build();
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
					return owner.Statement;
				}
				return base.EditValue(context, provider, value);
			}
		}

		#endregion

		#endregion

		#region コンパイラ:

		/// <summary>
		/// コンパイル
		/// </summary>
		/// <param name="type">言語種別</param>
		/// <param name="references">参照設定</param>
		/// <param name="imports">名前空間</param>
		/// <param name="statement">構文</param>
		/// <returns>
		///		コンパイル結果を返します。
		/// </returns>
		public virtual CompilerResults Compile(ExLanguageType type, CxReferencedAssembly[] references, string[] imports, string statement)
		{
			string target_ns = "Users";
			string target_type = "Script";
			var provider = this.GetProvider(type);
			var cc_param = new CompilerParameters();
			var cc_unit = new CodeCompileUnit();
			var refasm = this.GetReferencedAssemblies(references);

			#region コンパイラパラメータの設定:
			{
				cc_param.GenerateExecutable = false;
				cc_param.GenerateInMemory = true;
				cc_param.WarningLevel = 4;
				cc_param.TreatWarningsAsErrors = false;
				cc_param.ReferencedAssemblies.AddRange(refasm);

				#region DebugMode
				if (this.AuxInfo != null &&
					this.AuxInfo.DebugMode &&
					string.IsNullOrEmpty(this.AuxInfo.ProjectDir) == false)
				{
					// スクリプトをデバッグする為の設定です。
					// ビルドの度に一時ファイルが生成されることに注意してください。
					// 一時ファイルは下記ディレクトリ配下に生成されます。
					// Dir="%USERPROFILE%\Documents\XIE-Studio-1.0\TempFiles"

					cc_param.IncludeDebugInformation = true;
					string tempDir = System.IO.Path.Combine(this.AuxInfo.ProjectDir, "TempFiles");
					if (System.IO.Directory.Exists(tempDir) == false)
						System.IO.Directory.CreateDirectory(tempDir);
					cc_param.TempFiles = new TempFileCollection(tempDir, true);
				}
				else
				{
					cc_param.IncludeDebugInformation = false;
				}
				#endregion

				#region CompilerOptions
				cc_param.CompilerOptions = "/t:library /optimize";
				switch (type)
				{
					case XIE.Tasks.ExLanguageType.CSharp:
						cc_param.CompilerOptions += " /unsafe+";
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						break;
					default:
						throw new XIE.CxException(XIE.ExStatus.InvalidParam);
				}
				#endregion
			}
			#endregion

			#region コンパイルユニットの生成:
			{
				// 参照設定:
				cc_unit.ReferencedAssemblies.AddRange(refasm);

				// 名前空間:
				var code_ns = new CodeNamespace(target_ns);
				{
					foreach (string item in imports)
					{
						string name = item.Trim();
						if (name.Length > 0)
							code_ns.Imports.Add(new CodeNamespaceImport(name));
					}
				}

				// クラス:
				var code_type = new CodeTypeDeclaration(target_type);
				{
					code_type.BaseTypes.Add("XIE.Tasks.CxTaskUnit");
					code_type.CustomAttributes.Add(
						new CodeAttributeDeclaration(
							new CodeTypeReference(typeof(SerializableAttribute))
						));
					code_type.CustomAttributes.Add(
						new CodeAttributeDeclaration(
							new CodeTypeReference(typeof(TypeConverterAttribute)),
							new CodeAttributeArgument[]
							{
							new CodeAttributeArgument(new CodeTypeOfExpression(typeof(ExpandableObjectConverter)))
							}
						));
					code_type.IsPartial = true;
				}

				// コンストラクタ:
				{
					var method = new CodeConstructor();
					method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
					code_type.Members.Add(method);
				}

				// メソッド: (実行)
				{
					var method = new CodeMemberMethod();
					method.Name = "Execute";
					method.ReturnType = new CodeTypeReference(typeof(void));
					method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
					method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
					method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskExecuteEventArgs), "e"));
					method.Statements.Add(new CodeSnippetStatement(statement));
					code_type.Members.Add(method);
				}

				code_ns.Types.Add(code_type);
				cc_unit.Namespaces.Add(code_ns);
			}
			#endregion

			var results = provider.CompileAssemblyFromDom(cc_param, cc_unit);

			return results;
		}

		/// <summary>
		/// 言語種別に対応するプロバイダを取得します。
		/// </summary>
		/// <param name="type">言語種別</param>
		/// <returns>
		///		プロバイダを返します。
		/// </returns>
		public virtual CodeDomProvider GetProvider(ExLanguageType type)
		{
			var options = new Dictionary<string, string>();

			#region ProviderOptions
			try
			{
				// (!) var を使用したいだけなので .NET 4.0 以降では指定不要です.
				if (Environment.Version.Major == 2)
				{
					Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", false);
					object value = regkey.GetValue("Install");
					if (value is int && ((int)value) == 1)
					{
						options["CompilerVersion"] = "v3.5";
					}
				}
			}
			catch (System.Exception)
			{
			}
			#endregion

			switch (type)
			{
				case ExLanguageType.CSharp:
					return new Microsoft.CSharp.CSharpCodeProvider(options);
				case ExLanguageType.VisualBasic:
					return new Microsoft.VisualBasic.VBCodeProvider(options);
				default:
					throw new XIE.CxException(XIE.ExStatus.InvalidParam);
			}
		}

		/// <summary>
		/// 参照設定を取得します。
		/// </summary>
		/// <param name="references">参照設定</param>
		/// <returns>
		///		参照するアセンブリ名の一覧を返します。
		/// </returns>
		public virtual string[] GetReferencedAssemblies(IEnumerable<CxReferencedAssembly> references)
		{
			var result = new List<string>();

			foreach (var item in references)
			{
				Assembly asm = null;

				#region 厳密名があれば優先的に使用する.
				if (string.IsNullOrWhiteSpace(item.FullName) == false)
				{
					try
					{
						asm = Assembly.Load(item.FullName);

						// フルパスで参照する形式.
						result.Add(asm.Location);
					}
					catch (System.Exception)
					{
						// 一時的に GAC 登録解除している可能性を考慮する.
					}
				}
				#endregion

				#region ファイル名でロードを試みる.
				if (asm == null &&
					string.IsNullOrWhiteSpace(item.HintPath) == false &&
					System.IO.File.Exists(item.HintPath))
				{
					try
					{
						asm = Assembly.LoadFrom(item.HintPath);

						// ファイル名で参照する形式.
						result.Add(item.HintPath);
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion
			}

			return result.ToArray();
		}

		#endregion

		#region ビルド:

		/// <summary>
		/// 行番号のオフセット
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxScript.LineOffset")]
		public int LineOffset
		{
			get
			{
				return 32;
			}
		}

		/// <summary>
		/// コンパイル結果
		/// </summary>
		[NonSerialized]
		internal CompilerResults Results = null;

		/// <summary>
		/// タスク
		/// </summary>
		[NonSerialized]
		internal CxTaskUnit Task = null;

		/// <summary>
		/// 更新フラグ
		/// </summary>
		[NonSerialized]
		internal bool IsUpdated = false;

		/// <summary>
		/// ビルド
		/// </summary>
		/// <returns>
		///		コンパイルが正常に完了した場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Build()
		{
			this.Results = null;
			this.Task = null;
			this.IsUpdated = false;

			var references = ApiHelper.CreateReferences();

			this.Results = Compile(this.LanguageType, references, this.Imports, this.Statement);

			if (this.Results == null)
			{
				XIE.Log.Api.Error("Failed to compile.");
			}
			else if (this.Results.Errors.Count > 0)
			{
				XIE.Log.Api.Error("Failed to compile.");
				foreach (CompilerError item in this.Results.Errors)
				{
					int line_no = item.Line - this.LineOffset;
					string msg = string.Format(
						"({0},{1}): {2} {3} {4}\n",
						line_no,
						item.Column,
						(item.IsWarning) ? "Warning" : "Error",
						item.ErrorNumber,
						item.ErrorText
						);
					XIE.Log.Api.Error(msg);
				}
			}
			else
			{
				Assembly asm = this.Results.CompiledAssembly;
				Type[] types = asm.GetTypes();
				foreach (Type type in types)
				{
					if (typeof(CxTaskUnit).IsAssignableFrom(type))
					{
						this.Task = asm.CreateInstance(type.FullName) as CxTaskUnit;
						break;
					}
				}
			}

			return (this.Task != null);
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
			this.AuxInfo = e.AuxInfo;
		}
		[NonSerialized]
		private CxAuxInfo AuxInfo = null;

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

			// 実行:
			if (this.Task != null)
			{
				// 複製.
				this.Task.Category = this.Category;
				this.Task.Name = this.Name;
				this.Task.IconKey = this.IconKey;
				this.Task.IconImage = this.IconImage;
				this.Task.ControlIn = this.ControlIn;
				this.Task.ControlOut = this.ControlOut;
				this.Task.DataIn = this.DataIn;
				this.Task.DataParam = this.DataParam;
				this.Task.DataOut = this.DataOut;
				this.Task.Location = this.Location;
				this.Task.Breakpoint = this.Breakpoint;
				this.Task.Selected = this.Selected;
				// 実行.
				this.Task.Execute(sender, e);
			}
		}

		/// <summary>
		/// 処理開始前の準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Prepare(object sender, CxTaskExecuteEventArgs e)
		{
			// ビルド:
			if (IsUpdated == true || this.Task == null)
			{
				if (this.Build() == false)
					throw new System.InvalidProgramException();
			}
		}

		/// <summary>
		/// 処理終了後の復旧
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Restore(object sender, CxTaskExecuteEventArgs e)
		{
		}

		#endregion
	}

	/// <summary>
	/// スクリプト。任意の処理コードを直接記述できます。呼び出し元とのデータの受け渡しはデータ入出力ポートを介して行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxScriptEx : CxScript
		, ISerializable
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScriptEx()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">言語種別</param>
		public CxScriptEx(ExLanguageType type)
			: base()
		{
			this.LanguageType = type;
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "";
			this.Name = "Script";
			switch (this.LanguageType)
			{
				default:
				case ExLanguageType.CSharp:
					this.IconKey = "Lang-CS";
					break;
				case ExLanguageType.VisualBasic:
					this.IconKey = "Lang-VB";
					break;
			}

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxScriptEx(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					for(int i=0 ; i<this.DataParam.Length ; i++)
					{
						var key = string.Format("DataParam{0}", i);
						if (key == entry.Name)
						{
							var port = this.DataParam[i];
							if (port.Types.Length > 0 &&
								port.Types[0] != null &&
								port.Types[0].IsAssignableFrom(entry.ObjectType))
							{
								port.SubData = info.GetValue(entry.Name, entry.ObjectType);
								break;
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
			}
		}

		/// <summary>
		/// シリアライズ
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">シリアライズ先</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			for (int i = 0; i < this.DataParam.Length; i++)
			{
				var key = string.Format("DataParam{0}", i);
				var port = this.DataParam[i];
				if (port.CanSerializeSubData())
					info.AddValue(key, port.SubData);
			}
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: クラス定義
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		public virtual void GenerateTypeCode(object sender, CxGenerateCodeArgs e)
		{
			#region クラスのメンバ関数内のインデントの段数:
			// namespace XXXX
			// {
			//     class XXXXXX
			//     {
			//         public void XXXXXXX()
			//         {
			//             var xxxx = ###;
			// |___|___|___|
			// 0   1   2   3
			//         }
			//     }
			// }
			int method_indent_level;
			switch (e.LanguageType)
			{
				case ExLanguageType.CSharp:
				case ExLanguageType.VisualBasic:
				default:
					method_indent_level = 3;
					break;
			}
			#endregion

			#region クラス:
			var code_type = new CodeTypeDeclaration(e.ClassNames[this]);
			{
				code_type.Comments.Add(new CodeCommentStatement("<summary>", true));
				code_type.Comments.Add(new CodeCommentStatement(this.Name, true));
				code_type.Comments.Add(new CodeCommentStatement("</summary>", true));
				code_type.BaseTypes.Add("XIE.Tasks.CxTaskUnit");
				code_type.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(SerializableAttribute))
					));
				code_type.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(TypeConverterAttribute)),
						new CodeAttributeArgument[]
							{
							new CodeAttributeArgument(new CodeTypeOfExpression(typeof(ExpandableObjectConverter)))
							}
					));
				code_type.IsPartial = true;
			}
			#endregion

			#region コンストラクタ:
			{
				var member = new CodeConstructor();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.#ctor"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;

				// Category = "";
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("Category"), new CodePrimitiveExpression("")));
				// Name = "hogehoge";
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("Name"), new CodePrimitiveExpression(this.Name)));
				// Icon = "Relationship";
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("IconKey"), new CodePrimitiveExpression("Relationship")));

				// DataIn = new CxTaskPortIn[##];
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("DataIn"), new CodeArrayCreateExpression(typeof(CxTaskPortIn), this.DataIn.Length)));
				// DataIn[0] = new CxTaskPortIn("Src", typeof(XIE.CxImage));
				for (int i = 0; i < this.DataIn.Length; i++)
				{
					var port = this.DataIn[i];
					var dst = new CodeVariableReferenceExpression("DataIn");
					var index = new CodePrimitiveExpression(i);
					var src_type = new CodeExtraType(typeof(CxTaskPortIn));
					var src_args = new CodeExpression[1 + port.Types.Length];
					src_args[0] = new CodePrimitiveExpression(port.Name);
					for (int k = 0; k < port.Types.Length; k++)
						src_args[k + 1] = new CodeTypeOfExpression(port.Types[k]);
					var src_value = src_type.New(src_args);
					member.Statements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dst, index), src_value));
				}

				// DataOut = new CxTaskPortOut[##];
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("DataOut"), new CodeArrayCreateExpression(typeof(CxTaskPortOut), this.DataOut.Length)));
				// DataOut[0] = new CxTaskPortOut("Dst", typeof(XIE.CxImage));
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var port = this.DataOut[i];
					var dst = new CodeVariableReferenceExpression("DataOut");
					var index = new CodePrimitiveExpression(i);
					var src_type = new CodeExtraType(typeof(CxTaskPortOut));
					var src_args = new CodeExpression[1 + port.Types.Length];
					src_args[0] = new CodePrimitiveExpression(port.Name);
					for (int k = 0; k < port.Types.Length; k++)
						src_args[k + 1] = new CodeTypeOfExpression(port.Types[k]);
					var src_value = src_type.New(src_args);
					member.Statements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dst, index), src_value));
				}

				// DataParam = new CxTaskPortIn[##];
				member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("DataParam"), new CodeArrayCreateExpression(typeof(CxTaskPortIn), this.DataParam.Length)));
				// DataParam[0] = new CxTaskPortIn("Mode", typeof(int));
				for (int i = 0; i < this.DataParam.Length; i++)
				{
					var port = this.DataParam[i];
					var dst = new CodeVariableReferenceExpression("DataParam");
					var index = new CodePrimitiveExpression(i);
					var src_type = new CodeExtraType(typeof(CxTaskPortIn));
					var src_args = new CodeExpression[1 + port.Types.Length];
					src_args[0] = new CodePrimitiveExpression(port.Name);
					for (int k = 0; k < port.Types.Length; k++)
						src_args[k + 1] = new CodeTypeOfExpression(port.Types[k]);
					var src_value = src_type.New(src_args);
					member.Statements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(dst, index), src_value));
				}

				// Properties
				for (int i = 0; i < this.DataParam.Length; i++)
				{
					var port = this.DataParam[i];
					member.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(port.Name), CodeLiteral.From(port.Data)));
				}

				code_type.Members.Add(member);
			}
			#endregion

			#region メソッド: (デシリアライズ (シグネチャコンストラクタ))
			{
				var member = new CodeConstructor();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.#ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Attributes = MemberAttributes.Family | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(SerializationInfo), "info"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(StreamingContext), "context"));
				// -- base
				member.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("info"));
				member.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("context"));

				code_type.Members.Add(member);
			}
			#endregion

			#region メソッド: (シリアライズ)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "GetObjectData";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(SerializationInfo), "info"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(StreamingContext), "context"));
				member.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(SecurityPermissionAttribute)),
						new CodeAttributeArgument[]
							{
								new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(SecurityAction)), "Demand")),
								new CodeAttributeArgument("SerializationFormatter", new CodePrimitiveExpression(true))
							}
					));

				#region 基本クラスの呼び出し:
				{
					// base.GetObjectData(info, context);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeBaseReferenceExpression(),
							member.Name,
							new CodeExpression[]
								{
									new CodeArgumentReferenceExpression("info"),
									new CodeArgumentReferenceExpression("context")
								}
						));
				}
				#endregion

				code_type.Members.Add(member);
			}
			#endregion

			#region メソッド: (Execute)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.Execute(System.Object,XIE.Tasks.CxTaskExecuteEventArgs)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "Execute";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskExecuteEventArgs), "e"));

				#region 処理コード:
				if (e.LanguageType == this.LanguageType)
				{
					var textlines = this.Statement.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					foreach (var item in textlines)
					{
						member.Statements.Add(new CodeSnippetStatement(new string('	', method_indent_level) + item));
					}
				}
				else
				{
					member.Statements.Add(new CodeCommentStatement("Warning: Language type does not match."));
				}
				#endregion

				code_type.Members.Add(member);
			}
			#endregion

			#region プロパティ:
			for (int i = 0; i < this.DataParam.Length; i++)
			{
				var port = this.DataParam[i];
				var data_type = (port.Types.Length == 0) ? typeof(object) : port.Types[0];
				var data_port = new CodeArrayIndexerExpression(
					new CodeVariableReferenceExpression("DataParam"),
					new CodePrimitiveExpression(i)
					);
				var data_port_data = new CodePropertyReferenceExpression(data_port, "Data");
				var member = new CodeMemberProperty();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(port.Description, true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = port.Name;
				member.Type = new CodeTypeReference(data_type);
				member.Attributes = MemberAttributes.Public;
				// get
				{
					// return (ttt)DataParam[#];
					member.GetStatements.Add(
						new CodeMethodReturnStatement(new CodeCastExpression(data_type, data_port_data))
						);
				}
				// set
				{
					// new Primitive_Pass(value)
					var primitive_pass_type = new CodeExtraType(typeof(Primitive_Pass));
					var primitive_pass = primitive_pass_type.New(new CodePropertySetValueReferenceExpression());
					// DataParam[#].Connect(new Primitive_Pass(value), XIE.Tasks.ExOutputPortType.DataOut, 0);
					var data_port_Connect = new CodeMethodInvokeExpression(data_port, "Connect",
						primitive_pass,
						CodeLiteral.From(XIE.Tasks.ExOutputPortType.DataOut),
						CodeLiteral.From(0)
						);
					member.SetStatements.Add(data_port_Connect);
				}
				code_type.Members.Add(member);
			}
			#endregion

			e.TargetNS.Types.Add(code_type);
		}

		/// <summary>
		/// 指定のキーに該当する Summary を抽出します。
		/// </summary>
		/// <param name="key"></param>
		/// <returns>
		///		抽出した文字列を返します。
		///		該当する Summary がなければ空白を返します。
		/// </returns>
		private string GetSummary(string key)
		{
			try
			{
				return XIE.AxiTextStorage.Texts[key];
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message);
				return "";
			}
		}

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public override void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			base.GenerateDeclarationCode(sender, e, scope);

			if (e.TargetMethod.Name == "Execute")
			{
				var name = e.TaskNames[this];
				var variable = new CodeExtraVariable(string.Format("{0}", name), typeof(CxTaskUnit));

				var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, null);
				var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
				e.Variables[key] = value;

				scope.Add(variable.Declare(new CodeObjectCreateExpression(e.ClassNames[this])));
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

				var body = new CodeExtraVariable(e.GetVariable(this, null));

				#region body.DataIn[index].Connect( new Primitive_Pass(task$_This), XIE.Tasks.ExOutputPortType.DataOut, 0 );
				for (int i = 0; i < this.DataIn.Length; i++)
				{
					if (this.DataIn[i].IsConnected)
					{
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[i]));

						var pass = new CodeExtraVariable("pass", typeof(Primitive_Pass));
						var pass_new = pass.New(src);
						var pass_type = CodeLiteral.From(XIE.Tasks.ExOutputPortType.DataOut);
						var pass_index = CodeLiteral.From(0);

						var port = new CodeIndexerExpression(body.Ref("DataIn"), CodeLiteral.From(i));

						scope.Add(new CodeMethodInvokeExpression(port, "Connect", pass_new, pass_type, pass_index));
					}
				}
				#endregion

				#region body.DataParam[index].Connect( new Primitive_Pass(task$_This), XIE.Tasks.ExOutputPortType.DataOut, 0 );
				for (int i = 0; i < this.DataParam.Length; i++)
				{
					if (this.DataParam[i].IsConnected)
					{
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[i]));

						var pass = new CodeExtraVariable("pass", typeof(Primitive_Pass));
						var pass_new = pass.New(src);
						var pass_type = CodeLiteral.From(XIE.Tasks.ExOutputPortType.DataOut);
						var pass_index = CodeLiteral.From(0);

						var port = new CodeIndexerExpression(body.Ref("DataParam"), CodeLiteral.From(i));

						scope.Add(new CodeMethodInvokeExpression(port, "Connect", pass_new, pass_type, pass_index));
					}
				}
				#endregion

				// body.Execute(this, new CxTaskExecuteEventArgs());
				{
					var body_sender = new CodeThisReferenceExpression();
					var body_args = new CodeExtraVariable("args", typeof(CxTaskExecuteEventArgs));
					scope.Add(body.Call("Execute", body_sender, body_args.New()));
				}

				#region task#_Data = (Xxxxxx)body.DataOut[index].Data;
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));

					var port = new CodeIndexerExpression(body.Ref("DataOut"), CodeLiteral.From(i));
					var port_data = new CodePropertyReferenceExpression(port, "Data");
					var cast_data = new CodeCastExpression(dst.Type, port_data);

					scope.Add(dst.Assign(cast_data));
				}
				#endregion
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
			var dlg = new CxScriptEditorForm(this);
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				this.Build();
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

		#region SelfConverter:

		/// <summary>
		/// 型変換クラス
		/// </summary>
		public class SelfConverter : CxSortingConverter
		{
			/// <summary>
			/// 指定されたデータ型に対して公開されているプロパティのコレクションの取得
			/// </summary>
			/// <param name="context"></param>
			/// <param name="value"></param>
			/// <param name="attributes"></param>
			/// <returns>
			///		公開されているプロパティと DataParam, DataOut のコレクションを返します。
			/// </returns>
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				var items = base.GetProperties(context, value, attributes);
				var names = new List<string>();
				foreach (var info in value.GetType().GetProperties())
					names.Add(info.Name);

				var collection = items.Sort(names.ToArray());
				var self = context.Instance as CxTaskUnit;
				if (self != null)
				{
					foreach (var port in self.DataIn)
					{
						var pd = new CxTaskPortInPropertyDescriptor("Inputs", true, port, new Attribute[0]);
						collection.Add(pd);
					}
					foreach (var port in self.DataOut)
					{
						var pd = new CxTaskPortOutPropertyDescriptor("Outputs", true, port, new Attribute[0]);
						collection.Add(pd);
					}
					foreach (var port in self.DataParam)
					{
						var pd = new CxTaskPortInPropertyDescriptor("Parameters", false, port, new Attribute[0]);
						collection.Add(pd);
					}
				}
				return collection;
			}
		}

		#endregion
	}
}
