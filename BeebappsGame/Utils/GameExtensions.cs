namespace Beebapps.Game.Utils
{
    public static class GameExtensions
    {
        public static float Height(this Microsoft.Xna.Framework.Game game)
        {
            return game.GraphicsDevice.Viewport.Height;
        }

        public static float Width(this Microsoft.Xna.Framework.Game game)
        {
            return game.GraphicsDevice.Viewport.Width;
        }

    }
}
