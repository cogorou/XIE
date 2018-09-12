/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDICIRCLE_H_INCLUDED_
#define _CXGDICIRCLE_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxCircleD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiCircle : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxCircleD		Shape;

protected:
	TxGdi2dParam	m_Param;

private:
	void _Constructor();

public:
	CxGdiCircle();
	CxGdiCircle( CxGdiCircle&& src );
	CxGdiCircle( const CxGdiCircle& src );
	CxGdiCircle( const TxCircleD& shape );
	CxGdiCircle( const TxCircleI& shape );
	CxGdiCircle(double x, double y, double radius);
	CxGdiCircle(TxPointD center, double radius);
	virtual ~CxGdiCircle();

	CxGdiCircle& operator = ( CxGdiCircle&& src );
	CxGdiCircle& operator = ( const CxGdiCircle& src );
	bool operator == ( const CxGdiCircle& src ) const;
	bool operator != ( const CxGdiCircle& src ) const;

	virtual TxCircleD Tag() const;
	virtual void* TagPtr() const;

	CxGdiCircle& operator = (const TxCircleD& src);
	CxGdiCircle& operator = (const TxCircleI& src);
	virtual operator TxCircleD() const;
	virtual operator TxCircleI() const;

	virtual CxGdiCircle Clone() const;

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
	double X() const;
	void X(double value);

	double Y() const;
	void Y(double value);

	double Radius() const;
	void Radius(double value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDICIRCLE_H_INCLUDED_
