/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDILINE_H_INCLUDED_
#define _CXGDILINE_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxLineD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "Core/TxScanner1D.h"
#include "Core/CxArray.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiLine : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxLineD			Shape;

protected:
	TxGdi2dParam	m_Param;

private:
	void _Constructor();

public:
	CxGdiLine();
	CxGdiLine( CxGdiLine&& src );
	CxGdiLine( const CxGdiLine& src );
	CxGdiLine( const TxLineD& shape );
	CxGdiLine( const TxLineI& shape );
	CxGdiLine(double a, double b, double c);
	virtual ~CxGdiLine();

	CxGdiLine& operator = ( CxGdiLine&& src );
	CxGdiLine& operator = ( const CxGdiLine& src );
	bool operator == ( const CxGdiLine& src ) const;
	bool operator != ( const CxGdiLine& src ) const;

	virtual TxLineD Tag() const;
	virtual void* TagPtr() const;

	CxGdiLine& operator = (const TxLineD& src);
	CxGdiLine& operator = (const TxLineI& src);
	virtual operator TxLineD() const;
	virtual operator TxLineI() const;

	virtual CxGdiLine Clone() const;

public:
	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

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
	double A() const;
	void A(double value);

	double B() const;
	void B(double value);

	double C() const;
	void C(double value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDILINE_H_INCLUDED_
