using System;
using System.Diagnostics;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    public class PinchTouch
    {
        public Vector2 ZoomStartPosition { get; private set; }
        public Vector2 Finger1Position { get; private set; }
        public Vector2 Finger2Position { get; private set; }
        private Vector2 _previousFinger1Position;
        private Vector2 _previousFinger2Position;

        private static PinchTouch _current;
        public static PinchTouch Current
        {
            get { return _current ?? (_current = new PinchTouch()); }
        }

        private float _currentPinch;

        public float CurrentPinch
        {
            get
            {
                return _currentPinch;
            }
            set { _currentPinch = value; }
        }

        public void GetState(GameTime currentTime)
        {
            Finger1Position = Misc.NonInitialized;
            Finger2Position = Misc.NonInitialized;
            TouchCollection touches = TouchPanel.GetState();

            if(touches.Count > 0)
            {
                Finger1Position = touches[0].Position;
                if(touches.Count > 1)
                {
                    Finger2Position = touches[1].Position;
                }
            }

            if (touches.Count <= 1)
            {
                ResetPinch();
            }
            else if (IsFingerOnScreen(touches[0].State) && IsFingerOnScreen(touches[1].State))
            {
                var finger1Direction = new Direction(Finger1Position - _previousFinger1Position, 1f);
                var finger2Direction = new Direction(Finger2Position - _previousFinger2Position, 1f);

                //if(IsPinchMove2(Finger1Position, _previousFinger1Position, Finger2Position, _previousFinger2Position))
                if (IsPinchMove(finger1Direction, finger2Direction))
                {
                        CurrentPinch = Math.Abs((Finger1Position - Finger2Position).Length());
                        ZoomStartPosition = 0.5f * Finger1Position + 0.5f * Finger2Position;
                }

                _previousFinger1Position = Finger1Position;
                _previousFinger2Position = Finger2Position;
            }
            else
            {
                ResetPinch();
            }
        }

        private bool IsFingerOnScreen(TouchLocationState state)
        {
            return state == TouchLocationState.Moved || state == TouchLocationState.Pressed;
        }

        private bool IsPinchMove(Direction finger1Direction, Direction finger2Direction)
        {
            //if (IsMoving)
            //{
                if (finger1Direction.Down && finger2Direction.Down)
                    return false;
                if (finger1Direction.Up && finger2Direction.Up)
                    return false;
                if (finger1Direction.Left && finger2Direction.Left)
                    return false;
                if (finger1Direction.Right && finger2Direction.Right)
                    return false;
                if (!(finger1Direction.Down || finger1Direction.Up || finger1Direction.Left || finger1Direction.Right))
                    return false;
                if (!(finger2Direction.Down || finger2Direction.Up || finger2Direction.Left || finger2Direction.Right))
                    return false;

                return true;
            //}
            //else
            //{
            //    if (finger1Direction.Down && !finger2Direction.Down)
            //        return true;
            //    if (finger1Direction.Up && !finger2Direction.Up)
            //        return true;
            //    if (finger1Direction.Left && !finger2Direction.Left)
            //        return true;
            //    if (finger1Direction.Right && !finger2Direction.Right)
            //        return true;
            //    if (finger2Direction.Down && !finger1Direction.Down)
            //        return true;
            //    if (finger2Direction.Up && !finger1Direction.Up)
            //        return true;
            //    if (finger2Direction.Left && !finger1Direction.Left)
            //        return true;
            //    if (finger2Direction.Right && !finger1Direction.Right)
            //        return true;

            //    return false;
            //}
        }

        private bool IsPinchMove2(Vector2 finger1, Vector2 previousFinger1, Vector2 finger2, Vector2 previousFinger2)
        {
            Vector2 move1 = (finger1 - previousFinger1);
            Vector2 move2 = (finger2 - previousFinger2);
            Vector2 totalMove = move1 + move2; 

            //Da pinch er modsat rettet vil en totalmove der er meget lav - tæt på nul, indikerer en pinch.
            return totalMove.Length() < 10f;
        }


        private void ResetPinch()
        {
            CurrentPinch = 0f;
            ZoomStartPosition = Vector2.Zero;
            _previousFinger1Position = Finger1Position;
            _previousFinger2Position = Finger2Position;
        }
    }
}
