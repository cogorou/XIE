
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_ctor_03()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src1("images/src/color_grad.png", true);
	xie::CxImage src2("images/src/color_grad.png", false);

	printf("src1 = %p,%d,%d,%s,%d\n",
		src1.Address(0),
		src1.Width(),
		src1.Height(),
		xie::ToString(src1.Model()).Address(),
		src1.Channels()
		);

	printf("src2 = %p,%d,%d,%s,%d\n",
		src2.Address(0),
		src2.Width(),
		src2.Height(),
		xie::ToString(src2.Model()).Address(),
		src2.Channels()
		);
}

}
