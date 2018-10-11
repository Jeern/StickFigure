using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class Line
    {
        public ConcreteJoint Start { get; set; }
        public ConcreteJoint Finish { get; set; }

        private Vector2 _start;
        private Vector2 _finish;

        public Line(ConcreteJoint start, ConcreteJoint finish)
        {
            Start = start;
            Finish = finish;
        }

        public void Update(GameTime gameTime)
        {
            var line = Finish.Position - Start.Position;
            _start = Start.Visible ? Start.Position + Vector2.Normalize(line) * Start.Radius : Start.Position;
            _finish = Finish.Visible ? Finish.Position - Vector2.Normalize(line) * Finish.Radius : Finish.Position;
        }


        public void Draw(Vector2 offSet, Color color)
        {
            BeebappsGame.Current.SpriteBatch.DrawLine(_start + offSet, _finish + offSet, color, 4f);
        }
    }
}
