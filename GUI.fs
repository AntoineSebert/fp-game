module FPGame.GUI

open System.Windows

type Name = string
type Event = Name
type State = Name
type Action = Name
type Transition = { OldState: State; Event: Event; NewState: State; Actions: Action list }
type Header = Header of Name * Name

// view to model
let create_gui () = async {return ()}

// model to view
let win (player: bool) =
    if player = false
    then MessageBox.Show("You have won")
    else MessageBox.Show("You have lost")

// controller to model
let get_configuration(): List<int> * int =
    let result = async { return [2;3;6], 0 }

    result
    |> Async.RunSynchronously // replace by UI bindings

// model to view
let update(board: List<int>) = async {return ()}

// controller to model
let play(board: List<int>): List<int> =
    let result = async { return List.Empty }
    
    result
    |> Async.RunSynchronously // replace by UI bindings
