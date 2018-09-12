/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/TxArray.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxArray::TxArray()
{
	Address = NULL;
	Length = 0;
	Model = TxModel::Default();
}

// ============================================================
TxArray::TxArray(void* addr, int length, TxModel model)
{
	Address = addr;
	Length = length;
	Model = model;
}

// ============================================================
bool TxArray::operator == (const TxArray& cmp) const
{
	const TxArray& src = *this;
	if (src.Address	!= cmp.Address) return false;
	if (src.Length	!= cmp.Length) return false;
	if (src.Model	!= cmp.Model) return false;
	return true;
}

// ============================================================
bool TxArray::operator != (const TxArray& cmp) const
{
	return !(operator == (cmp));
}

}
