/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Effectors/CxMonochrome.h"
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

static const char* g_ClassName = "CxMonochrome";

// ===================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Effectors_Monochrome(HxModule hsrc, HxModule hdst, HxModule hmask, double red_ratio, double green_ratio, double blue_ratio)
{
	try
	{
		CxMonochrome effector(red_ratio, green_ratio, blue_ratio);
		effector.Execute(hsrc, hdst, hmask);
		return ExStatus::Success;
	}
	catch(const xie::CxException& ex)
	{
		return ex.Code();
	}
}

// ===================================================================
void CxMonochrome::_Constructor()
{
	this->RedRatio		= 1.0;
	this->GreenRatio	= 1.0;
	this->BlueRatio		= 1.0;
	this->Matrix.Resize(3, 3);
	this->Matrix.Reset();
}

// ===================================================================
CxMonochrome::CxMonochrome()
{
	_Constructor();
}

// ===================================================================
CxMonochrome::CxMonochrome(double red_ratio, double green_ratio, double blue_ratio)
{
	_Constructor();

	this->RedRatio		= red_ratio;
	this->GreenRatio	= green_ratio;
	this->BlueRatio		= blue_ratio;
}

// ===================================================================
CxMonochrome::CxMonochrome(const CxMonochrome& src)
{
	operator = (src);
}

// ============================================================
CxMonochrome::~CxMonochrome()
{
}

// ===================================================================
CxMonochrome& CxMonochrome::operator = ( const CxMonochrome& src )
{
	if (this == &src) return *this;
	CopyFrom(src);
	return *this;
}

// ===================================================================
bool CxMonochrome::operator == ( const CxMonochrome& src ) const
{
	return ContentEquals(src);
}

// ===================================================================
bool CxMonochrome::operator != ( const CxMonochrome& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
void CxMonochrome::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxMonochrome>(src))
	{
		auto&	_src = static_cast<const CxMonochrome&>(src);
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
bool CxMonochrome::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxMonochrome>(src))
	{
		auto& _src = static_cast<const CxMonochrome&>(src);
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
void CxMonochrome::Execute(HxModule hsrc, HxModule hdst, HxModule hmask) const
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
void CxMonochrome::SetupMatrix() const
{
	if (this->Matrix.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Rows() != 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Columns() != 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (this->Matrix.Model() != TxModel::F64(1))
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	const double coefR = 0.299;
	const double coefG = 0.587;
	const double coefB = 0.114;
	auto R = RedRatio;
	auto G = GreenRatio;
	auto B = BlueRatio;

	double src_data[3][3] =
	{
		{ coefR * R, coefR * G, coefR * B },
		{ coefG * R, coefG * G, coefG * B },
		{ coefB * R, coefB * G, coefB * B },
	};

	auto dst_scan = this->Matrix.Scanner<double>();
	for(int r=0 ; r<3 ; r++)
		for(int c=0 ; c<3 ; c++)
			dst_scan(r, c) = src_data[r][c];
}

}	// Effectors
}	// xie
