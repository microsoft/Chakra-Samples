#pragma once
#include "Task.h"
#include <time.h>

using namespace std;

Task::Task(JsValueRef func, int delay, JsValueRef thisArg, JsValueRef extraArgs, bool repeat)
{
	_func = func;
	_delay = delay;
	_argCount = 1;
	_args[0] = thisArg;
	_args[1] = extraArgs;
	_repeat = repeat;
	_time = clock() / (double)(CLOCKS_PER_SEC / 1000);
	JsAddRef(_func, nullptr);
	JsAddRef(_args[0], nullptr);
	if (extraArgs != JS_INVALID_REFERENCE) {
		JsErrorCode err = JsAddRef(_args[1], nullptr);
		_argCount = 2;
	}
}

JsValueRef Task::invoke()
{
	JsValueRef ret = JS_INVALID_REFERENCE;
	JsCallFunction(_func, _args, _argCount, &ret);
	return ret;
}

Task::~Task()
{
	JsRelease(_func, nullptr);
	JsRelease(_args[0], nullptr);
	if (_args[1] != JS_INVALID_REFERENCE) {
		JsRelease(_args[1], nullptr);
	}
}
