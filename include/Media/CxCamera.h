/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXCAMERA_H_INCLUDED_
#define _CXCAMERA_H_INCLUDED_

#include "xie_high.h"
#include "Media/CxGrabber.h"
#include "Media/CxControlProperty.h"
#include "Media/IxMediaControl.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxDisposable.h"
#include "Core/IxInternalModule.h"
#include "Core/IxParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxCamera : public CxModule
	, public IxInternalModule
	, public IxDisposable
	, public IxMediaControl
	, public IxParam
{
private:
	void _Constructor();
	HxModule m_Handle;

public:
	CxCamera();
	virtual ~CxCamera();
	virtual IxModule* GetModule() const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

public:
	virtual void Setup(HxModule hVideo, HxModule hAudio = NULL, HxModule hOutput = NULL);

	virtual CxGrabber CreateGrabber(ExMediaType type = ExMediaType::Video) const;
	virtual CxControlProperty ControlProperty(TxCharCPtrA name) const;

#if defined(_MSC_VER)
	virtual void OpenPropertyDialog(HWND hWnd, ExMediaType type = ExMediaType::Video, int mode = 0, TxCharCPtrA caption = NULL);
#else
	virtual void OpenPropertyDialog(Window window, ExMediaType type = ExMediaType::Video, int mode = 0, TxCharCPtrA caption = NULL);
#endif

	// IxMediaControl
	virtual void Reset();
	virtual void Start();
	virtual void Stop();
	virtual void Abort();
	virtual void Pause();
	virtual bool Wait(int timeout) const;
	virtual bool IsRunning() const;
	virtual bool IsPaused() const;

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
	virtual int Timeout() const;
	virtual void Timeout(int value);

	virtual TxImageSize GetFrameSize() const;
};

}
}

#pragma pack(pop)

#endif
