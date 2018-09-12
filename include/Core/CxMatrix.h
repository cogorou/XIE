/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXMATRIX_H_INCLUDED_
#define _CXMATRIX_H_INCLUDED_

#include "xie_core.h"

#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include "Core/TxMatrix.h"
#include "Core/TxPointI.h"
#include "Core/TxSizeI.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxAttachable.h"
#include "Core/IxFileAccess.h"
#include "Core/IxRawFile.h"
#include "Core/TxScanner2D.h"
#include "Core/CxMatrixFilter.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxMatrix : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
	, public IxEquatable
	, public IxAttachable
	, public IxFileAccess
	, public IxRawFile
{
protected:
	TxMatrix	m_Tag;
	bool		m_IsAttached;

private:
	void _Constructor();

public:
	static CxMatrix FromTag(const TxMatrix& src);

	template<class TE> static CxMatrix From(int rows, int cols, const xie::CxArrayEx<TE>& src)
	{
		CxMatrix result(rows, cols, ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}
	template<class TE> static CxMatrix From(int rows, int cols, const std::vector<TE>& src)
	{
		CxMatrix result(rows, cols, ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}
	template<class TE> static CxMatrix From(int rows, int cols, const std::initializer_list<TE>& src)
	{
		CxMatrix result(rows, cols, ModelOf<TE>());
		result.Scanner<TE>().Copy(src);
		return result;
	}

	static CxMatrix PresetRotate(double degree, double axis_x, double axis_y);
	static CxMatrix PresetScale(double sx, double sy);
	static CxMatrix PresetTranslate(double tx, double ty);
	static CxMatrix PresetShear(double degree_x, double degree_y);

public:
	CxMatrix();
	CxMatrix(CxMatrix&& src);
	CxMatrix(const CxMatrix& src);
	CxMatrix(int rows, int cols, TxModel model = TxModel::Default());
	CxMatrix(const TxSizeI& size, TxModel model = TxModel::Default());
	CxMatrix(TxCharCPtrA filename);
	CxMatrix(TxCharCPtrW filename);
	virtual ~CxMatrix();

	CxMatrix& operator = ( CxMatrix&& src );
	CxMatrix& operator = ( const CxMatrix& src );
	bool operator == ( const CxMatrix& src ) const;
	bool operator != ( const CxMatrix& src ) const;

	virtual TxMatrix Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxMatrix& src);

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
	virtual void Attach(const TxMatrix& src);
	virtual bool IsAttached() const;

public:
	virtual void Resize(int rows, int cols, TxModel model = TxModel::Default());
	virtual void Resize(const TxSizeI& size, TxModel model = TxModel::Default());
	virtual void Reset();

	virtual void Eye(double value, int mode);
	virtual void Clear(const void* value, TxModel model);
	template<class TV> void Clear(TV value) { Clear(&value, xie::ModelOf(value)); }

	virtual CxMatrix Clone() const;
	virtual CxMatrix Clone(TxModel model) const;

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
	virtual int Rows() const;
	virtual int Columns() const;

	virtual TxSizeI Size() const;

	virtual TxModel Model() const;
	virtual int Stride() const;

	virtual       void* Address();
	virtual const void* Address() const;

	virtual       void* operator [] ( int row );
	virtual const void* operator [] ( int row ) const;

	virtual	      void* operator () (int row, int col);
	virtual	const void* operator () (int row, int col) const;

	virtual TxStatistics Statistics() const;

	virtual CxArray Extract	(int row, int col, int length, ExScanDir dir) const;

	virtual CxMatrixFilter Filter() const;

public:
	virtual CxMatrix  operator*  (const CxMatrix& val) const;
	virtual CxMatrix& operator*= (const CxMatrix& val);

	virtual double Det() const;
	virtual double Trace() const;
	virtual TxSizeD ScaleFactor() const;

	virtual CxMatrix Invert		() const;
	virtual CxMatrix Submatrix	(int row, int col) const;

	template<class TE> TxScanner2D<TE> Scanner() const
		{
			return TxScanner2D<TE>((TE*)m_Tag.Address, m_Tag.Columns, m_Tag.Rows, m_Tag.Stride, m_Tag.Model);
		}
	template<class TE> TxScanner2D<TE> Scanner(const TxRectangleI& bounds) const
		{
			if (bounds.X < 0 || bounds.Y < 0)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.X + bounds.Width <= m_Tag.Columns))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.Y + bounds.Height <= m_Tag.Rows))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			return TxScanner2D<TE>((TE*)(*this)(bounds.Y, bounds.X), bounds.Width, bounds.Height, m_Tag.Stride, m_Tag.Model);
		}
};

}

#pragma pack(pop)

#endif
