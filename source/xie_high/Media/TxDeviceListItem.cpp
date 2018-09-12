/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "Media/TxDeviceListItem.h"

namespace xie
{
namespace Media
{

// =================================================================
TxDeviceListItem::TxDeviceListItem()
{
	MediaType	= ExMediaType::None;
	MediaDir	= ExMediaDir::None;
	Name	= NULL;
	Index	= 0;
}

// =================================================================
TxDeviceListItem::TxDeviceListItem(ExMediaType type, ExMediaDir dir, char* name, int index)
{
	MediaType	= type;
	MediaDir	= dir;
	Name	= name;
	Index	= index;
}

}	// Media
}	// xie
