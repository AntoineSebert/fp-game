namespace FPGame
    module GUI =
        val create_gui: unit -> unit
        val win: bool -> Async
        val get_configuration: unit -> List<int> * int
        val update: List<int> -> unit
        val play: List<int> -> List<int>
