/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#include "Media/DS/CxDSGrabber.h"
#include "Media/api_media.h"
#include "Core/CxException.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDSGrabber";

// ============================================================
CxDSGrabber::CxDSGrabber(CxGrabber* parent)
{
	m_Parent = parent;
	m_Receiver = NULL;
}

// ============================================================
CxDSGrabber::~CxDSGrabber()
{
	Dispose();
}

// ============================================================
void CxDSGrabber::Dispose()
{
	Receiver(NULL);
}

// ============================================================
bool CxDSGrabber::IsValid() const
{
	if (m_Receiver == NULL) return false;
	return true;
}

// ================================================================================
void CxDSGrabber::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxDSGrabber>(src))
	{
		auto&	_src = static_cast<const CxDSGrabber&>(src);
		auto&	_dst = *this;
		_dst.Receiver(_src.m_Receiver);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxDSGrabber::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxDSGrabber>(src))
	{
		auto&	_src = static_cast<const CxDSGrabber&>(src);
		auto&	_dst = *this;
		if (_dst.m_Receiver	!= _src.m_Receiver) return false;
		return true;
	}
	return false;
}

// ============================================================
CxGrabber* CxDSGrabber::Parent() const
{
	return m_Parent;
}

// ============================================================
CxDSSampleReceiver* CxDSGrabber::Receiver() const
{
	return m_Receiver;
}

// ============================================================
void CxDSGrabber::Receiver(const CxDSSampleReceiver* receiver)
{
	if (m_Receiver != NULL)
		m_Receiver->Remove(this->m_Parent);

	m_Receiver = (CxDSSampleReceiver*)receiver;

	if (m_Receiver != NULL)
		m_Receiver->Add(this->m_Parent);
}

}
}

#endif	// _MCS_VER
