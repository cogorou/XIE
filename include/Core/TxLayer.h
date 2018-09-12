/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXLAYER_H_INCLUDED_
#define _TXLAYER_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxLayer
{
	void*	Address0;
	void*	Address1;
	void*	Address2;
	void*	Address3;
	void*	Address4;
	void*	Address5;
	void*	Address6;
	void*	Address7;
	void*	Address8;
	void*	Address9;
	void*	Address10;
	void*	Address11;
	void*	Address12;
	void*	Address13;
	void*	Address14;
	void*	Address15;

#if defined(__cplusplus)
	static inline TxLayer Default()
	{
		TxLayer result;
		result.Address0 = NULL;
		result.Address1 = NULL;
		result.Address2 = NULL;
		result.Address3 = NULL;
		result.Address4 = NULL;
		result.Address5 = NULL;
		result.Address6 = NULL;
		result.Address7 = NULL;
		result.Address8 = NULL;
		result.Address9 = NULL;
		result.Address10 = NULL;
		result.Address11 = NULL;
		result.Address12 = NULL;
		result.Address13 = NULL;
		result.Address14 = NULL;
		result.Address15 = NULL;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxLayer();
	TxLayer(void* addr);
	TxLayer(void** addrs, int count);

	bool operator == (const TxLayer& cmp) const;
	bool operator != (const TxLayer& cmp) const;

	void*& operator [] (int index);
	void*  operator [] (int index) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
