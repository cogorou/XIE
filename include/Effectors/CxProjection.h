/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXPROJECTION_H_INCLUDED_
#define _CXPROJECTION_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxProjection : public CxModule
	, public IxEquatable
{
public:
	ExScanDir	ScanDir;
	int			Channel;

public:
	CxProjection();
	CxProjection(ExScanDir dir, int ch);
	CxProjection( const CxProjection& src );
	virtual ~CxProjection();

	CxProjection& operator = ( const CxProjection& src );
	bool operator == ( const CxProjection& src ) const;
	bool operator != ( const CxProjection& src ) const;

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
