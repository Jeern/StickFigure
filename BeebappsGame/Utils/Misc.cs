using System;
using Beebapps.Game.GraphicUtils;
using Microsoft.Xna.Framework;
#if IPHONE
using MonoTouch.UIKit;
#endif

namespace Beebapps.Game.Utils
{
    public static class Misc
    {
        public const float Iphone4ScreenWidth = 640;
        public const float Iphone4ScreenHeight = 960;
        public const float Iphone3ScreenWidth = 320;
        public const float Iphone3ScreenHeight = 480;
        public const float Iphone5ScreenWidth = 640;
        public const float Iphone5ScreenHeight = 1136;
        public const float Iphone5NonRetinaScreenHeight = 568;
// ReSharper disable InconsistentNaming
        public const float IPadNonRetinaScreenWidth = 320; //Egentlig 768 men det klarer skalering
        public const float IPadNonRetinaScreenHeight = 480; //Egentlig 1024 men det klarer skalering
// ReSharper restore InconsistentNaming
        
        
        public static readonly Vector2 NonInitialized = new Vector2(float.MinValue, float.MinValue);

        public static bool PositionIsWithinArea(Vector2 position, DoubleRectangle area)
        {
            return
                position.X >= area.X &&
                    position.X <= area.X + area.Width &&
                    position.Y >= area.Y &&
                    position.Y <= area.Y + area.Height;
        }

        public static ITextureLoader Textures
        {
            get; set;
        }

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }
        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        private static bool? _isIPad = null;

        public static bool IsIPad
        {
            get
            {
#if IPHONE
				if(!_isIPad.HasValue) 
				{
				    _isIPad = (UIScreen.MainScreen.Bounds.Height == 1024);
				}
				return _isIPad.Value;						
#else
                return false;
#endif
            }
        }
    }
}
