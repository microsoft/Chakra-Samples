Namespace Hosting
    ''' <summary>
    '''     A script context.
    ''' </summary>
    ''' <remarks>
    '''     <para>
    '''     Each script context contains its own global object, distinct from the global object in 
    '''     other script contexts.
    '''     </para>
    '''     <para>
    '''     Many Chakra hosting APIs require an "active" script context, which can be set using 
    '''     Current. Chakra hosting APIs that require a current context to be set will note 
    '''     that explicitly in their documentation.
    '''     </para>
    ''' </remarks>
    Public Structure JavaScriptContext
        ''' <summary>
        '''     The reference.
        ''' </summary>
        Private ReadOnly reference As IntPtr

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptContext"/> struct. 
        ''' </summary>
        ''' <param name="reference">The reference.</param>
        Friend Sub New(reference As IntPtr)
            Me.reference = reference
        End Sub

        ''' <summary>
        '''     Gets an invalid context.
        ''' </summary>
        Public Shared ReadOnly Property Invalid() As JavaScriptContext
            Get
                Return New JavaScriptContext(IntPtr.Zero)
            End Get
        End Property

        ''' <summary>
        '''     Gets or sets the current script context on the thread.
        ''' </summary>
        Public Shared Property Current() As JavaScriptContext
            Get
                Dim reference As JavaScriptContext
                ThrowIfError(JsGetCurrentContext(reference))
                Return reference
            End Get

            Set(value As JavaScriptContext)
                ThrowIfError(JsSetCurrentContext(value))
            End Set
        End Property

        ''' <summary>
        '''     Gets a value indicating whether the runtime of the current context is in an exception state.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     If a call into the runtime results in an exception (either as the result of running a 
        '''     script or due to something like a conversion failure), the runtime is placed into an 
        '''     "exception state." All calls into any context created by the runtime (except for the 
        '''     exception APIs) will fail with <c>InExceptionState</c> until the exception is 
        '''     cleared.
        '''     </para>
        '''     <para>
        '''     If the runtime of the current context is in the exception state when a callback returns 
        '''     into the engine, the engine will automatically rethrow the exception.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        Public Shared ReadOnly Property HasException() As Boolean
            Get
                Dim hasException__1 As Boolean
                ThrowIfError(JsHasException(hasException__1))
                Return hasException__1
            End Get
        End Property

        ''' <summary>
        '''     Gets a value indicating whether the heap of the current context is being enumerated.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property IsEnumeratingHeap() As Boolean
            Get
                Dim isEnumerating As Boolean
                ThrowIfError(JsIsEnumeratingHeap(isEnumerating))
                Return isEnumerating
            End Get
        End Property

        ''' <summary>
        '''     Gets the runtime that the context belongs to.
        ''' </summary>
        Public ReadOnly Property Runtime() As JavaScriptRuntime
            Get
                Dim handle As JavaScriptRuntime
                ThrowIfError(JsGetRuntime(Me, handle))
                Return handle
            End Get
        End Property

        ''' <summary>
        '''     Gets a value indicating whether the context is a valid context or not.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return reference <> IntPtr.Zero
            End Get
        End Property

        ''' <summary>
        '''     Tells the runtime to do any idle processing it need to do.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     If idle processing has been enabled for the current runtime, calling <c>Idle</c> will 
        '''     inform the current runtime that the host is idle and that the runtime can perform 
        '''     memory cleanup tasks.
        '''     </para>
        '''     <para>
        '''     <c>Idle</c> will also return the number of system ticks until there will be more idle work
        '''     for the runtime to do. Calling <c>Idle</c> before this number of ticks has passed will do
        '''     no work.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>
        '''     The next system tick when there will be more idle work to do. Returns the 
        '''     maximum number of ticks if there no upcoming idle work to do.
        ''' </returns>
        Public Shared Function Idle() As UInteger
            Dim ticks As UInteger
            ThrowIfError(JsIdle(ticks))
            Return ticks
        End Function

        ''' <summary>
        '''     Parses a script and returns a <c>Function</c> representing the script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to parse.</param>
        ''' <param name="sourceContext">
        '''     A cookie identifying the script that can be used by script contexts that have debugging enabled.
        ''' </param>
        ''' <param name="sourceName">The location the script came from.</param>
        ''' <returns>A <c>Function</c> representing the script code.</returns>
        Public Shared Function ParseScript(script As String, sourceContext As JavaScriptSourceContext, sourceName As String) As JavaScriptValue
            Dim result As JavaScriptValue
            ThrowIfError(JsParseScript(script, sourceContext, sourceName, result))
            Return result
        End Function

        ''' <summary>
        '''     Parses a serialized script and returns a <c>Function</c> representing the script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to parse.</param>
        ''' <param name="buffer">The serialized script.</param>
        ''' <param name="sourceContext">
        '''     A cookie identifying the script that can be used by script contexts that have debugging enabled.
        ''' </param>
        ''' <param name="sourceName">The location the script came from.</param>
        ''' <returns>A <c>Function</c> representing the script code.</returns>
        Public Shared Function ParseScript(script As String, buffer As Byte(), sourceContext As JavaScriptSourceContext, sourceName As String) As JavaScriptValue
            Dim result As JavaScriptValue
            ThrowIfError(JsParseSerializedScript(script, buffer, sourceContext, sourceName, result))
            Return result
        End Function

        ''' <summary>
        '''     Parses a script and returns a <c>Function</c> representing the script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to parse.</param>
        ''' <returns>A <c>Function</c> representing the script code.</returns>
        Public Shared Function ParseScript(script As String) As JavaScriptValue
            Return ParseScript(script, JavaScriptSourceContext.None, String.Empty)
        End Function

        ''' <summary>
        '''     Parses a serialized script and returns a <c>Function</c> representing the script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to parse.</param>
        ''' <param name="buffer">The serialized script.</param>
        ''' <returns>A <c>Function</c> representing the script code.</returns>
        Public Shared Function ParseScript(script As String, buffer As Byte()) As JavaScriptValue
            Return ParseScript(script, buffer, JavaScriptSourceContext.None, String.Empty)
        End Function

        ''' <summary>
        '''     Executes a script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to run.</param>
        ''' <param name="sourceContext">
        '''     A cookie identifying the script that can be used by script contexts that have debugging enabled.
        ''' </param>
        ''' <param name="sourceName">The location the script came from.</param>
        ''' <returns>The result of the script, if any.</returns>
        Public Shared Function RunScript(script As String, sourceContext As JavaScriptSourceContext, sourceName As String) As JavaScriptValue
            Dim result As JavaScriptValue
            ThrowIfError(JsRunScript(script, sourceContext, sourceName, result))
            Return result
        End Function

        ''' <summary>
        '''     Runs a serialized script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The source code of the serialized script.</param>
        ''' <param name="buffer">The serialized script.</param>
        ''' <param name="sourceContext">
        '''     A cookie identifying the script that can be used by script contexts that have debugging enabled.
        ''' </param>
        ''' <param name="sourceName">The location the script came from.</param>
        ''' <returns>The result of the script, if any.</returns>
        Public Shared Function RunScript(script As String, buffer As Byte(), sourceContext As JavaScriptSourceContext, sourceName As String) As JavaScriptValue
            Dim result As JavaScriptValue
            ThrowIfError(JsRunSerializedScript(script, buffer, sourceContext, sourceName, result))
            Return result
        End Function

        ''' <summary>
        '''     Executes a script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The script to run.</param>
        ''' <returns>The result of the script, if any.</returns>
        Public Shared Function RunScript(script As String) As JavaScriptValue
            Return RunScript(script, JavaScriptSourceContext.None, String.Empty)
        End Function

        ''' <summary>
        '''     Runs a serialized script.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="script">The source code of the serialized script.</param>
        ''' <param name="buffer">The serialized script.</param>
        ''' <returns>The result of the script, if any.</returns>
        Public Shared Function RunScript(script As String, buffer As Byte()) As JavaScriptValue
            Return RunScript(script, buffer, JavaScriptSourceContext.None, String.Empty)
        End Function

        ''' <summary>
        '''     Serializes a parsed script to a buffer than can be reused.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     SerializeScript parses a script and then stores the parsed form of the script in a 
        '''     runtime-independent format. The serialized script then can be deserialized in any
        '''     runtime without requiring the script to be re-parsed.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="script">The script to serialize.</param>
        ''' <param name="buffer">The buffer to put the serialized script into. Can be null.</param>
        ''' <returns>
        '''     The size of the buffer, in bytes, required to hold the serialized script.
        ''' </returns>
        Public Shared Function SerializeScript(script As String, buffer As Byte()) As ULong
            Dim bufferSize = CULng(buffer.Length)
            ThrowIfError(JsSerializeScript(script, buffer, bufferSize))
            Return bufferSize
        End Function

        ''' <summary>
        '''     Returns the exception that caused the runtime of the current context to be in the 
        '''     exception state and resets the exception state for that runtime.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     If the runtime of the current context is not in an exception state, this API will throw
        '''     <c>JsErrorInvalidArgument</c>. If the runtime is disabled, this will return an exception
        '''     indicating that the script was terminated, but it will not clear the exception (the 
        '''     exception will be cleared if the runtime is re-enabled using 
        '''     <c>EnableRuntimeExecution</c>).
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>The exception for the runtime of the current context.</returns>
        Public Shared Function GetAndClearException() As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsGetAndClearException(reference))
            Return reference
        End Function

        ''' <summary>
        '''     Sets the runtime of the current context to an exception state.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     If the runtime of the current context is already in an exception state, this API will 
        '''     throw <c>JsErrorInExceptionState</c>.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="exception">
        '''     The JavaScript exception to set for the runtime of the current context.
        ''' </param>
        Public Shared Sub SetException(exception As JavaScriptValue)
            ThrowIfError(JsSetException(exception))
        End Sub

        ''' <summary>
        '''     Starts debugging in the context.
        ''' </summary>
        Public Shared Sub StartDebugging()
            ThrowIfError(JsStartDebugging())
        End Sub

        ''' <summary>
        '''     Starts profiling in the current context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="callback">The profiling callback to use.</param>
        ''' <param name="eventMask">The profiling events to callback with.</param>
        ''' <param name="context">A context to pass to the profiling callback.</param>
        Public Shared Sub StartProfiling(callback As IActiveScriptProfilerCallback, eventMask As ProfilerEventMask, context As Integer)
            ThrowIfError(JsStartProfiling(callback, eventMask, context))
        End Sub

        ''' <summary>
        '''     Stops profiling in the current context.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     Will not return an error if profiling has not started.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="reason">
        '''     The reason for stopping profiling to pass to the profiler callback.
        ''' </param>
        Public Shared Sub StopProfiling(reason As Integer)
            ThrowIfError(JsStopProfiling(reason))
        End Sub

        ''' <summary>
        '''     Enumerates the heap of the current context.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     While the heap is being enumerated, the current context cannot be removed, and all calls to
        '''     modify the state of the context will fail until the heap enumerator is released.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>A heap enumerator.</returns>
        Public Shared Function EnumerateHeap() As IActiveScriptProfilerHeapEnum
            Dim enumerator As IActiveScriptProfilerHeapEnum = Nothing
            ThrowIfError(JsEnumerateHeap(enumerator))
            Return enumerator
        End Function

        ''' <summary>
        '''     Adds a reference to a script context.
        ''' </summary>
        ''' <remarks>
        '''     Calling AddRef ensures that the context will not be freed until Release is called.
        ''' </remarks>
        ''' <returns>The object's new reference count.</returns>
        Public Function AddRef() As UInteger
            Dim count As UInteger
            ThrowIfError(JsContextAddRef(Me, count))
            Return count
        End Function

        ''' <summary>
        '''     Releases a reference to a script context.
        ''' </summary>
        ''' <remarks>
        '''     Removes a reference to a context that was created by AddRef.
        ''' </remarks>
        ''' <returns>The object's new reference count.</returns>
        Public Function Release() As UInteger
            Dim count As UInteger
            ThrowIfError(JsContextRelease(Me, count))
            Return count
        End Function

        ''' <summary>
        '''     A scope automatically sets a context to current and resets the original context
        '''     when disposed.
        ''' </summary>
        Public Structure Scope
            Implements IDisposable
            ''' <summary>
            '''     The previous context.
            ''' </summary>
            Private ReadOnly previousContext As JavaScriptContext

            ''' <summary>
            '''     Whether the structure has been disposed.
            ''' </summary>
            Private disposed As Boolean

            ''' <summary>
            '''     Initializes a new instance of the <see cref="Scope"/> struct. 
            ''' </summary>
            ''' <param name="context">The context to create the scope for.</param>
            Public Sub New(context As JavaScriptContext)
                disposed = False
                previousContext = Current
                Current = context
            End Sub

            ''' <summary>
            '''     Disposes the scope and sets the previous context to current.
            ''' </summary>
            Public Sub Dispose() Implements IDisposable.Dispose
                If disposed Then
                    Return
                End If

                Current = previousContext
                disposed = True
            End Sub
        End Structure
    End Structure
End Namespace
