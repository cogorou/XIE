/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE
{
	/// <summary>
	/// 例外クラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Core/Base/CxException_01.cs"/>
	/// </example>
	public class CxException : System.Exception
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code">エラーコード</param>
		public CxException(ExStatus code)
			: base(code.ToString())
		{
			this.Code = code;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code">エラーコード</param>
		/// <param name="message">例外の説明</param>
		public CxException(ExStatus code, string message)
			: base(code.ToString() + ":" + message)
		{
			this.Code = code;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code">エラーコード</param>
		public CxException(int code)
			: base(((ExStatus)code).ToString())
		{
			this.Code = (ExStatus)code;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code">エラーコード</param>
		/// <param name="message">例外の説明</param>
		public CxException(int code, string message)
			: base(((ExStatus)code).ToString() + ":" + message)
		{
			this.Code = (ExStatus)code;
		}

		/// <summary>
		/// エラーコード
		/// </summary>
		public virtual ExStatus Code
		{
			get { return m_Code; }
			set { m_Code = value; }
		}
		private ExStatus m_Code = ExStatus.Success;
	}
}
