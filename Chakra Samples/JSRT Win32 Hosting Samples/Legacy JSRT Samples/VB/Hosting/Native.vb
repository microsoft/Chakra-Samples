Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices

Namespace Hosting
    ''' <summary>
    '''     Native interfaces.
    ''' </summary>
    Public Module Native
        ''' <summary>
        '''     Event mask for profiling.
        ''' </summary>
        Public Enum ProfilerEventMask
            ''' <summary>
            '''     Trace calls to script functions.
            ''' </summary>
            TraceScriptFunctionCall = &H1

            ''' <summary>
            '''     Trace calls to built-in functions.
            ''' </summary>
            TraceNativeFunctionCall = &H2

            ''' <summary>
            '''     Trace calls to DOM methods.
            ''' </summary>
            TraceDomFunctionCall = &H4

            ''' <summary>
            '''     Trace all calls except DOM methods.
            ''' </summary>
            TraceAll = (TraceScriptFunctionCall Or TraceNativeFunctionCall)

            ''' <summary>
            '''     Trace all calls.
            ''' </summary>
            TraceAllWithDom = (TraceAll Or TraceDomFunctionCall)
        End Enum

        ''' <summary>
        '''     Profiled script type.
        ''' </summary>
        Public Enum ProfilerScriptType
            ''' <summary>
            '''     A user script.
            ''' </summary>
            User

            ''' <summary>
            '''     A dynamic script.
            ''' </summary>
            Dynamic

            ''' <summary>
            '''     A native script.
            ''' </summary>
            Native

            ''' <summary>
            '''     A DOM-related script.
            ''' </summary>
            Dom
        End Enum

        ''' <summary>
        '''     IProcessDebugManager32 COM interface.
        ''' </summary>
        <Guid("51973C2f-CB0C-11d0-B5C9-00A0244A0E7A")>
        <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
        Public Interface IProcessDebugManager32
            ''' <summary>
            '''     Creates a new debug application.
            ''' </summary>
            ''' <param name="debugApplication">The new debug application.</param>
            Sub CreateApplication(ByRef debugApplication As IDebugApplication32)

            ''' <summary>
            '''     Gets the default debug application.
            ''' </summary>
            ''' <param name="debugApplication">The default debug application.</param>
            Sub GetDefaultApplication(ByRef debugApplication As IDebugApplication32)

            ''' <summary>
            '''     Adds a new debug application.
            ''' </summary>
            ''' <param name="debugApplication">The new debug application.</param>
            ''' <param name="cookie">An engine-defined cookie.</param>
            Sub AddApplication(debugApplication As IDebugApplication32, ByRef cookie As UInteger)

            ''' <summary>
            '''     Removes a debug application.
            ''' </summary>
            ''' <param name="cookie">The cookie of the debug application to remove.</param>
            Sub RemoveApplication(cookie As UInteger)

            ''' <summary>
            '''     Creates a debug document helper.
            ''' </summary>
            ''' <param name="outerUnknown">The outer unknown.</param>
            ''' <param name="helper">The new debug document helper.</param>
            Sub CreateDebugDocumentHelper(outerUnknown As Object, ByRef helper As IDebugDocumentHelper32)
        End Interface

        ''' <summary>
        '''     IProcessDebugManager64 COM interface.
        ''' </summary>
        <Guid("56b9fC1C-63A9-4CC1-AC21-087D69A17FAB")>
        <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
        Public Interface IProcessDebugManager64
            ''' <summary>
            '''     Creates a new debug application.
            ''' </summary>
            ''' <param name="debugApplication">The new debug application.</param>
            Sub CreateApplication(ByRef debugApplication As IDebugApplication64)

            ''' <summary>
            '''     Gets the default debug application.
            ''' </summary>
            ''' <param name="debugApplication">The default debug application.</param>
            Sub GetDefaultApplication(ByRef debugApplication As IDebugApplication64)

            ''' <summary>
            '''     Adds a new debug application.
            ''' </summary>
            ''' <param name="debugApplication">The new debug application.</param>
            ''' <param name="cookie">An engine-defined cookie.</param>
            Sub AddApplication(debugApplication As IDebugApplication64, ByRef cookie As UInteger)

            ''' <summary>
            '''     Removes a debug application.
            ''' </summary>
            ''' <param name="cookie">The cookie of the debug application to remove.</param>
            Sub RemoveApplication(cookie As UInteger)

            ''' <summary>
            '''     Creates a debug document helper.
            ''' </summary>
            ''' <param name="outerUnknown">The outer unknown.</param>
            ''' <param name="helper">The new debug document helper.</param>
            Sub CreateDebugDocumentHelper(outerUnknown As Object, ByRef helper As IDebugDocumentHelper64)
        End Interface

        ''' <summary>
        '''     IDebugApplication32 COM interface.
        ''' </summary>
        <Guid("51973C32-CB0C-11d0-B5C9-00A0244A0E7A")>
        Public Interface IDebugApplication32
        End Interface

        ''' <summary>
        '''     IDebugApplication64 COM interface.
        ''' </summary>
        <Guid("4dedc754-04c7-4f10-9e60-16a390fe6e62")>
        Public Interface IDebugApplication64
        End Interface

        ''' <summary>
        '''     IDebugDocumentHelper32 COM interface.
        ''' </summary>
        <Guid("51973C26-CB0C-11d0-B5C9-00A0244A0E7A")>
        Public Interface IDebugDocumentHelper32
        End Interface

        ''' <summary>
        '''     IDebugDocumentHelper64 COM interface.
        ''' </summary>
        <Guid("c4c7363c-20fd-47f9-bd82-4855e0150871")>
        Public Interface IDebugDocumentHelper64
        End Interface

        ''' <summary>
        '''     IActiveScriptProfilerCallback COM interface.
        ''' </summary>
        <Guid("740eca23-7d9d-42e5-ba9d-f8b24b1c7a9b")>
        <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
        Public Interface IActiveScriptProfilerCallback
            ''' <summary>
            '''     Called when the profile is started.
            ''' </summary>
            ''' <param name="context">The context provided when profiling was started.</param>
            Sub Initialize(context As UInteger)

            ''' <summary>
            '''     Called when profiling is stopped.
            ''' </summary>
            ''' <param name="reason">The reason code provided when profiling was stopped.</param>
            Sub Shutdown(reason As UInteger)

            ''' <summary>
            '''     Called when a script is compiled.
            ''' </summary>
            ''' <param name="scriptId">The ID of the script.</param>
            ''' <param name="type">The type of the script.</param>
            ''' <param name="debugDocumentContext">The debug document context, if any.</param>
            Sub ScriptCompiled(scriptId As Integer, type As ProfilerScriptType, debugDocumentContext As IntPtr)

            ''' <summary>
            '''     Called when a function is compiled.
            ''' </summary>
            ''' <param name="functionId">The ID of the function.</param>
            ''' <param name="scriptId">The ID of the script.</param>
            ''' <param name="functionName">The name of the function.</param>
            ''' <param name="functionNameHint">The function name hint.</param>
            ''' <param name="debugDocumentContext">The debug document context, if any.</param>
            Sub FunctionCompiled(functionId As Integer, scriptId As Integer, <MarshalAs(UnmanagedType.LPWStr)> functionName As String, <MarshalAs(UnmanagedType.LPWStr)> functionNameHint As String, debugDocumentContext As IntPtr)

            ''' <summary>
            '''     Called when a function is entered.
            ''' </summary>
            ''' <param name="scriptId">The ID of the script.</param>
            ''' <param name="functionId">The ID of the function.</param>
            Sub OnFunctionEnter(scriptId As Integer, functionId As Integer)

            ''' <summary>
            '''     Called when a function is exited.
            ''' </summary>
            ''' <param name="scriptId">The ID of the script.</param>
            ''' <param name="functionId">The ID of the function.</param>
            Sub OnFunctionExit(scriptId As Integer, functionId As Integer)
        End Interface

        ''' <summary>
        '''     IActiveScriptProfilerCallback2 COM interface.
        ''' </summary>
        <Guid("31B7F8AD-A637-409C-B22F-040995B6103D")>
        Public Interface IActiveScriptProfilerCallback2
            Inherits IActiveScriptProfilerCallback
            ''' <summary>
            '''     Called when a function is entered by name.
            ''' </summary>
            ''' <param name="functionName">The name of the function.</param>
            ''' <param name="type">The type of the function.</param>
            Sub OnFunctionEnterByName(functionName As String, type As ProfilerScriptType)

            ''' <summary>
            '''     Called when a function is exited by name.
            ''' </summary>
            ''' <param name="functionName">The name of the function.</param>
            ''' <param name="type">The type of the function.</param>
            Sub OnFunctionExitByName(functionName As String, type As ProfilerScriptType)
        End Interface

        ''' <summary>
        '''     IActiveScriptProfilerHeapEnum COM interface.
        ''' </summary>
        <SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification:="Name defined in COM.")>
        <Guid("32E4694E-0D37-419B-B93D-FA20DED6E8EA")>
        Public Interface IActiveScriptProfilerHeapEnum
        End Interface

        ''' <summary>
        ''' Throws if a native method returns an error code.
        ''' </summary>
        ''' <param name="errorCode">The errorCode.</param>
        Friend Sub ThrowIfError(errorCode As JavaScriptErrorCode)
            If errorCode <> JavaScriptErrorCode.NoError Then
                Select Case errorCode
                    Case JavaScriptErrorCode.InvalidArgument
                        Throw New JavaScriptUsageException(errorCode, "Invalid argument.")

                    Case JavaScriptErrorCode.NullArgument
                        Throw New JavaScriptUsageException(errorCode, "Null argument.")

                    Case JavaScriptErrorCode.NoCurrentContext
                        Throw New JavaScriptUsageException(errorCode, "No current context.")

                    Case JavaScriptErrorCode.InExceptionState
                        Throw New JavaScriptUsageException(errorCode, "Runtime is in exception state.")

                    Case JavaScriptErrorCode.NotImplemented
                        Throw New JavaScriptUsageException(errorCode, "Method is not implemented.")

                    Case JavaScriptErrorCode.WrongThread
                        Throw New JavaScriptUsageException(errorCode, "Runtime is active on another thread.")

                    Case JavaScriptErrorCode.RuntimeInUse
                        Throw New JavaScriptUsageException(errorCode, "Runtime is in use.")

                    Case JavaScriptErrorCode.BadSerializedScript
                        Throw New JavaScriptUsageException(errorCode, "Bad serialized script.")

                    Case JavaScriptErrorCode.InDisabledState
                        Throw New JavaScriptUsageException(errorCode, "Runtime is disabled.")

                    Case JavaScriptErrorCode.CannotDisableExecution
                        Throw New JavaScriptUsageException(errorCode, "Cannot disable execution.")

                    Case JavaScriptErrorCode.AlreadyDebuggingContext
                        Throw New JavaScriptUsageException(errorCode, "Context is already in debug mode.")

                    Case JavaScriptErrorCode.HeapEnumInProgress
                        Throw New JavaScriptUsageException(errorCode, "Heap enumeration is in progress.")

                    Case JavaScriptErrorCode.ArgumentNotObject
                        Throw New JavaScriptUsageException(errorCode, "Argument is not an object.")

                    Case JavaScriptErrorCode.InProfileCallback
                        Throw New JavaScriptUsageException(errorCode, "In a profile callback.")

                    Case JavaScriptErrorCode.InThreadServiceCallback
                        Throw New JavaScriptUsageException(errorCode, "In a thread service callback.")

                    Case JavaScriptErrorCode.CannotSerializeDebugScript
                        Throw New JavaScriptUsageException(errorCode, "Cannot serialize a debug script.")

                    Case JavaScriptErrorCode.AlreadyProfilingContext
                        Throw New JavaScriptUsageException(errorCode, "Already profiling this context.")

                    Case JavaScriptErrorCode.IdleNotEnabled
                        Throw New JavaScriptUsageException(errorCode, "Idle is not enabled.")

                    Case JavaScriptErrorCode.OutOfMemory
                        Throw New JavaScriptEngineException(errorCode, "Out of memory.")

                    Case JavaScriptErrorCode.ScriptException
                        Dim errorObject As JavaScriptValue
                        Dim innerError As JavaScriptErrorCode = JsGetAndClearException(errorObject)

                        If innerError <> JavaScriptErrorCode.NoError Then
                            Throw New JavaScriptFatalException(innerError)
                        End If

                        Throw New JavaScriptScriptException(errorCode, errorObject, "Script threw an exception.")

                    Case JavaScriptErrorCode.ScriptCompile
                        Dim errorObject As JavaScriptValue
                        Dim innerError As JavaScriptErrorCode = JsGetAndClearException(errorObject)

                        If innerError <> JavaScriptErrorCode.NoError Then
                            Throw New JavaScriptFatalException(innerError)
                        End If

                        Throw New JavaScriptScriptException(errorCode, errorObject, "Compile error.")

                    Case JavaScriptErrorCode.ScriptTerminated
                        Throw New JavaScriptScriptException(errorCode, JavaScriptValue.Invalid, "Script was terminated.")

                    Case JavaScriptErrorCode.ScriptEvalDisabled
                        Throw New JavaScriptScriptException(errorCode, JavaScriptValue.Invalid, "Eval of strings is disabled in this runtime.")

                    Case JavaScriptErrorCode.Fatal
                        Throw New JavaScriptFatalException(errorCode)

                    Case Else
                        Throw New JavaScriptFatalException(errorCode)
                End Select
            End If
        End Sub

        Friend Declare Unicode Function JsCreateRuntime Lib "jscript9.dll" (attributes As JavaScriptRuntimeAttributes, runtimeVersion As JavaScriptRuntimeVersion, threadService As JavaScriptThreadServiceCallback, ByRef runtime As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCollectGarbage Lib "jscript9.dll" (handle As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDisposeRuntime Lib "jscript9.dll" (handle As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetRuntimeMemoryUsage Lib "jscript9.dll" (runtime As JavaScriptRuntime, ByRef memoryUsage As UIntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetRuntimeMemoryLimit Lib "jscript9.dll" (runtime As JavaScriptRuntime, ByRef memoryLimit As UIntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetRuntimeMemoryLimit Lib "jscript9.dll" (runtime As JavaScriptRuntime, memoryLimit As UIntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetRuntimeMemoryAllocationCallback Lib "jscript9.dll" (runtime As JavaScriptRuntime, callbackState As IntPtr, allocationCallback As JavaScriptMemoryAllocationCallback) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetRuntimeBeforeCollectCallback Lib "jscript9.dll" (runtime As JavaScriptRuntime, callbackState As IntPtr, beforeCollectCallback As JavaScriptBeforeCollectCallback) As JavaScriptErrorCode

        Friend Declare Unicode Function JsContextAddRef Lib "jscript9.dll" Alias "JsAddRef" (reference As JavaScriptContext, ByRef count As UInteger) As JavaScriptErrorCode

        Friend Declare Unicode Function JsAddRef Lib "jscript9.dll" (reference As JavaScriptValue, ByRef count As UInteger) As JavaScriptErrorCode
        
        Friend Declare Unicode Function JsContextRelease Lib "jscript9.dll" Alias "JsRelease" (reference As JavaScriptContext, ByRef count As UInteger) As JavaScriptErrorCode

        Friend Declare Unicode Function JsRelease Lib "jscript9.dll" (reference As JavaScriptValue, ByRef count As UInteger) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateContext Lib "jscript9.dll" (runtime As JavaScriptRuntime, debugSite As IDebugApplication64, ByRef newContext As JavaScriptContext) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateContext Lib "jscript9.dll" (runtime As JavaScriptRuntime, debugSite As IDebugApplication32, ByRef newContext As JavaScriptContext) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetCurrentContext Lib "jscript9.dll" (ByRef currentContext As JavaScriptContext) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetCurrentContext Lib "jscript9.dll" (context As JavaScriptContext) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetRuntime Lib "jscript9.dll" (context As JavaScriptContext, ByRef runtime As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStartDebugging Lib "jscript9.dll" (debugApplication As IDebugApplication64) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStartDebugging Lib "jscript9.dll" (debugApplication As IDebugApplication32) As JavaScriptErrorCode

        Friend Declare Unicode Function JsIdle Lib "jscript9.dll" (ByRef nextIdleTick As UInteger) As JavaScriptErrorCode

        Friend Declare Unicode Function JsParseScript Lib "jscript9.dll" (script As String, sourceContext As JavaScriptSourceContext, sourceUrl As String, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsRunScript Lib "jscript9.dll" (script As String, sourceContext As JavaScriptSourceContext, sourceUrl As String, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSerializeScript Lib "jscript9.dll" (script As String, buffer As Byte(), ByRef bufferSize As ULong) As JavaScriptErrorCode

        Friend Declare Unicode Function JsParseSerializedScript Lib "jscript9.dll" (script As String, buffer As Byte(), sourceContext As JavaScriptSourceContext, sourceUrl As String, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsRunSerializedScript Lib "jscript9.dll" (script As String, buffer As Byte(), sourceContext As JavaScriptSourceContext, sourceUrl As String, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetPropertyIdFromName Lib "jscript9.dll" (name As String, ByRef propertyId As JavaScriptPropertyId) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetPropertyNameFromId Lib "jscript9.dll" (propertyId As JavaScriptPropertyId, ByRef name As String) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetUndefinedValue Lib "jscript9.dll" (ByRef undefinedValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetNullValue Lib "jscript9.dll" (ByRef nullValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetTrueValue Lib "jscript9.dll" (ByRef trueValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetFalseValue Lib "jscript9.dll" (ByRef falseValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsBoolToBoolean Lib "jscript9.dll" (value As Boolean, ByRef booleanValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsBooleanToBool Lib "jscript9.dll" (booleanValue As JavaScriptValue, ByRef boolValue As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsConvertValueToBoolean Lib "jscript9.dll" (value As JavaScriptValue, ByRef booleanValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetValueType Lib "jscript9.dll" (value As JavaScriptValue, ByRef type As JavaScriptValueType) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDoubleToNumber Lib "jscript9.dll" (doubleValue As Double, ByRef value As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsIntToNumber Lib "jscript9.dll" (intValue As Integer, ByRef value As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsNumberToDouble Lib "jscript9.dll" (value As JavaScriptValue, ByRef doubleValue As Double) As JavaScriptErrorCode

        Friend Declare Unicode Function JsConvertValueToNumber Lib "jscript9.dll" (value As JavaScriptValue, ByRef numberValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetStringLength Lib "jscript9.dll" (sringValue As JavaScriptValue, ByRef length As Integer) As JavaScriptErrorCode

        Friend Declare Unicode Function JsPointerToString Lib "jscript9.dll" (value As String, stringLength As UIntPtr, ByRef stringValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStringToPointer Lib "jscript9.dll" (value As JavaScriptValue, ByRef stringValue As IntPtr, ByRef stringLength As UIntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsConvertValueToString Lib "jscript9.dll" (value As JavaScriptValue, ByRef stringValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsVariantToValue Lib "jscript9.dll" (<MarshalAs(UnmanagedType.Struct)> ByRef var As Object, ByRef value As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsValueToVariant Lib "jscript9.dll" (obj As JavaScriptValue, <MarshalAs(UnmanagedType.Struct)> ByRef var As Object) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetGlobalObject Lib "jscript9.dll" (ByRef globalObject As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateObject Lib "jscript9.dll" (ByRef obj As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateExternalObject Lib "jscript9.dll" (data As IntPtr, finalizeCallback As JavaScriptObjectFinalizeCallback, ByRef obj As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsConvertValueToObject Lib "jscript9.dll" (value As JavaScriptValue, ByRef obj As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetPrototype Lib "jscript9.dll" (obj As JavaScriptValue, ByRef prototypeObject As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetPrototype Lib "jscript9.dll" (obj As JavaScriptValue, prototypeObject As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetExtensionAllowed Lib "jscript9.dll" (obj As JavaScriptValue, ByRef value As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsPreventExtension Lib "jscript9.dll" (obj As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetProperty Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, ByRef value As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetOwnPropertyDescriptor Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, ByRef propertyDescriptor As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetOwnPropertyNames Lib "jscript9.dll" (obj As JavaScriptValue, ByRef propertyNames As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetProperty Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, value As JavaScriptValue, useStrictRules As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsHasProperty Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, ByRef hasProperty As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDeleteProperty Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, useStrictRules As Boolean, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDefineProperty Lib "jscript9.dll" (obj As JavaScriptValue, propertyId As JavaScriptPropertyId, propertyDescriptor As JavaScriptValue, ByRef result As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsHasIndexedProperty Lib "jscript9.dll" (obj As JavaScriptValue, index As JavaScriptValue, ByRef result As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetIndexedProperty Lib "jscript9.dll" (obj As JavaScriptValue, index As JavaScriptValue, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetIndexedProperty Lib "jscript9.dll" (obj As JavaScriptValue, index As JavaScriptValue, value As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDeleteIndexedProperty Lib "jscript9.dll" (obj As JavaScriptValue, index As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsEquals Lib "jscript9.dll" (obj1 As JavaScriptValue, obj2 As JavaScriptValue, ByRef result As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStrictEquals Lib "jscript9.dll" (obj1 As JavaScriptValue, obj2 As JavaScriptValue, ByRef result As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsHasExternalData Lib "jscript9.dll" (obj As JavaScriptValue, ByRef value As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetExternalData Lib "jscript9.dll" (obj As JavaScriptValue, ByRef externalData As IntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetExternalData Lib "jscript9.dll" (obj As JavaScriptValue, externalData As IntPtr) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateArray Lib "jscript9.dll" (length As UInteger, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCallFunction Lib "jscript9.dll" (func As JavaScriptValue, arguments As JavaScriptValue(), argumentCount As UShort, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsConstructObject Lib "jscript9.dll" (func As JavaScriptValue, arguments As JavaScriptValue(), argumentCount As UShort, ByRef result As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateFunction Lib "jscript9.dll" (nativeFunction As JavaScriptNativeFunction, ByVal callbackData As IntPtr, ByRef func As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateRangeError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateReferenceError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateSyntaxError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateTypeError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsCreateURIError Lib "jscript9.dll" (message As JavaScriptValue, ByRef errorValue As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsHasException Lib "jscript9.dll" (ByRef hasException As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsGetAndClearException Lib "jscript9.dll" (ByRef exception As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsSetException Lib "jscript9.dll" (exception As JavaScriptValue) As JavaScriptErrorCode

        Friend Declare Unicode Function JsDisableRuntimeExecution Lib "jscript9.dll" (runtime As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsEnableRuntimeExecution Lib "jscript9.dll" (runtime As JavaScriptRuntime) As JavaScriptErrorCode

        Friend Declare Unicode Function JsIsRuntimeExecutionDisabled Lib "jscript9.dll" (runtime As JavaScriptRuntime, ByRef isDisabled As Boolean) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStartProfiling Lib "jscript9.dll" (callback As IActiveScriptProfilerCallback, eventMask As ProfilerEventMask, context As Integer) As JavaScriptErrorCode

        Friend Declare Unicode Function JsStopProfiling Lib "jscript9.dll" (reason As Integer) As JavaScriptErrorCode

        Friend Declare Unicode Function JsEnumerateHeap Lib "jscript9.dll" (ByRef enumerator As IActiveScriptProfilerHeapEnum) As JavaScriptErrorCode

        Friend Declare Unicode Function JsIsEnumeratingHeap Lib "jscript9.dll" (ByRef isEnumeratingHeap As Boolean) As JavaScriptErrorCode

        ''' <summary>
        '''     ProcessDebugManager COM interface.
        ''' </summary>
        <ComImport>
        <Guid("78A51822-51F4-11D0-8F20-00805F2CD064")>
        Public Class ProcessDebugManager
        End Class
    End Module
End Namespace
