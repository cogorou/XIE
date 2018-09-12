/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _CXVLSAMPLERECEIVER_H_INCLUDED_
#define _CXVLSAMPLERECEIVER_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxMutex.h"
#include "Core/TxImageSize.h"
#include "Core/IxEventReceiver.h"

#include <vector>

namespace xie
{
namespace Media
{

// /////////////////////////////////////////////////////////////////
class CxVLSampleReceiver
{
public:
	CxVLSampleReceiver();
	virtual ~CxVLSampleReceiver();

    virtual void Receive(double progress, void *addr, int length);

	virtual void Add(IxEventReceiver* src);
	virtual void Remove(IxEventReceiver* src);

	CxMutex Mutex;
	TxImageSize FrameSize;
	std::vector<IxEventReceiver*> Notify;
};

}
}

#endif

#endif	// _MCS_VER
