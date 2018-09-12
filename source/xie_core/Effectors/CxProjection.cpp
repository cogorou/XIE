/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxProjection.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Data/api_data.h"
#include "CxProjection.hpp"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxProjection";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_Projection(HxModule hsrc, HxModule hdst, HxModule hmask, ExScanDir dir, int ch)
{
	try
	{
		CxProjection effector(dir, ch);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
CxProjection::CxProjection()
{
	ScanDir = ExScanDir::X;
	Channel = 0;
}

// ============================================================
CxProjection::CxProjection(ExScanDir dir, int ch)
{
	ScanDir = dir;
	Channel = ch;
}

// ===================================================================
CxProjection::CxProjection(const CxProjection& src)
{
	operator = (src);
}

// ============================================================
CxProjection::~CxProjection()
{
}

// ===================================================================
CxProjection& CxProjection::operator = ( const CxProjection& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxProjection::operator == ( const CxProjection& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxProjection::operator != ( const CxProjection& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxProjection::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxProjection>(src))
	{
		auto&	_src = static_cast<const CxProjection&>(src);
		auto&	_dst = *this;
		_dst.ScanDir = _src.ScanDir;
		_dst.Channel = _src.Channel;
		return;
	}
	if (auto _src = xie::Axi::SafeCast<IxConvertible>(&src))
	{
		_src->CopyTo(*this);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxProjection::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxProjection>(src))
	{
		auto& _src = static_cast<const CxProjection&>(src);
		auto& _dst = *this;
		if (_dst.ScanDir != _src.ScanDir) return false;
		if (_dst.Channel != _src.Channel) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxProjection::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
{
	typedef unsigned char	TM;
	typedef TxStatistics	TR;

	if (auto result = xie::Axi::SafeCast<CxArray>(hdst))
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		int pxc = src->Model().Pack * src->Channels();
		if (!(0 <= this->Channel && this->Channel < pxc))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		auto dst_model = ModelOf<TR>();
		int dst_length = 0;
		switch(this->ScanDir)
		{
		case ExScanDir::X: dst_length = src->Height(); break;
		case ExScanDir::Y: dst_length = src->Width(); break;
		default:
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}

		if (result->IsValid() == false)
			result->Resize(dst_length, dst_model);
		if (result->Length() != dst_length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (result->Model() != dst_model)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage src_tag = src->Tag();
		TxImage mask_tag = TxImage::Default();
		TxArray dst_tag = result->Tag();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		if (mask_tag.Channels != 0)
		{
			if (mask_tag.Channels != 1)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask_tag.Width != src_tag.Width)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask_tag.Height != src_tag.Height)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask_tag.Model != xie::ModelOf<TM>())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}

		fnPRV_2D_Projection___<TM,TR>(src_tag, mask_tag, this->ScanDir, this->Channel, dst_tag);

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
