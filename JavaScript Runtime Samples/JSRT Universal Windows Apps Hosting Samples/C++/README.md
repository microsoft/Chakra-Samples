# JavaScript Runtime Sample
This Universal Windows Application sample is a JavaScript console application built by utilizing [JavaScript Runtime (JSRT) APIs](https://msdn.microsoft.com/en-us/library/dn249673.aspx), a public API for hosting the standards-based Chakra JavaScript engine outside the browser. This sample demonstrates the following tasks in JSRT:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Handling exceptions throw in JavaScript.
4. Initializing JavaScript debugging.
5. Initializing ES6 Promises.
6. Projecting UWP namespace to allow scripts to natively call UWP APIs.


## Requirements
This sample requires [Windows 10](http://www.microsoft.com/en-US/windows/features) and [Visual Studio 2015](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) (with Windows 10 SDK installed). 


## Build the sample
1. Before you start, make sure you have Windows, Windows SDK and Visual Studio specified in the Requirements section above. 
2. Download the sample. 
3. Open JsRT_UWP_Sample.sln with Visual Studio. 
4. Press Ctrl+Shift+B or use Build > Build Solution to build the sample. 


## Run the sample
To debug the app and then run it, press F5 or use Debug >  Start Debugging. To run the app without debugging, press Ctrl+F5 or use  Debug > Start Without Debugging.


## Debug the scripts
Script debugging is enabled in Visual Studio. Put 'debugger;' in scripts to set break points. For example, the sample below set a break point after the first line.  
```
var text = 'Hello World!';
debugger;
text; 
```


## Known Issues
1. If Visual Studio asks you to "Enable developer mode", see instructions [here](https://msdn.microsoft.com/en-us/library/windows/apps/dn706236.aspx#GroupPolicy). 


## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).


Copyright (c) Microsoft. All rights reserved.  Licensed under the MIT license. See LICENSE file in the project root for full license information.