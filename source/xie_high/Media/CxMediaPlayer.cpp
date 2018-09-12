/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxMediaPlayer.h"

#if defined(_MSC_VER)
#include "Media/DS/CxDSMediaPlayer.h"
typedef xie::Media::CxDSMediaPlayer	TBODY;	//!< The type only used in this file.
#else
#include "Media/V4L2/CxVLMediaPlayer.h"
typedef xie::Media::CxVLMediaPlayer	TBODY;	//!< The type only used in this file.
#endif

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxMediaPlayer";

// ============================================================
void CxMediaPlayer::_Constructor()
{
	m_Handle = (HxModule)new TBODY();
}

// ============================================================
CxMediaPlayer::CxMediaPlayer()
{
	_Constructor();
}

// ============================================================
CxMediaPlayer::~CxMediaPlayer()
{
	Dispose();
	delete reinterpret_cast<TBODY*>(m_Handle);
}

// ============================================================
IxModule* CxMediaPlayer::GetModule() const
{
	return reinterpret_cast<IxModule*>(m_Handle);
}

//
// IxDisposable
//

// ============================================================
void CxMediaPlayer::Dispose()
{
	reinterpret_cast<TBODY*>(m_Handle)->Dispose();
}

// ============================================================
bool CxMediaPlayer::IsValid() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsValid();
}

//
// Setup
//

// ============================================================
void CxMediaPlayer::Setup(HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	reinterpret_cast<TBODY*>(m_Handle)->Setup(hVideo, hAudio, hOutput);
}

// ============================================================
CxGrabber CxMediaPlayer::CreateGrabber(ExMediaType type) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->CreateGrabber(type);
}

//
// IxMediaControl
//

// ============================================================
void CxMediaPlayer::Reset()
{
	reinterpret_cast<TBODY*>(m_Handle)->Reset();
}

// ============================================================
void CxMediaPlayer::Start()
{
	reinterpret_cast<TBODY*>(m_Handle)->Start();
}

// ============================================================
void CxMediaPlayer::Stop()
{
	reinterpret_cast<TBODY*>(m_Handle)->Stop();
}

// ============================================================
void CxMediaPlayer::Abort()
{
	reinterpret_cast<TBODY*>(m_Handle)->Abort();
}

// ============================================================
void CxMediaPlayer::Pause()
{
	reinterpret_cast<TBODY*>(m_Handle)->Pause();
}

// ============================================================
bool CxMediaPlayer::Wait(int timeout) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Wait(timeout);
}

// ============================================================
bool CxMediaPlayer::IsRunning() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsRunning();
}

// ============================================================
bool CxMediaPlayer::IsPaused() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsPaused();
}

//
// IxParam
//

// ============================================================
void CxMediaPlayer::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	reinterpret_cast<TBODY*>(m_Handle)->GetParam(name, value, model);
}

// ============================================================
void CxMediaPlayer::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	reinterpret_cast<TBODY*>(m_Handle)->SetParam(name, value, model);
}

// ============================================================
int CxMediaPlayer::Timeout() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Timeout();
}

// ============================================================
void CxMediaPlayer::Timeout(int value)
{
	reinterpret_cast<TBODY*>(m_Handle)->Timeout(value);
}

// ============================================================
TxImageSize CxMediaPlayer::GetFrameSize() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetFrameSize();
}

//
// Media Seeking
//

// ============================================================
long long CxMediaPlayer::GetDuration() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetDuration();
}

// ============================================================
long long CxMediaPlayer::GetCurrentPosition() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetCurrentPosition();
}

// ============================================================
long long CxMediaPlayer::GetStartPosition() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetStartPosition();
}

// ============================================================
long long CxMediaPlayer::GetStopPosition() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetStopPosition();
}

// ============================================================
void CxMediaPlayer::SetStartPosition(long long value)
{
	return reinterpret_cast<TBODY*>(m_Handle)->SetStartPosition(value);
}

// ============================================================
void CxMediaPlayer::SetStopPosition(long long value)
{
	return reinterpret_cast<TBODY*>(m_Handle)->SetStopPosition(value);
}

// ============================================================
bool CxMediaPlayer::WaitForCompletion(int timeout) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->WaitForCompletion(timeout);
}

}
}
