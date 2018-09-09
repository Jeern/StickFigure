using System.Collections.Generic;
using System.Diagnostics;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Beebapps.Game.Input;

namespace Beebapps.Game.Scenes
{
    public class SceneScheduler : GameComponent
    {
        public SceneScheduler() : base(BeebappsGame.Current)
        {
        }

        private readonly Dictionary<Scene, List<SceneChange>> _sceneChanges = new Dictionary<Scene, List<SceneChange>>();

        private static SceneSwipeAnimation _sceneSwipeAnimation;
        private static SceneSwipeAnimation SceneSwipeAnimation
        {
            get { return _sceneSwipeAnimation ?? (_sceneSwipeAnimation = new SceneSwipeAnimation()); }
        }

        public void AddSceneChange(SceneChange sceneChange)
        {
            if (CurrentScene == null)
            {
                CurrentScene = sceneChange.From;
            }
            if (_sceneChanges.ContainsKey(sceneChange.From))
            {
                _sceneChanges[sceneChange.From].Add(sceneChange);
            }
            else
            {
                _sceneChanges.Add(sceneChange.From, new List<SceneChange> { sceneChange });
            }
        }

        private Scene _currentScene;
        public Scene CurrentScene
        {
            get
            {
                return _currentScene;
            }
            set
            {
                HideAndDisableScene(_currentScene);
                _currentScene = value;
                ShowAndEnableScene(_currentScene);
            }
        }

        private void GetReadyForSwipeAnimationLeft(Scene currentScene, Scene nextScene, bool leftAnimation)
        {
            SceneSwipeAnimation.OldScene = currentScene;
            SceneSwipeAnimation.CurrentScene = nextScene;
            SceneSwipeAnimation.LeftAnimation = leftAnimation;
            currentScene.Camera.Position = Vector2.Zero;
            currentScene.Camera.OriginalPosition = currentScene.Camera.Position;
            nextScene.Camera.Position = leftAnimation ? new Vector2(-Misc.Iphone3ScreenWidth, 0) : new Vector2(Misc.Iphone3ScreenWidth, 0);
            nextScene.Camera.OriginalPosition = nextScene.Camera.Position;
            currentScene.Show();
            nextScene.Show();
            currentScene.Disable();
            nextScene.Disable();
            SceneSwipeAnimation.Start();
        }

        public static Scene NextScene { get; private set; }

        private bool _firstUpdate = true;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (SceneSwipeAnimation.IsFinished)
            {
                HideAndDisableScene(SceneSwipeAnimation.OldScene);
                UnloadScene(SceneSwipeAnimation.OldScene);
                SceneSwipeAnimation.Reset();
                CurrentScene.OnSwiped();
                CurrentScene.Camera = Camera.Empty;
                ShowAndEnableScene(CurrentScene);
                return;
            }
            if (SceneSwipeAnimation.IsStarted)
            {
                SceneSwipeAnimation.Update();
                SceneSwipeAnimation.OldScene.Camera.Position = SceneSwipeAnimation.CurrentValue;
                SceneSwipeAnimation.CurrentScene.Camera.Position = SceneSwipeAnimation.CurrentValue +
                                                                   (SceneSwipeAnimation.LeftAnimation
                                                                        ? new Vector2(-Misc.Iphone3ScreenWidth, 0)
                                                                        : new Vector2(Misc.Iphone3ScreenWidth, 0));
                return;
            }

            if (_sceneChanges.ContainsKey(CurrentScene))
            {
                if (_firstUpdate)
                {
                    _firstUpdate = false;
                    LoadScene(CurrentScene);
                    CurrentScene.OnEnter(null);
                }

                foreach (SceneChange sc in _sceneChanges[CurrentScene])
                {
                    if (sc.ChangeChecker(gameTime))
                    {
                        NextScene = sc.To;
                        if (CurrentScene != null)
                        {
                            CurrentScene.OnExit();
                            if (sc.SceneChangeType == SceneChangeType.Normal)
                            {
                                UnloadScene(CurrentScene);
                            }
                        }
                        Scene previousScene = CurrentScene;
                        if (sc.SceneChangeType == SceneChangeType.Normal)
                        {
                            CurrentScene = NextScene;
                            CurrentScene.Camera = Camera.Empty;
                        }
                        else if(sc.SceneChangeType == SceneChangeType.AutoSwipeLeft)
                        {
                            GetReadyForSwipeAnimationLeft(_currentScene, NextScene, true);
                            _currentScene = NextScene;
                        }
                        else if (sc.SceneChangeType == SceneChangeType.AutoSwipeRight)
                        {
                            GetReadyForSwipeAnimationLeft(_currentScene, NextScene, false);
                            _currentScene = NextScene;
                        }
                        CurrentScene.StartTime = gameTime.TotalRealTime();
                        LoadScene(CurrentScene);
                        CurrentScene.OnEnter(previousScene);
                    }
                }
            }
        }

        private void UnloadScene(Scene scene)
        {
            scene.Unload();
            //var swipeScene = scene as ISwipeableScene;
            //if (swipeScene != null)
            //{
            //    if (swipeScene.LeftScene != null)
            //    {
            //        swipeScene.LeftScene.Unload();
            //    }
            //    if (swipeScene.RightScene != null)
            //    {
            //        swipeScene.RightScene.Unload();
            //    }
            //}
        }

        private void LoadScene(Scene scene)
        {
            scene.Load();
            //var swipeScene = scene as ISwipeableScene;
            //if(swipeScene != null)
            //{
            //    if(swipeScene.LeftScene != null)
            //    {
            //        swipeScene.LeftScene.Load();
            //    }
            //    if (swipeScene.RightScene != null)
            //    {
            //        swipeScene.RightScene.Load();
            //    }
            //}
            //if (allowPreload && scene is IAutoSwipeLeftScene)
            //{
            //    foreach (var sceneChange in _sceneChanges[scene])
            //    {
            //        PreloadScene(sceneChange.To);
            //    }
            //}
        }

        //private void PreloadScene(Scene scene)
        //{
        //    LoadScene(scene, false);
        //    HideAndDisableScene(scene);
        //}

        private void HideAndDisableScene(Scene scene)
        {
            scene.Hide();
            scene.Disable();
            //var swipeScene = scene as ISwipeableScene;
            //if (swipeScene != null)
            //{
            //    if (swipeScene.LeftScene != null)
            //    {
            //        swipeScene.LeftScene.Hide();
            //        swipeScene.LeftScene.Disable();
            //    }
            //    if (swipeScene.RightScene != null)
            //    {
            //        swipeScene.RightScene.Hide();
            //        swipeScene.RightScene.Disable();
            //    }
            //}
        }
        private void ShowAndEnableScene(Scene scene)
        {
            scene.Enable();
            scene.Show();
            //var swipeScene = scene as ISwipeableScene;
            //if (swipeScene != null)
            //{
            //    if (swipeScene.LeftScene != null)
            //    {
            //        swipeScene.LeftScene.Show();
            //    }
            //    if (swipeScene.RightScene != null)
            //    {
            //        swipeScene.RightScene.Show();
            //    }
            //}
        }
    }
}
