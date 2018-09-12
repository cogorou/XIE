/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XIE.Forms
{
	/// <summary>
	/// クリップボード監視クラス
	/// </summary>
	/// <remarks>
	/// 参考:<br/>
	/// http://www.atmarkit.co.jp/fdotnet/dotnettips/848cbviewer/cbviewer.html <br/>
	/// http://wisdom.sakura.ne.jp/system/winapi/win32/win92.html <br/>
	/// </remarks>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	public class CxClipboardObserver : NativeWindow
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parent"></param>
		public CxClipboardObserver(Form parent)
		{
			parent.HandleCreated += this.OnHandleCreated;
			parent.HandleDestroyed += this.OnHandleDestroyed;
			this.Parent = parent;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 親ウィンドウ
		/// </summary>
		private Form Parent = null;

		/// <summary>
		/// 監視有効属性
		/// </summary>
		public bool Enabled = false;

		#endregion

		#region イベント:

		/// <summary>
		/// 通知イベント
		/// </summary>
		public event EventHandler Notify;

		#endregion

		#region Win32:

		/// <summary>
		/// クリップボードビューアチェインに、指定されたウィンドウを追加します
		/// </summary>
		/// <param name="hWndNewViewer">クリップボードチェインに追加するウィンドウを指定します</param>
		/// <returns>
		///		次に位置するウィンドウのハンドル。失敗、または次がない場合は NULL
		/// </returns>
		[DllImport("user32")]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		/// <summary>
		/// クリップボードビューアのチェインから、指定されたウィンドウを削除します
		/// </summary>
		/// <param name="hWndRemove">削除するウィンドウのハンドルを指定します</param>
		/// <param name="hWndNewNext">hWndRemove の次のウィンドウのハンドルを指定します</param>
		/// <returns>
		///		通常は FALSE、ウィンドウが1つしかなかったときは TRUE
		/// </returns>
		[DllImport("user32")]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="Msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns>
		/// </returns>
		[DllImport("user32")]
		public extern static int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		private const int WM_DRAWCLIPBOARD = 0x0308;
		private const int WM_CHANGECBCHAIN = 0x030D;
		private IntPtr nextHandle;

		#endregion

		#region 内部関数:

		/// <summary>
		/// ビューアを登録する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnHandleCreated(object sender, EventArgs e)
		{
			AssignHandle(((Form)sender).Handle);
			nextHandle = SetClipboardViewer(this.Handle);
		}

		/// <summary>
		/// ビューアを解除する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnHandleDestroyed(object sender, EventArgs e)
		{
			bool status = ChangeClipboardChain(this.Handle, nextHandle);
			ReleaseHandle();
		}

		/// <summary>
		/// クリップボードを監視する
		/// </summary>
		/// <param name="msg"></param>
		protected override void WndProc(ref Message msg)
		{
			switch (msg.Msg)
			{
				// クリップボードが更新された.
				case WM_DRAWCLIPBOARD:
					// 自身の処理.
					if (this.Notify != null && this.Enabled)
					{
						this.Notify(this, new CxClipboardObserverEventArgs(msg));
					}
					// 次のビューワの処理.
					if (nextHandle != IntPtr.Zero)
					{
						SendMessage(nextHandle, msg.Msg, msg.WParam, msg.LParam);
					}
					break;

				// クリップボード・ビューア・チェーンが更新された.
				case WM_CHANGECBCHAIN:
					if (msg.WParam == nextHandle)
					{
						nextHandle = msg.LParam;
					}
					else if (nextHandle != IntPtr.Zero)
					{
						SendMessage(nextHandle, msg.Msg, msg.WParam, msg.LParam);
					}
					break;
			}
			base.WndProc(ref msg);
		}

		#endregion
	}

	#region イベント引数:

	/// <summary>
	/// クリップボード監視用イベント引数
	/// </summary>
	public class CxClipboardObserverEventArgs : EventArgs
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="msg"></param>
		public CxClipboardObserverEventArgs(Message msg)
		{
			Msg = msg;
		}

		/// <summary>
		/// メッセージ
		/// </summary>
		public Message Msg;
	}
	#endregion
}
