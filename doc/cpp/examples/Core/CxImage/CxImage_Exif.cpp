
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_Exif()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");
	xie::CxImage dst = src.Clone();

	// Copy
	dst.Exif( src.Exif() );

	// Erase
	dst.Exif( xie::TxExif::Default() );
}

}
