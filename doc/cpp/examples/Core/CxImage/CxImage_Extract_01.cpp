
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Extract_01()
{
	printf("%s\n", __FUNCTION__);

	typedef double	TE;
	int width = 4;
	int height = 3;
	auto model = xie::ModelOf<TE>();
	int channels = 1;

	auto src = xie::CxImage::From<TE>(width, height,
		{
			10.0, 20.0, 30.0, 40.0,
			10.1, 20.1, 30.1, 40.1,
			10.2, 20.2, 30.2, 40.2,
		});

	// extract X direction
	{
		/*
			   |  0 | [1]|  2 |  3 |   + 3cols        +
			[0]|10.0|20.0|30.0|40.0| Å® |20.0|30.0|40.0| 1row
			 1 |10.1|20.1|30.1|40.1|
			 2 |10.2|20.2|30.2|40.2|
		*/
		auto dst = src.Extract(0, 0, 1, 3, xie::ExScanDir::X);
		printf("ExScanDir::X=");
		dst.Scanner<TE>().ForEach([](int i, TE* _dst)
		{
			printf("%4.1f ", *_dst);
		});
		printf("\n");
	}

	// extract Y direction
	{
		/*
			   |  0 | [1]|  2 |  3 |
			[0]|10.0|20.0|30.0|40.0|
			 1 |10.1|20.1|30.1|40.1|
			 2 |10.2|20.2|30.2|40.2|
			          Å´
			        |20.0|+
			        |20.1|3rows
			        |20.2|+
				    +1col+
		*/
		auto dst = src.Extract(0, 0, 1, 3, xie::ExScanDir::Y);
		printf("ExScanDir::Y=");
		dst.Scanner<TE>().ForEach([](int i, TE* _dst)
		{
			printf("%4.1f ", *_dst);
		});
		printf("\n");
	}
}

}
