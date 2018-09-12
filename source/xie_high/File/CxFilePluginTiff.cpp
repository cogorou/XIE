/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginTiff.h"
#include "Core/CxException.h"
#include "Core/CxStringA.h"
#include <functional>

namespace xie
{
namespace File
{

static const char* g_ClassName = "CxFilePluginTiff";

// ============================================================
CxFilePluginTiff::CxFilePluginTiff()
{
}

// ============================================================
CxFilePluginTiff::~CxFilePluginTiff()
{
}

// ============================================================
void* CxFilePluginTiff::OpenA(TxCharCPtrA filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	// Disable Handler
	TIFFSetErrorHandler(NULL);
	TIFFSetWarningHandler(NULL);
	TIFFSetErrorHandlerExt(NULL);
	TIFFSetWarningHandlerExt(NULL);

	TIFF* tif = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		tif = TIFFOpen(filename, "r");
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		tif = TIFFOpen(filename, "w");
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return tif;
}

// ============================================================
void* CxFilePluginTiff::OpenW(TxCharCPtrW filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

#ifndef _MSC_VER
	typedef TIFF* TIFFOPENW(const wchar_t* filename_w, const char* mode_w);
	std::function<TIFFOPENW> TIFFOpenW = [](const wchar_t* filename_w, const char* mode_a)
		{
			CxStringA filename_a = CxStringA::From(filename_w);
			return TIFFOpen( filename_a.Address(), mode_a );
		};
#endif

	// Disable Handler
	TIFFSetErrorHandler(NULL);
	TIFFSetWarningHandler(NULL);
	TIFFSetErrorHandlerExt(NULL);
	TIFFSetWarningHandlerExt(NULL);

	TIFF* tif = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		tif = TIFFOpenW(filename, "r");
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		tif = TIFFOpenW(filename, "w");
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return tif;
}

// ============================================================
void CxFilePluginTiff::Close(void* handle)
{
	TIFF* tif = (TIFF*)handle;
	if (tif != NULL)
		TIFFClose( tif );
}

}
}
