namespace Shared

open System

type Todo =
    { Id : Guid
      Description : string }

type SalaryInput = { YearsOfExperience: float32 }
type SalaryPrediction = { PredictedSalary: float32 }

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

type ISalaryPrediction = { getSalaryPrediction: float32 -> Async<float32> }