/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRAWFILEHEADER_H_INCLUDED_
#define _TXRAWFILEHEADER_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxRawFileHeader
{
	int				Signature;
	int				Version;
	int				Revision;
	char			ClassName[256];
	int				Terminal;

#if defined(__cplusplus)
	static inline TxRawFileHeader Default()
	{
		TxRawFileHeader result;
		result.Signature = XIE_MODULE_ID;
		result.Version = XIE_VER;
		result.Revision = 0;
		int count = (int)sizeof(result.ClassName);
		for(int i=0 ; i<count ; i++)
			result.ClassName[i] = 0;
		result.Terminal = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRawFileHeader();
	TxRawFileHeader(TxCharCPtrA name);

	bool operator == (const TxRawFileHeader& cmp) const;
	bool operator != (const TxRawFileHeader& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
