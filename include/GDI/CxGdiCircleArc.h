/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDICIRCLEARC_H_INCLUDED_
#define _CXGDICIRCLEARC_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxCircleArcD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiCircleArc : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxCircleArcD	Shape;

protected:
	TxGdi2dParam	m_Param;
	bool			m_Closed;

private:
	void _Constructor();

public:
	CxGdiCircleArc();
	CxGdiCircleArc( CxGdiCircleArc&& src );
	CxGdiCircleArc( const CxGdiCircleArc& src );
	CxGdiCircleArc( const TxCircleArcD& shape, bool closed = false );
	CxGdiCircleArc( const TxCircleArcI& shape, bool closed = false );
	CxGdiCircleArc(double x, double y, double radius, double start_angle, double sweep_angle, bool closed = false);
	CxGdiCircleArc(TxPointD center, double radius, double start_angle, double sweep_angle, bool closed = false);
	virtual ~CxGdiCircleArc();

	CxGdiCircleArc& operator = ( CxGdiCircleArc&& src );
	CxGdiCircleArc& operator = ( const CxGdiCircleArc& src );
	bool operator == ( const CxGdiCircleArc& src ) const;
	bool operator != ( const CxGdiCircleArc& src ) const;

	virtual TxCircleArcD Tag() const;
	virtual void* TagPtr() const;

	CxGdiCircleArc& operator = (const TxCircleArcD& src);
	CxGdiCircleArc& operator = (const TxCircleArcI& src);
	virtual operator TxCircleArcD() const;
	virtual operator TxCircleArcI() const;

	virtual CxGdiCircleArc Clone() const;

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

	double StartAngle() const;
	void StartAngle(double value);

	double SweepAngle() const;
	void SweepAngle(double value);

	bool Closed() const;
	void Closed(bool value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDIARC_H_INCLUDED_
