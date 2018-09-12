/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLCamera.h"
#include "Media/V4L2/CxVLGrabber.h"
#include "Media/CxDeviceParam.h"
#include "Media/api_media.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <dirent.h>
#include <fcntl.h>		/* low-level i/o */
#include <errno.h>
#include <getopt.h>		/* getopt_long() */
#include <unistd.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <sys/time.h>
#include <sys/mman.h>
#include <sys/ioctl.h>
#include <linux/videodev2.h>

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxVLCamera";

// ============================================================
void CxVLCamera::_Constructor()
{
	m_Timeout = 5000;
	m_VideoProductName = "";
	// for Thread
	m_Thread.Notify	= CxThreadEvent(std::bind(&CxVLCamera::ThreadProc, this, std::placeholders::_1, std::placeholders::_2));
	m_Thread.Param	= this;
	m_Thread.Delay(10);
	// for V4L2
	m_FD = -1;
	m_VideoStream = false;
}

// ============================================================
CxVLCamera::CxVLCamera()
{
	_Constructor();
}

// ============================================================
CxVLCamera::~CxVLCamera()
{
	Dispose();
}

// ============================================================
void CxVLCamera::Dispose()
{
	try
	{
		Stop();
	}
	catch(const CxException&)
	{
	}

	// スレッド解放.
	m_Thread.Dispose();

	// メモリマップの解放.
	for(int i=0 ; i<m_Buffers.Length() ; i++)
		m_Buffers[i].Dispose();
	m_Buffers.Dispose();

	// デバイスの解放.
	if (m_FD != -1)
		close(m_FD);
	m_FD = -1;

	m_VideoDeviceName.Dispose();
	m_VideoProductName.Dispose();
	m_AudioDeviceName.Dispose();
	m_AudioProductName.Dispose();
	m_OutputFileName.Dispose();
	m_OutputDeviceName.Dispose();
	m_OutputProductName.Dispose();
}

// ============================================================
bool CxVLCamera::IsValid() const
{
	if (m_FD == -1) return false;
	if (m_Thread.IsValid() == false) return false;
	return true;
}

//
// Setup
//

