/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginBmp.h"
#include "Core/CxException.h"
#include "Core/CxStringA.h"
#include "Core/Axi.h"
#include <functional>

namespace xie
{
namespace File
{

static const char* g_ClassName = "CxFilePluginBmp";

// ============================================================
CxFilePluginBmp::CxFilePluginBmp()
{
}

// ============================================================
CxFilePluginBmp::~CxFilePluginBmp()
{
}

// ============================================================
void* CxFilePluginBmp::OpenA(TxCharCPtrA filename, int mode)
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
void* CxFilePluginBmp::OpenW(TxCharCPtrW filename, int mode)
{
	if (filename == NULL)
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
void CxFilePluginBmp::Close(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

// ============================================================
bool CxFilePluginBmp::CreateBitmapInfo( int width, int height, int bpp, BITMAPINFO** ppBmpInfo, unsigned int* puiBmpInfoSize )
{
	if( !(1 <= bpp && bpp <= 32) )
		return false;

	int bpp_packed = 0;
	if(      bpp ==  1 )	bpp_packed =  1;
	else if( bpp <=  8 )	bpp_packed =  8;
	else if( bpp <= 16 )	bpp_packed = 16;
	else if( bpp <= 24 )	bpp_packed = 24;
	else if( bpp <= 32 )	bpp_packed = 32;

	int				size_of_image = (int)(width * abs(height) * ((double)bpp_packed / 8));
	int				iClrUsed = (bpp_packed<=8) ? (1<<bpp_packed) : 0;
	unsigned int	uiBmpInfoSize = sizeof(BITMAPINFOHEADER) + (sizeof(TxBGR8x4)*iClrUsed);
	if( puiBmpInfoSize != NULL )
		*puiBmpInfoSize = uiBmpInfoSize;

	BITMAPINFO* bmpInfo = (BITMAPINFO*)Axi::MemoryAlloc( uiBmpInfoSize );
	if( bmpInfo == NULL )
		return false;
	if( ppBmpInfo != NULL )
		*ppBmpInfo = bmpInfo;
	
	bmpInfo->bmiHeader.biSize			= sizeof( BITMAPINFOHEADER );
	bmpInfo->bmiHeader.biWidth			= width;
	bmpInfo->bmiHeader.biHeight			= height;
	bmpInfo->bmiHeader.biPlanes			= 1;
	bmpInfo->bmiHeader.biBitCount		= bpp_packed;
	bmpInfo->bmiHeader.biCompression	= BI_RGB;
	bmpInfo->bmiHeader.biSizeImage		= size_of_image;
	bmpInfo->bmiHeader.biXPelsPerMeter	= 0;	// (LONG)(72*100/2.54 + 0.9);	// 72pixel/inch
	bmpInfo->bmiHeader.biYPelsPerMeter	= 0;	// (LONG)(72*100/2.54 + 0.9);	// 72pixel/inch
	bmpInfo->bmiHeader.biClrUsed		= iClrUsed;
	bmpInfo->bmiHeader.biClrImportant	= 0;
	
	// making TxBGR8x4 Table
	if( iClrUsed != 0 )
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

	return true;
}

// ============================================================
void CxFilePluginBmp::FreeBitmapInfo( BITMAPINFO* bmpInfo )
{
	Axi::MemoryFree( bmpInfo );
}

}
}
