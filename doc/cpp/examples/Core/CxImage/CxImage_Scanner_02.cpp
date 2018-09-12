
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Scanner_02()
{
	printf("%s\n", __FUNCTION__);

	typedef xie::TxPointD	TS;
	typedef double			TD;

	xie::CxImage src(3, 2, xie::ModelOf<TS>(), 1);
	xie::CxImage dst(3, 2, xie::ModelOf<TD>(), 2);

	// make test data
	src.Scanner<TS>(0).ForEach(
		[](int y, int x, TS* _src)
		{
			_src->X = y * 10 + x * 1;
			_src->Y = y * 20 + x * 2;
		});

	// copy data
	src.Scanner<TS>(0).ForEach(
		dst.Scanner<TD>(0),
		dst.Scanner<TD>(1),
		[](int y, int x, TS* _src, TD* _dst0, TD* _dst1)
		{
			*_dst0 = static_cast<TD>(_src->X);
			*_dst1 = static_cast<TD>(_src->Y);

			printf("%d,%d: %f,%f\n", y, x, _src->X, _src->Y);
		});
}

}
