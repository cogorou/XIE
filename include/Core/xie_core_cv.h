/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_CORE_CV_H_INCLUDED_
#define _XIE_CORE_CV_H_INCLUDED_

#include "xie_core.h"

// ======================================================================

#if defined(__cplusplus)
namespace xie
{
namespace CVDefs
{
#endif	// __cplusplus

const int CV_CN_MAX		= 512;
const int CV_CN_SHIFT	= 3;
const int CV_DEPTH_MAX	= (1 << CV_CN_SHIFT);

const int CV_8U		= 0;
const int CV_8S		= 1;
const int CV_16U	= 2;
const int CV_16S	= 3;
const int CV_32S	= 4;
const int CV_32F	= 5;
const int CV_64F	= 6;
const int CV_USRTYPE1 = 7;

const int CV_MAT_DEPTH_MASK	= (CV_DEPTH_MAX - 1);
const int CV_MAT_CN_MASK	= ((CV_CN_MAX - 1) << CV_CN_SHIFT);

static inline int CV_MAT_DEPTH(int flag)
{
	return ((flag) & CV_MAT_DEPTH_MASK);
}

static inline int CV_MAT_CN(int flags)
{
	return ((((flags) & CV_MAT_CN_MASK) >> CV_CN_SHIFT) + 1);
}

static inline int CV_MAKETYPE(int depth, int cn)
{
	return (CV_MAT_DEPTH(depth) + (((cn)-1) << CV_CN_SHIFT));
}

#if defined(__cplusplus)
}
}
#endif	// __cplusplus

#endif
