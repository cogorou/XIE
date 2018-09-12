/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginPng.h"
#include "Core/CxException.h"
#include "Core/CxStringA.h"
#include <functional>

namespace xie
{
namespace File
{

static const char* g_ClassName = "CxFilePluginPng";

// ============================================================
CxFilePluginPng::CxFilePluginPng()
{
}

// ============================================================
CxFilePluginPng::~CxFilePluginPng()
{
}

// ============================================================
void* CxFilePluginPng::OpenA(TxCharCPtrA filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	FILE* stream = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		stream = fopen( filename, "rb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		stream = fopen( filename, "wb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return stream;
}

// ============================================================
void* CxFilePluginPng::OpenW(TxCharCPtrW filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

#if defined(_MSC_VER)
#else
	typedef FILE* WFOPEN(const wchar_t* filename_w, const wchar_t* mode_w);
	std::function<WFOPEN> _wfopen = [](const wchar_t* filename_w, const wchar_t* mode_w)
		{
			CxStringA filename_a = CxStringA::From(filename_w);
			CxStringA mode_a = CxStringA::From(mode_w);
			return fopen( filename_a.Address(), mode_a.Address() );
		};
#endif

	FILE* stream = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		stream = _wfopen( filename, L"rb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		stream = _wfopen( filename, L"wb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return stream;
}

// ============================================================
void CxFilePluginPng::Close(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

}
}
