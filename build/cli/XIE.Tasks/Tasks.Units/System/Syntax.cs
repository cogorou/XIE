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
	// Syntax
	// 

	#region Syntax.Class

	/// <summary>
	/// クラス。ダイアグラムを階層化して処理を記述できます。呼び出し元とのデータの受け渡しはデータ入出力ポートを介して行います。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class Syntax_Class : CxTaskflow
		, IxTaskControlPanel
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Class()
			: base()
		{
			this.Name = "Class";
			this.IconKey = "Service-Connect";

			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="iconKey"></param>
		public Syntax_Class(string name, string iconKey)
			: base(name, iconKey)
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "Syntax";

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
		protected Syntax_Class(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					for (int i = 0; i < this.DataParam.Length; i++)
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

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Syntax_Class)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Class)src;

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
				var _src = (Syntax_Class)src;
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

			base.Execute(sender, e);
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
			// このスコープ内の全てのタスク.
			var all_tasks = this.GetTasksInScope(true);

			// タスク名リスト.
			var tasknames = new Dictionary<XIE.Tasks.CxTaskUnit, string>();
			for (int i = 0; i < all_tasks.Count; i++)
				tasknames[all_tasks[i]] = string.Format("task{0}", i);

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

				code_type.Members.Add(member);

				#region 処理コード:
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
				}
				#endregion
			}
			#endregion

			#region メソッド: (デシリアライズ (シグネチャコンストラクタ))
			if (1 <= SharedData.CodeGenerationLevel)
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
			if (1 <= SharedData.CodeGenerationLevel)
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

			#region メソッド: (Setup)
			if (1 <= SharedData.CodeGenerationLevel)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.Setup(System.Object,XIE.Tasks.CxTaskSetupEventArgs)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "Setup";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskSetupEventArgs), "e"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskflow), "parent"));
				code_type.Members.Add(member);

				#region 処理コード:
				{
					// base.Setup(sender, e, parent);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeBaseReferenceExpression(),
							"Setup",
							new CodeExpression[]
							{
								new CodeArgumentReferenceExpression("sender"),
								new CodeArgumentReferenceExpression("e"),
								new CodeArgumentReferenceExpression("parent"),
							}
						));

					// イベント引数:
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
				}
				#endregion
			}
			#endregion

			#region メソッド: (Prepare)
			if (1 <= SharedData.CodeGenerationLevel)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.Prepare(System.Object,XIE.Tasks.CxTaskExecuteEventArgs)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "Prepare";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskExecuteEventArgs), "e"));
				code_type.Members.Add(member);

				#region 処理コード:
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
				}
				#endregion
			}
			#endregion

			#region メソッド: (Restore)
			if (1 <= SharedData.CodeGenerationLevel)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.Restore(System.Object,XIE.Tasks.CxTaskExecuteEventArgs)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "Restore";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskExecuteEventArgs), "e"));

				#region 処理コード:
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
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
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 変数宣言:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateDeclarationCode(this, args, member.Statements);
					}

					// 処理:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateProcedureCode(this, args, member.Statements);
					}
				}
				#endregion

				code_type.Members.Add(member);
			}
			#endregion

			#region メソッド: (ValueChanged)
			if (1 <= SharedData.CodeGenerationLevel)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.ValueChanged(System.Object,XIE.Tasks.CxTaskValueChangedEventArgs)"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "ValueChanged";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Tasks.CxTaskValueChangedEventArgs), "e"));

				#region 基本クラスの呼び出し:
				{
					// base.ValueChanged(sender, e);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeBaseReferenceExpression(),
							member.Name,
							new CodeExpression[]
								{
									new CodeArgumentReferenceExpression("sender"),
									new CodeArgumentReferenceExpression("e")
								}
						));
				}
				#endregion

				#region 処理コード:
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
				}
				#endregion

				code_type.Members.Add(member);
			}
			#endregion

			#region メソッド: (Reset)
			if (1 <= SharedData.CodeGenerationLevel)
			{
				var member = new CodeMemberMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement(GetSummary("M:XIE.Tasks.CxTaskUnit.Reset"), true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));
				member.Name = "Reset";
				member.ReturnType = new CodeTypeReference(typeof(void));
				member.Attributes = MemberAttributes.Public | MemberAttributes.Override;

				#region 基本クラスの呼び出し:
				{
					// base.Reset();
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeBaseReferenceExpression(),
							member.Name
						));
				}
				#endregion

				#region 処理コード:
				{
					var args = (CxGenerateCodeArgs)e.Clone();
					args.IndentLevel = method_indent_level;
					args.TargetType = code_type;
					args.TargetMethod = member;
					args.TaskNames = tasknames;

					// 処理:
					foreach (var task in this.TaskUnits)
						task.GenerateProcedureCode(this, args, member.Statements);
				}
				#endregion

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
			var dlg = new XIE.Tasks.CxTaskPortEditForm(this);
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				#region タスクユニットのアイコンを初期化する.
				try
				{
					int ci = this.DataIn.Length + this.DataParam.Length;
					int co = this.DataOut.Length;

					string icon = this.IconKey;
					if (ci > 0 && co > 0)
						icon = "Service-Port";
					else if (ci > 0)
						icon = "Service-PortIn";
					else if (co > 0)
						icon = "Service-PortOut";
					else
						icon = "Service-Connect";
					this.IconKey = icon;
					this.IconImage = SharedData.Icons16.ImageList.Images[icon];
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
				#endregion
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
			///		公開されているプロパティと DataParam のコレクションを返します。
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

	#endregion

	#region Syntax.Scope

	/// <summary>
	/// スコープ。ダイアグラムを階層化して処理を記述できます。但し、If とは異なり条件式はありません。ソースコード生成では try/finally で表現されます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_Scope : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Scope()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Scope";
			this.IconKey = "Unit-Scope";

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
		protected Syntax_Scope(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_Scope)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Scope)src;

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
				var _src = (Syntax_Scope)src;
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

			base.Execute(sender, e);
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
				e.IndentLevel++;

				var body_statement = new CodeTryCatchFinallyStatement();

				#region try:
				{
					var this_scope = body_statement.TryStatements;

					// 変数宣言:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateDeclarationCode(this, e, this_scope);
					}

					// 処理:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateProcedureCode(this, e, this_scope);
					}
				}
				#endregion

				#region finally:
				{
					var this_scope = body_statement.FinallyStatements;
					this_scope.Add(new CodeSnippetStatement());
				}
				#endregion

				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));
				scope.Add(body_statement);

				e.IndentLevel--;
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.If

	/// <summary>
	/// 条件文（if）。入力ポート(Condition)に指定された値が true の場合に処理を実行します。[関連: System.Operations.Condition]
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_If : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_If() : base()
		{
			this.Category = "Syntax";
			this.Name = "If";
			this.IconKey = "Chart.Org";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Condition", new Type[] { typeof(bool) })
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
			/// 真理値。[true=実行、false=無視]
			/// </summary>
			DataIn0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_If(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_If)
			{
				base.CopyFrom(src);

				var _src = (Syntax_If)src;

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
				var _src = (Syntax_If)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 条件
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_If.Condition")]
		public bool Condition
		{
			get { return m_Condition; }
			private set { m_Condition = value; }
		}
		private bool m_Condition = false;

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
			this.Condition = false;
			this.DataIn[0].CheckValidity(true);

			this.Condition = Convert.ToBoolean(this.DataIn[0].Data);
			if (this.Condition)
				base.Execute(sender, e);
			this.ControlOut.Data = this.Condition;
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

				var if1 = new CodeConditionStatement();
				scope.Add(if1);

				// if: condition
				if1.Condition = ApiHelper.CodeOptionalExpression(e, this.DataIn[0], CodeLiteral.From(false));

				// if: statements
				{
					e.IndentLevel++;

					// 変数宣言:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateDeclarationCode(this, e, if1.TrueStatements);
					}

					// 処理:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateProcedureCode(this, e, if1.TrueStatements);
					}

					e.IndentLevel--;
				}

				// else: statements
				foreach (var item in e.TaskNames)
				{
					var task = item.Key;
					if (task is Syntax_ElseIf ||
						task is Syntax_Else)
					{
						if (task.ControlIn.IsConnected && task.ControlIn.ReferenceTask == this)
						{
							task.GenerateProcedureCode(this, e, if1.FalseStatements);
							break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.ElseIf

	/// <summary>
	/// 条件文（else if）。上流の If または ElseIf の真理値が false であり、自身の入力ポート(Condition)に指定された値が true の場合に処理を実行します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_ElseIf : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_ElseIf()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "ElseIf";
			this.IconKey = "Chart.Org";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Condition", new Type[] { typeof(bool) })
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
			/// 真理値。[true=実行、false=無視]
			/// </summary>
			DataIn0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_ElseIf(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_ElseIf)
			{
				base.CopyFrom(src);

				var _src = (Syntax_ElseIf)src;

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
				var _src = (Syntax_ElseIf)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 条件
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_ElseIf.Condition")]
		public bool Condition
		{
			get { return m_Condition; }
			private set { m_Condition = value; }
		}
		private bool m_Condition = false;

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
			this.Condition = false;
			this.ControlIn.CheckValidity(true);
			this.DataIn[0].CheckValidity(true);
			if (this.ControlIn.ReferenceTask is Syntax_If ||
				this.ControlIn.ReferenceTask is Syntax_ElseIf)
			{
				// 前方で例外が発生した場合は処理を行わない.
				if (this.ControlIn.Data != null)
				{
					if (Convert.ToBoolean(this.ControlIn.Data))
					{
						// 上流の If/ElseIf が真の時:
						this.Condition = false;
						this.ControlOut.Data = true;	// 後続の Else の実行を禁止する.
					}
					else
					{
						// 上流の If/ElseIf が偽の時:
						this.Condition = Convert.ToBoolean(this.DataIn[0].Data);
						if (this.Condition)
							base.Execute(sender, e);
						this.ControlOut.Data = this.Condition;
					}
				}
			}
			else
			{
				throw new ArgumentException("ControlIn must be If or ElseIf.");
			}
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
				if (this.ControlIn.IsConnected && this.ControlIn.ReferenceTask == sender)
				{
					#region if: condition
					var condition = ApiHelper.CodeOptionalExpression(e, this.DataIn[0], CodeLiteral.From(false));
					#endregion

					#region if: statements
					CodeStatement[] true_statements;
					{
						var true_scope = new CodeStatementCollection();

						e.IndentLevel++;

						// 変数宣言:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateDeclarationCode(this, e, true_scope);
						}

						// 処理:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateProcedureCode(this, e, true_scope);
						}

						e.IndentLevel--;

						true_statements = new CodeStatement[true_scope.Count];
						if (true_scope.Count > 0)
							true_scope.CopyTo(true_statements, 0);
					}
					#endregion

					#region else: statements
					CodeStatement[] else_statements = null;
					{
						foreach (var item in e.TaskNames)
						{
							var task = item.Key;
							if (task is Syntax_ElseIf ||
								task is Syntax_Else)
							{
								if (task.ControlIn.IsConnected && task.ControlIn.ReferenceTask == this)
								{
									var this_scope = new CodeStatementCollection();

									task.GenerateProcedureCode(this, e, this_scope);

									else_statements = new CodeStatement[this_scope.Count];
									if (this_scope.Count > 0)
										this_scope.CopyTo(else_statements, 0);

									break;
								}
							}
						}
					}
					#endregion

					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));
					if (else_statements == null)
						scope.Add(new CodeConditionStatement(condition, true_statements));
					else
						scope.Add(new CodeConditionStatement(condition, true_statements, else_statements));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.Else

	/// <summary>
	/// 条件文（else）。上流の If または ElseIf の真理値が false の場合に処理を実行します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_Else : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Else()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Else";
			this.IconKey = "Chart.Org";

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
		protected Syntax_Else(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_Else)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Else)src;

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
				var _src = (Syntax_Else)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 条件
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_Else.Condition")]
		public bool Condition
		{
			get { return m_Condition; }
			private set { m_Condition = value; }
		}
		private bool m_Condition = false;

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
			this.Condition = false;
			this.ControlIn.CheckValidity(true);
			if (this.ControlIn.ReferenceTask is Syntax_If ||
				this.ControlIn.ReferenceTask is Syntax_ElseIf)
			{
				// 前方で例外が発生した場合は処理を行わない.
				if (this.ControlIn.Data != null)
				{
					// 上流の If/ElseIf が何れも偽の時は、自身が真になる.
					this.Condition = !(Convert.ToBoolean(this.ControlIn.Data));
					if (this.Condition)
						base.Execute(sender, e);
				}
			}
			else
			{
				throw new ArgumentException("ControlIn must be If or ElseIf.");
			}
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
				if (this.ControlIn.IsConnected && this.ControlIn.ReferenceTask == sender)
				{
					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

					#region else: statements
					{
						e.IndentLevel++;

						// 変数宣言:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateDeclarationCode(this, e, scope);
						}

						// 処理:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateProcedureCode(this, e, scope);
						}

						e.IndentLevel--;
					}
					#endregion
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.For

	/// <summary>
	/// 反復処理（for）。次に示す順序 [1~4] で処理を繰り返します。［順序： １．指標の初期化（Index＝Initial）、２．継続条件（Index＜Limit）の評価、３．処理の実行、４．指標の更新（Index＋Step）］
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_For : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_For()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "For";
			this.IconKey = "List.Number";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Initial", new Type[] { typeof(int) }),
				new CxTaskPortIn("Limit", new Type[] { typeof(int) }),
				new CxTaskPortIn("Step", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Index", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 初期値。指標の初期値を示します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// 上限値（または下限値）。指標がこの値に到達するまで繰り返します。
			/// </summary>
			DataParam1,

			/// <summary>
			/// 増分（または減分）。
			/// </summary>
			DataParam2,

			/// <summary>
			/// 指標。繰り返し処理が停止した際の指標を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_For(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "Initial":
							this.Initial = this.GetValue<int>(info, entry.Name);
							break;
						case "Limit":
							this.Limit = this.GetValue<int>(info, entry.Name);
							break;
						case "Step":
							this.Step = this.GetValue<int>(info, entry.Name);
							break;
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

			info.AddValue("Initial", this.Initial);
			info.AddValue("Limit", this.Limit);
			info.AddValue("Step", this.Step);
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
			if (src is Syntax_For)
			{
				base.CopyFrom(src);

				var _src = (Syntax_For)src;

				this.Initial = _src.Initial;
				this.Limit = _src.Limit;
				this.Step = _src.Step;

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
				var _src = (Syntax_For)src;

				if (this.Initial != _src.Initial) return false;
				if (this.Limit != _src.Limit) return false;
				if (this.Step != _src.Step) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 初期値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Syntax_For.Initial")]
		public int Initial
		{
			get { return m_Initial; }
			set { m_Initial = value; }
		}
		private int m_Initial = 0;

		/// <summary>
		/// 上限または下限
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Syntax_For.Limit")]
		public int Limit
		{
			get { return m_Limit; }
			set { m_Limit = value; }
		}
		private int m_Limit = 1;

		/// <summary>
		/// 増分 [±N] ※ 0 は実行時にエラーになります。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Syntax_For.Step")]
		public int Step
		{
			get { return m_Step; }
			set { m_Step = value; }
		}
		private int m_Step = 1;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 指標
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_For.Index")]
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}
		private int m_Index = 0;

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
						case 0: this.Initial = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Limit = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Step = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			if (this.Step > 0)
			{
				for (this.Index = this.Initial; this.Index < this.Limit; this.Index += this.Step)
				{
					#region 子タスクの初期化:
					var tasks = this.GetTasks(true);
					foreach (var task in tasks)
					{
						if (task is Syntax_For_Index)
						{
							var child = (Syntax_For_Index)task;
							child.This = this.Index;
						}
					}
					#endregion

					base.Execute(sender, e);

					#region 制御構文:
					if (e.ControlSyntax == ExControlSyntax.Continue)
					{
						e.ControlSyntax = ExControlSyntax.None;
						continue;
					}
					if (e.ControlSyntax == ExControlSyntax.Break)
					{
						e.ControlSyntax = ExControlSyntax.None;
						break;
					}
					if (e.ControlSyntax == ExControlSyntax.Return) break;
					#endregion
				}
				this.DataOut[0].Data = this.Index;

				return;
			}
			if (this.Step < 0)
			{
				for (this.Index = this.Initial; this.Index > this.Limit; this.Index += this.Step)
				{
					#region 子タスクの初期化:
					var tasks = this.GetTasks(true);
					foreach (var task in tasks)
					{
						if (task is Syntax_For_Index)
						{
							var child = (Syntax_For_Index)task;
							child.This = this.Index;
						}
					}
					#endregion

					base.Execute(sender, e);

					#region 制御構文:
					if (e.ControlSyntax == ExControlSyntax.Continue)
					{
						e.ControlSyntax = ExControlSyntax.None;
						continue;
					}
					if (e.ControlSyntax == ExControlSyntax.Break)
					{
						e.ControlSyntax = ExControlSyntax.None;
						break;
					}
					if (e.ControlSyntax == ExControlSyntax.Return) break;
					#endregion
				}
				this.DataOut[0].Data = this.Index;

				return;
			}

			throw new ArgumentException("Step must be not zero.");
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
						target_port.Data = Convert.ToInt32(value);
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
				var index = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

				e.TargetIterations.Add(this);
				e.IndentLevel++;

				var debug_write = new CodeExpressionStatement(
					new CodeExtraType(typeof(System.Diagnostics.Debug)).Call("WriteLine", CodeLiteral.From(""))
					);
				var label_Continue = new CodeLabeledStatement(string.Format("{0}_Continue", e.TaskNames[this]));
				var label_Break = new CodeLabeledStatement(string.Format("{0}_Break", e.TaskNames[this]));

				#region for:
				CodeStatement initial_statement;
				{
					var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Initial));
					initial_statement = index.Assign(value);
				}
				CodeExpression limit_expression;
				{
					var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Limit));
					limit_expression = (this.Step > 0)
						? index.LessThan(value)
						: index.GreaterThan(value);
				}
				CodeStatement step_statement;
				{
					var value = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Step));
					step_statement = (this.Step > 0)
						? index.Assign(index.Add(value))
						: index.Assign(index.Subtract(value));
				}
				#endregion

				#region for: body_statements
				CodeStatement[] body_statements;
				{
					var this_scope = new CodeStatementCollection();

					// 変数宣言:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateDeclarationCode(this, e, this_scope);
					}

					// 処理:
					foreach (var task in this.TaskUnits)
					{
						task.GenerateProcedureCode(this, e, this_scope);
					}

					#region Continue Label:
					switch (e.LanguageType)
					{
						case ExLanguageType.CSharp:
							// C# の場合は continue; で継続する.
							break;
						case ExLanguageType.VisualBasic:
							this_scope.Add(new CodeSnippetStatement());
							this_scope.Add(label_Continue);
							break;
						default:
							label_Continue.Statement = debug_write;
							this_scope.Add(new CodeSnippetStatement());
							this_scope.Add(label_Continue);
							break;
					}
					#endregion

					body_statements = new CodeStatement[this_scope.Count];
					if (this_scope.Count > 0)
						this_scope.CopyTo(body_statements, 0);
				}
				#endregion

				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));
				scope.Add(new CodeIterationStatement(initial_statement, limit_expression, step_statement, body_statements));

				#region Break Label:
				switch (e.LanguageType)
				{
					case ExLanguageType.CSharp:
						// C# の場合は break; で中断する.
						break;
					case ExLanguageType.VisualBasic:
						// VB の場合は Exit Do で中断する.
						break;
					default:
						label_Break.Statement = debug_write;
						scope.Add(label_Break);
						break;
				}
				#endregion

				e.IndentLevel--;
				e.TargetIterations.Remove(this);
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.For.Index

	/// <summary>
	/// 反復処理（for）の指標。[関連: System.Syntax.For] 
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_For_Index : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_For_Index()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Index";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 現在の指標を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_For_Index(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_For_Index)
			{
				base.CopyFrom(src);

				var _src = (Syntax_For_Index)src;

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
				var _src = (Syntax_For_Index)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 指標
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_For_Index.This")]
		public int This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private int m_This = 0;

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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
						target_port.Data = Convert.ToInt32(value);
						return;
					}
			}
			throw new NotSupportedException();
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
				Syntax_For parent = this.GetBelongTaskflow(this.Parent) as Syntax_For;

				if (parent != null)
				{
					#region 親が設定されていれば、親の出力を参照する.
					var port = this.DataOut[0];
					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var parent_key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(parent, parent.DataOut[0]);
					e.Variables[key] = e.Variables[parent_key];
					#endregion
				}
				else
				{
					#region 親が設定されていなければ、一般的な Int32 オブジェクトとして振る舞う.
					var name = e.TaskNames[this];
					var port = this.DataOut[0];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), this.This.GetType());

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.From(this.This)));
					#endregion
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
			// for の index を参照しているだけなので処理の実体はない.
		}

		#endregion

		#region 関数: (自身が所属する For)

		/// <summary>
		/// 自身が所属する ForEach の取得
		/// </summary>
		/// <param name="parent">自身を所有するタスクフロー</param>
		/// <returns>
		///		Syntax_For のインスタンスを返します。
		/// </returns>
		private CxTaskflow GetBelongTaskflow(CxTaskflow parent)
		{
			if (parent == null)
				return null;
			if (parent is Syntax_For)
				return parent;
			return GetBelongTaskflow(parent.Parent);
		}

		#endregion
	}

	#endregion

	#region Syntax.ForEach

	/// <summary>
	/// 反復処理（foreach）。配列などの列挙可能なコレクションの各要素に対して処理を実行します。[関連: System.Collections.List、System.Collections.FileNameList] 
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_ForEach : CxTaskflow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_ForEach()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "ForEach";
			this.IconKey = "List.Number";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Items", new Type[] { typeof(System.Collections.IEnumerable) }),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Item", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 配列などの列挙可能なコレクション。
			/// </summary>
			DataIn0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_ForEach(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_ForEach)
			{
				base.CopyFrom(src);

				var _src = (Syntax_ForEach)src;

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
				var _src = (Syntax_ForEach)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 要素
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_ForEach.This")]
		public string Item
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
		/// 要素の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_ForEach.Type")]
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

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.Reset();

			this.DataIn[0].CheckValidity(true);
			var items = (System.Collections.IEnumerable)this.DataIn[0].Data;

			foreach (var item in items)
			{
				this.DataOut[0].Data = item;

				#region 子タスクの初期化:
				var tasks = this.GetTasks(true);
				foreach (var task in tasks)
				{
					if (task is Syntax_ForEach_Item)
					{
						var child = (Syntax_ForEach_Item)task;
						child.This = item;
					}
				}
				#endregion

				base.Execute(sender, e);

				#region 制御構文:
				if (e.ControlSyntax == ExControlSyntax.Continue)
				{
					e.ControlSyntax = ExControlSyntax.None;
					continue;
				}
				if (e.ControlSyntax == ExControlSyntax.Break)
				{
					e.ControlSyntax = ExControlSyntax.None;
					break;
				}
				if (e.ControlSyntax == ExControlSyntax.Return) break;
				#endregion
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
						target_port.Data = value;
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
				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					e.TargetIterations.Add(this);
					e.IndentLevel++;

					var datain0 = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));
					var dataout0 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var counter = new CodeExtraVariable(string.Format("{0}_i", e.TaskNames[this]), typeof(int));
					var enumerator = new CodeExtraVariable(string.Format("{0}_enumerator", e.TaskNames[this]), typeof(System.Collections.IEnumerator));

					var debug_write = new CodeExpressionStatement(
						new CodeExtraType(typeof(System.Diagnostics.Debug)).Call("WriteLine", CodeLiteral.From(""))
						);
					var label_Continue = new CodeLabeledStatement(string.Format("{0}_Continue", e.TaskNames[this]));
					var label_Break = new CodeLabeledStatement(string.Format("{0}_Break", e.TaskNames[this]));

					#region for:
					// task#_i = 0
					var initial_statement = counter.Declare(CodeLiteral.From(0));
					// task#_enumerator.MoveNext()
					var limit_expression = enumerator.Call("MoveNext");
					// task#_i = task#_i+1
					var step_statement = counter.Assign(counter.Add(CodeLiteral.From(1)));
					#endregion

					#region for: body_statements
					CodeStatement[] body_statements;
					{
						var this_scope = new CodeStatementCollection();

						// item = task#_enumerator.Current
						this_scope.Add(dataout0.Assign(new CodeCastExpression(dataout0.TypeRef, enumerator.Ref("Current"))));

						// 変数宣言:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateDeclarationCode(this, e, this_scope);
						}

						// 処理:
						foreach (var task in this.TaskUnits)
						{
							task.GenerateProcedureCode(this, e, this_scope);
						}

						#region Continue Label:
						switch (e.LanguageType)
						{
							case ExLanguageType.CSharp:
								// C# の場合は continue; で継続する.
								break;
							case ExLanguageType.VisualBasic:
								this_scope.Add(new CodeSnippetStatement());
								this_scope.Add(label_Continue);
								break;
							default:
								label_Continue.Statement = debug_write;
								this_scope.Add(new CodeSnippetStatement());
								this_scope.Add(label_Continue);
								break;
						}
						#endregion

						body_statements = new CodeStatement[this_scope.Count];
						if (this_scope.Count > 0)
							this_scope.CopyTo(body_statements, 0);
					}
					#endregion

					scope.Add(new CodeSnippetStatement());
					scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));
					// IEnumerator task#_enumerator = task$.GetEnumerator()
					scope.Add(enumerator.Declare(datain0.Call("GetEnumerator")));
					scope.Add(new CodeIterationStatement(initial_statement, limit_expression, step_statement, body_statements));

					#region Break Label:
					switch (e.LanguageType)
					{
						case ExLanguageType.CSharp:
							// C# の場合は break; で中断する.
							break;
						case ExLanguageType.VisualBasic:
							// VB の場合は Exit Do で中断する.
							break;
						default:
							label_Break.Statement = debug_write;
							scope.Add(label_Break);
							break;
					}
					#endregion

					e.IndentLevel--;
					e.TargetIterations.Remove(this);
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.ForEach.Item

	/// <summary>
	/// 反復処理（foreach）の項目。[関連: System.Syntax.ForEach]
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_ForEach_Item : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_ForEach_Item()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Item";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
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
			/// 現在の要素を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected Syntax_ForEach_Item(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
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
			if (src is Syntax_ForEach_Item)
			{
				base.CopyFrom(src);

				var _src = (Syntax_ForEach_Item)src;

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
				var _src = (Syntax_ForEach_Item)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 要素
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Syntax_ForEach_Item.This")]
		public object This
		{
			get { return m_This; }
			set { m_This = value; }
		}
		private object m_This = null;

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			this.DataOut[0].Data = this.This;
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
						target_port.Data = value;
						return;
					}
			}
			throw new NotSupportedException();
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
				Syntax_ForEach parent = this.GetBelongTaskflow(this.Parent) as Syntax_ForEach;

				if (parent != null)
				{
					#region 親が設定されていれば、親の出力を参照する.
					var port = this.DataOut[0];
					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var parent_key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(parent, parent.DataOut[0]);
					e.Variables[key] = e.Variables[parent_key];
					#endregion
				}
				else
				{
					#region 親が設定されていなければ、一般的な System.Object として振る舞う.
					var name = e.TaskNames[this];
					var port = this.DataOut[0];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					// var task#_This = xxx;
					scope.Add(variable.Declare(CodeLiteral.Null));
					#endregion
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
			// foreach の item を参照しているだけなので処理の実体はない.
		}

		#endregion

		#region 関数: (自身が所属する ForEach)

		/// <summary>
		/// 自身が所属する ForEach の取得
		/// </summary>
		/// <param name="parent">自身を所有するタスクフロー</param>
		/// <returns>
		///		Syntax_ForEach のインスタンスを返します。
		/// </returns>
		private CxTaskflow GetBelongTaskflow(CxTaskflow parent)
		{
			if (parent == null)
				return null;
			if (parent is Syntax_ForEach)
				return parent;
			return GetBelongTaskflow(parent.Parent);
		}

		#endregion
	}

	#endregion

	#region Syntax.Continue

	/// <summary>
	/// 反復処理の末尾まで処理をスキップします。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_Continue : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Continue()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Continue";
			this.IconKey = "Unit-StepOut";

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

		private enum Warnings
		{
			/// <summary>
			/// Continue は For または ForEach 内でのみ使用できます。
			/// </summary>
			Unsupported,
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
			if (src is Syntax_Continue)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Continue)src;

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
				var _src = (Syntax_Continue)src;
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
			e.ControlSyntax = ExControlSyntax.Continue;
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

					switch (e.LanguageType)
					{
						case ExLanguageType.CSharp:
							scope.Add(new CodeSnippetStatement(new string('	', e.IndentLevel) + "continue;"));
							break;
						default:
						case ExLanguageType.VisualBasic:
							#region Continue Label:
							// Continue は CodeDom に無いので goto で代用する.
							if (e.TargetIterations.Count > 0)
							{
								var iter = e.TargetIterations[e.TargetIterations.Count - 1];
								var label = string.Format("{0}_Continue", e.TaskNames[iter]);
								scope.Add(new CodeGotoStatement(label));
							}
							#endregion
							break;
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.Break

	/// <summary>
	/// 反復処理を中断します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_Break : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Break()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Break";
			this.IconKey = "Notify-None";

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

		private enum Warnings
		{
			/// <summary>
			/// Break は For または ForEach 内でのみ使用できます。
			/// </summary>
			Unsupported,
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
			if (src is Syntax_Break)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Break)src;

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
				var _src = (Syntax_Break)src;
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
			e.ControlSyntax = ExControlSyntax.Break;
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

					switch (e.LanguageType)
					{
						case ExLanguageType.CSharp:
							scope.Add(new CodeSnippetStatement(new string('	', e.IndentLevel) + "break;"));
							break;
						case ExLanguageType.VisualBasic:
							scope.Add(new CodeSnippetStatement(new string('	', e.IndentLevel) + "Exit Do"));
							break;
						default:
							#region Break Label:
							// Break は CodeDom に無いので goto で代用する.
							if (e.TargetIterations.Count > 0)
							{
								var iter = e.TargetIterations[e.TargetIterations.Count - 1];
								var label = string.Format("{0}_Break", e.TaskNames[iter]);
								scope.Add(new CodeGotoStatement(label));
							}
							#endregion
							break;
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region Syntax.Return

	/// <summary>
	/// 関数を終了します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Syntax_Return : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Syntax_Return()
			: base()
		{
			this.Category = "Syntax";
			this.Name = "Return";
			this.IconKey = "GotoShortcuts";

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

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			#region 同一型:
			if (src is Syntax_Return)
			{
				base.CopyFrom(src);

				var _src = (Syntax_Return)src;

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
				var _src = (Syntax_Return)src;
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
			e.ControlSyntax = ExControlSyntax.Return;
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
					scope.Add(new CodeMethodReturnStatement());
				}
			}
		}

		#endregion
	}

	#endregion
}
