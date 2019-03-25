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
using System.Threading;
using System.Diagnostics;
using System.CodeDom;

namespace XIE.Tasks
{
	// //////////////////////////////////////////////////
	// Device
	// 

	#region Camera

	/// <summary>
	/// カメラを参照します。（メインフォームの左側のツリービューの Camera フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_Camera_Controller : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_Camera_Controller()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "Camera";
			this.IconKey = "Camera-Connect";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Controller", new Type[] { typeof(XIE.Media.CxCamera)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つコントローラを返します。
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
			if (src is CxAuxInfo_Camera_Controller)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_Camera_Controller)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_Camera_Controller)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Prepare)

		/// <summary>
		/// 開始時にコントローラを開始するか否か
		/// </summary>
		[CxCategory("Prepare")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Camera_Controller.StartWhenBegin")]
		public bool StartWhenBegin
		{
			get { return m_StartWhenBegin; }
			set { m_StartWhenBegin = value; }
		}
		private bool m_StartWhenBegin = true;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Camera_Controller.Index")]
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
					var aux = ((CxAuxInfo_Camera_Controller)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Camera_Controller.Controller")]
		public XIE.Media.CxCamera Controller
		{
			get { return m_Controller; }
			private set { m_Controller = value; }
		}
		[NonSerialized]
		private XIE.Media.CxCamera m_Controller = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoCameras)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Controllers.Length)
				this.Controller = this.AuxInfo.Controllers[this.Index];
			else
				this.Controller = null;

			this.DataOut[0].Data = this.Controller;
		}
		[NonSerialized]
		private IxAuxInfoCameras AuxInfo = null;

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 処理開始前の準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Prepare(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// 処理終了後の復旧
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Restore(object sender, CxTaskExecuteEventArgs e)
		{
			if (this.Controller != null)
			{
				this.Controller.Stop();
			}
		}

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoCameras)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Controllers.Length)
				this.Controller = aux.Controllers[this.Index];
			else
				this.Controller = null;

			if (this.StartWhenBegin)
			{
				if (this.Controller != null)
				{
					this.Controller.Start();

					// 待機:
					for (int i = 0; i < 50; i++)
					{
						if (this.Controller.IsRunning) break;
						System.Threading.Thread.Sleep(100);
					}
				}
			}

			this.DataOut[0].Data = this.Controller;
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

				// task#_Controller = ((IxAuxInfoCameras)e.AuxInfo).Controllers[index];
				//                                       ~~~~~(1)~              ~(4)~
				//                     ~~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//                    ~~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoCameras)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoCameras), e_AuxInfo);

					// 3) ((IxAuxInfoCameras)e.AuxInfo).Controllers
					var controllers = new CodePropertyReferenceExpression(aux, "Controllers");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Controller = ((IxAuxInfoCameras)e.AuxInfo).Controllers[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(controllers, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region SerialPort

	/// <summary>
	/// シリアル通信を参照します。（メインフォームの左側のツリービューの SerialPort フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_SerialPort_Controller : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_SerialPort_Controller()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "SerialPort";
			this.IconKey = "SerialPort-Connect";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Controller", new Type[] { typeof(XIE.IO.CxSerialPort) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つコントローラを返します。
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
			if (src is CxAuxInfo_SerialPort_Controller)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_SerialPort_Controller)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_SerialPort_Controller)src;

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
		[CxDescription("P:XIE.Tasks.CxAuxInfo_SerialPort_Controller.Index")]
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
					var aux = ((CxAuxInfo_SerialPort_Controller)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_SerialPort_Controller.Controller")]
		public XIE.IO.CxSerialPort Controller
		{
			get { return m_Port; }
			private set { m_Port = value; }
		}
		[NonSerialized]
		private XIE.IO.CxSerialPort m_Port = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoSerialPorts)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Controllers.Length)
				this.Controller = this.AuxInfo.Controllers[this.Index];
			else
				this.Controller = null;

			this.DataOut[0].Data = this.Controller;
		}
		[NonSerialized]
		private IxAuxInfoSerialPorts AuxInfo = null;

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoSerialPorts)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Controllers.Length)
				this.Controller = aux.Controllers[this.Index];
			else
				this.Controller = null;
			this.DataOut[0].Data = this.Controller;
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

				// task#_Controller = ((IxAuxInfoSerialPorts)e.AuxInfo).Controllers[index];
				//                                           ~~~~~(1)~              ~(4)~
				//                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//                    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoSerialPorts)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoSerialPorts), e_AuxInfo);

					// 3) ((IxAuxInfoSerialPorts)e.AuxInfo).Controllers
					var controllers = new CodePropertyReferenceExpression(aux, "Controllers");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Controller = ((IxAuxInfoSerialPorts)e.AuxInfo).Controllers[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(controllers, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TcpServer

	/// <summary>
	/// TCP/IP 通信サーバーを参照します。（メインフォームの左側のツリービューの TcpServer フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_TcpServer_Controller : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_TcpServer_Controller()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "TcpServer";
			this.IconKey = "Net-Connect";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Controller", new Type[] { typeof(XIE.Net.CxTcpServer) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つコントローラを返します。
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
			if (src is CxAuxInfo_TcpServer_Controller)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_TcpServer_Controller)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_TcpServer_Controller)src;

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
		[CxDescription("P:XIE.Tasks.CxAuxInfo_TcpServer_Controller.Index")]
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
					var aux = ((CxAuxInfo_TcpServer_Controller)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_TcpServer_Controller.Controller")]
		public XIE.Net.CxTcpServer Controller
		{
			get { return m_Controller; }
			private set { m_Controller = value; }
		}
		[NonSerialized]
		private XIE.Net.CxTcpServer m_Controller = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoTcpServers)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Controllers.Length)
				this.Controller = this.AuxInfo.Controllers[this.Index];
			else
				this.Controller = null;

			this.DataOut[0].Data = this.Controller;
		}
		[NonSerialized]
		private IxAuxInfoTcpServers AuxInfo = null;

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoTcpServers)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Controllers.Length)
				this.Controller = aux.Controllers[this.Index];
			else
				this.Controller = null;
			this.DataOut[0].Data = this.Controller;
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

				// task#_Controller = ((IxAuxInfoTcpServers)e.AuxInfo).Controllers[index];
				//                                          ~~~~~(1)~              ~(4)~
				//                     ~~~~~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//                    ~~~~~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoTcpServers)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoTcpServers), e_AuxInfo);

					// 3) ((IxAuxInfoTcpServers)e.AuxInfo).Controllers
					var controllers = new CodePropertyReferenceExpression(aux, "Controllers");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Controller = ((IxAuxInfoTcpServers)e.AuxInfo).Controllers[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(controllers, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TcpClient

	/// <summary>
	/// TCP/IP 通信クライアントを参照します。（メインフォームの左側のツリービューの TcpClient フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_TcpClient_Controller : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_TcpClient_Controller()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "TcpClient";
			this.IconKey = "Net-Connect";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Controller", new Type[] { typeof(XIE.Net.CxTcpClient) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つコントローラを返します。
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
			if (src is CxAuxInfo_TcpClient_Controller)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_TcpClient_Controller)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_TcpClient_Controller)src;

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
		[CxDescription("P:XIE.Tasks.CxAuxInfo_TcpClient_Controller.Index")]
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
					var aux = ((CxAuxInfo_TcpClient_Controller)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_TcpClient_Controller.Controller")]
		public XIE.Net.CxTcpClient Controller
		{
			get { return m_Controller; }
			private set { m_Controller = value; }
		}
		[NonSerialized]
		private XIE.Net.CxTcpClient m_Controller = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoTcpClients)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Controllers.Length)
				this.Controller = this.AuxInfo.Controllers[this.Index];
			else
				this.Controller = null;

			this.DataOut[0].Data = this.Controller;
		}
		[NonSerialized]
		private IxAuxInfoTcpClients AuxInfo = null;

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoTcpClients)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Controllers.Length)
				this.Controller = aux.Controllers[this.Index];
			else
				this.Controller = null;
			this.DataOut[0].Data = this.Controller;
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

				// task#_Controller = ((IxAuxInfoTcpClients)e.AuxInfo).Controllers[index];
				//                                          ~~~~~(1)~              ~(4)~
				//                     ~~~~~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//                    ~~~~~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoTcpClients)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoTcpClients), e_AuxInfo);

					// 3) ((IxAuxInfoTcpClients)e.AuxInfo).Controllers
					var controllers = new CodePropertyReferenceExpression(aux, "Controllers");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Controller = ((IxAuxInfoTcpClients)e.AuxInfo).Controllers[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(controllers, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// Data
	// 

	#region Media

	/// <summary>
	/// メディアプレーヤーを参照します。（メインフォームの左側のツリービューの Media フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_Media_Player : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_Media_Player()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "Media";
			this.IconKey = "Media-Connect";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Player", new Type[] { typeof(XIE.Media.CxMediaPlayer) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つメディアプレーヤーのオブジェクトを返します。
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
			if (src is CxAuxInfo_Media_Player)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_Media_Player)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_Media_Player)src;

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
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Media_Player.Index")]
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
					var aux = ((CxAuxInfo_Media_Player)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Media_Player.Data")]
		public XIE.Media.CxMediaPlayer Player
		{
			get { return m_Player; }
			private set { m_Player = value; }
		}
		[NonSerialized]
		private XIE.Media.CxMediaPlayer m_Player = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoMedias)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Players.Length)
				this.Player = this.AuxInfo.Players[this.Index];
			else
				this.Player = null;

			this.DataOut[0].Data = this.Player;
		}
		[NonSerialized]
		private IxAuxInfoMedias AuxInfo = null;

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoMedias)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Players.Length)
				this.Player = aux.Players[this.Index];
			else
				this.Player = null;
			this.DataOut[0].Data = this.Player;
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

				// task#_Player = ((IxAuxInfoMedias)e.AuxInfo).Players[index];
				//                                  ~~~~~(1)~           ~(4)~
				//                 ~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//                ~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoMedias)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoMedias), e_AuxInfo);

					// 3) ((IxAuxInfoMedias)e.AuxInfo).Players
					var players = new CodePropertyReferenceExpression(aux, "Players");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Player = ((IxAuxInfoMedias)e.AuxInfo).Players[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(players, index)
						));
				}
			}
		}

		#endregion
	}

	#endregion

	#region Image

	/// <summary>
	/// 画像オブジェクトを参照します。（メインフォームの左側のツリービューの Image フォルダ配下のノードを示します。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxAuxInfo_Image_Data : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_Image_Data()
			: base()
		{
			this.Category = "XIE.Tasks.CxAuxInfo";
			this.Name = "Image";
			this.IconKey = "Data-Image";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Data", new Type[] { typeof(CxImage) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ノード指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するノードが持つ画像オブジェクトを返します。
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
			if (src is CxAuxInfo_Image_Data)
			{
				base.CopyFrom(src);

				var _src = (CxAuxInfo_Image_Data)src;

				this.Index = _src.Index;

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
				var _src = (CxAuxInfo_Image_Data)src;

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
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Image_Data.Index")]
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
					var aux = ((CxAuxInfo_Image_Data)context.Instance).AuxInfo;
					if (aux != null)
					{
						for (int i = 0; i < aux.Infos.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Image_Data.Data")]
		public CxImage Data
		{
			get { return m_Data; }
			private set { m_Data = value; }
		}
		[NonSerialized]
		private CxImage m_Data = null;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			this.AuxInfo = (IxAuxInfoImages)e.AuxInfo;
			if (0 <= this.Index && this.Index < this.AuxInfo.Datas.Length)
				this.Data = this.AuxInfo.Datas[this.Index];
			else
				this.Data = null;

			this.DataOut[0].Data = this.Data;
		}
		[NonSerialized]
		private IxAuxInfoImages AuxInfo = null;

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
						case 0: this.Index = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			var aux = (IxAuxInfoImages)e.AuxInfo;
			if (0 <= this.Index && this.Index < aux.Datas.Length)
				this.Data = aux.Datas[this.Index];
			else
				this.Data = null;
			this.DataOut[0].Data = this.Data;
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

				// task#_Data = ((IxAuxInfoImages)e.AuxInfo).Datas[index];
				//                                ~~~~~(1)~        ~(4)~
				//               ~~~~~~~~~~~~~~~~~~~~~~(2)~             
				//              ~~~~~~~~~~~~~~~~~~~~~~~(3)~~~~~~~~
				{
					// 1) e.AuxInfo
					var e_AuxInfo = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("e"), "AuxInfo");

					// 2) (IxAuxInfoImages)e.AuxInfo
					var aux = new CodeCastExpression(typeof(IxAuxInfoImages), e_AuxInfo);

					// 3) ((IxAuxInfoImages)e.AuxInfo).Datas
					var datas = new CodePropertyReferenceExpression(aux, "Datas");

					// 4) index
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// E) task#_Data = ((IxAuxInfoImages)e.AuxInfo).Datas[index];
					scope.Add(new CodeAssignStatement(
						new CodeExtraVariable(e.GetVariable(this, this.DataOut[0])),
						new CodeIndexerExpression(datas, index)
						));
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			if (this.m_OutputImage == null)
				this.m_OutputImage = new CxImage();
			if (!update && m_OutputImage.IsValid)
				view.Image = m_OutputImage;
			else
				ApiHelper.OutputImage(view, this.m_OutputImage, this.DataOut[0].Data);
		}
		[NonSerialized]
		private CxImage m_OutputImage = new CxImage();

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// DataPorts
	// 

	#region DataIn.Data

	/// <summary>
	/// タスクユニットのデータ入力ポートを参照します。（ツールバーの Task Property でポートの個数を設定してください。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskUnit_DataIn_Data : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskUnit_DataIn_Data()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">参照先のタスク</param>
		public CxTaskUnit_DataIn_Data(CxTaskUnit task)
			: base()
		{
			_Constructor();
			this.SetOwnerTaskflow(task);
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "XIE.Tasks.CxTaskflow";
			this.Name = "DataIn";
			this.IconKey = "Node-DataIn";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Data", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ポート指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するポートのデータを返します。
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
			if (src is CxTaskUnit_DataIn_Data)
			{
				base.CopyFrom(src);

				var _src = (CxTaskUnit_DataIn_Data)src;

				this.Index = _src.Index;

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
				var _src = (CxTaskUnit_DataIn_Data)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataIn_Data.Index")]
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
					var task = ((CxTaskUnit)context.Instance).GetOwnerTaskflow();
					if (task != null)
					{
						for (int i = 0; i < task.DataIn.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataIn_Data.Data")]
		public object Data
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length <= 0) return null;
				return this.DataOut[0].Data;
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

			if (this.GetOwnerTaskflow() != null)
			{
				var port = this.GetOwnerTaskflow().DataIn;
				if (0 <= this.Index && this.Index < port.Length)
					this.DataOut[0].Data = this.GetOwnerTaskflow().DataIn[this.Index].Data;
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

				// task#_Data = this.DataIn[index].Data;
				{
					// dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// this.DataIn[index].Data
					var port = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataIn");
					var port_index = new CodeIndexerExpression(port, index);
					var port_index_data = new CodePropertyReferenceExpression(port_index, "Data");
					var cast_data = new CodeCastExpression(dst.Type, port_index_data);

					// task#_Data = (Xxxxxx)this.DataIn[index].Data;
					scope.Add(dst.Assign(cast_data));
				}
			}
		}

		#endregion
	}

	#endregion

	#region DataParam.Data

	/// <summary>
	/// タスクユニットのパラメータポートを参照します。（ツールバーの Task Property でポートの個数を設定してください。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskUnit_DataParam_Data : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskUnit_DataParam_Data()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">参照先のタスク</param>
		public CxTaskUnit_DataParam_Data(CxTaskUnit task)
			: base()
		{
			_Constructor();
			this.SetOwnerTaskflow(task);
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "XIE.Tasks.CxTaskflow";
			this.Name = "DataParam";
			this.IconKey = "Node-DataParam";

			this.DataIn = new CxTaskPortIn[]
			{
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Data", new Type[] { typeof(object) }),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ポート指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 指定された指標に該当するポートのデータを返します。
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
			if (src is CxTaskUnit_DataParam_Data)
			{
				base.CopyFrom(src);

				var _src = (CxTaskUnit_DataParam_Data)src;

				this.Index = _src.Index;

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
				var _src = (CxTaskUnit_DataParam_Data)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataParam_Data.Index")]
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
					var task = ((CxTaskUnit)context.Instance).GetOwnerTaskflow();
					if (task != null)
					{
						for (int i = 0; i < task.DataParam.Length; i++)
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
		/// 出力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataParam_Data.Data")]
		public object Data
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length <= 0) return null;
				return this.DataOut[0].Data;
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

			if (this.GetOwnerTaskflow() != null)
			{
				var port = this.GetOwnerTaskflow().DataParam;
				if (0 <= this.Index && this.Index < port.Length)
					this.DataOut[0].Data = this.GetOwnerTaskflow().DataParam[this.Index].Data;
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

				// task#_Data = this.DataParam[index].Data;
				{
					// dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// this.DataParam[index].Data
					var port = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataParam");
					var port_index = new CodeIndexerExpression(port, index);
					var port_index_data = new CodePropertyReferenceExpression(port_index, "Data");
					var cast_data = new CodeCastExpression(dst.Type, port_index_data);

					// task#_Data = (Xxxxxx)this.DataParam[index].Data;
					scope.Add(dst.Assign(cast_data));
				}
			}
		}

		#endregion
	}

	#endregion

	#region DataOut.Data

	/// <summary>
	/// タスクユニットのデータ出力ポートを参照します。（ツールバーの Task Property でポートの個数を設定してください。）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskUnit_DataOut_Data : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskUnit_DataOut_Data()
			: base()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">参照先のタスク</param>
		public CxTaskUnit_DataOut_Data(CxTaskUnit task)
			: base()
		{
			_Constructor();
			this.SetOwnerTaskflow(task);
		}

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.Category = "XIE.Tasks.CxTaskflow";
			this.Name = "DataOut";
			this.IconKey = "Node-DataOut";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Data", new Type[] { typeof(object) }),
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
			/// 指定された指標に該当するポートに設定するデータ
			/// </summary>
			DataIn0,

			/// <summary>
			/// ポート指標 [0~]
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
			if (src is CxTaskUnit_DataOut_Data)
			{
				base.CopyFrom(src);

				var _src = (CxTaskUnit_DataOut_Data)src;

				this.Index = _src.Index;

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
				var _src = (CxTaskUnit_DataOut_Data)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 指標 [範囲:0~]
		/// </summary>
		[TypeConverter(typeof(IndexConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataOut_Data.Index")]
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
					var task = ((CxTaskUnit)context.Instance).GetOwnerTaskflow();
					if (task != null)
					{
						for (int i = 0; i < task.DataOut.Length; i++)
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

		#region プロパティ: (Inputs)

		/// <summary>
		/// 入力データ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Inputs")]
		[CxDescription("P:XIE.Tasks.CxTaskUnit_DataOut_Data.Data")]
		public object Data
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length <= 0) return null;
				return this.DataOut[0].Data;
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

			if (this.GetOwnerTaskflow() != null)
			{
				var port = this.GetOwnerTaskflow().DataOut;
				if (0 <= this.Index && this.Index < port.Length)
					this.GetOwnerTaskflow().DataOut[this.Index].Data = this.DataIn[0].Data;
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

				// this.DataOut[index].Data = task#_Data;
				{
					// parameters
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// this.DataOut[index].Data
					var port = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataOut");
					var port_index = new CodeIndexerExpression(port, index);
					var port_index_data = new CodePropertyReferenceExpression(port_index, "Data");

					var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
					if (connected)
					{
						// src
						var src = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// this.DataOut[index].Data = task#_Data;
						scope.Add(new CodeAssignStatement(port_index_data, src));
					}
					else
					{
						// this.DataOut[index].Data = null;
						scope.Add(new CodeAssignStatement(port_index_data, CodeLiteral.Null));
					}
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// Utilities
	// 

	#region CxGrabber

	/// <summary>
	/// 画像取り込み処理（ワンショット）
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxGrabber_OneShot : CxTaskUnit
		, IxTaskOutputImage
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabber_OneShot()
			: base()
		{
			this.Category = "XIE.Media.CxGrabber";
			this.Name = "OneShot";
			this.IconKey = "Grab-Oneshot";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Controller", new Type[] { typeof(XIE.Media.CxCamera), typeof(XIE.Media.CxMediaPlayer) }),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Dst", new Type[] {typeof(CxImage)}),
				new CxTaskPortIn("MediaType", new Type[] {typeof(XIE.Media.ExMediaType)}),
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Dst", new Type[] {typeof(CxImage)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// カメラのコントローラまたはメディアプレーヤー
			/// </summary>
			DataIn0,

			/// <summary>
			/// 出力画像 [Model×Channels: U8(1)×1ch, U8(3)×1ch, U8(4)×1ch, U8(1)×3ch, U8(1)×4ch] ※ サイズはコントローラによって異なります。要素モデルやチャネル数が許容値でなければ内部で再確保されます。
			/// </summary>
			DataParam0,

			/// <summary>
			/// 取得するメディアの種類
			/// </summary>
			DataParam1,

			/// <summary>
			/// タイムアウト (msec)
			/// </summary>
			DataParam2,

			/// <summary>
			/// 出力画像。外部から指定された場合は、そのオブジェクトを返します。未指定の場合は内部で生成して返します。
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
			if (src is CxGrabber_OneShot)
			{
				base.CopyFrom(src);

				var _src = (CxGrabber_OneShot)src;

				this.MediaType = _src.MediaType;
				this.Timeout = _src.Timeout;

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
				var _src = (CxGrabber_OneShot)src;

				if (this.MediaType != _src.MediaType) return false;
				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 取得するメディアの種類
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxGrabber_OneShot.MediaType")]
		public XIE.Media.ExMediaType MediaType
		{
			get { return m_MediaType; }
			set { m_MediaType = value; }
		}
		private XIE.Media.ExMediaType m_MediaType = XIE.Media.ExMediaType.Video;

		/// <summary>
		/// タイムアウト (msec)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxGrabber_OneShot.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 5000;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Setup(object sender, CxTaskSetupEventArgs e)
		{
			if (this.DataIn[0].Data is XIE.Media.CxCamera)
			{
				var controller = (XIE.Media.CxCamera)this.DataIn[0].Data;
				var image_size = controller.GetFrameSize();
				var dst = new CxImage(image_size);
				this.DataOut[0].Data = dst;
			}
			if (this.DataIn[0].Data is XIE.Media.CxMediaPlayer)
			{
				var controller = (XIE.Media.CxMediaPlayer)this.DataIn[0].Data;
				var image_size = controller.GetFrameSize();
				var dst = new CxImage(image_size);
				this.DataOut[0].Data = dst;
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

			var dst = (this.DataParam[0].CheckValidity())
				? (CxImage)this.DataParam[0].Data
				: new CxImage();
			if (this.DataParam[1].CheckValidity())
				this.MediaType = (XIE.Media.ExMediaType)this.DataParam[1].Data;
			if (this.DataParam[2].CheckValidity())
				this.Timeout = Convert.ToInt32(this.DataParam[2].Data);

			if (this.DataIn[0].Data is XIE.Media.CxCamera)
			{
				var controller = (XIE.Media.CxCamera)this.DataIn[0].Data;
				bool is_running = controller.IsRunning;

				#region XIE.Media.CxCamera
				try
				{
					using (var grabber = controller.CreateGrabber(this.MediaType))
					{
						grabber.Notify += delegate(object _sender, XIE.Media.CxGrabberArgs _e)
						{
							dst.CopyFrom(_e);
							_e.Cancellation = true;
						};
						grabber.Start();

						// 露光していなければ開始する.
						if (!is_running)
							controller.Start();

						bool ans = grabber.Wait(this.Timeout);
						if (ans == false)
							throw new CxException(ExStatus.Timeout);
					}
				}
				finally
				{
					// 本タスク内で実行前に露光開始した場合は停止する.
					if (!is_running)
						controller.Stop();
				}
				#endregion
			}
			else if (this.DataIn[0].Data is XIE.Media.CxMediaPlayer)
			{
				var controller = (XIE.Media.CxMediaPlayer)this.DataIn[0].Data;
				bool is_running = controller.IsRunning;

				#region XIE.Media.CxMediaPlayer
				try
				{
					using (var grabber = controller.CreateGrabber(this.MediaType))
					{
						grabber.Notify += delegate(object _sender, XIE.Media.CxGrabberArgs _e)
						{
							dst.CopyFrom(_e);
							_e.Cancellation = true;
						};
						grabber.Start();

						// 露光していなければ開始する.
						if (!is_running)
							controller.Start();

						bool ans = grabber.Wait(this.Timeout);
						if (ans == false)
							throw new CxException(ExStatus.Timeout);
					}
				}
				finally
				{
					// 本タスク内で実行前に露光開始した場合は停止する.
					if (!is_running)
						controller.Stop();
				}
				#endregion
			}
			else
			{
				throw new System.ArgumentException(this.DataIn[0].Name);
			}

			// 出力.
			this.DataOut[0].Data = dst;
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
			base.GenerateDeclarationCode(sender, e, scope);

			if (e.TargetMethod.Name == "Execute")
			{
				// XIE.CxImage m_task#_Dst;
				{
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var m_dst = new CodeMemberField(dst.Type, string.Format("m_{0}", dst.VariableName));
					m_dst.CustomAttributes.Add(
						new CodeAttributeDeclaration(
							new CodeTypeReference(typeof(NonSerializedAttribute))
						));
					e.TargetType.Members.Add(m_dst);
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

				var connected = !Array.Exists(this.DataIn, (item) => { return (item.IsConnected == false); });
				if (connected)
				{
					var controller_Notify = string.Format("{0}_Notify", e.GetVariable(this.DataIn[0]).Key);

					// task#_Dst
					var dst = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var m_dst = new CodeExtraVariable(string.Format("m_{0}", dst.VariableName), dst.Type);

					#region 初期化/実行:
					{
						// task$_Controller
						var controller = new CodeExtraVariable(e.GetVariable(this.DataIn[0]));

						// media_type
						var media_type = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.MediaType));

						// timeout
						var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[2], CodeLiteral.From(this.Timeout));

						var brace = new CodeTryCatchFinallyStatement();
						scope.Add(brace);
						// try
						{
							// var is_running = task$_Controller.IsRunning;
							var is_running = new CodeExtraVariable("is_running", typeof(bool));
							brace.TryStatements.Add(is_running.Declare(controller.Ref("IsRunning")));

							// ref) task#_Dst = task$_Xxxx;
							// new) task#_Dst = new XIE.CxImage();
							brace.TryStatements.Add(dst.Assign(
								ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From<XIE.CxImage>())
								));

							// m_task_Dst = task_Dst;
							brace.TryStatements.Add(m_dst.Assign(dst));

							// var grabber = task$_Controller.CreateGrabber(media_type);
							var grabber = new CodeExtraVariable("grabber", typeof(XIE.Media.CxGrabber));
							brace.TryStatements.Add(grabber.Declare(controller.Call("CreateGrabber", media_type)));

							// grabber.Notify += new XIE.Media.CxGrabberHandler(controller_Notify);
							brace.TryStatements.Add(
								new CodeAttachEventStatement(
									grabber,
									"Notify",
									new CodeDelegateCreateExpression(
										new CodeTypeReference("XIE.Media.CxGrabberHandler"),
										new CodeThisReferenceExpression(),
										controller_Notify
									)
								));

							// grabber.Start();
							brace.TryStatements.Add(grabber.Call("Start"));

							// if (is_running == false)
							//     task$_Controller.Start();
							{
								var if1 = new CodeConditionStatement();
								brace.TryStatements.Add(if1);

								if1.Condition = is_running.Equal(CodeLiteral.From(false));
								if1.TrueStatements.Add(controller.Call("Start"));
							}

							// var ans = grabber.Wait(timeout);
							var ans = new CodeExtraVariable("ans", typeof(bool));
							brace.TryStatements.Add(ans.Declare(grabber.Call("Wait", timeout)));

							// if (ans == false)
							//     throw new XIE.CxException(XIE.ExStatus.Timeout);
							{
								var if1 = new CodeConditionStatement();
								brace.TryStatements.Add(if1);

								if1.Condition = ans.Equal(CodeLiteral.From(false));
								if1.TrueStatements.Add(new CodeExtraThrow(typeof(XIE.CxException), CodeLiteral.From(XIE.ExStatus.Timeout)));
							}

							// if (is_running == false)
							//     task$_Controller.Stop();
							{
								var if1 = new CodeConditionStatement();
								brace.TryStatements.Add(if1);

								if1.Condition = is_running.Equal(CodeLiteral.From(false));
								if1.TrueStatements.Add(controller.Call("Stop"));
							}
						}
						// finally
						{
							// m_task_Dst = null;
							brace.FinallyStatements.Add(m_dst.Assign(CodeLiteral.From(null)));
						}
					}
					#endregion

					#region 取り込み処理: (controller_Notify)
					{
						// void controller_Notify(object sender, XIE.Media.CxGrabberArgs e)
						var member = new CodeMemberMethod();
						member.Comments.Add(new CodeCommentStatement("<summary>", true));
						member.Comments.Add(new CodeCommentStatement(XIE.AxiTextStorage.GetValue("M:XIE.Media.CxCameraSelectionForm.Grabber_Notify(System.Object,XIE.Media.CxGrabberArgs)"), true));
						member.Comments.Add(new CodeCommentStatement("</summary>", true));
						member.Name = controller_Notify;
						member.ReturnType = new CodeTypeReference(typeof(void));
						member.Attributes = MemberAttributes.Private;
						member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
						member.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XIE.Media.CxGrabberArgs), "e"));

						#region 処理コード:
						if (this.DataIn[0].Data is XIE.Media.CxCamera)
						{
							// e
							var args_e = new CodeArgumentReferenceExpression("e");

							// m_dst.CopyFrom(e);
							member.Statements.Add(m_dst.Call("CopyFrom", args_e));

							// e.Cancellation = true;
							member.Statements.Add(
								new CodeAssignStatement(new CodePropertyReferenceExpression(args_e, "Cancellation"), CodeLiteral.From(true))
								);
						}
						else if (this.DataIn[0].Data is XIE.Media.CxMediaPlayer)
						{
							// e
							var args_e = new CodeArgumentReferenceExpression("e");

							// m_dst.CopyFrom(e);
							member.Statements.Add(m_dst.Call("CopyFrom", args_e));

							// e.Cancellation = true;
							member.Statements.Add(
								new CodeAssignStatement(new CodePropertyReferenceExpression(args_e, "Cancellation"), CodeLiteral.From(true))
								);
						}
						#endregion

						e.TargetType.Members.Add(member);
					}
					#endregion
				}
			}
		}

		#endregion

		#region IxTaskOutputImage の実装:

		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void IxTaskOutputImage.OutputImage(XIE.GDI.CxImageView view, bool update)
		{
			view.Image = this.DataOut[0].Data as CxImage;
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// IO
	// 

	#region CxSerialPort.Read

	/// <summary>
	/// Read メソッド。データを読み込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxSerialPort_Read : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPort_Read()
			: base()
		{
			this.Category = "XIE.IO.CxSerialPort";
			this.Name = "Read";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.IO.CxSerialPort)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] {typeof(int)}),
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Data", new Type[] {typeof(byte[])}),
				new CxTaskPortOut("Length", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// シリアル通信ポートオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// データ長 (bytes) [0,1~] 
			/// </summary>
			DataParam0,

			/// <summary>
			/// タイムアウト (msec) [-1=無限,0~=有限]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 読み込まれたデータを返します。
			/// </summary>
			DataOut0,

			/// <summary>
			/// 実際に読み込まれたデータの長さ (bytes) を返します。
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
			if (src is CxSerialPort_Read)
			{
				base.CopyFrom(src);

				var _src = (CxSerialPort_Read)src;

				this.Length = _src.Length;
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
				var _src = (CxSerialPort_Read)src;

				if (this.Length != _src.Length) return false;
				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// データ長 (bytes) [0,1~] 
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Read.Length")]
		public int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

		/// <summary>
		/// タイムアウト (msec) [-1=無限,0~=有限]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Read.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 読み込まれたデータ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Read.Data")]
		public byte[] Data
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length == 0) return null;
				return this.DataOut[0].Data as byte[];
			}
		}

		/// <summary>
		/// 実際に読み込まれたデータの長さ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Read.DataLength")]
		public string DataLength
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[1].Data;
				if (data == null) return "";
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

			// 引数の取得.
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.IO.CxSerialPort)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Length = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Timeout = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			var data = new byte[this.Length];
			int length = 0;

			if (data.Length != 0)
			{
				length = body.Read(data, data.Length, this.Timeout);
			}

			// 出力.
			this.DataOut[0].Data = data;
			this.DataOut[1].Data = length;

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
					var data = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var length2 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[1]));

					// parameters
					var length1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Length));
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Timeout));

					// data = new byte[length1];
					scope.Add(data.Assign(new CodeArrayCreateExpression(typeof(byte), length1)));

					// length2 = body.Read(data, length1, timeout);
					scope.Add(length2.Assign(body.Call("Read", data, length1, timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region CxSerialPort.Readable

	/// <summary>
	/// Readable メソッド。データを読み込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxSerialPort_Readable : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPort_Readable()
			: base()
		{
			this.Category = "XIE.IO.CxSerialPort";
			this.Name = "Readable";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.IO.CxSerialPort)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] {typeof(bool)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// シリアル通信ポートオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// タイムアウト (msec) [-1=無限,0~=有限]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 読み込み可否を示すブール値を返します。
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
			if (src is CxSerialPort_Readable)
			{
				base.CopyFrom(src);

				var _src = (CxSerialPort_Readable)src;

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
				var _src = (CxSerialPort_Readable)src;

				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムアウト (msec) [-1=無限,0~=有限]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Readable.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 読み込み可否を示すブール値を返します。
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Readable.Result")]
		public string Result
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
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.IO.CxSerialPort)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Timeout = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			bool result = body.Readable(this.Timeout);

			// 出力.
			this.DataOut[0].Data = result;

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
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Timeout));

					// result = body.Readable(timeout);
					scope.Add(result.Assign(body.Call("Readable", timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region CxSerialPort.Write

	/// <summary>
	/// Write メソッド。データを書き込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxSerialPort_Write : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPort_Write()
			: base()
		{
			this.Category = "XIE.IO.CxSerialPort";
			this.Name = "Write";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.IO.CxSerialPort)}),
				new CxTaskPortIn("Data", new Type[] {typeof(byte[])}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] {typeof(int)}),
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// シリアル通信ポートオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// 書き込むデータが格納された配列
			/// </summary>
			DataIn1,

			/// <summary>
			/// 書き込むデータの長さ (bytes) ※未接続の場合は Data.Length を適用します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// タイムアウト (msec) [-1=無限,0~=有限]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 実際に書き込まれたデータの長さ (bytes) を返します。 
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
			if (src is CxSerialPort_Write)
			{
				base.CopyFrom(src);

				var _src = (CxSerialPort_Write)src;

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
				var _src = (CxSerialPort_Write)src;

				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムアウト (msec) [-1=無限,0~=有限]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Write.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 実際に書き込まれたデータの長さ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Write.Result")]
		public string Result
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
			var body = (XIE.IO.CxSerialPort)this.DataIn[0].Data;
			var data = (byte[])this.DataIn[1].Data;

			// length
			int length = 0;
			if (this.DataParam[0].CheckValidity())
				length = Convert.ToInt32(this.DataParam[0].Data);
			else
				length = data.Length;

			// timeout
			if (this.DataParam[1].CheckValidity())
				this.Timeout = Convert.ToInt32(this.DataParam[1].Data);

			// 実行.
			int result = 0;
			if (data.Length != 0)
				result = body.Write(data, length, this.Timeout);

			// 出力.
			this.DataOut[0].Data = result;

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
					var data = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var result = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var length = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], data.Ref("Length"));
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Timeout));

					// result = body.Write(data, length, timeout);
					scope.Add(result.Assign(body.Call("Write", data, length, timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region CxSerialPort.Writeable

	/// <summary>
	/// Writeable メソッド。データを書き込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxSerialPort_Writeable : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPort_Writeable()
			: base()
		{
			this.Category = "XIE.IO.CxSerialPort";
			this.Name = "Writeable";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.IO.CxSerialPort)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Result", new Type[] {typeof(bool)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// シリアル通信ポートオブジェクト
			/// </summary>
			DataIn0,

			/// <summary>
			/// タイムアウト (msec) [-1=無限,0~=有限]
			/// </summary>
			DataParam0,

			/// <summary>
			/// 書き込み可否を示すブール値を返します。
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
			if (src is CxSerialPort_Writeable)
			{
				base.CopyFrom(src);

				var _src = (CxSerialPort_Writeable)src;

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
				var _src = (CxSerialPort_Writeable)src;

				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムアウト (msec) [-1=無限,0~=有限]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Writeable.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 実際に書き込まれたデータの長さ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxSerialPort_Writeable.Result")]
		public string Result
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
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.IO.CxSerialPort)this.DataIn[0].Data;

			// timeout
			if (this.DataParam[0].CheckValidity())
				this.Timeout = Convert.ToInt32(this.DataParam[0].Data);

			// 実行.
			bool result = body.Writeable(this.Timeout);

			// 出力.
			this.DataOut[0].Data = result;

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
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Timeout));

					// result = body.Writeable(timeout);
					scope.Add(result.Assign(body.Call("Writeable", timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

	// //////////////////////////////////////////////////
	// Net
	// 

	#region CxTcpServer.Connections

	/// <summary>
	/// Connections メソッド。TCP/IP 通信サーバーに接続しているクライアント数を取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTcpServer_Connections : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServer_Connections()
			: base()
		{
			this.Category = "XIE.Net.CxTcpServer";
			this.Name = "Connections";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.Net.CxTcpServer)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Connections", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// TCP/IP 通信サーバー
			/// </summary>
			DataIn0,

			/// <summary>
			/// クライアント数を返します。[0~]
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
			if (src is CxTcpServer_Connections)
			{
				base.CopyFrom(src);

				var _src = (CxTcpServer_Connections)src;

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
				var _src = (CxTcpServer_Connections)src;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 接続しているクライアント数 [0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.CxTcpServer_Connections.Connections")]
		public int Connections
		{
			get
			{
				if (this.DataOut == null) return 0;
				if (this.DataOut.Length == 0) return 0;
				if (this.DataOut[0].Data is int)
					return (int)this.DataOut[0].Data;
				return 0;
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

			// 引数の取得.
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.Net.CxTcpServer)this.DataIn[0].Data;

			// 実行.
			var connections = body.Connections();

			// 出力.
			this.DataOut[0].Data = connections;

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
					var connections = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// connections = body.Connections();
					scope.Add(connections.Assign(new CodeIndexerExpression(body.Call("Connections"))));
				}
			}
		}

		#endregion
	}

	#endregion

	#region CxTcpServer.Stream

	/// <summary>
	/// Stream メソッド。TCP/IP 通信サーバーからソケットストリームを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTcpServer_Stream : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServer_Stream()
			: base()
		{
			this.Category = "XIE.Net.CxTcpServer";
			this.Name = "Stream";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.Net.CxTcpServer)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Index", new Type[] { typeof(int) })
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Stream", new Type[] {typeof(XIE.Net.TxSocketStream)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// TCP/IP 通信サーバー
			/// </summary>
			DataIn0,

			/// <summary>
			/// TCP/IP 通信クライアント指標 [0~]
			/// </summary>
			DataParam0,

			/// <summary>
			/// ソケットストリームを返します。
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
			if (src is CxTcpServer_Stream)
			{
				base.CopyFrom(src);

				var _src = (CxTcpServer_Stream)src;

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
				var _src = (CxTcpServer_Stream)src;

				if (this.Index != _src.Index) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// TCP/IP 通信クライアント指標 [範囲:0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTcpServer_Stream.Index")]
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

			// 引数の取得.
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.Net.CxTcpServer)this.DataIn[0].Data;

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
			var stream = body.Stream(this.Index);

			// 出力.
			this.DataOut[0].Data = stream;

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
					var stream = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var index = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Index));

					// stream = body.Stream(index);
					scope.Add(stream.Assign(body.Call("Stream", index)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region CxTcpClient.Stream

	/// <summary>
	/// Stream メソッド。TCP/IP 通信クライアントからソケットストリームを取得します。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTcpClient_Stream : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpClient_Stream()
			: base()
		{
			this.Category = "XIE.Net.CxTcpClient";
			this.Name = "Stream";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.Net.CxTcpClient)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Stream", new Type[] {typeof(XIE.Net.TxSocketStream)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// TCP/IP 通信クライアント
			/// </summary>
			DataIn0,

			/// <summary>
			/// ソケットストリームを返します。
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
			if (src is CxTcpClient_Stream)
			{
				base.CopyFrom(src);

				var _src = (CxTcpClient_Stream)src;

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
				var _src = (CxTcpClient_Stream)src;
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

			// 引数の取得.
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.Net.CxTcpClient)this.DataIn[0].Data;

			// 実行.
			var stream = body.Stream();

			// 出力.
			this.DataOut[0].Data = stream;

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
					var stream = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// stream = body.Stream();
					scope.Add(stream.Assign(body.Call("Stream")));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxSocketStream.Read

	/// <summary>
	/// Read メソッド。データを読み込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSocketStream_Read : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSocketStream_Read()
			: base()
		{
			this.Category = "XIE.Net.TxSocketStream";
			this.Name = "Read";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.Net.TxSocketStream)}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] {typeof(int)}),
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Data", new Type[] {typeof(byte[])}),
				new CxTaskPortOut("Length", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ソケットストリーム
			/// </summary>
			DataIn0,

			/// <summary>
			/// データ長 (bytes) [0,1~] 
			/// </summary>
			DataParam0,

			/// <summary>
			/// タイムアウト (msec) [-1,0,1~]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 読み込まれたデータを返します。
			/// </summary>
			DataOut0,

			/// <summary>
			/// 実際に読み込まれたデータの長さ (bytes) を返します。
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
			if (src is TxSocketStream_Read)
			{
				base.CopyFrom(src);

				var _src = (TxSocketStream_Read)src;

				this.Length = _src.Length;
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
				var _src = (TxSocketStream_Read)src;

				if (this.Length != _src.Length) return false;
				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// データ長 (bytes) [0,1~] 
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Read.Length")]
		public int Length
		{
			get { return m_Length; }
			set { m_Length = value; }
		}
		private int m_Length = 0;

		/// <summary>
		/// タイムアウト (msec) [-1,0,1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Read.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 読み込まれたデータ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Read.Data")]
		public byte[] Data
		{
			get
			{
				if (this.DataOut == null) return null;
				if (this.DataOut.Length == 0) return null;
				return this.DataOut[0].Data as byte[];
			}
		}

		/// <summary>
		/// 実際に読み込まれたデータの長さ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Read.DataLength")]
		public string DataLength
		{
			get
			{
				if (this.DataOut == null) return "";
				if (this.DataOut.Length == 0) return "";
				object data = this.DataOut[1].Data;
				if (data == null) return "";
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

			// 引数の取得.
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.Net.TxSocketStream)this.DataIn[0].Data;

			for (int iii = 0; iii < this.DataParam.Length; iii++)
			{
				if (this.DataParam[iii].CheckValidity())
				{
					switch (iii)
					{
						case 0: this.Length = Convert.ToInt32(this.DataParam[iii].Data); break;
						case 1: this.Timeout = Convert.ToInt32(this.DataParam[iii].Data); break;
					}
				}
			}

			// 実行.
			var data = new byte[this.Length];
			int length = 0;

			if (data.Length != 0)
			{
				length = body.Read(data, data.Length, this.Timeout);
			}

			// 出力.
			this.DataOut[0].Data = data;
			this.DataOut[1].Data = length;

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
					var data = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));
					var length2 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[1]));

					// parameters
					var length1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], CodeLiteral.From(this.Length));
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Timeout));

					// data = new byte[length1];
					scope.Add(data.Assign(new CodeArrayCreateExpression(typeof(byte), length1)));

					// length2 = body.Read(data, length1, timeout);
					scope.Add(length2.Assign(body.Call("Read", data, length1, timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

	#region TxSocketStream.Write

	/// <summary>
	/// Write メソッド。データを書き込みます。
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class TxSocketStream_Write : CxTaskUnit
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxSocketStream_Write()
			: base()
		{
			this.Category = "XIE.Net.TxSocketStream";
			this.Name = "Write";
			this.IconKey = "Unit-Method";

			this.DataIn = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Body", new Type[] {typeof(XIE.Net.TxSocketStream)}),
				new CxTaskPortIn("Data", new Type[] {typeof(byte[])}),
			};
			this.DataParam = new CxTaskPortIn[]
			{
				new CxTaskPortIn("Length", new Type[] {typeof(int)}),
				new CxTaskPortIn("Timeout", new Type[] {typeof(int)}),
			};
			this.DataOut = new CxTaskPortOut[]
			{
				new CxTaskPortOut("Length", new Type[] {typeof(int)}),
			};
		}

		private enum Descriptions
		{
			/// <summary>
			/// ソケットストリーム
			/// </summary>
			DataIn0,

			/// <summary>
			/// 書き込むデータが格納された配列
			/// </summary>
			DataIn1,

			/// <summary>
			/// 書き込むデータの長さ (bytes) ※未接続の場合は Data.Length を適用します。
			/// </summary>
			DataParam0,

			/// <summary>
			/// タイムアウト (msec) [-1,0,1~]
			/// </summary>
			DataParam1,

			/// <summary>
			/// 実際に書き込まれたデータの長さ (bytes) を返します。 
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
			if (src is TxSocketStream_Write)
			{
				base.CopyFrom(src);

				var _src = (TxSocketStream_Write)src;

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
				var _src = (TxSocketStream_Write)src;

				if (this.Timeout != _src.Timeout) return false;
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムアウト (msec) [-1,0,1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Write.Timeout")]
		public int Timeout
		{
			get { return m_Timeout; }
			set { m_Timeout = value; }
		}
		private int m_Timeout = 0;

		#endregion

		#region プロパティ: (Outputs)

		/// <summary>
		/// 実際に書き込まれたデータの長さ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Outputs")]
		[CxDescription("P:XIE.Tasks.TxSocketStream_Write.This")]
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
			for (int i = 0; i < this.DataIn.Length; i++)
				this.DataIn[i].CheckValidity(true);
			var body = (XIE.Net.TxSocketStream)this.DataIn[0].Data;
			var data = (byte[])this.DataIn[1].Data;

			// length
			int length1 = 0;
			if (this.DataParam[0].CheckValidity())
				length1 = Convert.ToInt32(this.DataParam[0].Data);
			else
				length1 = data.Length;

			// timeout
			if (this.DataParam[1].CheckValidity())
				this.Timeout = Convert.ToInt32(this.DataParam[1].Data);

			// 実行.
			int length2 = 0;
			if (data.Length != 0)
				length2 = body.Write(data, length1, this.Timeout);

			// 出力.
			this.DataOut[0].Data = length2;

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
					var data = new CodeExtraVariable(e.GetVariable(this.DataIn[1]));
					var length2 = new CodeExtraVariable(e.GetVariable(this, this.DataOut[0]));

					// parameters
					var length1 = ApiHelper.CodeOptionalExpression(e, this.DataParam[0], data.Ref("Length"));
					var timeout = ApiHelper.CodeOptionalExpression(e, this.DataParam[1], CodeLiteral.From(this.Timeout));

					// length2 = body.Write(data, length1, timeout);
					scope.Add(length2.Assign(body.Call("Write", data, length1, timeout)));
				}
			}
		}

		#endregion
	}

	#endregion

}
