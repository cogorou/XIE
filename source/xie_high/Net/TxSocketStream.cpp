/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/TxSocketStream.h"

namespace xie
{
namespace Net
{

// =================================================================
TxSocketStream::TxSocketStream()
{
	Socket			= XIE_INVALID_SOCKET;
	LocalEndPoint	= TxIPEndPoint::Default();
	RemoteEndPoint	= TxIPEndPoint::Default();
}

// =================================================================
TxSocketStream::TxSocketStream(HxSocket sock)
{
	Socket			= sock;
	LocalEndPoint	= TxIPEndPoint::Default();
	RemoteEndPoint	= TxIPEndPoint::Default();
}

// =================================================================
TxSocketStream::TxSocketStream(HxSocket sock, TxIPEndPoint localeEP, TxIPEndPoint remoteEP)
{
	Socket			= sock;
	LocalEndPoint	= localeEP;
	RemoteEndPoint	= remoteEP;
}

// =================================================================
bool TxSocketStream::operator == (const TxSocketStream& cmp) const
{
	if (Socket			!= cmp.Socket) return false;
	if (LocalEndPoint	!= cmp.LocalEndPoint) return false;
	if (RemoteEndPoint	!= cmp.RemoteEndPoint) return false;
	return true;
}

// =================================================================
bool TxSocketStream::operator != (const TxSocketStream& cmp) const
{
	return !(operator == (cmp));
}

// =================================================================
bool TxSocketStream::Connected() const
{
	// 
	// seealso:
	// http://msdn.microsoft.com/en-us/library/windows/desktop/ms740141(v=vs.85).aspx
	// http://msdn.microsoft.com/en-us/library/windows/desktop/ms740121(v=vs.85).aspx
	// 

	if (Socket != XIE_INVALID_SOCKET && Socket < 0x7FFFFFFF)
	{
		int		nfds = (int)(Socket + 1);

		fd_set	fdsR; FD_ZERO(&fdsR); FD_SET(Socket, &fdsR);

		struct timeval	tv;
		tv.tv_sec  = 0 / 1000;
		tv.tv_usec = 0 % 1000 * 1000;

		char	buf[1];
		int		buflen = 1;

		bool abnormal = (
			::select(nfds, &fdsR, NULL, NULL, &tv) > 0 &&
			::recv(Socket, buf, buflen, MSG_PEEK) <= 0
			);
		
		return !abnormal;
	}
	return false;
}

// =================================================================
bool TxSocketStream::Readable(int timeout) const
{
	if (Socket != XIE_INVALID_SOCKET && Socket < 0x7FFFFFFF)
	{
		int		nfds = (int)(Socket + 1);

		fd_set	fds; FD_ZERO(&fds); FD_SET(Socket, &fds);

		struct timeval	tv;
		tv.tv_sec  = timeout / 1000;
		tv.tv_usec = timeout % 1000 * 1000;

		return (0 < ::select(nfds, &fds, NULL, NULL, &tv));
	}
	return false;
}

// =================================================================
int TxSocketStream::Read(char* buffer, int length, int timeout) const
{
	if (Readable(timeout) == false) return 0;

	int		flags = 0;

	#if defined(_MSC_VER)
	#else
	flags = MSG_DONTWAIT;
	#endif

	int status = ::recv(Socket, buffer, length, flags);

	return status;
}

// =================================================================
bool TxSocketStream::Writeable(int timeout) const
{
	if (Socket != XIE_INVALID_SOCKET && Socket < 0x7FFFFFFF)
	{
		int		nfds = (int)(Socket + 1);

		fd_set	fds; FD_ZERO(&fds); FD_SET(Socket, &fds);

		struct timeval	tv;
		tv.tv_sec  = timeout / 1000;
		tv.tv_usec = timeout % 1000 * 1000;

		return (0 < ::select(nfds, NULL, &fds, NULL, &tv));
	}
	return false;
}

// =================================================================
int TxSocketStream::Write(const char* buffer, int length, int timeout) const
{
	if (Writeable(timeout) == false) return 0;

	int		flags = 0;

	#if defined(_MSC_VER)
	#else
	flags = MSG_DONTWAIT;
	#endif

	int status = ::send(Socket, buffer, length, flags);

	return status;
}

}
}
