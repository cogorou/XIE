/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;

namespace XIE.Tasks
{
	/// <summary>
	/// 外部機器情報通知イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxAuxNotifyEventHandler(object sender, CxAuxNotifyEventArgs e);

	/// <summary>
	/// 外部機器情報通知イベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxAuxNotifyEventArgs : EventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxNotifyEventArgs()
		{
		}

		#endregion
	}

	/// <summary>
	/// 外部機器情報通知イベント引数クラス (画像ノード追加)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxAuxNotifyEventArgs_AddImage : CxAuxNotifyEventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="data">データオブジェクト</param>
		/// <param name="name">ノード名称</param>
		/// <param name="selected">ノードの選択指示</param>
		public CxAuxNotifyEventArgs_AddImage(XIE.CxImage data, string name, bool selected = false)
		{
			Data = data;
			Name = name;
			Selected = selected;
		}

		/// <summary>
		/// データオブジェクト
		/// </summary>
		public XIE.CxImage Data;

		/// <summary>
		/// ノード名称
		/// </summary>
		public string Name;

		/// <summary>
		/// ノードの選択指示
		/// </summary>
		public bool Selected;

		#endregion
	}

	/// <summary>
	/// 外部機器情報通知イベント引数クラス (表示更新)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxAuxNotifyEventArgs_Refresh : CxAuxNotifyEventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxNotifyEventArgs_Refresh()
		{
		}

		#endregion
	}

	/// <summary>
	/// 初期化指示イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxTaskSetupEventHandler(object sender, CxTaskSetupEventArgs e);

	/// <summary>
	/// 初期化指示イベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxTaskSetupEventArgs : EventArgs
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskSetupEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ (メッセージ通知)
		/// </summary>
		/// <param name="taskflow">最上位のタスクフロー</param>
		/// <param name="auxInfo">外部機器情報</param>
		/// <param name="baseDir">基準ディレクトリ</param>
		/// <param name="baseName">基準名称</param>
		public CxTaskSetupEventArgs(CxTaskflow taskflow, CxAuxInfo auxInfo, string baseDir, string baseName)
		{
			Taskflow = taskflow;
			AuxInfo = auxInfo;
			BaseDir = baseDir;
			BaseName = baseName;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		自身の内容を複製したインスタンスを返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			var clone = (CxTaskSetupEventArgs)asm.CreateInstance(type.FullName);
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;

			if (src is CxTaskSetupEventArgs)
			{
				var _src = (CxTaskSetupEventArgs)src;
				this.Taskflow = _src.Taskflow;
				this.AuxInfo = _src.AuxInfo;
				this.BaseDir = _src.BaseDir;
				this.BaseName = _src.BaseName;
			}

			return;
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxTaskSetupEventArgs)src;
				if (this.Taskflow != _src.Taskflow) return false;
				if (this.AuxInfo != _src.AuxInfo) return false;
				if (this.BaseDir != _src.BaseDir) return false;
				if (this.BaseName != _src.BaseName) return false;
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 最上位のタスクフロー
		/// </summary>
		[NonSerialized]
		public CxTaskflow Taskflow = null;

		/// <summary>
		/// 外部機器情報
		/// </summary>
		[NonSerialized]
		public CxAuxInfo AuxInfo = null;

		/// <summary>
		/// 基準ディレクトリ
		/// </summary>
		[NonSerialized]
		public string BaseDir = "";

		/// <summary>
		/// 基準名称
		/// </summary>
		[NonSerialized]
		public string BaseName = "";

		#endregion
	}

	/// <summary>
	/// 実行指示イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxTaskExecuteEventHandler(object sender, CxTaskExecuteEventArgs e);

	/// <summary>
	/// 実行指示イベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxTaskExecuteEventArgs : CxTaskSetupEventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskExecuteEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ (メッセージ通知)
		/// </summary>
		/// <param name="taskflow">最上位のタスクフロー</param>
		/// <param name="auxInfo">外部機器情報</param>
		/// <param name="baseDir">基準ディレクトリ</param>
		/// <param name="baseName">基準名称</param>
		/// <param name="loopCount">スレッドの繰り返し回数 [初期値:0、範囲:1~]</param>
		/// <param name="loopMax">スレッドの繰り返し上限 [初期値:0、範囲:0=無限, 1~=有限]</param>
		public CxTaskExecuteEventArgs(CxTaskflow taskflow, CxAuxInfo auxInfo, string baseDir, string baseName, int loopCount, int loopMax)
			: base(taskflow, auxInfo, baseDir, baseName)
		{
			LoopCount = loopCount;
			LoopMax = loopMax;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;

			if (src is CxTaskSetupEventArgs)
			{
				base.CopyFrom(src);
			}
			if (src is CxTaskExecuteEventArgs)
			{
				var _src = (CxTaskExecuteEventArgs)src;
				this.LoopCount = _src.LoopCount;
				this.LoopMax = _src.LoopMax;
				this.ControlSyntax = _src.ControlSyntax;
			}

			return;
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

			try
			{
				if (src is CxTaskSetupEventArgs)
				{
					if (base.ContentEquals(src) == false) return false;
				}
				if (src is CxTaskExecuteEventArgs)
				{
					var _src = (CxTaskExecuteEventArgs)src;
					if (this.LoopCount != _src.LoopCount) return false;
					if (this.LoopMax != _src.LoopMax) return false;
					if (this.ControlSyntax != _src.ControlSyntax) return false;
				}
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// スレッドの繰り返し回数 [初期値:0、範囲:1~]
		/// </summary>
		public int LoopCount = 0;

		/// <summary>
		/// スレッドの繰り返し上限 [初期値:0、範囲:0=無限, 1~=有限]
		/// </summary>
		public int LoopMax = 0;

		/// <summary>
		/// タスクの制御構文。呼び出したタスクによって書き換えられます。呼び出し元は指示に従って処理を制御します。
		/// </summary>
		/// <seealso cref="M:XIE.Tasks.CxTaskflow.Execute(System.Object,XIE.Tasks.CxTaskExecuteEventArgs)"/>
		/// <seealso cref="M:XIE.Tasks.CxTaskComposite.Execute(System.Object,XIE.Tasks.CxTaskExecuteEventArgs)"/>
		/// <seealso cref="M:XIE.Tasks.CxTaskflow.ValueChanged(System.Object,XIE.Tasks.CxTaskValueChangedEventArgs)"/>
		/// <seealso cref="M:XIE.Tasks.CxTaskComposite.ValueChanged(System.Object,XIE.Tasks.CxTaskValueChangedEventArgs)"/>
		public ExControlSyntax ControlSyntax = ExControlSyntax.None;

		#endregion

		#region コールバック関数:

		/// <summary>
		/// コールバック関数
		/// </summary>
		/// <remarks>
		///		このフィールドは評価アプリケーションがステップ実行を行うために使用するものです。
		///		通常の実行では null が設定されています。
		/// </remarks>
		public CxTaskExecuteEventHandler Callback = null;

		#endregion
	}

	/// <summary>
	/// 値変更通知イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxTaskValueChangedEventHandler(object sender, CxTaskValueChangedEventArgs e);

	/// <summary>
	/// 値変更通知イベント引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxTaskValueChangedEventArgs : CxTaskExecuteEventArgs
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskValueChangedEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ (メッセージ通知)
		/// </summary>
		/// <param name="taskflow">最上位のタスクフロー</param>
		/// <param name="auxInfo">外部機器情報</param>
		/// <param name="baseDir">基準ディレクトリ</param>
		/// <param name="baseName">基準名称</param>
		/// <param name="changedItem">変更された項目</param>
		/// <param name="oldValue">変更される前の値</param>
		public CxTaskValueChangedEventArgs(CxTaskflow taskflow, CxAuxInfo auxInfo, string baseDir, string baseName, GridItem changedItem, object oldValue)
			: base(taskflow, auxInfo, baseDir, baseName, 1, 1)
		{
			ChangedItem = changedItem;
			OldValue = oldValue;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public override void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;

			if (src is CxTaskExecuteEventArgs)
			{
				base.CopyFrom(src);
			}
			if (src is CxTaskValueChangedEventArgs)
			{
				var _src = (CxTaskValueChangedEventArgs)src;
				this.ChangedItem = _src.ChangedItem;
				this.OldValue = _src.OldValue;
				this.ControlSyntax = _src.ControlSyntax;
			}

			return;
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

			try
			{
				if (src is CxTaskExecuteEventArgs)
				{
					if (base.ContentEquals(src) == false) return false;
				}
				if (src is CxTaskValueChangedEventArgs)
				{
					var _src = (CxTaskValueChangedEventArgs)src;
					if (this.ChangedItem != _src.ChangedItem) return false;
					if (this.OldValue != _src.OldValue) return false;
					if (this.ControlSyntax != _src.ControlSyntax) return false;
				}
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 変更された項目
		/// </summary>
		[NonSerialized]
		public GridItem ChangedItem = null;

		/// <summary>
		/// 変更される前の値
		/// </summary>
		[NonSerialized]
		public object OldValue = null;

		#endregion
	}
}
