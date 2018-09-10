using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class Line
    {
        public Joint Start { get; }
        public Joint Finish { get; }

        private Vector2 _start;
        private Vector2 _finish;

        public Line(Joint start, Joint finish)
        {
            Start = start;
            Finish = finish;
        }

        public void Update(GameTime gameTime)
        {
            var line = Finish.Position - Start.Position;
            _start = Start.Position + Vector2.Normalize(line) * Start.Radius;
            _finish = Finish.Position - Vector2.Normalize(line) * Finish.Radius;
        }


        public void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.DrawLine(_start, _finish, Color.Black, 4f);
        }
    }
}
