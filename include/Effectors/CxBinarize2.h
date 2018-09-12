/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXBINARIZE2_H_INCLUDED_
#define _CXBINARIZE2_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "Core/TxRangeD.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxBinarize2 : public CxModule
	, public IxEquatable
{
public:
	TxRangeD	Threshold;
	bool		UseAbs;
	TxRangeD	Value;

public:
	CxBinarize2();
	CxBinarize2(TxRangeD threshold, bool use_abs, TxRangeD value);
	CxBinarize2( const CxBinarize2& src );
	virtual ~CxBinarize2();

	CxBinarize2& operator = ( const CxBinarize2& src );
	bool operator == ( const CxBinarize2& src ) const;
	bool operator != ( const CxBinarize2& src ) const;

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
