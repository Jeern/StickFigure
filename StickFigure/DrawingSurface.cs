using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickFigure.Drawing;
using StickFigure.Graphics;
using StickFigure.Helpers;
using StickFigure.Input;
using MouseCursor = StickFigure.Drawing.MouseCursor;

namespace StickFigure
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DrawingSurface : TheGame
    {
        private SpriteFont _sfFont;

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

            base.Initialize();
        }

        private MouseCursor _mouseCursor;
        private JointManager _jointManager;
        private LineManager _lineManager;
        private MarkingRectangleManager _markingRectangleManager;

        private Color _background;
        private Color _foreground;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _mouseCursor = new MouseCursor();
            _lineManager = new LineManager(_mouseCursor);
            _jointManager = new JointManager(_mouseCursor, _lineManager);
            _markingRectangleManager = new MarkingRectangleManager(_mouseCursor, _jointManager);

            _sfFont = Content.Load<SpriteFont>("SF");

            _background = ColorManager.GetColorFromHex(ConfigurationManager.AppSettings["BackgroundColor"]);
            _foreground = ColorManager.GetColorFromHex(ConfigurationManager.AppSettings["ForegroundColor"]);
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
            KeyboardExtended.Current.GetState(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.RefreshFiles(_jointManager);

            _mouseCursor.Update(gameTime);

            _markingRectangleManager.Update(gameTime);

            _lineManager.Update(gameTime);

            if (!_markingRectangleManager.IsDragging && !_markingRectangleManager.IsMarking)
            {
                _jointManager.Update(gameTime);
            }

            InputManager.LeftRightArrowKey();
            InputManager.UpDOwnArrowKey();
            InputManager.Save(_jointManager);
            InputManager.MarkAsLast();
            InputManager.Copy();
            InputManager.InBetweenGeneration();
            InputManager.CreateGraphic(GraphicsDevice, DrawToRenderTarget);
            InputManager.CreateGif();

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

            Current.SpriteBatch.DrawString(_sfFont, "Use Arrow keys - Left/Right to change shown file, and Up/Down to change the one that is Copied to or marked as Last.", new Vector2(20), Color.Black);
            Current.SpriteBatch.DrawString(_sfFont, $"Current #{Globals.CurrentShownNumber}, (S)ave #{Globals.CurrentShownNumber}, (C)opy #{Globals.CurrentShownNumber} to #{Globals.CurrentActionNumber}, Mark #{Globals.CurrentActionNumber} as (L)ast, (I)n-between generation, (P)ng and Jpg, (G)if", new Vector2(20, 40), Color.Black);

            _mouseCursor.Draw(gameTime);
            _markingRectangleManager.Draw(gameTime);

            DrawStickFigure(Vector2.Zero, false, Color.Black);

            Current.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawToRenderTarget(RenderTarget2D target, Vector2 offSet)
        {
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = true };

//#90a959
            GraphicsDevice.Clear(_background);
            // Draw the scene
            Current.SpriteBatch.Begin();
            DrawStickFigure(offSet, true, _foreground);
            Current.SpriteBatch.End();

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawStickFigure(Vector2 offSet, bool drawFinal, Color color)
        {
            _lineManager.Draw(offSet, color);
            _jointManager.Draw(offSet, drawFinal, color);
        }
    }
}
