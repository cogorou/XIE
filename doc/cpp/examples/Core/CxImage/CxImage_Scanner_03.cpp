
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Scanner_03()
{
	printf("%s\n", __FUNCTION__);

	typedef unsigned char	TS;
	int width = 4;
	int height = 3;
	auto model = xie::ModelOf<TS>();
	int channels = 1;

	xie::CxImage src(width, height, model, channels);
	src.Clear(0xFF);

	// copy region
	{
		/*
			   |  0 | [1]|  2 |  3 |
			[0]|0xFF|0xFF|0xFF|0xFF|
			 1 |0xFF|0xFF|0xFF|0xFF|
			 2 |0xFF|0xFF|0xFF|0xFF|
			          Å™
			        |0x7F|+
			        |0x7F|3rows
			        |0x7F|+
				    +1col+
		*/
		src.Scanner<TS>(0, {1, 0, 1, 3}).Copy(
			{
				0x7F,
				0x7F,
				0x7F,
			});
	}

	printf("Data=");
	src.Scanner<TS>(0).ForEach([](int y, int x, TS* _src)
	{
		if (x == 0)
			printf("\n");
		printf("%0X ", *_src);
	});
	printf("\n");
}

}
