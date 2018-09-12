/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxFrameIndex.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxFrameIndex::TxFrameIndex()
{
	Track		= 0;
	Frame		= 0;
	Flag		= 0;
	Progress	= 0;
	TimeStamp	= 0;
}

// ============================================================
TxFrameIndex::TxFrameIndex(int track, int frame, int flag, double progress, unsigned long long timestamp)
{
	Track		= track;
	Frame		= frame;
	Flag		= flag;
	Progress	= progress;
	TimeStamp	= timestamp;
}

// ============================================================
bool TxFrameIndex::operator == (const TxFrameIndex& cmp) const
{
	const TxFrameIndex& src = *this;
	if (src.Track		!= cmp.Track) return false;
	if (src.Frame		!= cmp.Frame) return false;
	if (src.Flag		!= cmp.Flag) return false;
	if (src.Progress	!= cmp.Progress) return false;
	if (src.TimeStamp	!= cmp.TimeStamp) return false;
	return true;
}

// ============================================================
bool TxFrameIndex::operator != (const TxFrameIndex& cmp) const
{
	return !(operator == (cmp));
}

}
