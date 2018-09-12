/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTHREADEVENT_H_INCLUDED_
#define _CXTHREADEVENT_H_INCLUDED_

#include "xie_core.h"
#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/IxEventReceiver.h"
#include "Core/CxThreadArgs.h"
#include <functional>
#include <memory>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class CxThreadEvent : public CxModule
	, public IxEventReceiver
{
public:
	typedef void Handler(void* sender, CxThreadArgs* e);

	std::function<Handler> CallBack;

	CxThreadEvent()
	{
	}
	CxThreadEvent(const std::function<Handler>& callback)
	{
		CallBack = callback;
	}
	CxThreadEvent& operator = (const CxThreadEvent& src)
	{
		this->CallBack = src.CallBack;
		return *this;
	}
	operator std::shared_ptr<CxThreadEvent> () const
	{
		CxThreadEvent* clone = new CxThreadEvent(this->CallBack);
		return std::shared_ptr<CxThreadEvent>(clone);
	}

	void Receive(void* sender, IxModule* e)
	{
		if (CallBack != NULL)
		{
			if (auto args = xie::Axi::SafeCast<CxThreadArgs>(e))
			{
				CallBack(sender, args);
			}
		}
	}
};

}

#pragma pack(pop)

#endif
