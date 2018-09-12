/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIPLIMAGE_H_INCLUDED_
#define _TXIPLIMAGE_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "TxIplROI.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxIplImage
{
	int					nSize;				// [-] sizeof(IplImage)
	int					ID;					// [-] version (=0)
	int					nChannels;			// [*] channels (1,2,3 or 4)
	int					alphaChannel;		// [-] Ignored.
	int					depth;				// [*] Pixel depth in bits:
	char				colorModel[4];		// [-] Ignored.
	char				channelSeq[4];		// [-] Ignored.
	int					dataOrder;			// [*] Pixel / Plane
	int					origin;				// [*] TopLeft / BottomLeft
	int					align;				// [-] Ignored. (Alignment of image rows (4 or 8).)
	int					width;				// [*] Image width in pixels.
	int					height;				// [*] Image height in pixels.
	TxIplROI*			roi;				// [-] Ignored.
	TxIplImage*			maskROI;			// [-] Ignored.
	void*				imageId;			// [-] Ignored.
	void*				tileInfo;			// [-] Ignored.
	int					imageSize;			// [-] Ignored.
	char*				imageData;			// [*] Pointer to aligned image data.
	int					widthStep;			// [*] Size of aligned image row in bytes.
	int					BorderMode[4];		// [-] Ignored.
	int					BorderConst[4];		// [-] Ignored.
	char*				imageDataOrigin;	// [-] Ignored.

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	static TxIplImage Default();
	TxIplImage();
	TxIplImage(void* addr, int _width, int _height, int _depth, int channels, int step);
	TxIplImage(TxImage src, int ch);

	bool operator == (const TxIplImage& cmp) const;
	bool operator != (const TxIplImage& cmp) const;

	operator TxImage() const;

	bool IsValid() const;
	void CopyTo(IxModule& dst) const;
	void CopyFrom(const IxModule& src);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
