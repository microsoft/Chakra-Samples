using System;
using System.Runtime.InteropServices;
using ChakraHost.Hosting;

namespace ChakraHost
{
    [Guid("C3C44476-A1CE-403E-AFE2-D53EF618B139")]
    internal class Profiler : Native.IActiveScriptProfilerCallback2
    {
        public void OnFunctionEnterByName(string pwszFunctionName, Native.ProfilerScriptType type)
        {
            Console.WriteLine("Profiler.OnFunctionEnterByName: {0}, {1}", pwszFunctionName, type);
        }

        public void OnFunctionExitByName(string pwszFunctionName, Native.ProfilerScriptType type)
        {
            Console.WriteLine("Profiler.OnFunctionExitByName: {0}, {1}", pwszFunctionName, type);
        }

        public void Initialize(uint dwContext)
        {
            Console.WriteLine("Profiler.Initialize: {0}", dwContext);
        }

        public void Shutdown(uint hrReason)
        {
            Console.WriteLine("Profiler.Shutdown: {0}", hrReason);
        }

        public void ScriptCompiled(int scriptId, Native.ProfilerScriptType type, IntPtr pIDebugDocumentContext)
        {
            Console.WriteLine("Profiler.ScriptCompiled: {0}, {1}", scriptId, type);
        }

        public void FunctionCompiled(int functionId, int scriptId, string pwszFunctionName, string pwszFunctionNameHint, IntPtr pIDebugDocumentContext)
        {
            Console.WriteLine("Profiler.FunctionCompiled: {0}, {1}, {2}, {3}", functionId, scriptId, pwszFunctionName, pwszFunctionNameHint);
        }

        public void OnFunctionEnter(int scriptId, int functionId)
        {
            Console.WriteLine("Profiler.OnFunctionEnter: {0}, {1}", scriptId, functionId);
        }

        public void OnFunctionExit(int scriptId, int functionId)
        {
            Console.WriteLine("Profiler.OnFunctionExit: {0}, {1}", scriptId, functionId);
        }
    }
}