// ============================================================
void CxVLCamera::Setup(HxModule hVideo, HxModule hAudio, HxModule hOutput)
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
	CxStringA aszOutputFile;
	CxDeviceParam output;

	if (hOutput == NULL)
	{
		// ignore
	}
	else if (auto str = xie::Axi::SafeCast<xie::CxStringA>(hOutput))
	{
		aszOutputFile.CopyFrom(*str);
	}
	else if (auto str = xie::Axi::SafeCast<xie::CxStringW>(hOutput))
	{
		aszOutputFile.CopyFrom(*str);
	}
	else if (auto tmp = xie::Axi::SafeCast<CxDeviceParam>(hOutput))
	{
		output = *tmp;
	}
	else
	{
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	// ※ Linux では音声入出力や動画ファイル保存は非対応:
	//    有効なパラメータが指定されていた場合は例外を発行してサポート対象外であることを通知する.
	if (audio.IsValid())
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	if (aszOutputFile.IsValid())
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	if (output.IsValid())
		throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		// デバイス名称.
		CxStringA device_name = video.Name();
		if (device_name.IsValid() == false)
		{
			auto names = fnPRV_VL_GetDeviceNames(ExMediaType::Video, ExMediaDir::Input);
			if (names.Length() == 0)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			device_name = names[video.Index()];
		}

		struct stat st;
		if (-1 == stat(device_name, &st))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		if (!S_ISCHR(st.st_mode))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// オープン.
		m_FD = open(device_name, O_RDWR|O_NONBLOCK, 0);
		if (-1 == m_FD)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		{
			int fd = m_FD;
			
			struct v4l2_capability	cap;
			struct v4l2_cropcap		cropcap;
			struct v4l2_crop		crop;
			struct v4l2_format		fmt;

			// 1) カメラデバイスが保有する機能の問い合わせ.
			if (-1 == xioctl(fd, VIDIOC_QUERYCAP, &cap))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			if (!(cap.capabilities & V4L2_CAP_VIDEO_CAPTURE))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			if (!(cap.capabilities & V4L2_CAP_STREAMING))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			// 2) ビデオ入力情報の設定.
			{
				memset(&cropcap, 0, sizeof(cropcap));
				memset(&fmt, 0, sizeof(fmt));
				
				cropcap.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
				fmt.type		= V4L2_BUF_TYPE_VIDEO_CAPTURE;

				// 2-1) cropcap
				if (0 == xioctl(fd, VIDIOC_CROPCAP, &cropcap))
				{
					crop.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
					crop.c		= cropcap.defrect; // reset to default

					if (-1 == xioctl(fd, VIDIOC_S_CROP, &crop))
					{
						switch (errno)
						{
						case EINVAL:	break;	// Cropping not supported.
						default:		break;	// Errors ignored.
						}
					}
				}
				else
				{
					// Errors ignored.
				}
				
				// 2-2) フォーマットの設定.
				{
					// 既定値.
					// Preserve original settings as set by v4l2-ctl for example
					if (-1 == xioctl(fd, VIDIOC_G_FMT, &fmt))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				}

				if (video.Size().Width != 0 && video.Size().Height != 0)
				{
					fmt.fmt.pix.width		= video.Size().Width;
					fmt.fmt.pix.height		= video.Size().Height;
					fmt.fmt.pix.pixelformat	= V4L2_PIX_FMT_YUYV;	// YUYV (16bit/pixel)
				//	fmt.fmt.pix.pixelformat	= V4L2_PIX_FMT_UYVY;
				//	fmt.fmt.pix.pixelformat	= V4L2_PIX_FMT_YVU420;
				//	fmt.fmt.pix.pixelformat	= V4L2_PIX_FMT_YUV411P;
				//	fmt.fmt.pix.pixelformat	= V4L2_PIX_FMT_BGR24;	// BGR8x3 (24bit/pixel)
					fmt.fmt.pix.field		= V4L2_FIELD_NONE;
				//	fmt.fmt.pix.field		= V4L2_FIELD_SEQ_TB;
				//	fmt.fmt.pix.field		= V4L2_FIELD_INTERLACED;

					// 指定値.
					if (-1 == xioctl(fd, VIDIOC_S_FMT, &fmt))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				}

				// 2-3) Buggy driver paranoia.
				{
					unsigned int	minval;
					
					minval = fmt.fmt.pix.width * 2;
					if (fmt.fmt.pix.bytesperline < minval)
						fmt.fmt.pix.bytesperline = minval;

					minval = fmt.fmt.pix.bytesperline * fmt.fmt.pix.height;
					if (fmt.fmt.pix.sizeimage < minval)
						fmt.fmt.pix.sizeimage = minval;
				}
			}

			// 3) initial mmap
			{
				struct v4l2_requestbuffers req;

				memset(&req, 0, sizeof(req));
				req.count	= 4;
				req.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
				req.memory	= V4L2_MEMORY_MMAP;

				// 3-1) 画像データ用のバッファの要求.
				if (-1 == xioctl(fd, VIDIOC_REQBUFS, &req))
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
				if (req.count < 2)
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

				// 3-2) メモリマップの生成.
				m_Buffers.Resize(req.count);
				for (int index=0; index<(int)req.count; index++)
				{
					struct v4l2_buffer buf;
					
					memset(&buf, 0, sizeof(buf));
					buf.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
					buf.memory	= V4L2_MEMORY_MMAP;
					buf.index	= index;

					// 前述 2-1 で要求したバッファ情報の受け取り.
					if (-1 == xioctl(fd, VIDIOC_QUERYBUF, &buf))
						throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

					m_Buffers[index].Resize(buf.length, buf.m.offset, fd);
				}
			}

			// 更新.
			m_VideoGrabberReceiver.FrameSize = TxImageSize(fmt.fmt.pix.width, fmt.fmt.pix.height, ModelOf<TxRGB8x4>(), 1);
			m_VideoDeviceName	= device_name;
			m_VideoProductName	= fnPRV_VL_GetProductName(ExMediaType::Video, ExMediaDir::Input, video.Name(), video.Index());
		}

		// スレッド生成.
		m_Thread.Setup();
	}
	catch(const xie::CxException& ex)
	{
		Dispose();
		throw ex;
	}
}

