/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using XIE.Ptr;

namespace XIE.Log
{
	/// <summary>
	/// 補助関数群 (ログ)
	/// </summary>
	public static partial class Api : System.Object
	{
		#region ログ:

		/// <summary>
		/// ログコレクション
		/// </summary>
		public static XIE.Log.CxLogCollection Logs
		{
			get { return m_Logs; }
		}
		private static XIE.Log.CxLogCollection m_Logs = new XIE.Log.CxLogCollection(20000);

		#endregion

		#region メソッド: (トレース)

		/// <summary>
		/// トレース
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Trace_01.cs"/>
		/// </example>
		public static void Trace(string format, object arg0)
		{
			Trace(string.Format(format, arg0));
		}

		/// <summary>
		/// トレース
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Trace_01.cs"/>
		/// </example>
		public static void Trace(string format, params object[] args)
		{
			Trace(string.Format(format, args));
		}

		/// <summary>
		/// トレース
		/// </summary>
		/// <param name="message"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Trace_01.cs"/>
		/// </example>
		public static void Trace(string message)
		{
			Logs.Add(XIE.Log.ExLogLevel.Trace, message);
		}

		#endregion

		#region メソッド: (エラー)

		/// <summary>
		/// エラー
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Error_01.cs"/>
		/// </example>
		public static void Error(string format, object arg0)
		{
			Error(string.Format(format, arg0));
		}

		/// <summary>
		/// エラー
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Error_01.cs"/>
		/// </example>
		public static void Error(string format, params object[] args)
		{
			Error(string.Format(format, args));
		}

		/// <summary>
		/// エラー
		/// </summary>
		/// <param name="message"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Error_01.cs"/>
		/// </example>
		public static void Error(string message)
		{
			Logs.Add(XIE.Log.ExLogLevel.Error, message);
		}

		#endregion
	}
}
