/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_exif.h"
#include "Core/CxExif.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"
#include "File/api_file.h"
#include <typeinfo>
#include <math.h>
#include <stdio.h>

namespace xie
{

static const char* g_ClassName = "CxExif";

static const int TiffOffset = 6;
static const int MarkerOffset = 8;
static const int IFDStructSize = 12;

// ============================================================
void CxExif::_Constructor()
{
	m_Tag = TxExif::Default();
	m_IsAttached = false;
}

// ============================================================
CxExif CxExif::FromTag(const TxExif& src)
{
	CxExif dst;
	dst.Attach(src);
	return dst;
}

// ============================================================
CxExif::CxExif()
{
	_Constructor();
}

// ============================================================
CxExif::CxExif(CxExif&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxExif::CxExif(const CxExif& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxExif::CxExif(int length)
{
	_Constructor();
	Resize(length);
}

// ============================================================
CxExif::CxExif(TxCharCPtrA filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxExif::CxExif(TxCharCPtrW filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxExif::~CxExif()
{
	Dispose();
}

// ============================================================
CxExif& CxExif::operator = ( CxExif&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxExif& CxExif::operator = ( const CxExif& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxExif::operator == ( const CxExif& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxExif::operator != ( const CxExif& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
TxExif CxExif::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxExif::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
void CxExif::Dispose()
{
	if (IsAttached() == false)
	{
		if (m_Tag.Address != NULL)
			Axi::MemoryFree( m_Tag.Address );
	}
	m_Tag = TxExif::Default();
	m_IsAttached = false;
}

// ================================================================================
void CxExif::MoveFrom(CxExif& src)
{
	if (this == &src) return;

	CxExif& dst = *this;
	dst.Dispose();
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;
}

// ============================================================
void CxExif::CopyTo(IxModule& dst) const
{
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxExif::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxExif>(src))
	{
		auto&	_src = static_cast<const CxExif&>(src);
		auto&	_dst = *this;

		_dst.Dispose();
		if (_src.IsValid())
		{
			_dst.Resize(_src.Length());
			auto size = (TxIntPtr)_dst.Length() * _dst.Model().Size();
			memcpy(_dst.Address(), _src.Address(), size);
		}

		return;
	}
	if (auto _src = xie::Axi::SafeCast<IxConvertible>(&src))
	{
		_src->CopyTo(*this);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxExif::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxExif>(src))
	{
		auto& _src = static_cast<const CxExif&>(src);
		auto& _dst = *this;

		if (_dst.m_Tag.Model	!= _src.m_Tag.Model) return false;
		if (_dst.m_Tag.Length	!= _src.m_Tag.Length) return false;

		size_t	size = _dst.Model().Size() * _dst.Length();

		if (size != 0 &&
			_src.Address() != NULL &&
			_dst.Address() != NULL)
		{
			if (memcmp(_dst.Address(), _src.Address(), size) != 0) return false;
		}
		return true;
	}
	return false;
}

// ============================================================
bool CxExif::IsValid() const
{
	if (m_Tag.IsValid() == false) return false;
	return true;
}

// ============================================================
bool CxExif::IsAttached() const
{
	return m_IsAttached;
}

// ============================================================
void CxExif::Attach(const IxModule& src)
{
	this->Dispose();

	if (xie::Axi::ClassIs<CxExif>(src))
	{
		const CxExif& _src = static_cast<const CxExif&>(src);
		this->m_IsAttached = true;
		this->m_Tag = _src.m_Tag;
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxExif::Attach(const TxExif& src)
{
	this->Dispose();
	this->m_IsAttached = true;
	this->m_Tag = src;
}

// ============================================================
void CxExif::Attach(const CxExif& src, int index, int length)
{
	this->Dispose();

	TxExif src_tag = src.Tag();
	if (src_tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (index > src_tag.Length - 1)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (index + length > src_tag.Length)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	
	this->m_IsAttached = true;
	this->m_Tag = TxExif((void*)src[index], length, src.Model());
}

// ============================================================
void CxExif::Resize(int length)
{
	this->Dispose();

	if (length == 0) return;

	if (length < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		auto model = TxModel::U8(1);
		int size = model.Size();
		if (size < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		void* addr = Axi::MemoryAlloc( (TxIntPtr)length * size );
		if (addr == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		m_Tag.Address	= addr;
		m_Tag.Length	= length;
		m_Tag.Model		= model;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxExif::Reset()
{
	if (m_Tag.Address != NULL &&
		m_Tag.Length > 0 &&
		m_Tag.Model.Size() > 0)
	{
		memset(m_Tag.Address, 0, m_Tag.Length * m_Tag.Model.Size());
	}
}

// ============================================================
CxExif CxExif::Clone() const
{
	CxExif clone;
	clone.CopyFrom(*this);
	return clone;
}

// ============================================================
void CxExif::Load(TxCharCPtrA filename)
{
	LoadA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxExif::Load(TxCharCPtrW filename)
{
	LoadW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxExif::Save(TxCharCPtrA filename) const
{
	SaveA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxExif::Save(TxCharCPtrW filename) const
{
	SaveW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxExif::LoadA(TxCharCPtrA filename, const void* option, TxModel model)
{
	this->Dispose();

	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)strlen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (option != NULL)
	{
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringA::EndsWith(filename, ".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadRawA(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxExif::LoadW(TxCharCPtrW filename, const void* option, TxModel model)
{
	this->Dispose();

	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)wcslen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (option != NULL)
	{
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringW::EndsWith(filename, L".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadRawW(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxExif::SaveA(TxCharCPtrA filename, const void* option, TxModel model) const
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)strlen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (option != NULL)
	{
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringA::EndsWith(filename, ".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveRawA(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxExif::SaveW(TxCharCPtrW filename, const void* option, TxModel model) const
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)wcslen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (option != NULL)
	{
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringW::EndsWith(filename, L".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveRawW(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void* CxExif::OpenRawA(TxCharCPtrA filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	FILE* stream = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		stream = fopen( filename, "rb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		stream = fopen( filename, "wb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return stream;
}

// ============================================================
void* CxExif::OpenRawW(TxCharCPtrW filename, int mode)
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

#ifndef _MSC_VER
	typedef FILE* WFOPEN(const wchar_t* filename_w, const wchar_t* mode_w);
	std::function<WFOPEN> _wfopen = [](const wchar_t* filename_w, const wchar_t* mode_w)
		{
			CxStringA filename_a = CxStringA::From(filename_w);
			CxStringA mode_a = CxStringA::From(mode_w);
			return fopen( filename_a.Address(), mode_a.Address() );
		};
#endif

	FILE* stream = NULL;

	switch(mode)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	case 0:
		stream = _wfopen( filename, L"rb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	case 1:
		stream = _wfopen( filename, L"wb" );
		if( stream == NULL )
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	return stream;
}

// ============================================================
void CxExif::CloseRaw(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

// ============================================================
void CxExif::LoadRaw(void* handle)
{
	Dispose();

	FILE* stream = (FILE*)handle;
	if (stream == NULL)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	size_t	uiReadSize;

	// ----------------------------------------------------------------------
	// ヘッダー読み込み.
	// ----------------------------------------------------------------------
	{
		TxRawFileHeader header;
		size_t header_size = sizeof(header);
		memset(&header, 0, header_size);

		uiReadSize = fread(&header, header_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		if (header.Signature != XIE_MODULE_ID)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (strcmp(header.ClassName, g_ClassName) != 0)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	}

	// ----------------------------------------------------------------------
	// データ読み込み.
	// ----------------------------------------------------------------------
	{
		CxExif* dst = this;

		// データ識別情報.
		{
			int			length = 0;
			TxModel		model = TxModel::Default();

			uiReadSize = fread(&length, sizeof(length), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiReadSize = fread(&model, sizeof(model), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			if (model != TxModel::U8(1))
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			dst->Resize(length);
		}

		// データ読み込み.
		if (dst->Address() != NULL)
		{
			uiReadSize = fread((*dst)[0], dst->Model().Size(), dst->Length(), stream);
			if ((int)uiReadSize != dst->Length())
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
void CxExif::SaveRaw(void* handle) const
{
	FILE* stream = (FILE*)handle;
	if (stream == NULL)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	size_t	uiWriteSize;

	// ----------------------------------------------------------------------
	// ヘッダー初期化.
	// ----------------------------------------------------------------------
	TxRawFileHeader header = TxRawFileHeader(g_ClassName);

	// ----------------------------------------------------------------------
	// データ書き込み.
	// ----------------------------------------------------------------------
	{
		const CxExif* src = this;

		// ヘッダー書き込み.
		uiWriteSize = fwrite(&header, sizeof(TxRawFileHeader), 1, stream);
		if (uiWriteSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// データ識別情報.
		{
			int			length = src->Length();
			TxModel		model = src->Model();

			uiWriteSize = fwrite(&length, sizeof(length), 1, stream);
			if (uiWriteSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiWriteSize = fwrite(&model, sizeof(model), 1, stream);
			if (uiWriteSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		// データ書き込み.
		uiWriteSize = fwrite(src->Address(), src->Model().Size(), src->Length(), stream);
		if ((int)uiWriteSize != src->Length())
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ============================================================
int CxExif::Length() const
{
	return m_Tag.Length;
}

// ============================================================
TxModel CxExif::Model() const
{
	return m_Tag.Model;
}

// ============================================================
void* CxExif::Address()
{
	return m_Tag.Address;
}

// ============================================================
const void* CxExif::Address() const
{
	return m_Tag.Address;
}

// ============================================================
void* CxExif::operator [] (int index)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + (size * index);
}

// ============================================================
const void* CxExif::operator [] (int index) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + (size * index);
}

// ============================================================
ExEndianType CxExif::EndianType() const
{
	if (IsValid() == false)
	{
		return ExEndianType::None;
	}
	else
	{
		auto buf = (unsigned char*)m_Tag.Address;

		// 6~7: endian type
		auto endian = (unsigned short)(buf[TiffOffset + 0] << 8 | buf[TiffOffset + 1]);
		switch (endian)
		{
		case 0x4D4D: return ExEndianType::LE;
		case 0x4949: return ExEndianType::BE;
		}

		return ExEndianType::None;
	}
}

// ============================================================
CxArrayEx<TxExifItem> CxExif::GetItems() const
{
	CxArrayEx<TxExifItem> result;
	if (IsValid() == false)
		return result;

	// endian type
	auto endian_type = EndianType();
	if (endian_type == ExEndianType::None)
		return result;

	// --------------------------------------------------
	std::vector<TxExifItem> base_items;
	std::vector<TxExifItem> exif_items;

	auto scan = this->Scanner<unsigned char>();
	
	// 8~15: marker, offset, count
	auto tag_marker = (unsigned short)fnPRV_Exif_GetValueU16(scan, TiffOffset + 2, endian_type);
	auto dir_offset = (int)fnPRV_Exif_GetValueU32(scan, TiffOffset + 4, endian_type);
	auto dir_count = (int)fnPRV_Exif_GetValueU16(scan, TiffOffset + 8, endian_type);

	// base_offset は、タグマーカー(指標 8)から起算する.
	int base_offset = MarkerOffset + dir_offset;
	int exif_offset = 0;

	// 基本部の収集.
	for(int i=0 ; i<dir_count ; i++, base_offset+=IFDStructSize)
	{
		if (!(base_offset < (scan.Length - IFDStructSize))) break;
		auto item = fnPRV_Exif_GetItem(scan, base_offset, endian_type);
		base_items.push_back(item);

		switch(item.ID)
		{
		case 0x8769:	// Exif IFD pointer
			{
				int index = TiffOffset + item.ValueOrIndex;
				int count  = (int)fnPRV_Exif_GetValueU16(scan, index, endian_type);
				int offset = index + 2;
				exif_offset = index;

				for (int x = 0; x < count; x++, offset += IFDStructSize)
				{
					if (!(offset < (scan.Length - IFDStructSize))) break;
					auto exif_item = fnPRV_Exif_GetItem(scan, offset, endian_type);
					exif_items.push_back(exif_item);
				}
			}
			break;
		}
	}

	// --------------------------------------------------
	result.Resize( (int)(base_items.size() + exif_items.size()), true );
	{
		int index = 0;

		for(auto iter=base_items.begin() ; iter!=base_items.end() ; iter++, index++)
			result[index] = *iter;

		for(auto iter=exif_items.begin() ; iter!=exif_items.end() ; iter++, index++)
			result[index] = *iter;
	}

	return result;
}

// ============================================================
CxExif CxExif::GetPurgedExif() const
{
	CxExif result;
	if (IsValid() == false)
		return result;

	// endian type
	auto endian_type = EndianType();
	if (endian_type == ExEndianType::None)
		return result;

	// --------------------------------------------------
	std::vector<TxExifItem> base_items;
	std::vector<TxExifItem> exif_items;

	auto src_scan = this->Scanner<unsigned char>();
	
	// 8~15: marker, offset, count
	auto tag_marker = (unsigned short)fnPRV_Exif_GetValueU16(src_scan, TiffOffset + 2, endian_type);
	auto dir_offset = (int)fnPRV_Exif_GetValueU32(src_scan, TiffOffset + 4, endian_type);
	auto dir_count = (int)fnPRV_Exif_GetValueU16(src_scan, TiffOffset + 8, endian_type);

	// base_offset は、タグマーカー(指標 8)から起算する.
	int base_offset = MarkerOffset + dir_offset;
	int exif_offset = 0;

	// 基本部の収集.
	for(int i=0 ; i<dir_count ; i++, base_offset+=IFDStructSize)
	{
		if (!(base_offset < (src_scan.Length - IFDStructSize))) break;
		auto item = fnPRV_Exif_GetItem(src_scan, base_offset, endian_type);
		base_items.push_back(item);

		switch(item.ID)
		{
		case 0x8769:	// Exif IFD pointer
			{
				int index = TiffOffset + item.ValueOrIndex;
				int count  = (int)fnPRV_Exif_GetValueU16(src_scan, index, endian_type);
				int offset = index + 2;
				exif_offset = index;

				for (int x = 0; x < count; x++, offset += IFDStructSize)
				{
					if (!(offset < (src_scan.Length - IFDStructSize))) break;
					auto exif_item = fnPRV_Exif_GetItem(src_scan, offset, endian_type);
					exif_items.push_back(exif_item);
				}
			}
			break;
		}
	}

	// --------------------------------------------------
	result.Resize(this->Length());
	result.Reset();
	auto dst_scan = result.Scanner<unsigned char>();

	// --------------------------------------------------
	// ヘッダー部の複製:
	memcpy(&dst_scan[0], &src_scan[0], (MarkerOffset + dir_offset));

	// --------------------------------------------------
	// 基本部の複製.
	int base_ignores = 0;
	for(int i=0 ; i<(int)base_items.size() ; i++)
	{
		auto item = base_items[i];
		int src_offset = item.Offset;
		int dst_offset = item.Offset - (IFDStructSize * base_ignores);

		switch(item.ID)
		{
		default:
			memcpy(&dst_scan[dst_offset], &src_scan[src_offset], IFDStructSize);
			fnPRV_Exif_CopyRelatedItem(dst_scan, src_scan, item, TiffOffset);
			break;
		case 0x8769:	// Exif IFD pointer
			memcpy(&dst_scan[dst_offset], &src_scan[src_offset], IFDStructSize);
			break;
		case 0x8825:	// GPS IFD pointer
			base_ignores++;
			break;
		case 0xA005:	// Interoperatibility IFD pointer
			base_ignores++;
			break;
		}
	}
	// 基本部の項目数の設定.
	fnPRV_Exif_SetValueU16(dst_scan, TiffOffset + 8, endian_type, ((int)base_items.size() - base_ignores));

	// --------------------------------------------------
	// Exif 部の複製.
	int exif_ignores = 0;
	for(int i=0 ; i<(int)exif_items.size() ; i++)
	{
		auto item = exif_items[i];
		int src_offset = item.Offset;
		int dst_offset = item.Offset - (IFDStructSize * exif_ignores);

		memcpy(&dst_scan[dst_offset], &src_scan[src_offset], IFDStructSize);
		fnPRV_Exif_CopyRelatedItem(dst_scan, src_scan, item, TiffOffset);
	}
	// Exif 部の項目数の設定.
	fnPRV_Exif_SetValueU16(dst_scan, exif_offset, endian_type, ((int)exif_items.size() - exif_ignores));

	return result;
}

// ============================================================
void CxExif::GetValue(const TxExifItem& item, HxModule hval) const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	auto src_scan = this->Scanner<unsigned char>();

	switch(item.Type)
	{
	case 1:		// BYTE
	case 7:		// UNDEFINED
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef unsigned char	TE;
			val->Resize(item.Count, ModelOf<TE>());
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 2:		// ASCII
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef unsigned char	TE;
			val->Resize(item.Count, ModelOf<TE>());
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else if (auto val = Axi::SafeCast<CxStringA>(hval))
		{
			typedef unsigned char	TE;
			CxArray tmp(item.Count, ModelOf<TE>());
			auto tmp_scan = tmp.Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(tmp_scan, src_scan, item, TiffOffset);

			int valid_length = 0;
			tmp_scan.ForEach([&valid_length](int x, TE* addr)
			{
				if (0x20 <= *addr && *addr <= 0x7E)
					valid_length++;
			});

			val->Resize(valid_length);
			val->Reset();
			if (valid_length > 0)
			{
				TxScanner1D<TE> val_scan;
				val_scan.Address	= (TE*)val->Address();
				val_scan.Length		= val->Length();
				val_scan.Model		= TxModel::U8(1);
				int val_index = 0;
				for(int i=0 ; i<tmp_scan.Length ; i++)
				{
					if (0x20 <= tmp_scan[i] && tmp_scan[i] <= 0x7E)
						val_scan[val_index++] = tmp_scan[i];
				}
			}
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:		// SHORT
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef unsigned short	TE;
			val->Resize(item.Count, ModelOf<TE>());
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 4:		// LONG
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef unsigned int	TE;
			val->Resize(item.Count, ModelOf<TE>());
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 9:		// SLONG A 32-bit
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef short	TE;
			val->Resize(item.Count, ModelOf<TE>());
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 5:		// RATIONAL
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef unsigned int	TE;
			int pack = 2;
			val->Resize(item.Count, ModelOf<TE>() * pack);
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 10:	// SRATIONAL
		if (auto val = Axi::SafeCast<CxArray>(hval))
		{
			typedef int	TE;
			int pack = 2;
			val->Resize(item.Count, ModelOf<TE>() * pack);
			auto val_scan = val->Scanner<TE>();
			fnPRV_Exif_GetValues<TE>(val_scan, src_scan, item, TiffOffset);
		}
		else
		{
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 6:		// unknown
	case 8:		// unknown
		break;
	}
}

// ============================================================
void CxExif::SetValue(const TxExifItem& item, HxModule hval)
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	auto dst_scan = this->Scanner<unsigned char>();

	switch(item.Type)
	{
	case 1:		// BYTE
	case 7:		// UNDEFINED
		{
			typedef unsigned char	TE;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 2:		// ASCII
		{
			typedef unsigned char	TE;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else if (auto val = Axi::SafeCast<CxStringA>(hval))
			{
				val_scan.Address	= (TE*)val->Address();
				val_scan.Length		= val->Length();
				val_scan.Model		= TxModel::U8(1);
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 3:		// SHORT
		{
			typedef unsigned short	TE;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 4:		// LONG
		{
			typedef unsigned int	TE;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 9:		// SLONG A 32-bit
		{
			typedef short	TE;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 5:		// RATIONAL
		{
			typedef unsigned int	TE;
			int pack = 2;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 10:	// SRATIONAL
		{
			typedef int	TE;
			int pack = 2;
			TxScanner1D<TE> val_scan;
			if (auto val = Axi::SafeCast<CxArray>(hval))
			{
				val_scan = val->Scanner<TE>();
			}
			else
			{
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			fnPRV_Exif_SetValues<TE>(dst_scan, val_scan, item, TiffOffset);
		}
		break;
	case 6:		// unknown
	case 8:		// unknown
		break;
	}
}

}
