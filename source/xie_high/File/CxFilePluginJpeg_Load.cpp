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
#include "Core/TxStringA.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginJpeg::Load(HxModule hdst, void* handle, bool unpack)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		FILE* stream = (FILE*)handle;
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// JPEG Information Object
		struct jpeg_decompress_struct	dinfo;

		CxFinalizer jpeg_finalizer([&dinfo]()
			{
				// Dispose Object
				jpeg_destroy_decompress(&dinfo);
			});

		// ----------------------------------------------------------------------
		// ‰Šú‰».
		// ----------------------------------------------------------------------
		TxJpegErrorManager jerr;
		{
			// Step 1: allocate and initialize JPEG decompression object

			// We set up the normal JPEG error routines, then override error_exit.
			dinfo.err = jpeg_std_error(&jerr.pub);
			jerr.pub.error_exit = fnPRV_Jpeg_ErrorHandler;
			jerr.pub.output_message = fnPRV_Jpeg_OutputMessage;

			// Establish the setjmp return context for fnPRV_Load_Jpeg_ErrorHandler to use.
			if (setjmp(jerr.setjmp_buffer))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// Now we can initialize the JPEG decompression object.
			jpeg_create_decompress(&dinfo);

			// Step 2: specify data source (eg, a file)
			jpeg_stdio_src(&dinfo, stream);

			// Step 2.5: Comment, Exif
			jpeg_save_markers(&dinfo, JPEG_COM, 0xFFFF);
			jpeg_save_markers(&dinfo, JPEG_APP0 + 1, 0xFFFF);

			// Step 3: read file parameters with jpeg_read_header()
			jpeg_read_header(&dinfo, TRUE);

			// Step 4: set parameters for decompression
		}

		// ----------------------------------------------------------------------
		// ƒf[ƒ^“Ç‚Ýž‚Ý.
		// ----------------------------------------------------------------------
		{
			// Step 5: Start decompressor
			jpeg_start_decompress(&dinfo);

			CxFinalizer jpeg_finish([&dinfo]()
				{
					// Step 7: Finish decompression
					jpeg_finish_decompress(&dinfo);
				});

			// JSAMPLEs per row in output buffer
			int row_stride = dinfo.output_width * dinfo.output_components;

			/* Make a one-row-high sample array that will go away when done with image */
			JSAMPARRAY buffer = (*dinfo.mem->alloc_sarray)((j_common_ptr) &dinfo, JPOOL_IMAGE, row_stride, 1);

			// Step 6: while (scan lines remain to be read)
			switch(dinfo.output_components)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case 1:
				{
					//
					// Gray
					//
					if (dinfo.data_precision <= 8)
					{
						typedef unsigned char	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, ModelOf<TE>(), 1);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							memcpy((*dst)(0, y, 0), buffer[0], row_stride);
						}
					}
					else if (dinfo.data_precision <= 16)
					{
						typedef unsigned short	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, ModelOf<TE>(), 1);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							memcpy((*dst)(0, y, 0), buffer[0], row_stride);
						}
					}
					else
					{
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					}
				}
				break;
			case 3:
				if (unpack == false)
				{
					//
					// Color (Packing)
					//
					if (dinfo.data_precision <= 8)
					{
						typedef unsigned char	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, TxRGBx3<TE>::Model(), 1);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							memcpy((*dst)(0, y, 0), buffer[0], row_stride);
						}
					}
					else if (dinfo.data_precision <= 16)
					{
						typedef unsigned short	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, TxRGBx3<TE>::Model(), 1);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							memcpy((*dst)(0, y, 0), buffer[0], row_stride);
						}
					}
					else
					{
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					}
				}
				else
				{
					//
					// Color (Unpacking)
					//
					if (dinfo.data_precision <= 8)
					{
						typedef unsigned char	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, ModelOf<TE>(), 3);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							Buffer_Copy_up<TE, TxRGBx3<TE>>(
								(TE*)(*dst)(0, y, 0),
								(TE*)(*dst)(1, y, 0),
								(TE*)(*dst)(2, y, 0),
								(const TxRGBx3<TE>*)buffer[0],
								dinfo.output_width);
						}
					}
					else if (dinfo.data_precision <= 16)
					{
						typedef unsigned short	TE;
						dst->Resize(dinfo.output_width, dinfo.output_height, ModelOf<TE>(), 3);
						dst->Depth(dinfo.data_precision);
						for (int y=0 ; dinfo.output_scanline < dinfo.output_height ; y++)
						{
							jpeg_read_scanlines(&dinfo, buffer, 1);
							Buffer_Copy_up<TE, TxRGBx3<TE>>(
								(TE*)(*dst)(0, y, 0),
								(TE*)(*dst)(1, y, 0),
								(TE*)(*dst)(2, y, 0),
								(const TxRGBx3<TE>*)buffer[0],
								dinfo.output_width);
						}
					}
					else
					{
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					}
				}
				break;
			}

			// Step 5.5: Comment, Exif
			for (jpeg_saved_marker_ptr marker = dinfo.marker_list; marker; marker = marker->next)
			{
				switch(marker->marker)
				{
				case JPEG_COM:			// 0xFE
					{
					}
					break;
				case JPEG_APP0+1:		// 0xE0+1
					if (marker->data != NULL && marker->data_length > 16 && memcmp(marker->data, "Exif\0\0", 6) == 0)
					{
						auto exif = TxExif(marker->data, (int)marker->data_length, TxModel::U8(1));
						dst->Exif(exif);
					}
					break;
				}
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
