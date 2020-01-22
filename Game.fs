(*
https://docs.microsoft.com/fr-fr/dotnet/fsharp/language-reference/interfaces
https://docs.microsoft.com/fr-fr/dotnet/fsharp/language-reference/object-expressions
https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/asynchronous-and-concurrent-programming/async
*)
module Game

open System
open FsXaml
open FPGame.GUI

let rand = System.Random()

let set_strategy(difficulty: int): (List<int> -> List<int>) =
    let random = fun (board: List<int>) ->
        let select = board.Item (rand.Next(List.length board))
        let updated = List.mapi (fun e i -> if i = select then rand.Next(e) else e) board
        updated

    let optimal = fun (board: List<int>) ->
        match List.reduce (fun acc elem -> acc ^^^ elem) board with
        | 0 ->
            let mutable updated = List.toArray board
            let index = Array.findIndex (fun e -> e = List.max board) updated
            updated.[index] <- updated.[index] - 1
            updated |> Array.toList
        | result ->
            let mutable updated = List.toArray board
            let index = List.findIndexBack (fun e -> e <> 0) board
            updated.[index] <- updated.[index] ^^^ result
            updated |> Array.toList

    match difficulty with
    | 0 -> random
    | 1 -> fun(board: List<int>) -> (if rand.Next(2) = 1 then optimal else random) board
    | _ -> optimal

let play_as_computer (board: List<int>, strategy: inref<List<int> -> List<int>>) =
  strategy board

let is_victory (board: List<int>): bool =
    List.forall (fun e -> e = 0) board
    
let loop (board: List<int>, difficulty: int): bool =
  let mutable finished: bool = false
  let mutable turns: int = 0
  let strategy = set_strategy difficulty

  while not finished do
    turns <- turns + 1

    // play
    let result: List<int> =
        if (turns % 2) = 0
        then play_as_computer(board, &strategy)
        else gui.play
        gui.update board

    // check for finished game
    if is_victory result (**) || 5 < turns (**) then
        finished <- true
        gui.win (turns % 2) = 0

  finished

type Game = XAML<"Game.xaml">

[<EntryPoint;STAThread>]
let main argv =
  let (board: List<int>, difficulty: int, gui) = create_gui().get_configuration()

  Game().Run()

  loop(board, difficulty)

  0
