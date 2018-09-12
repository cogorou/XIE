/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"

#include "Core/Axi.h"
#include "Core/CxException.h"

#include "File/api_file_plugins.h"
#include "GDI/api_gdi.h"
#include "IO/api_io.h"
#include "Net/api_net.h"
#include "Media/api_media.h"

#if defined(_MSC_VER)
	#pragma comment (lib,XIE_CORE_PREFIX)
	#pragma comment( lib,"msimg32.lib" )
//	#pragma comment (lib,"glew32.lib")		// require glew
	#pragma comment (lib,"opengl32.lib")
	#pragma comment (lib,"Ws2_32.lib")
	#pragma comment (lib,"strmiids.lib")
	#pragma comment (lib,"winmm.lib")
#endif

namespace xie
{

// ============================================================
// FUNCTION

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API xie_high_setup()
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	xie::File::fnPRV_File_Setup();
	xie::GDI::fnPRV_GDI_Setup();
	xie::IO::fnPRV_IO_Setup();
	xie::Net::fnPRV_Net_Setup();
	xie::Media::fnPRV_Media_Setup();
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API xie_high_teardown()
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	xie::File::fnPRV_File_TearDown();
	xie::GDI::fnPRV_GDI_TearDown();
	xie::IO::fnPRV_IO_TearDown();
	xie::Net::fnPRV_Net_TearDown();
	xie::Media::fnPRV_Media_TearDown();
}

// ============================================================
XIE_EXPORT_FUNCTION int XIE_API xie_high_setup_ex(const char* category)
{
	if (category == NULL)
	{
		fnXIE_Core_TraceOut(1, "%s(NULL)\n", __FUNCTION__);
		return (int)ExStatus::NotFound;
	}
	if (strcmp(category, "File") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::File::fnPRV_File_Setup();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "GDI") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::GDI::fnPRV_GDI_Setup();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "IO") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::IO::fnPRV_IO_Setup();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "Net") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::Net::fnPRV_Net_Setup();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "Media") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::Media::fnPRV_Media_Setup();
		return (int)ExStatus::Success;
	}

	fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
	return (int)ExStatus::NotFound;
}

// ============================================================
XIE_EXPORT_FUNCTION int XIE_API xie_high_teardown_ex(const char* category)
{
	if (category == NULL)
	{
		fnXIE_Core_TraceOut(1, "%s(NULL)\n", __FUNCTION__);
		return (int)ExStatus::NotFound;
	}
	if (strcmp(category, "File") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::File::fnPRV_File_TearDown();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "GDI") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::GDI::fnPRV_GDI_TearDown();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "IO") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::IO::fnPRV_IO_TearDown();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "Net") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::Net::fnPRV_Net_TearDown();
		return (int)ExStatus::Success;
	}
	if (strcmp(category, "Media") == 0)
	{
		fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
		xie::Media::fnPRV_Media_TearDown();
		return (int)ExStatus::Success;
	}

	fnXIE_Core_TraceOut(1, "%s(\"%s\")\n", __FUNCTION__, category);
	return (int)ExStatus::NotFound;
}

}
