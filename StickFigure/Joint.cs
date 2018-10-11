using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public abstract class Joint
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Thickness { get; set; }
        public bool Visible { get; set; }

        protected Joint() { }

        protected Joint(Vector2 position, float radius, float thickness, bool visible)
        {
            Position = position;
            Radius = radius;
            Thickness = thickness;
            Visible = visible;
        }

        public void Draw(Vector2 offSet, bool drawFinal, Color color)
        {
            if (!drawFinal || Visible)
            {
                BeebappsGame.Current.SpriteBatch.DrawCircle(Position + offSet, Radius, 100, Visible ? color : Color.Red, Thickness);
            }
        }

        public bool PointIsWithin(Vector2 vector)
        {
            return (Position - vector).LengthSquared() <= Radius * Radius;

        }
    }
}
