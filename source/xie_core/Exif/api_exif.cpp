/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_exif.h"
#include "Core/CxExif.h"
#include "Core/CxArray.h"
#include "Core/CxArrayEx.h"

namespace xie
{

// ////////////////////////////////////////////////////////////
// FUNCTION

// ============================================================
TxExifItem XIE_API fnPRV_Exif_GetItem(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type)
{
	auto id = (unsigned short)fnPRV_Exif_GetValueU16(scan, offset + 0, endian_type);
	auto type = (short)fnPRV_Exif_GetValueU16(scan, offset + 2, endian_type);
	auto count = (int)fnPRV_Exif_GetValueU32(scan, offset + 4, endian_type);
	auto value = (int)fnPRV_Exif_GetValueU32(scan, offset + 8, endian_type);
	return TxExifItem(offset, endian_type, id, type, count, value);
}

// ============================================================
unsigned short XIE_API fnPRV_Exif_GetValueU16(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type)
{
	if (endian_type == ExEndianType::LE)
		return (unsigned short)(scan[offset + 0] << 8 | scan[offset + 1]);
	else
		return (unsigned short)(scan[offset + 1] << 8 | scan[offset + 0]);
}

// ============================================================
unsigned int XIE_API fnPRV_Exif_GetValueU32(const TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type)
{
	if (endian_type == ExEndianType::LE)
		return (unsigned int)(scan[offset + 0] << 24 | scan[offset + 1] << 16 | scan[offset + 2] << 8 | scan[offset + 3]);
	else
		return (unsigned int)(scan[offset + 3] << 24 | scan[offset + 2] << 16 | scan[offset + 1] << 8 | scan[offset + 0]);
}

// ============================================================
void XIE_API fnPRV_Exif_SetValueU16(TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type, unsigned short value)
{
	if (endian_type == ExEndianType::LE)
	{
		scan[offset + 1] = (unsigned char)((value >> 0) & 0xFF);
		scan[offset + 0] = (unsigned char)((value >> 8) & 0xFF);
	}
	else
	{
		scan[offset + 0] = (unsigned char)((value >> 0) & 0xFF);
		scan[offset + 1] = (unsigned char)((value >> 8) & 0xFF);
	}
}

// ============================================================
void XIE_API fnPRV_Exif_SetValueU32(TxScanner1D<unsigned char>& scan, int offset, ExEndianType endian_type, unsigned int value)
{
	if (endian_type == ExEndianType::LE)
	{
		scan[offset + 3] = (unsigned char)((value >> 0) & 0xFF);
		scan[offset + 2] = (unsigned char)((value >> 8) & 0xFF);
		scan[offset + 1] = (unsigned char)((value >> 16) & 0xFF);
		scan[offset + 0] = (unsigned char)((value >> 24) & 0xFF);
	}
	else
	{
		scan[offset + 0] = (unsigned char)((value >> 0) & 0xFF);
		scan[offset + 1] = (unsigned char)((value >> 8) & 0xFF);
		scan[offset + 2] = (unsigned char)((value >> 16) & 0xFF);
		scan[offset + 3] = (unsigned char)((value >> 24) & 0xFF);
	}
}

// ============================================================
void XIE_API fnPRV_Exif_CopyRelatedItem(TxScanner1D<unsigned char>& dst_scan, const TxScanner1D<unsigned char>& src_scan, const TxExifItem& item, int offset)
{
	switch(item.Type)
	{
	case 1:		// BYTE
	case 7:		// UNDEFINED
		if (item.Count > 4)
		{
			int index = offset + item.ValueOrIndex;
			int size = 1 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 2:		// ASCII
		if (item.Count > 4)
		{
			int index = offset + item.ValueOrIndex;
			int size = 1 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 3:		// SHORT
		if (item.Count > 2)
		{
			int index = offset + item.ValueOrIndex;
			int size = 2 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 4:		// LONG
		if (item.Count > 1)
		{
			int index = offset + item.ValueOrIndex;
			int size = 4 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 9:		// SLONG A 32-bit
		if (item.Count > 1)
		{
			int index = offset + item.ValueOrIndex;
			int size = 4 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 5:		// RATIONAL
		if (item.Count >= 1)
		{
			int index = offset + item.ValueOrIndex;
			int size = 4 * 2 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 10:	// SRATIONAL
		if (item.Count >= 1)
		{
			int index = offset + item.ValueOrIndex;
			int size = 4 * 2 * item.Count;
			memcpy(&dst_scan[index], &src_scan[index], size);
		}
		break;
	case 6:		// unknown
	case 8:		// unknown
		break;
	}
}

// //////////////////////////////////////////////////////////////////////
// Exif
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Exif_Attach(HxModule handle, TxExif src)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Attach(src);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Exif_Resize(HxModule handle, int length)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Resize(length);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_Reset(HxModule handle)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Reset();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_EndianType	(HxModule handle, ExEndianType* value)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (value != NULL)
			*value = _src->EndianType();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetItems		(HxModule handle, HxModule hval)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto _dst = xie::Axi::SafeCast<CxArray>(hval);
		if (_dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArrayEx<TxExifItem> items = _src->GetItems();

		typedef int	TD;
		_dst->Resize(items.Length(), ModelOf<TD>() * 6);

		auto scan = _dst->Scanner<TD>();
		if (scan.IsValid())
		{
			scan.ForEach([&items](int x, TD* addr)
			{
				addr[0] = (TD)items[x].Offset;
				addr[1] = (TD)items[x].EndianType;
				addr[2] = (TD)items[x].ID;
				addr[3] = (TD)items[x].Type;
				addr[4] = (TD)items[x].Count;
				addr[5] = (TD)items[x].ValueOrIndex;
			});
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetPurgedExif	(HxModule handle, HxModule hval)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto _dst = xie::Axi::SafeCast<CxExif>(hval);
		if (_dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		*_dst = _src->GetPurgedExif();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_GetValue		(HxModule handle, TxExifItem item, HxModule hval)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->GetValue(item, hval);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Exif_SetValue		(HxModule handle, TxExifItem item, HxModule hval)
{
	try
	{
		auto _src = xie::Axi::SafeCast<CxExif>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->SetValue(item, hval);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
