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
	// Defs (定数)
	// 

	#region Defs_XIE_PI

	/// <summary>
	/// 円周率
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Defs_XIE_PI : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Defs_XIE_PI()
			: base()
		{
			this.Category = "XIE.Defs";
			this.Name = "XIE_PI";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 定数を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 定数を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Defs_XIE_PI.This")]
		public double This
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
			if (src is Defs_XIE_PI)
			{
				base.CopyFrom(src);

				var _src = (Defs_XIE_PI)src;

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
				var _src = (Defs_XIE_PI)src;
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

			// 出力.
			this.DataOut[0].Data = XIE.Defs.XIE_PI;
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

					var defs = new CodeExtraType(typeof(XIE.Defs));

					// var task#_This = XIE.Defs.XIE_PI;
					scope.Add(variable.Declare(defs.Ref("XIE_PI")));
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

	#region Defs_XIE_EPSd

	/// <summary>
	/// イプシロン(ε) [double 用]
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Defs_XIE_EPSd : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Defs_XIE_EPSd()
			: base()
		{
			this.Category = "XIE.Defs";
			this.Name = "XIE_EPSd";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(double) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 定数を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 定数を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Defs_XIE_EPSd.This")]
		public double This
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
			if (src is Defs_XIE_EPSd)
			{
				base.CopyFrom(src);

				var _src = (Defs_XIE_EPSd)src;

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
				var _src = (Defs_XIE_EPSd)src;
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

			// 出力.
			this.DataOut[0].Data = XIE.Defs.XIE_EPSd;
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

					var defs = new CodeExtraType(typeof(XIE.Defs));

					// var task#_This = XIE.Defs.XIE_EPSd;
					scope.Add(variable.Declare(defs.Ref("XIE_EPSd")));
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

	#region Defs_XIE_EPSf

	/// <summary>
	/// イプシロン(ε) [float 用]
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class Defs_XIE_EPSf : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Defs_XIE_EPSf()
			: base()
		{
			this.Category = "XIE.Defs";
			this.Name = "XIE_EPSf";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(float) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 定数を返します。
			/// </summary>
			DataOut0,
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 定数を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Defs_XIE_EPSf.This")]
		public float This
		{
			get
			{
				int index = 0;
				if (this.DataOut[index].Data is float)
					return (float)this.DataOut[index].Data;
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
			if (src is Defs_XIE_EPSf)
			{
				base.CopyFrom(src);

				var _src = (Defs_XIE_EPSf)src;

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
				var _src = (Defs_XIE_EPSf)src;
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

			// 出力.
			this.DataOut[0].Data = XIE.Defs.XIE_EPSf;
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

					var defs = new CodeExtraType(typeof(XIE.Defs));

					// var task#_This = XIE.Defs.XIE_EPSf;
					scope.Add(variable.Declare(defs.Ref("XIE_EPSf")));
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
