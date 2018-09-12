/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxDateTime.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxDateTime::TxDateTime()
{
	Year		= 0;
	Month		= 0;
	Day			= 0;
	Hour		= 0;
	Minute		= 0;
	Second		= 0;
	Millisecond	= 0;
}

// ============================================================
TxDateTime::TxDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
{
	Year		= year;
	Month		= month;
	Day			= day;
	Hour		= hour;
	Minute		= minute;
	Second		= second;
	Millisecond	= millisecond;
}

#if defined(_MSC_VER)
// ============================================================
TxDateTime::TxDateTime(const SYSTEMTIME& src)
{
	Year		= src.wYear;
	Month		= src.wMonth;
	Day			= src.wDay;
	Hour		= src.wHour;
	Minute		= src.wMinute;
	Second		= src.wSecond;	// 0~59
	Millisecond	= src.wMilliseconds;
}
#else
// ============================================================
TxDateTime::TxDateTime(const tm& src, int msec)
{
	Year		= src.tm_year + 1900;
	Month		= src.tm_mon + 1;
	Day			= src.tm_mday;
	Hour		= src.tm_hour;
	Minute		= src.tm_min;
	Second		= src.tm_sec;	// 0~59 or 60
	Millisecond	= msec;
}
#endif

// ============================================================
bool TxDateTime::operator == (const TxDateTime& cmp) const
{
	const TxDateTime& src = *this;
	if (src.Year		!= cmp.Year) return false;
	if (src.Month		!= cmp.Month) return false;
	if (src.Day			!= cmp.Day) return false;
	if (src.Hour		!= cmp.Hour) return false;
	if (src.Minute		!= cmp.Minute) return false;
	if (src.Second		!= cmp.Second) return false;
	if (src.Millisecond	!= cmp.Millisecond) return false;
	return true;
}

// ============================================================
bool TxDateTime::operator != (const TxDateTime& cmp) const
{
	return !(operator == (cmp));
}

#if defined(_MSC_VER)

// ============================================================
TxDateTime& TxDateTime::operator = (const SYSTEMTIME& src)
{
	TxDateTime& dst = *this;
	dst.Year		= src.wYear;
	dst.Month		= src.wMonth;
	dst.Day			= src.wDay;
	dst.Hour		= src.wHour;
	dst.Minute		= src.wMinute;
	dst.Second		= src.wSecond;	// 0~59
	dst.Millisecond	= src.wMilliseconds;
	return dst;
}

// ============================================================
TxDateTime::operator SYSTEMTIME () const
{
	const TxDateTime& src = *this;
	SYSTEMTIME dst;
	dst.wYear			= src.Year;
	dst.wMonth			= src.Month;
	dst.wDay			= src.Day;
	dst.wHour			= src.Hour;
	dst.wMinute			= src.Minute;
	dst.wSecond			= src.Second;	// 0~59
	dst.wMilliseconds	= src.Millisecond;
	dst.wDayOfWeek		= 0;
	return dst;
}

#else

// ============================================================
TxDateTime& TxDateTime::operator = (const tm& src)
{
	TxDateTime& dst = *this;
	dst.Year		= src.tm_year + 1900;
	dst.Month		= src.tm_mon + 1;
	dst.Day			= src.tm_mday;
	dst.Hour		= src.tm_hour;
	dst.Minute		= src.tm_min;
	dst.Second		= src.tm_sec;	// 0~59 or 60
	dst.Millisecond	= 0;
	return dst;
}

// ============================================================
TxDateTime::operator tm () const
{
	const TxDateTime& src = *this;
	tm dst;
	dst.tm_year			= src.Year - 1900;
	dst.tm_mon			= src.Month - 1;
	dst.tm_mday			= src.Day;
	dst.tm_hour			= src.Hour;
	dst.tm_min			= src.Minute;
	dst.tm_sec			= src.Second;	// 0~59
	dst.tm_wday			= 0;
	dst.tm_yday			= 0;
	dst.tm_isdst		= 0;
	return dst;
}

#endif

// ============================================================
TxDateTime TxDateTime::Now(bool ltc)
{
	#if defined(_MSC_VER)
	{
		TxDateTime result;
		SYSTEMTIME	systemtime;
		if (ltc)
			::GetLocalTime(&systemtime);
		else
			::GetSystemTime(&systemtime);
		result = systemtime;
		return result;
	}
	#else
	{
		struct timespec	now;
		if (0 != clock_gettime( CLOCK_REALTIME, &now ))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		struct tm		_tm;
		if (ltc)
			localtime_r(&now.tv_sec, &_tm);
		else
			gmtime_r(&now.tv_sec, &_tm);

		TxDateTime result;
		result = _tm;
		result.Millisecond = now.tv_nsec / 1000 / 1000;

		return result;
	}
	#endif
}

// ======================================================================
TxDateTime TxDateTime::FromBinary(unsigned long long src, bool ltc)
{
	#if defined(_MSC_VER)
	{
		SYSTEMTIME	systemtime;

		if (ltc)
		{
			unsigned long long temp = 0;
			if (FALSE == ::FileTimeToLocalFileTime((LPFILETIME)&src, (LPFILETIME)&temp))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			if (FALSE == ::FileTimeToSystemTime((LPFILETIME)&temp, &systemtime))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
		else
		{
			if (FALSE == ::FileTimeToSystemTime((LPFILETIME)&src, &systemtime))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		TxDateTime result;
		result = systemtime;
		return result;
	}
	#else
	{
		struct timespec*	_ts = (timespec*)&src;
		struct tm			_tm;

		if (ltc)
			localtime_r(&_ts->tv_sec, &_tm);
		else
			gmtime_r(&_ts->tv_sec, &_tm);

		TxDateTime result;
		result = _tm;
		result.Millisecond = _ts->tv_nsec / 1000 / 1000;
		return result;
	}
	#endif
}

// ======================================================================
unsigned long long	TxDateTime::ToBinary(bool ltc) const
{
	#if defined(_MSC_VER)
	{
		SYSTEMTIME	systemtime = (SYSTEMTIME)*this;
		unsigned long long result = 0;
		if (ltc)
		{
			unsigned long long temp = 0;
			if (FALSE == ::SystemTimeToFileTime(&systemtime, (LPFILETIME)&temp))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			if (FALSE == ::LocalFileTimeToFileTime((LPFILETIME)&temp, (LPFILETIME)&result))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
		else
		{
			if (FALSE == ::SystemTimeToFileTime(&systemtime, (LPFILETIME)&result))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
		return result;
	}
	#else
	{
		struct tm	_tm = (struct tm)*this;
		time_t epoch = mktime(&_tm);	// EPOCH Å© LTC

		if (ltc == false)
		{
			time_t now = time(NULL);
			struct tm	_tm1;
			struct tm	_tm2;
			localtime_r(&now, &_tm1);
			gmtime_r(&now, &_tm2);
			time_t LTC = mktime(&_tm1);
			time_t UTC = mktime(&_tm2);

			if (LTC < UTC)
				epoch -= (UTC - LTC);
			else
				epoch += (LTC - UTC);
		}

		unsigned long long result = 0;
		struct timespec* _ts = (timespec*)&result;
		_ts->tv_sec  = epoch;
		_ts->tv_nsec = Millisecond * 1000 * 1000;
		return result;
	}
	#endif
}

}
