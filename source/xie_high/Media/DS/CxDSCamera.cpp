/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#include "Media/DS/CxDSCamera.h"
#include "Media/DS/CxDSGrabber.h"
#include "Media/api_media.h"
#include "Media/CxDeviceParam.h"
#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDSCamera";

// ============================================================
void CxDSCamera::_Constructor()
{
	m_Timeout = 5000;

	m_Graph = NULL;
	m_Builder = NULL;
	m_Video = NULL;
	m_VideoGrabber = NULL;
	m_VideoRenderer = NULL;
	m_Audio = NULL;
	m_AudioGrabber = NULL;
	m_AudioRenderer = NULL;
	m_FileSink = NULL;
	m_MediaControl = NULL;
	m_MediaEvent = NULL;
}

// ============================================================
CxDSCamera::CxDSCamera()
{
	_Constructor();
}

// ============================================================
CxDSCamera::~CxDSCamera()
{
	Dispose();
}

// ============================================================
void CxDSCamera::Dispose()
{
	if (m_MediaControl != NULL)
	{
		try
		{
			Stop();
		}
		catch(const CxException&)
		{
		}
	}

	m_VideoDeviceName.Dispose();
	m_VideoProductName.Dispose();
	m_AudioDeviceName.Dispose();
	m_AudioProductName.Dispose();
	m_OutputFileName.Dispose();
	m_OutputDeviceName.Dispose();
	m_OutputProductName.Dispose();

	if (m_MediaControl != NULL)
		m_MediaControl->Release();
	m_MediaControl = NULL;

	if (m_MediaEvent != NULL)
		m_MediaEvent->Release();
	m_MediaEvent = NULL;

	if (m_Video != NULL)
		m_Video->Release();
	m_Video = NULL;

	if (m_VideoGrabber != NULL)
		m_VideoGrabber->Release();
	m_VideoGrabber = NULL;

	if (m_VideoRenderer != NULL)
		m_VideoRenderer->Release();
	m_VideoRenderer = NULL;

	if (m_Audio != NULL)
		m_Audio->Release();
	m_Audio = NULL;

	if (m_AudioGrabber != NULL)
		m_AudioGrabber->Release();
	m_AudioGrabber = NULL;

	if (m_AudioRenderer != NULL)
		m_AudioRenderer->Release();
	m_AudioRenderer = NULL;

	if (m_FileSink != NULL)
		m_FileSink->Release();
	m_FileSink = NULL;

	if (m_Builder != NULL)
		m_Builder->Release();
	m_Builder = NULL;

	if (m_Graph != NULL)
		m_Graph->Release();
	m_Graph = NULL;
}

// ============================================================
bool CxDSCamera::IsValid() const
{
	if (m_Graph == NULL) return false;
	return true;
}

//
// Setup
//

