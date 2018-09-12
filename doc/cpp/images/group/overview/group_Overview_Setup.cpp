/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"
#include "xie_core.h"	// (1) 
#include "xie_high.h"	// (1) 
#include "xie.h"		// (2) Setup/TearDown

// ============================================================
int main(int argc, char* argv[])
{
	xie::Axi::Setup();		// (3)

	try
	{
		// your code		// (4)
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();	// (5)

	return 0;
}
