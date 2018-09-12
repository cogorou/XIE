
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src1(10, 6, xie::TxModel::U8(1), 1);
	xie::CxImage src2(10, 6, xie::TxModel::U8(1), 3);
	xie::CxImage src3(10, 6, xie::TxModel::U8(3), 1);

	printf("src1 = %d,%d,%s,%d,%d\n",
		src1.Width(),
		src1.Height(),
		xie::ToString(src1.Model()).Address(),
		src1.Channels(),
		src1.Stride()
		);

	printf("src2 = %d,%d,%s,%d,%d\n",
		src2.Width(),
		src2.Height(),
		xie::ToString(src2.Model()).Address(),
		src2.Channels(),
		src2.Stride()
		);

	printf("src3 = %d,%d,%s,%d,%d\n",
		src3.Width(),
		src3.Height(),
		xie::ToString(src3.Model()).Address(),
		src3.Channels(),
		src3.Stride()
		);
}

}
