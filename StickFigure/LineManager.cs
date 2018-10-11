using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class LineManager
    {
        private readonly MouseCursor _mouseCursor;
        public List<Line> Lines { get; set; }

        public LineManager(MouseCursor mouseCursor)
        {
            _mouseCursor = mouseCursor;
            Lines = new List<Line>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var line in Lines)
            {
                line.Update(gameTime);
            }
        }

        public void Draw(Vector2 offSet, Color color)
        {
            foreach (var line in Lines)
            {
                line.Draw(offSet, color);
            }
        }

        public void AddConnection(ConcreteJoint start, ConcreteJoint finish)
        {
            Lines.Add(new Line(start, finish));
        }

        public void DeleteJoint(Joint joint)
        {
            for(int idx= Lines.Count-1; idx >= 0; idx--)
            {
                var line = Lines[idx];
                if (line.Start == joint || line.Finish == joint)
                {
                    Lines.RemoveAt(idx);
                }
            }
        }
    }
}