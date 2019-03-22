/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/CxArray.h"
#include "Core/CxMatrix.h"
#include "File/api_file.h"
#include <typeinfo>
#include <math.h>
#include <stdio.h>

namespace xie
{

static const char* g_ClassName = "CxMatrix";

// ============================================================
void CxMatrix::_Constructor()
{
	m_Tag = TxMatrix::Default();
	m_IsAttached = false;
}

// ============================================================
CxMatrix CxMatrix::FromTag(const TxMatrix& src)
{
	CxMatrix dst;
	dst.Attach(src);
	return dst;
}

// ============================================================
CxMatrix CxMatrix::PresetRotate(double degree, double axis_x, double axis_y)
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Preset_Rotate(dst, degree, axis_x, axis_y);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix CxMatrix::PresetScale(double sx, double sy)
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Preset_Scale(dst, sx, sy);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix CxMatrix::PresetTranslate(double tx, double ty)
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Preset_Translate(dst, tx, ty);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix CxMatrix::PresetShear(double degree_x, double degree_y)
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Preset_Shear(dst, degree_x, degree_y);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix::CxMatrix()
{
	_Constructor();
}

// ============================================================
CxMatrix::CxMatrix(CxMatrix&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxMatrix::CxMatrix(const CxMatrix& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxMatrix::CxMatrix(int rows, int cols, TxModel model)
{
	_Constructor();
	Resize(rows, cols, model);
}

// ============================================================
CxMatrix::CxMatrix(const TxSizeI& size, TxModel model)
{
	_Constructor();
	Resize(size, model);
}

// ============================================================
CxMatrix::CxMatrix(TxCharCPtrA filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxMatrix::CxMatrix(TxCharCPtrW filename)
{
	_Constructor();
	Load(filename);
}

// ============================================================
CxMatrix::~CxMatrix()
{
	Dispose();
}

// ============================================================
CxMatrix& CxMatrix::operator = ( CxMatrix&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxMatrix& CxMatrix::operator = ( const CxMatrix& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxMatrix::operator == ( const CxMatrix& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxMatrix::operator != ( const CxMatrix& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
TxMatrix CxMatrix::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxMatrix::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
void CxMatrix::Dispose()
{
	if (IsAttached() == false)
	{
		if (m_Tag.Address != NULL)
			Axi::MemoryFree( m_Tag.Address );
	}
	m_Tag = TxMatrix::Default();
	m_IsAttached = false;
}

// ================================================================================
void CxMatrix::MoveFrom(CxMatrix& src)
{
	if (this == &src) return;

	CxMatrix& dst = *this;
	dst.Dispose();
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;
}

// ============================================================
void CxMatrix::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxArray>(dst))
	{
		const CxMatrix&	_src = *this;
		CxArray&		_dst = static_cast<CxArray&>(dst);

		_dst.Dispose();
		if (_src.IsValid())
		{
			int length = _src.Rows() * _src.Columns();
			_dst.Resize(length, _src.Model());

			TxArray dst_tag = _dst.Tag();
			TxImage src_tag = To2D(_src.Tag());

			ExStatus status = fnPRV_Core_2D_Serialize(dst_tag, src_tag);
			if (status != ExStatus::Success)
				throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		}

		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrix::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxMatrix>(src))
	{
		auto& _src = static_cast<const CxMatrix&>(src);
		auto& _dst = *this;

		_dst.Dispose();
		if (_src.IsValid())
		{
			_dst.Resize(_src.Size(), _src.Model());
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
bool CxMatrix::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxMatrix>(src))
	{
		auto& _src = static_cast<const CxMatrix&>(src);
		auto& _dst = *this;

		if (_dst.m_Tag.Rows		!= _src.m_Tag.Rows) return false;
		if (_dst.m_Tag.Columns	!= _src.m_Tag.Columns) return false;
		if (_dst.m_Tag.Model	!= _src.m_Tag.Model) return false;

		int height = _src.Rows();
		int wbytes = _src.Model().Size() * _src.Columns();
		{
			for(int y=0 ; y<height ; y++)
			{
				const void*	src_addr = _src[y];
				const void* dst_addr = _dst[y];
				if (memcmp(dst_addr, src_addr, wbytes) != 0) return false;
			}
		}
		return true;
	}
	return false;
}

// ============================================================
bool CxMatrix::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Rows <= 0) return false;
	if (m_Tag.Columns <= 0) return false;

	int min_stride = m_Tag.Model.Size() * m_Tag.Columns;
	if (min_stride <= 0) return false;

	if (m_Tag.Stride < min_stride) return false;
	return true;
}

// ============================================================
bool CxMatrix::IsAttached() const
{
	return m_IsAttached;
}

// ============================================================
void CxMatrix::Attach(const IxModule& src)
{
	this->Dispose();

	if (xie::Axi::ClassIs<CxMatrix>(src))
	{
		const CxMatrix& _src = static_cast<const CxMatrix&>(src);
		this->m_IsAttached = true;
		this->m_Tag = _src.m_Tag;
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrix::Attach(const TxMatrix& src)
{
	this->Dispose();
	this->m_IsAttached = true;
	this->m_Tag = src;
}

// ============================================================
void CxMatrix::Resize(const TxSizeI& size, TxModel model)
{
	Resize(size.Height, size.Width, model);
}

// ============================================================
void CxMatrix::Resize(int rows, int cols, TxModel model)
{
	this->Dispose();

	if (rows == 0 || cols == 0) return;

	if (rows < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (cols < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		if (model == TxModel::Default())
			model = TxModel::F64(1);

		int	length = rows * cols;
		void* addr = Axi::MemoryAlloc( (TxIntPtr)length * model.Size() );
		if (addr == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		m_Tag.Address	= addr;
		m_Tag.Rows		= rows;
		m_Tag.Columns	= cols;
		m_Tag.Model		= model;
		m_Tag.Stride	= model.Size() * cols;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxMatrix::Reset()
{
	if (this->IsValid() == false) return;

	ExStatus status = fnXIE_Core_Matrix_Reset((HxModule)this);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrix::Eye(double value, int mode)
{
	ExStatus status = fnXIE_Core_Matrix_Eye((HxModule)this, value, mode);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxMatrix::Clear(const void* value, TxModel model)
{
	ExStatus status = fnXIE_Core_Matrix_Clear((HxModule)this, value, model);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
CxMatrix CxMatrix::Clone() const
{
	CxMatrix clone;
	clone.CopyFrom(*this);
	return clone;
}

// ============================================================
CxMatrix CxMatrix::Clone(TxModel model) const
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	TxModel dst_model;
	dst_model.Type = (model.Type != ExType::None) ? model.Type : this->Model().Type;
	dst_model.Pack = (model.Pack != 0) ? model.Pack : this->Model().Pack;
	CxMatrix clone(this->Size(), dst_model);
	clone.Filter().Copy(*this);

	return clone;
}

// ============================================================
void CxMatrix::Load(TxCharCPtrA filename)
{
	LoadA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxMatrix::Load(TxCharCPtrW filename)
{
	LoadW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxMatrix::Save(TxCharCPtrA filename) const
{
	SaveA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxMatrix::Save(TxCharCPtrW filename) const
{
	SaveW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxMatrix::LoadA(TxCharCPtrA filename, const void* option, TxModel model)
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
void CxMatrix::LoadW(TxCharCPtrW filename, const void* option, TxModel model)
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
void CxMatrix::SaveA(TxCharCPtrA filename, const void* option, TxModel model) const
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
void CxMatrix::SaveW(TxCharCPtrW filename, const void* option, TxModel model) const
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
void* CxMatrix::OpenRawA(TxCharCPtrA filename, int mode)
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
void* CxMatrix::OpenRawW(TxCharCPtrW filename, int mode)
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
void CxMatrix::CloseRaw(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

// ============================================================
void CxMatrix::LoadRaw(void* handle)
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
		CxMatrix* dst = this;

		// データ識別情報.
		{
			int			rows = 0;
			int			columns = 0;
			TxModel		model = TxModel::Default();

			uiReadSize = fread(&rows, sizeof(rows), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiReadSize = fread(&columns, sizeof(columns), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiReadSize = fread(&model, sizeof(model), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			dst->Resize(rows, columns, model);
			if (dst->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}

		// データ読み込み.
		for(int row=0 ; row<dst->Rows() ; row++)
		{
			uiReadSize = fread((*dst)[row], dst->Model().Size(), dst->Columns(), stream);
			if ((int)uiReadSize != dst->Columns())
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
	}
}

// ============================================================
void CxMatrix::SaveRaw(void* handle) const
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
		const CxMatrix* src = this;

		// ヘッダー書き込み.
		uiWriteSize = fwrite(&header, sizeof(TxRawFileHeader), 1, stream);
		if (uiWriteSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// データ識別情報.
		{
			int			rows = src->Rows();
			int			columns = src->Columns();
			TxModel		model = src->Model();

			uiWriteSize = fwrite(&rows, sizeof(rows), 1, stream);
			if (uiWriteSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiWriteSize = fwrite(&columns, sizeof(columns), 1, stream);
			if (uiWriteSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			uiWriteSize = fwrite(&model, sizeof(model), 1, stream);
			if (uiWriteSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		// データ書き込み.
		for(int row=0 ; row<src->Rows() ; row++)
		{
			uiWriteSize = fwrite((*src)[row], src->Model().Size(), src->Columns(), stream);
			if ((int)uiWriteSize != src->Columns())
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}
	}
}

// ============================================================
int CxMatrix::Rows() const
{
	return m_Tag.Rows;
}

// ============================================================
int CxMatrix::Columns() const
{
	return m_Tag.Columns;
}

// ============================================================
TxSizeI CxMatrix::Size() const
{
	return TxSizeI(m_Tag.Columns, m_Tag.Rows);
}

// ============================================================
TxModel CxMatrix::Model() const
{
	return m_Tag.Model;
}

// ============================================================
int CxMatrix::Stride() const
{
	return m_Tag.Stride;
}

// ============================================================
void* CxMatrix::Address()
{
	return m_Tag.Address;
}

// ============================================================
const void* CxMatrix::Address() const
{
	return m_Tag.Address;
}

// ============================================================
void* CxMatrix::operator [] ( int row )
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= row && row <m_Tag.Rows))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * row);
}

// ============================================================
const void* CxMatrix::operator [] ( int row ) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= row && row <m_Tag.Rows))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * row);
}

// ============================================================
void* CxMatrix::operator () (int row, int col)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= row && row <m_Tag.Rows) ||
		!(0 <= col && col <m_Tag.Columns))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * row) + (m_Tag.Model.Size() * col);
}

// ============================================================
const void* CxMatrix::operator () (int row, int col) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= row && row <m_Tag.Rows) ||
		!(0 <= col && col <m_Tag.Columns))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * row) + (m_Tag.Model.Size() * col);
}

// ============================================================
TxStatistics CxMatrix::Statistics() const
{
	TxStatistics result;
	ExStatus status = fnXIE_Core_Matrix_Statistics(*this, 0, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxArray CxMatrix::Extract(int row, int col, int length, ExScanDir dir) const
{
	CxArray result;
	ExStatus status = fnXIE_Core_Matrix_Extract(*this, row, col, length, dir, result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxMatrixFilter CxMatrix::Filter() const
{
	return CxMatrixFilter(*this);
}

// ============================================================
CxMatrix CxMatrix::operator* (const CxMatrix &val) const
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Product(dst, *this, val);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix& CxMatrix::operator*= (const CxMatrix &val)
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Product(dst, *this, val);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	this->MoveFrom(dst);
	return *this;
}

// ============================================================
double CxMatrix::Det() const
{
	double result = 0;
	ExStatus status = fnXIE_Core_Matrix_Det(*this, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
double CxMatrix::Trace() const
{
	double result = 0;
	ExStatus status = fnXIE_Core_Matrix_Trace(*this, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
TxSizeD CxMatrix::ScaleFactor() const
{
	TxSizeD result = TxSizeD(0, 0);
	ExStatus status = fnXIE_Core_Matrix_ScaleFactor(*this, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxMatrix CxMatrix::Invert() const
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Invert(dst, *this);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// ============================================================
CxMatrix CxMatrix::Submatrix(int row, int col) const
{
	CxMatrix dst;
	ExStatus status = fnXIE_Core_Matrix_Submatrix(dst, *this, row, col);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return dst;
}

// //////////////////////////////////////////////////////////////////////
// Matrix
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Matrix_Attach(HxModule handle, TxMatrix src)
{
	try
	{
		CxMatrix* _src = xie::Axi::SafeCast<CxMatrix>(handle);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Matrix_Resize(HxModule handle, int rows, int cols, TxModel model)
{
	try
	{
		CxMatrix* _src = xie::Axi::SafeCast<CxMatrix>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Resize(rows, cols, model);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Statistics	(HxModule hsrc, int ch, TxStatistics* result)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (result == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix& src = *reinterpret_cast<CxMatrix*>(hsrc);
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage src_tag = To2D(src.Tag());
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Extract		(HxModule hsrc, int row, int col, int length, ExScanDir dir, HxModule hresult)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
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

		ExStatus status = fnPRV_Core_2D_Extract(src_tag, mask_tag, 0, row, col, length, dir, result_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Reset		(HxModule hdst)
{
	try
	{
		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = To2D(dst->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status;

		switch(dst->Model().Type)
		{
		case ExType::U8:
		case ExType::U16:
		case ExType::U32:
		case ExType::U64:
		case ExType::S8:
		case ExType::S16:
		case ExType::S32:
		case ExType::S64:
		case ExType::F32:
		case ExType::F64:
			if (dst->Model().Pack == 1)
			{
				status = fnXIE_Core_Matrix_Eye(hdst, 1.0, 0);
			}
			else
			{
				const int	value = 0;
				TxModel	model = xie::ModelOf(value);
				status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, &value, model);
			}
			break;
		default:
			{
				const int	value = 0;
				TxModel	model = xie::ModelOf(value);
				status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, &value, model);
			}
			break;
		}

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Clear		(HxModule hdst, const void* value, TxModel model)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix& dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = To2D(dst.Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, value, model);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ============================================================
template<class TD, class TV> static inline void fnPRV_Matrix_Eye_cc(TxScanner2D<TD>&& dst, TV val, int mode)
{
	switch (mode)
	{
	case 0:
		for(int y=0 ; y<dst.Height ; y++)
		{
			for(int x=0 ; x<dst.Width ; x++)
			{
				dst(y, x) = static_cast<TD>((y == x) ? val : 0);
			}
		}
		break;
	case 1:
		for(int y=0 ; y<dst.Height ; y++)
		{
			for(int x=0 ; x<dst.Width ; x++)
			{
				if (y == x)
					dst(y, x) = (TD)val;
			}
		}
		break;
	case 2:
		for(int y=0 ; y<dst.Height ; y++)
		{
			for(int x=0 ; x<dst.Width ; x++)
			{
				if (y != x)
					dst(y, x) = (TD)val;
			}
		}
		break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Eye			(HxModule hdst, double value, int mode)
{
	try
	{
		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->Model().Pack != 1)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		switch(dst->Model().Type)
		{
		case ExType::U8:	fnPRV_Matrix_Eye_cc	( dst->Scanner<unsigned char>(), value, mode );	break;
		case ExType::U16:	fnPRV_Matrix_Eye_cc	( dst->Scanner<unsigned short>(), value, mode );	break;
		case ExType::U32:	fnPRV_Matrix_Eye_cc	( dst->Scanner<unsigned int>(), value, mode );	break;
		case ExType::U64:	fnPRV_Matrix_Eye_cc	( dst->Scanner<unsigned long long>(), value, mode );	break;
		case ExType::S8:	fnPRV_Matrix_Eye_cc	( dst->Scanner<char>(), value, mode );	break;
		case ExType::S16:	fnPRV_Matrix_Eye_cc	( dst->Scanner<short>(), value, mode );	break;
		case ExType::S32:	fnPRV_Matrix_Eye_cc	( dst->Scanner<int>(), value, mode );	break;
		case ExType::S64:	fnPRV_Matrix_Eye_cc	( dst->Scanner<long long>(), value, mode );	break;
		case ExType::F32:	fnPRV_Matrix_Eye_cc	( dst->Scanner<float>(), value, mode );	break;
		case ExType::F64:	fnPRV_Matrix_Eye_cc	( dst->Scanner<double>(), value, mode );	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Cast		(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			dst->Resize(src->Size(), src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Copy		(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			dst->Resize(src->Size(), src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = ExStatus::Success;

		if (src_tag.Model == dst_tag.Model)
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Compare		(HxModule hdst, HxModule hsrc, HxModule hcmp, double error_range)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* cmp = xie::Axi::SafeCast<CxMatrix>(hcmp);
		if (cmp == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (cmp->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			ExType	type	= ExType::U8;
			int		pack	= Axi::Min(src->Model().Pack, cmp->Model().Pack);
			dst->Resize(src->Size(), TxModel(type, pack));
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage cmp_tag = To2D(cmp->Tag());
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Mirror		(HxModule hdst, HxModule hsrc, int mode)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			dst->Resize(src->Size(), src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Mirror(dst_tag, mask_tag, src_tag, mode);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Rotate		(HxModule hdst, HxModule hsrc, int mode)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			int dst_rows = 0;
			int dst_cols = 0;
			switch(mode)
			{
			default:
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			case +1:
			case -1:
				dst_rows = src->Columns();
				dst_cols = src->Rows();
				break;
			case +2:
			case -2:
				dst_rows = src->Rows();
				dst_cols = src->Columns();
				break;
			}

			dst->Resize(dst_rows, dst_cols, src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Rotate(dst_tag, mask_tag, src_tag, mode);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Transpose	(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			dst->Resize(src->Columns(), src->Rows(), src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Transpose(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Det			(HxModule hsrc, double* value)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix src = CxMatrix::FromTag(reinterpret_cast<CxMatrix*>(hsrc)->Tag());
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (IsCompatible(src.Tag()) == false)
			src = reinterpret_cast<CxMatrix*>(hsrc)->Clone();
		if (src.Rows() != src.Columns())
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

		TxScanner2D<double> src_scan = src.Scanner<double>();

		switch(src.Rows())
		{
		case 1:
			{
				*value = src_scan(0,0);
			}
			break;
		case 2:
			{
				*value = (src_scan(0,0) * src_scan(1,1) - src_scan(0,1) * src_scan(1,0));
			}
			break;
		case 3:
			{
				*value = 
					src_scan(0,0) * (src_scan(1,1) * src_scan(2,2) - src_scan(1,2) * src_scan(2,1)) - 
					src_scan(0,1) * (src_scan(1,0) * src_scan(2,2) - src_scan(1,2) * src_scan(2,0)) + 
					src_scan(0,2) * (src_scan(1,0) * src_scan(2,1) - src_scan(1,1) * src_scan(2,0));
			}
			break;
		case 4:
			{
				CxMatrix sm0; sm0.Filter().Submatrix(src, 0,0);
				CxMatrix sm1; sm1.Filter().Submatrix(src, 1,0);
				CxMatrix sm2; sm2.Filter().Submatrix(src, 2,0);
				CxMatrix sm3; sm3.Filter().Submatrix(src, 3,0);
				double det0 = src_scan(0,0) * sm0.Det();
				double det1 = src_scan(1,0) * sm1.Det();
				double det2 = src_scan(2,0) * sm2.Det();
				double det3 = src_scan(3,0) * sm3.Det();
				*value = det0 - det1 + det2 - det3;
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Trace		(HxModule hsrc, double* value)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix	src = CxMatrix::FromTag(reinterpret_cast<CxMatrix*>(hsrc)->Tag());
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (IsCompatible(src.Tag()) == false)
			src = reinterpret_cast<CxMatrix*>(hsrc)->Clone();
		if (src.Rows() != src.Columns())
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

		TxScanner2D<double> src_scan = src.Scanner<double>();

		double sum = 0;
		int length = src.Rows();
		for(int i=0 ; i<length ; i++)
			sum += src_scan(i, i);
		*value = sum;

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_ScaleFactor	(HxModule hsrc, TxSizeD* value)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix	src = CxMatrix::FromTag(reinterpret_cast<CxMatrix*>(hsrc)->Tag());
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (IsCompatible(src.Tag()) == false)
			src = reinterpret_cast<CxMatrix*>(hsrc)->Clone();
		if (src.Rows() != 3 ||
			src.Columns() != 3)
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

		TxScanner2D<double> src_scan = src.Scanner<double>();

		// 変換行列のスケールの抽出.
		double m00 = src_scan(0, 0);
		double m01 = src_scan(0, 1);
		double m10 = src_scan(1, 0);
		double m11 = src_scan(1, 1);
		double sx = sqrt(m00 * m00 + m10 * m10);
		double sy = sqrt(m01 * m01 + m11 * m11);
		value->Width	= sx;
		value->Height	= sy;

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Invert		(HxModule hdst, HxModule hsrc)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix	src = CxMatrix::FromTag(reinterpret_cast<CxMatrix*>(hsrc)->Tag());
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (IsCompatible(src.Tag()) == false)
			src = reinterpret_cast<CxMatrix*>(hsrc)->Clone();
		if (src.Rows() != src.Columns())
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.IsValid() == false)
		{
			dst.Resize(src.Rows(), src.Columns());
		}
		if (IsCompatible(dst.Tag()) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// 係数 = 1 / 元の行列の行列式.
		double det = src.Det();
		if (_abs(det) < XIE_EPSd)
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);
		double _1_det = 1.0 / det;

		int rows = src.Rows();
		int cols = src.Columns();

		switch(src.Rows())
		{
		case 1:
			{
				TxScanner2D<double> dst_scan = dst.Scanner<double>();
				dst_scan(0,0) = _1_det;
			}
			break;
		case 2:
			{
				TxScanner2D<double> src_scan = src.Scanner<double>();
				TxScanner2D<double> dst_scan = dst.Scanner<double>();
				dst_scan(0,0) = _1_det * src_scan(1,1);
				dst_scan(1,1) = _1_det * src_scan(0,0);
				dst_scan(0,1) = _1_det * src_scan(0,1) * -1;
				dst_scan(1,0) = _1_det * src_scan(1,0) * -1;
			}
			break;
		case 3:
			{
				TxScanner2D<double> src_scan = src.Scanner<double>();
				TxScanner2D<double> dst_scan = dst.Scanner<double>();
				dst_scan(0,0) = _1_det * (src_scan(1,1) * src_scan(2,2) - src_scan(1,2) * src_scan(2,1));
				dst_scan(0,1) = _1_det * (src_scan(0,2) * src_scan(2,1) - src_scan(0,1) * src_scan(2,2));
				dst_scan(0,2) = _1_det * (src_scan(0,1) * src_scan(1,2) - src_scan(0,2) * src_scan(1,1));
				// --
				dst_scan(1,0) = _1_det * (src_scan(1,2) * src_scan(2,0) - src_scan(1,0) * src_scan(2,2));
				dst_scan(1,1) = _1_det * (src_scan(0,0) * src_scan(2,2) - src_scan(0,2) * src_scan(2,0));
				dst_scan(1,2) = _1_det * (src_scan(0,2) * src_scan(1,0) - src_scan(0,0) * src_scan(1,2));
				// --
				dst_scan(2,0) = _1_det * (src_scan(1,0) * src_scan(2,1) - src_scan(1,1) * src_scan(2,0));
				dst_scan(2,1) = _1_det * (src_scan(0,1) * src_scan(2,0) - src_scan(0,0) * src_scan(2,1));
				dst_scan(2,2) = _1_det * (src_scan(0,0) * src_scan(1,1) - src_scan(0,1) * src_scan(1,0));
			}
			break;
		case 4:
			{
				// 余因子行列.
				TxScanner2D<double> dst_scan = dst.Scanner<double>();
				for(int row=0 ; row<rows ; row++)
				{
					for(int col=0 ; col<cols ; col++)
					{
						CxMatrix sub; sub.Filter().Submatrix(src, row,col);	// 小行列.
						double cofactor = sub.Det();						// 余因子. (小行列の行列式)
						int sign = ((row+col)%2 == 0) ? +1 : -1;			// 符号 (-1)^(i+j)
						dst_scan(col,row) = (cofactor * sign) * _1_det;		// 余因子 * 符号 * 係数 (※同時に転置する)
					}
				}
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Submatrix	(HxModule hdst, HxModule hsrc, int row, int col)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (xie::Axi::ClassIs<CxMatrix>(hsrc) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix	src = CxMatrix::FromTag(reinterpret_cast<CxMatrix*>(hsrc)->Tag());
		if (src.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (IsCompatible(src.Tag()) == false)
			src = reinterpret_cast<CxMatrix*>(hsrc)->Clone();
		int rows = src.Rows();
		int cols = src.Columns();
		if (rows <= 1 || cols <= 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.IsValid() == false)
		{
			dst.Resize(rows-1,cols-1);
		}

		if (dst.Rows() != rows-1 || dst.Columns() != cols-1 || !IsCompatible(dst.Tag()))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxScanner2D<double> src_scan = src.Scanner<double>();
		TxScanner2D<double> dst_scan = dst.Scanner<double>();

		for(int r=0 ; r<rows ; r++)
		{
			for(int c=0 ; c<cols ; c++)
			{
				if (r == row) continue;
				if (c == col) continue;

				int dr = (r < row) ? r : r - 1;
				int dc = (c < col) ? c : c - 1;

				dst_scan(dr, dc) = src_scan(r, c);
			}
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Product		(HxModule hdst, HxModule hsrc, HxModule hval)
{
	try
	{
		typedef double	TE;
		TxModel model = ModelOf<TE>();

		CxMatrix* psrc = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (psrc == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (psrc->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* pval = xie::Axi::SafeCast<CxMatrix>(hval);
		if (pval == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (pval->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// ------------------------------------------------------------
		/*
			dst = src * val
		*/

		if (psrc->Columns() != pval->Rows())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		int dst_rows = psrc->Rows();
		int dst_cols = pval->Columns();

		CxMatrix* pdst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (pdst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (pdst->IsValid() == false)
			pdst->Resize(dst_rows, dst_cols, model);
		if (pdst->Rows() != dst_rows || pdst->Columns() != dst_cols)
			throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

		// ------------------------------------------------------------

		CxMatrix src;
		if (IsCompatible(psrc->Tag()))
			src.Attach(*psrc);
		else
		{
			src.Resize(psrc->Size(), model);
			src.Filter().Copy(*psrc);
		}

		CxMatrix val;
		if (IsCompatible(pval->Tag()))
			val.Attach(*pval);
		else
		{
			val.Resize(pval->Size(), model);
			val.Filter().Copy(*pval);
		}

		CxMatrix dst;
		if (IsCompatible(pdst->Tag()))
			dst.Attach(*pdst);
		else
			dst.Resize(pdst->Size(), model);

		TxScanner2D<double> src_scan = src.Scanner<double>();
		TxScanner2D<double> val_scan = val.Scanner<double>();
		TxScanner2D<double> dst_scan = dst.Scanner<double>();

		dst_scan.ForEach(
			[&src_scan, &val_scan](int y, int x, double* _dst)
			{
				double sum = 0;
				for(int i=0; i<src_scan.Width; i++ )
					sum += (src_scan(y, i) * val_scan(i, x));
				*_dst = sum;
			}
		);

		if (IsCompatible(pdst->Tag()) == false)
			pdst->Filter().Copy(dst);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Not			(HxModule hdst, HxModule hsrc)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			dst->Resize(src->Size(), src->Model());
		}

		TxImage dst_tag = To2D(dst->Tag());
		TxImage src_tag = To2D(src->Tag());
		TxImage mask_tag = TxImage::Default();

		ExStatus status = fnPRV_Core_2D_Not(dst_tag, mask_tag, src_tag);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Math		(HxModule hdst, HxModule hsrc, ExMath mode)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Size(), src->Model());

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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Ope1A		(HxModule hdst, HxModule hsrc, double value, ExOpe1A mode)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Size(), src->Model());

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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Ope2A		(HxModule hdst, HxModule hsrc, HxModule hval, ExOpe2A mode)
{
	try
	{
		CxMatrix* src = xie::Axi::SafeCast<CxMatrix>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* val = xie::Axi::SafeCast<CxMatrix>(hval);
		if (val == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (val->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* dst = xie::Axi::SafeCast<CxMatrix>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Size(), src->Model());

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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Rotate		(HxModule hdst, double degree, double axis_x, double axis_y)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.Rows() != 3 || dst.Columns() != 3 || !IsCompatible(dst.Tag()))
			dst.Resize(3, 3);
		dst.Reset();

		TxScanner2D<double> dst_scan = dst.Scanner<double>();
		double rad = Axi::DegToRad(degree);
		double cosR = cos(rad);
		double sinR = sin(rad);

		// [+cos(R)][-sin(R)][ax - (ax * cos(R) - ay * sin(R))]
		// [+sin(R)][+cos(R)][ay - (ax * sin(R) + ay * cos(R))]
		// [      0][      0][ 1]

		dst_scan(0, 0) = +cosR;
		dst_scan(0, 1) = -sinR;
		dst_scan(0, 2) = axis_x - (axis_x * cosR - axis_y * sinR);
		dst_scan(1, 0) = +sinR;
		dst_scan(1, 1) = +cosR;
		dst_scan(1, 2) = axis_y - (axis_x * sinR + axis_y * cosR);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Scale		(HxModule hdst, double sx, double sy)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.Rows() != 3 || dst.Columns() != 3 || !IsCompatible(dst.Tag()))
			dst.Resize(3, 3);
		dst.Reset();

		TxScanner2D<double> dst_scan = dst.Scanner<double>();

		// [sx][ 0][ 0]
		// [ 0][sy][ 0]
		// [ 0][ 0][ 1]

		dst_scan(0, 0) = sx;
		dst_scan(1, 1) = sy;

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Translate	(HxModule hdst, double tx, double ty)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.Rows() != 3 || dst.Columns() != 3 || !IsCompatible(dst.Tag()))
			dst.Resize(3, 3);
		dst.Reset();

		TxScanner2D<double> dst_scan = dst.Scanner<double>();

		// [ 1][ 0][tx]
		// [ 0][ 1][ty]
		// [ 0][ 0][ 1]

		dst_scan(0, 2) = tx;
		dst_scan(1, 2) = ty;

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Matrix_Preset_Shear		(HxModule hdst, double degree_x, double degree_y)
{
	try
	{
		if (xie::Axi::ClassIs<CxMatrix>(hdst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix&	dst = *reinterpret_cast<CxMatrix*>(hdst);
		if (dst.Rows() != 3 || dst.Columns() != 3 || !IsCompatible(dst.Tag()))
			dst.Resize(3, 3);
		dst.Reset();

		double rad_x = Axi::DegToRad(degree_x);
		double rad_y = Axi::DegToRad(degree_y);

		TxScanner2D<double> dst_scan = dst.Scanner<double>();

		// [      1][tan(rx)][ 0]
		// [tan(ry)][      1][ 0]
		// [      0][      0][ 1]

		dst_scan(0, 1) = tan(rad_x);
		dst_scan(1, 0) = tan(rad_y);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
