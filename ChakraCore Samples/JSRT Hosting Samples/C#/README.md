# ChakraCore Hosting Sample
This sample shows how to embed ChakraCore with [JavaScript Runtime (JSRT) APIs](http://aka.ms/corejsrtref), and complete the following tasks:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Creating host objects that scripts can interact with.
4. Creating callback functions to execution actions in the host.
5. Handling exceptions throw in JavaScript.
6. Printing out script results.

## Requirements
This sample requires Windows 7 SP1 or above with either Visual Studio 2013 or 2015 with C++ support installed. 

## Build and run the sample
1. Clone and build [ChakraCore](https://github.com/Microsoft/ChakraCore). You will need to grab **ChakraCore.dll** from `Build\VcBuild\bin\[platform+output]\`.
2. Open **ChakraCoreHost.sln** with Visual Studio.
3. Copy **ChakraCore.dll** to the sample project's output directory. 
4. Build the sample by pressing  **F6**  or using  **Build > Build Solution**.
5. Run the sample by pressing  **Ctrl+F5**  or using  **Debug > Start Without Debugging**.

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).

Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license. See LICENSE file in the project root for full license information.