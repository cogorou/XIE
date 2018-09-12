/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxLayer.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxLayer::TxLayer()
{
	for(int i=0 ; i<XIE_IMAGE_CHANNELS_MAX ; i++)
		(*this)[i] = NULL;
}

// ============================================================
TxLayer::TxLayer(void* addr)
{
	for(int i=0 ; i<XIE_IMAGE_CHANNELS_MAX ; i++)
		(*this)[i] = NULL;
	(*this)[0] = addr;
}

// ============================================================
TxLayer::TxLayer(void** addrs, int count)
{
	if (!(0 <= count && count <= XIE_IMAGE_CHANNELS_MAX))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	for(int i=0 ; i<XIE_IMAGE_CHANNELS_MAX ; i++)
		(*this)[i] = NULL;
	if (addrs != NULL)
	{
		for(int i=0 ; i<count ; i++)
			(*this)[i] = addrs[i];
	}
}

// ============================================================
bool TxLayer::operator == (const TxLayer& cmp) const
{
	const TxLayer& src = *this;
	for(int i=0 ; i<XIE_IMAGE_CHANNELS_MAX ; i++)
	{
		if (src[i] != cmp[i]) return false;
	}
	return true;
}

// ============================================================
bool TxLayer::operator != (const TxLayer& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
void*& TxLayer::operator [] (int index)
{
	switch(index)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case  0: return Address0;
	case  1: return Address1;
	case  2: return Address2;
	case  3: return Address3;
	case  4: return Address4;
	case  5: return Address5;
	case  6: return Address6;
	case  7: return Address7;
	case  8: return Address8;
	case  9: return Address9;
	case 10: return Address10;
	case 11: return Address11;
	case 12: return Address12;
	case 13: return Address13;
	case 14: return Address14;
	case 15: return Address15;
	}
}

// ============================================================
void* TxLayer::operator [] (int index) const
{
	return (*const_cast<TxLayer*>(this))[index];
}

}
