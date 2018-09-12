/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/xie_core_ipl.h"
#include "Core/TxIplImage.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"

namespace xie
{

// ============================================================
TxIplImage TxIplImage::Default()
{
	TxIplImage result;
	return result;
}

// ============================================================
TxIplImage::TxIplImage()
{
	memset(this, 0, sizeof(TxIplImage));

	nSize		= sizeof(TxIplImage);
	ID			= 0;
	imageData	= NULL;
	width		= 0;
	height		= 0;
	depth		= 0;
	nChannels	= 0;
	widthStep	= 0;
	dataOrder	= IPLDefs::IPL_DATA_ORDER_PIXEL;
	origin		= IPLDefs::IPL_ORIGIN_TL;
	align		= XIE_IMAGE_PACKING_SIZE;

	alphaChannel	= 0;		// [-] Ignored.
	for(int i=0 ; i<4 ; i++)
		colorModel[i] = 0;		// [-] Ignored.
	for(int i=0 ; i<4 ; i++)
		channelSeq[i] = 0;		// [-] Ignored.
	roi				= NULL;		// [-] Ignored.
	maskROI			= NULL;		// [-] Ignored.
	imageId			= NULL;		// [-] Ignored.
	tileInfo		= NULL;		// [-] Ignored.
	imageSize		= 0;		// [-] Ignored.
	for(int i=0 ; i<4 ; i++)
		BorderMode[i] = 0;		// [-] Ignored.
	for(int i=0 ; i<4 ; i++)
		BorderConst[i] = 0;		// [-] Ignored.
	imageDataOrigin	= NULL;		// [-] Ignored.
}

// ============================================================
TxIplImage::TxIplImage(void* addr, int _width, int _height, int _depth, int channels, int step)
{
	memset(this, 0, sizeof(TxIplImage));

	nSize		= sizeof(TxIplImage);
	ID			= 0;
	imageData	= (char*)addr;
	width		= _width;
	height		= _height;
	depth		= _depth;
	nChannels	= channels;
	widthStep	= step;
	dataOrder	= IPLDefs::IPL_DATA_ORDER_PIXEL;
	origin		= IPLDefs::IPL_ORIGIN_TL;
	align		= XIE_IMAGE_PACKING_SIZE;
}

// ============================================================
TxIplImage::TxIplImage(TxImage src, int ch)
{
	memset(this, 0, sizeof(TxIplImage));

	int	dst_depth = 0;
	switch(src.Model.Type)
	{
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		case ExType::U8:		dst_depth = IPLDefs::IPL_DEPTH_8U; break;
		case ExType::U16:	dst_depth = IPLDefs::IPL_DEPTH_16U; break;
		case ExType::S8:		dst_depth = IPLDefs::IPL_DEPTH_8S; break;
		case ExType::S16:	dst_depth = IPLDefs::IPL_DEPTH_16S; break;
		case ExType::S32:	dst_depth = IPLDefs::IPL_DEPTH_32S; break;
		case ExType::F32:	dst_depth = IPLDefs::IPL_DEPTH_32F; break;
		case ExType::F64:	dst_depth = IPLDefs::IPL_DEPTH_64F; break;
	}

	nSize		= sizeof(TxIplImage);
	ID			= 0;
	imageData	= static_cast<char*>(src.Layer[ch]);
	width		= src.Width;
	height		= src.Height;
	depth		= dst_depth;
	nChannels	= src.Model.Pack;
	widthStep	= src.Stride;
	dataOrder	= IPLDefs::IPL_DATA_ORDER_PIXEL;
	origin		= IPLDefs::IPL_ORIGIN_TL;
	align		= XIE_IMAGE_PACKING_SIZE;
}

// ============================================================
bool TxIplImage::operator == (const TxIplImage& cmp) const
{
	const TxIplImage& src = *this;
	if (src.nSize			!= cmp.nSize) return false;
	if (src.ID				!= cmp.ID) return false;
	if (src.nChannels		!= cmp.nChannels) return false;
	if (src.alphaChannel	!= cmp.alphaChannel) return false;
	if (src.depth	!= cmp.depth) return false;
	for(int i=0 ; i<4 ; i++)
		if (src.colorModel[i] != cmp.colorModel[i]) return false;
	for(int i=0 ; i<4 ; i++)
		if (src.channelSeq[i] != cmp.channelSeq[i]) return false;
	if (src.dataOrder		!= cmp.dataOrder) return false;
	if (src.origin			!= cmp.origin) return false;
	if (src.align			!= cmp.align) return false;
	if (src.width			!= cmp.width) return false;
	if (src.height			!= cmp.height) return false;
	if (src.roi				!= cmp.roi) return false;
	if (src.maskROI			!= cmp.maskROI) return false;
	if (src.imageId			!= cmp.imageId) return false;
	if (src.tileInfo		!= cmp.tileInfo) return false;
	if (src.imageSize		!= cmp.imageSize) return false;
	if (src.imageData		!= cmp.imageData) return false;
	if (src.widthStep		!= cmp.widthStep) return false;
	for(int i=0 ; i<4 ; i++)
		if (src.BorderMode[i] != cmp.BorderMode[i]) return false;
	for(int i=0 ; i<4 ; i++)
		if (src.BorderConst[i] != cmp.BorderConst[i]) return false;
	if (src.imageDataOrigin	!= cmp.imageDataOrigin) return false;
	return true;
}

// ============================================================
bool TxIplImage::operator != (const TxIplImage& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxIplImage::operator TxImage() const
{
	TxImage				dst;
	const TxIplImage&	src = *(this);

	TxModel dst_model = TxModel::Default();
	switch(src.depth)
	{
	case IPLDefs::IPL_DEPTH_8U	: dst_model = TxModel::U8(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_16U	: dst_model = TxModel::U16(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_32F	: dst_model = TxModel::F32(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_64F	: dst_model = TxModel::F64(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_8S	: dst_model = TxModel::S8(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_16S	: dst_model = TxModel::S16(src.nChannels); break;
	case IPLDefs::IPL_DEPTH_32S	: dst_model = TxModel::S32(src.nChannels); break;
	default:
		break;
	}

	dst.Layer[0]	= src.imageData;
	dst.Width		= src.width;
	dst.Height		= src.height;
	dst.Model		= dst_model;
	dst.Channels	= 1;
	dst.Stride		= src.widthStep;
	dst.Depth		= 0;

	return dst;
}

// ============================================================
bool TxIplImage::IsValid() const
{
	CxImage src = CxImage::FromTag((TxImage)*this);
	return src.IsValid();
}

// ============================================================
void TxIplImage::CopyTo(IxModule& dst) const
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
void TxIplImage::CopyFrom(const IxModule& src)
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
XIE_EXPORT_FUNCTION ExBoolean	XIE_API fnXIE_Core_IplImage_Equals(TxIplImage src, TxIplImage cmp)
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
XIE_EXPORT_FUNCTION ExBoolean	XIE_API fnXIE_Core_IplImage_IsValid(TxIplImage src)
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_IplImage_CopyTo(TxIplImage src, HxModule dst)
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_IplImage_CopyFrom(TxIplImage dst, HxModule src)
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
