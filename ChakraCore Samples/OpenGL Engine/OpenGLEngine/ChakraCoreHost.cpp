#pragma once
#include "ChakraCoreHost.h"
#include <string>
#include <assert.h>
#include <time.h>

using namespace std;

// ************************************************************
//						 ChakraCoreHost
// ************************************************************

// ES6 Promise callback
void CALLBACK PromiseContinuationCallback(JsValueRef task, void *callbackState)
{
	// Save promises in taskQueue.
	JsValueRef global;
	JsGetGlobalObject(&global);
	queue<Task*> * q = (queue<Task*> *)callbackState;
	q->push(new Task(task, 0, global, JS_INVALID_REFERENCE));
}

// ChakraCoreHost constructor
ChakraCoreHost::ChakraCoreHost()
{
	currentSourceContext = 0;
	JsContextRef context;

	// Create the runtime. We're only going to use one runtime for this host.
	if (JsCreateRuntime(JsRuntimeAttributeNone, nullptr, &runtime) != JsNoError)
		throw "failed to create runtime.";

	// Create a single execution context. 
	if (JsCreateContext(runtime, &context) != JsNoError)
		throw "failed to create execution context.";

	// Now set the execution context as being the current one on this thread.
	if (JsSetCurrentContext(context) != JsNoError)
		throw "failed to set current context.";

	// Set up ES6 Promise 
	if (JsSetPromiseContinuationCallback(PromiseContinuationCallback, &taskQueue) != JsNoError)
		throw "failed to set PromiseContinuationCallback.";

	// Add bindings to native methods 
	Binding::host = this;
	Binding::addNativeBindings();
}

// load script from file
wstring ChakraCoreHost::loadScript(wstring fileName)
{
	FILE *file;
	if (_wfopen_s(&file, fileName.c_str(), L"rb"))
	{
		fwprintf(stderr, L"chakrahost: unable to open file: %s.\n", fileName.c_str());
		return wstring();
	}

	unsigned int current = ftell(file);
	fseek(file, 0, SEEK_END);
	unsigned int end = ftell(file);
	fseek(file, current, SEEK_SET);
	unsigned int lengthBytes = end - current;
	char *rawBytes = (char *)calloc(lengthBytes + 1, sizeof(char));

	if (rawBytes == nullptr)
	{
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	fread((void *)rawBytes, sizeof(char), lengthBytes, file);

	wchar_t *contents = (wchar_t *)calloc(lengthBytes + 1, sizeof(wchar_t));
	if (contents == nullptr)
	{
		free(rawBytes);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	if (MultiByteToWideChar(CP_UTF8, 0, rawBytes, lengthBytes + 1, contents, lengthBytes + 1) == 0)
	{
		free(rawBytes);
		free(contents);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	wstring result = contents;
	free(rawBytes);
	free(contents);
	return result;
}

// run script
wstring ChakraCoreHost::runScript(wstring script)
{
	try
	{
		JsValueRef result;
		JsValueRef promiseResult;

		// Run the script.
		if (JsRunScript(script.c_str(), currentSourceContext++, L"", &result) != JsNoError)
		{
			// Get error message
			JsValueRef exception;
			if (JsGetAndClearException(&exception) != JsNoError)
				return L"failed to get and clear exception";

			JsPropertyIdRef messageName;
			if (JsGetPropertyIdFromName(L"message", &messageName) != JsNoError)
				return L"failed to get error message id";

			JsValueRef messageValue;
			if (JsGetProperty(exception, messageName, &messageValue))
				return L"failed to get error message";

			const wchar_t *message;
			size_t length;
			if (JsStringToPointer(messageValue, &message, &length) != JsNoError)
				return L"failed to convert error message";

			return message;
		}

		// Execute tasks stored in taskQueue
		while (!taskQueue.empty()) {
			Task* task = taskQueue.front();
			taskQueue.pop();
			int currentTime = clock() / (double)(CLOCKS_PER_SEC / 1000);
			if (currentTime - task->_time > task->_delay) {
				task->invoke();
				if (task->_repeat) {
					task->_time = currentTime;
					taskQueue.push(task);
				}
				else {
					delete task;
				}
			}
			else {
				taskQueue.push(task);
			}
		}

		// Convert the return value to wstring.
		JsValueRef stringResult;
		const wchar_t *returnValue;
		size_t stringLength;
		if (JsConvertValueToString(result, &stringResult) != JsNoError)
			return L"failed to convert value to string.";
		if (JsStringToPointer(stringResult, &returnValue, &stringLength) != JsNoError)
			return L"failed to convert return value.";
		return returnValue;
	}
	catch (...)
	{
		return L"chakrahost: fatal error: internal error.\n";
	}
}

ChakraCoreHost::~ChakraCoreHost()
{
	JsDisposeRuntime(runtime);
}

// ************************************************************
//						    Binding
// ************************************************************

ChakraCoreHost* Binding::host;
JsValueRef Binding::JSPointPrototype;
JsValueRef Binding::JSLinePrototype;
JsValueRef Binding::JSTrianglePrototype;
JsValueRef Binding::JSQuadPrototype;
JsValueRef Binding::JSPolygonPrototype;
JsValueRef Binding::mouseCallbackFunc;
JsValueRef Binding::mouseCallbackThisArg;

// ******************************
//	  Binding - Util functions
// ******************************

void Binding::setCallback(JsValueRef object, const wchar_t *propertyName, JsNativeFunction callback, void *callbackState)
{
	JsPropertyIdRef propertyId;
	JsGetPropertyIdFromName(propertyName, &propertyId);
	JsValueRef function;
	JsCreateFunction(callback, callbackState, &function);
	JsSetProperty(object, propertyId, function, true);
}

void Binding::setProperty(JsValueRef object, const wchar_t *propertyName, JsValueRef property)
{
	JsPropertyIdRef propertyId;
	JsGetPropertyIdFromName(propertyName, &propertyId);
	JsSetProperty(object, propertyId, property, true);
}

JsValueRef Binding::getProperty(JsValueRef object, const wchar_t *propertyName)
{
	JsValueRef output;
	JsPropertyIdRef propertyId;
	JsGetPropertyIdFromName(propertyName, &propertyId);
	JsGetProperty(object, propertyId, &output);
	return output;
}

// ******************************
//	 Binding - General methods
// ******************************

// JsNativeFunction for console.log(expr)
JsValueRef CALLBACK Binding::JSLog(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	for (unsigned int index = 1; index < argumentCount; index++)
	{
		if (index > 1)
		{
			wprintf(L" ");
		}
		JsValueRef stringValue;
		JsConvertValueToString(arguments[index], &stringValue);
		const wchar_t *string;
		size_t length;
		JsStringToPointer(stringValue, &string, &length);
		wprintf(L"%s", string);
	}
	wprintf(L"\n");
	return JS_INVALID_REFERENCE;
}

// JsNativeFunction for setTimeout(func, delay)
JsValueRef CALLBACK Binding::JSSetTimeout(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 3);
	JsValueRef func = arguments[1];
	int delay = 0;
	JsNumberToInt(arguments[2], &delay);
	host->taskQueue.push(new Task(func, delay, arguments[0], JS_INVALID_REFERENCE));
	return JS_INVALID_REFERENCE;
}

// JsNativeFunction for setInterval(func, delay)
JsValueRef CALLBACK Binding::JSSetInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 3);
	JsValueRef func = arguments[1];
	int delay = 0;
	JsNumberToInt(arguments[2], &delay);
	host->taskQueue.push(new Task(func, delay, arguments[0], JS_INVALID_REFERENCE, true));
	return JS_INVALID_REFERENCE;
}

