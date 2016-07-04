using System;
using System.IO;
using ChakraHost.Hosting;
using System.Collections.Generic;
using System.Threading;

namespace ChakraHost
{
    public static class Program
    {
        private struct CommandLineArguments
        {
            public int ArgumentsStart;
        };

        class TimeoutCall
        {
            public TimeoutCall( int msecs, JavaScriptValue callback_, JavaScriptValue callee_)
            {
                dt = InspecificNow().AddMilliseconds(msecs);
                callback = callback_;
                callee = callee_;
                callback.AddRef();
                callee.AddRef();
            }
            public DateTime dt;
            public JavaScriptValue callback;
            public JavaScriptValue callee;
            public void CallTimeoutFunction()
            {
                callback.CallFunction(callee);
                callback.Release();
                callee.Release();
            }
        }

        private static DateTime InspecificNow()
        {
            return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        }

        private static SortedList<DateTime, TimeoutCall> timeouts = new SortedList<DateTime, TimeoutCall>();

        private static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        
        // We have to hold on to the delegates on the managed side of things so that the
        // delegates aren't collected while the script is running.
        private static readonly JavaScriptNativeFunction echoDelegate = Echo;
        private static readonly JavaScriptNativeFunction setTimeoutDelegate = SetTimeout;
        private static readonly JavaScriptNativeFunction runScriptDelegate = RunScript;

        private static void ThrowException(string errorString)
        {
            // We ignore error since we're already in an error state.
            JavaScriptValue errorValue = JavaScriptValue.FromString(errorString);
            JavaScriptValue errorObject = JavaScriptValue.CreateError(errorValue);
            JavaScriptContext.SetException(errorObject);
        }

        private static JavaScriptValue Echo(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            for (uint index = 1; index < argumentCount; index++)
            {
                if (index > 1)
                {
                    Console.Write(" ");
                }

                Console.Write(arguments[index].ConvertToString().ToString());
            }

            Console.WriteLine();

            return JavaScriptValue.Invalid;
        }

        private static JavaScriptValue SetTimeout(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            // setTimeout signature is (callback, after)
            if (arguments.Length < 2)
            {
                Console.WriteLine("Invalid argument lengths, required 2");
                return JavaScriptValue.Invalid;
            }
            JavaScriptValue callbackValue = arguments[1];
            JavaScriptValue timeout = arguments[2];
            JavaScriptValue afterValue = arguments[2].ConvertToNumber();
            int after = (int)Math.Max(afterValue.ToDouble(), 1.0);
            TimeoutCall to = new TimeoutCall(after, callbackValue, callee);
            timeouts.Add(to.dt,to);
            return JavaScriptValue.Invalid;
        }

        private static JavaScriptValue RunScript(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            if (argumentCount < 2)
            {
                ThrowException("not enough arguments");
                return JavaScriptValue.Invalid;
            }

            //
            // Convert filename.
            //

            string filename = arguments[1].ToString();

            //
            // Load the script from the disk.
            //

            string script = File.ReadAllText(filename);
            if (string.IsNullOrEmpty(script))
            {
                ThrowException("invalid script");
                return JavaScriptValue.Invalid;
            }

            //
            // Run the script.
            //

            return JavaScriptContext.RunScript(script, currentSourceContext++, filename);
        }

        private static void DefineHostCallback(JavaScriptValue globalObject, string callbackName, JavaScriptNativeFunction callback, IntPtr callbackData)
        {
            //
            // Get property ID.
            //

            JavaScriptPropertyId propertyId = JavaScriptPropertyId.FromString(callbackName);

            //
            // Create a function
            //

            JavaScriptValue function = JavaScriptValue.CreateFunction(callback, callbackData);

            //
            // Set the property
            //

            globalObject.SetProperty(propertyId, function, true);
        }

