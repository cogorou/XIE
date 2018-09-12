/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginTiff.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxImage.h"
#include "Core/CxArrayEx.h"

#pragma warning (disable:4101)	// ローカル変数は 1 度も使われていません。

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginTiff::Save(HxModule hsrc, void* handle, int level)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		TIFF* tif = (TIFF*)handle;
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImageSize image_size = src->ImageSize();
		int max_depth = xie::Axi::CalcDepth(image_size.Model.Type);

		switch(image_size.Model.Type)
		{
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		case ExType::S8:
		case ExType::U8:
			if (image_size.Depth != 1)
				image_size.Depth = max_depth;
			break;
		case ExType::S16: image_size.Depth = 16; break;
		case ExType::U16: image_size.Depth = 16; break;
		case ExType::S32: image_size.Depth = 32; break;
		case ExType::U32: image_size.Depth = 32; break;
		case ExType::F32: image_size.Depth = 32; break;
		case ExType::S64: image_size.Depth = 64; break;
		case ExType::U64: image_size.Depth = 64; break;
		case ExType::F64: image_size.Depth = 64; break;
		}

#if 0
		int max_depth = Axi::CalcDepth(image_size.Model.Type);
		if (image_size.Depth < 1 ||
			image_size.Depth > max_depth)
			image_size.Depth = max_depth;
		else
		{
			switch(image_size.Model.Type)
			{
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			case ExType::S8:
			case ExType::U8:
				break;
			case ExType::S16:
			case ExType::U16:
				if (image_size.Depth < 8+1)
					image_size.Depth = 8+1;
				break;
			case ExType::S32:
			case ExType::U32:
				if (image_size.Depth < 16+1)
					image_size.Depth = 16+1;
				break;
			case ExType::S64:
			case ExType::U64:
				if (image_size.Depth < 32+1)
					image_size.Depth = 32+1;
				break;
			case ExType::F32:
			case ExType::F64:
				break;
			}
		}
