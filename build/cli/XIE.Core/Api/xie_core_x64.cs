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

namespace XIE
{
	using Module_T = IntPtr;

	// Common
	using LPCSTR = System.String;

	/// <summary>
	/// ネイティブ関数群
	/// </summary>
	static partial class xie_core_x64
	{
		#region ライブラリ名.

#if (LINUX)
#if DEBUG
		internal const string LIBRARY_FILENAME = "libxie_cored.so";
#else
		internal const string LIBRARY_FILENAME = "libxie_core.so";
#endif
#else
		internal const string LIBRARY_FILENAME = "xie_core_x64_" + XIE.Defs.XIE_VER_STR + ".dll";
#endif

		#endregion

		#region 初期化と解放.
		
		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void xie_core_setup();

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void xie_core_teardown();

		#endregion

		#region デバッガ.

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_Core_TraceLevel_Set(int value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_TraceLevel_Get();

		#endregion

		// ============================================================

		#region Module

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern Module_T fnXIE_Core_Module_Create([MarshalAs(UnmanagedType.LPStr)] LPCSTR name);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Module_Destroy(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Module_Dispose(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_Module_IsValid(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Module_ID(HxModule handle);

		#endregion

		#region Memory Alloc/Free

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr fnXIE_Core_Axi_MemoryAlloc(SIZE_T size, ExBoolean zero_clear);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_Core_Axi_MemoryFree(IntPtr ptr);

		#endregion

		#region Memory Map/Unmap

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr fnXIE_Core_Axi_MemoryMap(SIZE_T size);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_Core_Axi_MemoryUnmap(IntPtr ptr, SIZE_T size);

		#endregion

		#region Memory Lock/Unlock

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_MemoryLock(IntPtr ptr, SIZE_T size);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_MemoryUnlock(IntPtr ptr, SIZE_T size);

		#endregion

		#region Model

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_SizeOf(ExType type);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_CalcBpp(ExType type);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_CalcDepth(ExType type);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void fnXIE_Core_Axi_CalcRange(ExType type, int depth, out TxRangeD range);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern double fnXIE_Core_Axi_CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int fnXIE_Core_Axi_CalcStride(TxModel model, int width, int packing_size);

		#endregion

		#region File - Raw

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckRawA(out TxRawFileHeader header, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckRawW(out TxRawFileHeader header, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_LoadRawA(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_LoadRawW(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_SaveRawA(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_SaveRawW(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename);

		#endregion

		#region File - Bmp

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckBmpA(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, ExBoolean unpack);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckBmpW(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, ExBoolean unpack);

		#endregion

		#region File - Jpeg

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckJpegA(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, ExBoolean unpack);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckJpegW(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, ExBoolean unpack);

		#endregion

		#region File - Png

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckPngA(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, ExBoolean unpack);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckPngW(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, ExBoolean unpack);

		#endregion

		#region File - Tiff

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckTiffA(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, ExBoolean unpack);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_File_CheckTiffW(out TxImageSize image_size, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, ExBoolean unpack);

		#endregion

		#region MP

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Axi_ProcessorNum_Get(out int num);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Axi_ParallelNum_Get(out int num);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Axi_ParallelNum_Set(int num);

		#endregion

		#region DateTime

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Axi_GetTime(out ulong result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_DateTime_Now(ExBoolean ltc, out TxDateTime result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_DateTime_ToBinary(TxDateTime src, ExBoolean ltc, out ulong dst);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_DateTime_FromBinary(ulong src, ExBoolean ltc, out TxDateTime dst);

		#endregion

		#region DIB

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_DIB_Load(HxModule hdst, IntPtr pvdib, ExBoolean unpack);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_DIB_Save(HxModule hsrc, ref IntPtr pvdib, ref uint bmpInfoSize, ref uint imageSize);

		#endregion

		// ============================================================

		#region IxTagPtr

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr fnXIE_Core_TagPtr(HxModule handle);

		#endregion

		#region IxEquatable

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Equatable_CopyFrom(HxModule handle, HxModule src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_Equatable_ContentEquals(HxModule handle, HxModule cmp);

		#endregion

		#region IxAttachable

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Attachable_Attach(HxModule handle, HxModule src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_Attachable_IsAttached(HxModule handle);

		#endregion

		#region IxLockable

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Lockable_Lock(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Lockable_Unlock(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_Lockable_IsLocked(HxModule handle);

		#endregion

		#region IxRunnable

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Runnable_Reset(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Runnable_Start(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Runnable_Stop(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Runnable_Wait(HxModule handle, int timeout, out ExBoolean result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Runnable_IsRunning(HxModule handle, out ExBoolean result);

		#endregion

		#region IxParam

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Param_GetParam(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name, IntPtr value, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Param_SetParam(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name, IntPtr value, TxModel model);

		#endregion

		#region IxIndexedParam

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_IndexedParam_GetParam(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name, int index, IntPtr value, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_IndexedParam_SetParam(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR name, int index, IntPtr value, TxModel model);

		#endregion

		#region IxFileAccess

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_FileAccess_LoadA(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, IntPtr option, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_FileAccess_LoadW(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, IntPtr option, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_FileAccess_SaveA(HxModule handle, [MarshalAs(UnmanagedType.LPStr)] LPCSTR filename, IntPtr option, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_FileAccess_SaveW(HxModule handle, [MarshalAs(UnmanagedType.LPWStr)] LPCSTR filename, IntPtr option, TxModel model);

		#endregion

		// ============================================================

		#region Array

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Attach(HxModule handle, TxArray src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Resize(HxModule handle, int length, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Reset(HxModule handle);

		#endregion

		#region Array - Extraction

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Statistics(HxModule hsrc, int ch, out TxStatistics result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Extract(HxModule hsrc, int index, int length, HxModule hresult);

		#endregion

		#region Array - Handling

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Clear(HxModule hdst, IntPtr value, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_ClearEx(HxModule hsrc, IntPtr value, TxModel model, int index, int count);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Cast(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Copy(HxModule hdst, HxModule hsrc, double scale);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_CopyEx(HxModule hdst, HxModule hsrc, int index, int count);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_RgbToBgr(HxModule hdst, HxModule hsrc, double scale);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Compare(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range);

		#endregion

		#region Array - Filter

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_ColorMatrix(HxModule hdst, HxModule hsrc, HxModule hmatrix);

		#endregion

		#region Array - Operation

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Not(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Math(HxModule hdst, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Ope1A(HxModule hdst, HxModule hsrc, double value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Ope1L(HxModule hdst, HxModule hsrc, uint value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Ope2A(HxModule hdst, HxModule hsrc, HxModule hval, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Array_Ope2L(HxModule hdst, HxModule hsrc, HxModule hval, int mode);

		#endregion

		// ============================================================

		#region Image

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Attach(HxModule handle, TxImage src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Resize(HxModule handle, int width, int height, TxModel model, int channels, int packing_size);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Reset(HxModule handle);

		#endregion

		#region Image - Exif

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Exif_Get(HxModule handle, out TxExif tag);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Exif_Set(HxModule handle, TxExif tag);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_ExifCopy(HxModule handle, TxExif tag, ExBoolean ltc);

		#endregion

		#region Image - Extraction

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_CalcDepth(HxModule hsrc, HxModule hmask, int ch, out int depth);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Statistics(HxModule hsrc, HxModule hmask, int ch, out TxStatistics result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Extract(HxModule hsrc, HxModule hmask, int ch, int sy, int sx, int length, ExScanDir dir, HxModule hresult);

		#endregion

		#region Image - Handling

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Clear(HxModule hdst, HxModule hmask, IntPtr value, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_ClearEx(HxModule hdst, HxModule hmask, IntPtr value, TxModel model, int index, int count);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Cast(HxModule hdst, HxModule hmask, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Copy(HxModule hdst, HxModule hmask, HxModule hsrc, double scale);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_CopyEx(HxModule hdst, HxModule hmask, HxModule hsrc, int index, int count);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_RgbToBgr(HxModule hdst, HxModule hmask, HxModule hsrc, double scale);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Compare(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hcmp, double error_range);

		#endregion

		#region Image - Filter

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_ColorMatrix(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Effect(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hparam);

		#endregion

		#region Image - GeoTrans

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Affine(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix, int interpolation);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Mirror(HxModule hdst, HxModule hmask, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Rotate(HxModule hdst, HxModule hmask, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Transpose(HxModule hdst, HxModule hmask, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Scale(HxModule hdst, HxModule hmask, HxModule hsrc, double sx, double sy, int interpolation);

		#endregion

		#region Image - Operatoin

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Not(HxModule hdst, HxModule hmask, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Math(HxModule hdst, HxModule hmask, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Ope1A(HxModule hdst, HxModule hmask, HxModule hsrc, double value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Ope1L(HxModule hdst, HxModule hmask, HxModule hsrc, uint value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Ope2A(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Image_Ope2L(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, int mode);

		#endregion

		// ============================================================

		#region Matrix

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Attach(HxModule handle, TxMatrix src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Resize(HxModule handle, int rows, int cols, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Reset(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Clear(HxModule hdst, IntPtr value, TxModel model);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Eye(HxModule hdst, double value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Cast(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Copy(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Statistics(HxModule hsrc, int ch, out TxStatistics result);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Extract(HxModule hsrc, int row, int col, int length, ExScanDir dir, HxModule hresult);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Det(HxModule hsrc, out double value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Trace(HxModule hsrc, out double value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_ScaleFactor(HxModule hsrc, out TxSizeD value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Submatrix(HxModule hdst, HxModule hsrc, int row, int col);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Invert(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Product(HxModule hdst, HxModule hsrc, HxModule hval);

		#endregion

		#region Matrix - Segmentation

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Compare(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Tril(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Triu(HxModule hdst, HxModule hsrc);

		#endregion

		#region Matrix - GeoTrans

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Mirror(HxModule hdst, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Rotate(HxModule hdst, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Transpose(HxModule hdst, HxModule hsrc);

		#endregion

		#region Matrix - Operation

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Not(HxModule hdst, HxModule hsrc);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Math(HxModule hdst, HxModule hsrc, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Ope1A(HxModule hdst, HxModule hsrc, double value, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Ope2A(HxModule hdst, HxModule hsrc, HxModule hval, int mode);

		#endregion

		#region Matrix - Preset

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Preset_Rotate(HxModule hdst, double degree, double axis_x, double axis_y);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Preset_Scale(HxModule hdst, double sx, double sy);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Preset_Translate(HxModule hdst, double tx, double ty);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Matrix_Preset_Shear(HxModule hdst, double degree_x, double degree_y);

		#endregion

		// ============================================================

		#region String

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_String_Resize(HxModule handle, int length);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_String_Reset(HxModule handle);

		#endregion

		// ============================================================

		#region Exif

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_Attach(HxModule handle, TxExif src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_Resize(HxModule handle, int length);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_Reset(HxModule handle);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_EndianType(HxModule handle, ref  ExEndianType value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_GetItems(HxModule handle, HxModule hval);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_GetPurgedExif(HxModule handle, HxModule hval);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_GetValue(HxModule handle, TxExifItem item, HxModule hval);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_Exif_SetValue(HxModule handle, TxExifItem item, HxModule hval);

		#endregion

		// ============================================================

		#region TxIplImage

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_IplImage_Equals(TxIplImage src, TxIplImage cmp);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_IplImage_IsValid(TxIplImage src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_IplImage_CopyTo(TxIplImage src, HxModule dst);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_IplImage_CopyFrom(TxIplImage dst, HxModule src);

		#endregion

		#region TxCvMat

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_CvMat_Equals(TxCvMat src, TxCvMat cmp);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExBoolean fnXIE_Core_CvMat_IsValid(TxCvMat src);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_CvMat_CopyTo(TxCvMat src, HxModule dst);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Core_CvMat_CopyFrom(TxCvMat dst, HxModule src);

		#endregion

		// ============================================================

		#region Effectors

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_Binarize1(HxModule hsrc, HxModule hdst, HxModule hmask, double threshold, bool use_abs, TxRangeD value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_Binarize2(HxModule hsrc, HxModule hdst, HxModule hmask, TxRangeD threshold, bool use_abs, TxRangeD value);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_GammaConverter(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, double gamma);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_HsvConverter(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, int hue_dir, double saturation_factor, double value_factor);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_HsvToRgb(HxModule hsrc, HxModule hdst, HxModule hmask, int depth);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_Integral(HxModule hsrc, HxModule hdst, HxModule hmask, int mode);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_Monochrome(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_PartColor(HxModule hsrc, HxModule hdst, HxModule hmask, double scale, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_RgbConverter(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_RgbToGray(HxModule hsrc, HxModule hdst, HxModule hmask, double scale, double red_ratio, double green_ratio, double blue_ratio);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_RgbToHsv(HxModule hsrc, HxModule hdst, HxModule hmask, int depth);

		[DllImport(LIBRARY_FILENAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ExStatus fnXIE_Effectors_Projection(HxModule hsrc, HxModule hdst, HxModule hmask, ExScanDir dir, int ch);

		#endregion
	}
}
