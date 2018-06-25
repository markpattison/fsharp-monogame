module FSharpMonogame.Sample

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Input

type Content =
    {
        SpriteBatch: SpriteBatch
        SpriteFont: SpriteFont
        Effect: Effect
        WidthOverHeight: single
        Vertices: VertexPositionColor []
    }

let loadContent (_this: Game) device (graphics: GraphicsDeviceManager) =
    {
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

type State =
    {
        ShowParameters: bool
        Exiting: bool
    }

let initialState =
    { ShowParameters = false; Exiting = false }

let update (input: Input) gameContent (gameTime: GameTime) gameState =
    let time = float32 gameTime.TotalGameTime.TotalSeconds

    { gameState with
        ShowParameters = if input.JustPressed(Keys.P) then not gameState.ShowParameters else gameState.ShowParameters
        Exiting = input.Quit
    }

let showParameters gameContent =
    let colour = Color.DarkSlateGray

    gameContent.SpriteBatch.Begin()
    gameContent.SpriteBatch.DrawString(gameContent.SpriteFont, "fsharp-MonoGame", new Vector2(0.0f, 0.0f), colour)
    gameContent.SpriteBatch.End()

let draw (device: GraphicsDevice) gameContent gameState (gameTime: GameTime) =
    let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

    do device.Clear(Color.LightGray)

    gameContent.Effect.CurrentTechnique <- gameContent.Effect.Techniques.["Coloured"]

    gameContent.Effect.CurrentTechnique.Passes |> Seq.iter
        (fun pass ->
            pass.Apply()
            device.DrawUserPrimitives(PrimitiveType.TriangleList, gameContent.Vertices, 0, 1)
        )

    if gameState.ShowParameters then showParameters gameContent
