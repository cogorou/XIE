/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGRAPHICS_H_INCLUDED_
#define _CXGRAPHICS_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/IxTagPtr.h"
#include "Core/IxParam.h"
#include "Core/TxSizeI.h"
#include "Core/TxSizeD.h"
#include "Core/TxPointI.h"
#include "Core/TxPointD.h"
#include "Core/TxRectangleI.h"
#include "Core/TxRectangleD.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxBitmap.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGraphics : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxParam
{
protected:
	TxCanvas		m_Tag;
	CxBitmap		m_DrawImage;

#if defined(_MSC_VER)
	HDC				m_DC;
#else
	void*			m_DC;
	Display*		m_XServer;
	GLXContext		m_Context;
	Drawable		m_DrawableID;
#endif

private:
	void _Constructor();

public:
	CxGraphics();
	CxGraphics(const CxGraphics& src);
	virtual ~CxGraphics();

	CxGraphics& operator = ( const CxGraphics& src );
	bool operator == ( const CxGraphics& cmp ) const;
	bool operator != ( const CxGraphics& cmp ) const;

	virtual TxCanvas Tag() const;
	virtual void Tag(TxCanvas value);
	virtual void* TagPtr() const;

public:
	virtual void Dispose();
	virtual bool IsValid() const;

public:
#if defined(_MSC_VER)
	static bool CheckValidity(HDC hdc);
#else
	static bool CheckValidity(void* hdc);
#endif

#if defined(_MSC_VER)
	virtual void Setup(HDC hdc);
#else
	virtual void Setup(void* hdc);
#endif
	virtual void Resize(const TxSizeI& size);
	virtual void Resize(int width, int height);

	virtual void DrawImage(HxModule himage);

	// low level functions

	virtual void BeginPaint();
	virtual void EndPaint();

	virtual void Lock();
	virtual void Unlock();

public:
#if defined(_MSC_VER)
	virtual HDC				DC() const;
#else
	virtual void*			DC() const;
	virtual Display*		XServer() const;
	virtual GLXContext		Context() const;
	virtual Drawable		DrawableID() const;
#endif

public:
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

// ============================================================
#if defined(_MSC_VER)
#else
struct GDIPlus_Graphics
{
	int				backend;				// GraphicsBackEnd [-1:Invalid, 0:Cairo, 1:Metafile]
	/* cairo-specific stuff */
	void*			ct;					// cairo_t
	void*			copy_of_ctm;		// GpMatrix
	//cairo_matrix_t		previous_matrix;
	double			previous_matrix_xx;
	double			previous_matrix_yx;
	double			previous_matrix_xy;
	double			previous_matrix_yy;
	double			previous_matrix_x0;
	double			previous_matrix_y0;
	Display*		display;
	Drawable		drawable;
	void			*image;
	int				type; 
	void*			last_pen;				// GpPen
	void*			last_brush;				// GpBrush
	float			aa_offset_x;
	float			aa_offset_y;
	///* metafile-specific stuff */
	//EmfType			emf_type;
	//GpMetafile		*metafile;
	//cairo_surface_t		*metasurface;	/* bogus surface to satisfy some API calls */
	///* common-stuff */
	//GpRegion*		clip;
	//GpMatrix*		clip_matrix;
	//GpRect			bounds;
	//GpUnit			page_unit;
	//float			scale;
	//InterpolationMode	interpolation;
	//SmoothingMode		draw_mode;
	//TextRenderingHint	text_mode;
	//GpState*		saved_status;
	//int			saved_status_pos;
	//CompositingMode		composite_mode;
	//CompositingQuality	composite_quality;
	//PixelOffsetMode		pixel_mode;
	//int			render_origin_x;
	//int			render_origin_y;
	//float			dpi_x;
	//float			dpi_y;
	//int			text_contrast;
};
#endif

}
}

#pragma pack(pop)

#endif
