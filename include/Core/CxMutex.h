/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXMUTEX_H_INCLUDED_
#define _CXMUTEX_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxMutex : public CxModule
{
public:
	CxMutex();
	virtual ~CxMutex();

	void Lock();
	void Unlock();
	bool Trylock( int timeout );

protected:
	#ifdef WIN32
	CRITICAL_SECTION	m_Mutex;
	#else
	pthread_mutex_t		m_Mutex;
	#endif
};

}

#pragma pack(pop)

#endif
