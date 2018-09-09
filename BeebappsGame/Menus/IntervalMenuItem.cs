using System;
using System.Globalization;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Beebapps.Game.Menus
{
    public class IntervalMenuItem : TextMenuItem
    {
        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }
        public int TickInterval { get; private set; }

        private int _currentValue;
        public int CurrentValue
        {
            get { return _currentValue; }
            set
            {
                if (value >= MinValue && value <= MaxValue)
                {
                    _currentValue = value;
                }
                else
                {
                    throw new Exception("CurrentValue must be between MinValue and MaxValue! Trying to set CurrentValue=" + value + " while MinValue=" + MinValue + " and MaxValue=" + MaxValue);
                }
            }
        }

        public void TickUp()
        {
            if (CurrentValue < MaxValue)
            {
                CurrentValue+= TickInterval;
            }
        }

        public void TickDown()
        {
            if (CurrentValue > MinValue)
            {
                CurrentValue -= TickInterval;
            }
        }


        //private List<Rectangle> _tickMarks = new List<Rectangle>();
        Vector2 _textSize;

        public IntervalMenuItem(string name, SpriteFont font, string text, int maxValue, int minValue, int tickInterval) : this(name, font, text, Vector2.Zero, Color.White, Color.Gray, true, maxValue, minValue, tickInterval)
        {

        }

        
        public IntervalMenuItem(string name, SpriteFont font, string text, Vector2 position, Color activeColor, Color inactiveColor, bool centered, int maxValue, int minValue, int tickInterval)
            : base(name,font, text,position, activeColor, inactiveColor,centered)
        {

            if (minValue >= maxValue)
            {
                throw new ArgumentException("minValue must be smaller than maxValue");
            }
            MaxValue = maxValue;
            MinValue = minValue;
            CurrentValue = minValue;
            TickInterval = tickInterval;
            NeedsPositionRecalculation = true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            BeebappsGame.Current.SpriteBatch.DrawString(Font, Text + " < " + CurrentValue.ToString(CultureInfo.InvariantCulture) + " >", Position, CurrentColor);
            //GameDevGame.Current.SpriteBatch.DrawRectangle(_sliderBar, Color.Gray);
            //foreach (Rectangle  rect in _tickMarks)
            //{
            //    GameDevGame.Current.SpriteBatch.DrawRectangle(rect, Color.Gray);
            //}
            //GameDevGame.Current.SpriteBatch.DrawRectangle(_tickMarks[], Color.Gray);

        }

        protected override void RecalculatePosition()
        {
            if (Centered)
            {
                _textSize = Font.MeasureString(Text);
                var x = (int)(BeebappsGame.Current.GraphicsDevice.Viewport.Width - _textSize.X) / 2;
                var y = (int)Position.Y;
                Position = new Vector2(x, y);
            }

            //int sliderRange = MaxValue - MinValue;
            //int numberOfTicks = sliderRange / TickInterval;
            
            //int pixelsBetweenTicks =(int)( _textSize.X / numberOfTicks);
            //_tickMarks.Clear();
            //for (int i = 0; i <= numberOfTicks; i++)
            //{
            //    _tickMarks.Add(new Rectangle(i * pixelsBetweenTicks + (int)Position.X, (int)(Position.Y + _textSize.Y), 3, 15));
            //}
            //_sliderBar = new Rectangle((int)Position.X, (int)(Position.Y + _textSize.Y) +8,(int) _textSize.X, 3);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsSelected && IsReadyForKeyboardInteraction)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Subtract) || state.IsKeyDown(Keys.OemMinus))
                {
                    TickDown();
                    ResetKeyboardIntervalTimer();
                }
                else
                {
                    if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.Add) || state.IsKeyDown(Keys.OemPlus))
                    {
                        TickUp();
                        ResetKeyboardIntervalTimer();
                    }

                }
            }
        }
 
    }
}
