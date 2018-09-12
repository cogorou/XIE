/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxRgbConverter.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Data/api_data.h"

#pragma warning (disable:4100)	// ˆø”‚ÍŠÖ”‚Ì–{‘Ì•”‚Å 1 “x‚àŽQÆ‚³‚ê‚Ü‚¹‚ñ.

namespace xie
{
namespace Effectors
{

static const char* g_ClassName = "CxRgbConverter";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_RgbConverter(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio)
{
	try
	{
		CxRgbConverter effector(red_ratio, green_ratio, blue_ratio);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
void CxRgbConverter::_Constructor()
{
	this->RedRatio		= 1.0;
	this->GreenRatio	= 1.0;
	this->BlueRatio		= 1.0;
	this->Matrix.Resize(3, 3);
	this->Matrix.Reset();
}

// ===================================================================
CxRgbConverter::CxRgbConverter()
{
	_Constructor();
}

// ============================================================
CxRgbConverter::CxRgbConverter(double red_ratio, double green_ratio, double blue_ratio)
{
	_Constructor();

	this->RedRatio		= red_ratio;
	this->GreenRatio	= green_ratio;
	this->BlueRatio		= blue_ratio;
}

// ===================================================================
CxRgbConverter::CxRgbConverter(const CxRgbConverter& src)
{
	operator = (src);
}

// ============================================================
CxRgbConverter::~CxRgbConverter()
{
}

// ===================================================================
CxRgbConverter& CxRgbConverter::operator = ( const CxRgbConverter& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxRgbConverter::operator == ( const CxRgbConverter& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxRgbConverter::operator != ( const CxRgbConverter& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxRgbConverter::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxRgbConverter>(src))
	{
		auto&	_src = static_cast<const CxRgbConverter&>(src);
		auto&	_dst = *this;
		_dst.RedRatio	= _src.RedRatio;
		_dst.GreenRatio	= _src.GreenRatio;
		_dst.BlueRatio	= _src.BlueRatio;
		_dst.Matrix		= _src.Matrix;
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
bool CxRgbConverter::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxRgbConverter>(src))
	{
		auto& _src = static_cast<const CxRgbConverter&>(src);
		auto& _dst = *this;
		if (_dst.RedRatio	!= _src.RedRatio) return false;
		if (_dst.GreenRatio	!= _src.GreenRatio) return false;
		if (_dst.BlueRatio	!= _src.BlueRatio) return false;
		if (_dst.Matrix		!= _src.Matrix) return false;
		return true;
	}
	return false;
}

// ===================================================================
void CxRgbConverter::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
{
	this->SetupMatrix();

	if (auto dst = xie::Axi::SafeCast<CxArray>(hdst))
	{
		dst->Filter().ColorMatrix(hsrc, Matrix);
		return;
	}

	if (auto dst = xie::Axi::SafeCast<CxImage>(hdst))
	{
		dst->Filter(hmask).ColorMatrix(hsrc, Matrix);
		return;
	}

	throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxRgbConverter::SetupMatrix() const
{
	if (this->Matrix.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Rows() != 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Columns() != 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Model() != TxModel::F64(1))
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	auto R = RedRatio;
	auto G = GreenRatio;
	auto B = BlueRatio;

	double src_data[3][3] =
	{
		{ R, 0, 0 },
		{ 0, G, 0 },
		{ 0, 0, B },
	};

	auto dst_scan = this->Matrix.Scanner<double>();
	for(int r=0 ; r<3 ; r++)
		for(int c=0 ; c<3 ; c++)
			dst_scan(r, c) = src_data[r][c];
}

}	// Effectors
}	// xie
