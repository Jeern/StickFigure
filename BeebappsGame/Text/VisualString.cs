using Beebapps.Game.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Text
{
    public class VisualString : ISpatial
    {
        public static SpriteFont NormalFont { get; set; }
        public static SpriteFont BigFont {get; set; }

        public VisualString(string caption, SpriteFont font)
            : this(caption, font, false)
        {
        }

        public VisualString(string caption, SpriteFont font, bool capitalLetters)
        {
            Caption = capitalLetters ? caption.ToUpper() : caption;
            Font = font;
        }


        public string Caption { get; private set; }
        public SpriteFont Font { get; private set; }
        public Vector2 Position { get; set; }

        public static implicit operator VisualString(string text)
        {
            if (text.StartsWith("\b"))
                return new VisualString(text.Substring(1), BigFont);

            return new VisualString(text, NormalFont);
        }

        public static implicit operator string(VisualString text)
        {
            return text.Caption;
        }

        public float Width
        {
            get { return Font.MeasureString(Caption).X; }
        }

        public float Height
        {
            get { return Font.MeasureString(Caption).Y; }
        }
    }
}
