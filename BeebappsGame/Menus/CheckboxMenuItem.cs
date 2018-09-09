using Beebapps.Game.GraphicUtils;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Beebapps.Game.Menus
{
    public class CheckBoxMenuItem : TextMenuItem
    {

        public bool Checked { get; set; }
        public GameImage CheckBoxBackground { get; set; }
        public GameImage CheckMark { get; set; }
        private Vector2 _textSize, _textPosition;
        private Rectangle _checkboxRectangle;

        public CheckBoxMenuItem(string name, SpriteFont font, string text) : this(name, font, text, Vector2.Zero, Color.White, Color.Gray, false, true) { }


        public CheckBoxMenuItem(string name, SpriteFont font, string text, Vector2 position, Color activeColor, Color inactiveColor, bool isChecked, bool centered)
            : base(name, font, text, position, activeColor, inactiveColor, centered)
        {
            Checked = isChecked;
            NeedsPositionRecalculation = true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (CheckBoxBackground == null)
            {
                //CheckBoxBackground = BaseTextures.Box_128x128;
            }
            if (CheckMark == null)
            {
                //CheckMark = BaseTextures.CheckMark_128x128;
            }

            BeebappsGame.Current.SpriteBatch.Draw(CheckBoxBackground, _checkboxRectangle, CurrentColor);

            if (Checked)
            {
                BeebappsGame.Current.SpriteBatch.Draw(CheckMark, _checkboxRectangle, CurrentColor);
            }


            BeebappsGame.Current.SpriteBatch.DrawString(Font, Text, _textPosition, CurrentColor);

        }

        protected override void RecalculatePosition()
        {
            const float checkBoxToTextRatio = .6F;
            const float checkboxTopMarginRatio = .2F;
            const int space = 10;
            
            var x = (int)Position.X;
            var y = (int)Position.Y;
            
            _textSize = Font.MeasureString(Text);
            var checkboxSize = (int)(_textSize.Y * checkBoxToTextRatio);

            if (Centered)
            {
                x = (int)(BeebappsGame.Current.GraphicsDevice.Viewport.GetCenter().X - (checkboxSize + space + _textSize.X)/ 2);
            }
            Position = new Vector2(x, y);

            _checkboxRectangle = new Rectangle((int)Position.X, (int)(Position.Y + checkboxTopMarginRatio * _textSize.Y), checkboxSize,checkboxSize);
            
            
            _textPosition = new Vector2(Position.X + checkboxSize + space, Position.Y);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsSelected && IsReadyForKeyboardInteraction)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Enter))
                {
                    Checked = !Checked;
                    if (Checked)
                    {
                        OnActivate();
                    }
                    ResetKeyboardIntervalTimer();
                }
            }
        }

    }
}
