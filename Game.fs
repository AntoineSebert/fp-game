(*
https://docs.microsoft.com/fr-fr/dotnet/fsharp/language-reference/interfaces
https://docs.microsoft.com/fr-fr/dotnet/fsharp/language-reference/object-expressions
https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/asynchronous-and-concurrent-programming/async
*)
module Game

open System
open System
open System.Windows
open System.Windows.Data
open System.Windows.Input
open System.Windows.Controls.Primitives
open FsXaml

let set_strategy(difficulty: int): (List<int> -> List<int>) =
    let rand = System.Random()

    let random = fun (board: List<int>) ->
        let select = board.Item (rand.Next(List.length board))
        let updated = List.mapi (fun e i -> if i = select then rand.Next(e) else e) board
        updated

    let optimal = fun (board: List<int>) ->
        List.mapi (fun e i ->
            match List.reduce (fun acc elem -> acc ^^^ elem) board with
            | 0      -> if i = (List.findIndex (fun e -> e = List.max board) board) then e - 1 else e
            | result -> if i = (List.findIndexBack (fun el -> el <> 0) board) then e ^^^ result else e
        ) board

    match difficulty with
    | 0 -> random
    | 1 -> fun(board: List<int>) -> (if rand.Next(2) = 1 then optimal else random) board
    | _ -> optimal

let play_as_computer (board: List<int>, strategy: inref<List<int> -> List<int>>): List<int> = strategy board

let is_victory (board: List<int>): bool = List.forall (fun e -> e = 0) board
    
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
            else FPGame.GUI.play board
    
        ignore(FPGame.GUI.update board)

        // check for finished game
        if is_victory result (**) || 5 < turns (**) then
            ignore(FPGame.GUI.win ((turns % 2) = 0))
            finished <- true

    finished

type Game = XAML<"Game.xaml">
type Res = XAML<"GameResources.xaml">

[<EntryPoint;STAThread>]
let main argv =
    FPGame.GUI.create_gui()
    let (board: List<int>, difficulty: int) = FPGame.GUI.get_configuration()
    let game = Game()

    let res = Res()
    let conv = res.validationConverter

    //ignore(loop(board, difficulty))

    game.Run()
