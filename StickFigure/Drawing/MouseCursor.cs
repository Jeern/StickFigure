﻿using C3.XNA;
using Microsoft.Xna.Framework;
using StickFigure.Helpers;
using StickFigure.Input;

namespace StickFigure.Drawing
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
            TheGame.Current.SpriteBatch.DrawCircle(Position, 4, 20, Color.Black, 2);
        }
    }
}
