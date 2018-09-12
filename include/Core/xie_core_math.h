/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_CORE_MATH_H_INCLUDED_
#define _XIE_CORE_MATH_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_defs.h"
#include <math.h>

namespace xie
{

// ////////////////////////////////////////////////////////////
// MACRO

// ======================================================================
#define XIE_MIN(left, right)	((left < right) ? left : right)
#define XIE_MAX(left, right)	((left > right) ? left : right)

// ////////////////////////////////////////////////////////////
// FUNCTION

// ======================================================================
static inline unsigned char			_abs(unsigned char value)		{ return value; }
static inline unsigned short		_abs(unsigned short value)		{ return value; }
static inline unsigned int			_abs(unsigned int value)		{ return value; }
static inline unsigned long long	_abs(unsigned long long value)	{ return value; }
static inline short					_abs(short value)				{ return (value < 0) ? -value : value; }
static inline int					_abs(int value)					{ return (value < 0) ? -value : value; }
static inline long long				_abs(long long value)			{ return (value < 0) ? -value : value; }
static inline float					_abs(float value)				{ return (value < 0) ? -value : value; }
static inline double				_abs(double value)				{ return (value < 0) ? -value : value; }

// ======================================================================
static inline unsigned char			_not(unsigned char value)		{ return ~value; }
static inline unsigned short		_not(unsigned short value)		{ return ~value; }
static inline unsigned int			_not(unsigned int value)		{ return ~value; }
static inline unsigned long long	_not(unsigned long long value)	{ return ~value; }
static inline char					_not(char value)				{ return -value; }
static inline short					_not(short value)				{ return -value; }
static inline int					_not(int value)					{ return -value; }
static inline long long				_not(long long value)			{ return -value; }
static inline float					_not(float value)				{ return -value; }
static inline double				_not(double value)				{ return -value; }

// ======================================================================
static inline double _mod(double src, double val)
{
	return src - (trunc(src / val) * val);
}

// ======================================================================
static inline double _modf(double value)
{
	double result = 0;
	return modf(value, &result);
}

// ======================================================================
static inline double _sign(double value)
{
	return (value == 0) ? 0 : (value < 0) ? -1 : +1;
}

// ======================================================================
template<class TD> static inline TD saturate_cast(double value) { return (TD)value; }

#if !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
template<> inline unsigned char			saturate_cast(double value)	{ return (unsigned char)	XIE_MAX(XIE_U08Min, XIE_MIN(XIE_U08Max, value)); }
template<> inline unsigned short		saturate_cast(double value)	{ return (unsigned short)	XIE_MAX(XIE_U16Min, XIE_MIN(XIE_U16Max, value)); }
template<> inline unsigned int			saturate_cast(double value)	{ return (unsigned int)		XIE_MAX(XIE_U32Min, XIE_MIN(XIE_U32Max, value)); }
template<> inline unsigned long long	saturate_cast(double value)	{ return (unsigned long long)value; }

template<> inline char					saturate_cast(double value)	{ return (char)				XIE_MAX(XIE_S08Min, XIE_MIN(XIE_S08Max, value)); }
template<> inline short					saturate_cast(double value)	{ return (short)			XIE_MAX(XIE_S16Min, XIE_MIN(XIE_S16Max, value)); }
template<> inline int					saturate_cast(double value)	{ return (int)				XIE_MAX(XIE_S32Min, XIE_MIN(XIE_S32Max, value)); }
template<> inline long long				saturate_cast(double value)	{ return (long long)value; }

template<> inline float					saturate_cast(double value)	{ return (float)			XIE_MAX(XIE_F32Min, XIE_MIN(XIE_F32Max, value)); }
template<> inline double				saturate_cast(double value)	{ return value; }
#endif

}

#endif
