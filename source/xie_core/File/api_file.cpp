/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_file.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Core/CxMatrix.h"
#include "File/CxFilePluginBmp.h"
#include "Core/IxFilePlugin.h"
#include "Core/TxRawFileHeader.h"
#include <stdio.h>

namespace xie
{
namespace File
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

IxFilePluginJpeg*		p_jpeg = NULL;
IxFilePluginPng*		p_png = NULL;
IxFilePluginTiff*		p_tiff = NULL;

// //////////////////////////////////////////////////////////////////////
// Raw
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckRawA(TxRawFileHeader* header, TxCharCPtrA filename)
{
	ExStatus		status = ExStatus::Success;
	TxRawFileHeader result;
	FILE*			stream = NULL;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (header == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		// OPEN
		stream = fopen( filename, "rb" );
		if (stream == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		size_t	header_size = sizeof(TxRawFileHeader);
		memset(&result, 0, header_size);

		size_t	uiReadSize = fread(&result, header_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		status = ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	// CLOSE
	if (stream != NULL)
		fclose( stream );
	stream = NULL;

	*header = result;

	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckRawW(TxRawFileHeader* header, TxCharCPtrW filename)
{
	ExStatus		status = ExStatus::Success;
	TxRawFileHeader result;
	FILE*			stream = NULL;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (header == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

#ifndef _MSC_VER
	typedef FILE* WFOPEN(const wchar_t* filename_w, const wchar_t* mode_w);
	std::function<WFOPEN> _wfopen = [](const wchar_t* filename_w, const wchar_t* mode_w)
		{
			CxStringA filename_a = CxStringA::From(filename_w);
			CxStringA mode_a = CxStringA::From(mode_w);
			return fopen( filename_a.Address(), mode_a.Address() );
		};
#endif

		// OPEN
		stream = _wfopen( filename, L"rb" );
		if (stream == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		size_t	header_size = sizeof(TxRawFileHeader);
		memset(&result, 0, header_size);

		size_t	uiReadSize = fread(&result, header_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		status = ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	// CLOSE
	if (stream != NULL)
		fclose( stream );
	stream = NULL;

	*header = result;

	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadRawA(HxModule hdst, TxCharCPtrA filename)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		IxRawFile* dst = xie::Axi::SafeCast<IxRawFile>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		void* handle = dst->OpenRawA(filename, xie::File::XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([dst,handle]()
			{
				dst->CloseRaw(handle);
			});
		dst->LoadRaw(handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadRawW(HxModule hdst, TxCharCPtrW filename)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		IxRawFile* dst = xie::Axi::SafeCast<IxRawFile>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		void* handle = dst->OpenRawW(filename, xie::File::XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([dst,handle]()
			{
				dst->CloseRaw(handle);
			});
		dst->LoadRaw(handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveRawA(HxModule hsrc, TxCharCPtrA filename)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		IxRawFile* src = xie::Axi::SafeCast<IxRawFile>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		void* handle = src->OpenRawA(filename, xie::File::XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([src,handle]()
			{
				src->CloseRaw(handle);
			});
		src->SaveRaw(handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveRawW(HxModule hsrc, TxCharCPtrW filename)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		if (filename == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		IxRawFile* src = xie::Axi::SafeCast<IxRawFile>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		void* handle = src->OpenRawW(filename, xie::File::XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([src,handle]()
			{
				src->CloseRaw(handle);
			});
		src->SaveRaw(handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// //////////////////////////////////////////////////////////////////////
// Bmp
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckBmpA(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckBmpW(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadBmpA(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadBmpW(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveBmpA(HxModule hsrc, TxCharCPtrA filename)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenA(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Save(hsrc, handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveBmpW(HxModule hsrc, TxCharCPtrW filename)
{
	CxFilePluginBmp bmp;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = bmp.OpenW(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer stream_finalizer([&bmp,handle]()
			{
				bmp.Close(handle);
			});
		status = bmp.Save(hsrc, handle);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// //////////////////////////////////////////////////////////////////////
// Jpeg
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckJpegA(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckJpegW(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadJpegA(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadJpegW(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveJpegA(HxModule hsrc, TxCharCPtrA filename, int quality)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenA(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Save(hsrc, handle, quality);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveJpegW(HxModule hsrc, TxCharCPtrW filename, int quality)
{
	if (p_jpeg == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_jpeg->OpenW(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_jpeg->Close(handle);
			});
		status = p_jpeg->Save(hsrc, handle, quality);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// //////////////////////////////////////////////////////////////////////
// Png
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckPngA(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckPngW(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadPngA(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadPngW(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SavePngA(HxModule hsrc, TxCharCPtrA filename, int level)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenA(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Save(hsrc, handle, level);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SavePngW(HxModule hsrc, TxCharCPtrW filename, int level)
{
	if (p_png == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_png->OpenW(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_png->Close(handle);
			});
		status = p_png->Save(hsrc, handle, level);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// //////////////////////////////////////////////////////////////////////
// Tiff
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckTiffA(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckTiffW(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Check(image_size, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadTiffA(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenA(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadTiffW(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenW(filename, XIE_FILE_OPENMODE_LOAD);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Load(hdst, handle, (unpack == ExBoolean::True));
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveTiffA(HxModule hsrc, TxCharCPtrA filename, int level)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenA(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Save(hsrc, handle, level);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveTiffW(HxModule hsrc, TxCharCPtrW filename, int level)
{
	if (p_tiff == NULL) return ExStatus::Unsupported;
	ExStatus status = ExStatus::Success;
	try
	{
		void* handle = p_tiff->OpenW(filename, XIE_FILE_OPENMODE_SAVE);
		CxFinalizer handle_finalizer([handle]()
			{
				p_tiff->Close(handle);
			});
		status = p_tiff->Save(hsrc, handle, level);
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}
	return status;
}

}	// File
}	// xie
