/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArray.h"
#include "Core/CxArrayFilter.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
CxArrayFilter::CxArrayFilter()
{
	hDst = NULL;
}

// ============================================================
CxArrayFilter::CxArrayFilter(HxModule hdst)
{
	hDst = hdst;
}

// ============================================================
CxArrayFilter::CxArrayFilter(const CxArrayFilter& src)
{
	operator = (src);
}

// ============================================================
CxArrayFilter::~CxArrayFilter()
{
}

// ============================================================
CxArrayFilter& CxArrayFilter::operator = ( const CxArrayFilter& src )
{
	this->hDst = src.hDst;
	return *this;
}

// ============================================================
bool CxArrayFilter::operator == ( const CxArrayFilter& src ) const
{
	if (this->hDst != src.hDst) return false;
	return true;
}

// ============================================================
bool CxArrayFilter::operator != ( const CxArrayFilter& src ) const
{
	return !(CxArrayFilter::operator == (src));
}

// ============================================================
void CxArrayFilter::Cast(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Cast(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Copy(HxModule hsrc, double scale)
{
	ExStatus status = fnXIE_Core_Array_Copy(this->hDst, hsrc, scale);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::CopyEx(HxModule hsrc, int index, int count)
{
	ExStatus status = fnXIE_Core_Array_CopyEx(this->hDst, hsrc, index, count);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::RgbToBgr(HxModule hsrc, double scale)
{
	ExStatus status = fnXIE_Core_Array_RgbToBgr(this->hDst, hsrc, scale);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Compare(HxModule hsrc, HxModule hcmp, double error_range)
{
	ExStatus status = fnXIE_Core_Array_Compare(this->hDst, hsrc, hcmp, error_range);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::ColorMatrix(HxModule hsrc, HxModule hmatrix)
{
	ExStatus status = fnXIE_Core_Array_ColorMatrix(this->hDst, hsrc, hmatrix);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Math		(HxModule hsrc, ExMath type)
{
	ExStatus status = fnXIE_Core_Array_Math(this->hDst, hsrc, type);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Not		(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Not(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Add		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Add		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Mul		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Mul		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Sub		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Sub		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Sub		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::SubInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Div		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Div		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Div		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::DivInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Mod		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Mod		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Mod		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::ModInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Pow		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Pow		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Pow		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::PowInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Atan2		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Atan2		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Atan2		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Diff		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Diff		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Max		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Max		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Min		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Min		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Array_Ope1A(this->hDst, hsrc, value, ExOpe1A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::And		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2L(this->hDst, hsrc, hval, ExOpe2L::And);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::And		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Array_Ope1L(this->hDst, hsrc, value, ExOpe1L::And);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Nand		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2L(this->hDst, hsrc, hval, ExOpe2L::Nand);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Nand		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Array_Ope1L(this->hDst, hsrc, value, ExOpe1L::Nand);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Or		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2L(this->hDst, hsrc, hval, ExOpe2L::Or);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Or		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Array_Ope1L(this->hDst, hsrc, value, ExOpe1L::Or);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Xor		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Array_Ope2L(this->hDst, hsrc, hval, ExOpe2L::Xor);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArrayFilter::Xor		(HxModule hsrc, unsigned int value)
{
	ExStatus status = fnXIE_Core_Array_Ope1L(this->hDst, hsrc, value, ExOpe1L::Xor);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

}
