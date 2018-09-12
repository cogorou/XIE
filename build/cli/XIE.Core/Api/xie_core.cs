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
	using x86 = xie_core_x86;
	using x64 = xie_core_x64;

	using Module_T = IntPtr;

	// Common
	using LPCSTR = System.String;

	/// <summary>
	/// ネイティブ関数群
	/// </summary>
	static partial class xie_core
	{
		#region 初期化と解放.
		
		/// <summary>
		/// 初期化
		/// </summary>
		public static void xie_core_setup()
		{
			if (Environment.Is64BitProcess)
				x64.xie_core_setup();
			else
				x86.xie_core_setup();
		}

		/// <summary>
		/// 解放
		/// </summary>
		public static void xie_core_teardown()
		{
			if (Environment.Is64BitProcess)
				x64.xie_core_teardown();
			else
				x86.xie_core_teardown();
		}

		#endregion

		#region デバッガ.

		/// <summary>
		/// トレースの ON/OFF の設定
		/// </summary>
		/// <param name="value">トレースの ON/OFF [0:OFF,1~:ON]</param>
		public static void fnXIE_Core_TraceLevel_Set(int value)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_Core_TraceLevel_Set(value);
			else
				x86.fnXIE_Core_TraceLevel_Set(value);
		}

		/// <summary>
		/// トレースの ON/OFF の取得
		/// </summary>
		/// <returns>
		///		現在設定されている値を返します。
		///		0 はトレース OFF を示します。
		///		1 以上はトレース ON を示します。
		///		既定では 0 です。
		/// </returns>
		public static int fnXIE_Core_TraceLevel_Get()
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_TraceLevel_Get();
			else
				return x86.fnXIE_Core_TraceLevel_Get();
		}

		#endregion

		#region Memory Alloc/Free

		/// <summary>
		/// ヒープメモリの確保
		/// </summary>
		/// <param name="size">要素のサイズ [1~]</param>
		/// <param name="zero_clear">0 初期化の指示</param>
		/// <returns>
		///		確保したヒープメモリの先頭アドレスを返します。<br/>
		///		使用後は fnXIE_Core_Axi_MemoryFree で解放する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_MemoryFree(System.IntPtr)"/>
		public static IntPtr fnXIE_Core_Axi_MemoryAlloc(SIZE_T size, ExBoolean zero_clear)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_MemoryAlloc(size, zero_clear);
			else
				return x86.fnXIE_Core_Axi_MemoryAlloc(size, zero_clear);
		}

		/// <summary>
		/// ヒープメモリの解放
		/// </summary>
		/// <param name="ptr">ヒープメモリの先頭アドレス</param>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_MemoryAlloc(XIE.SIZE_T,XIE.ExBoolean)"/>
		public static void fnXIE_Core_Axi_MemoryFree(IntPtr ptr)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_Core_Axi_MemoryFree(ptr);
			else
				x86.fnXIE_Core_Axi_MemoryFree(ptr);
		}

		#endregion

		#region Memory Map/Unmap

		/// <summary>
		/// メモリマップ
		/// </summary>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		確保したメモリの先頭アドレスを返します。<br/>
		///		使用後は fnXIE_Core_Axi_MemoryUnmap で解放する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_MemoryUnmap(System.IntPtr,XIE.SIZE_T)"/>
		public static IntPtr fnXIE_Core_Axi_MemoryMap(SIZE_T size)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_MemoryMap(size);
			else
				return x86.fnXIE_Core_Axi_MemoryMap(size);
		}

		/// <summary>
		/// メモリマップ解放
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		public static void fnXIE_Core_Axi_MemoryUnmap(IntPtr ptr, SIZE_T size)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_Core_Axi_MemoryUnmap(ptr, size);
			else
				x86.fnXIE_Core_Axi_MemoryUnmap(ptr, size);
		}

		#endregion

		#region Memory Lock/Unlock

		/// <summary>
		/// メモリロック
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		正常の場合は 0 を返します。異常の場合はそれ以外を返します。
		///		使用後は fnXIE_Core_Axi_MemoryUnlock で解除する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_MemoryUnlock(System.IntPtr,XIE.SIZE_T)"/>
		public static int fnXIE_Core_Axi_MemoryLock(IntPtr ptr, SIZE_T size)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_MemoryLock(ptr, size);
			else
				return x86.fnXIE_Core_Axi_MemoryLock(ptr, size);
		}

		/// <summary>
		/// メモリロック解除
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		正常の場合は 0 を返します。異常の場合はそれ以外を返します。
		/// </returns>
		public static int fnXIE_Core_Axi_MemoryUnlock(IntPtr ptr, SIZE_T size)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_MemoryUnlock(ptr, size);
			else
				return x86.fnXIE_Core_Axi_MemoryUnlock(ptr, size);
		}

		#endregion

		#region Model

		/// <summary>
		/// 型のサイズ (bytes) の計算
		/// </summary>
		/// <param name="model">型</param>
		/// <returns>
		///		指定された型のサイズ (bytes) を返します。
		/// </returns>
		public static int fnXIE_Core_Axi_SizeOf(ExType model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_SizeOf(model);
			else
				return x86.fnXIE_Core_Axi_SizeOf(model);
		}

		/// <summary>
		/// 型のサイズ (bits) の計算
		/// </summary>
		/// <param name="model">型</param>
		/// <returns>
		///		指定された型のサイズ (bits) を返します。
		/// </returns>
		public static int fnXIE_Core_Axi_CalcBpp(ExType model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_CalcBpp(model);
			else
				return x86.fnXIE_Core_Axi_CalcBpp(model);
		}

		/// <summary>
		/// ビット深度の計算
		/// </summary>
		/// <param name="model">型</param>
		/// <returns>
		///		指定された型が表わすことができる最大のビット深度を計算します。<br/>
		///		対応しない型が指定された場合は 0 を返します。<br/>
		///		<br/>
		///		<list model="table">
		///			<listheader>
		///				<term>ExType</term>
		///				<value>値</value>
		///			</listheader>
		///			<item>
		///				<term>U8</term>
		///				<value>8</value>
		///			</item>
		///			<item>
		///				<term>U32</term>
		///				<value>32</value>
		///			</item>
		///			<item>
		///				<term>U64</term>
		///				<value>64</value>
		///			</item>
		///			<item>
		///				<term>S8</term>
		///				<value>7</value>
		///			</item>
		///			<item>
		///				<term>S16</term>
		///				<value>15</value>
		///			</item>
		///			<item>
		///				<term>S32</term>
		///				<value>31</value>
		///			</item>
		///			<item>
		///				<term>S64</term>
		///				<value>63</value>
		///			</item>
		///			<item>
		///				<term>F32</term>
		///				<value>32</value>
		///			</item>
		///			<item>
		///				<term>F64</term>
		///				<value>64</value>
		///			</item>
		///		</list>
		/// </returns>
		public static int fnXIE_Core_Axi_CalcDepth(ExType model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_CalcDepth(model);
			else
				return x86.fnXIE_Core_Axi_CalcDepth(model);
		}

		/// <summary>
		/// 型のレンジの計算
		/// </summary>
		/// <param name="model">型</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		/// <param name="range">
		///		指定された型が表わすことができる値の範囲を計算します。<br/>
		///		depth に指定された値が範囲外の場合は最大値に補正します。<br/>
		///		対応しない型が指定された場合は上下限共に 0 になります。<br/>
		/// </param>
		///	<remarks>
		///		上下限値は、要素の型が符号無しか符号有りかによって異なります。<br/>
		///		例えば、U16 の depth=15 は 0~32767 、S16 の depth=15 は -32767~32767 となります。<br/>
		///		S16 (System.Int16) の最小値は -32768 ですが、画像の濃度値としては範囲外ですので、
		///		処理の過程で -32767 に飽和されることになります。<br/>
		///		<br/>
		///		<list model="table">
		///			<listheader>
		///				<term>符号</term>
		///				<lower>lower</lower>
		///				<upper>upper</upper>
		///			</listheader>
		///			<item>
		///				<term>無し</term>
		///				<lower>0</lower>
		///				<upper>+(2<sup>depth</sup> -1)</upper>
		///			</item>
		///			<item>
		///				<term>有り</term>
		///				<lower>-(2<sup>depth</sup> -1)</lower>
		///				<upper>+(2<sup>depth</sup> -1)</upper>
		///			</item>
		///		</list>
		///	</remarks>
		///	<seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_CalcDepth(TxModel)"/>
		public static void fnXIE_Core_Axi_CalcRange(ExType model, int depth, out TxRangeD range)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_Core_Axi_CalcRange(model, depth, out range);
			else
				x86.fnXIE_Core_Axi_CalcRange(model, depth, out range);
		}

		/// <summary>
		/// 濃度値のスケーリング値の計算
		/// </summary>
		/// <param name="src_type">入力画像の要素の型</param>
		/// <param name="src_depth">入力画像のビット深度 (bits) [1~]</param>
		/// <param name="dst_type">出力画像の要素の型</param>
		/// <param name="dst_depth">出力画像のビット深度 (bits) [1~]</param>
		/// <returns>
		///		ビット深度が異なる画像間で濃度値をスケーリングする際の倍率を計算して返します。<br/>
		///		範囲外の値が指定された場合は最大のビット深度に補正します。<br/>
		///	</returns>
		///	<remarks>
		///		下式で計算します。<br/>
		///		式) scale = (2<sup>dst_depth</sup> -1) / (2<sup>src_depth</sup> -1) <br/>
		///		<br/>
		///		例えば、depth=8 を depth=16 にスケーリングの場合は、src_depth=4、dst_depth=16 を指定してください。<br/>
		///		下記のように計算して 4.011764705882 を返します。<br/>
		///		<pre>
		///		src_max = 2<sup>8</sup> -1 … 255
		///		dst_max = 2<sup>16</sup> -1 … 1023
		///		scale = dst_max / srcmax … 4.011764705882
		///		</pre>
		///		濃度値をスケーリングする場合は、dst(ch,y,x) = src(ch,y,x) * scale とします。<br/>
		/// </remarks>
		///	<seealso cref="M:XIE.xie_core.fnXIE_Core_Axi_CalcDepth(TxModel)"/>
		public static double fnXIE_Core_Axi_CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_CalcScale(src_type, src_depth, dst_type, dst_depth);
			else
				return x86.fnXIE_Core_Axi_CalcScale(src_type, src_depth, dst_type, dst_depth);
		}

		/// <summary>
		/// ２次元領域の水平方向サイズ (bytes) の計算
		/// </summary>
		/// <param name="model">要素モデル</param>
		/// <param name="width">幅 (pixels)</param>
		/// <param name="packing_size">パッキングサイズ (bytes) [1,2,4,8,16]</param>
		/// <returns>
		///		指定された要素モデル(型×パック数)と幅から計算されたサイズ (bytes) が 
		///		packing_size で割り切れるように
		///		パディングを含めたサイズ (bytes) を計算します。<br/>
		///		対応しない型が指定された場合は 0 を返します。<br/>
		///		CxImage の水平方向サイズは 8 bytes (Axi.XIE_PACKING_SIZE) でパッキングされています。
		/// </returns>
		public static int fnXIE_Core_Axi_CalcStride(TxModel model, int width, int packing_size)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_CalcStride(model, width, packing_size);
			else
				return x86.fnXIE_Core_Axi_CalcStride(model, width, packing_size);
		}

		#endregion

		#region File - Raw

		/// <summary>
		/// Raw フォーマットファイルヘッダーの読み込み
		/// </summary>
		/// <param name="header">読み込み先の構造体。</param>
		/// <param name="filename">ファイル名。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_CheckRaw(out TxRawFileHeader header, string filename)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckRawA(out header, filename);
			else
				return x86.fnXIE_Core_File_CheckRawA(out header, filename);
		}

		/// <summary>
		/// Raw フォーマットファイル読み込み
		/// </summary>
		/// <param name="handle">読み込み先のオブジェクト。</param>
		/// <param name="filename">ファイル名。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_LoadRaw(HxModule handle, string filename)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_LoadRawA(handle, filename);
			else
				return x86.fnXIE_Core_File_LoadRawA(handle, filename);
		}

		/// <summary>
		/// Raw フォーマットファイル書き込み
		/// </summary>
		/// <param name="handle">保存対象のオブジェクト。</param>
		/// <param name="filename">ファイル名。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_SaveRaw(HxModule handle, string filename)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_SaveRawA(handle, filename);
			else
				return x86.fnXIE_Core_File_SaveRawA(handle, filename);
		}

		#endregion

		#region File - Bmp

		/// <summary>
		/// 画像ファイル情報の読み込み (bmp)
		/// </summary>
		/// <param name="image_size">読み込み先の画像サイズ構造体。</param>
		/// <param name="filename">ファイル名。</param>
		/// <param name="unpack">カラー画像のアンパックの指示 [True:Unpacking, False=Packing]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_CheckBmp(out TxImageSize image_size, string filename, ExBoolean unpack)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckBmpA(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckBmpA(out image_size, filename, unpack);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckBmpW(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckBmpW(out image_size, filename, unpack);
