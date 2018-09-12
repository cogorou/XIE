/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"

#pragma warning(disable : 4995)	// warning C4995: 'sprintf': ���O��������ꂽ #pragma �Ƃ��ċL�q����Ă��܂��B

#include <time.h>
#include <sys/stat.h>
#include <ShellAPI.h>

#include <string>
#include <vector>

// ======================================================================
LPCTSTR		EnvironmentKey	= _T("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment");
LPCTSTR		EnvKeyTOP	= _T("XIE100ROOT");
LPCTSTR		EnvKeyBIN	= _T("XIE100BIN");
LPCTSTR		EnvKeyASM	= _T("XIE100ASM");

const size_t _BUFFER_SIZE_ = 1024;

#if defined(_UNICODE) | defined(UNICODE)
	typedef std::wstring	_TSTRING;
#else
	typedef std::string		_TSTRING;
#endif

// ======================================================================
std::vector<_TSTRING> Split(const _TSTRING& str, const _TSTRING& delim);
BOOL IsWindowsVersionOK(DWORD dwMajor, DWORD dwMinor, WORD dwSPMajor);
_TSTRING GetEnvironmentSZ(LPCTSTR key);
void SetEnvironmentSZ(LPCTSTR key, LPCTSTR value);
void DelEnvironmentSZ(LPCTSTR key);
void FlushEnvironment();

// ======================================================================
/*
	@brief	�C���X�g�[��
*/
extern "C" UINT __stdcall InstallEnv(MSIHANDLE hInstall)
{
	HRESULT hr = S_OK;
	UINT er = ERROR_SUCCESS;

	// TODO: Add your custom action code here.
#if 1
	{
		// WiX Property
		TCHAR	DIR_BIN[MAX_PATH] = _T("");
		DWORD	length = MAX_PATH;
		MsiGetProperty (hInstall, _T("BIN"), DIR_BIN, &length);

		// ShellExecute
		std::string cmd = DIR_BIN;				// CAUTION: Assume as MBCS
		cmd += "\\XIEversion_100.exe";

		struct stat	statbuf = {0};
		int status = stat(cmd.c_str(), &statbuf);
		if (status == 0)
		{
			SHELLEXECUTEINFO	info = { 0 };
			info.cbSize			= sizeof(SHELLEXECUTEINFO);						
			info.hwnd			= NULL;
			info.lpFile			= cmd.c_str();
			info.lpParameters	= "/i";
			info.lpDirectory	= DIR_BIN;		// CAUTION: Assume as MBCS

			// Vista �ȍ~(UAC ���������ꂽ O/S)�ł� runas ���g�p����.����ȑO�ł� open ���g�p����.
			// Windows Vista = 6.0
			if (IsWindowsVersionOK(6,0,0))
				info.lpVerb		= "runas";
			else
				info.lpVerb		= "open";

			#ifdef _DEBUG
			info.nShow			= SW_SHOWNORMAL;
			#else
			info.nShow			= SW_HIDE;
			#endif
			info.fMask			= SEE_MASK_NOCLOSEPROCESS;

			if (TRUE == ::ShellExecuteEx(&info))
			{
				::WaitForSingleObject(info.hProcess, INFINITE);
			}
		}
	}
#else
	{
		// WiX Property
		TCHAR	DIR_TOP[MAX_PATH] = _T("");
		TCHAR	DIR_BIN[MAX_PATH] = _T("");

		DWORD length = MAX_PATH;
		MsiGetProperty (hInstall, _T("CustomActionData"), DIR_TOP, &length);

		length = _tcslen(DIR_TOP);
		if (0 < length)
		{
			if (DIR_TOP[length - 1] == _T('\\'))
				DIR_TOP[length - 1] = _T('\0');
			_sntprintf(DIR_BIN, MAX_PATH, _T("%s\\bin"), DIR_TOP);
		}

		// PATH ���ϐ�.
		INT		status = 0;
		HKEY	hkEnvironment = NULL;
		REGSAM	desired = KEY_WRITE|KEY_QUERY_VALUE;

		if (IsWindowsVersionOK(5, 1, 0))
			desired |= KEY_WOW64_32KEY;

		status = ::RegCreateKeyEx(HKEY_LOCAL_MACHINE, EnvironmentKey, 0, REG_NONE, REG_OPTION_NON_VOLATILE, desired, NULL, &hkEnvironment, NULL);
		if (ERROR_SUCCESS == status)
		{
			// ���ϐ��̎Q��.
			TCHAR	EnvRefBIN[_BUFFER_SIZE_];
			TCHAR	EnvRefASM[_BUFFER_SIZE_];

			_sntprintf(EnvRefBIN, _BUFFER_SIZE_, _T("%%%s%%"), EnvKeyBIN);
			_sntprintf(EnvRefASM, _BUFFER_SIZE_, _T("%%%s%%"), EnvKeyASM);

			// -- Open
			DWORD	dwType = 0;
			DWORD	dwPathLen = 0;
			status = ::RegQueryValueEx(hkEnvironment, _T("PATH"), 0, &dwType, NULL, &dwPathLen);
			if (ERROR_SUCCESS == status)
			{
				_TCHAR*	szPath = (_TCHAR*)calloc(dwPathLen + 256, 1);
				status = ::RegQueryValueEx(hkEnvironment, _T("PATH"), 0, &dwType, (BYTE*)szPath, &dwPathLen);
				if (ERROR_SUCCESS == status)
				{
					int len = _tcslen(szPath);
					if (0 < len && szPath[len - 1] != _T(';'))
					{
						szPath[len - 1] = _T(';');
						szPath[len - 0] = _T('\0');
					}

					_TSTRING	strPath = szPath;

					// %XIExxxBIN%
					if (_tcsstr(szPath, EnvRefBIN) == NULL)
						strPath += _TSTRING(EnvRefBIN) + _T(";");

					// %XIExxxASM%
					//if (_tcsstr(szPath, EnvRefASM) == NULL)
					//	strPath += _TSTRING(EnvRefASM) + _T(";");

					// note:
					// PATH ���ϐ��� REG_EXPAND_SZ �łȂ���΂Ȃ�܂���.
					// REG_SZ ���g�p�����ꍇ�́APATH �ɒǉ��������ϐ����W�J���ꂸ OS ������ɓ��삵�Ȃ��Ȃ�̂Œ��ӂ��K�v�ł�.
					// 
					// �})
					// ���W�X�g����̕�����: PATH=%SystemRoot%\system32;%SystemRoot%;...;
					//                            ~~~~~~~~~~~~
					// �W�J���ꂽ������    : PATH=C:\Windows\system32;C:\Windows;...;
					//                            ~~~~~~~~~~ PATH �� REG_EXPAND_SZ �̏ꍇ�́A���̂悤�ɓW�J����܂�.

					if (strPath.length() != 0)
						status = ::RegSetValueEx(hkEnvironment, _T("PATH"), 0, REG_EXPAND_SZ, (BYTE*)strPath.c_str(), (strPath.length() + 1) * sizeof(_TCHAR));
				}

				if (szPath != NULL)
					free(szPath);
				szPath = NULL;
			}

			// -- Close
			status = ::RegCloseKey( hkEnvironment );
		}

		// XIE ���ϐ�.
		{
			// note:
			// XIE �Ǝ��̊��ϐ��́A�Ӑ}�I�� REG_SZ �ɂ��Ă��܂�.
			// REG_EXPAND_SZ ���g�p�����ꍇ�́APATH �ɒǉ������ϐ���(Flush ���Ă�)�����ɓW�J����Ȃ����_������܂�.

			SetEnvironmentSZ( EnvKeyTOP, DIR_TOP );
			SetEnvironmentSZ( EnvKeyBIN, DIR_BIN );
			SetEnvironmentSZ( EnvKeyASM, DIR_BIN );

			FlushEnvironment();
		}
	}
#endif

//LExit:
	//er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
	er = ERROR_SUCCESS;
	return WcaFinalize(er);
}

