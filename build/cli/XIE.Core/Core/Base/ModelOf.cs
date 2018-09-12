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
using System.Threading;
using System.Diagnostics;

namespace XIE
{
	/// <summary>
	/// 要素モデル生成クラス
	/// </summary>
	public class ModelOf
	{
		#region 生成関数:

		/// <summary>
		/// 生成関数
		/// </summary>
		/// <typeparam name="TE">型</typeparam>
		/// <returns>
		///		指定された型の要素モデルを返します。\n
		///		該当しない場合は TxModel.Default を返します。
		/// </returns>
		public static TxModel From<TE>()
		{
			return ModelOf.From(typeof(TE));
		}

		/// <summary>
		/// 生成関数
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		指定された型の要素モデルを返します。\n
		///		該当しない場合は TxModel.Default を返します。
		/// </returns>
		public static TxModel From(Type type)
		{
			if (type == null) return TxModel.Default;
			else if (type == typeof(IntPtr)) return TxModel.Ptr(1);
			else if (type == typeof(HxModule)) return TxModel.Ptr(1);
			else if (type.IsPrimitive)
			{
				var model = new TxModel();
				model.Type = Axi.TypeOf(type);
				model.Pack = (model.Type != ExType.None) ? 1 : 0;
				return model;
			}
			else if (type.IsEnum)
			{
				var model = new TxModel();
				model.Type = Axi.TypeOf(type.GetEnumUnderlyingType());
				model.Pack = 1;
				return model;
			}

			// [CxModelOfAttribute] 属性が付加された構造体か否かの検査.
			object[] attrs = type.GetCustomAttributes(typeof(XIE.CxModelOfAttribute), true);
			if (attrs.Length != 0)
			{
				foreach (var attr in attrs)
				{
					if (attr is CxModelOfAttribute)
					{
						var model_attr = (CxModelOfAttribute)attr;
						var model = new TxModel();
						model.Type = model_attr.Type;
						model.Pack = model_attr.Pack;
						return model;
					}
				}
			}

			return TxModel.Default;
		}

		#endregion
	}
}
