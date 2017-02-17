using System;
using System.IO;
using ChakraHost.Hosting;

namespace ChakraHost
{
    public static class Program
    {
        private struct CommandLineArguments
        {
            public bool Debug;
            public bool Profile;
            public int ArgumentsStart;
        };

        private static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        
        // We have to hold on to the delegates on the managed side of things so that the
        // delegates aren't collected while the script is running.
        private static readonly JavaScriptNativeFunction echoDelegate = Echo;
        private static readonly JavaScriptNativeFunction runScriptDelegate = RunScript;

        private static CommandLineArguments ProcessArguments(string[] arguments)
        {
            var commandLineArguments = new CommandLineArguments();
            int current = 0;

            for (; current < arguments.Length; current++)
            {
                string argument = arguments[current];

                if (argument.Length > 0 && (argument[0] == '/' || argument[0] == '-'))
                {
                    string argumentFlag = argument.Substring(1);

                    if (argumentFlag == "debug")
                    {
                        commandLineArguments.Debug = true;
                    }
                    else if (argumentFlag == "profile")
                    {
                        commandLineArguments.Profile = true;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            commandLineArguments.ArgumentsStart = current;
            return commandLineArguments;
        }

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

        private static void StartDebugging()
        {
            if (Environment.Is64BitProcess)
            {
                var ipdm64 = (Native.IProcessDebugManager64)new Native.ProcessDebugManager();
                Native.IDebugApplication64 ida64;
                ipdm64.GetDefaultApplication(out ida64);
                JavaScriptContext.StartDebugging(ida64);
            }
            else
            {
                var ipdm32 = (Native.IProcessDebugManager32)new Native.ProcessDebugManager();
                Native.IDebugApplication32 ida32;
                ipdm32.GetDefaultApplication(out ida32);
                JavaScriptContext.StartDebugging(ida32);
            }
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
            // beginning, we would have passed in an IDebugApplication pointer here.
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
            CommandLineArguments commandLineArguments = ProcessArguments(arguments);

            if (arguments.Length - commandLineArguments.ArgumentsStart < 1)
            {
                Console.Error.WriteLine("usage: chakrahost [-debug] [-profile] <script name> <arguments>");
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
                        // Start debugging if requested.
                        //

                        if (commandLineArguments.Debug)
                        {
                            StartDebugging();
                        }

                        //
                        // Start profiling if requested.
                        //

                        if (commandLineArguments.Profile)
                        {
                            var profiler = new Profiler();
                            JavaScriptContext.StartProfiling(profiler, Native.ProfilerEventMask.TraceAll, 0);
                        }

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

                        //
                        // Stop profiling.
                        //

                        if (commandLineArguments.Profile)
                        {
                            JavaScriptContext.StopProfiling(0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("chakrahost: fatal error: internal error: {0}.", e.Message);
            }

            return returnValue;
        }
    }
}