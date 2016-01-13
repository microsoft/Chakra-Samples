Imports System
Imports System.Diagnostics.CodeAnalysis

Namespace Hosting
    ''' <summary>
    '''     Attributes of a runtime.
    ''' </summary>
    <Flags>
    Public Enum JavaScriptRuntimeAttributes
        ''' <summary>
        '''     No special attributes.
        ''' </summary>
        None = &H0

        ''' <summary>
        '''     The runtime will not do any work (such as garbage collection) on background threads.
        ''' </summary>
        DisableBackgroundWork = &H1

        ''' <summary>
        '''     The runtime should support reliable script interruption. This increases the number of
        '''     places where the runtime will check for a script interrupt request at the cost of a
        '''     small amount of runtime performance.
        ''' </summary>
        AllowScriptInterrupt = &H2

        ''' <summary>
        '''     Host will call Idle, so enable idle processing. Otherwise, the runtime will manage
        '''     memory slightly more aggressively.
        ''' </summary>
        EnableIdleProcessing = &H4

        ''' <summary>
        '''     Runtime will not generate native code.
        ''' </summary>
        DisableNativeCodeGeneration = &H8

        ''' <summary>
        '''     Using Eval or Function constructor will throw an exception.
        ''' </summary>
        <SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification:="Eval is a valid function name.")>
        DisableEval = &H10
    End Enum
End Namespace
