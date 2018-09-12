/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSTRINGW_H_INCLUDED_
#define _TXSTRINGW_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxStringW
{
	wchar_t*	Address;
	int			Length;

#if defined(__cplusplus)
	static inline TxStringW Default()
	{
		TxStringW result;
		result.Address	= NULL;
		result.Length	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxStringW();
	TxStringW(wchar_t* addr, int length);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
