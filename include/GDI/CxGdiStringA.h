/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDISTRINGA_H_INCLUDED_
#define _CXGDISTRINGA_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "GDI/TxGdiStringA.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "Core/CxString.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiStringA : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
	, public IxDisposable
	, public IxEquatable
	, public IxConvertible
{
protected:
	TxGdiStringA		m_Tag;

private:
	void _Constructor();

public:
	CxGdiStringA();
	CxGdiStringA( const CxGdiStringA& src );
	CxGdiStringA( const CxStringA& src );
	virtual ~CxGdiStringA();

	CxGdiStringA& operator = ( const CxGdiStringA& src );
	CxGdiStringA& operator = ( const CxStringA& src );
	CxGdiStringA& operator = ( TxCharCPtrA src );

	bool operator == ( const CxGdiStringA& src ) const;
	bool operator != ( const CxGdiStringA& src ) const;

	virtual TxGdiStringA Tag() const;
	virtual void* TagPtr() const;

	virtual operator TxCharCPtrA() const;
	virtual operator CxStringA() const;

	virtual CxGdiStringA Clone() const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;

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
	TxCharCPtrA Text() const;
	void Text(TxCharCPtrA value);

	int Length() const;

public:
	double X() const;
	void X(double value);

	double Y() const;
	void Y(double value);

	TxCharCPtrA FontName() const;
	void FontName(TxCharCPtrA value);

	int FontSize() const;
	void FontSize(int value);

	ExGdiTextAlign Align() const;
	void Align(ExGdiTextAlign value);

	int CodePage() const;
	void CodePage(int value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);

private:
	#if defined(_MSC_VER)
	HFONT _CreateFont() const;
	#else
	CxStringA _CreateFontName() const;
	#endif
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDISTRINGA_H_INCLUDED_
