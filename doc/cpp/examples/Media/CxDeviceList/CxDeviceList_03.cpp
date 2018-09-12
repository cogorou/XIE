
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxDeviceList_03()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::Media::CxDeviceList devices;
		printf("%s: Audio Output Device\n", __FUNCTION__);
		devices.Setup(xie::Media::ExMediaType::Audio, xie::Media::ExMediaDir::Output);
		int length = devices.Length();
		printf("%-20s : %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = devices[i];
			printf("|- %d\n", i);
			printf("|  |- %-20s : \"%s\" (%d)\n", "Name(Index)", item.Name(), item.Index());
			printf("|  |- %-20s : \"%s\" \n", "ProductName", item.GetProductName().Address());
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
