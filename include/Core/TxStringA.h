/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSTRINGA_H_INCLUDED_
#define _TXSTRINGA_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxStringA
{
	char*		Address;
	int			Length;

#if defined(__cplusplus)
	static inline TxStringA Default()
	{
		TxStringA result;
		result.Address	= NULL;
		result.Length	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxStringA();
	TxStringA(char* addr, int length);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
