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
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace XIE
{
	/// <summary>
	/// 要素モデル属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Struct)]
	public partial class CxModelOfAttribute : Attribute
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxModelOfAttribute()
		{
			Type = ExType.None;
			Pack = 0;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="pack">パック数 [1~]</param>
		public CxModelOfAttribute(ExType type, int pack)
		{
			Type = type;
			Pack = pack;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 型
		/// </summary>
		public ExType Type;

		/// <summary>
		/// パック数 [1~]
		/// </summary>
		public int Pack;

		#endregion
	}
}
