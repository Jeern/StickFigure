using System.Collections.Generic;
using System.Linq;
using C3.XNA;
using Microsoft.Xna.Framework;
using StickFigure.Helpers;
using StickFigure.Input;

namespace StickFigure.Drawing
{
    public class MarkingRectangleManager
    {
        private readonly MouseCursor _cursor;
        private readonly JointManager _jointManager;

        public MarkingRectangleManager(MouseCursor cursor, JointManager jointManager)
        {
            _cursor = cursor;
            _jointManager = jointManager;
        }

        private Rectangle? _currentRectangle;
        private Vector2 _dragOffSetSinceLastUpdate;
        private Vector2 _mouseAtLastUpdate;

        private Vector2 _markStart;
        private Vector2 _markCurrent;


        public bool IsDragging { get; set; }
        public bool IsMarking { get; set; }
        public bool IsMarked { get; set; }

        public void Update(GameTime gameTime)
        {
            SetState();
            CalculateMarkingRectangle();
            DragCirclesToNewPositions();
            _mouseAtLastUpdate = _cursor.Position;
        }

        private void DragCirclesToNewPositions()
        {
            if (IsMarked)
            {
                _markStart += _dragOffSetSinceLastUpdate;
                _markCurrent += _dragOffSetSinceLastUpdate;
                //_jointManager.MoveAllCirclesWithinRectangleOffset(_currentRectangle, _dragOffSetSinceLastUpdate);
                foreach (var concreteJoint in _jointsWithinRectangle)
                {
                    concreteJoint.Position += _dragOffSetSinceLastUpdate;
                }
                _jointManager.RemoveAnyJointOutsideArea();
            }
        }

        private void CalculateMarkingRectangle()
        {
            if (!IsMarked && !IsMarking)
            {
                _currentRectangle = null;
            }
            else if (_markStart.X == _markCurrent.X || _markStart.Y == _markCurrent.Y)
            {
                _currentRectangle = null;
            }
            else if (_markStart.X < _markCurrent.X && _markStart.Y < _markCurrent.Y)
            {
                _currentRectangle = new Rectangle((int)_markStart.X, (int)_markStart.Y, (int)(_markCurrent.X - _markStart.X), 
                    (int)(_markCurrent.Y - _markStart.Y));
            }
            else if (_markStart.X < _markCurrent.X && _markStart.Y > _markCurrent.Y)
            {
                _currentRectangle = new Rectangle((int)_markStart.X, (int)_markCurrent.Y, (int)(_markCurrent.X - _markStart.X),
                    (int)(_markStart.Y - _markCurrent.Y));
            }
            else if (_markStart.X > _markCurrent.X && _markStart.Y > _markCurrent.Y)
            {
                _currentRectangle = new Rectangle((int)_markCurrent.X, (int)_markCurrent.Y, (int)(_markStart.X - _markCurrent.X),
                    (int)(_markStart.Y - _markCurrent.Y));
            }
            else if (_markStart.X > _markCurrent.X && _markStart.Y < _markCurrent.Y)
            {
                _currentRectangle = new Rectangle((int)_markCurrent.X, (int)_markStart.Y, (int)(_markCurrent.X - _markStart.X),
                    (int)(_markStart.Y - _markCurrent.Y));
            }
        }

        private List<ConcreteJoint> _jointsWithinRectangle = new List<ConcreteJoint>();

        private void SetState()
        {
            if (MouseExtended.Current.WasSingleClick(MouseButton.Right))
            {
                IsMarked = false;
                IsDragging = false;
                IsMarking = false;
                return;
            }

            if (_jointManager.IsDragging)
                return;

            if (!IsDragging && !IsMarking && _jointManager.IsTouching(_cursor))
                return;

            if(!_jointManager.IsWithinDrawArea(_cursor))
                return;

            var bse = MouseExtended.Current.CurrentState.ButtonStateExtended(MouseButton.Left);

            if (bse.CheckState(ButtonStateExtended.IsReleased))
            {
                _dragOffSetSinceLastUpdate = Vector2.Zero;
            }
            if ( bse.CheckState(ButtonStateExtended.DraggingPressed) && IsDragging)
            {
                _dragOffSetSinceLastUpdate = _cursor.Position - _mouseAtLastUpdate;
            }
            else if (bse.CheckState(ButtonStateExtended.DraggingPressed) && IsMarking)
            {
                _markCurrent = _cursor.Position;
            }
            else if (bse.CheckState(ButtonStateExtended.JustPressed) && !_jointManager.IsTouching(_cursor))
            {
                bool cursorInRectangle = _currentRectangle?.Contains(_cursor.Position) ?? false;
                if (IsMarked && cursorInRectangle)
                {
                    IsDragging = true;
                    IsMarking = false;
                    _dragOffSetSinceLastUpdate = Vector2.Zero;
                }
                else if(IsMarked || !cursorInRectangle)
                {
                    IsDragging = false;
                    IsMarking = true;
                    IsMarked = false;
                    _markStart = _cursor.Position;
                }
                else
                {
                    IsDragging = false;
                    IsMarking = false;
                    IsMarked = false;
                }
            }
            else if (bse.CheckState(ButtonStateExtended.JustReleased) && IsMarking)
            {
                IsDragging = false;
                IsMarking = false;
                IsMarked = true;
                _jointsWithinRectangle = _jointManager.GetJointsWithinRectangle(_currentRectangle).ToList();
            }
        }

        public void Draw(GameTime gameTime)
        {
            if ((IsMarking || IsMarked) && _currentRectangle.HasValue)
            {
                TheGame.Current.SpriteBatch.DrawRectangle(_currentRectangle.Value, Color.Black);
            }
        }
    }
}
