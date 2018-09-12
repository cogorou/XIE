/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace XIE.Forms
{
	/// <summary>
	/// ツールストリップ拡張版
	/// </summary>
	/// <remarks>
	/// see: http://blogs.msdn.com/b/rickbrew/archive/2006/01/09/511003.aspx
	/// </remarks>
	public class CxToolStripEx : ToolStrip
	{
		/// <summary>
		/// Windows メッセージを処理します。 (ScrollableControl.WndProc(Message) をオーバーライドします。)
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == WM_MOUSEACTIVATE && m.Result == (IntPtr)MA_ACTIVATEANDEAT)
			{
				m.Result = (IntPtr)MA_ACTIVATE;
			}
		}

		internal const uint WM_MOUSEACTIVATE = 0x21;
		internal const uint MA_ACTIVATE = 1;
		internal const uint MA_ACTIVATEANDEAT = 2;
		internal const uint MA_NOACTIVATE = 3;
		internal const uint MA_NOACTIVATEANDEAT = 4;
	}
}