// ============================================================
void CxDSCamera::Setup(HxModule hVideo, HxModule hAudio, HxModule hOutput)
{
	Dispose();

	// --------------------------------------------------
	// ビデオ入力.
	CxDeviceParam video;

	if (hVideo == NULL)
	{
		// default
	}
	else if (auto tmp = xie::Axi::SafeCast<CxDeviceParam>(hVideo))
	{
		video = *tmp;
	}
	else
	{
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	// --------------------------------------------------
	// オーディオ入力.
	CxDeviceParam audio;

	if (hAudio == NULL)
	{
		// ignore
	}
	else if (auto tmp = xie::Axi::SafeCast<CxDeviceParam>(hAudio))
	{
		audio = *tmp;
	}
	else
	{
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	// --------------------------------------------------
	// 出力先.
	CxStringW wszOutputFile;
	CxDeviceParam output;

	if (hOutput == NULL)
	{
		// ignore
	}
	else if (auto str = xie::Axi::SafeCast<xie::CxStringA>(hOutput))
	{
		wszOutputFile.CopyFrom(*str);
	}
	else if (auto str = xie::Axi::SafeCast<xie::CxStringW>(hOutput))
	{
		wszOutputFile.CopyFrom(*str);
	}
	else if (auto tmp = xie::Axi::SafeCast<CxDeviceParam>(hOutput))
	{
		output = *tmp;
	}
	else
	{
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	// --------------------------------------------------
	try
	{
		HRESULT hr = S_OK;

		// グラフ生成.
		CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void **)&m_Graph);
		if (m_Graph == NULL)
			throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// ビデオ入力フィルタ.
		{
			CxStringA device_name;
			CxStringA product_name;

			m_Video = fnPRV_DS_CreateDeviceFilter(ExMediaType::Video, ExMediaDir::Input, video.Name(), video.Index(), &device_name, &product_name);
			if (m_Video == NULL)
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			hr = m_Graph->AddFilter(m_Video, L"Video");
			if (FAILED(hr))
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			m_VideoDeviceName	= device_name;
			m_VideoProductName	= product_name;
		}

		// オーディオ入力フィルタ.
		if (audio.IsValid())
		{
			CxStringA device_name;
			CxStringA product_name;

			m_Audio = fnPRV_DS_CreateDeviceFilter(ExMediaType::Audio, ExMediaDir::Input, audio.Name(), audio.Index(), &device_name, &product_name);
			if (m_Audio == NULL)
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			hr = m_Graph->AddFilter(m_Audio, L"Audio");
			if (FAILED(hr))
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			m_AudioDeviceName = device_name;
			m_AudioProductName = product_name;
		}

		// ビデオグラバー.(require xie_ds)
		{
			hr = CoCreateInstance(xie::DS::CLSID_SampleGrabber, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_VideoGrabber);
			if (m_VideoGrabber == NULL)
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			hr = m_Graph->AddFilter(m_VideoGrabber, L"VideoGrabber");
			if (FAILED(hr))
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			xie::DS::ISampleGrabber* grabber = NULL;
			hr = m_VideoGrabber->QueryInterface(xie::DS::IID_SampleGrabber, (void**)&grabber);
			if (SUCCEEDED(hr))
			{
				AM_MEDIA_TYPE mt;
				memset(&mt, 0, sizeof(mt));
				mt.majortype	= MEDIATYPE_Video;
				mt.subtype		= MEDIASUBTYPE_RGB24;
				mt.formattype	= FORMAT_VideoInfo;
				hr = grabber->SetMediaType(&mt);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				grabber->SetBufferSamples(false);	// サンプルコピー 無効.
				grabber->SetOneShot(false);			// One Shot 無効.
				grabber->SetCallback(&m_VideoGrabberReceiver, 0);		// 0:SampleCB メソッドを呼び出すよう指示する.
				//grabber->SetCallback(&m_VideoGrabberReceiver, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
			}
		}

		// オーディオグラバー.(require xie_ds)
		{
			hr = CoCreateInstance(xie::DS::CLSID_SampleGrabber, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_AudioGrabber);
			if (m_AudioGrabber == NULL)
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			hr = m_Graph->AddFilter(m_AudioGrabber, L"AudioGrabber");
			if (FAILED(hr))
				throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			xie::DS::ISampleGrabber* grabber = NULL;
			hr = m_AudioGrabber->QueryInterface(xie::DS::IID_SampleGrabber, (void**)&grabber);
			if (SUCCEEDED(hr))
			{
				AM_MEDIA_TYPE mt;
				memset(&mt, 0, sizeof(mt));
				mt.majortype	= MEDIATYPE_Audio;
				mt.subtype		= MEDIASUBTYPE_PCM;
				mt.formattype	= FORMAT_WaveFormatEx;
				hr = grabber->SetMediaType(&mt);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				grabber->SetBufferSamples(false);	// サンプルコピー 無効.
				grabber->SetOneShot(false);			// One Shot 無効.
				grabber->SetCallback(&m_AudioGrabberReceiver, 0);		// 0:SampleCB メソッドを呼び出すよう指示する.
				//grabber->SetCallback(&m_AudioGrabberReceiver, 1);		// 1:BufferCB メソッドを呼び出すよう指示する.
			}
		}

		// ビデオ出力フォーマットの設定.
		// Width,Height に 0,0 が指定されている場合は既定値を使用する.
		// 指定されている場合は、VIDEOINFOHEADER を書き換えて SetFormat を行う.
		// ※) この処理は "ピンの接続" より前に行わなければならない.
		//
		if (video.Size().Width != 0 && video.Size().Height != 0)
			fnPRV_DS_SetFrameSize(m_Video, video.Size());

		// キャプチャビルダ生成.
		CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC, IID_ICaptureGraphBuilder2, (void **)&m_Builder);
		if (m_Builder == NULL)
			throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		m_Builder->SetFiltergraph(m_Graph);

		// --------------------------------------------------
		// 出力先.
		if (wszOutputFile.IsValid())
		{
			// ファイル名保管.
			m_OutputFileName.CopyFrom(wszOutputFile);

			if (wszOutputFile.EndsWith(L".avi", true))
			{
				// Capturing Video to an AVI File
				// https://msdn.microsoft.com/en-us/library/windows/desktop/dd318627(v=vs.85).aspx

				hr = m_Builder->SetOutputFileName(&MEDIASUBTYPE_Avi, wszOutputFile.Address(), &m_VideoRenderer, &m_FileSink);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
			else if (
				wszOutputFile.EndsWith(L".wmv", true) ||
				wszOutputFile.EndsWith(L".asf", true)
				)
			{
				// Capturing Video to a Windows Media File
				// https://msdn.microsoft.com/en-us/library/windows/desktop/dd318630(v=vs.85).aspx

				hr = m_Builder->SetOutputFileName(&MEDIASUBTYPE_Asf, wszOutputFile.Address(), &m_VideoRenderer, NULL);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				// Configure the ASF Writer filter.
				IConfigAsfWriter* pConfig = NULL;
				hr = m_VideoRenderer->QueryInterface(IID_IConfigAsfWriter, (void**)&pConfig);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				else
				{
					pConfig->Release();
					pConfig = NULL;
				}
			}
			else
			{
				throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		else
		{
			// ビデオレンダラー.
			{
				hr = CoCreateInstance(xie::DS::CLSID_NullRenderer, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_VideoRenderer);
				if (m_VideoRenderer == NULL)
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				hr = m_Graph->AddFilter(m_VideoRenderer, L"VideoRenderer");
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// オーディオレンダラー.
			if (output.IsValid())
			{
				CxStringA device_name;
				CxStringA product_name;

				m_AudioRenderer = fnPRV_DS_CreateDeviceFilter(ExMediaType::Audio, ExMediaDir::Output, output.Name(), output.Index(), &device_name, &product_name);
				if (m_AudioRenderer == NULL)
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				hr = m_Graph->AddFilter(m_AudioRenderer, L"AudioRenderer");
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				m_OutputDeviceName = device_name;
				m_OutputProductName = product_name;
			}
			else
			{
				hr = CoCreateInstance(xie::DS::CLSID_NullRenderer, NULL, CLSCTX_INPROC_SERVER, IID_IBaseFilter, (void**)&m_AudioRenderer);
				if (m_AudioRenderer == NULL)
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				hr = m_Graph->AddFilter(m_AudioRenderer, L"AudioRenderer");
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}

		// --------------------------------------------------
		// フィルタの接続.
		// IGraphBuilder.Connect の代わりに ICaptureGraphBuilder2.RenderStream を使用する.
		{
			/*
				VideoInput               
				+- Video - VideoGrabber -+- Renderer (avi,asf,wmv)
				                         |
				AudioInput               |
				+- Audio - AudioGrabber -+
			*/

			// Video
			{
				hr = m_Builder->RenderStream(NULL, &MEDIATYPE_Video, m_Video, m_VideoGrabber, m_VideoRenderer);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}

			// Audio
			IBaseFilter* audio_destination = (wszOutputFile.IsValid()) ? m_VideoRenderer : m_AudioRenderer;
			if (m_Audio != NULL)
			{
				hr = m_Builder->RenderStream(NULL, &MEDIATYPE_Audio, m_Audio, m_AudioGrabber, audio_destination);
				if (FAILED(hr))
					throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}

		// フレームサイズ.
		{
			// フレームサイズ取得.
			auto frame_size = fnPRV_DS_GetFrameSize(m_Video);

			// フレームサイズ設定. (WMV 形式ファイル保存用)
			fnPRV_DS_SetVideoFrameSize(m_VideoRenderer, frame_size);

			// フレームサイズ設定.
			m_VideoGrabberReceiver.FrameSize = TxImageSize(frame_size, ModelOf<TxBGR8x3>(), 1);
		}

		// インターフェース抽出.
		m_Graph->QueryInterface(IID_IMediaControl, (void **)&m_MediaControl);
		if (m_MediaControl == NULL)
			throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// インターフェース抽出.
		m_Graph->QueryInterface(IID_IMediaEvent, (void **)&m_MediaEvent);
		if (m_MediaEvent == NULL)
			throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	}
	catch(const xie::CxException& ex)
	{
#if 0
		// DEBUG: GRF ファイル保存 ( graphedt で確認する場合に使用します。)
		{
			TxDateTime dt = TxDateTime::Now(true);
			CxString grf = CxString::Format(
				"%s-%04d%02d%02d-%02d%02d%02d.GRF",
				g_ClassName,
				dt.Year, dt.Month, dt.Day,
				dt.Hour, dt.Minute, dt.Second
				);
			HRESULT hr = fnPRV_DS_SaveGraphFileA(m_Graph, grf);
		}
#endif
		Dispose();
		throw ex;
	}
}

// ============================================================
CxGrabber CxDSCamera::CreateGrabber(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return CxDSGrabber::From(&m_AudioGrabberReceiver);
	case ExMediaType::Video:
		return CxDSGrabber::From(&m_VideoGrabberReceiver);
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
/*
	フィルタのプロパティ ページの表示
	https://msdn.microsoft.com/ja-jp/library/Cc353961.aspx
*/
void CxDSCamera::OpenPropertyDialog(HWND hWnd, ExMediaType type, int mode, TxCharCPtrA caption)
{
	if (hWnd == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	HRESULT hr;
	ISpecifyPropertyPages* spp = NULL;
	IUnknown* pUnknown = NULL;
	CAUUID caGUID;
	memset(&caGUID, 0, sizeof(caGUID));
	CxFinalizer com_finalizer([&spp,&pUnknown,&caGUID]()
	{
		if (spp != NULL)
			spp->Release();
		if (pUnknown != NULL)
			pUnknown->Release();
		if (caGUID.pElems != NULL)
			CoTaskMemFree(caGUID.pElems); 
	});

	switch(type)
	{
	case ExMediaType::Audio:
		if (m_Audio == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		else
		{
			CxStringW caption_w;
			if (caption != NULL)
				caption_w.CopyFrom(CxStringA(caption));
			else
				caption_w.CopyFrom(m_AudioProductName);

			switch(mode)
			{
			case 0:
				{
					hr = m_Audio->QueryInterface<ISpecifyPropertyPages>(&spp);
					if (FAILED(hr))
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					hr = m_Audio->QueryInterface<IUnknown>(&pUnknown);
					if (FAILED(hr))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

					spp->GetPages(&caGUID);
					hr = OleCreatePropertyFrame(
						hWnd,					// 親ウィンドウ。
						0, 0,					// 予約済み。
						caption_w.Address(),	// ダイアログ ボックスのキャプション。
						1,						// オブジェクト数 (フィルタのみ)。
						&pUnknown,				// オブジェクト ポインタの配列。
						caGUID.cElems,			// プロパティ ページ数。
						caGUID.pElems,			// プロパティ ページ CLSID の配列。
						0,						// ロケール識別子。
						0, NULL					// 予約済み。
					);
					if (FAILED(hr))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				}
				return;
			default:
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		break;
	case ExMediaType::Video:
		if (m_Video == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		else
		{
			CxStringW caption_w;
			if (caption != NULL)
				caption_w.CopyFrom(CxStringA(caption));
			else
				caption_w.CopyFrom(m_VideoProductName);

			switch(mode)
			{
			case 0:
				{
					hr = m_Video->QueryInterface<ISpecifyPropertyPages>(&spp);
					if (FAILED(hr))
						throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
					hr = m_Video->QueryInterface<IUnknown>(&pUnknown);
					if (FAILED(hr))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

					spp->GetPages(&caGUID);
					hr = OleCreatePropertyFrame(
						hWnd,					// 親ウィンドウ。
						0, 0,					// 予約済み。
						caption_w.Address(),	// ダイアログ ボックスのキャプション。
						1,						// オブジェクト数 (フィルタのみ)。
						&pUnknown,				// オブジェクト ポインタの配列。
						caGUID.cElems,			// プロパティ ページ数。
						caGUID.pElems,			// プロパティ ページ CLSID の配列。
						0,						// ロケール識別子。
						0, NULL					// 予約済み。
					);
					if (FAILED(hr))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				}
				return;
			default:
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

//
// IxDSGraphBuilderProvider
//

// ============================================================
IGraphBuilder* CxDSCamera::GraphBuilder() const
{
	return m_Graph;
}

//
// IxMediaControl
//

// ============================================================
void CxDSCamera::Reset()
{
	// no job
}

// ============================================================
void CxDSCamera::Start()
{
	if (m_MediaControl == NULL)
		throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// 1) ストリーミングの開始.
	OAFilterState state = State_Stopped;
	HRESULT hr = m_MediaControl->GetState(0, &state);
	//if (FAILED(hr))
	//	throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	if (state != State_Running)
	{
		hr = m_MediaControl->Run();

		// ステータスが変化するまで待機する.
		state = State_Running;
		hr = m_MediaControl->GetState(m_Timeout, &state);
	}
}

// ============================================================
void CxDSCamera::Stop()
{
	if (m_MediaControl == NULL)
		throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// 1) ストリーミングの停止.
	m_MediaControl->Stop();

	// 2) 完了するまで待機する.
	WaitForCompletion(m_Timeout);
}

// ============================================================
void CxDSCamera::Abort()
{
	if (m_MediaControl == NULL)
		return;
	HRESULT hr = m_MediaControl->Stop();
}

// ============================================================
void CxDSCamera::Pause()
{
	if (m_MediaControl == NULL)
		throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// 1) 状態確認.
	OAFilterState state = State_Stopped;
	HRESULT hr = m_MediaControl->GetState(0, &state);
	//if (FAILED(hr))
	//	throw xie::CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	// 2) ストリーミングの一時停止.
	if (state == State_Running)
	{
		HRESULT hr = m_MediaControl->Pause();

		// ステータスが変化するまで待機する.
		OAFilterState state = State_Paused;
		hr = m_MediaControl->GetState(m_Timeout, &state);
	}
}

// ============================================================
bool CxDSCamera::Wait(int timeout) const
{
	xie::CxStopwatch watch;
	watch.Start();
	while(IsRunning())
	{
		watch.Stop();
		if (0 <= timeout && timeout <= watch.Elapsed)
			return false;
		xie::Axi::Sleep(1);
	}
	return true;
}

// ============================================================
bool CxDSCamera::IsRunning() const
{
	if (m_MediaControl == NULL) return false;

	OAFilterState state = State_Stopped;
	HRESULT hr = m_MediaControl->GetState(0, &state);
	if (FAILED(hr)) return false;

	return (state == State_Running || state == State_Paused);
}

// ============================================================
bool CxDSCamera::IsPaused() const
{
	if (m_MediaControl == NULL) return false;

	OAFilterState state = State_Stopped;
	HRESULT hr = m_MediaControl->GetState(0, &state);
	if (FAILED(hr)) return false;

	return (state == State_Paused);
}

//
// IxParam
//

// ============================================================
void CxDSCamera::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxDSCamera::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxDSCamera::GetFrameSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Video.Connected") == 0)
	{
		if (model == ModelOf<bool>())
		{
			*static_cast<bool*>(value) = Connected(ExMediaType::Video);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.Connected") == 0)
	{
		if (model == ModelOf<bool>())
		{
			*static_cast<bool*>(value) = Connected(ExMediaType::Audio);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Video.DeviceName") == 0 || strcmp(name, "DeviceName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_VideoDeviceName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_VideoDeviceName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Video.ProductName") == 0 || strcmp(name, "ProductName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_VideoProductName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_VideoProductName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.DeviceName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_AudioDeviceName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_AudioDeviceName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Audio.ProductName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_AudioProductName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_AudioProductName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.FileName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputFileName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputFileName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.DeviceName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputDeviceName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputDeviceName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "Output.ProductName") == 0)
	{
		if (auto dst = xie::Axi::SafeCast<CxStringA>((HxModule)value))
		{
			dst->CopyFrom(m_OutputProductName);
			return;
		}
		if (auto dst = xie::Axi::SafeCast<CxStringW>((HxModule)value))
		{
			dst->CopyFrom(m_OutputProductName);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxDSCamera::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxDSCamera::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "SaveGraphFile") == 0)
	{
		if (auto tmp = Axi::SafeCast<CxStringA>((HxModule)value))
		{
			fnPRV_DS_SaveGraphFileA(m_Graph, tmp->Address());
			return;
		}
		if (auto tmp = Axi::SafeCast<CxStringW>((HxModule)value))
		{
			fnPRV_DS_SaveGraphFileW(m_Graph, tmp->Address());
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
int CxDSCamera::Timeout() const
{
	return m_Timeout;
}

// ============================================================
void CxDSCamera::Timeout(int value)
{
	m_Timeout = value;
}

// ============================================================
TxImageSize CxDSCamera::GetFrameSize() const
{
	return m_VideoGrabberReceiver.FrameSize;
}

// ============================================================
bool CxDSCamera::Connected(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return fnPRV_DS_Connected(m_AudioGrabber);
	case ExMediaType::Video:
		return fnPRV_DS_Connected(m_VideoGrabber);
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
CxStringA CxDSCamera::GetDeviceName(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return m_AudioDeviceName;
	case ExMediaType::Video:
		return m_VideoDeviceName;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
CxStringA CxDSCamera::GetProductName(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return m_AudioProductName;
	case ExMediaType::Video:
		return m_VideoProductName;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

//
// Media Event
//

// ============================================================
bool CxDSCamera::WaitForCompletion(int timeout) const
{
	if (m_MediaEvent == NULL)
		throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// S_OK				: 成功.
	// E_ABORT			: タイムアウトによる時間切れ.
	// VFW_E_WRONG_STATE: フィルタ グラフが実行中ではない.

	long code = 0;
	HRESULT hr = m_MediaEvent->WaitForCompletion(timeout, &code);
	if (FAILED(hr))
		return false;

	switch(code)
	{
	case EC_COMPLETE:
	case EC_ERRORABORT:
	case EC_USERABORT:
		break;
	}
	return true;
}

}
}

#endif	// _MCS_VER
