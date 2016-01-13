Imports System
Imports System.Runtime.Serialization

Namespace Hosting
    ''' <summary>
    '''     A fatal exception occurred.
    ''' </summary>
    <Serializable>
    Public NotInheritable Class JavaScriptFatalException
        Inherits JavaScriptException
        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptFatalException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        Public Sub New(code As JavaScriptErrorCode)
            Me.New(code, "A fatal exception has occurred in a JavaScript runtime")
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptFatalException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        ''' <param name="message">The error message.</param>
        Public Sub New(code As JavaScriptErrorCode, message As String)
            MyBase.New(code, message)
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptFatalException"/> class.
        ''' </summary>
        ''' <param name="info">The serialization info.</param>
        ''' <param name="context">The streaming context.</param>
        Private Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class
End Namespace
