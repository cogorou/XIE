/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSCONTROLPROPERTYWRAPPER_H_INCLUDED_
#define _CXDSCONTROLPROPERTYWRAPPER_H_INCLUDED_

#include "api_ds.h"
#include "Core/CxModule.h"
#include "Core/CxString.h"
#include "Core/IxDisposable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

// ============================================================
class CxDSControlPropertyWrapper
{
public:
	CxDSControlPropertyWrapper();
	virtual ~CxDSControlPropertyWrapper();

	CxDSControlPropertyWrapper& operator = ( const CxDSControlPropertyWrapper& src );
	bool operator == ( const CxDSControlPropertyWrapper& src ) const;
	bool operator != ( const CxDSControlPropertyWrapper& src ) const;

	virtual void Dispose();
	virtual bool IsValid() const;

	virtual HxModule Controller() const;
	virtual void Controller(HxModule value);

	virtual TxCharCPtrA Name() const;
	virtual void Name(TxCharCPtrA value);

	virtual bool IsSupported() const;
	virtual HRESULT GetRange(long* minval, long* maxval, long* step, long* defval, long* flags) const;
	virtual HRESULT Get(long* value, long* flags) const;
	virtual HRESULT Set(long value, long flags);

protected:
	virtual IGraphBuilder* GraphBuilder() const;

	int ToCameraControlProperty(TxCharCPtrA name) const;
	int ToVideoProcAmpProperty(TxCharCPtrA name) const;

protected:
	HxModule	m_Controller;
	CxString	m_Name;

	IAMCameraControl*	CameraControl;
	IAMVideoProcAmp*	VideoProcAmp;

	int			CameraControlID;
	int			VideoProcAmpID;
};

}
}

#pragma pack(pop)

#endif

#endif	// _MCS_VER