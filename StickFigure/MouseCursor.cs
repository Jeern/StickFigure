using Beebapps.Game.Input;
using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class MouseCursor
    {
        public Vector2 Position { get; private set; }

        public void Update(GameTime gameTime)
        {
            var state = MouseExtended.Current.CurrentState;
            Position = new Vector2(state.X, state.Y);
        }

        public void Draw(GameTime gameTime)
        {
            BeebappsGame.Current.SpriteBatch.DrawCircle(Position, 4, 20, Color.Black, 2);
        }
    }
}
