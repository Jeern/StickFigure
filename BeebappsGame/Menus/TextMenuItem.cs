using Beebapps.Game.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Utils
{
    public class TextMenuItem : MenuItem
    {
        private SpriteFont _font;
        public SpriteFont Font {
            get { return _font; }
            set
            {
                _font = value;
                NeedsPositionRecalculation = true;
            }
        }
        private string _text;
        public string Text {

            get { return _text; }

            set {
                _text = value;
                NeedsPositionRecalculation = true;
            }
        }
        public Color ActiveColor { get; set; }
        public Color InactiveColor { get; set; }
        public Color CurrentColor { get {
            if (IsSelected)
            {
                return ActiveColor;
            }
            else
            { return InactiveColor; }

        } }

        public TextMenuItem(string name, SpriteFont font)
            : this(name, font, name, true)
        { }


        public TextMenuItem(string name, SpriteFont font, string text)
            : this(name, font, text, true)
        { }


        public TextMenuItem(string name, SpriteFont font, string text, bool centered) : this(name, font, text, new Vector2(), Color.White, Color.Gray, centered)
        {}

        public TextMenuItem(string name, SpriteFont font, string text, Vector2 position, Color activeColor, Color inactiveColor, bool centered)
            : base(name, position, centered)
        {
            Font = font;
            Text = text;
            ActiveColor = activeColor;
            InactiveColor = inactiveColor;
            NeedsPositionRecalculation = true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            BeebappsGame.Current.SpriteBatch.DrawString(Font, Text, Position, CurrentColor);
        }

        protected override void RecalculatePosition()
        {
            if (Centered)
            {
                Vector2 fontSize = Font.MeasureString(Text);
                int x = (int)(BeebappsGame.Current.GraphicsDevice.Viewport.Width - fontSize.X) / 2;
                int y = (int)Position.Y;
                Position = new Vector2(x, y);
            }
            NeedsPositionRecalculation = false;

        }

 
    }
}
