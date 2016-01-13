#pragma once
#include "pch.h"
#include "ChakraHost.h"

using namespace std;

// ES6 Promise callback
void CALLBACK PromiseContinuationCallback(JsValueRef task, void *callbackState)
{
	// Save promise task in taskQueue.
	queue<JsValueRef> * q = (queue<JsValueRef> *)callbackState;
	q->push(task);
}

// Initilize host
wstring _cdecl ChakraHost::init()
{
	try
	{
		currentSourceContext = 0;
		JsRuntimeHandle runtime;
		JsContextRef context;

		// Create the runtime. We're only going to use one runtime for this host.
		if (JsCreateRuntime(JsRuntimeAttributeNone, nullptr, &runtime) != JsNoError)
			return L"failed to create runtime.";

		// Create a single execution context. 
		if (JsCreateContext(runtime, &context) != JsNoError)
			return L"failed to create execution context.";

		// Now set the execution context as being the current one on this thread.
		if (JsSetCurrentContext(context) != JsNoError)
			return L"failed to set current context.";

		// Set up ES6 Promise 
		if (JsSetPromiseContinuationCallback(PromiseContinuationCallback, &taskQueue) != JsNoError)
			return L"failed to set PromiseContinuationCallback.";

		// UWP namespace projection; all UWP under Windows namespace should work.
		if (JsProjectWinRTNamespace(L"Windows") != JsNoError)
			return L"failed to project windows namespace.";

		// Put Chakra in debug mode.
		if (JsStartDebugging() != JsNoError)
			return L"failed to start debugging.";

		return L"NoError";
	}
	catch (...)
	{
		return L"chakrahost: fatal error: internal error.\n";
	}
}

wstring _cdecl ChakraHost::runScript(wstring script)
{
	try
	{
		JsValueRef result;
		JsValueRef promiseResult;

		// Run the script.
		if (JsRunScript(script.c_str(), currentSourceContext++, L"", &result) != JsNoError)
		{
			// Get error message
			JsValueRef exception;
			if (JsGetAndClearException(&exception) != JsNoError)
				return L"failed to get and clear exception";

			JsPropertyIdRef messageName;
			if (JsGetPropertyIdFromName(L"message", &messageName) != JsNoError)
				return L"failed to get error message id";

			JsValueRef messageValue;
			if (JsGetProperty(exception, messageName, &messageValue))
				return L"failed to get error message";

			const wchar_t *message;
			size_t length;
			if (JsStringToPointer(messageValue, &message, &length) != JsNoError)
				return L"failed to convert error message";

			return message;
		}

		// Execute promise tasks stored in taskQueue
		while (!taskQueue.empty()) {
			JsValueRef task = taskQueue.front();
			taskQueue.pop();
			JsValueRef global;
			JsGetGlobalObject(&global);
			JsCallFunction(task, &global, 1, &result);
		}

		// Convert the return value to wstring.
		JsValueRef stringResult;
		const wchar_t *returnValue;
		size_t stringLength;
		if (JsConvertValueToString(result, &stringResult) != JsNoError)
			return L"failed to convert value to string.";
		if (JsStringToPointer(stringResult, &returnValue, &stringLength) != JsNoError)
			return L"failed to convert return value.";
		return returnValue;
	}
	catch (...)
	{
		return L"chakrahost: fatal error: internal error.\n";
	}
}