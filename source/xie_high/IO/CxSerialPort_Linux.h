/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSERIALPORT_LINUX_H_INCLUDED_
#define _CXSERIALPORT_LINUX_H_INCLUDED_

#include "xie_high.h"
#include "api_io.h"
#include "IO/CxSerialPort.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

#ifndef XIE_INVALID_HANDLE
#define XIE_INVALID_HANDLE -1	//!< The model only used in this file.
#endif

namespace xie
{
namespace IO
{

class CxSerialPort_Linux
{
private:
	int					m_Handle;

public:
	// ============================================================
	CxSerialPort_Linux()
	{
		m_Handle = XIE_INVALID_HANDLE;
	}

	// ============================================================
	virtual ~CxSerialPort_Linux()
	{
		Close();
	}

	// =================================================================
	bool IsValid() const
	{
		if (m_Handle == XIE_INVALID_HANDLE) return false;
		return true;
	}

	// ============================================================
	void Open(TxCharCPtrA portname, const TxSerialPort& param)
	{
		try
		{
			// ----- open
			{
				int flags = O_RDWR;
				#if !defined(_MSC_VER)
				flags |= O_NOCTTY;
				flags |= O_NDELAY;
				#endif

				m_Handle = ::open(portname, flags);
				if (m_Handle == XIE_INVALID_HANDLE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- parameters
			{
				struct termios tio;

				if (tcgetattr(m_Handle, &tio) == -1)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				tio.c_cflag |=  (CLOCAL | CREAD);
				tio.c_lflag &= ~(ICANON | ECHO | ECHOE | ECHOK | ECHONL | ISIG | IEXTEN );
				tio.c_oflag &= ~(OPOST);
				tio.c_iflag = IGNBRK;

				// ----- BaudRate
				{
					speed_t baudrate = ToBaudRate(param.BaudRate);
					if (baudrate == (speed_t)-1)
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

					if (::cfsetspeed(&tio, baudrate) == -1)
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				}

				// ----- DataBits
				{
					tio.c_cflag &= ~CSIZE;
					unsigned int value = CS8;
					switch(param.DataBits)
					{
					case 5: value = CS5; break;
					case 6: value = CS6; break;
					case 7: value = CS7; break;
					case 8: value = CS8; break;
					}
					tio.c_cflag |= value;
				}

				// ----- StopBits
				{
					switch(param.StopBits)
					{
					default:
					case ExStopBits::One	: tio.c_cflag &= ~CSTOPB; break;
					case ExStopBits::One5: break;
					case ExStopBits::Two	: tio.c_cflag |=  CSTOPB; break;
					}
				}

				// ----- Parity
				{
					switch(param.Parity)
					{
					default:
					case ExParity::None	: tio.c_cflag &= ~(PARENB|PARODD); break;
					case ExParity::Odd	: tio.c_cflag |=  (PARENB|PARODD); break;
					case ExParity::Even	: tio.c_cflag &= ~PARODD; tio.c_cflag |= PARENB; break;
					case ExParity::Mark	: break;
					case ExParity::Space	: break;
					}
				}

				// ----- Handshake
				{
					tio.c_iflag &= ~(IXOFF|IXON);
					tio.c_iflag &= CRTSCTS;
					switch(param.Handshake)
					{
					default:
					case ExHandshake::None	: break;
					case ExHandshake::XonXoff: tio.c_iflag |= (IXOFF|IXON); break;
					case ExHandshake::RtsCts	: tio.c_iflag |= CRTSCTS; break;
					case ExHandshake::DsrDtr	: break;
					}
				}

				if (::tcsetattr(m_Handle, TCSANOW, &tio) == -1)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		catch(const CxException& ex)
		{
			Close();
			throw CxException(ex);
		}
	}

	// ============================================================
	void Close()
	{
		if (m_Handle != XIE_INVALID_HANDLE)
			::close(m_Handle);
		m_Handle = XIE_INVALID_HANDLE;
	}

	// ============================================================
	bool Readable(int timeout)
	{
		int fd = m_Handle;
		if (fd != XIE_INVALID_HANDLE && fd < 0x7FFFFFFF)
		{
			int		nfds = (int)(fd + 1);

			fd_set	fds; FD_ZERO(&fds); FD_SET(fd, &fds);

			struct timeval	tv;
			tv.tv_sec  = timeout / 1000;
			tv.tv_usec = timeout % 1000 * 1000;

			return (0 < ::select(nfds, &fds, NULL, NULL, &tv));
		}
		return false;
	}

	// ============================================================
	int Read(char* buffer, int length, int timeout)
	{
		int fd = m_Handle;
		if (fd != XIE_INVALID_HANDLE && fd < 0x7FFFFFFF)
		{
			int		nfds = (int)(fd + 1);

			fd_set	fds; FD_ZERO(&fds); FD_SET(fd, &fds);

			struct timeval	tv;
			tv.tv_sec  = timeout / 1000;
			tv.tv_usec = timeout % 1000 * 1000;

			if (0 < ::select(nfds, &fds, NULL, NULL, &tv))
			{
				int status = (int)::read(fd, buffer, length);
				return status;
			}
		}
		return 0;
	}

	// ============================================================
	bool Writeable(int timeout)
	{
		int fd = m_Handle;
		if (fd != XIE_INVALID_HANDLE && fd < 0x7FFFFFFF)
		{
			int		nfds = (int)(fd + 1);

			fd_set	fds; FD_ZERO(&fds); FD_SET(fd, &fds);

			struct timeval	tv;
			tv.tv_sec  = timeout / 1000;
			tv.tv_usec = timeout % 1000 * 1000;

			return (0 < ::select(nfds, NULL, &fds, NULL, &tv));
		}
		return false;
	}

	// ============================================================
	int Write(const char* buffer, int length, int timeout)
	{
		int fd = m_Handle;
		if (fd != XIE_INVALID_HANDLE && fd < 0x7FFFFFFF)
		{
			int		nfds = (int)(fd + 1);

			fd_set	fds; FD_ZERO(&fds); FD_SET(fd, &fds);

			struct timeval	tv;
			tv.tv_sec  = timeout / 1000;
			tv.tv_usec = timeout % 1000 * 1000;

			if (0 < ::select(nfds, NULL, &fds, NULL, &tv))
			{
				int status = (int)::write(m_Handle, buffer, length);
				return status;
			}
		}
		return 0;
	}

private:
	// ============================================================
	speed_t ToBaudRate(int value)
	{
		switch(value)
		{
		#if defined(B921600)
			case 921600: return B921600; break;
		#endif
		#if defined(B460800)
			case 460800: return B460800; break;
		#endif
			case 230400: return B230400;break;
			case 115200: return B115200;break;
			case  57600: return B57600;break;
			case  38400: return B38400;break;
			case  19200: return B19200;break;
			case   9600: return B9600;break;
			case   4800: return B4800;break;
			case   2400: return B2400;break;
			case   1800: return B1800;break;
			case   1200: return B1200;break;
			case    600: return B600;break;
			case    300: return B300;break;
			case    200: return B200;break;
			case    150: return B150;break;
			case    134: return B134;break;
			case    110: return B110;break;
			case     75: return B75;break;
		}
		return (speed_t)-1;
	}
};

}
}

#endif
