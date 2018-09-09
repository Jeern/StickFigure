using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Utils
{
    public class TouchableAreaUpdateable
    {
        public UpdateableRectangle TouchArea { get; set; }

        public TouchableAreaUpdateable(): this(UpdateableRectangle.Empty)
        {
        }

        public TouchableAreaUpdateable(UpdateableRectangle touchArea)
        {
            TouchArea = touchArea;
        }

        public int TouchCount
        {
            get { return SingleTouchExtended.Current.TouchCount;  }
        }

        private bool _isPressed;

        public bool IsPressed
        {
            get
            {
                if (SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Pressed)
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
                if (SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Moved)
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
                if (_isPressed && (SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Released || 
                    SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Invalid))
                {
                    _isPressed = false;
                    return true;
                }
                return false;
            }
        }

        public Vector2 TouchPosition
        {
            get { return SingleTouchExtended.Current.CurrentState.Position; }
        }

        public void Update()
        {
            TouchArea.Update();           
        }
    }
}
