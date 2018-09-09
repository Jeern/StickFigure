using System;
using System.Collections.Generic;
using System.Linq;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Utils
{
    public class TouchableArea
    {
        private readonly Func<Camera> _camera;
        private readonly Vector2 _cameraScale;
        public FloatRectangle TouchArea { get; set; }

        public TouchableArea(): this(FloatRectangle.Empty)
        {
        }

        public TouchableArea(FloatRectangle touchArea) : this(() => Camera.Empty, touchArea, Vector2.One)
        {
        }

        public TouchableArea(Func<Camera> camera, FloatRectangle touchArea, Vector2 cameraScale)
        {
            _camera = camera;
            _cameraScale = cameraScale;
            TouchArea = touchArea;
        }

        public int TouchCount
        {
            get { return MultitouchMouseWrap.Current.TouchCount;  }
        }

        private bool _isPressed;

        public bool IsPressed
        {
            get
            {
                if (MultitouchMouseWrap.Current.CurrentStates.Count > 0 && 
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Pressed)
                {
                    if (IsWithin)
                    {
                        _pressPosition = TouchPosition;
                        _isPressed = true;
                        return true;
                    }
                }
                return false;
            }
        }

        private Vector2 _pressPosition;
        private Vector2 _releasePosition;

        private bool PressVsReleaseProximityOK
        {
            get
            {
#if IPHONE
				float proximity = Util.IsIphone5 ? 35f * 1136f / 960f : 35f;
#else
                const float proximity = 35f;
#endif
				 
				return Math.Abs(_pressPosition.Y - _releasePosition.Y) < proximity;
            }
        }

        public bool IsMoving
        {
            get
            {
                if (MultitouchMouseWrap.Current.CurrentStates.Count > 0 &&
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Moved)
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
                return TouchPosition.X >= TouchArea.X - _camera().Position.X * _cameraScale.X && TouchPosition.X <= TouchArea.X - _camera().Position.X * _cameraScale.X + TouchArea.Width &&
                       TouchPosition.Y >= TouchArea.Y - _camera().Position.Y * _cameraScale.Y && TouchPosition.Y <= TouchArea.Y - _camera().Position.Y * _cameraScale.Y + TouchArea.Height;
            }
        }

        public bool IsReleased
        {
            get
            {
                if (MultitouchMouseWrap.Current.CurrentStates.Count == 0 ||
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Released ||
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Invalid)
                {
					_releasePosition = TouchPosition;
                    return true;
                }
                return false;
            }
        }

        public bool WasClicked
        {
            get
            {
                if (_isPressed && (MultitouchMouseWrap.Current.CurrentStates.Count == 0 ||
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Released ||
                    MultitouchMouseWrap.Current.CurrentStates[0].State == TouchLocationState.Invalid) && PressVsReleaseProximityOK)
                {
                    _isPressed = false;
                    return true;
                }
                return false;
            }
        }

        public Vector2 TouchPosition
        {
            get { return MultitouchMouseWrap.Current.CurrentStates[0].Position; }
        }

        public Vector2 RelativeTouchPosition
        {
            get
            {
                return TouchPosition -
                       new Vector2(TouchArea.X - _camera().Position.X*_cameraScale.X,
                                   TouchArea.Y - _camera().Position.Y*_cameraScale.Y);
            }
        }

        public IEnumerable<Vector2> TouchPositions
        {
            get 
            {
                return MultitouchMouseWrap.Current.CurrentStates.Select(state => state.Position);
            }
        }
    }
}
