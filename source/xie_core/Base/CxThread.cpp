/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxThread.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

namespace xie
{

static const char* g_ClassName = "CxThread";

// =================================================================
void CxThread::_Constructor()
{
	Notify		= NULL;
	Param		= NULL;
	m_Index		= -1;
	m_IsValid	= false;
	m_Enabled	= false;
	m_Delay		= 100;
	
#if defined(_MSC_VER)
	m_ThreadID	= 0;
	m_hThread	= NULL;
#else
	memset(&m_ThreadID, 0, sizeof(m_ThreadID));
#endif
}

// =================================================================
CxThread::CxThread()
{
	_Constructor();
}

// =================================================================
CxThread::CxThread( const CxThread& src )
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxThread::~CxThread()
{
	Dispose();
}

// ============================================================
CxThread& CxThread::operator = ( const CxThread& src )
{
	if (this == &src) return *this;
	Notify	= src.Notify;
	Param	= src.Param;
	return *this;
}

// ============================================================
bool CxThread::operator == ( const CxThread& src ) const
{
	return (this == &src);
}

// ============================================================
bool CxThread::operator != ( const CxThread& src ) const
{
	return !(CxThread::operator == (src));
}

// =================================================================
void CxThread::ThreadProc()
{
#if defined(_MSC_VER)
	while(this->m_IsValid)
	{
		if (this->m_Enabled && this->Notify != NULL)
		{
			int index = this->m_Index + 1;
			if (index < 0)
				index = 0;
			this->m_Index = index;
			auto e = CxThreadArgs(this->Param, this->m_Index);
			this->Notify->Receive(this, &e);
			if (e.Cancellation)
				this->m_Enabled = false;
			this->SafeSleep(this->m_Delay);
		}
		else
		{
			::SuspendThread(this->m_hThread);
		}
	}
#else
	while(this->m_IsValid)
	{
		if (this->m_Enabled && this->Notify != NULL)
		{
			int index = this->m_Index + 1;
			if (index < 0)
				index = 0;
			this->m_Index = index;
			auto e = CxThreadArgs(this->Param, this->m_Index);
			this->Notify->Receive(this, &e);
			if (e.Cancellation)
				this->m_Enabled = false;
		}
		this->SafeSleep(this->m_Delay);
	}
#endif
}

// =================================================================
#if defined(_MSC_VER)
unsigned int __stdcall CxThread::_ThreadStart(void* sender)
{
	CxThread* pThis = static_cast<CxThread*>(sender);
	if( pThis != NULL )
		pThis->ThreadProc();
	_endthreadex( 0 );
	return 0;
}
#else
void* CxThread::_ThreadStart(void* sender)
{
	CxThread* pThis = static_cast<CxThread*>(sender);
	if( pThis != NULL )
		pThis->ThreadProc();
	return NULL;
}
#endif

// =================================================================
void CxThread::Setup()
{
	Dispose();

#if defined(_MSC_VER)
	m_hThread = (HANDLE)_beginthreadex(NULL, 0, _ThreadStart, this, CREATE_SUSPENDED, &m_ThreadID);
	if (m_hThread == NULL)
		throw xie::CxException( xie::ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__ );
	m_IsValid	= true;
#else
	int status = pthread_create(&m_ThreadID, NULL, _ThreadStart, (void*)this);
	if (status != 0)
		throw xie::CxException( xie::ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__ );
	m_IsValid	= true;
#endif
}

// =================================================================
void CxThread::Dispose()
{
	m_IsValid	= false;
	m_Enabled	= false;

#if defined(_MSC_VER)
	if (m_hThread != NULL)
	{
		::ResumeThread( m_hThread );
		int sleep = 100 + ((m_Delay < 0) ? 0 : m_Delay);
		if (WAIT_OBJECT_0 == ::WaitForSingleObject( m_hThread, sleep ))
			::CloseHandle( m_hThread );
	}
	m_hThread = NULL;
	m_ThreadID = 0;
#else
	pthread_join(m_ThreadID, NULL);
	memset(&m_ThreadID, 0, sizeof(m_ThreadID));
#endif
}

// =================================================================
bool CxThread::IsValid() const
{
	if (m_IsValid == false)	return false;
	if (Notify == NULL)		return false;
#if defined(_MSC_VER)
	if (m_hThread == NULL)	return false;
	if (m_ThreadID == 0)	return false;
#else
#endif
	return true;
}

// =================================================================
void CxThread::Reset()
{
	m_Index = -1;
}

// =================================================================
void CxThread::Start()
{
	if (IsValid() != true) return;
	m_Enabled = true;
#if defined(_MSC_VER)
	::ResumeThread( m_hThread );
#else
#endif
}

// =================================================================
void CxThread::Stop()
{
	if (IsValid() != true) return;
	m_Enabled = false;
}

// ============================================================
bool CxThread::Wait(int timeout) const
{
	xie::CxStopwatch watch;
	watch.Start();
	while(IsRunning())
	{
		watch.Stop();
		if (0 <= timeout && timeout <= watch.Elapsed)
			return false;
		xie::Axi::Sleep(1);
	}
	return true;
}

// =================================================================
bool CxThread::IsRunning() const
{
	return (m_IsValid && m_Enabled);
}

// =================================================================
int CxThread::Index() const
{
	return m_Index;
}

// =================================================================
void CxThread::Index(int value)
{
	m_Index = value;
}

// =================================================================
int CxThread::Delay() const
{
	return m_Delay;
}

// =================================================================
void CxThread::Delay(int value)
{
	if (value < 0)
		value = 0;
	m_Delay = value;
}

// =================================================================
void CxThread::SafeSleep(int timeout) const
{
	if (timeout == 0)
	{
		xie::Axi::Sleep(0);
	}
	else if (timeout < 0)
	{
		while(m_IsValid && m_Enabled)
		{
			xie::Axi::Sleep(1);
		}
	}
	else
	{
		CxStopwatch watch;
		while(m_IsValid && m_Enabled)
		{
			watch.Stop();
			if (timeout <= watch.Elapsed) break;
			xie::Axi::Sleep(1);
		}
	}
}

#if defined(_MSC_VER)
// =================================================================
void* CxThread::hThread() const
{
	return m_hThread;
}
// =================================================================
unsigned int CxThread::ThreadID() const
{
	return m_ThreadID;
}
#else
// =================================================================
pthread_t CxThread::ThreadID() const
{
	return m_ThreadID;
}
#endif

}
