using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System;

namespace Beebapps.Game.Input
{
    public class SingleTouchExtended //: InputDeviceExtended<TouchLocationState>
    {

        private static SingleTouchExtended _current;
        public static SingleTouchExtended Current
        {
            get { return _current ?? (_current = new SingleTouchExtended()); }
        }

        public TouchLocation CurrentState { get; set; }

        public TouchLocation GetState(GameTime currentTime)
        {
            TouchCollection touches = TouchPanel.GetState();
            TouchCount = touches.Count;
            if (touches.Count <= 0)
            {
                var location = new TouchLocation();
                CurrentState = location;
            }
            else
            {
                CurrentState = touches[0];
            }
            return CurrentState;
        }

        public bool IsScreenTapped
        {
            get
            {
                return (Current.CurrentState.State == TouchLocationState.Pressed);
            }
        }

        public int TouchCount { get; private set; }


    }
}
