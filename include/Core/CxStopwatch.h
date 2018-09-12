/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSTOPWATCH_H_INCLUDED_
#define _CXSTOPWATCH_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxStopwatch : public CxModule
{
public:
	CxStopwatch();
	CxStopwatch( const CxStopwatch& src );
	virtual ~CxStopwatch();

	CxStopwatch& operator = ( const CxStopwatch& src );
	bool operator == ( const CxStopwatch& src ) const;
	bool operator != ( const CxStopwatch& src ) const;

public:
	void Reset();
	void Start();
	void Stop();

	double Lap;
	double Elapsed;

private:
#if defined(_MSC_VER)
	LARGE_INTEGER    Time;			// time
	LARGE_INTEGER    Freq;			// frequency
#else
	struct timespec	Time;
#endif
};

}

#pragma pack(pop)

#endif
