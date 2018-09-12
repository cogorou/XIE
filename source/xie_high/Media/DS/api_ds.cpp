/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#include "api_ds.h"
#include "api_ds_diagnostics.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"
#include "Core/CxFinalizer.h"

#include <math.h>
#include <vector>
#include <map>
#include <unordered_map>

// ============================================================
namespace std
{

// unordered_map �� error C2338 ����������ׁA�L�[�Ɏg�p����\���̂̌^�ɂ����ꉻ���s��.
template<> struct hash<xie::TxSizeI> : public _Bitwise_hash<xie::TxSizeI> {};

}	// std

namespace xie
{
namespace Media
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;

// ======================================================================
void XIE_API fnPRV_DS_Setup()
{
	if (g_setuped) return;
	g_setuped = true;

	// COINIT_APARTMENTTHREADED
	// COINIT_MULTITHREADED
	HRESULT hr = CoInitializeEx(0, COINIT_MULTITHREADED|COINIT_APARTMENTTHREADED);
	fnXIE_Core_TraceOut(1, "%s(%d): CoInitializeEx=0x%08x\n", __FILE__, __LINE__, hr);
}

// ======================================================================
void XIE_API fnPRV_DS_TearDown()
{
	CoUninitialize();
}

// =================================================================
/*
	@brief	GUID ������̔�r

	@retval	true	��v
	@retval	false	�s��v
*/
bool XIE_API fnPRV_DS_CompareGuid(TxCharCPtrW src1, TxCharCPtrW src2)
{
	if (src1 == NULL && src2 == NULL) return true;
	if (src1 != NULL && src2 == NULL) return false;
	if (src1 == NULL && src2 != NULL) return false;

	int src1_pos = 0;
	int src2_pos = 0;
	int	src1_length = (int)wcslen(src1);
	int	src2_length = (int)wcslen(src2);

	if (src1_length > 0)
	{
		if (src1[src1_length - 1] == L'}')
		{
			src1_length--;
		}
		if (src1[0] == L'{')
		{
			src1_pos = 1;
			src1_length--;
		}
	}
	if (src2_length > 0)
	{
		if (src2[src2_length - 1] == L'}')
		{
			src2_length--;
		}
		if (src2[0] == L'{')
		{
			src2_pos = 1;
			src2_length--;
		}
	}

	if (src1_length != src2_length) return false;

	int result = _wcsnicmp(&src1[src1_pos], &src2[src2_pos], src1_length);

	return (result == 0);
}

// =================================================================
/*
	@brief	GUID �̕�����

	@return	GUID �𕶎���ɕϊ����ĕԂ��܂��B
*/
CxStringA XIE_API fnPRV_DS_ToString(GUID guid)
{
	unsigned char* d = guid.Data4;
	return CxStringA::Format("%08X-%04X-%04X-%02X%02X%02X%02X%02X%02X%02X%02X",
			guid.Data1,
			guid.Data2,
			guid.Data3,
			d[0], d[1], d[2], d[3],
			d[4], d[5], d[6], d[7]
		);
}

// =================================================================
/*
	@brief	�f�o�C�X�J�e�S�� (GUID) �̎擾

	@param[in]		type			���f�B�A���
	@param[in]		dir				���f�B�A����

	@return	�f�o�C�X�J�e�S�� (GUID)��Ԃ��܂��B
			�G���[���L��Η�O�𔭍s���܂��B
*/
GUID XIE_API fnPRV_DS_GetDeviceCategory(ExMediaType type, ExMediaDir dir)
{
	GUID category;
	switch(type)
	{
	case ExMediaType::Video:
		switch(dir)
		{
		case ExMediaDir::Input:
			category = CLSID_VideoInputDeviceCategory;
			break;
		case ExMediaDir::Output:
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		default:
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case ExMediaType::Audio:
		switch(dir)
		{
		case ExMediaDir::Input:
			category = CLSID_AudioInputDeviceCategory;
			break;
		case ExMediaDir::Output:
			category = CLSID_AudioRendererCategory;
			break;
		default:
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	return category;
}

// ======================================================================
/*
	@brief	�f�o�C�X�̌��̎擾

	@param[in]		type			���f�B�A���
	@param[in]		dir				���f�B�A����

	@return	�f�o�C�X�̌���Ԃ��܂��B
			������Ȃ����A�G���[���L��� 0 ��Ԃ��܂��B
*/
int XIE_API fnPRV_DS_GetDeviceCount(ExMediaType type, ExMediaDir dir)
{
	int count = 0;
	fnPRV_DS_EnumFilters(type, dir,
		[&count](int seqno, IBaseFilter* filter, IMoniker* moniker) -> bool
		{
			count++;
			return true;	// continue
		});
	return count;
}

// ======================================================================
/*
	@brief	�f�o�C�X���ꗗ�̎擾

	@param[in]		type			���f�B�A���
	@param[in]		dir				���f�B�A����

	@return	�f�o�C�X��(CLSID)�̈ꗗ��Ԃ��܂��B
*/
CxArrayEx<CxStringA> XIE_API fnPRV_DS_GetDeviceNames(ExMediaType type, ExMediaDir dir)
{
	std::vector<CxStringA>	result;

	fnPRV_DS_EnumFilters(type, dir,
		[&result](int seqno, IBaseFilter* filter, IMoniker* moniker) -> bool
		{
			CxStringW name_w;
			IPropertyBag* propbag = NULL;
			moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
			VARIANT tmp;
			VariantInit(&tmp);
			{
				HRESULT hr = propbag->Read(L"CLSID", &tmp, NULL);
				if (SUCCEEDED(hr))
					name_w = tmp.bstrVal;
			}
			VariantClear(&tmp);

			if (propbag != NULL)
				propbag->Release();
			propbag = NULL;

			xie::CxStringA name_a;
			name_a.CopyFrom(name_w);
			result.push_back(name_a);

			return true;	// continue
		});

	return result;
}

// ======================================================================
/*
	@brief	�f�o�C�X�̒ʂ��ԍ��̎擾

	@param[in]		type			���f�B�A���
	@param[in]		dir				���f�B�A����
	@param[in]		name			�f�o�C�X���� (�ȗ����� NULL �܂��� �󕶎�)
	@param[in]		index			�f�o�C�X�w�W [0~]

	@return	�f�o�C�X���Ǝw�W����ʂ��ԍ����擾���ĕԂ��܂��B
			������Ȃ����A�G���[���L��� -1 ��Ԃ��܂��B
*/
int XIE_API fnPRV_DS_GetDeviceIndex(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	const int error_result = -1;
	int result = error_result;

	if (index < 0)
		return error_result;

	CxStringW	src_name;
	if (name != NULL && strlen(name) > 0)
		src_name.CopyFrom(CxStringA(name));

	fnPRV_DS_EnumFilters(type, dir,
		[&result,&src_name,&index](int seqno, IBaseFilter* filter, IMoniker* moniker) -> bool
		{
			// �f�o�C�X�� (CLSID) �̊m�F.
			if (src_name.IsValid())
			{
				CxStringW dst_name;
				IPropertyBag* propbag = NULL;
				moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
				VARIANT tmp;
				VariantInit(&tmp);
				{
					HRESULT hr = propbag->Read(L"CLSID", &tmp, NULL);
					if (SUCCEEDED(hr))
						dst_name = tmp.bstrVal;
				}
				VariantClear(&tmp);

				if (propbag != NULL)
					propbag->Release();
				propbag = NULL;

				if (fnPRV_DS_CompareGuid(src_name.Address(), (TxCharCPtrW)dst_name) == false)
					return true;	// continue
			}

			// �w�W�̊m�F.
			if (index == 0)
			{
				result = seqno;		// ���ʊi�[.
				return false;		// found
			}
			index--;

			return true;			// continue
		});

	return result;
}

// ======================================================================
CxStringA XIE_API fnPRV_DS_GetProductName(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	CxStringA result;

	if (index < 0)
		return result;

	CxStringW	src_name;
	if (name != NULL && strlen(name) > 0)
		src_name.CopyFrom(CxStringA(name));

	fnPRV_DS_EnumFilters(type, dir,
		[&result,&src_name,&index](int seqno, IBaseFilter* filter, IMoniker* moniker) -> bool
		{
			// �f�o�C�X�� (CLSID) �̊m�F.
			if (src_name.IsValid())
			{
				CxStringW clsid;
				IPropertyBag* propbag = NULL;
				moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
				VARIANT tmp;
				VariantInit(&tmp);
				{
					HRESULT hr = propbag->Read(L"CLSID", &tmp, NULL);
					if (SUCCEEDED(hr))
						clsid = tmp.bstrVal;
				}
				VariantClear(&tmp);

				if (propbag != NULL)
					propbag->Release();
				propbag = NULL;

				if (fnPRV_DS_CompareGuid(src_name.Address(), (TxCharCPtrW)clsid) == false)
					return true;	// continue
			}

			// �w�W�̊m�F.
			if (index == 0)
			{
				CxStringW nickname;
				IPropertyBag* propbag = NULL;
				moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
				VARIANT tmp;
				VariantInit(&tmp);
				{
					HRESULT hr = propbag->Read(L"FriendlyName", &tmp, NULL);
					if (SUCCEEDED(hr))
						nickname = tmp.bstrVal;
				}
				VariantClear(&tmp);

				if (propbag != NULL)
					propbag->Release();
				propbag = NULL;

				result.CopyFrom(nickname);	// ���ʊi�[.
				return false;				// found
			}
			index--;

			return true;			// continue
		});

	return result;
}

// ======================================================================
CxArrayEx<CxStringA>	XIE_API fnPRV_DS_GetPinNames(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	std::vector<CxStringA> result;

	HRESULT hr = S_OK;
	IGraphBuilder*	pGraph = NULL;	// This must be released.
	IBaseFilter*	pSource = NULL;	// This must be released.
	CxFinalizer com_finalizer([&pGraph,&pSource]()
		{
			if (pSource != NULL)
				pSource->Release();
			if (pGraph != NULL)
				pGraph->Release();
		});

	// �O���t����.
	CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);
	if (pGraph == NULL)
		return result;

	// ���̓t�B���^�ǉ�.
	{
		pSource = fnPRV_DS_CreateDeviceFilter(type, dir, name, index, NULL, NULL);
		if (pSource == NULL)
			return result;
		hr = pGraph->AddFilter(pSource, L"Source");
		if (FAILED(hr))
			return result;
	}

	// �s���ꗗ�̎擾.
	fnPRV_DS_EnumPins(pSource,
		[&result](int no, IPin* pin, PIN_INFO info) -> bool
		{
			if (info.dir == PINDIR_OUTPUT)
			{
				CxStringA name;
				name.CopyFrom(CxStringW(info.achName));
				result.push_back(name);
			}
			return true;
		});

	return result;
}

// ======================================================================
CxArrayEx<TxSizeI> XIE_API fnPRV_DS_GetFrameSizes(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, int pin)
{
	CxArrayEx<TxSizeI> result;

	HRESULT hr = S_OK;
	IGraphBuilder*	pGraph = NULL;		// This must be released.
	IBaseFilter*	pCapture = NULL;	// This must be released.
	CxFinalizer com_finalizer([&pGraph,&pCapture]()
		{
			if (pCapture != NULL)
				pCapture->Release();
			if (pGraph != NULL)
				pGraph->Release();
		});

	// NOTE: For now, Support video input only.
	if (!(type == ExMediaType::Video && dir == ExMediaDir::Input))
		return result;	// unsupported

	// �O���t����.
	CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&pGraph);
	if (pGraph == NULL)
		return result;

	// �r�f�I���̓t�B���^�ǉ�.
	{
		pCapture = fnPRV_DS_CreateDeviceFilter(type, dir, name, index, NULL, NULL);
		if (pCapture == NULL)
			return result;
		hr = pGraph->AddFilter(pCapture, L"Video");
		if (FAILED(hr))
			return result;
	}

	// �T�C�Y�ꗗ�̎擾.
	xie::CxArrayEx<TxSizeI> sizes = fnPRV_DS_GetFrameSizes(pCapture);
	{
		// ����T�C�Y�̓}�[�W����.
		std::unordered_map<TxSizeI,TxSizeI> sizes_map;
		for(int s=0 ; s<sizes.Length() ; s++)
			sizes_map[sizes[s]] = sizes[s];

		// ���ʂ��i�[����.
		// �� ��Q������ true �́AMemoryAlloc �Ŋm�ۂ��邱�Ƃ��Ӗ����܂��B
		//    ������� MemoryFree ���g�p���܂��B
		//    false ���w�肵���ꍇ�� new/delete ���g�p����܂��B
		//    ���̏ꍇ�A�m�ۂƉ������ CRT ����v���Ȃ��\��������댯�ł��B
		//
		result.Resize((int)sizes_map.size(), true);
		int s = 0;
		for(auto iter=sizes_map.begin() ; iter!=sizes_map.end() ; iter++, s++)
			result[s] = iter->second;
	}

	return result;
}

// =================================================================
/*
	@brief	�w�肳�ꂽ�f�o�C�X�t�B���^�𐶐����܂��B

	seealso:
	https://msdn.microsoft.com/en-us/library/windows/desktop/dd407292(v=vs.85).aspx

	@param[in]		type			���f�B�A���
	@param[in]		dir				���f�B�A����
	@param[in]		name			�f�o�C�X���� (�ȗ����� NULL �܂��� �󕶎�)
	@param[in]		index			�f�o�C�X�w�W [0~]
	@param[out]		device_name		�f�o�C�X�� (CLSID) (�ȗ����� NULL)
	@param[out]		product_name	���i�� (�ȗ����� NULL)

	@return	���������f�o�C�X�t�B���^��Ԃ��܂��B
			������Ȃ���� NULL ��Ԃ��܂��B

	@exception	CxException	�����Ɏw�肳�ꂽ type �� dir �������ȏꍇ�ɔ��s����܂��B
*/
IBaseFilter* XIE_API fnPRV_DS_CreateDeviceFilter(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, CxStringA* device_name, CxStringA* product_name)
{
	IBaseFilter*	result = NULL;

	if (index < 0)
		return result;

	CxStringW	src_name;
	if (name != NULL && strlen(name) > 0)
		src_name.CopyFrom(CxStringA(name));

	GUID category = fnPRV_DS_GetDeviceCategory(type, dir);

	HRESULT hr = S_OK;
	IEnumMoniker*	enumerator = NULL;	// This must be released.
	ICreateDevEnum*	device = NULL;		// This must be released.
	CxFinalizer com_finalizer([&enumerator,&device]()
		{
			if (enumerator != NULL)
				enumerator->Release();
			if (device != NULL)
				device->Release();
		});

	// ICreateDevEnum �C���^�[�t�F�[�X�擾.
	hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_ICreateDevEnum, (void **)&device);
	if (FAILED(hr))
		return result;

	// EnumMoniker�̍쐬.
	hr = device->CreateClassEnumerator(category, &enumerator, 0);
	if (FAILED(hr))
		return result;

	// ��.
	while (true)
	{
		bool found = false;
		IMoniker*	moniker = NULL;
		ULONG		fetched = 0;

		CxFinalizer moniker_finalizer([&moniker,&found,&result]()
			{
				if (moniker != NULL)
					moniker->Release();

				if (found == false)
				{
					if (result != NULL)
						result->Release();
					result = NULL;
				}
			});

		if (enumerator->Next(1, &moniker, &fetched) != S_OK) break;
		hr = moniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&result);
		if (SUCCEEDED(hr))
		{
			CxStringW clsid;

			// �f�o�C�X�� (CLSID) �̎擾.
			{
				IPropertyBag* propbag = NULL;
				moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
				VARIANT tmp;
				VariantInit(&tmp);
				{
					hr = propbag->Read(L"CLSID", &tmp, NULL);
					if (SUCCEEDED(hr))
						clsid = tmp.bstrVal;
				}
				VariantClear(&tmp);

				if (propbag != NULL)
					propbag->Release();
				propbag = NULL;
			}

			// �f�o�C�X�� (CLSID) �̊m�F.
			if (src_name.IsValid())
			{
				if (fnPRV_DS_CompareGuid(src_name.Address(), (TxCharCPtrW)clsid) == false)
					continue;
			}

			// �w�W�̊m�F.
			if (index == 0)
			{
				found = true;

				// �f�o�C�X�� (CLSID) �̎擾.
				if (device_name != NULL)
				{
					device_name->CopyFrom(clsid);
				}

				// ���i���̎擾.
				if (product_name != NULL)
				{
					CxStringW nickname;
					IPropertyBag* propbag = NULL;
					moniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void**)&propbag);
					VARIANT tmp;
					VariantInit(&tmp);
					{
						HRESULT hr = propbag->Read(L"FriendlyName", &tmp, NULL);
						if (SUCCEEDED(hr))
							nickname = tmp.bstrVal;
					}
					VariantClear(&tmp);

					if (propbag != NULL)
						propbag->Release();
					propbag = NULL;

					// ���ʊi�[.
					product_name->CopyFrom(nickname);
				}
				break;
			}
			index--;
		}
	}

	return result;
}

