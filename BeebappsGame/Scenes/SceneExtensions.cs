using System;
using System.Collections.Generic;
using System.Diagnostics;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Scenes
{
    public static class SceneExtensions
    {
        public static void Disable(this GameComponent component)
        {
            if (component == null)
                return;
            component.Enabled = false;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                comp.Disable();
            }
        }

        public static void Enable(this GameComponent component)
        {
            if (component == null)
                return;
            component.Enabled = true;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                if (comp != null && comp.CanEnableAutomatically())
                {
                    comp.Enable();
                }
            }
        }

        public static void Show(this DrawableGameComponent component)
        {
            if (component == null)
                return;
            component.Visible = true;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                var drawable = comp as DrawableGameComponent;
                if (drawable != null && drawable.CanEnableAutomatically())
                {
                    drawable.Show();
                }
                else if(drawable != null && !drawable.CanEnableAutomatically())
                {
                    Debug.WriteLine(drawable.GetType().FullName);
                }
            }
        }

        private static Hashset<GameComponent> _onlyEnableManually = new Hashset<GameComponent>();

        public static void OnlyEnableManually(this GameComponent component)
        {
            if (!_onlyEnableManually.Contains(component))
            {
                _onlyEnableManually.Add(component);
            }
        }

        public static bool CanEnableAutomatically(this GameComponent component)
        {
            return !_onlyEnableManually.Contains(component);
        }


        public static void Hide(this DrawableGameComponent component)
        {
            if (component == null)
                return;
            component.Visible = false;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                var drawable = comp as DrawableGameComponent;
                if (drawable != null)
                {
                    drawable.Hide();
                }
            }
        }
    }
}
