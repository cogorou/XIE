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
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;

namespace XIE
{
	/// <summary>
	/// XML シリアライザ 補助関数群
	/// </summary>
	public static partial class AxiXml
	{
		#region Utility

		/// <summary>
		/// ドロップされた情報からXMLファイル名称を取り出します。
		/// </summary>
		/// <param name="e">ドロップ情報</param>
		/// <returns>
		///		取り出したXMLファイル名称のリストを返します。
		/// </returns>
		public static List<string> GetDropFiles(DragEventArgs e)
		{
			string[] drops = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			List<string> files = new List<string>(drops.Length);
			for (int i = 0; i < drops.Length; i++)
			{
				if (drops[i].EndsWith(".xml", true, null))
				{
					files.Add(drops[i]);
				}
			}
			return files;
		}

		/// <summary>
		/// 指定されたXMLファイルのトップノードを判定します.
		/// </summary>
		/// <param name="url">XMLファイルへのパス</param>
		/// <param name="topnode">トップノードの名前</param>
		/// <returns>
		///		トップノードが一致すれば true を返します。
		///		そうでなければ false を返します。
		/// </returns>
		public static bool CheckTopNode(string url, string topnode)
		{
			try
			{
				System.Xml.XmlTextReader parser = new System.Xml.XmlTextReader(url);
				while (parser.Read())
				{
					if (parser.NodeType == System.Xml.XmlNodeType.Element)
						return (parser.Name == topnode);
				}
			}
			catch (System.Exception)
			{
			}
			return false;
		}

		/// <summary>
		/// 指定されたXMLファイルのトップノードを取得します.
		/// </summary>
		/// <param name="url">XMLファイルへのパス</param>
		/// <returns>
		///		トップノードを返します。
		///		なければ空白を返します。
		/// </returns>
		public static string GetTopNode(string url)
		{
			try
			{
				System.Xml.XmlTextReader parser = new System.Xml.XmlTextReader(url);
				while (parser.Read())
				{
					if (parser.NodeType == System.Xml.XmlNodeType.Element)
						return parser.Name;
				}
			}
			catch (System.Exception)
			{
			}
			return "";
		}

		#endregion

		#region GetValue

		/// <summary>
		/// オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <typeparam name="T">項目の型</typeparam>
		/// <param name="reader">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <returns>
		///		逆シリアル化されたオブジェクトを返します。
		/// </returns>
		public static T GetValue<T>(XmlReader reader)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			return (T)xs.Deserialize(reader);
		}

		/// <summary>
		/// オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <param name="type">項目のタイプオブジェクト</param>
		/// <returns>
		///		逆シリアル化されたオブジェクトを返します。
		/// </returns>
		public static object GetValue(XmlReader reader, Type type)
		{
			XmlSerializer xs = new XmlSerializer(type);
			return xs.Deserialize(reader);
		}

		/// <summary>
		/// オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <typeparam name="T">項目の型</typeparam>
		/// <param name="reader">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <param name="name">ノード名</param>
		/// <returns>
		///		逆シリアル化されたオブジェクトを返します。
		/// </returns>
		public static T GetValue<T>(XmlReader reader, string name)
		{
			return (T)GetValue(reader, name, typeof(T));
		}

		/// <summary>
		/// オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <typeparam name="T">項目の型</typeparam>
		/// <param name="reader">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <param name="name">ノード名</param>
		/// <param name="src">項目の型と同一型のオブジェクト (項目の型を決定する為だけに使用します。)</param>
		/// <returns>
		///		逆シリアル化されたオブジェクトを返します。
		/// </returns>
		public static T GetValue<T>(XmlReader reader, string name, T src)
		{
			return (T)GetValue(reader, name, typeof(T));
		}

		/// <summary>
		/// オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <param name="name">ノード名</param>
		/// <param name="type">項目のタイプオブジェクト</param>
		/// <returns>
		///		逆シリアル化されたオブジェクトを返します。
		/// </returns>
		public static object GetValue(XmlReader reader, string name, Type type)
		{
			if (type.IsEnum)
			{
				reader.ReadStartElement(name);
				string value = (string)reader.ReadContentAs(typeof(string), null);
				object result = Enum.Parse(type, value);
				reader.ReadEndElement();
				return result;
			}
			else if (type.IsPrimitive)
			{
				reader.ReadStartElement(name);
				object result = reader.ReadContentAs(type, null);
				reader.ReadEndElement();
				return result;
			}
			else if (type == typeof(string))
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(name);
					return "";
				}
				else
				{
					reader.ReadStartElement(name);
					object result = reader.ReadContentAs(type, null);
					reader.ReadEndElement();
					return result;
				}
			}
			else if (type == typeof(DateTime))
			{
				reader.ReadStartElement(name);
				object result = reader.ReadContentAs(type, null);
				reader.ReadEndElement();
				return result;
			}
			else if (type == typeof(Color))
			{
				reader.ReadStartElement(name);
				int A = GetValue<int>(reader, "A");
				int R = GetValue<int>(reader, "R");
				int G = GetValue<int>(reader, "G");
				int B = GetValue<int>(reader, "B");
				object result = Color.FromArgb(A, R, G, B);
				reader.ReadEndElement();
				return result;
			}
			else
			{
				reader.ReadStartElement(name);
				XmlSerializer xs = new XmlSerializer(type);
				object result = xs.Deserialize(reader);
				reader.ReadEndElement();
				return result;
			}
		}

		#endregion

		#region AddValue

		/// <summary>
		/// オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">XML ドキュメントを書き込むために使用する XmlWriter。</param>
		/// <param name="src">シリアル化するオブジェクト。</param>
		/// <param name="xs">シリアライザ (null を指定すると src の Type を使用します。)</param>
		/// <param name="ns">名前空間 (null を指定すると名前空間を空白にします)</param>
		public static void AddValue(XmlWriter writer, object src, XmlSerializer xs, XmlSerializerNamespaces ns)
		{
			#region 引数省略時の既定の処理.
			if (ns == null)
			{
				ns = new XmlSerializerNamespaces();
				ns.Add("", "");	// 各項目にプレフィックスと名前空間を入れない.
			}
			if (xs == null)
			{
				xs = new XmlSerializer(src.GetType());
			}
			#endregion

			xs.Serialize(writer, src, ns);
		}

		/// <summary>
		/// オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">XML ドキュメントを書き込むために使用する XmlWriter。</param>
		/// <param name="name">ノード名</param>
		/// <param name="src">対象のオブジェクト</param>
		public static void AddValue(XmlWriter writer, string name, object src)
		{
			if (src == null) return;

			Type type = src.GetType();

			if (type.IsEnum)
			{
				writer.WriteStartElement(name);
				writer.WriteValue(src.ToString());
				writer.WriteEndElement();
			}
			else if (type.IsPrimitive)
			{
				writer.WriteStartElement(name);
				writer.WriteValue(src);
				writer.WriteEndElement();
			}
			else if (type == typeof(string))
			{
				writer.WriteStartElement(name);
				writer.WriteValue(src);
				writer.WriteEndElement();
			}
			else if (type == typeof(DateTime))
			{
				writer.WriteStartElement(name);
				writer.WriteValue(src);
				writer.WriteEndElement();
			}
			else if (type == typeof(Color))
			{
				Color value = (Color)src;
				writer.WriteStartElement(name);
				AddValue(writer, "A", value.A);
				AddValue(writer, "R", value.R);
				AddValue(writer, "G", value.G);
				AddValue(writer, "B", value.B);
				writer.WriteEndElement();
			}
			else
			{
				writer.WriteStartElement(name);
				AddValue(writer, src, null, null);
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}
