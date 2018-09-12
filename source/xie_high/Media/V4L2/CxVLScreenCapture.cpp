/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLScreenCapture.h"
#include "Media/V4L2/CxVLGrabber.h"
#include "Media/api_media.h"

#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"
#include "Core/CxFinalizer.h"
#include "GDI/CxCanvas.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxVLScreenCapture";

// ============================================================
void CxVLScreenCapture::_Constructor()
{
	m_Timeout = 5000;
	m_FrameRate = 10;
	// for Thread
	m_Thread.Notify	= CxThreadEvent(std::bind(&CxVLScreenCapture::ThreadProc, this, std::placeholders::_1, std::placeholders::_2));
	m_Thread.Param	= this;
	m_Thread.Delay(10);
	// for V4L2
	m_FD = -1;
	m_VideoStream = false;
}

// ============================================================
CxVLScreenCapture::CxVLScreenCapture()
{
	_Constructor();
}

// ============================================================
CxVLScreenCapture::~CxVLScreenCapture()
{
	Dispose();
}

// ============================================================
void CxVLScreenCapture::Dispose()
{
	try
	{
		Stop();
	}
	catch(const CxException&)
	{
	}

	m_Thread.Dispose();

	m_AudioDeviceName.Dispose();
	m_AudioProductName.Dispose();
	m_OutputFileName.Dispose();
	m_OutputDeviceName.Dispose();
	m_OutputProductName.Dispose();
}

// ============================================================
bool CxVLScreenCapture::IsValid() const
{
	if (m_Thread.IsValid() == false) return false;
	return true;
}

//
// Setup
//

// ============================================================
void CxVLScreenCapture::Setup(HxModule hWindow, HxModule hAudio, HxModule hOutput)
{
	Dispose();

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
CxGrabber CxVLScreenCapture::CreatePreparer() const
{
	return CxVLGrabber::From(&m_VideoPreparerReceiver);
}

// ============================================================
CxGrabber CxVLScreenCapture::CreateGrabber(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Video:
		return CxVLGrabber::From(&m_VideoGrabberReceiver);
	case ExMediaType::Audio:
		return CxVLGrabber::From(&m_AudioGrabberReceiver);
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

//
// IxMediaControl
//

// ============================================================
void CxVLScreenCapture::Reset()
{
	m_Thread.Reset();
}

// ============================================================
void CxVLScreenCapture::Start()
{
	VideoStream(true);
	m_Thread.Start();
}

// ============================================================
void CxVLScreenCapture::Stop()
{
	m_Thread.Stop();
	VideoStream(false);
}

// ============================================================
void CxVLScreenCapture::Abort()
{
	m_Thread.Stop();

	try
	{
		VideoStream(false);
	}
	catch(const CxException&)
	{
	}
}

// ============================================================
void CxVLScreenCapture::Pause()
{
	m_Thread.Stop();
}

// ============================================================
bool CxVLScreenCapture::Wait(int timeout) const
{
	xie::CxStopwatch watch;
	watch.Start();
	while(IsRunning())
	{
		watch.Stop();
		if (0 <= timeout && timeout <= watch.Elapsed)
			return false;
		xie::Axi::Sleep(1);
	}
	return true;
}

// ============================================================
bool CxVLScreenCapture::IsRunning() const
{
	return (m_VideoStream);
}

// ============================================================
bool CxVLScreenCapture::IsPaused() const
{
	return (m_VideoStream && !m_Thread.IsRunning());
}

//
// IxParam
//

// ============================================================
void CxVLScreenCapture::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLScreenCapture::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameRate") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLScreenCapture::FrameRate)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLScreenCapture::GetFrameSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Video.Connected") == 0)
	{
		if (model == ModelOf<bool>())
		{
			*static_cast<bool*>(value) = Connected(ExMediaType::Video);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.Connected") == 0)
	{
		if (model == ModelOf<bool>())
		{
			*static_cast<bool*>(value) = Connected(ExMediaType::Audio);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.DeviceName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_AudioDeviceName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_AudioDeviceName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.ProductName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_AudioProductName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_AudioProductName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.FileName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputFileName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputFileName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.DeviceName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputDeviceName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputDeviceName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.ProductName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputProductName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputProductName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxVLScreenCapture::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLScreenCapture::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameRate") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLScreenCapture::FrameRate)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
int CxVLScreenCapture::Timeout() const
{
	return m_Timeout;
}

// ============================================================
void CxVLScreenCapture::Timeout(int value)
{
	m_Timeout = value;
}

// ============================================================
int CxVLScreenCapture::FrameRate() const
{
	return m_FrameRate;
}

// ============================================================
void CxVLScreenCapture::FrameRate(int value)
{
	m_FrameRate = value;
}

// ============================================================
TxImageSize CxVLScreenCapture::GetFrameSize() const
{
	return m_VideoGrabberReceiver.FrameSize;
}

// ============================================================
bool CxVLScreenCapture::Connected(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return false;
	case ExMediaType::Video:
		return IsValid();
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
void CxVLScreenCapture::ThreadProc(void* sender, CxThreadArgs* e)
{
	e->Cancellation = true;
}

// ============================================================
void CxVLScreenCapture::VideoStream(bool value)
{
	if (value)
	{
		int fd = m_FD;
		if (fd == -1)
			throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (m_VideoStream) return;	// ignore

		m_VideoStream = true;
	}
	else
	{
		m_VideoStream = false;
	}
}

}
}

#endif	// _MCS_VER
