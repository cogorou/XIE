/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXIMAGE_H_INCLUDED_
#define _CXIMAGE_H_INCLUDED_

#include "xie_core.h"

#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include "Core/TxImage.h"
#include "Core/TxImageSize.h"
#include "Core/TxExif.h"
#include "Core/TxPointI.h"
#include "Core/TxSizeI.h"
#include "Core/TxRectangleI.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxAttachable.h"
#include "Core/IxLockable.h"
#include "Core/IxFileAccess.h"
#include "Core/IxRawFile.h"
#include "Core/TxScanner2D.h"
#include "Core/CxImageFilter.h"
#include "Core/CxArrayEx.h"
#include <vector>
#include <initializer_list>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxImage : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
	, public IxEquatable
	, public IxAttachable
	, public IxLockable
	, public IxFileAccess
	, public IxRawFile
{
protected:
	TxImage		m_Tag;
	TxExif		m_Exif;
	bool		m_IsAttached;
	bool		m_IsLocked;

private:
	void _Constructor();

public:
	static CxImage FromTag(const TxImage& src);

	template<class TE> static CxImage From(int width, int height, const xie::CxArrayEx<TE>& src)
	{
		CxImage result(width, height, ModelOf<TE>(), 1, 1);
		result.Scanner<TE>(0).Copy(src);
		return result;
	}
	template<class TE> static CxImage From(int width, int height, const std::vector<TE>& src)
	{
		CxImage result(width, height, ModelOf<TE>(), 1, 1);
		result.Scanner<TE>(0).Copy(src);
		return result;
	}
	template<class TE> static CxImage From(int width, int height, const std::initializer_list<TE>& src)
	{
		CxImage result(width, height, ModelOf<TE>(), 1, 1);
		result.Scanner<TE>(0).Copy(src);
		return result;
	}

public:
	CxImage();
	CxImage(CxImage&& src);
	CxImage(const CxImage& src);
	CxImage(const TxImageSize& size, int packing_size = XIE_IMAGE_PACKING_SIZE);
	CxImage(int width, int height, TxModel model, int channels, int packing_size = XIE_IMAGE_PACKING_SIZE);
	CxImage(TxCharCPtrA filename, bool unpack = false);
	CxImage(TxCharCPtrW filename, bool unpack = false);
	virtual ~CxImage();

	CxImage& operator = ( CxImage&& src );
	CxImage& operator = ( const CxImage& src );
	bool operator == ( const CxImage& src ) const;
	bool operator != ( const CxImage& src ) const;

	virtual TxImage Tag() const;
	virtual void* TagPtr() const;

	virtual TxExif Exif() const;
	virtual void Exif(const TxExif& value);

	virtual void ExifCopy(const TxExif& exif, bool ltc = true);

protected:
	virtual void MoveFrom(CxImage& src);

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
	virtual void Attach(const TxImage& src);
	virtual void Attach(const CxImage& src, const TxRectangleI& bounds);
	virtual void Attach(const CxImage& src, int ch);
	virtual void Attach(const CxImage& src, int ch, const TxRectangleI& bounds);
	virtual bool IsAttached() const;

	// IxLockable
	virtual void Lock();
	virtual void Unlock();
	virtual bool IsLocked() const;

public:
	virtual void Resize(const TxImageSize& size, int packing_size = XIE_IMAGE_PACKING_SIZE);
	virtual void Resize(int width, int height, TxModel model, int channels, int packing_size = XIE_IMAGE_PACKING_SIZE);
	virtual void Reset();

	virtual void Clear(const void* value, TxModel model, HxModule hmask = NULL);
	template<class TV> void Clear(TV value, HxModule hmask = NULL)
	{
		Clear(&value, xie::ModelOf(value));
	}

	virtual void ClearEx(const void* value, TxModel model, int index, int count, HxModule hmask = NULL);
	template<class TV> void ClearEx(TV value, int index, int count, HxModule hmask = NULL)
	{
		ClearEx(&value, xie::ModelOf(value), index, count);
	}

	virtual CxImage Clone() const;
	virtual CxImage Clone(TxModel model, int channels = 0, double scale = 0) const;

	virtual CxImage Child() const;
	virtual CxImage Child(const TxRectangleI& bounds) const;
	virtual CxImage Child(int ch) const;
	virtual CxImage Child(int ch, const TxRectangleI& bounds) const;

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
	virtual int Width() const;
	virtual int Height() const;
	virtual int Channels() const;
	virtual TxModel Model() const;
	virtual int Stride() const;

	virtual int Depth() const;
	virtual void Depth(int value);

	virtual TxSizeI Size() const;
	virtual TxImageSize ImageSize() const;

	virtual       void* Address(int ch);
	virtual const void* Address(int ch) const;

	virtual       void* operator [] (int ch);
	virtual const void* operator [] (int ch) const;

	virtual	      void* operator () (int ch, int y, int x);
	virtual	const void* operator () (int ch, int y, int x) const;

	virtual int CalcDepth			(int ch, HxModule hmask = NULL) const;
	virtual TxStatistics Statistics	(int ch, HxModule hmask = NULL) const;

	virtual CxArray Extract		(int ch, int sy, int sx, int length, ExScanDir dir, HxModule hmask = NULL) const;

	virtual CxImageFilter Filter() const;
	virtual CxImageFilter Filter(HxModule hmask) const;

	template<class TE> TxScanner2D<TE> Scanner(int ch) const
		{
			return TxScanner2D<TE>((TE*)(*this)[ch], m_Tag.Width, m_Tag.Height, m_Tag.Stride, m_Tag.Model);
		}
	template<class TE> TxScanner2D<TE> Scanner(int ch, const TxRectangleI& bounds) const
		{
			if (!(0 <= ch && ch < m_Tag.Channels))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (bounds.X < 0 || bounds.Y < 0)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.X + bounds.Width <= m_Tag.Width))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(bounds.Y + bounds.Height <= m_Tag.Height))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			return TxScanner2D<TE>((TE*)(*this)(ch, bounds.Y, bounds.X), bounds.Width, bounds.Height, m_Tag.Stride, m_Tag.Model);
		}
};

}

#pragma pack(pop)

#endif
