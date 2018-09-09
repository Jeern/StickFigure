using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.GraphicUtils
{
    public static class ViewPortExtensions
    {
        public static int GetRight(this Viewport viewPort)
        {
            return viewPort.X + viewPort.Width;
        }

        public static int GetBottom(this Viewport viewPort)
        {
            return viewPort.Y + viewPort.Height;
        }

        public static Vector2 GetCenter(this Viewport viewPort)
        {
            return new Vector2((viewPort.X + viewPort.Width) / 2, (viewPort.Y + viewPort.Height) / 2);
        }

        public static int GetLeftOutside(this Viewport viewport)
        {
            return 0; // -Environment.OutsideViewportSize;
        }

        public static int GetRightOutside(this Viewport viewport)
        {
            return viewport.Width; // +Environment.OutsideViewportSize;
        }

        public static int GetTopOutside(this Viewport viewport)
        {
            return 0; // -Environment.OutsideViewportSize;
        }

        public static int GetBottomOutside(this Viewport viewport)
        {
            return viewport.Height; // +Environment.OutsideViewportSize;
        }

    }
}
