/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXFINALIZER_H_INCLUDED_
#define _CXFINALIZER_H_INCLUDED_

#include "xie_core.h"
#include <functional>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
class CxFinalizer
{
public:
	typedef void Handler();
	
	std::function<Handler> Finalize;

	CxFinalizer()
	{
	}
	CxFinalizer(const std::function<Handler>& finalize)
	{
		Finalize = finalize;
	}
	virtual ~CxFinalizer()
	{
		if (Finalize != NULL)
			Finalize();
	}
};

}

#pragma pack(pop)

#endif
