/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSCANNER2D_H_INCLUDED_
#define _TXSCANNER2D_H_INCLUDED_

#include "xie_core.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxModel.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

// ======================================================================
// Parallel

#ifdef _OPENMP
#include <omp.h>
#endif

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ======================================================================
template<class TE> struct TxScanner2D
{
public:
	TE*			Address;
	int			Width;
	int			Height;
	int			Stride;
	TxModel		Model;
	int			Parallel;

private:
	void _Constructor()
	{
		this->Address	= NULL;
		this->Width		= 0;
		this->Height	= 0;
		this->Stride	= 0;
		this->Model		= TxModel::Default();
		this->Parallel	= 0;
	}

public:
	// ============================================================
	TxScanner2D()
	{
		_Constructor();
	}
	// ============================================================
	TxScanner2D(const TxScanner2D<TE>& src)
	{
		_Constructor();
		operator = (src);
	}
	// ============================================================
	TxScanner2D(TE* addr, int width, int height, int stride, TxModel model = ModelOf<TE>())
	{
		_Constructor();
		this->Address	= addr;
		this->Width		= width;
		this->Height	= height;
		this->Stride	= stride;
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
		if (this->Width <= 0) return false;
		if (this->Height <= 0) return false;
		if (this->ElementSize() == 0) return false;
		if (this->Stride < (this->ElementSize() * this->Width)) return false;
		return true;
	}

	// ============================================================
	TxScanner2D<TE>& operator = (const TxScanner2D<TE>& src)
	{
		this->Address	= src.Address;
		this->Width		= src.Width;
		this->Height	= src.Height;
		this->Stride	= src.Stride;
		this->Model		= src.Model;
		this->Parallel	= src.Parallel;
		return *this;
	}
	// ============================================================
	bool operator == ( const TxScanner2D<TE>& other )
	{
		if (this->Address		!= other.Address) return false;
		if (this->Width			!= other.Width) return false;
		if (this->Height		!= other.Height) return false;
		if (this->Stride		!= other.Stride) return false;
		if (this->Model.Type	!= other.Model.Type) return false;
		if (this->Model.Pack	!= other.Model.Pack) return false;
		if (this->Parallel		!= other.Parallel) return false;
		return true;
	}
	// ============================================================
	bool operator != ( const TxScanner2D<TE>& other )
	{
		return !(*this == other);
	}

	// ============================================================
	TE* operator [] (int y)
	{
		return (TE*)(reinterpret_cast<char*>(Address) + (Stride * y));
	}
	// ============================================================
	const TE* operator [] (int y) const
	{
		return (TE*)(reinterpret_cast<char*>(Address) + (Stride * y));
	}

	// ============================================================
	TE& operator () (int y, int x)
	{
		return *(TE*)(reinterpret_cast<char*>(Address) + (Stride * y) + (ElementSize() * x));
	}
	// ============================================================
	const TE& operator () (int y, int x) const
	{
		return *(TE*)(reinterpret_cast<char*>(Address) + (Stride * y) + (ElementSize() * x));
	}

	// ============================================================
	TxScanner2D<TE>& Copy(const xie::TxScanner2D<TE>& src)
	{
		this->ForEach(src,
			[](int y, int x, TE* _dst, TE* _src)
			{
				*_dst = *_src;
			});
		return *this;
	}

	// ============================================================
	TxScanner2D<TE>& Copy(const xie::CxArrayEx<TE>& src)
	{
		int length = src.Length();
		if (this->Width*this->Height != src.Length())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.Address();
		for(int i=0 ; i<length ; i++)
		{
			int y = i / this->Width;
			int x = i % this->Width;
			(*this)(y, x) = *iter++;
		}
		return *this;
	}

	// ============================================================
	TxScanner2D<TE>& Copy(const std::vector<TE>& src)
	{
		int length = (int)src.size();
		if (this->Width*this->Height != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
		{
			int y = i / this->Width;
			int x = i % this->Width;
			(*this)(y, x) = *iter++;
		}
		return *this;
	}

	// ============================================================
	TxScanner2D<TE>& Copy(const std::initializer_list<TE>& src)
	{
		int length = (int)src.size();
		if (this->Width*this->Height != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		auto iter = src.begin();
		for(int i=0 ; i<length ; i++)
		{
			int y = i / this->Width;
			int x = i % this->Width;
			(*this)(y, x) = *iter++;
		}
		return *this;
	}

	// ============================================================
	operator xie::CxArrayEx<TE>() const
	{
		xie::CxArrayEx<TE> result;
		result.Resize(this->Width * this->Height, true);
		auto iter = result.Address();
		TxScanner2D<TE> src = *this;
		src.Parallel = 1;
		src.ForEach(
			[&iter](int y, int x, TE* _src)
			{
				*iter++ = *_src;
			});
		return result;
	}

	// ============================================================
	operator std::vector<TE>() const
	{
		std::vector<TE> result;
		result.resize(this->Width * this->Height);
		auto iter = result.begin();
		TxScanner2D<TE> src = *this;
		src.Parallel = 1;
		src.ForEach(
			[&iter](int y, int x, TE* _src)
			{
				*iter++ = *_src;
			});
		return result;
	}

	// ============================================================
	template<class TFUNC> void ForEach(TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1) { (your code); }
						func(y, x, (TE*)_src1);
						_src1 += _src1_size;
					}
				}
			}
		}
	// ============================================================
	template<class TS2, class TFUNC> void ForEach(TxScanner2D<TS2> src2, TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Width != src2.Width || src1.Height != src2.Height 
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					char* _src2 = (char*)src2[y];	int _src2_size = src2.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1, TS2* _src2) { (your code); }
						func(y, x, (TE*)_src1, (TS2*)_src2);
						_src1 += _src1_size;
						_src2 += _src2_size;
					}
				}
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TFUNC> void ForEach(TxScanner2D<TS2> src2, TxScanner2D<TS3> src3, TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Width != src2.Width || src1.Height != src2.Height ||
				src3.IsValid() == false || src1.Width != src3.Width || src1.Height != src3.Height 
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					char* _src2 = (char*)src2[y];	int _src2_size = src2.ElementSize();
					char* _src3 = (char*)src3[y];	int _src3_size = src3.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1, TS2* _src2, TS3* _src3) { (your code); }
						func(y, x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3);
						_src1 += _src1_size;
						_src2 += _src2_size;
						_src3 += _src3_size;
					}
				}
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TFUNC> void ForEach(TxScanner2D<TS2> src2, TxScanner2D<TS3> src3, TxScanner2D<TS4> src4, TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Width != src2.Width || src1.Height != src2.Height ||
				src3.IsValid() == false || src1.Width != src3.Width || src1.Height != src3.Height ||
				src4.IsValid() == false || src1.Width != src4.Width || src1.Height != src4.Height 
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					char* _src2 = (char*)src2[y];	int _src2_size = src2.ElementSize();
					char* _src3 = (char*)src3[y];	int _src3_size = src3.ElementSize();
					char* _src4 = (char*)src4[y];	int _src4_size = src4.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4) { (your code); }
						func(y, x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4);
						_src1 += _src1_size;
						_src2 += _src2_size;
						_src3 += _src3_size;
						_src4 += _src4_size;
					}
				}
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TFUNC> void ForEach(TxScanner2D<TS2> src2, TxScanner2D<TS3> src3, TxScanner2D<TS4> src4, TxScanner2D<TS5> src5, TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Width != src2.Width || src1.Height != src2.Height ||
				src3.IsValid() == false || src1.Width != src3.Width || src1.Height != src3.Height ||
				src4.IsValid() == false || src1.Width != src4.Width || src1.Height != src4.Height ||
				src5.IsValid() == false || src1.Width != src5.Width || src1.Height != src5.Height 
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					char* _src2 = (char*)src2[y];	int _src2_size = src2.ElementSize();
					char* _src3 = (char*)src3[y];	int _src3_size = src3.ElementSize();
					char* _src4 = (char*)src4[y];	int _src4_size = src4.ElementSize();
					char* _src5 = (char*)src5[y];	int _src5_size = src5.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5) { (your code); }
						func(y, x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4, (TS5*)_src5);
						_src1 += _src1_size;
						_src2 += _src2_size;
						_src3 += _src3_size;
						_src4 += _src4_size;
						_src5 += _src5_size;
					}
				}
			}
		}
	// ============================================================
	template<class TS2, class TS3, class TS4, class TS5, class TS6, class TFUNC> void ForEach(TxScanner2D<TS2> src2, TxScanner2D<TS3> src3, TxScanner2D<TS4> src4, TxScanner2D<TS5> src5, TxScanner2D<TS6> src6, TFUNC func)
		{
			TxScanner2D<TE>& src1 = *this;

			if (src1.IsValid() == false ||
				src2.IsValid() == false || src1.Width != src2.Width || src1.Height != src2.Height ||
				src3.IsValid() == false || src1.Width != src3.Width || src1.Height != src3.Height ||
				src4.IsValid() == false || src1.Width != src4.Width || src1.Height != src4.Height ||
				src5.IsValid() == false || src1.Width != src5.Width || src1.Height != src5.Height ||
				src6.IsValid() == false || src1.Width != src6.Width || src1.Height != src6.Height
				)
			{
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}

			#ifdef _OPENMP
			#pragma omp parallel num_threads(this->Parallel)
			#endif
			{
				#ifdef _OPENMP
				#pragma omp for
				#endif
				for(int y=0 ; y<src1.Height ; y++)
				{
					char* _src1 = (char*)src1[y];	int _src1_size = src1.ElementSize();
					char* _src2 = (char*)src2[y];	int _src2_size = src2.ElementSize();
					char* _src3 = (char*)src3[y];	int _src3_size = src3.ElementSize();
					char* _src4 = (char*)src4[y];	int _src4_size = src4.ElementSize();
					char* _src5 = (char*)src5[y];	int _src5_size = src5.ElementSize();
					char* _src6 = (char*)src6[y];	int _src6_size = src6.ElementSize();
					for(int x=0 ; x<src1.Width ; x++)
					{
						// Lambda: [&](int y, int x, TE* _src1, TS2* _src2, TS3* _src3, TS4* _src4, TS5* _src5, TS6* _src6) { (your code); }
						func(y, x, (TE*)_src1, (TS2*)_src2, (TS3*)_src3, (TS4*)_src4, (TS5*)_src5, (TS6*)_src6);
						_src1 += _src1_size;
						_src2 += _src2_size;
						_src3 += _src3_size;
						_src4 += _src4_size;
						_src5 += _src5_size;
						_src6 += _src6_size;
					}
				}
			}
		}
};

}

#pragma pack(pop)

#endif
