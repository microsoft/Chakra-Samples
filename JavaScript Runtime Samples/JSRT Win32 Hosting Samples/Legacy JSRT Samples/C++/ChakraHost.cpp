#include "stdafx.h"
#include <string>

using namespace std;

//
// Class to store information about command-line arguments to the host.
//

class CommandLineArguments
{
public:
	bool debug;
	bool profile;
	int argumentsStart;

	CommandLineArguments() :
		debug(false),
		profile(false),
		argumentsStart(1)
	{
	}
};

//
// Source context counter.
//

unsigned currentSourceContext = 0;

//
// Process the host command-line arguments.
//

void ProcessArguments(int argc, __in_ecount(argc) wchar_t *argv [], CommandLineArguments &arguments)
{
	wstring debugFlag = L"debug";
	wstring profileFlag = L"profile";
	int current = 1;

	for (; current < argc; current++)
	{
		wstring argument = argv[current];

		if (argument.length() > 0 && 
			(argument[0] == '/' || argument[0] == '-'))
		{
			wstring argumentFlag = argument.substr(1);

			if (_wcsnicmp(argumentFlag.c_str(), debugFlag.c_str(), debugFlag.length()) == 0)
			{
				arguments.debug = true;
			}
			else if (_wcsnicmp(argumentFlag.c_str(), profileFlag.c_str(), profileFlag.length()) == 0)
			{
				arguments.profile = true;
			}
			else
			{
				break;
			}
		}
		else
		{
			break;
		}
	}

	arguments.argumentsStart = current;
	return;
}

//
// This "throws" an exception in the Chakra space. Useful routine for callbacks
// that need to throw a JS error to indicate failure.
//

void ThrowException(wstring errorString)
{
	// We ignore error since we're already in an error state.
	JsValueRef errorValue;
	JsValueRef errorObject;
	JsPointerToString(errorString.c_str(), errorString.length(), &errorValue);
	JsCreateError(errorValue, &errorObject);
	JsSetException(errorObject);
}

//
// Helper to load a script from disk.
//