// =================================================================
IPin* XIE_API fnPRV_DS_FindPin(IBaseFilter* filter, int index, PIN_DIRECTION direction)
{
	if (filter == NULL) return NULL;

	HRESULT hr = S_OK;
	IPin* pin = NULL;
	IEnumPins* enumpins = NULL;
	CxFinalizer com_finalizer([&enumpins]()
		{
			if (enumpins != NULL)
				enumpins->Release();
			enumpins = NULL;
		});

	hr = filter->EnumPins(&enumpins);
	if (FAILED(hr))
		return pin;

	while (true)
	{
		ULONG fetched = 0;
		if (enumpins->Next(1, &pin, &fetched) != S_OK) break;
		if (fetched == 0) break;

		PIN_INFO info;
		pin->QueryPinInfo(&info);
		if (info.dir == direction)
		{
			if (index <= 0)
				break;
			index--;
		}

		if (info.pFilter != NULL)
			info.pFilter->Release();

		if (pin != NULL)
			pin->Release();
		pin = NULL;
	}

	return pin;
}

// =================================================================
void XIE_API fnPRV_DS_SetFrameSize(IBaseFilter* filter, TxSizeI frame_size)
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
		if (info.dir == PINDIR_OUTPUT)
		{
			IAMStreamConfig* pConfig = NULL;
			CxFinalizer cfg_finalizer([&pConfig]()
				{
					if (pConfig != NULL)
						pConfig->Release();
				});

			hr = pin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
			if (SUCCEEDED(hr))
			{
				int count = 0;
				int size = 0;
				hr = pConfig->GetNumberOfCapabilities(&count, &size);

				// �T�C�Y�𒲂ׁA�������\���̂�n�����Ƃ��m�F����.
				if (size == sizeof(VIDEO_STREAM_CONFIG_CAPS))
				{
					// �r�f�I�\�͍\���̂��g��.
					for (int iFormat = 0; iFormat < count; iFormat++)
					{
						VIDEO_STREAM_CONFIG_CAPS scc;
						AM_MEDIA_TYPE* media_type = NULL;
						CxFinalizer mt_finalizer([&media_type]()
							{
								if (media_type != NULL)
								{
									if (media_type->cbFormat != 0 &&
										media_type->pbFormat != NULL)
										CoTaskMemFree(media_type->pbFormat);
									media_type->cbFormat = 0;
									media_type->pbFormat = NULL;

									if (media_type->pUnk != NULL)
										media_type->pUnk->Release();
									media_type->pUnk = NULL;
									CoTaskMemFree(media_type);
								}
							});
						hr = pConfig->GetStreamCaps(iFormat, &media_type, (BYTE*)&scc);
						if (SUCCEEDED(hr))
						{
							if ((media_type->majortype == MEDIATYPE_Video) &&
							//	(media_type->subtype == MEDIASUBTYPE_RGB24) &&
								(media_type->formattype == FORMAT_VideoInfo) &&
								(media_type->cbFormat >= sizeof(VIDEOINFOHEADER)) &&
								(media_type->pbFormat != NULL))
							{
								VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)media_type->pbFormat;
								if (frame_size.Width  == pVih->bmiHeader.biWidth &&
									frame_size.Height == pVih->bmiHeader.biHeight)
								{
									hr = pConfig->SetFormat(media_type);
									if (SUCCEEDED(hr))
										return;
								}
							}
						}
					}
				}
			}
		}
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
TxSizeI XIE_API fnPRV_DS_GetFrameSize(IBaseFilter* filter)
{
	TxSizeI	result = TxSizeI(0, 0);
	if (filter == NULL)
		return result;

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
		if (info.dir == PINDIR_OUTPUT)
		{
			IAMStreamConfig* pConfig = NULL;
			CxFinalizer cfg_finalizer([&pConfig]()
				{
					if (pConfig != NULL)
						pConfig->Release();
				});

			hr = pin->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
			if (SUCCEEDED(hr))
			{
				AM_MEDIA_TYPE* media_type = NULL;
				CxFinalizer mt_finalizer([&media_type]()
					{
						if (media_type != NULL)
						{
							if (media_type->cbFormat != 0 &&
								media_type->pbFormat != NULL)
								CoTaskMemFree(media_type->pbFormat);
							media_type->cbFormat = 0;
							media_type->pbFormat = NULL;

							if (media_type->pUnk != NULL)
								media_type->pUnk->Release();
							media_type->pUnk = NULL;
							CoTaskMemFree(media_type);
						}
					});

				hr = pConfig->GetFormat(&media_type);
				if (SUCCEEDED(hr))
				{
					if (media_type->pbFormat != NULL)
					{
						VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)media_type->pbFormat;
						result.Width  = pVih->bmiHeader.biWidth;
						result.Height = pVih->bmiHeader.biHeight;
						return result;
					}
				}
			}
		}
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
CxArrayEx<TxSizeI> XIE_API fnPRV_DS_GetFrameSizes(IBaseFilter* filter)
{
	std::vector<TxSizeI> result;

	// �o�̓s��.
	IPin* filter_out = fnPRV_DS_FindPin(filter, 0, PINDIR_OUTPUT);
	if (filter_out != NULL)
	{
		CxFinalizer com_finalizer([&filter_out]()
			{
				if (filter_out != NULL)
					filter_out->Release();
			});

		HRESULT hr = S_OK;
		IAMStreamConfig* pConfig = NULL;
		hr = filter_out->QueryInterface(IID_IAMStreamConfig, (void**)&pConfig);
		if (SUCCEEDED(hr))
		{
			int count = 0;
			int size = 0;
			hr = pConfig->GetNumberOfCapabilities(&count, &size);

			// �T�C�Y�𒲂ׁA�������\���̂�n�����Ƃ��m�F����.
			if (size == sizeof(VIDEO_STREAM_CONFIG_CAPS))
			{
				// �r�f�I�\�͍\���̂��g��.
				for (int iFormat = 0; iFormat < count; iFormat++)
				{
					VIDEO_STREAM_CONFIG_CAPS scc;
					AM_MEDIA_TYPE* media_type = NULL;
					CxFinalizer mt_finalizer([&media_type]()
						{
							// ����������A���f�B�A�^�C�v���폜����.
							if (media_type != NULL)
							{
								if (media_type->cbFormat != 0 &&
									media_type->pbFormat != NULL)
									CoTaskMemFree(media_type->pbFormat);
								media_type->cbFormat = 0;
								media_type->pbFormat = NULL;

								if (media_type->pUnk != NULL)
									media_type->pUnk->Release();
								media_type->pUnk = NULL;
								CoTaskMemFree(media_type);
							}
						});

					hr = pConfig->GetStreamCaps(iFormat, &media_type, (BYTE*)&scc);
					if (SUCCEEDED(hr))
					{
						if ((media_type->majortype == MEDIATYPE_Video) &&
						//	(media_type->subtype == MEDIASUBTYPE_RGB24) &&
							(media_type->formattype == FORMAT_VideoInfo) &&
							(media_type->cbFormat >= sizeof(VIDEOINFOHEADER)) &&
							(media_type->pbFormat != NULL))
						{
							// --- �r�f�I���̓T�C�Y.
							VIDEOINFOHEADER* pVih = (VIDEOINFOHEADER*)media_type->pbFormat;
							result.push_back(
								TxSizeI(
									pVih->bmiHeader.biWidth,
									pVih->bmiHeader.biHeight
								));
						}
					}
				}
			}
			pConfig->Release();
		}
	}

	return result;
}

