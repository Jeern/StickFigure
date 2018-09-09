using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.GraphicUtils
{
    public interface ITextureLoader
    {
        float ScreenWidth { get; }
        float ScreenHeight { get; }
        Vector2 GlobalOffSet { get; }
        Vector2 GlobalTextureScale { get;  }
        Texture2D Get(string name);
        float GlobalFontScale { get; }
        float RetinaFactor { get; }
    }
}
