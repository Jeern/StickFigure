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

        public static ConcreteJoint CreateInBetweenJoint(ConcreteJoint joint1, ConcreteJoint joint2, int number, int distance)
        {
            if(joint1.Id != joint2.Id)
                throw new ArgumentException("Only the same joint can be in-betweened");

            var newJoint = new ConcreteJoint
            {
                Id = joint1.Id,
                Color = joint1.Color,
                Radius = joint1.Radius,
                Thickness = joint1.Thickness,
                Visible = joint1.Visible,
                Position = joint1.Position + (joint2.Position - joint1.Position) * number / distance
            };
            return newJoint;
        }
    }
}