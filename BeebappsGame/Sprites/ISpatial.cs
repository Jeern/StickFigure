using Microsoft.Xna.Framework;

namespace Beebapps.Game.Sprites
{
    public interface ISpatial
    {
        Vector2 Position { get; set; }
        float Width { get; }
        float Height { get; }
    }
}
