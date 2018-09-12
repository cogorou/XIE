/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxBitmap.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxBitmap::TxBitmap()
{
	Address = NULL;
	Width = 0;
	Height = 0;
	Model = TxModel::Default();
	Stride = 0;
}

// ================================================================================
TxBitmap::TxBitmap(void* addr, int width, int height, TxModel model, int stride)
{
	Address = addr;
	Width = width;
	Height = height;
	Model = model;
	Stride = stride;
}

// ================================================================================
TxBitmap::operator TxImage() const
{
	return TxImage(TxLayer(Address), Width, Height, Model, 1, Stride, 0); 
}

}	// GDI
}	// xie
