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
	using x86 = xie_high_x86;
	using x64 = xie_high_x64;

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

	/// <summary>
	/// ネイティブ関数群
	/// </summary>
	static partial class xie_high
	{
		#region 初期化と解放.

		/// <summary>
		/// 初期化
		/// </summary>
		public static void xie_high_setup()
		{
			try
			{
				if (Environment.Is64BitProcess)
					x64.xie_high_setup();
				else
					x86.xie_high_setup();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 解放
		/// </summary>
		public static void xie_high_teardown()
		{
			try
			{
				if (Environment.Is64BitProcess)
					x64.xie_high_teardown();
				else
					x86.xie_high_teardown();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 初期化 (カテゴリ指定)
		/// </summary>
		/// <param name="category">カテゴリ [File,GDI,IO,Net,UVC]</param>
		/// <returns>
		///		正常の場合は 0 を返します。
		///		異常のそれ以外を返します。
		/// </returns>
		public static int xie_high_setup_ex(string category)
		{
			try
			{
				if (Environment.Is64BitProcess)
					return x64.xie_high_setup_ex(category);
				else
					return x86.xie_high_setup_ex(category);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
				return (int)ExStatus.NotFound;
			}
		}

		/// <summary>
		/// 解放 (カテゴリ指定)
		/// </summary>
		/// <param name="category">カテゴリ [File,GDI,IO,Net,UVC]</param>
		/// <returns>
		///		正常の場合は 0 を返します。
		///		異常のそれ以外を返します。
		/// </returns>
		public static int xie_high_teardown_ex(string category)
		{
			try
			{
				if (Environment.Is64BitProcess)
					return x64.xie_high_teardown_ex(category);
				else
					return x86.xie_high_teardown_ex(category);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
				return (int)ExStatus.NotFound;
			}
		}

		#endregion

		// GDI

		#region Module:

		/// <summary>
		/// オブジェクトの生成。
		/// </summary>
		/// <param name="name">クラス名</param>
		/// <returns>
		///		オブジェクトを新規に生成して返します。
		///		失敗した場合は IntPtr.Zero を返します。
		///		使用後は fnXIE_Core_Module_Destroy で解放する必要があります。
		/// </returns>
		public static Module_T fnXIE_GDI_Module_Create(string name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Module_Create(name);
			else
				return x86.fnXIE_GDI_Module_Create(name);
		}

		#endregion

		#region Canvas: (画像描画オブジェクト)

#if LINUX
		/// <summary>
		/// 画像描画オブジェクトの初期化を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="window">描画先のウィンドウ</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Setup(HxModule handle, Window window)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Setup(handle, window);
			else
				return x86.fnXIE_GDI_Canvas_Setup(handle, window);
		}
#else
		/// <summary>
		/// 画像描画オブジェクトの初期化を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hdc">描画先のデバイスコンテキスト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Setup(HxModule handle, HDC hdc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Setup(handle, hdc);
			else
				return x86.fnXIE_GDI_Canvas_Setup(handle, hdc);
		}
#endif

		/// <summary>
		/// 画像描画オブジェクトの描画領域のサイズ変更を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="width">幅 [範囲:1~] ※幅・高さ共に 0 を指定すると解放します。</param>
		/// <param name="height">高さ [範囲:1~] ※幅・高さ共に 0 を指定すると解放します。</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Resize(HxModule handle, int width, int height)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Resize(handle, width, height);
			else
				return x86.fnXIE_GDI_Canvas_Resize(handle, width, height);
		}

		/// <summary>
		/// 画像描画オブジェクトへの描画準備を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_BeginPaint(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_BeginPaint(handle);
			else
				return x86.fnXIE_GDI_Canvas_BeginPaint(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトへの描画処理を完了します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_EndPaint(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_EndPaint(handle);
			else
				return x86.fnXIE_GDI_Canvas_EndPaint(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトの描画領域へ接続します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Lock(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Lock(handle);
			else
				return x86.fnXIE_GDI_Canvas_Lock(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトの描画領域への接続を解除します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Unlock(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Unlock(handle);
			else
				return x86.fnXIE_GDI_Canvas_Unlock(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトの背景をクリアします。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Clear(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Clear(handle);
			else
				return x86.fnXIE_GDI_Canvas_Clear(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトに背景画像を描画します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="himage">背景画像</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_DrawImage(HxModule handle, HxModule himage)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_DrawImage(handle, himage);
			else
				return x86.fnXIE_GDI_Canvas_DrawImage(handle, himage);
		}

		/// <summary>
		/// 画像描画オブジェクトに図形を描画します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="figures">描画対象の図形</param>
		/// <param name="count">描画対象の図形の個数</param>
		/// <param name="mode">伸縮モード</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_DrawOverlay(HxModule handle, HxModule[] figures, int count, ExGdiScalingMode mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_DrawOverlay(handle, figures, count, mode);
			else
				return x86.fnXIE_GDI_Canvas_DrawOverlay(handle, figures, count, mode);
		}

		/// <summary>
		/// 画像描画オブジェクトに図形を描画するための初期化を行い、指定されたコールバック関数を呼び出します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="render">描画処理関数へのポインタ</param>
		/// <param name="param">任意のパラメータ</param>
		/// <param name="mode">伸縮モード</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_DrawOverlayCB(HxModule handle, IntPtr render, IntPtr param, ExGdiScalingMode mode)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_DrawOverlayCB(handle, render, param, mode);
			else
				return x86.fnXIE_GDI_Canvas_DrawOverlayCB(handle, render, param, mode);
		}

		/// <summary>
		/// 画像描画オブジェクトに描画したデータをデバイスコンテキストに反映します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Flush(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Flush(handle);
			else
				return x86.fnXIE_GDI_Canvas_Flush(handle);
		}

		/// <summary>
		/// 画像描画オブジェクトに描画したデータを画像オブジェクトに反映します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hdst">反映先の画像オブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_FlushToImage(HxModule handle, HxModule hdst)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_FlushToImage(handle, hdst);
			else
				return x86.fnXIE_GDI_Canvas_FlushToImage(handle, hdst);
		}

		/// <summary>
		/// 有効範囲を計算します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="result">
		///		画像が表示される範囲を返します。
		///		表示倍率を乗算した画像サイズが表示範囲より小さい場合はセンタリングされます。
		///		それ以外は、表示範囲と同一です。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_EffectiveRect(HxModule handle, out TxRectangleI result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_EffectiveRect(handle, out result);
			else
				return x86.fnXIE_GDI_Canvas_EffectiveRect(handle, out result);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="result">
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_VisibleRect(HxModule handle, out TxRectangleD result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_VisibleRect(handle, out result);
			else
				return x86.fnXIE_GDI_Canvas_VisibleRect(handle, out result);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="includePartialPixel">画面淵の端数画素を含むか否か [true=切り上げ、false=切り捨て]</param>
		/// <param name="result">
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_VisibleRectI(HxModule handle, ExBoolean includePartialPixel, out TxRectangleI result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_VisibleRectI(handle, includePartialPixel, out result);
			else
				return x86.fnXIE_GDI_Canvas_VisibleRectI(handle, includePartialPixel, out result);
		}

		/// <summary>
		/// ディスプレイ座標を画像座標に変換します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="dp">キャンバス座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <param name="result">
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_DPtoIP(HxModule handle, TxPointD dp, ExGdiScalingMode mode, out TxPointD result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_DPtoIP(handle, dp, mode, out result);
			else
				return x86.fnXIE_GDI_Canvas_DPtoIP(handle, dp, mode, out result);
		}

		/// <summary>
		/// 画像座標をディスプレイ座標に変換します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="ip">画像座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <param name="result">
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </param>
		/// <returns>
		///		正常の場合は ExStatus を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_IPtoDP(HxModule handle, TxPointD ip, ExGdiScalingMode mode, out TxPointD result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_IPtoDP(handle, ip, mode, out result);
			else
				return x86.fnXIE_GDI_Canvas_IPtoDP(handle, ip, mode, out result);
		}

		#endregion

		#region Canvas: (スタティック関数)

		/// <summary>
		/// 有効範囲を計算します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="bg_size">背景サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="result">
		///		画像が表示される範囲を返します。
		///		表示倍率を乗算した画像サイズが表示範囲より小さい場合はセンタリングされます。
		///		それ以外は、表示範囲と同一です。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </param>
		public static void fnXIE_GDI_Canvas_Api_EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, out TxRectangleD result)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_GDI_Canvas_Api_EffectiveRect(display_rect, bg_size, mag, out result);
			else
				x86.fnXIE_GDI_Canvas_Api_EffectiveRect(display_rect, bg_size, mag, out result);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="bg_size">背景サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="result">
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </param>
		public static void fnXIE_GDI_Canvas_Api_VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, out TxRectangleD result)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_GDI_Canvas_Api_VisibleRect(display_rect, bg_size, mag, view_point, out result);
			else
				x86.fnXIE_GDI_Canvas_Api_VisibleRect(display_rect, bg_size, mag, view_point, out result);
		}

		/// <summary>
		/// キャンバス座標を画像座標に変換します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="bg_size">背景サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="dp">キャンバス座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <param name="result">
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </param>
		public static void fnXIE_GDI_Canvas_Api_DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, ExGdiScalingMode mode, out TxPointD result)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_GDI_Canvas_Api_DPtoIP(display_rect, bg_size, mag, view_point, dp, mode, out result);
			else
				x86.fnXIE_GDI_Canvas_Api_DPtoIP(display_rect, bg_size, mag, view_point, dp, mode, out result);
		}

		/// <summary>
		/// 画像座標をキャンバス座標に変換します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="bg_size">背景サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="ip">画像座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <param name="result">
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </param>
		public static void fnXIE_GDI_Canvas_Api_IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, ExGdiScalingMode mode, out TxPointD result)
		{
			if (Environment.Is64BitProcess)
				x64.fnXIE_GDI_Canvas_Api_IPtoDP(display_rect, bg_size, mag, view_point, ip, mode, out result);
			else
				x86.fnXIE_GDI_Canvas_Api_IPtoDP(display_rect, bg_size, mag, view_point, ip, mode, out result);
		}

		/// <summary>
		/// キャンバスの背景に描画する画像を切り出します。
		/// </summary>
		/// <param name="hdst">出力画像</param>
		/// <param name="hsrc">入力画像 (表示対象画像)</param>
		/// <param name="canvas">現在のキャンバスの情報</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Canvas_Api_Extract(HxModule hdst, HxModule hsrc, XIE.GDI.TxCanvas canvas)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Canvas_Api_Extract(hdst, hsrc, canvas);
			else
				return x86.fnXIE_GDI_Canvas_Api_Extract(hdst, hsrc, canvas);
		}

		#endregion

		#region GDI: General

		/// <summary>
		/// オーバレイ図形の内部領域のサイズ変更を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">要素数 [範囲:0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_2d_Resize(HxModule handle, int length)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_2d_Resize(handle, length);
			else
				return x86.fnXIE_GDI_2d_Resize(handle, length);
		}

		/// <summary>
		/// オーバレイ図形の要素を 0 初期化します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_2d_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_2d_Reset(handle);
			else
				return x86.fnXIE_GDI_2d_Reset(handle);
		}

		#endregion

		#region GDI: Image

		/// <summary>
		/// オーバレイ図形(画像)の内部領域のサイズ変更を行います。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="width">幅 [範囲:1~] ※幅・高さ共に 0 を指定すると解放します。</param>
		/// <param name="height">高さ [範囲:1~] ※幅・高さ共に 0 を指定すると解放します。</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Image_Resize(HxModule handle, int width, int height)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Image_Resize(handle, width, height);
			else
				return x86.fnXIE_GDI_Image_Resize(handle, width, height);
		}

		#endregion

		#region GDI: StringA

		/// <summary>
		/// オーバレイ図形(文字列)にテキストを設定します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="value">テキスト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_StringA_Text_Set(HxModule handle, string value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_StringA_Text_Set(handle, value);
			else
				return x86.fnXIE_GDI_StringA_Text_Set(handle, value);
		}

		/// <summary>
		/// オーバレイ図形(文字列)にフォント名を設定します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="value">フォント名</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_StringA_FontName_Set(HxModule handle, string value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_StringA_FontName_Set(handle, value);
			else
				return x86.fnXIE_GDI_StringA_FontName_Set(handle, value);
		}

		#endregion

		#region GDI: StringW

		/// <summary>
		/// オーバレイ図形(文字列)にテキストを設定します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="value">テキスト</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_StringW_Text_Set(HxModule handle, string value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_StringW_Text_Set(handle, value);
			else
				return x86.fnXIE_GDI_StringW_Text_Set(handle, value);
		}

		/// <summary>
		/// オーバレイ図形(文字列)にフォント名を設定します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="value">フォント名</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_StringW_FontName_Set(HxModule handle, string value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_StringW_FontName_Set(handle, value);
			else
				return x86.fnXIE_GDI_StringW_FontName_Set(handle, value);
		}

		#endregion

		#region GDI: Graphics

		/// <summary>
		/// デバイスコンテキストの有効性を検査します。
		/// </summary>
		/// <param name="hdc">デバイスコンテキスト</param>
		/// <returns>
		///		正常の場合は True を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExBoolean fnXIE_GDI_Graphics_CheckValidity(IntPtr hdc)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Graphics_CheckValidity(hdc);
			else
				return x86.fnXIE_GDI_Graphics_CheckValidity(hdc);
		}

		/// <summary>
		/// デバイスコンテキストの背景に画像を描画します。
		/// </summary>
		/// <param name="hdc">デバイスコンテキスト</param>
		/// <param name="hsrc">入力画像 (表示対象画像)</param>
		/// <param name="canvas">現在のキャンバスの情報</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Graphics_DrawImage(IntPtr hdc, HxModule hsrc, TxCanvas canvas)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Graphics_DrawImage(hdc, hsrc, canvas);
			else
				return x86.fnXIE_GDI_Graphics_DrawImage(hdc, hsrc, canvas);
		}

		/// <summary>
		/// 画像プロファイルグラフの生成
		/// </summary>
		/// <param name="hsrc">対象画像</param>
		/// <param name="canvas">キャンバスの情報</param>
		/// <param name="position">プロファイル取得位置</param>
		/// <param name="hgraphs_x">X 位置のグラフ [TxModel.F64(2) の配列] ※要素数は画像の Cnannels×Pack 分必要です。但し、Unpack の場合は 1 です。</param>
		/// <param name="hgraphs_y">Y 位置のグラフ [TxModel.F64(2) の配列] ※要素数は画像の Cnannels×Pack 分必要です。但し、Unpack の場合は 1 です。</param>
		/// <param name="values">取得位置の濃度値。[TxModel.F64(1) の配列] 要素数は画像の Cnannels×Pack 分必要です。但し、Unpack の場合は 1 です。</param>
		/// <returns>
		///		正常の場合は ExSuccess を返します。
		///		異常の場合はそれ以外を返します。
		/// </returns>
		public static ExStatus fnXIE_GDI_Profile_MakeGraph(HxModule hsrc, TxCanvas canvas, TxPointD position, IntPtr hgraphs_x, IntPtr hgraphs_y, IntPtr values)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_GDI_Profile_MakeGraph(hsrc, canvas, position, hgraphs_x, hgraphs_y, values);
			else
				return x86.fnXIE_GDI_Profile_MakeGraph(hsrc, canvas, position, hgraphs_x, hgraphs_y, values);
		}

		#endregion

		// IO

		#region 操作用.

		public static Module_T fnXIE_IO_Module_Create(LPCSTR name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_Module_Create(name);
			else
				return x86.fnXIE_IO_Module_Create(name);
		}

		public static ExStatus fnXIE_IO_Module_Setup(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_Module_Setup(handle);
			else
				return x86.fnXIE_IO_Module_Setup(handle);
		}

		public static IntPtr fnXIE_IO_SerialPort_PortName_Get(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_PortName_Get(handle);
			else
				return x86.fnXIE_IO_SerialPort_PortName_Get(handle);
		}

		public static ExStatus fnXIE_IO_SerialPort_PortName_Set(HxModule handle, LPCSTR value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_PortName_Set(handle, value);
			else
				return x86.fnXIE_IO_SerialPort_PortName_Set(handle, value);
		}

		public static ExStatus fnXIE_IO_SerialPort_Readable(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_Readable(handle, timeout, out result);
			else
				return x86.fnXIE_IO_SerialPort_Readable(handle, timeout, out result);
		}

		public static ExStatus fnXIE_IO_SerialPort_Writeable(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_Writeable(handle, timeout, out result);
			else
				return x86.fnXIE_IO_SerialPort_Writeable(handle, timeout, out result);
		}

		public static ExStatus fnXIE_IO_SerialPort_Read(HxModule handle, byte[] buffer, int length, int timeout, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_Read(handle, buffer, length, timeout, out result);
			else
				return x86.fnXIE_IO_SerialPort_Read(handle, buffer, length, timeout, out result);
		}

		public static ExStatus fnXIE_IO_SerialPort_Write(HxModule handle, byte[] buffer, int length, int timeout, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_IO_SerialPort_Write(handle, buffer, length, timeout, out result);
			else
				return x86.fnXIE_IO_SerialPort_Write(handle, buffer, length, timeout, out result);
		}

		#endregion

		// Net

		#region 操作用.

		public static Module_T fnXIE_Net_Module_Create(LPCSTR name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_Module_Create(name);
			else
				return x86.fnXIE_Net_Module_Create(name);
		}

		public static ExStatus fnXIE_Net_Module_Setup(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_Module_Setup(handle);
			else
				return x86.fnXIE_Net_Module_Setup(handle);
		}

		public static ExStatus fnXIE_Net_SocketStream_Readable(TxSocketStream tag, int timeout, out ExBoolean value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_SocketStream_Readable(tag, timeout, out value);
			else
				return x86.fnXIE_Net_SocketStream_Readable(tag, timeout, out value);
		}

		public static ExStatus fnXIE_Net_SocketStream_Writeable(TxSocketStream tag, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_SocketStream_Writeable(tag, timeout, out result);
			else
				return x86.fnXIE_Net_SocketStream_Writeable(tag, timeout, out result);
		}

		public static ExStatus fnXIE_Net_SocketStream_Read(TxSocketStream tag, byte[] buffer, int length, int timeout, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_SocketStream_Read(tag, buffer, length, timeout, out result);
			else
				return x86.fnXIE_Net_SocketStream_Read(tag, buffer, length, timeout, out result);
		}

		public static ExStatus fnXIE_Net_SocketStream_Write(TxSocketStream tag, byte[] buffer, int length, int timeout, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_SocketStream_Write(tag, buffer, length, timeout, out result);
			else
				return x86.fnXIE_Net_SocketStream_Write(tag, buffer, length, timeout, out result);
		}

		public static ExStatus fnXIE_Net_TcpClient_Connected(HxModule handle, out ExBoolean value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_TcpClient_Connected(handle, out value);
			else
				return x86.fnXIE_Net_TcpClient_Connected(handle, out value);
		}

		public static ExStatus fnXIE_Net_UdpClient_Readable(HxModule handle, int timeout, out ExBoolean value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_UdpClient_Readable(handle, timeout, out value);
			else
				return x86.fnXIE_Net_UdpClient_Readable(handle, timeout, out value);
		}

		public static ExStatus fnXIE_Net_UdpClient_Writeable(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_UdpClient_Writeable(handle, timeout, out result);
			else
				return x86.fnXIE_Net_UdpClient_Writeable(handle, timeout, out result);
		}

		public static ExStatus fnXIE_Net_UdpClient_Read(HxModule handle, byte[] buffer, int length, int timeout, ref TxIPEndPoint remoteEP, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_UdpClient_Read(handle, buffer, length, timeout, ref remoteEP, out result);
			else
				return x86.fnXIE_Net_UdpClient_Read(handle, buffer, length, timeout, ref remoteEP, out result);
		}

		public static ExStatus fnXIE_Net_UdpClient_Write(HxModule handle, byte[] buffer, int length, int timeout, ref TxIPEndPoint remoteEP, out int result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Net_UdpClient_Write(handle, buffer, length, timeout, ref remoteEP, out result);
			else
				return x86.fnXIE_Net_UdpClient_Write(handle, buffer, length, timeout, ref remoteEP, out result);
		}

		#endregion

		// Media

		#region 共通:

		/// <summary>
		/// オブジェクトを新規に生成します。
		/// </summary>
		/// <param name="name">クラス名称</param>
		/// <returns>
		///		オブジェクトを新規に生成して返します。
		///		使用後は fnXIE_Core_Module_Destroy で解放する必要があります。
		/// </returns>
		public static Module_T fnXIE_Media_Module_Create(string name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Module_Create(name);
			else
				return x86.fnXIE_Media_Module_Create(name);
		}

		#endregion

		#region CxCamera:

		/// <summary>
		/// カメラデバイスの初期化
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hVideo">ビデオ入力</param>
		/// <param name="hAudio">オーディオ入力</param>
		/// <param name="hOutput">出力先</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_Camera_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Camera_Setup(handle, hVideo, hAudio, hOutput);
			else
				return x86.fnXIE_Media_Camera_Setup(handle, hVideo, hAudio, hOutput);
		}

		/// <summary>
		/// カメラデバイスのメディアグラバーの生成
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="type">メディア種別</param>
		/// <param name="hEvent">結果を格納するグラバー</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_Camera_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Camera_CreateGrabber(handle, type, hEvent);
			else
				return x86.fnXIE_Media_Camera_CreateGrabber(handle, type, hEvent);
		}

