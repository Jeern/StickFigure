using System.Collections.Generic;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Sprites
{
    public class SpatialRow : DrawableGameComponent, ISpatial
    {
        public SpatialRow(Vector2 position, float width, float spaceBetween, 
            HorizontalAlignment alignment, params ISpatial[] spatials) : base(BeebappsGame.Current)
        {
            Position = position;
            Width = width;
            HorizontalAlignment = alignment;
            SpaceBetween = spaceBetween;
            Spatials = spatials;
            InitializeSpatials();
            CreateDrawableGameComponents();
        }

        private List<DrawableGameComponent> DrawableGameComponents { get; set; }

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

        public void InitializeSpatials()
        {
            float totalWidth = 0;
            foreach (ISpatial spatial in Spatials)
            {
                totalWidth += spatial.Width;
                totalWidth += SpaceBetween;
            }
            if (totalWidth > 0)
            {
                totalWidth -= SpaceBetween;
            }

            float startPosX;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    startPosX = 0;
                    break;
                case HorizontalAlignment.Center:
                    startPosX = (Width - totalWidth) / 2;
                    break;
                case HorizontalAlignment.Right:
                    startPosX = Width - totalWidth;
                    break;
                default:
                    startPosX = 0;
                    break;
            }
            startPosX += Position.X;

            var position = new Vector2(startPosX, Position.Y);
            foreach (ISpatial spatial in Spatials)
            {
                spatial.Position = position;
                position += new Vector2(spatial.Width + SpaceBetween, 0);
            }
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Width
        {
            get; private set;
        }

        public float Height
        {
            get; private set; 
        }

        private HorizontalAlignment HorizontalAlignment { get; set; }
        /// <summary>
        /// Margins between each ISpatial
        /// </summary>
        private float SpaceBetween { get; set; }
        private ISpatial[] Spatials { get; set; }

        public override void Draw(GameTime gameTime)
        {
            foreach (DrawableGameComponent drawable in DrawableGameComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DrawableGameComponent drawable in DrawableGameComponents)
            {
                if (drawable.Enabled)
                {
                    drawable.Update(gameTime);
                }
            }
        }

    }
}
