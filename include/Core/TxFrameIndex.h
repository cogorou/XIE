/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXFRAMEINDEX_H_INCLUDED_
#define _TXFRAMEINDEX_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxFrameIndex
{
	int					Track;
	int					Frame;
	int					Flag;
	double				Progress;
	unsigned long long	TimeStamp;

#if defined(__cplusplus)
	static inline TxFrameIndex Default()
	{
		TxFrameIndex result;
		result.Track	= 0;
		result.Frame	= 0;
		result.Flag		= 0;
		result.Progress	= 0;
		result.TimeStamp	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxFrameIndex();
	TxFrameIndex(int track, int frame, int flag, double progress, unsigned long long timestamp);

	bool operator == (const TxFrameIndex& cmp) const;
	bool operator != (const TxFrameIndex& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
