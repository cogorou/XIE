/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_data.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxScanner2D.h"

// ======================================================================
//
// boilerplate code
//
#ifndef XIE_ELEMENT_OPERATOR
#define XIE_ELEMENT_OPERATOR(left,right)	(double)((left) * (right))
#endif

#include "fnPRV_Core_2D_Convert.h"

namespace xie
{

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Convert(TxImage dst, TxImage mask, TxImage src, double scale)
{
	typedef unsigned char	TM;

	try
	{
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_Convert___<TM>(dst, mask, src, scale);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
