namespace FSharp.Dapr

open Microsoft.Extensions.Logging

[<AutoOpen>]
module Logger =

    let logWarn2 (logger: ILogger) message (arg1: obj) (arg2: obj) = logger.LogWarning(message, arg1, arg2)

    let logWarn3 (logger: ILogger) message (arg1: obj) (arg2: obj) (arg3: obj) =
        logger.LogWarning(message, arg1, arg2, arg3)

    //
    let logTrace (logger: ILogger) message = logger.LogTrace(message)

    let logTrace2 (logger: ILogger) message (arg1: obj) (arg2: obj) = logger.LogTrace(message, arg1, arg2)

    let logTrace3 (logger: ILogger) message (arg1: obj) (arg2: obj) (arg3: obj) =
        logger.LogTrace(message, arg1, arg2, arg3)
