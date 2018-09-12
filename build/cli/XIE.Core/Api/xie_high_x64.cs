/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using XIE.GDI;
using XIE.IO;
using XIE.Net;
using XIE.Media;

namespace XIE
{
	using Module_T = IntPtr;

	// for Windows
	using HWND = System.IntPtr;
	using HGLRC = System.IntPtr;
	using HDC = System.IntPtr;
	using HBITMAP = System.IntPtr;
	// for Linux
	using Display = System.IntPtr;
	using Window = System.Int32;

	// Common
	using LPCSTR = System.String;
	using LPCWSTR = System.String;

	/// <summary>
	/// ネイティブ関数群
	/// </summary>
	static partial class xie_high_x64
	{
		#region ライブラリ名.

#if (LINUX)
#if DEBUG
		internal const string LIBRARY_FILENAME = "libxie_highd.so";
#else
		internal const string LIBRARY_FILENAME = "libxie_high.so";
#endif
#else
		internal const string LIBRARY_FILENAME = "xie_high_x64_" + XIE.Defs.XIE_VER_STR + ".dll";
#endif

		#endregion

		#region 初期化と解放.

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void xie_high_setup();

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void xie_high_teardown();

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int xie_high_setup_ex([MarshalAs(UnmanagedType.LPStr)] LPCSTR category);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int xie_high_teardown_ex([MarshalAs(UnmanagedType.LPStr)] LPCSTR category);

		#endregion

		// GDI