// ******************************
//		 Binding - Shapes
// ******************************

// project JavaScript Point object back to native GLPoint
GLPoint* Binding::JSPointToNativePoint(JsValueRef point) {
	void* p;
	JsGetExternalData(point, &p);
	return reinterpret_cast<GLPoint*>(p);
}

// JsNativeFunction for Pointer constructor - Point(x, y, z)
JsValueRef CALLBACK Binding::JSPointConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(isConstructCall && argumentCount == 4);
	JsValueRef output = JS_INVALID_REFERENCE;
	double x, y, z;
	JsNumberToDouble(arguments[1], &x);
	JsNumberToDouble(arguments[2], &y);
	JsNumberToDouble(arguments[3], &z);
	GLPoint* p1 = new GLPoint((float)x, (float)y, (float)z);
	JsCreateExternalObject(p1, nullptr, &output);
	JsSetPrototype(output, JSPointPrototype);
	return output;
}

// create GLPolygon
JsValueRef Binding::createPolygon(JsValueRef *arguments, unsigned short argumentCount) {
	JsValueRef output = JS_INVALID_REFERENCE;
	vector<GLPoint> points;
	for (int i = 1; i < argumentCount; i++)
	{
		points.push_back(*(JSPointToNativePoint(arguments[i])));
	}
	GLPolygon* polygon = new GLPolygon(points);
	JsCreateExternalObject(polygon, nullptr, &output);
	return output;
}

// JsNativeFunction for Line constructor - new Line(point1, point2)
JsValueRef CALLBACK Binding::JSLineConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(isConstructCall && argumentCount == 3);
	JsValueRef output = JS_INVALID_REFERENCE;
	output = createPolygon(arguments, argumentCount);
	JsSetPrototype(output, JSLinePrototype);
	return output;
}

