open System.Linq
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Db
open MinimalFSharpExtensions

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder args
    builder.Services.AddDbContext<TodoDb>(fun opt -> opt.UseInMemoryDatabase("TodoList") |> ignore) |> ignore
    builder.Services.AddDatabaseDeveloperPageExceptionFilter() |> ignore

    builder.Build()

    |> get "/" 
        (fun () -> "Hello World!")

    |> get1 "/todoitems" 
        (fun (db: TodoDb) -> task { 
            return! db.Todos.ToListAsync() 
        })

    |> get1 "/todoitems/complete"
        (fun (db: TodoDb) -> task { 
            return! db.Todos.Where(fun t -> t.IsComplete).ToListAsync() 
        })

    |> get2 "/todoitems/{id}"
        (fun (id: int) (db: TodoDb) -> task {
            let! x = db.Todos.FindAsync id
            match x with
            | null -> return Results.NotFound ()
            | x -> return Results.Ok x
        })

    |> post2 "/todoitems"
        (fun (todo: Todo) (db: TodoDb) -> task {            
            db.Todos.Add(todo) |> ignore
            let! _ = db.SaveChangesAsync()
            return Results.Created($"/todoitems/{todo.Id}", todo)        
        })

    |> put3 "/todoitems/{id}"
        (fun (id: int) (inputTodo: Todo) (db: TodoDb) -> task {
            let! todo = db.Todos.FindAsync id
            match todo with
            | null -> return Results.NotFound()
            | _ ->
                todo.Name <- inputTodo.Name
                todo.IsComplete <- inputTodo.IsComplete
                let! _ = db.SaveChangesAsync()
                return Results.NoContent()
        })

    |> delete2 "/todoitems/{id}"
        (fun (id: int) (db: TodoDb) -> task {
            let! todo = db.Todos.FindAsync id
            match todo with
            | null -> return Results.NotFound()
            | _ -> 
                db.Todos.Remove todo |> ignore
                let! _ = db.SaveChangesAsync()
                return Results.Ok todo
        })

    |> run