#if LINUX
		/// <summary>
		/// カメラデバイスのプロパティページの生成
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="window">オーナーウィンドウのハンドル</param>
		/// <param name="type">メディア種別</param>
		/// <param name="mode">モード (予約されています。常に 0 を指定してください。)</param>
		/// <param name="caption">ウィンドウタイトル (IntPtr.Zero を指定した場合は製品名を使用します。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, Window window, ExMediaType type, int mode, LPCSTR caption)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Camera_OpenPropertyDialog(handle, window, type, mode, caption);
			else
				return x86.fnXIE_Media_Camera_OpenPropertyDialog(handle, window, type, mode, caption);
		}
#else
		/// <summary>
		/// カメラデバイスのプロパティページの生成
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hWnd">オーナーウィンドウのハンドル</param>
		/// <param name="type">メディア種別</param>
		/// <param name="mode">モード (予約されています。常に 0 を指定してください。)</param>
		/// <param name="caption">ウィンドウタイトル (null を指定した場合は製品名を使用します。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, HWND hWnd, ExMediaType type, int mode, LPCSTR caption)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Camera_OpenPropertyDialog(handle, hWnd, type, mode, caption);
			else
				return x86.fnXIE_Media_Camera_OpenPropertyDialog(handle, hWnd, type, mode, caption);
		}
