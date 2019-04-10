module TopToppings

open System
open FSharp.Data

[<Literal>]
let pizzaOrdersUrl = @"http://files.olo.com/pizzas.json"
type OrderData = JsonProvider<pizzaOrdersUrl>

let orders = OrderData.GetSamples() |> Array.map( fun o -> o.Toppings )

let rank = fun (count : int, total : int) -> Math.Round ( ( ( count |> double ) / ( total |> double ) ) * 100.0, 2 )
let key = fun ( key : string[] ) -> key |> Array.sort |> String.concat ", "

let orderStats = fun ( orders : string[][] ) ->
               let orderCount = orders.Length
               orders
               |> Seq.countBy ( fun order -> key ( order ) )
               |> Seq.sortBy ( fun ( key, count ) -> -count )
               |> Seq.map ( fun ( key, count ) -> ( key, count, rank ( count, orderCount ) ) )
               |> Seq.truncate 20

[<EntryPoint>]
for i in orderStats( orders ) do
    let ( key, count, rank ) = i
    printfn "Toppings: %A, # Times Ordered: %A, Rank: %A" key count rank

