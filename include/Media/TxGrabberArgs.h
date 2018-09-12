/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXGRABBERARGS_H_INCLUDED_
#define _TXGRABBERARGS_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxImageSize.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace Media
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxGrabberArgs
{
	// initialized by this
	unsigned long long	TimeStamp;

	// set by receiver
	TxImageSize		FrameSize;	// common
	double			Progress;	// common
	void*			Address;	// address of data
	int				Length;		// length of data

	// set by sender
	int				Index;

	// response from user to sender
	ExBoolean		Cancellation;

#if defined(__cplusplus)
	static inline TxGrabberArgs Default()
	{
		TxGrabberArgs result;
		// initialize
		result.TimeStamp = 0;

		// set by receiver
		result.FrameSize = TxImageSize::Default();
		result.Progress = 0;
		result.Address = NULL;
		result.Length = 0;

		// set by sender
		result.Index = -1;

		// response from user to sender
		result.Cancellation = ExBoolean::False;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxGrabberArgs();
#endif
};

#if defined(__cplusplus)
}	// Media
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
