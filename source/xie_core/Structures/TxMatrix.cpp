/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/TxMatrix.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxMatrix::TxMatrix()
{
	Address	= NULL;
	Rows	= 0;
	Columns	= 0;
	Model	= TxModel::Default();
	Stride	= 0;
}

// ============================================================
TxMatrix::TxMatrix(void* addr, int rows, int cols, TxModel model, int stride)
{
	Address	= addr;
	Rows	= rows;
	Columns	= cols;
	Model	= model;
	Stride	= stride;
}

// ============================================================
bool TxMatrix::operator == (const TxMatrix& cmp) const
{
	const TxMatrix& src = *this;
	if (src.Address		!= cmp.Address) return false;
	if (src.Rows		!= cmp.Rows) return false;
	if (src.Columns		!= cmp.Columns) return false;
	if (src.Model		!= cmp.Model) return false;
	if (src.Stride		!= cmp.Stride) return false;
	return true;
}

// ============================================================
bool TxMatrix::operator != (const TxMatrix& cmp) const
{
	return !(operator == (cmp));
}

}
