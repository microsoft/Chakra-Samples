Imports System.IO
Imports ChakraHost.Hosting

Public Module Program
    Private Structure CommandLineArguments
        Public Debug As Boolean
        Public Profile As Boolean
        Public ArgumentsStart As Integer
    End Structure

    Private currentSourceContext As JavaScriptSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero)

    ' We have to hold on to the delegates on the managed side of things so that the
    ' delegates aren't collected while the script is running.
    Private ReadOnly echoDelegate As JavaScriptNativeFunction = AddressOf Echo
    Private ReadOnly runScriptDelegate As JavaScriptNativeFunction = AddressOf RunScript

    Private Function ProcessArguments(arguments As String()) As CommandLineArguments
        Dim commandLineArguments = New CommandLineArguments()
        Dim current As Integer = 0

        While current < arguments.Length
            Dim argument As String = arguments(current)

            If argument.Length > 0 AndAlso (argument(0) = "/"c OrElse argument(0) = "-"c) Then
                Dim argumentFlag As String = argument.Substring(1)

                If argumentFlag = "debug" Then
                    commandLineArguments.Debug = True
                ElseIf argumentFlag = "profile" Then
                    commandLineArguments.Profile = True
                Else
                    Exit While
                End If
            Else
                Exit While
            End If
            current += 1
        End While

        commandLineArguments.ArgumentsStart = current
        Return commandLineArguments
    End Function

    Private Sub ThrowException(errorString As String)
        ' We ignore error since we're already in an error state.
        Dim errorValue As JavaScriptValue = JavaScriptValue.FromString(errorString)
        Dim errorObject As JavaScriptValue = JavaScriptValue.CreateError(errorValue)
        JavaScriptContext.SetException(errorObject)
    End Sub

    Private Function Echo(callee As JavaScriptValue, isConstructCall As Boolean, arguments As JavaScriptValue(), argumentCount As UShort, callbackData As IntPtr) As JavaScriptValue
        For index As Integer = 1 To argumentCount - 1
            If index > 1 Then
                Console.Write(" ")
            End If

            Console.Write(arguments(index).ConvertToString().ToString())
        Next

        Console.WriteLine()

        Return JavaScriptValue.Invalid
    End Function

    Private Sub StartDebugging()
        If Environment.Is64BitProcess Then
            Dim ipdm64 = DirectCast(New ProcessDebugManager(), IProcessDebugManager64)
            Dim ida64 As IDebugApplication64 = Nothing
            ipdm64.GetDefaultApplication(ida64)
            JavaScriptContext.StartDebugging(ida64)
        Else
            Dim ipdm32 = DirectCast(New ProcessDebugManager(), IProcessDebugManager32)
            Dim ida32 As IDebugApplication32 = Nothing
            ipdm32.GetDefaultApplication(ida32)
            JavaScriptContext.StartDebugging(ida32)
        End If
    End Sub

    Private Function RunScript(callee As JavaScriptValue, isConstructCall As Boolean, arguments As JavaScriptValue(), argumentCount As UShort, callbackData As IntPtr) As JavaScriptValue
        If argumentCount < 2 Then
            ThrowException("not enough arguments")
            Return JavaScriptValue.Invalid
        End If

        '
        ' Convert filename.
        '

        Dim filename As String = arguments(1).ToString()

        '
        ' Load the script from the disk.
        '

        Dim script As String = File.ReadAllText(filename)
        If String.IsNullOrEmpty(script) Then
            ThrowException("invalid script")
            Return JavaScriptValue.Invalid
        End If

        '
        ' Run the script.
        '

        Dim returnValue As JavaScriptValue =
            JavaScriptContext.RunScript(script, currentSourceContext, filename)
        currentSourceContext += 1
        Return returnValue
    End Function

    Private Sub DefineHostCallback(globalObject As JavaScriptValue, callbackName As String, callback As JavaScriptNativeFunction, callbackData As IntPtr)
        '
        ' Get property ID.
        '

        Dim propertyId As JavaScriptPropertyId = JavaScriptPropertyId.FromString(callbackName)

        '
        ' Create a function
        '

        Dim func As JavaScriptValue = JavaScriptValue.CreateFunction(callback, callbackData)

        '
        ' Set the property
        '

        globalObject.SetProperty(propertyId, func, True)
    End Sub

    Private Function CreateHostContext(runtime As JavaScriptRuntime, arguments As String(), argumentsStart As Integer) As JavaScriptContext
        '
        ' Create the context. Note that if we had wanted to start debugging from the very
        ' beginning, we would have passed in an IDebugApplication pointer here.
        '

        Dim context As JavaScriptContext = runtime.CreateContext()

        '
        ' Now set the execution context as being the current one on this thread.
        '

        Using New JavaScriptContext.Scope(context)
            '
            ' Create the host object the script will use.
            '

            Dim hostObject As JavaScriptValue = JavaScriptValue.CreateObject()

            '
            ' Get the global object
            '

            Dim globalObject As JavaScriptValue = JavaScriptValue.GlobalObject

            '
            ' Get the name of the property ("host") that we're going to set on the global object.
            '

            Dim hostPropertyId As JavaScriptPropertyId = JavaScriptPropertyId.FromString("host")

            '
            ' Set the property.
            '

            globalObject.SetProperty(hostPropertyId, hostObject, True)

            '
            ' Now create the host callbacks that we're going to expose to the script.
            '

            DefineHostCallback(hostObject, "echo", echoDelegate, IntPtr.Zero)
            DefineHostCallback(hostObject, "runScript", runScriptDelegate, IntPtr.Zero)

            '
            ' Create an array for arguments.
            '

            Dim hostArguments As JavaScriptValue = JavaScriptValue.CreateArray(CUInt(arguments.Length - argumentsStart))

            For index As Integer = argumentsStart To arguments.Length - 1
                '
                ' Create the argument value.
                '

                Dim argument As JavaScriptValue = JavaScriptValue.FromString(arguments(index))

                '
                ' Create the index.
                '

                Dim indexValue As JavaScriptValue = JavaScriptValue.FromInt32(index - argumentsStart)

                '
                ' Set the value.
                '

                hostArguments.SetIndexedProperty(indexValue, argument)
            Next

            '
            ' Get the name of the property that we're going to set on the host object.
            '

            Dim argumentsPropertyId As JavaScriptPropertyId = JavaScriptPropertyId.FromString("arguments")

            '
            ' Set the arguments property.
            '

            hostObject.SetProperty(argumentsPropertyId, hostArguments, True)
        End Using

        Return context
    End Function

    Private Sub PrintScriptException(exception As JavaScriptValue)
        '
        ' Get message.
        '

        Dim messageName As JavaScriptPropertyId = JavaScriptPropertyId.FromString("message")
        Dim messageValue As JavaScriptValue = exception.GetProperty(messageName)
        Dim message As String = messageValue.ToString()

        Console.Error.WriteLine("chakrahost: exception: {0}", message)
    End Sub

    '
    ' The main entry point for the host.
    '
    Public Function Main(arguments As String()) As Integer
        Dim returnValue As Integer = 1
        Dim commandLineArguments As CommandLineArguments = ProcessArguments(arguments)

        If arguments.Length - commandLineArguments.ArgumentsStart < 0 Then
            Console.Error.WriteLine("usage: chakrahost [-debug] [-profile] <script name> <arguments>")
            Return returnValue
        End If

        Try
            '
            ' Create the runtime. We're only going to use one runtime for this host.
            '

            Dim runtime As JavaScriptRuntime = JavaScriptRuntime.Create()
            Using runtime
                '
                ' Similarly, create a single execution context. Note that we're putting it on the stack here,
                ' so it will stay alive through the entire run.
                '

                Dim context As JavaScriptContext = CreateHostContext(runtime, arguments, commandLineArguments.ArgumentsStart)

                '
                ' Now set the execution context as being the current one on this thread.
                '

                Using New JavaScriptContext.Scope(context)
                    '
                    ' Start debugging if requested.
                    '

                    If commandLineArguments.Debug Then
                        StartDebugging()
                    End If

                    '
                    ' Start profiling if requested.
                    '

                    If commandLineArguments.Profile Then
                        Dim profiler = New Profiler()
                        JavaScriptContext.StartProfiling(profiler, ProfilerEventMask.TraceAll, 0)
                    End If

                    '
                    ' Load the script from the disk.
                    '

                    Dim script As String = File.ReadAllText(arguments(commandLineArguments.ArgumentsStart))

                    '
                    ' Run the script.
                    '

                    Dim result As JavaScriptValue
                    Try
                        result = JavaScriptContext.RunScript(script, currentSourceContext, arguments(commandLineArguments.ArgumentsStart))
                        currentSourceContext += 1
                    Catch e As JavaScriptScriptException
                        PrintScriptException(e.JavaScriptError)
                        Return 1
                    Catch e As Exception
                        Console.Error.WriteLine("chakrahost: failed to run script: {0}", e.Message)
                        Return 1
                    End Try

                    '
                    ' Convert the return value.
                    '

                    Dim numberResult As JavaScriptValue = result.ConvertToNumber()
                    Dim doubleResult As Double = numberResult.ToDouble()
                    If Double.IsNaN(doubleResult) Then
                        returnValue = 0
                    Else
                        returnValue = CInt(doubleResult)
                    End If

                    '
                    ' Stop profiling.
                    '

                    If commandLineArguments.Profile Then
                        JavaScriptContext.StopProfiling(0)
                    End If
                End Using
            End Using
        Catch e As Exception
            Console.Error.WriteLine("chakrahost: fatal error: internal error: {0}.", e.Message)
        End Try

        Return returnValue
    End Function
End Module
