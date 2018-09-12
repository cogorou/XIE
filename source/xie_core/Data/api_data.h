/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_DATA_H_INCLUDED_
#define _API_DATA_H_INCLUDED_

#include "api_core.h"

namespace xie
{

// ////////////////////////////////////////////////////////////
// FUNCTION

// ======================================================================
static inline TxImage To2D(TxArray src)
{
	return TxImage(TxLayer(src.Address), src.Length, 1, src.Model, 1, src.Length * src.Model.Size(), 0);
}
static inline TxImage To2D(TxMatrix src)
{
	return TxImage(TxLayer(src.Address), src.Columns, src.Rows, src.Model, 1, src.Stride, 0); 
}

// ======================================================================
template<class TE> static inline TxScanner1D<TE> ToScanner(TxArray src)
{
	return TxScanner1D<TE>((TE*)src.Address, src.Length, src.Model);
}
template<class TE> static inline TxScanner2D<TE> ToScanner(TxImage src, int ch)
{
	return TxScanner2D<TE>((TE*)src.Layer[ch], src.Width, src.Height, src.Stride, src.Model);
}
template<class TE> static inline TxScanner2D<TE> ToScanner(TxMatrix src)
{
	return TxScanner2D<TE>((TE*)src.Address, src.Columns, src.Rows, src.Stride, src.Model);
}

// ======================================================================
template<class TM> static inline bool MaskValidity(TxImage mask, TxImage src)
{
	if (mask.Channels != 0)
	{
		if (mask.Channels != 1 && mask.Channels != src.Channels) return false;
		if (mask.Width != src.Width) return false;
		if (mask.Height != src.Height) return false;
		if (mask.Model != xie::ModelOf<TM>()) return false;
	}
	return true;
}

// ======================================================================
static inline int ElementType(TxImage src)
{
	if (src.Model.Pack == 1 && src.Channels != 1)
		return 1;
	if (src.Model.Pack != 1 && src.Channels == 1)
		return 2;
	if (src.Model.Pack != 1 && src.Channels != 1)
		return 3;
	return 0;
}

// ============================================================
static inline bool IsCompatible(TxMatrix src)
{
	if (src.Model	!= TxModel::F64(1)) return false;
	if (src.Stride	!= (src.Model.Size() * src.Columns)) return false;
	return true;
}

// ============================================================
template<class TS> static inline bool fnPRV_Field_GetParam(void* dst, TxModel model, const TS& src)
{
	if (model == ModelOf<TS>())
	{
		*static_cast<TS*>(dst) = src;
		return true;
	}
	return false;
}

// ============================================================
template<class TD> static inline bool fnPRV_Field_SetParam(const void* src, TxModel model, TD& dst)
{
	if (model == ModelOf<TD>())
	{
		dst = (*static_cast<const TD*>(src));
		return true;
	}
	return false;
}

// ============================================================
template<class TCLASS, class TS> static inline bool fnPRV_Method_GetParam(void* dst, TxModel model, const TCLASS* owner, TS (TCLASS::*src)(void) const)
{
	if (model == ModelOf<TS>())
	{
		*static_cast<TS*>(dst) = (owner->*src)();
		return true;
	}
	return false;
}

// ============================================================
template<class TCLASS, class TD> static inline bool fnPRV_Method_SetParam(const void* src, TxModel model, TCLASS* owner, void (TCLASS::*dst)(TD value))
{
	if (model == ModelOf<TD>())
	{
		(owner->*dst)(*static_cast<const TD*>(src));
		return true;
	}
	return false;
}

// ////////////////////////////////////////////////////////////
// PROTOTYPE

// ======================================================================
// 2D

ExStatus XIE_API fnPRV_Core_2D_Serialize	(TxArray dst, TxImage src);
ExStatus XIE_API fnPRV_Core_2D_Statistics	(TxImage src, TxImage mask, int ch, TxStatistics* result);
ExStatus XIE_API fnPRV_Core_2D_Extract		(TxImage src, TxImage mask, int ch, int sy, int sx, int length, ExScanDir dir, TxArray result);
ExStatus XIE_API fnPRV_Core_2D_Projection	(TxImage src, TxImage mask, int ch, ExScanDir dir, TxArray result);

ExStatus XIE_API fnPRV_Core_2D_Clear		(TxImage dst, TxImage mask, const void* value, TxModel model);
ExStatus XIE_API fnPRV_Core_2D_ClearEx		(TxImage dst, TxImage mask, const void* value, TxModel model, int index, int count);
ExStatus XIE_API fnPRV_Core_2D_Cast			(TxImage dst, TxImage mask, TxImage src);
ExStatus XIE_API fnPRV_Core_2D_Copy			(TxImage dst, TxImage mask, TxImage src);
ExStatus XIE_API fnPRV_Core_2D_CopyEx		(TxImage dst, TxImage mask, TxImage src, int index, int count);
ExStatus XIE_API fnPRV_Core_2D_Convert		(TxImage dst, TxImage mask, TxImage src, double scale);
ExStatus XIE_API fnPRV_Core_2D_RgbToBgr		(TxImage dst, TxImage mask, TxImage src, double scale);
ExStatus XIE_API fnPRV_Core_2D_Math			(TxImage dst, TxImage mask, TxImage src, ExMath mode);
ExStatus XIE_API fnPRV_Core_2D_Not			(TxImage dst, TxImage mask, TxImage src);
ExStatus XIE_API fnPRV_Core_2D_Ope1A		(TxImage dst, TxImage mask, TxImage src, double value, ExOpe1A mode);
ExStatus XIE_API fnPRV_Core_2D_Ope1L		(TxImage dst, TxImage mask, TxImage src, unsigned int value, ExOpe1L mode);
ExStatus XIE_API fnPRV_Core_2D_Ope2A		(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2A mode);
ExStatus XIE_API fnPRV_Core_2D_Ope2L		(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2L mode);

ExStatus XIE_API fnPRV_Core_2D_Compare		(TxImage dst, TxImage mask, TxImage src, TxImage cmp, double error_range);
ExStatus XIE_API fnPRV_Core_2D_ColorMatrix	(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix);
ExStatus XIE_API fnPRV_Core_2D_Affine		(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, int interpolation);
ExStatus XIE_API fnPRV_Core_2D_Mirror		(TxImage dst, TxImage mask, TxImage src, int mode);
ExStatus XIE_API fnPRV_Core_2D_Rotate		(TxImage dst, TxImage mask, TxImage src, int mode);
ExStatus XIE_API fnPRV_Core_2D_Transpose	(TxImage dst, TxImage mask, TxImage src);

// ======================================================================

// Array - Measure
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Statistics		(HxModule hsrc, int ch, TxStatistics* result);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Extract		(HxModule hsrc, int index, int length, HxModule hresult);

// Array - Basic
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Reset			(HxModule hdst);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Clear			(HxModule hdst, const void* value, TxModel model);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_ClearEx		(HxModule hdst, const void* value, TxModel model, int index, int count);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Cast			(HxModule hdst, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Copy			(HxModule hdst, HxModule hsrc, double scale);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_CopyEx			(HxModule hdst, HxModule hsrc, int index, int count);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_RgbToBgr		(HxModule hdst, HxModule hsrc, double scale);

// Array - Segmentation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Compare		(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range);

// Array - Filter
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_ColorMatrix	(HxModule hdst, HxModule hsrc, HxModule hmatrix);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Effect			(HxModule hdst, HxModule hsrc, HxModule hparam);

// Array - Operation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Not			(HxModule hdst, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Math			(HxModule hdst, HxModule hsrc, ExMath mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope1A			(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope1L			(HxModule hdst, HxModule hsrc, unsigned int value, ExOpe1L mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope2A			(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope2L			(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2L mode);

// ======================================================================

// Image - Measure
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_CalcDepth		(HxModule hsrc, HxModule hmask, int ch, int* result);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Statistics		(HxModule hsrc, HxModule hmask, int ch, TxStatistics* result);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Extract		(HxModule hsrc, HxModule hmask, int ch, int sy, int sx, int length, ExScanDir dir, HxModule hresult);

// Image - Basic
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Reset			(HxModule hdst);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Clear			(HxModule hdst, HxModule hmask, const void* value, TxModel model);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_ClearEx		(HxModule hdst, HxModule hmask, const void* value, TxModel model, int index, int count);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Cast			(HxModule hdst, HxModule hmask, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Copy			(HxModule hdst, HxModule hmask, HxModule hsrc, double scale);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_CopyEx			(HxModule hdst, HxModule hmask, HxModule hsrc, int index, int count);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_RgbToBgr		(HxModule hdst, HxModule hmask, HxModule hsrc, double scale);

// Image - Segmentation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Compare		(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hcmp, double error_range);

// Image - Filter
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_ColorMatrix	(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix);

// Image - GeoTrans
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Affine			(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix, int interpolation);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Mirror			(HxModule hdst, HxModule hmask, HxModule hsrc, int mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Rotate			(HxModule hdst, HxModule hmask, HxModule hsrc, int mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Transpose		(HxModule hdst, HxModule hmask, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Scale			(HxModule hdst, HxModule hmask, HxModule hsrc, double sx, double sy, int interpolation);

// Image - Operation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Not			(HxModule hdst, HxModule hmask, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Math			(HxModule hdst, HxModule hmask, HxModule hsrc, ExMath mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope1A			(HxModule hdst, HxModule hmask, HxModule hsrc, double value, ExOpe1A mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope1L			(HxModule hdst, HxModule hmask, HxModule hsrc, unsigned int value, ExOpe1L mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope2A			(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2A mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope2L			(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2L mode);

// Image - Exchange
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_FromDIB		(HxModule hdst, const void* dib);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_ToDIB			(HxModule hsrc, void* dib);

// ======================================================================

// Matrix - Measure
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Statistics	(HxModule hsrc, int ch, TxStatistics* result);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Extract		(HxModule hsrc, int row, int col, int length, ExScanDir dir, HxModule hresult);

// Matrix - Basic
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Reset			(HxModule hdst);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Clear			(HxModule hdst, const void* value, TxModel model);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Eye			(HxModule hdst, double value, int mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Cast			(HxModule hdst, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Copy			(HxModule hdst, HxModule hsrc);

// Matrix - Segmentation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Compare		(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range);

// Matrix - GeoTrans
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Mirror		(HxModule hdst, HxModule hsrc, int mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Rotate		(HxModule hdst, HxModule hsrc, int mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Transpose		(HxModule hdst, HxModule hsrc);

// Matrix - Linear
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Det			(HxModule hsrc, double* value);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Trace			(HxModule hsrc, double* value);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_ScaleFactor	(HxModule hsrc, TxSizeD* value);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Invert		(HxModule hdst, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Submatrix		(HxModule hdst, HxModule hsrc, int row, int col);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Product		(HxModule hdst, HxModule hsrc, HxModule hval);

// Matrix - Operation
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Not			(HxModule hdst, HxModule hsrc);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Math			(HxModule hdst, HxModule hsrc, ExMath mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Ope1A			(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Ope2A			(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode);

// Matrix - Preset
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Rotate		(HxModule hdst, double degree, double axis_x, double axis_y);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Scale		(HxModule hdst, double sx, double sy);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Translate	(HxModule hdst, double tx, double ty);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Shear		(HxModule hdst, double degree_x, double degree_y);

// ======================================================================

// String
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_String_Resize		(HxModule handle, int length);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_String_Reset			(HxModule handle);

}

#endif
