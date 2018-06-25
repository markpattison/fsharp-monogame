namespace FSharpMonogame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Input

type Sample() as _this =
    inherit Game()
    let mutable device = Unchecked.defaultof<GraphicsDevice>
    let mutable input = Unchecked.defaultof<Input>
    let mutable gameContent = Unchecked.defaultof<GameContent>
    let mutable gameState = Unchecked.defaultof<GameState>

    let resetMouseEachFrame = DoNotReset

    let graphics = new GraphicsDeviceManager(_this)
    do graphics.GraphicsProfile <- GraphicsProfile.HiDef
    do graphics.PreferredBackBufferWidth <- 600
    do graphics.PreferredBackBufferHeight <- 480
    do graphics.IsFullScreen <- false
    do graphics.ApplyChanges()
    do base.Content.RootDirectory <- "Content"

    let showParameters () =
        let colour = Color.DarkSlateGray

        gameContent.SpriteBatch.Begin()
        gameContent.SpriteBatch.DrawString(gameContent.SpriteFont, "fsharp-MonoGame", new Vector2(0.0f, 0.0f), colour)
        gameContent.SpriteBatch.End()

    override _this.Initialize() =
        device <- base.GraphicsDevice
        base.Initialize()
    
    override _this.LoadContent() =
        
        if resetMouseEachFrame = Reset && not (obj.ReferenceEquals(_this.Window, null)) then
            Mouse.SetPosition(_this.Window.ClientBounds.Width / 2, _this.Window.ClientBounds.Height / 2)
        
        input <- Input(Keyboard.GetState(), Keyboard.GetState(), Mouse.GetState(), Mouse.GetState(), _this.Window, Mouse.GetState(), 0, 0, resetMouseEachFrame)

        gameContent <- {
            Effect = _this.Content.Load<Effect>("Effects/effects")
            SpriteFont = _this.Content.Load<SpriteFont>("Fonts/Arial")
            SpriteBatch = new SpriteBatch(device)
            WidthOverHeight = (single graphics.PreferredBackBufferWidth) / (single graphics.PreferredBackBufferHeight)

            Vertices =
                [|
                    new VertexPositionColor(new Vector3( 0.0f,  0.8f, 0.0f), Color.Red)
                    new VertexPositionColor(new Vector3( 0.8f, -0.8f, 0.0f), Color.Green)
                    new VertexPositionColor(new Vector3(-0.8f, -0.8f, 0.0f), Color.Blue)
                |]
        }

        gameState <- { ShowParameters = false }
    
    override _this.Update(gameTime) =
        let time = float32 gameTime.TotalGameTime.TotalSeconds

        input <- input.Updated(Keyboard.GetState(), Mouse.GetState(), _this.Window)

        if input.Quit then _this.Exit()

        if input.JustPressed(Keys.P) then
            gameState <- { gameState with ShowParameters = not gameState.ShowParameters }

        do base.Update(gameTime)
    
    override _this.Draw(gameTime) =
        let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

        do device.Clear(Color.LightGray)

        gameContent.Effect.CurrentTechnique <- gameContent.Effect.Techniques.["Coloured"]

        gameContent.Effect.CurrentTechnique.Passes |> Seq.iter
            (fun pass ->
                pass.Apply()
                device.DrawUserPrimitives(PrimitiveType.TriangleList, gameContent.Vertices, 0, 1)
            )

        if gameState.ShowParameters then showParameters ()

        do base.Draw(gameTime)




