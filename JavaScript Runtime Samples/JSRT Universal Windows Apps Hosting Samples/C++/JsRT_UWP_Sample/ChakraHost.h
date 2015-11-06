#pragma once
#include "pch.h"
#define USE_EDGEMODE_JSRT
#include <jsrt.h>
#include <string>
#include <queue>

using namespace std;

class ChakraHost
{
	private:
		unsigned currentSourceContext;
		queue<JsValueRef> taskQueue;

	public:
		wstring _cdecl init();
		wstring _cdecl runScript(wstring script);
};