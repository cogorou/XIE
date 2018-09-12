// $Revision: $

// file: CSharpHighlighter.cs
// brief: C# highlighter.
// author: YAMAMOTO Suguru
// update: 2009-09-05
//=========================================================
using System;
using Color = System.Drawing.Color;

namespace Sgry.Azuki.Highlighter
{
	/// <summary>
	/// Highlighter for C# language based on keyword matching.
	/// </summary>
	class VisualBasicHighlighter : KeywordHighlighter
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public VisualBasicHighlighter()
		{
			AddKeywordSet( new string[] {
				"ByRef",
				"Case",
				"Case Else",
				"Ctype",
				"End Select",
				"Finalize",
				"Friend",
				"Get",
				"Implements",
				"Imports",
				"Inherits",
				"Me",
				"MustInherit",
				"MustOverride",
				"MyBase",
				"New",
				"NotInheritable",
				"NotOverridable",
				"Overloads",
				"Overridable",
				"Overrides",
				"Private",
				"Property",
				"Protected",
				"Protected Friend",
				"Public",
				"Select Case",
				"Set",
				"Shadows",
				"Shared",
				"Structure",
				"SyncLock",
				"Value",
			}, CharClass.Keyword);

			AddKeywordSet( new string[] {
				"add", "from", "get", "global", "group", "into",
				"join", "let", "orderby", "partial", "remove",
				"select", "set", "value", "var", "where", "yield"
			}, CharClass.Keyword2 );

			AddKeywordSet( new string[] {
				"#define", "#elif", "#else", "#endif",
				"#endregion", "#error", "#if", "#line",
				"#region", "#undef", "#warning"
			}, CharClass.Macro );

			AddEnclosure( "'", "'", CharClass.String, false, '\\' );
			AddEnclosure( "@\"", "\"", CharClass.String, true, '\"' );
			AddEnclosure( "\"", "\"", CharClass.String, false, '\\' );
			AddEnclosure( "/**", "*/", CharClass.DocComment, true );
			AddEnclosure( "/*", "*/", CharClass.Comment, true );
			AddLineHighlight( "'''", CharClass.DocComment );
			AddLineHighlight( "'", CharClass.Comment );
		}
	}
}
