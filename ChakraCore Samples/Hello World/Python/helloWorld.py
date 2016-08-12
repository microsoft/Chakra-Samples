# -------------------------------------------------------------------------------------------------------
# Copyright (C) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
# -------------------------------------------------------------------------------------------------------

import sys
import os.path
from ctypes import *

def usage():
    print("usage:\npython helloWorld.py [path to ChakraCore.dll or libChakraCore.so]");
    sys.exit(0)

if len(sys.argv) < 2:
    usage()

if os.path.isfile(sys.argv[1]):
    chakraCore = CDLL(sys.argv[1])
else:
    print("\nFile not found at " + sys.argv[1] + "\n\n")
    usage()

if sys.platform != 'win32':
    # You are expected to call DllMain manually on non-Windows
    # Attach process
    chakraCore.DllMain(0, 1, 0);
    # Attach main thread
    chakraCore.DllMain(0, 2, 0);

script = create_string_buffer("(()=>{return \'Hello world!\';})()");

runtime = c_void_p()
# Create Javascript Runtime. 
chakraCore.JsCreateRuntime(0, 0, byref(runtime));

context = c_void_p()
# Create an execution context. 
chakraCore.JsCreateContext(runtime, byref(context));

# Now set the current execution context.
chakraCore.JsSetCurrentContext(context);

jsResult=c_void_p();
# Run the script.
chakraCore.JsRunScriptUtf8(script, 0, "", byref(jsResult));

# Convert your script result to String in JavaScript; redundant if your script returns a String
resultJSString = c_void_p()
chakraCore.JsConvertValueToString(jsResult, byref(resultJSString));

# Project script result back to Python.
resultSTR=c_char_p();
stringLength = c_size_t();
chakraCore.JsStringToPointerUtf8Copy(resultJSString, byref(resultSTR), byref(stringLength));

print("Result from ChakraCore: ", resultSTR.value);

chakraCore.JsStringFree(resultSTR);

# Dispose runtime
chakraCore.JsDisposeRuntime(runtime);
