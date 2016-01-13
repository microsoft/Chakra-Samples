Imports System.Runtime.InteropServices

Namespace Hosting
    ''' <summary>
    '''     A function callback.
    ''' </summary>
    ''' <param name="callee">
    '''     A <c>Function</c> object that represents the function being invoked.
    ''' </param>
    ''' <param name="isConstructCall">Indicates whether this is a regular call or a 'new' call.</param>
    ''' <param name="arguments">The arguments to the call.</param>
    ''' <param name="argumentCount">The number of arguments.</param>
    ''' <param name="callbackData">The callback data, if any.</param>
    ''' <returns>The result of the call, if any.</returns>
    Public Delegate Function JavaScriptNativeFunction(callee As JavaScriptValue, isConstructCall As Boolean, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=3)> arguments As JavaScriptValue(), argumentCount As UShort, callbackData As IntPtr) As JavaScriptValue
End Namespace
