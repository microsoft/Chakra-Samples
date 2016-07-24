using System;
using System.Runtime.InteropServices;
// wrapper namespace
using ChakraHost.Hosting;

public class HelloWorld
{
    static void Main() {
        JavaScriptRuntime runtime;
        JavaScriptContext context;
        JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        JavaScriptValue result;

      	// Your script, try replace the basic hello world with something else
        string script = "(()=>{return \'Hello world!\';})()";

        // Create a runtime. 
        Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime);
        
        // Create an execution context. 
        Native.JsCreateContext(runtime, out context);
        
        // Now set the execution context as being the current one on this thread.
        Native.JsSetCurrentContext(context);
        
        // Run the script.
        Native.JsRunScript(script, currentSourceContext++, "", out result);

        // Convert your script result to String in JavaScript; redundant if your script returns a String
        JavaScriptValue resultJSString;
        Native.JsConvertValueToString(result, out resultJSString);
        
        // Project script result in JS back to C#.
        IntPtr resultPtr;
        UIntPtr stringLength;
        Native.JsStringToPointer(resultJSString, out resultPtr, out stringLength);

        string resultString = Marshal.PtrToStringUni(resultPtr);
        Console.WriteLine(resultString);

        // Dispose runtime
        Native.JsSetCurrentContext(JavaScriptContext.Invalid);
        Native.JsDisposeRuntime(runtime);
    }
}