		#region Module

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern Module_T fnXIE_GDI_Module_Create([MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		#endregion

		#region Canvas: (画像描画オブジェクト)

#if LINUX
		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Setup(HxModule handle, Window window);
#else
		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Setup(HxModule handle, HWND hWnd);
#endif

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Resize(HxModule handle, int width, int height);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_BeginPaint(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_EndPaint(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Lock(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Unlock(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Clear(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_DrawImage(HxModule handle, HxModule himage);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_DrawOverlay(HxModule handle, HxModule[] figures, int count, ExGdiScalingMode mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_DrawOverlayCB(HxModule handle, IntPtr render, IntPtr param, ExGdiScalingMode mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Flush(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_FlushToImage(HxModule handle, HxModule hdst);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_EffectiveRect(HxModule handle, out TxRectangleI result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_VisibleRect(HxModule handle, out TxRectangleD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_VisibleRectI(HxModule handle, ExBoolean includePartialPixel, out TxRectangleI result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_DPtoIP(HxModule handle, TxPointD dp, ExGdiScalingMode mode, out TxPointD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_IPtoDP(HxModule handle, TxPointD ip, ExGdiScalingMode mode, out TxPointD result);

		#endregion

		#region Canvas: (スタティック関数)

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_GDI_Canvas_Api_EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, out TxRectangleD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_GDI_Canvas_Api_VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, out TxRectangleD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_GDI_Canvas_Api_DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, ExGdiScalingMode mode, out TxPointD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_GDI_Canvas_Api_IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, ExGdiScalingMode mode, out TxPointD result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Canvas_Api_Extract(HxModule hdst, HxModule hsrc, XIE.GDI.TxCanvas canvas);

		#endregion

		#region GDI: General

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_2d_Resize(HxModule handle, int length);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_2d_Reset(HxModule handle);

		#endregion

		#region GDI: Image

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Image_Resize(HxModule handle, int width, int height);

		#endregion

		#region GDI: StringA

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_StringA_Text_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_StringA_FontName_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR value);

		#endregion

		#region GDI: StringW

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_StringW_Text_Set(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCWSTR value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_StringW_FontName_Set(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCWSTR value);

		#endregion

		#region GDI: Graphics

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_GDI_Graphics_CheckValidity(IntPtr hdc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Graphics_DrawImage(IntPtr hdc, HxModule hsrc, TxCanvas canvas);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_GDI_Profile_MakeGraph(HxModule hsrc, TxCanvas canvas, TxPointD position, IntPtr hgraphs_x, IntPtr hgraphs_y, IntPtr values);

		#endregion

		// IO

		#region 操作用.

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern Module_T fnXIE_IO_Module_Create([MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_Module_Setup(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr fnXIE_IO_SerialPort_PortName_Get(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_SerialPort_PortName_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_SerialPort_Readable(HxModule handle, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_SerialPort_Writeable(HxModule handle, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_SerialPort_Read(HxModule handle, byte[] buffer, int length, int timeout, out int result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_IO_SerialPort_Write(HxModule handle, byte[] buffer, int length, int timeout, out int result);

		#endregion

		// Net

		#region 操作用.

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern Module_T fnXIE_Net_Module_Create([MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_Module_Setup(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_SocketStream_Readable(TxSocketStream tag, int timeout, out ExBoolean value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_SocketStream_Writeable(TxSocketStream tag, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_SocketStream_Read(TxSocketStream tag, byte[] buffer, int length, int timeout, out int result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_SocketStream_Write(TxSocketStream tag, byte[] buffer, int length, int timeout, out int result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_TcpClient_Connected(HxModule handle, out ExBoolean value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_UdpClient_Readable(HxModule handle, int timeout, out ExBoolean value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_UdpClient_Writeable(HxModule handle, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_UdpClient_Read(HxModule handle, byte[] buffer, int length, int timeout, ref TxIPEndPoint remoteEP, out int result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Net_UdpClient_Write(HxModule handle, byte[] buffer, int length, int timeout, ref TxIPEndPoint remoteEP, out int result);

		#endregion

		// Media

		#region 共通:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern Module_T fnXIE_Media_Module_Create([MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		#endregion

		#region CxCamera:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_Camera_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_Camera_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent);

#if LINUX
		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, Window window, ExMediaType type, int mode, [MarshalAs(UnmanagedType.LPStr)] LPCSTR caption);
#else
		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, HWND hWnd, ExMediaType type, int mode, [MarshalAs(UnmanagedType.LPStr)] LPCSTR caption);
#endif

		#endregion

		#region CxMediaPlayer:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaPlayer_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaPlayer_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaPlayer_WaitForCompletion(HxModule handle, int timeout, out ExBoolean result);

		#endregion

		#region CxScreenCapture:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenCapture_Setup(HxModule handle, HxModule hWindow, HxModule hAudio, HxModule hOutput);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenCapture_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent);

		#endregion

		#region CxScreenList:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenList_Setup(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenList_Length(HxModule handle, ref int length);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenList_Item_Get(HxModule handle, int index, HxModule hVal);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenList_Item_Set(HxModule handle, int index, HxModule hVal);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ScreenListItem_Name_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		#endregion

		#region CxDeviceList:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceList_Setup(HxModule handle, ExMediaType type, ExMediaDir dir);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceList_Length(HxModule handle, ref int length);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceList_Item_Get(HxModule handle, int index, HxModule hVal);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceList_Item_Set(HxModule handle, int index, HxModule hVal);

		#endregion

		#region CxDeviceListItem:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceListItem_Name_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceListItem_GetProductName(HxModule handle, HxModule hResult);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceListItem_GetPinNames(HxModule handle, HxModule hResult);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceListItem_GetFrameSizes(HxModule handle, HxModule hResult, int pin);

		#endregion

		#region CxDeviceParam:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_DeviceParam_Name_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		#endregion

		#region CxMediaEvent:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_Grabber_Notify_Set(HxModule handle, IntPtr function);

		#endregion

		#region IxMediaControl:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Reset(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Start(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Stop(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Abort(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Pause(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_Wait(HxModule handle, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_IsRunning(HxModule handle, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_MediaControl_IsPaused(HxModule handle, out ExBoolean result);

		#endregion

		#region CxControlProperty:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_Controller_Set(HxModule handle, HxModule value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_Name_Set(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_IsSupported(HxModule handle, out ExBoolean value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_GetRange(HxModule handle, out TxRangeI value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_GetStep(HxModule handle, out int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_GetDefault(HxModule handle, out int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_GetFlags(HxModule handle, out int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_SetFlags(HxModule handle, int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_GetValue(HxModule handle, out int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Media_ControlProperty_SetValue(HxModule handle, int value, ExBoolean relative);

		#endregion

		// File

		#region Jpeg:

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_File_CopyJpegMarkers(LPCSTR src_file, LPCSTR dst_file, HxModule option);

		#endregion
	}
}
