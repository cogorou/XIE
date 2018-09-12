/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXMEDIACONTROL_H_INCLUDED_
#define _IXMEDIACONTROL_H_INCLUDED_

#include "xie_high.h"
#include "Core/IxRunnable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

struct IxMediaControl : public IxRunnable
{
	virtual void Abort() = 0;
	virtual void Pause() = 0;
	virtual bool IsPaused() const = 0;
};

}
}

#pragma pack(pop)

#endif
