Imports System.Runtime.Serialization

Namespace Hosting
    ''' <summary>
    '''     An exception returned from the Chakra engine.
    ''' </summary>
    <Serializable>
    Public Class JavaScriptException
        Inherits Exception
        ''' <summary>
        ''' The error code.
        ''' </summary>
        Private ReadOnly code As JavaScriptErrorCode

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        Public Sub New(code As JavaScriptErrorCode)
            Me.New(code, "A fatal exception has occurred in a JavaScript runtime")
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        ''' </summary>
        ''' <param name="code">The error code returned.</param>
        ''' <param name="message">The error message.</param>
        Public Sub New(code As JavaScriptErrorCode, message As String)
            MyBase.New(message)
            Me.code = code
        End Sub

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        ''' </summary>
        ''' <param name="info">The serialization info.</param>
        ''' <param name="context">The streaming context.</param>
        Protected Sub New(info As SerializationInfo, context As StreamingContext)
            MyBase.New(info, context)
            If info IsNot Nothing Then
                code = DirectCast(info.GetUInt32("code"), JavaScriptErrorCode)
            End If
        End Sub

        ''' <summary>
        '''     Serializes the exception information.
        ''' </summary>
        ''' <param name="info">The serialization information.</param>
        ''' <param name="context">The streaming context.</param>
        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)
            MyBase.GetObjectData(info, context)
            info.AddValue("code", CUInt(code))
        End Sub

        ''' <summary>
        '''     Gets the error code.
        ''' </summary>
        Public ReadOnly Property ErrorCode() As JavaScriptErrorCode
            Get
                Return code
            End Get
        End Property
    End Class
End Namespace
