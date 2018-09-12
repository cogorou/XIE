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
#include "Core/CxMatrix.h"
#include "Core/CxImage.h"
#include "Core/TxScanner2D.h"
#include "Effectors/CxIntegral.h"

#include <math.h>

// ======================================================================
#include "fnPRV_Core_2D_Affine0.h"
#include "fnPRV_Core_2D_Affine1.h"
#include "fnPRV_Core_2D_Affine2.h"

namespace xie
{

// ======================================================================
static void fnPRV_2D_Affine0(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef unsigned char	TM;
	typedef double			TK;
	fnPRV_2D_Affine0___<TM,TK> (dst, mask, src, matrix);
}

// ======================================================================
static void fnPRV_2D_Affine1(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef unsigned char	TM;
	typedef double			TK;
	fnPRV_2D_Affine1___<TM,TK> (dst, mask, src, matrix);
}

// ======================================================================
static void fnPRV_2D_Affine2(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef unsigned char	TM;
	typedef double			TS;
	typedef double			TK;
	typedef TxPointI		TR;

	// 変換行列のスケールの抽出.
	TxSizeD mag = CxMatrix::FromTag(matrix).ScaleFactor();
	// 注目画素の周辺.
	TR region = TR(
			(int)((mag.Width  < 2) ? 1 : mag.Width  / 2),
			(int)((mag.Height < 2) ? 1 : mag.Height / 2)
			);
	// 積分画像.
	TxModel buf_model(TypeOf<TS>(), Axi::Min(dst.Model.Pack, src.Model.Pack));
	CxImage sum1(src.Width, src.Height, buf_model, src.Channels);
	CxImage src1;
	src1.Attach(src);
	xie::Effectors::CxIntegral integral(1);	// 1:総和 / 2:２乗の総和.
	integral.Execute(src1, sum1);

	fnPRV_2D_Affine2___<TM,TS,TK,TR> (dst, mask, sum1.Tag(), matrix, region);
}

// ======================================================================
static void XIE_API fnPRV_2D_Affine(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, int interpolation)
{
	switch(interpolation)
	{
	case 0: fnPRV_2D_Affine0(dst, mask, src, matrix); break;
	case 1: fnPRV_2D_Affine1(dst, mask, src, matrix); break;
	case 2: fnPRV_2D_Affine2(dst, mask, src, matrix); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Affine(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, int interpolation)
{
	typedef unsigned char	TM;

	try
	{
		if (MaskValidity<TM>(mask, src) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (matrix.Rows != 3 || matrix.Columns != 3)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		// Invert Matrix
		CxMatrix matrix_org;
		CxMatrix matrix_inv;
		matrix_org.Attach(matrix);
		matrix_inv.Filter().Invert(matrix_org);
		if (matrix_inv.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Affine(dst, mask, src, matrix_inv.Tag(), interpolation);
		}
		else
		{
			TxModel dst_model(src.Model.Type, dst.Model.Pack);
			CxImage dst_tmp(dst.Width, dst.Height, dst_model, dst.Channels);
			fnPRV_2D_Affine(dst_tmp.Tag(), mask, src, matrix_inv.Tag(), interpolation);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Copy(dst_tmp);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
