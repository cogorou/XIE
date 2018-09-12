
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxScreenList_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::Media::CxScreenList list;
		list.Setup();
		int length = list.Length();
		printf("%s = %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = list[i];
			printf("%2d: %p, %-20s, %d,%d-%d,%d\n"
				, i
				, item.Handle()
				, item.Name()
				, item.Bounds().X
				, item.Bounds().Y
				, item.Bounds().Width
				, item.Bounds().Height
				);
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
