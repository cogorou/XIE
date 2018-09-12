/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxStringW.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxStringW::TxStringW()
{
	Address = NULL;
	Length = 0;
}

// ============================================================
TxStringW::TxStringW(wchar_t* addr, int length)
{
	Address = addr;
	Length = length;
}

}
