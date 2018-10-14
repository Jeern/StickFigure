using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StickFigure.Helpers
{
    public class TheGame : Microsoft.Xna.Framework.Game
    {
        private static readonly Random _random = new Random();
        public static Random Random
        {
            get { return _random; }
        }

        private static readonly object Locker = new object();
        private static TheGame _game;
        public static TheGame
            Current { get { return _game; }
                private set 
                {
                    lock (Locker)
                    {
                        if (_game == null)
                        {
                            _game = value;
                        }
                    }
                }
            }

        public bool DebugMode { get; set; }

        public SpriteBatch SpriteBatch { get; set; }
        public SpriteBatch ParticleSpriteBatch { get; set; }
        public float GameSpeed { get; set; }

        public Vector2 ViewPortSize { get {
            return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); 
        } }
        public Vector2 ViewPortCenter { get { return ViewPortSize / 2; } }
        
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        //public SpriteFont DebugFont { get; set; }

        public TheGame()
        {
            Current = this;
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), GraphicsDeviceManager);
            GameSpeed = 1.0F;
			//Placeret i info.plist
//			UIApplication.SharedApplication.StatusBarHidden = false; //Normal Exen er true
        }

        protected override void Initialize()
        {
            base.Initialize();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
//            ParticleSpriteBatch = new SpriteBatch(GraphicsDevice);

            //DebugFont = Content.Load<SpriteFont>("DebugFont");
        }


        // From the Particles Sample on the XNA/MSDN site
        //  a handy little function that gives a _random float between two
        // values. This will be used in several places in the sample, in particilar in
        // ParticleSystem.InitializeParticle.
        public static float RandomBetween(float min, float max)
        {
            return min + (float)_random.NextDouble() * (max - min);
        }

        public static int RandomBetween(int min, int max)
        {
            return _random.Next(min, max+1);
        }

    }
}
