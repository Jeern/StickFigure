using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D _simpleTexture;

        public static void DrawSegment(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            if (_simpleTexture == null)
            {
				//Erstat med noget andet i Monotouch
//                _simpleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 2, false, SurfaceFormat.Color);
                Color[] pixel = { Color.White, Color.White}; // White. 0xFF is Red, 0xFF0000 is Blue
//                _simpleTexture.SetData<Color>(pixel, 0, _simpleTexture.Width * _simpleTexture.Height);
            }

            // Paint a 100x1 line starting at 20, 50
            // calculate the distance between the two vectors                
            float distance = Vector2.Distance(start, end);

            // calculate the angle between the two vectors                
            var angle = (float)Math.Atan2(end.Y - start.Y,
                end.X - start.X);

            // stretch the pixel between the two vectors                
            spriteBatch.Draw
                (_simpleTexture,
                start,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(distance, 1),
                SpriteEffects.None,
                1f);
        }
    }
}
