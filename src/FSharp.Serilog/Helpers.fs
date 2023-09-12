/// <summary>Some helpers for working with Serilog in F#.</summary>
///
[<AutoOpen>]
module FSharp.Serilog.Helpers

open Serilog

/// <summary>Calls the <paramref name="f"/> function, logging the events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="level">The level of the <paramref name="f"/> function start and end events.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
[<CompiledName("WrapBlockToLog")>]
let wrapBlockToLog
    (logger: ILogger)
    (level: Events.LogEventLevel)
    (blockName: string)
    (paramProjector: 'a -> 'p)
    (f: 'a -> 'b)
    (x: 'a)
    =
    if logger.IsEnabled(level) then
        let param = paramProjector x

        logger.Write(
            level,
            """>>> The block "{BlockName}" ("{BlockParam}") started""",
            blockName,
            param
        )

        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()

        let res = f x

        logger.Write(
            level,
            """<<< The block "{BlockName}" ("{BlockParam}") completed, elapsed time: {Time:hh\:mm\:ss\.ffffff}""",
            blockName,
            param,
            timer.Elapsed
        )

        res
    else
        f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Verbose</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogVerbose logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Verbose blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Debug</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogDebug logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Debug blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Information</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogInformation logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Information blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Warning</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogWarning logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Warning blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Error</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogError logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Error blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Fatal</c> level events of the function start and end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToLogFatal logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Fatal blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function start and end events.</param>
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLog level blockName paramProjector f x =
    wrapBlockToLog Log.Logger level blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Verbose</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogVerbose blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Verbose blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Debug</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogDebug blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Debug blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Information</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogInformation blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Information blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Warning</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogWarning blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Warning blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Error</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogError blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Error blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Fatal</c> level events of the function start and end using the global logger, as well as its execution time.</summary>
///
/// <param name="blockName">The friendly name of the <paramref name="f"/> function to use in the events of the function start and end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function start and end events.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let inline wrapBlockToGlobalLogFatal blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Fatal blockName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
[<CompiledName("WriteOnCompletionToLog")>]
let writeOnCompletionToLog
    (logger: ILogger)
    (level: Events.LogEventLevel)
    (operationName: string)
    (paramProjector: 'a -> 'p)
    (f: 'a -> 'b)
    (x: 'a)
    =
    if logger.IsEnabled(level) then
        let param = paramProjector x
        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()

        let res = f x

        logger.Write(
            level,
            """The operation "{OperationName}" ("{OperationParam}") completed, elapsed time: {Time:hh\:mm\:ss\.ffffff}""",
            operationName,
            param,
            timer.Elapsed
        )

        res
    else
        f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Verbose</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogVerbose logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Verbose operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Debug</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogDebug logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Debug operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Information</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogInformation logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Information operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Warning</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogWarning logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Warning operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Error</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogError logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Error operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Fatal</c> level event of the function end, as well as its execution time.</summary>
///
/// <param name="logger">The Logger.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToLogFatal logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Fatal operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLog level operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger level operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Verbose</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogVerbose operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Verbose operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Debug</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogDebug operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Debug operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Information</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogInformation operationName paramProjector f x =
    writeOnCompletionToLog
        Log.Logger
        Events.LogEventLevel.Information
        operationName
        paramProjector
        f
        x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Warning</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogWarning operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Warning operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Error</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogError operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Error operationName paramProjector f x

/// <summary>Calls the <paramref name="f"/> function, logging the <c>Fatal</c> level event of the function end using the global logger, as well as its execution time.</summary>
///
/// <param name="level">The level of the <paramref name="f"/> function end event.</param>
/// <param name="operationName">The friendly name of the <paramref name="f"/> function to use in the event of the function end.</param>
/// <param name="paramProjector">The function to get the clear value of the <paramref name="x"/> parameter to use in the <paramref name="f"/> function end event.</param>
/// <param name="f">The called function.</param>
/// <param name="x">The parameter to be passed to the <paramref name="f"/> function.</param>
/// <returns>The result of calling function <paramref name="f"/> with parameter <paramref name="x"/></returns>
let writeOnCompletionToGlobalLogFatal operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Fatal operationName paramProjector f x
