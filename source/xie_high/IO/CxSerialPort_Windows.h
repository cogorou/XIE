/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSERIALPORT_WINDOWS_H_INCLUDED_
#define _CXSERIALPORT_WINDOWS_H_INCLUDED_

#include "xie_high.h"
#include "api_io.h"
#include "IO/CxSerialPort.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

#ifndef XIE_INVALID_HANDLE
#define XIE_INVALID_HANDLE INVALID_HANDLE_VALUE	//!< The model only used in this file.
#endif

namespace xie
{
namespace IO
{

class CxSerialPort_Windows
{
private:
	HANDLE				m_Handle;
	HANDLE				m_hEvent;
	OVERLAPPED			m_Overlapped;

public:
	// ============================================================
	CxSerialPort_Windows()
	{
		m_Handle = XIE_INVALID_HANDLE;
		m_hEvent = XIE_INVALID_HANDLE;
	}

	// ============================================================
	virtual ~CxSerialPort_Windows()
	{
		Close();
	}

	// =================================================================
	bool IsValid() const
	{
		if (m_Handle == XIE_INVALID_HANDLE) return false;
		if (m_hEvent == XIE_INVALID_HANDLE) return false;
		return true;
	}

	// ============================================================
	void Open(TxCharCPtrA portname, const TxSerialPort& param)
	{
		try
		{
			// ----- create
			{
				CxString filename = CxString::Format("\\\\.\\%s", portname);

				m_Handle = ::CreateFileA(
							filename.Address(),
							FILE_SHARE_READ | FILE_SHARE_WRITE,
							//GENERIC_READ|GENERIC_WRITE,
							0,
							NULL,
							OPEN_EXISTING,
							FILE_FLAG_OVERLAPPED,
							NULL
						);
						
				if (m_Handle == XIE_INVALID_HANDLE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- parameters
			{
				DCB dcb;

				memset(&dcb, 0, sizeof(DCB));

				dcb.DCBlength			= sizeof(DCB);
				dcb.BaudRate			= param.BaudRate;
				dcb.ByteSize			= param.DataBits;

				switch(param.StopBits)
				{
				default:
				case ExStopBits::One:	dcb.StopBits = ONESTOPBIT;		break;
				case ExStopBits::One5:	dcb.StopBits = ONE5STOPBITS;	break;
				case ExStopBits::Two:	dcb.StopBits = TWOSTOPBITS;		break;
				}

				switch(param.Parity)
				{
				default:
				case ExParity::None:		dcb.Parity = NOPARITY;		dcb.fParity = FALSE; break;
				case ExParity::Odd:		dcb.Parity = ODDPARITY;		dcb.fParity = TRUE; break;
				case ExParity::Even:		dcb.Parity = EVENPARITY;	dcb.fParity = TRUE; break;
				case ExParity::Mark:		dcb.Parity = MARKPARITY;	dcb.fParity = TRUE; break;
				case ExParity::Space:	dcb.Parity = SPACEPARITY;	dcb.fParity = TRUE; break;
				}

				dcb.ErrorChar			= (param.Parity == ExParity::None) ? '\0' : '?';

				dcb.fOutX				= FALSE;
				dcb.fInX				= FALSE;
				dcb.fOutxCtsFlow		= FALSE;
				dcb.fRtsControl			= RTS_CONTROL_DISABLE;
				dcb.fOutxDsrFlow		= FALSE;
				dcb.fDtrControl			= DTR_CONTROL_DISABLE;
				dcb.fDsrSensitivity		= FALSE;

				switch (param.Handshake)
				{
				default:
				case ExHandshake::None:
					break;
				case ExHandshake::XonXoff:
					dcb.fOutX			= TRUE;
					dcb.fInX			= TRUE;
					break;
				case ExHandshake::RtsCts:
					dcb.fOutxCtsFlow	= TRUE;
					dcb.fRtsControl		= RTS_CONTROL_HANDSHAKE;
					break;
				case ExHandshake::DsrDtr:
					dcb.fOutxDsrFlow	= TRUE;
					dcb.fDtrControl		= DTR_CONTROL_HANDSHAKE;
					break;
				}

				dcb.fBinary				= TRUE;		// always TRUE
				dcb.fNull				= FALSE;
				dcb.EofChar				= 0x1A;

				dcb.XonChar				= 0x11;
				dcb.XoffChar			= 0x13;
				dcb.XonLim				= 8;		// RXQUEUE / 4
				dcb.XoffLim				= 8;		// RXQUEUE / 4

				dcb.fErrorChar			= FALSE;	// parityReplace
				dcb.fAbortOnError		= FALSE;
				dcb.fTXContinueOnXoff	= FALSE;

				// do not use.
				dcb.fDummy2				= 0;
				dcb.wReserved			= 0;
				dcb.wReserved1			= 0;

				BOOL status = ::SetCommState(m_Handle, &dcb);
				if (status == FALSE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- timeout
			{
				COMMTIMEOUTS timeouts;
				timeouts.ReadIntervalTimeout			= MAXDWORD;	// 
				timeouts.ReadTotalTimeoutConstant		= 0;		// 
				timeouts.ReadTotalTimeoutMultiplier		= 0;		// 
				timeouts.WriteTotalTimeoutConstant		= 0;		// 
				timeouts.WriteTotalTimeoutMultiplier	= 0;		// 

				BOOL status = ::SetCommTimeouts(m_Handle, &timeouts);
				if (status == FALSE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- buffer size
			{
				// read  = 4096
				// write = 2048
				BOOL status = ::SetupComm(m_Handle, 4096, 2048);
				if (status == FALSE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- event mask
			{
				BOOL status = ::SetCommMask(m_Handle, EV_TXEMPTY|EV_RXCHAR);
				if (status == FALSE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- wait event setting
			{
				m_hEvent = ::CreateEventA(NULL, TRUE, FALSE, NULL);
				if (m_Handle == XIE_INVALID_HANDLE)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// ----- purge
			{
				BOOL status = ::PurgeComm(m_Handle, PURGE_RXCLEAR | PURGE_TXCLEAR | PURGE_RXABORT | PURGE_TXABORT);
				if (status == FALSE)
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
			::CloseHandle(m_Handle);
		m_Handle = XIE_INVALID_HANDLE;

		if (m_hEvent != XIE_INVALID_HANDLE)
			::CloseHandle(m_hEvent);
		m_hEvent = XIE_INVALID_HANDLE;
	}

	// ============================================================
	bool Readable(int timeout)
	{
		//::ResetEvent(m_hEvent);

		DWORD	dwTimeout = (timeout < 0) ? INFINITE : timeout;
		DWORD	mask = EV_RXCHAR;

		memset(&m_Overlapped, 0, sizeof(m_Overlapped));
		m_Overlapped.hEvent = m_hEvent;

		if (::WaitCommEvent(m_Handle, &mask, &m_Overlapped) == FALSE)
		{
			DWORD dwError = ::GetLastError();
			switch(dwError)
			{
			default:
				return false;
			case 87:	// unknown
			case ERROR_IO_PENDING:
				break;
			}
		}

		DWORD status = ::WaitForSingleObject(m_hEvent, dwTimeout);
		switch (status)
		{
		case WAIT_OBJECT_0:
			{
				DWORD dwNumber = 0;
				if (::GetOverlappedResult(m_Handle, &m_Overlapped, &dwNumber, FALSE))
					return true;
			}
			break;
		case WAIT_ABANDONED:
		case WAIT_TIMEOUT:
		default:
			break;
		}
		return false;
	}

	// ============================================================
	int Read(char* buffer, int length, int timeout)
	{
		//::ResetEvent(m_hEvent);

		DWORD	dwLength = 0;
		DWORD	dwTimeout = (timeout < 0) ? INFINITE : timeout;

		memset(&m_Overlapped, 0, sizeof(m_Overlapped));
		m_Overlapped.hEvent = m_hEvent;

		do
		{
			if (::ReadFile(m_Handle, buffer, length, &dwLength, &m_Overlapped) == TRUE)
			{
				if (dwLength == 0)
				{
					if (Readable(timeout) == false)
						return 0;
					if (::ReadFile(m_Handle, buffer, length, &dwLength, &m_Overlapped) == FALSE)
						break;
				}

				return (int)dwLength;
			}
		} while(false);

		// error
		{
			DWORD dwError = ::GetLastError();
			switch(dwError)
			{
			default:
				return 0;
			case 87:	// unknown
			case ERROR_IO_PENDING:
				break;
			}
		}

		DWORD status = ::WaitForSingleObject(m_hEvent, dwTimeout);
		switch (status)
		{
		case WAIT_OBJECT_0:
			{
				if (::GetOverlappedResult(m_Handle, &m_Overlapped, &dwLength, FALSE))
					return (int)dwLength;
			}
			break;
		case WAIT_ABANDONED:
		case WAIT_TIMEOUT:
		default:
			break;
		}
		return 0;
	}

	// ============================================================
	bool Writeable(int timeout)
	{
		//::ResetEvent(m_hEvent);

		DWORD	dwTimeout = (timeout < 0) ? INFINITE : timeout;
		DWORD	mask = EV_TXEMPTY;

		memset(&m_Overlapped, 0, sizeof(m_Overlapped));
		m_Overlapped.hEvent = m_hEvent;

		if (::WaitCommEvent(m_Handle, &mask, &m_Overlapped) == FALSE)
		{
			DWORD dwError = ::GetLastError();
			switch(dwError)
			{
			default:
				return false;
			case 87:	// unknown
			case ERROR_IO_PENDING:
				break;
			}
		}

		DWORD status = ::WaitForSingleObject(m_hEvent, dwTimeout);
		switch (status)
		{
		case WAIT_OBJECT_0:
			{
				DWORD dwNumber = 0;
				if (::GetOverlappedResult(m_Handle, &m_Overlapped, &dwNumber, FALSE))
					return true;
			}
			break;
		case WAIT_ABANDONED:
		case WAIT_TIMEOUT:
		default:
			break;
		}
		return false;
	}

	// ============================================================
	int Write(const char* buffer, int length, int timeout)
	{
		//::ResetEvent(m_hEvent);

		DWORD	dwLength = 0;
		DWORD	dwTimeout = (timeout < 0) ? INFINITE : timeout;

		memset(&m_Overlapped, 0, sizeof(m_Overlapped));
		m_Overlapped.hEvent = m_hEvent;

		if (::WriteFile(m_Handle, buffer, length, &dwLength, &m_Overlapped))
			return (int)dwLength;
		else
		{
			DWORD dwError = ::GetLastError();
			switch(dwError)
			{
			default:
				return 0;
			case 87:	// unknown
			case ERROR_IO_PENDING:
				break;
			}
		}

		DWORD status = ::WaitForSingleObject(m_hEvent, dwTimeout);
		switch (status)
		{
		case WAIT_OBJECT_0:
			{
				if (::GetOverlappedResult(m_Handle, &m_Overlapped, &dwLength, FALSE))
					return (int)dwLength;
			}
			break;
		case WAIT_ABANDONED:
		case WAIT_TIMEOUT:
		default:
			break;
		}
		return 0;
	}
};

}
}

#endif
