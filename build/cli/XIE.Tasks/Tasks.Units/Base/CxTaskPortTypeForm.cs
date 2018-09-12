/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XIE.Tasks
{
	/// <summary>
	/// データ入出力ポート型選択フォーム
	/// </summary>
	public partial class CxTaskPortTypeForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortTypeForm()
		{
			InitializeComponent();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 要素の型
		/// </summary>
		public virtual Type Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}
		private Type m_Type = null;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskPortTypeForm_Load(object sender, EventArgs e)
		{
			treeTypes_Setup();
		}

		/// <summary>
		/// フォームが閉じられる時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskPortTypeForm_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		#endregion

		#region toolbar: (コントロールイベント)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			var node = (CxTaskPortTypeNode)treeTypes.SelectedNode;
			m_Type = node.Type;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region treeTypes: (初期化)

		/// <summary>
		/// treeTypes: 初期化
		/// </summary>
		private void treeTypes_Setup()
		{
			#region 要素の型: (System)
			{
				var top = new CxTaskPortTypeNode("System");
				treeTypes.Nodes.Add(top);
				{
					var category = new CxTaskPortTypeNode("Classes");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Object)));
				}
				{
					var category = new CxTaskPortTypeNode("Structures");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.Point)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.PointF)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.Size)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.SizeF)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.Rectangle)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Drawing.RectangleF)));
				}
				{
					var category = new CxTaskPortTypeNode("Primitives");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Byte)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.SByte)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Char)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Int16)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.UInt16)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Int32)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.UInt32)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Int64)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.UInt64)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Single)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Double)));
				}
				{
					var category = new CxTaskPortTypeNode("Text");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.String)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(System.Text.Encoding)));
				}
				top.Expand();
			}
			#endregion

			#region 要素の型: (XIE)
			{
				var top = new CxTaskPortTypeNode("XIE");
				treeTypes.Nodes.Add(top);
				{
					var category = new CxTaskPortTypeNode("Classes");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.CxArray)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.CxImage)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.CxMatrix)));
				}
				{
					var category = new CxTaskPortTypeNode("Structures");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxPointD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxPointI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxSizeD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxSizeI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxRangeD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxRangeI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxRectangleD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxRectangleI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxCircleD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxCircleI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxCircleArcD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxCircleArcI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxEllipseD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxEllipseI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxEllipseArcD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxEllipseArcI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxLineD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxLineI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxLineSegmentD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxLineSegmentI)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxTrapezoidD)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.TxTrapezoidI)));
				}
				{
					var category = new CxTaskPortTypeNode("Enumerations");
					top.Nodes.Add(category);
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.ExBoolean)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.ExEndianType)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.ExScanDir)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.ExStatus)));
					category.Nodes.Add(new CxTaskPortTypeNode(typeof(XIE.ExType)));
				}
				top.Expand();
			}
			#endregion

			#region カレントノードの設定:
			var result = treeTypes.Nodes.Find(this.Type.FullName, true);
			if (result.Length > 0)
			{
				var node = result[0];
				node.EnsureVisible();
				treeTypes.SelectedNode = node;
			}
			#endregion
		}

		/// <summary>
		/// treeTypes: ノードの追加
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="path"></param>
		/// <param name="index"></param>
		/// <returns>
		///		追加されたノードを返します。
		/// </returns>
		private CxTaskPortTypeNode treeTypes_AddNode(TreeNodeCollection nodes, string[] path, int index)
		{
			var name = path[index];
			var result = nodes.Find(name, false);
			if (result.Length == 0)
			{
				var node = new CxTaskPortTypeNode(name);
				nodes.Add(node);
				if (index < path.Length - 1)
					return treeTypes_AddNode(node.Nodes, path, index + 1);
				return node;
			}
			else
			{
				var node = (CxTaskPortTypeNode)result[0];
				if (index < path.Length - 1)
					return treeTypes_AddNode(node.Nodes, path, index + 1);
				return node;
			}
		}

		#endregion

		#region treeTypes: (コントロールイベント)

		/// <summary>
		/// ツリーノードが選択された後
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeTypes_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var node = (CxTaskPortTypeNode)treeTypes.SelectedNode;
			toolOK.Enabled = (node.Type != null);
		}

		#endregion
	}

	#region CxTaskPortTypeNode

	/// <summary>
	/// 型を表すツリーノード
	/// </summary>
	public class CxTaskPortTypeNode : TreeNode
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortTypeNode()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		public CxTaskPortTypeNode(string name)
			: base(name)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">要素の型</param>
		/// <param name="description">説明</param>
		public CxTaskPortTypeNode(Type type, string description = null)
			: base(type.Name)
		{
			this.Name = type.FullName;
			this.Type = type;
			if (description != null)
				this.ToolTipText = description;
			else
				this.ToolTipText = XIE.AxiTextStorage.GetValue(string.Format("T:{0}", type.FullName));
		}

		/// <summary>
		/// 要素の型
		/// </summary>
		public Type Type = null;
	}

	#endregion
}