// ======================================================================
/*
	@brief	WMV �`���t�@�C���ۑ��p: �r�f�I�̃t���[���T�C�Y��ݒ肵�܂��B

	@param[in]	mux			���݂� ASF Writer �ɂ̂ݑΉ����Ă��܂��B���͖������܂��B
	@param[in]	frame_size	�ݒ�l
*/
void XIE_API fnPRV_DS_SetVideoFrameSize(IBaseFilter* mux, TxSizeI frame_size)
{
	if (mux == NULL) return;	// ignore

	HRESULT hr;
	IConfigAsfWriter* config = NULL;
	CxFinalizer config_finalizer([&config]()
		{
			if (config != NULL)
				config->Release();
		});
	hr = mux->QueryInterface<IConfigAsfWriter>(&config);
	if (FAILED(hr)) return;		// ignore

	IWMProfile* profile = NULL;
	CxFinalizer profile_finalizer([&profile]()
		{
			if (profile != NULL)
				profile->Release();
		});

	// ���݂̃v���t�@�C�����擾���܂�.
	hr = config->GetCurrentProfile(&profile);
	if (FAILED(hr))
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// �X�g���[���̌����擾���܂�.
	DWORD stream_num = 0;
	hr = profile->GetStreamCount(&stream_num);
	if (FAILED(hr))
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// �f�����͂̃X�g���[����T�����ăt���[���T�C�Y��ݒ肵�܂�.
	for (DWORD index = 0; index < stream_num; index++)
	{
		// �X�g���[�����擾���܂�.
		IWMStreamConfig* stream_config = NULL;
		hr = profile->GetStream(index, &stream_config);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// �X�g���[���̃��f�B�A�^�C�v���m�F���܂�.
		GUID stream_type;
		hr = stream_config->GetStreamType(&stream_type);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// �f���ȊO�͖������܂�.
		if (stream_type != MEDIATYPE_Video)
			continue;

		// ���f�B�A�^�C�v�����i�[���邽�߂ɕK�v�ȃT�C�Y (bytes) ���擾���܂�.
		IWMMediaProps* props = NULL;
		CxFinalizer props_finalizer([&props]()
			{
				if (props != NULL)
					props->Release();
			});
		hr = stream_config->QueryInterface<IWMMediaProps>(&props);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		DWORD cbType = 0;
		hr = props->GetMediaType(NULL, &cbType);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// ���f�B�A�^�C�v�����i�[����̈���m�ۂ��܂�.
		CxArray buffer((int)cbType, TxModel::U8(1));
		auto wmt = reinterpret_cast<WM_MEDIA_TYPE*>(buffer.Address());

		// ���f�B�A�^�C�v�����擾���܂�.
		hr = props->GetMediaType(wmt, &cbType);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// �t���[���T�C�Y���w�肳�ꂽ�l�ɏ����ւ��܂�.
		auto vih = reinterpret_cast<VIDEOINFOHEADER*>(wmt->pbFormat);
		vih->bmiHeader.biWidth = frame_size.Width;		// ��.
		vih->bmiHeader.biHeight = frame_size.Height;	// ����.

		// ���f�B�A�^�C�v��ݒ肵�܂�.
		hr = props->SetMediaType(wmt);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// �X�g���[�����č\�����܂�.
		hr = profile->ReconfigStream(stream_config);
		if (FAILED(hr))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	}

	// �v���t�@�C�����č\�����܂�.
	hr = config->ConfigureFilterUsingProfile(profile);
	if (FAILED(hr))
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
}

