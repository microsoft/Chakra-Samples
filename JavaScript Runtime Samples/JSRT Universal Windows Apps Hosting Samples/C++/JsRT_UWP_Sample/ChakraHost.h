#pragma once
#include "pch.h"
#define USE_EDGEMODE_JSRT
#include <jsrt.h>
#include <string>

using namespace std;

class ChakraHost
{
	private:
		unsigned currentSourceContext;
		JsValueRef promiseCallback;

	public:
		wstring _cdecl init();
		wstring _cdecl runScript(wstring script);
};