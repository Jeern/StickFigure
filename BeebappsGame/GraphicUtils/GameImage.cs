using System;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.GraphicUtils
{
    public class GameImage
    {
        private readonly Func<int> _delayMethod = () => 0;

        public GameImage(Texture2D texture) : this(texture, 0) { }

        public GameImage(Texture2D texture, int delay) : this(texture, () => delay) { }

        public GameImage(Texture2D texture, Func<int> delayMethod)
        {
            Texture = texture;
            _delayMethod = delayMethod;
        }

        public Texture2D Texture
        {
            get;
            private set;
        }

        public int Delay
        {
            get { return _delayMethod(); }
        }

        public static implicit operator Texture2D(GameImage image)
        {
            return image.Texture;
        }
    }
}
