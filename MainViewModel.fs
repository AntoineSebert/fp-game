namespace ViewModels

open System
open System.Windows
open System.Windows.Input
open System.Windows.Controls

open FsXaml

open ViewModule
open ViewModule.FSharp
open ViewModule.Validation.FSharp

type MainViewModel() as self = 
    inherit ViewModelBase()

    let heaps = self.Factory.Backing(<@ self.Heaps @>, "", notNullOrWhitespace)
    let matches = self.Factory.Backing(<@ self.Matches @>, "", notNullOrWhitespace)
    let difficulty = self.Factory.Backing(<@ self.Difficulty @>, "", notNullOrWhitespace)

    //let canvas = self.Factory.Backing(<@ self._Canvas @>, Canvas)

    let hasValue str = not(System.String.IsNullOrWhiteSpace(str))

    let playCommand =
        self.Factory.CommandSyncParamChecked(
            (fun _ ->
                MessageBox.Show(
                    sprintf "%O\n%c\n%s"
                        (heaps.Value.Chars(heaps.Value.Length - 1))
                        (matches.Value.Chars(matches.Value.Length - 1))
                        (difficulty.Value.Split(' ').[1])
                ) |> ignore
            ), 
            (fun _ -> self.IsValid && hasValue self.Heaps && hasValue self.Matches && hasValue self.Difficulty), 
            [ <@ self.Heaps @> ; <@ self.Matches @> ; <@ self.Difficulty @> ; <@ self.IsValid @>]
        )
    do
        self.DependencyTracker.AddPropertyDependencies(<@@ self.Heaps @@>, [ <@@ self.Matches @@> ; <@@ self.Difficulty @@> ])

    member x.Heaps with get() = heaps.Value and set value = heaps.Value <- value
    member x.Matches with get() = matches.Value and set value = matches.Value <- value
    member x.Difficulty with get() = difficulty.Value and set value = difficulty.Value <- value

    //member x._Canvas with get() = canvas.Value.Children and set element = canvas.Children.Add <- element

    member x.PlayCommand = playCommand
