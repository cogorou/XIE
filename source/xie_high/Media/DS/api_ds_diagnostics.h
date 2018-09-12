/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _API_DS_DIAGNOSTICS_H_INCLUDED_
#define _API_DS_DIAGNOSTICS_H_INCLUDED_

#include "api_ds.h"


// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace Media
{

// =================================================================
/*
	@brief	フィルタの列挙

	@tparam	TFUNC	コールバック関数の型

	@param[in]		type			メディア種別
	@param[in]		dir				メディア方向
	@param[in]		func			コールバック関数
*/
template<class TFUNC> void XIE_API fnPRV_DS_EnumFilters(ExMediaType type, ExMediaDir dir, TFUNC func)
{
	GUID category = fnPRV_DS_GetDeviceCategory(type, dir);
	HRESULT hr = S_OK;
	IEnumMoniker* enumerator = NULL;
	ICreateDevEnum* device = NULL;
	CxFinalizer com_finalizer([&enumerator,&device]()
		{
			if (enumerator != NULL)
				enumerator->Release();
			if (device != NULL)
				device->Release();
		});

	// ICreateDevEnum インターフェース取得.
	hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_ICreateDevEnum, (void **)&device);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	// EnumMonikerの作成.
	hr = device->CreateClassEnumerator(category, &enumerator, 0);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	// 列挙.
	if (enumerator != NULL)
	{
		int seqno = 0;
		while (true)
		{
			IBaseFilter*	filter = NULL;
			IMoniker*		moniker = NULL;
			ULONG			fetched = 0;

			CxFinalizer moniker_finalizer([&filter,&moniker]()
				{
					if (filter != NULL)
						filter->Release();
					if (moniker != NULL)
						moniker->Release();
				});

			if (enumerator->Next(1, &moniker, &fetched) != S_OK) break;
			hr = moniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&filter);
			if (SUCCEEDED(hr))
			{
				if (func(seqno, filter, moniker) == false) break;
				seqno++;
			}
		}
	}
}

// =================================================================
/*
	@brief	フィルタのピンの列挙

	@tparam	TFUNC	コールバック関数の型

	@param[in]		filter			対象のフィルタ
	@param[in]		func			コールバック関数
*/
template<class TFUNC> void XIE_API fnPRV_DS_EnumPins(IBaseFilter* filter, TFUNC func)
{
	HRESULT hr = S_OK;
	IEnumPins* enumpins = NULL;
	CxFinalizer com_finalizer([&enumpins]()
		{
			if (enumpins != NULL)
				enumpins->Release();
			enumpins = NULL;
		});

	hr = filter->EnumPins(&enumpins);
	if (FAILED(hr))
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	if (enumpins != NULL)
	{
		int index = 0;
		while (true)
		{
			IPin* pin = NULL;
			ULONG fetched = 0;
			PIN_INFO info;
			memset(&info, 0, sizeof(PIN_INFO));

			CxFinalizer pin_finalizer([&pin,&info]()
				{
					if (info.pFilter != NULL)
						info.pFilter->Release();
					if (pin != NULL)
						pin->Release();
				});

			if (enumpins->Next(1, &pin, &fetched) != S_OK) break;
			if (fetched == 0) break;

			pin->QueryPinInfo(&info);

			if (func(index, pin, info) == false) break;
			index++;
		}
	}
}

// =================================================================
template<class TFUNC> void XIE_API fnPRV_DS_EnumMediaTypes(IPin* pin, TFUNC func)
{
	HRESULT hr = S_OK;
	IEnumMediaTypes* enum_mts = NULL;
	CxFinalizer com_finalizer([&enum_mts]()
		{
			if (enum_mts != NULL)
				enum_mts->Release();
			enum_mts = NULL;
		});

	hr = pin->EnumMediaTypes(&enum_mts);
	if (FAILED(hr))
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	if (enum_mts != NULL)
	{
		int index = 0;
		while (true)
		{
			AM_MEDIA_TYPE* mt = NULL;
			ULONG fetched = 0;
			CxFinalizer mt_finalizer([&mt]()
				{
					// DeleteMediaType
					// https://msdn.microsoft.com/ja-jp/library/cc353852.aspx
					// FreeMediaType
					// https://msdn.microsoft.com/ja-jp/library/cc354534.aspx
					if (mt != NULL)
					{
						if (mt->cbFormat != 0 && 
							mt->cbFormat != NULL)
							CoTaskMemFree((PVOID)mt->pbFormat);
						mt->cbFormat = 0;
						mt->pbFormat = NULL;

						if (mt->pUnk != NULL)
							mt->pUnk->Release();
						mt->pUnk = NULL;
						CoTaskMemFree(mt);
					}
					mt = NULL;
				});

			if (enum_mts->Next(1, &mt, &fetched) != S_OK) break;
			if (fetched == 0) break;
			if (func(index, mt) == false) break;
			index++;
		}
	}
}

}	// Media
}	// xie

#endif

#endif	// _MCS_VER
