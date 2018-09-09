using System;

namespace Beebapps.Game.Scenes
{
    public interface IScene
    {
        void OnEnter(Scene previousScene);
        void OnExit();
        TimeSpan StartTime { get; set; }
    }
}
