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
	/// Category 属性
	/// </summary>
	public partial class CxCategoryAttribute : CategoryAttribute
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="key">メンバを特定するキー (例: F:XIE.Category.Parameter)</param>
		public CxCategoryAttribute(string key)
			: base(key)
		{
		}

		/// <summary>
		/// 説明文の取得
		/// </summary>
		/// <returns>
		///		コンストラクタで指定されたキーに関連付けられた説明文を取得します。
		/// </returns>
		protected override string GetLocalizedString(string value)
		{
			try
			{
				if (base.Category.StartsWith("T:") ||
					base.Category.StartsWith("M:") ||
					base.Category.StartsWith("E:") ||
					base.Category.StartsWith("P:") ||
					base.Category.StartsWith("F:") ||
					base.Category.StartsWith("N:"))
				{
					string description = AxiTextStorage.GetValue(value);
					return description;
				}
			}
			catch (System.Exception)
			{
			}
			return base.GetLocalizedString(value);
		}
	}
}
