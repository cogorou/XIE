/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxArray.h"

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/CxMatrix.h"
#include "Effectors/CxRgbToGray.h"
#include "File/api_file.h"
#include <typeinfo>
#include <math.h>
#include <stdio.h>

namespace xie
{

static const char* g_ClassName = "CxArray";

// ============================================================
void CxArray::_Constructor()
{
	m_Tag = TxArray::Default();
	m_IsAttached = false;
}

// ============================================================
CxArray CxArray::FromTag(const TxArray& src)
{
	CxArray dst;
	dst.Attach(src);
	return dst;
}

// ============================================================
CxArray::CxArray()
{
	_Constructor();
}

// ============================================================
CxArray::CxArray(CxArray&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxArray::CxArray(const CxArray& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxArray::CxArray(int length, TxModel model)
{
	_Constructor();
	Resize(length, model);
}

// ============================================================
CxArray::CxArray(TxCharCPtrA filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxArray::CxArray(TxCharCPtrW filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxArray::~CxArray()
{
	Dispose();
}

// ============================================================
CxArray& CxArray::operator = ( CxArray&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxArray& CxArray::operator = ( const CxArray& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxArray::operator == ( const CxArray& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxArray::operator != ( const CxArray& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
TxArray CxArray::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxArray::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
void CxArray::Dispose()
{
	if (IsAttached() == false)
	{
		if (m_Tag.Address != NULL)
			Axi::MemoryFree( m_Tag.Address );
	}
	m_Tag = TxArray::Default();
	m_IsAttached = false;
}

// ================================================================================
void CxArray::MoveFrom(CxArray& src)
{
	if (this == &src) return;

	CxArray& dst = *this;
	dst.Dispose();
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;
}

// ============================================================
void CxArray::CopyTo(IxModule& dst) const
{
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArray::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxArray>(src))
	{
		auto&	_src = static_cast<const CxArray&>(src);
		auto&	_dst = *this;

		_dst.Dispose();
		if (_src.IsValid())
		{
			_dst.Resize(_src.Length(), _src.Model());
			_dst.Filter().Copy(_src);
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
bool CxArray::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxArray>(src))
	{
		auto& _src = static_cast<const CxArray&>(src);
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
bool CxArray::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Length <= 0) return false;
	if (m_Tag.Model.Size() <= 0) return false;
	return true;
}

// ============================================================
bool CxArray::IsAttached() const
{
	return m_IsAttached;
}

// ============================================================
void CxArray::Attach(const IxModule& src)
{
	this->Dispose();

	if (xie::Axi::ClassIs<CxArray>(src))
	{
		const CxArray& _src = static_cast<const CxArray&>(src);
		this->m_IsAttached = true;
		this->m_Tag = _src.m_Tag;
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArray::Attach(const TxArray& src)
{
	this->Dispose();
	this->m_IsAttached = true;
	this->m_Tag = src;
}

// ============================================================
void CxArray::Attach(const CxArray& src, int index, int length)
{
	this->Dispose();

	TxArray src_tag = src.Tag();
	if (src_tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (index > src_tag.Length - 1)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (index + length > src_tag.Length)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	
	this->m_IsAttached = true;
	this->m_Tag = TxArray((void*)src[index], length, src.Model());
}

// ============================================================
void CxArray::Resize(int length, TxModel model)
{
	this->Dispose();

	if (length == 0) return;

	if (length < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
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
void CxArray::Reset()
{
	if (IsValid() == false) return;

	ExStatus status = fnXIE_Core_Array_Reset((HxModule)this);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArray::Clear(const void* value, TxModel model)
{
	ExStatus status = fnXIE_Core_Array_Clear((HxModule)this, value, model);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxArray::ClearEx(const void* value, TxModel model, int index, int count)
{
	ExStatus status = fnXIE_Core_Array_ClearEx((HxModule)this, value, model, index, count);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
CxArray CxArray::Clone() const
{
	CxArray clone;
	clone.CopyFrom(*this);
	return clone;
}

// ============================================================
CxArray CxArray::Clone(TxModel model, double scale) const
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	TxModel dst_model = this->Model();
	dst_model.Type = (model.Type != ExType::None) ? model.Type : this->Model().Type;
	dst_model.Pack = (model.Pack != 0) ? model.Pack : this->Model().Pack;
	CxArray clone(this->Length(), dst_model);

	clone.Filter().Copy(*this, scale);

	return clone;
}

// ============================================================
CxArray CxArray::Child() const
{
	CxArray dst;
	dst.Attach(*this);
	return dst;
}

// ============================================================
CxArray CxArray::Child(int index, int length) const
{
	CxArray dst;
	dst.Attach(*this, index, length);
	return dst;
}

// ============================================================
void CxArray::Load(TxCharCPtrA filename)
{
	LoadA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxArray::Load(TxCharCPtrW filename)
{
	LoadW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxArray::Save(TxCharCPtrA filename) const
{
	SaveA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxArray::Save(TxCharCPtrW filename) const
{
	SaveW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxArray::LoadA(TxCharCPtrA filename, const void* option, TxModel model)
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
void CxArray::LoadW(TxCharCPtrW filename, const void* option, TxModel model)
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
void CxArray::SaveA(TxCharCPtrA filename, const void* option, TxModel model) const
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
void CxArray::SaveW(TxCharCPtrW filename, const void* option, TxModel model) const
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
void* CxArray::OpenRawA(TxCharCPtrA filename, int mode)
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
void* CxArray::OpenRawW(TxCharCPtrW filename, int mode)
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
void CxArray::CloseRaw(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

// ============================================================
void CxArray::LoadRaw(void* handle)
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
		CxArray* dst = this;

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

			dst->Resize(length, model);
			if (dst->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}

		// データ読み込み.
		{
			uiReadSize = fread((*dst)[0], dst->Model().Size(), dst->Length(), stream);
			if ((int)uiReadSize != dst->Length())
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
	}
}

// ============================================================
void CxArray::SaveRaw(void* handle) const
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
		const CxArray* src = this;

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
int CxArray::Length() const
{
	return m_Tag.Length;
}

// ============================================================
TxModel CxArray::Model() const
{
	return m_Tag.Model;
}

// ============================================================
void* CxArray::Address()
{
	return m_Tag.Address;
}

// ============================================================
const void* CxArray::Address() const
{
	return m_Tag.Address;
}

// ============================================================
void* CxArray::operator [] (int index)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + (size * index);
}

// ============================================================
const void* CxArray::operator [] (int index) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + (size * index);
}

// ============================================================
TxStatistics CxArray::Statistics(int ch) const
{
	TxStatistics result;
	ExStatus status = fnXIE_Core_Array_Statistics(*this, ch, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxArray CxArray::Extract(int index, int length) const
{
	CxArray result;
	ExStatus status = fnXIE_Core_Array_Extract(*this, index, length, result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxArrayFilter CxArray::Filter() const
{
	return CxArrayFilter((HxModule)this);
}

// //////////////////////////////////////////////////////////////////////
// Export
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Array_Attach(HxModule handle, TxArray src)
{
	try
	{
		CxArray* _src = xie::Axi::SafeCast<CxArray>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Attach(src);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Array_Resize(HxModule handle, int length, TxModel model)
{
	try
	{
		CxArray* _src = xie::Axi::SafeCast<CxArray>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Resize(length, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Statistics	(HxModule hsrc, int ch, TxStatistics* result)
{
	try
	{
		if (result == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		TxStatistics ans = TxStatistics::Default();
		ExStatus status = fnPRV_Core_2D_Statistics(src_tag, mask_tag, ch, &ans);
		*result = ans;

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Extract		(HxModule hsrc, int index, int length, HxModule hresult)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* result = xie::Axi::SafeCast<CxArray>(hresult);
		if (result == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (result->IsValid() == false)
			result->Resize(length, src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();
		TxArray result_tag = result->Tag();

		ExStatus status = fnPRV_Core_2D_Extract(src_tag, mask_tag, 0, 0, index, length, ExScanDir::X, result_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Reset		(HxModule hdst)
{
	try
	{
		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		const int	value = 0;
		TxModel	model = xie::ModelOf(value);

		ExStatus status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, &value, model);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Clear		(HxModule hdst, const void* value, TxModel model)
{
	try
	{
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, value, model);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_ClearEx		(HxModule hdst, const void* value, TxModel model, int index, int count)
{
	try
	{
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_ClearEx(dst_tag, mask_tag, value, model, index, count);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Cast			(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Cast(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Copy			(HxModule hdst, HxModule hsrc, double scale)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		// Scale
		if (scale == 0)
			scale = 1;

		ExStatus status = ExStatus::Success;

		if (src_tag.Model.Pack >= 3 && dst_tag.Model.Pack == 1)
		{
			xie::Effectors::CxRgbToGray effector(scale);
			effector.Execute(hsrc, *dst);
		}
		else if (scale != 1)
		{
			status = fnPRV_Core_2D_Convert(dst_tag, mask_tag, src_tag, scale);
		}
		else if (src_tag.Model == dst_tag.Model)
		{
			status = fnPRV_Core_2D_Cast(dst_tag, mask_tag, src_tag);
		}
		else
		{
			status = fnPRV_Core_2D_Copy(dst_tag, mask_tag, src_tag);
		}

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_CopyEx		(HxModule hdst, HxModule hsrc, int index, int count)
{
	try
	{
		if (index < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (count < 1)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), TxModel(src->Model().Type, count));

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_CopyEx(dst_tag, mask_tag, src_tag, index, count);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_RgbToBgr		(HxModule hdst, HxModule hsrc, double scale)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		// Scale
		if (scale == 0)
			scale = 1;

		ExStatus status = fnPRV_Core_2D_RgbToBgr(dst_tag, mask_tag, src_tag, scale);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Compare		(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* cmp = xie::Axi::SafeCast<CxArray>(hcmp);
		if (cmp == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (cmp->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			ExType	type	= ExType::U8;
			int		pack	= Axi::Min(src->Model().Pack, cmp->Model().Pack);
			dst->Resize(src->Length(), TxModel(type, pack));
		}

		TxImage src_tag = To2D(src->Tag());
		TxImage cmp_tag = To2D(cmp->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Compare(dst_tag, mask_tag, src_tag, cmp_tag, error_range);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_ColorMatrix	(HxModule hdst, HxModule hsrc, HxModule hmatrix)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* matrix = xie::Axi::SafeCast<CxMatrix>(hmatrix);
		if (matrix == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (matrix->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			ExType	type	= ExType::F64;
			int		pack	= src->Model().Pack;
			dst->Resize(src->Length(), TxModel(type, pack));
		}

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();
		TxMatrix matrix_tag = matrix->Tag();

		ExStatus status = fnPRV_Core_2D_ColorMatrix(dst_tag, mask_tag, src_tag, matrix_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Not				(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage src_tag = To2D(src->Tag());
		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Not(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Math				(HxModule hdst, HxModule hsrc, ExMath mode)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage mask_tag = TxImage::Default();

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		ExStatus status = fnPRV_Core_2D_Math(dst_tag, mask_tag, src_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope1A			(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage mask_tag = TxImage::Default();

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		ExStatus status = fnPRV_Core_2D_Ope1A(dst_tag, mask_tag, src_tag, value, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope1L			(HxModule hdst, HxModule hsrc, unsigned int value, ExOpe1L mode)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage mask_tag = TxImage::Default();

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		ExStatus status = fnPRV_Core_2D_Ope1L(dst_tag, mask_tag, src_tag, value, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope2A			(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* val = xie::Axi::SafeCast<CxArray>(hval);
		if (val == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (val->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage mask_tag = TxImage::Default();

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage val_tag = To2D(val->Tag());
		ExStatus status = fnPRV_Core_2D_Ope2A(dst_tag, mask_tag, src_tag, val_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Array_Ope2L			(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2L mode)
{
	try
	{
		CxArray* src = xie::Axi::SafeCast<CxArray>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* val = xie::Axi::SafeCast<CxArray>(hval);
		if (val == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (val->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* dst = xie::Axi::SafeCast<CxArray>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Length(), src->Model());

		TxImage mask_tag = TxImage::Default();

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage val_tag = To2D(val->Tag());
		ExStatus status = fnPRV_Core_2D_Ope2L(dst_tag, mask_tag, src_tag, val_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