// =================================================================
VIDEOINFOHEADER XIE_API fnPRV_DS_GetVideoInfo(IBaseFilter* grabber)
{
	VIDEOINFOHEADER vih;
	memset(&vih, 0, sizeof(vih));

	HRESULT hr;
	xie::DS::ISampleGrabber* isg = NULL;
	CxFinalizer config_finalizer([&isg]()
		{
			if (isg != NULL)
				isg->Release();
		});
	hr = grabber->QueryInterface<xie::DS::ISampleGrabber>(&isg);
	if (SUCCEEDED(hr))
	{
		AM_MEDIA_TYPE mt;
		hr = isg->GetConnectedMediaType(&mt);
		if (SUCCEEDED(hr))
			vih = *(VIDEOINFOHEADER*)mt.pbFormat;
	}
	return vih;
}

// =================================================================
bool XIE_API fnPRV_DS_Connected(IBaseFilter* grabber)
{
	HRESULT hr;
	xie::DS::ISampleGrabber* isg = NULL;
	CxFinalizer config_finalizer([&isg]()
		{
			if (isg != NULL)
				isg->Release();
		});
	hr = grabber->QueryInterface<xie::DS::ISampleGrabber>(&isg);
	if (SUCCEEDED(hr))
	{
		AM_MEDIA_TYPE mt;
		hr = isg->GetConnectedMediaType(&mt);
		if (SUCCEEDED(hr))
		{
			return true;
		}
	}
	return false;
}

