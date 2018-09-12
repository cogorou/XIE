/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "Media/TxScreenListItem.h"

namespace xie
{
namespace Media
{

// =================================================================
TxScreenListItem::TxScreenListItem()
{
	#if defined(_MSC_VER)
	Handle	= NULL;
	#else
	Handle	= 0;
	#endif
	Name	= NULL;
	X		= 0;
	Y		= 0;
	Width	= 0;
	Height	= 0;
}

#if defined(_MSC_VER)
// =================================================================
TxScreenListItem::TxScreenListItem(HWND handle, char* name, int x, int y, int width, int height)
{
	Handle	= handle;
	Name	= name;
	X		= x;
	Y		= y;
	Width	= width;
	Height	= height;
}
#else
// =================================================================
TxScreenListItem::TxScreenListItem(TxIntPtr handle, char* name, int x, int y, int width, int height)
{
	Handle	= handle;
	Name	= name;
	X		= x;
	Y		= y;
	Width	= width;
	Height	= height;
}
#endif

}	// Media
}	// xie
