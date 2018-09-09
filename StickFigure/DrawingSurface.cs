using System.Collections.Generic;
using Beebapps.Game.Input;
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

        private MouseCursor _mouseCursor;
        private JointManager _jointManager;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _mouseCursor = new MouseCursor();
            _jointManager = new JointManager(_mouseCursor);
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
            MouseExtended.Current.GetState(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mouseCursor.Update(gameTime);
            _jointManager.Update(gameTime);

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

            _mouseCursor.Draw(gameTime);
            _jointManager.Draw(gameTime);

            Current.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
