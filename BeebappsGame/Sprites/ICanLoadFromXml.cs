using Beebapps.Game.Scenes;

namespace Beebapps.Game.Sprites
{
    public interface ICanLoadFromXml
    {
        string ComponentName { get; }
        Scene Scene { get; }
        bool IsLoadedFromXml { get; }
        void Load(Scene scene, string name);
        void Load(Scene scene);
    }
}
