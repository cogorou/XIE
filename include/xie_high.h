/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_HIGH_H_INCLUDED_
#define _XIE_HIGH_H_INCLUDED_

// ============================================================
// Header

#include "xie_core.h"
#include "Core/TxModel.h"

#if defined(_MSC_VER)
#else
	#include <X11/Xlib.h>
	#include <X11/Xutil.h>
	#if defined(__cplusplus)
	#ifdef None
	#undef None
	#endif
	#ifdef True
	#undef True
	#endif
	#ifdef False
	#undef False
	#endif
	#ifdef Success
	#undef Success
	#endif
	#endif
	#include <GL/glew.h>
	#include <GL/gl.h>
	#include <GL/glx.h>
	#include <sys/types.h>
	#include <sys/stat.h>
	#include <sys/ioctl.h>
	#include <fcntl.h>
	#include <unistd.h>
	#include <limits.h>
	#include <stdio.h>
	#include <stdlib.h>
#endif

// ////////////////////////////////////////////////////////////
// GDI
//

// ============================================================
// ENUM

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{

enum class ExGdiScalingMode : int
{
	None = 0,
	Center,
	TopLeft,
};

enum class ExGdiAnchorStyle : int
{
	None = 0,
	Arrow,
	Cross,
	Diagcross,
	Diamond,
	Rectangle,
	Triangle,
};

enum class ExGdiBrushStyle : int
{
	None = 0,
	Solid,
	Cross,
	Diagcross,
	Horizontal,
	Vertical,
	Diagonal,
	DiagonalB,
};

enum class ExGdiPenStyle : int
{
	None = 0,
	Solid,
	Dot,
	Dash,
	DashDot,
	DashDotDot,
};

enum class ExGdiTextAlign : int
{
	TopLeft			= 0,
	TopRight		= 2,
	TopCenter		= 6,
	BottomLeft		= 8,
	BottomRight		= 10,
	BottomCenter	= 14,
	BaselineLeft	= 24,
	BaselineRight	= 22,
	BaselineCenter	= 30,
};

enum class ExGdiEventType
{
	None	= 0,
	KeyDown,
	KeyUp,
	MouseDown,
	MouseUp,
	MouseDoubleClick,
	MouseWheel,
	MouseMove,
};

enum class ExMouseButton : int
{
	None	= 0,
	Left,
	Right,
	Middle,
	X1,
	X2,
};

}	// GDI
}	// xie
#endif	// __cplusplus

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
	template<> inline TxModel ModelOf<GDI::ExGdiScalingMode>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExGdiAnchorStyle>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExGdiBrushStyle>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExGdiPenStyle>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExGdiTextAlign>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExGdiEventType>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<GDI::ExMouseButton>() { return TxModel::From(ExType::S32, 1); }
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
// IO
//

// ============================================================
// ENUM

#if defined(__cplusplus)
namespace xie
{
namespace IO
{

enum class ExParity : int
{
	None	= 0,
	Odd		= 1,
	Even	= 2,
	Mark	= 3,
	Space	= 4,
};

enum class ExStopBits : int
{
	One		= 0,
	One5	= 1,
	Two		= 2,
};

enum class ExHandshake : int
{
	None	= 0,
	XonXoff	= 1,
	RtsCts	= 2,
	DsrDtr	= 3,
};

enum class ExNewLine : int
{
	None	= 0,
	LF		= 1,
	CR		= 2,
	CRLF	= 3,
};

}	// IO
}	// xie
#endif	// __cplusplus

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
	template<> inline TxModel ModelOf<IO::ExParity>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<IO::ExStopBits>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<IO::ExHandshake>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<IO::ExNewLine>() { return TxModel::From(ExType::S32, 1); }
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
// Media
//

// ============================================================
// ENUM

#if defined(__cplusplus)
namespace xie
{
namespace Media
{

enum class ExMediaType : int
{
	None	= 0,
	Audio	= 1,
	Video	= 2,
};

enum class ExMediaDir : int
{
	None	= 0,
	Input	= 1,
	Output	= 2,
};

}	// Media
}	// xie
#endif	// __cplusplus

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
	template<> inline TxModel ModelOf<Media::ExMediaType>() { return TxModel::From(ExType::S32, 1); }
	template<> inline TxModel ModelOf<Media::ExMediaDir>() { return TxModel::From(ExType::S32, 1); }
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
// Net
//

