Namespace Hosting
    ''' <summary>
    '''     A background work item callback.
    ''' </summary>
    ''' <remarks>
    '''     This is passed to the host's thread service (if provided) to allow the host to 
    '''     invoke the work item callback on the background thread of its choice.
    ''' </remarks>
    ''' <param name="callbackData">Data argument passed to the thread service.</param>
    Public Delegate Sub JavaScriptBackgroundWorkItemCallback(callbackData As IntPtr)
End Namespace
