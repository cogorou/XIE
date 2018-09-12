/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxGrabberArgs.h"

#include "api_media.h"
#include "Core/Axi.h"
#include "Core/CxImage.h"
#include "GDI/api_gdi.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxGrabberArgs";

// =================================================================
void CxGrabberArgs::_Constructor()
{
	m_Tag = TxGrabberArgs::Default();
	m_Tag.TimeStamp = xie::Axi::GetTime();
}

// =================================================================
CxGrabberArgs::CxGrabberArgs()
{
	_Constructor();
}

// =================================================================
CxGrabberArgs::CxGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length)
{
	_Constructor();
	m_Tag.FrameSize = frame_size;
	m_Tag.Progress = progress;
	m_Tag.Address = addr;
	m_Tag.Length = length;
}

// =================================================================
CxGrabberArgs::CxGrabberArgs(CxGrabberArgs&& src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxGrabberArgs::CxGrabberArgs(const CxGrabberArgs& src)
{
	_Constructor();
	CopyFrom(src);
}

// =================================================================
CxGrabberArgs::~CxGrabberArgs()
{
	Dispose();
}

// =================================================================
CxGrabberArgs& CxGrabberArgs::operator = ( CxGrabberArgs&& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
CxGrabberArgs& CxGrabberArgs::operator = ( const CxGrabberArgs& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxGrabberArgs::operator == ( const CxGrabberArgs& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxGrabberArgs::operator != ( const CxGrabberArgs& src ) const
{
	return !(operator == (src));
}

// =================================================================
TxGrabberArgs CxGrabberArgs::Tag() const
{
	return m_Tag;
}

// =================================================================
void* CxGrabberArgs::TagPtr() const
{
	return (void*)&m_Tag;
}

//
// IxDisposable
//

// ============================================================
void CxGrabberArgs::Dispose()
{
	m_Tag = TxGrabberArgs::Default();
	m_Tag.TimeStamp = xie::Axi::GetTime();
}

// ============================================================
bool CxGrabberArgs::IsValid() const
{
	return true;
}

// =================================================================
CxGrabberArgs::operator CxImage() const
{
	CxImage dst;
	CopyTo(dst);
	return dst;
}

// =================================================================
void CxGrabberArgs::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxImage>(dst))
	{
#if defined(_MSC_VER)
		// RGB Å© BGR (BottomUp)
		TxModel	_src_model = TxModel::U8(3);	// TxBGR8x3
		TxImage _src_tag(
			m_Tag.Address,
			m_Tag.FrameSize.Width,
			m_Tag.FrameSize.Height,
			_src_model,
			Axi::CalcStride(_src_model, m_Tag.FrameSize.Width, 4),
			0	// default depth
			);
		CxImage		_src = CxImage::FromTag(_src_tag);
		CxImage&	_dst = static_cast<CxImage&>(dst);
		if (xie::GDI::fnPRV_GDI_CanConvertFrom(_dst, m_Tag.FrameSize.Size()) == false)
			_dst.Resize(m_Tag.FrameSize);
		xie::GDI::fnPRV_GDI_ConvertFrom_DDB(_dst, _src);
#else
		// RGB Å© YUYV
		TxModel	_src_model = TxModel::U8(4);	// TxYUYV
		TxImage _src_tag(
			m_Tag.Address,
			m_Tag.FrameSize.Width / 2,
			m_Tag.FrameSize.Height,
			_src_model,
			_src_model.Size() * (m_Tag.FrameSize.Width / 2),
			0	// default depth
			);
		CxImage		_src = CxImage::FromTag(_src_tag);
		CxImage&	_dst = static_cast<CxImage&>(dst);
		if (xie::GDI::fnPRV_GDI_CanConvertFrom(_dst, m_Tag.FrameSize.Size()) == false)
			_dst.Resize(m_Tag.FrameSize);
		xie::GDI::fnPRV_GDI_ConvertFrom_YUYV(_dst, _src);
#endif
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
void CxGrabberArgs::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		auto&	_src = static_cast<const CxGrabberArgs&>(src);
		auto&	_dst = *this;
		_dst.m_Tag = _src.m_Tag;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
bool CxGrabberArgs::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxGrabberArgs>(src)) 
	{
		auto&	_src = static_cast<const CxGrabberArgs&>(src);
		auto&	_dst = *this;

		// initialize
		if (_dst.m_Tag.TimeStamp	!= _src.m_Tag.TimeStamp) return false;

		// set by receiver
		if (_dst.m_Tag.FrameSize	!= _src.m_Tag.FrameSize) return false;
		if (_dst.m_Tag.Progress		!= _src.m_Tag.Progress) return false;
		if (_dst.m_Tag.Address		!= _src.m_Tag.Address) return false;
		if (_dst.m_Tag.Length		!= _src.m_Tag.Length) return false;

		// set by sender
		if (_dst.m_Tag.Index		!= _src.m_Tag.Index) return false;

		// response from user to sender
		if (_dst.m_Tag.Cancellation != _src.m_Tag.Cancellation) return false;
		return true;
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxGrabberArgs::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "TimeStamp") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::TimeStamp)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::FrameSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Progress") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::Progress)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Address") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::Address)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Length") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::Length)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Index") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::Index)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Cancellation") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxGrabberArgs::Cancellation)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGrabberArgs::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "TimeStamp") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::TimeStamp)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::FrameSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Progress") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::Progress)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Address") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::Address)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Length") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::Length)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Index") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::Index)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Cancellation") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxGrabberArgs::Cancellation)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
unsigned long long CxGrabberArgs::TimeStamp() const
{
	return m_Tag.TimeStamp;
}

// ============================================================
void CxGrabberArgs::TimeStamp(unsigned long long value)
{
	m_Tag.TimeStamp = value;
}

// ============================================================
TxImageSize CxGrabberArgs::FrameSize() const
{
	return m_Tag.FrameSize;
}

// ============================================================
void CxGrabberArgs::FrameSize(TxImageSize value)
{
	m_Tag.FrameSize = value;
}

// ============================================================
double CxGrabberArgs::Progress() const
{
	return m_Tag.Progress;
}

// ============================================================
void CxGrabberArgs::Progress(double value)
{
	m_Tag.Progress = value;
}

// ============================================================
void* CxGrabberArgs::Address() const
{
	return m_Tag.Address;
}

// ============================================================
void CxGrabberArgs::Address(void* value)
{
	m_Tag.Address = value;
}

// ============================================================
int CxGrabberArgs::Length() const
{
	return m_Tag.Length;
}

// ============================================================
void CxGrabberArgs::Length(int value)
{
	m_Tag.Length = value;
}

// ============================================================
int CxGrabberArgs::Index() const
{
	return m_Tag.Index;
}

// ============================================================
void CxGrabberArgs::Index(int value)
{
	m_Tag.Index = value;
}

// ============================================================
bool CxGrabberArgs::Cancellation() const
{
	return (m_Tag.Cancellation == ExBoolean::True);
}

// ============================================================
void CxGrabberArgs::Cancellation(bool value)
{
	m_Tag.Cancellation = value ? ExBoolean::True : ExBoolean::False;
}

}
}
