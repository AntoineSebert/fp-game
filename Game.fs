module Game

open System
open FsXaml

type Game = XAML<"Game.xaml">

[<EntryPoint;STAThread>]
let main argv =
    Game().Run()
