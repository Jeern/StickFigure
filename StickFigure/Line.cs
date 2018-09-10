using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class Line
    {
        public Joint Start { get; }
        public Joint Finish { get; }

        public Line(Joint start, Joint finish)
        {
            Start = start;
            Finish = finish;
        }

        public void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.DrawLine(Start.Position, Finish.Position, Color.Black, 4f);
        }
    }
}
