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
using System.Threading;
using System.Diagnostics;

namespace XIE.Tasks
{
	/// <summary>
	/// データ入出力ポート編集フォーム
	/// </summary>
	public partial class CxTaskPortEditForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortEditForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">タスクユニット</param>
		public CxTaskPortEditForm(CxTaskUnit task)
		{
			InitializeComponent();
			this.Task = task;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タスクユニット
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.Forms.CxTaskPortEditForm.Task")]
		public virtual CxTaskUnit Task
		{
			get { return m_Task; }
			set { m_Task = value; }
		}
		private CxTaskUnit m_Task = new Syntax_Class();

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームがロードされたときの初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskPortEditForm_Load(object sender, EventArgs e)
		{
			treePorts.ImageList = SharedData.Icons16.ImageList;

			#region ツリーノード追加:
			{
				var top = new TreeNode();
				top.Text = top.Name = "DataIn";
				top.ImageKey = top.SelectedImageKey = "TabPage";
				treePorts.Nodes.Add(top);
				foreach (var item in this.Task.DataIn)
					top.Nodes.Add(new CxTaskPortInNode((CxTaskPortIn)item.Clone(), "Node-DataIn"));
				top.Expand();
			}
			{
				var top = new TreeNode();
				top.Text = top.Name = "DataParam";
				top.ImageKey = top.SelectedImageKey = "TabPage";
				treePorts.Nodes.Add(top);
				foreach (var item in this.Task.DataParam)
					top.Nodes.Add(new CxTaskPortInNode((CxTaskPortIn)item.Clone(), "Node-DataParam"));
				top.Expand();
			}
			{
				var top = new TreeNode();
				top.Text = top.Name = "DataOut";
				top.ImageKey = top.SelectedImageKey = "TabPage";
				treePorts.Nodes.Add(top);
				foreach (var item in this.Task.DataOut)
					top.Nodes.Add(new CxTaskPortOutNode((CxTaskPortOut)item.Clone(), "Node-DataOut"));
				top.Expand();
			}
			#endregion

			ResetControlPanel();
			EnableControlPanel(false);
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskPortEditForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				var dataIn = new List<CxTaskPortIn>();
				var dataParam = new List<CxTaskPortIn>();
				var dataOut = new List<CxTaskPortOut>();

				foreach (var item in treePorts.Nodes["DataIn"].Nodes)
					dataIn.Add(((CxTaskPortInNode)item).Port);
				foreach (var item in treePorts.Nodes["DataParam"].Nodes)
					dataParam.Add(((CxTaskPortInNode)item).Port);
				foreach (var item in treePorts.Nodes["DataOut"].Nodes)
					dataOut.Add(((CxTaskPortOutNode)item).Port);

				foreach (var item in dataParam)
					item.ResetSubData();

				this.Task.DataIn = dataIn.ToArray();
				this.Task.DataParam = dataParam.ToArray();
				this.Task.DataOut = dataOut.ToArray();
			}
		}

		#endregion

		#region コントロールイベント: (OK/Cancel)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region treePorts:

		/// <summary>
		/// ノードが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treePorts_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var node = (CxTaskPortInNode)selected_node;
				textName.Text = node.Port.Name;
				if (node.Port.Types.Length == 0)
					textType.Text = "";
				else if (node.Port.Types[0] == null)
					textType.Text = "";
				else
					textType.Text = node.Port.Types[0].FullName;
				textDescription.Text = node.Port.Description;
				EnableControlPanel(true);
				return;
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var node = (CxTaskPortOutNode)selected_node;
				textName.Text = node.Port.Name;
				if (node.Port.Types.Length == 0)
					textType.Text = "";
				else if (node.Port.Types[0] == null)
					textType.Text = "";
				else
					textType.Text = node.Port.Types[0].FullName;
				textDescription.Text = node.Port.Description;
				EnableControlPanel(true);
				return;
			}
			ResetControlPanel();
			EnableControlPanel(false);
		}

		#endregion

		#region toolbar:

		/// <summary>
		/// Add が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAdd_Click(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (ReferenceEquals(selected_node, treePorts.Nodes["DataIn"]))
			{
				var name = string.Format("In{0}", selected_node.Nodes.Count);
				var port = new CxTaskPortIn(name, new Type[] { typeof(object) });
				var node = new CxTaskPortInNode(port, "Node-DataIn");
				treePorts.Nodes["DataIn"].Nodes.Add(node);
				treePorts.Nodes["DataIn"].Expand();
				treePorts.SelectedNode = node;
				return;
			}
			if (ReferenceEquals(selected_node, treePorts.Nodes["DataParam"]))
			{
				var name = string.Format("Param{0}", selected_node.Nodes.Count);
				var port = new CxTaskPortIn(name, new Type[] { typeof(object) });
				var node = new CxTaskPortInNode(port, "Node-DataParam");
				treePorts.Nodes["DataParam"].Nodes.Add(node);
				treePorts.Nodes["DataParam"].Expand();
				treePorts.SelectedNode = node;
				return;
			}
			if (ReferenceEquals(selected_node, treePorts.Nodes["DataOut"]))
			{
				var name = string.Format("Out{0}", selected_node.Nodes.Count);
				var port = new CxTaskPortOut(name, new Type[] { typeof(object) });
				var node = new CxTaskPortOutNode(port, "Node-DataOut");
				treePorts.Nodes["DataOut"].Nodes.Add(node);
				treePorts.Nodes["DataOut"].Expand();
				treePorts.SelectedNode = node;
				return;
			}
			if (selected_node is CxTaskPortInNode)
			{
				if (ReferenceEquals(selected_node.Parent, treePorts.Nodes["DataIn"]))
				{
					var name = string.Format("In{0}", selected_node.Parent.Nodes.Count);
					var port = new CxTaskPortIn(name, new Type[] { typeof(object) });
					var node = new CxTaskPortInNode(port, "Node-DataIn");
					treePorts.Nodes["DataIn"].Nodes.Insert(selected_node.Index + 1, node);
					treePorts.Nodes["DataIn"].Expand();
					treePorts.SelectedNode = node;
					return;
				}
				if (ReferenceEquals(selected_node.Parent, treePorts.Nodes["DataParam"]))
				{
					var name = string.Format("Param{0}", selected_node.Parent.Nodes.Count);
					var port = new CxTaskPortIn(name, new Type[] { typeof(object) });
					var node = new CxTaskPortInNode(port, "Node-DataParam");
					treePorts.Nodes["DataParam"].Nodes.Insert(selected_node.Index + 1, node);
					treePorts.Nodes["DataParam"].Expand();
					treePorts.SelectedNode = node;
					return;
				}
			}
			if (selected_node is CxTaskPortOutNode)
			{
				if (ReferenceEquals(selected_node.Parent, treePorts.Nodes["DataOut"]))
				{
					var name = string.Format("Out{0}", selected_node.Parent.Nodes.Count);
					var port = new CxTaskPortOut(name, new Type[] { typeof(object) });
					var node = new CxTaskPortOutNode(port, "Node-DataOut");
					treePorts.Nodes["DataOut"].Nodes.Insert(selected_node.Index + 1, node);
					treePorts.Nodes["DataOut"].Expand();
					treePorts.SelectedNode = node;
					return;
				}
			}
		}

		/// <summary>
		/// Remove が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolRemove_Click(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node.Level == 1)
			{
				selected_node.Remove();
				return;
			}
		}

		/// <summary>
		/// Up が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolUp_Click(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var parent = selected_node.Parent;
				var node = (CxTaskPortInNode)selected_node;
				var index = node.Index - 1;
				if (index < 0) return;
				var new_node = new CxTaskPortInNode(node.Port, node.ImageKey);
				node.Remove();
				parent.Nodes.Insert(index, new_node);
				treePorts.SelectedNode = new_node;
				return;
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var parent = selected_node.Parent;
				var node = (CxTaskPortOutNode)selected_node;
				var index = node.Index - 1;
				if (index < 0) return;
				var new_node = new CxTaskPortOutNode(node.Port, node.ImageKey);
				node.Remove();
				parent.Nodes.Insert(index, new_node);
				treePorts.SelectedNode = new_node;
				return;
			}
		}

		/// <summary>
		/// Down が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolDown_Click(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var parent = selected_node.Parent;
				var node = (CxTaskPortInNode)selected_node;
				var index = node.Index + 1;
				if (index >= parent.Nodes.Count) return;
				var new_node = new CxTaskPortInNode(node.Port, node.ImageKey);
				node.Remove();
				parent.Nodes.Insert(index, new_node);
				treePorts.SelectedNode = new_node;
				return;
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var parent = selected_node.Parent;
				var node = (CxTaskPortOutNode)selected_node;
				var index = node.Index + 1;
				if (index >= parent.Nodes.Count) return;
				if (index < 0) return;
				var new_node = new CxTaskPortOutNode(node.Port, node.ImageKey);
				node.Remove();
				parent.Nodes.Insert(index, new_node);
				treePorts.SelectedNode = new_node;
				return;
			}
		}

		#endregion

		#region 右辺パネル:

		/// <summary>
		/// 右辺パネルの有効化/無効化
		/// </summary>
		/// <param name="value">true:有効化/false:無効化</param>
		private void EnableControlPanel(bool value)
		{
			textName.Enabled = value;
			textType.Enabled = value;
			buttonType.Enabled = value;
			textDescription.Enabled = value;
		}

		/// <summary>
		/// 右辺パネルのリセット
		/// </summary>
		private void ResetControlPanel()
		{
			textName.Text = "";
			textType.Text = "";
			textDescription.Text = "";
		}

		#endregion

		#region textName:

		/// <summary>
		/// キーが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textName_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (IsValidAsPortName(e.KeyChar)) return;
			if ((char)Keys.Back == e.KeyChar) return;
			if (22 == e.KeyChar) return;	// Ctrl+V

			e.Handled = true;
		}

		/// <summary>
		/// 入出力ポート名として有効か否かの判定
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool IsValidAsPortName(Char value)
		{
			if ('0' <= value && value <= '9') return true;
			if ('A' <= value && value <= 'Z') return true;
			if ('a' <= value && value <= 'z') return true;
			if ('_' == value) return true;
			return false;
		}

		/// <summary>
		/// テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textName_TextChanged(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var node = (CxTaskPortInNode)selected_node;
				if (node.Port.Name != textName.Text)
				{
					foreach (var item in textName.Text.ToCharArray())
					{
						if (IsValidAsPortName(item) == false)
						{
							MessageBox.Show(this, "It contains invalid characters.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							textName.Text = node.Port.Name;
							return;
						}
					}
					node.Port.Name = textName.Text;
					node.Text = textName.Text;
				}
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var node = (CxTaskPortOutNode)selected_node;
				if (node.Port.Name != textName.Text)
				{
					foreach (var item in textName.Text.ToCharArray())
					{
						if (IsValidAsPortName(item) == false)
						{
							MessageBox.Show(this, "It contains invalid characters.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							textName.Text = node.Port.Name;
							return;
						}
					}
					node.Port.Name = textName.Text;
					node.Text = textName.Text;
				}
			}
		}

		#endregion

		#region textDescription:

		/// <summary>
		/// テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textDescription_TextChanged(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var node = (CxTaskPortInNode)selected_node;
				node.Port.Description = textDescription.Text;
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var node = (CxTaskPortOutNode)selected_node;
				node.Port.Description = textDescription.Text;
			}
		}

		#endregion

		#region buttonType:

		/// <summary>
		/// Type ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonType_Click(object sender, EventArgs e)
		{
			var selected_node = treePorts.SelectedNode;
			if (selected_node is CxTaskPortInNode)
			{
				var node = (CxTaskPortInNode)selected_node;
				var dlg = new CxTaskPortTypeForm();
				dlg.Type = (node.Port.Types.Length == 0) ? null : node.Port.Types[0];
				dlg.StartPosition = FormStartPosition.CenterParent;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (dlg.Type == null)
					{
						node.Port.Types = new Type[] {};
						textType.Text = "";
					}
					else
					{
						node.Port.Types = new Type[] { dlg.Type };
						textType.Text = dlg.Type.FullName;
					}
				}
				return;
			}
			if (selected_node is CxTaskPortOutNode)
			{
				var node = (CxTaskPortOutNode)selected_node;
				var dlg = new CxTaskPortTypeForm();
				dlg.Type = (node.Port.Types.Length == 0) ? null : node.Port.Types[0];
				dlg.StartPosition = FormStartPosition.CenterParent;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (dlg.Type == null)
					{
						node.Port.Types = new Type[] { };
						textType.Text = "";
					}
					else
					{
						node.Port.Types = new Type[] { dlg.Type };
						textType.Text = dlg.Type.FullName;
					}
				}
				return;
			}
		}

		#endregion
	}

	#region CxTaskPortInNode:

	/// <summary>
	/// データ入力ポートツリーノード
	/// </summary>
	public class CxTaskPortInNode : TreeNode
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortInNode()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="port">データ入力ポート</param>
		/// <param name="icon">アイコンキー</param>
		public CxTaskPortInNode(XIE.Tasks.CxTaskPortIn port, string icon)
		{
			this.Port = port;
			this.Text = port.Name;
			this.ImageKey = icon;
			this.SelectedImageKey = icon;
		}

		/// <summary>
		/// データ入力ポート
		/// </summary>
		public XIE.Tasks.CxTaskPortIn Port = null;
	}

	#endregion

	#region CxTaskPortOutNode:

	/// <summary>
	/// データ出力ポートツリーノード
	/// </summary>
	public class CxTaskPortOutNode : TreeNode
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortOutNode()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="port">データ出力ポート</param>
		/// <param name="icon">アイコンキー</param>
		public CxTaskPortOutNode(XIE.Tasks.CxTaskPortOut port, string icon)
		{
			this.Port = port;
			this.Text = port.Name;
			this.ImageKey = icon;
			this.SelectedImageKey = icon;
		}

		/// <summary>
		/// データ出力ポート
		/// </summary>
		public XIE.Tasks.CxTaskPortOut Port = null;
	}

	#endregion
}
