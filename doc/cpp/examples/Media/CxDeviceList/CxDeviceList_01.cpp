
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxDeviceList_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::Media::CxDeviceList devices;
		printf("%s: Video Input Device\n", __FUNCTION__);
		devices.Setup(xie::Media::ExMediaType::Video, xie::Media::ExMediaDir::Input);
		int length = devices.Length();
		printf("%-20s : %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = devices[i];
			printf("|- %d\n", i);
			printf("|  |- %-20s : \"%s\" (%d)\n", "Name(Index)", item.Name(), item.Index());
			printf("|  |- %-20s : \"%s\" \n", "ProductName", item.GetProductName().Address());

			auto pins = item.GetPinNames();
			printf("|  |- %-20s : %d\n", "PinNames", pins.Length());
			for(int s=0 ; s<pins.Length() ; s++)
			{
				printf("|  |  |- %2d: \"%s\"\n", s, pins[s].Address());
			}

			auto sizes = item.GetFrameSizes();
			printf("|  |- %-20s : %d\n", "FrameSizes", sizes.Length());
			for(int s=0 ; s<sizes.Length() ; s++)
			{
				printf("|  |  |- %2d: %4d x %4d\n", s, sizes[s].Width, sizes[s].Height);
			}
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
