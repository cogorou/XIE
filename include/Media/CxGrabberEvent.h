/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGRABBEREVENT_H_INCLUDED_
#define _CXGRABBEREVENT_H_INCLUDED_

#include "xie_high.h"
#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/IxEventReceiver.h"
#include "Media/CxGrabberArgs.h"
#include <functional>
#include <memory>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class CxGrabberEvent : public CxModule
	, public IxEventReceiver
{
public:
	typedef void Handler(void* sender, CxGrabberArgs* e);

	std::function<Handler> CallBack;

	CxGrabberEvent()
	{
	}
	CxGrabberEvent(const std::function<Handler>& callback)
	{
		CallBack = callback;
	}
	CxGrabberEvent& operator = (const CxGrabberEvent& src)
	{
		this->CallBack = src.CallBack;
		return *this;
	}
	operator std::shared_ptr<CxGrabberEvent> () const
	{
		CxGrabberEvent* clone = new CxGrabberEvent(this->CallBack);
		return std::shared_ptr<CxGrabberEvent>(clone);
	}

	void Receive(void* sender, IxModule* e)
	{
		if (CallBack != NULL)
		{
			if (auto args = xie::Axi::SafeCast<CxGrabberArgs>(e))
			{
				CallBack(sender, args);
			}
		}
	}
};

}
}

#pragma pack(pop)

#endif
