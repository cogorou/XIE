/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/AxiMediaDiagnostics.h"
#include "Core/CxException.h"

namespace xie
{

// ==================================================
CxStringA ToString(xie::Media::ExMediaType value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::Media::ExMediaType::None:		result = "None"; break;
	case xie::Media::ExMediaType::Audio:	result = "Audio"; break;
	case xie::Media::ExMediaType::Video:	result = "Video"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::Media::ExMediaType::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::Media::ExMediaDir value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::Media::ExMediaDir::None:		result = "None"; break;
	case xie::Media::ExMediaDir::Input:		result = "Input"; break;
	case xie::Media::ExMediaDir::Output:	result = "Output"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::Media::ExMediaDir::%s", result.Address());
	return result;
}

}	// xie