// JsNativeFunction for Triangle constructor - Triangle(point1, point2, point3)
JsValueRef CALLBACK Binding::JSTriangleConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(isConstructCall && argumentCount == 4);
	JsValueRef output = JS_INVALID_REFERENCE;
	output = createPolygon(arguments, argumentCount);
	JsSetPrototype(output, JSTrianglePrototype);
	return output;
}

// JsNativeFunction for Quad constructor - Quad(point1, point2, point3, point4)
JsValueRef CALLBACK Binding::JSQuadConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(isConstructCall && argumentCount == 5);
	JsValueRef output = JS_INVALID_REFERENCE;
	output = createPolygon(arguments, argumentCount);
	JsSetPrototype(output, JSQuadPrototype);
	return output;
}

// JsNativeFunction for Polygon constructor - Polygon([points])
JsValueRef CALLBACK Binding::JSPolygonConstructor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(isConstructCall && argumentCount == 2);
	JsValueRef output = JS_INVALID_REFERENCE;
	vector<GLPoint> points;
	// retrieve all elements in [points] param 
	int length;
	JsNumberToInt(getProperty(arguments[1], L"length"), &length);
	for (int i = 0; i < length; i++) {
		JsValueRef jsIndex;
		JsIntToNumber(i, &jsIndex);
		JsValueRef jsPoint;
		JsGetIndexedProperty(arguments[1], jsIndex, &jsPoint);
		points.push_back(*(JSPointToNativePoint(jsPoint)));
	}
	GLPolygon* polygon = new GLPolygon(points);
	JsCreateExternalObject(polygon, nullptr, &output);
	JsSetPrototype(output, JSPolygonPrototype);
	return output;
}

// JsNativeFunction for rotate - shape.rotate(rotateAngle, x, y, z)
JsValueRef CALLBACK Binding::JSRotate(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 5);
	JsValueRef output = JS_INVALID_REFERENCE;
	void* shape;
	if (JsGetExternalData(arguments[0], &shape) == JsNoError) {
		GLShape* s = static_cast<GLShape*>(shape);
		double rotateAngle, x, y, z;
		JsNumberToDouble(arguments[1], &rotateAngle);
		JsNumberToDouble(arguments[2], &x);
		JsNumberToDouble(arguments[3], &y);
		JsNumberToDouble(arguments[4], &z);
		s->rotate((float)rotateAngle, GLTriple((float)x, (float)y, (float)z));
	};
	return output;
}

// JsNativeFunction for setColor - shape.setColor(R, G, B)
JsValueRef CALLBACK Binding::JSSetColor(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 4);
	JsValueRef output = JS_INVALID_REFERENCE;
	void* shape;
	if (JsGetExternalData(arguments[0], &shape) == JsNoError) {
		GLShape* s = static_cast<GLShape*>(shape);
		double x, y, z;
		JsNumberToDouble(arguments[1], &x);
		JsNumberToDouble(arguments[2], &y);
		JsNumberToDouble(arguments[3], &z);
		s->setColor(GLTriple((float)x, (float)y, (float)z));
	};
	return output;
}

// JsNativeFunction for setPosition - shape.setPosition([points])
JsValueRef CALLBACK Binding::JSSetPosition(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 2);
	JsValueRef output = JS_INVALID_REFERENCE;
	void* shape;
	if (JsGetExternalData(arguments[0], &shape) == JsNoError) {
		GLPolygon* s = static_cast<GLPolygon*>(shape);
		s->_points.clear();
		// retrieve all elements in [points] param 
		int length;
		JsNumberToInt(getProperty(arguments[1], L"length"), &length);
		for (int i = 0; i < length; i++) {
			JsValueRef jsIndex;
			JsIntToNumber(i, &jsIndex);
			JsValueRef jsPoint;
			JsGetIndexedProperty(arguments[1], jsIndex, &jsPoint);
			s->_points.push_back(*(JSPointToNativePoint(jsPoint)));
		}
	};
	return output;
}

// ******************************
//	  Binding - Canvas methods
// ******************************

// JsNativeFunction for canvas.addShape(shape)
JsValueRef CALLBACK Binding::JSAddShape(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 2);
	void* shape;
	JsGetExternalData(arguments[1], &shape);
	GLShape* f = static_cast<GLShape*>(shape);
	host->canvas.addShape(static_cast<GLShape*>(shape));
	return JS_INVALID_REFERENCE;
}

// JsNativeFunction for canvas.removeShape(shape)
JsValueRef CALLBACK Binding::JSRemoveShape(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 2);
	void* shape;
	JsGetExternalData(arguments[1], &shape);
	host->canvas.removeShape(static_cast<GLShape*>(shape));
	return JS_INVALID_REFERENCE;
}

// JsNativeFunction for canvas.render()
JsValueRef CALLBACK Binding::JSRender(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 1);
	host->canvas.render();
	return JS_INVALID_REFERENCE;
}

