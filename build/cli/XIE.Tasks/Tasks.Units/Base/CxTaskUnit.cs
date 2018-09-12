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
	/// タスクユニットクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxTaskUnit : System.Object
		, IDisposable
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// ユニーク IDの生成
		/// </summary>
		/// <returns>
		///		生成した ID を返します。
		/// </returns>
		private string GenerateUniqueID()
		{
			var timestamp = DateTime.Now;
			string suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}_{6:000}_{7:X4}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second,
				timestamp.Millisecond, SeqNo++);
			return suffix;
		}
		private static ushort SeqNo = 0;

		/// <summary>
		/// コンストラクタ補助関数
		/// </summary>
		private void _Constructor()
		{
			UniqueID = GenerateUniqueID();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskUnit()
		{
			_Constructor();
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">シリアライズ先</param>
		protected CxTaskUnit(SerializationInfo info, StreamingContext context)
		{
			_Constructor();

			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "UniqueID":
							this.UniqueID = this.GetValue<string>(info, entry.Name);
							break;
						case "Category":
							this.Category = this.GetValue<string>(info, entry.Name);
							break;
						case "Name":
							this.Name = this.GetValue<string>(info, entry.Name);
							break;
						case "IconKey":
							this.IconKey = this.GetValue<string>(info, entry.Name);
							break;
						case "Location":
							this.Location = this.GetValue<Point>(info, entry.Name);
							break;
						case "Breakpoint":
							this.Breakpoint = this.GetValue<bool>(info, entry.Name);
							break;
						case "Selected":
							this.Selected = this.GetValue<bool>(info, entry.Name);
							break;
						case "ControlIn":
							this.ControlIn = this.GetValue<CxTaskPortIn>(info, entry.Name);
							break;
						case "ControlOut":
							this.ControlOut = this.GetValue<CxTaskPortOut>(info, entry.Name);
							break;
						case "DataIn":
							this.DataIn = this.GetValue<CxTaskPortIn[]>(info, entry.Name);
							break;
						case "DataParam":
							this.DataParam = this.GetValue<CxTaskPortIn[]>(info, entry.Name);
							break;
						case "DataOut":
							this.DataOut = this.GetValue<CxTaskPortOut[]>(info, entry.Name);
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
		/// デシリアライズ補助関数
		/// </summary>
		/// <typeparam name="TV">項目の型</typeparam>
		/// <param name="info">データの読み込み先</param>
		/// <param name="name">項目名</param>
		/// <returns>
		///		抽出した値を返します。
		/// </returns>
		protected virtual TV GetValue<TV>(SerializationInfo info, string name)
		{
			return (TV)info.GetValue(name, typeof(TV));
		}

		/// <summary>
		/// シリアライズ
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">シリアライズ先</param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("UniqueID", this.UniqueID);
			info.AddValue("Category", this.Category);
			info.AddValue("Name", this.Name);
			info.AddValue("IconKey", this.IconKey);
			info.AddValue("Location", this.Location);
			info.AddValue("Breakpoint", this.Breakpoint);
			info.AddValue("Selected", this.Selected);
			info.AddValue("ControlIn", this.ControlIn);
			info.AddValue("ControlOut", this.ControlOut);
			info.AddValue("DataIn", this.DataIn);
			info.AddValue("DataParam", this.DataParam);
			info.AddValue("DataOut", this.DataOut);
		}

		#endregion

		#region IDisposable の実装:

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			object clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
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
			if (ReferenceEquals(this, src)) return;

			this.Dispose();

			if (src is CxTaskUnit)
			{
				#region 同一型:
				var _src = (CxTaskUnit)src;

				// (!) UniqueID は複製対象外.
				//this.UniqueID = _src.UniqueID;

				this.Category = _src.Category;
				this.Name = _src.Name;
				this.IconKey = _src.IconKey;
				this.IconImage = _src.IconImage;
				this.Location = _src.Location;
				this.Breakpoint = _src.Breakpoint;
				this.Selected = _src.Selected;

				// コントロール入力.
				if (_src.ControlIn == null)
					this.ControlIn = new CxTaskPortIn("In");
				else if (this.ControlIn == null)
					this.ControlIn = (CxTaskPortIn)_src.ControlIn.Clone();
				else
					this.ControlIn.CopyFrom(_src.ControlIn);

				// コントロール出力.
				if (_src.ControlOut == null)
					this.ControlOut = new CxTaskPortOut("Out");
				else if (this.ControlOut == null)
					this.ControlOut = (CxTaskPortOut)_src.ControlOut.Clone();
				else
					this.ControlOut.CopyFrom(_src.ControlOut);

				// データ入力.
				if (_src.DataIn == null)
					this.DataIn = new CxTaskPortIn[0];
				else
				{
					if (this.DataIn == null)
						this.DataIn = new CxTaskPortIn[_src.DataIn.Length];
					else if (this.DataIn.Length != _src.DataIn.Length)
						this.DataIn = new CxTaskPortIn[_src.DataIn.Length];
					for (int i = 0; i < this.DataIn.Length; i++)
					{
						if (_src.DataIn[i] == null)
							this.DataIn[i] = null;
						else if (this.DataIn[i] == null)
							this.DataIn[i] = (CxTaskPortIn)_src.DataIn[i].Clone();
						else
							this.DataIn[i].CopyFrom(_src.DataIn[i]);
					}
				}

				// パラメータ.
				if (_src.DataParam == null)
					this.DataParam = new CxTaskPortIn[0];
				else
				{
					if (this.DataParam == null)
						this.DataParam = new CxTaskPortIn[_src.DataParam.Length];
					else if (this.DataParam.Length != _src.DataParam.Length)
						this.DataParam = new CxTaskPortIn[_src.DataParam.Length];
					for (int i = 0; i < this.DataParam.Length; i++)
					{
						if (_src.DataParam[i] == null)
							this.DataParam[i] = null;
						else if (this.DataParam[i] == null)
							this.DataParam[i] = (CxTaskPortIn)_src.DataParam[i].Clone();
						else
							this.DataParam[i].CopyFrom(_src.DataParam[i]);
					}
				}

				// データ出力.
				if (_src.DataOut == null)
					this.DataOut = new CxTaskPortOut[0];
				else
				{
					if (this.DataOut == null)
						this.DataOut = new CxTaskPortOut[_src.DataOut.Length];
					else if (this.DataOut.Length != _src.DataOut.Length)
						this.DataOut = new CxTaskPortOut[_src.DataOut.Length];
					for (int i = 0; i < this.DataOut.Length; i++)
					{
						if (_src.DataOut[i] == null)
							this.DataOut[i] = null;
						else if (this.DataOut[i] == null)
							this.DataOut[i] = (CxTaskPortOut)_src.DataOut[i].Clone();
						else
							this.DataOut[i].CopyFrom(_src.DataOut[i]);
					}
				}

				// このタスクユニットを所有するタスクフロー:
				this.SetOwnerTaskflow(_src.GetOwnerTaskflow());

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
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			var _src = (CxTaskUnit)src;

			// (!) UniqueID は比較対象外.
			// if (this.UniqueID != _src.UniqueID) return false;

			if (this.Category != _src.Category) return false;
			if (this.Name != _src.Name) return false;
			if (this.IconKey != _src.IconKey) return false;
			if (this.IconImage != _src.IconImage) return false;
			if (this.Location != _src.Location) return false;
			if (this.Breakpoint != _src.Breakpoint) return false;
			if (this.Selected != _src.Selected) return false;

			if (this.ControlIn == null && _src.ControlIn != null) return false;
			if (this.ControlIn != null && _src.ControlIn == null) return false;
			if (this.ControlIn != null && _src.ControlIn != null)
			{
				if (this.ControlIn.ContentEquals(_src.ControlIn) == false) return false;
			}

			if (this.ControlOut == null && _src.ControlOut != null) return false;
			if (this.ControlOut != null && _src.ControlOut == null) return false;
			if (this.ControlOut != null && _src.ControlOut != null)
			{
				if (this.ControlOut.ContentEquals(_src.ControlOut) == false) return false;
			}

			if (this.DataIn == null && _src.DataIn != null) return false;
			if (this.DataIn != null && _src.DataIn == null) return false;
			if (this.DataIn != null && _src.DataIn != null)
			{
				if (this.DataIn.Length != _src.DataIn.Length) return false;
				for (int i = 0; i < this.DataIn.Length; i++)
				{
					var dst_port = this.DataIn[i];
					var src_port = _src.DataIn[i];
					if (dst_port == null && src_port != null) return false;
					if (dst_port != null && src_port == null) return false;
					if (dst_port != null && src_port != null)
					{
						if (dst_port.ContentEquals(src_port) == false) return false;
					}
				}
			}

			if (this.DataParam == null && _src.DataParam != null) return false;
			if (this.DataParam != null && _src.DataParam == null) return false;
			if (this.DataParam != null && _src.DataParam != null)
			{
				if (this.DataParam.Length != _src.DataParam.Length) return false;
				for (int i = 0; i < this.DataParam.Length; i++)
				{
					var dst_port = this.DataParam[i];
					var src_port = _src.DataParam[i];
					if (dst_port == null && src_port != null) return false;
					if (dst_port != null && src_port == null) return false;
					if (dst_port != null && src_port != null)
					{
						if (dst_port.ContentEquals(src_port) == false) return false;
					}
				}
			}

			if (this.DataOut == null && _src.DataOut != null) return false;
			if (this.DataOut != null && _src.DataOut == null) return false;
			if (this.DataOut != null && _src.DataOut != null)
			{
				if (this.DataOut.Length != _src.DataOut.Length) return false;
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var dst_port = this.DataOut[i];
					var src_port = _src.DataOut[i];
					if (dst_port == null && src_port != null) return false;
					if (dst_port != null && src_port == null) return false;
					if (dst_port != null && src_port != null)
					{
						if (dst_port.ContentEquals(src_port) == false) return false;
					}
				}
			}
			#endregion

			return true;
		}

		#endregion

		#region プロパティ: (基本)

		/// <summary>
		/// ユニーク ID
		/// </summary>
		/// <remarks>
		///		インスタンス生成時に初期化されます。
		///		シリアライズ時に復旧します。
		///		他のプロパティとは異なり複製や比較の対象ではありません。
		/// </remarks>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.UniqueID")]
		public virtual string UniqueID
		{
			get { return m_UniqueID; }
			set { m_UniqueID = value; }
		}
		private string m_UniqueID = "";

		/// <summary>
		/// 分類
		/// </summary>
		[Browsable(true)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.Category")]
		public virtual string Category
		{
			get { return m_Category; }
			set { m_Category = value; }
		}
		private string m_Category = "";

		/// <summary>
		/// 名称
		/// </summary>
		[Browsable(true)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.Name")]
		public virtual string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name = "";

		/// <summary>
		/// 説明文のリソースキー
		/// </summary>
		[Browsable(false)]
		public virtual string DescriptionKey
		{
			get { return m_DescriptionKey; }
			set { m_DescriptionKey = value; }
		}
		private string m_DescriptionKey = "";

		/// <summary>
		/// アイコンキー
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.IconKey")]
		public virtual string IconKey
		{
			get { return m_IconKey; }
			set { m_IconKey = value; }
		}
		private string m_IconKey = "";

		/// <summary>
		/// アイコンイメージ
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public Image IconImage = null;

		#endregion

		#region プロパティ: (入出力)

		/// <summary>
		/// コントロール入力ポート
		/// </summary>
		[Browsable(false)]
		[CxCategory("I/O")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.ControlIn")]
		public virtual CxTaskPortIn ControlIn
		{
			get { return m_ControlIn; }
			set { m_ControlIn = value; }
		}
		private CxTaskPortIn m_ControlIn = new CxTaskPortIn("In");

		/// <summary>
		/// コントロール出力ポート
		/// </summary>
		[Browsable(false)]
		[CxCategory("I/O")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.ControlOut")]
		public virtual CxTaskPortOut ControlOut
		{
			get { return m_ControlOut; }
			set { m_ControlOut = value; }
		}
		private CxTaskPortOut m_ControlOut = new CxTaskPortOut("Out");

		/// <summary>
		/// データ入力ポート
		/// </summary>
		[Browsable(false)]
		[CxCategory("I/O")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.DataIn")]
		public virtual CxTaskPortIn[] DataIn
		{
			get { return m_DataIn; }
			set { m_DataIn = value; }
		}
		private CxTaskPortIn[] m_DataIn = new CxTaskPortIn[0];

		/// <summary>
		/// パラメータポート
		/// </summary>
		[Browsable(false)]
		[CxCategory("I/O")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.DataParam")]
		public virtual CxTaskPortIn[] DataParam
		{
			get { return m_DataParam; }
			set { m_DataParam = value; }
		}
		private CxTaskPortIn[] m_DataParam = new CxTaskPortIn[0];

		/// <summary>
		/// データ出力ポート
		/// </summary>
		[Browsable(false)]
		[CxCategory("I/O")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.DataOut")]
		public virtual CxTaskPortOut[] DataOut
		{
			get { return m_DataOut; }
			set { m_DataOut = value; }
		}
		private CxTaskPortOut[] m_DataOut = new CxTaskPortOut[0];

		#endregion

		#region プロパティ: (操作)

		/// <summary>
		/// 基準位置 (シート上の要素の位置を示します。)
		/// </summary>
		[Browsable(false)]
		[CxCategory("Handling")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.Location")]
		public virtual Point Location
		{
			get { return m_Location; }
			set { m_Location = value; }
		}
		private Point m_Location = new Point();

		/// <summary>
		/// ブレークポイントフラグ
		/// </summary>
		[Browsable(false)]
		[CxCategory("Handling")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.Breakpoint")]
		public virtual bool Breakpoint
		{
			get { return m_Breakpoint; }
			set { m_Breakpoint = value; }
		}
		private bool m_Breakpoint = false;

		/// <summary>
		/// 選択フラグ
		/// </summary>
		[Browsable(false)]
		[CxCategory("Handling")]
		[CxDescription("P:XIE.Tasks.CxTaskNode.Selected")]
		public virtual bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}
		private bool m_Selected = false;

		#endregion

		#region メソッド: (初期化)

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は Form または Control です。)</param>
		/// <param name="e">イベントの内容</param>
		/// <remarks>
		///		インスタンスが生成された直後の初期化処理を行います。
		///		既定では何も行いません。
		///		独自の処理があればオーバーライドして処理を実装してください。
		///		このメソッドはオーナーフォームから呼び出されます。
		///		フォームやコントロール等へのアクセスが可能です。
		/// </remarks>
		public virtual void Setup(object sender, CxTaskSetupEventArgs e)
		{
		}

		#endregion

		#region メソッド: (実行)

		/// <summary>
		/// 処理開始前の準備
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は Form または Control です。)</param>
		/// <param name="e">イベントの内容 (ValueChanged の実行前は CxTaskValueChangedEventArgs が渡されます。)</param>
		/// <remarks>
		///		処理開始前の準備を行います。
		///		既定では何も行いません。
		///		独自の処理があればオーバーライドして処理を実装してください。
		///		このメソッドはオーナーフォームから呼び出されます。
		///		フォームやコントロール等へのアクセスが可能です。
		/// </remarks>
		public virtual void Prepare(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// 処理終了後の復旧
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は Form または Control または BackgroundWorker です。)</param>
		/// <param name="e">イベントの内容 (ValueChanged の実行後は CxTaskValueChangedEventArgs が渡されます。)</param>
		/// <remarks>
		///		処理終了後の復旧を行います。
		///		既定では何も行いません。
		///		独自の処理があればオーバーライドして処理を実装してください。
		///		このメソッドは BackgroundWorker の RunWorkerCompleted イベントハンドラから呼び出されます。
		///		フォームやコントロール等へのアクセスが可能です。
		/// </remarks>
		public virtual void Restore(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は BackgroundWorker です。)</param>
		/// <param name="e">イベントの内容</param>
		/// <remarks>
		///		このメソッドをオーバーライドして独自の処理を実装してください。
		///		既定では何も行いません。
		///		このメソッドは BackgroundWorker の DoWork イベントハンドラから呼び出されます。
		///		フォームやコントロール等へアクセスする場合は Invoke を介する事で可能ですが、
		///		その間、スレッドがブロックされますのでパフォーマンス低下の原因になります。
		/// </remarks>
		public virtual void Execute(object sender, CxTaskExecuteEventArgs e)
		{
		}

		/// <summary>
		/// プロパティ値が変更されたときに実行する処理
		/// </summary>
		/// <param name="sender">イベントの発行元 (通常は PropertyGrid です。)</param>
		/// <param name="e">イベントの内容</param>
		/// <remarks>
		///		このタスクユニットのプロパティ値が変更された後に呼び出されます。
		///		既定の動作は Execute を呼び出します。
		///		挙動を変える場合はオーバーライドして処理を実装してください。
		///		このメソッドは PropertyGrid の PropertyValueChanged イベントハンドラから呼び出されます。
		///		フォームやコントロール等へのアクセスが可能です。
		/// </remarks>
		public virtual void ValueChanged(object sender, CxTaskValueChangedEventArgs e)
		{
			this.Execute(sender, e);
		}

		#endregion

		#region メソッド: (接続データ)

		/// <summary>
		/// 出力データのリセット
		/// </summary>
		public virtual void Reset()
		{
			this.ControlOut.Data = null;
			for (int i = 0; i < this.DataOut.Length; i++)
				this.DataOut[i].Data = null;
		}

		/// <summary>
		/// 接続状態の確認 (指定のデータ出力ポートに接続しているか否かを検査します。)
		/// </summary>
		/// <param name="target_port">データ出力ポート</param>
		/// <returns>
		///		接続している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsConnected(CxTaskPortOut target_port)
		{
			#region コントロール入力ポート:
			{
				if (ReferenceEquals(this.ControlIn.ReferencePort, target_port))
					return true;
			}
			#endregion

			#region データ入力ポート:
			foreach (var item in this.DataIn)
			{
				if (ReferenceEquals(item.ReferencePort, target_port))
					return true;
			}
			#endregion

			#region パラメータポート:
			foreach (var item in this.DataParam)
			{
				if (ReferenceEquals(item.ReferencePort, target_port))
					return true;
			}
			#endregion

			return false;
		}

		/// <summary>
		/// 接続解除 (指定のデータ出力ポートへの接続を解除します。)
		/// </summary>
		/// <param name="target_port">検索対象のデータ出力ポート</param>
		public virtual void Disconnect(CxTaskPortOut target_port)
		{
			#region コントロール入力ポート:
			{
				if (ReferenceEquals(this.ControlIn.ReferencePort, target_port))
				{
					this.ControlIn.ReferenceTask = null;
					this.ControlIn.ReferencePort = null;
					this.ControlIn.ReferenceTaskIndex = -1;
					this.ControlIn.ReferencePortIndex = -1;
				}
			}
			#endregion

			#region データ入力ポート:
			foreach (var item in this.DataIn)
			{
				if (ReferenceEquals(item.ReferencePort, target_port))
				{
					item.ReferenceTask = null;
					item.ReferencePort = null;
					item.ReferenceTaskIndex = -1;
					item.ReferencePortIndex = -1;
				}
			}
			#endregion

			#region パラメータポート:
			foreach (var item in this.DataParam)
			{
				if (ReferenceEquals(item.ReferencePort, target_port))
				{
					item.ReferenceTask = null;
					item.ReferencePort = null;
					item.ReferenceTaskIndex = -1;
					item.ReferencePortIndex = -1;
				}
			}
			#endregion
		}

		#endregion

		#region メソッド: (コード生成)

		/// <summary>
		/// コード生成: 変数宣言
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public virtual void GenerateDeclarationCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
			if (e.TargetMethod.Name == "Execute")
			{
				for (int i = 0; i < this.DataOut.Length; i++)
				{
					var name = e.TaskNames[this];
					var port = this.DataOut[i];

					var variable = new CodeExtraVariable(string.Format("{0}_{1}", name, port.Name), typeof(object));

					#region 変数の型の決定:
					if (port.Types.Length > 0)
					{
						// 最初の型を既定値とする.
						variable.Type = port.Types[0];

						// 現在のデータと比較して決定する.
						if (!ReferenceEquals(port.Data, null))
						{
							Type port_type = null;
							Type data_type = port.Data.GetType();

							#region 現在のデータと一致する型:
							if (port_type == null)
							{
								foreach (var type in port.Types)
								{
									if (type == data_type)
									{
										port_type = type;
										break;
									}
								}
							}
							#endregion

							#region 現在のデータを代入可能な型:
							if (port_type == null)
							{
								foreach (var type in port.Types)
								{
									if (type == typeof(object))
									{
										port_type = data_type;
										break;
									}
									else if (type.IsAssignableFrom(data_type))
									{
										port_type = type;
										break;
									}
								}
							}
							#endregion

							if (port_type != null)
								variable.Type = port_type;
						}
					}
					#endregion

					var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(this, port);
					var value = new KeyValuePair<string, Type>(variable.VariableName, variable.Type);
					e.Variables[key] = value;

					//scope.Add(new CodeSnippetStatement());
					//scope.Add(new CodeCommentStatement(string.Format("{0}: {1} ({2})", name, this.Name, this.Category)));
					scope.Add(variable.Declare());
				}
			}
		}

		/// <summary>
		/// コード生成: 処理部
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">引数</param>
		/// <param name="scope">追加先のスコープ</param>
		public virtual void GenerateProcedureCode(object sender, CxGenerateCodeArgs e, CodeStatementCollection scope)
		{
		}

		#endregion

		#region メソッド: (説明文)

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <param name="key">リソースキー。(※ 省略時（null や Empty が指定された場合）は DescriptionKey プロパティを参照します。)</param>
		/// <returns>
		///		指定されたキーに対応する説明文を返します。
		///		見つからなければ Empty を返します。
		/// </returns>
		public virtual string GetDescription(string key)
		{
			var reskey = (string.IsNullOrWhiteSpace(key)) ? this.DescriptionKey : key;
			string value;
			if (XIE.AxiTextStorage.Texts.TryGetValue(reskey, out value))
			{
				return value;
			}
			else
			{
				var fullname = this.GetType().FullName;

				for (int i=0 ; i<this.DataIn.Length ; i++)
					if (key == string.Format("F:{0}.Descriptions.DataIn{1}", fullname, i))
						return this.DataIn[i].Description;
				for (int i = 0; i < this.DataParam.Length; i++)
					if (key == string.Format("F:{0}.Descriptions.DataParam{1}", fullname, i))
						return this.DataParam[i].Description;
				for (int i = 0; i < this.DataOut.Length; i++)
					if (key == string.Format("F:{0}.Descriptions.DataOut{1}", fullname, i))
						return this.DataOut[i].Description;

				return "";
			}
		}

		#endregion

		#region メソッド: (OwnerTaskflow)

		/// <summary>
		/// このタスクユニットを所有するタスクフローを設定します。派生クラスが子ノードを持つ場合は再帰的に処理します。
		/// </summary>
		/// <param name="owner">所有者となるタスクフロー</param>
		public virtual void SetOwnerTaskflow(CxTaskUnit owner)
		{
			this.m_OwnerTaskflow = owner;
		}

		/// <summary>
		/// このタスクユニットを所有するタスクフローを取得します。
		/// </summary>
		/// <returns>
		///		タスクフローを返します。
		/// </returns>
		public virtual CxTaskUnit GetOwnerTaskflow()
		{
			return this.m_OwnerTaskflow;
		}
		[NonSerialized]
		private CxTaskUnit m_OwnerTaskflow = null;

		#endregion
	}

	#region CxGenerateCodeArgs

	/// <summary>
	/// タスクユニットのコード生成メソッドの引数
	/// </summary>
	[Serializable]
	public class CxGenerateCodeArgs : EventArgs
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		/// <summary>
		/// 言語種別
		/// </summary>
		public ExLanguageType LanguageType;

		/// <summary>
		/// インデントの段数 [0~]
		/// </summary>
		public int IndentLevel;

		/// <summary>
		/// 追加先の名前空間
		/// </summary>
		public CodeNamespace TargetNS;

		/// <summary>
		/// 追加先のクラス
		/// </summary>
		public CodeTypeDeclaration TargetType;

		/// <summary>
		/// 追加先のメソッド
		/// </summary>
		public CodeMemberMethod TargetMethod;

		/// <summary>
		/// 追加先の反復処理タスクのコレクション
		/// </summary>
		public List<CxTaskUnit> TargetIterations;

		/// <summary>
		/// タスク名リスト
		/// </summary>
		public Dictionary<CxTaskUnit, string> TaskNames;

		/// <summary>
		/// クラス名リスト
		/// </summary>
		public Dictionary<CxTaskUnit, string> ClassNames;

		/// <summary>
		/// 変数リスト
		/// </summary>
		public Dictionary<KeyValuePair<CxTaskUnit, CxTaskPortOut>, KeyValuePair<string, Type>> Variables;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGenerateCodeArgs()
		{
			LanguageType = ExLanguageType.None;
			IndentLevel = 0;
			TargetNS = null;
			TargetType = null;
			TargetMethod = null;
			TargetIterations = new List<CxTaskUnit>();
			TaskNames = new Dictionary<CxTaskUnit, string>();
			ClassNames = new Dictionary<CxTaskUnit, string>();
			Variables = new Dictionary<KeyValuePair<CxTaskUnit, CxTaskPortOut>, KeyValuePair<string, Type>>();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <param name="indent_level">インデントの段数 [0~]</param>
		/// <param name="target_ns">追加先の名前空間</param>
		/// <param name="target_type">追加先のクラス</param>
		/// <param name="target_method">追加先のメンバ</param>
		public CxGenerateCodeArgs(
			ExLanguageType language_type,
			int indent_level,
			CodeNamespace target_ns,
			CodeTypeDeclaration target_type,
			CodeMemberMethod target_method
			)
		{
			LanguageType = language_type;
			IndentLevel = indent_level;
			TargetNS = target_ns;
			TargetType = target_type;
			TargetMethod = target_method;
			TargetIterations = new List<CxTaskUnit>();
			TaskNames = new Dictionary<CxTaskUnit, string>();
			ClassNames = new Dictionary<CxTaskUnit, string>();
			Variables = new Dictionary<KeyValuePair<CxTaskUnit, CxTaskPortOut>, KeyValuePair<string, Type>>();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <param name="indent_level">インデントの段数 [0~]</param>
		/// <param name="target_ns">追加先の名前空間</param>
		/// <param name="target_type">追加先のクラス</param>
		/// <param name="target_method">追加先のメンバ</param>
		/// <param name="target_iterations">追加先の反復処理タスクのコレクション</param>
		/// <param name="tasknames">タスク名リスト</param>
		/// <param name="classnames">クラス名リスト</param>
		/// <param name="variables">変数リスト</param>
		public CxGenerateCodeArgs(
			ExLanguageType language_type,
			int indent_level,
			CodeNamespace target_ns,
			CodeTypeDeclaration target_type,
			CodeMemberMethod target_method,
			List<CxTaskUnit> target_iterations,
			Dictionary<CxTaskUnit, string> tasknames,
			Dictionary<CxTaskUnit, string> classnames,
			Dictionary<KeyValuePair<CxTaskUnit, CxTaskPortOut>, KeyValuePair<string, Type>> variables
			)
		{
			LanguageType = language_type;
			IndentLevel = indent_level;
			TargetNS = target_ns;
			TargetType = target_type;
			TargetMethod = target_method;
			TargetIterations = target_iterations;
			TaskNames = tasknames;
			ClassNames = classnames;
			Variables = variables;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var type = this.GetType();
			var asm = Assembly.GetAssembly(type);
			var clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
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
			if (ReferenceEquals(this, src)) return;

			if (src is CxGenerateCodeArgs)
			{
				#region 同一型:
				var _src = (CxGenerateCodeArgs)src;

				this.LanguageType = _src.LanguageType;
				this.IndentLevel = _src.IndentLevel;
				this.TargetNS = _src.TargetNS;
				this.TargetType = _src.TargetType;
				this.TargetMethod = _src.TargetMethod;
				this.TargetIterations = _src.TargetIterations;
				this.TaskNames = _src.TaskNames;
				this.ClassNames = _src.ClassNames;
				this.Variables = _src.Variables;

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
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			#region 同一型の比較:
			var _src = (CxGenerateCodeArgs)src;

			if (this.LanguageType != _src.LanguageType) return false;
			if (this.IndentLevel != _src.IndentLevel) return false;
			if (this.TargetNS != _src.TargetNS) return false;
			if (this.TargetType != _src.TargetType) return false;
			if (this.TargetMethod != _src.TargetMethod) return false;
			if (this.TargetIterations != _src.TargetIterations) return false;
			if (this.TaskNames != _src.TaskNames) return false;
			if (this.ClassNames != _src.ClassNames) return false;
			if (this.Variables != _src.Variables) return false;
			#endregion

			return true;
		}

		#endregion

		#region メソッド: (変数リスト)

		/// <summary>
		/// 指定された入力ポートの依存先の出力ポートの変数名と型を取得します。
		/// </summary>
		/// <param name="port">入力ポート</param>
		/// <returns>
		///		変数名と型のペアを返します。
		/// </returns>
		public KeyValuePair<string, Type> GetVariable(CxTaskPortIn port)
		{
			var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(port.ReferenceTask, port.ReferencePort);
			return Variables[key];
		}

		/// <summary>
		/// 依存先のタスクの出力ポートの変数名と型を取得します。
		/// </summary>
		/// <param name="task">依存先のタスク</param>
		/// <param name="port">依存先の出力ポート</param>
		/// <returns>
		///		変数名と型のペアを返します。
		/// </returns>
		public KeyValuePair<string, Type> GetVariable(CxTaskUnit task, CxTaskPortOut port)
		{
			var key = new KeyValuePair<CxTaskUnit, CxTaskPortOut>(task, port);
			return Variables[key];
		}

		#endregion
	}

	#endregion
}
