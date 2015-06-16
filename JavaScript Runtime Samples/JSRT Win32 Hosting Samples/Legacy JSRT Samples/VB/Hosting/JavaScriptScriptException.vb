Imports System
Imports System.Runtime.Serialization

Namespace Hosting
    ''' <summary>
    '''     A script exception.
    ''' </summary>
    <Serializable>
    Public NotInheritable Class JavaScriptScriptException
        Inherits JavaScriptException
        ''' <summary>
        ''' The error.
        ''' </summary>
        <NonSerialized>
        Private ReadOnly errorObject As JavaScriptValue

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptScriptException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        ''' <param name="errorObject">The JavaScript error object.</param>
        Public Sub New(code As JavaScriptErrorCode, errorObject As JavaScriptValue)
            Me.New(code, errorObject, "JavaScript Exception")
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptScriptException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        ''' <param name="errorObject">The JavaScript error object.</param>
        ''' <param name="message">The error message.</param>
        Public Sub New(code As JavaScriptErrorCode, errorObject As JavaScriptValue, message As String)
            MyBase.New(code, message)
            Me.errorObject = errorObject
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptScriptException"/> class.
        ''' </summary>
        ''' <param name="info">The serialization info.</param>
        ''' <param name="context">The streaming context.</param>
        Private Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        ''' <summary>
        '''     Gets a JavaScript object representing the script error.
        ''' </summary>
        Public ReadOnly Property JavaScriptError() As JavaScriptValue
            Get
                Return errorObject
            End Get
        End Property
    End Class
End Namespace
