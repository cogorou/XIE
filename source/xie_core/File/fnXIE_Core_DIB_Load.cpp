/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "File/api_file.h"
#include "File/api_file_dib.h"
#include "Data/api_data.h"

#pragma warning (disable:4101)	// ローカル変数は 1 度も使われていません。

namespace xie
{
namespace File
{

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DIB_Load(HxModule hdst, void* pvdib, ExBoolean unpack)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (pvdib == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// 
		// Reading a BITMAPINFOHEADER.

		// --- Allocating information header.
		BITMAPINFOHEADER&		bmi = *static_cast<BITMAPINFOHEADER*>(pvdib);

		// unsupported spec.
		if( bmi.biSize > sizeof(BITMAPINFOHEADER) )
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		// BI_RGB/BI_BITFIELDS
		//if( bmi.biCompression != BI_RGB )
		//	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		// --- Reading of a palette.
		typedef TxBGR8x4	TPALETTE;
		CxArray		palette;
		int			iPaletteSize = 0;

		// BI_RGB/BI_BITFIELDS
		if (bmi.biCompression == BI_RGB)
		{
			// (1) It is specified by ClrUsed.
			if (iPaletteSize == 0 && bmi.biClrUsed > 0)
				iPaletteSize = sizeof(TPALETTE) * bmi.biClrUsed;

			// (2) It calculates from the depth.
			if (iPaletteSize == 0 && bmi.biBitCount <= 8)
				iPaletteSize = sizeof(TPALETTE) * (unsigned int)(1<<bmi.biBitCount);

			if (iPaletteSize % sizeof(TPALETTE) != 0)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			// (3) The palette is specified.
			if (iPaletteSize != 0)
			{
				// --- The domain of a palette is secured.(Size with a margin)
				int			length = iPaletteSize / sizeof(TPALETTE);
				TPALETTE*	src_palette = (TPALETTE*)(static_cast<char*>(pvdib) + sizeof(BITMAPINFOHEADER));
				TxArray		dst_tag(src_palette, length, ModelOf<TPALETTE>());
				palette.Attach(dst_tag);
			}
		}
		else if(bmi.biCompression == BI_BITFIELDS)
		{
			int			length = 3;
			TPALETTE*	src_palette = (TPALETTE*)(static_cast<char*>(pvdib) + sizeof(BITMAPINFOHEADER));
			TxArray		dst_tag(src_palette, length, ModelOf<TPALETTE>());
			palette.Attach(dst_tag);

			iPaletteSize = length * sizeof(TPALETTE);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		// (5) read data
		int			width		= abs(bmi.biWidth);
		int			height		= abs(bmi.biHeight);
		int			bpp			= bmi.biBitCount;
		int			direction	= (bmi.biHeight < 0) ? +1 : -1;

		if (bpp == 0)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		void* pvimg = static_cast<char*>(pvdib) + sizeof(BITMAPINFOHEADER) + iPaletteSize;

		switch(bpp)
		{
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		case 1:
			{
				dst->Resize(width, height, TxModel::U8(1), 1);
				dst->Depth(1);
				fnPRV_DIB_Load_1g<unsigned char>( *dst, pvimg, palette, direction );
			}
			break;
		case 4:
			{
				bool is_color = false;
				TPALETTE* addr = (TPALETTE*)palette[0];
				int		length = palette.Length();
				for( int i=0 ; i<length ; i++ )
				{
					if (addr[i].R != addr[i].G || addr[i].R != addr[i].B)
					{
						is_color = true;
						break;
					}
				}

				if (is_color == false)
				{
					dst->Resize(width, height, TxModel::U8(1), 1);
					fnPRV_DIB_Load_4g<unsigned char>( *dst, pvimg, palette, direction );
				}
				else if (unpack == ExBoolean::False)
				{
					dst->Resize(width, height, TxModel::U8(3), 1);
					fnPRV_DIB_Load_4p<TxRGB8x3>( *dst, pvimg, palette, direction );
				}
				else
				{
					dst->Resize(width, height, TxModel::U8(1), 3);
					fnPRV_DIB_Load_4u<unsigned char>( *dst, pvimg, palette, direction );
				}
			}
			break;
		case 8:
			{
				bool is_color = false;
				TPALETTE* addr = (TPALETTE*)palette[0];
				int		length = palette.Length();
				for( int i=0 ; i<length ; i++ )
				{
					if (addr[i].R != addr[i].G || addr[i].R != addr[i].B)
					{
						is_color = true;
						break;
					}
				}

				if (is_color == false)
				{
					dst->Resize(width, height, TxModel::U8(1), 1);
					fnPRV_DIB_Load_8g<unsigned char>( *dst, pvimg, palette, direction );
				}
				else if (unpack == ExBoolean::False)
				{
					dst->Resize(width, height, TxModel::U8(3), 1);
					fnPRV_DIB_Load_8p<TxRGB8x3>( *dst, pvimg, palette, direction );
				}
				else
				{
					dst->Resize(width, height, TxModel::U8(1), 3);
					fnPRV_DIB_Load_8u<unsigned char>( *dst, pvimg, palette, direction );
				}
			}
			break;
		case 16:
			if(bmi.biCompression == BI_RGB)
			{
				unsigned int mask[3] = {0x7C00, 0x03E0, 0x001F};
				int shift[3] = {7, 2, 3};
				if (unpack == ExBoolean::False)
				{
					dst->Resize(width, height, TxModel::U8(3), 1);
					fnPRV_DIB_Load_16p<TxRGB8x3>( *dst, pvimg, palette, direction, mask, shift );
				}
				else
				{
					dst->Resize(width, height, TxModel::U8(1), 3);
					fnPRV_DIB_Load_16u<unsigned char>( *dst, pvimg, palette, direction, mask, shift );
				}
			}
			else if(bmi.biCompression == BI_BITFIELDS)
			{
				unsigned int* mask = (unsigned int*)palette.Address();
				if (mask[0] == 0x7C00 &&
					mask[1] == 0x03E0 &&
					mask[2] == 0x001F)
				{
					int shift[3] = {7, 2, 3};
					if (unpack == ExBoolean::False)
					{
						dst->Resize(width, height, TxModel::U8(3), 1);
						fnPRV_DIB_Load_16p<TxRGB8x3>( *dst, pvimg, palette, direction, mask, shift );
					}
					else
					{
						dst->Resize(width, height, TxModel::U8(1), 3);
						fnPRV_DIB_Load_16u<unsigned char>( *dst, pvimg, palette, direction, mask, shift );
					}
				}
				else if (mask[0] == 0xF800 &&
						 mask[1] == 0x07E0 &&
						 mask[2] == 0x001F)
				{
					int shift[3] = {8, 3, 3};
					if (unpack == ExBoolean::False)
					{
						dst->Resize(width, height, TxModel::U8(3), 1);
						fnPRV_DIB_Load_16p<TxRGB8x3>( *dst, pvimg, palette, direction, mask, shift );
					}
					else
					{
						dst->Resize(width, height, TxModel::U8(1), 3);
						fnPRV_DIB_Load_16u<unsigned char>( *dst, pvimg, palette, direction, mask, shift );
					}
				}
				else if (mask[0] == 0x3FF ||
						 mask[1] == 0x3FF ||
						 mask[2] == 0x3FF)
				{
					float scale = 0.25f;
					dst->Resize(width, height, TxModel::U8(1), 1);
					fnPRV_DIB_Load_16g<unsigned char>( *dst, pvimg, palette, direction, scale );
				}
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		case 24:
			if (unpack == ExBoolean::False)
			{
				dst->Resize(width, height, TxModel::U8(3), 1);
				fnPRV_DIB_Load_24p<TxRGB8x3>( *dst, pvimg, palette, direction );
			}
			else
			{
				dst->Resize(width, height, TxModel::U8(1), 3);
				fnPRV_DIB_Load_pu<TxBGR8x3, unsigned char>( *dst, pvimg, palette, direction );
			}
			break;
		case 32:
			if (unpack == ExBoolean::False)
			{
				dst->Resize(width, height, TxModel::U8(4), 1);
				fnPRV_DIB_Load_32p<TxRGB8x4>( *dst, pvimg, palette, direction );
			}
			else
			{
				dst->Resize(width, height, TxModel::U8(1), 3);
				fnPRV_DIB_Load_pu<TxBGR8x4, unsigned char>( *dst, pvimg, palette, direction );
			}
			break;
		}
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

}	// File
}	// xie
