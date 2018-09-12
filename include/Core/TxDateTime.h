/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXDATETIME_H_INCLUDED_
#define _TXDATETIME_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxDateTime
{
	int		Year;
	int		Month;
	int		Day;
	int		Hour;
	int		Minute;
	int		Second;
	int		Millisecond;

#if defined(__cplusplus)
	static inline TxDateTime Default()
	{
		TxDateTime result;
		result.Year		= 0;
		result.Month	= 0;
		result.Day		= 0;
		result.Hour		= 0;
		result.Minute	= 0;
		result.Second	= 0;
		result.Millisecond	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	static TxDateTime Now(bool ltc);
	static TxDateTime FromBinary(unsigned long long src, bool ltc);

	TxDateTime();
	TxDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond);

	#if defined(_MSC_VER)
	TxDateTime(const SYSTEMTIME& src);
	#else
	TxDateTime(const tm& src, int msec = 0);
	#endif

	bool operator == (const TxDateTime& cmp) const;
	bool operator != (const TxDateTime& cmp) const;

	#if defined(_MSC_VER)
	TxDateTime& operator = (const SYSTEMTIME& src);
	operator SYSTEMTIME () const;
	#else
	TxDateTime& operator = (const tm& src);
	operator tm () const;
	#endif

	unsigned long long ToBinary(bool ltc) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
