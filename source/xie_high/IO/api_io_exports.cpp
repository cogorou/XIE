/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_io.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"

#include "IO/CxSerialPort.h"
#include "IO/TxSerialPort.h"

namespace xie
{
namespace IO
{

// ================================================================================
XIE_EXPORT_FUNCTION HxModule XIE_API fnXIE_IO_Module_Create(TxCharCPtrA name)
{
	if (name == NULL) return NULL;
	try
	{
		if (strcmp(name, "CxSerialPort") == 0)	return (HxModule)new CxSerialPort();
	}
	catch(const CxException&)
	{
	}
	return NULL;
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_Module_Setup(HxModule handle)
{
	if (handle == NULL)
		return ExStatus::InvalidObject;
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			_src->Setup();
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION TxCharCPtrA XIE_API fnXIE_IO_SerialPort_PortName_Get(HxModule handle)
{
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			return _src->PortName();
		}
		return NULL;
	}
	catch(const CxException&)
	{
		return NULL;
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_SerialPort_PortName_Set(HxModule handle, TxCharCPtrA value)
{
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			_src->PortName(value);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_SerialPort_Readable(HxModule handle, int timeout, ExBoolean* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			*result = (_src->Readable(timeout) ? ExBoolean::True : ExBoolean::False);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_SerialPort_Writeable(HxModule handle, int timeout, ExBoolean* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			*result = (_src->Writeable(timeout) ? ExBoolean::True : ExBoolean::False);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_SerialPort_Read(HxModule handle, char* buffer, int length, int timeout, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			*result = _src->Read(buffer, length, timeout);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_IO_SerialPort_Write(HxModule handle, const char* buffer, int length, int timeout, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxSerialPort>(handle))
		{
			CxSerialPort* _src = reinterpret_cast<CxSerialPort*>(handle);
			*result = _src->Write(buffer, length, timeout);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
}