#endif

		#endregion

		#region CxMediaPlayer:

		/// <summary>
		/// メディアプレーヤの初期化
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hVideo">ビデオ入力</param>
		/// <param name="hAudio">オーディオ入力</param>
		/// <param name="hOutput">出力先[]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaPlayer_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaPlayer_Setup(handle, hVideo, hAudio, hOutput);
			else
				return x86.fnXIE_Media_MediaPlayer_Setup(handle, hVideo, hAudio, hOutput);
		}

		/// <summary>
		/// メディアプレーヤのメディアグラバーの生成
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="type">メディア種別</param>
		/// <param name="hEvent">結果を格納するグラバー</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaPlayer_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaPlayer_CreateGrabber(handle, type, hEvent);
			else
				return x86.fnXIE_Media_MediaPlayer_CreateGrabber(handle, type, hEvent);
		}

		/// <summary>
		/// メディアプレーヤのレンダリングが完了するまで待機します。
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <param name="result">完了を検知すると true を返します。タイムアウトすると false を返します。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaPlayer_WaitForCompletion(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaPlayer_WaitForCompletion(handle, timeout, out result);
			else
				return x86.fnXIE_Media_MediaPlayer_WaitForCompletion(handle, timeout, out result);
		}

		#endregion

		#region CxScreenCapture:

		/// <summary>
		/// スクリーンキャプチャの初期化
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hWindow">キャプチャ対象のウィンドウ情報 [CxScreenListItem]</param>
		/// <param name="hAudio">オーディオ入力</param>
		/// <param name="hOutput">出力先 []</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenCapture_Setup(HxModule handle, HxModule hWindow, HxModule hAudio, HxModule hOutput)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenCapture_Setup(handle, hWindow, hAudio, hOutput);
			else
				return x86.fnXIE_Media_ScreenCapture_Setup(handle, hWindow, hAudio, hOutput);
		}

		/// <summary>
		/// スクリーンキャプチャのメディアグラバーの生成
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="type">メディア種別</param>
		/// <param name="hEvent">結果を格納するグラバー</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenCapture_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenCapture_CreateGrabber(handle, type, hEvent);
			else
				return x86.fnXIE_Media_ScreenCapture_CreateGrabber(handle, type, hEvent);
		}

		#endregion

		#region CxScreenList:

		/// <summary>
		/// デバイスリストの初期化
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenList_Setup(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenList_Setup(handle);
			else
				return x86.fnXIE_Media_ScreenList_Setup(handle);
		}

		/// <summary>
		/// デバイスリストの要素数の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">配列要素数 [0~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenList_Length(HxModule handle, ref int length)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenList_Length(handle, ref length);
			else
				return x86.fnXIE_Media_ScreenList_Length(handle, ref length);
		}

		/// <summary>
		/// デバイスリストの項目の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="index">配列指標 [0~]</param>
		/// <param name="hVal">取得する項目を格納するオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenList_Item_Get(HxModule handle, int index, HxModule hVal)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenList_Item_Get(handle, index, hVal);
			else
				return x86.fnXIE_Media_ScreenList_Item_Get(handle, index, hVal);
		}

		/// <summary>
		/// デバイスリストの項目の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="index">配列指標 [0~]</param>
		/// <param name="hVal">設定する項目が格納されたオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenList_Item_Set(HxModule handle, int index, HxModule hVal)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenList_Item_Set(handle, index, hVal);
			else
				return x86.fnXIE_Media_ScreenList_Item_Set(handle, index, hVal);
		}

		#endregion

		#region CxScreenListItem:

		/// <summary>
		/// デバイス名称の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="name">設定値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ScreenListItem_Name_Set(HxModule handle, LPCSTR name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ScreenListItem_Name_Set(handle, name);
			else
				return x86.fnXIE_Media_ScreenListItem_Name_Set(handle, name);
		}

		#endregion

		#region CxDeviceList:

		/// <summary>
		/// デバイスリストの初期化
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceList_Setup(HxModule handle, ExMediaType type, ExMediaDir dir)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceList_Setup(handle, type, dir);
			else
				return x86.fnXIE_Media_DeviceList_Setup(handle, type, dir);
		}

		/// <summary>
		/// デバイスリストの要素数の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="length">配列要素数 [0~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceList_Length(HxModule handle, ref int length)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceList_Length(handle, ref length);
			else
				return x86.fnXIE_Media_DeviceList_Length(handle, ref length);
		}

		/// <summary>
		/// デバイスリストの項目の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="index">配列指標 [0~]</param>
		/// <param name="hVal">取得する項目を格納するオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceList_Item_Get(HxModule handle, int index, HxModule hVal)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceList_Item_Get(handle, index, hVal);
			else
				return x86.fnXIE_Media_DeviceList_Item_Get(handle, index, hVal);
		}

		/// <summary>
		/// デバイスリストの項目の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="index">配列指標 [0~]</param>
		/// <param name="hVal">設定する項目が格納されたオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceList_Item_Set(HxModule handle, int index, HxModule hVal)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceList_Item_Set(handle, index, hVal);
			else
				return x86.fnXIE_Media_DeviceList_Item_Set(handle, index, hVal);
		}

		#endregion

		#region CxDeviceListItem:

		/// <summary>
		/// デバイス名称の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="name">設定値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceListItem_Name_Set(HxModule handle, LPCSTR name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceListItem_Name_Set(handle, name);
			else
				return x86.fnXIE_Media_DeviceListItem_Name_Set(handle, name);
		}

		/// <summary>
		/// プロダクト名の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hResult">取得する値を格納する文字列オブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceListItem_GetProductName(HxModule handle, HxModule hResult)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceListItem_GetProductName(handle, hResult);
			else
				return x86.fnXIE_Media_DeviceListItem_GetProductName(handle, hResult);
		}

		/// <summary>
		/// ピン名称一覧の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hResult">取得する値を格納する配列オブジェクト (※注意:各要素はヒープに確保されていますので明示的に解放する必要があります。)</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceListItem_GetPinNames(HxModule handle, HxModule hResult)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceListItem_GetPinNames(handle, hResult);
			else
				return x86.fnXIE_Media_DeviceListItem_GetPinNames(handle, hResult);
		}

		/// <summary>
		/// フレームサイズ一覧の取得
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="hResult">取得する値を格納する配列オブジェクト</param>
		/// <param name="pin">ピン番号 [0~]</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceListItem_GetFrameSizes(HxModule handle, HxModule hResult, int pin)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceListItem_GetFrameSizes(handle, hResult, pin);
			else
				return x86.fnXIE_Media_DeviceListItem_GetFrameSizes(handle, hResult, pin);
		}

		#endregion

		#region CxDeviceParam:

		/// <summary>
		/// デバイス名称の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="name">設定値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_DeviceParam_Name_Set(HxModule handle, LPCSTR name)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_DeviceParam_Name_Set(handle, name);
			else
				return x86.fnXIE_Media_DeviceParam_Name_Set(handle, name);
		}

		#endregion

		#region CxMediaEvent:

		/// <summary>
		/// Notify コールバック関数の設定
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <param name="function">登録する関数のポインタ</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_Grabber_Notify_Set(HxModule handle, IntPtr function)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_Grabber_Notify_Set(handle, function);
			else
				return x86.fnXIE_Media_Grabber_Notify_Set(handle, function);
		}

		#endregion

		#region IxMediaControl

		/// <summary>
		/// 非同期処理をリセットします。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_Reset(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Reset(handle);
			else
				return x86.fnXIE_Media_MediaControl_Reset(handle);
		}

		/// <summary>
		/// 非同期処理を開始します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_Start(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Start(handle);
			else
				return x86.fnXIE_Media_MediaControl_Start(handle);
		}

		/// <summary>
		/// 非同期処理を停止します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_Stop(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Stop(handle);
			else
				return x86.fnXIE_Media_MediaControl_Stop(handle);
		}

		/// <summary>
		/// 非同期処理の中断
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_Abort(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Abort(handle);
			else
				return x86.fnXIE_Media_MediaControl_Abort(handle);
		}

		/// <summary>
		/// 非同期処理の一時停止
		/// </summary>
		/// <param name="handle">対象のオブジェクト</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_Pause(HxModule handle)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Pause(handle);
			else
				return x86.fnXIE_Media_MediaControl_Pause(handle);
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
		public static ExStatus fnXIE_Media_MediaControl_Wait(HxModule handle, int timeout, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_Wait(handle, timeout, out result);
			else
				return x86.fnXIE_Media_MediaControl_Wait(handle, timeout, out result);
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
		public static ExStatus fnXIE_Media_MediaControl_IsRunning(HxModule handle, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_IsRunning(handle, out result);
			else
				return x86.fnXIE_Media_MediaControl_IsRunning(handle, out result);
		}

		/// <summary>
		/// 非同期処理の一時停止状態を取得します。
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="result">一時停止中の場合は true を返します。それ以外は false を返します。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_MediaControl_IsPaused(HxModule handle, out ExBoolean result)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_MediaControl_IsPaused(handle, out result);
			else
				return x86.fnXIE_Media_MediaControl_IsPaused(handle, out result);
		}

		#endregion

		#region CxControlProperty:

		/// <summary>
		/// 制御プロパティ: コントローラの設定
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">コントローラ</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_Controller_Set(HxModule handle, HxModule value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_Controller_Set(handle, value);
			else
				return x86.fnXIE_Media_ControlProperty_Controller_Set(handle, value);
		}

		/// <summary>
		/// 制御プロパティ: プロパティ名称の設定
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">プロパティ名称</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_Name_Set(HxModule handle, LPCSTR value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_Name_Set(handle, value);
			else
				return x86.fnXIE_Media_ControlProperty_Name_Set(handle, value);
		}

		/// <summary>
		/// 制御プロパティ: サポート状態の取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_IsSupported(HxModule handle, out ExBoolean value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_IsSupported(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_IsSupported(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: 設定範囲の取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_GetRange(HxModule handle, out TxRangeI value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_GetRange(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_GetRange(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: ステップサイズの取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_GetStep(HxModule handle, out int value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_GetStep(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_GetStep(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: 既定値の取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_GetDefault(HxModule handle, out int value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_GetDefault(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_GetDefault(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: 制御フラグの取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_GetFlags(HxModule handle, out int value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_GetFlags(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_GetFlags(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: 制御フラグの設定
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_SetFlags(HxModule handle, int value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_SetFlags(handle, value);
			else
				return x86.fnXIE_Media_ControlProperty_SetFlags(handle, value);
		}

		/// <summary>
		/// 制御プロパティ: 現在値の取得
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">取得値</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_GetValue(HxModule handle, out int value)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_GetValue(handle, out value);
			else
				return x86.fnXIE_Media_ControlProperty_GetValue(handle, out value);
		}

		/// <summary>
		/// 制御プロパティ: 現在値の設定
		/// </summary>
		/// <param name="handle">操作対象のオブジェクト</param>
		/// <param name="value">設定値</param>
		/// <param name="relative">設定値が相対値であるか否か</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_Media_ControlProperty_SetValue(HxModule handle, int value, ExBoolean relative)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_Media_ControlProperty_SetValue(handle, value, relative);
			else
				return x86.fnXIE_Media_ControlProperty_SetValue(handle, value, relative);
		}

		#endregion

		// File

		#region Jpeg:

		/// <summary>
		/// Jpeg Markers 複製
		/// </summary>
		/// <param name="src_file">入力ファイル (※存在するファイルを指定してください。)</param>
		/// <param name="dst_file">出力ファイル (※存在するファイルを指定してください。)</param>
		/// <param name="option">オプション。※ 現在、使用していません。常に IntPtr.Zero を指定してください。</param>
		/// <returns>
		///		正常の場合は ExStatus.Success を返します。
		///		異常の場合はその他のエラーコードを返します。
		/// </returns>
		public static ExStatus fnXIE_File_CopyJpegMarkers(LPCSTR src_file, LPCSTR dst_file, HxModule option)
		{
			if (Environment.Is64BitProcess)
				return x64.fnXIE_File_CopyJpegMarkers(src_file, dst_file, option);
			else
				return x86.fnXIE_File_CopyJpegMarkers(src_file, dst_file, option);
		}

		#endregion
	}
}
