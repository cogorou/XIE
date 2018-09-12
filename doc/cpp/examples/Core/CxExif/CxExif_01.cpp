
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxExif_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/bird_320x240.jpg");
	xie::CxImage dst = src.Clone();

	auto src_exif = xie::CxExif::FromTag(src.Exif());
	auto dst_exif = src_exif.GetPurgedExif();

	dst.Exif( dst_exif.Tag() );
}

}