// ======================================================================
/*
	@brief	�A���C���X�g�[��
*/
extern "C" UINT __stdcall UninstallEnv(MSIHANDLE hInstall)
{
	HRESULT hr = S_OK;
	UINT er = ERROR_SUCCESS;

	// TODO: Add your custom action code here.
#if 1
	{
		// WiX Property
		TCHAR	DIR_BIN[MAX_PATH] = _T("");
		DWORD	length = MAX_PATH;
		MsiGetProperty (hInstall, _T("BIN"), DIR_BIN, &length);

		// ShellExecute
		std::string cmd = DIR_BIN;				// CAUTION: Assume as MBCS
		cmd += "\\XIEversion_100.exe";

		struct stat	statbuf = {0};
		int status = stat(cmd.c_str(), &statbuf);
		if (status == 0)
		{
			SHELLEXECUTEINFO	info = { 0 };
			info.cbSize			= sizeof(SHELLEXECUTEINFO);						
			info.hwnd			= NULL;
			info.lpFile			= cmd.c_str();
			info.lpParameters	= "/u";
			info.lpDirectory	= DIR_BIN;		// CAUTION: Assume as MBCS

			// Vista �ȍ~(UAC ���������ꂽ O/S)�ł� runas ���g�p����.����ȑO�ł� open ���g�p����.
			// Windows Vista = 6.0
			if (IsWindowsVersionOK(6,0,0))
				info.lpVerb		= "runas";
			else
				info.lpVerb		= "open";

			#ifdef _DEBUG
			info.nShow			= SW_SHOWNORMAL;
			#else
			info.nShow			= SW_HIDE;
			#endif
			info.fMask			= SEE_MASK_NOCLOSEPROCESS;

			if (TRUE == ::ShellExecuteEx(&info))
			{
				::WaitForSingleObject(info.hProcess, INFINITE);
			}
		}
	}
#else
	{
		// PATH ���ϐ�.
		INT		status = 0;
		HKEY	hkEnvironment = NULL;
		REGSAM	desired = KEY_WRITE|KEY_QUERY_VALUE;

		if( IsWindowsVersionOK(5, 1, 0) )
			desired |= KEY_WOW64_32KEY;

		status = ::RegCreateKeyEx(HKEY_LOCAL_MACHINE, EnvironmentKey, 0, REG_NONE, REG_OPTION_NON_VOLATILE, desired, NULL, &hkEnvironment, NULL );
		if (ERROR_SUCCESS == status)
		{
			// ���ϐ��̎Q��.
			TCHAR	EnvRefBIN[_BUFFER_SIZE_];
			TCHAR	EnvRefASM[_BUFFER_SIZE_];

			_sntprintf(EnvRefBIN, _BUFFER_SIZE_, _T("%%%s%%"), EnvKeyBIN);
			_sntprintf(EnvRefASM, _BUFFER_SIZE_, _T("%%%s%%"), EnvKeyASM);

			// -- Open
			DWORD	dwPathLen = 0;
			status = ::RegQueryValueEx(hkEnvironment, _T("PATH"), 0, NULL, NULL, &dwPathLen);
			if (ERROR_SUCCESS == status)
			{
				_TCHAR*	szPath = (_TCHAR*)calloc(dwPathLen + 256, 1);
				status = ::RegQueryValueEx(hkEnvironment, _T("PATH"), 0, NULL, (BYTE*)szPath, &dwPathLen);
				if (ERROR_SUCCESS == status)
				{
					// PATH �̕����ƍĘA��.
					_TSTRING				strPath;
					std::vector<_TSTRING>	strItems = Split(szPath, _T(";"));
				
					for(int i=0 ; i<(int)strItems.size() ; i++)
					{
						if (strItems[i].length() == 0) continue;
						if (strItems[i] == EnvRefBIN) continue;
						if (strItems[i] == EnvRefASM) continue;

						strPath += strItems[i];
						strPath += _T(";");
					}

					// note:
					// PATH ���ϐ��� REG_EXPAND_SZ �łȂ���΂Ȃ�܂���.
					// REG_SZ ���g�p�����ꍇ�́APATH �ɒǉ��������ϐ����W�J���ꂸ OS ������ɓ��삵�Ȃ��Ȃ�̂Œ��ӂ��K�v�ł�.

					if (strPath.length() != 0)
						status = ::RegSetValueEx(hkEnvironment, _T("PATH"), 0, REG_EXPAND_SZ, (BYTE*)strPath.c_str(), (strPath.length() + 1) * sizeof(_TCHAR));
				}

				if (szPath != NULL)
					free(szPath);
				szPath = NULL;
			}

			// -- Close
			status = ::RegCloseKey( hkEnvironment );
		}

		// XIE ���ϐ�.
		{
			DelEnvironmentSZ( EnvKeyTOP );
			DelEnvironmentSZ( EnvKeyBIN );
			DelEnvironmentSZ( EnvKeyASM );

			FlushEnvironment();
		}
	}
#endif

//LExit:
	//er = SUCCEEDED(hr) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
	er = ERROR_SUCCESS;
	return WcaFinalize(er);
}

