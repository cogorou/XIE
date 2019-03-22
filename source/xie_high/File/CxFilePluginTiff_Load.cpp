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
ExStatus CxFilePluginTiff::Load(HxModule hdst, void* handle, bool unpack)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		TIFF* tif = (TIFF*)handle;
		if( tif == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		int photometric = 0;
		int planer_config = 0;

		int image_width = 0;
		int image_height = 0;
		int bits_per_sample = 0;
		int samples_per_pixel = 0;
		int sample_format = 0;
		int compression = 0;
		int rows_per_strip = 0;
		int scanline = (int)TIFFScanlineSize(tif);
		int is_tiled = TIFFIsTiled(tif);

		TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &photometric);
		TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLEFORMAT, &sample_format);
		TIFFGetFieldDefaulted(tif, TIFFTAG_COMPRESSION, &compression);
		TIFFGetFieldDefaulted(tif, TIFFTAG_PLANARCONFIG, &planer_config);

		TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &image_width);
		TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &image_height);
		TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &bits_per_sample);
		TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &samples_per_pixel);
		TIFFGetFieldDefaulted(tif, TIFFTAG_ROWSPERSTRIP, &rows_per_strip);

		fnXIE_Core_TraceOut(1, "%-30s : %d (%s)\n", "TIFFTAG_PHOTOMETRIC", photometric, ToString_PHOTOMETRIC(photometric).Address());
		fnXIE_Core_TraceOut(1, "%-30s : %d (%s)\n", "TIFFTAG_COMPRESSION", compression, ToString_COMPRESSION(compression).Address());
		fnXIE_Core_TraceOut(1, "%-30s : %d (%s)\n", "TIFFTAG_SAMPLEFORMAT", sample_format, ToString_SAMPLEFORMAT(sample_format).Address());
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_PLANARCONFIG", planer_config);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFIsTiled", is_tiled);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFScanlineSize", scanline);

		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_IMAGEWIDTH", image_width);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_IMAGELENGTH", image_height);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_BITSPERSAMPLE", bits_per_sample);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_SAMPLESPERPIXEL", samples_per_pixel);
		fnXIE_Core_TraceOut(1, "%-30s : %d\n", "TIFFTAG_ROWSPERSTRIP", rows_per_strip);

		// --------------------------------------------------
		if (is_tiled != 0)
		{
			LoadDefault(*dst, tif, unpack);
		}
		else if (photometric == PHOTOMETRIC_PALETTE)
		{
			LoadPalette(*dst, tif, unpack);
		}
		else if ((
			photometric == PHOTOMETRIC_RGB ||
			photometric == PHOTOMETRIC_MINISWHITE ||
			photometric == PHOTOMETRIC_MINISBLACK) &&
			planer_config == PLANARCONFIG_CONTIG
			)
		{
			LoadScanline(*dst, tif, unpack);
		}
		else
		{
			LoadDefault(*dst, tif, unpack);
		}
	}
	catch (const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

// ======================================================================
void CxFilePluginTiff::LoadDefault(CxImage& dst, TIFF* tif, bool unpack)
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	int image_width = 0;
	int image_height = 0;
	int bits_per_sample = 0;
	int samples_per_pixel = 0;
	int sample_format = 0;
	int photometric = 0;
	int planer_config = 0;
	int compression = 0;
	int rows_per_strip = 0;
	int is_tiled = TIFFIsTiled(tif);

	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &image_width);
	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &image_height);
	TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &bits_per_sample);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &samples_per_pixel);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLEFORMAT, &sample_format);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &photometric);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PLANARCONFIG, &planer_config);
	TIFFGetFieldDefaulted(tif, TIFFTAG_COMPRESSION, &compression);
	TIFFGetFieldDefaulted(tif, TIFFTAG_ROWSPERSTRIP, &rows_per_strip);

	ExType type = ToExType(bits_per_sample, sample_format);
	if (type == ExType::None)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	TxImageSize image_size;
	image_size.Width = image_width;
	image_size.Height = image_height;
	image_size.Model.Type = type;

	if (unpack == false)
	{
		image_size.Model.Pack = samples_per_pixel;
		image_size.Channels = 1;
	}
	else
	{
		image_size.Model.Pack = 1;
		image_size.Channels = samples_per_pixel;
	}

	if (Axi::CalcDepth(type) != bits_per_sample)
		image_size.Depth = bits_per_sample;

	dst.Resize(image_size);

	// ------------------------------------------------------------
	if (is_tiled != 0)
	{
		int tile_width = 0;
		int tile_height = 0;
		TIFFGetField(tif, TIFFTAG_TILEWIDTH, &tile_width);
		TIFFGetField(tif, TIFFTAG_TILELENGTH, &tile_height);

		TxModel	tile_model(type, 4);
		CxImage src_tile(tile_width, tile_height, tile_model, 1, 1);
		CxImage dst_tile(tile_width, tile_height, tile_model, 1, 1);

		int rows = (image_height + (tile_height - 1)) / tile_height;
		int cols = (image_width + (tile_width - 1)) / tile_width;

		for(int y=0 ; y<rows ; y++)
		{
			for(int x=0 ; x<cols ; x++)
			{
				TxRectangleI bounds;
				bounds.X = x * tile_width;
				bounds.Y = y * tile_height;
				bounds.Width = Axi::Min(tile_width, image_width - bounds.X);
				bounds.Height = Axi::Min(tile_height, image_height - bounds.Y);

				void* tile_addr = src_tile.Address(0);

				TIFFReadRGBATile(tif, bounds.X, bounds.Y, (unsigned int*)tile_addr);

				dst_tile.Filter().Mirror(src_tile, 2);	// 1=X,2=Y,3=XY

				CxImage tile_roi;
				tile_roi.Attach(dst_tile, TxRectangleI(0, 0, bounds.Width, bounds.Height));

				CxImage dst_roi;
				dst_roi.Attach(dst, bounds);
				dst_roi.Filter().Copy(tile_roi);
			}
		}
	}
	//else if (rows_per_strip != 0)
	//{
	//	CxImage buf1(image_width, image_height, TxModel(type, 4), 1);
	//	CxImage buf2(image_width, image_height, TxModel(type, 4), 1);

	//	TIFFReadRGBAStrip(tif, image_height, (unsigned int*)buf1.Address(0));

	//	buf2.Filter().Mirror(buf1, 2);	// 1=X,2=Y,3=XY
	//	dst.Filter().Copy(buf2);
	//}
	else
	{
		CxImage buf1(image_width, image_height, TxModel(type, 4), 1);
		CxImage buf2(image_width, image_height, TxModel(type, 4), 1);

		TIFFReadRGBAImage(tif, image_width, image_height, (unsigned int*)buf1.Address(0), 0);

		buf2.Filter().Mirror(buf1, 2);	// 1=X,2=Y,3=XY
		dst.Filter().Copy(buf2);
	}
}

