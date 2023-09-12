namespace FSharp.Serilog.Tests

open System
open System.Collections.Generic
open Serilog
open Serilog.Events
open Serilog.Core
open Expecto
open FSharp.Serilog.Helpers

type FunSink(cb: LogEvent -> unit) =
    interface ILogEventSink with
        member _.Emit(event: LogEvent) = cb event

module Tests =
    [<Tests>]
    let tests =
        testList "helpers tests" [
            let creareFixture () =
                let events = ResizeArray<LogEvent>()
                let sink = FunSink(events.Add)

                let logger =
                    LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Sink(sink).CreateLogger()

                let double i =
                    let res = i * 2
                    logger.Verbose("double({Param}) = {Result}", i, res)
                    res

                logger, events, double

            testCase "wrapBlockToLog test"
            <| fun _ ->
                let logger, events, double = creareFixture ()
                use logger = logger
                let blockEventLevel = LogEventLevel.Information
                let blockName = "Block1"

                let wrappedDouble =
                    wrapBlockToLog logger blockEventLevel blockName (fun p -> $"x{p}y") double

                let res = wrappedDouble 8

                Expect.equal
                    res
                    16
                    "the wrapped function must return the result of the original function"

                Expect.equal events.Count 3 "should be three messages"

                Expect.equal
                    events[0].Level
                    blockEventLevel
                    $"the message that the block has started should be at the {blockEventLevel} level"

                Expect.equal
                    (events[0].Properties["BlockName"].ToString())
                    $"\"{blockName}\""
                    $"the block name in the start message must be correct"

                Expect.equal
                    (events[0].Properties["BlockParam"].ToString())
                    "\"x8y\""
                    $"the block parameter in the start message must be correct"

                Expect.equal
                    events[2].Level
                    blockEventLevel
                    $"the message that the block has completed should be at the {blockEventLevel} level"

                Expect.equal
                    (events[2].Properties["BlockName"].ToString())
                    $"\"{blockName}\""
                    $"the block name in the completion message must be correct"

                Expect.equal
                    (events[2].Properties["BlockParam"].ToString())
                    "\"x8y\""
                    $"the block parameter in the completion message must be correct"

                Expect.equal
                    (events[1].Properties.ContainsKey("Result"))
                    true
                    $"the block is executed between the start and finish messages"

            testCase "writeOnCompletionToLog test"
            <| fun _ ->
                let logger, events, double = creareFixture ()
                use logger = logger
                let completionEventLevel = LogEventLevel.Information
                let operationName = "Operation1"

                let wrappedDouble =
                    writeOnCompletionToLog
                        logger
                        completionEventLevel
                        operationName
                        (fun p -> $"x{p}y")
                        double

                let res = wrappedDouble 8

                Expect.equal
                    res
                    16
                    "the wrapped function must return the result of the original function"

                Expect.equal events.Count 2 "should be two messages"

                Expect.equal
                    events[1].Level
                    completionEventLevel
                    $"the message that the operation has completed should be at the {completionEventLevel} level"

                Expect.equal
                    (events[1].Properties["OperationName"].ToString())
                    $"\"{operationName}\""
                    $"the operation name must be correct"

                Expect.equal
                    (events[1].Properties["OperationParam"].ToString())
                    "\"x8y\""
                    $"the operation parameter  must be correct"

                Expect.equal
                    (events[0].Properties.ContainsKey("Result"))
                    true
                    $"the operation completion message should occur after the operation itself has completed"


        //Expect.equal events[0].MessageTemplate 1 "should be 1 messages"

        // var logger = new LoggerConfiguration()
        //     .Destructure.With(new ProjectedDestructuringPolicy(
        //         canApply: t => typeof(Type).IsAssignableFrom(t),
        //         projection: o => ((Type)o).AssemblyQualifiedName!))
        //     .WriteTo.Sink(sink)
        //     .CreateLogger();

        // var thisType = GetType();
        // logger.Information("{@thisType}", thisType);

        // var ev = events.Single();
        // var prop = ev.Properties["thisType"];
        // var sv = Assert.IsAssignableFrom<ScalarValue>(prop);
        // Assert.Equal(thisType.AssemblyQualifiedName, sv.LiteralValue());

        // let subject = Say.add 1 2
        // Expect.equal subject 3 "Addition works"
        ]
