using System.Collections.Generic;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StickFigure
{
    public class JointManager
    {
        private readonly MouseCursor _mouseCursor;

        public JointManager(MouseCursor mouseCursor)
        {
            _mouseCursor = mouseCursor;
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 150), 40, 5, Color.Black, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 240), 30, 4, Color.Black, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 300), 10, 3, Color.Black, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 340), 10, 2, Color.Red, false));
        }

        private readonly List<TemplateJoint> _templateJoints = new List<TemplateJoint>();
        private readonly List<ConcreteJoint> _concreteJoints = new List<ConcreteJoint>();

        public void Draw(GameTime gameTime)
        {
            foreach (var joint in _templateJoints)
            {
                joint.Draw(gameTime);
            }
            foreach (var joint in _concreteJoints)
            {
                joint.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (MouseExtended.Current.CurrentState.LeftButton == ButtonState.Pressed)
            {
                if (_draggingJoint == null)
                {
                    _draggingJoint = GetFirstTouching(_mouseCursor);
                }

                if (_draggingJoint != null)
                {
                    _draggingJoint.Position = _mouseCursor.Position;
                }
            }
            else
            {
                var templateJoint = _draggingJoint as TemplateJoint;
                var concreteJoint = _draggingJoint as ConcreteJoint;
                if (concreteJoint != null)
                {
                    if (!IsWithinDrawArea(_mouseCursor))
                    {
                        _concreteJoints.Remove(concreteJoint);
                    }
                    _draggingJoint = null;
                }
                else if (templateJoint != null)
                {
                    if (IsWithinDrawArea(_mouseCursor))
                    {
                        _concreteJoints.Add(new ConcreteJoint(templateJoint));
                    }

                    templateJoint.Position = templateJoint.OriginalPosition;
                    _draggingJoint = null;
                }
            }
        }

        private Joint GetFirstTouching(MouseCursor cursor)
        {
            foreach (var joint in _templateJoints)
            {
                if (joint.PointIsWithin(cursor.Position))
                    return joint;
            }
            foreach (var joint in _concreteJoints)
            {
                if (joint.PointIsWithin(cursor.Position))
                    return joint;
            }

            return null;
        }

        public Joint _draggingJoint;

        public bool IsWithinDrawArea(MouseCursor cursor)
        {
            if (cursor.Position.X < 100)
                return false;
            if (cursor.Position.Y < 50)
                return false;
            if (cursor.Position.X > Consts.ScreenWidth)
                return false;
            if (cursor.Position.Y > Consts.ScreenHeight)
                return false;
            return true;
        }
    }
}
