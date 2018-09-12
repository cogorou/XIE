/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDIPOINT_H_INCLUDED_
#define _CXGDIPOINT_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "Core/TxPointD.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiPoint : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
{
public:
	TxPointD			Shape;

protected:
	TxGdi2dParam		m_Param;
	ExGdiAnchorStyle	m_AnchorStyle;
	TxSizeD				m_AnchorSize;

private:
	void _Constructor();

public:
	CxGdiPoint();
	CxGdiPoint( CxGdiPoint&& src );
	CxGdiPoint( const CxGdiPoint& src );
	CxGdiPoint( const TxPointD& shape );
	CxGdiPoint( const TxPointI& shape );
	CxGdiPoint(double x, double y);
	virtual ~CxGdiPoint();

	CxGdiPoint& operator = ( CxGdiPoint&& src );
	CxGdiPoint& operator = ( const CxGdiPoint& src );
	bool operator == ( const CxGdiPoint& src ) const;
	bool operator != ( const CxGdiPoint& src ) const;

	virtual TxPointD Tag() const;
	virtual void* TagPtr() const;

	CxGdiPoint& operator = ( const TxPointD& src );
	CxGdiPoint& operator = ( const TxPointI& src );
	virtual operator TxPointD() const;
	virtual operator TxPointI() const;

	virtual CxGdiPoint Clone() const;

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

public:
	ExGdiAnchorStyle AnchorStyle() const;
	void AnchorStyle( ExGdiAnchorStyle value );

	TxSizeD AnchorSize() const;
	void AnchorSize( TxSizeD value );

protected:
	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDIPOINT_H_INCLUDED_
