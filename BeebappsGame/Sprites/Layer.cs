using System;
using System.Diagnostics;
using Beebapps.Game.GraphicUtils;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Sprites
{
    public class Layer
    {
        public ImageState Image { get; private set; }
        public Color Color { get; set; }
        public Vector2 Middle { get; set; }

        public Layer(ImageState image)
        {
            Image = image;
            Color = Color.White;
            Middle = new Vector2(image.Current.Texture.Width / 2f, image.Current.Texture.Height / 2f);
        }

        public void Draw(Camera camera, Vector2 position, float rotation, Vector2 scale)
        {
             BeebappsGame.Current.SpriteBatch.Draw(Image,
                                                      Middle * scale  + position - camera.Position,
                                                      null,
                                                      Color,
                                                      rotation,
                                                      Middle , 
                                                      scale,
                                                      SpriteEffects.None,
                                                      1f); //Layer
        }

        public void Draw(Camera camera, Rectangle source, Vector2 position, float rotation, Vector2 scale)
        {
            BeebappsGame.Current.SpriteBatch.Draw(Image,
               position - camera.Position,
               source,
               Color,
               rotation,
               Vector2.Zero, // Middle / Scale, //Formerly Middle
               scale,
               SpriteEffects.None,
               1f); //Layer
        }

        public void Draw(Rectangle dest, Rectangle source, float rotation)
        {
            BeebappsGame.Current.SpriteBatch.Draw(Image,
               dest,
               source,
               Color,
               rotation,
               Vector2.Zero, // Middle / Scale, //Formerly Middle
               SpriteEffects.None,
               1f); //Layer
        }

        public void Draw(Camera camera, FloatRectangle dest, FloatRectangle source, float rotation, Vector2 scale)
        {
            var position = new Vector2(dest.X, dest.Y);
            float sourceWidth = Math.Min(source.Width, Image.CurrentTexture.Width);
            float sourceHeight = Math.Min(source.Height, Image.CurrentTexture.Height);
            Rectangle? calculatedSourceRectangle = null;
            if(sourceWidth < Image.CurrentTexture.Width || sourceHeight < Image.CurrentTexture.Height)
            {
                calculatedSourceRectangle =
                    new Rectangle(Convert.ToInt32(source.X), Convert.ToInt32(source.Y),
                                  Convert.ToInt32(sourceWidth), Convert.ToInt32(sourceHeight));
            }
            //var scale = new Vector2(dest.Width / sourceWidth, dest.Height / sourceHeight);
            BeebappsGame.Current.SpriteBatch.Draw(Image,
               position - camera.Position,
               calculatedSourceRectangle,
               Color,
               rotation,
               Vector2.Zero, // Middle / Scale, //Formerly Middle
               scale,
               SpriteEffects.None,
               1f); //Layer
        }

        public void Draw(Camera camera, DoubleRectangle dest, DoubleRectangle source, float rotation, Vector2 scale)
        {
            var position = new Vector2((float)dest.X, (float)dest.Y);
            double sourceWidth = Math.Min(source.Width, Image.CurrentTexture.Width);
            double sourceHeight = Math.Min(source.Height, Image.CurrentTexture.Height);
            Rectangle? calculatedSourceRectangle = null;
            if (sourceWidth < Image.CurrentTexture.Width || sourceHeight < Image.CurrentTexture.Height)
            {
                calculatedSourceRectangle =
                    new Rectangle(Convert.ToInt32(source.X), Convert.ToInt32(source.Y),
                                  Convert.ToInt32(sourceWidth), Convert.ToInt32(sourceHeight));
            }
            //var scale = new Vector2(dest.Width / sourceWidth, dest.Height / sourceHeight);
            BeebappsGame.Current.SpriteBatch.Draw(Image,
               position - camera.Position,
               calculatedSourceRectangle,
               Color,
               rotation,
               Vector2.Zero, // Middle / Scale, //Formerly Middle
               scale,
               SpriteEffects.None,
               1f); //Layer
        }

    }
}
