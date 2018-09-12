/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxMutex.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

namespace xie
{

static const char* g_ClassName = "CxMutex";

// =================================================================
CxMutex::CxMutex()
{
#ifdef WIN32
	InitializeCriticalSection( &m_Mutex );
#else
	m_Mutex = PTHREAD_RECURSIVE_MUTEX_INITIALIZER_NP;
#endif
}

// =================================================================
CxMutex::~CxMutex()
{
#ifdef WIN32
	DeleteCriticalSection( &m_Mutex );
#else
	pthread_mutex_destroy( &m_Mutex );
#endif
}

// =================================================================
void CxMutex::Lock()
{
#ifdef WIN32
	EnterCriticalSection( &m_Mutex );
#else
	pthread_mutex_lock( &m_Mutex );
#endif
}

// =================================================================
void CxMutex::Unlock()
{
#ifdef WIN32
	LeaveCriticalSection( &m_Mutex );
#else
	pthread_mutex_unlock( &m_Mutex );
#endif
}

// =================================================================
bool CxMutex::Trylock( int timeout )
{
	xie::CxStopwatch watch;
	while(true)
	{
#ifdef WIN32
		BOOL status = TryEnterCriticalSection( &m_Mutex );
		if (status != FALSE)
			return true;
#else
		int status = pthread_mutex_trylock( &m_Mutex );
		if (status == 0)
			return true;
#endif
		watch.Stop();
		if (timeout <= watch.Elapsed) break;
		xie::Axi::Sleep(1);
	}
	return false;
}

}
