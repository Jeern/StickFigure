using System;
using System.Collections.Generic;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Scenes
{
    public class Scene : DrawableGameComponent, ILoadable, IScene
    {
        private static Scene _empty;
        public static Scene Empty
        {
            get { return _empty ?? (_empty = new Scene()); }
        }

        public Camera Camera { get; set; }

        public Scene(): base(BeebappsGame.Current)
        {
            Name = GetType().Name;
            Camera = Camera.Empty;
            this.Disable();
        }

        public string Name { get; set; }
        public Color BackColor { get; set; }

        private readonly List<GameComponent> _components = new List<GameComponent>();

        public List<GameComponent> Components
        {
            get
            {
                return _components;
            }
        }

        protected void AddComponent(GameComponent component)
        {
            AddComponentNoUpdate(component);
            Game.Components.Add(component);
        }

        protected void AddComponent(DrawableGameComponent component)
        {
            AddComponentNoUpdate(component);
            Game.Components.Add(component);
        }

        protected void AddComponentNoUpdate(GameComponent component)
        {
            component.Enabled = Enabled;
            Components.Add(component);
        }

        protected void AddComponentNoUpdate(DrawableGameComponent component)
        {
            component.Enabled = Enabled;
            component.Visible = Visible;
            Components.Add(component);
        }

        protected void RemoveComponent(GameComponent component)
        {
            Components.Remove(component);
            Game.Components.Remove(component);
        }

        protected void RemoveComponentNoUpdate(GameComponent component)
        {
            Components.Remove(component);
        }

        public bool IsLoaded { get; set; }

        public virtual void Load()
        {
			IsLoaded = true;
            foreach (GameComponent component in Components) 
            {
                var loadable = component as ILoadable;
                if (loadable != null)
                {
                    loadable.Load();
                }
            }
        }

        public virtual void OnEnter(Scene previousScene)
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void Unload()
        {
            IsLoaded = false;
        }

        public TimeSpan StartTime
        {
            get;
            set;
        }

        public virtual void OnSwiped()
        {
            
        }
    }
}
