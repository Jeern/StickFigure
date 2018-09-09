using Beebapps.Game.Input;
using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;

namespace StickFigure
{
    public class MouseCursor
    {
        public void Draw(GameTime gameTime)
        {
            var state = MouseExtended.Current.CurrentState;
            var mousePos = new Vector2(state.X, state.Y);
            BeebappsGame.Current.SpriteBatch.DrawCircle(mousePos, 4, 20, Color.Black, 2);
        }
    }
}
