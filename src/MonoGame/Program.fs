module FSharpMonogame.Program

[<EntryPoint>]
let Main args =
    let game = new Sample()
    do game.Run()
    0