/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXMODEL_H_INCLUDED_
#define _TXMODEL_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxModel
{
	ExType			Type;
	int				Pack;

#if defined(__cplusplus)
	// ============================================================
	static inline TxModel From(ExType type, int pack)
	{
		TxModel model;
		model.Type = type;
		model.Pack = pack;
		return model;
	}

	// ============================================================
	static inline TxModel Default()		{ return TxModel::From(ExType::None, 0); }
	static inline TxModel Ptr(int pack)	{ return TxModel::From(ExType::Ptr, pack); }
	static inline TxModel U8(int pack)	{ return TxModel::From(ExType::U8, pack); }
	static inline TxModel U16(int pack)	{ return TxModel::From(ExType::U16, pack); }
	static inline TxModel U32(int pack)	{ return TxModel::From(ExType::U32, pack); }
	static inline TxModel U64(int pack)	{ return TxModel::From(ExType::U64, pack); }
	static inline TxModel S8(int pack)	{ return TxModel::From(ExType::S8, pack); }
	static inline TxModel S16(int pack)	{ return TxModel::From(ExType::S16, pack); }
	static inline TxModel S32(int pack)	{ return TxModel::From(ExType::S32, pack); }
	static inline TxModel S64(int pack)	{ return TxModel::From(ExType::S64, pack); }
	static inline TxModel F32(int pack)	{ return TxModel::From(ExType::F32, pack); }
	static inline TxModel F64(int pack)	{ return TxModel::From(ExType::F64, pack); }

	// ============================================================
	static inline int SizeOf(ExType type)
	{
		switch(type)
		{
			default:			return 0;
			case ExType::Ptr:	return sizeof(void*);
			case ExType::U8:	return 1;
			case ExType::U16:	return 2;
			case ExType::U32:	return 4;
			case ExType::U64:	return 8;
			case ExType::S8:	return 1;
			case ExType::S16:	return 2;
			case ExType::S32:	return 4;
			case ExType::S64:	return 8;
			case ExType::F32:	return 4;
			case ExType::F64:	return 8;
		}
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxModel();
	TxModel(ExType type, int pack);
	bool operator == (const TxModel& cmp) const;
	bool operator != (const TxModel& cmp) const;
	int Size() const;

	// ============================================================
	TxModel  operator +  (int value) const;
	TxModel& operator += (int value);
	TxModel  operator -  (int value) const;
	TxModel& operator -= (int value);
	TxModel  operator *  (int value) const;
	TxModel& operator *= (int value);
	TxModel  operator /  (int value) const;
	TxModel& operator /= (int value);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

// ============================================================
#if defined(__cplusplus)
namespace xie
{

template<class T> static inline ExType TypeOf()				{ return ExType::None; }
template<class T> static inline ExType TypeOf(T value)		{ return TypeOf<T>(); }

template<class T> static inline TxModel ModelOf()			{ return TxModel::From(ExType::None, 0); }
template<class T> static inline TxModel ModelOf(T value)	{ return ModelOf<T>(); }

#if !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
template<> inline ExType TypeOf<HxModule>()				{ return ExType::Ptr; }
template<> inline ExType TypeOf<unsigned char>()		{ return ExType::U8; }
template<> inline ExType TypeOf<unsigned short>()		{ return ExType::U16; }
template<> inline ExType TypeOf<unsigned int>()			{ return ExType::U32; }
template<> inline ExType TypeOf<unsigned long long>()	{ return ExType::U64; }
template<> inline ExType TypeOf<char>()					{ return ExType::S8; }
template<> inline ExType TypeOf<short>()				{ return ExType::S16; }
template<> inline ExType TypeOf<int>()					{ return ExType::S32; }
template<> inline ExType TypeOf<long long>()			{ return ExType::S64; }
template<> inline ExType TypeOf<float>()				{ return ExType::F32; }
template<> inline ExType TypeOf<double>()				{ return ExType::F64; }
template<> inline ExType TypeOf<bool>()					{ return ExType::S8; }

template<> inline TxModel ModelOf<HxModule>()			{ return TxModel::From(ExType::Ptr, 1); }
template<> inline TxModel ModelOf<unsigned char>()		{ return TxModel::From(ExType::U8, 1); }
template<> inline TxModel ModelOf<unsigned short>()		{ return TxModel::From(ExType::U16, 1); }
template<> inline TxModel ModelOf<unsigned int>()		{ return TxModel::From(ExType::U32, 1); }
template<> inline TxModel ModelOf<unsigned long long>()	{ return TxModel::From(ExType::U64, 1); }
template<> inline TxModel ModelOf<char>()				{ return TxModel::From(ExType::S8, 1); }
template<> inline TxModel ModelOf<short>()				{ return TxModel::From(ExType::S16, 1); }
template<> inline TxModel ModelOf<int>()				{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<long long>()			{ return TxModel::From(ExType::S64, 1); }
template<> inline TxModel ModelOf<float>()				{ return TxModel::From(ExType::F32, 1); }
template<> inline TxModel ModelOf<double>()				{ return TxModel::From(ExType::F64, 1); }
template<> inline TxModel ModelOf<bool>()				{ return TxModel::From(ExType::S8, 1); }

template<> inline TxModel ModelOf<ExStatus>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExType>()				{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExBoolean>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExScanDir>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExEndianType>()		{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExMath>()				{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExOpe1A>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExOpe1L>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExOpe2A>()			{ return TxModel::From(ExType::S32, 1); }
template<> inline TxModel ModelOf<ExOpe2L>()			{ return TxModel::From(ExType::S32, 1); }
#endif

}	// xie
#endif	// __cplusplus

#endif
