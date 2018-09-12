/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"
#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"		// Setup/TearDown

int _tmain(int argc, _TCHAR* argv[])
{
	xie::Axi::Setup();

	#if defined(_MSC_VER)
	::CreateDirectoryA( "Results", NULL );
	#endif

	try
	{
		xie::CxImage src("TestFiles/color_grad.bmp");
		xie::CxImage dst;
		xie::Effectors::CxRgbToGray effector(0.299, 0.587, 0.114);
		effector.Execute(src, dst);
		dst.Save("Results/result.png");
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

