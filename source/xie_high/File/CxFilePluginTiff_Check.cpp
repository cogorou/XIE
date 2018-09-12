/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginTiff.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/TxImageSize.h"

#pragma warning (disable:4101)	// ローカル変数は 1 度も使われていません。

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginTiff::Check(TxImageSize* image_size, void* handle, bool unpack)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		TIFF* tif = (TIFF*)handle;
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		if (image_size == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		int image_width = 0;
		int image_height = 0;
		int bits_per_sample = 0;
		int samples_per_pixel = 0;
		int sample_format = 0;
		int photometric = 0;
		int planer_config = 0;

		TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &image_width);
		TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &image_height);
		TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &bits_per_sample);
		TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &samples_per_pixel);
		TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLEFORMAT, &sample_format);
		TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &photometric);
		TIFFGetFieldDefaulted(tif, TIFFTAG_PLANARCONFIG, &planer_config);

		switch (photometric)
		{
		default:
			{
				ExType type = ExType::None;
				switch (sample_format)
				{
				case SAMPLEFORMAT_UINT:
					if (bits_per_sample <= 8)		type = ExType::U8;
					else if (bits_per_sample <= 16)	type = ExType::U16;
					else if (bits_per_sample <= 32)	type = ExType::U32;
					else if (bits_per_sample <= 64)	type = ExType::U64;
					break;
				case SAMPLEFORMAT_INT:
					if (bits_per_sample <= 8)		type = ExType::S8;
					else if (bits_per_sample <= 16)	type = ExType::S16;
					else if (bits_per_sample <= 32)	type = ExType::S32;
					else if (bits_per_sample <= 64)	type = ExType::S64;
					break;
				case SAMPLEFORMAT_IEEEFP:
					type = ExType::F32;
					break;
				case SAMPLEFORMAT_VOID:
				case SAMPLEFORMAT_COMPLEXINT:
				case SAMPLEFORMAT_COMPLEXIEEEFP:
				default:
					break;
				}

				int colors = samples_per_pixel;
				switch(planer_config)
				{
				default:
				case PLANARCONFIG_CONTIG:
					break;
				case PLANARCONFIG_SEPARATE:
					switch(samples_per_pixel)
					{
					case 1: colors = 1; break;
					case 2: colors = 1; break;	// ignore alpha
					case 3: colors = 3; break;
					case 4: colors = 4; break;
					}
					break;
				}

				image_size->Width = image_width;
				image_size->Height = image_height;
				image_size->Model.Type = type;

				if (unpack == false)
				{
					image_size->Model.Pack = colors;
					image_size->Channels = 1;
				}
				else
				{
					image_size->Model.Pack = 1;
					image_size->Channels = colors;
				}

				if (Axi::CalcDepth(type) != bits_per_sample)
					image_size->Depth = bits_per_sample;
			}
			break;
		case PHOTOMETRIC_PALETTE:
			{
				image_size->Width = image_width;
				image_size->Height = image_height;
				image_size->Model.Type = ExType::U16;

				if (unpack == false)
				{
					image_size->Model.Pack = 3;
					image_size->Channels = 1;
				}
				else
				{
					image_size->Model.Pack = 1;
					image_size->Channels = 3;
				}
			}
			break;
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
