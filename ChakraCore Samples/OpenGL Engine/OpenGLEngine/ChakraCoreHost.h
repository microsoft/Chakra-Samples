#pragma once
#include "Task.h"
#include "Canvas.h"
#include "ChakraCore.h"
#include <queue>

using namespace std;

// a JavaScript host powered by ChakraCore
class ChakraCoreHost
{
private:
	JsRuntimeHandle runtime;
	unsigned currentSourceContext;
public:
	queue<Task*> taskQueue;
	Canvas canvas;
	ChakraCoreHost();
	wstring runScript(wstring script);					// run a script
	wstring loadScript(wstring fileName);				// load a script from file
	~ChakraCoreHost();
};

// a utility class containing bindings to native methods
class Binding
{
public:
	static ChakraCoreHost* host;
	static void addNativeBindings();
private:
	static JsValueRef JSPointPrototype;
	static JsValueRef JSLinePrototype;
	static JsValueRef JSTrianglePrototype;
	static JsValueRef JSQuadPrototype;
	static JsValueRef JSPolygonPrototype;
	static JsValueRef mouseCallbackFunc;
	static JsValueRef mouseCallbackThisArg;
	static void setCallback(JsValueRef object, const wchar_t *propertyName, JsNativeFunction callback, void *callbackState);
	static void setProperty(JsValueRef object, const wchar_t *propertyName, JsValueRef property);
	static JsValueRef getProperty(JsValueRef object, const wchar_t *propertyName);
	static JsValueRef CALLBACK JSLog(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSSetTimeout(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSSetInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static GLPoint* JSPointToNativePoint(JsValueRef point);
	static JsValueRef CALLBACK JSPointConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef createPolygon(JsValueRef *arguments, unsigned short argumentCount);
	static JsValueRef CALLBACK JSLineConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSTriangleConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSQuadConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSPolygonConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSRotate(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSSetColor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSSetPosition(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSAddShape(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSRemoveShape(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static JsValueRef CALLBACK JSRender(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static void mouse_click_callback(GLFWwindow* window, int button, int action, int mods);
	static JsValueRef CALLBACK JSSetMouseClickCallback(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	static void projectNativeClass(const wchar_t *className, JsNativeFunction constructor, JsValueRef &prototype, vector<const wchar_t *> memberNames, vector<JsNativeFunction> memberFuncs);
};
