
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_ExifCopy()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");
	xie::CxImage dst;

	// filter
	dst.Filter().Scale(src, 0.5, 0.5, 2);

	// Copy
	dst.ExifCopy( src.Exif() );
}

}
