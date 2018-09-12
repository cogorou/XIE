
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Clear_01()
{
	printf("%s\n", __FUNCTION__);

	// -----
	xie::CxImage src1(3, 2, xie::TxModel::F64(1), 1);
	src1.Clear(1.1);

	printf("src1=");
	src1.Scanner<double>(0).ForEach(
		[](int y, int x, double* _src)
		{
			printf("%3.1f ", *_src);
		});
	printf("\n");

	// -----
	xie::CxImage src2(3, 2, xie::TxModel::F64(2), 1);
	src2.Clear(xie::TxPointD(1.1, 2.2));

	printf("src2=");
	src2.Scanner<xie::TxPointD>(0).ForEach(
		[](int y, int x, xie::TxPointD* _src)
		{
			printf("{%3.1f,%3.1f} ", _src->X, _src->Y);
		});
	printf("\n");
}

}
