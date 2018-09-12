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

namespace XIE.Tasks
{
	/// <summary>
	/// タスク参照クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskReference : CxTaskUnit
		, ISerializable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskReference()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">参照先のタスク</param>
		public CxTaskReference(CxTaskUnit task)
		{
			if (task != null)
			{
				this.DependencyTask = task;
				this.DependencyTaskID = task.UniqueID;
				this.Sync();
			}
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (シグネチャコンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxTaskReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "DependencyTaskID":
							{
								this.DependencyTaskID = this.GetValue<string>(info, "DependencyTaskID");
							}
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

			info.AddValue("DependencyTaskID", this.DependencyTaskID);
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

			if (src is CxTaskReference)
			{
				#region 同一型:
				var _src = (CxTaskReference)src;

				this.DependencyTask = _src.DependencyTask;
				this.DependencyTaskID = _src.DependencyTaskID;

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

			var _src = (CxTaskReference)src;

			if (base.ContentEquals(src) == false) return false;

			if (ReferenceEquals(this.DependencyTask, _src.DependencyTask)) return false;

			return true;
		}

		#endregion

		#region プロパティ: (参照先のタスクユニット)

		/// <summary>
		/// 参照先のタスクユニット
		/// </summary>
		[Browsable(true)]
		[XmlIgnore]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskReference.DependencyTask")]
		public CxTaskUnit DependencyTask
		{
			get { return m_DependencyTask; }
			set { m_DependencyTask = value; }
		}
		private CxTaskUnit m_DependencyTask = null;

		/// <summary>
		/// 参照先のタスクユニットの ID
		/// </summary>
		[Browsable(true)]
		[XmlIgnore]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskReference.DependencyTaskID")]
		public string DependencyTaskID
		{
			get { return m_DependencyTaskID; }
			set { m_DependencyTaskID = value; }
		}
		private string m_DependencyTaskID = "";

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

		#region メソッド: (同期)

		/// <summary>
		/// 自身のデータ出力ポートを切断します。
		/// </summary>
		/// <param name="tasks">検索対象のタスク</param>
		private void Disconnect(IEnumerable<CxTaskUnit> tasks)
		{
			foreach (var port in this.DataOut)
				port.Disconnect(tasks);
		}

		/// <summary>
		/// 参照先のタスクとの同期が可能か否かを検査します。
		/// </summary>
		/// <returns>
		///		参照先のタスクが有効で、データ出力ポートの個数が一致していれば true を返します。
		///		それ以外は false を返します。
		///		false の場合はデータ出力ポートの切断を行ってから Sync を呼び出す必要があります。
		/// </returns>
		private bool CanSync()
		{
			var _src = this.DependencyTask;
			if (_src == null) return false;
			if (_src.DataOut == null) return false;
			if (_src.DataOut.Length != this.DataOut.Length) return false;
			return true;
		}

		/// <summary>
		/// 参照先のタスクとの同期 (※注意：事前に CanSync で接続可否の確認を行ってください。)
		/// </summary>
		private void Sync()
		{
			var _src = this.DependencyTask;
			if (_src == null)
			{
				#region 配列要素数の初期化:
				this.DataOut = new CxTaskPortOut[0];
				#endregion

				#region 内容のリセット:
				{
					this.Category = "Unknown";
					this.Name = "Unknown";
					this.IconKey = "Notify-None";
					this.IconImage = null;
					this.DependencyTaskID = "";
				}
				#endregion
			}
			else
			{
				#region 配列要素数の初期化:
				if (_src.DataOut == null)
					this.DataOut = new CxTaskPortOut[0];
				else if (_src.DataOut.Length != this.DataOut.Length)
				{
					this.DataOut = new CxTaskPortOut[_src.DataOut.Length];
					for (int i = 0; i < this.DataOut.Length; i++)
						this.DataOut[i] = new CxTaskPortOut(_src.DataOut[i].Name, _src.DataOut[i].Types);
				}
				#endregion

				#region 内容の複製:
				{
					// (!) UniqueID は複製対象外.
					//this.UniqueID = _src.UniqueID;

					this.Category = _src.Category;
					this.Name = _src.Name;
					this.IconKey = _src.IconKey;
					this.IconImage = _src.IconImage;
					//this.Location = _src.Location;
					//this.Breakpoint = _src.Breakpoint;
					//this.Selected = _src.Selected;

					this.ControlOut.Data = _src.ControlOut.Data;

					for (int i = 0; i < this.DataOut.Length; i++)
					{
						this.DataOut[i].Name = _src.DataOut[i].Name;
						this.DataOut[i].Types = _src.DataOut[i].Types;
						this.DataOut[i].Data = _src.DataOut[i].Data;
					}
				}
				#endregion
			}
		}

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 処理開始前の準備
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は Form または Control です。)</param>
		/// <param name="e">イベントの内容 (ValueChanged の実行前は CxTaskValueChangedEventArgs が渡されます。)</param>
		public override void Prepare(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// 処理終了後の復旧
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は Form または Control または BackgroundWorker です。)</param>
		/// <param name="e">イベントの内容 (ValueChanged の実行後は CxTaskValueChangedEventArgs が渡されます。)</param>
		public override void Restore(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は BackgroundWorker です。)</param>
		/// <param name="e">イベントの内容</param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			// 実体の出力をタスク参照の出力に複製します。
			// 
			//      +---+
			// 1 -->|src|--> 2
			//      +---+
			// ====== | ===============================
			//      +---+
			//      |ref|--> 2'
			//      +---+
			// 
			if (e.Taskflow != null)
			{
				if (this.CanSync() == false)
				{
					var tasks = e.Taskflow.GetTasks(true);
					this.Disconnect(tasks);
				}
				this.Sync();
			}
		}

		/// <summary>
		/// プロパティ値が変更されたときに実行する処理
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は PropertyGrid です。)</param>
		/// <param name="e">イベントの内容</param>
		public override void ValueChanged(object sender, CxTaskValueChangedEventArgs e)
		{
			base.ValueChanged(sender, e);
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
				if (this.DependencyTask != null)
				{
					#region 参照先が設定されていれば、参照先の出力を参照する.
					for (int i = 0; i < this.DataOut.Length; i++)
					{
						var port = this.DataOut[i];
						var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
						var dependency_key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this.DependencyTask, this.DependencyTask.DataOut[i]);
						e.Variables[key] = e.Variables[dependency_key];
					}
					#endregion
				}
				else
				{
					#region 参照先が設定されていなければ、一般的なタスクとして振る舞う.
					base.GenerateDeclarationCode(sender, e, scope);
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
			if (e.TargetMethod.Name == "Execute")
			{
				//scope.Add(new CodeSnippetStatement());
				//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", e.TaskNames[this], this.Name, this.Category)));

				// 参照しているだけなので処理の実体はない.
				{
				}
			}
		}

		#endregion
	}
}
