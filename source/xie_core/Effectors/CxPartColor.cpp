/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxPartColor.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Data/api_data.h"
#include "CxPartColor.hpp"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxPartColor";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_PartColor(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	try
	{
		CxPartColor effector(depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
CxPartColor::CxPartColor()
{
	Depth			= 0;
	HueDir			= 0;
	HueRange		= 30;
	RedRatio		= 0.299;
	GreenRatio		= 0.587;
	BlueRatio		= 0.114;
}

// ============================================================
CxPartColor::CxPartColor(int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	Depth			= depth;
	HueDir			= hue_dir;
	HueRange		= hue_range;
	RedRatio		= red_ratio;
	GreenRatio		= green_ratio;
	BlueRatio		= blue_ratio;
}

// ===================================================================
CxPartColor::CxPartColor(const CxPartColor& src)
{
	operator = (src);
}

// ============================================================
CxPartColor::~CxPartColor()
{
}

// ===================================================================
CxPartColor& CxPartColor::operator = ( const CxPartColor& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxPartColor::operator == ( const CxPartColor& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxPartColor::operator != ( const CxPartColor& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxPartColor::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxPartColor>(src))
	{
		auto&	_src = static_cast<const CxPartColor&>(src);
		auto&	_dst = *this;
		_dst.Depth			= _src.Depth;
		_dst.HueDir			= _src.HueDir;
		_dst.HueRange		= _src.HueRange;
		_dst.RedRatio		= _src.RedRatio;
		_dst.GreenRatio		= _src.GreenRatio;
		_dst.BlueRatio		= _src.BlueRatio;
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
bool CxPartColor::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxPartColor>(src))
	{
		auto& _src = static_cast<const CxPartColor&>(src);
		auto& _dst = *this;
		if (_dst.Depth		!= _src.Depth) return false;
		if (_dst.HueDir		!= _src.HueDir) return false;
		if (_dst.HueRange	!= _src.HueRange) return false;
		if (_dst.RedRatio	!= _src.RedRatio) return false;
		if (_dst.BlueRatio	!= _src.BlueRatio) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxPartColor::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
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
			dst->Resize(src->Length(), src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		if (dst->Length() != src->Length())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (dst->Model() == src->Model())
		{
			fnPRV_2D_PartColor___<TM>(dst_tag, mask_tag, src_tag, Depth, HueDir, HueRange, RedRatio, GreenRatio, BlueRatio);
		}
		else
		{
			CxArray tmp = src->Clone(TxModel::F64(src->Model().Pack));
			src_tag = To2D(tmp.Tag());
			dst_tag = To2D(tmp.Tag());

			fnPRV_2D_PartColor___<TM>(dst_tag, mask_tag, src_tag, Depth, HueDir, HueRange, RedRatio, GreenRatio, BlueRatio);

			dst->Filter().Copy(tmp);
		}

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
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

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

		if (dst->ImageSize() == src->ImageSize())
		{
			fnPRV_2D_PartColor___<TM>(dst_tag, mask_tag, src_tag, Depth, HueDir, HueRange, RedRatio, GreenRatio, BlueRatio);
		}
		else
		{
			CxImage tmp = src->Clone(TxModel::F64(src->Model().Pack));
			src_tag = tmp.Tag();
			dst_tag = tmp.Tag();

			fnPRV_2D_PartColor___<TM>(dst_tag, mask_tag, src_tag, Depth, HueDir, HueRange, RedRatio, GreenRatio, BlueRatio);

			dst->Filter().Copy(tmp);
		}

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