#endif
		}

		#endregion

		#region File - Jpeg

		/// <summary>
		/// 画像ファイル情報の読み込み (jpeg)
		/// </summary>
		/// <param name="image_size">読み込み先の画像サイズ構造体。</param>
		/// <param name="filename">ファイル名。</param>
		/// <param name="unpack">カラー画像のアンパックの指示 [True:Unpacking, False=Packing]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_CheckJpeg(out TxImageSize image_size, string filename, ExBoolean unpack)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckJpegA(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckJpegA(out image_size, filename, unpack);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckJpegW(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckJpegW(out image_size, filename, unpack);
#endif
		}

		#endregion

		#region File - Png

		/// <summary>
		/// 画像ファイル情報の読み込み (png)
		/// </summary>
		/// <param name="image_size">読み込み先の画像サイズ構造体。</param>
		/// <param name="filename">ファイル名。</param>
		/// <param name="unpack">カラー画像のアンパックの指示 [True:Unpacking, False=Packing]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_CheckPng(out TxImageSize image_size, string filename, ExBoolean unpack)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckPngA(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckPngA(out image_size, filename, unpack);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckPngW(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckPngW(out image_size, filename, unpack);
#endif
		}

		#endregion

		#region File - Tiff

		/// <summary>
		/// 画像ファイル情報の読み込み (tiff)
		/// </summary>
		/// <param name="image_size">読み込み先の画像サイズ構造体。</param>
		/// <param name="filename">ファイル名。</param>
		/// <param name="unpack">カラー画像のアンパックの指示 [True:Unpacking, False=Packing]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_File_CheckTiff(out TxImageSize image_size, string filename, ExBoolean unpack)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckTiffA(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckTiffA(out image_size, filename, unpack);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_File_CheckTiffW(out image_size, filename, unpack);
			else
				return x86.fnXIE_Core_File_CheckTiffW(out image_size, filename, unpack);
