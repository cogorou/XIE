/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxThreadArgs.h"

#include "api_core.h"
#include "Core/Axi.h"

namespace xie
{

static const char* g_ClassName = "CxThreadArgs";

// =================================================================
void CxThreadArgs::_Constructor()
{
	// set by sender
	Param = NULL;
	Index = -1;

	// response from user to sender
	Cancellation = false;
}

// =================================================================
CxThreadArgs::CxThreadArgs()
{
	_Constructor();
}

// =================================================================
CxThreadArgs::CxThreadArgs(void* param, int index)
{
	_Constructor();
	Param = param;
	Index = index;
}

// =================================================================
CxThreadArgs::CxThreadArgs(const CxThreadArgs& src)
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxThreadArgs::~CxThreadArgs()
{
}

// =================================================================
CxThreadArgs& CxThreadArgs::operator = ( const CxThreadArgs& src )
{
	if (this == &src) return *this;

	CxThreadArgs& dst = *this;

	// set by sender
	dst.Param	= src.Param;
	dst.Index	= src.Index;

	// response from user to sender
	dst.Cancellation = src.Cancellation;

	return *this;
}

// =================================================================
bool CxThreadArgs::operator == ( const CxThreadArgs& src ) const
{
	if (this == &src) return true;

	const CxThreadArgs& dst = *this;

	// set by sender
	if (dst.Param	!= src.Param) return false;
	if (dst.Index	!= src.Index) return false;

	// response from user to sender
	if (dst.Cancellation != src.Cancellation) return false;

	return true;
}

// =================================================================
bool CxThreadArgs::operator != ( const CxThreadArgs& src ) const
{
	return !(operator == (src));
}

}

