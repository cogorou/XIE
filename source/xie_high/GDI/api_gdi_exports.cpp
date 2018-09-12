/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "GDI/CxBitmap.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxGdiVoid.h"
#include "GDI/CxGdiImage.h"
#include "GDI/CxGdiPolyline.h"
#include "GDI/CxGdiStringA.h"
#include "GDI/CxGdiStringW.h"
#include "GDI/CxGdiPoint.h"
#include "GDI/CxGdiRectangle.h"
#include "GDI/CxGdiCircle.h"
#include "GDI/CxGdiCircleArc.h"
#include "GDI/CxGdiEllipse.h"
#include "GDI/CxGdiEllipseArc.h"
#include "GDI/CxGdiLine.h"
#include "GDI/CxGdiLineSegment.h"
#include "GDI/CxGdiTrapezoid.h"
#include "GDI/CxGraphics.h"
#include "GDI/CxOverlayGrid.h"
#include "GDI/CxOverlayProfile.h"
#include "GDI/IxGdi2d.h"
#include "GDI/TxGdi2dParam.h"

namespace xie
{
namespace GDI
{

// //////////////////////////////////////////////////////////////////////
// Module
//

// ======================================================================
XIE_EXPORT_FUNCTION HxModule XIE_API fnXIE_GDI_Module_Create(TxCharCPtrA name)
{
	if (name == NULL) return NULL;

	try
	{
		if (strcmp(name, "CxCanvas") == 0)			return (HxModule)new CxCanvas();
		if (strcmp(name, "CxCanvas") == 0)			return (HxModule)new CxCanvas();
		if (strcmp(name, "CxGdiImage") == 0)		return (HxModule)new CxGdiImage();
		if (strcmp(name, "CxGdiStringA") == 0)		return (HxModule)new CxGdiStringA();
		if (strcmp(name, "CxGdiStringW") == 0)		return (HxModule)new CxGdiStringW();
		if (strcmp(name, "CxGdiPolyline") == 0)		return (HxModule)new CxGdiPolyline();
		if (strcmp(name, "CxGdiPoint") == 0)		return (HxModule)new CxGdiPoint();
		if (strcmp(name, "CxGdiRectangle") == 0)	return (HxModule)new CxGdiRectangle();
		if (strcmp(name, "CxGdiCircle") == 0)		return (HxModule)new CxGdiCircle();
		if (strcmp(name, "CxGdiCircleArc") == 0)	return (HxModule)new CxGdiCircleArc();
		if (strcmp(name, "CxGdiEllipse") == 0)		return (HxModule)new CxGdiEllipse();
		if (strcmp(name, "CxGdiEllipseArc") == 0)	return (HxModule)new CxGdiEllipseArc();
		if (strcmp(name, "CxGdiLine") == 0)			return (HxModule)new CxGdiLine();
		if (strcmp(name, "CxGdiLineSegment") == 0)	return (HxModule)new CxGdiLineSegment();
		if (strcmp(name, "CxGdiTrapezoid") == 0)	return (HxModule)new CxGdiTrapezoid();
		if (strcmp(name, "CxGdiVoid") == 0)			return (HxModule)new CxGdiVoid();
		if (strcmp(name, "CxGraphics") == 0)		return (HxModule)new CxGraphics();
		if (strcmp(name, "CxOverlayGrid") == 0)		return (HxModule)new CxOverlayGrid();
		if (strcmp(name, "CxOverlayProfile") == 0)	return (HxModule)new CxOverlayProfile();
	}
	catch(const CxException&)
	{
	}
	return NULL;
}

// //////////////////////////////////////////////////////////////////////
// Canvas
//

// ================================================================================
#if defined(_MSC_VER)
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Setup(HxModule handle, void* hdc)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Setup((HDC)hdc);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Setup(HxModule handle, int drawable)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Setup((GLXDrawable)drawable);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Resize(HxModule handle, int width, int height)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Resize(width, height);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_BeginPaint(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->BeginPaint();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_EndPaint(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->EndPaint();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Lock(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Lock();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Unlock(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Unlock();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Clear(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Clear();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_DrawImage(HxModule handle, HxModule himage)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->DrawImage(himage);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_DrawOverlay(HxModule handle, HxModule* hfigures, int count, ExGdiScalingMode mode)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->DrawOverlay(hfigures, count, mode);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_DrawOverlayCB(HxModule handle, fnXIE_GDI_CallBack_Render render, void* param, ExGdiScalingMode mode)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->DrawOverlayCB(render, param, mode);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Flush(HxModule handle)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Flush();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_FlushToImage(HxModule handle, HxModule hdst)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			_src->Flush(hdst);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_EffectiveRect(HxModule handle, TxRectangleI* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			if (result != NULL)
				*result = _src->EffectiveRect();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_VisibleRect(HxModule handle, TxRectangleD* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			if (result != NULL)
				*result = _src->VisibleRect();
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_VisibleRectI(HxModule handle, ExBoolean includePartialPixel, TxRectangleI* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			if (result != NULL)
				*result = _src->VisibleRectI((includePartialPixel == ExBoolean::True));
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_DPtoIP(HxModule handle, TxPointD dp, ExGdiScalingMode mode, TxPointD* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			if (result != NULL)
				*result = _src->DPtoIP(dp, mode);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_IPtoDP(HxModule handle, TxPointD ip, ExGdiScalingMode mode, TxPointD* result)
{
	try
	{
		if (auto _src = xie::Axi::SafeCast<CxCanvas>(handle))
		{
			if (result != NULL)
				*result = _src->IPtoDP(ip, mode);
			return ExStatus::Success;
		}
		return ExStatus::InvalidObject;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_GDI_Canvas_Api_EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxRectangleI* result)
{
	if (result != NULL)
		*result = xie::GDI::CxCanvas::EffectiveRect(display_rect, bg_size, mag);
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_GDI_Canvas_Api_VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxRectangleD* result)
{
	if (result != NULL)
		*result = xie::GDI::CxCanvas::VisibleRect(display_rect, bg_size, mag, view_point);
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_GDI_Canvas_Api_DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, ExGdiScalingMode mode, TxPointD* result)
{
	if (result != NULL)
		*result = xie::GDI::CxCanvas::DPtoIP(display_rect, bg_size, mag, view_point, dp, mode);
}

// ============================================================
XIE_EXPORT_FUNCTION void XIE_API fnXIE_GDI_Canvas_Api_IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, ExGdiScalingMode mode, TxPointD* result)
{
	if (result != NULL)
		*result = xie::GDI::CxCanvas::IPtoDP(display_rect, bg_size, mag, view_point, ip, mode);
}

// ============================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Canvas_Api_Extract(HxModule hdst, HxModule hsrc, TxCanvas canvas)
{
	try
	{
		fnPRV_GDI_Extract(hdst, hsrc, canvas, false);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

//
// General
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_2d_Reset(HxModule handle)
{
	try
	{
		if (handle == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (auto _src = xie::Axi::SafeCast<CxGdiImage>(handle))
		{
			_src->Reset();
			return ExStatus::Success;
		}
		if (auto _src = xie::Axi::SafeCast<CxGdiPolyline>(handle))
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

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_2d_Resize(HxModule handle, int length)
{
	try
	{
		if (handle == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (length < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		if (xie::Axi::ClassIs<CxGdiPolyline>(handle))
		{
			auto _src = reinterpret_cast<CxGdiPolyline*>(handle);
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

//
// Image
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Image_Resize(HxModule handle, int width, int height)
{
	CxGdiImage* _src = xie::Axi::SafeCast<CxGdiImage>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->Resize(width, height);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

//
// StringA
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_StringA_Text_Set(HxModule handle, TxCharCPtrA text)
{
	CxGdiStringA* _src = xie::Axi::SafeCast<CxGdiStringA>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->Text(text);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_StringA_FontName_Set(HxModule handle, TxCharCPtrA text)
{
	CxGdiStringA* _src = xie::Axi::SafeCast<CxGdiStringA>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->FontName(text);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

//
// StringW
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_StringW_Text_Set(HxModule handle, TxCharCPtrW text)
{
	CxGdiStringW* _src = xie::Axi::SafeCast<CxGdiStringW>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->Text(text);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_StringW_FontName_Set(HxModule handle, TxCharCPtrW text)
{
	CxGdiStringW* _src = xie::Axi::SafeCast<CxGdiStringW>(handle);
	if (_src == NULL)
		return ExStatus::InvalidObject;
	try
	{
		_src->FontName(text);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

//
// etc
//

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_CanConvertFrom(HxModule hdst, TxSizeI size, ExBoolean* value)
{
	if (value == NULL)
		return ExStatus::InvalidParam;

	try
	{
		*value = fnPRV_GDI_CanConvertFrom(hdst, size) ? ExBoolean::True : ExBoolean::False;
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_ConvertFrom_DDB(HxModule hdst, HxModule hsrc)
{
	try
	{
		fnPRV_GDI_ConvertFrom_DDB(hdst, hsrc);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ================================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_ConvertFrom_YUYV(HxModule hdst, HxModule hsrc)
{
	try
	{
		fnPRV_GDI_ConvertFrom_YUYV(hdst, hsrc);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
}
