/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_gdi.h"
#include "GDI/CxCanvas.h"
#include "GDI/fnPRV_GDI_ConvertFrom_DDB.h"
#include "GDI/fnPRV_GDI_ConvertFrom_YUV.h"
#include "GDI/fnPRV_GDI_ConvertFrom_YUYV.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include <math.h>

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

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;

// ================================================================================
void XIE_API fnPRV_GDI_Setup()
{
	if (g_setuped) return;
	g_setuped = true;
}

// ================================================================================
void XIE_API fnPRV_GDI_TearDown()
{
}

// ================================================================================
#if defined(_MSC_VER)
#else
Display* XIE_API fnPRV_GDI_XServer_Open()
{
	Display* xserver = NULL;

	// X Server
	if (xserver == NULL)
		xserver = ::XOpenDisplay(NULL);	// default (use environment variable)
	if (xserver == NULL)
		xserver = ::XOpenDisplay(":0");	// local
	if (xserver == NULL)
	{
		fnXIE_Core_TraceOut(1, "%s(%d): XOpenDisplay failed in %s.\n", __FILE__, __LINE__, __FUNCTION__);
	}

	return xserver;
}
#endif

// ================================================================================
#if defined(_MSC_VER)
#else
XVisualInfo* XIE_API fnPRV_GDI_XVisual_Open(Display* xserver)
{
	XVisualInfo* xvisual = NULL;

	// X Visual Info
	int attrSB[] =
	{
		GLX_RGBA,
		GLX_RED_SIZE,	1,
		GLX_GREEN_SIZE,	1,
		GLX_BLUE_SIZE,	1,
		None
	};

	int attrDB[] =
	{
		GLX_RGBA,
		GLX_RED_SIZE,	1,
		GLX_GREEN_SIZE,	1,
		GLX_BLUE_SIZE,	1,
		GLX_DOUBLEBUFFER,
		None
	};

	if (xvisual == NULL)
		xvisual = ::glXChooseVisual(xserver, DefaultScreen(xserver), attrSB);	// single buffer
	if (xvisual == NULL)
		xvisual = ::glXChooseVisual(xserver, DefaultScreen(xserver), attrDB);	// double buffer
	if (xvisual == NULL)
	{
		fnXIE_Core_TraceOut(1, "%s(%d): glXChooseVisual failed in %s.\n", __FILE__, __LINE__, __FUNCTION__);
	}

	return xvisual;
}
#endif

// ================================================================================
#if defined(_MSC_VER)
#else
void XIE_API fnPRV_GDI_XServer_Close(Display* xserver)
{
	if (xserver != NULL)
		::XCloseDisplay(xserver);
}
#endif

// ================================================================================
#if defined(_MSC_VER)
#else
void XIE_API fnPRV_GDI_XVisual_Close(XVisualInfo* xvisual)
{
	if (xvisual != NULL)
		XFree(xvisual);
}
#endif

// ======================================================================
bool XIE_API fnPRV_GDI_CanConvertFrom(HxModule hdst, TxSizeI size)
{
	CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
	if (dst == NULL) return false;
	if (dst->IsValid() == false) return false;

	if (dst->Size() != size) return false;

	switch(dst->Channels())
	{
	case 1:
		switch(dst->Model().Type)
		{
		case ExType::U8:
			switch(dst->Model().Pack)
			{
			case 1:
			case 3:
			case 4:
				return true;
			}
			break;
		default:
			return false;
		}
		break;
	case 3:
	case 4:
		switch(dst->Model().Type)
		{
		case ExType::U8:
			switch(dst->Model().Pack)
			{
			case 1:
				return true;
			default:
				return false;
			}
			break;
		default:
			return false;
		}
		break;
	}
	return false;
}

