/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXBINARIZE1_H_INCLUDED_
#define _CXBINARIZE1_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "Core/TxRangeD.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxBinarize1 : public CxModule
	, public IxEquatable
{
public:
	double		Threshold;
	bool		UseAbs;
	TxRangeD	Value;

public:
	CxBinarize1();
	CxBinarize1(double threshold, bool use_abs, TxRangeD value);
	CxBinarize1( const CxBinarize1& src );
	virtual ~CxBinarize1();

	CxBinarize1& operator = ( const CxBinarize1& src );
	bool operator == ( const CxBinarize1& src ) const;
	bool operator != ( const CxBinarize1& src ) const;

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
