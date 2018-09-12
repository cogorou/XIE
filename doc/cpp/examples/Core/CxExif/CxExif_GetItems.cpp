
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxExif_GetItems()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");

	auto exif = xie::CxExif::FromTag(src.Exif());
	auto items = exif.GetItems();

	for(int i=0 ; i<items.Length() ; i++)
	{
		auto item = items[i];

		printf("[%08x] %04x %2d count=%6d value=%12d (%08x)\n",
			item.Offset, item.ID, item.Type, item.Count, item.ValueOrIndex, item.ValueOrIndex);
	}
}

}
