
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_CopyFrom_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage dst;
	xie::CxImage tmp;
	xie::CxImage ope1(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage ope2(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage ans;

	ans.Filter().Add(ope1, ope2);
	printf("ans = %p,%d,%d,%s,%d\n",
		ans.Address(0),
		ans.Width(),
		ans.Height(),
		xie::ToString(ans.Model()).Address(),
		ans.Channels()
		);

	// Copy Operator
	dst = ans;

	printf("dst = %p,%d,%d,%s,%d\n",
		dst.Address(0),
		dst.Width(),
		dst.Height(),
		xie::ToString(dst.Model()).Address(),
		dst.Channels()
		);

	// CopyFrom Method
	tmp.CopyFrom(dst);

	printf("tmp = %p,%d,%d,%s,%d\n",
		tmp.Address(0),
		tmp.Width(),
		tmp.Height(),
		xie::ToString(tmp.Model()).Address(),
		tmp.Channels()
		);
}

}
