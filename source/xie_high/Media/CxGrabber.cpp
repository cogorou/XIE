/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxGrabber.h"

#include "api_media.h"

#if defined(_MSC_VER)
#include "DS/CxDSGrabber.h"
typedef xie::Media::CxDSGrabber	TBODY;	//!< The type only used in this file.
#else
#include "V4L2/CxVLGrabber.h"
typedef xie::Media::CxVLGrabber	TBODY;	//!< The type only used in this file.
#endif

#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxGrabber";

// ============================================================
void CxGrabber::_Constructor()
{
	m_Handle = (HxModule)new TBODY(this);
	m_Enabled = false;
	m_Index = -1;
}

// ============================================================
CxGrabber::CxGrabber()
{
	_Constructor();
}

// ============================================================
CxGrabber::CxGrabber(const CxGrabber& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxGrabber::~CxGrabber()
{
	Dispose();
	delete reinterpret_cast<TBODY*>(m_Handle);
}

// ============================================================
IxModule* CxGrabber::GetModule() const
{
	return reinterpret_cast<IxModule*>(m_Handle);
}

// =================================================================
CxGrabber& CxGrabber::operator = ( const CxGrabber& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxGrabber::operator == ( const CxGrabber& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxGrabber::operator != ( const CxGrabber& src ) const
{
	return !(operator == (src));
}

//
// IxDisposable
//

// ============================================================
void CxGrabber::Dispose()
{
	Stop();
	Reset();
}

// ============================================================
bool CxGrabber::IsValid() const
{
	return true;
}

// ================================================================================
void CxGrabber::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxGrabber>(src))
	{
		auto&	_src = static_cast<const CxGrabber&>(src);
		auto&	_dst = *this;
		_dst.Notify		= _src.Notify;
		_dst.m_Enabled	= _src.m_Enabled;
		_dst.m_Index	= _src.m_Index;
		{
			auto _src_body = reinterpret_cast<TBODY*>(_src.m_Handle);
			auto _dst_body = reinterpret_cast<TBODY*>(_dst.m_Handle);
			_dst_body->CopyFrom(*_src_body);
		}
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxGrabber::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxGrabber>(src))
	{
		auto&	_src = static_cast<const CxGrabber&>(src);
		auto&	_dst = *this;
		if (_dst.m_Enabled	!= _src.m_Enabled) return false;
		if (_dst.m_Index	!= _src.m_Index) return false;
		{
			TBODY*	_src_body = reinterpret_cast<TBODY*>(_src.m_Handle);
			TBODY*	_dst_body = reinterpret_cast<TBODY*>(_dst.m_Handle);
			_dst_body->ContentEquals(*_src_body);
		}
		return true;
	}
	return false;
}

//
// IxRunnable
//

// ============================================================
void CxGrabber::Reset()
{
	m_Index = -1;
}

// ============================================================
void CxGrabber::Start()
{
	m_Enabled = true;
}

// ============================================================
void CxGrabber::Stop()
{
	m_Enabled = false;
}

// ============================================================
bool CxGrabber::Wait(int timeout) const
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

// ============================================================
bool CxGrabber::IsRunning() const
{
	return m_Enabled;
}

// ============================================================
int CxGrabber::Index() const
{
	return m_Index;
}

// ============================================================
void CxGrabber::Index(int value)
{
	m_Index = value;
}

//
// IxEventReceiver
//

// ============================================================
void CxGrabber::Receive(void* sender, IxModule* e)
{
	if (m_Enabled)
	{
		if (Notify != NULL)
		{
			if (auto args = xie::Axi::SafeCast<CxGrabberArgs>(e))
			{
				// フレーム指標更新.
				int index = m_Index + 1;
				if (index < 0)
					index = 0;
				m_Index = index;
				args->Index(m_Index);

				// 呼び出し.
				Notify->Receive(this, args);

				// 応答の確認.
				if (args->Cancellation())
					m_Enabled = false;
			}
		}
	}
}

}
}
