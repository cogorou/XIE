/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxStringA.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxStringA::TxStringA()
{
	Address = NULL;
	Length = 0;
}

// ============================================================
TxStringA::TxStringA(char* addr, int length)
{
	Address = addr;
	Length = length;
}

}
