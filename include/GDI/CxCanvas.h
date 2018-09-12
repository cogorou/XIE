/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXCANVAS_H_INCLUDED_
#define _CXCANVAS_H_INCLUDED_

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
#include "GDI/TxCanvas.h"
#include "GDI/CxBitmap.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

typedef void XIE_API fnXIE_GDI_CallBack_Render(void* canvas, void* param, ExGdiScalingMode mode);

class XIE_EXPORT_CLASS CxCanvas : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxParam
{
protected:
	TxCanvas		m_Tag;
	CxBitmap		m_DrawImage;

#if defined(_MSC_VER)
	HDC				m_Target;
	HGLRC			m_Context;
	CxBitmap		m_Buffer;
#else
	GLXDrawable		m_Target;
	GLXContext		m_Context;
	Window			m_WindowID;
	Display*		m_XServer;
	XVisualInfo*	m_XVisual;
	unsigned int	m_FrameBufferID;
	unsigned int	m_RenderBufferID;
	unsigned int	m_TextureID;
#endif

private:
	void _Constructor();
	void Free();

public:
	CxCanvas();
	CxCanvas(const CxCanvas& src);
	virtual ~CxCanvas();

	CxCanvas& operator = ( const CxCanvas& src );
	bool operator == ( const CxCanvas& cmp ) const;
	bool operator != ( const CxCanvas& cmp ) const;

	virtual TxCanvas Tag() const;
	virtual void* TagPtr() const;

public:
	virtual void Dispose();
	virtual bool IsValid() const;

public:
#if defined(_MSC_VER)
	virtual void Setup(HDC target);
#else
	virtual void Setup(GLXDrawable target);
#endif
	virtual void Resize(const TxSizeI& size);
	virtual void Resize(int width, int height);

	virtual void Clear();

	virtual void DrawImage(HxModule himage);

	virtual void DrawOverlayCB(fnXIE_GDI_CallBack_Render render, void* param, ExGdiScalingMode mode);

	virtual void DrawOverlay(HxModule hfigure, ExGdiScalingMode mode);
	virtual void DrawOverlay(const HxModule* hfigures, int count, ExGdiScalingMode mode);

	template<class TE> void DrawOverlay(const CxArrayEx<TE>& figures, ExGdiScalingMode mode)
		{
			CxArrayEx<HxModule> handles(figures.Length());
			for(int i=0 ; i<figures.Length() ; i++)
				handles[i] = static_cast<HxModule>(figures[i]);
			DrawOverlay(handles.Address(), handles.Length(), mode);
		}
	template<class TE> void DrawOverlay(const std::vector<TE>& figures, ExGdiScalingMode mode)
		{
			CxArrayEx<HxModule> handles((int)figures.size());
			for(int i=0 ; i<(int)figures.size() ; i++)
				handles[i] = static_cast<HxModule>(figures[i]);
			DrawOverlay(handles, mode);
		}
	template<class TE> void DrawOverlay(const std::initializer_list<TE>& figures, ExGdiScalingMode mode)
		{
			CxArrayEx<HxModule> handles((int)figures.size());
			int index = 0;
			for(auto iter=figures.begin() ; iter!=figures.end() ; iter++, index++)
				handles[index] = static_cast<HxModule>(*iter);
			DrawOverlay(handles, mode);
		}

	virtual void Flush();
	virtual void Flush(HxModule hdst);

	// low level functions

	virtual void BeginPaint();
	virtual void EndPaint();

	virtual void Lock();
	virtual void Unlock();

public:
#if defined(_MSC_VER)
	virtual HDC				Target() const;
	virtual HGLRC			Context() const;
	virtual const CxBitmap&	Buffer() const;
#else
	virtual GLXDrawable		Target() const;
	virtual GLXContext		Context() const;
	virtual Display*		XServer() const;
	virtual XVisualInfo*	XVisual() const;
	virtual Window			WindowID() const;
	virtual unsigned int FrameBufferID() const;
	virtual unsigned int RenderBufferID() const;
	virtual unsigned int TextureID() const;
#endif

public:
	virtual TxSizeI Size() const;
	virtual int Width() const;
	virtual int Height() const;

	virtual TxSizeI BgSize() const;
	virtual void BgSize(TxSizeI value);

	virtual TxRGB8x4 BkColor() const;
	virtual void BkColor(TxRGB8x4 value);

	virtual bool BkEnable() const;
	virtual void BkEnable(bool value);

	virtual double Magnification() const;
	virtual void Magnification(double value);

	virtual TxPointD ViewPoint() const;
	virtual void ViewPoint(TxPointD value);

	virtual int ChannelNo() const;
	virtual void ChannelNo(int value);

	virtual bool Unpack() const;
	virtual void Unpack(bool value);

	virtual bool Halftone() const;
	virtual void Halftone(bool value);

	virtual TxSizeI DisplaySize() const;
	virtual TxRectangleI DisplayRect() const;
	virtual TxRectangleI EffectiveRect() const;
	virtual TxRectangleD VisibleRect() const;
	virtual TxRectangleI VisibleRectI(bool includePartialPixel) const;
	virtual TxPointD DPtoIP(const TxPointD& dp, ExGdiScalingMode mode) const;
	virtual TxPointD IPtoDP(const TxPointD& ip, ExGdiScalingMode mode) const;

protected:
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);

public:
	static TxRectangleI	EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag);
	static TxRectangleD	VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point);
	static TxPointD		DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, xie::GDI::ExGdiScalingMode mode);
	static TxPointD		IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, xie::GDI::ExGdiScalingMode mode);
};

}
}

#pragma pack(pop)

#endif
