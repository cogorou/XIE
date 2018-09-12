
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Attach_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src(4, 3, xie::TxModel::U8(1), 3);
	xie::CxImage child;
	child.Attach(src);

	printf("%-5s = %p,%d,%d,%s,%d,%d,%s\n", "src",
		src.Address(0),
		src.Width(),
		src.Height(),
		xie::ToString(src.Model()).Address(),
		src.Channels(),
		src.Stride(),
		(src.IsAttached() ? "true" : "false")
		);

	printf("%-5s = %p,%d,%d,%s,%d,%d,%s\n", "child",
		child.Address(0),
		child.Width(),
		child.Height(),
		xie::ToString(child.Model()).Address(),
		child.Channels(),
		child.Stride(),
		(child.IsAttached() ? "true" : "false")
		);
}

}
