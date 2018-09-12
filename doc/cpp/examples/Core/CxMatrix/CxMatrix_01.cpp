
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxMatrix_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxMatrix src(4, 3);
	xie::CxMatrix dst;
	dst.Resize(4, 3, xie::TxModel::F32(1));

	int cols = src.Columns();

	// make test data
	src.Scanner<double>().ForEach(
		[cols](int y, int x, double* _src)
		{
			*_src = y * cols + x;
		});

	// copy data
	dst.Scanner<float>().ForEach(
		src.Scanner<double>(),
		[](int y, int x, float* _dst, double* _src)
		{
			*_dst = static_cast<float>(*_src);
		});

	printf("%-20s = %d\n", "src.Columns", src.Columns());
	printf("%-20s = %d\n", "src.Rows", src.Rows());
	printf("%-20s = %d\n", "src.Model.Type", src.Model().Type);
	printf("%-20s = %d\n", "src.Model.Pack", src.Model().Pack);
	printf("%-20s = %d\n", "dst.Columns", dst.Columns());
	printf("%-20s = %d\n", "dst.Rows", dst.Rows());
	printf("%-20s = %d\n", "dst.Model.Type", dst.Model().Type);
	printf("%-20s = %d\n", "dst.Model.Pack", dst.Model().Pack);
}

}
