namespace ChakraHost.Hosting
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Native interfaces.
    /// </summary>
    public static class Native
    {
        [DllImport("Chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateRuntime(JavaScriptRuntimeAttributes attributes, JavaScriptThreadServiceCallback threadService, out JavaScriptRuntime runtime);

        [DllImport("Chakra.dll")]
        internal static extern JavaScriptErrorCode JsCollectGarbage(JavaScriptRuntime handle);

        [DllImport("Chakra.dll")]
        internal static extern JavaScriptErrorCode JsDisposeRuntime(JavaScriptRuntime handle);

        [DllImport("Chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetRuntimeMemoryUsage(JavaScriptRuntime runtime, out UIntPtr memoryUsage);

        [DllImport("Chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetRuntimeMemoryLimit(JavaScriptRuntime runtime, out UIntPtr memoryLimit);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetRuntimeMemoryLimit(JavaScriptRuntime runtime, UIntPtr memoryLimit);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetRuntimeMemoryAllocationCallback(JavaScriptRuntime runtime, IntPtr callbackState, JavaScriptMemoryAllocationCallback allocationCallback);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetRuntimeBeforeCollectCallback(JavaScriptRuntime runtime, IntPtr callbackState, JavaScriptBeforeCollectCallback beforeCollectCallback);

        [DllImport("chakra.dll", EntryPoint = "JsAddRef")]
        internal static extern JavaScriptErrorCode JsContextAddRef(JavaScriptContext reference, out uint count);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsAddRef(JavaScriptValue reference, out uint count);

        [DllImport("chakra.dll", EntryPoint = "JsRelease")]
        internal static extern JavaScriptErrorCode JsContextRelease(JavaScriptContext reference, out uint count);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsRelease(JavaScriptValue reference, out uint count);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateContext(JavaScriptRuntime runtime, out JavaScriptContext newContext);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetCurrentContext(out JavaScriptContext currentContext);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetCurrentContext(JavaScriptContext context);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetRuntime(JavaScriptContext context, out JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsStartDebugging();

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsIdle(out uint nextIdleTick);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsParseScript(string script, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsRunScript(string script, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsSerializeScript(string script, byte[] buffer, ref ulong bufferSize);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsParseSerializedScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsRunSerializedScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsGetPropertyIdFromName(string name, out JavaScriptPropertyId propertyId);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsGetPropertyNameFromId(JavaScriptPropertyId propertyId, out string name);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetUndefinedValue(out JavaScriptValue undefinedValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetNullValue(out JavaScriptValue nullValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetTrueValue(out JavaScriptValue trueValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetFalseValue(out JavaScriptValue falseValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsBoolToBoolean(bool value, out JavaScriptValue booleanValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsBooleanToBool(JavaScriptValue booleanValue, out bool boolValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsConvertValueToBoolean(JavaScriptValue value, out JavaScriptValue booleanValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetValueType(JavaScriptValue value, out JavaScriptValueType type);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsDoubleToNumber(double doubleValue, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsIntToNumber(int intValue, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsNumberToDouble(JavaScriptValue value, out double doubleValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsConvertValueToNumber(JavaScriptValue value, out JavaScriptValue numberValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetStringLength(JavaScriptValue sringValue, out int length);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsPointerToString(string value, UIntPtr stringLength, out JavaScriptValue stringValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsStringToPointer(JavaScriptValue value, out IntPtr stringValue, out UIntPtr stringLength);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsConvertValueToString(JavaScriptValue value, out JavaScriptValue stringValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsVariantToValue([MarshalAs(UnmanagedType.Struct)] ref object var, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsValueToVariant(JavaScriptValue obj, [MarshalAs(UnmanagedType.Struct)] out object var);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetGlobalObject(out JavaScriptValue globalObject);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateObject(out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateExternalObject(IntPtr data, JavaScriptObjectFinalizeCallback finalizeCallback, out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsConvertValueToObject(JavaScriptValue value, out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetPrototype(JavaScriptValue obj, out JavaScriptValue prototypeObject);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetPrototype(JavaScriptValue obj, JavaScriptValue prototypeObject);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetExtensionAllowed(JavaScriptValue obj, out bool value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsPreventExtension(JavaScriptValue obj);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetOwnPropertyDescriptor(JavaScriptValue obj, JavaScriptPropertyId propertyId, out JavaScriptValue propertyDescriptor);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetOwnPropertyNames(JavaScriptValue obj, out JavaScriptValue propertyNames);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, JavaScriptValue value, bool useStrictRules);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsHasProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, out bool hasProperty);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsDeleteProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, bool useStrictRules, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsDefineProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, JavaScriptValue propertyDescriptor, out bool result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsHasIndexedProperty(JavaScriptValue obj, JavaScriptValue index, out bool result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetIndexedProperty(JavaScriptValue obj, JavaScriptValue index, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetIndexedProperty(JavaScriptValue obj, JavaScriptValue index, JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsDeleteIndexedProperty(JavaScriptValue obj, JavaScriptValue index);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsEquals(JavaScriptValue obj1, JavaScriptValue obj2, out bool result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsStrictEquals(JavaScriptValue obj1, JavaScriptValue obj2, out bool result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsHasExternalData(JavaScriptValue obj, out bool value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetExternalData(JavaScriptValue obj, out IntPtr externalData);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetExternalData(JavaScriptValue obj, IntPtr externalData);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateArray(uint length, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCallFunction(JavaScriptValue function, JavaScriptValue[] arguments, ushort argumentCount, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsConstructObject(JavaScriptValue function, JavaScriptValue[] arguments, ushort argumentCount, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateFunction(JavaScriptNativeFunction nativeFunction, IntPtr externalData, out JavaScriptValue function);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateRangeError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateReferenceError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateSyntaxError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateTypeError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateURIError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsHasException(out bool hasException);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetAndClearException(out JavaScriptValue exception);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetException(JavaScriptValue exception);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsDisableRuntimeExecution(JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsEnableRuntimeExecution(JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsIsRuntimeExecutionDisabled(JavaScriptRuntime runtime, out bool isDisabled);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetObjectBeforeCollectCallback(JavaScriptValue reference, IntPtr callbackState, JavaScriptObjectBeforeCollectCallback beforeCollectCallback);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateNamedFunction(JavaScriptValue name, JavaScriptNativeFunction nativeFunction, IntPtr callbackState, out JavaScriptValue function);

        [DllImport("chakra.dll", CharSet = CharSet.Unicode)]
        internal static extern JavaScriptErrorCode JsProjectWinRTNamespace(string namespaceName);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsInspectableToObject(System.Object inspectable, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetProjectionEnqueueCallback(JavaScriptProjectionEnqueueCallback projectionEnqueueCallback, IntPtr context);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetPromiseContinuationCallback(JavaScriptPromiseContinuationCallback promiseContinuationCallback, IntPtr callbackState);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateArrayBuffer(uint byteLength, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateTypedArray(JavaScriptTypedArrayType arrayType, JavaScriptValue arrayBuffer, uint byteOffset,
            uint elementLength, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateDataView(JavaScriptValue arrayBuffer, uint byteOffset, uint byteOffsetLength, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetArrayBufferStorage(JavaScriptValue arrayBuffer, out byte[] buffer, out uint bufferLength);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetTypedArrayStorage(JavaScriptValue typedArray, out byte[] buffer, out uint bufferLength, out JavaScriptTypedArrayType arrayType, out int elementSize);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetDataViewStorage(JavaScriptValue dataView, out byte[] buffer, out uint bufferLength);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetPropertyIdType(JavaScriptPropertyId propertyId, out JavaSciptPropertyIdType propertyIdType);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsCreateSymbol(JavaScriptValue description, out JavaScriptValue symbol);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetSymbolFromPropertyId(JavaScriptPropertyId propertyId, out JavaScriptValue symbol);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetPropertyIdFromSymbol(JavaScriptValue symbol, out JavaScriptPropertyId propertyId);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetOwnPropertySymbols(JavaScriptValue obj, out JavaScriptValue propertySymbols);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsNumberToInt(JavaScriptValue value, out int intValue);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsSetIndexedPropertiesToExternalData(JavaScriptValue obj, IntPtr data, JavaScriptTypedArrayType arrayType, uint elementLength);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsGetIndexedPropertiesExternalData(JavaScriptValue obj, IntPtr data, out JavaScriptTypedArrayType arrayType, out uint elementLength);

        [DllImport("chakra.dll")]
        internal static extern JavaScriptErrorCode JsHasIndexedPropertiesExternalData(JavaScriptValue obj, out bool value);
    }
}
