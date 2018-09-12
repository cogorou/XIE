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
	/// 関連ライブラリであることを意味する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class CxRelationClassAttribute : Attribute
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRelationClassAttribute()
		{
		}
	}

	/// <summary>
	/// 関連ライブラリの初期化処理が記述されたメソッドを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CxRelationSetupAttribute : Attribute
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRelationSetupAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="level">実行レベル [0:無効、1:暗黙的に実行する、2~:任意のタイミングで実行する]</param>
		public CxRelationSetupAttribute(int level)
		{
		}

		/// <summary>
		/// 実行レベル [0:無効、1:暗黙的に実行する、2~:任意のタイミングで実行する]
		/// </summary>
		[CxDescription("P:XIE.CxRelationSetupAttribute.Level")]
		[DefaultValue(1)]
		public int Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}
		private int m_Level = 1;
	}

	/// <summary>
	/// 関連ライブラリの解放処理が記述されたメソッドを指定する属性です。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CxRelationTearDownAttribute : Attribute
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRelationTearDownAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="level">実行レベル [0:無効、1:暗黙的に実行する、2~:任意のタイミングで実行する]</param>
		public CxRelationTearDownAttribute(int level)
		{
		}

		/// <summary>
		/// 実行レベル [0:無効、1:暗黙的に実行する、2~:任意のタイミングで実行する]
		/// </summary>
		[CxDescription("P:XIE.CxRelationTearDownAttribute.Level")]
		[DefaultValue(1)]
		public int Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}
		private int m_Level = 1;
	}
}
