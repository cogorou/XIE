/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginPng.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/TxImageSize.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginPng::Check(TxImageSize* image_size, void* handle, bool unpack)
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

		// PNG Information Object
		png_structp png_ptr = NULL;
		png_infop	info_ptr = NULL;

		CxFinalizer png_finalizer([&png_ptr,&info_ptr]()
			{
				// Clean up after the read, and free any memory allocated - REQUIRED
				if (png_ptr != NULL && info_ptr != NULL)
					png_destroy_read_struct(&png_ptr, &info_ptr, NULL);
				else if (png_ptr != NULL)
					png_destroy_read_struct(&png_ptr, NULL, NULL);
				png_ptr = NULL;
				info_ptr = NULL;
			});

		size_t			uiReadSize = 0;

		int				width = 0;
		int				height = 0;
		int				bit_depth = 0;
		int				color_type = 0;
		int				interlace_type = 0;

		// ----------------------------------------------------------------------
		// 初期化.
		// ----------------------------------------------------------------------
		{
			// シグネチャの確認.
			const size_t	SIG_SIZE = 4;
			char			buf[SIG_SIZE];

			// Read in some of the signature bytes
			uiReadSize = fread(buf, 1, SIG_SIZE, stream);
			if (uiReadSize != SIG_SIZE)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// Compare the first PNG_BYTES_TO_CHECK bytes of the signature.
			if (0 != png_sig_cmp((png_bytep)buf, (png_size_t)0, SIG_SIZE))
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			// Create and initialize the png_struct.
			png_ptr = png_create_read_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
			if (png_ptr == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// Allocate/initialize the memory for image information.  REQUIRED.
			info_ptr = png_create_info_struct(png_ptr);
			if (info_ptr == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// Set error handling
			if (setjmp( png_jmpbuf(png_ptr)))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// One of the following I/O initialization methods is REQUIRED
			png_init_io(png_ptr, stream);

			// If we have already read some of the signature.
			png_set_sig_bytes(png_ptr, (int)SIG_SIZE);
		}

		// ----------------------------------------------------------------------
		// 情報の読み込み.
		// ----------------------------------------------------------------------
		{
			// The call to png_read_info() gives us all of the information from the
			// PNG file before the first IDAT (image data chunk).  REQUIRED
			png_read_info(png_ptr, info_ptr);
		}

		// ----------------------------------------------------------------------
		// 入力変換設定.
		// ----------------------------------------------------------------------
		{
			/* Set up the data transformations you want.
			 * Note that these are all optional.
			 * Only call them if you want/need them.
			 * Many of the transformations only work on specific types of images, and many are mutually exclusive.
			 */

			png_get_IHDR(png_ptr, info_ptr, (png_uint_32*)&width, (png_uint_32*)&height, &bit_depth, &color_type, &interlace_type, NULL, NULL);

			// Tell libpng to strip 16 bit/color files down to 8 bits/color.
			// Use accurate scaling if it's available, otherwise just chop off the low byte.
			//#ifdef PNG_READ_SCALE_16_TO_8_SUPPORTED
			//png_set_scale_16(png_ptr);
			//#else
			//png_set_strip_16(png_ptr);
			//#endif

			// Strip alpha bytes from the input data without combining with the background (not recommended).
			//png_set_strip_alpha(png_ptr);
			if (color_type != PNG_COLOR_TYPE_RGB_ALPHA)
				png_set_strip_alpha(png_ptr);

			// Extract multiple pixels with bit depths of 1, 2, and 4 from a single
			// byte into separate bytes (useful for paletted and grayscale images).
			png_set_packing(png_ptr);

			// Change the order of packed pixels to least significant bit first
			// (not useful if you are using png_set_packing).
			png_set_packswap(png_ptr);

			// Expand paletted colors into true RGB triplets
			if (color_type == PNG_COLOR_TYPE_PALETTE)
			{
				png_set_palette_to_rgb(png_ptr);

				// ２値や濃淡をパレットカラーとして保存された画像の対策.
				{
					bool		is_gray = true;
					png_colorp	palette;
					int			palette_size;

					png_get_PLTE( png_ptr, info_ptr, &palette, &palette_size );
					for(int i=0 ; i<palette_size ; i++)
					{
						if (palette[i].red != palette[i].green ||
							palette[i].red != palette[i].blue)
						{
							is_gray = false;
							break;
						}
					}

					if (is_gray)
						png_set_rgb_to_gray(png_ptr, 1, 0, 0);
				}
			}

			// Expand grayscale images to the full 8 bits from 1, 2, or 4 bits/pixel
			if (color_type == PNG_COLOR_TYPE_GRAY && bit_depth < 8)
				png_set_expand_gray_1_2_4_to_8(png_ptr);

			// Expand paletted or RGB images with transparency to full alpha channels
			// so the data will be available as RGBA quartets.
			if (png_get_valid(png_ptr, info_ptr, PNG_INFO_tRNS))
				png_set_tRNS_to_alpha(png_ptr);

			// Set the background color to draw transparent and alpha images over.
			/*
				It is possible to set the red, green, and blue components directly
				for paletted images instead of supplying a palette index.  Note that
				even if the PNG file supplies a background, you are not required to
				use it - you should use the (solid) application background if it has one.
			*/
			{
				//png_color_16	my_background;
				//png_color_16*	image_background;

				//if (png_get_bKGD(png_ptr, info_ptr, &image_background))
				//	png_set_background(png_ptr, image_background, PNG_BACKGROUND_GAMMA_FILE, 1, 1.0);
				//else
				//	png_set_background(png_ptr, &my_background, PNG_BACKGROUND_GAMMA_SCREEN, 0, 1.0);
			}

			// Some suggestions as to how to get a screen gamma value
			/*
				Note that screen gamma is the display_exponent, which includes
				the CRT_exponent and any correction for viewing conditions
			*/

			// --- setup a gamma value.
			// (!) When file has not a gamma value,
			//     the value calculated from the following is set up.
			//
			//            (1)  (2)  (3)  (4)
			// Windows  : 1.0  2.2  2.2  0.45
			// Macintosh: 1.0  1.8  1.8  0.55
			// 
			// (1) A gamma value in case the luminosity value on a memory is changed into a video signal
			// (2) The gamma value at the time of a video signal being reflected as luminosity on a monitor.
			// (3) The gamma value of the display system of PC. [ex: (1) * (2) ]
			// (4) The gamma value of a image file. [ex: 1.0 / (2) ]
			// 

			// Tell libpng to handle the gamma conversion for you.
			{
				double screen_gamma = 2.2;
				int intent;

				if (png_get_sRGB(png_ptr, info_ptr, &intent))
					png_set_gamma(png_ptr, screen_gamma, -1.0);
				else
				{
					double image_gamma;
					if (png_get_gAMA(png_ptr, info_ptr, &image_gamma))
						png_set_gamma(png_ptr, screen_gamma, image_gamma);
					else
						png_set_gamma(png_ptr, screen_gamma, 0.45455);
				}
			}

			// Invert monochrome files to have 0 as white and 1 as black
			//png_set_invert_mono(png_ptr);

			/* If you want to shift the pixel values from the range [0,255] or
				* [0,65535] to the original [0,7] or [0,31], or whatever range the
				* colors were originally in:
				*/
			if (png_get_valid(png_ptr, info_ptr, PNG_INFO_sBIT))
			{
				png_color_8p sig_bit_p;
				png_get_sBIT(png_ptr, info_ptr, &sig_bit_p);
				png_set_shift(png_ptr, sig_bit_p);
			}

			// これはやってはいけない.
			// Swap bytes of 16 bit files to least significant byte first
			if (bit_depth > 8)
				png_set_swap(png_ptr);

			// Flip the RGB pixels to BGR (or RGBA to BGRA)
			//if (color_type & PNG_COLOR_MASK_COLOR)
			//	png_set_bgr(png_ptr);

			// これはやってはいけない.
			// 前述の処理で既に入れ替わっている.
			// Swap the RGBA or GA data to ARGB or AG (or BGRA to ABGR)
			//png_set_swap_alpha(png_ptr);

			// これをやってはいけない.
			// やると alpha チャネルが必ず付加される.
			// Add filler (or alpha) byte (before/after each RGB triplet)
			//png_set_filler(png_ptr, 0xff, PNG_FILLER_AFTER);

			// Optional call to gamma correct and add the background to the palette and update info structure.  REQUIRED
			/*
			 * if you are expecting libpng to update the palette for you
			 * (ie you selected such a transform above).
			*/
			png_read_update_info(png_ptr, info_ptr);
		}

		// ----------------------------------------------------------------------
		// 画像オブジェクトの確保.
		// ----------------------------------------------------------------------
		{
			png_get_IHDR(png_ptr, info_ptr, (png_uint_32*)&width, (png_uint_32*)&height, &bit_depth, &color_type, &interlace_type, NULL, NULL);

			TxModel	model = TxModel::Default();
			int			channels = 0;
			int			depth = -1;

			if (unpack == false)
			{
				channels = 1;
				depth = bit_depth;

				switch( color_type )
				{
				case PNG_COLOR_TYPE_GRAY:					// 0
					model = (depth <= 8)
						? TxModel::U8(1)
						: TxModel::U16(1);
					break;
				case PNG_COLOR_TYPE_RGB:					// 2
					model = (depth <= 8)
						? TxModel::U8(3)
						: TxModel::U16(3);
					break;
				case PNG_COLOR_TYPE_RGB_ALPHA:				// 6 = 2 | 4
					model = (depth <= 8)
						? TxModel::U8(4)
						: TxModel::U16(4);
					break;
				default:
				case PNG_COLOR_TYPE_GRAY_ALPHA:				// 4 = 0 | 4
				case PNG_COLOR_TYPE_PALETTE:				// 3 = 2 | 1
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				}
			}
			else
			{
				depth = bit_depth;
				model = (depth <= 8)
					? TxModel::U8(1)
					: TxModel::U16(1);

				switch( color_type )
				{
				case PNG_COLOR_TYPE_GRAY:					// 0
					channels = 1;
					break;
				case PNG_COLOR_TYPE_RGB:					// 2
					channels = 3;
					break;
				case PNG_COLOR_TYPE_RGB_ALPHA:				// 6 = 2 | 4
					channels = 3;
					break;
				default:
				case PNG_COLOR_TYPE_GRAY_ALPHA:				// 4 = 0 | 4
				case PNG_COLOR_TYPE_PALETTE:				// 3 = 2 | 1
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				}
			}

			// Allocate.
			result = TxImageSize( width, height, model, channels );
			result.Depth = depth;
		}

		// Read rest of file, and get additional chunks in info_ptr - REQUIRED
		png_read_end(png_ptr, info_ptr);
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
