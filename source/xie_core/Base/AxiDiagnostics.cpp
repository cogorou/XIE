/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/AxiDiagnostics.h"
#include "Core/CxException.h"

namespace xie
{

// ==================================================
CxStringA ToString(ExStatus value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExStatus::Success:			result = "Success"; break;
	case ExStatus::InvalidParam:	result = "InvalidParam"; break;
	case ExStatus::InvalidObject:	result = "InvalidObject"; break;
	case ExStatus::MemoryError:		result = "MemoryError"; break;
	case ExStatus::NotFound:		result = "NotFound"; break;
	case ExStatus::Impossible:		result = "Impossible"; break;
	case ExStatus::Interrupted:		result = "Interrupted"; break;
	case ExStatus::IOError:			result = "IOError"; break;
	case ExStatus::Timeout:			result = "Timeout"; break;
	case ExStatus::Unsupported:		result = "Unsupported"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExStatus::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExType value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExType::None:	result = "None"; break;
	case ExType::Ptr:	result = "Ptr"; break;
	case ExType::U8:	result = "U8"; break;
	case ExType::U16:	result = "U16"; break;
	case ExType::U32:	result = "U32"; break;
	case ExType::U64:	result = "U64"; break;
	case ExType::S8:	result = "S8"; break;
	case ExType::S16:	result = "S16"; break;
	case ExType::S32:	result = "S32"; break;
	case ExType::S64:	result = "S64"; break;
	case ExType::F32:	result = "F32"; break;
	case ExType::F64:	result = "F64"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExType::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(TxModel value, bool is_fullname)
{
	if (is_fullname)
		return CxStringA::Format("xie::TxModel::%s(%d)", ToString(value.Type).Address(), value.Pack);
	else
		return CxStringA::Format("%s(%d)", ToString(value.Type).Address(), value.Pack);
}

// ==================================================
CxStringA ToString(ExBoolean value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExBoolean::False:	result = "False"; break;
	case ExBoolean::True:	result = "True"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExBoolean::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExScanDir value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExScanDir::X:	result = "X"; break;
	case ExScanDir::Y:	result = "Y"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExScanDir::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExEndianType value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExEndianType::None:	result = "None"; break;
	case ExEndianType::LE:		result = "LE"; break;
	case ExEndianType::BE:		result = "BE"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExEndianType::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExMath value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExMath::Abs:		result = "Abs"; break;
	case ExMath::Sign:		result = "Sign"; break;
	case ExMath::Sqrt:		result = "Sqrt"; break;
	case ExMath::Exp:		result = "Exp"; break;
	case ExMath::Log:		result = "Log"; break;
	case ExMath::Log10:		result = "Log10"; break;
	case ExMath::Sin:		result = "Sin"; break;
	case ExMath::Cos:		result = "Cos"; break;
	case ExMath::Tan:		result = "Tan"; break;
	case ExMath::Sinh:		result = "Sinh"; break;
	case ExMath::Cosh:		result = "Cosh"; break;
	case ExMath::Tanh:		result = "Tanh"; break;
	case ExMath::Asin:		result = "Asin"; break;
	case ExMath::Acos:		result = "Acos"; break;
	case ExMath::Atan:		result = "Atan"; break;
	case ExMath::Ceiling:	result = "Celing"; break;
	case ExMath::Floor:		result = "Floor"; break;
	case ExMath::Round:		result = "Round"; break;
	case ExMath::Truncate:	result = "Truncate"; break;
	case ExMath::Modf:		result = "Modf"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExMath::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExOpe1A value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExOpe1A::Add:		result = "Add"; break;
	case ExOpe1A::Atan2:	result = "Atan2"; break;
	case ExOpe1A::Atan2Inv:	result = "Atan2Inv"; break;
	case ExOpe1A::Diff:		result = "Diff"; break;
	case ExOpe1A::Div:		result = "Div"; break;
	case ExOpe1A::DivInv:	result = "DivInv"; break;
	case ExOpe1A::Max:		result = "Max"; break;
	case ExOpe1A::Min:		result = "Min"; break;
	case ExOpe1A::Mod:		result = "Mod"; break;
	case ExOpe1A::ModInv:	result = "Modf"; break;
	case ExOpe1A::Mul:		result = "Mul"; break;
	case ExOpe1A::Pow:		result = "Pow"; break;
	case ExOpe1A::PowInv:	result = "PowInv"; break;
	case ExOpe1A::Sub:		result = "Sub"; break;
	case ExOpe1A::SubInv:	result = "SubInv"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExOpe1A::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExOpe1L value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExOpe1L::And:		result = "And"; break;
	case ExOpe1L::Nand:		result = "Nand"; break;
	case ExOpe1L::Or:		result = "Or"; break;
	case ExOpe1L::Xor:		result = "Xor"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExOpe1L::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExOpe2A value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExOpe2A::Add:		result = "Add"; break;
	case ExOpe2A::Atan2:	result = "Atan2"; break;
	case ExOpe2A::Diff:		result = "Diff"; break;
	case ExOpe2A::Div:		result = "Div"; break;
	case ExOpe2A::Max:		result = "Max"; break;
	case ExOpe2A::Min:		result = "Min"; break;
	case ExOpe2A::Mod:		result = "Mod"; break;
	case ExOpe2A::Mul:		result = "Mul"; break;
	case ExOpe2A::Pow:		result = "Pow"; break;
	case ExOpe2A::Sub:		result = "Sub"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExOpe2A::%s", result.Address());
	return result;
}

// ==================================================
CxStringA ToString(ExOpe2L value, bool is_fullname)
{
	CxStringA result;
	switch(value)
	{
	case ExOpe2L::And:		result = "And"; break;
	case ExOpe2L::Nand:		result = "Nand"; break;
	case ExOpe2L::Or:		result = "Or"; break;
	case ExOpe2L::Xor:		result = "Xor"; break;
	}
	if (result.IsValid() == false)
		result = "";
	else if (is_fullname)
		result = CxStringA::Format("xie::ExOpe2L::%s", result.Address());
	return result;
}

}	// xie
