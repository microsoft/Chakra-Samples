Imports System

Namespace Hosting
    ''' <summary>
    '''     A finalization callback.
    ''' </summary>
    ''' <param name="data">
    '''     The external data that was passed in when creating the object being finalized.
    ''' </param>
    Public Delegate Sub JavaScriptObjectFinalizeCallback(data As IntPtr)
End Namespace
