# ChakraCore OpenGL Engine

This sample is a mock OpenGL engine implemented in C++. The engine supports developing apps with JavaScript by embedding [ChakraCore](https://github.com/Microsoft/ChakraCore) and exposes custom JavaScript APIs to create OpenGL entities, change colors and other properties, and draw to the screen. Some notable tasks of this sample include, 

1. Create a JavaScript host.
2. Handle message queue and event loop in the JavaScript host.
3. Expose native functionalities as JavaScript APIs to the host.
4. Implement a simplified version of some commonly used functions in browsers - console.log, setTimeout, setInterval.

To build an app with [custom APIs](CustomAPI.md), add your script in a `app.js` file in the project output folder along with the engine executable `OpenGLEngine.exe`. Running the executable will execute the code in app.js. See a [sample script](OpenGLEngine/app.js) which creates a bouncing ball with each mouse click.

This sample includes the [glew](http://glew.sourceforge.net/) and [glfw](http://www.glfw.org/) library.

## Requirements
This sample requires Windows 7 SP1 or above with either Visual Studio 2013 or 2015 with C++ support installed. 

## Build the sample
1. Clone and build [ChakraCore](https://github.com/Microsoft/ChakraCore). You will need to grab,
	* **ChakraCore.h** and **chakracommon.h** from `lib\jsrt\`, which are the headers. 
	* **ChakraCore.lib** and **ChakraCore.dll** from `Build\VcBuild\bin\[platform+output]\`.
2. Copy the headers to `OpenGLEngine\dep\ChakraCore\include\`.
3. Copy **ChakraCore.lib** to `OpenGLEngine\dep\ChakraCore\[platform]\`.
4. Copy the following DLLs to the project's output directory,
	* **ChakraCore.dll**,
	* **glew32.dll** from `OpenGLEngine\dep\glew-1.13.0\bin\Release\[platform]\`,
	* **glfw3.dll** from `OpenGLEngine\dep\glfw-3.1.2\glfw-3.1.2.bin.[platform]\[lib-vc2013|lib-vc2015]\`.
5. Open **OpenGLEngine.sln** with Visual Studio.
6. Build the sample by pressing **F6** or using **Build > Build Solution**.

## Run the sample
1. Run the sample by pressing **Ctrl+F5** or using **Debug > Start Without Debugging**, or copy `app.js` to the project's output directory and open `OpenGLEngine.exe`.

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).

Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license. See LICENSE file in the project root for full license information.
