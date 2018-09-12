
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxControlProperty_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;

	try
	{
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
