/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_CORE_H_INCLUDED_
#define _XIE_CORE_H_INCLUDED_

// ============================================================
// header

#if defined(_MSC_VER)
	#ifndef WIN32_LEAN_AND_MEAN
	#define WIN32_LEAN_AND_MEAN
	#endif
	#ifndef WINVER
	#define WINVER 0x0601
	#endif
	#ifndef _WIN32_WINNT
	// http://msdn.microsoft.com/ja-jp/library/aa383745.aspx
	// ------: -------- (see SDKDDKVer.h)
	// 0x0400: NT4.0	()
	// 0x0500: 2000		()
	// 0x0501: XP		(_WIN32_WINNT_WINXP)
	// 0x0600: Vista	(_WIN32_WINNT_VISTA)
	// 0x0601: 7		(_WIN32_WINNT_WIN7)
	// 0x0602: 8		(_WIN32_WINNT_WIN8)
	// 0x0603: 8.1		(_WIN32_WINNT_WINBLUE)
	#define _WIN32_WINNT 0x0601	// 7 or later
	#endif
	#if defined(_DEBUG)
	#define _CRTDBG_MAP_ALLOC
	#include <crtdbg.h>
	#endif
	#include <windows.h>
	#include <process.h>	// _beginthread, _endthread
	#include <stdio.h>
	#include <stdlib.h>
#else
	#include <stdio.h>
	#include <stddef.h>
	#include <stdlib.h>
	#include <string.h>
	#include <memory.h>
	#include <wchar.h>
	#include <sys/types.h>
	#include <pthread.h>
#endif

// ============================================================
// Information

#define XIE_VER 100	// 1.00

#define XIE_VER_STR	"100"

#if defined(_MSC_VER)
	#if defined(_M_X64)
		#define XIE_X64
		#define XIE_ARCH "x64"
	#else
		#define XIE_X86
		#define XIE_ARCH "x86"
	#endif
#else
	#if defined(__x86_64__)
		#define XIE_X64
		#define XIE_ARCH "x64"
	#else
		#define XIE_X86
		#define XIE_ARCH "x86"
	#endif
#endif

#define XIE_MODULE_ID	0x00454958

// ============================================================
// Structure Align Attribute

#if defined(__GNUC__)
	#define XIE_ALIGNED(sz)	__attribute__ ((aligned (sz)))
	#define XIE_PACKED		__attribute__ ((packed))
#else
	#define XIE_ALIGNED(sz)
	#define XIE_PACKED
#endif

// ============================================================
// warning disable
#if defined(_MSC_VER)
#ifndef XIE_NOUSE_WARNING_DISABLE
	#pragma warning (disable:4068)
	#pragma warning (disable:4251)
//	#pragma warning (disable:4482)
//	#pragma warning (disable:4819)
#endif
#endif

// ============================================================
// DLL exports macro

// specification for function name decoration
#ifdef __cplusplus
	#define XIE_EXPORT_FUNCTION extern "C"
#else
	#define FIE_EXPORT_FUNCTION
#endif

// specification for class name decoration
#if defined(XIE_EXPORTS_DISABLED)
	#define XIE_EXPORT_CLASS							// Disable Exports Marco
#elif !defined(_MSC_VER)
	#define XIE_EXPORT_CLASS							// Shared object or etc
#elif defined(__CUDACC__)
	#define XIE_EXPORT_CLASS							// NVIDIA CUDA Kernel
#elif defined(XIE_EXPORTS)
	#define XIE_EXPORT_CLASS __declspec(dllexport)		// Windows dll export
#else
	#define XIE_EXPORT_CLASS __declspec(dllimport)		// Windows dll import
#endif

// function calling convension
#if defined(_MSC_VER)
	#define XIE_API __stdcall
	#define XIE_CDECL _cdecl
#else
	#define XIE_API
	#define XIE_CDECL
#endif

#if defined(UNICODE)
	#define XIE_TEXT(c)		L ## c
#else
	#define XIE_TEXT(c)		c
#endif

#define XIE_ATOW(m)			XIE_TEXT(m)

#define __XIE_FILE__		XIE_ATOW(__FILE__)
#define __XIE_DATE__		XIE_ATOW(__DATE__)
#define __XIE_TIME__		XIE_ATOW(__TIME__)
#define __XIE_TIMESTAMP__	XIE_ATOW(__TIMESTAMP__)
#define __XIE_FUNCTION__	XIE_ATOW(__FUNCTION__)

// ////////////////////////////////////////////////////////////
// CONSTANTS

// ============================================================
// Packing Size
#ifdef XIE_PACKING_SIZE
#undef XIE_PACKING_SIZE
#endif
#define XIE_PACKING_SIZE 8

// ============================================================
// Image Packing Size
#ifdef XIE_IMAGE_PACKING_SIZE
#undef XIE_IMAGE_PACKING_SIZE
#endif
#define XIE_IMAGE_PACKING_SIZE 4

