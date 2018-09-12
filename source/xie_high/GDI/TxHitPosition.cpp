/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/TxHitPosition.h"

namespace xie
{
namespace GDI
{

// ============================================================
TxHitPosition::TxHitPosition()
{
	Mode	= 0;
	Index	= 0;
	Site	= 0;
}

// ============================================================
TxHitPosition::TxHitPosition(int mode, int index, int site)
{
	Mode	= mode;
	Index	= index;
	Site	= site;
}

// ============================================================
bool TxHitPosition::operator == (const TxHitPosition& cmp) const
{
	const TxHitPosition& src = *this;
	if (src.Mode	!= cmp.Mode) return false;
	if (src.Index	!= cmp.Index) return false;
	if (src.Site	!= cmp.Site) return false;
	return true;
}

// ============================================================
bool TxHitPosition::operator != (const TxHitPosition& cmp) const
{
	return !(operator == (cmp));
}

}
}
