/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "Media/TxGrabberArgs.h"

namespace xie
{
namespace Media
{

// =================================================================
TxGrabberArgs::TxGrabberArgs()
{
	// initialize
	TimeStamp = 0;

	// set by receiver
	FrameSize = TxImageSize::Default();
	Progress = 0;
	Address = NULL;
	Length = 0;

	// set by sender
	Index = -1;

	// response from user to sender
	Cancellation = ExBoolean::False;
}

}	// Media
}	// xie
