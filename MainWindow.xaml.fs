namespace FPGame

open FsXaml
open System.Windows
open System.Drawing
open System.Windows.Input
open System.Windows.Automation
open System.Windows.Controls

type MainWindow = XAML<"MainWindow.xaml">

// Inherited class is MainView, which is referred to/used in MainWindow.xaml directly
type MainView() as self =
    inherit MainWindow()
