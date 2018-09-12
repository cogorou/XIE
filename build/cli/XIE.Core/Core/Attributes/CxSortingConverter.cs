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
using System.Globalization;
using System.Xml;

namespace XIE
{
	/// <summary>
	/// 型変換クラス
	/// </summary>
	public class CxSortingConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// 指定されたデータ型に対して公開されているプロパティのコレクションの取得
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <param name="attributes"></param>
		/// <returns>
		///		公開されているプロパティのコレクションを定義の順序に並び替えて返します。
		/// </returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			var items = base.GetProperties(context, value, attributes);
			var names = new List<string>();
			foreach (var info in value.GetType().GetProperties())
				names.Add(info.Name);
			return items.Sort(names.ToArray());
		}
	}
}
