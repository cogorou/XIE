using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Sgml;

namespace customhhc
{
	class Program
	{
		static void Main(string[] args)
		{
			Encoding encoding = Encoding.GetEncoding("shift_jis");
			//const string local_attr = "<param name=\"Local\" value=\"";

			#region 引数チェック.
			string filename = "";
			if (args.Length < 1)
			{
				Console.WriteLine("ERR: invalid args.");
				Console.WriteLine("Usage:");
				Console.WriteLine("customhhc (filename)");
				return;
			}
			filename = Path.GetFullPath(args[0]);
			Console.WriteLine(filename);
			if (File.Exists(filename) == false)
			{
				Console.WriteLine("ERR: not found.");
				return;
			}
			#endregion

			#region hhc
			if (filename.EndsWith(".hhc", StringComparison.OrdinalIgnoreCase))
			{
				List<CxNode> nodes1 = AxiParser.ParseHHC(filename, encoding);
				//foreach (var node in nodes1)
				//{
				//    if (node.Statement.Contains(local_attr))
				//        node.Statement = node.Statement.Replace(local_attr, local_attr + "cpp\\html\\");
				//}
				List<CxNode> nodes1_MainPage = AxiParser.ExtractMainPage(nodes1);
				List<CxNode> nodes1_Modules = AxiParser.ExtractModules(nodes1);
				List<CxNode> nodes = new List<CxNode>();
				// -----
				nodes.Add(new CxNode(0, "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML/EN\">"));
				nodes.Add(new CxNode(0, "<HTML>"));
				nodes.Add(new CxNode(0, "<BODY>"));
				// -----
				nodes.Add(new CxNode(0, "<UL>"));
				nodes.Add(new CxNode(0, "<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"Main Page\"><param name=\"Local\" value=\"index.html\"><param name=\"ImageNumber\" value=\"1\"></param></param></param></OBJECT></LI>"));
				nodes.AddRange(nodes1_MainPage);
				nodes.Add(new CxNode(0, "</UL>"));
				// -----
				nodes.Add(new CxNode(0, "<UL>"));
				nodes.Add(new CxNode(0, "<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"Modules\"><param name=\"Local\" value=\"modules.html\"><param name=\"ImageNumber\" value=\"1\"></param></param></param></OBJECT></LI>"));
				nodes.AddRange(nodes1_Modules);
				nodes.Add(new CxNode(0, "</UL>"));
				// -----
				nodes.Add(new CxNode(0, "<UL>"));
				nodes.Add(new CxNode(0, "<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"Namespaces\"><param name=\"Local\" value=\"namespaces.html\"><param name=\"ImageNumber\" value=\"1\"></param></param></param></OBJECT></LI>"));
				nodes.Add(new CxNode(0, "</UL>"));
				// -----
				nodes.Add(new CxNode(0, "<UL>"));
				nodes.Add(new CxNode(0, "<LI><OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"Classes\"><param name=\"Local\" value=\"annotated.html\"><param name=\"ImageNumber\" value=\"1\"></param></param></param></OBJECT></LI>"));
				nodes.Add(new CxNode(0, "</UL>"));
				// -----
				nodes.Add(new CxNode(0, "</BODY>"));
				nodes.Add(new CxNode(0, "</HTML>"));

				Save(filename, nodes, encoding);
			}
			#endregion
		}

		#region 出力:

		/// <summary>
		/// HTML ノードのコレクションを標準出力へ表示します。
		/// </summary>
		/// <param name="nodes">ノードコレクション</param>
		public static void Dump(List<CxNode> nodes)
		{
			foreach (var node in nodes)
			{
				string indent = "";
				for (int i = 0; i < node.Level; i++)
					indent += "	";
				Console.WriteLine(indent + node.Statement);
			}
		}

		#endregion

		#region 保存:

