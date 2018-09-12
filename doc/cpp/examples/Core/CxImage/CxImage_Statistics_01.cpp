
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Statistics_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/starflower_320x240.jpg");

	int pxc = src.Model().Pack * src.Channels();
	for(int ch=0 ; ch<pxc ; ch++)
	{
		xie::TxStatistics stat = src.Statistics(ch);

		printf("%d/%d\n", ch+1, pxc);
		printf("%-20s = %7.3f\n", "dst.Max", stat.Max);
		printf("%-20s = %7.3f\n", "dst.Min", stat.Min);
		printf("%-20s = %7.3f\n", "dst.Mean", stat.Mean());
		printf("%-20s = %7.3f\n", "dst.Sigma", stat.Sigma());
	}
}

}
