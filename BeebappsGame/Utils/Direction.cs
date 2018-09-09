using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public struct Direction
    {
        public Direction(Vector2 direction, float precision)
            : this(direction.Y < -precision, direction.Y > precision, direction.X < -precision, direction.X > precision)
        {
            
        }

        public Direction(bool up, bool down, bool left, bool right) : this()
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;

            if (Up && Down)
            {
                Up = false;
                Down = false;
            }
            if (Left && Right)
            {
                Left = false;
                Right = false;
            }
        }

        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }
    }
}
