/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTHREADARGS_H_INCLUDED_
#define _CXTHREADARGS_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxThreadArgs : public CxModule
{
private:
	void _Constructor();

public:
	CxThreadArgs();
	CxThreadArgs(void* param, int index);
	CxThreadArgs(const CxThreadArgs& src);
	virtual ~CxThreadArgs();

	CxThreadArgs& operator = ( const CxThreadArgs& src );
	bool operator == ( const CxThreadArgs& src ) const;
	bool operator != ( const CxThreadArgs& src ) const;

	// set by sender
	void*			Param;	// user param
	int				Index;

	// response from user to sender
	bool			Cancellation;
};

}

#pragma pack(pop)

#endif
