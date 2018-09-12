/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXHITPOSITION_H_INCLUDED_
#define _TXHITPOSITION_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxHitPosition
{
	int		Mode;
	int		Index;
	int		Site;

#if defined(__cplusplus)
	static inline TxHitPosition Default()
	{
		TxHitPosition result;
		result.Mode		= 0;
		result.Index	= 0;
		result.Site		= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxHitPosition();
	TxHitPosition(int mode, int index, int site);

	bool operator == (const TxHitPosition& cmp) const;
	bool operator != (const TxHitPosition& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
	template<> inline TxModel ModelOf<xie::GDI::TxHitPosition>()
	{
		return TxModel(ExType::S32, 3);
	}
}	// xie
#endif	// __cplusplus

#endif
