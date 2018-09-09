using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class TemplateJoint : Joint
    {
        public Vector2 OriginalPosition { get; set; }

        public TemplateJoint(Vector2 position, float radius, float thickness, Color color, bool visible) : base(position, radius, thickness, color, visible)
        {
            OriginalPosition = position;
        }
    }
}
