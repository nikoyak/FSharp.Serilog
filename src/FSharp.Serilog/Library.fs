[<AutoOpen>]
module FSharp.Serilog.Helpers

open Serilog

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

let inline wrapBlockToLogVerbose logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Verbose blockName paramProjector f x

let inline wrapBlockToLogDebug logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Debug blockName paramProjector f x

let inline wrapBlockToLogInformation logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Information blockName paramProjector f x

let inline wrapBlockToLogWarning logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Warning blockName paramProjector f x

let inline wrapBlockToLogError logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Error blockName paramProjector f x

let inline wrapBlockToLogFatal logger blockName paramProjector f x =
    wrapBlockToLog logger Events.LogEventLevel.Fatal blockName paramProjector f x

let inline wrapBlockToGlobalLog level blockName paramProjector f x =
    wrapBlockToLog Log.Logger level blockName paramProjector f x

let inline wrapBlockToGlobalLogVerbose blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Verbose blockName paramProjector f x

let inline wrapBlockToGlobalLogDebug blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Debug blockName paramProjector f x

let inline wrapBlockToGlobalLogInformation blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Information blockName paramProjector f x

let inline wrapBlockToGlobalLogWarning blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Warning blockName paramProjector f x

let inline wrapBlockToGlobalLogError blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Error blockName paramProjector f x

let inline wrapBlockToGlobalLogFatal blockName paramProjector f x =
    wrapBlockToLog Log.Logger Events.LogEventLevel.Fatal blockName paramProjector f x

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
            """The operation "{OperationName}" ("{OperaionParam}") completed, elapsed time: {Time:hh\:mm\:ss\.ffffff}""",
            operationName,
            param,
            timer.Elapsed
        )

        res
    else
        f x

let writeOnCompletionToLogVerbose logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Verbose operationName paramProjector f x

let writeOnCompletionToLogDebug logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Debug operationName paramProjector f x

let writeOnCompletionToLogInformation logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Information operationName paramProjector f x

let writeOnCompletionToLogWarning logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Warning operationName paramProjector f x

let writeOnCompletionToLogError logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Error operationName paramProjector f x

let writeOnCompletionToLogFatal logger operationName paramProjector f x =
    writeOnCompletionToLog logger Events.LogEventLevel.Fatal operationName paramProjector f x

let writeOnCompletionToGlobalLog level operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger level operationName paramProjector f x

let writeOnCompletionToGlobalLogVerbose operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Verbose operationName paramProjector f x

let writeOnCompletionToGlobalLogDebug operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Debug operationName paramProjector f x

let writeOnCompletionToGlobalLogInformation operationName paramProjector f x =
    writeOnCompletionToLog
        Log.Logger
        Events.LogEventLevel.Information
        operationName
        paramProjector
        f
        x

let writeOnCompletionToGlobalLogWarning operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Warning operationName paramProjector f x

let writeOnCompletionToGlobalLogError operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Error operationName paramProjector f x

let writeOnCompletionToGlobalLogFatal operationName paramProjector f x =
    writeOnCompletionToLog Log.Logger Events.LogEventLevel.Fatal operationName paramProjector f x
