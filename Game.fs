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
    | 1 -> fun(board: List<int>) -> (if rand.Next(3) = 0 then optimal else random) board
    | _ -> optimal

let play_as_computer (board: List<int>, strategy: inref<List<int> -> List<int>>): List<int> = strategy board

let is_victory (board: List<int>): bool = List.forall (fun e -> e = 0) board
    
let rec loop (board: List<int>, difficulty: int, turns: int): unit =
    let strategy = set_strategy difficulty

    let result: List<int> =
        if (turns % 2) = 0
        then play_as_computer(board, &strategy)
        else FPGame.GUI.play board

    ignore(FPGame.GUI.update board)

    match (is_victory result) || (50 < turns) with
    | false -> loop(board, difficulty, (turns+1))
    | true  -> ignore(FPGame.GUI.win ((turns % 2) = 0))

type Game = XAML<"Game.xaml">
type Res = XAML<"GameResources.xaml">

[<EntryPoint;STAThread>]
let main argv =
    FPGame.GUI.create_gui()
    let (board: List<int>, difficulty: int) = FPGame.GUI.get_configuration()

    let game = Game()

    let res = Res()
    let conv = res.validationConverter

    ignore(loop(board, difficulty, 0))

    game.Run()
