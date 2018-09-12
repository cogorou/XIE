/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginJpeg.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/TxImageSize.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginJpeg::Check(TxImageSize* image_size, void* handle, bool unpack)
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

		// JPEG Information Object
		struct jpeg_decompress_struct	cinfo;

		CxFinalizer jpeg_finalizer([&cinfo]()
			{
				// Dispose Object
				jpeg_destroy_decompress(&cinfo);
			});

		// ----------------------------------------------------------------------
		// ‰Šú‰».
		// ----------------------------------------------------------------------
		{
			// Step 1: allocate and initialize JPEG decompression object

			// We set up the normal JPEG error routines, then override error_exit.
			TxJpegErrorManager jerr;
			cinfo.err = jpeg_std_error(&jerr.pub);
			jerr.pub.error_exit = fnPRV_Jpeg_ErrorHandler;
			jerr.pub.output_message = fnPRV_Jpeg_OutputMessage;

			// Establish the setjmp return context for fnPRV_Load_Jpeg_ErrorHandler to use.
			if (setjmp(jerr.setjmp_buffer))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// Now we can initialize the JPEG decompression object.
			jpeg_create_decompress(&cinfo);

			// Step 2: specify data source (eg, a file)
			jpeg_stdio_src(&cinfo, stream);

			// Step 3: read file parameters with jpeg_read_header()
			jpeg_read_header(&cinfo, TRUE);

			// Step 4: set parameters for decompression
		}

		// ----------------------------------------------------------------------
		// ƒf[ƒ^“Ç‚Ýž‚Ý.
		// ----------------------------------------------------------------------
		try
		{
			// Step 5: Start decompressor
			jpeg_start_decompress(&cinfo);

			// JSAMPLEs per row in output buffer
			int row_stride = cinfo.output_width * cinfo.output_components;

			/* Make a one-row-high sample array that will go away when done with image */
			JSAMPARRAY buffer = (*cinfo.mem->alloc_sarray)((j_common_ptr) &cinfo, JPOOL_IMAGE, row_stride, 1);

			// Step 6: while (scan lines remain to be read)
			switch(cinfo.output_components)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case 1:
				{
					if (cinfo.data_precision <= 8)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U8(1), 1);
						result.Depth = cinfo.data_precision;
					}
					else if (cinfo.data_precision <= 16)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U16(1), 1);
						result.Depth = cinfo.data_precision;
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
					if (cinfo.data_precision <= 8)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U8(3), 1);
						result.Depth = cinfo.data_precision;
					}
					else if (cinfo.data_precision <= 16)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U16(3), 1);
						result.Depth = cinfo.data_precision;
					}
					else
					{
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					}
				}
				else
				{
					if (cinfo.data_precision <= 8)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U8(1), 3);
						result.Depth = cinfo.data_precision;
					}
					else if (cinfo.data_precision <= 16)
					{
						result = TxImageSize(cinfo.output_width, cinfo.output_height, TxModel::U16(1), 3);
						result.Depth = cinfo.data_precision;
					}
					else
					{
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					}
				}
				break;
			}
		}
		catch(const CxException& ex)
		{
			status = ex.Code();
		}

		// Step 7: Finish decompression
		jpeg_finish_decompress(&cinfo);

		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
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
