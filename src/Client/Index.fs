module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model =
    { Input: string
      PredictedSalary: string }

type Msg =
    | SetInput of string
    | PredictSalary
    | PredictedSalary of string

let predictionApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ISalaryPrediction>

let init(): Model * Cmd<Msg> =
    let model =
        { Input = ""
          PredictedSalary = "" }
    model, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | SetInput value ->
        { model with Input = value }, Cmd.none
    | PredictSalary ->
        let salary = float32 model.Input
        let cmd = Cmd.OfAsync.perform predictionApi.getSalaryPrediction salary PredictedSalary
        { model with Input = "" }, cmd
    | PredictedSalary newSalary ->
        { model with PredictedSalary = newSalary }, Cmd.none

open Fable.React
open Fable.React.Props
open Fulma

let containerBox (model : Model) (dispatch : Msg -> unit) =
    Box.box' [ ] [
        Content.content [ ] [
            div [ ] [ label [ ] [ if not (System.String.IsNullOrWhiteSpace model.PredictedSalary) then sprintf "Predicted salary: %s" model.PredictedSalary |> str ]]
        ]
        Field.div [ Field.IsGrouped ] [
            Control.p [ Control.IsExpanded ] [
                Input.text [
                  Input.Value model.Input
                  Input.Placeholder "How many years of experience?"
                  Input.OnChange (fun x -> SetInput x.Value |> dispatch) ]
            ]
            Control.p [ ] [
                Button.a [
                    Button.Color IsPrimary
                    Button.Disabled (Todo.isValid model.Input |> not)
                    Button.OnClick (fun _ -> dispatch PredictSalary)
                ] [
                    str "Add"
                ]
            ]
        ]
    ]

let view (model : Model) (dispatch : Msg -> unit) =
    Hero.hero [
        Hero.Color IsPrimary
        Hero.IsFullHeight
        Hero.Props [
            Style [
                Background """linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://unsplash.it/1200/900?random") no-repeat center center fixed"""
                BackgroundSize "cover"
            ]
        ]
    ] [
        Hero.body [ ] [
            Container.container [ ] [
                Column.column [
                    Column.Width (Screen.All, Column.Is6)
                    Column.Offset (Screen.All, Column.Is3)
                ] [
                    Heading.p [ Heading.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [ str "ML.NET Salary Prediction" ]
                    containerBox model dispatch
                ]
            ]
        ]
    ]
