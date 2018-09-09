using Beebapps.Game.Sprites;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class Camera
    {
        public Sprite Sprite { get; private set; }
        
        public static Camera Empty
        {
            get { return new Camera(Vector2.Zero);  }
        }

        private Vector2 _position;
        public Vector2 Position 
        {
            get 
            {
                if (Sprite == null)
                    return _position;

                return _position + new Vector2(Sprite.Position.X, 0); 
            }
            set { _position = value; }
        }

        public Vector2 OriginalPosition { get; set; }

        public Camera(Vector2 position) : this(null, position)
        {
        }

        public Camera(Sprite sprite, Vector2 position)
        {
            Sprite = sprite;
            _position = position;
            OriginalPosition = position;
        }
    }
}
