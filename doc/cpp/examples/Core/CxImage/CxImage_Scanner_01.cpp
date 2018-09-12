
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Scanner_01()
{
	printf("%s\n", __FUNCTION__);

	auto src = xie::CxImage::From(3, 2,
	{
		0.0, 0.1, 0.2,
		1.0, 1.1, 1.2,
	});

	auto scan = src.Scanner<double>(0);
	scan.ForEach(
		[](int y, int x, double* _src)
		{
			printf("%d,%d: %f\n", y, x, *_src);
		});
}

}
