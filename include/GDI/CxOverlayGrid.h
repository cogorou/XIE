/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXOVERLAYGRID_H_INCLUDED_
#define _CXOVERLAYGRID_H_INCLUDED_

#include "GDI/CxOverlay.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxOverlayGrid : public CxOverlay
{
private:
	void _Constructor();

public:
	CxOverlayGrid();
	CxOverlayGrid(const CxOverlayGrid& src);
	virtual ~CxOverlayGrid();

	CxOverlayGrid& operator = ( const CxOverlayGrid& src );
	bool operator == ( const CxOverlayGrid& src ) const;
	bool operator != ( const CxOverlayGrid& src ) const;

	virtual CxOverlayGrid Clone() const;

public:
	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	template<class TV> TV GetParam(TxCharCPtrA name) const
	{
		TV value;
		GetParam(name, &value, xie::ModelOf(value));
		return value;
	}
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
	template<class TV> void SetParam(TxCharCPtrA name, TV value)
	{
		SetParam(name, &value, xie::ModelOf(value));
	}

public:
	virtual void Render(HxModule hcanvas, ExGdiScalingMode mode) const;
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXOVERLAYGRID_H_INCLUDED_
