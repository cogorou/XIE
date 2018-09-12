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
	// Color
	// 

	#region Color.ctor

	/// <summary>
	/// カラー構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Color_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Color_ctor()
			: base()
		{
			this.Category = "System.Drawing.Color";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("R", new Type[] { typeof(byte) }),
				new CxTaskPortIn("G", new Type[] { typeof(byte) }),
				new CxTaskPortIn("B", new Type[] { typeof(byte) }),
				new CxTaskPortIn("A", new Type[] { typeof(byte) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(Color) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataParam2,

			/// <summary>
			/// アルファ値 [0~255]
			/// </summary>
			DataParam3,

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
			if (src is Color_ctor)
			{
				base.CopyFrom(src);

				var _src = (Color_ctor)src;

				this.m_This = _src.m_This;

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
				var _src = (Color_ctor)src;

				if (this.m_This != _src.m_This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 赤成分
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Color_ctor.R")]
		public byte R
		{
			get { return m_This.R; }
			set { m_This.R = value; }
		}

		/// <summary>
		/// 緑成分
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Color_ctor.G")]
		public byte G
		{
			get { return m_This.G; }
			set { m_This.G = value; }
		}

		/// <summary>
		/// 青成分
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Color_ctor.B")]
		public byte B
		{
			get { return m_This.B; }
			set { m_This.B = value; }
		}

		/// <summary>
		/// アルファ値
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Color_ctor.A")]
		public byte A
		{
			get { return m_This.A; }
			set { m_This.A = value; }
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// カラー
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Color_ctor.This")]
		public TxRGB8x4 This
		{
			get { return m_This; }
		}
		private TxRGB8x4 m_This = new TxRGB8x4(0, 0, 0);

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
						case 0: this.m_This.R = Convert.ToByte(this.DataParam[iii].Data); break;
						case 1: this.m_This.G = Convert.ToByte(this.DataParam[iii].Data); break;
						case 2: this.m_This.B = Convert.ToByte(this.DataParam[iii].Data); break;
						case 3: this.m_This.A = Convert.ToByte(this.DataParam[iii].Data); break;
					}
				}
			}

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

				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					var r = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.m_This.R));
					var g = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.m_This.G));
					var b = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.m_This.B));
					var a = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.m_This.A));

					// task#_This = Color.FromArgb(a, r, g, b);
					var color = new CodeExtraType(typeof(Color));
					scope.Add(dst.Assign(color.Call("FromArgb", a, r, g, b)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Color.Properties

	/// <summary>
	/// カラー構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class Color_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Color_Properties()
			: base()
		{
			this.Category = "System.Drawing.Color";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(Color), typeof(TxRGB8x3), typeof(TxRGB8x4), typeof(TxBGR8x3), typeof(TxBGR8x4) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("R", new Type[] { typeof(byte) }),
				new CxTaskPortOut("G", new Type[] { typeof(byte) }),
				new CxTaskPortOut("B", new Type[] { typeof(byte) }),
				new CxTaskPortOut("A", new Type[] { typeof(byte) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 赤成分 [0~255]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 緑成分 [0~255]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 青成分 [0~255]
			/// </summary>
			DataOut2,

			/// <summary>
			/// アルファ [0~255]
			/// </summary>
			DataOut3,
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
			if (src is Color_Properties)
			{
				base.CopyFrom(src);

				var _src = (Color_Properties)src;

				this.Body = _src.Body;

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
				var _src = (Color_Properties)src;

				if (this.Body != _src.Body) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// カラー
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Color_Properties.Body")]
		public TxRGB8x4 Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private TxRGB8x4 m_Body = new TxRGB8x4(0, 0, 0);

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 赤成分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Color_Properties.R")]
		public int R
		{
			get { return Body.R; }
		}

		/// <summary>
		/// 緑成分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Color_Properties.G")]
		public int G
		{
			get { return Body.G; }
		}

		/// <summary>
		/// 青成分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Color_Properties.B")]
		public int B
		{
			get { return Body.B; }
		}

		/// <summary>
		/// アルファ値
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.Color_Properties.A")]
		public int A
		{
			get { return Body.A; }
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
			if (this.DataParam[0].CheckValidity())
			{
				var body = this.DataParam[0].Data;
				if (body is Color)
				{
					this.m_Body = (Color)(body);
				}
				else if (body is TxRGB8x3)
				{
					this.m_Body = (TxRGB8x3)(body);
				}
				else if (body is TxRGB8x4)
				{
					this.m_Body = (TxRGB8x4)(body);
				}
				else if (body is TxBGR8x3)
				{
					this.m_Body = (TxBGR8x3)(body);
				}
				else if (body is TxBGR8x4)
				{
					this.m_Body = (TxBGR8x4)(body);
				}
			}

			// 出力.
			this.DataOut[0].Data = this.m_Body.R;
			this.DataOut[1].Data = this.m_Body.G;
			this.DataOut[2].Data = this.m_Body.B;
			this.DataOut[3].Data = this.m_Body.A;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

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
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.m_Body.R))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.m_Body.G))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.m_Body.B))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.m_Body.A))); break;
						}
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

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("R"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("G"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("B"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("A"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// DateTime
	// 

	#region DateTime.ctor

	/// <summary>
	/// 日時構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class DateTime_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DateTime_ctor()
			: base()
		{
			this.Category = "System.DateTime";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Year", new Type[] { typeof(int) }),
				new CxTaskPortIn("Month", new Type[] { typeof(int) }),
				new CxTaskPortIn("Day", new Type[] { typeof(int) }),
				new CxTaskPortIn("Hour", new Type[] { typeof(int) }),
				new CxTaskPortIn("Minute", new Type[] { typeof(int) }),
				new CxTaskPortIn("Second", new Type[] { typeof(int) }),
				new CxTaskPortIn("Millisecond", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(DateTime) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 年 (西暦) [1~9999]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 月 [1~12]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 日 [1~31]
			/// </summary>
			DataParam2,

			/// <summary>
			/// 時 [0~23]
			/// </summary>
			DataParam3,

			/// <summary>
			/// 分 [0~59]
			/// </summary>
			DataParam4,

			/// <summary>
			/// 秒 [0~59]
			/// </summary>
			DataParam5,

			/// <summary>
			/// ミリ秒 [0~999]
			/// </summary>
			DataParam6,

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
			if (src is DateTime_ctor)
			{
				base.CopyFrom(src);

				var _src = (DateTime_ctor)src;

				this.Year = _src.Year;
				this.Month = _src.Month;
				this.Day = _src.Day;
				this.Hour = _src.Hour;
				this.Minute = _src.Minute;
				this.Second = _src.Second;
				this.Millisecond = _src.Millisecond;

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
				var _src = (DateTime_ctor)src;

				if (this.Year != _src.Year) return false;
				if (this.Month != _src.Month) return false;
				if (this.Day != _src.Day) return false;
				if (this.Hour != _src.Hour) return false;
				if (this.Minute != _src.Minute) return false;
				if (this.Second != _src.Second) return false;
				if (this.Millisecond != _src.Millisecond) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 年 (西暦) [1~9999]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Year")]
		public int Year
		{
			get { return m_Year; }
			set { m_Year = value; }
		}
		private int m_Year = 1;

		/// <summary>
		/// 月 [1~12]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Month")]
		public int Month
		{
			get { return m_Month; }
			set { m_Month = value; }
		}
		private int m_Month = 1;

		/// <summary>
		/// 日 [1~31]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Day")]
		public int Day
		{
			get { return m_Day; }
			set { m_Day = value; }
		}
		private int m_Day = 1;

		/// <summary>
		/// 時 [0~23]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Hour")]
		public int Hour
		{
			get { return m_Hour; }
			set { m_Hour = value; }
		}
		private int m_Hour = 0;

		/// <summary>
		/// 分 [0~59]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Minute")]
		public int Minute
		{
			get { return m_Minute; }
			set { m_Minute = value; }
		}
		private int m_Minute = 0;

		/// <summary>
		/// 秒 [0~59]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Second")]
		public int Second
		{
			get { return m_Second; }
			set { m_Second = value; }
		}
		private int m_Second = 0;

		/// <summary>
		/// ミリ秒 [0~999]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Millisecond")]
		public int Millisecond
		{
			get { return m_Millisecond; }
			set { m_Millisecond = value; }
		}
		private int m_Millisecond = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 日時
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_ctor.This")]
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
		[CxDescription("P:XIE.Tasks.DateTime_ctor.Type")]
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

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Year = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Month = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Day = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Hour = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.Minute = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 5: this.Second = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 6: this.Millisecond = Convert.ToInt32(this.DataParam[iii].Data);break;
					}
				}
			}

			var dt = new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);

			this.DataOut[0].Data = dt;
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
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					var year = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Year));
					var month = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Month));
					var day = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Day));
					var hour = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Hour));
					var minute = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Minute));
					var second = ApiHelper.CodeOptionalExpression(e, this.DataParam[5], CodeLiteral.From(this.Second));
					var millisecond = ApiHelper.CodeOptionalExpression(e, this.DataParam[6], CodeLiteral.From(this.Millisecond));

					// task#_This = new DateTime(year, month, day, hour, minute, second, millisecond);
					var dt = new CodeExtraType(typeof(DateTime));
					scope.Add(dst.Assign(dt.New(year, month, day, hour, minute, second, millisecond)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region DateTime.Now

	/// <summary>
	/// 現在のローカル時刻で日時構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class DateTime_Now : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DateTime_Now()
			: base()
		{
			this.Category = "System.DateTime";
			this.Name = "Now";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(DateTime) })
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
			if (src is DateTime_Now)
			{
				base.CopyFrom(src);

				var _src = (DateTime_Now)src;

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
				var _src = (DateTime_Now)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 日時
		/// </summary>
		[XmlIgnore]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Now.This")]
		public DateTime This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private DateTime m_This = DateTime.Now;

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
			this.This = DateTime.Now;
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

				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// task#_This = DateTime.Now;
					var dt = new CodeExtraType(typeof(DateTime));
					scope.Add(dst.Assign(dt.Ref("Now")));
				}
			}
		}

		#endregion
	}

	#endregion

	#region DateTime.UtcNow

	/// <summary>
	/// 世界協定時刻(UTC)で日時構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class DateTime_UtcNow : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DateTime_UtcNow()
			: base()
		{
			this.Category = "System.DateTime";
			this.Name = "UtcNow";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(DateTime) })
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
			if (src is DateTime_UtcNow)
			{
				base.CopyFrom(src);

				var _src = (DateTime_UtcNow)src;

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
				var _src = (DateTime_UtcNow)src;

				if (this.This != _src.This) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 日時
		/// </summary>
		[XmlIgnore]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_UtcNow.This")]
		public DateTime This
		{
			get { return m_This; }
			private set { m_This = value; }
		}
		private DateTime m_This = DateTime.UtcNow;

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
			this.This = DateTime.UtcNow;
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

				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// task#_This = DateTime.UtcNow;
					var dt = new CodeExtraType(typeof(DateTime));
					scope.Add(dst.Assign(dt.Ref("UtcNow")));
				}
			}
		}

		#endregion
	}

	#endregion

	#region DateTime.Properties

	/// <summary>
	/// 日時構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class DateTime_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DateTime_Properties()
			: base()
		{
			this.Category = "System.DateTime";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(DateTime), typeof(TxDateTime) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Year", new Type[] { typeof(int) }),
				new CxTaskPortOut("Month", new Type[] { typeof(int) }),
				new CxTaskPortOut("Day", new Type[] { typeof(int) }),
				new CxTaskPortOut("Hour", new Type[] { typeof(int) }),
				new CxTaskPortOut("Minute", new Type[] { typeof(int) }),
				new CxTaskPortOut("Second", new Type[] { typeof(int) }),
				new CxTaskPortOut("Millisecond", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 年 (西暦) [1~9999]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 月 [1~12]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 日 [1~31]
			/// </summary>
			DataOut2,

			/// <summary>
			/// 時 [0~23]
			/// </summary>
			DataOut3,

			/// <summary>
			/// 分 [0~59]
			/// </summary>
			DataOut4,

			/// <summary>
			/// 秒 [0~59]
			/// </summary>
			DataOut5,

			/// <summary>
			/// ミリ秒 [0~999]
			/// </summary>
			DataOut6,
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
			if (src is DateTime_Properties)
			{
				base.CopyFrom(src);

				var _src = (DateTime_Properties)src;

				this.Body = _src.Body;
				this.BodyIsInitialized = _src.BodyIsInitialized;

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
				var _src = (DateTime_Properties)src;

				if (this.Body != _src.Body) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 日時
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Body")]
		public DateTime Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private DateTime m_Body = DateTime.Now;

		/// <summary>
		/// 日時 (初期化済みフラグ)
		/// </summary>
		[Browsable(false)]
		public virtual bool BodyIsInitialized
		{
			get { return m_BodyIsInitialized; }
			set { m_BodyIsInitialized = value; }
		}
		private bool m_BodyIsInitialized = false;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 年
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Year")]
		public int Year
		{
			get { return Body.Year; }
		}

		/// <summary>
		/// 月
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Month")]
		public int Month
		{
			get { return Body.Month; }
		}

		/// <summary>
		/// 日
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Day")]
		public int Day
		{
			get { return Body.Day; }
		}

		/// <summary>
		/// 時
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Hour")]
		public int Hour
		{
			get { return Body.Hour; }
		}

		/// <summary>
		/// 分
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Minute")]
		public int Minute
		{
			get { return Body.Minute; }
		}

		/// <summary>
		/// 秒
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Second")]
		public int Second
		{
			get { return Body.Second; }
		}

		/// <summary>
		/// ミリ秒
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Millisecond")]
		public int Millisecond
		{
			get { return Body.Millisecond; }
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
			if (this.BodyIsInitialized == false)
			{
				this.BodyIsInitialized = true;
				this.Body = DateTime.Now;
			}
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
			if (this.DataParam[0].CheckValidity())
			{
				var body = this.DataParam[0].Data;
				if (body is DateTime)
				{
					this.Body = (DateTime)(body);
				}
				else if (body is TxDateTime)
				{
					this.Body = (TxDateTime)(body);
				}
			}

			// 出力.
			this.DataOut[0].Data = this.Body.Year;
			this.DataOut[1].Data = this.Body.Month;
			this.DataOut[2].Data = this.Body.Day;
			this.DataOut[3].Data = this.Body.Hour;
			this.DataOut[4].Data = this.Body.Minute;
			this.DataOut[5].Data = this.Body.Second;
			this.DataOut[6].Data = this.Body.Millisecond;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

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
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Year))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Month))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Day))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Hour))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Minute))); break;
							case 5: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Second))); break;
							case 6: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Millisecond))); break;
						}
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

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Year"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Month"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Day"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Hour"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("Minute"))); break;
							case 5: scope.Add(dst.Assign(src.Ref("Second"))); break;
							case 6: scope.Add(dst.Assign(src.Ref("Millisecond"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region DateTime.GetProperty

	/// <summary>
	/// 日時構造体のプロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class DateTime_GetProperty : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DateTime_GetProperty()
			: base()
		{
			_Construtor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="item_name">取得するプロパティの名称</param>
		public DateTime_GetProperty(string item_name)
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
			this.Category = "System.DateTime";
			this.Name = string.IsNullOrWhiteSpace(this.ItemName) ? "GetProperty" : this.ItemName;
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(DateTime) }),
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
			/// このインスタンスの日付の部分を取得します。
			/// </summary>
			Date,

			/// <summary>
			/// このインスタンスの時刻を取得します。
			/// </summary>
			TimeOfDay,

			/// <summary>
			/// このインスタンスで表される曜日を取得します。
			/// </summary>
			DayOfWeek,

			/// <summary>
			/// このインスタンスで表される年間積算日を取得します。
			/// </summary>
			DayOfYear,

			/// <summary>
			/// このインスタンスが表す時刻の種類 (現地時刻、世界協定時刻 (UTC)、または、そのどちらでもない) を示す値を取得します。
			/// </summary>
			Kind,

			/// <summary>
			/// このインスタンスの日付と時刻を表すタイマー刻み数を取得します。
			/// </summary>
			Ticks,
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
			if (src is DateTime_GetProperty)
			{
				base.CopyFrom(src);

				var _src = (DateTime_GetProperty)src;

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
				var _src = (DateTime_GetProperty)src;

				if (this.Body != _src.Body) return false;
				if (this.ItemName != _src.ItemName) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 日時
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.DateTime_GetProperty.Body")]
		public DateTime Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private DateTime m_Body = new DateTime();

		/// <summary>
		/// 取得するプロパティの名称
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.DateTime_GetProperty.ItemName")]
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
		[CxDescription("P:XIE.Tasks.DateTime_GetProperty.Value")]
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
		[CxDescription("P:XIE.Tasks.DateTime_GetProperty.Type")]
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
						case 0: this.Body = (DateTime)(this.DataParam[iii].Data); break;
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
	// TimeSpan
	// 

	#region TimeSpan.ctor

	/// <summary>
	/// 時間間隔構造体を構築します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TimeSpan_ctor : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TimeSpan_ctor()
			: base()
		{
			this.Category = "System.TimeSpan";
			this.Name = "Constructor";
			this.IconKey = "Unit-Constructor";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Days", new Type[] { typeof(int) }),
				new CxTaskPortIn("Hours", new Type[] { typeof(int) }),
				new CxTaskPortIn("Minutes", new Type[] { typeof(int) }),
				new CxTaskPortIn("Seconds", new Type[] { typeof(int) }),
				new CxTaskPortIn("Milliseconds", new Type[] { typeof(int) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("This", new Type[] { typeof(TimeSpan) })
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 日数
			/// </summary>
			DataParam0,

			/// <summary>
			/// 時間数
			/// </summary>
			DataParam1,

			/// <summary>
			/// 分数
			/// </summary>
			DataParam2,

			/// <summary>
			/// 秒数
			/// </summary>
			DataParam3,

			/// <summary>
			/// ミリ秒数
			/// </summary>
			DataParam4,

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
			if (src is TimeSpan_ctor)
			{
				base.CopyFrom(src);

				var _src = (TimeSpan_ctor)src;

				this.Days = _src.Days;
				this.Hours = _src.Hours;
				this.Minutes = _src.Minutes;
				this.Seconds = _src.Seconds;
				this.Milliseconds = _src.Milliseconds;

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
				var _src = (TimeSpan_ctor)src;

				if (this.Days != _src.Days) return false;
				if (this.Hours != _src.Hours) return false;
				if (this.Minutes != _src.Minutes) return false;
				if (this.Seconds != _src.Seconds) return false;
				if (this.Milliseconds != _src.Milliseconds) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 日数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Days")]
		public int Days
		{
			get { return m_Days; }
			set { m_Days = value; }
		}
		private int m_Days = 0;

		/// <summary>
		/// 時間数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Hours")]
		public int Hours
		{
			get { return m_Hours; }
			set { m_Hours = value; }
		}
		private int m_Hours = 0;

		/// <summary>
		/// 分数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Minutes")]
		public int Minutes
		{
			get { return m_Minutes; }
			set { m_Minutes = value; }
		}
		private int m_Minutes = 0;

		/// <summary>
		/// 秒数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Seconds")]
		public int Seconds
		{
			get { return m_Seconds; }
			set { m_Seconds = value; }
		}
		private int m_Seconds = 0;

		/// <summary>
		/// ミリ秒数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Milliseconds")]
		public int Milliseconds
		{
			get { return m_Milliseconds; }
			set { m_Milliseconds = value; }
		}
		private int m_Milliseconds = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 時間間隔
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.This")]
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
		[CxDescription("P:XIE.Tasks.TimeSpan_ctor.Type")]
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

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Days = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Hours = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 2: this.Minutes = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 3: this.Seconds = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 4: this.Milliseconds = Convert.ToInt32(this.DataParam[iii].Data);break;
					}
				}
			}

			var ts = new TimeSpan(Days, Hours, Minutes, Seconds, Milliseconds);

			this.DataOut[0].Data = ts;
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
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					var days = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Days));
					var hours = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Hours));
					var minutes = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Minutes));
					var seconds = ApiHelper.CodeOptionalExpression(e, this.DataParam[3], CodeLiteral.From(this.Seconds));
					var milliseconds = ApiHelper.CodeOptionalExpression(e, this.DataParam[4], CodeLiteral.From(this.Milliseconds));

					// task#_This = new TimeSpan(days, hours, minutes, seconds, milliseconds);
					var ts = new CodeExtraType(typeof(TimeSpan));
					scope.Add(dst.Assign(ts.New(days, hours, minutes, seconds, milliseconds)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TimeSpan.Properties

	/// <summary>
	/// 時間間隔構造体の各種プロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TimeSpan_Properties : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TimeSpan_Properties()
			: base()
		{
			this.Category = "System.TimeSpan";
			this.Name = "Properties";
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TimeSpan) }),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Days", new Type[] { typeof(int) }),
				new CxTaskPortOut("Hours", new Type[] { typeof(int) }),
				new CxTaskPortOut("Minutes", new Type[] { typeof(int) }),
				new CxTaskPortOut("Seconds", new Type[] { typeof(int) }),
				new CxTaskPortOut("Milliseconds", new Type[] { typeof(int) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// 対象のオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 日 [1~31]
			/// </summary>
			DataOut0,

			/// <summary>
			/// 時 [0~23]
			/// </summary>
			DataOut1,

			/// <summary>
			/// 分 [0~59]
			/// </summary>
			DataOut2,

			/// <summary>
			/// 秒 [0~59]
			/// </summary>
			DataOut3,

			/// <summary>
			/// ミリ秒 [0~999]
			/// </summary>
			DataOut4,
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
			if (src is TimeSpan_Properties)
			{
				base.CopyFrom(src);

				var _src = (TimeSpan_Properties)src;

				this.Body = _src.Body;

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
				var _src = (TimeSpan_Properties)src;

				if (this.Body != _src.Body) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 時間間隔
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_Properties.Body")]
		public TimeSpan Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private TimeSpan m_Body = new TimeSpan();

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 日数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Days")]
		public int Days
		{
			get { return Body.Days; }
		}

		/// <summary>
		/// 時数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Hours")]
		public int Hours
		{
			get { return Body.Hours; }
		}

		/// <summary>
		/// 分数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Minutes")]
		public int Minutes
		{
			get { return Body.Minutes; }
		}

		/// <summary>
		/// 秒数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Seconds")]
		public int Seconds
		{
			get { return Body.Seconds; }
		}

		/// <summary>
		/// ミリ秒数
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.DateTime_Properties.Milliseconds")]
		public int Milliseconds
		{
			get { return Body.Milliseconds; }
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
						case 0: this.Body = (TimeSpan)(this.DataParam[iii].Data); break;
					}
				}
			}

			// 出力.
			this.DataOut[0].Data = this.Body.Days;
			this.DataOut[1].Data = this.Body.Hours;
			this.DataOut[2].Data = this.Body.Minutes;
			this.DataOut[3].Data = this.Body.Seconds;
			this.DataOut[4].Data = this.Body.Milliseconds;
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

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), port.Types[0]);

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
						switch (i)
						{
							case 0: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Days))); break;
							case 1: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Hours))); break;
							case 2: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Minutes))); break;
							case 3: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Seconds))); break;
							case 4: scope.Add(variable.Declare(CodeLiteral.From(this.Body.Milliseconds))); break;
						}
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

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[i]));
						var src = new CodeExtraVariable(e.GetVariable(this.DataParam[0]));

						// task#_X = task$.X;
						switch (i)
						{
							case 0: scope.Add(dst.Assign(src.Ref("Days"))); break;
							case 1: scope.Add(dst.Assign(src.Ref("Hours"))); break;
							case 2: scope.Add(dst.Assign(src.Ref("Minutes"))); break;
							case 3: scope.Add(dst.Assign(src.Ref("Seconds"))); break;
							case 4: scope.Add(dst.Assign(src.Ref("Milliseconds"))); break;
						}
					}
				}
			}
		}

		#endregion
	}

	#endregion

	#region TimeSpan.GetProperty

	/// <summary>
	/// 時間間隔構造体のプロパティを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TimeSpan_GetProperty : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TimeSpan_GetProperty()
			: base()
		{
			_Construtor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="item_name">取得するプロパティの名称</param>
		public TimeSpan_GetProperty(string item_name)
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
			this.Category = "System.TimeSpan";
			this.Name = string.IsNullOrWhiteSpace(this.ItemName) ? "GetProperty" : this.ItemName;
			this.IconKey = "Unit-PropertyGet";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] { typeof(TimeSpan) }),
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
			/// 現在の TimeSpan 構造体の値を表すタイマー刻みの数を取得します。
			/// </summary>
			Ticks,

			/// <summary>
			/// 整数部と小数部から成る日数で表される、現在の TimeSpan 構造体の値を取得します。
			/// </summary>
			TotalDays,

			/// <summary>
			/// 整数部と小数部から成る時間数で表される、現在の TimeSpan 構造体の値を取得します。
			/// </summary>
			TotalHours,

			/// <summary>
			/// 整数部と小数部から成る分数で表される、現在の TimeSpan 構造体の値を取得します。
			/// </summary>
			TotalMinutes,

			/// <summary>
			/// 整数部と小数部から成る秒数で表される、現在の TimeSpan 構造体の値を取得します。
			/// </summary>
			TotalSeconds,

			/// <summary>
			/// 整数部と小数部から成るミリ秒数で表される、現在の TimeSpan 構造体の値を取得します。
			/// </summary>
			TotalMilliseconds,
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
			if (src is TimeSpan_GetProperty)
			{
				base.CopyFrom(src);

				var _src = (TimeSpan_GetProperty)src;

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
				var _src = (TimeSpan_GetProperty)src;

				if (this.Body != _src.Body) return false;
				if (this.ItemName != _src.ItemName) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 時間間隔
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_GetProperty.Body")]
		public TimeSpan Body
		{
			get { return m_Body; }
			set { m_Body = value; }
		}
		private TimeSpan m_Body = new TimeSpan();

		/// <summary>
		/// 取得するプロパティの名称
		/// </summary>
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TimeSpan_GetProperty.ItemName")]
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
		[CxDescription("P:XIE.Tasks.TimeSpan_GetProperty.Value")]
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
		[CxDescription("P:XIE.Tasks.TimeSpan_GetProperty.Type")]
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
						case 0: this.Body = (TimeSpan)(this.DataParam[iii].Data); break;
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
}
