/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxScreenCapture.h"

#if defined(_MSC_VER)
#include "Media/DS/CxDSScreenCapture.h"
typedef xie::Media::CxDSScreenCapture	TBODY;	//!< The type only used in this file.
#else
#include "Media/V4L2/CxVLScreenCapture.h"
typedef xie::Media::CxVLScreenCapture	TBODY;	//!< The type only used in this file.
#endif

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxScreenCapture";

// ============================================================
void CxScreenCapture::_Constructor()
{
	m_Handle = (HxModule)new TBODY();
}

// ============================================================
CxScreenCapture::CxScreenCapture()
{
	_Constructor();
}

// ============================================================
CxScreenCapture::~CxScreenCapture()
{
	Dispose();
	delete reinterpret_cast<TBODY*>(m_Handle);
}

// ============================================================
IxModule* CxScreenCapture::GetModule() const
{
	return reinterpret_cast<IxModule*>(m_Handle);
}

//
// IxDisposable
//

// ============================================================
void CxScreenCapture::Dispose()
{
	reinterpret_cast<TBODY*>(m_Handle)->Dispose();
}

// ============================================================
bool CxScreenCapture::IsValid() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsValid();
}

//
// Setup
//

// ============================================================
void CxScreenCapture::Setup(HxModule hWindow, HxModule hAudio, HxModule hOutput)
{
#if defined(_MSC_VER)
	reinterpret_cast<TBODY*>(m_Handle)->Setup(hWindow, hAudio, hOutput);
#else
	reinterpret_cast<TBODY*>(m_Handle)->Setup(hWindow, hAudio, hOutput);
#endif
}

// ============================================================
CxGrabber CxScreenCapture::CreateGrabber(ExMediaType type) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->CreateGrabber(type);
}

//
// IxMediaControl
//

// ============================================================
void CxScreenCapture::Reset()
{
	reinterpret_cast<TBODY*>(m_Handle)->Reset();
}

// ============================================================
void CxScreenCapture::Start()
{
	reinterpret_cast<TBODY*>(m_Handle)->Start();
}

// ============================================================
void CxScreenCapture::Stop()
{
	reinterpret_cast<TBODY*>(m_Handle)->Stop();
}

// ============================================================
void CxScreenCapture::Abort()
{
	reinterpret_cast<TBODY*>(m_Handle)->Abort();
}

// ============================================================
void CxScreenCapture::Pause()
{
	reinterpret_cast<TBODY*>(m_Handle)->Pause();
}

// ============================================================
bool CxScreenCapture::Wait(int timeout) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Wait(timeout);
}

// ============================================================
bool CxScreenCapture::IsRunning() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsRunning();
}

// ============================================================
bool CxScreenCapture::IsPaused() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsPaused();
}

//
// IxParam
//

// ============================================================
void CxScreenCapture::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	reinterpret_cast<TBODY*>(m_Handle)->GetParam(name, value, model);
}

// ============================================================
void CxScreenCapture::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	reinterpret_cast<TBODY*>(m_Handle)->SetParam(name, value, model);
}

// ============================================================
int CxScreenCapture::Timeout() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Timeout();
}

// ============================================================
void CxScreenCapture::Timeout(int value)
{
	reinterpret_cast<TBODY*>(m_Handle)->Timeout(value);
}

// ============================================================
int CxScreenCapture::FrameRate() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->FrameRate();
}

// ============================================================
void CxScreenCapture::FrameRate(int value)
{
	reinterpret_cast<TBODY*>(m_Handle)->FrameRate(value);
}

// ============================================================
TxImageSize CxScreenCapture::GetFrameSize() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetFrameSize();
}

}
}
