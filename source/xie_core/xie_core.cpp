/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_core.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/IxRunnable.h"
#include "Core/IxParam.h"
#include "Core/TxStatistics.h"
#include "Data/api_data.h"
#include "File/api_file.h"

// Core Modules
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "Core/CxMatrix.h"
#include "Core/CxString.h"
#include "Core/CxExif.h"

#include <stdio.h>
#include <stdlib.h>
#include <stdarg.h>
#include <string.h>

#if defined(_MSC_VER)
#include <locale.h>
#endif

#pragma warning (disable:4101)	// ローカル変数は 1 度も使われていません。

namespace xie
{

// //////////////////////////////////////////////////////////////////////
// setup/teardown
//

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API xie_core_setup()
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);

	#if defined(_MSC_VER)
	{
		char* ans = setlocale(LC_ALL, "");
		if (ans != NULL)
		{
			fnXIE_Core_TraceOut(1, "setlocale = %s\n", ans);
		}
		else
		{
			fnXIE_Core_TraceOut(1, "setlocale failed.\n");
		}
	}
	#endif
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API xie_core_teardown()
{
	fnXIE_Core_TraceOut(1, "%s\n", __FUNCTION__);
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API xie_core_plugin(void* handle)
{
	// ----------------------------------------------------------------------
	// api_file
	/*
		These implementations can be found under [source/xie_high/File] directory.
		These extern variables are declared in [File/api_file.h] and the bodies exist in [File/api_file.cpp] .
	*/
	if (auto ope = xie::Axi::SafeCast<IxFilePluginJpeg>((HxModule)handle))	xie::File::p_jpeg = ope;
	if (auto ope = xie::Axi::SafeCast<IxFilePluginPng>((HxModule)handle))	xie::File::p_png = ope;
	if (auto ope = xie::Axi::SafeCast<IxFilePluginTiff>((HxModule)handle))	xie::File::p_tiff = ope;
}

// //////////////////////////////////////////////////////////////////////
// Debugger
//

// ============================================================
static int g_TraceLevel = 0;

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_Core_TraceLevel_Set(int value)
{
	g_TraceLevel = value;
}

// ============================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_TraceLevel_Get()
{
	return g_TraceLevel;
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_CDECL fnXIE_Core_TraceOutA(int level, TxCharCPtrA format, ...)
{
#if defined(_MSC_VER)
	if (level <= g_TraceLevel)
	{
		va_list argList;
		va_start(argList, format);
		int length = _vscprintf( format, argList );
		if (length > 0)
		{
			CxArrayEx<char> buffer;
			buffer.Resize(length + 1);
			buffer.Clear(0);
			vsprintf( buffer.Address(), format, argList );
			fputs(buffer.Address(), stdout);
			OutputDebugStringA(buffer.Address());
			buffer.Dispose();
		}
	}
#else
	if (level <= g_TraceLevel)
	{
		va_list argList;
		va_start(argList, format);
		char* buffer = NULL;
		int length = vasprintf( &buffer, format, argList );
		if (length > 0)
		{
			fputs(buffer, stdout);
		}
		if (buffer != NULL)
			free( buffer );
	}
#endif
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_CDECL fnXIE_Core_TraceOutW(int level, TxCharCPtrW format, ...)
{
#if defined(_MSC_VER)
	if (level <= g_TraceLevel)
	{
		va_list argList;
		va_start(argList, format);
		int length = _vscwprintf( format, argList );
		if (length > 0)
		{
			CxArrayEx<wchar_t> buffer;
			for(; length < 1024*10 ; length++)
			{
				buffer.Resize(length + 1);
				buffer.Clear(0);
				int ans_length = _vsnwprintf_s( buffer.Address(), (size_t)length, _TRUNCATE, format, argList );
				if (ans_length >= 0)
				{
					fputws(buffer.Address(), stdout);
					OutputDebugStringW(buffer.Address());
					break;
				}
			}
			buffer.Dispose();
		}
	}
#else
	if (level <= g_TraceLevel)
	{
		va_list argList;
		va_start(argList, format);
		CxArrayEx<wchar_t> buffer;
		// vscwprintf( &buffer, format, argList );
		for(int length = 1024 ; length < 1024*10 ; length += 1024)
		{
			buffer.Resize(length);
			buffer.Clear(0);

			va_list args;
			va_copy(args, argList);
			int ans_length = vswprintf(buffer.Address(), length, format, args);
			if (ans_length >= 0)
			{
				fputws(buffer.Address(), stdout);
				break;
			}
		}
		buffer.Dispose();
	}
#endif
}

// //////////////////////////////////////////////////////////////////////
// Module
//

// ======================================================================
XIE_EXPORT_FUNCTION HxModule XIE_API fnXIE_Core_Module_Create(TxCharCPtrA name)
{
	if (name == NULL) return NULL;

	try
	{
		// Core Modules
		if (strcmp(name, "CxArray") == 0)		return (HxModule)new CxArray();
		if (strcmp(name, "CxImage") == 0)		return (HxModule)new CxImage();
		if (strcmp(name, "CxMatrix") == 0)		return (HxModule)new CxMatrix();
		if (strcmp(name, "CxString") == 0)		return (HxModule)new CxString();
		if (strcmp(name, "CxStringA") == 0)		return (HxModule)new CxStringA();
		if (strcmp(name, "CxStringW") == 0)		return (HxModule)new CxStringW();
		if (strcmp(name, "CxExif") == 0)		return (HxModule)new CxExif();
	}
	catch(const CxException&)
	{
	}
	return NULL;
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Module_Destroy(HxModule handle)
{
	if (handle == NULL)
		return ExStatus::Success;

	auto _src = xie::Axi::SafeCast<CxModule>(handle);
	if (_src == NULL ||
		_src->ModuleID() != XIE_MODULE_ID)
		return ExStatus::InvalidObject;
	try
	{
		delete _src;
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Module_Dispose(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<IxDisposable>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->Dispose();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_Core_Module_IsValid(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<IxDisposable>(handle);
	if (_src == NULL)
		return ExBoolean::False;
	return _src->IsValid() ? ExBoolean::True : ExBoolean::False;
}

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Module_ID(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<CxModule>(handle);
	if (_src == NULL)
		return 0;
	return _src->ModuleID();
}

// //////////////////////////////////////////////////////////////////////
// Memory Alloc/Free
//

// ======================================================================
XIE_EXPORT_FUNCTION void* XIE_API fnXIE_Core_Axi_MemoryAlloc(size_t size, ExBoolean zero_clear)
{
	return xie::Axi::MemoryAlloc(size, (zero_clear == ExBoolean::True));
}

// ======================================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_Core_Axi_MemoryFree(void* ptr)
{
	xie::Axi::MemoryFree(ptr);
}

// //////////////////////////////////////////////////////////////////////
// Memory Map/Unmap
//

// ======================================================================
XIE_EXPORT_FUNCTION void* XIE_API fnXIE_Core_Axi_MemoryMap(size_t size)
{
	return xie::Axi::MemoryMap(size);
}

// ======================================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_Core_Axi_MemoryUnmap(void* ptr, size_t size)
{
	xie::Axi::MemoryUnmap(ptr, size);
}

// //////////////////////////////////////////////////////////////////////
// Memory Lock/Unlock
//

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_MemoryLock(void* ptr, size_t size)
{
	return xie::Axi::MemoryLock(ptr, size);
}

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_MemoryUnlock(void* ptr, size_t size)
{
	return xie::Axi::MemoryUnlock(ptr, size);
}

// //////////////////////////////////////////////////////////////////////
// Model
//

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_SizeOf(ExType type)
{
	return xie::Axi::SizeOf(type);
}

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_CalcBpp(ExType type)
{
	return xie::Axi::CalcBpp(type);
}

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_CalcDepth(ExType type)
{
	return xie::Axi::CalcDepth(type);
}

// ======================================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_Core_Axi_CalcRange(ExType type, int depth, TxRangeD* range)
{
	if (range != NULL)
		*range = xie::Axi::CalcRange(type, depth);
}

// ======================================================================
XIE_EXPORT_FUNCTION double XIE_API fnXIE_Core_Axi_CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth)
{
	return xie::Axi::CalcScale(src_type, src_depth, dst_type, dst_depth);
}

// ======================================================================
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_Axi_CalcStride(TxModel model, int width, int packing_size)
{
	return xie::Axi::CalcStride(model, width, packing_size);
}

// //////////////////////////////////////////////////////////////////////
// TxDateTime
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Axi_GetTime(unsigned long long* result)
{
	if (result == NULL) return ExStatus::InvalidParam;

	try
	{
		*result = xie::Axi::GetTime();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DateTime_Now(ExBoolean ltc, TxDateTime* result)
{
	if (result == NULL) return ExStatus::InvalidParam;

	try
	{
		*result = TxDateTime::Now((ltc == ExBoolean::True));
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DateTime_ToBinary(TxDateTime src, ExBoolean ltc, unsigned long long* dst)
{
	if (dst == NULL) return ExStatus::InvalidParam;

	try
	{
		*dst = src.ToBinary((ltc == ExBoolean::True));
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_DateTime_FromBinary(unsigned long long src, ExBoolean ltc, TxDateTime* dst)
{
	if (dst == NULL) return ExStatus::InvalidParam;

	try
	{
		*dst = TxDateTime::FromBinary(src, (ltc == ExBoolean::True));
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// IxTagPtr
//

// ======================================================================
XIE_EXPORT_FUNCTION void* XIE_API fnXIE_Core_TagPtr(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<IxTagPtr>(handle);
	if (_src == NULL)
		return NULL;
	try
	{
		return _src->TagPtr();
	}
	catch(const CxException&)
	{
		return NULL;
	}
}

// //////////////////////////////////////////////////////////////////////
// IxEquatable
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Equatable_CopyFrom(HxModule handle, HxModule src)
{
	auto _dst = xie::Axi::SafeCast<IxEquatable>(handle);
	auto _src = xie::Axi::SafeCast<CxModule>(src);
	if (_dst == NULL || _src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_dst->CopyFrom(*_src);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_Core_Equatable_ContentEquals(HxModule handle, HxModule cmp)
{
	auto _dst = xie::Axi::SafeCast<IxEquatable>(handle);
	auto _src = xie::Axi::SafeCast<CxModule>(cmp);
	if (_dst == NULL || _src == NULL)
		return ExBoolean::False;
	return _dst->ContentEquals(*_src) ? ExBoolean::True : ExBoolean::False;
}

// //////////////////////////////////////////////////////////////////////
// IxAttachable
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Attachable_Attach(HxModule handle, HxModule src)
{
	auto _dst = xie::Axi::SafeCast<IxAttachable>(handle);
	auto _src = xie::Axi::SafeCast<CxModule>(src);
	if (_dst == NULL || _src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_dst->Attach(*_src);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_Core_Attachable_IsAttached(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<IxAttachable>(handle);
	if (_src == NULL)
		return ExBoolean::False;
	return _src->IsAttached() ? ExBoolean::True : ExBoolean::False;
}

// //////////////////////////////////////////////////////////////////////
// IxLockable
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Lockable_Lock(HxModule handle)
{
	auto _dst = xie::Axi::SafeCast<IxLockable>(handle);
	if (_dst == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_dst->Lock();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Lockable_Unlock(HxModule handle)
{
	auto _dst = xie::Axi::SafeCast<IxLockable>(handle);
	if (_dst == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_dst->Unlock();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_Core_Lockable_IsLocked(HxModule handle)
{
	auto _src = xie::Axi::SafeCast<IxLockable>(handle);
	if (_src == NULL)
		return ExBoolean::False;
	return _src->IsLocked() ? ExBoolean::True : ExBoolean::False;
}

// //////////////////////////////////////////////////////////////////////
// IxRunnable
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Runnable_Reset(HxModule handle)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxRunnable>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->Reset();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Runnable_Start(HxModule handle)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxRunnable>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->Start();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Runnable_Stop(HxModule handle)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxRunnable>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->Stop();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Runnable_Wait(HxModule handle, int timeout, ExBoolean* result)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxRunnable>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		if (result != NULL)
			*result = _isrc->Wait(timeout) ? ExBoolean::True : ExBoolean::False;
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Runnable_IsRunning(HxModule handle, ExBoolean* result)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxRunnable>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		if (result != NULL)
			*result = _isrc->IsRunning() ? ExBoolean::True : ExBoolean::False;
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// IxParam
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Param_GetParam(HxModule handle, TxCharCPtrA name, void* value, TxModel model)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxParam>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->GetParam(name, value, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Param_SetParam(HxModule handle, TxCharCPtrA name, const void* value, TxModel model)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxParam>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->SetParam(name, value, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// IxIndexedParam
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_IndexedParam_GetParam(HxModule handle, TxCharCPtrA name, int index, void* value, TxModel model)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxIndexedParam>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->GetParam(name, index, value, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_IndexedParam_SetParam(HxModule handle, TxCharCPtrA name, int index, const void* value, TxModel model)
{
	try
	{
		auto _isrc = xie::Axi::SafeCast<IxIndexedParam>(handle);
		if (_isrc == NULL)
			return ExStatus::InvalidObject;
		_isrc->SetParam(name, index, value, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// IxFileAccess
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_FileAccess_LoadA(HxModule hdst, TxCharCPtrA filename, const void* option, TxModel model)
{
	try
	{
		auto _dst = xie::Axi::SafeCast<IxFileAccess>(hdst);
		if (_dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_dst->LoadA(filename, option, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_FileAccess_LoadW(HxModule hdst, TxCharCPtrW filename, const void* option, TxModel model)
{
	try
	{
		auto _dst = xie::Axi::SafeCast<IxFileAccess>(hdst);
		if (_dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_dst->LoadW(filename, option, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_FileAccess_SaveA(HxModule hsrc, TxCharCPtrA filename, const void* option, TxModel model)
{
	try
	{
		auto _src = xie::Axi::SafeCast<IxFileAccess>(hsrc);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->SaveA(filename, option, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_FileAccess_SaveW(HxModule hsrc, TxCharCPtrW filename, const void* option, TxModel model)
{
	try
	{
		auto _src = xie::Axi::SafeCast<IxFileAccess>(hsrc);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->SaveW(filename, option, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
