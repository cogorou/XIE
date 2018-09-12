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
#include "File/fnPRV_Buffer_Copy.h"

#pragma warning (disable:4101)	// ローカル変数は 1 度も使われていません。

namespace xie
{
namespace File
{

static bool fnPRV_DIB_Create( int width, int height, int bpp, void** ppvdib, unsigned int* puiBmpInfoSize, unsigned int* puiImageSize );

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DIB_Save(HxModule hsrc, void** ppvdib, unsigned int* puiBmpInfoSize, unsigned int* puiImageSize)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (ppvdib == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		void*		pvimg		= NULL;
		int			width		= src->Width();
		int			height		= src->Height();
		int			channels	= src->Channels();
		TxModel		model		= src->Model();
		int			bpp			= model.Size() * 8 * channels;
		int			stride		= fnPRV_DIB_CalcStride(width, bpp);

		if (channels == 1 && model == TxModel::U8(1) && src->Depth() == 1)
		{
			bpp = 1;
			stride = fnPRV_DIB_CalcStride(width, bpp);
		}

		// ----------------------------------------------------------------------
		// ヘッダー書き込み.
		// ----------------------------------------------------------------------
		{
			void*			pvdib = NULL;
			unsigned int	uiBitmapInfoSize = 0;
			if (!fnPRV_DIB_Create( width, height, bpp, &pvdib, &uiBitmapInfoSize, puiImageSize ) )
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

			*ppvdib = pvdib;
			if (puiBmpInfoSize != NULL)
				*puiBmpInfoSize = uiBitmapInfoSize;

			pvimg = static_cast<char*>(pvdib) + uiBitmapInfoSize;
		}

		// ----------------------------------------------------------------------
		// データ書き込み.
		// ----------------------------------------------------------------------

		if (channels == 1)
		{
			switch(model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				switch(model.Pack)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case 1:
					if (bpp == 1)
					{
						unsigned char* pucdst = (unsigned char*)pvimg;
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_1u<unsigned char, unsigned char>(
								(unsigned char*)pucdst,
								(unsigned char*)(*src)(0, y, 0),
								width, 0x1);
							pucdst += stride;
						}
					}
					else
					{
						unsigned char* pucdst = (unsigned char*)pvimg;
						for (int y=height-1 ; y>=0 ; y--)
						{
							unsigned char* pucsrc = (unsigned char*)(*src)(0, y, 0);
							for (int x=0 ; x<width ; x++)
								pucdst[x] = pucsrc[x];
							pucdst += stride;
						}
					}
					break;
				case 3:
					{
						unsigned char* pucdst = (unsigned char*)pvimg;
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_pp<TxBGR8x3, TxRGB8x3>(
								(TxBGR8x3*)pucdst,
								(TxRGB8x3*)(*src)(0, y, 0),
								width);
							pucdst += stride;
						}
					}
					break;
				case 4:
					{
						unsigned char* pucdst = (unsigned char*)pvimg;
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_pp<TxBGR8x4, TxRGB8x4>(
								(TxBGR8x4*)pucdst,
								(TxRGB8x4*)(*src)(0, y, 0),
								width);
							pucdst += stride;
						}
					}
					break;
				}
				break;
			}
		}
		else if (channels == 3)
		{
			switch(model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				{
					unsigned char* pucdst = (unsigned char*)pvimg;
					for (int y=height-1 ; y>=0 ; y--)
					{
						fnPRV_Buffer_Copy_pu<TxBGR8x3, unsigned char>(
							(TxBGR8x3*)pucdst,
							(unsigned char*)(*src)(0, y, 0),
							(unsigned char*)(*src)(1, y, 0),
							(unsigned char*)(*src)(2, y, 0),
							width);
						pucdst += stride;
					}
				}
				break;
			}
		}
		else if (channels == 4)
		{
			switch(model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				{
					unsigned char* pucdst = (unsigned char*)pvimg;
					for (int y=height-1 ; y>=0 ; y--)
					{
						fnPRV_Buffer_Copy_qu<TxBGR8x4, unsigned char>(
							(TxBGR8x4*)pucdst,
							(unsigned char*)(*src)(0, y, 0),
							(unsigned char*)(*src)(1, y, 0),
							(unsigned char*)(*src)(2, y, 0),
							(unsigned char*)(*src)(3, y, 0),
							width);
						pucdst += stride;
					}
				}
				break;
			}
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// ============================================================
static bool fnPRV_DIB_Create( int width, int height, int bpp, void** ppvdib, unsigned int* puiBmpInfoSize, unsigned int* puiImageSize )
{
	if ( !(1 <= bpp && bpp <= 32) )
		return false;
	if ( ppvdib == NULL )
		return false;

	int bpp_packed = 0;
	if(      bpp ==  1 )	bpp_packed =  1;
	else if( bpp <=  8 )	bpp_packed =  8;
	else if( bpp <= 16 )	bpp_packed = 16;
	else if( bpp <= 24 )	bpp_packed = 24;
	else if( bpp <= 32 )	bpp_packed = 32;

	int				stride = fnPRV_DIB_CalcStride(width, bpp);
	int				size_of_image = stride * abs(height);
	if ( puiImageSize != NULL)
		*puiImageSize = size_of_image;

	int				iClrUsed = (bpp_packed<=8) ? (1<<bpp_packed) : 0;
	int				iClrMask = (bpp_packed==16 || bpp_packed==32) ? 3 : 0;
	unsigned int	biCompression = (bpp_packed==16 || bpp_packed==32) ? BI_BITFIELDS : BI_RGB;

	unsigned int	palette_size = 0;
	if (iClrUsed > 0)
		palette_size = sizeof(TxBGR8x4)*iClrUsed;
	if (iClrMask > 0)
		palette_size = sizeof(TxBGR8x4)*iClrMask;

	unsigned int	uiBmpInfoSize = sizeof(BITMAPINFOHEADER) + palette_size;
	if ( puiBmpInfoSize != NULL )
		*puiBmpInfoSize = uiBmpInfoSize;

	*ppvdib = Axi::MemoryAlloc( uiBmpInfoSize + size_of_image );
	if (*ppvdib == NULL)
		return false;

	// BITMAPINFO
	BITMAPINFO* bmpInfo = (BITMAPINFO*)*ppvdib;
	
	bmpInfo->bmiHeader.biSize			= sizeof( BITMAPINFOHEADER );
	bmpInfo->bmiHeader.biWidth			= width;
	bmpInfo->bmiHeader.biHeight			= height;
	bmpInfo->bmiHeader.biPlanes			= 1;
	bmpInfo->bmiHeader.biBitCount		= bpp_packed;
	bmpInfo->bmiHeader.biCompression	= biCompression;
	bmpInfo->bmiHeader.biSizeImage		= size_of_image;
	bmpInfo->bmiHeader.biXPelsPerMeter	= 0;	// (LONG)(72*100/2.54 + 0.9);	// 72pixel/inch
	bmpInfo->bmiHeader.biYPelsPerMeter	= 0;	// (LONG)(72*100/2.54 + 0.9);	// 72pixel/inch
	bmpInfo->bmiHeader.biClrUsed		= iClrUsed;
	bmpInfo->bmiHeader.biClrImportant	= 0;
	
	// making TxBGR8x4 Table
	if (iClrUsed != 0)
	{
		for( int i=0; i<iClrUsed; i++ )
		{
			unsigned char dens = (unsigned char)(255 * i / (iClrUsed - 1));
			bmpInfo->bmiColors[i].rgbBlue	= dens;
			bmpInfo->bmiColors[i].rgbGreen	= dens;
			bmpInfo->bmiColors[i].rgbRed	= dens;
			bmpInfo->bmiColors[i].rgbReserved = 0x00;
		}
	}
	if (iClrMask == 3)
	{
		bmpInfo->bmiColors[0] = fnPRV_DIB_FromRGBA(0xFF, 0x00, 0x00, 0x00);
		bmpInfo->bmiColors[1] = fnPRV_DIB_FromRGBA(0x00, 0xFF, 0x00, 0x00);
		bmpInfo->bmiColors[2] = fnPRV_DIB_FromRGBA(0x00, 0x00, 0xFF, 0x00);
	}

	return true;
}

}	// File
}	// xie
