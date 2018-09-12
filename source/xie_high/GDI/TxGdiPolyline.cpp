/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxGdiPolyline.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxGdiPolyline::TxGdiPolyline()
{
	Address = NULL;
	Length = 0;
	Model = TxModel::Default();
	Closed = ExBoolean::False;
	Param = TxGdi2dParam::Default();
}

// ================================================================================
TxGdiPolyline::TxGdiPolyline(void* addr, int length, TxModel model, ExBoolean closed)
{
	Address = addr;
	Length = length;
	Model = model;
	Closed = closed;
	Param = TxGdi2dParam::Default();
}

// ================================================================================
TxGdiPolyline::operator TxArray() const
{
	return TxArray(Address, Length, Model); 
}

}	// GDI
}	// xie
