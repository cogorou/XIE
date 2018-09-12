/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginBmp.h"
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
ExStatus CxFilePluginBmp::Save(HxModule hsrc, void* handle)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		FILE* stream = (FILE*)handle;
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		size_t	uiWriteSize;

		int			width		= src->Width();
		int			height		= src->Height();
		int			channels	= src->Channels();
		TxModel		model		= src->Model();
		int			bpp			= model.Size() * 8 * channels;
		int			stride		= fnPRV_Bmp_CalcStride(width, bpp);

		if (channels == 1 && model == TxModel::U8(1) && src->Depth() == 1)
		{
			bpp = 1;
			stride = fnPRV_Bmp_CalcStride(width, bpp);
		}

		// ----------------------------------------------------------------------
		// ヘッダー書き込み.
		// ----------------------------------------------------------------------
		{
			BITMAPINFO*		pBitmapInfo = NULL;
			unsigned int	uiBitmapInfoSize = 0;

			if (!CreateBitmapInfo( width, height, bpp, &pBitmapInfo, &uiBitmapInfoSize ) )
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

			CxFinalizer bmi_finalizer([&pBitmapInfo]()
				{
					FreeBitmapInfo( pBitmapInfo );
				});

			// BITMAP file header.
			// (!) BITMAPFILEHEADER 構造体を使用しないこと. ヘッダーは 14bytes なので.
			unsigned short		usBmfHeader[7];

			unsigned int		uiImageSize = stride * height;
			unsigned int		uiOffset   = sizeof(usBmfHeader) + uiBitmapInfoSize;
			unsigned int		uiFileSize = sizeof(usBmfHeader) + uiBitmapInfoSize + uiImageSize;

			//
			// Write a file header.
			//
			usBmfHeader[0] = BM_HEADER_MARKER;	// bfType
			memcpy( &usBmfHeader[1], &uiFileSize, sizeof(unsigned int) );
			usBmfHeader[3] = 0;					// bfReserved1
			usBmfHeader[4] = 0;					// bfReserved2
			memcpy( &usBmfHeader[5], &uiOffset, sizeof(unsigned int) );

			uiWriteSize = fwrite( usBmfHeader, _BITMAPFILEHEADER_SIZE, 1, stream );
			if (uiWriteSize != 1)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

			//
			// Writing a BITMAPINFOHEADER and pallete.
			//
			uiWriteSize = fwrite( pBitmapInfo, uiBitmapInfoSize, 1, stream );
			if (uiWriteSize != 1)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		}

		// ----------------------------------------------------------------------
		// データ書き込み.
		// ----------------------------------------------------------------------

		if (channels == 1)
		{
			CxArray buf(stride, TxModel::U8(1));
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
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_1u<unsigned char, unsigned char>(
								(unsigned char*)buf[0],
								(unsigned char*)(*src)(0, y, 0),
								width, 0x1);
							uiWriteSize = fwrite( buf[0], stride, 1, stream );
							if (uiWriteSize != 1)
								throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
						}
					}
					else
					{
						for (int y=height-1 ; y>=0 ; y--)
						{
							uiWriteSize = fwrite( (*src)(0, y, 0), stride, 1, stream );
							if (uiWriteSize != 1)
								throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
						}
					}
					break;
				case 3:
					{
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_pp<TxBGR8x3, TxRGB8x3>(
								(TxBGR8x3*)buf[0],
								(TxRGB8x3*)(*src)(0, y, 0),
								width);
							uiWriteSize = fwrite( buf[0], stride, 1, stream );
							if (uiWriteSize != 1)
								throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
						}
					}
					break;
				case 4:
					{
						for (int y=height-1 ; y>=0 ; y--)
						{
							fnPRV_Buffer_Copy_pp<TxBGR8x4, TxRGB8x4>(
								(TxBGR8x4*)buf[0],
								(TxRGB8x4*)(*src)(0, y, 0),
								width);
							uiWriteSize = fwrite( buf[0], stride, 1, stream );
							if (uiWriteSize != 1)
								throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
						}
					}
					break;
				}
				break;
			}
		}
		else if (channels == 3)
		{
			CxArray buf(stride, TxModel::U8(1));
			switch(model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				for (int y=height-1 ; y>=0 ; y--)
				{
					fnPRV_Buffer_Copy_pu<TxBGR8x3, unsigned char>(
						(TxBGR8x3*)buf[0],
						(unsigned char*)(*src)(0, y, 0),
						(unsigned char*)(*src)(1, y, 0),
						(unsigned char*)(*src)(2, y, 0),
						width);
					uiWriteSize = fwrite( buf[0], stride, 1, stream );
					if (uiWriteSize != 1)
						throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				}
				break;
			}
		}
		else if (channels == 4)
		{
			CxArray buf(stride, TxModel::U8(1));
			switch(model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				for (int y=height-1 ; y>=0 ; y--)
				{
					fnPRV_Buffer_Copy_qu<TxBGR8x4, unsigned char>(
						(TxBGR8x4*)buf[0],
						(unsigned char*)(*src)(0, y, 0),
						(unsigned char*)(*src)(1, y, 0),
						(unsigned char*)(*src)(2, y, 0),
						(unsigned char*)(*src)(3, y, 0),
						width);
					uiWriteSize = fwrite( buf[0], stride, 1, stream );
					if (uiWriteSize != 1)
						throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
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

}
}