// ======================================================================
void XIE_API fnPRV_GDI_ConvertFrom_DDB(HxModule hdst, HxModule hsrc)
{
	CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
	if (src == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
	if (dst == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst->IsValid() == false)
		dst->Resize(src->ImageSize());

	if (dst->Size() != src->Size())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	TxImage src_tag = src->Tag();
	TxImage dst_tag = dst->Tag();

	// Convert
	fnPRV_GDI_ConvertFrom_DDB___(dst_tag, src_tag);
}

// ======================================================================
void XIE_API fnPRV_GDI_ConvertFrom_YUYV(HxModule hdst, HxModule hsrc)
{
	CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
	if (src == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
	if (dst == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst->IsValid() == false)
		dst->Resize(src->Width() * 2, src->Height(), src->Model(), src->Channels());

	if (dst->Width() * 2 != src->Width() ||
		dst->Height() != src->Height())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	TxImage src_tag = src->Tag();
	TxImage dst_tag = dst->Tag();

	// Convert
	fnPRV_GDI_ConvertFrom_YUYV___(dst_tag, src_tag);
}

// ================================================================================
TxPointD XIE_API fnPRV_GDI_Scaling(double x, double y, double mag, ExGdiScalingMode mode)
{
	TxPointD dst;
	switch(mode)
	{
	default:
	case ExGdiScalingMode::None:
		dst.X = x;
		dst.Y = y;
		break;
	case ExGdiScalingMode::TopLeft:
		dst.X = x * mag;
		dst.Y = y * mag;
		break;
	case ExGdiScalingMode::Center:
		dst.X = (x - 0.5) * mag;
		dst.Y = (y - 0.5) * mag;
		break;
	}
	return dst;
}

// ================================================================================
TxPointD XIE_API fnPRV_GDI_Scaling(TxPointD src, double mag, ExGdiScalingMode mode)
{
	return fnPRV_GDI_Scaling(src.X, src.Y, mag, mode);
}

// ================================================================================
void XIE_API fnPRV_GDI_BkPrepare(const TxGdi2dParam* param)
{
	float	fR = param->BkColor.R / 255.0f;
	float	fG = param->BkColor.G / 255.0f;
	float	fB = param->BkColor.B / 255.0f;
	float	fA = param->BkColor.A / 255.0f;

	::glColor4f( fR, fG, fB, fA );
	::glLineWidth( 1.0f );
}

// ================================================================================
void XIE_API fnPRV_GDI_BkRestore(const TxGdi2dParam* param)
{
	::glDisable(GL_LINE_STIPPLE);
}

// ================================================================================
void XIE_API fnPRV_GDI_PenPrepare(const TxGdi2dParam* param)
{
	float	fR = param->PenColor.R / 255.0f;
	float	fG = param->PenColor.G / 255.0f;
	float	fB = param->PenColor.B / 255.0f;
	float	fA = param->PenColor.A / 255.0f;

	::glColor4f( fR, fG, fB, fA );

	::glLineWidth( (float)param->PenWidth );

	switch(param->PenStyle)
	{
	default:
	case ExGdiPenStyle::Solid:		::glDisable(GL_LINE_STIPPLE);	break;
	case ExGdiPenStyle::Dash:		::glEnable(GL_LINE_STIPPLE);	::glLineStipple(1 , 0xFFF0);	break;	// [---- ---- ----     ]
	case ExGdiPenStyle::DashDot:		::glEnable(GL_LINE_STIPPLE);	::glLineStipple(1 , 0xFC30);	break;	// [---- --     --     ]
	case ExGdiPenStyle::DashDotDot:	::glEnable(GL_LINE_STIPPLE);	::glLineStipple(1 , 0xFCCC);	break;	// [---- --   --   --  ]
	case ExGdiPenStyle::Dot:			::glEnable(GL_LINE_STIPPLE);	::glLineStipple(1 , 0xCCCC);	break;	// [--   --   --   --  ]
	}
}

// ================================================================================
void XIE_API fnPRV_GDI_PenRestore(const TxGdi2dParam* param)
{
	::glDisable(GL_LINE_STIPPLE);
}

// ================================================================================
int XIE_API fnPRV_GDI_HitTest_Point(const TxPointD& position, double margin, const TxPointD& figure)
{
	if (figure.X - margin <= position.X && position.X <= figure.X + margin &&
		figure.Y - margin <= position.Y && position.Y <= figure.Y + margin)
	{
		return 1;
	}
	return 0;
}

// ================================================================================
int XIE_API fnPRV_GDI_HitTest_Line(const TxPointD& position, double margin, const TxLineD& figure)
{
	double d1 =	(figure.A * position.X + figure.B * position.Y + figure.C);
	double d2 = sqrt(figure.A * figure.A + figure.B * figure.B);
	if (fabs(d2) > XIE_EPSd)
	{
		double distance = (d1 / d2);
		if (margin <= distance && distance <= margin)
			return 1;
	}
	return 0;
}

// ================================================================================
TxHitPosition XIE_API fnPRV_GDI_HitTest_LineSegment(const TxPointD& position, double margin, const TxLineSegmentD& figure)
{
	// 始点.
	if (figure.X1-margin <= position.X && position.X <= figure.X1+margin &&
		figure.Y1-margin <= position.Y && position.Y <= figure.Y1+margin)
	{
		return TxHitPosition(2, 0, 1);
	}
	// 終点.
	if (figure.X2-margin <= position.X && position.X <= figure.X2+margin &&
		figure.Y2-margin <= position.Y && position.Y <= figure.Y2+margin)
	{
		return TxHitPosition(2, 0, 2);
	}

	// 線分上.
	double	xL = figure.X2 - figure.X1;
	double	yL = figure.Y2 - figure.Y1;
	if( !(xL == 0 && yL == 0) )
	{
		// 始点を中心にしたときの終点の角度を算出する.
		double degree = xie::Axi::RadToDeg( atan2( yL, xL ) );

		// 各点を始点を中心に逆方向に回転する.
		TxPointD st = figure.Point1();
		TxPointD ed = xie::Axi::Rotate<TxPointD>(figure.Point2(), st, -degree);
		TxPointD p0 = xie::Axi::Rotate<TxPointD>(position, st, -degree);

		// マウス位置を水平線上で判定する.
		if( st.X-margin <= p0.X && p0.X <= ed.X+margin &&
			st.Y-margin <= p0.Y && p0.Y <= ed.Y+margin )
		{
			return TxHitPosition(1, 0, 0);
		}
	}

	return TxHitPosition::Default();
}

// ================================================================================
TxHitPosition XIE_API fnPRV_GDI_HitTest_Rectangle(const TxPointD& position, double margin, const TxRectangleD& figure)
{
	// 矩形の頂点.
	double	sx = figure.X;
	double	sy = figure.Y;
	double	ex = figure.X + figure.Width;
	double	ey = figure.Y + figure.Height;

	// 矩形の基準点との差.
	auto diff = position - figure.Location();

	// 範囲:
	bool L = (sx-margin <= position.X && position.X <= sx+margin);
	bool T = (sy-margin <= position.Y && position.Y <= sy+margin);
	bool R = (ex-margin <= position.X && position.X <= ex+margin);
	bool B = (ey-margin <= position.Y && position.Y <= ey+margin);

	bool W = (figure.Width < 0)
		? (figure.Width <= diff.X && diff.X <= 0)
		: (0 <= diff.X && diff.X <= figure.Width);

	bool H = (figure.Height < 0)
		? (figure.Height <= diff.Y && diff.Y <= 0)
		: (0 <= diff.Y && diff.Y <= figure.Height);

	// 頂点.
	if (T && L) return TxHitPosition(2, 0, 1);
	if (T && R) return TxHitPosition(2, 0, 2);
	if (B && R) return TxHitPosition(2, 0, 3);
	if (B && L) return TxHitPosition(2, 0, 4);

	// 辺.
	if (T && W) return TxHitPosition(2, 0, -1);
	if (R && H) return TxHitPosition(2, 0, -2);
	if (B && W) return TxHitPosition(2, 0, -3);
	if (L && H) return TxHitPosition(2, 0, -4);

	// 面.
	if (W && H) return TxHitPosition(1, 0, 0);

	return TxHitPosition::Default();
}

#if defined(_MSC_VER)

// ================================================================================
HDC XIE_API fnPRV_GDI_CreateDC()
{
	HDC hScreenDC = ::GetDC(NULL);
	HDC hDC = ::CreateCompatibleDC(hScreenDC);
	::ReleaseDC(NULL, hScreenDC);
	return hDC;
}

// ================================================================================
bool XIE_API fnPRV_GDI_WorldTransformReset( HDC hdc )
{
	if( hdc == NULL ) return false;

	::SetGraphicsMode( hdc, GM_ADVANCED );

	// | eM11 eM12 0 |
	// | eM21 eM22 0 |
	// | eDx  eDy  1 |

	XFORM	xform =
	{
		1.0f, 0.0f,
		0.0f, 1.0f,
		0.0f, 0.0f
	};
	::SetWorldTransform(hdc, &xform);

	return true;
}

// ================================================================================
bool XIE_API fnPRV_GDI_WorldTransformRotate( HDC hdc, double axis_x, double axis_y, double angle )
{
	if( hdc == NULL ) return false;

	// | eM11 eM12 0 |
	// | eM21 eM22 0 |
	// | eDx  eDy  1 |

	// ===> rotate setting
	const double R = xie::Axi::DegToRad( angle );

	double eM11 =  cos(R);	// eM11:
	double eM12 =  sin(R);	// eM12:
	double eM21 = -sin(R);	// eM21:
	double eM22 =  cos(R);	// eM22:

	// x' = (x * eM11) + (y * eM21) + eDx
	// y' = (x * eM12) + (y * eM22) + eDy
	//   *) dimension of world : x  , y
	//   *) dimension of page  : x' , y'
	XFORM	xform =
	{
		(float)eM11,
		(float)eM12,
		(float)eM21,
		(float)eM22,
		(float)(axis_x - axis_x * eM11 - axis_y * eM21),	// eDx : x' - (x * eM11) - (y * eM21)
		(float)(axis_y - axis_x * eM12 - axis_y * eM22)		// eDy : y' - (x * eM12) - (y * eM22)
	};
	::ModifyWorldTransform( hdc, &xform, MWT_LEFTMULTIPLY );
	// ===> rotate setting

	return true;
}

// ================================================================================
bool XIE_API fnPRV_GDI_WorldTransformScale( HDC hdc, double origin_x, double origin_y, double mag_x, double mag_y )
{
	if( hdc == NULL ) return false;

	// | eM11 eM12 0 |
	// | eM21 eM22 0 |
	// | eDx  eDy  1 |

	// ===> scale setting
	double eM11 = mag_x;	// eM11:
	double eM12 = 0.0;		// eM12:
	double eM21 = 0.0;		// eM21:
	double eM22 = mag_y;	// eM22:

	// x' = (x * eM11) + (y * eM21) + eDx
	// y' = (x * eM12) + (y * eM22) + eDy
	//   *) dimension of world : x  , y
	//   *) dimension of page  : x' , y'
	XFORM	xform =
	{
		(float)eM11,
		(float)eM12,
		(float)eM21,
		(float)eM22,
		(float)(origin_x - origin_x * eM11 - origin_y * eM21),	// eDx : x' - (x * eM11) - (y * eM21)
		(float)(origin_y - origin_x * eM12 - origin_y * eM22)	// eDy : y' - (x * eM12) - (y * eM22)
	};
	::ModifyWorldTransform( hdc, &xform, MWT_LEFTMULTIPLY );
	// ===> scale setting

	return true;
}

// ================================================================================
/*
	本関数は未完成です。
	CxGdiString で ShiftJIS の文字列を描画する為に取り敢えず作成したものです。
	CxGdiString::Render メソッド内で CreateFontIndirectA を行う際に使用しています。

		----------------------------------------------------------------------
		LOGFONTA	font;
		:
		font.lfCharSet = fnPRV_GDI_CodePageToCharset(m_Tag.CodePage);
		                                           ~~~~~~~~~~~~~~ ShiftJIS の場合は 932 を指定します.
		:
		HFONT hfont	= ::CreateFontIndirectA( &font );
		----------------------------------------------------------------------

	seealso:
	Code Page Identifiers
	https://msdn.microsoft.com/en-us/library/windows/desktop/dd317756%28v=vs.85%29.aspx

	Character sets and codepages
	https://www.microsoft.com/typography/unicode/cscp.htm

	CreateFont
	https://msdn.microsoft.com/ja-jp/library/cc428368.aspx

	WinGDi.h
*/
int XIE_API fnPRV_GDI_CodePageToCharset(int codepage)
{
	switch(codepage)
	{
	default   : return DEFAULT_CHARSET;
	case    37: return DEFAULT_CHARSET;	//	IBM037	IBM EBCDIC US-Canada
	case   437: return DEFAULT_CHARSET;	//	IBM437	OEM United States
	case   500: return DEFAULT_CHARSET;	//	IBM500	IBM EBCDIC International
	case   708: return ARABIC_CHARSET;	//	ASMO-708	Arabic (ASMO 708)
	case   709: return ARABIC_CHARSET;	//	            Arabic (ASMO-449+, BCON V4)
	case   710: return ARABIC_CHARSET;	//	            Arabic - Transparent Arabic
	case   720: return ARABIC_CHARSET;	//	DOS-720	Arabic (Transparent ASMO); Arabic (DOS)
	case   737: return GREEK_CHARSET;	//	ibm737	OEM Greek (formerly 437G); Greek (DOS)
	case   775: return BALTIC_CHARSET;	//	ibm775	OEM Baltic; Baltic (DOS)
	case   850: return FS_LATIN1;	//	ibm850	OEM Multilingual Latin 1; Western European (DOS)
	case   852: return FS_LATIN2;	//	ibm852	OEM Latin 2; Central European (DOS)
	case   855: return FS_CYRILLIC;	//	IBM855	OEM Cyrillic (primarily Russian)
	case   857: return FS_TURKISH;	//	ibm857	OEM Turkish; Turkish (DOS)
	case   858: return FS_LATIN1;	//	IBM00858	OEM Multilingual Latin 1 + Euro symbol
	case   860: return DEFAULT_CHARSET;	//	IBM860	OEM Portuguese; Portuguese (DOS)
	case   861: return DEFAULT_CHARSET;	//	ibm861	OEM Icelandic; Icelandic (DOS)
	case   862: return FS_HEBREW;	//	DOS-862	OEM Hebrew; Hebrew (DOS)
	case   863: return DEFAULT_CHARSET;	//	IBM863	OEM French Canadian; French Canadian (DOS)
	case   864: return ARABIC_CHARSET;	//	IBM864	OEM Arabic; Arabic (864)
	case   865: return DEFAULT_CHARSET;	//	IBM865	OEM Nordic; Nordic (DOS)
	case   866: return RUSSIAN_CHARSET;	//	cp866	OEM Russian; Cyrillic (DOS)
	case   869: return GREEK_CHARSET;	//	ibm869	OEM Modern Greek; Greek, Modern (DOS)
	case   870: return DEFAULT_CHARSET;	//	IBM870	IBM EBCDIC Multilingual/ROECE (Latin 2); IBM EBCDIC Multilingual Latin 2
	case   874: return THAI_CHARSET;	//	windows-874	ANSI/OEM Thai (ISO 8859-11); Thai (Windows)
	case   875: return DEFAULT_CHARSET;	//	cp875	IBM EBCDIC Greek Modern
	case   932: return SHIFTJIS_CHARSET;	//	shift_jis	ANSI/OEM Japanese; Japanese (Shift-JIS)
	case   936: return GB2312_CHARSET;	//	gb2312	ANSI/OEM Simplified Chinese (PRC, Singapore); Chinese Simplified (GB2312)
	case   949: return HANGEUL_CHARSET;	//	ks_c_5601-1987	ANSI/OEM Korean (Unified Hangul Code)
	case   950: return CHINESEBIG5_CHARSET;	//	big5	ANSI/OEM Traditional Chinese (Taiwan; Hong Kong SAR, PRC); Chinese Traditional (Big5)
	case  1026: return DEFAULT_CHARSET;	//	IBM1026	IBM EBCDIC Turkish (Latin 5)
	case  1047: return DEFAULT_CHARSET;	//	IBM01047	IBM EBCDIC Latin 1/Open System
	case  1140: return DEFAULT_CHARSET;	//	IBM01140	IBM EBCDIC US-Canada (037 + Euro symbol); IBM EBCDIC (US-Canada-Euro)
	case  1141: return DEFAULT_CHARSET;	//	IBM01141	IBM EBCDIC Germany (20273 + Euro symbol); IBM EBCDIC (Germany-Euro)
	case  1142: return DEFAULT_CHARSET;	//	IBM01142	IBM EBCDIC Denmark-Norway (20277 + Euro symbol); IBM EBCDIC (Denmark-Norway-Euro)
	case  1143: return DEFAULT_CHARSET;	//	IBM01143	IBM EBCDIC Finland-Sweden (20278 + Euro symbol); IBM EBCDIC (Finland-Sweden-Euro)
	case  1144: return DEFAULT_CHARSET;	//	IBM01144	IBM EBCDIC Italy (20280 + Euro symbol); IBM EBCDIC (Italy-Euro)
	case  1145: return DEFAULT_CHARSET;	//	IBM01145	IBM EBCDIC Latin America-Spain (20284 + Euro symbol); IBM EBCDIC (Spain-Euro)
	case  1146: return DEFAULT_CHARSET;	//	IBM01146	IBM EBCDIC United Kingdom (20285 + Euro symbol); IBM EBCDIC (UK-Euro)
	case  1147: return DEFAULT_CHARSET;	//	IBM01147	IBM EBCDIC France (20297 + Euro symbol); IBM EBCDIC (France-Euro)
	case  1148: return DEFAULT_CHARSET;	//	IBM01148	IBM EBCDIC International (500 + Euro symbol); IBM EBCDIC (International-Euro)
	case  1149: return DEFAULT_CHARSET;	//	IBM01149	IBM EBCDIC Icelandic (20871 + Euro symbol); IBM EBCDIC (Icelandic-Euro)
	case  1200: return DEFAULT_CHARSET;	//	utf-16	Unicode UTF-16, little endian byte order (BMP of ISO 10646); available only to managed applications
	case  1201: return DEFAULT_CHARSET;	//	unicodeFFFE	Unicode UTF-16, big endian byte order; available only to managed applications
	case  1250: return ANSI_CHARSET;	//	windows-1250	ANSI Central European; Central European (Windows)
	case  1251: return ANSI_CHARSET;	//	windows-1251	ANSI Cyrillic; Cyrillic (Windows)
	case  1252: return ANSI_CHARSET;	//	windows-1252	ANSI Latin 1; Western European (Windows)
	case  1253: return ANSI_CHARSET;	//	windows-1253	ANSI Greek; Greek (Windows)
	case  1254: return ANSI_CHARSET;	//	windows-1254	ANSI Turkish; Turkish (Windows)
	case  1255: return ANSI_CHARSET;	//	windows-1255	ANSI Hebrew; Hebrew (Windows)
	case  1256: return ANSI_CHARSET;	//	windows-1256	ANSI Arabic; Arabic (Windows)
	case  1257: return ANSI_CHARSET;	//	windows-1257	ANSI Baltic; Baltic (Windows)
	case  1258: return ANSI_CHARSET;	//	windows-1258	ANSI/OEM Vietnamese; Vietnamese (Windows)
	case  1361: return JOHAB_CHARSET;	//	Johab	Korean (Johab)
	case 10000: return MAC_CHARSET;	//	macintosh	MAC Roman; Western European (Mac)
	case 10001: return MAC_CHARSET;	//	x-mac-japanese	Japanese (Mac)
	case 10002: return MAC_CHARSET;	//	x-mac-chinesetrad	MAC Traditional Chinese (Big5); Chinese Traditional (Mac)
	case 10003: return MAC_CHARSET;	//	x-mac-korean	Korean (Mac)
	case 10004: return MAC_CHARSET;	//	x-mac-arabic	Arabic (Mac)
	case 10005: return MAC_CHARSET;	//	x-mac-hebrew	Hebrew (Mac)
	case 10006: return MAC_CHARSET;	//	x-mac-greek	Greek (Mac)
	case 10007: return MAC_CHARSET;	//	x-mac-cyrillic	Cyrillic (Mac)
	case 10008: return MAC_CHARSET;	//	x-mac-chinesesimp	MAC Simplified Chinese (GB 2312); Chinese Simplified (Mac)
	case 10010: return MAC_CHARSET;	//	x-mac-romanian	Romanian (Mac)
	case 10017: return MAC_CHARSET;	//	x-mac-ukrainian	Ukrainian (Mac)
	case 10021: return MAC_CHARSET;	//	x-mac-thai	Thai (Mac)
	case 10029: return MAC_CHARSET;	//	x-mac-ce	MAC Latin 2; Central European (Mac)
	case 10079: return MAC_CHARSET;	//	x-mac-icelandic	Icelandic (Mac)
	case 10081: return MAC_CHARSET;	//	x-mac-turkish	Turkish (Mac)
	case 10082: return MAC_CHARSET;	//	x-mac-croatian	Croatian (Mac)
	case 12000: return DEFAULT_CHARSET;	//	utf-32	Unicode UTF-32, little endian byte order; available only to managed applications
	case 12001: return DEFAULT_CHARSET;	//	utf-32BE	Unicode UTF-32, big endian byte order; available only to managed applications
	case 20000: return DEFAULT_CHARSET;	//	x-Chinese_CNS	CNS Taiwan; Chinese Traditional (CNS)
	case 20001: return DEFAULT_CHARSET;	//	x-cp20001	TCA Taiwan
	case 20002: return DEFAULT_CHARSET;	//	x_Chinese-Eten	Eten Taiwan; Chinese Traditional (Eten)
	case 20003: return DEFAULT_CHARSET;	//	x-cp20003	IBM5550 Taiwan
	case 20004: return DEFAULT_CHARSET;	//	x-cp20004	TeleText Taiwan
	case 20005: return DEFAULT_CHARSET;	//	x-cp20005	Wang Taiwan
	case 20105: return DEFAULT_CHARSET;	//	x-IA5	IA5 (IRV International Alphabet No. 5, 7-bit); Western European (IA5)
	case 20106: return DEFAULT_CHARSET;	//	x-IA5-German	IA5 German (7-bit)
	case 20107: return DEFAULT_CHARSET;	//	x-IA5-Swedish	IA5 Swedish (7-bit)
	case 20108: return DEFAULT_CHARSET;	//	x-IA5-Norwegian	IA5 Norwegian (7-bit)
	case 20127: return DEFAULT_CHARSET;	//	us-ascii	US-ASCII (7-bit)
	case 20261: return DEFAULT_CHARSET;	//	x-cp20261	T.61
	case 20269: return DEFAULT_CHARSET;	//	x-cp20269	ISO 6937 Non-Spacing Accent
	case 20273: return DEFAULT_CHARSET;	//	IBM273	IBM EBCDIC Germany
	case 20277: return DEFAULT_CHARSET;	//	IBM277	IBM EBCDIC Denmark-Norway
	case 20278: return DEFAULT_CHARSET;	//	IBM278	IBM EBCDIC Finland-Sweden
	case 20280: return DEFAULT_CHARSET;	//	IBM280	IBM EBCDIC Italy
	case 20284: return DEFAULT_CHARSET;	//	IBM284	IBM EBCDIC Latin America-Spain
	case 20285: return DEFAULT_CHARSET;	//	IBM285	IBM EBCDIC United Kingdom
	case 20290: return DEFAULT_CHARSET;	//	IBM290	IBM EBCDIC Japanese Katakana Extended
	case 20297: return DEFAULT_CHARSET;	//	IBM297	IBM EBCDIC France
	case 20420: return DEFAULT_CHARSET;	//	IBM420	IBM EBCDIC Arabic
	case 20423: return DEFAULT_CHARSET;	//	IBM423	IBM EBCDIC Greek
	case 20424: return DEFAULT_CHARSET;	//	IBM424	IBM EBCDIC Hebrew
	case 20833: return DEFAULT_CHARSET;	//	x-EBCDIC-KoreanExtended	IBM EBCDIC Korean Extended
	case 20838: return DEFAULT_CHARSET;	//	IBM-Thai	IBM EBCDIC Thai
	case 20866: return DEFAULT_CHARSET;	//	koi8-r	Russian (KOI8-R); Cyrillic (KOI8-R)
	case 20871: return DEFAULT_CHARSET;	//	IBM871	IBM EBCDIC Icelandic
	case 20880: return DEFAULT_CHARSET;	//	IBM880	IBM EBCDIC Cyrillic Russian
	case 20905: return DEFAULT_CHARSET;	//	IBM905	IBM EBCDIC Turkish
	case 20924: return DEFAULT_CHARSET;	//	IBM00924	IBM EBCDIC Latin 1/Open System (1047 + Euro symbol)
	case 20932: return DEFAULT_CHARSET;	//	EUC-JP	Japanese (JIS 0208-1990 and 0212-1990)
	case 20936: return DEFAULT_CHARSET;	//	x-cp20936	Simplified Chinese (GB2312); Chinese Simplified (GB2312-80)
	case 20949: return DEFAULT_CHARSET;	//	x-cp20949	Korean Wansung
	case 21025: return DEFAULT_CHARSET;	//	cp1025	IBM EBCDIC Cyrillic Serbian-Bulgarian
	case 21027: return DEFAULT_CHARSET;	//		(deprecated)
	case 21866: return DEFAULT_CHARSET;	//	koi8-u	Ukrainian (KOI8-U); Cyrillic (KOI8-U)
	case 28591: return DEFAULT_CHARSET;	//	iso-8859-1	ISO 8859-1 Latin 1; Western European (ISO)
	case 28592: return DEFAULT_CHARSET;	//	iso-8859-2	ISO 8859-2 Central European; Central European (ISO)
	case 28593: return DEFAULT_CHARSET;	//	iso-8859-3	ISO 8859-3 Latin 3
	case 28594: return DEFAULT_CHARSET;	//	iso-8859-4	ISO 8859-4 Baltic
	case 28595: return DEFAULT_CHARSET;	//	iso-8859-5	ISO 8859-5 Cyrillic
	case 28596: return DEFAULT_CHARSET;	//	iso-8859-6	ISO 8859-6 Arabic
	case 28597: return DEFAULT_CHARSET;	//	iso-8859-7	ISO 8859-7 Greek
	case 28598: return DEFAULT_CHARSET;	//	iso-8859-8	ISO 8859-8 Hebrew; Hebrew (ISO-Visual)
	case 28599: return DEFAULT_CHARSET;	//	iso-8859-9	ISO 8859-9 Turkish
	case 28603: return DEFAULT_CHARSET;	//	iso-8859-13	ISO 8859-13 Estonian
	case 28605: return DEFAULT_CHARSET;	//	iso-8859-15	ISO 8859-15 Latin 9
	case 29001: return DEFAULT_CHARSET;	//	x-Europa	Europa 3
	case 38598: return DEFAULT_CHARSET;	//	iso-8859-8-i	ISO 8859-8 Hebrew; Hebrew (ISO-Logical)
	case 50220: return DEFAULT_CHARSET;	//	iso-2022-jp	ISO 2022 Japanese with no halfwidth Katakana; Japanese (JIS)
	case 50221: return DEFAULT_CHARSET;	//	csISO2022JP	ISO 2022 Japanese with halfwidth Katakana; Japanese (JIS-Allow 1 byte Kana)
	case 50222: return DEFAULT_CHARSET;	//	iso-2022-jp	ISO 2022 Japanese JIS X 0201-1989; Japanese (JIS-Allow 1 byte Kana - SO/SI)
	case 50225: return DEFAULT_CHARSET;	//	iso-2022-kr	ISO 2022 Korean
	case 50227: return DEFAULT_CHARSET;	//	x-cp50227	ISO 2022 Simplified Chinese; Chinese Simplified (ISO 2022)
	case 50229: return DEFAULT_CHARSET;	//	            ISO 2022 Traditional Chinese
	case 50930: return DEFAULT_CHARSET;	//	            EBCDIC Japanese (Katakana) Extended
	case 50931: return DEFAULT_CHARSET;	//	            EBCDIC US-Canada and Japanese
	case 50933: return DEFAULT_CHARSET;	//	            EBCDIC Korean Extended and Korean
	case 50935: return DEFAULT_CHARSET;	//	            EBCDIC Simplified Chinese Extended and Simplified Chinese
	case 50936: return DEFAULT_CHARSET;	//	            EBCDIC Simplified Chinese
	case 50937: return DEFAULT_CHARSET;	//	            EBCDIC US-Canada and Traditional Chinese
	case 50939: return DEFAULT_CHARSET;	//	            EBCDIC Japanese (Latin) Extended and Japanese
	case 51932: return DEFAULT_CHARSET;	//	euc-jp	EUC Japanese
	case 51936: return DEFAULT_CHARSET;	//	EUC-CN	EUC Simplified Chinese; Chinese Simplified (EUC)
	case 51949: return DEFAULT_CHARSET;	//	euc-kr	EUC Korean
	case 51950: return DEFAULT_CHARSET;	//		    EUC Traditional Chinese
	case 52936: return DEFAULT_CHARSET;	//	hz-gb-2312	HZ-GB2312 Simplified Chinese; Chinese Simplified (HZ)
	case 54936: return DEFAULT_CHARSET;	//	GB18030	Windows XP and later: GB18030 Simplified Chinese (4 byte); Chinese Simplified (GB18030)
	case 57002: return DEFAULT_CHARSET;	//	x-iscii-de	ISCII Devanagari
	case 57003: return DEFAULT_CHARSET;	//	x-iscii-be	ISCII Bengali
	case 57004: return DEFAULT_CHARSET;	//	x-iscii-ta	ISCII Tamil
	case 57005: return DEFAULT_CHARSET;	//	x-iscii-te	ISCII Telugu
	case 57006: return DEFAULT_CHARSET;	//	x-iscii-as	ISCII Assamese
	case 57007: return DEFAULT_CHARSET;	//	x-iscii-or	ISCII Oriya
	case 57008: return DEFAULT_CHARSET;	//	x-iscii-ka	ISCII Kannada
	case 57009: return DEFAULT_CHARSET;	//	x-iscii-ma	ISCII Malayalam
	case 57010: return DEFAULT_CHARSET;	//	x-iscii-gu	ISCII Gujarati
	case 57011: return DEFAULT_CHARSET;	//	x-iscii-pa	ISCII Punjabi
	case 65000: return DEFAULT_CHARSET;	//	utf-7	Unicode (UTF-7)
	case 65001: return DEFAULT_CHARSET;	//	utf-8	Unicode (UTF-8)
	}
}
#endif

// ============================================================
bool fnPRV_Gdi2d_GetParam(TxCharCPtrA name, void* value, TxModel model, const TxGdi2dParam& param)
{
	if (strcmp(name, "Axis") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.Axis)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "Angle") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.Angle)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "BkColor") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.BkColor)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "BkEnable") == 0)
	{
		if (model == ModelOf<bool>())
		{
			*static_cast<bool*>(value) = (param.BkEnable == ExBoolean::True);
			return true;
		}
		if (model == ModelOf<ExBoolean>())
		{
			*static_cast<ExBoolean*>(value) = param.BkEnable;
			return true;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenColor") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.PenColor)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenStyle") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.PenStyle)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenWidth") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, param.PenWidth)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	return false;
}

// ============================================================
bool fnPRV_Gdi2d_SetParam(TxCharCPtrA name, const void* value, TxModel model, TxGdi2dParam& param)
{
	if (strcmp(name, "Axis") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.Axis)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "Angle") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.Angle)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "BkColor") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.BkColor)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "BkEnable") == 0)
	{
		if (model == ModelOf<bool>())
		{
			param.BkEnable = (*static_cast<const bool*>(value)) ? ExBoolean::True : ExBoolean::False;
			return true;
		}
		if (model == ModelOf<ExBoolean>())
		{
			param.BkEnable = *static_cast<const ExBoolean*>(value);
			return true;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenColor") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.PenColor)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenStyle") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.PenStyle)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "PenWidth") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, param.PenWidth)) return true;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	return false;
}

}	// GDI
}	// xie
