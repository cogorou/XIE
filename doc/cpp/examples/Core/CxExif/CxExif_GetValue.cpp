
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
template<class TE> xie::CxString format(xie::TxScanner1D<TE>& scan)
{
	xie::CxString strValue;
	if (scan.Model.Pack == 1)
	{
		scan.ForEach([&strValue](int x, TE* addr)
		{
			if (x == 0)
				strValue += xie::CxString::Format("%d", *addr);
			else
				strValue += xie::CxString::Format(",%d", *addr);
		});
	}
	else if (scan.Model.Pack == 2)
	{
		scan.ForEach([&strValue](int x, TE* addr)
		{
			if (x == 0)
				strValue += xie::CxString::Format("%d/%d", addr[0], addr[1]);
			else
				strValue += xie::CxString::Format(",%d/%d", addr[0], addr[1]);
		});
	}
	return strValue;
}

// ============================================================
void CxExif_GetValue()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/fern_300x200.jpg");

	auto exif = xie::CxExif::FromTag(src.Exif());
	auto items = exif.GetItems();

	for(int k=0 ; k<items.Length() ; k++)
	{
		auto item = items[k];
		xie::CxString strValue = "";

		switch(item.Type)
		{
		case 1:		// BYTE
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned char>());
			}
			break;
		case 7:		// UNDEFINED
			switch(item.ID)
			{
			case 0x9000:	// Exif version
			case 0xA000:	// Supported FlashPix version
				{
					xie::CxArray value;
					exif.GetValue(item, value);
					auto scan = value.Scanner<unsigned char>();
					if (scan.Length == 4)
						strValue = xie::CxString::Format("%c%c%c%c", scan[0], scan[1], scan[2], scan[3]);
				}
				break;
			}
			break;
		case 2:		// ASCII
			{
				xie::CxStringA value;
				exif.GetValue(item, value);
				strValue = value;
			}
			break;
		case 3:		// SHORT
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned short>());
			}
			break;
		case 4:		// LONG
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned int>());
			}
			break;
		case 9:		// SLONG 32bit
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<int>());
			}
			break;
		case 5:		// RATIONAL
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned int>());
			}
			break;
		case 10:	// SRATIONAL
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<int>());
			}
			break;
		}

		printf("[%08x] %04x %2d count=%6d value=%12d (%08x) [%-32s]\n",
			item.Offset, item.ID, item.Type, item.Count, item.ValueOrIndex, item.ValueOrIndex, strValue.Address());
	}
}

}
