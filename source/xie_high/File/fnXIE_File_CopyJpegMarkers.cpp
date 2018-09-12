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

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_File_CopyJpegMarkers(TxCharCPtrA src_file, TxCharCPtrA dst_file, HxModule option)
{
	ExStatus	status = ExStatus::Success;

	if (src_file == NULL || dst_file == NULL)
		return ExStatus::InvalidParam;
	if (option != NULL)
		return ExStatus::Unsupported;

	struct jpeg_error_mgr		src_jerr;
	struct jpeg_error_mgr		dst_jerr;

	try
	{
		jvirt_barray_ptr*		src_coef_arrays = NULL;
		jvirt_barray_ptr*		dst_coef_arrays = NULL;

		struct jpeg_decompress_struct	src_info;
		struct jpeg_compress_struct		dst_info;

		// Step 1: allocate and initialize JPEG decompression/compression object
		src_info.err = jpeg_std_error(&src_jerr);
		dst_info.err = jpeg_std_error(&dst_jerr);
		jpeg_create_decompress(&src_info);
		jpeg_create_compress(&dst_info);

		CxFinalizer jpeg_finalizer([&src_info, &dst_info]()
			{
				// Step 4: release JPEG decompression/compression object
				jpeg_destroy_compress(&dst_info);
				jpeg_destroy_decompress(&src_info);
			});

		TxJpegErrorManager src_jerr;
		TxJpegErrorManager dst_jerr;
		src_info.err = jpeg_std_error( &src_jerr.pub );
		dst_info.err = jpeg_std_error( &dst_jerr.pub );
		src_jerr.pub.error_exit = fnPRV_Jpeg_ErrorHandler;
		src_jerr.pub.output_message = fnPRV_Jpeg_OutputMessage;
		dst_jerr.pub.error_exit = fnPRV_Jpeg_ErrorHandler;
		dst_jerr.pub.output_message = fnPRV_Jpeg_OutputMessage;
		if (setjmp(src_jerr.setjmp_buffer))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		if (setjmp(dst_jerr.setjmp_buffer))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// Step 2: specify data source/destination
		FILE* src_stream = fopen( src_file, "rb" );
		FILE* dst_stream = fopen( dst_file, "wb" );

		CxFinalizer stream_finalizer([&src_stream, &dst_stream]()
			{
				// CLOSE
				if (src_stream != NULL)
					fclose( src_stream );
				src_stream = NULL;
				if (dst_stream != NULL)
					fclose( dst_stream );
				dst_stream = NULL;
			});

		if (src_stream == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		if (dst_stream == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// ----------------------------------------------------------------------
		// Step 3: decompress/compress
		// ----------------------------------------------------------------------
		{
			CxFinalizer jpeg_finish([&src_info, &dst_info]()
				{
					jpeg_finish_compress(&dst_info);
					jpeg_finish_decompress(&src_info);
				});

			// Specify data source for decompression
			jpeg_stdio_src(&src_info, src_stream);

			// Specify data destination for compression
			jpeg_stdio_dest(&dst_info, dst_stream);

			// Comment, Exif
			jpeg_save_markers(&src_info, JPEG_COM, 0xFFFF);
			for(int i=0 ; i<16 ; i++)
				jpeg_save_markers(&src_info, JPEG_APP0 + i, 0xFFFF);

			// Read file header
			jpeg_read_header(&src_info, TRUE);

			// Read source file as DCT coefficients
			src_coef_arrays = jpeg_read_coefficients(&src_info);

			// coefficient arrays will hold the output.
			dst_coef_arrays = src_coef_arrays;

			// Initialize destination compression parameters from source values
			jpeg_copy_critical_parameters(&src_info, &dst_info);

			// Start compressor (note no image data is actually written here)
			jpeg_write_coefficients(&dst_info, dst_coef_arrays);
		}
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

}
