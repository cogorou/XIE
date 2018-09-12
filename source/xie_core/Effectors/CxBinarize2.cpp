/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxBinarize2.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Data/api_data.h"
#include "CxBinarize.hpp"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxBinarize2";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_Binarize2(HxModule hsrc, HxModule hdst, HxModule hmask, TxRangeD threshold, bool use_abs, TxRangeD value)
{
	try
	{
		CxBinarize2 effector(threshold, use_abs, value);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
CxBinarize2::CxBinarize2()
{
	Threshold	= TxRangeD(96, 160);
	UseAbs		= false;
	Value		= TxRangeD(0, 1);
}

// ============================================================
CxBinarize2::CxBinarize2(TxRangeD threshold, bool use_abs, TxRangeD value)
{
	Threshold	= threshold;
	UseAbs		= use_abs;
	Value		= value;
}

// ===================================================================
CxBinarize2::CxBinarize2(const CxBinarize2& src)
{
	operator = (src);
}

// ============================================================
CxBinarize2::~CxBinarize2()
{
}

// ===================================================================
CxBinarize2& CxBinarize2::operator = ( const CxBinarize2& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxBinarize2::operator == ( const CxBinarize2& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxBinarize2::operator != ( const CxBinarize2& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxBinarize2::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxBinarize2>(src))
	{
		auto&	_src = static_cast<const CxBinarize2&>(src);
		auto&	_dst = *this;
		_dst.Threshold	= _src.Threshold;
		_dst.UseAbs		= _src.UseAbs;
		_dst.Value		= _src.Value;
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
bool CxBinarize2::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxBinarize2>(src))
	{
		auto& _src = static_cast<const CxBinarize2&>(src);
		auto& _dst = *this;
		if (_dst.Threshold	!= _src.Threshold) return false;
		if (_dst.UseAbs		!= _src.UseAbs) return false;
		if (_dst.Value		!= _src.Value) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxBinarize2::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
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
		{
			ExType	type	= ExType::U8;
			int		pack	= src->Model().Pack;
			dst->Resize(src->Length(), TxModel(type, pack));
		}

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		if (dst_tag.Model.Pack != src_tag.Model.Pack)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// -----
		CxImage dst_tmp;
		if (dst->Model().Type != ExType::U8)
		{
			dst_tmp.Resize(dst_tag.Width, dst_tag.Height, TxModel::U8(dst_tag.Model.Pack), dst_tag.Channels);
			dst_tag = dst_tmp.Tag();
		}

		if (UseAbs)
		{
			xie::Effectors::fnPRV_2D_Binarize___<TM>(dst_tag, mask_tag, src_tag,
				[this](double _src)
				{
					return (Threshold.Lower <= _src && _src <= Threshold.Upper) ? Value.Upper : Value.Lower;
				});
		}
		else
		{
			xie::Effectors::fnPRV_2D_Binarize___<TM>(dst_tag, mask_tag, src_tag,
				[this](double _src)
				{
					return (Threshold.Lower <= abs(_src) && abs(_src) <= Threshold.Upper) ? Value.Upper : Value.Lower;
				});
		}

		if (dst->Model().Type != ExType::U8)
		{
			dst->Filter().Copy(dst_tmp);
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
		{
			ExType	type	= ExType::U8;
			int		pack	= src->Model().Pack;
			dst->Resize(src->Width(), src->Height(), TxModel(type, pack), src->Channels());
		}

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

		if (dst_tag.Channels != src_tag.Channels)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst_tag.Model.Pack != src_tag.Model.Pack)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask_tag, dst_tag) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// -----
		CxImage dst_tmp;
		if (dst->Model().Type != ExType::U8)
		{
			dst_tmp.Resize(dst_tag.Width, dst_tag.Height, TxModel::U8(dst_tag.Model.Pack), dst_tag.Channels);
			dst_tag = dst_tmp.Tag();
		}

		if (UseAbs)
		{
			xie::Effectors::fnPRV_2D_Binarize___<TM>(dst_tag, mask_tag, src_tag,
				[this](double _src)
				{
					return (Threshold.Lower <= _src && _src <= Threshold.Upper) ? Value.Upper : Value.Lower;
				});
		}
		else
		{
			xie::Effectors::fnPRV_2D_Binarize___<TM>(dst_tag, mask_tag, src_tag,
				[this](double _src)
				{
					return (Threshold.Lower <= abs(_src) && abs(_src) <= Threshold.Upper) ? Value.Upper : Value.Lower;
				});
		}

		if (dst->Model().Type != ExType::U8)
		{
			dst->Filter().Copy(dst_tmp);
		}

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
