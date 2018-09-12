/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDILINESEGMENT_H_INCLUDED_
#define _CXGDILINESEGMENT_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxLineSegmentD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiLineSegment : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxLineSegmentD	Shape;

protected:
	TxGdi2dParam	m_Param;

private:
	void _Constructor();

public:
	CxGdiLineSegment();
	CxGdiLineSegment( CxGdiLineSegment&& src );
	CxGdiLineSegment( const CxGdiLineSegment& src );
	CxGdiLineSegment( const TxLineSegmentD& shape );
	CxGdiLineSegment( const TxLineSegmentI& shape );
	CxGdiLineSegment(double x1, double y1, double x2, double y2);
	CxGdiLineSegment(TxPointD st, TxPointD ed);
	virtual ~CxGdiLineSegment();

	CxGdiLineSegment& operator = ( CxGdiLineSegment&& src );
	CxGdiLineSegment& operator = ( const CxGdiLineSegment& src );
	bool operator == ( const CxGdiLineSegment& src ) const;
	bool operator != ( const CxGdiLineSegment& src ) const;

	virtual TxLineSegmentD Tag() const;
	virtual void* TagPtr() const;

	CxGdiLineSegment& operator = (const TxLineSegmentD& src);
	CxGdiLineSegment& operator = (const TxLineSegmentI& src);
	virtual operator TxLineSegmentD() const;
	virtual operator TxLineSegmentI() const;

	virtual CxGdiLineSegment Clone() const;

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

	virtual void PenStyle( ExGdiPenStyle value );
	virtual ExGdiPenStyle PenStyle() const;

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

public:
	TxPointD Point1() const;
	void Point1(const TxPointD& value);

	TxPointD Point2() const;
	void Point2(const TxPointD& value);

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDILINESEGMENT_H_INCLUDED_
