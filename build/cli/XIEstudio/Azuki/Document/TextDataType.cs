﻿// file: TextDataType.cs
// brief: Enumeration type representing text data type.
// author: YAMAMOTO Suguru
// update: 2010-08-09
//=========================================================
using System;

namespace Sgry.Azuki
{
	/// <summary>
	/// Specifies type of text data.
	/// </summary>
	enum TextDataType
	{
		/// <summary>
		/// Normal text data; a stream of characters.
		/// </summary>
		Normal,

		/// <summary>
		/// Word text data; a stream of characters which starts and ends at word boundaries.
		/// </summary>
		Words,

		/// <summary>
		/// Line text data; not a stream but a line.
		/// </summary>
		Line,

		/// <summary>
		/// Rectangle text data; graphically layouted text.
		/// </summary>
		Rectangle
	}
}

