/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_CORE_TYPES_H_INCLUDED_
#define _XIE_CORE_TYPES_H_INCLUDED_

#include "xie_core.h"

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

// Data
struct XIE_EXPORT_CLASS TxModel;
struct XIE_EXPORT_CLASS TxArray;
struct XIE_EXPORT_CLASS TxLayer;
struct XIE_EXPORT_CLASS TxImage;
struct XIE_EXPORT_CLASS TxImageSize;
struct XIE_EXPORT_CLASS TxMatrix;
struct XIE_EXPORT_CLASS TxStringA;
struct XIE_EXPORT_CLASS TxStringW;

// IO
struct XIE_EXPORT_CLASS TxDateTime;
struct XIE_EXPORT_CLASS TxFrameIndex;
struct XIE_EXPORT_CLASS TxRawFileHeader;

// Color
struct XIE_EXPORT_CLASS TxBGR8x3;
struct XIE_EXPORT_CLASS TxBGR8x4;
struct XIE_EXPORT_CLASS TxRGB8x3;
struct XIE_EXPORT_CLASS TxRGB8x4;

// Struct
struct XIE_EXPORT_CLASS TxPointD;
struct XIE_EXPORT_CLASS TxPointI;
struct XIE_EXPORT_CLASS TxSizeD;
struct XIE_EXPORT_CLASS TxSizeI;
struct XIE_EXPORT_CLASS TxRangeD;
struct XIE_EXPORT_CLASS TxRangeI;
struct XIE_EXPORT_CLASS TxCircleD;
struct XIE_EXPORT_CLASS TxCircleI;
struct XIE_EXPORT_CLASS TxCircleArcD;
struct XIE_EXPORT_CLASS TxCircleArcI;
struct XIE_EXPORT_CLASS TxEllipseD;
struct XIE_EXPORT_CLASS TxEllipseI;
struct XIE_EXPORT_CLASS TxEllipseArcD;
struct XIE_EXPORT_CLASS TxEllipseArcI;
struct XIE_EXPORT_CLASS TxLineD;
struct XIE_EXPORT_CLASS TxLineI;
struct XIE_EXPORT_CLASS TxLineSegmentD;
struct XIE_EXPORT_CLASS TxLineSegmentI;
struct XIE_EXPORT_CLASS TxRectangleD;
struct XIE_EXPORT_CLASS TxRectangleI;
struct XIE_EXPORT_CLASS TxTrapezoidD;
struct XIE_EXPORT_CLASS TxTrapezoidI;
struct XIE_EXPORT_CLASS TxStatistics;

// OpenCV/IPL
struct XIE_EXPORT_CLASS TxIplImage;
struct XIE_EXPORT_CLASS TxCvMat;

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#endif
