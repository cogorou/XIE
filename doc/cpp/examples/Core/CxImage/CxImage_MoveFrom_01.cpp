
#include "xie_core.h"
#include <stdio.h>

namespace User
{

static inline xie::CxImage Add(const xie::CxImage& ope1, const xie::CxImage& ope2);

// ============================================================
void CxImage_MoveFrom_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage ope1(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage ope2(32, 24, xie::TxModel::U8(1), 1);
	xie::CxImage dst;

	// Move Operator
	dst = Add(ope1, ope2);

	printf("dst = %p,%d,%d,%s,%d\n",
		dst.Address(0),
		dst.Width(),
		dst.Height(),
		xie::ToString(dst.Model()).Address(),
		dst.Channels()
		);
}

// ============================================================
static inline xie::CxImage Add(const xie::CxImage& ope1, const xie::CxImage& ope2)
{
	xie::CxImage ans;
	ans.Filter().Add(ope1, ope2);

	printf("ans = %p,%d,%d,%s,%d\n",
		ans.Address(0),
		ans.Width(),
		ans.Height(),
		xie::ToString(ans.Model()).Address(),
		ans.Channels()
		);

	return ans;
}

}
