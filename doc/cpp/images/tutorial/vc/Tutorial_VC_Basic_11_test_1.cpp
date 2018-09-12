
#include "stdafx.h"
#include "xie_core.h"
#include "xie.h"

int _tmain(int argc, _TCHAR* argv[])
{
	xie::Axi::Setup();

	::CreateDirectoryA( "Results", NULL );

	try
	{
		xie::CxImage src("TestFiles/color_grad.bmp");
		xie::CxImage dst;
		xie::Effectors::CxRgbToGray effector(0.299, 0.587, 0.114);
		effector.Execute(src, dst);
		dst.Save("Results/result.bmp");
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

