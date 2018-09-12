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

namespace XIE.Tasks
{
	/// <summary>
	/// タスクフォルダクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskFolder : CxTaskUnit
		, ISerializable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskFolder()
		{
			this.Name = "Folder";
			this.IconKey = "TabPage";
			this.DefaultIndex = -1;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="description_key">リソースキー</param>
		/// <param name="icon_key">アイコンキー</param>
		/// <param name="default_index">既定のタスクユニットの指標 [-1=なし,0~=あり]</param>
		public CxTaskFolder(string name, string description_key, string icon_key, int default_index = -1)
		{
			this.Name = name;
			this.DescriptionKey = description_key;
			this.IconKey = icon_key;
			this.DefaultIndex = default_index;
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (シグネチャコンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxTaskFolder(SerializationInfo info, StreamingContext context)
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

		#region プロパティ: (子タスク)

		/// <summary>
		/// 既定のタスクユニットの指標 [-1=なし,0~=あり] [既定値:-1]
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public int DefaultIndex = -1;

		/// <summary>
		/// 子タスクユニットのコレクション
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public Dictionary<CxTaskUnit, string> Tasks = new Dictionary<CxTaskUnit, string>();

		#endregion

		#region メソッド:

		/// <summary>
		/// 既定のタスクの取得
		/// </summary>
		/// <returns>
		///		既定のタスクを返します。
		///		既定のタスクがなければ null を返します。
		/// </returns>
		public virtual CxTaskUnit GetDefaultTask()
		{
			if (this.DefaultIndex < 0) return null;
			var tasks = GetAllTasks(false);
			if (this.DefaultIndex < tasks.Count)
			{
				return tasks[this.DefaultIndex];
			}
			return null;
		}

		/// <summary>
		/// 全ての子タスクユニットの取得
		/// </summary>
		/// <param name="include_folder">フォルダを含むか否か</param>
		/// <returns>
		///		タスクユニットのコレクションを返します。
		/// </returns>
		public virtual List<CxTaskUnit> GetAllTasks(bool include_folder)
		{
			var tasks = new List<CxTaskUnit>();
			foreach (var task in this.Tasks)
			{
				if (task.Key is CxTaskFolder)
				{
					var folder = (CxTaskFolder)task.Key;
					if (include_folder)
						tasks.Add(folder);
					tasks.AddRange(folder.GetAllTasks(include_folder));
				}
				else
				{
					tasks.Add(task.Key);
				}
			}
			return tasks;
		}

		#endregion
	}
}
