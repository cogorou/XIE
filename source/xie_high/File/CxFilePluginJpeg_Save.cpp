/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginJpeg.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginJpeg::Save(HxModule hsrc, void* handle, int quality)
{
	ExStatus	status = ExStatus::Success;

	struct jpeg_error_mgr		jerr;

	try
	{
		FILE* stream = (FILE*)handle;
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			return ExStatus::InvalidObject;
		if (src->IsValid() == false)
			return ExStatus::InvalidObject;

		struct jpeg_compress_struct	cinfo;

		// Step 1: allocate and initialize JPEG compression object
		cinfo.err = jpeg_std_error(&jerr);
		jpeg_create_compress(&cinfo);

		CxFinalizer jpeg_finalizer([&cinfo]()
			{
				// Step 7: release JPEG compression object
				jpeg_destroy_compress(&cinfo);
			});

		TxJpegErrorManager jerr;
		cinfo.err = jpeg_std_error( &jerr.pub );
		jerr.pub.error_exit = fnPRV_Jpeg_ErrorHandler;
		jerr.pub.output_message = fnPRV_Jpeg_OutputMessage;
		if (setjmp(jerr.setjmp_buffer))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// ----------------------------------------------------------------------
		// ‰Šú‰».
		// ----------------------------------------------------------------------
		{
			jpeg_stdio_dest(&cinfo, stream);

			// Step 3: set parameters for compression
			cinfo.image_width		= src->Width();
			cinfo.image_height		= src->Height();

			switch(src->Model().Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:
				switch(src->Model().Pack)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case 1:
					switch(src->Channels())
					{
					default:
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					case 1:
						cinfo.input_components	= 1;
						cinfo.in_color_space	= JCS_GRAYSCALE;
						break;
					case 3:
					case 4:
						cinfo.input_components	= 3;
						cinfo.in_color_space	= JCS_RGB;
						break;
					}
					break;
				case 3:
				case 4:
					switch(src->Channels())
					{
					default:
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					case 1:
						cinfo.input_components	= 3;
						cinfo.in_color_space	= JCS_RGB;
						break;
					}
					break;
				}
				break;
			}

			jpeg_set_defaults(&cinfo);
			jpeg_set_quality(&cinfo, quality, TRUE);
		}

		// ----------------------------------------------------------------------
		// ƒf[ƒ^‘‚«ž‚Ý.
		// ----------------------------------------------------------------------
		{
			// Step 4: Start compressor
			jpeg_start_compress(&cinfo, TRUE);

			CxFinalizer jpeg_finish([&cinfo]()
				{
					// Step 6: Finish compression
					jpeg_finish_compress(&cinfo);
				});

			// Step 4.5: Exif ‘‚«ž‚Ý.
			auto exif = src->Exif();
			if (exif.Address != NULL && exif.Length > 16 && memcmp(exif.Address, "Exif\0\0", 6) == 0)
			{
				jpeg_write_marker(&cinfo, JPEG_APP0+1, (const JOCTET*)exif.Address, exif.Length);
			}

			// Step 5: while (scan lines remain to be written)
			JSAMPROW	row_pointer[1];
			int			row_stride = cinfo.image_width * cinfo.input_components;

			CxArray buf( row_stride, TxModel::U8(1) );
			switch(src->Model().Type)
			{
			default:
				break;
			case ExType::U8:
				switch(src->Model().Pack)
				{
				case 1:
					// U8xNch
					switch(src->Channels())
					{
					case 1:
						for (int y=0 ; cinfo.next_scanline < cinfo.image_height ; y++)
						{
							memcpy(buf[0], (*src)(0, y, 0), row_stride);
							row_pointer[0] = (JSAMPROW)buf[0];
							jpeg_write_scanlines(&cinfo, row_pointer, 1);
						}
						break;
					case 3:
					case 4:
						for (int y=0 ; cinfo.next_scanline < cinfo.image_height ; y++)
						{
							Buffer_Copy_pu<TxRGB8x3,unsigned char>(
								(TxRGB8x3*)buf[0],
								(unsigned char*)(*src)(0, y, 0),
								(unsigned char*)(*src)(1, y, 0),
								(unsigned char*)(*src)(2, y, 0),
								cinfo.image_width);

							row_pointer[0] = (JSAMPROW)buf[0];
							jpeg_write_scanlines(&cinfo, row_pointer, 1);
						}
						break;
					}
					break;
				case 3:
					// RGB8x3
					for (int y=0 ; cinfo.next_scanline < cinfo.image_height ; y++)
					{
						memcpy(buf[0], (*src)(0, y, 0), row_stride);
						row_pointer[0] = (JSAMPROW)buf[0];
						jpeg_write_scanlines(&cinfo, row_pointer, 1);
					}
					break;
				case 4:
					// RGB8x4
					for (int y=0 ; cinfo.next_scanline < cinfo.image_height ; y++)
					{
						Buffer_Copy_pp<TxRGB8x3,TxRGB8x4>(
							(TxRGB8x3*)buf[0],
							(TxRGB8x4*)(*src)(0, y, 0),
							cinfo.image_width);

						row_pointer[0] = (JSAMPROW)buf[0];
						jpeg_write_scanlines(&cinfo, row_pointer, 1);
					}
					break;
				}
				break;
			}
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
