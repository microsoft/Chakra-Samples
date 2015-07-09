namespace ChakraHost.Hosting
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     An error code returned from a Chakra hosting API.
    /// </summary>
    public enum JavaScriptErrorCode : uint
    {
        /// <summary>
        ///     Success error code.
        /// </summary>
        NoError = 0,

        /// <summary>
        ///     Category of errors that relates to incorrect usage of the API itself.
        /// </summary>
        CategoryUsage = 0x10000,
        /// <summary>
        ///     An argument to a hosting API was invalid.
        /// </summary>
        InvalidArgument,
        /// <summary>
        ///     An argument to a hosting API was null in a context where null is not allowed.
        /// </summary>
        NullArgument,
        /// <summary>
        ///     The hosting API requires that a context be current, but there is no current context.
        /// </summary>
        NoCurrentContext,
        /// <summary>
        ///     The engine is in an exception state and no APIs can be called until the exception is
        ///     cleared.
        /// </summary>
        InExceptionState,
        /// <summary>
        ///     A hosting API is not yet implemented.
        /// </summary>
        NotImplemented,
        /// <summary>
        ///     A hosting API was called on the wrong thread.
        /// </summary>
        WrongThread,
        /// <summary>
        ///     A runtime that is still in use cannot be disposed.
        /// </summary>
        RuntimeInUse,
        /// <summary>
        ///     A bad serialized script was used, or the serialized script was serialized by a
        ///     different version of the Chakra engine.
        /// </summary>
        BadSerializedScript,
        /// <summary>
        ///     The runtime is in a disabled state.
        /// </summary>
        InDisabledState,
        /// <summary>
        ///     Runtime does not support reliable script interruption.
        /// </summary>
        CannotDisableExecution,
        /// <summary>
        ///     A heap enumeration is currently underway in the script context.
        /// </summary>
        HeapEnumInProgress,
        /// <summary>
        ///     A hosting API that operates on object values was called with a non-object value.
        /// </summary>
        ArgumentNotObject,
        /// <summary>
        ///     A script context is in the middle of a profile callback.
        /// </summary>
        InProfileCallback,
        /// <summary>
        ///     A thread service callback is currently underway.
        /// </summary>
        InThreadServiceCallback,
        /// <summary>
        ///     Scripts cannot be serialized in debug contexts.
        /// </summary>
        CannotSerializeDebugScript,
        /// <summary>
        ///     The context cannot be put into a debug state because it is already in a debug state.
        /// </summary>
        AlreadyDebuggingContext,
        /// <summary>
        ///     The context cannot start profiling because it is already profiling.
        /// </summary>
        AlreadyProfilingContext,
        /// <summary>
        ///     Idle notification given when the host did not enable idle processing.
        /// </summary>
        IdleNotEnabled,
        /// <summary>
        ///     The context did not accept the enqueue callback.
        /// </summary>
        CannotSetProjectionEnqueueCallback,
        /// <summary>
        ///     Failed to start WinRT projection.
        /// </summary>
        CannotStartProjection,
        /// <summary>
        ///     The operation is not supported in an object before collect callback.
        /// </summary>
        InObjectBeforeCollectCallback,
        /// <summary>
        ///     Object cannot be unwrapped to IInspectable pointer.
        /// </summary>
        ObjectNotInspectable,
        /// <summary>
        ///     A hosting API that operates on symbol property ids but was called with a non-symbol property id.
        ///     The error code is returned by JsGetSymbolFromPropertyId if the function is called with non-symbol property id.
        /// </summary>
        PropertyNotSymbol,
        /// <summary>
        ///     A hosting API that operates on string property ids but was called with a non-string property id.
        ///     The error code is returned by existing JsGetPropertyNamefromId if the function is called with non-string property id.
        /// </summary>
        PropertyNotString,

        /// <summary>
        ///     Category of errors that relates to errors occurring within the engine itself.
        /// </summary>
        CategoryEngine = 0x20000,
        /// <summary>
        ///     The Chakra engine has run out of memory.
        /// </summary>
        OutOfMemory,

        /// <summary>
        ///     Category of errors that relates to errors in a script.
        /// </summary>
        CategoryScript = 0x30000,
        /// <summary>
        ///     A JavaScript exception occurred while running a script.
        /// </summary>
        ScriptException,
        /// <summary>
        ///     JavaScript failed to compile.
        /// </summary>
        ScriptCompile,
        /// <summary>
        ///     A script was terminated due to a request to suspend a runtime.
        /// </summary>
        ScriptTerminated,
        /// <summary>
        ///     A script was terminated because it tried to use <c>eval</c> or <c>function</c> and eval
        ///     was disabled.
        /// </summary>
        ScriptEvalDisabled,

        /// <summary>
        ///     Category of errors that are fatal and signify failure of the engine.
        /// </summary>
        CategoryFatal = 0x40000,
        /// <summary>
        ///     A fatal error in the engine has occurred.
        /// </summary>
        Fatal,
    }

    /// <summary>
    ///     Allocation callback event type.
    /// </summary>
    public enum JavaScriptMemoryEventType
    {
        /// <summary>
        ///     Indicates a request for memory allocation.
        /// </summary>
        Allocate = 0,

        /// <summary>
        ///     Indicates a memory freeing event.
        /// </summary>
        Free = 1,

        /// <summary>
        ///     Indicates a failed allocation event.
        /// </summary>
        Failure = 2
    }

    /// <summary>
    ///     Attributes of a runtime.
    /// </summary>
    [Flags]
    public enum JavaScriptRuntimeAttributes
    {
        /// <summary>
        ///     No special attributes.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        ///     The runtime will not do any work (such as garbage collection) on background threads.
        /// </summary>
        DisableBackgroundWork = 0x00000001,

        /// <summary>
        ///     The runtime should support reliable script interruption. This increases the number of
        ///     places where the runtime will check for a script interrupt request at the cost of a
        ///     small amount of runtime performance.
        /// </summary>
        AllowScriptInterrupt = 0x00000002,

        /// <summary>
        ///     Host will call Idle, so enable idle processing. Otherwise, the runtime will manage
        ///     memory slightly more aggressively.
        /// </summary>
        EnableIdleProcessing = 0x00000004,

        /// <summary>
        ///     Runtime will not generate native code.
        /// </summary>
        DisableNativeCodeGeneration = 0x00000008,

        /// <summary>
        ///     Using Eval or Function constructor will throw an exception.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Eval is a valid function name.")]
        DisableEval = 0x00000010,
    }

    /// <summary>
    ///     The JavaScript type of a JavaScriptValue.
    /// </summary>
    public enum JavaScriptValueType
    {
        /// <summary>
        ///     The value is the <c>undefined</c> value.
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     The value is the <c>null</c> value.
        /// </summary>
        Null = 1,

        /// <summary>
        ///     The value is a JavaScript number value.
        /// </summary>
        Number = 2,

        /// <summary>
        ///     The value is a JavaScript string value.
        /// </summary>
        String = 3,

        /// <summary>
        ///     The value is a JavaScript Boolean value.
        /// </summary>
        Boolean = 4,

        /// <summary>
        ///     The value is a JavaScript object value.
        /// </summary>
        Object = 5,

        /// <summary>
        ///     The value is a JavaScript function object value.
        /// </summary>
        Function = 6,

        /// <summary>
        ///     The value is a JavaScript error object value.
        /// </summary>
        Error = 7,

        /// <summary>
        ///     The value is a JavaScript array object value.
        /// </summary>
        Array = 8,

        /// <summary>
        ///     The value is a JavaScript array object value.
        /// </summary>
        Symbol = 9,

        /// <summary>
        ///     The value is a JavaScript ArrayBuffer object value.
        /// </summary>
        ArrayBuffer = 10,

        /// <summary>
        ///     The value is a JavaScript typed array object value.
        /// </summary>
        TypedArray = 11,

        /// <summary>
        ///     The value is a JavaScript DataView object value.
        /// </summary>
        DataView = 12,
    }

    /// <summary>
    ///     The type of a typed JavaScript array.
    /// </summary>
    public enum  JavaScriptTypedArrayType
    {
        /// <summary>
        ///     An int8 array.
        /// </summary>
        Int8,
        /// <summary>
        ///     An uint8 array.
        /// </summary>
        Uint8,
        /// <summary>
        ///     An uint8 clamped array.
        /// </summary>
        Uint8Clamped,
        /// <summary>
        ///     An int16 array.
        /// </summary>
        Int16,
        /// <summary>
        ///     An uint16 array.
        /// </summary>
        Uint16,
        /// <summary>
        ///     An int32 array.
        /// </summary>
        TypeInt32,
        /// <summary>
        ///     An uint32 array.
        /// </summary>
        Uint32,
        /// <summary>
        ///     A float32 array.
        /// </summary>
        Float32,
        /// <summary>
        ///     A float64 array.
        /// </summary>
        Float64
    };

    /// <summary>
    ///     Type enumeration of a JavaScript property
    /// </summary>
    public enum JavaSciptPropertyIdType
    {
        /// <summary>
        ///     Type enumeration of a JavaScript string property
        /// </summary>
        JavaScriptPropertyIdTypeString,
        /// <summary>
        ///     Type enumeration of a JavaScript symbol property
        /// </summary>
        JavaScriptPropertyIdTypeSymbol
    };
}
