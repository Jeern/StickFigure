using Microsoft.Xna.Framework;

namespace StickFigure.Drawing
{
    public class TemplateJoint : Joint
    {
        public Vector2 OriginalPosition { get; set; }

        public TemplateJoint(Vector2 position, float radius, float thickness, bool visible) : base(position, radius, thickness, visible)
        {
            OriginalPosition = position;
        }
    }
}
