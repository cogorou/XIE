/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "Media/TxDeviceParam.h"

namespace xie
{
namespace Media
{

// =================================================================
TxDeviceParam::TxDeviceParam()
{
	Name	= NULL;
	Index	= 0;
	Pin		= 0;
	Width	= 0;
	Height	= 0;
}

// =================================================================
TxDeviceParam::TxDeviceParam(char* name, int index, int pin, int width, int height)
{
	Name	= name;
	Index	= index;
	Pin		= pin;
	Width	= width;
	Height	= height;
}

}	// Media
}	// xie
