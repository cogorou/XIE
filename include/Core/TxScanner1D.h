/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSCANNER1D_H_INCLUDED_
#define _TXSCANNER1D_H_INCLUDED_

#include "xie_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxModel.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

template<class TE> struct TxScanner1D
{
public:
	TE*			Address;
	int			Length;
	TxModel		Model;

private:
	void _Constructor()
	{
		this->Address	= NULL;
		this->Length	= 0;
		this->Model		= TxModel::Default();
	}

public:
	// ============================================================
	TxScanner1D()
	{
		_Constructor();
	}
	// ============================================================
	TxScanner1D(const TxScanner1D<TE>& src)
	{
		_Constructor();
		operator = (src);
	}
	// ============================================================
	TxScanner1D(TE* addr, int length, TxModel model = ModelOf<TE>())
	{
		_Constructor();
		this->Address	= addr;
		this->Length	= length;
		this->Model		= model;
	}

	// ============================================================
	int ElementSize() const
	{
		int size = TxModel::SizeOf(Model.Type) * Model.Pack;
		if (size == 0)
			return sizeof(TE);
		else
			return size;
	}

	// ============================================================
	bool IsValid() const
	{
		if (this->Address == NULL) return false;
		if (this->Length <= 0) return false;
		if (this->ElementSize() == 0) return false;
		return true;
	}

	// ============================================================
	TxScanner1D<TE>& operator = (const TxScanner1D<TE>& src)
	{
		this->Address	= src.Address;
		this->Length	= src.Length;
		this->Model		= src.Model;
		return *this;
	}
	// ============================================================
	bool operator == ( const TxScanner1D<TE>& other )
	{
		if (this->Address		!= other.Address) return false;
		if (this->Length		!= other.Length) return false;
		if (this->Model.Type	!= other.Model.Type) return false;
		if (this->Model.Pack	!= other.Model.Pack) return false;
		return true;
	}
	// ============================================================
	bool operator != ( const TxScanner1D<TE>& other )
	{
		return !(*this == other);
	}

	// ============================================================
	TE& operator [] (int index)
	{
		return *(TE*)(reinterpret_cast<char*>(this->Address) + (this->ElementSize() * index));
	}
	// ============================================================
	const TE& operator [] (int index) const
	{
		return *(TE*)(reinterpret_cast<char*>(this->Address) + (this->ElementSize() * index));
	}

	// ============================================================
	TxScanner1D<TE>& Copy(const xie::TxScanner1D<TE>& src)
	{
		this->ForEach(src,
			[](int i, TE* _dst, TE* _src)
			{
				*_dst = *_src;
			});
		return *this;
	}

	// ============================================================
	TxScanner1D<TE>& Copy(const xie::CxArrayEx<TE>& src)
	{
		int length = src.Length();
		if (this->Length != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.Address();
		for(int i=0 ; i<length ; i++)
			(*this)[i] = *iter++;
		return *this;
	}

	// ============================================================
	TxScanner1D<TE>& Copy(const std::vector<TE>& src)
	{
		int length = (int)src.size();
		if (this->Length != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
			(*this)[i] = *iter++;
		return *this;
	}

	// ============================================================
	TxScanner1D<TE>& Copy(const std::initializer_list<TE>& src)
	{
		int length = (int)src.size();
		if (this->Length != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
			(*this)[i] = *iter++;
		return *this;
	}

	// ============================================================
	operator xie::CxArrayEx<TE>() const
	{
		xie::CxArrayEx<TE> result;
		result.Resize(this->Length, true);
		for(int i=0 ; i<this->Length ; i++)
			result[i] = (*this)[i];
		return result;
	}

	// ============================================================
	operator std::vector<TE>() const
	{
		std::vector<TE> result;
		result.resize(this->Length);
		for(int i=0 ; i<this->Length ; i++)
			result[i] = (*this)[i];
		return result;
	}

	// ============================================================
	template<class TFUNC> void ForEach(TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1) { (your code); }
				func(x, (TE*)_src1);
				_src1 += _src1_size;
			}
		}
	// ============================================================
	template<class TS2, class TFUNC> void ForEach(TxScanner1D<TS2> src2, TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Length != src2.Length
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			char* _src2 = (char*)src2.Address;	int _src2_size = src2.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2) { (your code); }
				func(x, (TE*)_src1, (TS2*)_src2);
				_src1 += _src1_size;
				_src2 += _src2_size;
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TFUNC> void ForEach(TxScanner1D<TS2> src2, TxScanner1D<TS3> src3, TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Length != src2.Length ||
				src3.IsValid() == false || src1.Length != src3.Length
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			char* _src2 = (char*)src2.Address;	int _src2_size = src2.ElementSize();
			char* _src3 = (char*)src3.Address;	int _src3_size = src3.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3) { (your code); }
				func(x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3);
				_src1 += _src1_size;
				_src2 += _src2_size;
				_src3 += _src3_size;
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TFUNC> void ForEach(TxScanner1D<TS2> src2, TxScanner1D<TS3> src3, TxScanner1D<TS4> src4, TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Length != src2.Length ||
				src3.IsValid() == false || src1.Length != src3.Length ||
				src4.IsValid() == false || src1.Length != src4.Length
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			char* _src2 = (char*)src2.Address;	int _src2_size = src2.ElementSize();
			char* _src3 = (char*)src3.Address;	int _src3_size = src3.ElementSize();
			char* _src4 = (char*)src4.Address;	int _src4_size = src4.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4) { (your code); }
				func(x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4);
				_src1 += _src1_size;
				_src2 += _src2_size;
				_src3 += _src3_size;
				_src4 += _src4_size;
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TFUNC> void ForEach(TxScanner1D<TS2> src2, TxScanner1D<TS3> src3, TxScanner1D<TS4> src4, TxScanner1D<TS5> src5, TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Length != src2.Length ||
				src3.IsValid() == false || src1.Length != src3.Length ||
				src4.IsValid() == false || src1.Length != src4.Length ||
				src5.IsValid() == false || src1.Length != src5.Length
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			char* _src2 = (char*)src2.Address;	int _src2_size = src2.ElementSize();
			char* _src3 = (char*)src3.Address;	int _src3_size = src3.ElementSize();
			char* _src4 = (char*)src4.Address;	int _src4_size = src4.ElementSize();
			char* _src5 = (char*)src5.Address;	int _src5_size = src5.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5) { (your code); }
				func(x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4, (TS5*)_src5);
				_src1 += _src1_size;
				_src2 += _src2_size;
				_src3 += _src3_size;
				_src4 += _src4_size;
				_src5 += _src5_size;
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TS6, class TFUNC> void ForEach(TxScanner1D<TS2> src2, TxScanner1D<TS3> src3, TxScanner1D<TS4> src4, TxScanner1D<TS5> src5, TxScanner1D<TS6> src6, TFUNC func)
		{
			TxScanner1D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Length != src2.Length ||
				src3.IsValid() == false || src1.Length != src3.Length ||
				src4.IsValid() == false || src1.Length != src4.Length ||
				src5.IsValid() == false || src1.Length != src5.Length ||
				src6.IsValid() == false || src1.Length != src6.Length
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			char* _src1 = (char*)src1.Address;	int _src1_size = src1.ElementSize();
			char* _src2 = (char*)src2.Address;	int _src2_size = src2.ElementSize();
			char* _src3 = (char*)src3.Address;	int _src3_size = src3.ElementSize();
			char* _src4 = (char*)src4.Address;	int _src4_size = src4.ElementSize();
			char* _src5 = (char*)src5.Address;	int _src5_size = src5.ElementSize();
			char* _src6 = (char*)src6.Address;	int _src6_size = src6.ElementSize();
			for(int x=0 ; x<src1.Length ; x++)
			{
				// Lambda: [&](int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5, TS6* _src6) { (your code); }
				func(x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4, (TS5*)_src5, (TS6*)_src6);
				_src1 += _src1_size;
				_src2 += _src2_size;
				_src3 += _src3_size;
				_src4 += _src4_size;
				_src5 += _src5_size;
				_src6 += _src6_size;
			}
		}
};

}

#pragma pack(pop)

#endif
