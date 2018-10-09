using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            //Current.ViewPortSize = new Vector2(Consts.ScreenWidth, Consts.ScreenHeight);
            //Current.GraphicsDevice.Viewport = new Viewport(0,0,Consts.ScreenWidth, Consts.ScreenHeight);

            base.Initialize();
        }

        private MouseCursor _mouseCursor;
        private JointManager _jointManager;
        private LineManager _lineManager;

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
            _sfFont = Content.Load<SpriteFont>("SF");
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

            RefreshFiles();

            _mouseCursor.Update(gameTime);
            _lineManager.Update(gameTime);
            _jointManager.Update(gameTime);

            LeftRightArrowKey();
            UpDOwnArrowKey();
            Save();
            MarkAsLast();
            Copy();
            InBetweenGeneration();
            CreatePng();

            base.Update(gameTime);
        }

        private void InBetweenGeneration()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.I))
            {
                _actionHappened = true;
                if (Globals.Files.Count < 2)
                    return;

                var lastFileNumber = 0;
                foreach (var fileNumber in Globals.Files.Keys.OrderBy(k => k))
                {
                    if (lastFileNumber > 0 && lastFileNumber < fileNumber - 1)
                    {
                        InBetweenGeneration(lastFileNumber, fileNumber);
                    }

                    lastFileNumber = fileNumber;
                }
            }
        }

        private void InBetweenGeneration(int from, int to)
        {
            var file1 = Globals.Files[from];
            var file2 = Globals.Files[to];
            if (file2.IsLast)
            {
                file2 = Globals.Files[1];
            }

            for (int fileNumber = from + 1; fileNumber < to; fileNumber++)
            {
                var newJoints = new List<ConcreteJoint>();
                foreach (var concreteJoint1 in file1.ConcreteJoints)
                {
                    var concreteJoint2 = file2.ConcreteJoints.FirstOrDefault(cj => cj.Id == concreteJoint1.Id);
                    if (concreteJoint2 == null)
                    {
                        newJoints.Add(concreteJoint1);
                    }
                    else
                    {
                        newJoints.Add(ConcreteJoint.CreateInBetweenJoint(concreteJoint1, concreteJoint2, fileNumber - from, to - from));
                    }
                }

                var newFile = new JointFile
                {
                    ConcreteJoints = newJoints,
                    Lines = file1.Lines
                };
                FileManager.Save(FileManager.GetFileName(fileNumber, Globals.CurrentFolder), newFile);
            }



        }

        private void LeftRightArrowKey()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Left))
            {
                _actionHappened = true;
                var keys = Globals.Files.Keys.OrderBy(k => k);
                var max = keys.LastOrDefault();
                var min = keys.FirstOrDefault();
                do
                {
                    Globals.CurrentShownNumber--;
                    if (Globals.CurrentShownNumber < min && max < 1)
                    {
                        Globals.CurrentShownNumber = 1;
                    }
                    else if(Globals.CurrentShownNumber < min)
                    {
                        Globals.CurrentShownNumber = max;
                    }
                } while (Globals.CurrentShownNumber != 1 && !Globals.Files.ContainsKey(Globals.CurrentShownNumber));
            }
            else if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Right))
            {
                _actionHappened = true;
                var keys = Globals.Files.Keys.OrderBy(k => k);
                var max = keys.LastOrDefault();
                do
                {
                    Globals.CurrentShownNumber++;
                    if (Globals.CurrentShownNumber > max)
                    {
                        Globals.CurrentShownNumber = 1;
                    }
                } while (Globals.CurrentShownNumber != 1 && !Globals.Files.ContainsKey(Globals.CurrentShownNumber));
            }
        }

        private void UpDOwnArrowKey()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Up))
            {
                _actionHappened = true;
                Globals.CurrentActionNumber++;
            }
            else if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.Down))
            {
                _actionHappened = true;
                Globals.CurrentActionNumber--;
                if (Globals.CurrentActionNumber < 1)
                {
                    Globals.CurrentActionNumber = 1;
                }
            }
        }

        private void CreatePng()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.P))
            {
                _actionHappened = true;
                var dimensions = PngCreator.GetDimensions(Globals.Files.Values.SelectMany(f => f.ConcreteJoints));
                var texture = PngCreator.GetTexture(dimensions, GraphicsDevice);
                DrawToRenderTarget(texture, new Vector2(-dimensions.X, - dimensions.Y));
                PngCreator.Save(texture, FileManager.GetPngFileName(Globals.CurrentShownNumber, Globals.CurrentFolder));

            }
        }

        private void Save()
        {
            if (!_actionHappened  && KeyboardExtended.Current.WasSingleClick(Keys.S))
            {
                _actionHappened = true;
                FileManager.Save(FileManager.GetFileName(Globals.CurrentShownNumber, Globals.CurrentFolder),
                    _jointManager.ToFile());
            }
        }

        private void MarkAsLast()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.L))
            {
                _actionHappened = true;
                FileManager.MarkAsLast(FileManager.GetFileName(Globals.CurrentActionNumber, Globals.CurrentFolder));
            }
        }

        private void Copy()
        {
            if (!_actionHappened && KeyboardExtended.Current.WasSingleClick(Keys.C))
            {
                _actionHappened = true;
                if (Globals.CurrentShownNumber != Globals.CurrentActionNumber)
                {
                    FileManager.Copy(FileManager.GetFileName(Globals.CurrentShownNumber, Globals.CurrentFolder),
                        FileManager.GetFileName(Globals.CurrentActionNumber, Globals.CurrentFolder));
                }
            }
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
            Current.SpriteBatch.DrawString(_sfFont, $"Current #{Globals.CurrentShownNumber}, (S)ave #{Globals.CurrentShownNumber}, (C)opy #{Globals.CurrentShownNumber} to #{Globals.CurrentActionNumber}, Mark #{Globals.CurrentActionNumber} as (L)ast, (I)n-between generation, (P)ng", new Vector2(20, 40), Color.Black);

            _mouseCursor.Draw(gameTime);
            DrawStickFigure(Vector2.Zero, false);

            Current.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawToRenderTarget(RenderTarget2D target, Vector2 offSet)
        {
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = true };

            GraphicsDevice.Clear(Color.White);
            // Draw the scene
            Current.SpriteBatch.Begin();
            DrawStickFigure(offSet, true);
            Current.SpriteBatch.End();

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawStickFigure(Vector2 offSet, bool drawFinal)
        {
            _lineManager.Draw(offSet);
            _jointManager.Draw(offSet, drawFinal);
        }

        private bool _actionHappened = true;
        public void RefreshFiles()
        {
            if (_actionHappened)
            {
                _actionHappened = false;
                Globals.Files = FileManager.LoadAllFiles(Globals.CurrentFolder);
                if (!Globals.Files.ContainsKey(Globals.CurrentShownNumber))
                {
                    Globals.CurrentShownNumber = 1;
                }

                if (Globals.Files.ContainsKey(Globals.CurrentShownNumber))
                {
                    var file = Globals.Files[Globals.CurrentShownNumber];
                    _jointManager.FromFile(file);
                }
            }
        }
    }
}
