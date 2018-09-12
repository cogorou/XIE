/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using XIE;

namespace XIE
{
	/// <summary>
	/// プラグインクラスを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class CxPluginClassAttribute : Attribute
	{
		/// <summary>
		/// 識別子
		/// </summary>
		public string ID = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPluginClassAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="id">識別子</param>
		public CxPluginClassAttribute(string id)
		{
			this.ID = id;
		}
	}

	/// <summary>
	/// プラグインクラスの実行対象の処理が記述されたメソッドを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CxPluginExecuteAttribute : Attribute
	{
		/// <summary>
		/// 識別子
		/// </summary>
		public string ID = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPluginExecuteAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="id">識別子</param>
		public CxPluginExecuteAttribute(string id)
		{
			this.ID = id;
		}
	}

	/// <summary>
	/// プラグインクラスの初期化処理が記述されたメソッドを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CxPluginSetupAttribute : Attribute
	{
		/// <summary>
		/// 識別子
		/// </summary>
		public string ID = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPluginSetupAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="id">識別子</param>
		public CxPluginSetupAttribute(string id)
		{
			this.ID = id;
		}
	}

	/// <summary>
	/// プラグインクラスの解放処理が記述されたメソッドを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CxPluginTearDownAttribute : Attribute
	{
		/// <summary>
		/// 識別子
		/// </summary>
		public string ID = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPluginTearDownAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="id">識別子</param>
		public CxPluginTearDownAttribute(string id)
		{
			this.ID = id;
		}
	}
}
