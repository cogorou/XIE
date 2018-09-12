
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxException_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		// (1) 
		throw xie::CxException(xie::ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
		// (2) 
	catch(const xie::CxException& ex)
	{
		// (3) 
		printf("%s(%d): Code=%d, Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
