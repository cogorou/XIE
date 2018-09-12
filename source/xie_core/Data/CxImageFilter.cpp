/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/CxImageFilter.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
CxImageFilter::CxImageFilter()
{
	hDst = NULL;
	hMask = NULL;
}

// ============================================================
CxImageFilter::CxImageFilter(HxModule hdst, HxModule hmask)
{
	hDst = hdst;
	hMask = hmask;
}

// ============================================================
CxImageFilter::CxImageFilter(const CxImageFilter& src)
{
	operator = (src);
}

// ============================================================
CxImageFilter::~CxImageFilter()
{
}

// ============================================================
CxImageFilter& CxImageFilter::operator = ( const CxImageFilter& src )
{
	this->hDst = src.hDst;
	this->hMask = src.hMask;
	return *this;
}

// ============================================================
bool CxImageFilter::operator == ( const CxImageFilter& src ) const
{
	if (this->hDst != src.hDst) return false;
	if (this->hMask != src.hMask) return false;
	return true;
}

// ============================================================
bool CxImageFilter::operator != ( const CxImageFilter& src ) const
{
	return !(CxImageFilter::operator == (src));
}

// ============================================================
void CxImageFilter::Cast(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Cast(this->hDst, this->hMask, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Copy(HxModule hsrc, double scale)
{
	ExStatus status = fnXIE_Core_Image_Copy(this->hDst, this->hMask, hsrc, scale);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::CopyEx(HxModule hsrc, int index, int count)
{
	ExStatus status = fnXIE_Core_Image_CopyEx(this->hDst, this->hMask, hsrc, index, count);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::RgbToBgr(HxModule hsrc, double scale)
{
	ExStatus status = fnXIE_Core_Image_RgbToBgr(this->hDst, this->hMask, hsrc, scale);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Compare(HxModule hsrc, HxModule hcmp, double error_range)
{
	ExStatus status = fnXIE_Core_Image_Compare(this->hDst, this->hMask, hsrc, hcmp, error_range);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::ColorMatrix(HxModule hsrc, HxModule hmatrix)
{
	ExStatus status = fnXIE_Core_Image_ColorMatrix(this->hDst, this->hMask, hsrc, hmatrix);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Affine(HxModule hsrc, HxModule hmatrix, int interpolation)
{
	ExStatus status = fnXIE_Core_Image_Affine(this->hDst, this->hMask, hsrc, hmatrix, interpolation);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mirror(HxModule hsrc, int mode)
{
	ExStatus status = fnXIE_Core_Image_Mirror(this->hDst, this->hMask, hsrc, mode);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Rotate(HxModule hsrc, int mode)
{
	ExStatus status = fnXIE_Core_Image_Rotate(this->hDst, this->hMask, hsrc, mode);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Transpose(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Transpose(this->hDst, this->hMask, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Scale(HxModule hsrc, double sx, double sy, int interpolation)
{
	ExStatus status = fnXIE_Core_Image_Scale(this->hDst, this->hMask, hsrc, sx, sy, interpolation);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Math		(HxModule hsrc, ExMath type)
{
	ExStatus status = fnXIE_Core_Image_Math(this->hDst, this->hMask, hsrc, type);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Not		(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Not(this->hDst, this->hMask, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Add		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Add		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mul		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mul		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Sub		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Sub		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Sub		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::SubInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Div		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Div		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Div		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::DivInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mod		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mod		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Mod		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::ModInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Pow		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Pow		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Pow		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::PowInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Atan2		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Atan2		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Atan2		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Diff		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Diff		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Max		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Max		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Min		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2A(this->hDst, this->hMask, hsrc, hval, ExOpe2A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Min		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Image_Ope1A(this->hDst, this->hMask, hsrc, value, ExOpe1A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::And		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2L(this->hDst, this->hMask, hsrc, hval, ExOpe2L::And);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::And		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Image_Ope1L(this->hDst, this->hMask, hsrc, value, ExOpe1L::And);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Nand		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2L(this->hDst, this->hMask, hsrc, hval, ExOpe2L::Nand);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Nand		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Image_Ope1L(this->hDst, this->hMask, hsrc, value, ExOpe1L::Nand);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Or		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2L(this->hDst, this->hMask, hsrc, hval, ExOpe2L::Or);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Or		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Image_Ope1L(this->hDst, this->hMask, hsrc, value, ExOpe1L::Or);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Xor		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Image_Ope2L(this->hDst, this->hMask, hsrc, hval, ExOpe2L::Xor);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImageFilter::Xor		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Image_Ope1L(this->hDst, this->hMask, hsrc, value, ExOpe1L::Xor);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

}
