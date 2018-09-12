/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSSCREENCAPTURE_H_INCLUDED_
#define _CXDSSCREENCAPTURE_H_INCLUDED_

#include "api_ds.h"
#include "Media/CxGrabber.h"
#include "Media/IxMediaControl.h"
#include "Media/DS/CxDSSampleReceiver.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxDisposable.h"
#include "Core/IxParam.h"
#include <vector>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxDSScreenCapture : public CxModule
	, public IxDisposable
	, public IxMediaControl
	, public IxParam
	, public xie::DS::IxDSGraphBuilderProvider
{
private:
	void _Constructor();

public:
	CxDSScreenCapture();
	virtual ~CxDSScreenCapture();

public:
	virtual void Dispose();
	virtual bool IsValid() const;

public:
	virtual void Setup(HxModule hWindow, HxModule hAudio = NULL, HxModule hOutput = NULL);

	CxGrabber CreateGrabber(ExMediaType type) const;

	// IxDSGraphBuilderProvider
	virtual IGraphBuilder* GraphBuilder() const;

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

	virtual int FrameRate() const;
	virtual void FrameRate(int value);

	virtual TxImageSize GetFrameSize() const;
	virtual bool Connected(ExMediaType type) const;

	// Media Event
	virtual bool WaitForCompletion(int timeout) const;

protected:
	int			m_Timeout;
	int			m_FrameRate;

	CxStringA	m_AudioDeviceName;
	CxStringA	m_AudioProductName;
	CxStringW	m_OutputFileName;
	CxStringA	m_OutputDeviceName;
	CxStringA	m_OutputProductName;

	CxDSSampleReceiver		m_VideoGrabberReceiver;
	CxDSSampleReceiver		m_AudioGrabberReceiver;

	IGraphBuilder*			m_Graph;
	ICaptureGraphBuilder2*	m_Builder;
	IBaseFilter*			m_Video;
	IBaseFilter*			m_VideoGrabber;
	IBaseFilter*			m_VideoRenderer;
	IBaseFilter*			m_Audio;
	IBaseFilter*			m_AudioGrabber;
	IBaseFilter*			m_AudioRenderer;
	IFileSinkFilter*		m_FileSink;
	IMediaControl*			m_MediaControl;
	IMediaEvent*			m_MediaEvent;
	IPin*					m_VideoPin;
};

}
}

#pragma pack(pop)

#endif

#endif	// _MCS_VER
