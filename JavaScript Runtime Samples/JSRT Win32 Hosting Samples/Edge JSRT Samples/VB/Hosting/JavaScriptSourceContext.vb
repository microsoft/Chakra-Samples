Imports System

Namespace Hosting
    ''' <summary>
    '''     A cookie that identifies a script for debugging purposes.
    ''' </summary>
	Public Structure JavaScriptSourceContext
		Implements IEquatable(Of JavaScriptSourceContext)
		''' <summary>
		''' The context.
		''' </summary>
		Private ReadOnly context As IntPtr

		''' <summary>
		'''     Initializes a new instance of the <see cref="JavaScriptSourceContext"/> struct.
		''' </summary>
		''' <param name="context">The context.</param>
		Private Sub New(context As IntPtr)
			Me.context = context
		End Sub

		''' <summary>
		'''     Gets an empty source context.
		''' </summary>
		Public Shared ReadOnly Property None() As JavaScriptSourceContext
			Get
				Return New JavaScriptSourceContext(New IntPtr(-1))
			End Get
		End Property

		''' <summary>
		'''     The equality operator for source contexts.
		''' </summary>
		''' <param name="left">The first source context to compare.</param>
		''' <param name="right">The second source context to compare.</param>
		''' <returns>Whether the two source contexts are the same.</returns>
		Public Shared Operator =(left As JavaScriptSourceContext, right As JavaScriptSourceContext) As Boolean
			Return left.Equals(right)
		End Operator

		''' <summary>
		'''     The inequality operator for source contexts.
		''' </summary>
		''' <param name="left">The first source context to compare.</param>
		''' <param name="right">The second source context to compare.</param>
		''' <returns>Whether the two source contexts are not the same.</returns>
		Public Shared Operator <>(left As JavaScriptSourceContext, right As JavaScriptSourceContext) As Boolean
			Return Not left.Equals(right)
		End Operator

		''' <summary>
		'''     Subtracts an offset from the value of the source context.
		''' </summary>
		''' <param name="context">The source context to subtract the offset from.</param>
		''' <param name="offset">The offset to subtract.</param>
		''' <returns>A new source context that reflects the subtraction of the offset from the context.</returns>
		Public Shared Operator -(context As JavaScriptSourceContext, offset As Integer) As JavaScriptSourceContext
			Return FromIntPtr(context.context - offset)
		End Operator

		''' <summary>
		'''     Subtracts an offset from the value of the source context.
		''' </summary>
		''' <param name="left">The source context to subtract the offset from.</param>
		''' <param name="right">The offset to subtract.</param>
		''' <returns>A new source context that reflects the subtraction of the offset from the context.</returns>
		Public Shared Function Subtract(left As JavaScriptSourceContext, right As Integer) As JavaScriptSourceContext
			Return left - right
		End Function

        ''' <summary>
        '''     Adds an offset from the value of the source context.
        ''' </summary>
        ''' <param name="context">The source context to add the offset to.</param>
        ''' <param name="offset">The offset to add.</param>
        ''' <returns>A new source context that reflects the addition of the offset to the context.</returns>
		Public Shared Operator +(context As JavaScriptSourceContext, offset As Integer) As JavaScriptSourceContext
			Return FromIntPtr(context.context + offset)
		End Operator

		''' <summary>
		'''     Adds an offset from the value of the source context.
		''' </summary>
		''' <param name="left">The source context to add the offset to.</param>
		''' <param name="right">The offset to add.</param>
		''' <returns>A new source context that reflects the addition of the offset to the context.</returns>
		Public Shared Function Add(left As JavaScriptSourceContext, right As Integer) As JavaScriptSourceContext
			Return left + right
		End Function

        ''' <summary>
        '''     Creates a new source context. 
        ''' </summary>
        ''' <param name="cookie">
        '''     The cookie for the source context.
        ''' </param>
        ''' <returns>The new source context.</returns>
		Public Shared Function FromIntPtr(cookie As IntPtr) As JavaScriptSourceContext
			Return New JavaScriptSourceContext(cookie)
		End Function

		''' <summary>
		'''     Checks for equality between source contexts.
		''' </summary>
		''' <param name="other">The other source context to compare.</param>
		''' <returns>Whether the two source contexts are the same.</returns>
        Public Shadows Function Equals(other As JavaScriptSourceContext) As Boolean Implements IEquatable(Of JavaScriptSourceContext).Equals
            Return context = other.context
        End Function

		''' <summary>
		'''     Checks for equality between source contexts.
		''' </summary>
		''' <param name="obj">The other source context to compare.</param>
		''' <returns>Whether the two source contexts are the same.</returns>
        Public Shadows Function Equals(obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then
                Return False
            End If

            Return TypeOf obj Is JavaScriptSourceContext AndAlso Equals(DirectCast(obj, JavaScriptSourceContext))
        End Function

		''' <summary>
		'''     The hash code.
		''' </summary>
		''' <returns>The hash code of the source context.</returns>
		Public Overrides Function GetHashCode() As Integer
			Return context.ToInt32()
		End Function
	End Structure
End Namespace