// ============================================================
/*
	@brief	�����񕪊�
*/
std::vector<_TSTRING> Split(const _TSTRING& str, const _TSTRING& delim)
{
	std::vector<_TSTRING>	items;
	std::size_t				dlm_idx;
	_TSTRING				tmp = str;

	if(tmp.npos == (dlm_idx = tmp.find_first_of(delim)))
	{
		items.push_back(tmp.substr(0, dlm_idx));
	}
	while(tmp.npos != (dlm_idx = tmp.find_first_of(delim)))
	{
		if(tmp.npos == tmp.find_first_not_of(delim)) break;
		items.push_back(tmp.substr(0, dlm_idx));
		dlm_idx++;
		tmp = tmp.erase(0, dlm_idx);
		if(tmp.npos == tmp.find_first_of(delim) && _T("") != tmp)
		{
			items.push_back(tmp);
			break;
		}
	}

	return items;
}

// ============================================================
/*
	@brief	Windows �o�[�W�����`�F�b�N

	http://msdn.microsoft.com/en-us/library/windows/desktop/ms724833(v=vs.85).aspx \n
*/
BOOL IsWindowsVersionOK(DWORD dwMajor, DWORD dwMinor, WORD dwSPMajor)
{
	DWORDLONG dwlConditionMask = 0;
	OSVERSIONINFOEX osvi;
	ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));

	osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
	osvi.dwMajorVersion = dwMajor;
	osvi.dwMinorVersion = dwMinor;
	osvi.wServicePackMajor = dwSPMajor;// Set up the condition mask.

	VER_SET_CONDITION(dwlConditionMask, VER_MAJORVERSION, VER_GREATER_EQUAL);
	VER_SET_CONDITION(dwlConditionMask, VER_MINORVERSION, VER_GREATER_EQUAL);
	VER_SET_CONDITION(dwlConditionMask, VER_SERVICEPACKMAJOR, VER_GREATER_EQUAL);

	// Perform the test.
	return VerifyVersionInfo(&osvi, VER_MAJORVERSION
									| VER_MINORVERSION
									| VER_SERVICEPACKMAJOR,
									dwlConditionMask);
}

