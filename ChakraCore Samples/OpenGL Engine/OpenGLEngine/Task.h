#pragma once
#include "ChakraCore.h"

// represent a task in Javascript
class Task
{
public:
	JsValueRef _func;
	JsValueRef _args[2];
	int _argCount;
	int _delay;
	bool _repeat;
	int _time;
	Task(JsValueRef func, int delay, JsValueRef thisArg, JsValueRef extraArgs, bool repeat = false);
	JsValueRef invoke();						// invoke a task
	~Task();
};
