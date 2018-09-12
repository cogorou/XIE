/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"		// Setup/TearDown

void test();

// ============================================================
int main( int argc, char** argv )
{
	xie::Axi::Setup();

	try
	{
		test();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
	
	xie::Axi::TearDown();
	
	return 0;
}
