using Beebapps.Game.Sprites;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Text
{
    public class SimpleText : DrawableGameComponent, ISpatial
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get 
            {
                if (Scale == 1f)
                {
                    return _position;
                }
                float moveScale = (Scale -1f) / 2f + 1f;
                Vector2 fontVector = Font.MeasureString(Text);
                Vector2 moveVector = fontVector * moveScale - fontVector;
                return _position - moveVector;
            }
            set { _position = value; }
        }

        public Color Color { get; set; }
        public SpriteFont Font { get; private set; }
        public string Text { get; private set; }
        public float Scale { get; set; }

        public Rectangle GetTextAreaWithMargin(float margin)
        {
            return new Rectangle((int)(Position.X - margin * Scale), (int)(Position.Y - margin * Scale),
                        (int)(Font.MeasureString(Text).X * Scale + 2 * margin * Scale), (int)(Font.MeasureString(Text).Y * Scale + 2 * margin * Scale));
        }

        //TouchArea is the same as TextArea but not affected by scale.
        private FloatRectangle _touchArea = FloatRectangle.Empty;
        public FloatRectangle TouchArea
        {
            get
            {
                if (_touchArea == FloatRectangle.Empty)
                {
                    _touchArea = new FloatRectangle(Position.X, Position.Y,
                        Font.MeasureString(Text).X, Font.MeasureString(Text).Y);
                }
                return _touchArea;
            }
        }

        public SimpleText(string text, SpriteFont font,
            Color color, Vector2 position)
            : base(BeebappsGame.Current)
        {
            Initialize(text, font, color, position);
        }

        private void Initialize(string text, SpriteFont font, Color color, Vector2 position)
        {
            Font = font;
            Color = color;
            Position = position;
            Text = text;
            Scale = 1f;
        }

        ///// <summary>
        ///// This version of the constructor potentially centers the Text. If so it ignores the position Vector, and instead centers the text within the rectangle.
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="font"></param>
        ///// <param name="color"></param>
        ///// <param name="position"></param>
        ///// <param name="centerHorizontally"></param>
        ///// <param name="centerVertically"></param>
        ///// <param name="centerArea"></param>
        //public SimpleText(string text, SpriteFont font,
        //    Color color, Vector2 position, bool centerHorizontally, bool centerVertically, Rectangle centerArea)
        //    : base(GameDevGame.Current)
        //{
        //    float x = position.X;
        //    float y = position.Y;
        //    if (centerHorizontally)
        //    {
        //        x = centerArea.Left + ((float)centerArea.Width - font.MeasureString(text).X) / 2;
        //    }
        //    if (centerVertically)
        //    {
        //        y = centerArea.Top + ((float)centerArea.Height - font.MeasureString(text).Y) / 2;
        //    }
        //    Initialize(text, font, color, new Vector2(x, y));
        //}

        public override void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.DrawString(Font, Text, Position, Color, 0F, Vector2.Zero,
                Scale, SpriteEffects.None, 1F);
            //base.Draw(gameTime);
        }


        public float Width
        {
            get { return Font.MeasureString(Text).X; }
        }

        public float Height
        {
            get { return Font.MeasureString(Text).Y; }
        }
    }
}
