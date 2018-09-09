using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
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
    }
}
