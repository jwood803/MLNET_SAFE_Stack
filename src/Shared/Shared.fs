namespace Shared

open System
open Microsoft.ML.Data

type Todo =
    { Id : Guid
      Description : string }

type SalaryInput = {
    YearsExperience: float32
    Salary: float32
}

[<CLIMutable>]
type SalaryPrediction = {
    [<ColumnName("Score")>]
    PredictedSalary: float32
}

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos : unit -> Async<Todo list>
      addTodo : Todo -> Async<Todo> }

type ISalaryPrediction = { getSalaryPrediction: float32 -> Async<string> }