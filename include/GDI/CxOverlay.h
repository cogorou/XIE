/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXOVERLAY_H_INCLUDED_
#define _CXOVERLAY_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxCanvas.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxOverlay : public CxModule
	, public IxGdi2dRendering
	, public IxEquatable
	, public IxParam
{
private:
	void _Constructor();

public:
	CxOverlay();
	CxOverlay(const CxOverlay& src);
	virtual ~CxOverlay();

	CxOverlay& operator = ( const CxOverlay& src );
	bool operator == ( const CxOverlay& src ) const;
	bool operator != ( const CxOverlay& src ) const;

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
	virtual void Render(HxModule hcanvas, ExGdiScalingMode mode) const = 0;

	virtual bool Visible() const;
	virtual void Visible( bool value );

protected:
	bool	m_Visible;
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXOVERLAY_H_INCLUDED_
