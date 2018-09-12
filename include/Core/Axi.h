/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXI_H_INCLUDED_
#define _AXI_H_INCLUDED_

#include "xie_core.h"

#include "TxModel.h"
#include "TxArray.h"
#include "TxImage.h"
#include "TxImageSize.h"
#include "TxMatrix.h"

#include "TxDateTime.h"
#include "TxFrameIndex.h"
#include "TxRawFileHeader.h"

#include "TxBGR8x3.h"
#include "TxBGR8x4.h"
#include "TxRGB8x3.h"
#include "TxRGB8x4.h"

#include "TxPointD.h"
#include "TxPointI.h"
#include "TxSizeD.h"
#include "TxSizeI.h"
#include "TxRangeD.h"
#include "TxRangeI.h"
#include "TxCircleD.h"
#include "TxCircleI.h"
#include "TxCircleArcD.h"
#include "TxCircleArcI.h"
#include "TxEllipseD.h"
#include "TxEllipseI.h"
#include "TxEllipseArcD.h"
#include "TxEllipseArcI.h"
#include "TxLineD.h"
#include "TxLineI.h"
#include "TxLineSegmentD.h"
#include "TxLineSegmentI.h"
#include "TxRectangleD.h"
#include "TxRectangleI.h"
#include "TxTrapezoidD.h"
#include "TxTrapezoidI.h"
#include "TxStatistics.h"

#include "AxiMath.h"

#include <typeinfo>

namespace xie
{
namespace Axi
{

// Debugger
XIE_EXPORT_CLASS void TraceLevel(int level);
XIE_EXPORT_CLASS int TraceLevel();

// Aligned Buffer
XIE_EXPORT_CLASS void* AlignedAlloc(size_t size, size_t alignment);
XIE_EXPORT_CLASS void AlignedFree(void* ptr);

// Memory Alloc
XIE_EXPORT_CLASS void* MemoryAlloc(size_t size, bool zero_clear = false);
XIE_EXPORT_CLASS void MemoryFree(void* ptr);

// Memory Map
XIE_EXPORT_CLASS void* MemoryMap(size_t size);
XIE_EXPORT_CLASS void MemoryUnmap(void* ptr, size_t size);

// Memory Lock
XIE_EXPORT_CLASS int MemoryLock(void* ptr, size_t size);
XIE_EXPORT_CLASS int MemoryUnlock(void* ptr, size_t size);

// Type
XIE_EXPORT_CLASS int SizeOf(ExType type);
XIE_EXPORT_CLASS int CalcBpp(ExType type);
XIE_EXPORT_CLASS int CalcDepth(ExType type);
XIE_EXPORT_CLASS TxRangeD CalcRange(ExType type, int depth);
XIE_EXPORT_CLASS double CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth);

// Model
XIE_EXPORT_CLASS int CalcStride(TxModel model, int width, int packing_size);

// File - Check
XIE_EXPORT_CLASS TxImageSize CheckBmp		(TxCharCPtrA filename, bool unpack);
XIE_EXPORT_CLASS TxImageSize CheckJpeg		(TxCharCPtrA filename, bool unpack);
XIE_EXPORT_CLASS TxImageSize CheckPng		(TxCharCPtrA filename, bool unpack);
XIE_EXPORT_CLASS TxImageSize CheckTiff		(TxCharCPtrA filename, bool unpack);
XIE_EXPORT_CLASS TxRawFileHeader CheckRaw	(TxCharCPtrA filename);

// Time
XIE_EXPORT_CLASS void Sleep( int timeout );
XIE_EXPORT_CLASS unsigned long long	GetTime();

// SafeCast
#if !defined(_MSC_VER)
XIE_EXPORT_CLASS bool IsClassPtr(const void* body);
#endif

// ======================================================================
template<class TD> bool ClassIs(const IxModule& src)
{
	try
	{
		#if !defined(_MSC_VER)
		if (!IsClassPtr(&src)) return false;
		#endif
		return (NULL != dynamic_cast<const TD*>(&src));
	}
	catch(const std::exception&)
	{
		return false;
	}
}
// ======================================================================
template<class TD> bool ClassIs(const IxModule* psrc)
{
	try
	{
		if (psrc == NULL) return false;
		#if !defined(_MSC_VER)
		if (!IsClassPtr(psrc)) return false;
		#endif
		return (NULL != dynamic_cast<const TD*>(psrc));
	}
	catch(const std::exception&)
	{
		return false;
	}
}
// ======================================================================
template<class TD> bool ClassIs(HxModule hsrc)
{
	try
	{
		if (hsrc == NULL) return false;
		#if !defined(_MSC_VER)
		if (!IsClassPtr(hsrc)) return false;
		#endif
		return (NULL != dynamic_cast<const TD*>(reinterpret_cast<IxModule*>(hsrc)));
	}
	catch(const std::exception&)
	{
		return false;
	}
}

// ======================================================================
template<class TD> TD* SafeCast(const IxModule& src)
{
	try
	{
		#if !defined(_MSC_VER)
		if (!IsClassPtr(&src)) return NULL;
		#endif
		return dynamic_cast<TD*>(&const_cast<IxModule&>(src));
	}
	catch(const std::exception&)
	{
		return NULL;
	}
}
// ======================================================================
template<class TD> TD* SafeCast(const IxModule* psrc)
{
	try
	{
		if (psrc == NULL) return NULL;
		#if !defined(_MSC_VER)
		if (!IsClassPtr(psrc)) return NULL;
		#endif
		return dynamic_cast<TD*>(const_cast<IxModule*>(psrc));
	}
	catch(const std::exception&)
	{
		return NULL;
	}
}
// ======================================================================
template<class TD> TD* SafeCast(HxModule hsrc)
{
	try
	{
		if (hsrc == NULL) return NULL;
		#if !defined(_MSC_VER)
		if (!IsClassPtr(hsrc)) return NULL;
		#endif
		return dynamic_cast<TD*>(reinterpret_cast<IxModule*>(hsrc));
	}
	catch(const std::exception&)
	{
		return NULL;
	}
}

}
}

#endif
