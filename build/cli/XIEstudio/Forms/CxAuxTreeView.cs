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
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace XIEstudio
{
	/// <summary>
	/// AuxInfo 用ツリービュー
	/// </summary>
	class CxAuxTreeView : TreeView
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxTreeView()
			: base()
		{
		}

		#endregion

		#region イベント:

		/// <summary>
		/// Occurs after WM_PAINT
		/// </summary>
		public event PaintEventHandler PaintEx;

		#endregion

		#region WndProc:

		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			switch ((WindowMessages)m.Msg)
			{
				case WindowMessages.PAINT:
					base.WndProc(ref m);
					if (PaintEx != null)
						PaintEx.Invoke(this, new PaintEventArgs(this.CreateGraphics(), this.Bounds));
					return;
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Window messages / WM
		/// <summary>
		/// Window messages / WM
		/// </summary>
		enum WindowMessages
		{
			APP = 32768,
			ACTIVATE = 6,
			ACTIVATEAPP = 28,
			AFXFIRST = 864,
			AFXLAST = 895,
			ASKCBFORMATNAME = 780,
			CANCELJOURNAL = 75,
			CANCELMODE = 31,
			CAPTURECHANGED = 533,
			CHANGECBCHAIN = 781,
			CHAR = 258,
			CHARTOITEM = 47,
			CHILDACTIVATE = 34,
			CLEAR = 771,
			CLOSE = 16,
			COMMAND = 273,
			COMMNOTIFY = 68,
			COMPACTING = 65,
			COMPAREITEM = 57,
			CONTEXTMENU = 123,
			COPY = 769,
			COPYDATA = 74,
			CREATE = 1,
			CTLCOLOR = 0x0019,
			CTLCOLORBTN = 309,
			CTLCOLORDLG = 310,
			CTLCOLOREDIT = 307,
			CTLCOLORLISTBOX = 308,
			CTLCOLORMSGBOX = 306,
			CTLCOLORSCROLLBAR = 311,
			CTLCOLORSTATIC = 312,
			CUT = 768,
			DEADCHAR = 259,
			DELETEITEM = 45,
			DESTROY = 2,
			DESTROYCLIPBOARD = 775,
			DEVICECHANGE = 537,
			DEVMODECHANGE = 27,
			DISPLAYCHANGE = 126,
			DRAWCLIPBOARD = 776,
			DRAWITEM = 43,
			DROPFILES = 563,
			ENABLE = 10,
			ENDSESSION = 22,
			ENTERIDLE = 289,
			ENTERMENULOOP = 529,
			ENTERSIZEMOVE = 561,
			ERASEBKGND = 20,
			EXITMENULOOP = 530,
			EXITSIZEMOVE = 562,
			FONTCHANGE = 29,
			GETDLGCODE = 135,
			GETFONT = 49,
			GETHOTKEY = 51,
			GETICON = 127,
			GETMINMAXINFO = 36,
			GETTEXT = 13,
			GETTEXTLENGTH = 14,
			HANDHELDFIRST = 856,
			HANDHELDLAST = 863,
			HELP = 83,
			HOTKEY = 786,
			HSCROLL = 276,
			HSCROLLCLIPBOARD = 782,
			ICONERASEBKGND = 39,
			INITDIALOG = 272,
			INITMENU = 278,
			INITMENUPOPUP = 279,
			UNINITMENUPOPUP = 293,
			INPUTLANGCHANGE = 81,
			INPUTLANGCHANGEREQUEST = 80,
			KEYDOWN = 256,
			KEYUP = 257,
			KILLFOCUS = 8,
			MDIACTIVATE = 546,
			MDICASCADE = 551,
			MDICREATE = 544,
			MDIDESTROY = 545,
			MDIGETACTIVE = 553,
			MDIICONARRANGE = 552,
			MDIMAXIMIZE = 549,
			MDINEXT = 548,
			MDIREFRESHMENU = 564,
			MDIRESTORE = 547,
			MDISETMENU = 560,
			MDITILE = 550,
			MEASUREITEM = 44,
			MENUCHAR = 288,
			MENUSELECT = 287,
			MENUCOMMAND = 294,
			NEXTMENU = 531,
			MOVE = 3,
			MOVING = 534,
			NCACTIVATE = 134,
			NCCALCSIZE = 131,
			NCCREATE = 129,
			NCDESTROY = 130,
			NCHITTEST = 132,
			NCLBUTTONDBLCLK = 163,
			NCLBUTTONDOWN = 161,
			NCLBUTTONUP = 162,
			NCMBUTTONDBLCLK = 169,
			NCMBUTTONDOWN = 167,
			NCMBUTTONUP = 168,
			NCMOUSEMOVE = 160,
			NCPAINT = 133,
			NCRBUTTONDBLCLK = 166,
			NCRBUTTONDOWN = 164,
			NCRBUTTONUP = 165,
			NEXTDLGCTL = 40,
			NOTIFY = 78,
			NOTIFYFORMAT = 85,
			NULL = 0,
			PAINT = 15,
			PAINTCLIPBOARD = 777,
			PAINTICON = 38,
			PALETTECHANGED = 785,
			PALETTEISCHANGING = 784,
			PARENTNOTIFY = 528,
			PASTE = 770,
			PENWINFIRST = 896,
			PENWINLAST = 911,
			POWER = 72,
			POWERBROADCAST = 536,
			PRINT = 791,
			PRINTCLIENT = 792,
			QUERYDRAGICON = 55,
			QUERYENDSESSION = 17,
			QUERYNEWPALETTE = 783,
			QUERYOPEN = 19,
			QUEUESYNC = 35,
			QUIT = 18,
			RENDERALLFORMATS = 774,
			RENDERFORMAT = 773,
			SETCURSOR = 32,
			SETFOCUS = 7,
			SETFONT = 48,
			SETHOTKEY = 50,
			SETICON = 128,
			SETREDRAW = 11,
			SETTEXT = 12,
			SETTINGCHANGE = 26,
			SHOWWINDOW = 24,
			SIZE = 5,
			SIZECLIPBOARD = 779,
			SIZING = 532,
			SPOOLERSTATUS = 42,
			STYLECHANGED = 125,
			STYLECHANGING = 124,
			SYSCHAR = 262,
			SYSCOLORCHANGE = 21,
			SYSCOMMAND = 274,
			SYSDEADCHAR = 263,
			SYSKEYDOWN = 260,
			SYSKEYUP = 261,
			TCARD = 82,
			TIMECHANGE = 30,
			TIMER = 275,
			UNDO = 772,
			USER = 1024,
			USERCHANGED = 84,
			VKEYTOITEM = 46,
			VSCROLL = 277,
			VSCROLLCLIPBOARD = 778,
			WINDOWPOSCHANGED = 71,
			WINDOWPOSCHANGING = 70,
			WININICHANGE = 26,
			KEYFIRST = 256,
			KEYLAST = 264,
			SYNCPAINT = 136,
			MOUSEACTIVATE = 33,
			MOUSEMOVE = 512,
			LBUTTONDOWN = 513,
			LBUTTONUP = 514,
			LBUTTONDBLCLK = 515,
			RBUTTONDOWN = 516,
			RBUTTONUP = 517,
			RBUTTONDBLCLK = 518,
			MBUTTONDOWN = 519,
			MBUTTONUP = 520,
			MBUTTONDBLCLK = 521,
			MOUSEWHEEL = 522,
			MOUSEFIRST = 512,
			MOUSELAST = 522,
			MOUSEHOVER = 0x2A1,
			MOUSELEAVE = 0x2A3,
			SHNOTIFY = 0x0401,
			UNICHAR = 0x0109,
			THEMECHANGED = 0x031A,
		}
		#endregion

		#region GetAllNodes

		/// <summary>
		/// 全てのノードを取得します。
		/// </summary>
		/// <returns>
		///		全てのノードを返します。
		/// </returns>
		public List<TreeNode> GetAllNodes()
		{
			var result = new List<TreeNode>();
			foreach (TreeNode node in this.Nodes)
			{
				result.Add(node);
				result.AddRange(this.GetChildNodes(node));
			}
			return result;
		}

		/// <summary>
		/// 指定されたノードの配下のノードを取得します。
		/// </summary>
		/// <param name="node">検索対象</param>
		/// <returns>
		///		指定されたノードの配下のノードを返します。
		/// </returns>
		public List<TreeNode> GetChildNodes(TreeNode node)
		{
			var result = new List<TreeNode>();
			foreach (TreeNode child in node.Nodes)
			{
				result.Add(child);
				result.AddRange(this.GetChildNodes(child));
			}
			return result;
		}

		#endregion
	}
}
