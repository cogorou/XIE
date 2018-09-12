/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGAMMACONVERTER_H_INCLUDED_
#define _CXGAMMACONVERTER_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxGammaConverter : public CxModule
	, public IxEquatable
{
public:
	int		Depth;
	double	Gamma;

public:
	CxGammaConverter();
	CxGammaConverter(int depth, double gamma);
	CxGammaConverter( const CxGammaConverter& src );
	virtual ~CxGammaConverter();

	CxGammaConverter& operator = ( const CxGammaConverter& src );
	bool operator == ( const CxGammaConverter& src ) const;
	bool operator != ( const CxGammaConverter& src ) const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual void Execute(HxModule hsrc, HxModule hdst, HxModule hmask = NULL) const;
};

}
}

#pragma pack(pop)

#endif
