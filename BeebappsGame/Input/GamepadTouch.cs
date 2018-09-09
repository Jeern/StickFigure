using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    public class GamepadTouch
    {
        private static GamepadTouch _Current;
        public static GamepadTouch Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new GamepadTouch();
                }
                return _Current;
            }
        }

        public GamePadState GetState()
        {
            CurrentState = GamePad.GetState(PlayerIndex.One);
            return CurrentState;
        }

        public GamePadState CurrentState
        {
            get;
            private set;
        }

        private bool _WasBackbuttonPressed = false;

        public bool IsBackButtonClicked
        {
            get
            {
                bool wasBackButtonPressed = WasBackButtonPressed;
                if (CurrentState.Buttons.Back == ButtonState.Released && wasBackButtonPressed)
                {
                    _WasBackbuttonPressed = false;
                    return true;
                }
                return false;
            }
        }


        public bool WasBackButtonPressed
        {
            get
            {
                bool wasBackButtonPressed = _WasBackbuttonPressed;
                _WasBackbuttonPressed = (CurrentState.Buttons.Back == ButtonState.Pressed);
                return wasBackButtonPressed;
            }
        }




    }
}
