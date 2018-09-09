#if WINDOWS
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    public class MouseExtended : InputDeviceExtended<MouseState>
    {
        private static MouseExtended _current;
        public static MouseExtended Current
        {
            get { return _current ?? (_current = new MouseExtended()); }
        }

        public MouseState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            MouseState state = Mouse.GetState();
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(MouseButton checkButton, int requiredCount)
        {
            ButtonState found = ButtonState.Released;
            int count = 0;
            foreach (InputStateExtended<MouseState> stateExt in RecordedStates)
            {
                if (found == ButtonState.Pressed &&
                    ButtonStateToCheck(stateExt.State, checkButton) == ButtonState.Released)
                {
                    count++;
                    if (count >= requiredCount)
                    {
                        Reset();
                        return true;
                    }
                }
                found = ButtonStateToCheck(stateExt.State, checkButton);
            }
            return false;
        }

        private ButtonState ButtonStateToCheck(MouseState state, MouseButton checkButton)
        {
            switch (checkButton)
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

        public bool WasSingleClick(MouseButton checkButton)
        {
            return ClickCount(checkButton, 1);
        }

        public bool WasDoubleClick(MouseButton checkButton)
        {
            return ClickCount(checkButton, 2);
        }

        public override bool NothingPressed
        {
            get
            {
                return CurrentState.LeftButton == ButtonState.Released && CurrentState.MiddleButton == ButtonState.Released
                 && CurrentState.RightButton == ButtonState.Released;
            }
        }


    }
}
#endif