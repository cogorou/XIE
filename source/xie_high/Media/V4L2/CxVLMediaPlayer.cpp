/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLMediaPlayer.h"
#include "Media/V4L2/CxVLGrabber.h"
#include "Media/api_media.h"

#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxVLMediaPlayer";

// ============================================================
void CxVLMediaPlayer::_Constructor()
{
	m_Timeout = 1000;
	m_IsPaused = false;
	// for Thread
	m_Thread.Notify	= CxThreadEvent(std::bind(&CxVLMediaPlayer::ThreadProc, this, std::placeholders::_1, std::placeholders::_2));
	m_Thread.Param	= this;
	m_Thread.Delay(10);
	// for V4L2
	m_FD = -1;
	m_VideoStream = false;
}

// ============================================================
CxVLMediaPlayer::CxVLMediaPlayer()
{
	_Constructor();
}

// ============================================================
CxVLMediaPlayer::~CxVLMediaPlayer()
{
	Dispose();
}

// ============================================================
void CxVLMediaPlayer::Dispose()
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
bool CxVLMediaPlayer::IsValid() const
{
	if (m_Thread.IsValid() == false) return false;
	return true;
}

//
// Setup
//

// ============================================================
void CxVLMediaPlayer::Setup(HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	Dispose();

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
CxGrabber CxVLMediaPlayer::CreateGrabber(ExMediaType type) const
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
void CxVLMediaPlayer::Reset()
{
	m_Thread.Reset();
	SetStopPosition(GetDuration());
	SetStartPosition(0);
}

// ============================================================
void CxVLMediaPlayer::Start()
{
	VideoStream(true);
	m_Thread.Start();
}

// ============================================================
void CxVLMediaPlayer::Stop()
{
	m_Thread.Stop();
	VideoStream(false);
}

// ============================================================
void CxVLMediaPlayer::Abort()
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
void CxVLMediaPlayer::Pause()
{
	m_Thread.Stop();
}

// ============================================================
bool CxVLMediaPlayer::Wait(int timeout) const
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
bool CxVLMediaPlayer::IsRunning() const
{
	return (m_VideoStream);
}

// ============================================================
bool CxVLMediaPlayer::IsPaused() const
{
	return (m_VideoStream && !m_Thread.IsRunning());
}

//
// IxParam
//

// ============================================================
void CxVLMediaPlayer::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::GetFrameSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Duration") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::GetDuration)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "CurrentPosition") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::GetCurrentPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "StartPosition") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::GetStartPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "StopPosition") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLMediaPlayer::GetStopPosition)) return;
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
void CxVLMediaPlayer::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLMediaPlayer::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "StartPosition") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLMediaPlayer::SetStartPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "StopPosition") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLMediaPlayer::SetStopPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
int CxVLMediaPlayer::Timeout() const
{
	return m_Timeout;
}

// ============================================================
void CxVLMediaPlayer::Timeout(int value)
{
	m_Timeout = value;
}

// ============================================================
TxImageSize CxVLMediaPlayer::GetFrameSize() const
{
	return m_VideoGrabberReceiver.FrameSize;
}

// ============================================================
bool CxVLMediaPlayer::Connected(ExMediaType type) const
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
void CxVLMediaPlayer::ThreadProc(void* sender, CxThreadArgs* e)
{
	e->Cancellation = true;
}

// ============================================================
void CxVLMediaPlayer::VideoStream(bool value)
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

//
// Media Seeking
//

// ============================================================
long long CxVLMediaPlayer::GetDuration() const
{
	long long value = 0;
	return value;
}

// ============================================================
long long CxVLMediaPlayer::GetCurrentPosition() const
{
	long long value = 0;
	return value;
}

// ============================================================
long long CxVLMediaPlayer::GetStartPosition() const
{
	long long st = 0;
	long long ed = 0;
	return st;
}

// ============================================================
long long CxVLMediaPlayer::GetStopPosition() const
{
	long long st = 0;
	long long ed = 0;
	return ed;
}

// ============================================================
void CxVLMediaPlayer::SetStartPosition(long long value)
{
	long long st = value;
	long long ed = 0;
}

// ============================================================
void CxVLMediaPlayer::SetStopPosition(long long value)
{
	long long st = 0;
	long long ed = value;
}

//
// Media Event
//

// ============================================================
bool CxVLMediaPlayer::WaitForCompletion(int timeout) const
{
	throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

}
}

#endif	// _MCS_VER
