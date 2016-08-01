# ChakraCore Hello-world Python Sample
This sample shows how to embed ChakraCore with [JavaScript Runtime (JSRT) APIs](http://aka.ms/corejsrtref),
and complete the following tasks:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Printing out script results.

## Requirements
 - Python 2.7 (or something compatible) is installed.
 - Linux / Windows

## Build and run the sample
1. Follow the [instructions](https://github.com/Microsoft/ChakraCore/wiki/Building-ChakraCore) to build ChakraCore
2. Run the sample by `python helloWorld.py [path to ChakraCore.dll or libChakraCore.so]`

## Remarks
 - Make sure the target ChakraCore.dll or .so file shares the same cpu-arc with the hosting Python.

 - While working with Python's `CDLL/ctypes` keep in mind the [ChakraCore memory management](https://github.com/Microsoft/ChakraCore/wiki/JavaScript-Runtime-%28JSRT%29-Overview#memory-management).
  You may not hold reference to a JavaScript value unless it is explicitly referenced
  on the runtime's heap or stack, or preserved through [JsAddRef](https://github.com/Microsoft/ChakraCore/wiki/JsAddRef). 

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).

Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license.
See LICENSE file in the project root for full license information.
