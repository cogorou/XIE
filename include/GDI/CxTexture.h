/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTEXTURE_H_INCLUDED_
#define _CXTEXTURE_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxModule.h"
#include "Core/IxDisposable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

class XIE_EXPORT_CLASS CxTexture : public CxModule
	, public IxDisposable
{
private:
	unsigned int	m_TextureID;

private:
	void _Constructor();

public:
	CxTexture();
	CxTexture(const CxTexture& src);
	virtual ~CxTexture();

	CxTexture& operator = ( const CxTexture& src );
	bool operator == ( const CxTexture& cmp ) const;
	bool operator != ( const CxTexture& cmp ) const;

public:
	virtual void Dispose();
	virtual bool IsValid() const;

	virtual void Setup();
	virtual unsigned int TextureID() const;
};

}
}

#pragma pack(pop)

#endif
