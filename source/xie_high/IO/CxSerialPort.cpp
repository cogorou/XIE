/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_io.h"
#include "IO/CxSerialPort.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

#if defined(_MSC_VER)
#include "CxSerialPort_Windows.h"
#else
#include "CxSerialPort_Linux.h"
#endif

namespace xie
{
namespace IO
{

static const char* g_ClassName = "CxSerialPort";

#if defined(_MSC_VER)
typedef CxSerialPort_Windows	Controller;	//!< The model only used in this file.
#else
typedef CxSerialPort_Linux		Controller;	//!< The model only used in this file.
#endif

// =================================================================
void CxSerialPort::_Constructor()
{
	m_Controller = new Controller();
	m_Param = TxSerialPort::Default();
}

// =================================================================
CxSerialPort::CxSerialPort()
{
	_Constructor();
}

// =================================================================
CxSerialPort::CxSerialPort( const CxSerialPort& src )
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxSerialPort::~CxSerialPort()
{
	Dispose();

	if (m_Controller != NULL)
		delete ((Controller*)m_Controller);
	m_Controller = NULL;
}

// ============================================================
CxSerialPort& CxSerialPort::operator = ( const CxSerialPort& src )
{
	m_PortName	= src.m_PortName;
	m_Param		= src.m_Param;
	return *this;
}

// ============================================================
bool CxSerialPort::operator == ( const CxSerialPort& src ) const
{
	if (m_PortName	!= src.m_PortName) return false;
	if (m_Param		!= src.m_Param) return false;
	return true;
}

// ============================================================
bool CxSerialPort::operator != ( const CxSerialPort& src ) const
{
	return !(CxSerialPort::operator == (src));
}

// =================================================================
TxSerialPort CxSerialPort::Tag() const
{
	return m_Param;
}

// =================================================================
void* CxSerialPort::TagPtr() const
{
	return (void*)&m_Param;
}

// =================================================================
void CxSerialPort::Setup()
{
	Dispose();

	if (m_Controller == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	static_cast<Controller*>(m_Controller)->Open(m_PortName.Address(), m_Param);
}

// =================================================================
void CxSerialPort::Dispose()
{
	if (m_Controller != NULL)
		static_cast<Controller*>(m_Controller)->Close();
}

// =================================================================
bool CxSerialPort::IsValid() const
{
	if (m_Controller == NULL)
		return false;

	return static_cast<Controller*>(m_Controller)->IsValid();
}

// =================================================================
bool CxSerialPort::Readable(int timeout) const
{
	if (m_Controller == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<Controller*>(m_Controller)->Readable(timeout);
}

// =================================================================
int CxSerialPort::Read(char* buffer, int length, int timeout) const
{
	if (m_Controller == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<Controller*>(m_Controller)->Read(buffer, length, timeout);
}

// =================================================================
bool CxSerialPort::Writeable(int timeout) const
{
	if (m_Controller == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<Controller*>(m_Controller)->Writeable(timeout);
}

// =================================================================
int CxSerialPort::Write(const char* buffer, int length, int timeout) const
{
	if (m_Controller == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<Controller*>(m_Controller)->Write(buffer, length, timeout);
}

// =================================================================
TxCharCPtrA CxSerialPort::PortName() const
{
	return m_PortName.Address();
}

// =================================================================
void CxSerialPort::PortName(TxCharCPtrA value)
{
	m_PortName = value;
}

// =================================================================
TxSerialPort& CxSerialPort::Param()
{
	return m_Param;
}

// =================================================================
const TxSerialPort& CxSerialPort::Param() const
{
	return m_Param;
}

// =================================================================
void CxSerialPort::Param(const TxSerialPort& value)
{
	m_Param = value;
}

}
}
