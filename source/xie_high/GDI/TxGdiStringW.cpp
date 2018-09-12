/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxGdiStringW.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxGdiStringW::TxGdiStringW()
{
	Address = NULL;
	Length = 0;
	X = 0;
	Y = 0;
	FontName = NULL;
	FontSize = 12;
	Align = ExGdiTextAlign::TopLeft;
	CodePage = 0;
	Param = TxGdi2dParam::Default();
}

}	// GDI
}	// xie