// ============================================================
CxGrabber CxVLCamera::CreateGrabber(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Video:
		return CxVLGrabber::From(&m_VideoGrabberReceiver);
	case ExMediaType::Audio:
		return CxVLGrabber::From(&m_AudioGrabberReceiver);
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
void CxVLCamera::OpenPropertyDialog(Window window, ExMediaType type, int mode, TxCharCPtrA caption)
{
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

//
// IxMediaControl
//

// ============================================================
void CxVLCamera::Reset()
{
	m_Thread.Reset();
}

// ============================================================
void CxVLCamera::Start()
{
	VideoStream(true);
	m_Thread.Start();
}

// ============================================================
void CxVLCamera::Stop()
{
	m_Thread.Stop();
	VideoStream(false);
}

// ============================================================
void CxVLCamera::Abort()
{
	m_Thread.Stop();

	try
	{
		VideoStream(false);
	}
	catch(const CxException&)
	{
	}
}

// ============================================================
void CxVLCamera::Pause()
{
	m_Thread.Stop();
}

// ============================================================
bool CxVLCamera::Wait(int timeout) const
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
bool CxVLCamera::IsRunning() const
{
	return (m_VideoStream);
}

// ============================================================
bool CxVLCamera::IsPaused() const
{
	return (m_VideoStream && !m_Thread.IsRunning());
}

//
// IxParam
//

// ============================================================
void CxVLCamera::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLCamera::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (strcmp(name, "FrameSize") == 0)
	{
		if (fnPRV_Media_GetParam(value, model, this, &CxVLCamera::GetFrameSize)) return;
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
void CxVLCamera::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Timeout") == 0)
	{
		if (fnPRV_Media_SetParam(value, model, this, &CxVLCamera::Timeout)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
int CxVLCamera::Timeout() const
{
	return m_Timeout;
}

// ============================================================
void CxVLCamera::Timeout(int value)
{
	m_Timeout = value;
}

// ============================================================
TxImageSize CxVLCamera::GetFrameSize() const
{
	return m_VideoGrabberReceiver.FrameSize;
}

// ============================================================
bool CxVLCamera::Connected(ExMediaType type) const
{
	switch(type)
	{
	case ExMediaType::Audio:
		return false;
	case ExMediaType::Video:
		return IsValid();
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
CxStringA CxVLCamera::GetDeviceName() const
{
	return m_VideoDeviceName;
}

// ============================================================
CxStringA CxVLCamera::GetProductName() const
{
	return m_VideoProductName;
}

// ============================================================
void CxVLCamera::ThreadProc(void* sender, CxThreadArgs* e)
{
	try
	{
		int fd = m_FD;
		struct v4l2_buffer buf;

		memset(&buf, 0, sizeof(buf));
		buf.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
		buf.memory	= V4L2_MEMORY_MMAP;

		// ストリーミングキューへの接続.
		if (-1 == xioctl(fd, VIDIOC_DQBUF, &buf))
		{
			switch (errno)
			{
			case EAGAIN:
				return;
			case EIO:
			default:
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}

		// サンプリングデータ.
		void*	sample_addr = m_Buffers[buf.index].Address();
		int		sample_size = buf.bytesused;

		// サンプリングデータが不正.
		if (sample_addr == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		if (sample_size != m_Buffers[buf.index].Length())
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// コールバック.
		m_VideoGrabberReceiver.Receive(e->Index, sample_addr, sample_size);
		m_AudioGrabberReceiver.Receive(e->Index, NULL, 0);

		// ストリーミングキューへの接続.
		if (-1 == xioctl(fd, VIDIOC_QBUF, &buf))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	}
	catch(const CxException& ex)
	{
		xie::Axi::Sleep(1);
	}
}

// ============================================================
void CxVLCamera::VideoStream(bool value)
{
	if (value)
	{
		int fd = m_FD;
		if (fd == -1)
			throw xie::CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (m_VideoStream) return;	// ignore

		// 1) ストリーミングキューへの接続.
		for (int i=0; i<m_Buffers.Length(); i++)
		{
			struct v4l2_buffer buf;

			memset(&buf,0, sizeof(buf));
			buf.type	= V4L2_BUF_TYPE_VIDEO_CAPTURE;
			buf.memory	= V4L2_MEMORY_MMAP;
			buf.index	= i;

			// ストリーミングキューへの接続.
			if (-1 == xioctl(fd, VIDIOC_QBUF, &buf))
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
	
		// 2) ストリーミングの開始.
		v4l2_buf_type	type = V4L2_BUF_TYPE_VIDEO_CAPTURE;
		if (-1 == xioctl(fd, VIDIOC_STREAMON, &type))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		m_VideoStream = true;
	}
	else
	{
		// 1) ストリーミングの停止.
		int fd = m_FD;
		if (fd == -1) return;	// ignore

		v4l2_buf_type	type = V4L2_BUF_TYPE_VIDEO_CAPTURE;
		if (-1 == xioctl(fd, VIDIOC_STREAMOFF, &type))
		{
			if (m_VideoStream)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		m_VideoStream = false;
	}
}

}
}

#endif	// _MCS_VER
