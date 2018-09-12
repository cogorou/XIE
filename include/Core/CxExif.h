/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXEXIF_H_INCLUDED_
#define _CXEXIF_H_INCLUDED_

#include "xie_core.h"

#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/TxExif.h"
#include "Core/TxExifItem.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxAttachable.h"
#include "Core/IxFileAccess.h"
#include "Core/IxRawFile.h"
#include "Core/TxScanner1D.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxExif : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
	, public IxEquatable
	, public IxAttachable
	, public IxFileAccess
	, public IxRawFile
{
protected:
	TxExif	m_Tag;
	bool	m_IsAttached;

private:
	void _Constructor();

public:
	static CxExif FromTag(const TxExif& src);

public:
	CxExif();
	CxExif(CxExif&& src);
	CxExif(const CxExif& src);
	CxExif(int length);
	CxExif(TxCharCPtrA filename);
	CxExif(TxCharCPtrW filename);
	virtual ~CxExif();

	CxExif& operator = ( CxExif&& src );
	CxExif& operator = ( const CxExif& src );
	bool operator == ( const CxExif& src ) const;
	bool operator != ( const CxExif& src ) const;

	virtual TxExif Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxExif& src);

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;

	// IxAttachable
	virtual void Attach(const IxModule& src);
	virtual void Attach(const TxExif& src);
	virtual void Attach(const CxExif& src, int index, int length);
	virtual bool IsAttached() const;

public:
	virtual void Resize(int length);
	virtual void Reset();

	virtual CxExif Clone() const;

public:
	virtual void Load(TxCharCPtrA filename);
	virtual void Load(TxCharCPtrW filename);
	template<class TV> void Load(TxCharCPtrA filename, TV option)
	{
		LoadA(filename, &option, xie::ModelOf(option));
	}
	template<class TV> void Load(TxCharCPtrW filename, TV option)
	{
		LoadW(filename, &option, xie::ModelOf(option));
	}

	virtual void Save(TxCharCPtrA filename) const;
	virtual void Save(TxCharCPtrW filename) const;
	template<class TV> void Save(TxCharCPtrA filename, TV option) const
	{
		SaveA(filename, &option, xie::ModelOf(option));
	}
	template<class TV> void Save(TxCharCPtrW filename, TV option) const
	{
		SaveW(filename, &option, xie::ModelOf(option));
	}

protected:
	// IxFileAccess
	virtual void LoadA(TxCharCPtrA filename, const void* option, TxModel model);
	virtual void LoadW(TxCharCPtrW filename, const void* option, TxModel model);
	virtual void SaveA(TxCharCPtrA filename, const void* option, TxModel model) const;
	virtual void SaveW(TxCharCPtrW filename, const void* option, TxModel model) const;

	// IxRawFile
	virtual void* OpenRawA(TxCharCPtrA filename, int mode);
	virtual void* OpenRawW(TxCharCPtrW filename, int mode);
	virtual void CloseRaw(void* handle);
	virtual void LoadRaw(void* handle);
	virtual void SaveRaw(void* handle) const;

public:
	virtual int Length() const;
	virtual TxModel Model() const;

	virtual       void* Address();
	virtual const void* Address() const;

	virtual       void* operator [] (int index);
	virtual const void* operator [] (int index) const;

	template<class TE> TxScanner1D<TE> Scanner() const
		{
			return TxScanner1D<TE>((TE*)m_Tag.Address, m_Tag.Length, m_Tag.Model);
		}
	template<class TE> TxScanner1D<TE> Scanner(int index, int length) const
		{
			if (index < 0)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(index + length <= m_Tag.Length))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			return TxScanner1D<TE>((TE*)(*this)[index], length, m_Tag.Model);
		}

public:
	virtual ExEndianType EndianType() const;
	virtual CxArrayEx<TxExifItem> GetItems() const;
	virtual CxExif GetPurgedExif() const;

	virtual void GetValue(const TxExifItem& item, HxModule hval) const;
	virtual void SetValue(const TxExifItem& item, HxModule hval);
};

}

#pragma pack(pop)

#endif
