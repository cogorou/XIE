
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxExif_SetValue()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");
	xie::CxImage dst;

	dst.Filter().Scale(src, 0.5, 0.5, 2);

	auto src_exif = xie::CxExif::FromTag(src.Exif());
	auto dst_exif = src_exif.GetPurgedExif();

	auto items = dst_exif.GetItems();

	for(int k=0 ; k<items.Length() ; k++)
	{
		auto item = items[k];

		switch (item.ID)
		{
			case 0x0132:	// File change date and time
				if (item.Type == 2 && item.Count >= 19)
				{
					auto now = xie::TxDateTime::Now(true);
					auto value = xie::CxString::Format(
						"%04d:%02d:%02d %02d:%02d:%02d",
						now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
					dst_exif.SetValue(item, value);
				}
				break;
			case 0xA002:	// Valid image width
				if (item.Type == 3 && item.Count == 1)
				{
					typedef unsigned short TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)dst.Width();
					dst_exif.SetValue(item, value);
				}
				else if (item.Type == 4 && item.Count == 1)
				{
					typedef unsigned int TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)dst.Width();
					dst_exif.SetValue(item, value);
				}
				break;
			case 0xA003:	// Valid image height
				if (item.Type == 3 && item.Count == 1)
				{
					typedef unsigned short TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)dst.Height();
					dst_exif.SetValue(item, value);
				}
				else if (item.Type == 4 && item.Count == 1)
				{
					typedef unsigned int TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)dst.Height();
					dst_exif.SetValue(item, value);
				}
				break;
		}
	}


	dst.Exif( dst_exif.Tag() );
}

}
