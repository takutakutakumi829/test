// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "pch.h"
#include <stdio.h>
#include <sstream>
#include <fstream>
#include <iostream>
#include <cmath>
#include <string.h>


BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	return true;
}

//文字列変換
DLLEXPORT std::wstring __stdcall ChangeToWString(std::string str)
{
	std::wstring wstr;
	auto wchar = MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, nullptr, 0);
	wstr.resize(wchar);
	auto wstring = MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, &wstr[0], wstr.size());
	return wstr;
}

DLLEXPORT std::string __stdcall ChangeToString(char* name)
{
	std::string str = { name };
	return str;
}

//stringのchar*型変換
DLLEXPORT char* ChangeToChar(std::string str_name)
{
	char* char_name = new char[str_name.size() + 1];
	strcpy_s(char_name, str_name.size() + 1, str_name.c_str());
	return char_name;
}


//拡張子の探索
DLLEXPORT char* __stdcall GetExtension(char* name)
{
	std::string path = name;
	int idx = path.find_last_of('.');
	auto str = path.substr(idx + 1, path.length() - idx);
	char* value = new char[str.size() + 1];
	strcpy_s(value, str.size() + 1, str.c_str());
	return value;
}

//拡張子外の探索
DLLEXPORT char* __stdcall GetOutExtension(char* name)
{
	std::string path = name;
	int idx = path.find_last_of('.');
	auto str = path.substr(0, idx + 1);
	char* value = new char[str.size() + 1];
	strcpy_s(value, str.size() + 1, str.c_str());
	return value;
}

//ファイル名の探索
DLLEXPORT char* __stdcall GetExtensionFileName(char* name)
{
	std::string path = name;
	int idx = path.find_last_of('.');
	int fileidx = path.find_last_of('/');
	auto str = path.substr(fileidx + 1, idx + 1);
	char* value = new char[str.size() + 1];
	strcpy_s(value, str.size() + 1, str.c_str());
	return value;
}

//拡張子の変更
DLLEXPORT char* __stdcall ChangeExtension(char* name, char* exten)
{
	std::string outPath = GetOutExtension(name);
	std::string inPath = exten;
	std::string constValue = outPath + inPath;

	char* value = new char[constValue.size() + 1];
	strcpy_s(value, constValue.size() + 1, constValue.c_str());
	return value;
}

