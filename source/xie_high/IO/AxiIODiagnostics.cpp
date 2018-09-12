/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "IO/AxiIODiagnostics.h"
#include "Core/CxException.h"

namespace xie
{

// ==================================================
CxStringA ToString(xie::IO::ExParity value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::IO::ExParity::None:	result = "None";
	case xie::IO::ExParity::Odd:	result = "Odd";
	case xie::IO::ExParity::Even:	result = "Even";
	case xie::IO::ExParity::Mark:	result = "Mark";
	case xie::IO::ExParity::Space:	result = "Space";
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::IO::ExParity::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::IO::ExStopBits value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::IO::ExStopBits::One:	result = "One";
	case xie::IO::ExStopBits::One5:	result = "One5";
	case xie::IO::ExStopBits::Two:	result = "Two";
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::IO::ExStopBits::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::IO::ExHandshake value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::IO::ExHandshake::None:	result = "None"; break;
	case xie::IO::ExHandshake::XonXoff:	result = "XonXoff"; break;
	case xie::IO::ExHandshake::RtsCts:	result = "RtsCts"; break;
	case xie::IO::ExHandshake::DsrDtr:	result = "DsrDtr"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::IO::ExHandshake::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::IO::ExNewLine value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::IO::ExNewLine::None:	result = "None"; break;
	case xie::IO::ExNewLine::LF:	result = "LF"; break;
	case xie::IO::ExNewLine::CR:	result = "CR"; break;
	case xie::IO::ExNewLine::CRLF:	result = "CRLF"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::IO::ExNewLine::%s", result.Address());
	return result;
}

}	// xie
