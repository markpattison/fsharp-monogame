namespace FSharpMonogame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Input

type Program() as _this =
    inherit Game()
    let mutable device = Unchecked.defaultof<GraphicsDevice>
    let mutable input = Unchecked.defaultof<Input>
    let mutable gameContent = Unchecked.defaultof<Sample.Content>
    let mutable gameState = Unchecked.defaultof<Sample.State>

    let resetMouseEachFrame = DoNotReset

    let graphics = new GraphicsDeviceManager(_this)
    do graphics.GraphicsProfile <- GraphicsProfile.HiDef
    do graphics.PreferredBackBufferWidth <- 600
    do graphics.PreferredBackBufferHeight <- 480
    do graphics.IsFullScreen <- false
    do graphics.ApplyChanges()
    do base.Content.RootDirectory <- "Content"
    
    override _this.Initialize() =
        device <- base.GraphicsDevice
        base.Initialize()
    
    override _this.LoadContent() =
        let gameWindow = _this.Window
        if resetMouseEachFrame = Reset && not (obj.ReferenceEquals(gameWindow, null)) then
            Mouse.SetPosition(gameWindow.ClientBounds.Width / 2, gameWindow.ClientBounds.Height / 2)
        
        input <- Input(Keyboard.GetState(), Keyboard.GetState(), Mouse.GetState(), Mouse.GetState(), _this.Window, Mouse.GetState(), 0, 0, resetMouseEachFrame)

        gameContent <- Sample.loadContent _this device graphics
        gameState <- Sample.initialState
    
    override _this.Update(gameTime) =
        input <- input.Updated(Keyboard.GetState(), Mouse.GetState(), _this.Window)

        gameState <- Sample.update input gameContent gameTime gameState

        do base.Update(gameTime)
    
    override _this.Draw(gameTime) =
        Sample.draw device gameContent gameState gameTime

        do base.Draw(gameTime)

module EntryPoint =
    [<EntryPoint>]
    let Main args =
        let game = new Program()
        do game.Run()
        0