		/// <summary>
		/// HTML ノードのコレクションをファイルへ保存します。
		/// </summary>
		/// <param name="filename">保存先のファイル名</param>
		/// <param name="nodes">ノードコレクション</param>
		/// <param name="encoding">エンコード</param>
		public static void Save(string filename, List<CxNode> nodes, Encoding encoding)
		{
			using (var stream = new StreamWriter(filename, false, encoding))
			{
				foreach (var node in nodes)
				{
					string indent = "";
					for (int i = 0; i < node.Level - 1; i++)
						indent += "	";
					stream.WriteLine(indent + node.Statement);
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// HHC/HHK//HHP 解析
	/// </summary>
	class AxiParser
	{
		#region HHC からノードを収集します。

		/// <summary>
		/// HHC からノードを収集します。
		/// </summary>
		/// <param name="filename">対象の HHC</param>
		/// <param name="encoding">エンコード</param>
		/// <returns>
		///		収集したノードのコレクションを返します。
		/// </returns>
		public static List<CxNode> ParseHHC(string filename, Encoding encoding)
		{
			List<CxNode> nodes = new List<CxNode>();
			using (var stream = new StreamReader(filename, encoding))
			using (var sgml = new SgmlReader { InputStream = stream })
			{
				int level = 0;
				while (sgml.Read())
				{
					if (sgml.NodeType == System.Xml.XmlNodeType.Element)
					{
						if (sgml.Name == "UL")
						{
							level++;
							CxNode node = new CxNode();
							node.Level = level;
							node.Statement = "<UL>";
							nodes.Add(node);
						}
						else if (sgml.Name == "OBJECT")
						{
							if (level <= 0) continue;
							string xml1 = sgml.ReadOuterXml();
							XDocument doc = XDocument.Parse(xml1);
							string xml2 = doc.ToString(SaveOptions.DisableFormatting);
							CxNode node = new CxNode();
							node.Level = level;
							node.Statement = "<LI>" + xml2 + "</LI>";
							nodes.Add(node);
						}
					}
					if (sgml.NodeType == System.Xml.XmlNodeType.EndElement)
					{
						if (sgml.Name == "UL")
						{
							CxNode node = new CxNode();
							node.Level = level;
							node.Statement = "</UL>";
							nodes.Add(node);
							level--;
						}
					}
				}
			}
			return nodes;
		}

		#endregion

		#region HHP からノードを収集します。

		/// <summary>
		/// HHP からノードを収集します。
		/// </summary>
		/// <param name="filename">対象の HHP</param>
		/// <param name="encoding">エンコード</param>
		/// <returns>
		///		収集したノードのコレクションを返します。
		/// </returns>
		public static List<CxNode> ParseHHP(string filename, Encoding encoding)
		{
			List<CxNode> nodes = new List<CxNode>();
			using (var stream = new StreamReader(filename, encoding))
			{
				bool found = false;
				while (true)
				{
					string line = stream.ReadLine();
					if (line == null) break;
					if (line.Trim() == "") continue;
					if (line.StartsWith("[FILES]"))
					{
						found = true;
						continue;
					}
					else if (line.StartsWith("["))
					{
						found = false;
						continue;
					}
					if (found)
					{
						nodes.Add(new CxNode(0, line));
					}
				}
			}
			return nodes;
		}

		#endregion

		#region doxygen: MainPage 配下を収集します。

		/// <summary>
		/// doxygen: MainPage 配下を収集します。
		/// </summary>
		/// <param name="src">元のノードコレクション</param>
		/// <returns>
		///		収集した MainPage 配下を返します。
		/// </returns>
		public static List<CxNode> ExtractMainPage(List<CxNode> nodes)
		{
			List<CxNode> result = new List<CxNode>();
			int level = 0;
			bool found = false;
			foreach (var node in nodes)
			{
				if (node.Statement.Contains("index.html"))
				{
					level = node.Level;
					found = true;
					continue;
				}
				if (found)
				{
					if (node.Level <= level) break;
					result.Add(node);
				}
			}
			return result;
		}

		#endregion

		#region doxygen: Modules 配下を収集します。

		/// <summary>
		/// doxygen: Modules 配下を収集します。
		/// </summary>
		/// <param name="src">元のノードコレクション</param>
		/// <returns>
		///		収集した Modules 配下を返します。
		/// </returns>
		public static List<CxNode> ExtractModules(List<CxNode> nodes)
		{
			List<CxNode> result = new List<CxNode>();
			int level = 0;
			bool found = false;
			foreach (var node in nodes)
			{
				if (node.Statement.Contains("modules.html"))
				{
					level = node.Level;
					found = true;
					continue;
				}
				if (found)
				{
					if (node.Level <= level) break;
					result.Add(node);
				}
			}
			return result;
		}

		#endregion
	}

	/// <summary>
	/// HTML ノード
	/// </summary>
	class CxNode
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxNode()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="level">階層</param>
		/// <param name="statement">構文</param>
		public CxNode(int level, string statement)
		{
			Level = level;
			Statement = statement;
		}

		/// <summary>
		/// 階層
		/// </summary>
		public int Level = 0;

		/// <summary>
		/// 構文
		/// </summary>
		public string Statement = "";
	}
}
