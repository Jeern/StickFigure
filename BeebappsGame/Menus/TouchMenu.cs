using System;
using Beebapps.Game.Text;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Menus
{
    /// <summary>
    /// A static menu with a click event which fires if touched
    /// </summary>
    public class TouchMenu : DrawableGameComponent
    {
        private SimpleText MenuText { get; set; }
        private string Tag { get; set; }
        private event EventHandler<EventArgs<string>> _clicked = delegate { };
        public event EventHandler<EventArgs<string>> Clicked
        {
            add { _clicked += value; }
            remove { _clicked -= value; }
        }

        private TouchableArea TouchableArea { get; set; }



        public TouchMenu(SpriteFont font, Vector2 position, string text, string tag, Color color)
            : base(BeebappsGame.Current)
        {
            MenuText = new SimpleText(text, font, color, position);
            Tag = tag;
            TouchableArea = new TouchableArea(MenuText.TouchArea);
        }

        public Color Color 
        {
            get { return MenuText.Color; }
            set { MenuText.Color = value; } 
        }


        public override void Draw(GameTime gameTime)
        {
            DrawRectangle(MenuText.GetTextAreaWithMargin(15));
            MenuText.Draw(gameTime);
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            BeebappsGame.Current.SpriteBatch.
                DrawSegment(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), Color.White);
            BeebappsGame.Current.SpriteBatch.
                DrawSegment(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), Color.White);
            BeebappsGame.Current.SpriteBatch.
                DrawSegment(new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), Color.White);
            BeebappsGame.Current.SpriteBatch.
                DrawSegment(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X, rectangle.Y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (TouchableArea.IsPressed)
            {
                MenuText.Scale = 1.05f;
            }
            else if (TouchableArea.WasClicked)
            {
                MenuText.Scale = 1f;
                _clicked(this, new EventArgs<string>(Tag));
            }
            base.Update(gameTime);
        }
    }
}
