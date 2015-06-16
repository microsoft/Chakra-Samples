Imports System.Runtime.InteropServices
Imports ChakraHost.Hosting

<Guid("C3C44476-A1CE-403E-AFE2-D53EF618B139")>
Friend Class Profiler
    Implements IActiveScriptProfilerCallback2

    Public Sub OnFunctionEnterByName(pwszFunctionName As String, type As ProfilerScriptType) Implements IActiveScriptProfilerCallback2.OnFunctionEnterByName
        Console.WriteLine("Profiler.OnFunctionEnterByName: {0}, {1}", pwszFunctionName, type)
    End Sub

    Public Sub OnFunctionExitByName(pwszFunctionName As String, type As ProfilerScriptType) Implements IActiveScriptProfilerCallback2.OnFunctionExitByName
        Console.WriteLine("Profiler.OnFunctionExitByName: {0}, {1}", pwszFunctionName, type)
    End Sub

    Public Sub Initialize(dwContext As UInteger) Implements IActiveScriptProfilerCallback.Initialize
        Console.WriteLine("Profiler.Initialize: {0}", dwContext)
    End Sub

    Public Sub Shutdown(hrReason As UInteger) Implements IActiveScriptProfilerCallback.Shutdown
        Console.WriteLine("Profiler.Shutdown: {0}", hrReason)
    End Sub

    Public Sub ScriptCompiled(scriptId As Integer, type As ProfilerScriptType, pIDebugDocumentContext As IntPtr) Implements IActiveScriptProfilerCallback.ScriptCompiled
        Console.WriteLine("Profiler.ScriptCompiled: {0}, {1}", scriptId, type)
    End Sub

    Public Sub FunctionCompiled(functionId As Integer, scriptId As Integer, pwszFunctionName As String, pwszFunctionNameHint As String, pIDebugDocumentContext As IntPtr) Implements IActiveScriptProfilerCallback.FunctionCompiled
        Console.WriteLine("Profiler.FunctionCompiled: {0}, {1}, {2}, {3}", functionId, scriptId, pwszFunctionName, pwszFunctionNameHint)
    End Sub

    Public Sub OnFunctionEnter(scriptId As Integer, functionId As Integer) Implements IActiveScriptProfilerCallback.OnFunctionEnter
        Console.WriteLine("Profiler.OnFunctionEnter: {0}, {1}", scriptId, functionId)
    End Sub

    Public Sub OnFunctionExit(scriptId As Integer, functionId As Integer) Implements IActiveScriptProfilerCallback.OnFunctionExit
        Console.WriteLine("Profiler.OnFunctionExit: {0}, {1}", scriptId, functionId)
    End Sub
End Class
