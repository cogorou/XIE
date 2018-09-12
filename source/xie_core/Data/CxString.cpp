/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxString.h"

namespace xie
{

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_String_Resize(HxModule handle, int length)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxStringA>(handle))
		{
			_src->Resize(length);
			return ExStatus::Success;
		}
		if (auto _src = xie::Axi::SafeCast<CxStringW>(handle))
		{
			_src->Resize(length);
			return ExStatus::Success;
		}

		return ExStatus::Unsupported;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_String_Reset(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxStringA>(handle))
		{
			_src->Reset();
			return ExStatus::Success;
		}
		if (auto _src = xie::Axi::SafeCast<CxStringW>(handle))
		{
			_src->Reset();
			return ExStatus::Success;
		}

		return ExStatus::Unsupported;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
