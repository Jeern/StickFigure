using System.Collections.Generic;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Scenes
{
    public class SceneSwipeAnimation : Animation<Vector2>
    {
        public Scene OldScene { get; set; }
        public Scene CurrentScene { get; set; }

        public bool LeftAnimation { get; set; }

        public SceneSwipeAnimation()
        {
            Elements = new List<Vector2>
                                   {
                                       Vector2.Zero, 
                                       new Vector2(0.65f, 0), 
                                       new Vector2(2.5f, 0), 
                                       new Vector2(10f, 0), 
                                       new Vector2(40f, 0), 
                                       new Vector2(160f, 0), 
                                       new Vector2(280f, 0), 
                                       new Vector2(310f, 0), 
                                       new Vector2(317.5f, 0), 
                                       new Vector2(319.4f, 0), 
                                       new Vector2(320f, 0)
                                   };
        }

        public override Vector2 CurrentValue
        {
            get { return LeftAnimation ? GetCurrentValue() : -GetCurrentValue(); }
        }

        private Vector2 GetCurrentValue()
        {
            if (Frame < 0)
                return Vector2.Zero;

            if (Frame > Elements.Count - 1)
            {
                IsFinished = true;
                return new Vector2(320, 0);
            }

            return Elements[Frame];
        }
    }
}