// =================================================================
/*
	@brief	���ϐ��̎擾 (REG_SZ)

	@param[in]	key		�L�[
*/
_TSTRING GetEnvironmentSZ(LPCTSTR key)
{
	_TSTRING	result;
	HKEY		hKey = NULL;
	_TCHAR		szValue[_BUFFER_SIZE_];
	DWORD		dwType = 0;
	DWORD		dwDataLength = sizeof(szValue);
	REGSAM		desired = KEY_READ;

	if (IsWindowsVersionOK(5, 1, 0))
		desired |= KEY_WOW64_32KEY;

	if (ERROR_SUCCESS == ::RegOpenKeyEx(HKEY_LOCAL_MACHINE, EnvironmentKey, 0, desired, &hKey))
	{
		if (ERROR_SUCCESS == ::RegQueryValueEx( hKey, key, NULL, &dwType, (LPBYTE)&szValue, &dwDataLength ))
			result = szValue;
		::RegCloseKey( hKey );	hKey = NULL;
	}
	return result;
}

// =================================================================
/*
	@brief	���ϐ��̐ݒ� (REG_SZ)

	@param[in]	key		�L�[
	@param[in]	value	�ݒ�l
*/
void SetEnvironmentSZ(LPCTSTR key, LPCTSTR value)
{
	INT			status = 0;
	DWORD		lastError = 0;
	HKEY		hKey = NULL;
	REGSAM		desired = KEY_WRITE|KEY_QUERY_VALUE;

	if (IsWindowsVersionOK(5, 1, 0))
		desired |= KEY_WOW64_32KEY;

	status = ::RegCreateKeyEx(HKEY_LOCAL_MACHINE, EnvironmentKey, 0, REG_NONE, REG_OPTION_NON_VOLATILE, desired, NULL, &hKey, NULL);
	if (ERROR_SUCCESS == status)
	{
		status = ::RegSetValueEx(hKey, key, 0, REG_SZ, (const BYTE*)value, (_tcslen(value) + 1) * sizeof(_TCHAR));
		if (status != ERROR_SUCCESS)
			lastError = ::GetLastError();
		::RegCloseKey( hKey );	hKey = NULL;
	}
}

// =================================================================
/*
	@brief	���ϐ��̍폜 (REG_SZ)

	@param[in]	key		�L�[
*/
void DelEnvironmentSZ(LPCTSTR key)
{
	INT			status = 0;
	DWORD		lastError = 0;
	HKEY		hKey = NULL;
	REGSAM		desired = KEY_WRITE|KEY_QUERY_VALUE;

	if (IsWindowsVersionOK(5, 1, 0))
		desired |= KEY_WOW64_32KEY;

	status = ::RegCreateKeyEx(HKEY_LOCAL_MACHINE, EnvironmentKey, 0, REG_NONE, REG_OPTION_NON_VOLATILE, desired, NULL, &hKey, NULL);
	if (ERROR_SUCCESS == status)
	{
		status = ::RegDeleteValue(hKey, key);
		if (status != ERROR_SUCCESS)
			lastError = ::GetLastError();
		::RegCloseKey( hKey );	hKey = NULL;
	}
}

// =================================================================
/*
	@brief	���ϐ��̔��f
*/
void FlushEnvironment()
{
	// -- Flush
	// ���t�@�����X.
	//		http://msdn.microsoft.com/ja-jp/library/cc411010.aspx
	// ���ϐ����V�X�e���ɒʒm������@.
	//		http://support.microsoft.com/kb/104011/ja?wa=wsignin1.0
	DWORD result = 0;
	int status = ::SendMessageTimeoutA(HWND_BROADCAST, WM_SETTINGCHANGE, 0, (LPARAM)"Environment", SMTO_ABORTIFHUNG, 5000, &result );
}
