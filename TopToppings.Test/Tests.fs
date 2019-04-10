module Tests

open FSharp.Data
open TopToppings
open Xunit

let [<Literal>] sampleJson = """[{"toppings":["pepperoni"]},{"toppings":["feta cheese"]},{"toppings":["pepperoni"]},{"toppings":["bacon"]},{"toppings":["sausage","beef"]},{"toppings":["pepperoni"]},{"toppings":["beef"]},{"toppings":["jalapenos"]}]"""
type data = JsonProvider<sampleJson>

[<Fact>]
let ``Calculates rank as (count / total) * 100`` () =
    let expected = ( 1.0 / 2.0 ) * 100.0
    let actual = TopToppings.rank ( 1, 2 )
    Assert.Equal( expected, actual )

[<Fact>]
let ``Calculates key from string[]`` () =
    let expected = "a, b, c"
    let actual = TopToppings.key( [|"a"; "b"; "c"|] )
    Assert.Equal( expected, actual )

[<Fact>]
let ``Key is sorted alpha ascending`` () =
    let expected = "a, b, c"
    let actual = TopToppings.key ( [|"c"; "a"; "b"|])
    Assert.Equal( expected, actual )

[<Fact>]
let ``Stats are grouped by topping, sorted by count desc`` () =
    let orders = data.GetSamples() |> Array.map( fun o -> o.Toppings )
    let expected = [|("pepperoni", 3, 37.5); ("feta cheese", 1, 12.5); ("bacon", 1, 12.5); ("beef, sausage", 1, 12.5); ("beef", 1, 12.5); ("jalapenos", 1, 12.5)|]
    let actual = orderStats( orders )
    Assert.Equal( expected, actual )