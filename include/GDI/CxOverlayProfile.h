/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXOVERLAYPROFILE_H_INCLUDED_
#define _CXOVERLAYPROFILE_H_INCLUDED_

#include "GDI/CxOverlay.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/TxPointD.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxOverlayProfile : public CxOverlay
{
private:
	void _Constructor();

public:
	CxOverlayProfile();
	CxOverlayProfile(const CxOverlayProfile& src);
	virtual ~CxOverlayProfile();

	CxOverlayProfile& operator = ( const CxOverlayProfile& src );
	bool operator == ( const CxOverlayProfile& src ) const;
	bool operator != ( const CxOverlayProfile& src ) const;

	virtual CxOverlayProfile Clone() const;

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

	static void GetProfileX(CxArray& dst, const CxImage& src, int ch, int k, int x);
	static void GetProfileY(CxArray& dst, const CxImage& src, int ch, int k, int y);
	static double GetValue(const CxImage& src, int ch, int k, int y, int x);

protected:
	CxImage		m_Image;
	TxPointD	m_CursorPosition;
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXOVERLAYPROFILE_H_INCLUDED_