// ======================================================================
/*
	@brief	GraphEdit �t�@�C���ւ̃t�B���^ �O���t�̕ۑ� (ASCII)
*/
HRESULT XIE_API fnPRV_DS_SaveGraphFileA(IGraphBuilder *pGraph, TxCharCPtrA szPath) 
{
	CxStringA astrPath = szPath;
	CxStringW wstrPath;
	wstrPath.CopyFrom(astrPath);

	return fnPRV_DS_SaveGraphFileW(pGraph, wstrPath.Address());
}

// ======================================================================
/*
	@brief	GraphEdit �t�@�C���ւ̃t�B���^ �O���t�̕ۑ� (UNICODE)

	https://msdn.microsoft.com/ja-jp/library/Cc370617.aspx
*/
HRESULT XIE_API fnPRV_DS_SaveGraphFileW(IGraphBuilder *pGraph, TxCharCPtrW szPath)
{
	HRESULT hr;
	
	CxStringW wstrPath = szPath;

	IStorage *pStorage = NULL;
	hr = StgCreateDocfile(
		wstrPath.Address(),
		STGM_CREATE | STGM_TRANSACTED | STGM_READWRITE | STGM_SHARE_EXCLUSIVE,
		0, &pStorage);
	if(FAILED(hr)) 
	{
		return hr;
	}

	const WCHAR wszStreamName[] = L"ActiveMovieGraph"; 
	IStream *pStream = NULL;
	hr = pStorage->CreateStream(
		wszStreamName,
		STGM_WRITE | STGM_CREATE | STGM_SHARE_EXCLUSIVE,
		0, 0, &pStream);
	if (FAILED(hr)) 
	{
		pStorage->Release();	
		return hr;
	}

	IPersistStream *pPersist = NULL;
	pGraph->QueryInterface(IID_IPersistStream, (void**)&pPersist);
	hr = pPersist->Save(pStream, TRUE);
	pStream->Release();
	pPersist->Release();
	if (SUCCEEDED(hr)) 
	{
		hr = pStorage->Commit(STGC_DEFAULT);
	}
	pStorage->Release();
	return hr;
}

}	// Media
}	// xie

#endif	// _MCS_VER
