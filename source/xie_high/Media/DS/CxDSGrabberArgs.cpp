/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#include "Media/DS/CxDSGrabberArgs.h"
#include "Media/api_media.h"
#include "Core/Axi.h"

#if defined(_MSC_VER)

namespace xie
{
namespace Media
{

// =================================================================
void CxDSGrabberArgs::_Constructor()
{
	// set by CxDSSampleReceiver
	Sample = NULL;
}

// =================================================================
CxDSGrabberArgs::CxDSGrabberArgs()
	: CxGrabberArgs()
{
	_Constructor();
}

// =================================================================
CxDSGrabberArgs::CxDSGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length, IMediaSample *sample)
	: CxGrabberArgs(frame_size, progress, addr, length)
{
	_Constructor();
	Sample = sample;
}

// =================================================================
CxDSGrabberArgs::CxDSGrabberArgs(CxDSGrabberArgs&& src)
	: CxGrabberArgs(src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxDSGrabberArgs::CxDSGrabberArgs(const CxDSGrabberArgs& src)
	: CxGrabberArgs(src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxDSGrabberArgs::~CxDSGrabberArgs()
{
}

// =================================================================
CxDSGrabberArgs& CxDSGrabberArgs::operator = ( CxDSGrabberArgs&& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
CxDSGrabberArgs& CxDSGrabberArgs::operator = ( const CxDSGrabberArgs& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxDSGrabberArgs::operator == ( const CxDSGrabberArgs& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxDSGrabberArgs::operator != ( const CxDSGrabberArgs& src ) const
{
	return !(operator == (src));
}

// =================================================================
void CxDSGrabberArgs::CopyTo(IxModule& dst) const
{
	CxGrabberArgs::CopyTo(dst);
}

// =================================================================
void CxDSGrabberArgs::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		CxGrabberArgs::CopyFrom(src);

		if (auto _src = xie::Axi::SafeCast<CxDSGrabberArgs>(src)) 
		{
			// set by CxDSSampleReceiver
			this->Sample = _src->Sample;
		}
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
bool CxDSGrabberArgs::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		if (CxGrabberArgs::ContentEquals(src) == false) return false;

		if (auto _src = xie::Axi::SafeCast<CxDSGrabberArgs>(src)) 
		{
			// set by CxDSSampleReceiver
			if (this->Sample != _src->Sample) return false;
		}
		return true;
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxDSGrabberArgs::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Sample") == 0)
	{
		if (model != TxModel::Ptr(1))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		*static_cast<void**>(value) = this->Sample;
		return;
	}

	CxGrabberArgs::GetParam(name, value, model);
}

// ============================================================
void CxDSGrabberArgs::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	CxGrabberArgs::SetParam(name, value, model);
}

}
}

#endif

#endif	// _MCS_VER
