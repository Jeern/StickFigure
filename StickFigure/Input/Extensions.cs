using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StickFigure.Input
{
    public static class Extensions
    {
        private static DateTime _then = DateTime.MinValue;

        public static TimeSpan TotalRealTime(this GameTime time)
        {
            if (_then == DateTime.MinValue)
            {
                _then = DateTime.Now;
                return new TimeSpan(0, 0, 0, 0, 0);
            }

            return DateTime.Now.Subtract(_then);
        }

        private static Dictionary<MouseButton, bool> _wasPressed = new Dictionary<MouseButton, bool>();

        public static ButtonStateExtended ButtonStateExtended(this MouseState mouseState, MouseButton button)
        {
            var buttonState = ButtonStateOfButton(mouseState, button);
            bool wasPressed = WasPressed(button, buttonState);
            bool isPressed = IsPressed(buttonState);

            return (isPressed ? Input.ButtonStateExtended.IsPressed : Input.ButtonStateExtended.IsReleased) |
                   (wasPressed ? Input.ButtonStateExtended.WasPressed : Input.ButtonStateExtended.WasReleased);
        }

        public static bool CheckState(this ButtonStateExtended stateToCheck, ButtonStateExtended stateToCheckAgainst)
        {
            return (stateToCheck & stateToCheckAgainst) == stateToCheckAgainst;
        }

        private static ButtonState ButtonStateOfButton(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
                case MouseButton.Right:
                    return state.RightButton;
                case MouseButton.XButton1:
                    return state.XButton1;
                case MouseButton.XButton2:
                    return state.XButton2;
                default:
                    return state.LeftButton;
            }
        }


        private static bool WasPressed(MouseButton button, ButtonState newState)
        {
            bool wasPressed = false;
            if (_wasPressed.ContainsKey(button))
            {
                wasPressed = _wasPressed[button];
            }
            else
            {
                _wasPressed.Add(button, false);
            }
            _wasPressed[button] = IsPressed(newState);
            return wasPressed;
        }

        private static bool IsPressed(ButtonState state)
        {
            return state == ButtonState.Pressed;
        }
    }
}
