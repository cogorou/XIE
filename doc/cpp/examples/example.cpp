/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include <SDKDDKVer.h>
#include <stdio.h>
#include <tchar.h>

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"		// Setup/TearDown
#include "Core/AxiDiagnostics.h"
#include "GDI/AxiGDIDiagnostics.h"
#include "Media/AxiMediaDiagnostics.h"
#include "IO/AxiIODiagnostics.h"

#include "example.h"
#include "Core/Base/Axi.h"
#include "Core/Base/AxiModule.h"
#include "Core/Base/CxException.h"
#include "Core/Base/CxFinalizer.h"
#include "Core/Base/CxMutex.h"
#include "Core/Base/CxStopwatch.h"
#include "Core/Base/CxThread.h"
#include "Core/CxArray/CxArray.h"
#include "Core/CxImage/CxImage.h"
#include "Core/CxMatrix/CxMatrix.h"
#include "Core/CxExif/CxExif.h"
#include "Core/Macros/Macros.h"
#include "Core/Structures/Structures.h"

#include "Effectors/CxBinarize1.h"
#include "Effectors/CxBinarize2.h"
#include "Effectors/CxGammaConverter.h"
#include "Effectors/CxHsvConverter.h"
#include "Effectors/CxHsvToRgb.h"
#include "Effectors/CxIntegral.h"
#include "Effectors/CxMonochrome.h"
#include "Effectors/CxPartColor.h"
#include "Effectors/CxRgbConverter.h"
#include "Effectors/CxRgbToGray.h"
#include "Effectors/CxRgbToHsv.h"
#include "Effectors/CxProjection.h"

#include "GDI/CxCanvas/CxCanvas.h"

#include "IO/CxSerialPort/CxSerialPort.h"

#include "Net/CxTcpServer/CxTcpServer.h"
#include "Net/CxTcpClient/CxTcpClient.h"
#include "Net/CxUdpClient/CxUdpClient.h"

#include "Media/CxCamera/CxCamera.h"
#include "Media/CxControlProperty/CxControlProperty.h"
#include "Media/CxDeviceList/CxDeviceList.h"
#include "Media/CxDeviceParam/CxDeviceParam.h"
#include "Media/CxGrabber/CxGrabber.h"
#include "Media/CxMediaPlayer/CxMediaPlayer.h"
#include "Media/CxScreenCapture/CxScreenCapture.h"

// ============================================================
int main(int argc, char* argv[])
{
	xie::Axi::Setup();

	User::MakeDir( "Results" );

	try
	{
#if 1
		// --------------------------------------------------
		User::CxProjection();
#else
		// --------------------------------------------------
		User::Axi();
		User::AxiModule();
		User::CxException();
		User::CxFinalizer();
		User::CxMutex();
		User::CxThread();
		User::CxStopwatch();
		User::CxArray();
		User::CxImage();
		User::CxMatrix();
		User::CxExif();
		User::Core_Macros();
		User::Core_Structures();

		// --------------------------------------------------
		User::CxBinarize1();
		User::CxBinarize2();
		User::CxGammaConverter();
		User::CxHsvConverter();
		User::CxHsvToRgb();
		User::CxIntegral();
		User::CxMonochrome();
		User::CxPartColor();
		User::CxRgbConverter();
		User::CxRgbToGray();
		User::CxRgbToHsv();
		User::CxProjection();

		// --------------------------------------------------
		User::CxCanvas();

		// --------------------------------------------------
		User::CxSerialPort();

		// --------------------------------------------------
		User::CxTcpServer();
		User::CxTcpClient();
		User::CxUdpClient();

		// --------------------------------------------------
		User::CxCamera();
		User::CxControlProperty();
		User::CxDeviceList();
		User::CxDeviceParam();
		User::CxGrabber();
		User::CxMediaPlayer();
		User::CxScreenCapture();
#endif
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%s(%d) Function=%s\n"
			, ex.File()
			, ex.Line()
			, xie::ToString(ex.Code()).Address()
			, ex.Code()
			, ex.Function()
			);
	}

	xie::Axi::TearDown();

	return 0;
}

namespace User
{

// ============================================================
/*!
	@brief	指定されたディレクトリを作成します。
*/
void MakeDir(const char* dir)
{
	#if defined(_MSC_VER)
	::CreateDirectoryA( dir, NULL );
	#else
	mkdir(dir, 0775);
	#endif
}

}
