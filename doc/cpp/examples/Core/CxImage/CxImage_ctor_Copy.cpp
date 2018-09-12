
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_ctor_Copy()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage ope1(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage ope2(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage ans;
	ans.Filter().Add(ope1, ope2);

	// Copy Constructor
	xie::CxImage dst = ans;

	printf("ans = %p,%d,%d,%s,%d\n",
		ans.Address(0),
		ans.Width(),
		ans.Height(),
		xie::ToString(ans.Model()).Address(),
		ans.Channels()
		);
	printf("dst = %p,%d,%d,%s,%d\n",
		dst.Address(0),
		dst.Width(),
		dst.Height(),
		xie::ToString(dst.Model()).Address(),
		dst.Channels()
		);
}

}