wstring LoadScript(wstring fileName)
{
	FILE *file;
	if (_wfopen_s(&file, fileName.c_str(), L"rb"))
	{
		fwprintf(stderr, L"chakrahost: unable to open file: %s.\n", fileName.c_str());
		return wstring();
	}

	unsigned int current = ftell(file);
	fseek(file, 0, SEEK_END);
	unsigned int end = ftell(file);
	fseek(file, current, SEEK_SET);
	unsigned int lengthBytes = end - current;
	char *rawBytes = (char *) calloc(lengthBytes + 1, sizeof(char) );

	if (rawBytes == nullptr)
	{
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	fread((void *) rawBytes, sizeof(char) , lengthBytes, file);

	wchar_t *contents = (wchar_t *) calloc(lengthBytes + 1, sizeof(wchar_t) );
	if (contents == nullptr)
	{
		free(rawBytes);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	if (MultiByteToWideChar(CP_UTF8, 0, rawBytes, lengthBytes + 1, contents, lengthBytes + 1) == 0)
	{
        free(rawBytes);
        free(contents);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	wstring result = contents;
    free(rawBytes);
    free(contents);
	return result;
}

//
// Callback to echo something to the command-line.
//

JsValueRef CALLBACK Echo(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	for (unsigned int index = 1; index < argumentCount; index++)
	{
		if (index > 1)
		{
			wprintf(L" ");
		}

		JsValueRef stringValue;
		IfFailThrow(JsConvertValueToString(arguments[index], &stringValue), L"invalid argument");

		const wchar_t *string;
		size_t length;
		IfFailThrow(JsStringToPointer(stringValue, &string, &length), L"invalid argument");

		wprintf(L"%s", string);
	}

	wprintf(L"\n");

	return JS_INVALID_REFERENCE;
}

//
// Method to start up debugging in the current context.
//

void StartDebugging()
{
	HRESULT hr = S_OK;
	IClassFactory *classFactory = nullptr;
	IProcessDebugManager *pdm = nullptr;
	IDebugApplication *debugApplication = nullptr;

	//
	// Initialize COM because we're going to have to talk to some COM interfaces.
	//

	IfComFailError(CoInitializeEx(nullptr, COINIT_APARTMENTTHREADED));

	//
	// Get a pointer to the process debug manager (PDM).
	//

	IfComFailError(CoGetClassObject(__uuidof(ProcessDebugManager), CLSCTX_INPROC_SERVER, NULL, __uuidof(IClassFactory), (LPVOID *)&classFactory));
	IfComFailError(classFactory->CreateInstance(0, _uuidof(IProcessDebugManager), (LPVOID *)&pdm));

	//
	// Get the default application.
	//

	IfComFailError(pdm->GetDefaultApplication(&debugApplication));

	//
	// Now start debugging.
	//

	if (JsStartDebugging(debugApplication) != JsNoError)
	{
		hr = E_FAIL;
	}

error:
	if (debugApplication)
	{
		debugApplication->Release();
	}

	if (pdm)
	{
		pdm->Release();
	}
	
	if (classFactory)
	{
		classFactory->Release();
	}

	if (FAILED(hr))
	{
		fwprintf(stderr, L"chakrahost: couldn't start debugging.\n");
	}
}

//
// Callback to load a script and run it.
//

JsValueRef CALLBACK RunScript(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	JsValueRef result = JS_INVALID_REFERENCE;

	if (argumentCount < 2)
	{
		ThrowException(L"not enough arguments");
		return result;
	}

	//
	// Convert filename.
	//
	const wchar_t *filename;
	size_t length;

	IfFailThrow(JsStringToPointer(arguments[1], &filename, &length), L"invalid filename argument");

	//
	// Load the script from the disk.
	//

	wstring script = LoadScript(filename);
	if (script.empty())
	{
		ThrowException(L"invalid script");
		return result;
	}

	//
	// Run the script.
	//

	IfFailThrow(JsRunScript(script.c_str(), currentSourceContext++, filename, &result), L"failed to run script.");

	return result;
}

//
// Helper to define a host callback method on the global host object.
//

JsErrorCode DefineHostCallback(JsValueRef globalObject, const wchar_t *callbackName, JsNativeFunction callback, void *callbackState)
{
	//
	// Get property ID.
	//

	JsPropertyIdRef propertyId;
	IfFailRet(JsGetPropertyIdFromName(callbackName, &propertyId));

	//
	// Create a function
	//

	JsValueRef function;
	IfFailRet(JsCreateFunction(callback, callbackState, &function));

	//
	// Set the property
	//

	IfFailRet(JsSetProperty(globalObject, propertyId, function, true));

	return JsNoError;
}

//
// Creates a host execution context and sets up the host object in it.
//

JsErrorCode CreateHostContext(JsRuntimeHandle runtime, int argc, wchar_t *argv [], int argumentsStart, JsContextRef *context)
{
	//
	// Create the context. Note that if we had wanted to start debugging from the very
	// beginning, we would have passed in an IDebugApplication pointer here.
	//

	IfFailRet(JsCreateContext(runtime, nullptr, context));

	//
	// Now set the execution context as being the current one on this thread.
	//

	IfFailRet(JsSetCurrentContext(*context));

	//
	// Create the host object the script will use.
	//

	JsValueRef hostObject;
	IfFailRet(JsCreateObject(&hostObject));

	//
	// Get the global object
	//

	JsValueRef globalObject;
	IfFailRet(JsGetGlobalObject(&globalObject));

	//
	// Get the name of the property ("host") that we're going to set on the global object.
	//

	JsPropertyIdRef hostPropertyId;
	IfFailRet(JsGetPropertyIdFromName(L"host", &hostPropertyId));

	//
	// Set the property.
	//

	IfFailRet(JsSetProperty(globalObject, hostPropertyId, hostObject, true));

	//
	// Now create the host callbacks that we're going to expose to the script.
	//

	IfFailRet(DefineHostCallback(hostObject, L"echo", Echo, nullptr));
    IfFailRet(DefineHostCallback(hostObject, L"runScript", RunScript, nullptr));

	//
	// Create an array for arguments.
	//

	JsValueRef arguments;
	IfFailRet(JsCreateArray(argc - argumentsStart, &arguments));

	for (int index = argumentsStart; index < argc; index++)
	{
		//
		// Create the argument value.
		//

		JsValueRef argument;
		IfFailRet(JsPointerToString(argv[index], wcslen(argv[index]), &argument));

		//
		// Create the index.
		//

		JsValueRef indexValue;
		IfFailRet(JsIntToNumber(index - argumentsStart, &indexValue));

		//
		// Set the value.
		//

		IfFailRet(JsSetIndexedProperty(arguments, indexValue, argument));
	}

	//
	// Get the name of the property that we're going to set on the host object.
	//

	JsPropertyIdRef argumentsPropertyId;
	IfFailRet(JsGetPropertyIdFromName(L"arguments", &argumentsPropertyId));

	//
	// Set the arguments property.
	//

	IfFailRet(JsSetProperty(hostObject, argumentsPropertyId, arguments, true));

	//
	// Clean up the current execution context.
	//

	IfFailRet(JsSetCurrentContext(JS_INVALID_REFERENCE));

	return JsNoError;
}

//
// Print out a script exception.
//

JsErrorCode PrintScriptException()
{
	//
	// Get script exception.
	//

	JsValueRef exception;
	IfFailRet(JsGetAndClearException(&exception));

	//
	// Get message.
	//

	JsPropertyIdRef messageName;
	IfFailRet(JsGetPropertyIdFromName(L"message", &messageName));

	JsValueRef messageValue;
	IfFailRet(JsGetProperty(exception, messageName, &messageValue));

	const wchar_t *message;
	size_t length;
	IfFailRet(JsStringToPointer(messageValue, &message, &length));

	fwprintf(stderr, L"chakrahost: exception: %s\n", message);

	return JsNoError;
}

//
// The main entry point for the host.
//

int _cdecl wmain(int argc, wchar_t *argv[])
{
	int returnValue = EXIT_FAILURE;
	CommandLineArguments arguments;

	ProcessArguments(argc, argv, arguments);

	if (argc - arguments.argumentsStart < 1)
	{
		fwprintf(stderr, L"usage: chakrahost [-debug] [-profile] <script name> <arguments>\n");
		return returnValue;
	}

	try
	{
		JsRuntimeHandle runtime;
		JsContextRef context;

		//
		// Create the runtime. We're only going to use one runtime for this host.
		//

		IfFailError(JsCreateRuntime(JsRuntimeAttributeNone, JsRuntimeVersion11, nullptr, &runtime), L"failed to create runtime.");

		//
		// Similarly, create a single execution context. Note that we're putting it on the stack here,
		// so it will stay alive through the entire run.
		//

		IfFailError(CreateHostContext(runtime, argc, argv, arguments.argumentsStart, &context), L"failed to create execution context.");

		//
		// Now set the execution context as being the current one on this thread.
		//

		IfFailError(JsSetCurrentContext(context), L"failed to set current context.");

		//
		// Start debugging if requested.
		//

		if (arguments.debug)
		{
			StartDebugging();
		}

		//
		// Start profiling if requested.
		//

		if (arguments.profile)
		{
			Profiler *profiler = new Profiler();
			IActiveScriptProfilerCallback *callback;

			profiler->QueryInterface(IID_IActiveScriptProfilerCallback, (void **)&callback);
			profiler->Release();

			JsStartProfiling(callback, PROFILER_EVENT_MASK_TRACE_ALL, 0);
			callback->Release();
		}

		//
		// Load the script from the disk.
		//

		wstring script = LoadScript(argv[arguments.argumentsStart]);
		if (script.empty())
		{
			goto error;
		}

		//
		// Run the script.
		//

		JsValueRef result;
		JsErrorCode errorCode = JsRunScript(script.c_str(), currentSourceContext++, argv[arguments.argumentsStart], &result);
		
		if (errorCode == JsErrorScriptException)
		{
			IfFailError(PrintScriptException(), L"failed to print exception");
			return EXIT_FAILURE;
		}
		else
		{
			IfFailError(errorCode, L"failed to run script.");
		}

		//
		// Convert the return value.
		//

		JsValueRef numberResult;
		double doubleResult;
		IfFailError(JsConvertValueToNumber(result, &numberResult), L"failed to convert return value.");
		IfFailError(JsNumberToDouble(numberResult, &doubleResult), L"failed to convert return value.");
		returnValue = (int) doubleResult;

		//
		// Stop profiling.
		//

		if (arguments.profile)
		{
			JsStopProfiling(0);
		}

		//
		// Clean up the current execution context.
		//

		IfFailError(JsSetCurrentContext(JS_INVALID_REFERENCE), L"failed to cleanup current context.");

		//
		// Clean up the runtime.
		//

		IfFailError(JsDisposeRuntime(runtime), L"failed to cleanup runtime.");
	}
	catch (...)
	{
		fwprintf(stderr, L"chakrahost: fatal error: internal error.\n");
	}

error:
	return returnValue;
}
