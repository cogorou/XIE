/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTHREAD_H_INCLUDED_
#define _CXTHREAD_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/CxThreadArgs.h"
#include "Core/CxThreadEvent.h"
#include "Core/IxDisposable.h"
#include "Core/IxRunnable.h"
#include <memory>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxThread : public CxModule
	, public IxDisposable
	, public IxRunnable
{
private:
	void _Constructor();

public:
	std::shared_ptr<CxThreadEvent>	Notify;
	void*							Param;

public:
	CxThread();
	CxThread( const CxThread& src );
	virtual ~CxThread();

	CxThread& operator = ( const CxThread& src );
	bool operator == ( const CxThread& src ) const;
	bool operator != ( const CxThread& src ) const;

public:
	virtual void Setup();

	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxRunnable
	virtual void Reset();
	virtual void Start();
	virtual void Stop();
	virtual bool Wait(int timeout) const;
	virtual bool IsRunning() const;

	virtual int Index() const;
	virtual void Index(int value);

	virtual int Delay() const;
	virtual void Delay(int value);

	virtual void SafeSleep(int timeout) const;

#if defined(_MSC_VER)
	virtual void* hThread() const;
	virtual unsigned int ThreadID() const;
#else
	virtual pthread_t ThreadID() const;
#endif

protected:
	bool			m_IsValid;
	bool			m_Enabled;
	int				m_Index;
	int				m_Delay;
#if defined(_MSC_VER)
	void*			m_hThread;
	unsigned int	m_ThreadID;
#else
	pthread_t		m_ThreadID;
#endif

	virtual void ThreadProc();

#if defined(_MSC_VER)
	static unsigned int __stdcall _ThreadStart(void* sender);
#else
	static void* _ThreadStart(void* sender);
#endif
};

}

#pragma pack(pop)

#endif
