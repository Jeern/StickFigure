using System;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    //PinchZoomArea understøtter zoom vha. Pinch imellem en Min og en Max værdi
    public class PinchZoomArea : GameComponent
    {
        private DoubleRectangle Zoomarea { get; set; }
        private float MinZoom { get; set; }
        private float MaxZoom { get; set; }

        public Vector2 ZoomStartPosition { get; private set; }

        //public bool ZoomDirectionHasChanged { get; private set; }

        private float _currentZoom;
        public float CurrentZoom
        {
            get 
            { 
                return _currentZoom; 
            }
            private set
            {
                _currentZoom = Math.Min(value, MaxZoom);
                _currentZoom = Math.Max(_currentZoom, MinZoom);
            }
        }
        public bool ZoomChanged { get; private set; }

        private float _previousPinch;

        public PinchZoomArea(DoubleRectangle zoomArea, float minZoom, float maxZoom)
            : base(BeebappsGame.Current)
        {
            Zoomarea = zoomArea;
            MinZoom = minZoom;
            MaxZoom = maxZoom;
            CurrentZoom = minZoom;
            _zoomStart = CurrentZoom;
            _pinchStart = 0f;
            //ZoomDirectionHasChanged = false;
        }

        private float _zoomStart;
        private float _pinchStart;

        private float _previousPinchDirection;

        private bool UpdateRun { get; set; }

        public override void Update(GameTime gameTime)
        {
            if(!UpdateRun)
            {
                _previousFinger1Position = PinchTouch.Current.Finger1Position;
                _previousFinger2Position = PinchTouch.Current.Finger2Position;
                UpdateRun = true;
            }
            ZoomChanged = false;
            //ZoomDirectionHasChanged = false;
            float currentPinch = PinchTouch.Current.CurrentPinch;
            float currentPinchDirection = PinchDirection(currentPinch, _previousPinch);
            //if (currentPinchDirection != _previousPinchDirection)
            //{
            //    ZoomDirectionHasChanged = true;
            //}

            bool finger1InArea = Misc.PositionIsWithinArea(PinchTouch.Current.Finger1Position, Zoomarea);
            bool finger2InArea = Misc.PositionIsWithinArea(PinchTouch.Current.Finger2Position, Zoomarea);
            bool finger1WasInArea = Misc.PositionIsWithinArea(_previousFinger1Position, Zoomarea);
            bool finger2WasInArea = Misc.PositionIsWithinArea(_previousFinger2Position, Zoomarea);
            Vector2 finger1Delta = PinchTouch.Current.Finger1Position - _previousFinger1Position;
            Vector2 finger2Delta = PinchTouch.Current.Finger2Position - _previousFinger2Position;
            float delta1Length = finger1Delta.Length();
            float delta2Length = finger2Delta.Length();

            if (DeltaMove.Length() < 3f)
            {
                DeltaMove = Vector2.Zero;
            }
            else
            {
                DeltaMove = DeltaMove * 0.85f;
            }

            if (finger1InArea && finger1WasInArea && finger2InArea && finger2WasInArea && delta1Length < 120f && delta2Length < 120f)
            {
                DeltaMove = (finger1Delta + finger2Delta) / 2f;
            }
            else if (finger1InArea && finger1WasInArea && !finger2InArea && !finger2WasInArea &&  delta1Length < 120f)
            {
                DeltaMove = finger1Delta;
            }
            else if (finger2InArea && finger2WasInArea && !finger1InArea && !finger1WasInArea && delta2Length < 120f)
            {
                DeltaMove = finger2Delta;
            }

            //float minimumPinch = ZoomDirectionHasChanged ? 2f : 0f;

            if (currentPinch > 0 && _previousPinch > 0 && Math.Abs(currentPinch-_previousPinch) > 0f &&
                finger1InArea && finger1WasInArea &&
                finger2InArea && finger2WasInArea)
            {
                CurrentZoom = currentPinch/_previousPinch*CurrentZoom;
                ZoomChanged = true;
                ZoomStartPosition = PinchTouch.Current.ZoomStartPosition - new Vector2((float)Zoomarea.X, (float)Zoomarea.Y);
            }
            //else
            //{
            //    ZoomDirectionHasChanged = false;
            //}

            _previousPinchDirection = PinchDirection(currentPinch, _previousPinch);
            _previousPinch = currentPinch;
            _previousFinger1Position = PinchTouch.Current.Finger1Position;
            _previousFinger2Position = PinchTouch.Current.Finger2Position;
        }

        private Vector2 _previousFinger1Position;
        private Vector2 _previousFinger2Position;

        public Vector2 DeltaMove { get; set; }

        private float PinchDirection(float currentPinch, float previousPinch)
        {
            if (Math.Abs(currentPinch - previousPinch) < 3)
                return 0f;

            return Math.Sign(currentPinch - previousPinch);
        }

        public void ChangeZoom(float currentZoom)
        {
            CurrentZoom = currentZoom;
        }
    }
}
