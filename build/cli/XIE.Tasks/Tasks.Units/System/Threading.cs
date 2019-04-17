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
	// Thread
	// 

	#region Thread.Sleep

	/// <summary>
	/// 指定された時間が経過するまで待機します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Thread_Sleep : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Thread_Sleep()
			: base()
		{
			this.Category = "System.Threading.Thread";
			this.Name = "Sleep";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Timeout", new Type[] { typeof(int), typeof(TimeSpan) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 待機時間。int で指定した場合は msec 単位で処理します。
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
			if (src is Thread_Sleep)
			{
				base.CopyFrom(src);

				var _src = (Thread_Sleep)src;

				this.Timeout = _src.Timeout;

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
				var _src = (Thread_Sleep)src;

				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 待機時間 (msec)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Thread_Sleep.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

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
			this.DataParam[0].CheckValidity();
			if (this.DataParam[0].Data is TimeSpan)
			{
				TimeSpan timeout = (TimeSpan)this.DataParam[0].Data;
				System.Threading.Thread.Sleep(timeout);
			}
			else
			{
				if (this.DataParam[0].IsConnected)
					this.Timeout = Convert.ToInt32(this.DataParam[0].Data);
				System.Threading.Thread.Sleep(this.Timeout);
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
				scope.Add(new CodeSnippetStatement());
				scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				{
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Timeout));

					// System.Threading.Thread.Sleep(timeout);
					var thread = new CodeExtraType(typeof(System.Threading.Thread));
					scope.Add(thread.Call("Sleep", timeout));
				}
			}
		}

		#endregion
	}

	#endregion

}
