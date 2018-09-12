/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "Core/IxAttachable.h"
#include "File/api_file.h"
#include "Data/api_data.h"

#if !defined(_MSC_VER)
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <malloc.h>
#include <time.h>
#include <fcntl.h>
#include <sys/time.h>
#include <sys/mman.h>
#include <sys/types.h>
#include <sys/stat.h>
#endif

namespace xie
{
namespace Axi
{

// //////////////////////////////////////////////////////////////////////
// Debugger
//

// ======================================================================
void TraceLevel(int level)
{
	fnXIE_Core_TraceLevel_Set(level);
}

// ======================================================================
int TraceLevel()
{
	return fnXIE_Core_TraceLevel_Get();
}

// //////////////////////////////////////////////////////////////////////
// Aligned Buffer
//

// ======================================================================
void* AlignedAlloc(size_t size, size_t alignment)
{
	if (size == 0) return NULL;
	void* addr = NULL;
	{
		#if defined(_MSC_VER)
		addr = _aligned_malloc(size, alignment);
		#else
		addr = memalign(alignment, size);
		#endif
	}
	return addr;
}

// ======================================================================
void AlignedFree(void* ptr)
{
	if (ptr != NULL)
	{
		#if defined(_MSC_VER)
		_aligned_free(ptr);
		#else
		free(ptr);
		#endif
	}
}

// //////////////////////////////////////////////////////////////////////
// Memory Alloc/Free
//

// ======================================================================
void* MemoryAlloc(size_t size, bool zero_clear)
{
	if (size == 0) return NULL;
#if defined(XIE_X86)
	if (size > (size_t)XIE_S32Max) return NULL;
#else
	if (size > (size_t)XIE_S64Max) return NULL;
#endif
	if (zero_clear)
	{
		void* ptr = calloc(size, 1);
		return ptr;
	}
	else
	{
		void* ptr = malloc(size);
		return ptr;
	}
}

// ======================================================================
void MemoryFree(void* ptr)
{
	if (ptr != NULL)
		free(ptr);
}

// //////////////////////////////////////////////////////////////////////
// Memory Map/Unmap
//

// ======================================================================
void* MemoryMap(size_t size)
{
	if (size == 0) return NULL;
#if defined(XIE_X86)
	if (size > (size_t)XIE_S32Max) return NULL;
#else
	if (size > (size_t)XIE_S64Max) return NULL;
#endif

#if defined(_MSC_VER)
	void* ptr = calloc(1, size);
	return ptr;
#else
	int fd = open("/dev/zero", O_RDWR);
	if (fd < 0)
		return NULL;
	void* ptr = mmap(NULL, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fd, 0);
	close(fd);
	if (ptr == MAP_FAILED)
	{
		fnXIE_Core_TraceOut(1, "%s(%d): mmap failed\n", __FILE__, __LINE__);
		return NULL;
	}
	return ptr;
#endif
}

// ======================================================================
void MemoryUnmap(void* ptr, size_t size)
{
#if defined(_MSC_VER)
	if (ptr != NULL)
		free(ptr);
#else
	if (ptr != NULL)
	{
		int status = 0;
		status = munmap(ptr, size);
		if (status != 0)
		{
			fnXIE_Core_TraceOut(1, "%s(%d): munmap failed. (%p,%lu)\n", __FILE__, __LINE__, ptr, size);
			return NULL;
		}
	}
	return;
#endif
}

// //////////////////////////////////////////////////////////////////////
// Memory Lock/Unlock
//

// ======================================================================
int MemoryLock(void* ptr, size_t size)
{
#if defined(_MSC_VER)
	return 0;
#else
	int status = mlock(ptr, size);
	return status;
#endif
}

// ======================================================================
int MemoryUnlock(void* ptr, size_t size)
{
#if defined(_MSC_VER)
	return 0;
#else
	if (ptr == NULL) return 0;
	int status = munlock(ptr, size);
	return status;
#endif
}

// //////////////////////////////////////////////////////////////////////
// Model
//

// ======================================================================
int SizeOf(ExType type)
{
	return TxModel::SizeOf(type);
}

// ======================================================================
int CalcBpp(ExType type)
{
	return (SizeOf(type) * 8);
}

// ======================================================================
int CalcDepth(ExType type)
{
	switch(type)
	{
		default: return 0;
		case ExType::U8:	return 8;
		case ExType::U16:	return 16;
		case ExType::U32:	return 32;
		case ExType::U64:	return 64;
		case ExType::S8:	return 7;
		case ExType::S16:	return 15;
		case ExType::S32:	return 31;
		case ExType::S64:	return 63;
		case ExType::F32:	return 31;
		case ExType::F64:	return 63;
	}
}

// ======================================================================
TxRangeD CalcRange(ExType type, int depth)
{
	double lower = 0;
	double upper = 0;

	int max_depth = CalcDepth(type);
	if (!(0 < depth && depth <= max_depth))
		depth = max_depth;

	switch(type)
	{
		default:
			break;
		case ExType::U8:
		case ExType::U16:
		case ExType::U32:
		case ExType::U64:
			lower = 0;
			upper = +(pow(2.0, depth) - 1);
			break;
		case ExType::S8:
		case ExType::S16:
		case ExType::S32:
		case ExType::S64:
		case ExType::F32:
		case ExType::F64:
			lower = -(pow(2.0, depth) - 1);
			upper = +(pow(2.0, depth) - 1);
			break;
	}

	return TxRangeD(lower, upper);
}

// ======================================================================
double CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth)
{
	int src_depth_max = CalcDepth(src_type);
	int dst_depth_max = CalcDepth(dst_type);
	if (!(0 < src_depth && src_depth < src_depth_max)) src_depth = src_depth_max;
	if (!(0 < dst_depth && dst_depth < dst_depth_max)) dst_depth = dst_depth_max;
	double src_max = pow(2.0, src_depth) - 1;
	double dst_max = pow(2.0, dst_depth) - 1;
	return (dst_max <= 0 || src_max <= 0) ? 1 : dst_max / src_max;
}

// ======================================================================
int CalcStride(TxModel model, int width, int packing_size)
{
	if (width < 0)	return 0;

	int shift;
	switch(packing_size)
	{
	default: return 0;
	case  1: shift = 0; break;
	case  2: shift = 1; break;
	case  4: shift = 2; break;
	case  8: shift = 3; break;
	case 16: shift = 4; break;
	}

	int wbytes = model.Size() * width;
	if (wbytes < 0)	return 0;

	return ((wbytes + (packing_size - 1)) >> shift) << shift;
}

// //////////////////////////////////////////////////////////////////////
// File - Check
//

// ======================================================================
TxImageSize CheckBmp(TxCharCPtrA filename, bool unpack)
{
	TxImageSize result;
	ExStatus status = xie::File::fnXIE_Core_File_CheckBmpA(&result, filename, (unpack ? ExBoolean::True : ExBoolean::False));
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ======================================================================
TxImageSize CheckJpeg(TxCharCPtrA filename, bool unpack)
{
	TxImageSize result;
	ExStatus status = xie::File::fnXIE_Core_File_CheckJpegA(&result, filename, (unpack ? ExBoolean::True : ExBoolean::False));
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ======================================================================
TxImageSize CheckPng(TxCharCPtrA filename, bool unpack)
{
	TxImageSize result;
	ExStatus status = xie::File::fnXIE_Core_File_CheckPngA(&result, filename, (unpack ? ExBoolean::True : ExBoolean::False));
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ======================================================================
TxImageSize CheckTiff(TxCharCPtrA filename, bool unpack)
{
	TxImageSize result;
	ExStatus status = xie::File::fnXIE_Core_File_CheckTiffA(&result, filename, (unpack ? ExBoolean::True : ExBoolean::False));
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ======================================================================
TxRawFileHeader CheckRaw(TxCharCPtrA filename)
{
	TxRawFileHeader result;
	ExStatus status = xie::File::fnXIE_Core_File_CheckRawA(&result, filename);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// //////////////////////////////////////////////////////////////////////
// Etc
//

// ======================================================================
void Sleep( int timeout )
{
	#if defined(_MSC_VER)
	if (timeout < 0)
	{
		::Sleep( INFINITE );
	}
	else
	{
		::Sleep( timeout );
	}
	#else
	if (timeout < 0)
	{
		select( 0, NULL, NULL, NULL, NULL );
	}
	else
	{
		struct timeval	tv;
		tv.tv_sec  = timeout / 1000;
		tv.tv_usec = timeout % 1000 * 1000;
		select( 0, NULL, NULL, NULL, &tv );
	}
	#endif
}

// ======================================================================
unsigned long long GetTime()
{
	#if defined(_MSC_VER)
	{
		unsigned long long result = 0;
		::GetSystemTimeAsFileTime((LPFILETIME)&result);
		return result;
	}
	#else
	{
		unsigned long long result = 0;
		if (0 != clock_gettime( CLOCK_REALTIME, (struct timespec*)&result ))
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		return result;
	}
	#endif
}

// //////////////////////////////////////////////////////////////////////
// SafeCast
//

#if !defined(_MSC_VER)
// ======================================================================
/*
	@brief	Preventing segmentaion fault.
*/
static bool CanAccess(const void* addr, size_t size)
{
	bool result = false;
	if (addr == NULL) return false;
	if (size == 0) return false;

	int fd[2] = {0};
	int status = pipe(fd);
	if (status == 0)
	{
		ssize_t ws = write(fd[1], (void*)addr, size);
		result = !(ws < 0 || (size_t)ws != size);

		for(int i=0 ; i<2 ; i++)
			close(fd[i]);
	}

	return result;
}

// ======================================================================
/*
	@brief	 To check whether the pointer or not of the class

	----------------------------------------------------------------------------
	      |          this  | vtable    
	------+----------------+----------------------------------------------------
	obj2  | 0x7ffd50bd87a0 | 0x402760
	----------------------------------------------------------------------------

	-----------------------------------------------------------------------------
	index |        vtable  | value      | 
	------+----------------+------------+----------------------------------------
	[-02] |       0x402760 | 0x00000000 | The value must be zero.
	[-01] |       0x402768 | 0x004026f0 | 
	[000] |       0x402770 | 0x00402460 | The value is the first function pointer.
	[001] |       0x402778 | 0x00402450 | The value may be the destructor pointer. 
	[002] |       0x402780 | 0x00402480 | The value may be the destructor pointer.
	  :   |       :        | :          | 
	----------------------------------------------------------------------------

	Example)

	struct TxEmpty
	{
		int ID;
	};

	class CxEmpty
	{
	public:
		CxEmpty(){}
		virtual ~CxEmpty(){}
	};

	void main()
	{
		TxEmpty obj1;
		CxEmpty obj2;

		auto ret1 = xie::Axi::IsClassPtr(&obj1);	// return false
		auto ret2 = xie::Axi::IsClassPtr(&obj2);	// return true
	}
*/
bool IsClassPtr(const void* body)
{
	if (body == NULL) return false;

	auto vptr = (TxIntPtr**)body;
	if (!CanAccess(vptr, sizeof(TxIntPtr*))) return false;

	TxIntPtr* vtable_0 = vptr[0] - 2;
	TxIntPtr* vtable_1 = vptr[0] - 1;
	TxIntPtr* vtable_2 = vptr[0];

	if (!CanAccess(vtable_0, sizeof(TxIntPtr))) return false;
	if (!CanAccess(vtable_1, sizeof(TxIntPtr))) return false;
	if (!CanAccess(vtable_2, sizeof(TxIntPtr))) return false;

	if (*vtable_0 != 0) return false;

	// (!) 2017.04.24: pending
	#if 0
	// The value of [-01] must be less than the address of [-02].
	TxIntPtr diff = (TxIntPtr)vtable_0 - *vtable_1;
	if (diff < (TxIntPtr)sizeof(void*)) return false;
	if ((diff % (TxIntPtr)sizeof(void*)) != 0) return false;
	#endif

	return true;
}
#endif

}
}	// xie
