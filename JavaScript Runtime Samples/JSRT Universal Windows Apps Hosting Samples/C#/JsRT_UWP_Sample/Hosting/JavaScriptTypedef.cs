namespace ChakraHost.Hosting
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     A Chakra runtime.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Each Chakra runtime has its own independent execution engine, JIT compiler, and garbage 
    ///     collected heap. As such, each runtime is completely isolated from other runtimes.
    ///     </para>
    ///     <para>
    ///     Runtimes can be used on any thread, but only one thread can call into a runtime at any 
    ///     time.
    ///     </para>
    ///     <para>
    ///     NOTE: A JavaScriptRuntime, unlike other objects in the Chakra hosting API, is not 
    ///     garbage collected since it contains the garbage collected heap itself. A runtime will 
    ///     continue to exist until Dispose is called.
    ///     </para>
    /// </remarks>
    public struct JavaScriptRuntime : IDisposable
    {
        /// <summary>
        /// The handle.
        /// </summary>
        private IntPtr handle;

        /// <summary>
        ///     Gets a value indicating whether the runtime is valid.
        /// </summary>
        public bool IsValid
        {
            get { return handle != IntPtr.Zero; }
        }

        /// <summary>
        ///     Disposes a runtime.
        /// </summary>
        /// <remarks>
        ///     Once a runtime has been disposed, all resources owned by it are invalid and cannot be used.
        ///     If the runtime is active (i.e. it is set to be current on a particular thread), it cannot 
        ///     be disposed.
        /// </remarks>
        public void Dispose()
        {
            if (IsValid)
            {
                Native.JsDisposeRuntime(this);
            }

            handle = IntPtr.Zero;
        }

    }

    /// <summary>
    ///     A script context.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Each script context contains its own global object, distinct from the global object in 
    ///     other script contexts.
    ///     </para>
    ///     <para>
    ///     Many Chakra hosting APIs require an "active" script context, which can be set using 
    ///     Current. Chakra hosting APIs that require a current context to be set will note 
    ///     that explicitly in their documentation.
    ///     </para>
    /// </remarks>
    public struct JavaScriptContext
    {
        /// <summary>
        ///     The reference.
        /// </summary>
        private readonly IntPtr reference;
    }
    
    /// <summary>
    ///     A JavaScript value.
    /// </summary>
    /// <remarks>
    ///     A JavaScript value is one of the following types of values: Undefined, Null, Boolean, 
    ///     String, Number, or Object.
    /// </remarks>
    public struct JavaScriptValue
    {
        /// <summary>
        /// The reference.
        /// </summary>
        private readonly IntPtr reference;

        public JavaScriptValue(IntPtr reference) {
            this.reference = reference;
        }

        public IntPtr toIntPtr() {
            return reference;
        }
    }

    /// <summary>
    ///     A cookie that identifies a script for debugging purposes.
    /// </summary>
    public struct JavaScriptSourceContext : IEquatable<JavaScriptSourceContext>
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly IntPtr context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptSourceContext"/> struct.
        /// </summary>
        /// <param name="context">The context.</param>
        private JavaScriptSourceContext(IntPtr context)
        {
            this.context = context;
        }

        /// <summary>
        ///     Increments the value of the source context.
        /// </summary>
        /// <param name="context">The source context to increment.</param>
        /// <returns>A new source context that reflects the incrementing of the context.</returns>
        public static JavaScriptSourceContext operator ++(JavaScriptSourceContext context)
        {
            return FromIntPtr(context.context + 1);
        }

        /// <summary>
        ///     Creates a new source context. 
        /// </summary>
        /// <param name="cookie">
        ///     The cookie for the source context.
        /// </param>
        /// <returns>The new source context.</returns>
        public static JavaScriptSourceContext FromIntPtr(IntPtr cookie)
        {
            return new JavaScriptSourceContext(cookie);
        }

        /// <summary>
        ///     Checks for equality between source contexts.
        /// </summary>
        /// <param name="other">The other source context to compare.</param>
        /// <returns>Whether the two source contexts are the same.</returns>
        public bool Equals(JavaScriptSourceContext other)
        {
            return context == other.context;
        }

        /// <summary>
        ///     Checks for equality between source contexts.
        /// </summary>
        /// <param name="obj">The other source context to compare.</param>
        /// <returns>Whether the two source contexts are the same.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is JavaScriptSourceContext && Equals((JavaScriptSourceContext)obj);
        }

        /// <summary>
        ///     The hash code.
        /// </summary>
        /// <returns>The hash code of the source context.</returns>
        public override int GetHashCode()
        {
            return context.ToInt32();
        }
    }

    /// <summary>
    ///     A property identifier.
    /// </summary>
    /// <remarks>
    ///     Property identifiers are used to refer to properties of JavaScript objects instead of using
    ///     strings.
    /// </remarks>
    public struct JavaScriptPropertyId : IEquatable<JavaScriptPropertyId>
    {
        /// <summary>
        /// The id.
        /// </summary>
        private readonly IntPtr id;

        /// <summary>
        ///     Checks for equality between property IDs.
        /// </summary>
        /// <param name="other">The other property ID to compare.</param>
        /// <returns>Whether the two property IDs are the same.</returns>
        public bool Equals(JavaScriptPropertyId other)
        {
            return id == other.id;
        }

        /// <summary>
        ///     Checks for equality between property IDs.
        /// </summary>
        /// <param name="obj">The other property ID to compare.</param>
        /// <returns>Whether the two property IDs are the same.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is JavaScriptPropertyId && Equals((JavaScriptPropertyId)obj);
        }

        /// <summary>
        ///     The hash code.
        /// </summary>
        /// <returns>The hash code of the property ID.</returns>
        public override int GetHashCode()
        {
            return id.ToInt32();
        }
    }

    /// <summary>
    ///     The context passed into application callback, JsProjectionEnqueueCallback, from Jsrt and
    ///     then passed back to Jsrt in the provided callback, JsProjectionCallback, by the application
    ///     on the correct thread.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    public struct JavaScriptProjectionCallbackContext
    {
        /// <summary>
        /// The reference.
        /// </summary>
        private readonly IntPtr reference;
    }

    /// <summary>
    ///     A background work item callback.
    /// </summary>
    /// <remarks>
    ///     This is passed to the host's thread service (if provided) to allow the host to 
    ///     invoke the work item callback on the background thread of its choice.
    /// </remarks>
    /// <param name="callbackData">Data argument passed to the thread service.</param>
    public delegate void JavaScriptBackgroundWorkItemCallback(IntPtr callbackData);

    /// <summary>
    ///     A callback called before collection.
    /// </summary>
    /// <param name="callbackState">The state passed to SetBeforeCollectCallback.</param>
    public delegate void JavaScriptBeforeCollectCallback(IntPtr callbackState);

    /// <summary>
    ///     User implemented callback routine for memory allocation events
    /// </summary>
    /// <param name="callbackState">The state passed to SetRuntimeMemoryAllocationCallback.</param>
    /// <param name="allocationEvent">The type of type allocation event.</param>
    /// <param name="allocationSize">The size of the allocation.</param>
    /// <returns>
    ///     For the Allocate event, returning true allows the runtime to continue with 
    ///     allocation. Returning false indicates the allocation request is rejected. The return value
    ///     is ignored for other allocation events.
    /// </returns>
    public delegate bool JavaScriptMemoryAllocationCallback(IntPtr callbackState, JavaScriptMemoryEventType allocationEvent, UIntPtr allocationSize);

    /// <summary>
    ///     A finalization callback.
    /// </summary>
    /// <param name="data">
    ///     The external data that was passed in when creating the object being finalized.
    /// </param>
    public delegate void JavaScriptObjectFinalizeCallback(IntPtr data);

    /// <summary>
    ///     A thread service callback.
    /// </summary>
    /// <remarks>
    ///     The host can specify a background thread service when creating a runtime. If 
    ///     specified, then background work items will be passed to the host using this callback. The
    ///     host is expected to either begin executing the background work item immediately and return
    ///     true or return false and the runtime will handle the work item in-thread.
    /// </remarks>
    /// <param name="callbackFunction">The callback for the background work item.</param>
    /// <param name="callbackData">The data argument to be passed to the callback.</param>
    /// <returns>Whether the thread service will execute the callback.</returns>
    public delegate bool JavaScriptThreadServiceCallback(JavaScriptBackgroundWorkItemCallback callbackFunction, IntPtr callbackData);

    /// <summary>
    ///     A function callback.
    /// </summary>
    /// <param name="callee">
    ///     A <c>Function</c> object that represents the function being invoked.
    /// </param>
    /// <param name="isConstructCall">Indicates whether this is a regular call or a 'new' call.</param>
    /// <param name="arguments">The arguments to the call.</param>
    /// <param name="argumentCount">The number of arguments.</param>
    /// <param name="callbackData">Callback data, if any.</param>
    /// <returns>The result of the call, if any.</returns>
    public delegate JavaScriptValue JavaScriptNativeFunction(JavaScriptValue callee, [MarshalAs(UnmanagedType.U1)] bool isConstructCall, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData);

    /// <summary>
    ///     A callback called before collecting an object.
    /// </summary>
    /// <remarks>
    ///     Use <c>JsSetObjectBeforeCollectCallback</c> to register this callback.
    /// </remarks>
    /// <param name="ref">The object to be collected.</param>
    /// <param name="callbackState">The state passed to <c>JsSetObjectBeforeCollectCallback</c>.</param>
    public delegate void JavaScriptObjectBeforeCollectCallback(JavaScriptValue reference, IntPtr callbackState);

    /// <summary>
    ///     The Jsrt callback which should be called with the context passed to JsProjectionEnqueueCallback on
    ///     the correct thread.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    /// <param name="jsContext">The context originally received by a call to JsProjectionEnqueueCallback.</param>
    public delegate void JavaScriptProjectionCallback(JavaScriptProjectionCallbackContext jsContext);

    /// <summary>
    ///     The application callback which is called by Jsrt when a projection API is completed on
    ///     a different thread than the original.
    /// </summary>
    /// <remarks>
    ///     Requires calling JsSetProjectionEnqueueCallback to receive callbacks.
    /// </remarks>
    /// <param name="jsCallbck">The callback to be invoked on the original thread.</param>
    /// <param name="callbackState">The applications context.</param>
    /// <param name="jsContext">The Jsrt context that must be passed into jsCallback.</param>
    public delegate void JavaScriptProjectionEnqueueCallback(JavaScriptProjectionCallback jsCallback, JavaScriptProjectionCallbackContext jsContext, IntPtr callbackState);

    /// <summary>
    ///     A promise continuation callback.
    /// </summary>
    /// <remarks>
    ///     The host can specify a promise continuation callback in <c>JsSetPromiseContinuationCallback</c>. If
    ///     a script creates a task to be run later, then the promise continuation callback will be called with
    ///     the task and the task should be put in a FIFO queue, to be run when the current script is
    ///     done executing.
    /// </remarks>
    /// <param name="task">The task, represented as a JavaScript function.</param>
    /// <param name="callbackState">The data argument to be passed to the callback.</param>
    public delegate void JavaScriptPromiseContinuationCallback(JavaScriptValue task, IntPtr callbackState);
}
