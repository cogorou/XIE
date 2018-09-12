/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDIELLIPSE_H_INCLUDED_
#define _CXGDIELLIPSE_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxEllipseD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiEllipse : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxEllipseD		Shape;

protected:
	TxGdi2dParam	m_Param;

private:
	void _Constructor();

public:
	CxGdiEllipse();
	CxGdiEllipse( CxGdiEllipse&& src );
	CxGdiEllipse( const CxGdiEllipse& src );
	CxGdiEllipse( const TxEllipseD& shape );
	CxGdiEllipse( const TxEllipseI& shape );
	CxGdiEllipse(double x, double y, double radius_x, double radius_y);
	CxGdiEllipse(TxPointD center, double radius_x, double radius_y);
	virtual ~CxGdiEllipse();

	CxGdiEllipse& operator = ( CxGdiEllipse&& src );
	CxGdiEllipse& operator = ( const CxGdiEllipse& src );
	bool operator == ( const CxGdiEllipse& src ) const;
	bool operator != ( const CxGdiEllipse& src ) const;

	virtual TxEllipseD Tag() const;
	virtual void* TagPtr() const;

	CxGdiEllipse& operator = (const TxEllipseD& src);
	CxGdiEllipse& operator = (const TxEllipseI& src);
	virtual operator TxEllipseD() const;
	virtual operator TxEllipseI() const;

	virtual CxGdiEllipse Clone() const;

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
	virtual void Modify(HxModule, TxPointD prev_position, TxPointD move_position, double margin);

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
	double X() const;
	void X(double value);

	double Y() const;
	void Y(double value);

	double RadiusX() const;
	void RadiusX(double value);

	double RadiusY() const;
	void RadiusY(double value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDIELLIPSE_H_INCLUDED_
