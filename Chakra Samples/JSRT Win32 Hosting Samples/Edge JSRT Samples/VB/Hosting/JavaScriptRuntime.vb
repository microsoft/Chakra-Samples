Imports System

Namespace Hosting
    ''' <summary>
    '''     A Chakra runtime.
    ''' </summary>
    ''' <remarks>
    '''     <para>
    '''     Each Chakra runtime has its own independent execution engine, JIT compiler, and garbage 
    '''     collected heap. As such, each runtime is completely isolated from other runtimes.
    '''     </para>
    '''     <para>
    '''     Runtimes can be used on any thread, but only one thread can call into a runtime at any 
    '''     time.
    '''     </para>
    '''     <para>
    '''     NOTE: A JavaScriptRuntime, unlike other objects in the Chakra hosting API, is not 
    '''     garbage collected since it contains the garbage collected heap itself. A runtime will 
    '''     continue to exist until Dispose is called.
    '''     </para>
    ''' </remarks>
    Public Structure JavaScriptRuntime
        Implements IDisposable
        ''' <summary>
        ''' The handle.
        ''' </summary>
        Private handle As IntPtr

        ''' <summary>
        '''     Gets a value indicating whether the runtime is valid.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return handle <> IntPtr.Zero
            End Get
        End Property

        ''' <summary>
        '''     Gets the current memory usage for a runtime.
        ''' </summary>
        ''' <remarks>
        '''     Memory usage can be always be retrieved, regardless of whether or not the runtime is active
        '''     on another thread.
        ''' </remarks>
        Public ReadOnly Property MemoryUsage() As UIntPtr
            Get
                Dim memoryUsageValue As UIntPtr
                ThrowIfError(JsGetRuntimeMemoryUsage(Me, memoryUsageValue))
                Return memoryUsageValue
            End Get
        End Property

        ''' <summary>
        '''     Gets or sets the current memory limit for a runtime.
        ''' </summary>
        ''' <remarks>
        '''     The memory limit of a runtime can be always be retrieved, regardless of whether or not the 
        '''     runtime is active on another thread.
        ''' </remarks>
        Public Property MemoryLimit() As UIntPtr
            Get
                Dim memoryLimitValue As UIntPtr
                ThrowIfError(JsGetRuntimeMemoryLimit(Me, memoryLimitValue))
                Return memoryLimitValue
            End Get

            Set(value As UIntPtr)
                ThrowIfError(JsSetRuntimeMemoryLimit(Me, Value))
            End Set
        End Property

        ''' <summary>
        '''     Gets or sets a value indicating whether script execution is disabled in the runtime.
        ''' </summary>
        Public Property Disabled() As Boolean
            Get
                Dim isDisabled As Boolean
                ThrowIfError(JsIsRuntimeExecutionDisabled(Me, isDisabled))
                Return isDisabled
            End Get

            Set(value As Boolean)
                ThrowIfError(If(Value, JsDisableRuntimeExecution(Me), JsEnableRuntimeExecution(Me)))
            End Set
        End Property

        ''' <summary>
        '''     Creates a new runtime.
        ''' </summary>
        ''' <param name="attributes">The attributes of the runtime to be created.</param>
        ''' <param name="version">The version of the runtime to be created.</param>
        ''' <param name="threadServiceCallback">The thread service for the runtime. Can be null.</param>
        ''' <returns>The runtime created.</returns>
        Public Shared Function Create(attributes As JavaScriptRuntimeAttributes, version As JavaScriptRuntimeVersion, threadServiceCallback As JavaScriptThreadServiceCallback) As JavaScriptRuntime
            Dim handle As JavaScriptRuntime
            ThrowIfError(JsCreateRuntime(attributes, threadServiceCallback, handle))
            Return handle
        End Function

        ''' <summary>
        '''     Creates a new runtime.
        ''' </summary>
        ''' <param name="attributes">The attributes of the runtime to be created.</param>
        ''' <param name="version">The version of the runtime to be created.</param>
        ''' <returns>The runtime created.</returns>
        Public Shared Function Create(attributes As JavaScriptRuntimeAttributes, version As JavaScriptRuntimeVersion) As JavaScriptRuntime
            Return Create(attributes, version, Nothing)
        End Function

        ''' <summary>
        '''     Creates a new runtime.
        ''' </summary>
        ''' <returns>The runtime created.</returns>
        Public Shared Function Create() As JavaScriptRuntime
            Return Create(JavaScriptRuntimeAttributes.None, JavaScriptRuntimeVersion.Version11, Nothing)
        End Function

        ''' <summary>
        '''     Disposes a runtime.
        ''' </summary>
        ''' <remarks>
        '''     Once a runtime has been disposed, all resources owned by it are invalid and cannot be used.
        '''     If the runtime is active (i.e. it is set to be current on a particular thread), it cannot 
        '''     be disposed.
        ''' </remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            If IsValid Then
                ThrowIfError(JsDisposeRuntime(Me))
            End If

            handle = IntPtr.Zero
        End Sub

        ''' <summary>
        '''     Performs a full garbage collection.
        ''' </summary>
        Public Sub CollectGarbage()
            ThrowIfError(JsCollectGarbage(Me))
        End Sub

        ''' <summary>
        '''     Sets a memory allocation callback for specified runtime
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     Registering a memory allocation callback will cause the runtime to call back to the host 
        '''     whenever it acquires memory from, or releases memory to, the OS. The callback routine is
        '''     called before the runtime memory manager allocates a block of memory. The allocation will
        '''     be rejected if the callback returns false. The runtime memory manager will also invoke the
        '''     callback routine after freeing a block of memory, as well as after allocation failures. 
        '''     </para>
        '''     <para>
        '''     The callback is invoked on the current runtime execution thread, therefore execution is 
        '''     blocked until the callback completes.
        '''     </para>
        '''     <para>
        '''     The return value of the callback is not stored; previously rejected allocations will not
        '''     prevent the runtime from invoking the callback again later for new memory allocations.
        '''     </para>
        ''' </remarks>
        ''' <param name="callbackState">
        '''     User provided state that will be passed back to the callback.
        ''' </param>
        ''' <param name="allocationCallback">
        '''     Memory allocation callback to be called for memory allocation events.
        ''' </param>
        Public Sub SetMemoryAllocationCallback(callbackState As IntPtr, allocationCallback As JavaScriptMemoryAllocationCallback)
            ThrowIfError(JsSetRuntimeMemoryAllocationCallback(Me, callbackState, allocationCallback))
        End Sub

        ''' <summary>
        '''     Sets a callback function that is called by the runtime before garbage collection.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     The callback is invoked on the current runtime execution thread, therefore execution is 
        '''     blocked until the callback completes.
        '''     </para>
        '''     <para>
        '''     The callback can be used by hosts to prepare for garbage collection. For example, by 
        '''     releasing unnecessary references on Chakra objects.
        '''     </para>
        ''' </remarks>
        ''' <param name="callbackState">
        '''     User provided state that will be passed back to the callback.
        ''' </param>
        ''' <param name="beforeCollectCallback">The callback function being set.</param>
        Public Sub SetBeforeCollectCallback(callbackState As IntPtr, beforeCollectCallback As JavaScriptBeforeCollectCallback)
            ThrowIfError(JsSetRuntimeBeforeCollectCallback(Me, callbackState, beforeCollectCallback))
        End Sub

        ''' <summary>
        '''     Creates a script context for running scripts.
        ''' </summary>
        ''' <remarks>
        '''     Each script context has its own global object that is isolated from all other script 
        '''     contexts.
        ''' </remarks>
        ''' <returns>The created script context.</returns>
        Public Function CreateContext() As JavaScriptContext
            Dim reference As JavaScriptContext
            ThrowIfError(JsCreateContext(Me, reference))
            Return reference
        End Function
    End Structure
End Namespace
