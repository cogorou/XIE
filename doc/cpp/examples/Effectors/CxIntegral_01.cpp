
#include "xie_core.h"
#include <stdio.h>

namespace User
{

void CxIntegral_01_Dump(const xie::CxImage& dst);

// ============================================================
void CxIntegral_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src = xie::CxImage::From<unsigned char>(4, 3,
		{
			1, 1, 1, 1,
			2, 2, 2, 2,
			3, 3, 3, 3,
		}
	);

	// mode=1
	{
		xie::CxImage dst;
		xie::Effectors::CxIntegral effector(1);
		effector.Execute(src, dst);
		// for DEBUG
		printf("mode=1\n");
		CxIntegral_01_Dump(dst);
	}

	// mode=2
	{
		xie::CxImage dst;
		xie::Effectors::CxIntegral effector(2);
		effector.Execute(src, dst);
		// for DEBUG
		printf("mode=2\n");
		CxIntegral_01_Dump(dst);
	}
}

void CxIntegral_01_Dump(const xie::CxImage& dst)
{
	auto scan = dst.Scanner<double>(0);
	for(int y=0 ; y<scan.Height ; y++)
	{
		printf("[%d] ", y);
		for(int x=0 ; x<scan.Width ; x++)
		{
			printf("%3.0f, ", scan(y, x));
		}
		printf("\n");
	}
}

}
