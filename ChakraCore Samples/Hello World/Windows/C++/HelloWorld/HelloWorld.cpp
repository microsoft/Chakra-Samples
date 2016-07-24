#define _JSRT_
#include "ChakraCore.h"
#include <string>
#include <iostream>

using namespace std;

int main()
{
	JsRuntimeHandle runtime;
	JsContextRef context;
	JsValueRef result;
	unsigned currentSourceContext = 0;

	// Your script, try replace the basic hello world with something else
	wstring script = L"(()=>{return \'Hello world!\';})()";

	// Create a runtime. 
	JsCreateRuntime(JsRuntimeAttributeNone, nullptr, &runtime);

	// Create an execution context. 
	JsCreateContext(runtime, &context);

	// Now set the execution context as being the current one on this thread.
	JsSetCurrentContext(context);

	// Run the script.
	JsRunScript(script.c_str(), currentSourceContext++, L"", &result);

	// Convert your script result to String in JavaScript; redundant if your script returns a String
	JsValueRef resultJSString;
	JsConvertValueToString(result, &resultJSString);

	// Project script result in JS back to C++.
	const wchar_t *resultWC;
	size_t stringLength;
	JsStringToPointer(resultJSString, &resultWC, &stringLength);

	wstring resultW(resultWC);
	cout << string(resultW.begin(), resultW.end()) << endl;
	system("pause");

	// Dispose runtime
	JsSetCurrentContext(JS_INVALID_REFERENCE);
	JsDisposeRuntime(runtime);

	return 0;
}