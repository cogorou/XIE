/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_FILE_H_INCLUDED_
#define _API_FILE_H_INCLUDED_

#include "xie_core.h"
#include "api_core.h"
#include "Core/IxFilePlugin.h"

namespace xie
{
namespace File
{

// ////////////////////////////////////////////////////////////
// CONSTANT VARIABLE

const int XIE_FILE_OPENMODE_LOAD = 0;
const int XIE_FILE_OPENMODE_SAVE = 1;

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

extern IxFilePluginJpeg*	p_jpeg;
extern IxFilePluginPng*		p_png;
extern IxFilePluginTiff*	p_tiff;

// ////////////////////////////////////////////////////////////
// PROTOTYPE

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckRawA	(TxRawFileHeader* header, TxCharCPtrA filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckRawW	(TxRawFileHeader* header, TxCharCPtrW filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadRawA	(HxModule hdst, TxCharCPtrA filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadRawW	(HxModule hdst, TxCharCPtrW filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveRawA	(HxModule hsrc, TxCharCPtrA filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveRawW	(HxModule hsrc, TxCharCPtrW filename);

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckBmpA	(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckBmpW	(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadBmpA	(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadBmpW	(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveBmpA	(HxModule hsrc, TxCharCPtrA filename);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveBmpW	(HxModule hsrc, TxCharCPtrW filename);

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckJpegA	(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckJpegW	(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadJpegA	(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadJpegW	(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveJpegA	(HxModule hsrc, TxCharCPtrA filename, int quality);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveJpegW	(HxModule hsrc, TxCharCPtrW filename, int quality);

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckPngA	(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckPngW	(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadPngA	(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadPngW	(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SavePngA	(HxModule hsrc, TxCharCPtrA filename, int level);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SavePngW	(HxModule hsrc, TxCharCPtrW filename, int level);

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckTiffA	(TxImageSize* image_size, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_CheckTiffW	(TxImageSize* image_size, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadTiffA	(HxModule hdst, TxCharCPtrA filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_LoadTiffW	(HxModule hdst, TxCharCPtrW filename, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveTiffA	(HxModule hsrc, TxCharCPtrA filename, int level);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_File_SaveTiffW	(HxModule hsrc, TxCharCPtrW filename, int level);

XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DIB_Load	(HxModule hdst, void* pvdib, ExBoolean unpack);
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DIB_Save	(HxModule hsrc, void** ppvdib, unsigned int* puiBmpInfoSize, unsigned int* puiImageSize);

// ////////////////////////////////////////////////////////////
// TYPEDEF

#ifndef _MSC_VER

#define BI_RGB        0L
#define BI_RLE8       1L
#define BI_RLE4       2L
#define BI_BITFIELDS  3L
#define BI_JPEG       4L
#define BI_PNG        5L

// ======================================================================
struct TxBGR555
{
	unsigned short B: 5, G: 5, R: 5, A:1;
};

// ======================================================================
struct TxBGR565
{
	unsigned short B: 5, G: 6, R: 5;
};

// ======================================================================
struct RGBQUAD
{
	unsigned char rgbBlue;
	unsigned char rgbGreen;
	unsigned char rgbRed;
	unsigned char rgbReserved;
} XIE_PACKED;

// ======================================================================
struct BITMAPINFOHEADER
{
	unsigned int	biSize;
	int				biWidth;
	int				biHeight;
	unsigned short	biPlanes;
	unsigned short	biBitCount;
	unsigned int	biCompression;
	unsigned int	biSizeImage;
	int				biXPelsPerMeter;
	int				biYPelsPerMeter;
	unsigned int	biClrUsed;
	unsigned int	biClrImportant;
} XIE_PACKED;

// ======================================================================
struct BITMAPINFO
{
	BITMAPINFOHEADER	bmiHeader;
	RGBQUAD				bmiColors[1];
} XIE_PACKED;

// ======================================================================
struct BITMAPFILEHEADER
{
	unsigned short	bfType;
	unsigned int	bfSize;
	short			bfReserved1;
	short			bfReserved2;
	unsigned int	bfOffBits;
} XIE_PACKED;

#endif

}	// File
}	// xie

#endif