        private static JavaScriptContext CreateHostContext(JavaScriptRuntime runtime, string[] arguments, int argumentsStart)
        {
            //
            // Create the context. Note that if we had wanted to start debugging from the very
            // beginning, we would have called JsStartDebugging right after context is created.
            //

            JavaScriptContext context = runtime.CreateContext();

            //
            // Now set the execution context as being the current one on this thread.
            //

            using (new JavaScriptContext.Scope(context))
            {
                //
                // Create the host object the script will use.
                //

                JavaScriptValue hostObject = JavaScriptValue.CreateObject();

                //
                // Get the global object
                //

                JavaScriptValue globalObject = JavaScriptValue.GlobalObject;

                //
                // Get the name of the property ("host") that we're going to set on the global object.
                //

                JavaScriptPropertyId hostPropertyId = JavaScriptPropertyId.FromString("host");

                //
                // Set the property.
                //

                globalObject.SetProperty(hostPropertyId, hostObject, true);

                //
                // Now create the host callbacks that we're going to expose to the script.
                //

                DefineHostCallback(hostObject, "echo", echoDelegate, IntPtr.Zero);
                DefineHostCallback(hostObject, "runScript", runScriptDelegate, IntPtr.Zero);
                DefineHostCallback(JavaScriptValue.GlobalObject, "setTimeout", setTimeoutDelegate, IntPtr.Zero);

                //
                // Create an array for arguments.
                //

                JavaScriptValue hostArguments = JavaScriptValue.CreateArray((uint)(arguments.Length - argumentsStart));

                for (int index = argumentsStart; index < arguments.Length; index++)
                {
                    //
                    // Create the argument value.
                    //

                    JavaScriptValue argument = JavaScriptValue.FromString(arguments[index]);

                    //
                    // Create the index.
                    //

                    JavaScriptValue indexValue = JavaScriptValue.FromInt32(index - argumentsStart);

                    //
                    // Set the value.
                    //

                    hostArguments.SetIndexedProperty(indexValue, argument);
                }

                //
                // Get the name of the property that we're going to set on the host object.
                //

                JavaScriptPropertyId argumentsPropertyId = JavaScriptPropertyId.FromString("arguments");

                //
                // Set the arguments property.
                //

                hostObject.SetProperty(argumentsPropertyId, hostArguments, true);
            }

            return context;
        }

        private static void PrintScriptException(JavaScriptValue exception)
        {
            //
            // Get message.
            //

            JavaScriptPropertyId messageName = JavaScriptPropertyId.FromString("message");
            JavaScriptValue messageValue = exception.GetProperty(messageName);
            string message = messageValue.ToString();

            Console.Error.WriteLine("chakrahost: exception: {0}", message);
        }

        //
        // The main entry point for the host.
        //
        public static int Main(string[] arguments)
        {
            int returnValue = 1;
            CommandLineArguments commandLineArguments;
            commandLineArguments.ArgumentsStart = 0;

            if (arguments.Length - commandLineArguments.ArgumentsStart < 0)
            {
                Console.Error.WriteLine("usage: chakrahost <script name> <arguments>");
                return returnValue;
            }

            try
            {
                //
                // Create the runtime. We're only going to use one runtime for this host.
                //

                using (JavaScriptRuntime runtime = JavaScriptRuntime.Create())
                {
                    //
                    // Similarly, create a single execution context. Note that we're putting it on the stack here,
                    // so it will stay alive through the entire run.
                    //

                    JavaScriptContext context = CreateHostContext(runtime, arguments, commandLineArguments.ArgumentsStart);

                    //
                    // Now set the execution context as being the current one on this thread.
                    //

                    using (new JavaScriptContext.Scope(context))
                    {
                        //
                        // Load the script from the disk.
                        //

                        string script = File.ReadAllText(arguments[commandLineArguments.ArgumentsStart]);

                        //
                        // Run the script.
                        //

                        JavaScriptValue result;
                        try
                        {
                            result = JavaScriptContext.RunScript(script, currentSourceContext++, arguments[commandLineArguments.ArgumentsStart]);
                            while( timeouts.Count > 0)
                            {
                                DateTime inow = InspecificNow();
                                DateTime next = timeouts.Keys[0];
                                TimeSpan diff = next.Subtract(inow);
                                if ( diff.TotalMilliseconds <= 0 )
                                {
                                    timeouts.Values[0].CallTimeoutFunction();
                                    timeouts.RemoveAt(0);
                                }else
                                {
                                    Thread.Sleep(diff);
                                }
                            }
                        }
                        catch (JavaScriptScriptException e)
                        {
                            PrintScriptException(e.Error);
                            return 1;
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("chakrahost: failed to run script: {0}", e.Message);
                            return 1;
                        }

                        //
                        // Convert the return value.
                        //

                        JavaScriptValue numberResult = result.ConvertToNumber();
                        double doubleResult = numberResult.ToDouble();
                        returnValue = (int)doubleResult;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("chakrahost: fatal error: internal error: {0}.", e.Message);
            }

            Console.WriteLine(returnValue);
            return returnValue;
        }
    }
}