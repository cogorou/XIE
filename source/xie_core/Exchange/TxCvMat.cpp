/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/xie_core_cv.h"
#include "Core/TxCvMat.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"

namespace xie
{

// ============================================================
TxCvMat TxCvMat::Default()
{
	TxCvMat result;
	return result;
}

// ============================================================
TxCvMat::TxCvMat()
{
	memset(this, 0, sizeof(TxCvMat));
}

// ============================================================
TxCvMat::TxCvMat(void* addr, int _rows, int _cols, int _type, int _step, int* _refcount, int _hdr_refcount)
{
	memset(this, 0, sizeof(TxCvMat));

	type			= _type;
	step			= _step;
	refcount		= _refcount;
	hdr_refcount	= _hdr_refcount;
	ptr				= addr;
	rows			= _rows;
	cols			= _cols;
}

// ============================================================
TxCvMat::TxCvMat(TxImage src, int ch)
{
	memset(this, 0, sizeof(TxCvMat));

	int dst_depth = 0;
	int dst_channels = src.Model.Pack;
	switch(src.Model.Type)
	{
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	case ExType::U8:	dst_depth = CVDefs::CV_8U; break;
	case ExType::U16:	dst_depth = CVDefs::CV_16U; break;
	case ExType::S8:	dst_depth = CVDefs::CV_8S; break;
	case ExType::S16:	dst_depth = CVDefs::CV_16S; break;
	case ExType::S32:	dst_depth = CVDefs::CV_32S; break;
	case ExType::F32:	dst_depth = CVDefs::CV_32F; break;
	case ExType::F64:	dst_depth = CVDefs::CV_64F; break;
	}

	type			= CVDefs::CV_MAKETYPE(dst_depth, dst_channels);
	step			= src.Stride;
	refcount		= NULL;
	hdr_refcount	= 0;
	ptr				= static_cast<char*>(src.Layer[ch]);
	rows			= src.Height;
	cols			= src.Width;
}

// ============================================================
bool TxCvMat::operator == (const TxCvMat& cmp) const
{
	const TxCvMat& src = *this;
	if (src.type			!= cmp.type) return false;
	if (src.step			!= cmp.step) return false;
	if (src.refcount		!= cmp.refcount) return false;
	if (src.hdr_refcount	!= cmp.hdr_refcount) return false;
	if (src.ptr				!= cmp.ptr) return false;
	if (src.rows			!= cmp.rows) return false;
	if (src.cols			!= cmp.cols) return false;
	return true;
}

// ============================================================
bool TxCvMat::operator != (const TxCvMat& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxCvMat::operator TxImage() const
{
	TxImage			dst;
	const TxCvMat&	src = *(this);

	TxModel dst_model = TxModel::Default();
	int src_type	= CVDefs::CV_MAT_DEPTH(src.type);
	int src_pack	= CVDefs::CV_MAT_CN(src.type);
	switch(src_type)
	{
	case CVDefs::CV_8U	: dst_model = TxModel::U8(src_pack); break;
	case CVDefs::CV_16U	: dst_model = TxModel::U16(src_pack); break;
	case CVDefs::CV_32F	: dst_model = TxModel::F32(src_pack); break;
	case CVDefs::CV_64F	: dst_model = TxModel::F64(src_pack); break;
	case CVDefs::CV_8S	: dst_model = TxModel::S8(src_pack); break;
	case CVDefs::CV_16S	: dst_model = TxModel::S16(src_pack); break;
	case CVDefs::CV_32S	: dst_model = TxModel::S32(src_pack); break;
	default:
		break;
	}

	dst.Layer[0]	= src.ptr;
	dst.Width		= src.cols;
	dst.Height		= src.rows;
	dst.Model		= dst_model;
	dst.Channels	= 1;
	dst.Stride		= src.step;
	dst.Depth		= 0;

	return dst;
}

// ============================================================
bool TxCvMat::IsValid() const
{
	CxImage src = CxImage::FromTag((TxImage)*this);
	return src.IsValid();
}

// ============================================================
void TxCvMat::CopyTo(IxModule& dst) const
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (xie::Axi::ClassIs<CxImage>(dst))
	{
		CxImage		_src = CxImage::FromTag((TxImage)*this);
		CxImage&	_dst = static_cast<CxImage&>(dst);
		_dst.CopyFrom(_src);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void TxCvMat::CopyFrom(const IxModule& src)
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (xie::Axi::ClassIs<CxImage>(src))
	{
		auto&	_src = static_cast<const CxImage&>(src);
		auto	_dst = CxImage::FromTag((TxImage)*this);
		_dst.Filter().Copy(_src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// //////////////////////////////////////////////////////////////////////
// Export
//

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean	XIE_API fnXIE_Core_CvMat_Equals(TxCvMat src, TxCvMat cmp)
{
	try
	{
		return (src == cmp) ? ExBoolean::True : ExBoolean::False;
	}
	catch(const CxException&)
	{
		return ExBoolean::False;
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean	XIE_API fnXIE_Core_CvMat_IsValid(TxCvMat src)
{
	try
	{
		return src.IsValid() ? ExBoolean::True : ExBoolean::False;
	}
	catch(const CxException&)
	{
		return ExBoolean::False;
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_CvMat_CopyTo(TxCvMat src, HxModule dst)
{
	auto _dst = xie::Axi::SafeCast<CxModule>(dst);
	if (_dst == NULL)
		return ExStatus::InvalidObject;
	try
	{
		src.CopyTo(*_dst);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_CvMat_CopyFrom(TxCvMat dst, HxModule src)
{
	auto _src = xie::Axi::SafeCast<CxModule>(src);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		dst.CopyFrom(*_src);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
