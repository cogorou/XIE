
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Resize_02()
{
	printf("%s\n", __FUNCTION__);

	xie::TxImageSize image_size(30, 24, xie::TxModel::U8(3), 1);
	xie::CxImage src;
	
	src.Resize(image_size);
	printf("%-20s = %p\n", "src.Address(0)", src.Address(0));
	printf("%-20s = %d\n", "src.Width", src.Width());
	printf("%-20s = %d\n", "src.Height", src.Height());
	printf("%-20s = %s\n", "src.Model", xie::ToString(src.Model()).Address());
	printf("%-20s = %d\n", "src.Channels", src.Channels());
	printf("%-20s = %d\n", "src.Stride", src.Stride());
}

}
