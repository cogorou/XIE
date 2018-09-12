/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxStopwatch.h"
#include "Core/Axi.h"

#if !defined(_MSC_VER)
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <time.h>
#include <sys/time.h>
#endif

#include <math.h>

#pragma warning (disable:4100)	// ˆø”‚ÍŠÖ”‚Ì–{‘Ì•”‚Å 1 “x‚àŽQÆ‚³‚ê‚Ü‚¹‚ñ.

namespace xie
{

static const char* g_ClassName = "CxStopwatch";

// ===================================================================
CxStopwatch::CxStopwatch()
{
#if defined(_MSC_VER)
	Time.QuadPart = 0;
	Freq.QuadPart = 0;
	::QueryPerformanceFrequency( &Freq );
#else
	Time.tv_sec = 0;
	Time.tv_nsec = 0;
#endif

	Reset();
	Start();
}

// ===================================================================
CxStopwatch::CxStopwatch(const CxStopwatch& src)
{
	operator = (src);
}

// ============================================================
CxStopwatch::~CxStopwatch()
{
}

// ===================================================================
CxStopwatch& CxStopwatch::operator = ( const CxStopwatch& src )
{
	if (this == &src) return *this;

#if defined(_MSC_VER)
	Time	= src.Time;
	Freq	= src.Freq;
#else
	Time	= src.Time;
#endif
	Lap	= src.Lap;
	Elapsed	= src.Elapsed;

	return *this;
}

// ===================================================================
bool CxStopwatch::operator == ( const CxStopwatch& src ) const
{
#if defined(_MSC_VER)
	if( Time.QuadPart	!= src.Time.QuadPart ) return false;
	if( Freq.QuadPart	!= src.Freq.QuadPart ) return false;
#else
	if( Time.tv_sec		!= src.Time.tv_sec ) return false;
	if( Time.tv_nsec	!= src.Time.tv_nsec ) return false;
#endif
	if( Lap	!= src.Lap ) return false;
	if( Elapsed	!= src.Elapsed ) return false;
	return true;
}

// ===================================================================
bool CxStopwatch::operator != ( const CxStopwatch& src ) const
{
	return !(CxStopwatch::operator == (src));
}

// ===================================================================
void CxStopwatch::Reset()
{
	Lap = 0;
	Elapsed = 0;
}

// ===================================================================
void CxStopwatch::Start()
{
#if defined(_MSC_VER)
	::QueryPerformanceCounter( &Time );
#else
	clock_gettime( CLOCK_MONOTONIC, &Time );
#endif
}

// ===================================================================
void CxStopwatch::Stop()
{
#if defined(_MSC_VER)
	if( Freq.QuadPart != 0 )
	{
		LARGE_INTEGER	now = { 0 };
		if (::QueryPerformanceCounter( &now ))
		{
			double diff;
			if( now.QuadPart > Time.QuadPart )
				diff = (double)(now.QuadPart - Time.QuadPart);
			else
			{
				__int64 temp = Time.QuadPart - now.QuadPart;
				diff = (double)((~temp) + 1);
			}
			double	second	= diff / Freq.QuadPart;
			Lap = (second * 1000);
			Elapsed += Lap;
		}
		Time = now;
	}
#else
	{
		struct timespec now;
		clock_gettime( CLOCK_MONOTONIC, &now );

		int sec  = now.tv_sec  - Time.tv_sec;
		int nsec = now.tv_nsec - Time.tv_nsec;
		Lap = (sec * 1000 + nsec * 0.000001);
		Elapsed += Lap;
		Time = now;
	}
#endif
}

}	// xie
