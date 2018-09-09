using System;
using Microsoft.Xna.Framework;
using Beebapps.Game.Utils;
using Beebapps.Game.GraphicUtils;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Sprites
{
    public class StaticBackground : StaticObjects
    {
        private readonly string _textureFile;
        private readonly Vector2 _offset;
        private readonly Vector2 _stretchScale;

        public StaticBackground(string textureFile, Vector2 offset, Vector2 position, float screenHeight)
            : this(textureFile, offset, position, screenHeight, () => new Camera(null, Vector2.Zero))
        {
        }

        public StaticBackground(string textureFile, Vector2 offset, Vector2 position, float screenHeight, Func<Camera> camera)
            : base(position, camera)
        {
            _textureFile = textureFile;
            _offset = offset;
            ImageState = new ImageState(new GameImage(GetTexture(_textureFile)));
            _stretchScale = new Vector2(Misc.Iphone3ScreenWidth / (Width * Misc.Textures.RetinaFactor), screenHeight / (Height * Misc.Textures.RetinaFactor));
        }

        public StaticBackground(Texture2D texture, Vector2 offset, Vector2 position, float screenHeight)
            : this(texture, offset, position, screenHeight, () => new Camera(null, Vector2.Zero))
        {
        }

        public StaticBackground(Texture2D texture, Vector2 offset, Vector2 position, float screenHeight, Func<Camera> camera)
            : base(position, camera)
        {
            _offset = offset;
            ImageState = new ImageState(new GameImage(texture));
            _stretchScale = new Vector2(Misc.Iphone3ScreenWidth / (Width * Misc.Textures.RetinaFactor), screenHeight / (Height * Misc.Textures.RetinaFactor));
        }

        private Texture2D GetTexture(string textureFile)
        {
            return Game.Content.Load<Texture2D>(textureFile);
        }

        protected override ImageState ResetImageState()
        {
            return null;
        }

        public override void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.Draw(ImageState,
               _offset + (Position - Camera().Position) * Misc.Textures.GlobalTextureScale, // + Middle,
               null,
               TheColor,
               0F,
               Vector2.Zero, //Formerly Middle
               _stretchScale * Misc.Textures.GlobalTextureScale * Misc.Textures.RetinaFactor,
               SpriteEffects.None,
               1f); //Layer
        }

    }
}

