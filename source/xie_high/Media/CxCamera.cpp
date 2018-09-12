/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxCamera.h"

#if defined(_MSC_VER)
#include "Media/DS/CxDSCamera.h"
typedef xie::Media::CxDSCamera	TBODY;	//!< The type only used in this file.
#else
#include "Media/V4L2/CxVLCamera.h"
typedef xie::Media::CxVLCamera	TBODY;	//!< The type only used in this file.
#endif

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxCamera";

// ============================================================
void CxCamera::_Constructor()
{
	m_Handle = (HxModule)new TBODY();
}

// ============================================================
CxCamera::CxCamera()
{
	_Constructor();
}

// ============================================================
CxCamera::~CxCamera()
{
	Dispose();
	delete reinterpret_cast<TBODY*>(m_Handle);
}

// ============================================================
IxModule* CxCamera::GetModule() const
{
	return reinterpret_cast<IxModule*>(m_Handle);
}

//
// IxDisposable
//

// ============================================================
void CxCamera::Dispose()
{
	reinterpret_cast<TBODY*>(m_Handle)->Dispose();
}

// ============================================================
bool CxCamera::IsValid() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsValid();
}

//
// Setup
//

// ============================================================
void CxCamera::Setup(HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	reinterpret_cast<TBODY*>(m_Handle)->Setup(hVideo, hAudio, hOutput);
}

// ============================================================
CxGrabber CxCamera::CreateGrabber(ExMediaType type) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->CreateGrabber(type);
}

// ============================================================
CxControlProperty CxCamera::ControlProperty(TxCharCPtrA name) const
{
	return CxControlProperty(*this, name);
}

#if defined(_MSC_VER)
// ============================================================
void CxCamera::OpenPropertyDialog(HWND hWnd, ExMediaType type, int mode, TxCharCPtrA caption)
{
	reinterpret_cast<TBODY*>(m_Handle)->OpenPropertyDialog(hWnd, type, mode, caption);
}
#else
// ============================================================
void CxCamera::OpenPropertyDialog(Window window, ExMediaType type, int mode, TxCharCPtrA caption)
{
	reinterpret_cast<TBODY*>(m_Handle)->OpenPropertyDialog(window, type, mode, caption);
}
#endif

//
// IxMediaControl
//

// ============================================================
void CxCamera::Reset()
{
	reinterpret_cast<TBODY*>(m_Handle)->Reset();
}

// ============================================================
void CxCamera::Start()
{
	reinterpret_cast<TBODY*>(m_Handle)->Start();
}

// ============================================================
void CxCamera::Stop()
{
	reinterpret_cast<TBODY*>(m_Handle)->Stop();
}

// ============================================================
void CxCamera::Abort()
{
	reinterpret_cast<TBODY*>(m_Handle)->Abort();
}

// ============================================================
void CxCamera::Pause()
{
	reinterpret_cast<TBODY*>(m_Handle)->Pause();
}

// ============================================================
bool CxCamera::Wait(int timeout) const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Wait(timeout);
}

// ============================================================
bool CxCamera::IsRunning() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsRunning();
}

// ============================================================
bool CxCamera::IsPaused() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsPaused();
}

//
// IxParam
//

// ============================================================
void CxCamera::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	reinterpret_cast<TBODY*>(m_Handle)->GetParam(name, value, model);
}

// ============================================================
void CxCamera::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	reinterpret_cast<TBODY*>(m_Handle)->SetParam(name, value, model);
}

// ============================================================
int CxCamera::Timeout() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Timeout();
}

// ============================================================
void CxCamera::Timeout(int value)
{
	reinterpret_cast<TBODY*>(m_Handle)->Timeout(value);
}

// ============================================================
TxImageSize CxCamera::GetFrameSize() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetFrameSize();
}

}
}
