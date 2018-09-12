/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_net.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"

#include "Net/CxTcpServer.h"
#include "Net/CxTcpClient.h"
#include "Net/CxUdpClient.h"
#include "Net/TxIPAddress.h"
#include "Net/TxIPEndPoint.h"
#include "Net/TxSocketStream.h"
#include "Net/TxTcpServer.h"
#include "Net/TxTcpClient.h"
#include "Net/TxUdpClient.h"

namespace xie
{
namespace Net
{

// ================================================================================
XIE_EXPORT_FUNCTION HxModule XIE_API fnXIE_Net_Module_Create(TxCharCPtrA name)
{
	if (name == NULL) return NULL;
	try
	{
		if (strcmp(name, "CxTcpServer") == 0)	return (HxModule)new CxTcpServer();
		if (strcmp(name, "CxTcpClient") == 0)	return (HxModule)new CxTcpClient();
		if (strcmp(name, "CxUdpClient") == 0)	return (HxModule)new CxUdpClient();
	}
	catch(const CxException&)
	{
	}
	return NULL;
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_Module_Setup(HxModule handle)
{
	if (handle == NULL)
		return ExStatus::InvalidObject;
	try
	{
		if (xie::Axi::ClassIs<CxTcpClient>(handle))
		{
			CxTcpClient* _src = reinterpret_cast<CxTcpClient*>(handle);
			_src->Setup();
			return ExStatus::Success;
		}
		if (xie::Axi::ClassIs<CxTcpServer>(handle))
		{
			CxTcpServer* _src = reinterpret_cast<CxTcpServer*>(handle);
			_src->Setup();
			return ExStatus::Success;
		}
		if (xie::Axi::ClassIs<CxUdpClient>(handle))
		{
			CxUdpClient* _src = reinterpret_cast<CxUdpClient*>(handle);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_SocketStream_Readable(TxSocketStream tag, int timeout, ExBoolean* value)
{
	if (value == NULL)
		return ExStatus::InvalidParam;
	try
	{
		*value = (tag.Readable(timeout) ? ExBoolean::True : ExBoolean::False);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_SocketStream_Writeable(TxSocketStream tag, int timeout, ExBoolean* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		*result = (tag.Writeable(timeout) ? ExBoolean::True : ExBoolean::False);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_SocketStream_Read(TxSocketStream tag, char* buffer, int length, int timeout, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		*result = tag.Read(buffer, length, timeout);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_SocketStream_Write(TxSocketStream tag, const char* buffer, int length, int timeout, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		*result = tag.Write(buffer, length, timeout);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_TcpClient_Connected(HxModule handle, ExBoolean* value)
{
	if (value == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxTcpClient>(handle))
		{
			CxTcpClient* _src = reinterpret_cast<CxTcpClient*>(handle);
			*value = (_src->Connected() ? ExBoolean::True : ExBoolean::False);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_UdpClient_Readable(HxModule handle, int timeout, ExBoolean* value)
{
	if (value == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxUdpClient>(handle))
		{
			CxUdpClient* _src = reinterpret_cast<CxUdpClient*>(handle);
			*value = (_src->Readable(timeout) ? ExBoolean::True : ExBoolean::False);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_UdpClient_Writeable(HxModule handle, int timeout, ExBoolean* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxUdpClient>(handle))
		{
			CxUdpClient* _src = reinterpret_cast<CxUdpClient*>(handle);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_UdpClient_Read(HxModule handle, char* buffer, int length, int timeout, TxIPEndPoint* remoteEP, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	if (remoteEP == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxUdpClient>(handle))
		{
			CxUdpClient* _src = reinterpret_cast<CxUdpClient*>(handle);
			*result = _src->Read(buffer, length, timeout, *remoteEP);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Net_UdpClient_Write(HxModule handle, const char* buffer, int length, int timeout, const TxIPEndPoint* remoteEP, int* result)
{
	if (result == NULL)
		return ExStatus::InvalidParam;
	if (remoteEP == NULL)
		return ExStatus::InvalidParam;
	try
	{
		if (xie::Axi::ClassIs<CxUdpClient>(handle))
		{
			CxUdpClient* _src = reinterpret_cast<CxUdpClient*>(handle);
			*result = _src->Write(buffer, length, timeout, *remoteEP);
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
