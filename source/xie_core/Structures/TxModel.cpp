/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxModel.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxModel::TxModel()
{
	Type	= ExType::None;
	Pack	= 0;
}

// ============================================================
TxModel::TxModel(ExType type, int pack)
{
	Type	= type;
	Pack	= pack;
}

// ============================================================
bool TxModel::operator == (const TxModel& cmp) const
{
	const TxModel& src = *this;
	if (src.Type	!= cmp.Type) return false;
	if (src.Pack	!= cmp.Pack) return false;
	return true;
}

// ============================================================
bool TxModel::operator != (const TxModel& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
int TxModel::Size() const
{
	return (Pack < 0) ? 0 : SizeOf(Type) * Pack;
}

// ============================================================
TxModel TxModel::operator + (int value) const
{
	TxModel ans;
	ans.Type = this->Type;
	ans.Pack = this->Pack + value;
	return ans;
}

// ============================================================
TxModel& TxModel::operator += (int value)
{
	this->Pack += value;
	return *this;
}

// ============================================================
TxModel TxModel::operator - (int value) const
{
	TxModel ans;
	ans.Type = this->Type;
	ans.Pack = this->Pack - value;
	return ans;
}

// ============================================================
TxModel& TxModel::operator -= (int value)
{
	this->Pack -= value;
	return *this;
}

// ============================================================
TxModel TxModel::operator * (int value) const
{
	TxModel ans;
	ans.Type = this->Type;
	ans.Pack = this->Pack * value;
	return ans;
}

// ============================================================
TxModel& TxModel::operator *= (int value)
{
	this->Pack *= value;
	return *this;
}

// ============================================================
TxModel TxModel::operator / (int value) const
{
	TxModel ans;
	ans.Type = this->Type;
	ans.Pack = (value != 0)
		? this->Pack / value
		: this->Pack;
	return ans;
}

// ============================================================
TxModel& TxModel::operator /= (int value)
{
	if (value != 0)
		this->Pack /= value;
	return *this;
}

}
