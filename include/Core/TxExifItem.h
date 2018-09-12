/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXEXIFITEM_H_INCLUDED_
#define _TXEXIFITEM_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

// ======================================================================
struct XIE_EXPORT_CLASS TxExifItem
{
	int				Offset;
	ExEndianType	EndianType;
	unsigned short	ID;
	short			Type;
	int				Count;
	int				ValueOrIndex;

#if defined(__cplusplus)
	static inline TxExifItem Default()
	{
		TxExifItem result;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxExifItem();
	TxExifItem(int offset, ExEndianType endian_type, unsigned short id, short type, int count, int value);

	bool operator == ( const TxExifItem& cmp ) const;
	bool operator != ( const TxExifItem& cmp ) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
