namespace FSharpMonogame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Input
open System.IO
open System

type Sample() as _this =
    inherit Game()
    let mutable device = Unchecked.defaultof<GraphicsDevice>
    let mutable input = Unchecked.defaultof<Input>
    let mutable originalMouseState = Unchecked.defaultof<MouseState>

    let graphics = new GraphicsDeviceManager(_this)
    do graphics.GraphicsProfile <- GraphicsProfile.HiDef
    do graphics.PreferredBackBufferWidth <- 800
    do graphics.PreferredBackBufferHeight <- 600
    do graphics.IsFullScreen <- false
    do graphics.ApplyChanges()
    do base.Content.RootDirectory <- "Content"
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable spriteFont = Unchecked.defaultof<SpriteFont>

    let mutable effect = Unchecked.defaultof<Effect>
    let mutable widthOverHeight = 0.0f
    let mutable zoom = 0.314f
    let mutable offset = new Vector2(0.0f, 0.0f)
    let mutable showParameters = false

    let getDirectionalInput keyPos keyNeg (input: Input) =
        (if input.IsPressed keyPos then 1.0f else 0.0f) - (if input.IsPressed keyNeg then 1.0f else 0.0f)

    let getLeftRight = getDirectionalInput Keys.Left Keys.Right
    let getDownUp = getDirectionalInput Keys.Down Keys.Up
    let getPageDownPageUp = getDirectionalInput Keys.PageDown Keys.PageUp

    let getInput (input: Input) =
        let scaled = if (input.IsPressed Keys.LeftShift || input.IsPressed Keys.RightShift) then 0.1f else 1.0f

        let move = scaled * 0.005f * new Vector2(getLeftRight input, getDownUp input) / zoom
        let zoomIn = 1.02f ** (scaled * getPageDownPageUp input)

        (-move, zoomIn)

    let ShowParameters() =
        spriteBatch.Begin()

        let colour = Color.Gold

        spriteBatch.DrawString(spriteFont, "Hello", new Vector2(0.0f, 0.0f), colour)

        spriteBatch.End()

    override _this.Initialize() =
        device <- base.GraphicsDevice
        base.Initialize()
    
    override _this.LoadContent() =
        effect <- _this.Content.Load<Effect>("Effects/effects")
        spriteFont <- _this.Content.Load<SpriteFont>("Fonts/Miramo")

        spriteBatch <- new SpriteBatch(device)

        widthOverHeight <- (single graphics.PreferredBackBufferWidth) / (single graphics.PreferredBackBufferHeight)

        //Mouse.SetPosition(_this.Window.ClientBounds.Width / 2, _this.Window.ClientBounds.Height / 2)
        originalMouseState <- Mouse.GetState()
        input <- Input(Keyboard.GetState(), Keyboard.GetState(), Mouse.GetState(), Mouse.GetState(), _this.Window, originalMouseState, 0, 0)
    
    override _this.Update(gameTime) =
        let time = float32 gameTime.TotalGameTime.TotalSeconds

        if input.Quit then _this.Exit()
        if input.JustPressed(Keys.P) then showParameters <- not showParameters

        let (move, zoomIn) = getInput input

        offset <- offset + move
        zoom <- zoom / zoomIn

        do base.Update(gameTime)
    
    override _this.Draw(gameTime) =
        let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

        do device.Clear(Color.CornflowerBlue)

        //effect.CurrentTechnique <- effect.Techniques.[effectName]
        //effect.Parameters.["Zoom"].SetValue(zoom)
        //effect.Parameters.["WidthOverHeight"].SetValue(widthOverHeight)
        //effect.Parameters.["Offset"].SetValue(offset)
        //effect.Parameters.["JuliaSeed"].SetValue(juliaSeed)

        //effect.CurrentTechnique.Passes |> Seq.iter
        //    (fun pass ->
        //        pass.Apply()
        //        device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3)
        //    )

        if showParameters then ShowParameters()

        do base.Draw(gameTime)