#endif

		if (Axi::Min(image_size.Model.Pack, image_size.Channels) != 1)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		int image_width			= image_size.Width;
		int image_height		= image_size.Height;
		int bits_per_sample		= image_size.Depth;
		int samples_per_pixel	= Axi::Max(image_size.Model.Pack, image_size.Channels);
		if (samples_per_pixel == 4)
			samples_per_pixel = 3;
		int sample_format		= 0;
		int planer_config		= PLANARCONFIG_CONTIG;
		int photometric			= (samples_per_pixel == 1) ? PHOTOMETRIC_MINISBLACK : PHOTOMETRIC_RGB;

		switch (samples_per_pixel)
		{
		case 1:
		case 3:
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		switch (image_size.Model.Type)
		{
		case ExType::U8:	sample_format = SAMPLEFORMAT_UINT; break;
		case ExType::U16:	sample_format = SAMPLEFORMAT_UINT; break;
		case ExType::U32:	sample_format = SAMPLEFORMAT_UINT; break;
		case ExType::U64:	sample_format = SAMPLEFORMAT_UINT; break;
		case ExType::S8:	sample_format = SAMPLEFORMAT_INT; break;
		case ExType::S16:	sample_format = SAMPLEFORMAT_INT; break;
		case ExType::S32:	sample_format = SAMPLEFORMAT_INT; break;
		case ExType::S64:	sample_format = SAMPLEFORMAT_INT; break;
		case ExType::F32:	sample_format = SAMPLEFORMAT_IEEEFP; break;
		case ExType::F64:	sample_format = SAMPLEFORMAT_IEEEFP; break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		TIFFSetField(tif, TIFFTAG_IMAGEWIDTH, image_width);
		TIFFSetField(tif, TIFFTAG_IMAGELENGTH, image_height);
		TIFFSetField(tif, TIFFTAG_BITSPERSAMPLE, bits_per_sample);
		TIFFSetField(tif, TIFFTAG_SAMPLESPERPIXEL, samples_per_pixel);
		TIFFSetField(tif, TIFFTAG_SAMPLEFORMAT, sample_format);
		TIFFSetField(tif, TIFFTAG_PLANARCONFIG, planer_config);
		TIFFSetField(tif, TIFFTAG_PHOTOMETRIC, photometric);
		TIFFSetField(tif, TIFFTAG_FILLORDER, FILLORDER_MSB2LSB);

		if (-1 == level || (1 <= level && level <= 9))
			TIFFSetField(tif, TIFFTAG_COMPRESSION, COMPRESSION_DEFLATE, level);
		else
			TIFFSetField(tif, TIFFTAG_COMPRESSION, COMPRESSION_NONE);

		TxRangeD range = Axi::CalcRange(image_size.Model.Type, image_size.Depth);
		switch (image_size.Model.Type)
		{
		case ExType::U8:
		case ExType::U16:
		case ExType::U32:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (unsigned int)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (unsigned int)range.Upper);
			break;
		case ExType::U64:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (unsigned long long)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (unsigned long long)range.Upper);
			break;
		case ExType::S8:
		case ExType::S16:
		case ExType::S32:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (int)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (int)range.Upper);
			break;
		case ExType::S64:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (long long)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (long long)range.Upper);
			break;
		case ExType::F32:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (float)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (float)range.Upper);
			break;
		case ExType::F64:
			TIFFSetField(tif, TIFFTAG_SMINSAMPLEVALUE, (double)range.Lower);
			TIFFSetField(tif, TIFFTAG_SMAXSAMPLEVALUE, (double)range.Upper);
			break;
		default:
			break;
		}

		if (bits_per_sample == 1 && samples_per_pixel == 1)
		{
			int dst_stride = (int)TIFFScanlineSize(tif);
			CxArrayEx<unsigned char> buf(dst_stride);

			for(int y=0 ; y<image_size.Height ; y++)
			{
				void* _dst = buf.Address();
				void* _src = (*src)(0, y, 0);
				Buffer_Copy_ba<unsigned char, unsigned char>(_dst, _src, image_width);
				TIFFWriteScanline(tif, _dst, y, dst_stride);
			}
		}
		else
		{
			TxModel dst_model = TxModel(image_size.Model.Type, samples_per_pixel);
			CxArray buf(image_width, dst_model);

			// bits_per_sample が影響するので使用してはいけない.
			//int dst_stride = (int)TIFFScanlineSize(tif);	// don't use.
			int dst_stride = buf.Model().Size() * buf.Length(); 

			if (image_size.Channels == 1)
			{
				for(int y=0 ; y<image_size.Height ; y++)
				{
					int dst_pack = samples_per_pixel;
					int src_pack = image_size.Model.Pack;
					void* _dst = buf.Address();
					void* _src = (*src)(0, y, 0);
					switch(image_size.Model.Type)
					{
					case ExType::S8:
					case ExType::U8:	Buffer_Copy_pp<char>(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S16:
					case ExType::U16:	Buffer_Copy_pp<short>(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S32:
					case ExType::U32:	Buffer_Copy_pp<int>(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S64:
					case ExType::U64:	Buffer_Copy_pp<long long>(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::F32:	Buffer_Copy_pp<float>(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::F64:	Buffer_Copy_pp<double>(_dst, _src, image_width, dst_pack, src_pack); break;
					default: break;
					}
					TIFFWriteScanline(tif, _dst, y, dst_stride);
				}
			}
			else
			{
				for(int y=0 ; y<image_size.Height ; y++)
				{
					int dst_pack = samples_per_pixel;
					void* _dst = buf.Address();
					for(int ch=0 ; ch<image_size.Channels ; ch++)
					{
						void* _src = (*src)(ch, y, 0);
						switch(image_size.Model.Type)
						{
						case ExType::S8:
						case ExType::U8:	Buffer_Copy_pu<char>(_dst, _src, image_width, ch, dst_pack); break;
						case ExType::S16:
						case ExType::U16:	Buffer_Copy_pu<short>(_dst, _src, image_width, ch, dst_pack); break;
						case ExType::S32:
						case ExType::U32:	Buffer_Copy_pu<int>(_dst, _src, image_width, ch, dst_pack); break;
						case ExType::S64:
						case ExType::U64:	Buffer_Copy_pu<long long>(_dst, _src, image_width, ch, dst_pack); break;
						case ExType::F32:	Buffer_Copy_pu<float>(_dst, _src, image_width, ch, dst_pack); break;
						case ExType::F64:	Buffer_Copy_pu<double>(_dst, _src, image_width, ch, dst_pack); break;
						default: break;
						}
					}
					TIFFWriteScanline(tif, _dst, y, dst_stride);
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
