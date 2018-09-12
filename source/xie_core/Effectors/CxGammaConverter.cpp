/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxGammaConverter.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Data/api_data.h"
#include "CxGammaConverter.hpp"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxGammaConverter";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_GammaConverter(HxModule hsrc, HxModule hdst, HxModule hmask, int depth, double gamma)
{
	try
	{
		CxGammaConverter effector(depth, gamma);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
CxGammaConverter::CxGammaConverter()
{
	Depth	= 0;
	Gamma	= 1;
}

// ============================================================
CxGammaConverter::CxGammaConverter(int depth, double gamma)
{
	Depth	= depth;
	Gamma	= gamma;
}

// ===================================================================
CxGammaConverter::CxGammaConverter(const CxGammaConverter& src)
{
	operator = (src);
}

// ============================================================
CxGammaConverter::~CxGammaConverter()
{
}

// ===================================================================
CxGammaConverter& CxGammaConverter::operator = ( const CxGammaConverter& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxGammaConverter::operator == ( const CxGammaConverter& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxGammaConverter::operator != ( const CxGammaConverter& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxGammaConverter::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGammaConverter>(src))
	{
		auto&	_src = static_cast<const CxGammaConverter&>(src);
		auto&	_dst = *this;
		_dst.Depth	= _src.Depth;
		_dst.Gamma	= _src.Gamma;
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
bool CxGammaConverter::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxGammaConverter>(src))
	{
		auto& _src = static_cast<const CxGammaConverter&>(src);
		auto& _dst = *this;
		if (_dst.Depth	!= _src.Depth) return false;
		if (_dst.Gamma	!= _src.Gamma) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxGammaConverter::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
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

		if (dst_tag.Model.Type == src_tag.Model.Type)
		{
			fnPRV_2D_GammaConverter___<TM>(dst_tag, mask_tag, src_tag, Depth, Gamma);
		}
		else
		{
			CxArray tmp = src->Clone(TxModel::F64(src->Model().Pack));
			src_tag = To2D(tmp.Tag());
			dst_tag = To2D(tmp.Tag());

			fnPRV_2D_GammaConverter___<TM>(dst_tag, mask_tag, src_tag, Depth, Gamma);

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
			if (mask_tag.Channels != 1 && mask_tag.Channels != dst_tag.Channels)
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

		if (dst_tag.Model.Type == src_tag.Model.Type)
		{
			fnPRV_2D_GammaConverter___<TM>(dst_tag, mask_tag, src_tag, Depth, Gamma);
		}
		else
		{
			CxImage tmp = src->Clone(TxModel::F64(src->Model().Pack));
			src_tag = tmp.Tag();
			dst_tag = tmp.Tag();

			fnPRV_2D_GammaConverter___<TM>(dst_tag, mask_tag, src_tag, Depth, Gamma);

			dst->Filter().Copy(tmp);
		}

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
