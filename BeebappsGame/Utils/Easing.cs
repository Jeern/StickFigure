using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    //Fra http://joshondesign.com/2013/03/01/improvedEasingEquations

    public static class Easing
    {
        public static float OutCubic(float t) 
        {
            return 1 - InCubic(1-t);
        }

        public static float InCubic(float t)
        {
            return (float)Math.Pow(t, 3);
        }


        public static float LinearTimed(double startTime, double endTime, double currentTime, float start, float end)
        {
            if (currentTime >= endTime)
                return end;

            if (currentTime <= startTime)
                return start;

            return (float)((currentTime - startTime)*(end - start)/(endTime - startTime) + start);
        }

        public static Vector2 LinearTimed(double startTime, double endTime, double currentTime, Vector2 start, Vector2 end)
        {
            if (currentTime >= endTime)
                return end;

            if (currentTime <= startTime)
                return start;

            return (((float)(currentTime - startTime)) * (end - start) / (float)(endTime - startTime) + start);
        }

        public static float OutTimedCubic(double startTime, double endTime, double currentTime, float start, float end)
        {
            float linear = LinearTimed(startTime, endTime, currentTime, start, end);
            return OutCubic((linear - start)/(end - start)) + start;
        }

        public static Vector2 OutTimedCubic(double startTime, double endTime, double currentTime, Vector2 start, Vector2 end)
        {
            Vector2 linear = LinearTimed(startTime, endTime, currentTime, start, end);
            return new Vector2(
                OutCubic((linear.X - start.X) / (end.X - start.X)) * (end.X - start.X) + start.X,
                OutCubic((linear.Y - start.Y) / (end.Y - start.Y)) * (end.Y - start.Y) + start.Y);
        }
    }
}
