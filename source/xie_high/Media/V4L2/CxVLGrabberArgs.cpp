/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLGrabberArgs.h"
#include "Media/api_media.h"

#include "Core/Axi.h"

namespace xie
{
namespace Media
{

// =================================================================
void CxVLGrabberArgs::_Constructor()
{
}

// =================================================================
CxVLGrabberArgs::CxVLGrabberArgs()
	: CxGrabberArgs()
{
	_Constructor();
}

// =================================================================
CxVLGrabberArgs::CxVLGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length)
	: CxGrabberArgs(frame_size, progress, addr, length)
{
	_Constructor();
}

// =================================================================
CxVLGrabberArgs::CxVLGrabberArgs(CxVLGrabberArgs&& src)
	: CxGrabberArgs(src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxVLGrabberArgs::CxVLGrabberArgs(const CxVLGrabberArgs& src)
	: CxGrabberArgs(src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxVLGrabberArgs::~CxVLGrabberArgs()
{
}

// =================================================================
CxVLGrabberArgs& CxVLGrabberArgs::operator = ( CxVLGrabberArgs&& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
CxVLGrabberArgs& CxVLGrabberArgs::operator = ( const CxVLGrabberArgs& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxVLGrabberArgs::operator == ( const CxVLGrabberArgs& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxVLGrabberArgs::operator != ( const CxVLGrabberArgs& src ) const
{
	return !(operator == (src));
}

// =================================================================
void CxVLGrabberArgs::CopyTo(IxModule& dst) const
{
	CxGrabberArgs::CopyTo(dst);
}

// =================================================================
void CxVLGrabberArgs::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		CxGrabberArgs::CopyFrom(src);

		if (auto _src = xie::Axi::SafeCast<CxVLGrabberArgs>(src)) 
		{
		}
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
bool CxVLGrabberArgs::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		if (CxGrabberArgs::ContentEquals(src) == false) return false;

		if (auto _src = xie::Axi::SafeCast<CxVLGrabberArgs>(src)) 
		{
		}
		return true;
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxVLGrabberArgs::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	CxGrabberArgs::GetParam(name, value, model);
}

// ============================================================
void CxVLGrabberArgs::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	CxGrabberArgs::SetParam(name, value, model);
}

}
}

#endif	// _MCS_VER
