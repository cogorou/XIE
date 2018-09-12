/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxMatrix.h"
#include "Core/CxMatrixFilter.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
CxMatrixFilter::CxMatrixFilter()
{
	hDst = NULL;
}

// ============================================================
CxMatrixFilter::CxMatrixFilter(HxModule hdst)
{
	hDst = hdst;
}

// ============================================================
CxMatrixFilter::CxMatrixFilter(const CxMatrixFilter& src)
{
	operator = (src);
}

// ============================================================
CxMatrixFilter::~CxMatrixFilter()
{
}

// ============================================================
CxMatrixFilter& CxMatrixFilter::operator = ( const CxMatrixFilter& src )
{
	this->hDst = src.hDst;
	return *this;
}

// ============================================================
bool CxMatrixFilter::operator == ( const CxMatrixFilter& src ) const
{
	if (this->hDst != src.hDst) return false;
	return true;
}

// ============================================================
bool CxMatrixFilter::operator != ( const CxMatrixFilter& src ) const
{
	return !(CxMatrixFilter::operator == (src));
}

// ============================================================
void CxMatrixFilter::Cast(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Cast(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Copy(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Copy(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Compare(HxModule hsrc, HxModule hcmp, double error_range)
{
	ExStatus status = fnXIE_Core_Matrix_Compare(this->hDst, hsrc, hcmp, error_range);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mirror(HxModule hsrc, int mode)
{
	ExStatus status = fnXIE_Core_Matrix_Mirror(this->hDst, hsrc, mode);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Rotate(HxModule hsrc, int mode)
{
	ExStatus status = fnXIE_Core_Matrix_Rotate(this->hDst, hsrc, mode);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Transpose(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Transpose(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Invert(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Invert(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Submatrix(HxModule hsrc, int row, int col)
{
	ExStatus status = fnXIE_Core_Matrix_Submatrix(this->hDst, hsrc, row, col);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Product(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Product(this->hDst, hsrc, hval);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Math		(HxModule hsrc, ExMath type)
{
	ExStatus status = fnXIE_Core_Matrix_Math(this->hDst, hsrc, type);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Not		(HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Not(this->hDst, hsrc);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Add		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Add		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Add);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mul		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mul		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Mul);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Sub		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Sub		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Sub);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Sub		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::SubInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Div		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Div		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Div);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Div		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::DivInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mod		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mod		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Mod);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Mod		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::ModInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Pow		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Pow		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Pow);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Pow		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::PowInv);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Atan2		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Atan2		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Atan2		(double value, HxModule hsrc)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Atan2);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Diff		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Diff		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Diff);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Max		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Max		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Max);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Min		(HxModule hsrc, HxModule hval)
{
	ExStatus status = fnXIE_Core_Matrix_Ope2A(this->hDst, hsrc, hval, ExOpe2A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrixFilter::Min		(HxModule hsrc, double value)
{
	ExStatus status = fnXIE_Core_Matrix_Ope1A(this->hDst, hsrc, value, ExOpe1A::Min);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

}