// ======================================================================
void CxFilePluginTiff::LoadScanline(CxImage& dst, TIFF* tif, bool unpack)
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	int image_width = 0;
	int image_height = 0;
	int bits_per_sample = 0;
	int samples_per_pixel = 0;
	int sample_format = 0;
	int photometric = 0;
	int planer_config = 0;
	int compression = 0;

	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &image_width);
	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &image_height);
	TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &bits_per_sample);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &samples_per_pixel);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLEFORMAT, &sample_format);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &photometric);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PLANARCONFIG, &planer_config);
	TIFFGetFieldDefaulted(tif, TIFFTAG_COMPRESSION, &compression);

	ExType type = ToExType(bits_per_sample, sample_format);
	if (type == ExType::None)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	// ------------------------------------------------------------
	// ２値.
	if (bits_per_sample == 1 && samples_per_pixel == 1)
	{
		if (Axi::SizeOf(type) != 1)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		TxImageSize image_size;
		image_size.Width = image_width;
		image_size.Height = image_height;
		image_size.Model.Type = type;
		image_size.Model.Pack = samples_per_pixel;
		image_size.Channels = 1;
		image_size.Depth = bits_per_sample;
		dst.Resize(image_size);

		int scanline = (int)TIFFScanlineSize(tif);
		CxArrayEx<unsigned char> buf(scanline);

		for (int y = 0; y < image_size.Height; y++)
		{
			void* _dst = dst(0, y, 0);
			void* _src = buf.Address();
			TIFFReadScanline(tif, _src, y);
			TxRangeI value = (photometric == PHOTOMETRIC_MINISBLACK)
				? TxRangeI(0, 1)
				: TxRangeI(1, 0);
			Buffer_Copy_ab<unsigned char, unsigned char>(_dst, _src, image_width, value);
		}
	}
	// ------------------------------------------------------------
	// CMYK
	else if (photometric == PHOTOMETRIC_SEPARATED)
	{
		TxImageSize image_size;
		image_size.Width = image_width;
		image_size.Height = image_height;
		image_size.Model.Type = type;

		if (unpack == false)
		{
			image_size.Model.Pack = samples_per_pixel;
			image_size.Channels = 1;
		}
		else
		{
			image_size.Model.Pack = 1;
			image_size.Channels = samples_per_pixel;
		}
		if (Axi::CalcDepth(type) != bits_per_sample)
			image_size.Depth = bits_per_sample;

		dst.Resize(image_size);

		// ------------------------------------------------------------
		{
			int wbytes = image_width * samples_per_pixel * Axi::SizeOf(image_size.Model.Type);
			CxArrayEx<unsigned char> buf_cmyk(wbytes);
			CxArrayEx<unsigned char> buf_rgba(wbytes);

			TxRangeD range = Axi::CalcRange(image_size.Model.Type, image_size.Depth);

			if (unpack == false)
			{
				for (int y = 0; y < image_size.Height; y++)
				{
					int dst_pack = image_size.Model.Pack;
					int src_pack = samples_per_pixel;
					void* _dst = dst(0, y, 0);
					void* _src = buf_rgba.Address();
					void* _buf = buf_cmyk.Address();
					TIFFReadScanline(tif, _buf, y);

					// CMYK to RGB
					switch (image_size.Model.Type)
					{
					case ExType::U8:	Buffer_CmykToRgb_pp<unsigned char>		(_src, _buf, image_width, src_pack, src_pack, range.Upper); break;
					case ExType::U16:	Buffer_CmykToRgb_pp<unsigned short>		(_src, _buf, image_width, src_pack, src_pack, range.Upper); break;
					default: break;
					}

					// RGB
					switch (image_size.Model.Type)
					{
					case ExType::U8:	Buffer_Copy_pp<unsigned char>		(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::U16:	Buffer_Copy_pp<unsigned short>		(_dst, _src, image_width, dst_pack, src_pack); break;
					default: break;
					}
				}
			}
			else
			{
				for (int y = 0; y < image_size.Height; y++)
				{
					int src_pack = samples_per_pixel;
					void* _src = buf_rgba.Address();
					void* _buf = buf_cmyk.Address();
					TIFFReadScanline(tif, _src, y);

					// CMYK to RGB
					switch (image_size.Model.Type)
					{
					case ExType::U8:	Buffer_CmykToRgb_pp<unsigned char>		(_src, _buf, image_width, src_pack, src_pack, range.Upper); break;
					case ExType::U16:	Buffer_CmykToRgb_pp<unsigned short>		(_src, _buf, image_width, src_pack, src_pack, range.Upper); break;
					default: break;
					}

					// RGB
					for (int ch = 0; ch < image_size.Channels; ch++)
					{
						void* _dst = dst(ch, y, 0);
						switch (image_size.Model.Type)
						{
						case ExType::U8:	Buffer_Copy_up<unsigned char>		(_dst, _src, image_width, ch, src_pack); break;
						case ExType::U16:	Buffer_Copy_up<unsigned short>		(_dst, _src, image_width, ch, src_pack); break;
						default: break;
						}
					}
				}
			}
		}
	}
	// ------------------------------------------------------------
	// RGB / Gray
	else
	{
		TxImageSize image_size;
		image_size.Width = image_width;
		image_size.Height = image_height;
		image_size.Model.Type = type;

		if (unpack == false)
		{
			image_size.Model.Pack = samples_per_pixel;
			image_size.Channels = 1;
		}
		else
		{
			image_size.Model.Pack = 1;
			image_size.Channels = samples_per_pixel;
		}
		if (Axi::CalcDepth(type) != bits_per_sample)
			image_size.Depth = bits_per_sample;

		dst.Resize(image_size);

		// ------------------------------------------------------------
		if (type != ExType::F32 &&
			type != ExType::F64 &&
			(bits_per_sample % 8) != 0 &&
			(1 < bits_per_sample && bits_per_sample < 16))
		{
			CxImage buf1(image_width, image_height, TxModel(type, 4), 1);
			CxImage buf2(image_width, image_height, TxModel(type, 4), 1);

			TIFFReadRGBAImage(tif, image_width, image_height, (unsigned int*)buf1.Address(0), 0);

			buf2.Filter().Mirror(buf1, 2);	// 1=X,2=Y,3=XY
			dst.Filter().Copy(buf2);
		}
		else
		{
			int model_size = Axi::SizeOf(image_size.Model.Type);
			int wbytes = image_width * samples_per_pixel * model_size;
			int scanline = (int)TIFFScanlineSize(tif);
			CxArrayEx<unsigned char> buf(scanline);

			if (unpack == false)
			{
				for (int y = 0; y < image_size.Height; y++)
				{
					int dst_pack = image_size.Model.Pack;
					int src_pack = samples_per_pixel;
					void* _dst = dst(0, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					switch (image_size.Model.Type)
					{
					case ExType::S8:
					case ExType::U8:	Buffer_Copy_pp<char>		(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S16:
					case ExType::U16:	Buffer_Copy_pp<short>		(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S32:
					case ExType::U32:	Buffer_Copy_pp<int>			(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::S64:
					case ExType::U64:	Buffer_Copy_pp<long long>	(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::F32:	Buffer_Copy_pp<float>		(_dst, _src, image_width, dst_pack, src_pack); break;
					case ExType::F64:	Buffer_Copy_pp<double>		(_dst, _src, image_width, dst_pack, src_pack); break;
					default: break;
					}
				}
			}
			else
			{
				for (int y = 0; y < image_size.Height; y++)
				{
					int src_pack = samples_per_pixel;
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					for (int ch = 0; ch < image_size.Channels; ch++)
					{
						void* _dst = dst(ch, y, 0);
						switch (image_size.Model.Type)
						{
						case ExType::S8:
						case ExType::U8:	Buffer_Copy_up<char>		(_dst, _src, image_width, ch, src_pack); break;
						case ExType::S16:
						case ExType::U16:	Buffer_Copy_up<short>		(_dst, _src, image_width, ch, src_pack); break;
						case ExType::S32:
						case ExType::U32:	Buffer_Copy_up<int>			(_dst, _src, image_width, ch, src_pack); break;
						case ExType::S64:
						case ExType::U64:	Buffer_Copy_up<long long>	(_dst, _src, image_width, ch, src_pack); break;
						case ExType::F32:	Buffer_Copy_up<float>		(_dst, _src, image_width, ch, src_pack); break;
						case ExType::F64:	Buffer_Copy_up<double>		(_dst, _src, image_width, ch, src_pack); break;
						default: break;
						}
					}
				}
			}

			if (photometric == PHOTOMETRIC_MINISWHITE)
			{
				TxRangeD range = xie::Axi::CalcRange(image_size.Model.Type, image_size.Depth);
				switch(image_size.Model.Type)
				{
				case ExType::U8:
				case ExType::U16:
				case ExType::U32:
				case ExType::U64:
					dst.Filter().Sub(range.Upper, dst);
					break;
				default:
					break;
				}
			}
		}
	}
}

// ======================================================================
void CxFilePluginTiff::LoadPalette(CxImage& dst, TIFF* tif, bool unpack)
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	int image_width = 0;
	int image_height = 0;
	int bits_per_sample = 0;
	int samples_per_pixel = 0;
	int sample_format = 0;
	int photometric = 0;
	int planer_config = 0;
	int compression = 0;
	int rows_per_strip = 0;

	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &image_width);
	TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &image_height);
	TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &bits_per_sample);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &samples_per_pixel);
	TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLEFORMAT, &sample_format);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &photometric);
	TIFFGetFieldDefaulted(tif, TIFFTAG_PLANARCONFIG, &planer_config);
	TIFFGetFieldDefaulted(tif, TIFFTAG_COMPRESSION, &compression);
	TIFFGetFieldDefaulted(tif, TIFFTAG_ROWSPERSTRIP, &rows_per_strip);

	typedef unsigned short	TD;
	typedef unsigned short	TP;

	TP* colormapR = NULL;
	TP* colormapG = NULL;
	TP* colormapB = NULL;
	TIFFGetField(tif, TIFFTAG_COLORMAP, &colormapR, &colormapG, &colormapB);

	TxImageSize image_size;
	image_size.Width = image_width;
	image_size.Height = image_height;
	image_size.Model.Type = TypeOf<TD>();

	int colors = 3;
	if (unpack == false)
	{
		image_size.Model.Pack = colors;
		image_size.Channels = 1;
	}
	else
	{
		image_size.Model.Pack = 1;
		image_size.Channels = colors;
	}

	dst.Resize(image_size);

	// ------------------------------------------------------------
	{
		int scanline = (int)TIFFScanlineSize(tif);
		CxArrayEx<unsigned char> buf(scanline);

		if (unpack == false)
		{
			switch(bits_per_sample)
			{
			case 1:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst = dst(0, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_p1<TD, TS>(_dst, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			case 2:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst = dst(0, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_p2<TD, TS>(_dst, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			case 4:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst = dst(0, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_p4<TD, TS>(_dst, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			default:
				if (5 <= bits_per_sample && bits_per_sample <= 8)
				{
					for (int y = 0; y < image_size.Height; y++)
					{
						typedef unsigned char	TS;
						void* _dst = dst(0, y, 0);
						void* _src = buf.Address();
						TIFFReadScanline(tif, _src, y);
						Palette_Copy_p8<TD, TS>(_dst, _src, colormapR, colormapG, colormapB, image_width, colors);
					}
				}
				else if (9 <= bits_per_sample && bits_per_sample <= 16)
				{
					for (int y = 0; y < image_size.Height; y++)
					{
						typedef unsigned short	TS;
						void* _dst = dst(0, y, 0);
						void* _src = buf.Address();
						TIFFReadScanline(tif, _src, y);
						Palette_Copy_p8<TD, TS>(_dst, _src, colormapR, colormapG, colormapB, image_width, colors);
					}
				}
				else
				{
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				}
				break;
			}
		}
		else
		{
			switch(bits_per_sample)
			{
			case 1:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst0 = dst(0, y, 0);
					void* _dst1 = dst(1, y, 0);
					void* _dst2 = dst(2, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_u1<TD, TS>(_dst0, _dst1, _dst2, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			case 2:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst0 = dst(0, y, 0);
					void* _dst1 = dst(1, y, 0);
					void* _dst2 = dst(2, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_u2<TD, TS>(_dst0, _dst1, _dst2, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			case 4:
				for (int y = 0; y < image_size.Height; y++)
				{
					typedef unsigned char	TS;
					void* _dst0 = dst(0, y, 0);
					void* _dst1 = dst(1, y, 0);
					void* _dst2 = dst(2, y, 0);
					void* _src = buf.Address();
					TIFFReadScanline(tif, _src, y);
					Palette_Copy_u4<TD, TS>(_dst0, _dst1, _dst2, _src, colormapR, colormapG, colormapB, image_width, colors);
				}
				break;
			default:
				if (5 <= bits_per_sample && bits_per_sample <= 8)
				{
					for (int y = 0; y < image_size.Height; y++)
					{
						typedef unsigned char	TS;
						void* _dst0 = dst(0, y, 0);
						void* _dst1 = dst(1, y, 0);
						void* _dst2 = dst(2, y, 0);
						void* _src = buf.Address();
						TIFFReadScanline(tif, _src, y);
						Palette_Copy_u8<TD, TS>(_dst0, _dst1, _dst2, _src, colormapR, colormapG, colormapB, image_width, colors);
					}
				}
				else if (9 <= bits_per_sample && bits_per_sample <= 16)
				{
					for (int y = 0; y < image_size.Height; y++)
					{
						typedef unsigned short	TS;
						void* _dst0 = dst(0, y, 0);
						void* _dst1 = dst(1, y, 0);
						void* _dst2 = dst(2, y, 0);
						void* _src = buf.Address();
						TIFFReadScanline(tif, _src, y);
						Palette_Copy_u8<TD, TS>(_dst0, _dst1, _dst2, _src, colormapR, colormapG, colormapB, image_width, colors);
					}
				}
				else
				{
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				}
				break;
			}
		}
	}
}

// ======================================================================
CxStringA CxFilePluginTiff::ToString_PHOTOMETRIC(int value)
{
	switch(value)
	{
	case PHOTOMETRIC_MINISWHITE	: return CxStringA::Format("PHOTOMETRIC_MINISWHITE");	// 0	/* min value is white */
	case PHOTOMETRIC_MINISBLACK	: return CxStringA::Format("PHOTOMETRIC_MINISBLACK");	// 1	/* min value is black */
	case PHOTOMETRIC_RGB		: return CxStringA::Format("PHOTOMETRIC_RGB");			// 2	/* RGB color model */
	case PHOTOMETRIC_PALETTE	: return CxStringA::Format("PHOTOMETRIC_PALETTE");		// 3	/* color map indexed */
	case PHOTOMETRIC_MASK		: return CxStringA::Format("PHOTOMETRIC_MASK");			// 4	/* $holdout mask */
	case PHOTOMETRIC_SEPARATED	: return CxStringA::Format("PHOTOMETRIC_SEPARATED");	// 5	/* !color separations */
	case PHOTOMETRIC_YCBCR		: return CxStringA::Format("PHOTOMETRIC_YCBCR");		// 6	/* !CCIR 601 */
	case PHOTOMETRIC_CIELAB		: return CxStringA::Format("PHOTOMETRIC_CIELAB");		// 8	/* !1976 CIE L*a*b* */
	case PHOTOMETRIC_ICCLAB		: return CxStringA::Format("PHOTOMETRIC_ICCLAB");		// 9	/* ICC L*a*b* [Adobe TIFF Technote 4] */
	case PHOTOMETRIC_ITULAB		: return CxStringA::Format("PHOTOMETRIC_ITULAB");		// 10	/* ITU L*a*b* */
	case PHOTOMETRIC_LOGL		: return CxStringA::Format("PHOTOMETRIC_LOGL");			// 32844	/* CIE Log2(L) */
	case PHOTOMETRIC_LOGLUV		: return CxStringA::Format("PHOTOMETRIC_LOGLUV");		// 32845	/* CIE Log2(L) (u',v') */
	}
	return CxString::Format("");
}

// ======================================================================
CxStringA CxFilePluginTiff::ToString_SAMPLEFORMAT(int value)
{
	switch(value)
	{
	case SAMPLEFORMAT_VOID:				return CxStringA::Format("SAMPLEFORMAT_VOID");
	case SAMPLEFORMAT_UINT:				return CxStringA::Format("SAMPLEFORMAT_UINT");
	case SAMPLEFORMAT_INT:				return CxStringA::Format("SAMPLEFORMAT_INT");
	case SAMPLEFORMAT_IEEEFP:			return CxStringA::Format("SAMPLEFORMAT_IEEEFP");
	case SAMPLEFORMAT_COMPLEXINT:		return CxStringA::Format("SAMPLEFORMAT_COMPLEXINT");
	case SAMPLEFORMAT_COMPLEXIEEEFP:	return CxStringA::Format("SAMPLEFORMAT_COMPLEXIEEEFP");
	}
	return CxString::Format("");
}

// ======================================================================
CxStringA CxFilePluginTiff::ToString_COMPRESSION(int value)
{
	switch(value)
	{
	case COMPRESSION_NONE:			return CxStringA::Format("COMPRESSION_NONE");
	case COMPRESSION_CCITTRLE:		return CxStringA::Format("COMPRESSION_CCITTRLE");
	case COMPRESSION_CCITTFAX3:		return CxStringA::Format("COMPRESSION_CCITTFAX3");
//	case COMPRESSION_CCITT_T4:		return CxStringA::Format("COMPRESSION_CCITT_T4");
	case COMPRESSION_CCITTFAX4:		return CxStringA::Format("COMPRESSION_CCITTFAX4");
//	case COMPRESSION_CCITT_T6:		return CxStringA::Format("COMPRESSION_CCITT_T6");
	case COMPRESSION_LZW:			return CxStringA::Format("COMPRESSION_LZW");
	case COMPRESSION_OJPEG:			return CxStringA::Format("COMPRESSION_OJPEG");
	case COMPRESSION_JPEG:			return CxStringA::Format("COMPRESSION_JPEG");
//	case COMPRESSION_T85:			return CxStringA::Format("COMPRESSION_T85");
//	case COMPRESSION_T43:			return CxStringA::Format("COMPRESSION_T43");
	case COMPRESSION_NEXT:			return CxStringA::Format("COMPRESSION_NEXT");
	case COMPRESSION_CCITTRLEW:		return CxStringA::Format("COMPRESSION_CCITTRLEW");
	case COMPRESSION_PACKBITS:		return CxStringA::Format("COMPRESSION_PACKBITS");
	case COMPRESSION_THUNDERSCAN:	return CxStringA::Format("COMPRESSION_THUNDERSCAN");
	case COMPRESSION_IT8CTPAD:		return CxStringA::Format("COMPRESSION_IT8CTPAD");
	case COMPRESSION_IT8LW:			return CxStringA::Format("COMPRESSION_IT8LW");
	case COMPRESSION_IT8MP:			return CxStringA::Format("COMPRESSION_IT8MP");
	case COMPRESSION_IT8BL:			return CxStringA::Format("COMPRESSION_IT8BL");
	case COMPRESSION_PIXARFILM:		return CxStringA::Format("COMPRESSION_PIXARFILM");
	case COMPRESSION_PIXARLOG:		return CxStringA::Format("COMPRESSION_PIXARLOG");
	case COMPRESSION_DEFLATE:		return CxStringA::Format("COMPRESSION_DEFLATE");
	case COMPRESSION_ADOBE_DEFLATE:	return CxStringA::Format("COMPRESSION_ADOBE_DEFLATE");
	case COMPRESSION_DCS:			return CxStringA::Format("COMPRESSION_DCS");
	case COMPRESSION_JBIG:			return CxStringA::Format("COMPRESSION_JBIG");
	case COMPRESSION_SGILOG:		return CxStringA::Format("COMPRESSION_SGILOG");
	case COMPRESSION_SGILOG24:		return CxStringA::Format("COMPRESSION_SGILOG24");
	case COMPRESSION_JP2000:		return CxStringA::Format("COMPRESSION_JP2000");
//	case COMPRESSION_LZMA:			return CxStringA::Format("COMPRESSION_LZMA");
	}
	return CxString::Format("");
}

// ======================================================================
ExType CxFilePluginTiff::ToExType(int bits_per_sample, int sample_format)
{
	ExType type = ExType::None;
	if (bits_per_sample <= 8)
	{
		switch (sample_format)
		{
		default:
		case SAMPLEFORMAT_VOID:
		case SAMPLEFORMAT_UINT:		type = ExType::U8; break;
		case SAMPLEFORMAT_INT:		type = ExType::S8; break;
		case SAMPLEFORMAT_COMPLEXINT:
		case SAMPLEFORMAT_COMPLEXIEEEFP:
			break;
		}
	}
	else if (bits_per_sample <= 16)
	{
		switch (sample_format)
		{
		default:
		case SAMPLEFORMAT_VOID:
		case SAMPLEFORMAT_UINT:		type = ExType::U16; break;
		case SAMPLEFORMAT_INT:		type = ExType::S16; break;
		case SAMPLEFORMAT_COMPLEXINT:
		case SAMPLEFORMAT_COMPLEXIEEEFP:
			break;
		}
	}
	else if (bits_per_sample <= 32)
	{
		switch (sample_format)
		{
		default:
		case SAMPLEFORMAT_VOID:
		case SAMPLEFORMAT_UINT:		type = ExType::U32; break;
		case SAMPLEFORMAT_INT:		type = ExType::S32; break;
		case SAMPLEFORMAT_IEEEFP:	type = ExType::F32; break;
		case SAMPLEFORMAT_COMPLEXINT:
		case SAMPLEFORMAT_COMPLEXIEEEFP:
			break;
		}
	}
	else if (bits_per_sample <= 64)
	{
		switch (sample_format)
		{
		default:
		case SAMPLEFORMAT_VOID:
		case SAMPLEFORMAT_UINT:		type = ExType::U64; break;
		case SAMPLEFORMAT_INT:		type = ExType::S64; break;
		case SAMPLEFORMAT_IEEEFP:	type = ExType::F64; break;
		case SAMPLEFORMAT_COMPLEXINT:
		case SAMPLEFORMAT_COMPLEXIEEEFP:
			break;
		}
	}
	return type;
}

}
}
