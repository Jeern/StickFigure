using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickFigure.Drawing;

namespace StickFigure.Graphics
{
    public static class PngCreator
    {
        public static Rectangle GetDimensions(IEnumerable<ConcreteJoint> joints)
        {
            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MaxValue;
            float bottom = float.MinValue;

            foreach (var joint in joints)
            {
                left = Math.Min(joint.Position.X - joint.Radius - joint.Thickness, left);
                right = Math.Max(joint.Position.X + joint.Radius + joint.Thickness, right);
                top = Math.Min(joint.Position.Y - joint.Radius - joint.Thickness, top);
                bottom = Math.Max(joint.Position.Y + joint.Radius + joint.Thickness, bottom);
            }

            return new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
        }

        public static RenderTarget2D GetTexture(Rectangle rectangle, GraphicsDevice device)
        {
            return new RenderTarget2D(
                device,
                rectangle.Width,
                rectangle.Height,
                false,
                device.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
        }

        public static void Save(RenderTarget2D renderTarget, string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                renderTarget.SaveAsPng(fs, renderTarget.Width, renderTarget.Height);
            }
        }
    }
}