#if defined(__cplusplus)
namespace xie
{
namespace Net
{
#endif	// __cplusplus

// ============================================================
// TYPEDEF

#if defined(_MSC_VER)
typedef ULONG_PTR		HxSocket;
#else
typedef int				HxSocket;
#endif

#define XIE_INVALID_SOCKET  (HxSocket)(~0)

// Address families.
#define XIE_AF_INET		2	// internetwork: UDP, TCP, etc.

#if defined(__cplusplus)
}	// Net
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
//
// FUNCTION
//

XIE_EXPORT_FUNCTION void XIE_API xie_high_setup();
XIE_EXPORT_FUNCTION void XIE_API xie_high_teardown();
XIE_EXPORT_FUNCTION int XIE_API xie_high_setup_ex(const char* category);
XIE_EXPORT_FUNCTION int XIE_API xie_high_teardown_ex(const char* category);

// ////////////////////////////////////////////////////////////
//
// INCLUDE
//

#if defined(XIE_EXPLICIT_INCLUDE)
	// You need to explicitly include the headers.
#elif defined(XIE_EXPORTS)
	// You need to explicitly include the headers.
#else

#if defined(__cplusplus)
#include "GDI/AxiGDIDiagnostics.h"
#include "GDI/CxBitmap.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxTexture.h"
#include "GDI/CxGdiCircle.h"
#include "GDI/CxGdiCircleArc.h"
#include "GDI/CxGdiEllipse.h"
#include "GDI/CxGdiEllipseArc.h"
#include "GDI/CxGdiImage.h"
#include "GDI/CxGdiLine.h"
#include "GDI/CxGdiLineSegment.h"
#include "GDI/CxGdiPoint.h"
#include "GDI/CxGdiPolyline.h"
#include "GDI/CxGdiRectangle.h"
#include "GDI/CxGdiString.h"
#include "GDI/CxGdiStringA.h"
#include "GDI/CxGdiStringW.h"
#include "GDI/CxGdiTrapezoid.h"
#include "GDI/CxGdiVoid.h"
#include "GDI/CxOverlay.h"
#include "GDI/CxOverlayGrid.h"
#include "GDI/CxOverlayProfile.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxBitmap.h"
#include "GDI/TxCanvas.h"
#include "GDI/TxGdi2dParam.h"
#include "GDI/TxGdiImage.h"
#include "GDI/TxGdiPolyline.h"
#include "GDI/TxGdiStringA.h"
#include "GDI/TxGdiStringW.h"
#include "GDI/TxHitPosition.h"
#include "IO/AxiIODiagnostics.h"
#include "IO/CxSerialPort.h"
#include "IO/TxSerialPort.h"
#include "Net/CxTcpServer.h"
#include "Net/CxTcpClient.h"
#include "Net/CxUdpClient.h"
#include "Net/TxIPAddress.h"
#include "Net/TxIPEndPoint.h"
#include "Net/TxSocketStream.h"
#include "Net/TxTcpServer.h"
#include "Net/TxTcpClient.h"
#include "Net/TxUdpClient.h"
#include "Media/AxiMediaDiagnostics.h"
#include "Media/CxCamera.h"
#include "Media/CxControlProperty.h"
#include "Media/CxDeviceList.h"
#include "Media/CxDeviceListItem.h"
#include "Media/CxDeviceParam.h"
#include "Media/CxGrabber.h"
#include "Media/CxGrabberArgs.h"
#include "Media/CxMediaPlayer.h"
#include "Media/CxScreenCapture.h"
#include "Media/CxScreenList.h"
#include "Media/CxScreenListItem.h"
#include "Media/IxMediaControl.h"
#include "Media/TxGrabberArgs.h"
#include "Media/TxDeviceListItem.h"
#include "Media/TxDeviceParam.h"
#include "Media/TxScreenListItem.h"
#endif

#if defined(__cplusplus) && !defined(_MSC_VER)
#endif

#endif	// INCLUDE

// ////////////////////////////////////////////////////////////
//
// FILENAME
//

#define XIE_HIGH_PREFIX "xie_high_" XIE_ARCH "_" XIE_VER_STR

#if defined(_MSC_VER)
	#if defined(XIE_LINK) || !defined(XIE_LINK_DISABLED)
		#pragma comment (lib,XIE_HIGH_PREFIX)
	#endif
#endif

#endif
