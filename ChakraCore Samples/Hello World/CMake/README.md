# ChakraCore CMake Sample
This sample shows how to embed ChakraCore with
[JavaScript Runtime (JSRT) APIs](http://aka.ms/corejsrtref), and complete the following tasks:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Printing out script results.

## Build ChakraCore
[Here] (https://github.com/Microsoft/ChakraCore/wiki/Building-ChakraCore) you will
find the details on `How to build ChakraCore for your platform.`

## Build and Run the Sample

### Build

#### Linux/OSX
```
> cmake . -DCHAKRACORE_PATH=<ChakraCore Path>
> make
```

#### Windows
```
> cmake . -DCHAKRACORE_PATH=<ChakraCore Path> -G "Visual Studio 14 2015 Win64"
> msbuild /p:Platform=x64 HelloWorld.sln
```

`Visual Studio 14` is not a must! You may use another version (i.e. 2017 etc.)

As a last step, copy ChakraCore.dll (you will find it under the ChakraCore build
path) into `Debug/` folder

### Run

#### Linux/OSX
```
> ./Sample
```

#### Windows
```
> Debug\Sample.exe
```

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).

Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license.  
See LICENSE file in the project root for full license information.
