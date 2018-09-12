/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXARRAYEX_H_INCLUDED_
#define _CXARRAYEX_H_INCLUDED_

#include "xie_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include <vector>
#include <initializer_list>
#include <utility>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

template<class TE> class CxArrayEx
{
protected:
	TE*		m_Address;
	int		m_Length;
	bool	m_IsAttached;
	bool	m_UseInternalAllocator;

private:
	// ============================================================
	void _Constructor()
	{
		m_Address = NULL;
		m_Length = 0;
		m_IsAttached = false;
		m_UseInternalAllocator = false;
	}

public:
	// ============================================================
	CxArrayEx()
	{
		_Constructor();
	}
	// ============================================================
	CxArrayEx(int length, bool use_internal_allocator = false)
	{
		_Constructor();
		Resize(length, use_internal_allocator);
	}
	// ============================================================
	CxArrayEx(const std::vector<TE>& src)
	{
		_Constructor();
		operator = (src);
	}
	// ============================================================
	CxArrayEx(const std::initializer_list<TE>& src)
	{
		_Constructor();
		operator = (src);
	}
	// ============================================================
	CxArrayEx(CxArrayEx&& src)
	{
		_Constructor();
		operator = (std::move(src));
	}
	// ============================================================
	CxArrayEx(const CxArrayEx& src)
	{
		_Constructor();
		operator = (src);
	}
	// ============================================================
	virtual ~CxArrayEx()
	{
		Dispose();
	}

	// ============================================================
	CxArrayEx& operator = (const std::vector<TE>& src)
	{
		int length = (int)src.size();
		Resize(length);
		auto dst = this->Address();
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
			*dst++ = *iter++;
		return *this;
	}
	// ============================================================
	CxArrayEx& operator = (const std::initializer_list<TE>& src)
	{
		int length = (int)src.size();
		Resize(length);
		auto dst = this->Address();
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
			*dst++ = *iter++;
		return *this;
	}
	// ============================================================
	CxArrayEx& operator = ( CxArrayEx&& src )
	{
		if (this == &src) return *this;
		this->Dispose();
		this->m_Address		= src.m_Address;
		this->m_Length		= src.m_Length;
		this->m_IsAttached	= src.m_IsAttached;
		src.m_IsAttached	= true;
		this->m_UseInternalAllocator = src.m_UseInternalAllocator;
		return *this;
	}
	// ============================================================
	CxArrayEx& operator = ( const CxArrayEx& src )
	{
		if (this == &src) return *this;
		Resize(src.Length(), src.m_UseInternalAllocator);
		if (m_Address != NULL)
		{
			TE*			dst_addr = this->m_Address;
			const TE*	src_addr = src.m_Address;
			for(int i=0 ; i<m_Length ; i++)
				dst_addr[i] = src_addr[i];
		}
		return *this;
	}

	// ============================================================
	operator std::vector<TE>() const
	{
		int length = this->Length();
		std::vector<TE> result;
		result.resize(length);
		for(int i=0 ; i<length ; i++)
			result[i] = (*this)[i];
		return result;
	}

	// ============================================================
	CxArrayEx operator + (const CxArrayEx& src) const
	{
		CxArrayEx<TE> result(this->Length() + src.Length());
		int index = 0;
		for(int i=0 ; i<this->Length() ; i++)
			result[index++] = (*this)[i];
		for(int i=0 ; i<src.Length() ; i++)
			result[index++] = src[i];
		return result;
	}
	// ============================================================
	CxArrayEx& operator += (const CxArrayEx& src)
	{
		CxArrayEx<TE> result(this->Length() + src.Length());
		int index = 0;
		for(int i=0 ; i<this->Length() ; i++)
			result[index++] = (*this)[i];
		for(int i=0 ; i<src.Length() ; i++)
			result[index++] = src[i];
		(*this) = result;
		return *this;
	}

public:
	// ============================================================
	virtual void Dispose()
	{
		if (m_IsAttached == false && m_Address != NULL)
		{
			if (m_UseInternalAllocator)
				Axi::MemoryFree(m_Address);
			else
				delete [] m_Address;
		}
		m_Address = NULL;
		m_Length = 0;
		m_IsAttached = false;
		m_UseInternalAllocator = false;
	}

	// ============================================================
	virtual bool IsValid() const
	{
		if (m_Address == NULL) return false;
		if (m_Length <= 0) return false;
		return true;
	}

	// ============================================================
	virtual void Resize(int length, bool use_internal_allocator = false)
	{
		Dispose();

		if (length == 0) return;
		if (length < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		if (use_internal_allocator)
			m_Address = (TE*)Axi::MemoryAlloc((TxIntPtr)length * sizeof(TE));
		else
			m_Address = new TE[length];
		if (m_Address == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		m_Length = length;
		m_UseInternalAllocator = use_internal_allocator;
	}

	// ============================================================
	virtual void Clear(TE value)
	{
		if (m_Address == NULL) return;
		for(int i=0 ; i<m_Length ; i++)
			m_Address[i] = value;
	}

	// ============================================================
	virtual CxArrayEx Extract(int index, int length) const
	{
		if (m_Address == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (!(0 <= index && index < m_Length))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (!(index + length <= m_Length))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		CxArrayEx<TE> result(length);
		for(int i=0 ; i<length ; i++)
			result[i] = (*this)[index + i];
		return result;
	}

	// ============================================================
	virtual TE* Address()
	{
		return m_Address;
	}
	// ============================================================
	virtual const TE* Address() const
	{
		return m_Address;
	}
	// ============================================================
	virtual int Length() const
	{
		return m_Length;
	}

	// ============================================================
	virtual       TE& operator [] (int index)
	{
		if (m_Address == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (!(0 <= index && index <m_Length))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		return m_Address[index];
	}
	// ============================================================
	virtual const TE& operator [] (int index) const
	{
		if (m_Address == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (!(0 <= index && index <m_Length))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		return m_Address[index];
	}

	// ============================================================
	template<class TFUNC> void ForEach(TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1) { (your code); }
				func(x, &_src1[x]);
			}
		}
	// ============================================================
	template<class TS2, class TFUNC> void ForEach(const CxArrayEx<TS2>& src2, TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (src1.Length() != src2.Length())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			TS2*	_src2 = (TS2*)src2.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2) { (your code); }
				func(x, &_src1[x], &_src2[x]);
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TFUNC> void ForEach(const CxArrayEx<TS2>& src2, const CxArrayEx<TS3>& src3, TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false ||
				src3.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (src1.Length() != src2.Length() ||
				src1.Length() != src3.Length())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			TS2*	_src2 = (TS2*)src2.Address();
			TS3*	_src3 = (TS3*)src3.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3) { (your code); }
				func(x, &_src1[x], &_src2[x], &_src3[x]);
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TFUNC> void ForEach(const CxArrayEx<TS2>& src2, const CxArrayEx<TS3>& src3, const CxArrayEx<TS4>& src4, TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false ||
				src3.IsValid() == false ||
				src4.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (src1.Length() != src2.Length() ||
				src1.Length() != src3.Length() ||
				src1.Length() != src4.Length())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			TS2*	_src2 = (TS2*)src2.Address();
			TS3*	_src3 = (TS3*)src3.Address();
			TS4*	_src4 = (TS4*)src4.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4) { (your code); }
				func(x, &_src1[x], &_src2[x], &_src3[x], &_src4[x]);
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TFUNC> void ForEach(const CxArrayEx<TS2>& src2, const CxArrayEx<TS3>& src3, const CxArrayEx<TS4>& src4, const CxArrayEx<TS5>& src5, TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false ||
				src3.IsValid() == false ||
				src4.IsValid() == false ||
				src5.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (src1.Length() != src2.Length() ||
				src1.Length() != src3.Length() ||
				src1.Length() != src4.Length() ||
				src1.Length() != src5.Length())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			TS2*	_src2 = (TS2*)src2.Address();
			TS3*	_src3 = (TS3*)src3.Address();
			TS4*	_src4 = (TS4*)src4.Address();
			TS5*	_src5 = (TS5*)src5.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5) { (your code); }
				func(x, &_src1[x], &_src2[x], &_src3[x], &_src4[x], &_src5[x]);
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TS6, class TFUNC> void ForEach(const CxArrayEx<TS2>& src2, const CxArrayEx<TS3>& src3, const CxArrayEx<TS4>& src4, const CxArrayEx<TS5>& src5, const CxArrayEx<TS6>& src6, TFUNC func)
		{
			CxArrayEx<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false ||
				src3.IsValid() == false ||
				src4.IsValid() == false ||
				src5.IsValid() == false ||
				src6.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (src1.Length() != src2.Length() ||
				src1.Length() != src3.Length() ||
				src1.Length() != src4.Length() ||
				src1.Length() != src5.Length() ||
				src1.Length() != src6.Length())
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			TE*		_src1 = src1.Address();
			TS2*	_src2 = (TS2*)src2.Address();
			TS3*	_src3 = (TS3*)src3.Address();
			TS4*	_src4 = (TS4*)src4.Address();
			TS5*	_src5 = (TS5*)src5.Address();
			TS6*	_src6 = (TS6*)src6.Address();
			for(int x=0 ; x<src1.Length() ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5, TS6* _src6) { (your code); }
				func(x, &_src1[x], &_src2[x], &_src3[x], &_src4[x], &_src5[x], &_src6[x]);
			}
		}
};

}

#pragma pack(pop)

#endif
