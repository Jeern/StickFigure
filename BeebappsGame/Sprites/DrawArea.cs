using System.Collections.Generic;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Sprites
{
    /// <summary>
    /// The draw area can draw other components but only in the area defined outside the area it draws nothing.
    /// </summary>
    public class DrawArea : DrawableGameComponent
    {
        private Rectangle ViewBox {get; set; }
        private List<DrawableGameComponent> DrawableGameComponents { get; set; }
        ISpatial[] Spatials { get; set; }
        private Vector2 Offset { get; set; }

        public DrawArea(Rectangle viewBox, params ISpatial[] spatials) : base(BeebappsGame.Current)
        {
            ViewBox = viewBox;
            Offset = new Vector2(viewBox.X, viewBox.Y);
            Spatials = spatials;
            CreateDrawableGameComponents();
        }

        private void CreateDrawableGameComponents()
        {
            DrawableGameComponents = new List<DrawableGameComponent>();
            foreach (ISpatial spatial in Spatials)
            {
                var drgc = spatial as DrawableGameComponent;
                if (drgc != null)
                {
                    DrawableGameComponents.Add(drgc);
                }
            }
        }



        public override void Update(GameTime gameTime)
        {
            foreach (DrawableGameComponent component in DrawableGameComponents)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var savedViewport = new Viewport();
            if (ViewBox != Rectangle.Empty)
            {
                BeebappsGame.Current.SpriteBatch.End();
                BeebappsGame.Current.SpriteBatch.Begin();
                savedViewport = Game.GraphicsDevice.Viewport;
                Viewport currentViewPort = Game.GraphicsDevice.Viewport;
                currentViewPort.Width = ViewBox.Width;
                currentViewPort.Height = ViewBox.Height;
                currentViewPort.X = ViewBox.X;
                currentViewPort.Y = ViewBox.Y;
                Game.GraphicsDevice.Viewport = currentViewPort;
            }

            foreach (ISpatial spatial in Spatials)
            {
                spatial.Position -= Offset;
            }

            foreach (DrawableGameComponent component in DrawableGameComponents)
            {
                if (component.Visible)
                {
                    component.Draw(gameTime);
                }
            }

            foreach (ISpatial spatial in Spatials)
            {
                spatial.Position += Offset;
            }

            if (ViewBox != Rectangle.Empty)
            {
                BeebappsGame.Current.SpriteBatch.End();
                BeebappsGame.Current.SpriteBatch.Begin();
                Game.GraphicsDevice.Viewport = savedViewport;
            }

        }

    }
}
