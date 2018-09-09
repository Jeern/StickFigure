using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Utils
{
    public class MultiTouchArea
    {
        private FloatRectangle TouchArea { get; set; }

        public MultiTouchArea(FloatRectangle touchArea)
        {
            TouchArea = touchArea;
        }

        public int TouchCount
        {
            get { return MultiTouchExtended.Current.TouchCount;  }
        }

        private bool _isPressed;

        public bool IsPressed
        {
            get
            {
                if (MultiTouchExtended.Current.FirstState == TouchLocationState.Pressed)
                {
                    if (IsWithin)
                    {
                        _isPressed = true;
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsMoving
        {
            get
            {
                if (MultiTouchExtended.Current.FirstState == TouchLocationState.Moved)
                {
                    return IsWithin;
                }
                return false;
            }
        }

        private bool IsWithin
        {
            get
            {
                return TouchPosition.X >= TouchArea.X && TouchPosition.X <= TouchArea.X + TouchArea.Width &&
                       TouchPosition.Y >= TouchArea.Y && TouchPosition.Y <= TouchArea.Y + TouchArea.Height;
            }
        }

        public bool WasTouched
        {
            get
            {
                if (_isPressed && (MultiTouchExtended.Current.FirstState == TouchLocationState.Released ||
                    MultiTouchExtended.Current.FirstState == TouchLocationState.Invalid))
                {
                    _isPressed = false;
                    return true;
                }
                return false;
            }
        }

        public Vector2 TouchPosition
        {
            get { return MultiTouchExtended.Current.TouchPosition; }
        }

        

    }
}
