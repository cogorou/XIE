/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/CxImage.h"

#include "api_data.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/CxExif.h"
#include "Core/CxArray.h"
#include "Core/CxMatrix.h"
#include "Effectors/CxRgbToGray.h"
#include "File/api_file.h"

namespace xie
{

static const char* g_ClassName = "CxImage";

// ============================================================
void CxImage::_Constructor()
{
	m_Tag = TxImage::Default();
	m_Exif = TxExif::Default();
	m_IsAttached = false;
	m_IsLocked = false;
}

// ============================================================
CxImage CxImage::FromTag(const TxImage& src)
{
	CxImage dst;
	dst.Attach(src);
	return dst;
}

// ============================================================
CxImage::CxImage()
{
	_Constructor();
}

// ============================================================
CxImage::CxImage(CxImage&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxImage::CxImage(const CxImage& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxImage::CxImage(int width, int height, TxModel model, int channels, int packing_size)
{
	_Constructor();
	Resize(width, height, model, channels, packing_size);
}

// ============================================================
CxImage::CxImage(const TxImageSize& size, int packing_size)
{
	_Constructor();
	Resize(size, packing_size);
}

// ============================================================
CxImage::CxImage(TxCharCPtrA filename, bool unpack)
{
	_Constructor();
	Load(filename, unpack);
}

// ============================================================
CxImage::CxImage(TxCharCPtrW filename, bool unpack)
{
	_Constructor();
	Load(filename, unpack);
}

// ============================================================
CxImage::~CxImage()
{
	Dispose();
}

// ============================================================
CxImage& CxImage::operator = ( CxImage&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxImage& CxImage::operator = ( const CxImage& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxImage::operator == ( const CxImage& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxImage::operator != ( const CxImage& src ) const
{
	return !ContentEquals(src);
}

// ============================================================
TxImage CxImage::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxImage::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
TxExif CxImage::Exif() const
{
	return m_Exif;
}

// ============================================================
void CxImage::Exif(const TxExif& value)
{
	if (m_Exif == value) return;
	if (m_Exif.Address == value.Address)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (m_Exif.Address != NULL)
		xie::Axi::MemoryFree(m_Exif.Address);
	m_Exif = TxExif::Default();

	if (value.Address != NULL && value.Length > 0)
	{
		const void*	addr = value.Address;
		int			length = value.Length;
		TxModel		model = value.Model;
		TxIntPtr	size = (TxIntPtr)length * model.Size();

		m_Exif.Address	= xie::Axi::MemoryAlloc(size, true);
		if (m_Exif.Address == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		m_Exif.Length	= length;
		m_Exif.Model	= model;

		memcpy(m_Exif.Address, addr, size);
	}
}

// ============================================================
void CxImage::ExifCopy(const TxExif& exif, bool ltc)
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage& dst = *this;

	auto src_exif = xie::CxExif::FromTag(exif);
	auto dst_exif = src_exif.GetPurgedExif();

	if (dst_exif.IsValid())
	{
		auto items = dst_exif.GetItems();

		for (int i=0 ; i<items.Length() ; i++)
		{
			auto item = items[i];

			switch (item.ID)
			{
				case 0x0132:	// File change date and time
					if (item.Type == 2 && item.Count >= 19)
					{
						auto now = TxDateTime::Now(ltc);
						auto timestamp = CxStringA::Format(
							"%04d:%02d:%02d %02d:%02d:%02d",
							now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
						dst_exif.SetValue(item, timestamp);
					}
					break;
				case 0x0100:	// image width
				case 0xA002:	// Valid image width
					if (item.Type == 3 && item.Count == 1)
					{
						CxArray value(1,TxModel::U16(1));
						auto ptr = (unsigned short*)value.Address();
						ptr[0] = (unsigned short)dst.Width();
						dst_exif.SetValue(item, value);
					}
					else if (item.Type == 4 && item.Count == 1)
					{
						CxArray value(1, TxModel::U32(1));
						auto ptr = (unsigned int*)value.Address();
						ptr[0] = (unsigned int)dst.Width();
						dst_exif.SetValue(item, value);
					}
					break;
				case 0x0101:	// image height
				case 0xA003:	// Valid image height
					if (item.Type == 3 && item.Count == 1)
					{
						CxArray value(1, TxModel::U16(1));
						auto ptr = (unsigned short*)value.Address();
						ptr[0] = (unsigned short)dst.Height();
						dst_exif.SetValue(item, value);
					}
					else if (item.Type == 4 && item.Count == 1)
					{
						CxArray value(1, TxModel::U32(1));
						auto ptr = (unsigned int*)value.Address();
						ptr[0] = (unsigned int)dst.Height();
						dst_exif.SetValue(item, value);
					}
					break;
			}
		}
	}

	dst.Exif(dst_exif.Tag());
}

// ============================================================
void CxImage::Dispose()
{
	// Even if this attached to an external memory, it must be unlocked if locked.
	// However, there can be no such condition. seealso Lock() method.
	if (IsLocked())
		Unlock();

	if (IsAttached() == false)
	{
		for(int i=0 ; i<XIE_IMAGE_CHANNELS_MAX ; i++)
		{
			Axi::MemoryFree( Address(i) );
		}
	}
	Exif(TxExif::Default());
	m_Tag = TxImage::Default();
	m_IsAttached = false;
	m_IsLocked = false;
}

// ================================================================================
void CxImage::MoveFrom(CxImage& src)
{
	if (this == &src) return;

	CxImage& dst = *this;
	dst.Dispose();
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	dst.m_IsLocked		= src.m_IsLocked;
	src.m_IsAttached	= true;
	dst.m_Exif			= src.m_Exif;
	src.m_Exif			= TxExif::Default();
}

// ============================================================
void CxImage::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxArray>(dst))
	{
		const CxImage&	_src = *this;
		CxArray&		_dst = static_cast<CxArray&>(dst);

		_dst.Dispose();
		if (_src.IsValid())
		{
			int length = _src.Width() * _src.Height();
			_dst.Resize(length, _src.Model());

			TxArray dst_tag = _dst.Tag();
			TxImage src_tag = _src.Tag();

			ExStatus status = fnPRV_Core_2D_Serialize(dst_tag, src_tag);
			if (status != ExStatus::Success)
				throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		}

		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxImage>(src))
	{
		auto&	_src = static_cast<const CxImage&>(src);
		auto&	_dst = *this;
		_dst.Dispose();
		if (_src.IsValid())
		{
			_dst.Resize(_src.ImageSize());
			_dst.Depth(_src.Depth());
			_dst.Filter().Copy(_src);
			_dst.Exif(_src.Exif());
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
bool CxImage::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxImage>(src))
	{
		auto& _src = static_cast<const CxImage&>(src);
		auto& _dst = *this;

		if (_dst.Width()	!= _src.Width()) return false;
		if (_dst.Height()	!= _src.Height()) return false;
		if (_dst.Model()	!= _src.Model()) return false;
		if (_dst.Channels()	!= _src.Channels()) return false;
		if (_dst.Depth()	!= _src.Depth()) return false;

		int height = _src.Height();
		int wbytes = _src.Model().Size() * _src.Width();
		for(int ch=0 ; ch<_src.Channels() ; ch++)
		{
			for(int y=0 ; y<height ; y++)
			{
				const void*	src_addr = _src(ch, y, 0);
				const void* dst_addr = _dst(ch, y, 0);
				if (memcmp(dst_addr, src_addr, wbytes) != 0) return false;
			}
		}

		// Exif
		{
			const TxExif _src_tag = _src.Exif();
			const TxExif _dst_tag = _dst.Exif();
			if (_dst_tag.Length != _src_tag.Length) return false;
			if (_dst_tag.Address != NULL && _src_tag.Address == NULL) return false;
			if (_dst_tag.Address == NULL && _src_tag.Address != NULL) return false;
			if (_dst_tag.Address != NULL && _src_tag.Address != NULL)
			{
				int length = _src_tag.Length;
				const void*	src_addr = _src_tag.Address;
				const void* dst_addr = _dst_tag.Address;
				if (memcmp(dst_addr, src_addr, length) != 0) return false;
			}
		}

		return true;
	}
	return false;
}

// ============================================================
bool CxImage::IsValid() const
{
	if (!(0 < m_Tag.Channels && m_Tag.Channels <= XIE_IMAGE_CHANNELS_MAX)) return false;
	if (m_Tag.Width <= 0) return false;
	if (m_Tag.Height <= 0) return false;

	int min_stride = m_Tag.Model.Size() * m_Tag.Width;
	if (min_stride <= 0) return false;

	if (m_Tag.Stride < min_stride) return false;

	for(int ch=0 ; ch<m_Tag.Channels ; ch++)
	{
		if (Address(ch) == NULL) return false;
	}
	return true;
}

// ============================================================
bool CxImage::IsAttached() const
{
	return m_IsAttached;
}

// ============================================================
void CxImage::Attach(const IxModule& src)
{
	this->Dispose();

	if (xie::Axi::ClassIs<CxImage>(src))
	{
		const CxImage& _src = static_cast<const CxImage&>(src);
		this->m_IsAttached = true;
		this->m_Tag = _src.m_Tag;
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::Attach(const TxImage& src)
{
	this->Dispose();
	this->m_IsAttached = true;
	this->m_Tag = src;
}

// ============================================================
void CxImage::Attach(const CxImage& src, const TxRectangleI& bounds)
{
	this->Dispose();

	if (bounds.X < 0 ||
		bounds.Y < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		const TxImage&	src_tag = src.m_Tag;
		TxImage			dst_tag;

		// ----- bounds
		int x = bounds.X;
		int y = bounds.Y;
		if (!(0 <= x && 0 <= y))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		int width  = bounds.Width;
		int height = bounds.Height;
		if (width <= 0)
			width = src_tag.Width - x;
		if (height <= 0)
			height = src_tag.Height - y;
		if (!(x + width <= src_tag.Width))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (!(y + height <= src_tag.Height))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		// ----- tag
		for(int i=0 ; i<src_tag.Channels ; i++)
		{
			if (src_tag.Layer[i] == NULL)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			dst_tag.Layer[i] = static_cast<char*>(src_tag.Layer[i])
								+ (y * (TxIntPtr)src_tag.Stride)
								+ (x * (TxIntPtr)src_tag.Model.Size());
		}
		dst_tag.Width		= width;
		dst_tag.Height		= height;
		dst_tag.Model		= src_tag.Model;
		dst_tag.Channels	= src_tag.Channels;
		dst_tag.Depth		= src_tag.Depth;
		dst_tag.Stride		= src_tag.Stride;

		this->m_IsAttached = true;
		this->m_Tag = dst_tag;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxImage::Attach(const CxImage& src, int ch)
{
	this->Dispose();

	if ((ch != -1) && !(0 <= ch && ch < src.Channels()))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		const TxImage&	src_tag = src.m_Tag;
		TxImage			dst_tag = TxImage::Default();

		// ----- channel
		if (ch == -1)
			dst_tag.Layer = src_tag.Layer;
		else
			dst_tag.Layer[0] = src_tag.Layer[ch];

		// ----- tag
		dst_tag.Width		= src_tag.Width;
		dst_tag.Height		= src_tag.Height;
		dst_tag.Model		= src_tag.Model;
		dst_tag.Channels	= (ch == -1) ? src_tag.Channels : 1;
		dst_tag.Depth		= src_tag.Depth;
		dst_tag.Stride		= src_tag.Stride;

		this->m_IsAttached = true;
		this->m_Tag = dst_tag;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxImage::Attach(const CxImage& src, int ch, const TxRectangleI& bounds)
{
	this->Dispose();

	if ((ch != -1) && !(0 <= ch && ch < src.Channels()))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (bounds.X < 0 ||
		bounds.Y < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		const TxImage&	src_tag = src.m_Tag;
		TxImage			dst_tag = TxImage::Default();

		// ----- bounds
		int x = bounds.X;
		int y = bounds.Y;
		if (!(0 <= x && 0 <= y))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		int width  = bounds.Width;
		int height = bounds.Height;
		if (width <= 0)
			width = src_tag.Width - x;
		if (height <= 0)
			height = src_tag.Height - y;
		if (!(x + width <= src_tag.Width))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (!(y + height <= src_tag.Height))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		// ----- channel
		if (ch == -1)
		{
			for(int i=0 ; i<src_tag.Channels ; i++)
			{
				if (src_tag.Layer[i] == NULL)
					throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
				dst_tag.Layer[i] = static_cast<char*>(src_tag.Layer[i])
									+ (y * (TxIntPtr)src_tag.Stride)
									+ (x * (TxIntPtr)src_tag.Model.Size());
			}
		}
		else
		{
			if (src_tag.Layer[ch] == NULL)
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			dst_tag.Layer[0] = static_cast<char*>(src_tag.Layer[ch])
								+ (y * (TxIntPtr)src_tag.Stride)
								+ (x * (TxIntPtr)src_tag.Model.Size());
		}

		// ----- tag
		dst_tag.Width		= width;
		dst_tag.Height		= height;
		dst_tag.Model		= src_tag.Model;
		dst_tag.Channels	= (ch == -1) ? src_tag.Channels : 1;
		dst_tag.Depth		= src_tag.Depth;
		dst_tag.Stride		= src_tag.Stride;

		this->m_IsAttached = true;
		this->m_Tag = dst_tag;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxImage::Lock()
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// You must not lock a memory when this is attached to the external memory.
	if (IsAttached() == true)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	for(int i=0 ; i<m_Tag.Channels ; i++)
	{
		void* addr = m_Tag.Layer[i];
		TxIntPtr size = m_Tag.Height * m_Tag.Stride;
		int status = Axi::MemoryLock(addr, size);
		if (status != 0)
		{
			fnXIE_Core_TraceOut(1, "%s(%d): MemoryLock failed in ch%d (%p). status=%d\n", __FILE__, __LINE__, i, addr, status);
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		}
	}

	this->m_IsLocked = true;
}

// ============================================================
void CxImage::Unlock()
{
	if (IsValid() == false) return;

	if (IsLocked() == true)
	{
		for(int i=0 ; i<m_Tag.Channels ; i++)
		{
			void* addr = m_Tag.Layer[i];
			TxIntPtr size = m_Tag.Height * m_Tag.Stride;
			int status = Axi::MemoryUnlock(addr, size);
			if (status != 0)
			{
				fnXIE_Core_TraceOut(1, "%s(%d): MemoryUnlock failed in ch%d (%p). status=%d\n", __FILE__, __LINE__, i, addr, status);
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			}
		}
	}

	this->m_IsLocked = false;
}

// ============================================================
bool CxImage::IsLocked() const
{
	return this->m_IsLocked;
}

// ============================================================
void CxImage::Resize(const TxImageSize& size, int packing_size)
{
	Resize(size.Width, size.Height, size.Model, size.Channels, packing_size);
	Depth(size.Depth);
}

// ============================================================
void CxImage::Resize(int width, int height, TxModel model, int channels, int packing_size)
{
	this->Dispose();

	if (width == 0 || height == 0) return;

	if (width < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (height < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(1 <= channels && channels <= XIE_IMAGE_CHANNELS_MAX))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		int stride = Axi::CalcStride(model, width, packing_size);
		if (stride <= 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		for(int i=0 ; i<channels ; i++)
		{
			void* addr = Axi::MemoryAlloc( (TxIntPtr)height * stride );
			if (addr == NULL)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			m_Tag.Layer[i] = addr;
		}

		m_Tag.Width		= width;
		m_Tag.Height	= height;
		m_Tag.Channels	= channels;
		m_Tag.Model		= model;
		m_Tag.Stride	= stride;
		m_Tag.Depth		= 0;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxImage::Reset()
{
	if (IsValid() == false) return;

	ExStatus status = fnXIE_Core_Image_Reset((HxModule)this);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::Clear(const void* value, TxModel model, HxModule hmask)
{
	ExStatus status = fnXIE_Core_Image_Clear((HxModule)this, hmask, value, model);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::ClearEx(const void* value, TxModel model, int index, int count, HxModule hmask)
{
	ExStatus status = fnXIE_Core_Image_ClearEx((HxModule)this, hmask, value, model, index, count);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
CxImage CxImage::Clone() const
{
	CxImage clone;
	clone.CopyFrom(*this);
	return clone;
}

// ============================================================
CxImage CxImage::Clone(TxModel model, int channels, double scale) const
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	TxModel dst_model;
	dst_model.Type = (model.Type != ExType::None) ? model.Type : this->Model().Type;
	dst_model.Pack = (model.Pack != 0) ? model.Pack : this->Model().Pack;

	int  dst_channels = (channels <= 0) ? this->Channels() : channels;

	CxImage clone(
		this->Width(),
		this->Height(),
		dst_model,
		dst_channels
		);

	clone.Filter().Copy(*this, scale);

	return clone;
}

// ============================================================
CxImage CxImage::Child() const
{
	CxImage child;
	child.Attach(*this);
	return child;
}

// ============================================================
CxImage CxImage::Child(const TxRectangleI& bounds) const
{
	CxImage child;
	child.Attach(*this, bounds);
	return child;
}

// ============================================================
CxImage CxImage::Child(int ch) const
{
	CxImage child;
	child.Attach(*this, ch);
	return child;
}

// ============================================================
CxImage CxImage::Child(int ch, const TxRectangleI& bounds) const
{
	CxImage child;
	child.Attach(*this, ch, bounds);
	return child;
}

// ============================================================
void CxImage::Load(TxCharCPtrA filename)
{
	LoadA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxImage::Load(TxCharCPtrW filename)
{
	LoadW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxImage::Save(TxCharCPtrA filename) const
{
	SaveA(filename, NULL, TxModel::Default());
}

// ============================================================
void CxImage::Save(TxCharCPtrW filename) const
{
	SaveW(filename, NULL, TxModel::Default());
}

// ============================================================
void CxImage::LoadA(TxCharCPtrA filename, const void* option, TxModel model)
{
	this->Dispose();

	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)strlen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	bool unpack = false;
	if (option != NULL)
	{
		if (model == TxModel::S8(1))
			unpack = (*((bool*)option));
		else
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringA::EndsWith(filename, ".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadRawA(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".bmp", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadBmpA(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".png", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadPngA(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".jpg", true) ||
		CxStringA::EndsWith(filename, ".jpeg", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadJpegA(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".tif", true) ||
		CxStringA::EndsWith(filename, ".tiff", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadTiffA(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::LoadW(TxCharCPtrW filename, const void* option, TxModel model)
{
	this->Dispose();

	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)wcslen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	bool unpack = false;
	if (option != NULL)
	{
		if (model == TxModel::S8(1))
			unpack = (*((bool*)option));
		else
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (CxStringW::EndsWith(filename, L".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadRawW(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".bmp", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadBmpW(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".png", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadPngW(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".jpg", true) ||
		CxStringW::EndsWith(filename, L".jpeg", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadJpegW(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".tif", true) ||
		CxStringW::EndsWith(filename, L".tiff", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_LoadTiffW(*this, filename, (unpack ? ExBoolean::True : ExBoolean::False));
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::SaveA(TxCharCPtrA filename, const void* option, TxModel model) const
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)strlen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (CxStringA::EndsWith(filename, ".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveRawA(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".bmp", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveBmpA(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".png", true))
	{
		int level = -1;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				level = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SavePngA(*this, filename, level);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".jpg", true) ||
		CxStringA::EndsWith(filename, ".jpeg", true))
	{
		int quality = 100;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				quality = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SaveJpegA(*this, filename, quality);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringA::EndsWith(filename, ".tif", true) ||
		CxStringA::EndsWith(filename, ".tiff", true))
	{
		int level = 0;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				level = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SaveTiffA(*this, filename, level);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxImage::SaveW(TxCharCPtrW filename, const void* option, TxModel model) const
{
	if (filename == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	int length = (int)wcslen(filename);
	if (length == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (CxStringW::EndsWith(filename, L".raw", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveRawW(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".bmp", true))
	{
		ExStatus status = xie::File::fnXIE_Core_File_SaveBmpW(*this, filename);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".png", true))
	{
		int level = -1;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				level = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SavePngW(*this, filename, level);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".jpg", true) ||
		CxStringW::EndsWith(filename, L".jpeg", true))
	{
		int quality = 100;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				quality = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SaveJpegW(*this, filename, quality);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	if (CxStringW::EndsWith(filename, L".tif", true) ||
		CxStringW::EndsWith(filename, L".tiff", true))
	{
		int level = 0;
		if (option != NULL)
		{
			if (model == TxModel::S32(1))
				level = *((int*)option);
			else
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}
		ExStatus status = xie::File::fnXIE_Core_File_SaveTiffW(*this, filename, level);
		if (status != ExStatus::Success)
			throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void* CxImage::OpenRawA(TxCharCPtrA filename, int mode)
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
void* CxImage::OpenRawW(TxCharCPtrW filename, int mode)
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
void CxImage::CloseRaw(void* handle)
{
	FILE* stream = (FILE*)handle;
	if (stream != NULL)
		fclose( stream );
}

// ============================================================
void CxImage::LoadRaw(void* handle)
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
		CxImage* dst = this;

		// データ識別情報.
		{
			TxImageSize image_size;
			uiReadSize = fread(&image_size, sizeof(image_size), 1, stream);
			if (uiReadSize != 1)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

			dst->Resize(image_size);
			if (dst->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}

		// データ読み込み.
		for(int ch=0 ; ch<dst->Channels() ; ch++)
		{
			for(int y=0 ; y<dst->Height() ; y++)
			{
				uiReadSize = fread((*dst)(ch, y, 0), dst->Model().Size(), dst->Width(), stream);
				if ((int)uiReadSize != dst->Width())
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}
	}
}

// ============================================================
void CxImage::SaveRaw(void* handle) const
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
		const CxImage* src = this;

		// ヘッダー書き込み.
		uiWriteSize = fwrite(&header, sizeof(TxRawFileHeader), 1, stream);
		if (uiWriteSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// データ識別情報.
		auto image_size = src->ImageSize();
		uiWriteSize = fwrite(&image_size, sizeof(image_size), 1, stream);
		if (uiWriteSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// データ書き込み.
		for(int ch=0 ; ch<src->Channels() ; ch++)
		{
			for(int y=0 ; y<src->Height() ; y++)
			{
				uiWriteSize = fwrite((*src)(ch, y, 0), src->Model().Size(), src->Width(), stream);
				if ((int)uiWriteSize != src->Width())
					throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			}
		}
	}
}

// ============================================================
int CxImage::Width() const
{
	return m_Tag.Width;
}

// ============================================================
int CxImage::Height() const
{
	return m_Tag.Height;
}

// ============================================================
int CxImage::Channels() const
{
	return m_Tag.Channels;
}

// ============================================================
TxModel CxImage::Model() const
{
	return m_Tag.Model;
}

// ============================================================
int CxImage::Stride() const
{
	return m_Tag.Stride;
}

// ============================================================
int CxImage::Depth() const
{
	return m_Tag.Depth;
}

// ============================================================
void CxImage::Depth(int value)
{
	m_Tag.Depth = value;
}

// ============================================================
TxSizeI CxImage::Size() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ============================================================
TxImageSize CxImage::ImageSize() const
{
	return TxImageSize(m_Tag.Width, m_Tag.Height, m_Tag.Model, m_Tag.Channels, m_Tag.Depth);
}

// ============================================================
void* CxImage::Address(int ch)
{
	return m_Tag.Layer[ch];
}

// ============================================================
const void* CxImage::Address(int ch) const
{
	return m_Tag.Layer[ch];
}

// ============================================================
void* CxImage::operator [] (int ch)
{
	return this->Address(ch);
}

// ============================================================
const void* CxImage::operator [] (int ch) const
{
	return this->Address(ch);
}

// ============================================================
void* CxImage::operator () (int ch, int y, int x)
{
	void* addr = this->Address(ch);
	if (addr == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= ch && ch < m_Tag.Channels))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= x && x < m_Tag.Width))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<unsigned char*>(addr)
		+ (y * (TxIntPtr)m_Tag.Stride)
		+ (x * (TxIntPtr)m_Tag.Model.Size());
}

// ============================================================
const void* CxImage::operator () (int ch, int y, int x) const
{
	const void* addr = this->Address(ch);
	if (addr == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= ch && ch < m_Tag.Channels))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= x && x < m_Tag.Width))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	return static_cast<const unsigned char*>(addr)
		+ (y * (TxIntPtr)m_Tag.Stride)
		+ (x * (TxIntPtr)m_Tag.Model.Size());
}

// ============================================================
int CxImage::CalcDepth(int ch, HxModule hmask) const
{
	int result = 0;
	ExStatus status = fnXIE_Core_Image_CalcDepth(*this, hmask, ch, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
TxStatistics CxImage::Statistics(int ch, HxModule hmask) const
{
	TxStatistics result;
	ExStatus status = fnXIE_Core_Image_Statistics(*this, hmask, ch, &result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxArray CxImage::Extract(int ch, int sy, int sx, int length, ExScanDir dir, HxModule hmask) const
{
	CxArray result;
	ExStatus status = fnXIE_Core_Image_Extract(*this, hmask, ch, sy, sx, length, dir, result);
	if (status != ExStatus::Success)
		throw CxException(status, __FUNCTION__, __FILE__, __LINE__);
	return result;
}

// ============================================================
CxImageFilter CxImage::Filter() const
{
	return CxImageFilter((HxModule)this, NULL);
}

// ============================================================
CxImageFilter CxImage::Filter(HxModule hmask) const
{
	return CxImageFilter(*this, hmask);
}

// //////////////////////////////////////////////////////////////////////
// Export
//

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Image_Attach(HxModule handle, TxImage src)
{
	try
	{
		CxImage* _src = xie::Axi::SafeCast<CxImage>(handle);
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
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Image_Resize(HxModule handle, int width, int height, TxModel model, int channels, int packing_size)
{
	try
	{
		CxImage* _src = xie::Axi::SafeCast<CxImage>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Resize(width, height, model, channels, packing_size);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Image_Exif_Get(HxModule handle, TxExif* tag)
{
	try
	{
		CxImage* _src = xie::Axi::SafeCast<CxImage>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (tag != NULL)
			*tag = _src->Exif();
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Image_Exif_Set(HxModule handle, TxExif tag)
{
	try
	{
		CxImage* _src = xie::Axi::SafeCast<CxImage>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->Exif(tag);
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus XIE_API fnXIE_Core_Image_ExifCopy(HxModule handle, TxExif tag, ExBoolean ltc)
{
	try
	{
		CxImage* _src = xie::Axi::SafeCast<CxImage>(handle);
		if (_src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		_src->ExifCopy(tag, (ltc == ExBoolean::True));
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_CalcDepth	(HxModule hsrc, HxModule hmask, int ch, int* result)
{
	try
	{
		if (result != NULL)
			*result = 0;

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (src->IsValid())
		{
			TxStatistics stat;
			if (0 <= ch && ch < src->Channels())
			{
				// 指定のチャネルの統計.
				stat = src->Statistics(ch, hmask);
			}
			else
			{
				// 全てのチャネルの統計.
				for(int i=0 ; i<src->Channels() ; i++)
					stat += src->Statistics(i, hmask);
			}

			double value = xie::Axi::Max(_abs(stat.Min), _abs(stat.Max));
			if (value != 0)
			{
				for(int n=1 ; n<=64 ; n++)
				{
					if (value <= (pow(2.0, n)-1))
					{
						if (result != NULL)
							*result = n;
						break;
					}
				}
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Statistics	(HxModule hsrc, HxModule hmask, int ch, TxStatistics* result)
{
	try
	{
		if (result == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage src_tag = src->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Extract		(HxModule hsrc, HxModule hmask, int ch, int sy, int sx, int length, ExScanDir dir, HxModule hresult)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxArray* result = xie::Axi::SafeCast<CxArray>(hresult);
		if (result == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (result->IsValid() == false)
			result->Resize(length, src->Model());

		TxImage src_tag = src->Tag();
		TxImage mask_tag = TxImage::Default();
		TxArray result_tag = result->Tag();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Extract(src_tag, mask_tag, ch, sy, sx, length, dir, result_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Reset		(HxModule hdst)
{
	try
	{
		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		ExStatus status = ExStatus::Success;
		if (dst->IsValid())
		{
			TxImage dst_tag = dst->Tag();
			TxImage mask_tag = TxImage::Default();

			const int	value = 0;
			TxModel	model = xie::ModelOf(value);

			status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, &value, model);
		}
		dst->Depth(0);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Clear		(HxModule hdst, HxModule hmask, const void* value, TxModel model)
{
	try
	{
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Clear(dst_tag, mask_tag, value, model);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_ClearEx		(HxModule hdst, HxModule hmask, const void* value, TxModel model, int index, int count)
{
	try
	{
		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_ClearEx(dst_tag, mask_tag, value, model, index, count);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Cast			(HxModule hdst, HxModule hmask, HxModule hsrc)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Cast(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Copy		(HxModule hdst, HxModule hmask, HxModule hsrc, double scale)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		// Scale
		if (scale == 0)
			scale = 1;

		ExStatus status = ExStatus::Success;

		if (src_tag.Model.Pack >= 3 && dst_tag.Model.Pack == 1 && src_tag.Channels == dst_tag.Channels)
		{
			xie::Effectors::CxRgbToGray effector(scale);
			effector.Execute(hsrc, *dst, hmask);
		}
		else if (src_tag.Model.Pack == 1 && dst_tag.Model.Pack == 1 && src_tag.Channels >= 3 && dst_tag.Channels == 1)
		{
			xie::Effectors::CxRgbToGray effector(scale);
			effector.Execute(hsrc, *dst, hmask);
		}
		else if (scale != 1)
		{
			status = fnPRV_Core_2D_Convert(dst_tag, mask_tag, src_tag, scale);
		}
		else if (src_tag.Channels == dst_tag.Channels && src_tag.Model == dst_tag.Model)
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
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_CopyEx		(HxModule hdst, HxModule hmask, HxModule hsrc, int index, int count)
{
	try
	{
		if (index < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (count < 1)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), TxModel(src->Model().Type, count), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_CopyEx(dst_tag, mask_tag, src_tag, index, count);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_RgbToBgr		(HxModule hdst, HxModule hmask, HxModule hsrc, double scale)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		// Scale
		if (scale == 0)
			scale = 1;

		// Convert
		ExStatus status = fnPRV_Core_2D_RgbToBgr(dst_tag, mask_tag, src_tag, scale);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Compare		(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hcmp, double error_range)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* cmp = xie::Axi::SafeCast<CxImage>(hcmp);
		if (cmp == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (cmp->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			int		width		= src->Width();
			int		height		= src->Height();
			ExType	type		= ExType::U8;
			int		pack		= Axi::Min(src->Model().Pack, cmp->Model().Pack);
			int		channels	= Axi::Min(src->Channels(), cmp->Channels());

			dst->Resize(width, height, TxModel(type, pack), channels);
		}

		TxImage src_tag = src->Tag();
		TxImage cmp_tag = cmp->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Compare(dst_tag, mask_tag, src_tag, cmp_tag, error_range);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// Image - Filter

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_ColorMatrix	(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix* matrix = xie::Axi::SafeCast<CxMatrix>(hmatrix);
		if (matrix == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (matrix->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();
		TxMatrix matrix_tag = matrix->Tag();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_ColorMatrix(dst_tag, mask_tag, src_tag, matrix_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// Image - GeoTrans
// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Affine		(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hmatrix, int interpolation)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		CxMatrix* pmatrix = xie::Axi::SafeCast<CxMatrix>(hmatrix);
		if (pmatrix == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (pmatrix->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxMatrix matrix;
		if (IsCompatible(pmatrix->Tag()))
			matrix.Attach(*pmatrix);
		else
		{
			matrix.Resize(pmatrix->Size());
			matrix.Filter().Copy(*pmatrix);
		}

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();
		TxMatrix matrix_tag = matrix.Tag();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Affine(dst_tag, mask_tag, src_tag, matrix_tag, interpolation);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Mirror		(HxModule hdst, HxModule hmask, HxModule hsrc, int mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Mirror(dst_tag, mask_tag, src_tag, mode);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Rotate		(HxModule hdst, HxModule hmask, HxModule hsrc, int mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			int dst_w = 0;
			int dst_h = 0;
			switch(mode)
			{
			default:
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			case +1:
			case -1:
				dst_w = src->Height();
				dst_h = src->Width();
				break;
			case 0:
			case +2:
			case -2:
				dst_w = src->Width();
				dst_h = src->Height();
				break;
			}
			dst->Resize(dst_w, dst_h, src->Model(), src->Channels());
		}

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Rotate(dst_tag, mask_tag, src_tag, mode);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Transpose	(HxModule hdst, HxModule hmask, HxModule hsrc)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			int dst_w = src->Height();
			int dst_h = src->Width();
			dst->Resize(dst_w, dst_h, src->Model(), src->Channels());
		}

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Transpose(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Scale		(HxModule hdst, HxModule hmask, HxModule hsrc, double sx, double sy, int interpolation)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
		{
			int width = (int)round(src->Width() * sx);
			if (width < 1)
				width = 1;

			int height = (int)round(src->Height() * sy);
			if (height < 1)
				height = 1;

			dst->Resize(width, height, src->Model(), src->Channels());
		}

		CxMatrix matrix = CxMatrix::PresetScale(sx, sy);

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();
		TxMatrix matrix_tag = matrix.Tag();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Affine(dst_tag, mask_tag, src_tag, matrix_tag, interpolation);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Not				(HxModule hdst, HxModule hmask, HxModule hsrc)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage src_tag = src->Tag();
		TxImage dst_tag = dst->Tag();
		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		ExStatus status = fnPRV_Core_2D_Not(dst_tag, mask_tag, src_tag);

		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Math				(HxModule hdst, HxModule hmask, HxModule hsrc, ExMath mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		TxImage dst_tag = dst->Tag();
		TxImage src_tag = src->Tag();
		ExStatus status = fnPRV_Core_2D_Math(dst_tag, mask_tag, src_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope1A			(HxModule hdst, HxModule hmask, HxModule hsrc, double value, ExOpe1A mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		TxImage dst_tag = dst->Tag();
		TxImage src_tag = src->Tag();
		ExStatus status = fnPRV_Core_2D_Ope1A(dst_tag, mask_tag, src_tag, value, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope1L			(HxModule hdst, HxModule hmask, HxModule hsrc, unsigned int value, ExOpe1L mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		TxImage dst_tag = dst->Tag();
		TxImage src_tag = src->Tag();
		ExStatus status = fnPRV_Core_2D_Ope1L(dst_tag, mask_tag, src_tag, value, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope2A			(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2A mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* val = xie::Axi::SafeCast<CxImage>(hval);
		if (val == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (val->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		TxImage dst_tag = dst->Tag();
		TxImage src_tag = src->Tag();
		TxImage val_tag = val->Tag();
		ExStatus status = fnPRV_Core_2D_Ope2A(dst_tag, mask_tag, src_tag, val_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
XIE_EXPORT_FUNCTION ExStatus	XIE_API fnXIE_Core_Image_Ope2L			(HxModule hdst, HxModule hmask, HxModule hsrc, HxModule hval, ExOpe2L mode)
{
	try
	{
		CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
		if (src == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* val = xie::Axi::SafeCast<CxImage>(hval);
		if (val == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (val->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
		if (dst == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (dst->IsValid() == false)
			dst->Resize(src->Width(), src->Height(), src->Model(), src->Channels());

		TxImage mask_tag = TxImage::Default();

		if (hmask != NULL)
		{
			CxImage* mask = xie::Axi::SafeCast<CxImage>(hmask);
			if (mask == NULL)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			if (mask->IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			mask_tag = mask->Tag();
		}

		TxImage dst_tag = dst->Tag();
		TxImage src_tag = src->Tag();
		TxImage val_tag = val->Tag();
		ExStatus status = fnPRV_Core_2D_Ope2L(dst_tag, mask_tag, src_tag, val_tag, mode);
		return status;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
