module Db

open Microsoft.EntityFrameworkCore

[<AllowNullLiteral>]
type Todo() =
    member val Id = 0 with get, set
    member val Name : string option = None with get, set
    member val IsComplete = false with get, set

type TodoDb(options: DbContextOptions<TodoDb>) =
    inherit DbContext(options)

    member val Todos = base.Set<Todo>() with get
