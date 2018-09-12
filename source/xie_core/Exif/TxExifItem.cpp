/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxExifItem.h"
#include "Core/CxException.h"

namespace xie
{

// ======================================================================
TxExifItem::TxExifItem()
{
	Offset			= 0;
	EndianType		= ExEndianType::None;
	ID				= 0;
	Type			= 0;
	Count			= 0;
	ValueOrIndex	= 0;
}

// ======================================================================
TxExifItem::TxExifItem(int offset, ExEndianType endian_type, unsigned short id, short type, int count, int value)
{
	Offset			= offset;
	EndianType		= endian_type;
	ID				= id;
	Type			= type;
	Count			= count;
	ValueOrIndex	= value;
}

// ============================================================
bool TxExifItem::operator == ( const TxExifItem& cmp ) const
{
	const TxExifItem& dst = *this;

	if (dst.Offset			!= cmp.Offset) return false;
	if (dst.EndianType		!= cmp.EndianType) return false;
	if (dst.ID				!= cmp.ID) return false;
	if (dst.Type			!= cmp.Type) return false;
	if (dst.Count			!= cmp.Count) return false;
	if (dst.ValueOrIndex	!= cmp.ValueOrIndex) return false;

	return true;
}

// ============================================================
bool TxExifItem::operator != ( const TxExifItem& cmp ) const
{
	return !(operator == (cmp));
}

}
