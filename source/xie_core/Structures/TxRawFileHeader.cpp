/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRawFileHeader.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRawFileHeader::TxRawFileHeader()
{
	Signature = 0;
	Version = 0;
	Revision = 0;
	memset(ClassName, 0, sizeof(ClassName));
	Terminal = 0;
}

// ============================================================
TxRawFileHeader::TxRawFileHeader(TxCharCPtrA name)
{
	Signature = XIE_MODULE_ID;
	Version = XIE_VER;
	Revision = 0;
	memset(ClassName, 0, sizeof(ClassName));
	Terminal = 0;

	if (name != NULL)
	{
		int length = (int)strlen(name);
		if (length > 256)
			length = 256;
		for(int i=0 ; i<length ; i++)
			ClassName[i] = name[i];
	}
}

// ============================================================
bool TxRawFileHeader::operator == (const TxRawFileHeader& cmp) const
{
	const TxRawFileHeader& src = *this;
	if (src.Signature	!= cmp.Signature) return false;
	if (src.Version		!= cmp.Version) return false;
	if (src.Revision	!= cmp.Revision) return false;
	if (src.Terminal	!= cmp.Terminal) return false;
	if (strncmp(src.ClassName, cmp.ClassName, 256) != 0) return false;
	return true;
}

// ============================================================
bool TxRawFileHeader::operator != (const TxRawFileHeader& cmp) const
{
	return !(operator == (cmp));
}

}
