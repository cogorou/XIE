
#pragma once

namespace User
{

void CxExif_01();
void CxExif_EndianType();
void CxExif_GetItems();
void CxExif_GetPurgedExif();
void CxExif_GetValue();
void CxExif_SetValue();

// ============================================================
void CxExif()
{
	User::CxExif_01();
	User::CxExif_EndianType();
	User::CxExif_GetItems();
	User::CxExif_GetPurgedExif();
	User::CxExif_GetValue();
	User::CxExif_SetValue();
}

}
