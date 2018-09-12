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
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace XIEstudio
{
	/// <summary>
	/// ツールボックスツリーノード
	/// </summary>
	public class CxToolboxNode : TreeNode
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxToolboxNode()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task"></param>
		/// <param name="label"></param>
		public CxToolboxNode(XIE.Tasks.CxTaskUnit task, string label = null)
		{
			this.Task = task;
			this.Label = label;
			this.Setup();
		}

		/// <summary>
		/// タスク
		/// </summary>
		public XIE.Tasks.CxTaskUnit Task = null;

		/// <summary>
		/// ツールボックス上で表示するラベル (省略時は Task の Name を使用します。)
		/// </summary>
		public string Label = null;

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			#region 表示名.
			{
				string icon = "Service-Connect";
				if (this.Task != null)
					icon = this.Task.IconKey;
				this.ImageKey = icon;
				this.SelectedImageKey = icon;

				if (string.IsNullOrEmpty(this.Label))
				{
					this.Name = this.Task.Name;
					this.Text = this.Task.Name;
				}
				else
				{
					this.Name = this.Label;
					this.Text = this.Label;
				}

				#region ツールチップ.
				try
				{
					if (this.Task is XIE.Tasks.CxTaskFolder)
					{
						var folder = (XIE.Tasks.CxTaskFolder)this.Task;
						var task = (XIE.Tasks.CxTaskUnit)folder.GetDefaultTask();
						if (task != null)
						{
							string tooltip = "";
							if (!string.IsNullOrWhiteSpace(task.Category))
								tooltip += task.Category + "\n";
							this.ToolTipText = tooltip;
						}
					}
					else if (this.Task is XIE.Tasks.CxTaskUnit)
					{
						var task = this.Task;
						string tooltip = "";
						if (!string.IsNullOrWhiteSpace(task.Category))
							tooltip += task.Category + "\n";
						if (!string.IsNullOrWhiteSpace(task.Name))
							tooltip += task.Name + "\n";

						var tid = string.Format("T:{0}", task.GetType().FullName);
						tooltip += XIE.AxiTextStorage.GetValue(tid) + "\n";
						this.ToolTipText = tooltip;
					}
				}
				catch (System.Exception)
				{
					this.ToolTipText = "";
				}
				#endregion

				if (this.Task is XIE.Tasks.CxTaskReference)
					this.ForeColor = Color.Blue;
			}
			#endregion
		}
	}
}
