/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDITRAPEZOID_H_INCLUDED_
#define _CXGDITRAPEZOID_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxTrapezoidD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiTrapezoid : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxTrapezoidD	Shape;

protected:
	TxGdi2dParam	m_Param;

private:
	void _Constructor();

public:
	CxGdiTrapezoid();
	CxGdiTrapezoid( CxGdiTrapezoid&& src );
	CxGdiTrapezoid( const CxGdiTrapezoid& src );
	CxGdiTrapezoid( const TxTrapezoidD& shape );
	CxGdiTrapezoid( const TxTrapezoidI& shape );
	CxGdiTrapezoid(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4);
	CxGdiTrapezoid(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4);
	virtual ~CxGdiTrapezoid();

	CxGdiTrapezoid& operator = ( CxGdiTrapezoid&& src );
	CxGdiTrapezoid& operator = ( const CxGdiTrapezoid& src );
	bool operator == ( const CxGdiTrapezoid& src ) const;
	bool operator != ( const CxGdiTrapezoid& src ) const;

	virtual TxTrapezoidD Tag() const;
	virtual void* TagPtr() const;

	CxGdiTrapezoid& operator = ( const TxTrapezoidD& src );
	CxGdiTrapezoid& operator = ( const TxTrapezoidI& src );
	virtual operator TxTrapezoidD() const;
	virtual operator TxTrapezoidI() const;

	virtual CxGdiTrapezoid Clone() const;

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
	double X1() const;
	void X1(double value);

	double Y1() const;
	void Y1(double value);

	double X2() const;
	void X2(double value);

	double Y2() const;
	void Y2(double value);

	double X3() const;
	void X3(double value);

	double Y3() const;
	void Y3(double value);

	double X4() const;
	void X4(double value);

	double Y4() const;
	void Y4(double value);

public:
	TxPointD Vertex1() const;
	void Vertex1(const TxPointD& value);

	TxPointD Vertex2() const;
	void Vertex2(const TxPointD& value);

	TxPointD Vertex3() const;
	void Vertex3(const TxPointD& value);

	TxPointD Vertex4() const;
	void Vertex4(const TxPointD& value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDITRAPEZOID_H_INCLUDED_
