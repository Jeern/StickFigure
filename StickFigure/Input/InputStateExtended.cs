using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TS">The State (MouseState, KeyboardState, GamePadState)</typeparam>
    public class InputStateExtended<TS> where TS : struct
    {
        public InputStateExtended(GameTime gameTime, TS state)
        {
            StateTime = gameTime.TotalRealTime();
            State = state;
        }
        public TimeSpan StateTime { get; set; }
        public TS State { get; set; }

    }
}
