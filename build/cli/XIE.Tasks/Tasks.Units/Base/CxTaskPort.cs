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
	/// データ入力ポートクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxTaskPortIn : System.Object
		, ISerializable
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortIn()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="types">接続可能な型</param>
		public CxTaskPortIn(string name, params Type[] types)
		{
			this.Name = name;
			if (types != null)
				this.Types = types;
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxTaskPortIn(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "Name":
							this.Name = this.GetValue<string>(info, entry.Name);
							break;
						case "Description":
							this.Description = this.GetValue<string>(info, entry.Name);
							break;
						case "ReferenceTaskIndex":
							this.ReferenceTaskIndex = this.GetValue<int>(info, entry.Name);
							break;
						case "ReferencePortIndex":
							this.ReferencePortIndex = this.GetValue<int>(info, entry.Name);
							break;
						case "Types.Length":
							{
								var types = new List<Type>();
								int length = this.GetValue<int>(info, entry.Name);
								for (int i = 0; i < length; i++)
								{
									try
									{
										var fullname = this.GetValue<string>(info, string.Format("Types{0}", i));
										var type = Type.GetType(fullname);
										types.Add(type);
									}
									catch (System.Exception)
									{
									}
								}
								this.Types = types.ToArray();
							}
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
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", this.Name);
			info.AddValue("Description", this.Description);
			info.AddValue("ReferenceTaskIndex", this.ReferenceTaskIndex);
			info.AddValue("ReferencePortIndex", this.ReferencePortIndex);

			if (this.Types == null)
			{
				info.AddValue("Types.Length", 0);
			}
			else
			{
				info.AddValue("Types.Length", this.Types.Length);
				int index = 0;
				foreach (var type in this.Types)
				{
					if (type != null)
					{
						info.AddValue(string.Format("Types{0}", index), type.FullName);
						index++;
					}
				}
			}
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

			if (src is CxTaskPortIn)
			{
				#region 同一型:
				var _src = (CxTaskPortIn)src;

				this.Name = _src.Name;
				this.Description = _src.Description;
				this.SubData = _src.SubData;

				this.ReferencePortIndex = _src.ReferencePortIndex;
				this.ReferenceTaskIndex = _src.ReferenceTaskIndex;
				this.ReferencePort = _src.ReferencePort;
				this.ReferenceTask = _src.ReferenceTask;

				if (_src.Types == null)
					this.Types = null;
				else
				{
					this.Types = new Type[_src.Types.Length];
					for (int i = 0; i < this.Types.Length; i++)
						this.Types[i] = _src.Types[i];
				}

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

			var _src = (CxTaskPortIn)src;

			if (this.Name != _src.Name) return false;
			if (this.Description != _src.Description) return false;
			
			if (this.ReferencePortIndex != _src.ReferencePortIndex) return false;
			if (this.ReferenceTaskIndex != _src.ReferenceTaskIndex) return false;
			if (this.ReferencePort != _src.ReferencePort) return false;
			if (this.ReferenceTask != _src.ReferenceTask) return false;

			if (this.Types == null && _src.Types != null) return false;
			if (this.Types != null && _src.Types == null) return false;
			if (this.Types != null && _src.Types != null)
			{
				if (this.Types.Length != _src.Types.Length) return false;
				for (int i = 0; i < this.Types.Length; i++)
					if (this.Types[i] != _src.Types[i]) return false;
			}

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 名称
		/// </summary>
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.Name")]
		public virtual string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name = "";

		/// <summary>
		/// 接続可能な型
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.Types")]
		[XmlArrayItem(typeof(Type))]
		public virtual Type[] Types
		{
			get { return m_Types; }
			set { m_Types = value; }
		}
		private Type[] m_Types = new Type[0];

		/// <summary>
		/// 説明
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.Description")]
		public virtual string Description
		{
			get { return m_Description; }
			set { m_Description = value; }
		}
		private string m_Description = "";

		/// <summary>
		/// 接続されているデータの取得
		/// </summary>
		/// <returns>
		///		接続されているデータを返します。
		///		未接続の場合は null を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.Data")]
		public virtual object Data
		{
			get
			{
				if (IsConnected)
					return this.ReferencePort.Data;
				return SubData;
			}
		}

		/// <summary>
		/// 参照先のタスクユニット [-1:未接続、0~:接続先の指標]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.ReferenceTaskIndex")]
		public virtual int ReferenceTaskIndex
		{
			get { return m_ReferenceTaskIndex; }
			set { m_ReferenceTaskIndex = value; }
		}
		private int m_ReferenceTaskIndex = -1;

		/// <summary>
		/// 参照先のデータ出力ポート [-1:未接続、0~:接続先の指標]
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.ReferencePortIndex")]
		public virtual int ReferencePortIndex
		{
			get { return m_ReferencePortIndex; }
			set { m_ReferencePortIndex = value; }
		}
		private int m_ReferencePortIndex = -1;

		/// <summary>
		/// 参照先のタスクユニット
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public CxTaskUnit ReferenceTask = null;

		/// <summary>
		/// 参照先のデータ出力ポート
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public CxTaskPortOut ReferencePort = null;

		#endregion

		#region 代用データ:

		/// <summary>
		/// 未接続の場合に代用するデータ
		/// </summary>
		[NonSerialized]
		internal object SubData = null;

		/// <summary>
		/// 代用データのリセット
		/// </summary>
		internal void ResetSubData()
		{
			this.SubData = null;
			if (this.Types.Length == 0) return;
			if (this.Types[0] == null) return;
			var type = this.Types[0];
			if (type == typeof(string))
				this.SubData = "";
			else if (type == typeof(Color))
				this.SubData = Color.White;
			else if (type == typeof(DateTime))
				this.SubData = DateTime.Now;
			else if (type.IsPrimitive)
				this.SubData = Activator.CreateInstance(type);
			else if (type.IsEnum)
				this.SubData = Activator.CreateInstance(type);
			else if (type.IsValueType)
				this.SubData = Activator.CreateInstance(type);
		}

		/// <summary>
		/// 代用データをシリアライズ可能か否かを判定します。
		/// </summary>
		/// <returns>
		///		可能な場合は true 、それ以外は false を返します。
		/// </returns>
		internal bool CanSerializeSubData()
		{
			if (this.Types.Length > 0 &&
				this.Types[0] != null &&
				this.SubData != null)
			{
				var decl = this.Types[0];
				var type = this.SubData.GetType();
				if (decl.IsAssignableFrom(type))
				{
					if (type == typeof(string))
						return true;
					else if (type == typeof(Color))
						return true;
					else if (type == typeof(DateTime))
						return true;
					else if (type.IsPrimitive)
						return true;
					else if (type.IsEnum)
						return true;
					else if (type.IsValueType)
						return true;
				}
			}
			return false;
		}

		#endregion

		#region メソッド: (接続)

		/// <summary>
		/// 接続状態の確認
		/// </summary>
		/// <returns>
		///		何か接続されている場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortIn.IsConnected")]
		public virtual bool IsConnected
		{
			get
			{
				return (
					this.ReferenceTask != null &&
					this.ReferencePort != null
					);
			}
		}

		/// <summary>
		/// 接続解除
		/// </summary>
		public virtual void Disconnect()
		{
			this.ReferenceTask = null;
			this.ReferencePort = null;
			this.ReferenceTaskIndex = -1;
			this.ReferencePortIndex = -1;
		}

		/// <summary>
		/// 接続
		/// </summary>
		/// <param name="task">接続先のタスク</param>
		/// <param name="port_type">接続先のポートの種類 [ControlOut, DataOut]</param>
		/// <param name="port_index">接続先のポートの指標 [0~]</param>
		public virtual void Connect(CxTaskUnit task, ExOutputPortType port_type, int port_index)
		{
			if (task == null)
				throw new CxException(ExStatus.InvalidParam, "task is null.");

			switch (port_type)
			{
				case ExOutputPortType.ControlOut:
					if (port_index != 0)
						throw new CxException(ExStatus.InvalidParam, "port_index is not 0.");
					this.ReferenceTask = task;
					this.ReferencePort = task.ControlOut;
					break;
				case ExOutputPortType.DataOut:
					if (port_index < 0)
						throw new CxException(ExStatus.InvalidParam, "port_index is less than 0.");
					if (port_index >= task.DataOut.Length)
						throw new CxException(ExStatus.InvalidParam, string.Format("port_index is greater than or equal to {0}.", task.DataOut.Length));
					this.ReferenceTask = task;
					this.ReferencePort = task.DataOut[port_index];
					break;
				default:
					throw new CxException(ExStatus.InvalidParam);
			}
		}

		/// <summary>
		/// 接続可否の確認
		/// </summary>
		/// <param name="task">接続先のタスク</param>
		/// <param name="port_type">接続先のポートの種類 [ControlOut, DataOut]</param>
		/// <param name="port_index">接続先のポートの指標 [0~]</param>
		/// <returns>
		///		接続可能な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool CanConnect(CxTaskUnit task, ExOutputPortType port_type, int port_index)
		{
			if (task == null) return false;
			switch (port_type)
			{
				case ExOutputPortType.ControlOut:
					if (port_index != 0)
						return false;
					return CanConnect(task, task.ControlOut);
				case ExOutputPortType.DataOut:
					if (!(0 <= port_index && port_index < task.DataOut.Length))
						return false;
					return CanConnect(task, task.DataOut[port_index]);
				default:
					return false;
			}
		}

		/// <summary>
		/// 接続可否の確認
		/// </summary>
		/// <param name="task">接続先のタスク</param>
		/// <param name="port">接続先のポート</param>
		/// <returns>
		///		接続可能な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool CanConnect(CxTaskUnit task, CxTaskPortOut port)
		{
			if (task == null) return false;
			if (port == null) return false;

			// 型が明示されていない.
			if (this.Types.Length == 0 ||
				port.Types.Length == 0)
				return true;

			foreach (Type type in this.Types)
			{
				if (type == null) continue;

				// 同一の型.
				{
					int index = Array.IndexOf(port.Types, type);
					if (index >= 0)
						return true;
				}
				// 代入可能な型.
				{
					int index = Array.FindIndex(port.Types, (item) => { return (item != null && type.IsAssignableFrom(item)); });
					if (index >= 0)
						return true;
				}
				// ダウンキャストの可能性がある型.
				{
					int index = Array.FindIndex(port.Types, (item) => { return (item != null && item.IsAssignableFrom(type)); });
					if (index >= 0)
						return true;
				}
				// プリミティブ型.
				if (type.IsPrimitive)
				{
					int index = Array.FindIndex(port.Types, (item) => { return (item != null && item.IsPrimitive); });
					if (index >= 0)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 変換可否の検査
		/// </summary>
		/// <param name="data_type">検査対象のデータタイプ</param>
		/// <returns>
		///		変換可能な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool CanConvertFrom(Type data_type)
		{
			if (this.Types.Length == 0) return true;
			foreach (Type type in this.Types)
			{
				if (type == null) continue;
				if (type.IsAssignableFrom(data_type))
				{
					return true;
				}
				else if (type.IsPrimitive)
				{
					if (data_type.IsPrimitive) return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 整合性の確認
		/// </summary>
		/// <param name="essential">接続状態の必要条件 ※ 1 の場合は未接続をエラーとします。</param>
		/// <returns>
		///		接続状態を返します。
		/// </returns>
		public virtual bool CheckValidity(bool essential = false)
		{
			if (essential)
			{
				if (this.IsConnected == false)
					throw new ArgumentException(this.Name + " is not connected.");
			}

			if (this.IsConnected)
			{
				if (0 < this.Types.Length)
				{
					if (this.Data == null)
						throw new ArgumentException(this.Name + " is null.");

					if (this.CanConvertFrom(this.Data.GetType()) == false)
						throw new ArgumentException(this.Name + " is invalid type.");
				}
			}

			return this.IsConnected;
		}

		#endregion

		#region SelfConverter:

		/// <summary>
		/// このクラス固有のコンバータ
		/// </summary>
		private class SelfConverter : CxSortingConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(CxTaskPortIn)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		インスタンスの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is CxTaskPortIn)
				{
					var _value = (CxTaskPortIn)value;
					return string.Format("{0}",
						_value.Name
						);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のインスタンスを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					var values = value.ToString().Split(new char[] { ',' });
					var _dst = new CxTaskPortIn();
					if (values.Length > 0)
						_dst.Name = values[0];
					return _dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}

	/// <summary>
	/// データ出力ポートクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxTaskPortOut : System.Object
		, ISerializable
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskPortOut()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="types">データの型</param>
		public CxTaskPortOut(string name, params Type[] types)
		{
			this.Name = name;
			if (types != null)
				this.Types = types;
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// デシリアライズ (コンストラクタ)
		/// </summary>
		/// <param name="info">データの読み込み先</param>
		/// <param name="context">デシリアライズ元</param>
		protected CxTaskPortOut(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "Name":
							this.Name = this.GetValue<string>(info, entry.Name);
							break;
						case "Description":
							this.Description = this.GetValue<string>(info, entry.Name);
							break;
						case "Types.Length":
							{
								var types = new List<Type>();
								int length = this.GetValue<int>(info, entry.Name);
								for (int i = 0; i < length; i++)
								{
									try
									{
										var fullname = this.GetValue<string>(info, string.Format("Types{0}", i));
										var type = Type.GetType(fullname);
										types.Add(type);
									}
									catch (System.Exception)
									{
									}
								}
								this.Types = types.ToArray();
							}
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
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", this.Name);
			info.AddValue("Description", this.Description);

			if (this.Types == null)
			{
				info.AddValue("Types.Length", 0);
			}
			else
			{
				info.AddValue("Types.Length", this.Types.Length);
				int index = 0;
				foreach (var type in this.Types)
				{
					if (type != null)
					{
						info.AddValue(string.Format("Types{0}", index), type.FullName);
						index++;
					}
				}
			}
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

			if (src is CxTaskPortOut)
			{
				#region 同一型:
				var _src = (CxTaskPortOut)src;

				this.Name = _src.Name;
				this.Data = _src.Data;
				this.Description = _src.Description;

				if (_src.Types == null)
					this.Types = null;
				else
				{
					this.Types = new Type[_src.Types.Length];
					for (int i = 0; i < this.Types.Length; i++)
						this.Types[i] = _src.Types[i];
				}

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

			var _src = (CxTaskPortOut)src;

			if (this.Name != _src.Name) return false;
			if (this.Data != _src.Data) return false;
			if (this.Description != _src.Description) return false;

			if (this.Types == null && _src.Types != null) return false;
			if (this.Types != null && _src.Types == null) return false;
			if (this.Types != null && _src.Types != null)
			{
				if (this.Types.Length != _src.Types.Length) return false;
				for (int i = 0; i < this.Types.Length; i++)
					if (this.Types[i] != _src.Types[i]) return false;
			}

			return true;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 名称
		/// </summary>
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortOut.Name")]
		public virtual string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name = "";

		/// <summary>
		/// データの型
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortOut.Types")]
		[XmlArrayItem(typeof(Type))]
		public virtual Type[] Types
		{
			get { return m_Types; }
			set { m_Types = value; }
		}
		private Type[] m_Types = new Type[0];

		/// <summary>
		/// 説明
		/// </summary>
		[Browsable(false)]
		[CxCategory("Basic")]
		[CxDescription("P:XIE.Tasks.CxTaskPortOut.Description")]
		public virtual string Description
		{
			get { return m_Description; }
			set { m_Description = value; }
		}
		private string m_Description = "";

		/// <summary>
		/// データ
		/// </summary>
		[XmlIgnore]
		[NonSerialized]
		public object Data = null;

		#endregion

		#region メソッド: (接続データ)

		/// <summary>
		/// 接続状態の確認
		/// </summary>
		/// <param name="tasks">検索対象のタスクユニット</param>
		/// <returns>
		///		何か接続されている場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsConnected(IEnumerable<CxTaskUnit> tasks)
		{
			foreach (var task in tasks)
			{
				if (task != null)
				{
					if (task.IsConnected(this))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 接続解除
		/// </summary>
		/// <param name="tasks">検索対象のタスクユニット</param>
		public virtual void Disconnect(IEnumerable<CxTaskUnit> tasks)
		{
			foreach (var task in tasks)
			{
				if (task != null)
				{
					task.Disconnect(this);
				}
			}
		}

		#endregion

		#region SelfConverter:

		/// <summary>
		/// このクラス固有のコンバータ
		/// </summary>
		private class SelfConverter : CxSortingConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(CxTaskPortOut)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		インスタンスの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is CxTaskPortOut)
				{
					var _value = (CxTaskPortOut)value;
					return string.Format("{0}",
						_value.Name
						);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のインスタンスを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					var values = value.ToString().Split(new char[] { ',' });
					var _dst = new CxTaskPortOut();
					if (values.Length > 0)
						_dst.Name = values[0];
					return _dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}

	/// <summary>
	/// タスクユニットのデータ入力ポートのデータをプロパティグリッドに表示するクラス
	/// </summary>
	class CxTaskPortInPropertyDescriptor : PropertyDescriptor
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="category">カテゴリ名</param>
		/// <param name="is_readonly">読み取り専用か否か</param>
		/// <param name="port">対象のデータ入力ポート</param>
		/// <param name="attrs">プロパティの属性を格納している Attribute 型の配列。</param>
		public CxTaskPortInPropertyDescriptor(string category, bool is_readonly, CxTaskPortIn port, Attribute[] attrs)
			: base(port.Name, attrs)
		{
			this.m_Category = category;
			this.m_IsReadOnly = is_readonly;
			this.Port = port;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// カテゴリ名
		/// </summary>
		public string m_Category = "";

		/// <summary>
		/// 読み取り専用か否か
		/// </summary>
		private bool m_IsReadOnly = false;

		/// <summary>
		/// 対象のデータ入力ポート
		/// </summary>
		public CxTaskPortIn Port = null;

		#endregion

		#region 継承したメンバ:

		/// <summary>
		/// コンポーネントの値を別の値に設定します。
		/// </summary>
		/// <param name="component"></param>
		/// <param name="value"></param>
		public override void SetValue(object component, object value)
		{
			this.Port.SubData = value;
		}

		/// <summary>
		/// コンポーネントのプロパティの現在の値を取得します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override object GetValue(object component)
		{
			return this.Port.Data;
		}

		/// <summary>
		/// コンポーネントのプロパティの値を既定値にリセットします。
		/// </summary>
		/// <param name="component"></param>
		public override void ResetValue(object component)
		{
		}

		/// <summary>
		/// オブジェクトをリセットしたときに、そのオブジェクトの値が変化するかどうかを示す値を返します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool CanResetValue(object component)
		{
			return false;
		}

		/// <summary>
		/// プロパティの値を永続化する必要があるかどうかを示す値を決定します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		/// <summary>
		/// プロパティが読み取り専用かどうかを示す値を取得します。
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				if (this.Port.Data == null) return true;
				if (this.Port.IsConnected) return true;
				return this.m_IsReadOnly;
			}
		}

		/// <summary>
		/// プロパティの型を取得します。
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				if (this.Port.Types.Length == 0) return typeof(object);
				var data = this.Port.Data;
				if (data != null)
					return data.GetType();
				else
				{
					foreach (var type in this.Port.Types)
					{
						if (type != null)
							return type;
					}
				}
				return typeof(object);
			}
		}

		/// <summary>
		/// プロパティが関連付けられているコンポーネントの型を取得します。
		/// </summary>
		public override Type ComponentType
		{
			get
			{
				return this.Port.GetType();
			}
		}

		/// <summary>
		/// メンバーが属するカテゴリの名前を取得します。
		/// </summary>
		public override string Category
		{
			get
			{
				return this.m_Category;
			}
		}

		/// <summary>
		/// メンバーの説明を取得します。
		/// </summary>
		public override string Description
		{
			get
			{
				return this.Port.Description;
			}
		}

		#endregion
	}

	/// <summary>
	/// タスクユニットのデータ出力ポートのデータをプロパティグリッドに表示するクラス
	/// </summary>
	class CxTaskPortOutPropertyDescriptor : PropertyDescriptor
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="category">カテゴリ名</param>
		/// <param name="is_readonly">読み取り専用か否か</param>
		/// <param name="port">対象のデータ出力ポート</param>
		/// <param name="attrs">プロパティの属性を格納している Attribute 型の配列。</param>
		public CxTaskPortOutPropertyDescriptor(string category, bool is_readonly, CxTaskPortOut port, Attribute[] attrs)
			: base(port.Name, attrs)
		{
			this.m_Category = category;
			this.m_IsReadOnly = is_readonly;
			this.Port = port;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// カテゴリ名
		/// </summary>
		private string m_Category = "";

		/// <summary>
		/// 読み取り専用か否か
		/// </summary>
		private bool m_IsReadOnly = false;

		/// <summary>
		/// 対象のデータ出力ポート
		/// </summary>
		public CxTaskPortOut Port = null;

		#endregion

		#region 継承したメンバ:

		/// <summary>
		/// コンポーネントの値を別の値に設定します。
		/// </summary>
		/// <param name="component"></param>
		/// <param name="value"></param>
		public override void SetValue(object component, object value)
		{
			this.Port.Data = value;
		}

		/// <summary>
		/// コンポーネントのプロパティの現在の値を取得します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override object GetValue(object component)
		{
			return this.Port.Data;
		}

		/// <summary>
		/// コンポーネントのプロパティの値を既定値にリセットします。
		/// </summary>
		/// <param name="component"></param>
		public override void ResetValue(object component)
		{
		}

		/// <summary>
		/// オブジェクトをリセットしたときに、そのオブジェクトの値が変化するかどうかを示す値を返します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool CanResetValue(object component)
		{
			return false;
		}

		/// <summary>
		/// プロパティの値を永続化する必要があるかどうかを示す値を決定します。
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		/// <summary>
		/// プロパティが読み取り専用かどうかを示す値を取得します。
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				if (this.Port.Data == null) return true;
				return this.m_IsReadOnly;
			}
		}

		/// <summary>
		/// プロパティの型を取得します。
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				if (this.Port.Types.Length == 0) return typeof(object);
				var data = this.Port.Data;
				if (data != null)
					return data.GetType();
				else
				{
					foreach (var type in this.Port.Types)
					{
						if (type != null)
							return type;
					}
				}
				return typeof(object);
			}
		}

		/// <summary>
		/// プロパティが関連付けられているコンポーネントの型を取得します。
		/// </summary>
		public override Type ComponentType
		{
			get
			{
				return this.Port.GetType();
			}
		}

		/// <summary>
		/// メンバーが属するカテゴリの名前を取得します。
		/// </summary>
		public override string Category
		{
			get
			{
				return this.m_Category;
			}
		}

		/// <summary>
		/// メンバーの説明を取得します。
		/// </summary>
		public override string Description
		{
			get
			{
				return this.Port.Description;
			}
		}

		#endregion
	}
}
