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
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Reflection;

namespace XIE
{
	/// <summary>
	/// 文字列格納庫
	/// </summary>
	public static class AxiTextStorage
	{
		/// <summary>
		/// 文字列コレクション
		/// </summary>
		[CxDescription("P:XIE.AxiTextStorage.Level")]
		public static Dictionary<string, string> Texts
		{
			get { return m_Texts; }
		}
		private static Dictionary<string, string> m_Texts = new Dictionary<string, string>();

		/// <summary>
		/// 文字列の取得
		/// </summary>
		/// <param name="key">リソースキー</param>
		/// <returns>
		///		指定されたキーに該当する文字列を返します。
		///		見つからなければ Empty を返します。
		/// </returns>
		public static string GetValue(string key)
		{
			string value;
			if (Texts.TryGetValue(key, out value))
				return value;
			return string.Empty;
		}

		/// <summary>
		/// 文字列リソースファイルの読み込み
		/// </summary>
		/// <param name="filename">ファイル名</param>
		public static void Load(string filename)
		{
			string filepath = "";

			#region 探索:
			if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(filename)) == false)
			{
				var dir = System.IO.Path.GetDirectoryName(filename);
				var file = System.IO.Path.GetFileName(filename);
				filepath = GetFullPath(dir, file);
				if (System.IO.File.Exists(filepath) == false)
					return;	// ignore
			}
			else
			{
				var currentDir = System.IO.Directory.GetCurrentDirectory();
				var exeDir = Application.StartupPath;
				var binDir = XIE.AxiEnv.ProductBIN;

				if (Exists(currentDir, filename))
				{
					filepath = GetFullPath(currentDir, filename);
				}
				else if (Exists(exeDir, filename))
				{
					filepath = GetFullPath(exeDir, filename);
				}
				else if (string.IsNullOrEmpty(binDir) == false && Exists(binDir, filename))
				{
					filepath = GetFullPath(binDir, filename);
				}
				else
				{
					return;
				}
			}
			#endregion

			using (XmlTextReader reader = new XmlTextReader(filepath))
			{
				while (reader.Read())
				{
					if (reader.IsEmptyElement) continue;

					if (reader.NodeType == XmlNodeType.Element)
					{
						#region インテリセンス用 XML ファイルから読み込む場合.
						//
						// member の name 属性をキーにして、
						// summary の内容を 文字列格納庫 に追加する.
						//
						// インテリセンス用 XML ファイルの構造は下記のようになっている.
						// <?xml version="1.0" encoding="utf-8"?>
						// <doc>
						//   <assembly>
						//     <name>(アセンブリ名称)</name>
						//   </assembly>
						//   <members>
						//     <member name="(キー)">
						//       <summary>
						//         (文字列)
						//       </summary>
						//     </member>
						//   </members>
						// </doc>
						if (reader.Name == "member")
						{
							string key = reader.GetAttribute("name");
							if (key != null)
							{
								StringBuilder text = new StringBuilder();
								while (reader.Read())
								{
									if (reader.NodeType == XmlNodeType.Text)
										text.Append(reader.Value);
									else if (reader.NodeType == XmlNodeType.Element)
										continue;
									else if (reader.NodeType == XmlNodeType.EndElement)
									{
										if (reader.Name == "summary") break;
										continue;
									}
								}

								// 余計な文字を破棄する.
								text = text.Replace("\r\n", " ");	// 改行コード.
								text = text.Replace("\t", " ");		// タブコード.

								string value = text.ToString().Trim();
								text.Length = 0;
								text.Capacity = 0;

								// コレクションに追加する.
								Texts[key] = value;

								//System.Diagnostics.Debug.WriteLine(value);
							}
						}
						#endregion
					}
				}
			}
		}

		/// <summary>
		/// 文字列リソースファイルへのパスを取得します。
		/// </summary>
		/// <param name="dir">ディレクトリ</param>
		/// <param name="filename">ファイル名</param>
		/// <returns>
		///		文字列リソースファイルへのパスを返します。
		/// </returns>
		private static string GetFullPath(string dir, string filename)
		{
			#region 多言語化対応:
			{
				var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
				var culture_dir = System.IO.Path.Combine(dir, culture.Name);
				var culture_path = System.IO.Path.Combine(culture_dir, filename);

				if (System.IO.File.Exists(culture_path))
				{
					return culture_path;
				}
			}
			#endregion

			var filepath = System.IO.Path.Combine(dir, filename);
			return filepath;
		}

		/// <summary>
		/// 文字列リソースファイルの有無を確認します。
		/// </summary>
		/// <param name="dir">ディレクトリ</param>
		/// <param name="filename">ファイル名</param>
		/// <returns>
		///		存在すれば true 、それ以外は false を返します。
		/// </returns>
		private static bool Exists(string dir, string filename)
		{
			var filepath = GetFullPath(dir, filename);
			return System.IO.File.Exists(filepath);
		}
	}
}
