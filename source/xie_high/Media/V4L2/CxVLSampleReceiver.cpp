/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLGrabberArgs.h"
#include "Media/V4L2/CxVLSampleReceiver.h"
#include "Media/api_media.h"

#include "Core/CxFinalizer.h"

#include <algorithm>
#include <vector>

namespace xie
{
namespace Media
{

// =================================================================
CxVLSampleReceiver::CxVLSampleReceiver()
{
	FrameSize = TxImageSize::Default();
}

// =================================================================
CxVLSampleReceiver::~CxVLSampleReceiver()
{
}

// =================================================================
void CxVLSampleReceiver::Receive(double progress, void *addr, int length)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	for(int i=0 ; i<(int)Notify.size() ; i++)
	{
		if (Notify[i] != NULL)
		{
			CxVLGrabberArgs e(FrameSize, progress, addr, length);
			Notify[i]->Receive(this, &e);
		}
	}
}

// =================================================================
void CxVLSampleReceiver::Add(IxEventReceiver* src)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	if (src != NULL)
		Notify.push_back(src);
}

// =================================================================
void CxVLSampleReceiver::Remove(IxEventReceiver* src)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	if (src != NULL)
		Notify.erase(std::remove(Notify.begin(), Notify.end(), src), Notify.end());
}

}
}

#endif	// _MCS_VER
