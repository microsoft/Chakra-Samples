Imports System
Imports System.Runtime.InteropServices

Namespace Hosting
    ''' <summary>
    '''     A JavaScript value.
    ''' </summary>
    ''' <remarks>
    '''     A JavaScript value is one of the following types of values: Undefined, Null, Boolean, 
    '''     String, Number, or Object.
    ''' </remarks>
    Public Structure JavaScriptValue
        ''' <summary>
        ''' The reference.
        ''' </summary>
        Private ReadOnly reference As IntPtr

        ''' <summary>
        '''     Initializes a new instance of the <see cref="JavaScriptValue"/> struct.
        ''' </summary>
        ''' <param name="reference">The reference.</param>
        Private Sub New(reference As IntPtr)
            Me.reference = reference
        End Sub

        ''' <summary>
        '''     Gets an invalid value.
        ''' </summary>
        Public Shared ReadOnly Property Invalid() As JavaScriptValue
            Get
                Return New JavaScriptValue(IntPtr.Zero)
            End Get
        End Property

        ''' <summary>
        '''     Gets the value of <c>undefined</c> in the current script context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property Undefined() As JavaScriptValue
            Get
                Dim value As JavaScriptValue
                ThrowIfError(JsGetUndefinedValue(value))
                Return value
            End Get
        End Property

        ''' <summary>
        '''     Gets the value of <c>null</c> in the current script context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property Null() As JavaScriptValue
            Get
                Dim value As JavaScriptValue
                ThrowIfError(JsGetNullValue(value))
                Return value
            End Get
        End Property

        ''' <summary>
        '''     Gets the value of <c>true</c> in the current script context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property TrueValue() As JavaScriptValue
            Get
                Dim value As JavaScriptValue
                ThrowIfError(JsGetTrueValue(value))
                Return value
            End Get
        End Property

        ''' <summary>
        '''     Gets the value of <c>false</c> in the current script context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property FalseValue() As JavaScriptValue
            Get
                Dim value As JavaScriptValue
                ThrowIfError(JsGetFalseValue(value))
                Return value
            End Get
        End Property

        ''' <summary>
        '''     Gets the global object in the current script context.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Shared ReadOnly Property GlobalObject() As JavaScriptValue
            Get
                Dim value As JavaScriptValue
                ThrowIfError(JsGetGlobalObject(value))
                Return value
            End Get
        End Property

        ''' <summary>
        '''     Gets a value indicating whether the value is valid.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return reference <> IntPtr.Zero
            End Get
        End Property

        ''' <summary>
        '''     Gets the JavaScript type of the value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The type of the value.</returns>
        Public ReadOnly Property ValueType() As JavaScriptValueType
            Get
                Dim type As JavaScriptValueType
                ThrowIfError(JsGetValueType(Me, type))
                Return type
            End Get
        End Property

        ''' <summary>
        '''     Gets the length of a <c>String</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The length of the string.</returns>
        Public ReadOnly Property StringLength() As Integer
            Get
                Dim length As Integer
                ThrowIfError(JsGetStringLength(Me, length))
                Return length
            End Get
        End Property

        ''' <summary>
        '''     Gets or sets the prototype of an object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Property Prototype() As JavaScriptValue
            Get
                Dim prototypeReference As JavaScriptValue
                ThrowIfError(JsGetPrototype(Me, prototypeReference))
                Return prototypeReference
            End Get

            Set(value As JavaScriptValue)
                ThrowIfError(JsSetPrototype(Me, Value))
            End Set
        End Property

        ''' <summary>
        '''     Gets a value indicating whether an object is extensible or not.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public ReadOnly Property IsExtensionAllowed() As Boolean
            Get
                Dim allowed As Boolean
                ThrowIfError(JsGetExtensionAllowed(Me, allowed))
                Return allowed
            End Get
        End Property

        ''' <summary>
        '''     Gets a value indicating whether an object is an external object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public ReadOnly Property HasExternalData() As Boolean
            Get
                Dim hasExternalDataValue As Boolean
                ThrowIfError(JsHasExternalData(Me, hasExternalDataValue))
                Return hasExternalDataValue
            End Get
        End Property

        ''' <summary>
        '''     Gets or sets the data in an external object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Property ExternalData() As IntPtr
            Get
                Dim data As IntPtr
                ThrowIfError(JsGetExternalData(Me, data))
                Return data
            End Get

            Set(value As IntPtr)
                ThrowIfError(JsSetExternalData(Me, Value))
            End Set
        End Property

        ''' <summary>
        '''     Creates a <c>Boolean</c> value from a <c>bool</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="value">The value to be converted.</param>
        ''' <returns>The converted value.</returns>
        Public Shared Function FromBoolean(value As Boolean) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsBoolToBoolean(value, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a <c>Number</c> value from a <c>double</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="value">The value to be converted.</param>
        ''' <returns>The new <c>Number</c> value.</returns>
        Public Shared Function FromDouble(value As Double) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsDoubleToNumber(value, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a <c>Number</c> value from a <c>int</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="value">The value to be converted.</param>
        ''' <returns>The new <c>Number</c> value.</returns>
        Public Shared Function FromInt32(value As Integer) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsIntToNumber(value, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a <c>String</c> value from a string pointer.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="value">The string  to convert to a <c>String</c> value.</param>
        ''' <returns>The new <c>String</c> value.</returns>
        Public Shared Function FromString(value As String) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsPointerToString(value, New UIntPtr(CUInt(value.Length)), reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a JavaScript value that is a projection of the passed in object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="value">An object to be projected.</param>
        ''' <returns>A JavaScript value that is a projection of the object.</returns>
        Public Shared Function FromObject(value As Object) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsVariantToValue(value, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new <c>Object</c>.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The new <c>Object</c>.</returns>
        Public Shared Function CreateObject() As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateObject(reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new <c>Object</c> that stores some external data.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="data">External data that the object will represent. May be null.</param>
        ''' <param name="finalizer">
        '''     A callback for when the object is finalized. May be null.
        ''' </param>
        ''' <returns>The new <c>Object</c>.</returns>
        Public Shared Function CreateExternalObject(data As IntPtr, finalizer As JavaScriptObjectFinalizeCallback) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateExternalObject(data, finalizer, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript function.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="nativeFunction">The method to call when the function is invoked.</param>
        ''' <returns>The new function object.</returns>
        Public Shared Function CreateFunction(nativeFunction As JavaScriptNativeFunction) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateFunction(nativeFunction, IntPtr.Zero, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript function.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="nativeFunction">The method to call when the function is invoked.</param>
        ''' <param name="callbackData">Data to provide to the callback.</param>
        ''' <returns>The new function object.</returns>
        Public Shared Function CreateFunction(nativeFunction As JavaScriptNativeFunction, callbackData As IntPtr) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateFunction(nativeFunction, callbackData, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a JavaScript array object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="length">The initial length of the array.</param>
        ''' <returns>The new array object.</returns>
        Public Shared Function CreateArray(length As UInteger) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateArray(length, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript RangeError error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateRangeError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateRangeError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript ReferenceError error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateReferenceError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateReferenceError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript SyntaxError error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateSyntaxError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateSyntaxError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript TypeError error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateTypeError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateTypeError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Creates a new JavaScript URIError error object
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="message">Message for the error object.</param>
        ''' <returns>The new error object.</returns>
        Public Shared Function CreateUriError(message As JavaScriptValue) As JavaScriptValue
            Dim reference As JavaScriptValue
            ThrowIfError(JsCreateURIError(message, reference))
            Return reference
        End Function

        ''' <summary>
        '''     Adds a reference to the object.
        ''' </summary>
        ''' <remarks>
        '''     This only needs to be called on objects that are not going to be stored somewhere on 
        '''     the stack. Calling AddRef ensures that the JavaScript object the value refers to will not be freed 
        '''     until Release is called
        ''' </remarks>
        ''' <returns>The object's new reference count.</returns>
        Public Function AddRef() As UInteger
            Dim count As UInteger
            ThrowIfError(JsAddRef(Me, count))
            Return count
        End Function

        ''' <summary>
        '''     Releases a reference to the object.
        ''' </summary>
        ''' <remarks>
        '''     Removes a reference that was created by AddRef.
        ''' </remarks>
        ''' <returns>The object's new reference count.</returns>
        Public Function Release() As UInteger
            Dim count As UInteger
            ThrowIfError(JsRelease(Me, count))
            Return count
        End Function

        ''' <summary>
        '''     Retrieves the <c>bool</c> value of a <c>Boolean</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The converted value.</returns>
        Public Function ToBoolean() As Boolean
            Dim value As Boolean
            ThrowIfError(JsBooleanToBool(Me, value))
            Return value
        End Function

        ''' <summary>
        '''     Retrieves the <c>double</c> value of a <c>Number</c> value.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     This function retrieves the value of a Number value. It will fail with 
        '''     <c>InvalidArgument</c> if the type of the value is not <c>Number</c>.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>The <c>double</c> value.</returns>
        Public Function ToDouble() As Double
            Dim value As Double
            ThrowIfError(JsNumberToDouble(Me, value))
            Return value
        End Function

        ''' <summary>
        '''     Retrieves the <c>int</c> value of a <c>Number</c> value.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     This function retrieves the value of a Number value. It will fail with
        '''     <c>InvalidArgument</c> if the type of the value is not <c>Number</c>.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>The <c>int</c> value.</returns>
        Public Function ToInt32() As Integer
            Dim value As Integer
            ThrowIfError(JsNumberToInt(Me, value))
            Return value
        End Function

        ''' <summary>
        '''     Retrieves the string pointer of a <c>String</c> value.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     This function retrieves the string pointer of a <c>String</c> value. It will fail with 
        '''     <c>InvalidArgument</c> if the type of the value is not <c>String</c>.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <returns>The string.</returns>
        Public Shadows Function ToString() As String
            Dim buffer As IntPtr
            Dim length As UIntPtr
            ThrowIfError(JsStringToPointer(Me, buffer, length))
            Return Marshal.PtrToStringAuto(buffer, CInt(length))
        End Function

        ''' <summary>
        '''     Retrieves the object representation of an <c>Object</c> value.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The object representation of the value.</returns>
        Public Function ToObject() As Object
            Dim value As Object = Nothing
            ThrowIfError(JsValueToVariant(Me, value))
            Return value
        End Function

        ''' <summary>
        '''     Converts the value to <c>Boolean</c> using regular JavaScript semantics.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The converted value.</returns>
        Public Function ConvertToBoolean() As JavaScriptValue
            Dim booleanReference As JavaScriptValue
            ThrowIfError(JsConvertValueToBoolean(Me, booleanReference))
            Return booleanReference
        End Function

        ''' <summary>
        '''     Converts the value to <c>Number</c> using regular JavaScript semantics.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The converted value.</returns>
        Public Function ConvertToNumber() As JavaScriptValue
            Dim numberReference As JavaScriptValue
            ThrowIfError(JsConvertValueToNumber(Me, numberReference))
            Return numberReference
        End Function

        ''' <summary>
        '''     Converts the value to <c>String</c> using regular JavaScript semantics.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The converted value.</returns>
        Public Function ConvertToString() As JavaScriptValue
            Dim stringReference As JavaScriptValue
            ThrowIfError(JsConvertValueToString(Me, stringReference))
            Return stringReference
        End Function

        ''' <summary>
        '''     Converts the value to <c>Object</c> using regular JavaScript semantics.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>The converted value.</returns>
        Public Function ConvertToObject() As JavaScriptValue
            Dim objectReference As JavaScriptValue
            ThrowIfError(JsConvertValueToObject(Me, objectReference))
            Return objectReference
        End Function

        ''' <summary>
        '''     Sets an object to not be extensible.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        Public Sub PreventExtension()
            ThrowIfError(JsPreventExtension(Me))
        End Sub

        ''' <summary>
        '''     Gets a property descriptor for an object's own property.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="propertyId">The ID of the property.</param>
        ''' <returns>The property descriptor.</returns>
        Public Function GetOwnPropertyDescriptor(propertyId As JavaScriptPropertyId) As JavaScriptValue
            Dim descriptorReference As JavaScriptValue
            ThrowIfError(JsGetOwnPropertyDescriptor(Me, propertyId, descriptorReference))
            Return descriptorReference
        End Function

        ''' <summary>
        '''     Gets the list of all properties on the object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <returns>An array of property names.</returns>
        Public Function GetOwnPropertyNames() As JavaScriptValue
            Dim propertyNamesReference As JavaScriptValue
            ThrowIfError(JsGetOwnPropertyNames(Me, propertyNamesReference))
            Return propertyNamesReference
        End Function

        ''' <summary>
        '''     Determines whether an object has a property.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="propertyId">The ID of the property.</param>
        ''' <returns>Whether the object (or a prototype) has the property.</returns>
        Public Function HasProperty(propertyId As JavaScriptPropertyId) As Boolean
            Dim hasPropertyValue As Boolean
            ThrowIfError(JsHasProperty(Me, propertyId, hasPropertyValue))
            Return hasPropertyValue
        End Function

        ''' <summary>
        '''     Gets an object's property.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="id">The ID of the property.</param>
        ''' <returns>The value of the property.</returns>
        Public Function GetProperty(id As JavaScriptPropertyId) As JavaScriptValue
            Dim propertyReference As JavaScriptValue
            ThrowIfError(JsGetProperty(Me, id, propertyReference))
            Return propertyReference
        End Function

        ''' <summary>
        '''     Sets an object's property.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="id">The ID of the property.</param>
        ''' <param name="value">The new value of the property.</param>
        ''' <param name="useStrictRules">The property set should follow strict mode rules.</param>
        Public Sub SetProperty(id As JavaScriptPropertyId, value As JavaScriptValue, useStrictRules As Boolean)
            ThrowIfError(JsSetProperty(Me, id, value, useStrictRules))
        End Sub

        ''' <summary>
        '''     Deletes an object's property.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="propertyId">The ID of the property.</param>
        ''' <param name="useStrictRules">The property set should follow strict mode rules.</param>
        ''' <returns>Whether the property was deleted.</returns>
        Public Function DeleteProperty(propertyId As JavaScriptPropertyId, useStrictRules As Boolean) As JavaScriptValue
            Dim returnReference As JavaScriptValue
            ThrowIfError(JsDeleteProperty(Me, propertyId, useStrictRules, returnReference))
            Return returnReference
        End Function

        ''' <summary>
        '''     Defines a new object's own property from a property descriptor.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="propertyId">The ID of the property.</param>
        ''' <param name="propertyDescriptor">The property descriptor.</param>
        ''' <returns>Whether the property was defined.</returns>
        Public Function DefineProperty(propertyId As JavaScriptPropertyId, propertyDescriptor As JavaScriptValue) As Boolean
            Dim result As Boolean
            ThrowIfError(JsDefineProperty(Me, propertyId, propertyDescriptor, result))
            Return result
        End Function

        ''' <summary>
        '''     Test if an object has a value at the specified index.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="index">The index to test.</param>
        ''' <returns>Whether the object has an value at the specified index.</returns>
        Public Function HasIndexedProperty(index As JavaScriptValue) As Boolean
            Dim hasProperty As Boolean
            ThrowIfError(JsHasIndexedProperty(Me, index, hasProperty))
            Return hasProperty
        End Function

        ''' <summary>
        '''     Retrieve the value at the specified index of an object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="index">The index to retrieve.</param>
        ''' <returns>The retrieved value.</returns>
        Public Function GetIndexedProperty(index As JavaScriptValue) As JavaScriptValue
            Dim propertyReference As JavaScriptValue
            ThrowIfError(JsGetIndexedProperty(Me, index, propertyReference))
            Return propertyReference
        End Function

        ''' <summary>
        '''     Set the value at the specified index of an object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="index">The index to set.</param>
        ''' <param name="value">The value to set.</param>
        Public Sub SetIndexedProperty(index As JavaScriptValue, value As JavaScriptValue)
            ThrowIfError(JsSetIndexedProperty(Me, index, value))
        End Sub

        ''' <summary>
        '''     Delete the value at the specified index of an object.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="index">The index to delete.</param>
        Public Sub DeleteIndexedProperty(index As JavaScriptValue)
            ThrowIfError(JsDeleteIndexedProperty(Me, index))
        End Sub

        ''' <summary>
        '''     Compare two JavaScript values for equality.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     This function is equivalent to the "==" operator in JavaScript.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="other">The object to compare.</param>
        ''' <returns>Whether the values are equal.</returns>
        Public Shadows Function Equals(other As JavaScriptValue) As Boolean
            Dim equalsValue As Boolean
            ThrowIfError(JsEquals(Me, other, equalsValue))
            Return equalsValue
        End Function

        ''' <summary>
        '''     Compare two JavaScript values for strict equality.
        ''' </summary>
        ''' <remarks>
        '''     <para>
        '''     This function is equivalent to the "===" operator in JavaScript.
        '''     </para>
        '''     <para>
        '''     Requires an active script context.
        '''     </para>
        ''' </remarks>
        ''' <param name="other">The object to compare.</param>
        ''' <returns>Whether the values are strictly equal.</returns>
        Public Function StrictEquals(other As JavaScriptValue) As Boolean
            Dim equals As Boolean
            ThrowIfError(JsStrictEquals(Me, other, equals))
            Return equals
        End Function

        ''' <summary>
        '''     Invokes a function.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="arguments">The arguments to the call.</param>
        ''' <returns>The <c>Value</c> returned from the function invocation, if any.</returns>
        Public Function CallFunction(ParamArray arguments As JavaScriptValue()) As JavaScriptValue
            Dim returnReference As JavaScriptValue

            If arguments.Length > UShort.MaxValue Then
                Throw New ArgumentOutOfRangeException("arguments")
            End If

            ThrowIfError(JsCallFunction(Me, arguments, CUShort(arguments.Length), returnReference))
            Return returnReference
        End Function

        ''' <summary>
        '''     Invokes a function as a constructor.
        ''' </summary>
        ''' <remarks>
        '''     Requires an active script context.
        ''' </remarks>
        ''' <param name="arguments">The arguments to the call.</param>
        ''' <returns>The <c>Value</c> returned from the function invocation.</returns>
        Public Function ConstructObject(ParamArray arguments As JavaScriptValue()) As JavaScriptValue
            Dim returnReference As JavaScriptValue

            If arguments.Length > UShort.MaxValue Then
                Throw New ArgumentOutOfRangeException("arguments")
            End If

            ThrowIfError(JsConstructObject(Me, arguments, CUShort(arguments.Length), returnReference))
            Return returnReference
        End Function
    End Structure
End Namespace
