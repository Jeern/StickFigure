using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Input
{
    public class MultiTouchExtended
    {

        private static MultiTouchExtended _current;

        public static MultiTouchExtended Current
        {
            get { return _current ?? (_current = new MultiTouchExtended()); }
        }

        public TouchCollection CurrentStates { get; set; }

        public TouchCollection GetState(GameTime currentTime)
        {
            TouchCollection touches = TouchPanel.GetState();
            TouchCount = touches.Count;
            if (touches.Count <= 0)
            {
                CurrentStates = new TouchCollection();
            }
            else
            {
                CurrentStates = touches;
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
                    //foreach (TouchLocation touchLocation in CurrentStates)
                    //{
                    //    position += touchLocation.Position;
                    //}
                    //position /= TouchCount;
                }
                return position;
            }
        }
    }
}