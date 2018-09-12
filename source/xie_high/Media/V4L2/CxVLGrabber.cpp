/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLGrabber.h"
#include "Media/api_media.h"

#include "Core/CxException.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxVLGrabber";

// ============================================================
CxVLGrabber::CxVLGrabber(CxGrabber* parent)
{
	m_Parent = parent;
	m_Receiver = NULL;
}

// ============================================================
CxVLGrabber::~CxVLGrabber()
{
	Dispose();
}

// ============================================================
void CxVLGrabber::Dispose()
{
	Receiver(NULL);
}

// ============================================================
bool CxVLGrabber::IsValid() const
{
	if (m_Receiver == NULL) return false;
	return true;
}

// ================================================================================
void CxVLGrabber::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxVLGrabber>(src))
	{
		auto&	_src = static_cast<const CxVLGrabber&>(src);
		auto&	_dst = *this;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxVLGrabber::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxVLGrabber>(src))
	{
		auto&	_src = static_cast<const CxVLGrabber&>(src);
		auto&	_dst = *this;
		return true;
	}
	return false;
}

// ============================================================
CxGrabber* CxVLGrabber::Parent() const
{
	return m_Parent;
}

// ============================================================
CxVLSampleReceiver* CxVLGrabber::Receiver() const
{
	return m_Receiver;
}

// ============================================================
void CxVLGrabber::Receiver(const CxVLSampleReceiver* receiver)
{
	if (m_Receiver != NULL)
		m_Receiver->Remove(this->m_Parent);

	m_Receiver = (CxVLSampleReceiver*)receiver;

	if (m_Receiver != NULL)
		m_Receiver->Add(this->m_Parent);
}

}
}

#endif	// _MCS_VER
