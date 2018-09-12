/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace XIEstudio
{
	/// <summary>
	/// プラグイン選択フォーム
	/// </summary>
	partial class CxGrabberSelectionForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabberSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="pluginInfo">プラグイン情報</param>
		/// <param name="classType">対象のクラスの型。(null の場合は全てのクラスが対象になります。)</param>
		public CxGrabberSelectionForm(XIE.Tasks.CxGrabberInfo pluginInfo, Type classType)
		{
			InitializeComponent();
			PluginInfo = pluginInfo;
			ClassType = classType;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxGrabberSelectForm_Load(object sender, EventArgs e)
		{
			textConfigFile.Text = PluginInfo.ConfigFile;

			#region Plugin
			{
				comboPluginFile.Items.Clear();
				comboPluginFile.Items.Add("");

				#region CxGrabberThread の派生クラスを持つアセンブリを combobox に追加する.
				ResolveEventHandler asm_resolving = new ResolveEventHandler(AssemblyResolving);
				ResolveEventHandler type_resolving = new ResolveEventHandler(TypeResolving);
				try
				{
					AppDomain.CurrentDomain.AssemblyResolve += asm_resolving;
					AppDomain.CurrentDomain.TypeResolve += type_resolving;

					string dllpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
					string[] files = System.IO.Directory.GetFiles(dllpath, "XIE.*.dll");
					foreach (string filename in files)
					{
						if (filename.EndsWith("XIE.Core.dll", StringComparison.OrdinalIgnoreCase)) continue;
						if (filename.EndsWith("XIE.Tasks.dll", StringComparison.OrdinalIgnoreCase)) continue;

						try
						{
							Assembly asm1 = Assembly.ReflectionOnlyLoadFrom(filename);
							Assembly asm2 = Assembly.Load(asm1.FullName);
							Type[] types = asm2.GetTypes();
							foreach (System.Type type in types)
							{
								if (this.ClassType != null && type.IsSubclassOf(this.ClassType))
								{
									comboPluginFile.Items.Add(asm2.FullName);
									break;
								}
							}
						}
						catch (System.Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.Message);
						}
					}
				}
				catch (System.Exception)
				{
				}
				finally
				{
					AppDomain.CurrentDomain.AssemblyResolve -= asm_resolving;
					AppDomain.CurrentDomain.TypeResolve -= type_resolving;
				}
				#endregion

				comboPluginFile.Text = PluginInfo.AssemblyName;
				comboPluginFile_Text = comboPluginFile.Text;

				comboPluginClass.Text = "";
				comboPluginClass.Items.Clear();

				#region CxGrabberThread の派生クラスを combobox に追加する.
				List<string> class_names = GetClassNames(this.PluginInfo.AssemblyName);
				if (class_names.Count > 0)
					comboPluginClass.Items.AddRange(class_names.ToArray());
				#endregion

				comboPluginClass.Text = PluginInfo.ClassName;
			}
			#endregion

			#region ディレクトリ名の取得.
			// ---
			try
			{
				var is_path = (this.PluginInfo.AssemblyName.Contains("\\") || PluginInfo.AssemblyName.Contains("/"));
				var asmdir = "";
				if (is_path)
					asmdir = System.IO.Path.GetDirectoryName(this.PluginInfo.AssemblyName);

				if (asmdir != "")
					this.PluginDir = asmdir;
				else
					this.PluginDir = XIE.Tasks.SharedData.ProjectDir;
			}
			catch (System.Exception)
			{
			}
			// ---
			try
			{
				if (this.PluginInfo.ConfigFile != "")
					this.ConfigDir = System.IO.Path.GetDirectoryName(this.PluginInfo.ConfigFile);
				else
					this.ConfigDir = XIE.Tasks.SharedData.ProjectDir;
			}
			catch (System.Exception)
			{
			}
			#endregion
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxGrabberSelectForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				PluginInfo.AssemblyName = comboPluginFile.Text;
				PluginInfo.ClassName = comboPluginClass.Text;
				PluginInfo.ConfigFile = textConfigFile.Text;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// プラグイン情報
		/// </summary>
		public virtual XIE.Tasks.CxGrabberInfo PluginInfo
		{
			get { return m_PluginInfo; }
			set { m_PluginInfo = value; }
		}
		private XIE.Tasks.CxGrabberInfo m_PluginInfo = new XIE.Tasks.CxGrabberInfo();

		/// <summary>
		/// 対象のクラスの型。(null の場合は全てのクラスが対象になります。)
		/// </summary>
		public virtual Type ClassType
		{
			get { return m_ClassType; }
			set { m_ClassType = value; }
		}
		private Type m_ClassType = null;

		/// <summary>
		/// プラグインファイルディレクトリ。(空白の場合は ProjectDir になります。)
		/// </summary>
		public virtual string PluginDir
		{
			get { return m_PluginDir; }
			set { m_PluginDir = value; }
		}
		private string m_PluginDir = "";

		/// <summary>
		/// 構成ファイルディレクトリ。(空白の場合は ProjectDir になります。)
		/// </summary>
		public virtual string ConfigDir
		{
			get { return m_ConfigDir; }
			set { m_ConfigDir = value; }
		}
		private string m_ConfigDir = "";

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// プラグインファイル選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPluginFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = false;
			dlg.Filter = "Assembly files (*.dll;*.exe)|*.dll;*.exe";
			dlg.Filter += "|Library files (*.dll)|*.dll";
			dlg.Filter += "|Executable files (*.exe)|*.exe";
			dlg.Filter += "|All files (*.*)|*.*";
			dlg.Multiselect = false;

			if (string.IsNullOrWhiteSpace(this.PluginDir) == false)
				dlg.InitialDirectory = this.PluginDir;

			if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
				dlg.CustomPlaces.Add(new FileDialogCustomPlace(XIE.Tasks.SharedData.ProjectDir));

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.PluginDir = System.IO.Path.GetDirectoryName(dlg.FileName);

				comboPluginClass.Items.Clear();
				comboPluginClass.Text = "";

				comboPluginFile.SelectedIndex = 0;
				comboPluginFile.Text = dlg.FileName;
			}
		}

		/// <summary>
		/// プラグインファイル選択 (コンボボックス選択)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPluginFile_SelectionChangeCommitted(object sender, EventArgs e)
		{
			comboPluginClass.Items.Clear();
			comboPluginClass.Text = "";
		}

		/// <summary>
		/// プラグインファイル選択 (フォーカス移動)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPluginClass_Enter(object sender, EventArgs e)
		{
			if (comboPluginFile_Text == comboPluginFile.Text) return;

			comboPluginFile_Text = comboPluginFile.Text;
			comboPluginClass.Text = "";
			comboPluginClass.Items.Clear();

			#region CxGrabberThread の派生クラスを combobox に追加する.
			if (comboPluginFile.Text != "")
			{
				List<string> class_names = GetClassNames(comboPluginFile.Text);
				if (class_names.Count > 0)
					comboPluginClass.Items.AddRange(class_names.ToArray());

				if (comboPluginClass.Items.Count > 0)
					comboPluginClass.SelectedIndex = 0;
			}
			#endregion
		}
		private string comboPluginFile_Text = "";

		/// <summary>
		/// 構成ファイル選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonConfigFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = false;
			dlg.Filter = "Configuration files (*.ini;*.xml)|*.ini;*.xml";
			dlg.Filter += "|Windows Profiles (*.ini)|*.ini";
			dlg.Filter += "|XML files (*.xml)|*.xml";
			dlg.Filter += "|All files (*.*)|*.*";
			dlg.Multiselect = false;

			if (string.IsNullOrWhiteSpace(this.ConfigDir) == false)
				dlg.InitialDirectory = this.ConfigDir;

			if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
				dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				textConfigFile.Text = dlg.FileName;
				this.ConfigDir = System.IO.Path.GetDirectoryName(dlg.FileName);
			}
		}

		#endregion

		#region 内部関数:

		/// <summary>
		/// 指定されたファイルから対象のクラスを抽出します。
		/// </summary>
		/// <param name="asmname">アセンブリ名(ファイルパスまたは長いアセンブリ名)</param>
		/// <returns>
		///		抽出したクラス名のコレクションを返します。
		/// </returns>
		private List<string> GetClassNames(string asmname)
		{
			List<string> class_names = new List<string>();

			try
			{
				Assembly asm = null;
				if (asmname.Trim() == "")
					return class_names;
				else if (System.IO.File.Exists(asmname))
					asm = Assembly.LoadFile(asmname);
				else
					asm = Assembly.Load(asmname);

				if (asm == null)
					return class_names;

				Type[] types = asm.GetTypes();
				foreach (System.Type type in types)
				{
					if (this.ClassType == null)
						class_names.Add(type.ToString());
					else if (type.IsSubclassOf(this.ClassType))
						class_names.Add(type.ToString());
				}
			}
			catch (System.Exception)
			{
			}

			return class_names;
		}

		#endregion

		#region AppDomain 関連: (アセンブリまたは型の解決)

		/// <summary>
		/// デシリアライズ時にアセンブリの解決が失敗したときに発生します。(seealso:AppDomain.CurrentDomain.AssemblyResolve)
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="args">アセンブリ名</param>
		/// <returns>
		///		指定されたアセンブリ名に該当するアセンブリを返します。
		/// </returns>
		static Assembly AssemblyResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (name == assembly.FullName.Split(',')[0])
					return assembly;
			}
			return null;
		}

		/// <summary>
		/// Type.GetType() で型の解決が失敗したときに発生します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns>
		///		見つかったアセンブリを返します。
		/// </returns>
		/// <remarks>
		/// この処理は、下記で示されている方法とは異なります。
		/// <![CDATA[
		/// http://msdn.microsoft.com/ja-jp/library/system.appdomain.typeresolve(v=vs.80).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-4
		/// ]]>
		/// </remarks>
		static Assembly TypeResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (name == type.FullName)
						return assembly;
				}
			}
			return null;
		}

		#endregion

	}
}