#endif
		}

		#endregion

		#region MP

		/// <summary>
		/// 論理プロセッサー数の取得
		/// </summary>
		/// <param name="num">論理プロセッサー数 [1~]</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Axi_ProcessorNum_Get(out int num)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_ProcessorNum_Get(out num);
			else
				return x86.fnXIE_Core_Axi_ProcessorNum_Get(out num);
		}

		/// <summary>
		/// 並列化処理の並列数の取得
		/// </summary>
		/// <param name="num">並列数 [1~]</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Axi_ParallelNum_Get(out int num)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_ParallelNum_Get(out num);
			else
				return x86.fnXIE_Core_Axi_ParallelNum_Get(out num);
		}

		/// <summary>
		/// 並列化処理の並列数の設定
		/// </summary>
		/// <param name="num">並列数 [1~]</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Axi_ParallelNum_Set(int num)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_ParallelNum_Set(num);
			else
				return x86.fnXIE_Core_Axi_ParallelNum_Set(num);
		}

		#endregion

		#region DateTime

		/// <summary>
		/// 現在時刻の取得 (バイナリ日時)
		/// </summary>
		/// <param name="result">結果。(Windows では FileTime、Linux では timespec をキャストしたものです。)</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Axi_GetTime(out ulong result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Axi_GetTime(out result);
			else
				return x86.fnXIE_Core_Axi_GetTime(out result);
		}

		/// <summary>
		/// 現在時刻の取得 (日時構造体)
		/// </summary>
		/// <param name="ltc">true=ローカル時刻、false=協定世界時</param>
		/// <param name="result">結果。</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_DateTime_Now(ExBoolean ltc, out TxDateTime result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_DateTime_Now(ltc, out result);
			else
				return x86.fnXIE_Core_DateTime_Now(ltc, out result);
		}

		/// <summary>
		/// 日時構造体からバイナリ日時への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ltc">変換元のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		/// <param name="dst">変換後</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_DateTime_ToBinary(TxDateTime src, ExBoolean ltc, out ulong dst)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_DateTime_ToBinary(src, ltc, out dst);
			else
				return x86.fnXIE_Core_DateTime_ToBinary(src, ltc, out dst);
		}

		/// <summary>
		/// バイナリ日時から日時構造体への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ltc">変換後のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		/// <param name="dst">変換後</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_DateTime_FromBinary(ulong src, ExBoolean ltc, out TxDateTime dst)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_DateTime_FromBinary(src, ltc, out dst);
			else
				return x86.fnXIE_Core_DateTime_FromBinary(src, ltc, out dst);
		}

		#endregion

		#region DIB

		/// <summary>
		/// DIB から画像オブジェクトへのデータ複製
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="pvdib">DIB の先頭アドレス</param>
		/// <param name="unpack">カラー画像のアンパックの指示 [True:Unpacking, False=Packing]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_DIB_Load(HxModule hdst, IntPtr pvdib, ExBoolean unpack)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_DIB_Load(hdst, pvdib, unpack);
			else
				return x86.fnXIE_Core_DIB_Load(hdst, pvdib, unpack);
		}

		/// <summary>
		/// DIB の生成 (※取得した領域は MemoryFree で解放する必要があります。)
		/// </summary>
		/// <param name="hsrc">元画像</param>
		/// <param name="pvdib">DIB の先頭アドレス</param>
		/// <param name="bmpInfoSize">DIB の BITMAPINFO サイズ (bytes) [40~]</param>
		/// <param name="imageSize">DIB の画像サイズ (bytes)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_DIB_Save(HxModule hsrc, ref IntPtr pvdib, ref uint bmpInfoSize, ref uint imageSize)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_DIB_Save(hsrc, ref pvdib, ref bmpInfoSize, ref imageSize);
			else
				return x86.fnXIE_Core_DIB_Save(hsrc, ref pvdib, ref bmpInfoSize, ref imageSize);
		}

		#endregion

		// ============================================================

		#region Module

		/// <summary>
		/// オブジェクトの生成。
		/// </summary>
		/// <param name="name">クラス名</param>
		/// <returns>
		///		オブジェクトを新規に生成して返します。
		///		失敗した場合は IntPtr.Zero を返します。
		///		使用後は fnXIE_Core_Module_Destroy で解放する必要があります。
		/// </returns>
		public static Module_T fnXIE_Core_Module_Create(string name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Module_Create(name);
			else
				return x86.fnXIE_Core_Module_Create(name);
		}

		/// <summary>
		/// オブジェクトの破棄。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常に破棄された場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		/// <remarks>
		///		通常は、ユーザーがこのメソッドを直接呼び出すことはありません。
		///		このハンドルを保有するクラスのデストラクタで自動的に呼び出されます。
		/// </remarks>
		public static ExStatus fnXIE_Core_Module_Destroy(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Module_Destroy(handle);
			else
				return x86.fnXIE_Core_Module_Destroy(handle);
		}

		/// <summary>
		/// オブジェクトのデータ領域の解放。(※ オブジェクト自体は解放されません。)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常に解放された場合は ExSuccess を返します。
		///		失敗した場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Module_Dispose(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Module_Dispose(handle);
			else
				return x86.fnXIE_Core_Module_Dispose(handle);
		}

		/// <summary>
		/// オブジェクトの識別子の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定のオブジェクトの識別子を返します。
		///		オブジェクトが不正な場合は、0 を返します。
		/// </returns>
		public static int fnXIE_Core_Module_ID(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Module_ID(handle);
			else
				return x86.fnXIE_Core_Module_ID(handle);
		}

		/// <summary>
		/// オブジェクトの有効性の検査
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定のオブジェクトのデータ領域が有効か否かを検査します。
		///		有効の場合は true を、無効の場合は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_Module_IsValid(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Module_IsValid(handle);
			else
				return x86.fnXIE_Core_Module_IsValid(handle);
		}

		#endregion

		// ============================================================

		#region IxTagPtr

		/// <summary>
		/// オブジェクトの情報構造体へのポインタの取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定のオブジェクトのデータ領域の情報が格納された構造体へのポインタを返します。
		///		このオブジェクトが解放されるか、データ領域が解放された場合は無効になります。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Destroy(XIE.HxModule)"/>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static IntPtr fnXIE_Core_TagPtr(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_TagPtr(handle);
			else
				return x86.fnXIE_Core_TagPtr(handle);
		}

		#endregion

		#region IxEquatable

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">複製元</param>
		/// <returns>
		///		オブジェクトの内容を複製します。
		///		型の異なるオブジェクトには対応していません。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Equatable_CopyFrom(HxModule handle, HxModule src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Equatable_CopyFrom(handle, src);
			else
				return x86.fnXIE_Core_Equatable_CopyFrom(handle, src);
		}

		/// <summary>
		/// オブジェクトの比較
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="cmp">比較対象</param>
		/// <returns>
		///		オブジェクトの内容を比較します。
		///		型の異なるオブジェクトには対応していません。
		///		一致する場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_Equatable_ContentEquals(HxModule handle, HxModule cmp)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Equatable_ContentEquals(handle, cmp);
			else
				return x86.fnXIE_Core_Equatable_ContentEquals(handle, cmp);
		}

		#endregion

		#region IxAttachable

		/// <summary>
		/// オブジェクトのアタッチ
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">アタッチ先</param>
		/// <returns>
		///		外部リソースにアタッチします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Attachable_Attach(HxModule handle, HxModule src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Attachable_Attach(handle, src);
			else
				return x86.fnXIE_Core_Attachable_Attach(handle, src);
		}

		/// <summary>
		/// オブジェクトのアタッチ状態の検査
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定のオブジェクトが他のオブジェクトにアタッチしているか否かを検査します。
		///		アタッチしている場合は true を、アタッチしていない場合は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_Attachable_IsAttached(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Attachable_IsAttached(handle);
			else
				return x86.fnXIE_Core_Attachable_IsAttached(handle);
		}

		#endregion

		#region IxLockable

		/// <summary>
		/// メモリのロック
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		メモリをロックします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Lockable_Lock(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Lockable_Lock(handle);
			else
				return x86.fnXIE_Core_Lockable_Lock(handle);
		}

		/// <summary>
		/// メモリのロック解除
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		メモリをロック解除します。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Lockable_Unlock(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Lockable_Unlock(handle);
			else
				return x86.fnXIE_Core_Lockable_Unlock(handle);
		}

		/// <summary>
		/// メモリのロック状態の検査
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定のオブジェクトがメモリをロックしているか否かを検査します。
		///		ロックしている場合は true を、ロックしていない場合は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_Lockable_IsLocked(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Lockable_IsLocked(handle);
			else
				return x86.fnXIE_Core_Lockable_IsLocked(handle);
		}

		#endregion

		#region IxRunnable

		/// <summary>
		/// 非同期処理をリセットします。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Runnable_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Runnable_Reset(handle);
			else
				return x86.fnXIE_Core_Runnable_Reset(handle);
		}

		/// <summary>
		/// 非同期処理を開始します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Runnable_Start(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Runnable_Start(handle);
			else
				return x86.fnXIE_Core_Runnable_Start(handle);
		}

		/// <summary>
		/// 非同期処理を停止します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Runnable_Stop(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Runnable_Stop(handle);
			else
				return x86.fnXIE_Core_Runnable_Stop(handle);
		}

		/// <summary>
		/// 非同期処理が停止するまで待機します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <param name="result">停止を検知すると true を返します。タイムアウトすると false を返します。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Runnable_Wait(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Runnable_Wait(handle, timeout, out result);
			else
				return x86.fnXIE_Core_Runnable_Wait(handle, timeout, out result);
		}

		/// <summary>
		/// 非同期処理の動作状態を取得します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="result">動作中の場合は true を返します。停止中の場合は false を返します。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Runnable_IsRunning(HxModule handle, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Runnable_IsRunning(handle, out result);
			else
				return x86.fnXIE_Core_Runnable_IsRunning(handle, out result);
		}

		#endregion

		#region IxParam

		/// <summary>
		/// オブジェクトのパラメータを取得します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">取得する値を格納するオブジェクト (構造体へのポインタ または HxModule)</param>
		/// <param name="model">value に指定したオブジェクトの型 (HxModule の場合は TxModel.Pointer を指定してください。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Param_GetParam(HxModule handle, string name, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Param_GetParam(handle, name, value, model);
			else
				return x86.fnXIE_Core_Param_GetParam(handle, name, value, model);
		}

		/// <summary>
		/// オブジェクトのパラメータを設定します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">設定する値が格納されたオブジェクト (構造体へのポインタ または HxModule)</param>
		/// <param name="model">value に指定したオブジェクトの型 (HxModule の場合は TxModel.Pointer を指定してください。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Param_SetParam(HxModule handle, string name, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Param_SetParam(handle, name, value, model);
			else
				return x86.fnXIE_Core_Param_SetParam(handle, name, value, model);
		}

		#endregion

		#region IxIndexedParam

		/// <summary>
		/// オブジェクトのパラメータを取得します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="name">パラメータ名称</param>
		/// <param name="index">指標</param>
		/// <param name="value">取得する値を格納するオブジェクト (構造体へのポインタ または HxModule)</param>
		/// <param name="model">value に指定したオブジェクトの型 (HxModule の場合は TxModel.Pointer を指定してください。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_IndexedParam_GetParam(HxModule handle, string name, int index, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IndexedParam_GetParam(handle, name, index, value, model);
			else
				return x86.fnXIE_Core_IndexedParam_GetParam(handle, name, index, value, model);
		}

		/// <summary>
		/// オブジェクトのパラメータを設定します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="name">パラメータ名称</param>
		/// <param name="index">指標</param>
		/// <param name="value">設定する値が格納されたオブジェクト (構造体へのポインタ または HxModule)</param>
		/// <param name="model">value に指定したオブジェクトの型 (HxModule の場合は TxModel.Pointer を指定してください。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_IndexedParam_SetParam(HxModule handle, string name, int index, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IndexedParam_SetParam(handle, name, index, value, model);
			else
				return x86.fnXIE_Core_IndexedParam_SetParam(handle, name, index, value, model);
		}

		#endregion

		#region IxFileAccess

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="filename">ファイル名</param>
		/// <param name="option">オプション</param>
		/// <param name="model">オプションの要素モデル</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_FileAccess_Load(HxModule handle, LPCSTR filename, IntPtr option, TxModel model)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_FileAccess_LoadA(handle, filename, option, model);
			else
				return x86.fnXIE_Core_FileAccess_LoadA(handle, filename, option, model);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_FileAccess_LoadW(handle, filename, option, model);
			else
				return x86.fnXIE_Core_FileAccess_LoadW(handle, filename, option, model);
