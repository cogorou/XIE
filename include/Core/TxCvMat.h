/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCVMAT_H_INCLUDED_
#define _TXCVMAT_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct TxCvMat
{
	int		type;
	int		step;
	int*	refcount;
	int		hdr_refcount;
	void*	ptr;
	int		rows;
	int		cols;

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	static TxCvMat Default();
	TxCvMat();
	TxCvMat(void* addr, int _rows, int _cols, int _type, int _step, int* _refcount = NULL, int _hdr_refcount = 0);
	TxCvMat(TxImage src, int ch);

	bool operator == (const TxCvMat& cmp) const;
	bool operator != (const TxCvMat& cmp) const;

	operator TxImage() const;

	bool IsValid() const;
	void CopyTo(IxModule& dst) const;
	void CopyFrom(const IxModule& src);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
