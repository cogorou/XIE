/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/DS/CxDSControlProperty.h"
#include "Media/DS/api_ds.h"
#include "Media/DS/api_ds_diagnostics.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/IxInternalModule.h"

namespace xie
{
namespace Media
{

// ============================================================
CxDSControlPropertyWrapper::CxDSControlPropertyWrapper()
{
	m_Controller	= NULL;
	m_Name			= NULL;
	CameraControl	= NULL;
	VideoProcAmp	= NULL;
	CameraControlID	= -1;
	VideoProcAmpID	= -1;
}

// ============================================================
CxDSControlPropertyWrapper::~CxDSControlPropertyWrapper()
{
	Dispose();
}

// =================================================================
CxDSControlPropertyWrapper& CxDSControlPropertyWrapper::operator = ( const CxDSControlPropertyWrapper& src )
{
	Controller(src.Controller());
	Name(src.Name());
	return *this;
}

// =================================================================
bool CxDSControlPropertyWrapper::operator == ( const CxDSControlPropertyWrapper& src ) const
{
	if (m_Controller	!= src.m_Controller) return false;
	if (m_Name			!= src.m_Name) return false;
	return true;
}

// =================================================================
bool CxDSControlPropertyWrapper::operator != ( const CxDSControlPropertyWrapper& src ) const
{
	return !(operator == (src));
}

// ============================================================
void CxDSControlPropertyWrapper::Dispose()
{
	m_Controller	= NULL;
	m_Name			= NULL;

	if (CameraControl != NULL)
		CameraControl->Release();
	CameraControl = NULL;

	if (VideoProcAmp != NULL)
		VideoProcAmp->Release();
	VideoProcAmp = NULL;

	CameraControlID	= -1;
	VideoProcAmpID	= -1;
}

// ============================================================
bool CxDSControlPropertyWrapper::IsValid() const
{
	if (m_Name.IsValid() == false) return false;
	if (CameraControlID < 0 && VideoProcAmpID < 0) return false;
	if (CameraControlID >= 0 && CameraControl == NULL) return false;
	if (VideoProcAmpID >= 0 && VideoProcAmp == NULL) return false;
	return true;
}

// ============================================================
HxModule CxDSControlPropertyWrapper::Controller() const
{
	return m_Controller;
}

// ============================================================
void CxDSControlPropertyWrapper::Controller(HxModule value)
{
	m_Controller = value;

	if (CameraControl != NULL)
		CameraControl->Release();
	CameraControl = NULL;

	if (VideoProcAmp != NULL)
		VideoProcAmp->Release();
	VideoProcAmp = NULL;

	auto graph = this->GraphBuilder();
	if (graph != NULL)
	{
		HRESULT hr;
		IEnumFilters* pEnum = NULL;
		hr = graph->EnumFilters(&pEnum);
		if (pEnum != NULL)
		{
			while(true)
			{
				IBaseFilter* filter = NULL;
				ULONG fetched = 0;
				hr = pEnum->Next(1, &filter, &fetched);
				if (hr != S_OK) break;

				if (CameraControl == NULL)
					hr = filter->QueryInterface(&CameraControl);
				if (VideoProcAmp == NULL)
					hr = filter->QueryInterface(&VideoProcAmp);

				if (CameraControl != NULL) break;
				if (VideoProcAmp != NULL) break;
			}
		}
	}
}

// ============================================================
TxCharCPtrA CxDSControlPropertyWrapper::Name() const
{
	return m_Name.Address();
}

// ============================================================
void CxDSControlPropertyWrapper::Name(TxCharCPtrA value)
{
	m_Name = value;

	CameraControlID = ToCameraControlProperty(value);
	VideoProcAmpID = ToVideoProcAmpProperty(value);
}

// ============================================================
bool CxDSControlPropertyWrapper::IsSupported() const
{
	if (CameraControlID >= 0)
	{
		if (CameraControl != NULL)
		{
			long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
			HRESULT hr = CameraControl->GetRange(CameraControlID, &minval, &maxval, &step, &defval, &flags);
			if (SUCCEEDED(hr))
				return true;
		}
		return false;
	}
	if (VideoProcAmpID >= 0)
	{
		if (VideoProcAmp != NULL)
		{
			long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
			HRESULT hr = VideoProcAmp->GetRange(VideoProcAmpID, &minval, &maxval, &step, &defval, &flags);
			if (SUCCEEDED(hr))
				return true;
		}
		return false;
	}
	return false;
}

// ============================================================
HRESULT CxDSControlPropertyWrapper::GetRange(long* minval, long* maxval, long* step, long* defval, long* flags) const
{
	if (CameraControlID >= 0)
		if (CameraControl != NULL)
			return CameraControl->GetRange(CameraControlID, minval, maxval, step, defval, flags);

	if (VideoProcAmpID >= 0)
		if (VideoProcAmp != NULL)
			return VideoProcAmp->GetRange(VideoProcAmpID, minval, maxval, step, defval, flags);

	return E_FAIL;
}

// ============================================================
HRESULT CxDSControlPropertyWrapper::Get(long* value, long* flags) const
{
	if (CameraControlID >= 0)
		if (CameraControl != NULL)
			return CameraControl->Get(CameraControlID, value, flags);

	if (VideoProcAmpID >= 0)
		if (VideoProcAmp != NULL)
			return VideoProcAmp->Get(VideoProcAmpID, value, flags);

	return E_FAIL;
}

// ============================================================
HRESULT CxDSControlPropertyWrapper::Set(long value, long flags)
{
	if (CameraControlID >= 0)
		if (CameraControl != NULL)
			return CameraControl->Set(CameraControlID, value, flags);

	if (VideoProcAmpID >= 0)
		if (VideoProcAmp != NULL)
			return VideoProcAmp->Set(VideoProcAmpID, value, flags);

	return E_FAIL;
}

// ============================================================
IGraphBuilder* CxDSControlPropertyWrapper::GraphBuilder() const
{
	if (auto provider = xie::Axi::SafeCast<xie::DS::IxDSGraphBuilderProvider>(m_Controller))
	{
		return provider->GraphBuilder();
	}
	if (auto controller = xie::Axi::SafeCast<IxInternalModule>(m_Controller))
	{
		auto module = controller->GetModule();
		auto provider = dynamic_cast<const xie::DS::IxDSGraphBuilderProvider*>(module);
		if (provider != NULL)
			return provider->GraphBuilder();
	}
	return NULL;
}

// ============================================================
int CxDSControlPropertyWrapper::ToCameraControlProperty(TxCharCPtrA name) const
{
	if (name == NULL) return -1;

	if (!strcmp(name, "Pan"))		{ return CameraControl_Pan; }
	if (!strcmp(name, "Tilt"))		{ return CameraControl_Tilt; }
	if (!strcmp(name, "Roll"))		{ return CameraControl_Roll; }
	if (!strcmp(name, "Zoom"))		{ return CameraControl_Zoom; }
	if (!strcmp(name, "Exposure"))	{ return CameraControl_Exposure; }
	if (!strcmp(name, "Iris"))		{ return CameraControl_Iris; }
	if (!strcmp(name, "Focus"))		{ return CameraControl_Focus; }
	return -1;
}

// ============================================================
int CxDSControlPropertyWrapper::ToVideoProcAmpProperty(TxCharCPtrA name) const
{
	if (name == NULL) return -1;

	if (!strcmp(name, "Brightness"))			{ return VideoProcAmp_Brightness; }
	if (!strcmp(name, "Contrast"))				{ return VideoProcAmp_Contrast; }
	if (!strcmp(name, "Hue"))					{ return VideoProcAmp_Hue; }
	if (!strcmp(name, "Saturation"))			{ return VideoProcAmp_Saturation; }
	if (!strcmp(name, "Sharpness"))				{ return VideoProcAmp_Sharpness; }
	if (!strcmp(name, "Gamma"))					{ return VideoProcAmp_Gamma; }
	if (!strcmp(name, "ColorEnable"))			{ return VideoProcAmp_ColorEnable; }
	if (!strcmp(name, "WhiteBalance"))			{ return VideoProcAmp_WhiteBalance; }
	if (!strcmp(name, "BacklightCompensation"))	{ return VideoProcAmp_BacklightCompensation; }
	if (!strcmp(name, "Gain"))					{ return VideoProcAmp_Gain; }
	return -1;
}

}
}
