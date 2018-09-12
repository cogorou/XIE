
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_ClearEx_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src(3, 2, xie::TxModel::F64(2), 1);
	src.Reset();
	src.ClearEx(2.2, 1, 1);	// 2nd field

	printf("src=");
	src.Scanner<xie::TxPointD>(0).ForEach(
		[](int y, int x, xie::TxPointD* _src)
		{
			printf("{%3.1f,%3.1f} ", _src->X, _src->Y);
		});
	printf("\n");
}

}
