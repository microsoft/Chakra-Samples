Imports System

Namespace Hosting
    ''' <summary>
    '''     A property identifier.
    ''' </summary>
    ''' <remarks>
    '''     Property identifiers are used to refer to properties of JavaScript objects instead of using
    '''     strings.
    ''' </remarks>
    Public Structure JavaScriptPropertyId
        Implements IEquatable(Of JavaScriptPropertyId)
        ''' <summary>
        ''' The id.
        ''' </summary>
        Private ReadOnly id As IntPtr

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptPropertyId"/> struct. 
        ''' </summary>
        ''' <param name="id">The ID.</param>
        Friend Sub New(id As IntPtr)
            Me.id = id
        End Sub

        ''' <summary>
        '''     Gets an invalid ID.
        ''' </summary>
        Public Shared ReadOnly Property Invalid() As JavaScriptPropertyId
            Get
                Return New JavaScriptPropertyId(IntPtr.Zero)
            End Get
        End Property

        ''' <summary>
        '''     Gets the name associated with the property ID.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        Public ReadOnly Property Name() As String
            Get
                Dim nameValue As String = Nothing
                ThrowIfError(JsGetPropertyNameFromId(Me, nameValue))
                Return nameValue
            End Get
        End Property

        ''' <summary>
        '''     Gets the property ID associated with the name. 
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     Property IDs are specific to a context and cannot be used across contexts.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="name">
        '''     The name of the property ID to get or create. The name may consist of only digits.
        ''' </param>
        ''' <returns>The property ID in this runtime for the given name.</returns>
        Public Shared Function FromString(name As String) As JavaScriptPropertyId
            Dim id As JavaScriptPropertyId
            ThrowIfError(JsGetPropertyIdFromName(name, id))
            Return id
        End Function

        ''' <summary>
        '''     The equality operator for property IDs.
        ''' </summary>
        ''' <param name="left">The first property ID to compare.</param>
        ''' <param name="right">The second property ID to compare.</param>
        ''' <returns>Whether the two property IDs are the same.</returns>
        Public Shared Operator =(left As JavaScriptPropertyId, right As JavaScriptPropertyId) As Boolean
            Return left.Equals(right)
        End Operator

        ''' <summary>
        '''     The inequality operator for property IDs.
        ''' </summary>
        ''' <param name="left">The first property ID to compare.</param>
        ''' <param name="right">The second property ID to compare.</param>
        ''' <returns>Whether the two property IDs are not the same.</returns>
        Public Shared Operator <>(left As JavaScriptPropertyId, right As JavaScriptPropertyId) As Boolean
            Return Not left.Equals(right)
        End Operator

        ''' <summary>
        '''     Checks for equality between property IDs.
        ''' </summary>
        ''' <param name="other">The other property ID to compare.</param>
        ''' <returns>Whether the two property IDs are the same.</returns>
        Public Shadows Function Equals(other As JavaScriptPropertyId) As Boolean Implements IEquatable(Of JavaScriptPropertyId).Equals
            Return id = other.id
        End Function

        ''' <summary>
        '''     Checks for equality between property IDs.
        ''' </summary>
        ''' <param name="obj">The other property ID to compare.</param>
        ''' <returns>Whether the two property IDs are the same.</returns>
        Public Shadows Function Equals(obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then
                Return False
            End If

            Return TypeOf obj Is JavaScriptPropertyId AndAlso Equals(DirectCast(obj, JavaScriptPropertyId))
        End Function

        ''' <summary>
        '''     The hash code.
        ''' </summary>
        ''' <returns>The hash code of the property ID.</returns>
        Public Overrides Function GetHashCode() As Integer
            Return id.ToInt32()
        End Function

        ''' <summary>
        '''     Converts the property ID to a string.
        ''' </summary>
        ''' <returns>The name of the property ID.</returns>
        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Structure
End Namespace
