#pragma once

#include <sdkddkver.h>
#include <windows.h>
#include <jsrt.h>
#include "Profiler.h"

#define IfFailError(v, e) \
    { \
        JsErrorCode error = (v); \
        if (error != JsNoError) \
        { \
            fwprintf(stderr, L"chakrahost: fatal error: %s.\n", (e)); \
            goto error; \
        } \
    }

#define IfFailRet(v) \
    { \
        JsErrorCode error = (v); \
        if (error != JsNoError) \
        { \
            return error; \
        } \
    }

#define IfFailThrow(v, e) \
    { \
        JsErrorCode error = (v); \
        if (error != JsNoError) \
        { \
            ThrowException((e)); \
            return JS_INVALID_REFERENCE; \
        } \
    }

#define IfComFailError(v) \
    { \
        hr = (v); \
        if (FAILED(hr)) \
        { \
            goto error; \
        } \
    }

#define IfComFailRet(v) \
    { \
        HRESULT hr = (v); \
        if (FAILED(hr)) \
        { \
            return hr; \
        } \
    }
