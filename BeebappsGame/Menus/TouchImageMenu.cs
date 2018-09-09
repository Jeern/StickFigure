using System;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Menus
{
    /// <summary>
    /// A static menu with a click event which fires if touched
    /// </summary>
    public class TouchImageMenu : DrawableGameComponent
    {
        private string Tag { get; set; }
        private Texture2D Image { get; set; }
        private Vector2 Position { get; set; }
        private Vector2 ImageScale { get; set; }
        private float Scale { get; set; }

        private event EventHandler<EventArgs<string>> _clicked = delegate { };
        public event EventHandler<EventArgs<string>> Clicked
        {
            add { _clicked += value; }
            remove { _clicked -= value; }
        }

        private TouchableArea TouchableArea { get; set; }

        public TouchImageMenu(Texture2D image, Vector2 position, string tag, int width, int height)
            : base(BeebappsGame.Current)
        {
            Image = image;
            Tag = tag;
            Position = position;
            ImageScale = new Vector2(width / (float)image.Width, height / (float)image.Height);
            Scale = 1f;
            TouchableArea = new TouchableArea(new FloatRectangle(Position.X, Position.Y, width, height));
        }

        public override void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.Draw(Image,
               Position,
               null,
               Color.White,
               0F,
               Vector2.Zero, // Middle / Scale, //Formerly Middle
               ImageScale * Scale,
               SpriteEffects.None,
               1f); //Layer
        }

        public override void Update(GameTime gameTime)
        {
            if (TouchableArea.IsPressed)
            {
                Scale = 1.05f;
            }
            else if (TouchableArea.WasClicked)
            {
                Scale = 1f;
                _clicked(this, new EventArgs<string>(Tag));
            }
            base.Update(gameTime);
        }
    }
}
