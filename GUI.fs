module FPGame.GUI

open System.Windows

type Name = string
type Event = Name
type State = Name
type Action = Name
type Transition = { OldState: State; Event: Event; NewState: State; Actions: Action list }
type Header = Header of Name * Name

// all is async
let create_gui () =
  ()

// model to view
// async
let win (player: bool) =
    if player = false
    then MessageBox.Show("You have won")
    else MessageBox.Show("You have lost")

// async
let get_configuration(): List<int> * int = List.Empty, 0

// model to view
// async
let update(board: List<int>) = 0

// controller to model
// async
let play(board: List<int>): List<int> = List.Empty
