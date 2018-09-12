/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxIntegral.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Data/api_data.h"
#include "CxIntegral.hpp"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxIntegral";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_Integral(HxModule hsrc, HxModule hdst, HxModule hmask, int mode)
{
	try
	{
		CxIntegral effector(mode);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
CxIntegral::CxIntegral()
{
	Mode = 1;
}

// ============================================================
CxIntegral::CxIntegral(int mode)
{
	Mode = mode;
}

// ===================================================================
CxIntegral::CxIntegral(const CxIntegral& src)
{
	operator = (src);
}

// ============================================================
CxIntegral::~CxIntegral()
{
}

// ===================================================================
CxIntegral& CxIntegral::operator = ( const CxIntegral& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxIntegral::operator == ( const CxIntegral& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxIntegral::operator != ( const CxIntegral& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxIntegral::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxIntegral>(src))
	{
		auto&	_src = static_cast<const CxIntegral&>(src);
		auto&	_dst = *this;
		_dst.Mode = _src.Mode;
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
bool CxIntegral::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxIntegral>(src))
	{
		auto& _src = static_cast<const CxIntegral&>(src);
		auto& _dst = *this;
		if (_dst.Mode != _src.Mode) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxIntegral::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
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
			dst->Resize(src->Length(), TxModel::F64(src->Model().Pack));

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		if (dst->Length() != src->Length())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		bool dst_type_isvalid = false;
		switch(dst->Model().Type)
		{
			case ExType::F64:	dst_type_isvalid = true; break;
			default:
				break;
		}

		CxArray buf(src_tag.Width, TxModel::F64(1));

		if (dst_type_isvalid && dst->Model().Pack == src->Model().Pack)
		{
			switch(this->Mode)
			{
			case 1: fnPRV_2D_Integral1(dst_tag, mask_tag, src_tag, buf.Tag()); break;
			case 2: fnPRV_2D_Integral2(dst_tag, mask_tag, src_tag, buf.Tag()); break;
			}
		}
		else
		{
			CxArray tmp(dst_tag.Width, TxModel::F64(dst_tag.Model.Pack));
			TxImage tmp_tag = To2D(src->Tag());

			switch(this->Mode)
			{
			case 1: fnPRV_2D_Integral1(tmp_tag, mask_tag, src_tag, buf.Tag()); break;
			case 2: fnPRV_2D_Integral2(tmp_tag, mask_tag, src_tag, buf.Tag()); break;
			}

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
			dst->Resize(src->Width(), src->Height(), TxModel::F64(src->Model().Pack), src->Channels());

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

		bool dst_type_isvalid = false;
		switch(dst->Model().Type)
		{
			case ExType::F64:	dst_type_isvalid = true; break;
			default:
				break;
		}

		CxArray buf(src_tag.Width, TxModel::F64(1));

		if (dst_type_isvalid &&
			dst->Channels() == src->Channels() &&
			dst->Model().Pack == src->Model().Pack)
		{
			switch(this->Mode)
			{
			case 1: fnPRV_2D_Integral1(dst_tag, mask_tag, src_tag, buf.Tag()); break;
			case 2: fnPRV_2D_Integral2(dst_tag, mask_tag, src_tag, buf.Tag()); break;
			}
		}
		else
		{
			CxImage tmp(dst_tag.Width, dst_tag.Height, TxModel::F64(dst_tag.Model.Pack), dst_tag.Channels);
			TxImage tmp_tag = src->Tag();

			switch(this->Mode)
			{
			case 1: fnPRV_2D_Integral1(tmp_tag, mask_tag, src_tag, buf.Tag()); break;
			case 2: fnPRV_2D_Integral2(tmp_tag, mask_tag, src_tag, buf.Tag()); break;
			}

			dst->Filter().Copy(tmp);
		}

		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

}	// Effectors
}	// xie
