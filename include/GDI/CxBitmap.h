/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXBITMAP_H_INCLUDED_
#define _CXBITMAP_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxModule.h"
#include "Core/CxImage.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/TxScanner2D.h"
#include "GDI/TxBitmap.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxBitmap : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
{
protected:
	TxBitmap	m_Tag;

#if defined(_MSC_VER)
	HDC				m_hDC;
	HBITMAP			m_hBitmap;
#else
	XImage*			m_XImage;
	Display*		m_XServer;
	XVisualInfo*	m_XVisual;
#endif

private:
	void _Constructor();
	void Free();

public:
	CxBitmap();
	CxBitmap(const CxBitmap& src);
	CxBitmap(const TxSizeI& size);
	CxBitmap(int width, int height);
	virtual ~CxBitmap();

	CxBitmap& operator = ( const CxBitmap& src );
	bool operator == ( const CxBitmap& cmp ) const;
	bool operator != ( const CxBitmap& cmp ) const;

	TxBitmap Tag() const;
	virtual void* TagPtr() const;

	virtual operator CxImage() const;
	virtual void CopyTo(IxModule& dst) const;

public:
	virtual void Dispose();
	virtual bool IsValid() const;

	virtual void Resize(const TxSizeI& size);
	virtual void Resize(int width, int height);
	virtual void Clear(TxRGB8x4 value);

#if defined(_MSC_VER)
	HDC				GetHDC() const;
	HBITMAP			GetHBITMAP() const;
#else
	XImage*			GetXImage() const;
	Display*		XServer() const;
	XVisualInfo*	XVisual() const;
#endif

	virtual void* Address() const;

	virtual int Width() const;
	virtual int Height() const;
	virtual TxModel Model() const;
	virtual int Stride() const;

	virtual TxSizeI Size() const;
	virtual TxImageSize ImageSize() const;

	virtual       void* operator [] (int y);
	virtual const void* operator [] (int y) const;

	virtual       void* operator () (int y, int x);
	virtual const void* operator () (int y, int x) const;

	template<class TE> TxScanner2D<TE> Scanner() const
		{
			return TxScanner2D<TE>((TE*)m_Tag.Address, m_Tag.Width, m_Tag.Height, m_Tag.Stride);
		}
};

}
}

#pragma pack(pop)

#endif
