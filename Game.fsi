namespace FPGame
    module Game =
        val set_strategy: int -> (List<int> -> List<int>)
        val play_as_computer: (List<int> * (inref<List<int> -> List<int>>)) -> List<int>
        val is_victory: List<int> -> bool
        val loop: (List<int> * int * int) -> unit
