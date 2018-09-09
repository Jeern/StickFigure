namespace StickFigure
{
    public class ConcreteJoint : Joint
    {
        public ConcreteJoint(TemplateJoint template) : base(template.Position, template.Radius, template.Thickness, template.Color, template.Visible)
        {
        }
    }
}
