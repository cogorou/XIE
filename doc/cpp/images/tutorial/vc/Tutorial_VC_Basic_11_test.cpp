
#include "stdafx.h"
#include "xie_core.h"		// (1)
#include "xie.h"			// (1)

int _tmain(int argc, _TCHAR* argv[])
{
	xie::Axi::Setup();		// (2)

	// 
	// implementation
	// 

	xie::Axi::TearDown();	// (3)

	return 0;
}

