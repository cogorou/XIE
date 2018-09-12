/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxRgbToGray.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Data/api_data.h"
#include "CxRgbToGray.hpp"

#pragma warning (disable:4100)	// ˆø”‚ÍŠÖ”‚Ì–{‘Ì•”‚Å 1 “x‚àŽQÆ‚³‚ê‚Ü‚¹‚ñ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxRgbToGray";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_RgbToGray(HxModule hsrc, HxModule hdst, HxModule hmask, double scale, double red_ratio, double green_ratio, double blue_ratio)
{
	try
	{
		CxRgbToGray effector(scale, red_ratio, green_ratio, blue_ratio);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
void CxRgbToGray::_Constructor()
{
	this->Scale			= 0;
	this->RedRatio		= 0.299;
	this->GreenRatio	= 0.587;
	this->BlueRatio		= 0.114;
}

// ===================================================================
CxRgbToGray::CxRgbToGray()
{
	_Constructor();
}

// ============================================================
CxRgbToGray::CxRgbToGray(double scale, double red_ratio, double green_ratio, double blue_ratio)
{
	_Constructor();

	this->Scale			= scale;
	this->RedRatio		= red_ratio;
	this->GreenRatio	= green_ratio;
	this->BlueRatio		= blue_ratio;
}

// ===================================================================
CxRgbToGray::CxRgbToGray(const CxRgbToGray& src)
{
	operator = (src);
}

// ============================================================
CxRgbToGray::~CxRgbToGray()
{
}

// ===================================================================
CxRgbToGray& CxRgbToGray::operator = ( const CxRgbToGray& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxRgbToGray::operator == ( const CxRgbToGray& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxRgbToGray::operator != ( const CxRgbToGray& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxRgbToGray::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxRgbToGray>(src))
	{
		auto&	_src = static_cast<const CxRgbToGray&>(src);
		auto&	_dst = *this;
		_dst.Scale		= _src.Scale;
		_dst.RedRatio	= _src.RedRatio;
		_dst.GreenRatio	= _src.GreenRatio;
		_dst.BlueRatio	= _src.BlueRatio;
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
bool CxRgbToGray::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxRgbToGray>(src))
	{
		auto& _src = static_cast<const CxRgbToGray&>(src);
		auto& _dst = *this;
		if (_dst.Scale		!= _src.Scale) return false;
		if (_dst.RedRatio	!= _src.RedRatio) return false;
		if (_dst.GreenRatio	!= _src.GreenRatio) return false;
		if (_dst.BlueRatio	!= _src.BlueRatio) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxRgbToGray::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
{
	typedef unsigned char	TM;

	if (auto dst = xie::Axi::SafeCast<CxArray>(hdst))
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (dst->IsValid() == false)
			dst->Resize(src->Length(), TxModel(src->Model().Type, 1));

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		if (dst->Length() != src->Length())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->Model().Pack != 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_RgbToGray___<TM>(dst_tag, mask_tag, src_tag, this->Scale, this->RedRatio, this->GreenRatio, this->BlueRatio);

		return;
	}

	if (auto dst = xie::Axi::SafeCast<CxImage>(hdst))
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), TxModel(src->Model().Type, 1), (src->Model().Pack == 1) ? 1 : src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

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
			if (mask_tag.Width != dst_tag.Width)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask_tag.Height != dst_tag.Height)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask_tag.Model != xie::ModelOf<TM>())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}

		if (dst->Size() != src->Size())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->Model().Pack != 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_RgbToGray___<TM>(dst_tag, mask_tag, src_tag, this->Scale, this->RedRatio, this->GreenRatio, this->BlueRatio);

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
