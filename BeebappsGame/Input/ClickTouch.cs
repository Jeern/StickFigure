using System;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    public class ClickTouch
    {
        public Vector2 ClickPosition { get; private set; }
        public bool IsClick { get; private set; }
        public bool IsDoubleClick { get; private set; }

        private const double DoubleClickTimeMS = 300;
        private double _firstClickAt;
        private double _secondClickAt;
        private bool _firstClickFinished;

        private static ClickTouch _current;
        public static ClickTouch Current
        {
            get { return _current ?? (_current = new ClickTouch()); }
        }

        public void GetState(GameTime currentTime)
        {
            TouchCollection touches = TouchPanel.GetState();
            IsDoubleClick = false;
            IsClick = false;

            if (touches.Count >= 1 && (touches[0].State == TouchLocationState.Pressed || touches[0].State == TouchLocationState.Moved))
            {
                //Der er mindst en finger på skærmen
                ClickPosition = touches[0].Position;
                if (_firstClickFinished)
                {
                    _secondClickAt = currentTime.TotalGameTime.TotalMilliseconds;
                }
                else
                {
                    _firstClickAt = currentTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else
            {
                double timeBetweenClicks = _secondClickAt - _firstClickAt;
                if (0 < timeBetweenClicks && timeBetweenClicks < DoubleClickTimeMS)
                {
                    IsDoubleClick = true;
                    _firstClickAt = _secondClickAt;
                }
                else if (_firstClickAt + DoubleClickTimeMS > currentTime.TotalGameTime.TotalMilliseconds)
                {
                    _firstClickFinished = true;
                    IsClick = true;
                }
                else
                {
                    _firstClickFinished = false;
                }
            }
        }
    }
}
