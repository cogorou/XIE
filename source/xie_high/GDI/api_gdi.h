/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_GDI_H_INCLUDED_
#define _API_GDI_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_defs.h"
#include "Core/xie_core_math.h"
#include "Core/CxModule.h"
#include "Core/TxPointI.h"
#include "Core/TxPointD.h"
#include "Core/TxRectangleI.h"
#include "Core/TxRectangleD.h"
#include "Core/TxScanner1D.h"
#include "Core/TxScanner2D.h"
#include "GDI/TxCanvas.h"
#include "GDI/TxGdi2dParam.h"
#include "GDI/TxGdiPolyline.h"
#include "GDI/TxHitPosition.h"
#include <math.h>

// ////////////////////////////////////////////////////////////
// DEFINE

#if defined(_MSC_VER)
#else
const int None		= 0;
const int True		= 1;
const int False		= 0;
const int Success	= 0;
#endif

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace GDI
{

void XIE_API fnPRV_GDI_Setup();
void XIE_API fnPRV_GDI_TearDown();

#if defined(_MSC_VER)
#else
Display*		XIE_API fnPRV_GDI_XServer_Open();
void			XIE_API fnPRV_GDI_XServer_Close(Display* xserver);
XVisualInfo*	XIE_API fnPRV_GDI_XVisual_Open(Display* xserver);
void			XIE_API fnPRV_GDI_XVisual_Close(XVisualInfo* xvisual);
#endif

// ////////////////////////////////////////////////////////////
// Enumerations

// ////////////////////////////////////////////////////////////
// Structures

// ============================================================
struct TxYUV
{
	unsigned char	Y;
	unsigned char	U;
	unsigned char	V;
} XIE_ALIGNED(1);

// ============================================================
struct TxYUYV
{
	unsigned char	Y0;
	unsigned char	U;
	unsigned char	Y1;
	unsigned char	V;
} XIE_ALIGNED(1);

// ============================================================
struct TxUYVY
{
	unsigned char	U;
	unsigned char	Y0;
	unsigned char	V;
	unsigned char	Y1;
} XIE_ALIGNED(1);

// ////////////////////////////////////////////////////////////
// FUNCTION

// ======================================================================
template<class TE> static inline TxScanner1D<TE> ToScanner(TxArray src)
{
	return TxScanner1D<TE>((TE*)src.Address, src.Length, src.Model);
}
template<class TE> static inline TxScanner2D<TE> ToScanner(TxImage src, int ch)
{
	return TxScanner2D<TE>((TE*)src.Layer[ch], src.Width, src.Height, src.Stride, src.Model);
}
template<class TE> static inline TxScanner2D<TE> ToScanner(TxMatrix src)
{
	return TxScanner2D<TE>((TE*)src.Address, src.Columns, src.Rows, src.Stride, src.Model);
}

void XIE_API fnPRV_GDI_Extract(HxModule hdst, HxModule hsrc, const TxCanvas& canvas, bool swap);
void XIE_API fnPRV_GDI_Shrink0(HxModule hdst, HxModule hsrc, double mag, bool swap);
void XIE_API fnPRV_GDI_Shrink1(HxModule hdst, HxModule hsrc, double mag, bool swap);
void XIE_API fnPRV_GDI_Shrink2(HxModule hdst, HxModule hsrc, double mag, bool swap);

bool XIE_API fnPRV_GDI_CanConvertFrom(HxModule hdst, TxSizeI size);
void XIE_API fnPRV_GDI_ConvertFrom_DDB(HxModule hdst, HxModule hsrc);
void XIE_API fnPRV_GDI_ConvertFrom_YUYV(HxModule hdst, HxModule hsrc);

TxPointD XIE_API fnPRV_GDI_Scaling(double x, double y, double mag, ExGdiScalingMode mode);
TxPointD XIE_API fnPRV_GDI_Scaling(TxPointD src, double mag, ExGdiScalingMode mode);

void XIE_API fnPRV_GDI_BkPrepare(const TxGdi2dParam* param);
void XIE_API fnPRV_GDI_BkRestore(const TxGdi2dParam* param);

void XIE_API fnPRV_GDI_PenPrepare(const TxGdi2dParam* param);
void XIE_API fnPRV_GDI_PenRestore(const TxGdi2dParam* param);

int XIE_API fnPRV_GDI_HitTest_Point(const TxPointD& position, double margin, const TxPointD& figure);
int XIE_API fnPRV_GDI_HitTest_Line(const TxPointD& position, double margin, const TxLineD& figure);
TxHitPosition XIE_API fnPRV_GDI_HitTest_LineSegment(const TxPointD& position, double margin, const TxLineSegmentD& figure);
TxHitPosition XIE_API fnPRV_GDI_HitTest_Rectangle(const TxPointD& position, double margin, const TxRectangleD& figure);

#if defined(_MSC_VER)
HDC XIE_API fnPRV_GDI_CreateDC();
bool XIE_API fnPRV_GDI_WorldTransformReset( HDC hdc );
bool XIE_API fnPRV_GDI_WorldTransformRotate( HDC hdc, double axis_x, double axis_y, double angle );
bool XIE_API fnPRV_GDI_WorldTransformScale( HDC hdc, double origin_x, double origin_y, double mag_x, double mag_y );
int XIE_API fnPRV_GDI_CodePageToCharset(int codepage);
#endif

// ============================================================
template<class TS> static inline bool fnPRV_GDI_GetParam(void* dst, TxModel model, const TS& src)
{
	if (model == ModelOf<TS>())
	{
		*static_cast<TS*>(dst) = src;
		return true;
	}
	return false;
}

// ============================================================
template<class TD> static inline bool fnPRV_GDI_SetParam(const void* src, TxModel model, TD& dst)
{
	if (model == ModelOf<TD>())
	{
		dst = *static_cast<const TD*>(src);
		return true;
	}
	return false;
}

// ============================================================
bool fnPRV_Gdi2d_GetParam(TxCharCPtrA name, void* value, TxModel model, const TxGdi2dParam& param);
bool fnPRV_Gdi2d_SetParam(TxCharCPtrA name, const void* value, TxModel model, TxGdi2dParam& param);

}	// GDI
}	// xie

#endif
