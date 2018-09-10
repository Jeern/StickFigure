using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class LineManager
    {
        private readonly MouseCursor _mouseCursor;
        private readonly List<Line> _lines = new List<Line>();

        public LineManager(MouseCursor mouseCursor)
        {
            _mouseCursor = mouseCursor;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var line in _lines)
            {
                line.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var line in _lines)
            {
                line.Draw(gameTime);
            }
        }

        public void AddConnection(Joint start, Joint finish)
        {
            _lines.Add(new Line(start, finish));
        }

        public void DeleteJoint(Joint joint)
        {
            for(int idx=_lines.Count-1; idx >= 0; idx--)
            {
                var line = _lines[idx];
                if (line.Start == joint || line.Finish == joint)
                {
                    _lines.RemoveAt(idx);
                }
            }
        }
    }
}