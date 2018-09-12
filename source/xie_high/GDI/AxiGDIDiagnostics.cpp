/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/AxiGDIDiagnostics.h"
#include "Core/CxException.h"

namespace xie
{

// ==================================================
CxStringA ToString(xie::GDI::ExGdiScalingMode value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::GDI::ExGdiScalingMode::None:		result = "None"; break;
	case xie::GDI::ExGdiScalingMode::Center:	result = "Center"; break;
	case xie::GDI::ExGdiScalingMode::TopLeft:	result = "TopLeft"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::GDI::ExGdiScalingMode::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::GDI::ExGdiAnchorStyle value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::GDI::ExGdiAnchorStyle::None:		result = "None"; break;
	case xie::GDI::ExGdiAnchorStyle::Arrow:		result = "Arrow"; break;
	case xie::GDI::ExGdiAnchorStyle::Cross:		result = "Cross"; break;
	case xie::GDI::ExGdiAnchorStyle::Diagcross:	result = "Diagcross"; break;
	case xie::GDI::ExGdiAnchorStyle::Diamond:	result = "Diamond"; break;
	case xie::GDI::ExGdiAnchorStyle::Rectangle:	result = "Rectangle"; break;
	case xie::GDI::ExGdiAnchorStyle::Triangle:	result = "Triangle"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::GDI::ExGdiAnchorStyle::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::GDI::ExGdiBrushStyle value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::GDI::ExGdiBrushStyle::None:		result = "None"; break;
	case xie::GDI::ExGdiBrushStyle::Solid:		result = "Solid"; break;
	case xie::GDI::ExGdiBrushStyle::Cross:		result = "Cross"; break;
	case xie::GDI::ExGdiBrushStyle::Diagcross:	result = "Diagcross"; break;
	case xie::GDI::ExGdiBrushStyle::Horizontal:	result = "Horizontal"; break;
	case xie::GDI::ExGdiBrushStyle::Vertical:	result = "Vertical"; break;
	case xie::GDI::ExGdiBrushStyle::Diagonal:	result = "Diagonal"; break;
	case xie::GDI::ExGdiBrushStyle::DiagonalB:	result = "DiagonalB"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::GDI::ExGdiBrushStyle::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::GDI::ExGdiPenStyle value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::GDI::ExGdiPenStyle::None:			result = "None"; break;
	case xie::GDI::ExGdiPenStyle::Solid:		result = "Solid"; break;
	case xie::GDI::ExGdiPenStyle::Dot:			result = "Dot"; break;
	case xie::GDI::ExGdiPenStyle::Dash:			result = "Dash"; break;
	case xie::GDI::ExGdiPenStyle::DashDot:		result = "DashDot"; break;
	case xie::GDI::ExGdiPenStyle::DashDotDot:	result = "DashDotDot"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::GDI::ExGdiPenStyle::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(xie::GDI::ExGdiTextAlign value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case xie::GDI::ExGdiTextAlign::TopLeft:			result = "TopLeft"; break;
	case xie::GDI::ExGdiTextAlign::TopRight:		result = "TopRight"; break;
	case xie::GDI::ExGdiTextAlign::TopCenter:		result = "TopCenter"; break;
	case xie::GDI::ExGdiTextAlign::BottomLeft:		result = "BottomLeft"; break;
	case xie::GDI::ExGdiTextAlign::BottomRight:		result = "BottomRight"; break;
	case xie::GDI::ExGdiTextAlign::BottomCenter:	result = "BottomCenter"; break;
	case xie::GDI::ExGdiTextAlign::BaselineLeft:	result = "BaselineLeft"; break;
	case xie::GDI::ExGdiTextAlign::BaselineRight:	result = "BaselineRight"; break;
	case xie::GDI::ExGdiTextAlign::BaselineCenter:	result = "BaselineCenter"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::GDI::ExGdiTextAlign::%s", result.Address());
	return result;
}

}	// xie
