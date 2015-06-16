#include "stdafx.h"
#include <msopc.h>
#include <string>
#include <stack>
#include <queue>

using namespace std;

Profiler::Profiler(void)
{
	m_refCount = 1;
}

Profiler::~Profiler(void)
{
}

HRESULT Profiler::QueryInterface(REFIID riid, void **ppvObj)
{
	if (riid == IID_IUnknown)
	{
		*ppvObj = (IUnknown *) this;
	}
	else if (riid == IID_IActiveScriptProfilerCallback)
	{
		*ppvObj = (IActiveScriptProfilerCallback *) this;
	}
	else if (riid == IID_IActiveScriptProfilerCallback2)
	{
		*ppvObj = (IActiveScriptProfilerCallback2 *) this;
	}
	else
	{
		*ppvObj = NULL;
		return E_NOINTERFACE;
	}

	AddRef();
	return NOERROR;
}

ULONG Profiler::AddRef()
{
	return InterlockedIncrement(&m_refCount);
}

ULONG Profiler::Release()
{
	long lw;

	if (0 == (lw = InterlockedDecrement(&m_refCount)))
	{
		delete this;
	}
	return lw;
}

HRESULT Profiler::Initialize(DWORD dwContext)
{
	fwprintf(stdout, L"Profiler::Initialize: 0x%lx\n", dwContext);
	return S_OK;
}

HRESULT Profiler::Shutdown(HRESULT hrReason)
{
	fwprintf(stdout, L"Profiler::Shutdown: 0x%lx\n", hrReason);
	return S_OK;
}

HRESULT Profiler::ScriptCompiled(PROFILER_TOKEN scriptId, PROFILER_SCRIPT_TYPE type, IUnknown *pIDebugDocumentContext)
{
	fwprintf(stdout, L"Profiler::ScriptCompiled: 0x%lx, %u\n", scriptId, type);
	return S_OK;
}

HRESULT Profiler::FunctionCompiled(PROFILER_TOKEN functionId, PROFILER_TOKEN scriptId, const wchar_t *pwszFunctionName, const wchar_t *pwszFunctionNameHint, IUnknown *pIDebugDocumentContext)
{
	fwprintf(stdout, L"Profiler::FunctionCompiled: 0x%lx, 0x%lx, %s, %s\n", scriptId, functionId, pwszFunctionName, pwszFunctionNameHint);
	return S_OK;
}

HRESULT Profiler::OnFunctionEnter(PROFILER_TOKEN scriptId, PROFILER_TOKEN functionId)
{
	fwprintf(stdout, L"Profiler::OnFunctionEnter: 0x%lx, 0x%lx\n", scriptId, functionId);
	return S_OK;
}

HRESULT Profiler::OnFunctionExit(PROFILER_TOKEN scriptId, PROFILER_TOKEN functionId)
{
	fwprintf(stdout, L"Profiler::OnFunctionExit: 0x%lx, 0x%lx\n", scriptId, functionId);
	return S_OK;
}

HRESULT Profiler::OnFunctionEnterByName(const wchar_t *pwszFunctionName, PROFILER_SCRIPT_TYPE type)
{
	fwprintf(stdout, L"Profiler::OnFunctionEnterByName: %s, %u\n", pwszFunctionName, type);
	return S_OK;
}

HRESULT Profiler::OnFunctionExitByName(const wchar_t *pwszFunctionName, PROFILER_SCRIPT_TYPE type)
{
	fwprintf(stdout, L"Profiler::OnFunctionExitByName: %s, %u\n", pwszFunctionName, type);
	return S_OK;
}
