
#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

void test();		// ※注1) ウィンドウ生成処理.

// ============================================================
int main( int argc, char** argv )
{
	xie::Axi::Setup();

	try
	{
		test();		// ※注1) ウィンドウ生成処理.
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
	
	xie::Axi::TearDown();
	
	return 0;
}
