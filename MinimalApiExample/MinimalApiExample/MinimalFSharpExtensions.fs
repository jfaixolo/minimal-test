module MinimalFSharpExtensions

open System
open Microsoft.AspNetCore.Builder

let func (f: unit -> 'a) = Func<'a> f
let func1 (f: 'a -> 'b) = Func<'a, 'b> f
let func2 (f: 'a -> 'b -> 'c) = Func<'a, 'b, 'c> f
let func3 (f: 'a -> 'b -> 'c -> 'd) = Func<'a, 'b, 'c, 'd> f

let get (pattern: string) (f: unit -> 'a) (app: WebApplication) =
    app.MapGet(pattern, func f) |> ignore
    app

let get1 (pattern: string) (f: 'a -> 'b) (app: WebApplication) =
    app.MapGet(pattern, func1 f) |> ignore
    app

let get2 (pattern: string) (f: 'a -> 'b -> 'c) (app: WebApplication) =
    app.MapGet(pattern, func2 f) |> ignore
    app

let post2 (pattern: string) (f: 'a -> 'b -> 'c) (app: WebApplication) =
    app.MapPost(pattern, func2 f) |> ignore
    app

let put3 (pattern: string) (f: 'a -> 'b -> 'c -> 'd) (app: WebApplication) =
    app.MapPut(pattern, func3 f) |> ignore
    app

let delete2 (pattern: string) (f: 'a -> 'b -> 'c) (app: WebApplication) =
    app.MapDelete(pattern, func2 f) |> ignore
    app

let run (app: WebApplication) =
    app.Run()
    0