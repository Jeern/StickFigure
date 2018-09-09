using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    //Definerer et område man kan dobbeltklikke i og skifte imellem minimum og maximum zoom.
    //Zoomen animeres med zoomSpeed
    public class ClickZoomArea : GameComponent 
    {
        private DoubleRectangle Zoomarea { get; set; }
        private float MinZoom { get; set; }
        private float MaxZoom { get; set; }
        private double ZoomTime { get; set; }
        private float GoalZoom { get; set; }
        private float StartZoom { get; set; }
        private double StartTime { get; set; }

        private Vector2  _zoomStartPosition;
        public Vector2 ZoomStartPosition
        {
            get
            {
                return _zoomStartPosition;
            }
            private set { _zoomStartPosition = value; }
        }

        public bool ZoomChanged { get; private set; }

        private float _currentZoom;
        public float CurrentZoom
        {
            get { return _currentZoom; }
            private set 
            {
                _currentZoom = Math.Min(value, MaxZoom);
                _currentZoom = Math.Max(_currentZoom, MinZoom);
                if(_currentZoom != value)
                {
                    GoalZoom = _currentZoom;
                }
            }
        }

        /// <summary>
        /// Kaldes når Zoom ændres udefra.
        /// </summary>
        /// <param name="zoom"></param>
        public void ChangeZoom(float zoom)
        {
            CurrentZoom = zoom;
            GoalZoom = zoom;
        }

        public ClickZoomArea(DoubleRectangle zoomarea, float minZoom, float maxZoom, double zoomTime)
            : base(BeebappsGame.Current)
        {
            Zoomarea = zoomarea;
            MinZoom = minZoom;
            CurrentZoom = minZoom; //Måske laver jeg på et tidspunkt en der kan starte med en anden GoalZoom.
            GoalZoom = minZoom;
            MaxZoom = maxZoom;
            ZoomTime = zoomTime;
            StartTime = double.MaxValue;
        }

        private const float SmallestStep = 0.05f;
        private const float NumberOfFinalSteps = 3;

        public override void Update(GameTime gameTime)
        {
            ZoomChanged = false;
            if(ClickTouch.Current.IsDoubleClick && Misc.PositionIsWithinArea(ClickTouch.Current.ClickPosition, Zoomarea))
            {
                ZoomStartPosition = ClickTouch.Current.ClickPosition - new Vector2((float)Zoomarea.X, (float)Zoomarea.Y);

                if(Math.Abs(MinZoom - CurrentZoom) < Math.Abs(MaxZoom - CurrentZoom))
                {
                    GoalZoom = MaxZoom;
                }
                else
                {
                    GoalZoom = MinZoom;
                }
                StartZoom = CurrentZoom;
                StartTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if(CurrentZoom != GoalZoom)
            {
                //if (CurrentZoom < GoalZoom)
                //{
                //    CurrentZoom += ZoomSpeed;
                //    ZoomChanged = true;
                //}
                //else if(CurrentZoom > GoalZoom)
                //{
                //    CurrentZoom -= ZoomSpeed;
                //    ZoomChanged = true;
                //}
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                if(StartTime + ZoomTime < currentTime)
                {
                    ZoomChanged = true;
                    CurrentZoom = GoalZoom;
                }
                else
                {
                    ZoomChanged = true;
                    if(Math.Abs(GoalZoom - CurrentZoom) < SmallestStep * NumberOfFinalSteps)
                    {
                        CurrentZoom = CurrentZoom + SmallestStep * Math.Sign(GoalZoom - CurrentZoom);
                    }
                    else
                    {
                        CurrentZoom = StartZoom + (GoalZoom - StartZoom) * ((float)Math.Sin(((currentTime - StartTime) / ZoomTime) * Math.PI - 0.5 * Math.PI) + 1f) / 2f;
                    }
                }
            }

        }
    }
}
