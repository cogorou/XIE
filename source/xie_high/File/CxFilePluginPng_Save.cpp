/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxFilePluginPng.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"

namespace xie
{
namespace File
{

// ======================================================================
ExStatus CxFilePluginPng::Save(HxModule hsrc, void* handle, int level)
{
	ExStatus	status = ExStatus::Success;

	try
	{
		FILE* stream = (FILE*)handle;
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		png_structp		png_ptr = NULL;
		png_infop		info_ptr = NULL;

		CxFinalizer png_finalizer([&png_ptr, &info_ptr]()
			{
				// Clean up after the write, and free any memory allocated
				if (png_ptr != NULL && info_ptr != NULL)
					png_destroy_write_struct(&png_ptr, &info_ptr);
				else if (png_ptr != NULL)
					png_destroy_write_struct(&png_ptr, NULL);
				png_ptr = NULL;
				info_ptr = NULL;
			});

		// ----------------------------------------------------------------------
		// 初期化.
		// ----------------------------------------------------------------------
		{
			png_ptr = png_create_write_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
			if (png_ptr == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			info_ptr = png_create_info_struct(png_ptr);
			if (info_ptr == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			if (setjmp(png_jmpbuf(png_ptr)))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			png_init_io(png_ptr, stream);

			// 圧縮率の設定.
			// Z_NO_COMPRESSION			 0
			// Z_BEST_SPEED				 1
			// Z_BEST_COMPRESSION		 9
			// Z_DEFAULT_COMPRESSION	-1
			png_set_compression_level( png_ptr, level );

			// Set the palette if there is one.  REQUIRED for indexed-color images
			//png_colorp palette;
			//palette = (png_colorp)png_malloc(png_ptr, PNG_MAX_PALETTE_LENGTH * (sizeof (png_color)));
			//png_set_PLTE(png_ptr, info_ptr, palette, PNG_MAX_PALETTE_LENGTH);

			int	color_type			= PNG_COLOR_TYPE_GRAY;
			int	interlace_type		= PNG_INTERLACE_NONE;
			int	compression_type	= PNG_COMPRESSION_TYPE_BASE;
			int	filter_type			= PNG_FILTER_TYPE_BASE;

			// ------------------------------------------------------------

			switch(src->Channels())
			{
			case 1:
				switch(src->Model().Type)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case ExType::U8:
				case ExType::U16:
					break;
				}
				switch(src->Model().Pack)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case 1:
					color_type = PNG_COLOR_TYPE_GRAY;
					break;
				case 3:
					color_type = PNG_COLOR_TYPE_RGB;
					break;
				case 4:
					color_type = PNG_COLOR_TYPE_RGB;	// ※ アルファ値を使用しない.
					break;
				}
				break;
			case 3:
				if (src->Model().Pack != 1)
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				switch(src->Model().Type)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case ExType::U8:
				case ExType::U16:
					color_type = PNG_COLOR_TYPE_RGB;
					break;
				}
				break;
			case 4:
				if (src->Model().Pack != 1)
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				switch(src->Model().Type)
				{
				default:
					throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
				case ExType::U8:
				case ExType::U16:
					color_type = PNG_COLOR_TYPE_RGB_ALPHA;	// ※ CH0,1,2=RGB、CH3=Alpha
					break;
				}
				break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}

			/* Set the image information here.  Width and height are up to 2^31,
			 * bit_depth is one of 1, 2, 4, 8, or 16, but valid values also depend on
			 * the color_type selected. color_type is one of PNG_COLOR_TYPE_GRAY,
			 * PNG_COLOR_TYPE_GRAY_ALPHA, PNG_COLOR_TYPE_PALETTE, PNG_COLOR_TYPE_RGB,
			 * or PNG_COLOR_TYPE_RGB_ALPHA.  interlace is either PNG_INTERLACE_NONE or
			 * PNG_INTERLACE_ADAM7, and the compression_type and filter_type MUST
			 * currently be PNG_COMPRESSION_TYPE_BASE and PNG_FILTER_TYPE_BASE. REQUIRED
			 */

			int	width		= src->Width();
			int	height		= src->Height();
			int	bit_depth	= src->Depth();
			int	max_depth	= Axi::CalcDepth(src->Model().Type);
			if (!(0 < bit_depth && bit_depth <= max_depth))
				bit_depth = max_depth;

			png_set_IHDR(png_ptr, info_ptr, width, height, max_depth, color_type, interlace_type, compression_type, filter_type);

			/* Set the true bit depth of the image data */
			png_color_8	sig_bit;
			sig_bit.gray	= 0;
			sig_bit.red		= 0;
			sig_bit.green	= 0;
			sig_bit.blue	= 0;
			sig_bit.alpha	= 0;

			if (color_type & PNG_COLOR_MASK_COLOR)
			{
				sig_bit.red		= (png_byte)bit_depth;
				sig_bit.green	= (png_byte)bit_depth;
				sig_bit.blue	= (png_byte)bit_depth;
			}
			else
			{
				sig_bit.gray	= (png_byte)bit_depth;
			}
			if (color_type & PNG_COLOR_MASK_ALPHA)
			{
				sig_bit.alpha	= (png_byte)bit_depth;
			}
			png_set_sBIT( png_ptr, info_ptr, &sig_bit );

			/* Shift the pixels up to a legal bit depth and fill in
			 * as appropriate to correctly scale the image.
			 */
			png_set_shift(png_ptr, &sig_bit);

			// Optional gamma chunk is strongly suggested if you have any guess as to the correct gamma of the image.
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
			// (4) The gamma value of a image file. [ex: 1.0 / (3) ]
			{
				double	dbLUT = 1.0;					// (1)
				double	dbCRT = 2.2;					// (2)
				double	dbDisplay = dbLUT * dbCRT;		// (3)
				double	dbGamma   = 1.0 / dbDisplay;	// (4)
				png_set_gAMA( png_ptr, info_ptr, dbGamma );
			}

			/* Other optional chunks like cHRM, bKGD, tRNS, tIME, oFFs, pHYs */

			/* Note that if sRGB is present the gAMA and cHRM chunks must be ignored
			 * on read and, if your application chooses to write them, they must
			 * be written in accordance with the sRGB profile
			 */

			// Write the file header information.  REQUIRED
			png_write_info(png_ptr, info_ptr);

			// Invert monochrome pixels
			//png_set_invert_mono(png_ptr);

			// Swap location of alpha bytes from ARGB to RGBA
			//png_set_swap_alpha(png_ptr);

			/* Get rid of filler (OR ALPHA) bytes, pack XRGB/RGBX/ARGB/RGBA into
			 * RGB (4 channels -> 3 channels). The second parameter is not used.
			 */
			//png_set_filler(png_ptr, 0, PNG_FILLER_BEFORE);

			/* Flip BGR pixels to RGB */
			//png_set_bgr(png_ptr);

			// Swap bytes of 16-bit files to most significant byte first
			if (max_depth == 16)
				png_set_swap(png_ptr);

			// Swap bits of 1, 2, 4 bit packed pixel formats
			if (max_depth < 8)
				png_set_packswap(png_ptr);
		}

		// ----------------------------------------------------------------------
		// データ書き込み.
		// ----------------------------------------------------------------------
		{
			int		width	= src->Width();
			int		height	= src->Height();
			CxArray	buf;

			// Pack pixels into bytes
			png_set_packing(png_ptr);

			switch(src->Channels())
			{
			case 1:
				switch(src->Model().Type)
				{
				default:
					break;

				case ExType::U8:
					{
						typedef unsigned char	TE;
						switch(src->Model().Pack)
						{
						case 1:
							for(int y=0 ; y<height ; y++)
							{
								png_write_row( png_ptr, (png_bytep)(*src)(0, y, 0) );
							}
							break;
						case 3:
							buf.Resize(width, TxRGBx3<TE>::Model());
							for(int y=0 ; y<height ; y++)
							{
								Buffer_Copy_pp<TxRGBx3<TE>, TxRGBx3<TE>>(
									(TxRGBx3<TE>*)buf[0],
									(TxRGBx3<TE>*)(*src)(0, y, 0),
									width);
								png_write_row( png_ptr, (png_bytep)buf[0] );
							}
							break;
						case 4:
							buf.Resize(width, TxRGBx3<TE>::Model());
							for(int y=0 ; y<height ; y++)
							{
								Buffer_Copy_pp<TxRGBx3<TE>, TxRGBx4<TE>>(
									(TxRGBx3<TE>*)buf[0],
									(TxRGBx4<TE>*)(*src)(0, y, 0),
									width);
								png_write_row( png_ptr, (png_bytep)buf[0] );
							}
							break;
						}
					}
					break;
				case ExType::U16:
					{
						typedef unsigned short	TE;
						switch(src->Model().Pack)
						{
						case 1:
							for(int y=0 ; y<height ; y++)
							{
								png_write_row( png_ptr, (png_bytep)(*src)(0, y, 0) );
							}
							break;
						case 3:
							buf.Resize(width, TxRGBx3<TE>::Model());
							for(int y=0 ; y<height ; y++)
							{
								Buffer_Copy_pp<TxRGBx3<TE>, TxRGBx3<TE>>(
									(TxRGBx3<TE>*)buf[0],
									(TxRGBx3<TE>*)(*src)(0, y, 0),
									width);
								png_write_row( png_ptr, (png_bytep)buf[0] );
							}
							break;
						case 4:
							buf.Resize(width, TxRGBx3<TE>::Model());
							for(int y=0 ; y<height ; y++)
							{
								Buffer_Copy_pp<TxRGBx3<TE>, TxRGBx4<TE>>(
									(TxRGBx3<TE>*)buf[0],
									(TxRGBx4<TE>*)(*src)(0, y, 0),
									width);
								png_write_row( png_ptr, (png_bytep)buf[0] );
							}
							break;
						}
					}
					break;
				}
				break;
			case 3:
				switch(src->Model().Type)
				{
				default:
					break;

				case ExType::U8:
					{
						typedef unsigned char	TE;
						buf.Resize(width, TxRGBx3<TE>::Model());
						for(int y=0 ; y<height ; y++)
						{
							Buffer_Copy_pu<TxRGBx3<TE>, TE>(
								(TxRGBx3<TE>*)buf[0],
								(TE*)(*src)(0, y, 0),
								(TE*)(*src)(1, y, 0),
								(TE*)(*src)(2, y, 0),
								width);
							png_write_row( png_ptr, (png_bytep)buf[0] );
						}
					}
					break;
				case ExType::U16:
					{
						typedef unsigned short	TE;
						buf.Resize(width, TxRGBx3<TE>::Model());
						for(int y=0 ; y<height ; y++)
						{
							Buffer_Copy_pu<TxRGBx3<TE>, TE>(
								(TxRGBx3<TE>*)buf[0],
								(TE*)(*src)(0, y, 0),
								(TE*)(*src)(1, y, 0),
								(TE*)(*src)(2, y, 0),
								width);
							png_write_row( png_ptr, (png_bytep)buf[0] );
						}
					}
					break;
				}
				break;
			case 4:
				switch(src->Model().Type)
				{
				default:
					break;

				case ExType::U8:
					{
						typedef unsigned char	TE;
						buf.Resize(width, TxRGBx4<TE>::Model());
						for(int y=0 ; y<height ; y++)
						{
							Buffer_Copy_qu<TxRGBx4<TE>, TE>(
								(TxRGBx4<TE>*)buf[0],
								(TE*)(*src)(0, y, 0),
								(TE*)(*src)(1, y, 0),
								(TE*)(*src)(2, y, 0),
								(TE*)(*src)(3, y, 0),
								width);
							png_write_row( png_ptr, (png_bytep)buf[0] );
						}
					}
					break;
				case ExType::U16:
					{
						typedef unsigned char	TE;
						buf.Resize(width, TxRGBx4<TE>::Model());
						for(int y=0 ; y<height ; y++)
						{
							Buffer_Copy_qu<TxRGBx4<TE>, TE>(
								(TxRGBx4<TE>*)buf[0],
								(TE*)(*src)(0, y, 0),
								(TE*)(*src)(1, y, 0),
								(TE*)(*src)(2, y, 0),
								(TE*)(*src)(3, y, 0),
								width);
							png_write_row( png_ptr, (png_bytep)buf[0] );
						}
					}
					break;
				}
				break;
			}
		}

		// ----------------------------------------------------------------------
		// テキストチャンクの書き込み.
		// ----------------------------------------------------------------------
#if 0
		{
			png_text text_ptr[1];

			char key0[]		= "Library";
			char text0[]	= "XIE";
			text_ptr[0].key			= key0;
			text_ptr[0].text		= text0;
			text_ptr[0].compression	= PNG_TEXT_COMPRESSION_NONE;
			text_ptr[0].itxt_length	= 0;
			text_ptr[0].lang		= NULL;
			text_ptr[0].lang_key	= NULL;

			png_set_text(png_ptr, info_ptr, text_ptr, 3);
		}
#endif

		// It is REQUIRED to call this to finish writing the rest of the file
		png_write_end(png_ptr, info_ptr);

		//png_free(png_ptr, palette);
		//palette = NULL;
		//png_free(png_ptr, trans);
		//trans = NULL;
	}
	catch(const CxException& ex)
	{
		status = ex.Code();
	}

	return status;
}

}
}