void Binding::mouse_click_callback(GLFWwindow* window, int button, int action, int mods)
{
	double xpos, ypos, ratio;
	int width, height;
	glfwGetCursorPos(window, &xpos, &ypos);
	glfwGetWindowSize(window, &width, &height);
	ratio = (double)width / height;
	xpos = ratio * (xpos / width * 2 - 1);
	ypos = 1 - ypos / height * 2;
	JsValueRef jsXpos, jsYpos, jsArg;
	JsDoubleToNumber(xpos, &jsXpos);
	JsDoubleToNumber(ypos, &jsYpos);
	JsCreateObject(&jsArg);
	setProperty(jsArg, L"x", jsXpos);
	setProperty(jsArg, L"y", jsYpos);
	if (action == GLFW_PRESS) {
		host->taskQueue.push(new Task(mouseCallbackFunc, 0, mouseCallbackThisArg, jsArg));
	}
}

// JsNativeFunction for canvas.setMouseClickCallback((pos)=>{...})
JsValueRef CALLBACK Binding::JSSetMouseClickCallback(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	assert(!isConstructCall && argumentCount == 2);
	// release previously set callback 
	if (mouseCallbackFunc != JS_INVALID_REFERENCE && mouseCallbackThisArg != JS_INVALID_REFERENCE) {
		JsRelease(mouseCallbackFunc, nullptr);
		JsRelease(mouseCallbackThisArg, nullptr);
	}
	mouseCallbackFunc = arguments[1];
	mouseCallbackThisArg = arguments[0];
	// pin down the callback so that it will not be garbage collected
	JsAddRef(mouseCallbackFunc, nullptr);
	JsAddRef(mouseCallbackThisArg, nullptr);
	host->canvas.setMouseClickCallback(Binding::mouse_click_callback);
	return JS_INVALID_REFERENCE;
}

// project a custom native class and its member functions to JS
// there must be a one-to-one mapping between elements in memberNames and memberFunc
void Binding::projectNativeClass(const wchar_t *className, JsNativeFunction constructor, JsValueRef &prototype, vector<const wchar_t *> memberNames, vector<JsNativeFunction> memberFuncs) {
	// project constructor to global scope 
	JsValueRef globalObject;
	JsGetGlobalObject(&globalObject);
	JsValueRef jsConstructor;
	JsCreateFunction(constructor, nullptr, &jsConstructor);
	setProperty(globalObject, className, jsConstructor);
	// create class's prototype and project its member functions
	JsCreateObject(&prototype);
	assert(memberNames.size() == memberNames.size());
	for (int i = 0; i < memberNames.size(); ++i) {
		setCallback(prototype, memberNames[i], memberFuncs[i], nullptr);
	}
	setProperty(jsConstructor, L"prototype", prototype);
}

// add all native bindings
void Binding::addNativeBindings() {
	// project general methods - console.log, setTimeout, setInterval
	JsValueRef globalObject;
	JsGetGlobalObject(&globalObject);
	JsValueRef console;
	JsCreateObject(&console);
	setProperty(globalObject, L"console", console);
	setCallback(console, L"log", JSLog, nullptr);
	setCallback(globalObject, L"setTimeout", JSSetTimeout, nullptr);
	setCallback(globalObject, L"setInterval", JSSetInterval, nullptr);

	// project shape classes and their methods
	vector<const wchar_t *> memberNames;
	vector<JsNativeFunction> memberFuncs;
	memberNames.push_back(L"rotate");
	memberFuncs.push_back(JSRotate);
	memberNames.push_back(L"setColor");
	memberFuncs.push_back(JSSetColor);
	projectNativeClass(L"Point", JSPointConstructor, JSPointPrototype, memberNames, memberFuncs);
	// setPosition not available for Point
	memberNames.push_back(L"setPosition");
	memberFuncs.push_back(JSSetPosition);
	projectNativeClass(L"Line", JSLineConstructor, JSLinePrototype, memberNames, memberFuncs);
	projectNativeClass(L"Triangle", JSTriangleConstructor, JSTrianglePrototype, memberNames, memberFuncs);
	projectNativeClass(L"Quad", JSQuadConstructor, JSQuadPrototype, memberNames, memberFuncs);
	projectNativeClass(L"Polygon", JSPolygonConstructor, JSPolygonPrototype, memberNames, memberFuncs);

	// project canvas & its methods
	JsValueRef canvas;
	JsCreateObject(&canvas);
	setProperty(globalObject, L"canvas", canvas);
	setCallback(canvas, L"addShape", JSAddShape, nullptr);
	setCallback(canvas, L"removeShape", JSRemoveShape, nullptr);
	setCallback(canvas, L"render", JSRender, nullptr);
	setCallback(canvas, L"setMouseClickCallback", JSSetMouseClickCallback, nullptr);
}
