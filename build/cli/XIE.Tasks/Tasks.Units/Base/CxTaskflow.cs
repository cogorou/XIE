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
	/// タスクフロークラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public abstract class CxTaskflow : CxTaskUnit
		, ISerializable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflow()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="iconKey"></param>
		public CxTaskflow(string name, string iconKey)
		{
			this.Name = name;
			this.IconKey = iconKey;
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxTaskflow(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.m_TaskUnits = new List<CxTaskUnit>();

			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "SheetType":
							this.SheetType = this.GetValue<int>(info, entry.Name);
							break;
						case "ColorType":
							this.ColorType = this.GetValue<int>(info, entry.Name);
							break;
						case "LineType":
							this.LineType = this.GetValue<int>(info, entry.Name);
							break;
						case "BlockType":
							this.BlockType = this.GetValue<int>(info, entry.Name);
							break;
						case "DefaultImageIndex":
							this.DefaultImageIndex = this.GetValue<int>(info, entry.Name);
							break;
						case "DefaultReportIndex":
							this.DefaultReportIndex = this.GetValue<int>(info, entry.Name);
							break;
						case "CurrentIndex":
							this.CurrentIndex = this.GetValue<int>(info, entry.Name);
							break;
						case "SheetLocation":
							this.SheetLocation = this.GetValue<Point>(info, entry.Name);
							break;
						case "TaskUnits.Count":
							{
								int count = this.GetValue<int>(info, entry.Name);
								for (int i = 0; i < count; i++)
									this.TaskUnits.Add(this.GetValue<CxTaskUnit>(info, string.Format("TaskUnits{0}", i)));
							}
							break;
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
			}

			this.LoadDependency();
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

			info.AddValue("SheetType", this.SheetType);
			info.AddValue("ColorType", this.ColorType);
			info.AddValue("LineType", this.LineType);
			info.AddValue("BlockType", this.BlockType);

			info.AddValue("DefaultImageIndex", this.DefaultImageIndex);
			info.AddValue("DefaultReportIndex", this.DefaultReportIndex);
			info.AddValue("CurrentIndex", this.CurrentIndex);
			info.AddValue("SheetLocation", this.SheetLocation);

			if (this.TaskUnits == null)
			{
				info.AddValue("TaskUnits.Count", 0);
			}
			else
			{
				this.SaveDependency();
				info.AddValue("TaskUnits.Count", this.TaskUnits.Count);
				for (int i = 0; i < this.TaskUnits.Count; i++)
					info.AddValue(string.Format("TaskUnits{0}", i), this.TaskUnits[i]);
			}
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

			if (src is CxTaskflow)
			{
				#region 同一型:
				var _src = (CxTaskflow)src;

				this.SheetType = _src.SheetType;
				this.ColorType = _src.ColorType;
				this.LineType = _src.LineType;
				this.BlockType = _src.BlockType;

				this.DefaultImageIndex = _src.DefaultImageIndex;
				this.DefaultReportIndex = _src.DefaultReportIndex;
				this.SheetLocation = _src.SheetLocation;
				this.CurrentIndex = _src.CurrentIndex;

				_src.SaveDependency();
				this.TaskUnits.Clear();
				for (int i = 0; i < _src.TaskUnits.Count; i++)
				{
					if (_src.TaskUnits[i] == null)
						this.TaskUnits.Add(null);
					else
						this.TaskUnits.Add((CxTaskUnit)_src.TaskUnits[i].Clone());
				}
				this.LoadDependency();

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

			{
				#region 同一型:
				var _src = (CxTaskflow)src;

				if (base.ContentEquals(src) == false) return false;

				if (this.SheetType != _src.SheetType) return false;
				if (this.ColorType != _src.ColorType) return false;
				if (this.LineType != _src.LineType) return false;
				if (this.BlockType != _src.BlockType) return false;

				if (this.DefaultImageIndex != _src.DefaultImageIndex) return false;
				if (this.DefaultReportIndex != _src.DefaultReportIndex) return false;
				if (this.SheetLocation != _src.SheetLocation) return false;
				if (this.CurrentIndex != _src.CurrentIndex) return false;

				if (this.TaskUnits == null && _src.TaskUnits != null) return false;
				if (this.TaskUnits != null && _src.TaskUnits == null) return false;
				if (this.TaskUnits != null && _src.TaskUnits != null)
				{
					if (this.TaskUnits.Count != _src.TaskUnits.Count) return false;
					for (int i = 0; i < this.TaskUnits.Count; i++)
					{
						var dst_task = this.TaskUnits[i];
						var src_task = _src.TaskUnits[i];
						if (dst_task == null && src_task != null) return false;
						if (dst_task != null && src_task == null) return false;
						if (dst_task != null && src_task != null)
						{
							if (dst_task.ContentEquals(src_task) == false) return false;
						}
					}
				}
				#endregion
			}

			return true;
		}

		#endregion

		#region プロパティ: (描画属性)

		/// <summary>
		/// シートタイプ [1~9]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.SheetType")]
		public int SheetType
		{
			get { return m_SheetType; }
			set { m_SheetType = value; }
		}
		private int m_SheetType = 5;

		/// <summary>
		/// 背景色タイプ [0:Dark, 1:Light]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.ColorType")]
		public int ColorType
		{
			get { return m_ColorType; }
			set { m_ColorType = value; }
		}
		private int m_ColorType = 0;

		/// <summary>
		/// 接続線タイプ [0:Polyine, 1:Spline]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.LineType")]
		public int LineType
		{
			get { return m_LineType; }
			set { m_LineType = value; }
		}
		private int m_LineType = 0;

		/// <summary>
		/// ブロックタイプ [0:Large, 1:Small]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.BlockType")]
		public int BlockType
		{
			get { return m_BlockType; }
			set { m_BlockType = value; }
		}
		private int m_BlockType = 0;

		#endregion

		#region プロパティ: (実行属性)

		/// <summary>
		/// 画像ビューに描画する画像の指標 [-1:終端、0~:固定]
		/// </summary>
		/// <returns>
		///		最上位の Taskflow の GetTasks(true) メソッドが返すコレクションの指標を示します。
		///		但し、IxTaskOutputImage を実装するタスクが対象です。それ以外は無視します。
		/// </returns>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.DefaultImageIndex")]
		public int DefaultImageIndex
		{
			get { return m_DefaultImageIndex; }
			set { m_DefaultImageIndex = value; }
		}
		private int m_DefaultImageIndex = -1;

		/// <summary>
		/// データグリッドに描画するレポートの指標 [-1:終端、0~:固定]
		/// </summary>
		/// <returns>
		///		最上位の Taskflow の GetTasks(true) メソッドが返すコレクションの指標を示します。
		///		但し、IxTaskOutputReport を実装するタスクが対象です。それ以外は無視します。
		/// </returns>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.DefaultReportIndex")]
		public int DefaultReportIndex
		{
			get { return m_DefaultReportIndex; }
			set { m_DefaultReportIndex = value; }
		}
		private int m_DefaultReportIndex = -1;

		#endregion

		#region プロパティ: (編集関連)

		/// <summary>
		/// シートの位置
		/// </summary>
		/// <returns>
		///		picturebox の Location を示します。
		///		0,0 から開始し、右下へ進むとマイナス値になります。
		/// </returns>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.SheetLocation")]
		public Point SheetLocation
		{
			get { return m_SheetLocation; }
			set { m_SheetLocation = value; }
		}
		private Point m_SheetLocation = new Point();

		/// <summary>
		/// 現在編集されている子タスクの指標 [-1:なし、0~:子タスク]
		/// </summary>
		/// <returns>
		///		このクラスの TaskUnits プロパティの指標を示します。
		///		Taskflow がネストしている場合は CurrentIndex を追跡していく必要があります。
		/// </returns>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskComposite.CurrentIndex")]
		public int CurrentIndex
		{
			get { return m_CurrentIndex; }
			set { m_CurrentIndex = value; }
		}
		private int m_CurrentIndex = -1;

		#endregion

		#region プロパティ: (タスクユニットコレクション)

		/// <summary>
		/// タスクユニットコレクション
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskflow.TaskUnits")]
		public List<CxTaskUnit> TaskUnits
		{
			get { return m_TaskUnits; }
			private set { m_TaskUnits = value; }
		}
		[NonSerialized]
		private List<CxTaskUnit> m_TaskUnits = new List<CxTaskUnit>();

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void Execute(object sender, CxTaskExecuteEventArgs e)
		{
			if (e.ControlSyntax != ExControlSyntax.None) return;

			if (this.TaskUnits == null) return;
			foreach (var task in this.TaskUnits)
			{
				if (e.Callback == null)
					task.Execute(sender, e);
				else
					e.Callback(task, e);

				if (e.ControlSyntax != ExControlSyntax.None) break;
			}
		}

		#endregion

		#region メソッド: (接続データ)

		/// <summary>
		/// 出力データのリセット
		/// </summary>
		public override void Reset()
		{
			this.ControlOut.Data = null;
			for (int i = 0; i < this.DataOut.Length; i++)
				this.DataOut[i].Data = null;
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

		#region メソッド: (子タスクの操作)

		/// <summary>
		/// 子タスクの取得
		/// </summary>
		/// <param name="recursive">再帰的</param>
		/// <returns>
		///		タスクユニットのコレクションを返します。
		/// </returns>
		public virtual List<CxTaskUnit> GetTasks(bool recursive)
		{
			var task_list = new List<CxTaskUnit>();
			foreach (var child in this.TaskUnits)
			{
				task_list.Add(child);
				if (recursive)
				{
					if (child is CxTaskflow)
					{
						var taskflow = (CxTaskflow)child;
						task_list.AddRange(taskflow.GetTasks(recursive));
					}
				}
			}
			return task_list;
		}

		/// <summary>
		/// 子タスクの取得 (スコープ内のみ)
		/// </summary>
		/// <param name="recursive">再帰的</param>
		/// <returns>
		///		タスクユニットのコレクションを返します。
		/// </returns>
		public virtual List<CxTaskUnit> GetTasksInScope(bool recursive)
		{
			var task_list = new List<CxTaskUnit>();
			foreach (var child in this.TaskUnits)
			{
				task_list.Add(child);
				if (recursive)
				{
					if ((child is CxTaskflow) && !(child is Syntax_Class))
					{
						var taskflow = (CxTaskflow)child;
						task_list.AddRange(taskflow.GetTasksInScope(recursive));
					}
				}
			}
			return task_list;
		}

		/// <summary>
		/// 子タスクの並べ替え
		/// </summary>
		/// <param name="recursive">再帰的</param>
		public virtual void SortTaskSequence(bool recursive)
		{
			CxTaskUnit current_task = null;
			if (0 <= CurrentIndex && CurrentIndex < this.TaskUnits.Count)
				current_task = this.TaskUnits[CurrentIndex];

			var schedule = this.GetExecutionSchedule();
			this.m_TaskUnits.Clear();
			this.m_TaskUnits.AddRange(schedule);
			if (recursive)
			{
				foreach (var child in schedule)
				{
					if (child is CxTaskflow)
					{
						var taskflow = (CxTaskflow)child;
						taskflow.SortTaskSequence(recursive);
					}
				}
			}

			this.CurrentIndex = this.TaskUnits.FindIndex(item => ReferenceEquals(item, current_task));
		}

		#endregion

		#region メソッド: (依存関係)

		/// <summary>
		/// 依存関係の復元
		/// </summary>
		protected virtual void LoadDependency()
		{
			var task_list = this.TaskUnits;
			foreach (var child in this.TaskUnits)
			{
				#region コントロール入力ポート:
				if (child.ControlIn != null)
				{
					var src = child.ControlIn;
					if (0 <= src.ReferenceTaskIndex && src.ReferenceTaskIndex < task_list.Count)
					{
						var task = task_list[src.ReferenceTaskIndex];
						//if (src.CanConnect(task, ExOutputPortType.ControlOut, 0))
						if (src.ReferencePortIndex == 0)
							src.Connect(task, ExOutputPortType.ControlOut, 0);
					}
				}
				#endregion

				#region データ入力ポート:
				if (child.DataIn != null)
				{
					for (int d = 0; d < child.DataIn.Length; d++)
					{
						var src = child.DataIn[d];
						if (0 <= src.ReferenceTaskIndex && src.ReferenceTaskIndex < task_list.Count)
						{
							var task = task_list[src.ReferenceTaskIndex];
							//if (src.CanConnect(task, ExOutputPortType.DataOut, src.ReferencePortIndex))
							if (0 <= src.ReferencePortIndex && src.ReferencePortIndex < task.DataOut.Length)
								src.Connect(task, ExOutputPortType.DataOut, src.ReferencePortIndex);
						}
					}
				}
				#endregion

				#region パラメータポート:
				if (child.DataParam != null)
				{
					for (int d = 0; d < child.DataParam.Length; d++)
					{
						var src = child.DataParam[d];
						if (0 <= src.ReferenceTaskIndex && src.ReferenceTaskIndex < task_list.Count)
						{
							var task = task_list[src.ReferenceTaskIndex];
							//if (src.CanConnect(task, ExOutputPortType.DataOut, src.ReferencePortIndex))
							if (0 <= src.ReferencePortIndex && src.ReferencePortIndex < task.DataOut.Length)
								src.Connect(task, ExOutputPortType.DataOut, src.ReferencePortIndex);
						}
					}
				}
				#endregion

				// 子タスクの依存関係:
				if (child is CxTaskflow)
					((CxTaskflow)child).LoadDependency();
			}

			// タスクユニットを所有するタスクフローの設定:
			this.SetOwnerTaskflow(this.GetOwnerTaskflow());
		}

		/// <summary>
		/// 依存関係の保存
		/// </summary>
		protected virtual void SaveDependency()
		{
			var task_list = this.TaskUnits;

			foreach (var child in this.TaskUnits)
			{
				#region コントロール入力ポート:
				if (child.ControlIn != null)
				{
					if (child.ControlIn.IsConnected)
					{
						var ref_unit = child.ControlIn.ReferenceTask;
						var ref_port = child.ControlIn.ReferencePort;

						child.ControlIn.ReferenceTaskIndex = task_list.FindIndex(
							delegate(XIE.Tasks.CxTaskUnit item)
							{
								return ReferenceEquals(item, ref_unit);
							});
						child.ControlIn.ReferencePortIndex = (ref_unit.ControlOut == ref_port) ? 0 : -1;
					}
				}
				#endregion

				#region データ入力ポート:
				if (child.DataIn != null)
				{
					for (int d = 0; d < child.DataIn.Length; d++)
					{
						if (child.DataIn[d].IsConnected)
						{
							var ref_unit = child.DataIn[d].ReferenceTask;
							var ref_port = child.DataIn[d].ReferencePort;

							child.DataIn[d].ReferenceTaskIndex = task_list.FindIndex(
								delegate(XIE.Tasks.CxTaskUnit item)
								{
									return ReferenceEquals(item, ref_unit);
								});
							child.DataIn[d].ReferencePortIndex = Array.FindIndex(ref_unit.DataOut,
								delegate(XIE.Tasks.CxTaskPortOut item)
								{
									return ReferenceEquals(item, ref_port);
								});
						}
					}
				}
				#endregion

				#region パラメータポート:
				if (child.DataParam != null)
				{
					for (int d = 0; d < child.DataParam.Length; d++)
					{
						if (child.DataParam[d].IsConnected)
						{
							var ref_unit = child.DataParam[d].ReferenceTask;
							var ref_port = child.DataParam[d].ReferencePort;

							child.DataParam[d].ReferenceTaskIndex = task_list.FindIndex(
								delegate(XIE.Tasks.CxTaskUnit item)
								{
									return ReferenceEquals(item, ref_unit);
								});
							child.DataParam[d].ReferencePortIndex = Array.FindIndex(ref_unit.DataOut,
								delegate(XIE.Tasks.CxTaskPortOut item)
								{
									return ReferenceEquals(item, ref_port);
								});
						}
					}
				}
				#endregion

				// 子タスクの依存関係:
				if (child is CxTaskflow)
					((CxTaskflow)child).SaveDependency();
			}
		}

		#endregion

		#region メソッド: (編集対象タスクフローの操作)

		/// <summary>
		/// 現在編集中のタスクフローの取得
		/// </summary>
		/// <returns>
		///		CurrentIndex に該当するタスクを再帰的に検索して返します。
		///		該当するタスクが無ければ自身を返します。
		/// </returns>
		public virtual CxTaskflow GetCurrentTaskflow()
		{
			if (0 <= CurrentIndex && CurrentIndex < this.TaskUnits.Count)
			{
				var child = this.TaskUnits[CurrentIndex];
				if (child is CxTaskflow)
					return ((CxTaskflow)child).GetCurrentTaskflow();
			}
			return this;
		}

		/// <summary>
		/// 編集対象タスクフローの設定
		/// </summary>
		/// <param name="target">対象のタスクフロー</param>
		/// <returns>
		///		設定できた場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool SetCurrentTaskflow(CxTaskflow target)
		{
			this.ResetCurrentTaskflow();
			for (int i = 0; i < this.TaskUnits.Count; i++)
			{
				if (this.TaskUnits[i] is CxTaskflow)
				{
					var taskflow = (CxTaskflow)this.TaskUnits[i];
					if (ReferenceEquals(taskflow, target))
					{
						this.CurrentIndex = i;
						return true;
					}
					if (taskflow.SetCurrentTaskflow(target))
					{
						this.CurrentIndex = i;
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 編集対象タスクフローの指標のリセット
		/// </summary>
		public virtual void ResetCurrentTaskflow()
		{
			this.CurrentIndex = -1;
			for (int i = 0; i < this.TaskUnits.Count; i++)
			{
				if (this.TaskUnits[i] is CxTaskflow)
				{
					var taskflow = (CxTaskflow)this.TaskUnits[i];
					taskflow.CurrentIndex = -1;
				}
			}
		}

		#endregion

		#region メソッド: (実行順序)

		/// <summary>
		/// 実行順序の取得
		/// </summary>
		/// <returns>
		///		依存関係を辿って子タスクを実行順に格納したリストを返します。
		/// </returns>
		public virtual List<XIE.Tasks.CxTaskUnit> GetExecutionSchedule()
		{
			// 子タスクの実行順序を決定する.
			var schedules = new List<XIE.Tasks.CxTaskUnit>();
			var callstack = new List<XIE.Tasks.CxTaskUnit>();
			foreach (var task in this.TaskUnits)
			{
				GetExecutionSchedule(task, callstack, schedules);
			}
			return schedules;
		}

		/// <summary>
		/// 実行順序の取得
		/// </summary>
		/// <param name="task">実行候補のタスク</param>
		/// <param name="callstack">コールスタック</param>
		/// <param name="schedules">実行順序</param>
		protected virtual void GetExecutionSchedule(XIE.Tasks.CxTaskUnit task, List<XIE.Tasks.CxTaskUnit> callstack, List<XIE.Tasks.CxTaskUnit> schedules)
		{
			// 既に callstack にあれば循環参照になっているので中断する.
			if (0 <= callstack.FindIndex(delegate(XIE.Tasks.CxTaskUnit item) { return ReferenceEquals(item, task); }))
				throw new XIE.CxException(XIE.ExStatus.Impossible, "Circular reference");

			// 既に schedules にあれば無視する.
			if (0 <= schedules.FindIndex(delegate(XIE.Tasks.CxTaskUnit item) { return ReferenceEquals(item, task); }))
				return;

			var dependencies = new Dictionary<XIE.Tasks.CxTaskUnit, int>();

			#region コントロール入力の参照先を探索する.
			{
				var port = task.ControlIn;
				if (port.ReferenceTask != null)
				{
					dependencies[port.ReferenceTask] = schedules.FindIndex(
						delegate(XIE.Tasks.CxTaskUnit item)
						{
							return ReferenceEquals(item, port.ReferenceTask);
						});
				}
			}
			#endregion

			#region データ入力の参照先を探索する.
			foreach (var port in task.DataIn)
			{
				if (port.ReferenceTask != null)
				{
					dependencies[port.ReferenceTask] = schedules.FindIndex(
						delegate(XIE.Tasks.CxTaskUnit item)
						{
							return ReferenceEquals(item, port.ReferenceTask);
						});
				}
			}
			#endregion

			#region パラメータの参照先を探索する.
			foreach (var port in task.DataParam)
			{
				if (port.ReferenceTask != null)
				{
					dependencies[port.ReferenceTask] = schedules.FindIndex(
						delegate(XIE.Tasks.CxTaskUnit item)
						{
							return ReferenceEquals(item, port.ReferenceTask);
						});
				}
			}
			#endregion

			#region スケジュールへ追加する.
			if (dependencies.Count == 0)
			{
				// 依存先が無ければスケジュールに追加する.
				schedules.Add(task);
			}
			else
			{
				// 依存先が有れば更に参照先を探索する.
				callstack.Add(task);
				foreach (var dependency in dependencies)
				{
					// Value が 0 以上の物は、既に schedules にあるので無視する.
					if (dependency.Value < 0)
						GetExecutionSchedule(dependency.Key, callstack, schedules);
				}
				callstack.Remove(task);

				// 依存先の後ろに追加する.
				schedules.Add(task);
			}
			#endregion
		}

		#endregion
	}
}
