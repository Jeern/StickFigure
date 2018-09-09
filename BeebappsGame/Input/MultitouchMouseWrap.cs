using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Input
{
    public class MultitouchMouseWrap
    {
        private static MultitouchMouseWrap _current;

        public static MultitouchMouseWrap Current
        {
            get { return _current ?? (_current = new MultitouchMouseWrap()); }
        }

        public TouchCollection CurrentStates { get; set; }

        public TouchCollection GetState(GameTime currentTime)
        {
            MouseExtended.Current.GetState(currentTime);
            int x = MouseExtended.Current.CurrentState.X;
            int y = MouseExtended.Current.CurrentState.Y;
            TouchLocation location;
            if(MouseExtended.Current.CurrentState.LeftButton == ButtonState.Pressed)
            {
                location = new TouchLocation(1, TouchLocationState.Pressed, new Vector2(x, y));
                TouchCount = 1;
                CurrentStates = new TouchCollection(new [] { location });
            }
            else if (MouseExtended.Current.CurrentState.LeftButton == ButtonState.Released)
            {
                location = new TouchLocation(1, TouchLocationState.Released, new Vector2(x, y));
                TouchCount = 0;
                CurrentStates = new TouchCollection(new[] { location });
            }
            else
            {
                CurrentStates = new TouchCollection();
                TouchCount = 0;
            }
            return CurrentStates;
        }


        public TouchLocationState FirstState
        {
            get
            {
                if (TouchCount == 0)
                    return TouchLocationState.Invalid;

                return CurrentStates[0].State;
            }
        }

        public int TouchCount { get; private set; }

        public Vector2 TouchPosition
        {
            get
            {
                Vector2 position = Vector2.Zero;
                if (TouchCount > 0)
                {
                    position = CurrentStates[0].Position;
                }
                return position;
            }
        }
    }
}