// ============================================================
// Image Channels Max
#ifdef XIE_IMAGE_CHANNELS_MAX
#undef XIE_IMAGE_CHANNELS_MAX
#endif
#define XIE_IMAGE_CHANNELS_MAX 16

// ============================================================
#define	XIE_U08Min	(unsigned char)0
#define	XIE_U08Max	(unsigned char)255
#define	XIE_U16Min	(unsigned short)0
#define	XIE_U16Max	(unsigned short)65535
#define	XIE_U32Min	(unsigned int)0
#define	XIE_U32Max	(unsigned int)4294967295
#define	XIE_U64Min	0UL
#define	XIE_U64Max	18446744073709551615UL

#define	XIE_S08Min	(char)-128
#define	XIE_S08Max	(char)+127
#define	XIE_S16Min	(short)-32768
#define	XIE_S16Max	(short)+32767
#define	XIE_S32Min	(int)2147483648						// -2147483648
#define	XIE_S32Max	(int)2147483647						// +2147483647
#define	XIE_S64Min	(long long)9223372036854775808UL	// -9223372036854775808
#define	XIE_S64Max	(long long)9223372036854775807L		// +9223372036854775807

#define	XIE_F32Min	(float)-3.402823466e+38F
#define	XIE_F32Max	(float)+3.402823466e+38F
#define	XIE_F64Min	(double)-1.7976931348623158e+308
#define	XIE_F64Max	(double)+1.7976931348623158e+308

#define	XIE_PI 3.14159265358979323846	
#define	XIE_EPSd 1.0e-9	
#define	XIE_EPSf 1.0e-5f	

// ============================================================
// TYPEDEF

#if defined(__cplusplus)
namespace xie
{

#ifdef _MSC_VER
#pragma pack(push,XIE_PACKING_SIZE)
#endif
struct IxModule
{
	virtual int ModuleID() const = 0;
};
typedef IxModule*	HxModule;

#ifdef _MSC_VER
#pragma pack(pop)
#endif

}	// xie
#else
typedef void*	HxModule;
#endif	// __cplusplus

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

#if defined(XIE_X64)
	typedef	long long int			TxIntPtr;
	typedef unsigned long long int	TxUIntPtr;
#else
	typedef	int						TxIntPtr;
	typedef	unsigned int			TxUIntPtr;
#endif

#if defined(UNICODE)
	typedef wchar_t			TxChar;
	typedef wchar_t*		TxCharPtr;
	typedef const wchar_t*	TxCharCPtr;
#else
	typedef char			TxChar;
	typedef char*			TxCharPtr;
	typedef const char*		TxCharCPtr;
#endif

	typedef wchar_t*		TxCharPtrW;
	typedef const wchar_t*	TxCharCPtrW;
	typedef char*			TxCharPtrA;
	typedef const char*		TxCharCPtrA;

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
//

// ============================================================
// ENUM

#if defined(__cplusplus)
namespace xie
{

enum class ExStatus : int
{
	Success = 0,
	InvalidParam,
	InvalidObject,
	MemoryError,
	NotFound,
	Impossible,
	Interrupted,
	IOError,
	Timeout,
	Unsupported,
};

enum class ExType : int
{
	None = 0,
	Ptr,
	U8,
	U16,
	U32,
	U64,
	S8,
	S16,
	S32,
	S64,
	F32,
	F64,
};

enum class ExBoolean : int
{
	False = 0,
	True = 1,
};

enum class ExScanDir : int
{
	X = 0,
	Y = 1,
};

enum class ExEndianType : int
{
	None = 0,
	LE = 1,
	BE = 2,
};

enum class ExMath : int
{
	Abs,
	Sign,
	Sqrt,
	Exp,
	Log,
	Log10,
	Sin,
	Cos,
	Tan,
	Sinh,
	Cosh,
	Tanh,
	Asin,
	Acos,
	Atan,
	Ceiling,
	Floor,
	Round,
	Truncate,
	Modf,
};

// ======================================================================
enum class ExOpe1A : int
{
	Add,
	Mul,
	Sub,
	SubInv,
	Div,
	DivInv,
	Mod,
	ModInv,
	Pow,
	PowInv,
	Atan2,
	Atan2Inv,
	Diff,
	Min,
	Max,
};

// ======================================================================
enum class ExOpe1L : int
{
	And,
	Nand,
	Or,
	Xor,
};

// ======================================================================
enum class ExOpe2A : int
{
	Add,
	Mul,
	Sub,
	Div,
	Mod,
	Pow,
	Atan2,
	Diff,
	Min,
	Max,
};

// ======================================================================
enum class ExOpe2L : int
{
	And,
	Nand,
	Or,
	Xor,
};

}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
//
// FUNCTION
//

XIE_EXPORT_FUNCTION void XIE_API xie_core_setup();
XIE_EXPORT_FUNCTION void XIE_API xie_core_teardown();
XIE_EXPORT_FUNCTION void XIE_API xie_core_plugin(void* handle);

// ============================================================
// Debugger

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

