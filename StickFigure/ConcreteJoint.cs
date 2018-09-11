using System;

namespace StickFigure
{
    public class ConcreteJoint : Joint
    {
        public Guid Id { get; set; }

        public ConcreteJoint()
        {
        }

        public ConcreteJoint(TemplateJoint template) : base(template.Position, template.Radius, template.Thickness,
            template.Color, template.Visible)
        {
            Id = Guid.NewGuid();
        }
    }
}