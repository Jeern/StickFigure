using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Scenes
{
    public class SceneChange
    {
        public SceneChange(Scene from, Scene to, Func<GameTime, bool> changeChecker, SceneChangeType sceneChangeType)
        {
            From = from;
            To = to;
            ChangeChecker = changeChecker;
            SceneChangeType = sceneChangeType;
        }

        public Scene From { get; private set; }
        public Scene To { get; private set; }
        public Func<GameTime, bool> ChangeChecker { get; private set; }
        public SceneChangeType SceneChangeType { get; set; }
    }
}
