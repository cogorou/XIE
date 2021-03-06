// $Revision: $

using System;
using Sgry.Azuki;
using Path = System.IO.Path;
using IHighlighter = Sgry.Azuki.Highlighter.IHighlighter;
using Highlighters = Sgry.Azuki.Highlighter.Highlighters;

namespace Sgry.Azuki
{
	/// <summary>
	/// ファイルタイプ
	/// </summary>
	class FileType
	{
		#region Fields & Constants
		/// <summary>
		/// 
		/// </summary>
		public const string CppFileTypeName = "C/C++";
		/// <summary>
		/// 
		/// </summary>
		public const string CSharpFileTypeName = "C#";
		/// <summary>
		/// 
		/// </summary>
		public const string VbFileTypeName = "VB";
		/// <summary>
		/// 
		/// </summary>
		public const string JavaFileTypeName = "Java";
		/// <summary>
		/// 
		/// </summary>
		public const string RubyFileTypeName = "Ruby";
		/// <summary>
		/// 
		/// </summary>
		public const string LatexFileTypeName = "LaTeX";
		/// <summary>
		/// 
		/// </summary>
		public const string XmlFileTypeName = "XML";
		/// <summary>
		/// 
		/// </summary>
		public const string TextFileTypeName = "Text";
		string _Name = null;
		IHighlighter _Highlighter = null;
		AutoIndentHook _AutoIndentHook = null;
		#endregion

		private FileType()
		{}

		#region Factory
		/// <summary>
		/// Gets a new Text file type.
		/// </summary>
		public static FileType TextFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = TextFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new LaTeX file type.
		/// </summary>
		public static FileType LatexFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Latex;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = LatexFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new C/C++ file type.
		/// </summary>
		public static FileType CppFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Cpp;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = CppFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new C# file type.
		/// </summary>
		public static FileType CSharpFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.CSharp;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = CSharpFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Visual Basic file type.
		/// </summary>
		public static FileType VbFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.VisualBasic;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = VbFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Java file type.
		/// </summary>
		public static FileType JavaFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Java;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = JavaFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Ruby file type.
		/// </summary>
		public static FileType RubyFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Ruby;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = RubyFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new XML file type.
		/// </summary>
		public static FileType XmlFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Xml;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = XmlFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static FileType GetFileTypeByFileName( string fileName )
		{
			string ext;

			ext = Path.GetExtension( fileName );

			// C/C++?
			if (0 <= ".c .cpp .cxx .h .hpp .hxx".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase))
			{
				return CppFileType;
			}

			// C#?
			if( 0 <= ".cs".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase) )
			{
				return CSharpFileType;
			}

			// Visual Basic?
			if (0 <= ".vb".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase))
			{
				return VbFileType;
			}

			// Java?
			if( 0 <= ".java".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase) )
			{
				return JavaFileType;
			}

			// Ruby?
			if( 0 <= ".rb".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase) )
			{
				return RubyFileType;
			}

			// LaTeX?
			if (0 <= ".tex".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase))
			{
				return LatexFileType;
			}

			// XML?
			if (0 <= ".xml .xsl .htm .html".IndexOf(ext, StringComparison.CurrentCultureIgnoreCase))
			{
				return XmlFileType;
			}

			return TextFileType;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets an associated highlighter object.
		/// </summary>
		public IHighlighter Highlighter
		{
			get{ return _Highlighter; }
		}

		/// <summary>
		/// Gets key-hook procedure for Azuki's auto-indent associated with this file-type.
		/// </summary>
		public AutoIndentHook AutoIndentHook
		{
			get{ return _AutoIndentHook; }
		}

		/// <summary>
		/// Gets the name of the file mode.
		/// </summary>
		public String Name
		{
			get{ return _Name; }
		}
		#endregion
	}
}
