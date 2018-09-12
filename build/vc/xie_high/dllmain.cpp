/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"

#pragma warning (disable:4100)	// 引数は関数の本体部で 1 度も参照されません.

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

