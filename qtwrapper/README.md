# Qt Wrapper Sample (for Shared Library)
This sample shows how to wrap Qt's objects in order to embed them in ChakraCore.
It shows how to retrieve and set values from C++ world.

1. Initializing the JavaScript runtime and creating an execution context.
2. Running scripts and handling values.
3. Printing out script results.

## Requirements
This code was tested with Qt 5.10 and ChakraCore 1.9 (not the stable version, the master, commit 3f7691187ba02ab2a1079619fed867714786b693, at Feb 10 14:50:00 2018 -0800), under Linux.

This code should run with "any" Qt version as there is no fancy things.


## Build and run the sample
1. [Clone](https://github.com/Microsoft/ChakraCore) and [build](https://github.com/Microsoft/ChakraCore/wiki/Building-ChakraCore) ChakraCore on designated platform.
From here we will assume that ChakraCore was cloned in /home/xxx/ChakraCore, and built a shared library (the default build setting). If you cloned elsewhere or are under Windows, you have to adapt accordingly the qtwrapper.pro file.
2. then get in the qtwrapper directory, and `qmake && make` and it is done:
`LD_LIBRARY_PATH='/home/xxx/ChakraCore/out/Release' ./wrappersample`

## side note
this Qt's wrapper allows:
- to access from ChakraCore to Qt's Object
- set variables from C++
- get variables from ChakraCore
      
At least even though you're not interested by Qt, you will have examples for:
- access to variables from ChakraCore: see JSEngine::registerValue(const QString&, const QVariant&),  toJsValueRef; both are in qjsengine.cpp
- register attribute's object and be able to get notified when they are changed; see:    
   - JSEngine::JSEngine, that register the handler that will be called by the futur object's proxy
   - JSEngine::registerValue(QObject*, const QString&) that:
      - register one getter and one setter (see the set and get functions in the same file)
      - set the proxy by associating the handler and the object
