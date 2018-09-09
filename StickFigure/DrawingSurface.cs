using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StickFigure
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DrawingSurface : BeebappsGame
    {
        public DrawingSurface()
        {
            Content.RootDirectory = "Content";


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Current.GraphicsDeviceManager.IsFullScreen = false;
            Current.GraphicsDeviceManager.PreferredBackBufferWidth = Consts.ScreenWidth;
            Current.GraphicsDeviceManager.PreferredBackBufferHeight = Consts.ScreenHeight;
            Current.GraphicsDeviceManager.ApplyChanges();
            //Current.ViewPortSize = new Vector2(Consts.ScreenWidth, Consts.ScreenHeight);
            //Current.GraphicsDevice.Viewport = new Viewport(0,0,Consts.ScreenWidth, Consts.ScreenHeight);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Current.SpriteBatch.Begin();

            Current.SpriteBatch.DrawCircle(new Vector2(50, 150), 40, 100, Color.Black, 5);
            Current.SpriteBatch.DrawCircle(new Vector2(50, 250), 30, 100, Color.Black, 4);
            Current.SpriteBatch.DrawCircle(new Vector2(50, 350), 10, 100, Color.Black, 3);
            Current.SpriteBatch.DrawCircle(new Vector2(50, 450), 10, 100, Color.Red, 2);

            Current.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
