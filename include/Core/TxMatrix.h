/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXMATRIX_H_INCLUDED_
#define _TXMATRIX_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxMatrix
{
	void*		Address;
	int			Rows;
	int			Columns;
	TxModel		Model;
	int			Stride;

#if defined(__cplusplus)
	static inline TxMatrix Default()
	{
		TxMatrix result;
		result.Address	= NULL;
		result.Rows		= 0;
		result.Columns	= 0;
		result.Model	= TxModel::Default();
		result.Stride	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxMatrix();
	TxMatrix(void* addr, int rows, int cols, TxModel model, int stride);

	bool operator == (const TxMatrix& cmp) const;
	bool operator != (const TxMatrix& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
