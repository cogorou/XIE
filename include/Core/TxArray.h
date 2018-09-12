/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXARRAY_H_INCLUDED_
#define _TXARRAY_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxArray
{
	void*		Address;
	int			Length;
	TxModel		Model;

#if defined(__cplusplus)
	static inline TxArray Default()
	{
		TxArray result;
		result.Address = NULL;
		result.Length = 0;
		result.Model = TxModel::Default();
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxArray();
	TxArray(void* addr, int length, TxModel model);

	bool operator == (const TxArray& cmp) const;
	bool operator != (const TxArray& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
