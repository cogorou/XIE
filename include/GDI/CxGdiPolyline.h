/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDIPOLYLINE_H_INCLUDED_
#define _CXGDIPOLYLINE_H_INCLUDED_

#include "Core/CxModule.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"
#include "GDI/TxGdiPolyline.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "Core/TxScanner1D.h"
#include "Core/CxArray.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxGdiPolyline : public CxModule
	, public IxGdi2d
	, public IxTagPtr
	, public IxParam
	, public IxDisposable
	, public IxEquatable
	, public IxConvertible
{
public:
	typedef TxPointD	TE;

protected:
	TxGdiPolyline	m_Tag;
	bool			m_IsAttached;

private:
	void _Constructor();

public:
	static CxGdiPolyline From(const xie::CxArrayEx<TE>& src)
	{
		CxGdiPolyline result(src.Length());
		result.Scanner().Copy(src);
		return result;
	}
	static CxGdiPolyline From(const std::vector<TE>& src)
	{
		CxGdiPolyline result((int)src.size());
		result.Scanner().Copy(src);
		return result;
	}
	static CxGdiPolyline From(const std::initializer_list<TE>& src)
	{
		CxGdiPolyline result((int)src.size());
		result.Scanner().Copy(src);
		return result;
	}

public:
	CxGdiPolyline();
	CxGdiPolyline( CxGdiPolyline&& src );
	CxGdiPolyline( const CxGdiPolyline& src );
	CxGdiPolyline(int length);
	CxGdiPolyline(const CxArray& src);
	virtual ~CxGdiPolyline();

	CxGdiPolyline& operator = (const CxArray& src);
	CxGdiPolyline& operator = (const xie::CxArrayEx<TE>& src);

	CxGdiPolyline& operator = ( CxGdiPolyline&& src );
	CxGdiPolyline& operator = ( const CxGdiPolyline& src );
	bool operator == ( const CxGdiPolyline& src ) const;
	bool operator != ( const CxGdiPolyline& src ) const;

	virtual TxGdiPolyline Tag() const;
	virtual void* TagPtr() const;

	virtual operator CxArray() const;

	virtual operator xie::CxArrayEx<TE>() const
	{
		xie::CxArrayEx<TE> dst = this->Scanner();
		return dst;
	}

	virtual operator std::vector<TE>() const
	{
		std::vector<TE> dst = this->Scanner();
		return dst;
	}

	virtual CxGdiPolyline Clone() const;

protected:
	virtual void MoveFrom(CxGdiPolyline& src);

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
	virtual void Resize(int length);
	virtual void Reset();

	virtual int Length() const;
	virtual TxModel Model() const;

	virtual       TE* Address();
	virtual const TE* Address() const;

	virtual       TE& operator [] (int index);
	virtual const TE& operator [] (int index) const;

	TxScanner1D<TE> Scanner() const
		{
			return TxScanner1D<TE>(static_cast<TE*>(m_Tag.Address), m_Tag.Length);
		}
	TxScanner1D<TE> Scanner(int index, int length) const
		{
			if (index < 0)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(index + length <= m_Tag.Length))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			return TxScanner1D<TE>((TE*)this->Address() + index, length, m_Tag.Model);
		}

public:
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

#endif	// _CXGDIPOLYLINE_H_INCLUDED_
