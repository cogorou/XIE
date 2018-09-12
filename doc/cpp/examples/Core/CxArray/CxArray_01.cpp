
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxArray_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxArray src(10, xie::TxModel::F64(2));
	xie::CxArray dst;
	dst.Resize(10, xie::TxModel::S32(2));

	// make test data
	src.Scanner<xie::TxPointD>().ForEach(
		[](int x, xie::TxPointD* _src)
		{
			_src->X = x;
			_src->Y = x * 2;
		});

	// copy data
	dst.Scanner<xie::TxPointI>().ForEach(
		src.Scanner<xie::TxPointD>(),
		[](int x, xie::TxPointI* _dst, xie::TxPointD* _src)
		{
			_dst->X = static_cast<int>(_src->X);
			_dst->Y = static_cast<int>(_src->Y);
		});

	printf("%-20s = %d\n", "src.Length", src.Length());
	printf("%-20s = %d\n", "src.Model.Type", src.Model().Type);
	printf("%-20s = %d\n", "src.Model.Pack", src.Model().Pack);
	printf("%-20s = %d\n", "dst.Length", dst.Length());
	printf("%-20s = %d\n", "dst.Model.Type", dst.Model().Type);
	printf("%-20s = %d\n", "dst.Model.Pack", dst.Model().Pack);
}

}
