/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_media.h"

#include "Media/CxCamera.h"
#include "Media/CxControlProperty.h"
#include "Media/CxDeviceList.h"
#include "Media/CxDeviceListItem.h"
#include "Media/CxDeviceParam.h"
#include "Media/CxGrabber.h"
#include "Media/CxGrabberArgs.h"
#include "Media/CxMediaPlayer.h"
#include "Media/CxScreenCapture.h"
#include "Media/CxScreenList.h"
#include "Media/CxScreenListItem.h"
#include "Media/IxMediaControl.h"
#include "Media/TxDeviceParam.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxString.h"

namespace xie
{
namespace Media
{

// ================================================================================
XIE_EXPORT_FUNCTION HxModule	XIE_API fnXIE_Media_Module_Create(TxCharCPtrA name)
{
	if (name == NULL) return NULL;

	try
	{
		if (strcmp(name, "CxCamera") == 0)				return (HxModule)new CxCamera();
		if (strcmp(name, "CxControlProperty") == 0)		return (HxModule)new CxControlProperty();
		if (strcmp(name, "CxDeviceList") == 0)			return (HxModule)new CxDeviceList();
		if (strcmp(name, "CxDeviceListItem") == 0)		return (HxModule)new CxDeviceListItem();
		if (strcmp(name, "CxDeviceParam") == 0)			return (HxModule)new CxDeviceParam();
		if (strcmp(name, "CxGrabber") == 0)				return (HxModule)new CxGrabber();
		if (strcmp(name, "CxGrabberArgs") == 0)			return (HxModule)new CxGrabberArgs();
		if (strcmp(name, "CxMediaPlayer") == 0)			return (HxModule)new CxMediaPlayer();
		if (strcmp(name, "CxScreenCapture") == 0)		return (HxModule)new CxScreenCapture();
		if (strcmp(name, "CxScreenList") == 0)			return (HxModule)new CxScreenList();
		if (strcmp(name, "CxScreenListItem") == 0)		return (HxModule)new CxScreenListItem();
		return NULL;
	}
	catch(const CxException&)
	{
		return NULL;
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_Camera_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCamera>(handle))
		{
			_src->Setup(hVideo, hAudio, hOutput);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_Camera_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCamera>(handle))
		{
			if (auto _event = xie::Axi::SafeCast<CxGrabber>(hEvent))
			{
				auto notify = _event->Notify;
				*_event = _src->CreateGrabber(type);
				_event->Notify = notify;
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

#if defined(_MSC_VER)
// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, HWND hWnd, ExMediaType type, int mode, TxCharCPtrA caption)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCamera>(handle))
		{
			_src->OpenPropertyDialog(hWnd, type, mode, caption);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}
#else
// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_Camera_OpenPropertyDialog(HxModule handle, Window window, ExMediaType type, int mode, TxCharCPtrA caption)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCamera>(handle))
		{
			_src->OpenPropertyDialog(window, type, mode, caption);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}
#endif

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaPlayer_Setup(HxModule handle, HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxMediaPlayer>(handle))
		{
			_src->Setup(hVideo, hAudio, hOutput);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaPlayer_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxMediaPlayer>(handle))
		{
			if (auto _event = xie::Axi::SafeCast<CxGrabber>(hEvent))
			{
				auto notify = _event->Notify;
				*_event = _src->CreateGrabber(type);
				_event->Notify = notify;
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaPlayer_WaitForCompletion(HxModule handle, int timeout, ExBoolean* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxMediaPlayer>(handle))
		{
			if (result != NULL)
				*result = _src->WaitForCompletion(timeout) ? ExBoolean::True : ExBoolean::False;
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenCapture_Setup(HxModule handle, HxModule hWindow, HxModule hAudio, HxModule hOutput)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenCapture>(handle))
		{
			_src->Setup(hWindow, hAudio, hOutput);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenCapture_CreateGrabber(HxModule handle, ExMediaType type, HxModule hEvent)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenCapture>(handle))
		{
			if (auto _event = xie::Axi::SafeCast<CxGrabber>(hEvent))
			{
				auto notify = _event->Notify;
				*_event = _src->CreateGrabber(type);
				_event->Notify = notify;
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenList_Setup(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenList>(handle))
		{
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenList_Length(HxModule handle, int* length)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenList>(handle))
		{
			if (length != NULL)
				*length = _src->Length();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenList_Item_Get(HxModule handle, int index, HxModule hVal)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenList>(handle))
		{
			if (auto _value = xie::Axi::SafeCast<CxScreenListItem>(hVal))
			{
				*_value = (*_src)[index];
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenList_Item_Set(HxModule handle, int index, HxModule hVal)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenList>(handle))
		{
			if (auto _value = xie::Axi::SafeCast<CxScreenListItem>(hVal))
			{
				(*_src)[index] = *_value;
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ScreenListItem_Name_Set(HxModule handle, TxCharCPtrA name)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxScreenListItem>(handle))
		{
			_src->Name(name);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceList_Setup(HxModule handle, ExMediaType type, ExMediaDir dir)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceList>(handle))
		{
			_src->Setup(type, dir);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceList_Length(HxModule handle, int* length)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceList>(handle))
		{
			if (length != NULL)
				*length = _src->Length();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceList_Item_Get(HxModule handle, int index, HxModule hVal)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceList>(handle))
		{
			if (auto _value = xie::Axi::SafeCast<CxDeviceListItem>(hVal))
			{
				*_value = (*_src)[index];
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceList_Item_Set(HxModule handle, int index, HxModule hVal)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceList>(handle))
		{
			if (auto _value = xie::Axi::SafeCast<CxDeviceListItem>(hVal))
			{
				(*_src)[index] = *_value;
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceListItem_Name_Set(HxModule handle, TxCharCPtrA name)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceListItem>(handle))
		{
			_src->Name(name);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceListItem_GetProductName(HxModule handle, HxModule hResult)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceListItem>(handle))
		{
			if (auto _result = xie::Axi::SafeCast<CxStringA>(hResult))
			{
				_result->CopyFrom(_src->GetProductName());
				return ExStatus::Success;
			}
			if (auto _result = xie::Axi::SafeCast<CxStringW>(hResult))
			{
				_result->CopyFrom(_src->GetProductName());
				return ExStatus::Success;
			}
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceListItem_GetPinNames(HxModule handle, HxModule hResult)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceListItem>(handle))
		{
			if (auto _result = xie::Axi::SafeCast<CxArray>(hResult))
			{
				auto pinnames = _src->GetPinNames();
				_result->Resize(pinnames.Length(), TxModel::Ptr(1));
				_result->Scanner<void*>().ForEach([&pinnames](int i, void** addr)
				{
					*addr = new CxStringA(pinnames[i].Address());	// The caller must destroy each item.
				});
			}
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceListItem_GetFrameSizes(HxModule handle, HxModule hResult, int pin)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceListItem>(handle))
		{
			if (auto _result = xie::Axi::SafeCast<CxArray>(hResult))
			{
				auto ans = _src->GetFrameSizes(pin);
				*_result = CxArray::From(ans);
			}
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_DeviceParam_Name_Set(HxModule handle, TxCharCPtrA name)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxDeviceParam>(handle))
		{
			_src->Name(name);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_Grabber_Notify_Set(HxModule handle, void* function)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxGrabber>(handle))
		{
			typedef void (XIE_API *handler)(void* sender, xie::Media::CxGrabberArgs* e);
			_src->Notify = CxGrabberEvent(reinterpret_cast<handler>(function));
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// IxMediaControl
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Reset(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			_src->Reset();
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Start(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			_src->Start();
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Stop(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			_src->Stop();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Abort(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			_src->Abort();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Pause(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			_src->Pause();
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_Wait(HxModule handle, int timeout, ExBoolean* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			if (result != NULL)
				*result = _src->Wait(timeout) ? ExBoolean::True : ExBoolean::False;
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_IsRunning(HxModule handle, ExBoolean* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			if (result != NULL)
				*result = _src->IsRunning() ? ExBoolean::True : ExBoolean::False;
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_MediaControl_IsPaused(HxModule handle, ExBoolean* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<IxMediaControl>(handle))
		{
			if (result != NULL)
				*result = _src->IsPaused() ? ExBoolean::True : ExBoolean::False;
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// //////////////////////////////////////////////////////////////////////
// CxControlProperty
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_Controller_Set(HxModule handle, HxModule value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			_src->Controller(value);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_Name_Set(HxModule handle, TxCharCPtrA value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			_src->Name(value);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_IsSupported(HxModule handle, ExBoolean* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->IsSupported() ? ExBoolean::True : ExBoolean::False;
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_GetRange(HxModule handle, TxRangeI* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->GetRange();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_GetStep(HxModule handle, int* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->GetStep();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_GetDefault(HxModule handle, int* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->GetDefault();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_GetFlags(HxModule handle, int* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->GetFlags();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_SetFlags(HxModule handle, int value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			_src->SetFlags(value);
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_GetValue(HxModule handle, int* value)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			if (value != NULL)
				*value = _src->GetValue();
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Media_ControlProperty_SetValue(HxModule handle, int value, ExBoolean relative)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxControlProperty>(handle))
		{
			_src->SetValue(value, (relative == ExBoolean::True));
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
