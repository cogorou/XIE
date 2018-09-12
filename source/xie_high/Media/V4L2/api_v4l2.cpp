/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "api_v4l2.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/CxString.h"
#include "Core/CxFinalizer.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <dirent.h>
#include <assert.h>
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

#include <math.h>
#include <vector>
#include <map>
#include <unordered_map>
#include <string>

namespace xie
{
namespace Media
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;

// ======================================================================
void XIE_API fnPRV_VL_Setup()
{
	if (g_setuped) return;
	g_setuped = true;
}

// ======================================================================
void XIE_API fnPRV_VL_TearDown()
{
}

// ======================================================================
int XIE_API fnPRV_VL_GetDeviceCount(ExMediaType type, ExMediaDir dir)
{
	int count = 0;

	switch(type)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Audio:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Video:
		if (dir == ExMediaDir::Input)
		{
			struct dirent** namelist = NULL;
			int entries = scandir("/dev", &namelist, 0, alphasort);
			CxFinalizer namelist_finalizer([&namelist,entries]()
				{
					for(int i=0 ; i<entries ; i++)
						free(namelist[i]);
					if (namelist != NULL)
						free(namelist);
				});
			for(int i=0 ; i<entries ; i++)
			{
				const char* filename = namelist[i]->d_name;
				if (strncmp(filename, "video", 5) == 0)
					count++;
			}
		}
		else
		{
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	}

	return count;
}

// ======================================================================
CxArrayEx<CxStringA> XIE_API fnPRV_VL_GetDeviceNames(ExMediaType type, ExMediaDir dir)
{
	CxArrayEx<CxStringA>		result;

	switch(type)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Audio:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Video:
		if (dir == ExMediaDir::Input)
		{
			int count = fnPRV_VL_GetDeviceCount(type, dir);
			result.Resize(count);

			if (count > 0)
			{
				int index = 0;
				struct dirent** namelist = NULL;
				int entries = scandir("/dev", &namelist, 0, alphasort);
				CxFinalizer namelist_finalizer([&namelist,entries]()
					{
						for(int i=0 ; i<entries ; i++)
							free(namelist[i]);
						if (namelist != NULL)
							free(namelist);
					});
				for(int i=0 ; i<entries ; i++)
				{
					const char* filename = namelist[i]->d_name;
					if (strncmp(filename, "video", 5) == 0)
					{
						result[index] = CxStringA::Format("/dev/%s", filename);
						index++;
					}
				}
			}
		}
		else
		{
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	}

	return result;
}

// ======================================================================
CxStringA XIE_API fnPRV_VL_GetProductName(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	CxStringA result;

	CxStringA device_name = (name == NULL) ? "" : name;
	if (device_name == "")
	{
		auto device_names = fnPRV_VL_GetDeviceNames(type, dir);
		device_name = device_names[index];
	}

	switch(type)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Audio:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case ExMediaType::Video:
		if (dir == ExMediaDir::Input)
		{
			int fd = open(device_name, O_RDWR|O_NONBLOCK, 0);
			if (fd == -1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			CxFinalizer fd_finalizer([fd]()
				{
					close(fd);
				});

			// 1) カメラデバイスが保有する機能の問い合わせ.
			struct v4l2_capability	cap;
			if (0 == xioctl(fd, VIDIOC_QUERYCAP, &cap))
			{
				int max_length = (int)sizeof(cap.card);
				int min_length = (int)strlen((const char*)cap.card);
				int length = XIE_MIN(min_length, max_length);
				result.Resize(length);
				strncpy(result.Address(), (const char*)cap.card, length);
			}
		}
		else
		{
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	}

	return result;
}

// ======================================================================
CxArrayEx<CxStringA> XIE_API fnPRV_VL_GetPinNames(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	CxArrayEx<CxStringA> result;

	// NOTE: For now, Support video input only.
	if (!(type == ExMediaType::Video && dir == ExMediaDir::Input))
		return result;	// unsupported

	CxStringA device_name = (name == NULL) ? "" : name;
	if (device_name == "")
	{
		auto device_names = fnPRV_VL_GetDeviceNames(type, dir);
		device_name = device_names[index];
	}

	int fd = open(device_name, O_RDWR|O_NONBLOCK, 0);
	if (fd == -1)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	CxFinalizer fd_finalizer([fd]()
		{
			close(fd);
		});

	{
		result.Resize(1);
	}

	return result;
}

// ======================================================================
CxArrayEx<TxSizeI> XIE_API fnPRV_VL_GetFrameSizes(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, int pin)
{
	CxArrayEx<TxSizeI> result;

	// NOTE: For now, Support video input only.
	if (!(type == ExMediaType::Video && dir == ExMediaDir::Input))
		return result;	// unsupported

	CxStringA device_name = (name == NULL) ? "" : name;
	if (device_name == "")
	{
		auto device_names = fnPRV_VL_GetDeviceNames(type, dir);
		device_name = device_names[index];
	}

	int fd = open(device_name, O_RDWR|O_NONBLOCK, 0);
	if (fd == -1)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	CxFinalizer fd_finalizer([fd]()
		{
			close(fd);
		});

	{
		std::unordered_map<std::string,TxSizeI>	sizes;

		struct v4l2_fmtdesc	fmtdesc;
		fmtdesc.index = 0;
		fmtdesc.type = V4L2_BUF_TYPE_VIDEO_CAPTURE;	// v4l2_buf_type

		while (xioctl(fd, VIDIOC_ENUM_FMT, &fmtdesc) >= 0)
		{
			struct v4l2_frmsizeenum	frmsize;
			frmsize.pixel_format = fmtdesc.pixelformat;
			frmsize.index = 0;

			while (xioctl(fd, VIDIOC_ENUM_FRAMESIZES, &frmsize) >= 0)
			{
				if (frmsize.type == V4L2_FRMSIZE_TYPE_DISCRETE)
				{
					TxSizeI size;
					size.Width  = frmsize.discrete.width;
					size.Height = frmsize.discrete.height;
					std::string key = CxString::Format("%d,%d", size.Width, size.Height).Address();
					sizes[key] = size;
				}
				else if (frmsize.type == V4L2_FRMSIZE_TYPE_STEPWISE)
				{
					TxSizeI size;
					size.Width  = frmsize.stepwise.max_width;
					size.Height = frmsize.stepwise.max_height;
					std::string key = CxString::Format("%d,%d", size.Width, size.Height).Address();
					sizes[key] = size;
				}
				frmsize.index++;
			}
			fmtdesc.index++;
		}

		// 結果を格納する.
		// ※ 第２引数の true は、MemoryAlloc で確保することを意味します。
		//    解放時は MemoryFree を使用します。
		//    false を指定した場合は new/delete が使用されます。
		//    この場合、確保と解放時の CRT が一致しない可能性があり危険です。
		//
		result.Resize((int)sizes.size(), true);
		int s = 0;
		for(auto iter=sizes.begin() ; iter!=sizes.end() ; iter++, s++)
			result[s] = iter->second;
	}

	return result;
}

// ======================================================================
int xioctl(int fd, int request, void* arg)
{
	while(true)
	{
		int result = ioctl(fd, request, arg);
		if (-1 == result && EINTR == errno) continue;
		return result;
	}
}

}	// Media
}	// xie

#endif	// _MCS_VER
