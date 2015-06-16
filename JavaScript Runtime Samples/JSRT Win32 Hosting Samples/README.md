# JavaScript Runtime Hosting Sample
This sample shows how to host the JavaScript Runtime (JSRT) APIs in a non-IE application. The sample demonstrates the following tasks:

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Creating host objects that scripts can interact with.
4. Creating callback functions to execution actions in the host.
5. Handling exceptions throw in JavaScript.
6. Initializing JavaScript debugging.
7. Handling profile events from the JavaScript engine.

The sample has versions built with [Edge and Legacy JSRT](https://msdn.microsoft.com/en-us/library/dn903710.aspx). The legacy samples can also be downloaded [here](https://code.msdn.microsoft.com/JavaScript-Runtime-Hosting-d3a13880). 

## Operating system requirements
- Client: Windows 8.1
- Server: Windows Server 2012 R2 


## Build the sample
1. Start Visual Studio 2013 and select **File > Open > Project/Solution**. 
2. Go to the directory in which you unzipped the sample. Go to the directory named for the sample, and double-click the Microsoft Visual Studio Solution (.sln) file. 
3. Press F6 or use **Build > Build Solution** to build the sample. 



## Run the sample
To debug the app and then run it, press F5 or use **Debug > Start Debugging**. To run the app without debugging, press Ctrl+F5 or use **Debug > Start Without Debugging**.

###Run the C++ sample with script debugging enabled

To debug a script running in the C++ sample host, choose **Project > Properties**. Under **Configuration Properties**, choose **Debugging**. Change the values of **Debugger Type** to **Script** and choose **OK**. Note that while using script debugging, you cannot debug the host itself. To switch back to native code debugging, change **Debugger Type** to **Auto**.

###Run the C#/VB sample with script debugging enabled

Because the C# and VB project systems do not allow changing the debugger type to the script debugger, to debug a script running in the C# or VB sample hosts it is necessary to open the host executable directly. Having built the project, choose **File > Open > Project/Solution**. Navigate to where the host executable was built (for example, < project directory >\bin\Debug) and choose the host executable (for example, **chakrahost.exe**). Choose **Project >  Properties**. Change **Debugger Type** to **Script** and hit F5.



## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/Chakra-Samples/issues/new).


Copyright (c) Microsoft. All rights reserved.  Licensed under the Apache license. See LICENSE file in the project root for full license information.