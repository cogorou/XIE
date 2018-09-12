/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxExif.h"

namespace xie
{

// ============================================================
TxExif::TxExif()
{
	Address = NULL;
	Length = 0;
	Model = TxModel::Default();
}

// ============================================================
TxExif::TxExif(void* addr, int length, TxModel model)
{
	Address = addr;
	Length = length;
	Model = model;
}

// ============================================================
bool TxExif::operator == (const TxExif& cmp) const
{
	const TxExif& src = *this;
	if (src.Address	!= cmp.Address) return false;
	if (src.Length	!= cmp.Length) return false;
	if (src.Model	!= cmp.Model) return false;
	return true;
}

// ============================================================
bool TxExif::operator != (const TxExif& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
bool TxExif::IsValid() const
{
	if (CheckValidity(Address, Length) == false) return false;
	if (Model != TxModel::U8(1)) return false;
	return true;
}

// ============================================================
bool TxExif::CheckValidity(const void* addr, int length)
{
	if (addr == NULL) return false;
	if (length < 16) return false;
	if (memcmp(addr, "Exif\0\0", 6) != 0) return false;
	return true;
}

}
