
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxExif_EndianType()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");

	auto exif = xie::CxExif::FromTag(src.Exif());
	switch(exif.EndianType())
	{
	case xie::ExEndianType::LE:
		printf("Little Endian\n");
		break;
	case xie::ExEndianType::BE:
		printf("Big Endian\n");
		break;
	default:
	case xie::ExEndianType::None:
		printf("None\n");
		break;
	}
}

}
