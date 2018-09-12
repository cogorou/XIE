/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_EXIF_H_INCLUDED_
#define _API_EXIF_H_INCLUDED_

#include "api_core.h"
#include "Core/TxExif.h"
#include "Core/TxExifItem.h"
#include "Core/TxScanner1D.h"

namespace xie
{

// ////////////////////////////////////////////////////////////
// FUNCTION

TxExifItem XIE_API fnPRV_Exif_GetItem(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type);

unsigned short XIE_API fnPRV_Exif_GetValueU16(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type);
unsigned int XIE_API fnPRV_Exif_GetValueU32(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type);

void XIE_API fnPRV_Exif_SetValueU16(TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type, unsigned short value);
void XIE_API fnPRV_Exif_SetValueU32(TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type, unsigned int value);

void XIE_API fnPRV_Exif_CopyRelatedItem(TxScanner1D<unsigned char>& dst_scan, const TxScanner1D<unsigned char>& src_scan, const TxExifItem& item, int offset);

// ============================================================
template<class TD> TD XIE_API fnPRV_Exif_GetValue(const unsigned char* src, ExEndianType endian_type)
{
	int bytes = sizeof(TD);
	TD dst = (TD)0;
	if (endian_type == ExEndianType::LE)
	{
		for(int i=0 ; i<bytes ; i++)
			dst |= (src[i] << (8 * (bytes - i - 1)));
	}
	else
	{
		for(int i=0 ; i<bytes ; i++)
			dst |= (src[i] << (8 * i));
	}
	return dst;
}

// ============================================================
template<class TD> void XIE_API fnPRV_Exif_GetValues(TxScanner1D<TD>& dst_scan, const TxScanner1D<unsigned char>& src_scan, const TxExifItem& item, int offset)
{
	auto unit = xie::Axi::SizeOf(dst_scan.Model.Type);
	auto pack = dst_scan.Model.Pack;
	auto size = dst_scan.Model.Size();	// unit x pack

	if ((item.Count * size) <= (int)sizeof(item.ValueOrIndex))
	{
		for(int i=0 ; i<dst_scan.Length ; i++)
		{
			auto dst_addr = &dst_scan[i];
			for(int k=0 ; k<pack ; k++)
			{
				auto src_index = item.Offset + 8 + (size * i + unit * k);
				auto src_addr = &src_scan[src_index];
				dst_addr[k] = fnPRV_Exif_GetValue<TD>(src_addr, item.EndianType);
			}
		}
	}
	else
	{
		for(int i=0 ; i<dst_scan.Length ; i++)
		{
			auto dst_addr = &dst_scan[i];
			for(int k=0 ; k<pack ; k++)
			{
				auto src_index = offset + item.ValueOrIndex + (size * i + unit * k);
				auto src_addr = &src_scan[src_index];
				dst_addr[k] = fnPRV_Exif_GetValue<TD>(src_addr, item.EndianType);
			}
		}
	}
}

// ============================================================
template<class TS> void XIE_API fnPRV_Exif_SetValue(unsigned char* dst, const TS* src, ExEndianType endian_type)
{
	int bytes = sizeof(TS);
	if (endian_type == ExEndianType::LE)
	{
		for(int i=0 ; i<bytes ; i++)
			dst[i] = 0xFF & (*src >> (8 * (bytes - i - 1)));
	}
	else
	{
		for(int i=0 ; i<bytes ; i++)
			dst[i] = 0xFF & (*src >> (8 * i));
	}
}

// ============================================================
template<class TS> void XIE_API fnPRV_Exif_SetValues(TxScanner1D<unsigned char>& dst_scan, const TxScanner1D<TS>& src_scan, const TxExifItem& item, int offset)
{
	auto unit = xie::Axi::SizeOf(src_scan.Model.Type);
	auto pack = src_scan.Model.Pack;
	auto size = src_scan.Model.Size();	// unit x pack

	if ((item.Count * size) <= (int)sizeof(item.ValueOrIndex))
	{
		for(int i=0 ; i<src_scan.Length ; i++)
		{
			auto src_addr = &src_scan[i];
			for(int k=0 ; k<pack ; k++)
			{
				auto dst_index = item.Offset + 8 + (size * i + unit * k);
				auto dst_addr = &dst_scan[dst_index];
				fnPRV_Exif_SetValue<TS>(dst_addr, &src_addr[k], item.EndianType);
			}
		}
	}
	else
	{
		for(int i=0 ; i<src_scan.Length ; i++)
		{
			auto src_addr = &src_scan[i];
			for(int k=0 ; k<pack ; k++)
			{
				auto dst_index = offset + item.ValueOrIndex + (size * i + unit * k);
				auto dst_addr = &dst_scan[dst_index];
				fnPRV_Exif_SetValue<TS>(dst_addr, &src_addr[k], item.EndianType);
			}
		}
	}
}

// ////////////////////////////////////////////////////////////
// PROTOTYPE

// ======================================================================

// Exif - Basic
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_Attach	(HxModule handle, TxExif src);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_Resize	(HxModule handle, int length);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_Reset		(HxModule handle);

// Exif - Get/Set
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_EndianType	(HxModule handle, ExEndianType* value);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetItems		(HxModule handle, HxModule hval);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetPurgedExif	(HxModule handle, HxModule hval);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetValue		(HxModule handle, TxExifItem item, HxModule hval);
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_SetValue		(HxModule handle, TxExifItem item, HxModule hval);

}

#endif
