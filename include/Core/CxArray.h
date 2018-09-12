/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXARRAY_H_INCLUDED_
#define _CXARRAY_H_INCLUDED_

#include "xie_core.h"

#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include "Core/TxArray.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxAttachable.h"
#include "Core/IxFileAccess.h"
#include "Core/IxRawFile.h"
#include "Core/TxScanner1D.h"
#include "Core/CxArrayFilter.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxArray : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
	, public IxEquatable
	, public IxAttachable
	, public IxFileAccess
	, public IxRawFile
{
protected:
	TxArray		m_Tag;
	bool		m_IsAttached;

private:
	void _Constructor();

public:
	static CxArray FromTag(const TxArray& src);

	template<class TE> static CxArray From(const xie::CxArrayEx<TE>& src)
	{
		CxArray result(src.Length(), ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}
	template<class TE> static CxArray From(const std::vector<TE>& src)
	{
		CxArray result((int)src.size(), ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}
	template<class TE> static CxArray From(const std::initializer_list<TE>& src)
	{
		CxArray result((int)src.size(), ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}

public:
	CxArray();
	CxArray(CxArray&& src);
	CxArray(const CxArray& src);
	CxArray(int length, TxModel model);
	CxArray(TxCharCPtrA filename);
	CxArray(TxCharCPtrW filename);
	virtual ~CxArray();

	CxArray& operator = ( CxArray&& src );
	CxArray& operator = ( const CxArray& src );
	bool operator == ( const CxArray& src ) const;
	bool operator != ( const CxArray& src ) const;

	virtual TxArray Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxArray& src);

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
	virtual void Attach(const TxArray& src);
	virtual void Attach(const CxArray& src, int index, int length);
	virtual bool IsAttached() const;

public:
	virtual void Resize(int length, TxModel model);
	virtual void Reset();

	virtual void Clear(const void* value, TxModel model);
	template<class TV> void Clear(TV value)
	{
		Clear(&value, xie::ModelOf(value));
	}

	virtual void ClearEx(const void* value, TxModel model, int index, int count);
	template<class TV> void ClearEx(TV value, int index, int count)
	{
		ClearEx(&value, xie::ModelOf(value), index, count);
	}

	virtual CxArray Clone() const;
	virtual CxArray Clone(TxModel model, double scale = 0) const;

	virtual CxArray Child() const;
	virtual CxArray Child(int index, int length) const;

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

	virtual TxStatistics Statistics(int ch = 0) const;

	virtual CxArray Extract	(int index, int length) const;

	virtual CxArrayFilter Filter() const;

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
};

}

#pragma pack(pop)

#endif
