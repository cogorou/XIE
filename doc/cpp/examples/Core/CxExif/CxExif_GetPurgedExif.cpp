
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxExif_GetPurgedExif()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");
	xie::TxRectangleI roi(10, 10, 280, 180);

	xie::CxImage dst = src.Child(roi).Clone();

	auto src_exif = xie::CxExif::FromTag(src.Exif());
	auto dst_exif = src_exif.GetPurgedExif();

	dst.Exif( dst_exif.Tag() );
}

}
