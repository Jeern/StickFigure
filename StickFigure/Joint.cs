using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public abstract class Joint
    {
        public Vector2 Position { get; set; }
        public float Radius { get; }
        public float Thickness { get; }
        public Color Color { get; }
        public bool Visible { get; }

        protected Joint(Vector2 position, float radius, float thickness, Color color, bool visible)
        {
            Position = position;
            Radius = radius;
            Thickness = thickness;
            Color = color;
            Visible = visible;
        }

        public void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.DrawCircle(Position, Radius, 100, Color, Thickness);
        }

        public bool PointIsWithin(Vector2 vector)
        {
            return (Position - vector).LengthSquared() <= Radius * Radius;

        }
    }
}
