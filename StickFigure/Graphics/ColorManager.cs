using Microsoft.Xna.Framework;

namespace StickFigure.Graphics
{
    public static class ColorManager
    {
        public static Color GetColorFromHex(string hexColor)
        {
            var color = uint.Parse(hexColor, System.Globalization.NumberStyles.HexNumber);

            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return new Color(r,g,b,a);
        }
    }
}
