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
	/// �v���O�C���I���t�H�[��
	/// </summary>
	partial class CxGrabberSelectionForm : Form
	{
		#region �R���X�g���N�^:

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public CxGrabberSelectionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="pluginInfo">�v���O�C�����</param>
		/// <param name="classType">�Ώۂ̃N���X�̌^�B(null �̏ꍇ�͑S�ẴN���X���ΏۂɂȂ�܂��B)</param>
		public CxGrabberSelectionForm(XIE.Tasks.CxGrabberInfo pluginInfo, Type classType)
		{
			InitializeComponent();
			PluginInfo = pluginInfo;
			ClassType = classType;
		}

		#endregion

		#region �������Ɖ��:

		/// <summary>
		/// �t�H�[�����[�h���̏���������
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

				#region CxGrabberThread �̔h���N���X�����A�Z���u���� combobox �ɒǉ�����.
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

				#region CxGrabberThread �̔h���N���X�� combobox �ɒǉ�����.
				List<string> class_names = GetClassNames(this.PluginInfo.AssemblyName);
				if (class_names.Count > 0)
					comboPluginClass.Items.AddRange(class_names.ToArray());
				#endregion

				comboPluginClass.Text = PluginInfo.ClassName;
			}
			#endregion

			#region �f�B���N�g�����̎擾.
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
		/// �t�H�[����������Ƃ��̉������
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

		#region �v���p�e�B:

		/// <summary>
		/// �v���O�C�����
		/// </summary>
		public virtual XIE.Tasks.CxGrabberInfo PluginInfo
		{
			get { return m_PluginInfo; }
			set { m_PluginInfo = value; }
		}
		private XIE.Tasks.CxGrabberInfo m_PluginInfo = new XIE.Tasks.CxGrabberInfo();

		/// <summary>
		/// �Ώۂ̃N���X�̌^�B(null �̏ꍇ�͑S�ẴN���X���ΏۂɂȂ�܂��B)
		/// </summary>
		public virtual Type ClassType
		{
			get { return m_ClassType; }
			set { m_ClassType = value; }
		}
		private Type m_ClassType = null;

		/// <summary>
		/// �v���O�C���t�@�C���f�B���N�g���B(�󔒂̏ꍇ�� ProjectDir �ɂȂ�܂��B)
		/// </summary>
		public virtual string PluginDir
		{
			get { return m_PluginDir; }
			set { m_PluginDir = value; }
		}
		private string m_PluginDir = "";

		/// <summary>
		/// �\���t�@�C���f�B���N�g���B(�󔒂̏ꍇ�� ProjectDir �ɂȂ�܂��B)
		/// </summary>
		public virtual string ConfigDir
		{
			get { return m_ConfigDir; }
			set { m_ConfigDir = value; }
		}
		private string m_ConfigDir = "";

		#endregion

		#region �R���g���[���C�x���g:

		/// <summary>
		/// �v���O�C���t�@�C���I��
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
		/// �v���O�C���t�@�C���I�� (�R���{�{�b�N�X�I��)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPluginFile_SelectionChangeCommitted(object sender, EventArgs e)
		{
			comboPluginClass.Items.Clear();
			comboPluginClass.Text = "";
		}

		/// <summary>
		/// �v���O�C���t�@�C���I�� (�t�H�[�J�X�ړ�)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboPluginClass_Enter(object sender, EventArgs e)
		{
			if (comboPluginFile_Text == comboPluginFile.Text) return;

			comboPluginFile_Text = comboPluginFile.Text;
			comboPluginClass.Text = "";
			comboPluginClass.Items.Clear();

			#region CxGrabberThread �̔h���N���X�� combobox �ɒǉ�����.
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
		/// �\���t�@�C���I��
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

		#region �����֐�:

		/// <summary>
		/// �w�肳�ꂽ�t�@�C������Ώۂ̃N���X�𒊏o���܂��B
		/// </summary>
		/// <param name="asmname">�A�Z���u����(�t�@�C���p�X�܂��͒����A�Z���u����)</param>
		/// <returns>
		///		���o�����N���X���̃R���N�V������Ԃ��܂��B
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

		#region AppDomain �֘A: (�A�Z���u���܂��͌^�̉���)

		/// <summary>
		/// �f�V���A���C�Y���ɃA�Z���u���̉��������s�����Ƃ��ɔ������܂��B(seealso:AppDomain.CurrentDomain.AssemblyResolve)
		/// </summary>
		/// <param name="sender">���M��</param>
		/// <param name="args">�A�Z���u����</param>
		/// <returns>
		///		�w�肳�ꂽ�A�Z���u�����ɊY������A�Z���u����Ԃ��܂��B
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
		/// Type.GetType() �Ō^�̉��������s�����Ƃ��ɔ������܂��B
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns>
		///		���������A�Z���u����Ԃ��܂��B
		/// </returns>
		/// <remarks>
		/// ���̏����́A���L�Ŏ�����Ă�����@�Ƃ͈قȂ�܂��B
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
