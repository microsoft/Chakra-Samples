Namespace Hosting
    ''' <summary>
    '''     Allocation callback event type.
    ''' </summary>
    Public Enum JavaScriptMemoryEventType
        ''' <summary>
        '''     Indicates a request for memory allocation.
        ''' </summary>
        Allocate = 0

        ''' <summary>
        '''     Indicates a memory freeing event.
        ''' </summary>
        Free = 1

        ''' <summary>
        '''     Indicates a failed allocation event.
        ''' </summary>
        Failure = 2
    End Enum
End Namespace
