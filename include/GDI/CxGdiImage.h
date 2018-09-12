/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDIIMAGE_H_INCLUDED_
#define _CXGDIIMAGE_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "GDI/TxGdiImage.h"
#include "GDI/CxBitmap.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/CxImage.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiImage : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
	, public IxDisposable
	, public IxEquatable
	, public IxConvertible
{
protected:
	TxGdiImage		m_Tag;
	bool			m_IsAttached;
#if defined(_MSC_VER)
	HBITMAP			m_hBitmap;
#endif

private:
	void _Constructor();

public:
	CxGdiImage();
	CxGdiImage( CxGdiImage&& src );
	CxGdiImage( const CxGdiImage& src );
	CxGdiImage( const CxImage& src );
	CxGdiImage( int width, int height );
	virtual ~CxGdiImage();

	CxGdiImage& operator = ( CxGdiImage&& src );
	CxGdiImage& operator = ( const CxGdiImage& src );
	CxGdiImage& operator = ( const CxImage& src );
	bool operator == ( const CxGdiImage& src ) const;
	bool operator != ( const CxGdiImage& src ) const;

	virtual TxGdiImage Tag() const;
	virtual void* TagPtr() const;

	virtual CxGdiImage Clone() const;

protected:
	virtual void MoveFrom(CxGdiImage& src);

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;
	virtual bool IsAttached() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;
	virtual operator CxImage() const;

public:
	// IxGdi2dRendering
	virtual void Render(HxModule hcanvas, ExGdiScalingMode mode) const;

public:
	// IxGdi2dHandling
	virtual TxPointD Location() const;
	virtual void Location(TxPointD value);
	virtual TxRectangleD Bounds() const;

	virtual double Angle() const;
	virtual void Angle( double degree );

	virtual TxPointD Axis() const;
	virtual void Axis( TxPointD value );

	virtual TxHitPosition HitTest( TxPointD position, double margin ) const;
	virtual void Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin);

public:
	// IxGdi2dVisualizing
	virtual TxRGB8x4 BkColor() const;
	virtual void BkColor( TxRGB8x4 value );

	virtual bool BkEnable() const;
	virtual void BkEnable( bool value );

	virtual TxRGB8x4 PenColor() const;
	virtual void PenColor( TxRGB8x4 value );

	virtual ExGdiPenStyle PenStyle() const;
	virtual void PenStyle( ExGdiPenStyle value );

	virtual int PenWidth() const;
	virtual void PenWidth( int value );

public:
	virtual TxGdi2dParam Param() const;
	virtual void Param( const TxGdi2dParam& value );

public:
	virtual void Resize(const TxSizeI& size);
	virtual void Resize(int width, int height);
	virtual void Reset();

	virtual int Width() const;
	virtual int Height() const;
	virtual TxModel Model() const;
	virtual int Stride() const;

	virtual TxSizeI Size() const;
	virtual TxImageSize ImageSize() const;

	virtual       void* Address();
	virtual const void* Address() const;

	virtual       void* operator [] (int y);
	virtual const void* operator [] (int y) const;

	virtual       void* operator () (int y, int x);
	virtual const void* operator () (int y, int x) const;

	template<class TE> TxScanner2D<TE> Scanner() const
		{
			return TxScanner2D<TE>((TE*)m_Tag.Address, m_Tag.Width, m_Tag.Height, m_Tag.Stride);
		}
	template<class TE> TxScanner2D<TE> Scanner(const TxRectangleI& bounds) const
		{
			if (bounds.X < 0 || bounds.Y < 0)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.X + bounds.Width <= m_Tag.Width))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.Y + bounds.Height <= m_Tag.Height))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			return TxScanner2D<TE>((TE*)(*this)(bounds.Y, bounds.X), bounds.Width, bounds.Height, m_Tag.Stride, m_Tag.Model);
		}

public:
	double X() const;
	void X(double value);

	double Y() const;
	void Y(double value);

	TxSizeD VisualSize() const;

	double MagX() const;
	void MagX(double value);

	double MagY() const;
	void MagY(double value);

	double Alpha() const;
	void Alpha(double value);

	bool AlphaFormat() const;
	void AlphaFormat(bool value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDIIMAGE_H_INCLUDED_
