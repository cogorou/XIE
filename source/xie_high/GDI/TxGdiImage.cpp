/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxGdiImage.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxGdiImage::TxGdiImage()
{
	Address = NULL;
	Width = 0;
	Height = 0;
	Model = TxModel::Default();
	Stride = 0;
	X = 0;
	Y = 0;
	MagX = 1.0;
	MagY = 1.0;
	Alpha = 1.0;
	AlphaFormat = ExBoolean::False;
	Param = TxGdi2dParam::Default();
}

// ================================================================================
TxGdiImage::operator TxImage() const
{
	return TxImage(TxLayer(Address), Width, Height, Model, 1, Stride, 0); 
}

}	// GDI
}	// xie
