/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginBmp.h"
#include "api_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "File/api_file.h"
#include "File/api_file_bmp.h"
#include "File/fnPRV_Buffer_Copy.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginBmp::Check(TxImageSize* image_size, void* handle, bool unpack)
{
	ExStatus	status = ExStatus::Success;
	TxImageSize	result;

	try
	{
		FILE* stream = (FILE*)handle;
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		if (image_size == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		size_t				uiReadSize;
		//
		// Reading a BITMAP header.
		BITMAPFILEHEADER	bmf;
		uiReadSize = fread( &bmf, _BITMAPFILEHEADER_SIZE, 1, stream );
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		if (bmf.bfType != BM_HEADER_MARKER)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// 
		// Reading a BITMAPINFOHEADER.

		// --- Allocating information header.
		BITMAPINFOHEADER		bmi;

		// --- Reading an information header.
		uiReadSize = fread( (void*)&bmi, sizeof(BITMAPINFOHEADER), 1, stream );
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// unsupported spec.
		if( bmi.biSize > sizeof(BITMAPINFOHEADER) )
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		// BI_RGB/BI_BITFIELDS
		//if( bmi.biCompression != BI_RGB )
		//	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		// --- Reading of a palette.
		CxArray		palette;
		int			iPaletteSize = 0;

		// BI_RGB/BI_BITFIELDS
		if (bmi.biCompression == BI_RGB)
		{
			// (1) It is specified by ClrUsed.
			if (iPaletteSize == 0 && bmi.biClrUsed > 0)
				iPaletteSize = sizeof(TxBGR8x4) * bmi.biClrUsed;

			// (2) It calculates from the depth.
			if (iPaletteSize == 0 && bmi.biBitCount <= 8)
				iPaletteSize = sizeof(TxBGR8x4) * (unsigned int)(1<<bmi.biBitCount);

			if (iPaletteSize % sizeof(TxBGR8x4) != 0)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			// (3) The palette is specified.
			if (iPaletteSize != 0)
			{
				// --- The domain of a palette is secured.(Size with a margin)
				int length = iPaletteSize / sizeof(TxBGR8x4);
				palette.Resize(length, TxModel::U8(4));

				uiReadSize = fread( palette[0], iPaletteSize, 1, stream );
				if( uiReadSize != 1 )
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		else if(bmi.biCompression == BI_BITFIELDS)
		{
			palette.Resize(3, TxModel::U32(1));
			iPaletteSize = palette.Model().Size() * palette.Length();
			uiReadSize = fread( palette[0], iPaletteSize, 1, stream );
			if( uiReadSize != 1 )
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		// (4) dirty file
		{
			unsigned int	offset = _BITMAPFILEHEADER_SIZE + sizeof(BITMAPINFOHEADER) + iPaletteSize;
			if (offset < bmf.bfOffBits)
			{
				unsigned int	dustsize = bmf.bfOffBits - offset;
				CxArray			dustbuff(dustsize, TxModel::U8(1));
				uiReadSize = fread( dustbuff[0], dustsize, 1, stream );
				if (uiReadSize != 1)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}

		// (5) read data
		int			width		= abs(bmi.biWidth);
		int			height		= abs(bmi.biHeight);
		int			bpp			= bmi.biBitCount;
		int			direction	= (bmi.biHeight < 0) ? +1 : -1;

		switch(bpp)
		{
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		case 1:
			{
				result = TxImageSize(width, height, TxModel::U8(1), 1);
				result.Depth = 1;
			}
			break;
		case 4:
			{
				bool is_color = false;
				TxBGR8x4* addr = (TxBGR8x4*)palette[0];
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
					result = TxImageSize(width, height, TxModel::U8(1), 1);
				}
				else if (unpack == false)
				{
					result = TxImageSize(width, height, TxModel::U8(3), 1);
				}
				else
				{
					result = TxImageSize(width, height, TxModel::U8(1), 3);
				}
			}
			break;
		case 8:
			{
				bool is_color = false;
				TxBGR8x4* addr = (TxBGR8x4*)palette[0];
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
					result = TxImageSize(width, height, TxModel::U8(1), 1);
				}
				else if (unpack == false)
				{
					result = TxImageSize(width, height, TxModel::U8(3), 1);
				}
				else
				{
					result = TxImageSize(width, height, TxModel::U8(1), 3);
				}
			}
			break;
		case 16:
			if(bmi.biCompression == BI_RGB)
			{
				unsigned int mask[3] = {0x7C00, 0x03E0, 0x001F};
				int shift[3] = {7, 2, 3};
				if (unpack == false)
				{
					result = TxImageSize(width, height, TxModel::U8(3), 1);
				}
				else
				{
					result = TxImageSize(width, height, TxModel::U8(1), 3);
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
					if (unpack == false)
					{
						result = TxImageSize(width, height, TxModel::U8(3), 1);
					}
					else
					{
						result = TxImageSize(width, height, TxModel::U8(1), 3);
					}
				}
				else if (mask[0] == 0xF800 &&
						 mask[1] == 0x07E0 &&
						 mask[2] == 0x001F)
				{
					int shift[3] = {8, 3, 3};
					if (unpack == false)
					{
						result = TxImageSize(width, height, TxModel::U8(3), 1);
					}
					else
					{
						result = TxImageSize(width, height, TxModel::U8(1), 3);
					}
				}
				else if (mask[0] == 0x3FF ||
						 mask[1] == 0x3FF ||
						 mask[2] == 0x3FF)
				{
					result = TxImageSize(width, height, TxModel::U8(1), 1);
				}
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		case 24:
			if (unpack == false)
			{
				result = TxImageSize(width, height, TxModel::U8(3), 1);
			}
			else
			{
				result = TxImageSize(width, height, TxModel::U8(1), 3);
			}
			break;
		case 32:
			if (unpack == false)
			{
				result = TxImageSize(width, height, TxModel::U8(4), 1);
			}
			else
			{
				result = TxImageSize(width, height, TxModel::U8(1), 3);
			}
			break;
		}
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	*image_size = result;

	return status;
}

}
}
