 using Beebapps.Game.GraphicUtils;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Beebapps.Game.Sprites
{
    public abstract class StaticObjects : DrawableGameComponent, ISpatial
    {
        protected Func<Camera> Camera { get; set; }

        protected StaticObjects(Vector2 position)
            : this(position, () => new Camera(null, Vector2.Zero))
        {
        }

        protected StaticObjects(Vector2 position, Func<Camera> camera)
            : base(BeebappsGame.Current)
        {
            Reset();
            Position = position;
            Camera = camera;
            Layer = 1f;
        }
        public virtual Vector2 Position
        {
            get;
            set;
        }

        public virtual void Reset()
        {
            Scale = 1F;
            if (ImageState == null)
            {
                ImageState = ResetImageState();
            }
            TheColor = Color.White;
        }

        public ImageState ImageState
        {
            get;
            set;
        }

        protected abstract ImageState ResetImageState();

        public Color TheColor
        { get; set; }

        public override void Update(GameTime gameTime)
        {
            ImageState.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.Draw(ImageState,
               Position - Camera().Position, // + Middle,
               null,
               TheColor,
               0F,
               Vector2.Zero, //Formerly Middle
               Scale,
               SpriteEffects.None,
               1f); //Layer
        }

        public void BaseDraw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public float TouchDistance
        {
            get
            {
                return (new Vector2(Width / 3, Height / 3).Length()) * Scale;
            }
        }

        public float Scale { get; set; }

        public Vector2 Middle
        {
            get { return new Vector2(Width / 2F, Height / 2F) * Scale; }
        }

        public float Layer
        {
            get;
            set;

        }

        public float Height
        {
            get { return ImageState.CurrentTexture.Height * Scale; }
        }

        public float Width
        {
            get { return ImageState.CurrentTexture.Width * Scale; }
        }
    }
}
