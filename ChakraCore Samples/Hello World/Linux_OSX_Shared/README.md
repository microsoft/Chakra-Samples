# ChakraCore Hello-world Sample (for Shared Library)
This sample shows how to embed ChakraCore with [JavaScript Runtime (JSRT) APIs](http://aka.ms/corejsrtref) on Linux/OS X, and complete the following tasks:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Printing out script results.

## Requirements
This sample requires Ubuntu 16.04 LTS or OS X 10.9+.

## Build and run the sample
1. [Clone](https://github.com/Microsoft/ChakraCore) and [build](https://github.com/Microsoft/ChakraCore/wiki/Building-ChakraCore) ChakraCore on designated platform.
Do not use `--static` argument while building ChakraCore. You may build ChakraCore
using `--debug`, `--test-build` or release (no argument is needed). This sample,
consider using `--debug` to match the steps below.

2. Copy the files of this sample into `ChakraCore/CrossPlatform` folder.  
3. `cd` into `ChakraCore/CrossPlatform` folder, and build the sample with `make BUILD_TYPE=Debug` (for macOS add `PLATFORM=darwin`).
4. On MacOS, you should copy `libChakraCore.dylib` into `/usr/local/lib`
5. Run with `./sample.o`.

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).

Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license. See LICENSE file in the project root for full license information.
