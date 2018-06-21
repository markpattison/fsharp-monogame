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
    let mutable showParameters = false

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

        do base.Update(gameTime)
    
    override _this.Draw(gameTime) =
        let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

        do device.Clear(Color.CornflowerBlue)

        //effect.CurrentTechnique <- effect.Techniques.[effectName]

        //effect.CurrentTechnique.Passes |> Seq.iter
        //    (fun pass ->
        //        pass.Apply()
        //        device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3)
        //    )

        if showParameters then ShowParameters()

        do base.Draw(gameTime)