#endif
		}

		/// <summary>
		/// ファイル保存
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="filename">ファイル名</param>
		/// <param name="option">オプション</param>
		/// <param name="model">オプションの要素モデル</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_FileAccess_Save(HxModule handle, LPCSTR filename, IntPtr option, TxModel model)
		{
#if LINUX
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_FileAccess_SaveA(handle, filename, option, model);
			else
				return x86.fnXIE_Core_FileAccess_SaveA(handle, filename, option, model);
#else
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_FileAccess_SaveW(handle, filename, option, model);
			else
				return x86.fnXIE_Core_FileAccess_SaveW(handle, filename, option, model);
#endif
		}

		#endregion

		// ============================================================

		#region Array

		/// <summary>
		/// アタッチ (構造体指定)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">アタッチ先の情報が格納された構造体</param>
		/// <returns>
		///		指定された情報構造体に従ってアタッチします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Attach(HxModule handle, TxArray src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Attach(handle, src);
			else
				return x86.fnXIE_Core_Array_Attach(handle, src);
		}

		/// <summary>
		/// 配列オブジェクトのサイズ変更
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型</param>
		/// <returns>
		///		指定されたオブジェクトのサイズ変更を行い、その結果を通知します。
		///		0,0 を指定した場合は fnXIE_Core_Module_Dispose を呼び出した場合と等価です。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static ExStatus fnXIE_Core_Array_Resize(HxModule handle, int length, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Resize(handle, length, model);
			else
				return x86.fnXIE_Core_Array_Resize(handle, length, model);
		}

		/// <summary>
		/// 配列オブジェクトのリセット
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定されたオブジェクトの各要素の値を 0 リセットして、その結果を通知します。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Reset(handle);
			else
				return x86.fnXIE_Core_Array_Reset(handle);
		}

		#endregion

		#region Array - Extraction

		/// <summary>
		/// 統計
		/// </summary>
		/// <param name="hsrc">入力配列</param>
		/// <param name="ch">フィールド指標 [0~]</param>
		/// <param name="result">統計データ</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Statistics(HxModule hsrc, int ch, out TxStatistics result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Statistics(hsrc, ch, out result);
			else
				return x86.fnXIE_Core_Array_Statistics(hsrc, ch, out result);
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="hsrc">入力配列</param>
		/// <param name="index">抽出開始位置 [0~Length-1]</param>
		/// <param name="length">抽出する長さ</param>
		/// <param name="hresult">出力先</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Extract(HxModule hsrc, int index, int length, HxModule hresult)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Extract(hsrc, index, length, hresult);
			else
				return x86.fnXIE_Core_Array_Extract(hsrc, index, length, hresult);
		}

		#endregion

		#region Array - Handling

		/// <summary>
		/// 配列の要素のクリア
		/// </summary>
		/// <param name="hdst">入力配列</param>
		/// <param name="value">値が格納された変数へのポインタ</param>
		/// <param name="model">値の型</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Clear(HxModule hdst, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Clear(hdst, value, model);
			else
				return x86.fnXIE_Core_Array_Clear(hdst, value, model);
		}

		/// <summary>
		/// 配列の要素のクリア (フィールド指定)
		/// </summary>
		/// <param name="hdst">入力配列</param>
		/// <param name="value">値が格納された変数へのポインタ</param>
		/// <param name="model">値の型</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <returns>
		///		指定されたオブジェクトの各要素のフィールドを指定値で初期化して、その結果を通知します。
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_ClearEx(HxModule hdst, IntPtr value, TxModel model, int index, int count)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_ClearEx(hdst, value, model, index, count);
			else
				return x86.fnXIE_Core_Array_ClearEx(hdst, value, model, index, count);
		}

		/// <summary>
		/// 配列の要素のコピー (キャスト)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Cast(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Cast(hdst, hsrc);
			else
				return x86.fnXIE_Core_Array_Cast(hdst, hsrc);
		}

		/// <summary>
		/// 配列の要素のコピー
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Copy(HxModule hdst, HxModule hsrc, double scale)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Copy(hdst, hsrc, scale);
			else
				return x86.fnXIE_Core_Array_Copy(hdst, hsrc, scale);
		}

		/// <summary>
		/// 配列の要素のコピー (フィールド指定)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_CopyEx(HxModule hdst, HxModule hsrc, int index, int count)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_CopyEx(hdst, hsrc, index, count);
			else
				return x86.fnXIE_Core_Array_CopyEx(hdst, hsrc, index, count);
		}

		/// <summary>
		/// RGB/BGR の相互変換
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_RgbToBgr(HxModule hdst, HxModule hsrc, double scale)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_RgbToBgr(hdst, hsrc, scale);
			else
				return x86.fnXIE_Core_Array_RgbToBgr(hdst, hsrc, scale);
		}

		/// <summary>
		/// 配列の要素の比較
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">比較対象配列</param>
		/// <param name="hcmp">比較対象配列</param>
		/// <param name="error_range">許容誤差</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Compare(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Compare(hdst, hsrc, hcmp, error_range);
			else
				return x86.fnXIE_Core_Array_Compare(hdst, hsrc, hcmp, error_range);
		}

		#endregion

		#region Array - Filter

		/// <summary>
		/// カラー行列フィルタ
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="hmatrix">カラー行列</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_ColorMatrix(HxModule hdst, HxModule hsrc, HxModule hmatrix)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_ColorMatrix(hdst, hsrc, hmatrix);
			else
				return x86.fnXIE_Core_Array_ColorMatrix(hdst, hsrc, hmatrix);
		}

		#endregion

		#region Array - Operation

		/// <summary>
		/// 濃度値の反転 (!pixel または ~pixel)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Not(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Not(hdst, hsrc);
			else
				return x86.fnXIE_Core_Array_Not(hdst, hsrc);
		}

		/// <summary>
		/// 数理関数
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Math(HxModule hdst, HxModule hsrc, ExMath mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Math(hdst, hsrc, (int)mode);
			else
				return x86.fnXIE_Core_Array_Math(hdst, hsrc, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：value)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="value">値</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Ope1A(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Ope1A(hdst, hsrc, value, (int)mode);
			else
				return x86.fnXIE_Core_Array_Ope1A(hdst, hsrc, value, (int)mode);
		}

		/// <summary>
		/// 論理演算 (pixel：value)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="value">値</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Ope1L(HxModule hdst, HxModule hsrc, uint value, ExOpe1L mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Ope1L(hdst, hsrc, value, (int)mode);
			else
				return x86.fnXIE_Core_Array_Ope1L(hdst, hsrc, value, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：pixel)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="hval">入力配列(右辺値)</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Ope2A(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Ope2A(hdst, hsrc, hval, (int)mode);
			else
				return x86.fnXIE_Core_Array_Ope2A(hdst, hsrc, hval, (int)mode);
		}

		/// <summary>
		/// 論理演算 (pixel：pixel)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="hval">入力配列(右辺値)</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Array_Ope2L(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2L mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Array_Ope2L(hdst, hsrc, hval, (int)mode);
			else
				return x86.fnXIE_Core_Array_Ope2L(hdst, hsrc, hval, (int)mode);
		}

		#endregion

		// ============================================================

		#region Image

		/// <summary>
		/// アタッチ (構造体指定)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">アタッチ先の情報が格納された構造体</param>
		/// <returns>
		///		指定された情報構造体に従ってアタッチします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Attach(HxModule handle, TxImage src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Attach(handle, src);
			else
				return x86.fnXIE_Core_Image_Attach(handle, src);
		}

		/// <summary>
		/// 画像オブジェクトのサイズ変更
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="width">幅 [0~]</param>
		/// <param name="height">高さ [0~]</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [0,1~16]</param>
		/// <param name="packing_size">水平方向サイズのパッキングサイズ (bytes) [1,2,4,8,16]</param>
		/// <returns>
		///		指定されたオブジェクトのサイズ変更を行い、その結果を通知します。
		///		0,0 を指定した場合は fnXIE_Core_Module_Dispose を呼び出した場合と等価です。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static ExStatus fnXIE_Core_Image_Resize(HxModule handle, int width, int height, TxModel model, int channels, int packing_size)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Resize(handle, width, height, model, channels, packing_size);
			else
				return x86.fnXIE_Core_Image_Resize(handle, width, height, model, channels, packing_size);
		}

		/// <summary>
		/// 画像オブジェクトのリセット
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定されたオブジェクトの各要素の値を 0 リセットして、その結果を通知します。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Reset(handle);
			else
				return x86.fnXIE_Core_Image_Reset(handle);
		}

		#endregion

		#region Image - Exif

		/// <summary>
		/// Exif の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="tag">Exif が格納された領域を示す配列情報 (※内部領域を参照していますので解放しないでください。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Exif_Get(HxModule handle, out TxExif tag)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Exif_Get(handle, out tag);
			else
				return x86.fnXIE_Core_Image_Exif_Get(handle, out tag);
		}

		/// <summary>
		/// Exif の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="tag">Exif が格納された領域を示す配列情報 (※内容を複製するので処理後は解放して構いません。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Exif_Set(HxModule handle, TxExif tag)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Exif_Set(handle, tag);
			else
				return x86.fnXIE_Core_Image_Exif_Set(handle, tag);
		}

		/// <summary>
		/// Exif の加工・複製
		/// </summary>
		/// <param name="handle">複製先のオブジェクト</param>
		/// <param name="tag">複製元の Exif 構造体</param>
		/// <param name="ltc">複製後の Exif のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_ExifCopy(HxModule handle, TxExif tag, ExBoolean ltc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_ExifCopy(handle, tag, ltc);
			else
				return x86.fnXIE_Core_Image_ExifCopy(handle, tag, ltc);
		}

		#endregion

		#region Image - Extraction

		/// <summary>
		/// ビット深度 (bits) の計算。(指定されたオブジェクトの濃度を表わす事ができる最小のビット深度を計算します。)
		/// </summary>
		/// <param name="hsrc">対象のオブジェクト</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="ch">チャネル指標 [範囲:-1=すべてのチャネル、0~3:指定チャネル]</param>
		/// <param name="depth">ビット深度 (bits) (正常時は 1 以上の値を返します。それ以外は 0 を返します。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_CalcDepth(HxModule hsrc, HxModule hmask, int ch, out int depth)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_CalcDepth(hsrc, hmask, ch, out depth);
			else
				return x86.fnXIE_Core_Image_CalcDepth(hsrc, hmask, ch, out depth);
		}

		/// <summary>
		/// 統計
		/// </summary>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="ch">チャネル指標 [範囲:0~3]</param>
		/// <param name="result">統計データ</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Statistics(HxModule hsrc, HxModule hmask, int ch, out TxStatistics result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Statistics(hsrc, hmask, ch, out result);
			else
				return x86.fnXIE_Core_Image_Statistics(hsrc, hmask, ch, out result);
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="ch">チャネル指標 [範囲:0~3]</param>
		/// <param name="sy">抽出位置(Y) [0~Height-1]</param>
		/// <param name="sx">抽出位置(X) [0~Width-1]</param>
		/// <param name="length">抽出する長さ [範囲:0~]</param>
		/// <param name="dir">走査方向 [X=行抽出、Y=列抽出]</param>
		/// <param name="hresult">出力先</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Extract(HxModule hsrc, HxModule hmask, int ch, int sy, int sx, int length, ExScanDir dir, HxModule hresult)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Extract(hsrc, hmask, ch, sy, sx, length, dir, hresult);
			else
				return x86.fnXIE_Core_Image_Extract(hsrc, hmask, ch, sy, sx, length, dir, hresult);
		}

		#endregion

		#region Image - Handling

		/// <summary>
		/// 画像の要素のクリア
		/// </summary>
		/// <param name="hdst">入力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="value">値が格納された変数へのポインタ</param>
		/// <param name="model">値の型</param>
		/// <returns>
		///		指定されたオブジェクトの各要素の値を指定値で初期化して、その結果を通知します。
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Clear(HxModule hdst, HxModule hmask, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Clear(hdst, hmask, value, model);
			else
				return x86.fnXIE_Core_Image_Clear(hdst, hmask, value, model);
		}

		/// <summary>
		/// 画像の要素のクリア (フィールド指定)
		/// </summary>
		/// <param name="hdst">入力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="value">値が格納された変数へのポインタ</param>
		/// <param name="model">値の型</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <returns>
		///		指定されたオブジェクトの各要素のフィールドを指定値で初期化して、その結果を通知します。
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_ClearEx(HxModule hdst, HxModule hmask, IntPtr value, TxModel model, int index, int count)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_ClearEx(hdst, hmask, value, model, index, count);
			else
				return x86.fnXIE_Core_Image_ClearEx(hdst, hmask, value, model, index, count);
		}

		/// <summary>
		/// 画像の要素のコピー (キャスト)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Cast(HxModule hdst, HxModule hmask, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Cast(hdst, hmask, hsrc);
			else
				return x86.fnXIE_Core_Image_Cast(hdst, hmask, hsrc);
		}

		/// <summary>
		/// 画像の要素のコピー
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Copy(HxModule hdst, HxModule hmask, HxModule hsrc, double scale)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Copy(hdst, hmask, hsrc, scale);
			else
				return x86.fnXIE_Core_Image_Copy(hdst, hmask, hsrc, scale);
		}

		/// <summary>
		/// 画像の要素のコピー (フィールド指定)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_CopyEx(HxModule hdst, HxModule hmask, HxModule hsrc, int index, int count)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_CopyEx(hdst, hmask, hsrc, index, count);
			else
				return x86.fnXIE_Core_Image_CopyEx(hdst, hmask, hsrc, index, count);
		}

		/// <summary>
		/// RGB/BGR の相互変換
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_RgbToBgr(HxModule hdst, HxModule hmask, HxModule hsrc, double scale)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_RgbToBgr(hdst, hmask, hsrc, scale);
			else
				return x86.fnXIE_Core_Image_RgbToBgr(hdst, hmask, hsrc, scale);
		}

		/// <summary>
		/// 画像の要素の比較
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hcmp">比較対象画像</param>
		/// <param name="error_range">許容誤差</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Compare(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hcmp, double error_range)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Compare(hdst, hmask, hsrc, hcmp, error_range);
			else
				return x86.fnXIE_Core_Image_Compare(hdst, hmask, hsrc, hcmp, error_range);
		}

		#endregion

		#region Image - Filter

		/// <summary>
		/// カラー行列フィルタ
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hmatrix">カラー行列 [3x3, Model=F64(1)]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_ColorMatrix(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_ColorMatrix(hdst, hmask, hsrc, hmatrix);
			else
				return x86.fnXIE_Core_Image_ColorMatrix(hdst, hmask, hsrc, hmatrix);
		}

		#endregion

		#region Image - GeoTrans

		/// <summary>
		/// アフィン変換
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hmatrix">変換行列</param>
		/// <param name="interpolation">濃度補間モード [0:最近傍、1:双方向、2:平均値]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Affine(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix, int interpolation)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Affine(hdst, hmask, hsrc, hmatrix, interpolation);
			else
				return x86.fnXIE_Core_Image_Affine(hdst, hmask, hsrc, hmatrix, interpolation);
		}

		/// <summary>
		/// ミラー反転
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="mode">モード [1:X方向、2:Y方向、3:XY方向]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Mirror(HxModule hdst, HxModule hmask, HxModule hsrc, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Mirror(hdst, hmask, hsrc, mode);
			else
				return x86.fnXIE_Core_Image_Mirror(hdst, hmask, hsrc, mode);
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="mode">モード [-1:-90度、+1:+90度、+2:180度]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Rotate(HxModule hdst, HxModule hmask, HxModule hsrc, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Rotate(hdst, hmask, hsrc, mode);
			else
				return x86.fnXIE_Core_Image_Rotate(hdst, hmask, hsrc, mode);
		}

		/// <summary>
		/// 転置
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Transpose(HxModule hdst, HxModule hmask, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Transpose(hdst, hmask, hsrc);
			else
				return x86.fnXIE_Core_Image_Transpose(hdst, hmask, hsrc);
		}

		/// <summary>
		/// サイズ変更
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="sx">X方向の倍率 (1.0 が等倍です。)</param>
		/// <param name="sy">Y方向の倍率 (1.0 が等倍です。)</param>
		/// <param name="interpolation">濃度補間モード [0:最近傍、1:双方向、2:平均値]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Scale(HxModule hdst, HxModule hmask, HxModule hsrc, double sx, double sy, int interpolation)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Scale(hdst, hmask, hsrc, sx, sy, interpolation);
			else
				return x86.fnXIE_Core_Image_Scale(hdst, hmask, hsrc, sx, sy, interpolation);
		}

		#endregion

		#region Image - Operation

		/// <summary>
		/// 濃度値の反転 (!pixel または ~pixel)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Not(HxModule hdst, HxModule hmask, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Not(hdst, hmask, hsrc);
			else
				return x86.fnXIE_Core_Image_Not(hdst, hmask, hsrc);
		}

		/// <summary>
		/// 数理関数
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Math(HxModule hdst, HxModule hmask, HxModule hsrc, ExMath mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Math(hdst, hmask, hsrc, (int)mode);
			else
				return x86.fnXIE_Core_Image_Math(hdst, hmask, hsrc, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：value)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="value">値</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Ope1A(HxModule hdst, HxModule hmask, HxModule hsrc, double value, ExOpe1A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, (int)mode);
			else
				return x86.fnXIE_Core_Image_Ope1A(hdst, hmask, hsrc, value, (int)mode);
		}

		/// <summary>
		/// 論理演算 (pixel：value)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="value">値</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Ope1L(HxModule hdst, HxModule hmask, HxModule hsrc, uint value, ExOpe1L mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, (int)mode);
			else
				return x86.fnXIE_Core_Image_Ope1L(hdst, hmask, hsrc, value, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：pixel)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hval">入力画像(右辺値)</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Ope2A(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, (int)mode);
			else
				return x86.fnXIE_Core_Image_Ope2A(hdst, hmask, hsrc, hval, (int)mode);
		}

		/// <summary>
		/// 論理演算 (pixel：pixel)
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hmask">マスク画像 (省略する場合は IntPtr.Zero を指定してください)</param>
		/// <param name="hsrc">入力画像</param>
		/// <param name="hval">入力画像(右辺値)</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Image_Ope2L(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2L mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, (int)mode);
			else
				return x86.fnXIE_Core_Image_Ope2L(hdst, hmask, hsrc, hval, (int)mode);
		}

		#endregion

		// ============================================================

		#region Matrix

		/// <summary>
		/// アタッチ (構造体指定)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">アタッチ先の情報が格納された構造体</param>
		/// <returns>
		///		指定された情報構造体に従ってアタッチします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Attach(HxModule handle, TxMatrix src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Attach(handle, src);
			else
				return x86.fnXIE_Core_Matrix_Attach(handle, src);
		}

		/// <summary>
		/// 行列オブジェクトのサイズ変更
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="rows">行数 [0~]</param>
		/// <param name="cols">列数 [0~]</param>
		/// <param name="model">要素の型</param>
		/// <returns>
		///		指定されたオブジェクトのサイズ変更を行い、その結果を通知します。
		///		0,0 を指定した場合は fnXIE_Core_Module_Dispose を呼び出した場合と等価です。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static ExStatus fnXIE_Core_Matrix_Resize(HxModule handle, int rows, int cols, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Resize(handle, rows, cols, model);
			else
				return x86.fnXIE_Core_Matrix_Resize(handle, rows, cols, model);
		}

		/// <summary>
		/// 行列オブジェクトのリセット (単位行列にリセットします。)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定された行列オブジェクトを単位行列にリセットして、その結果を通知します。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Reset(handle);
			else
				return x86.fnXIE_Core_Matrix_Reset(handle);
		}

		#endregion

		#region Matrix - Extraction

		/// <summary>
		/// 統計
		/// </summary>
		/// <param name="hsrc">入力</param>
		/// <param name="ch">フィールド指標 [0~]</param>
		/// <param name="result">結果</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Statistics(HxModule hsrc, int ch, out TxStatistics result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Statistics(hsrc, ch, out result);
			else
				return x86.fnXIE_Core_Matrix_Statistics(hsrc, ch, out result);
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="hsrc">入力</param>
		/// <param name="row">抽出位置(行指標) [0~Rows-1]</param>
		/// <param name="col">抽出位置(列指標) [0~Columns-1]</param>
		/// <param name="length">抽出する長さ [範囲:0~]</param>
		/// <param name="dir">走査方向 [X=行抽出、Y=列抽出]</param>
		/// <param name="hresult">出力先</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Extract(HxModule hsrc, int row, int col, int length, ExScanDir dir, HxModule hresult)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Extract(hsrc, row, col, length, dir, hresult);
			else
				return x86.fnXIE_Core_Matrix_Extract(hsrc, row, col, length, dir, hresult);
		}

		#endregion

		#region Matrix - Basic

		/// <summary>
		/// 行列の要素のクリア
		/// </summary>
		/// <param name="hdst">対象</param>
		/// <param name="value">値が格納された変数へのポインタ</param>
		/// <param name="model">値の型</param>
		/// <returns>
		///		指定されたオブジェクトの各要素の値を指定値で初期化して、その結果を通知します。
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Clear(HxModule hdst, IntPtr value, TxModel model)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Clear(hdst, value, model);
			else
				return x86.fnXIE_Core_Matrix_Clear(hdst, value, model);
		}

		/// <summary>
		/// 対角要素の初期化
		/// </summary>
		/// <param name="hdst">対象</param>
		/// <param name="value">初期化値</param>
		/// <param name="mode">モード。[0,1,2] 
		///		<list model="bullet">
		///		<item>0 … 対角成分を value で初期化し、それ以外を 0 初期化します。</item>
		///		<item>1 … 対角成分を value で初期化し、それ以外は保持します。</item>
		///		<item>2 … 対角成分は保持し、それ以外を value で初期化します。</item>
		///		</list>
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Eye(HxModule hdst, double value, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Eye(hdst, value, mode);
			else
				return x86.fnXIE_Core_Matrix_Eye(hdst, value, mode);
		}

		/// <summary>
		/// 行列の要素のコピー
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Cast(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Cast(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Cast(hdst, hsrc);
		}

		/// <summary>
		/// 行列の要素のコピー
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Copy(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Copy(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Copy(hdst, hsrc);
		}

		#endregion

		#region Matrix - Linear

		/// <summary>
		/// 行列式の計算
		/// </summary>
		/// <param name="hsrc">対象</param>
		/// <param name="value">結果 (行列式)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Det(HxModule hsrc, out double value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Det(hsrc, out value);
			else
				return x86.fnXIE_Core_Matrix_Det(hsrc, out value);
		}

		/// <summary>
		/// 対角要素の和
		/// </summary>
		/// <param name="hsrc">対象</param>
		/// <param name="value">結果 (対角成分の和)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Trace(HxModule hsrc, out double value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Trace(hsrc, out value);
			else
				return x86.fnXIE_Core_Matrix_Trace(hsrc, out value);
		}

		/// <summary>
		/// スケール変換係数の抽出
		/// </summary>
		/// <param name="hsrc">対象</param>
		/// <param name="value">結果 (スケール変換係数)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_ScaleFactor(HxModule hsrc, out TxSizeD value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_ScaleFactor(hsrc, out value);
			else
				return x86.fnXIE_Core_Matrix_ScaleFactor(hsrc, out value);
		}

		/// <summary>
		/// 行列の積
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列(左辺値)</param>
		/// <param name="hval">入力配列(右辺値)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Product(HxModule hdst, HxModule hsrc, HxModule hval)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Product(hdst, hsrc, hval);
			else
				return x86.fnXIE_Core_Matrix_Product(hdst, hsrc, hval);
		}

		/// <summary>
		/// 逆行列の計算
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">対象</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Invert(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Invert(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Invert(hdst, hsrc);
		}

		/// <summary>
		/// 小行列の取得
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <param name="row">除去対象の行指標 [0~]</param>
		/// <param name="col">除去対象の列指標 [0~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Submatrix(HxModule hdst, HxModule hsrc, int row, int col)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Submatrix(hdst, hsrc, row, col);
			else
				return x86.fnXIE_Core_Matrix_Submatrix(hdst, hsrc, row, col);
		}

		#endregion

		#region Matrix - Segmentation

		/// <summary>
		/// 行列の要素の比較
		/// </summary>
		/// <param name="hdst">出力先</param>
		/// <param name="hsrc">入力</param>
		/// <param name="hcmp">比較対象</param>
		/// <param name="error_range">許容誤差</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Compare(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Compare(hdst, hsrc, hcmp, error_range);
			else
				return x86.fnXIE_Core_Matrix_Compare(hdst, hsrc, hcmp, error_range);
		}

		#endregion

		#region Matrix - Handling

		/// <summary>
		/// 下三角成分の抽出
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Tril(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Tril(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Tril(hdst, hsrc);
		}

		/// <summary>
		/// 上三角成分の抽出
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Triu(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Triu(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Triu(hdst, hsrc);
		}

		#endregion

		#region Matrix - GeoTrans

		/// <summary>
		/// ミラー反転
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <param name="mode">モード [範囲:1=X方向、2=Y方向、3:XY方向]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Mirror(HxModule hdst, HxModule hsrc, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Mirror(hdst, hsrc, mode);
			else
				return x86.fnXIE_Core_Matrix_Mirror(hdst, hsrc, mode);
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <param name="mode">モード [範囲:0=X方向、1=Y方向、2:XY方向]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Rotate(HxModule hdst, HxModule hsrc, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Rotate(hdst, hsrc, mode);
			else
				return x86.fnXIE_Core_Matrix_Rotate(hdst, hsrc, mode);
		}

		/// <summary>
		/// 転置
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Transpose(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Transpose(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Transpose(hdst, hsrc);
		}

		#endregion

		#region Matrix - Filter

		/// <summary>
		/// 符号反転
		/// </summary>
		/// <param name="hdst">出力先 (必要に応じて再確保されます。)</param>
		/// <param name="hsrc">入力</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Not(HxModule hdst, HxModule hsrc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Not(hdst, hsrc);
			else
				return x86.fnXIE_Core_Matrix_Not(hdst, hsrc);
		}

		/// <summary>
		/// 数理関数
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Math(HxModule hdst, HxModule hsrc, ExMath mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Math(hdst, hsrc, (int)mode);
			else
				return x86.fnXIE_Core_Matrix_Math(hdst, hsrc, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：value)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="value">値</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Ope1A(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, (int)mode);
			else
				return x86.fnXIE_Core_Matrix_Ope1A(hdst, hsrc, value, (int)mode);
		}

		/// <summary>
		/// 算術演算 (pixel：pixel)
		/// </summary>
		/// <param name="hdst">出力配列</param>
		/// <param name="hsrc">入力配列</param>
		/// <param name="hval">入力配列(右辺値)</param>
		/// <param name="mode">モード</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Ope2A(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, (int)mode);
			else
				return x86.fnXIE_Core_Matrix_Ope2A(hdst, hsrc, hval, (int)mode);
		}

		#endregion

		#region Matrix - Preset

		/// <summary>
		/// 回転行列の生成
		/// </summary>
		/// <param name="hdst">対象 (必要に応じて再確保されます。)</param>
		/// <param name="degree">回転角 (degree)</param>
		/// <param name="axis_x">回転の基軸(X)</param>
		/// <param name="axis_y">回転の基軸(Y)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Preset_Rotate(HxModule hdst, double degree, double axis_x, double axis_y)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Preset_Rotate(hdst, degree, axis_x, axis_y);
			else
				return x86.fnXIE_Core_Matrix_Preset_Rotate(hdst, degree, axis_x, axis_y);
		}

		/// <summary>
		/// スケール変換行列の生成
		/// </summary>
		/// <param name="hdst">対象 (必要に応じて再確保されます。)</param>
		/// <param name="sx">X 方向倍率 [1.0 = 等倍]</param>
		/// <param name="sy">Y 方向倍率 [1.0 = 等倍]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Preset_Scale(HxModule hdst, double sx, double sy)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Preset_Scale(hdst, sx, sy);
			else
				return x86.fnXIE_Core_Matrix_Preset_Scale(hdst, sx, sy);
		}

		/// <summary>
		/// 平行移動行列の生成
		/// </summary>
		/// <param name="hdst">対象 (必要に応じて再確保されます。)</param>
		/// <param name="tx">X 方向移動量</param>
		/// <param name="ty">Y 方向移動量</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Preset_Translate(HxModule hdst, double tx, double ty)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Preset_Translate(hdst, tx, ty);
			else
				return x86.fnXIE_Core_Matrix_Preset_Translate(hdst, tx, ty);
		}

		/// <summary>
		/// せん断行列の生成
		/// </summary>
		/// <param name="hdst">対象 (必要に応じて再確保されます。)</param>
		/// <param name="degree_x">X 方向せん断角度 (degree)</param>
		/// <param name="degree_y">Y 方向せん断角度 (degree)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Matrix_Preset_Shear(HxModule hdst, double degree_x, double degree_y)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Matrix_Preset_Shear(hdst, degree_x, degree_y);
			else
				return x86.fnXIE_Core_Matrix_Preset_Shear(hdst, degree_x, degree_y);
		}

		#endregion

		// ============================================================

		#region String

		/// <summary>
		/// 文字列オブジェクトのサイズ変更
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <returns>
		///		指定されたオブジェクトのサイズ変更を行い、その結果を通知します。
		///		0 を指定した場合は fnXIE_Core_Module_Dispose を呼び出した場合と等価です。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static ExStatus fnXIE_Core_String_Resize(HxModule handle, int length)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_String_Resize(handle, length);
			else
				return x86.fnXIE_Core_String_Resize(handle, length);
		}

		/// <summary>
		/// 文字列オブジェクトのリセット
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_String_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_String_Reset(handle);
			else
				return x86.fnXIE_Core_String_Reset(handle);
		}

		#endregion

		// ============================================================

		#region Exif

		/// <summary>
		/// アタッチ (構造体指定)
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="src">アタッチ先の情報が格納された構造体</param>
		/// <returns>
		///		指定された情報構造体に従ってアタッチします。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_Attach(HxModule handle, TxExif src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_Attach(handle, src);
			else
				return x86.fnXIE_Core_Exif_Attach(handle, src);
		}

		/// <summary>
		/// Exif オブジェクトのサイズ変更
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <returns>
		///		指定されたオブジェクトのサイズ変更を行い、その結果を通知します。
		///		0,0 を指定した場合は fnXIE_Core_Module_Dispose を呼び出した場合と等価です。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		/// <seealso cref="M:XIE.xie_core.fnXIE_Core_Module_Dispose(XIE.HxModule)"/>
		public static ExStatus fnXIE_Core_Exif_Resize(HxModule handle, int length)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_Resize(handle, length);
			else
				return x86.fnXIE_Core_Exif_Resize(handle, length);
		}

		/// <summary>
		/// Exif オブジェクトのリセット
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		指定されたオブジェクトの各要素の値を 0 リセットして、その結果を通知します。
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_Reset(handle);
			else
				return x86.fnXIE_Core_Exif_Reset(handle);
		}

		#endregion

		#region Exif - Get/Set

		/// <summary>
		/// エンディアンタイプを取得します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="value">取得した値を格納する変数</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_EndianType(HxModule handle, ref ExEndianType value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_EndianType(handle, ref value);
			else
				return x86.fnXIE_Core_Exif_EndianType(handle, ref value);
		}

		/// <summary>
		/// すべての項目を取得します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hval">取得した項目を格納する配列オブジェクト</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_GetItems(HxModule handle, HxModule hval)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_GetItems(handle, hval);
			else
				return x86.fnXIE_Core_Exif_GetItems(handle, hval);
		}

		/// <summary>
		/// 不要な情報を除去して取得します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hval">取得した項目を格納する配列オブジェクト</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_GetPurgedExif(HxModule handle, HxModule hval)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_GetPurgedExif(handle, hval);
			else
				return x86.fnXIE_Core_Exif_GetPurgedExif(handle, hval);
		}

		/// <summary>
		/// 指定された項目の値を取得します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="item">項目情報</param>
		/// <param name="hval">取得した項目を格納する配列オブジェクト</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_GetValue(HxModule handle, TxExifItem item, HxModule hval)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_GetValue(handle, item, hval);
			else
				return x86.fnXIE_Core_Exif_GetValue(handle, item, hval);
		}

		/// <summary>
		/// 指定された項目の値を設定します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="item">項目情報</param>
		/// <param name="hval">設定する項目を格納した配列オブジェクト</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_Exif_SetValue(HxModule handle, TxExifItem item, HxModule hval)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_Exif_SetValue(handle, item, hval);
			else
				return x86.fnXIE_Core_Exif_SetValue(handle, item, hval);
		}

		#endregion

		// ============================================================

		#region TxIplImage

		/// <summary>
		/// 構造体の各フィールドを比較します。
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <param name="cmp">比較対象</param>
		/// <returns>
		///		一致する場合は true を、それ以外は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_IplImage_Equals(TxIplImage src, TxIplImage cmp)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IplImage_Equals(src, cmp);
			else
				return x86.fnXIE_Core_IplImage_Equals(src, cmp);
		}

		/// <summary>
		/// 構造体に設定された情報が有効か否かを検査します。
		/// </summary>
		/// <param name="src">検査対象</param>
		/// <returns>
		///		有効な場合は true を、それ以外は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_IplImage_IsValid(TxIplImage src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IplImage_IsValid(src);
			else
				return x86.fnXIE_Core_IplImage_IsValid(src);
		}

		/// <summary>
		/// 画像データを IPL 仕様から XIE 仕様へ変換します。
		/// </summary>
		/// <param name="src">複製元</param>
		/// <param name="dst">複製先 (内部で領域の再確保が行われます。)</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_IplImage_CopyTo(TxIplImage src, HxModule dst)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IplImage_CopyTo(src, dst);
			else
				return x86.fnXIE_Core_IplImage_CopyTo(src, dst);
		}

		/// <summary>
		/// 画像データを XIE 仕様から IPL 仕様へ変換します。
		/// </summary>
		/// <param name="dst">複製先 (予め有効な領域を設定してください。)</param>
		/// <param name="src">複製元</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_IplImage_CopyFrom(TxIplImage dst, HxModule src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_IplImage_CopyFrom(dst, src);
			else
				return x86.fnXIE_Core_IplImage_CopyFrom(dst, src);
		}

		#endregion

		#region TxCvMat

		/// <summary>
		/// 構造体の各フィールドを比較します。
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <param name="cmp">比較対象</param>
		/// <returns>
		///		一致する場合は true を、それ以外は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_CvMat_Equals(TxCvMat src, TxCvMat cmp)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_CvMat_Equals(src, cmp);
			else
				return x86.fnXIE_Core_CvMat_Equals(src, cmp);
		}

		/// <summary>
		/// 構造体に設定された情報が有効か否かを検査します。
		/// </summary>
		/// <param name="src">検査対象</param>
		/// <returns>
		///		有効な場合は true を、それ以外は false を返します。
		/// </returns>
		public static ExBoolean fnXIE_Core_CvMat_IsValid(TxCvMat src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_CvMat_IsValid(src);
			else
				return x86.fnXIE_Core_CvMat_IsValid(src);
		}

		/// <summary>
		/// 画像データを IPL 仕様から XIE 仕様へ変換します。
		/// </summary>
		/// <param name="src">複製元</param>
		/// <param name="dst">複製先 (内部で領域の再確保が行われます。)</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_CvMat_CopyTo(TxCvMat src, HxModule dst)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_CvMat_CopyTo(src, dst);
			else
				return x86.fnXIE_Core_CvMat_CopyTo(src, dst);
		}

		/// <summary>
		/// 画像データを XIE 仕様から IPL 仕様へ変換します。
		/// </summary>
		/// <param name="dst">複製先 (予め有効な領域を設定してください。)</param>
		/// <param name="src">複製元</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Core_CvMat_CopyFrom(TxCvMat dst, HxModule src)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Core_CvMat_CopyFrom(dst, src);
			else
				return x86.fnXIE_Core_CvMat_CopyFrom(dst, src);
		}

		#endregion

		// ============================================================

		#region Effectors

		/// <summary>
		/// ２値化 (単一閾値)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="threshold">閾値 (threshold≦src を真とします。)</param>
		/// <param name="use_abs">入力値を絶対値として扱うか否か</param>
		/// <param name="value">出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_Binarize1(HxModule hsrc, HxModule hdst, HxModule hmask, double threshold, bool use_abs, TxRangeD value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_Binarize1(hsrc, hdst, hmask, threshold, use_abs, value);
			else
				return x86.fnXIE_Effectors_Binarize1(hsrc, hdst, hmask, threshold, use_abs, value);
		}

		/// <summary>
		/// ２値化 (バンド閾値)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="threshold">閾値 (threshold.Lower≦src≦threshold.Upper を真とします。)</param>
		/// <param name="use_abs">入力値を絶対値として扱うか否か</param>
		/// <param name="value">出力値の範囲 (真の場合 Upper、偽の場合は Lower を使用します。)</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_Binarize2(HxModule hsrc, HxModule hdst, HxModule hmask, TxRangeD threshold, bool use_abs, TxRangeD value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_Binarize2(hsrc, hdst, hmask, threshold, use_abs, value);
			else
				return x86.fnXIE_Effectors_Binarize2(hsrc, hdst, hmask, threshold, use_abs, value);
		}

		/// <summary>
		/// ガンマ変換
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <param name="gamma">ガンマ値 [0.0 以外] ※ 1.0 が無変換を意味します。</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_GammaConverter(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, double gamma)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_GammaConverter(hsrc, hdst, hmask, depth, gamma);
			else
				return x86.fnXIE_Effectors_GammaConverter(hsrc, hdst, hmask, depth, gamma);
		}

		/// <summary>
		/// HSV 変換
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <param name="hue_dir">色相の回転方向 [0~±180 または 0~360]</param>
		/// <param name="saturation_factor">彩度の変換係数 [0.0~]</param>
		/// <param name="value_factor">明度の変換係数 [0.0~]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_HsvConverter(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, int hue_dir, double saturation_factor, double value_factor)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_HsvConverter(hsrc, hdst, hmask, depth, hue_dir, saturation_factor, value_factor);
			else
				return x86.fnXIE_Effectors_HsvConverter(hsrc, hdst, hmask, depth, hue_dir, saturation_factor, value_factor);
		}

		/// <summary>
		/// 色空間変換 (HSV から RGB へ変換します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_HsvToRgb(HxModule hsrc, HxModule hdst, HxModule hmask, int depth)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_HsvToRgb(hsrc, hdst, hmask, depth);
			else
				return x86.fnXIE_Effectors_HsvToRgb(hsrc, hdst, hmask, depth);
		}

		/// <summary>
		/// 積分画像生成
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="mode">処理モード [1:総和、2:2乗の総和]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_Integral(HxModule hsrc, HxModule hdst, HxModule hmask, int mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_Integral(hsrc, hdst, hmask, mode);
			else
				return x86.fnXIE_Effectors_Integral(hsrc, hdst, hmask, mode);
		}

		/// <summary>
		/// モノクローム変換 (RGB 色空間のカラー画像の各画素に対し、指定のパラメータで生成したカラー行列を掛けてモノクローム画像に変換します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="red_ratio">赤成分の変換係数 [0~1]</param>
		/// <param name="green_ratio">緑成分の変換係数 [0~1]</param>
		/// <param name="blue_ratio">青成分の変換係数 [0~1]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_Monochrome(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_Monochrome(hsrc, hdst, hmask, red_ratio, green_ratio, blue_ratio);
			else
				return x86.fnXIE_Effectors_Monochrome(hsrc, hdst, hmask, red_ratio, green_ratio, blue_ratio);
		}

		/// <summary>
		/// パートカラー変換 (RGB 色空間のカラー画像の各画素が特定の色相範囲内 (HueDir±HueRange) にあるか否かを判定し、範囲内であれば入力元の値をそのまま出力し、範囲外であれば濃淡化して出力します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <param name="hue_dir">抽出する色相の方向 [0~±180 または 0~360] ※ hue_dir±hue_range が抽出対象になります。</param>
		/// <param name="hue_range">抽出する色相の範囲 [0~180] ※ hue_dir±hue_range が抽出対象になります。</param>
		/// <param name="red_ratio">赤成分の変換係数 [0~1] [既定値:0.299]</param>
		/// <param name="green_ratio">緑成分の変換係数 [0~1] [既定値:0.587]</param>
		/// <param name="blue_ratio">青成分の変換係数 [0~1] [既定値:0.114]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_PartColor(HxModule hsrc, HxModule hdst, HxModule hmask, double scale, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_PartColor(hsrc, hdst, hmask, scale, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
			else
				return x86.fnXIE_Effectors_PartColor(hsrc, hdst, hmask, scale, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
		}

		/// <summary>
		/// RGB 変換 (RGB 色空間のカラー画像の各画素に対し、指定のパラメータで生成したカラー行列を掛けて変換します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="red_ratio">赤成分の変換係数 [0~1]</param>
		/// <param name="green_ratio">緑成分の変換係数 [0~1]</param>
		/// <param name="blue_ratio">青成分の変換係数 [0~1]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_RgbConverter(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_RgbConverter(hsrc, hdst, hmask, red_ratio, green_ratio, blue_ratio);
			else
				return x86.fnXIE_Effectors_RgbConverter(hsrc, hdst, hmask, red_ratio, green_ratio, blue_ratio);
		}

		/// <summary>
		/// 濃淡化 (RGB 色空間のカラー画像を濃淡化します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <param name="red_ratio">赤成分の変換係数 [0~1] [既定値:0.299]</param>
		/// <param name="green_ratio">緑成分の変換係数 [0~1] [既定値:0.587]</param>
		/// <param name="blue_ratio">青成分の変換係数 [0~1] [既定値:0.114]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_RgbToGray(HxModule hsrc, HxModule hdst, HxModule hmask, double scale, double red_ratio, double green_ratio, double blue_ratio)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_RgbToGray(hsrc, hdst, hmask, scale, red_ratio, green_ratio, blue_ratio);
			else
				return x86.fnXIE_Effectors_RgbToGray(hsrc, hdst, hmask, scale, red_ratio, green_ratio, blue_ratio);
		}

		/// <summary>
		/// 色空間変換 (RGB から HSV へ変換します。)
		/// </summary>
		/// <param name="hsrc">入力元</param>
		/// <param name="hdst">出力先</param>
		/// <param name="hmask">マスク (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="depth">ビット深度 [0,1~64]</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_RgbToHsv(HxModule hsrc, HxModule hdst, HxModule hmask, int depth)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_RgbToHsv(hsrc, hdst, hmask, depth);
			else
				return x86.fnXIE_Effectors_RgbToHsv(hsrc, hdst, hmask, depth);
		}

		/// <summary>
		/// 濃度投影
		/// </summary>
		/// <param name="hsrc">入力元 [CxImage]</param>
		/// <param name="hdst">出力先 [CxArray]</param>
		/// <param name="hmask">マスク [CxImage] (※ 省略する場合は IntPtr.Zero を指定してください。)</param>
		/// <param name="dir">投影方向</param>
		/// <param name="ch">処理対象のチャネル指標(またはフィールド指標) [0~] ※ 処理対象画像の Model.Pack * Channels 未満である必要があります。</param>
		/// <returns>
		///		正常時は ExStatus.Success を返します。
		///		異常時は、それ以外のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Effectors_Projection(HxModule hsrc, HxModule hdst, HxModule hmask, ExScanDir dir, int ch)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Effectors_Projection(hsrc, hdst, hmask, dir, ch);
			else
				return x86.fnXIE_Effectors_Projection(hsrc, hdst, hmask, dir, ch);
		}

		#endregion
	}
}
