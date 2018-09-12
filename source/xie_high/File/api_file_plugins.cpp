/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_file_plugins.h"
#include "File/CxFilePluginJpeg.h"
#include "File/CxFilePluginPng.h"
#include "File/CxFilePluginTiff.h"

namespace xie
{
namespace File
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;
CxFilePluginJpeg	g_jpeg;
CxFilePluginPng		g_png;
CxFilePluginTiff	g_tiff;

// ======================================================================
void XIE_API fnPRV_File_Setup()
{
	if (g_setuped) return;
	g_setuped = true;

	xie_core_plugin(&g_jpeg);
	xie_core_plugin(&g_png);
	xie_core_plugin(&g_tiff);
}

// ======================================================================
void XIE_API fnPRV_File_TearDown()
{
}

}
}
