/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxFinalizer.h"
#include "GDI/IxGdi2d.h"
#include "GDI/CxOverlay.h"
#include "GDI/CxTexture.h"
#include "GDI/CxBitmap.h"
#include "GDI/CxGraphics.h"
#include "GDI/CxOverlayProfile.h"
#include "CxGLContext.h"

#if defined(_MSC_VER)
#include <GL/gl.h>
#else
#include <GL/glew.h>
#include <GL/gl.h>
#endif

namespace xie
{
namespace GDI
{

#if defined(_MSC_VER)
// ============================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_GDI_Graphics_CheckValidity(HDC hdc)
{
	return CxGraphics::CheckValidity(hdc) ? ExBoolean::True : ExBoolean::False;
}
// ============================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Graphics_DrawImage(HDC hdc, HxModule hsrc, TxCanvas canvas)
{
	try
	{
		if (CxGraphics::CheckValidity(hdc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		const CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (src->IsValid())
		{
			CxGraphics graphics;
			graphics.Setup(hdc);
			graphics.Tag(canvas);
			graphics.DrawImage(hsrc);
		}
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}
#else
// ============================================================
XIE_EXPORT_FUNCTION ExBoolean XIE_API fnXIE_GDI_Graphics_CheckValidity(void* hdc)
{
	return CxGraphics::CheckValidity(hdc) ? ExBoolean::True : ExBoolean::False;
}
// ============================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Graphics_DrawImage(void* hdc, HxModule hsrc, TxCanvas canvas)
{
	try
	{
		if (CxGraphics::CheckValidity(hdc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		const CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (src->IsValid())
		{
			CxGraphics graphics;
			graphics.Setup(hdc);
			graphics.Tag(canvas);
			graphics.DrawImage(hsrc);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}
#endif

// ============================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_GDI_Profile_MakeGraph(HxModule hsrc, TxCanvas canvas, TxPointD position, HxModule* hgraphs_x, HxModule* hgraphs_y, double* values)
{
	try
	{
		const CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (hgraphs_x == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (hgraphs_y == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (values == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		auto mag = canvas.Magnification;
		auto vis = canvas.VisibleRect();
		auto eff = canvas.EffectiveRect();

		auto size = src->ImageSize();
		auto elements = size.Model.Pack * size.Channels;

		auto channelNo = canvas.ChannelNo;
		if (channelNo < 0)
			channelNo = 0;
		if (channelNo > elements - 1)
			channelNo = elements - 1;

		auto unpack = (canvas.Unpack == ExBoolean::True);
		if (unpack)
			elements = 1;

		auto pos_x = (int)round(position.X);
		auto pos_y = (int)round(position.Y);
		if (pos_x < 0)
			pos_x = 0;
		if (pos_y < 0)
			pos_y = 0;
		if (pos_x > size.Width - 1)
			pos_x = size.Width - 1;
		if (pos_y > size.Height - 1)
			pos_y = size.Height - 1;

		// ----------------------------------------------------------------------
		/*
			RulerX         Cursor[0]
			 L M H           X
			 | | |           |
			-----------------+-------- Y: Cursor[1]
			 | | |           |
			 | | |           |
			-+-+-+-----------|-------- H: RulerY
			-+-+-+-----------|-------- M:
			-+-+-+-----------|-------- L:
		*/

		auto origin = TxPointD(eff.X, eff.Y + eff.Height);

		auto rulerL = TxPointD(origin.X +  25.0, origin.Y -  25.0);
		auto rulerM = TxPointD(origin.X +  75.0, origin.Y -  75.0);
		auto rulerH = TxPointD(origin.X + 125.0, origin.Y - 125.0);

		auto w_enough = (rulerH.X < (eff.X + eff.Width - 25.0));
		auto h_enough = (rulerH.Y > (eff.Y + 25.0));

		auto range = xie::Axi::CalcRange(size.Model.Type, size.Depth);
		auto range_diff = range.Upper - range.Lower;

		// ----------------------------------------------------------------------
		if (w_enough && h_enough && range_diff > 0)
		{
			auto st = canvas.DPtoIP(rulerM, ExGdiScalingMode::TopLeft);
			auto ed = canvas.DPtoIP(rulerH, ExGdiScalingMode::TopLeft);

			for(int ch=0 ; ch<size.Channels ; ch++)
			{
				for(int k=0 ; k<size.Model.Pack ; k++)
				{
					auto index = ch * size.Model.Pack + k;
					if (unpack)
					{
						if (index != channelNo) continue;
						index = 0;
					}

					CxArray profile_x(src->Height(), ModelOf<double>());
					CxArray profile_y(src->Width(), ModelOf<double>());
					CxOverlayProfile::GetProfileX(profile_x, *src, ch, k, pos_x);
					CxOverlayProfile::GetProfileY(profile_y, *src, ch, k, pos_y);
					values[index] = CxOverlayProfile::GetValue(*src, ch, k, pos_y, pos_x);

					CxArray* graph_x = xie::Axi::SafeCast<CxArray>(hgraphs_x[index]);
					CxArray* graph_y = xie::Axi::SafeCast<CxArray>(hgraphs_y[index]);
					if (graph_x == NULL)
						throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
					if (graph_y == NULL)
						throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
					graph_x->Resize(eff.Height, ModelOf<TxPointD>());
					graph_y->Resize(eff.Width, ModelOf<TxPointD>());

					auto src_x = profile_x.Scanner<double>();
					auto src_y = profile_y.Scanner<double>();
					auto dst_x = graph_x->Scanner<TxPointD>();
					auto dst_y = graph_y->Scanner<TxPointD>();

					for(int y=0 ; y<eff.Height ; y++)
					{
						auto ip = (int)(y / mag + vis.Y);
						auto dens = src_x[ip] / range_diff * (ed.X - st.X) + st.X;
						dst_x[y] = TxPointD(dens, ip);
					}

					for(int x=0 ; x<eff.Width ; x++)
					{
						auto ip = (int)(x / mag + vis.X);
						auto dens = src_y[ip] / range_diff * (ed.Y - st.Y) + st.Y;
						dst_y[x] = TxPointD(ip, dens);
					}
				}
			}
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
}