XIE_EXPORT_FUNCTION void XIE_API fnXIE_Core_TraceLevel_Set(int value);
XIE_EXPORT_FUNCTION int XIE_API fnXIE_Core_TraceLevel_Get();
XIE_EXPORT_FUNCTION void XIE_CDECL fnXIE_Core_TraceOutA(int level, TxCharCPtrA format, ...);
XIE_EXPORT_FUNCTION void XIE_CDECL fnXIE_Core_TraceOutW(int level, TxCharCPtrW format, ...);

#if defined(UNICODE)
	#define fnXIE_Core_TraceOut fnXIE_Core_TraceOutW
#else
	#define fnXIE_Core_TraceOut fnXIE_Core_TraceOutA
#endif

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
//
// INCLUDE
//

#if defined(XIE_EXPLICIT_INCLUDE)
	// You need to explicitly include the headers.
#elif defined(XIE_EXPORTS)
	// You need to explicitly include the headers.
#else

#if defined(__cplusplus)
#include "Core/Axi.h"
#include "Core/AxiMath.h"
#include "Core/AxiDiagnostics.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxMutex.h"
#include "Core/CxThread.h"
#include "Core/CxThreadArgs.h"
#include "Core/CxThreadEvent.h"
#include "Core/CxModule.h"
#include "Core/CxArray.h"
#include "Core/CxArrayFilter.h"
#include "Core/CxArrayEx.h"
#include "Core/CxExif.h"
#include "Core/CxImage.h"
#include "Core/CxImageFilter.h"
#include "Core/CxMatrix.h"
#include "Core/CxMatrixFilter.h"
#include "Core/CxStopwatch.h"
#include "Core/CxString.h"
#include "Core/CxStringA.h"
#include "Core/CxStringW.h"
#include "Core/IxAttachable.h"
#include "Core/IxTagPtr.h"
#include "Core/IxEquatable.h"
#include "Core/IxDisposable.h"
#include "Core/IxEventReceiver.h"
#include "Core/IxFileAccess.h"
#include "Core/IxFilePlugin.h"
#include "Core/IxInternalModule.h"
#include "Core/IxParam.h"
#include "Core/IxRawFile.h"
#include "Core/IxRunnable.h"
#include "Core/IxValidatable.h"
#include "Core/TxArray.h"
#include "Core/TxBGR8x3.h"
#include "Core/TxBGR8x4.h"
#include "Core/TxCircleD.h"
#include "Core/TxCircleI.h"
#include "Core/TxCircleArcD.h"
#include "Core/TxCircleArcI.h"
#include "Core/TxDateTime.h"
#include "Core/TxEllipseD.h"
#include "Core/TxEllipseI.h"
#include "Core/TxEllipseArcD.h"
#include "Core/TxEllipseArcI.h"
#include "Core/TxExif.h"
#include "Core/TxExifItem.h"
#include "Core/TxFrameIndex.h"
#include "Core/TxImage.h"
#include "Core/TxImageSize.h"
#include "Core/TxLayer.h"
#include "Core/TxLineD.h"
#include "Core/TxLineI.h"
#include "Core/TxLineSegmentD.h"
#include "Core/TxLineSegmentI.h"
#include "Core/TxMatrix.h"
#include "Core/TxModel.h"
#include "Core/TxPointD.h"
#include "Core/TxPointI.h"
#include "Core/TxRangeD.h"
#include "Core/TxRangeI.h"
#include "Core/TxRawFileHeader.h"
#include "Core/TxRectangleD.h"
#include "Core/TxRectangleI.h"
#include "Core/TxRGB8x3.h"
#include "Core/TxRGB8x4.h"
#include "Core/TxScanner1D.h"
#include "Core/TxScanner2D.h"
#include "Core/TxSizeD.h"
#include "Core/TxSizeI.h"
#include "Core/TxStatistics.h"
#include "Core/TxStringA.h"
#include "Core/TxStringW.h"
#include "Core/TxTrapezoidD.h"
#include "Core/TxTrapezoidI.h"
#include "Core/TxIplImage.h"
#include "Core/TxIplROI.h"
#include "Core/TxCvMat.h"
#include "Effectors/CxBinarize1.h"
#include "Effectors/CxBinarize2.h"
#include "Effectors/CxGammaConverter.h"
#include "Effectors/CxHsvConverter.h"
#include "Effectors/CxHsvToRgb.h"
#include "Effectors/CxIntegral.h"
#include "Effectors/CxMonochrome.h"
#include "Effectors/CxPartColor.h"
#include "Effectors/CxRgbConverter.h"
#include "Effectors/CxRgbToGray.h"
#include "Effectors/CxRgbToHsv.h"
#include "Effectors/CxProjection.h"
#endif

#endif	// INCLUDE

// ////////////////////////////////////////////////////////////
//
// FILENAME
//

#define XIE_CORE_PREFIX "xie_core_" XIE_ARCH "_" XIE_VER_STR

#if defined(_MSC_VER)
	#if defined(XIE_LINK) || !defined(XIE_LINK_DISABLED)
		#pragma comment (lib,XIE_CORE_PREFIX)
	#endif
#endif

#endif
