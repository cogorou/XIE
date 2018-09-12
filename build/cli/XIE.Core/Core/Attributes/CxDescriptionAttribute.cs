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
	/// Description 属性
	/// </summary>
	public partial class CxDescriptionAttribute : DescriptionAttribute
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="key">メンバを特定するキー (例: P:XIE.CxException.Code)</param>
		public CxDescriptionAttribute(string key)
			: base(key)
		{
		}

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <returns>
		///		コンストラクタで指定されたキーに関連付けられた説明文を取得します。
		/// </returns>
		public override string Description
		{
			get
			{
				try
				{
					if (base.DescriptionValue.StartsWith("T:") ||
						base.DescriptionValue.StartsWith("M:") ||
						base.DescriptionValue.StartsWith("E:") ||
						base.DescriptionValue.StartsWith("P:") ||
						base.DescriptionValue.StartsWith("F:") ||
						base.DescriptionValue.StartsWith("N:"))
					{
						string description = AxiTextStorage.GetValue(base.DescriptionValue);
					    return description;
					}
				}
				catch (System.Exception)
				{
				}
				return base.DescriptionValue;
			}
		}
	}
}
