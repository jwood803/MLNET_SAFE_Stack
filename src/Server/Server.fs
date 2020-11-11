module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared
open Microsoft.ML
open Microsoft.ML.Data

type Prediction () =
    let context = MLContext()

    let (model, _) = context.Model.Load("./MLModel/salary-model.zip")

    let predictionEngine = context.Model.CreatePredictionEngine<SalaryInput, SalaryPrediction>(model)

    member __.PredictSalary yearsOfExperience =
        let predictedSalary = predictionEngine.Predict { YearsExperience = yearsOfExperience; Salary = 0.0f }

        predictedSalary

let prediction = Prediction()

let predictionApi = { getSalaryPrediction =
    fun yearsOfExperience -> async {
        let prediction = prediction.PredictSalary yearsOfExperience
        match prediction with
        | p when p.PredictedSalary > 0.0f -> return p.PredictedSalary.ToString("C")
        | _ -> return "0"
    } }

let webApp =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue predictionApi
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app
