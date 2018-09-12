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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace XIEstudio
{
	/// <summary>
	/// タスクフローヘルプフォーム
	/// </summary>
	public partial class CxTaskflowHelpForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflowHelpForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task">表示対象のタスクユニット</param>
		public CxTaskflowHelpForm(XIE.Tasks.CxTaskUnit task)
		{
			InitializeComponent();
			m_Task = task;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowHelpForm_Load(object sender, EventArgs e)
		{
			this.Activated += CxTaskflowHelpForm_Activated;
			this.Deactivate += CxTaskflowHelpForm_Deactivate;
			checkDock.Checked = IsDocking;

			Update(this.Task);
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowHelpForm_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// オーナーフォームへの追従モード
		/// </summary>
		public bool IsDocking
		{
			get { return m_IsDocking; }
			set { m_IsDocking = value; }
		}
		private bool m_IsDocking = true;

		#endregion

		#region メソッド: (表示更新)

		/// <summary>
		/// 表示対象タスク
		/// </summary>
		public XIE.Tasks.CxTaskUnit Task
		{
			get { return m_Task; }
		}
		private XIE.Tasks.CxTaskUnit m_Task = null;

		/// <summary>
		/// 表示更新
		/// </summary>
		/// <param name="task">表示対象のタスクユニット</param>
		public void Update(XIE.Tasks.CxTaskUnit task)
		{
			try
			{
				m_Task = task;
				if (task == null)
				{
					#region アイコン設定.
					// 
					// 詳細は XIEstudio に記載するコメントを参照されたい.
					// 現象)
					//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
					//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
					// 
					switch (System.Environment.OSVersion.Platform)
					{
						case PlatformID.Unix:
							// 
							// 上記理由によりアイコンの設定は行わない.
							// 
							break;
						default:
							{
								var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
								var image = (Bitmap)aux.ImageList.Images["Notify-Help"];
								this.Icon = Icon.FromHandle(image.GetHicon());
							}
							break;
					}
					#endregion

					this.Text = "";
					labelName.Text = "";
					textDescription.Text = "";
					listPorts.Groups.Clear();
					listPorts.Items.Clear();
				}
				else
				{
					#region アイコン設定.
					// 
					// 詳細は XIEstudio に記載するコメントを参照されたい.
					// 現象)
					//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
					//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
					// 
					switch (System.Environment.OSVersion.Platform)
					{
						case PlatformID.Unix:
							// 
							// 上記理由によりアイコンの設定は行わない.
							// 
							break;
						default:
							{
								var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
								var image = (Bitmap)aux.ImageList.Images[task.IconKey];
								this.Icon = Icon.FromHandle(image.GetHicon());
							}
							break;
					}
					#endregion

					var fullname = task.GetType().FullName;
					this.Text = task.Category;
					labelName.Text = task.Name;

					#region Description:
					{
						var key = (task is XIE.Tasks.CxTaskFolder) ? "" : string.Format("T:{0}", fullname);
						var value = task.GetDescription(key);
						textDescription.Text = value;
					}
					#endregion

					listPorts.Groups.Clear();
					listPorts.Items.Clear();

					#region データ入力:
					{
						var group = listPorts.Groups.Add("DataIn", "Inputs");
						for (int index = 0; index < task.DataIn.Length; index++)
						{
							var port = task.DataIn[index];
							var lvitem = new ListViewItem(port.Name);
							lvitem.Group = group;
							listPorts.Items.Add(lvitem);

							#region Types:
							if (port.Types.Length == 0)
							{
								lvitem.SubItems.Add("");
							}
							else
							{
								for (int i = 0; i < port.Types.Length; i++)
								{
									string types = port.Types[i].ToString();
									if (types.StartsWith("System.Collections.Generic.IEnumerable`1", StringComparison.CurrentCulture))
									{
										var pattern = Regex.Escape("[") + "(.*)]";
										var regex = new Regex(pattern);
										var match = regex.Match(types);
										if (match != Match.Empty)
										{
											var part = match.Value;
											part = part.Replace("[", "<");
											part = part.Replace("]", ">");
											types = "IEnumerable" + part;
										}
									}
									if (i == 0)
									{
										lvitem.SubItems.Add(types);
									}
									else
									{
										var lvtype = new ListViewItem();
										lvtype.SubItems.Add(types);
										lvtype.Group = group;
										listPorts.Items.Add(lvtype);
									}
								}
							}
							#endregion

							#region Description:
							{
								var key = (task is XIE.Tasks.CxTaskFolder) ? "" : string.Format("F:{0}.Descriptions.DataIn{1}", fullname, index);
								var value = task.GetDescription(key);
								lvitem.SubItems.Add(value);
							}
							#endregion
						}
					}
					#endregion

					#region パラメータ入力:
					{
						var group = listPorts.Groups.Add("DataParam", "Parameters");
						for (int index = 0; index < task.DataParam.Length; index++)
						{
							var port = task.DataParam[index];
							var lvitem = new ListViewItem(port.Name);
							lvitem.Group = group;
							listPorts.Items.Add(lvitem);

							#region Types:
							if (port.Types.Length == 0)
							{
								lvitem.SubItems.Add("");
							}
							else
							{
								for (int i = 0; i < port.Types.Length; i++)
								{
									string types = port.Types[i].ToString();
									if (types.StartsWith("System.Collections.Generic.IEnumerable`1", StringComparison.CurrentCulture))
									{
										var pattern = Regex.Escape("[") + "(.*)]";
										var regex = new Regex(pattern);
										var match = regex.Match(types);
										if (match != Match.Empty)
										{
											var part = match.Value;
											part = part.Replace("[", "<");
											part = part.Replace("]", ">");
											types = "IEnumerable" + part;
										}
									}
									if (i == 0)
									{
										lvitem.SubItems.Add(types);
									}
									else
									{
										var lvtype = new ListViewItem();
										lvtype.SubItems.Add(types);
										lvtype.Group = group;
										listPorts.Items.Add(lvtype);
									}
								}
							}
							#endregion

							#region Description:
							{
								var key = (task is XIE.Tasks.CxTaskFolder) ? "" : string.Format("F:{0}.Descriptions.DataParam{1}", fullname, index);
								var value = task.GetDescription(key);
								lvitem.SubItems.Add(value);
							}
							#endregion
						}
					}
					#endregion

					#region データ出力:
					{
						var group = listPorts.Groups.Add("DataOut", "Outputs");
						for (int index = 0; index < task.DataOut.Length; index++)
						{
							var port = task.DataOut[index];
							var lvitem = new ListViewItem(port.Name);
							lvitem.Group = group;
							listPorts.Items.Add(lvitem);

							#region Types:
							if (port.Types.Length == 0)
							{
								lvitem.SubItems.Add("");
							}
							else
							{
								for (int i = 0; i < port.Types.Length; i++)
								{
									string types = port.Types[i].ToString();
									if (types.StartsWith("System.Collections.Generic.IEnumerable`1", StringComparison.CurrentCulture))
									{
										var pattern = Regex.Escape("[") + "(.*)]";
										var regex = new Regex(pattern);
										var match = regex.Match(types);
										if (match != Match.Empty)
										{
											var part = match.Value;
											part = part.Replace("[", "<");
											part = part.Replace("]", ">");
											types = "IEnumerable" + part;
										}
									}
									if (types.StartsWith("System.Collections.Generic.List`1", StringComparison.CurrentCulture))
									{
										var pattern = Regex.Escape("[") + "(.*)]";
										var regex = new Regex(pattern);
										var match = regex.Match(types);
										if (match != Match.Empty)
										{
											var part = match.Value;
											part = part.Replace("[", "<");
											part = part.Replace("]", ">");
											types = "List" + part;
										}
									}
									if (i == 0)
									{
										lvitem.SubItems.Add(types);
									}
									else
									{
										var lvtype = new ListViewItem();
										lvtype.SubItems.Add(types);
										lvtype.Group = group;
										listPorts.Items.Add(lvtype);
									}
								}
							}
							#endregion

							#region Description:
							{
								var key = (task is XIE.Tasks.CxTaskFolder) ? "" : string.Format("F:{0}.Descriptions.DataOut{1}", fullname, index);
								var value = task.GetDescription(key);
								lvitem.SubItems.Add(value);
							}
							#endregion
						}
					}
					#endregion

					listPorts.Columns[0].Width = -2;
					listPorts.Columns[1].Width = -2;
				}
			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// フォームがアクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowHelpForm_Activated(object sender, EventArgs e)
		{
			this.Opacity = 1.0;
		}

		/// <summary>
		/// フォームが非アクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowHelpForm_Deactivate(object sender, EventArgs e)
		{
			this.Opacity = 0.9;
		}

		#endregion

		#region コントロールイベント: (Dock)

		/// <summary>
		/// フォームの追従モードの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkDock_CheckedChanged(object sender, EventArgs e)
		{
			IsDocking = checkDock.Checked;
		}

		#endregion

		#region コントロールイベント: (説明の表示切替)

		/// <summary>
		/// タスク名のラベルがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelName_Click(object sender, EventArgs e)
		{
			if (this.Task != null)
				textDescription.Text = XIE.AxiTextStorage.GetValue(string.Format("T:{0}", this.Task.GetType().FullName));
			else
				textDescription.Text = "";
		}

		/// <summary>
		/// リストビュー項目の選択指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listPorts_SelectedIndexChanged(object sender, EventArgs e)
		{
			var items = listPorts.SelectedItems;
			if (items != null && items.Count > 0)
			{
				var lvitem = items[0];
				if (lvitem.SubItems.Count > 2)
					textDescription.Text = lvitem.SubItems[2].Text;
				else
					textDescription.Text = "";
			}
			else
			{
				if (this.Task != null)
					textDescription.Text = XIE.AxiTextStorage.GetValue(string.Format("T:{0}", this.Task.GetType().FullName));
				else
					textDescription.Text = "";
			}
		}

		#endregion
	}
}
