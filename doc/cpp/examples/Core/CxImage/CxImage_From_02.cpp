
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_From_02()
{
	printf("%s\n", __FUNCTION__);

	std::vector<unsigned char> data = 
	{
		0x00, 0x01, 0x02, 0x03,
		0x10, 0x11, 0x12, 0x13,
		0x20, 0x21, 0x22, 0x23,
	};

	auto src = xie::CxImage::From(4, 3, data);

	printf("%-20s = %p\n", "src.Address(0)", src.Address(0));
	printf("%-20s = %d\n", "src.Width", src.Width());
	printf("%-20s = %d\n", "src.Height", src.Height());
	printf("%-20s = %s\n", "src.Model", xie::ToString(src.Model()).Address());
	printf("%-20s = %d\n", "src.Channels", src.Channels());

	auto scan = src.Scanner<unsigned char>(0);
	for(int y=0 ; y<scan.Height ; y++)
	{
		printf("[%d] ", y);
		for(int x=0 ; x<scan.Width ; x++)
			printf("0x%02X ", scan(y, x));
		printf("\n");
	}
}

}
