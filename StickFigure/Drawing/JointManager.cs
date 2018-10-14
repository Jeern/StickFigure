using System.Collections.Generic;
using System.Linq;
using Beebapps.Game.Input;
using Beebapps.Game.Utils;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickFigure.Helpers;

namespace StickFigure.Drawing
{
    public class JointManager
    {
        private readonly MouseCursor _mouseCursor;
        private readonly LineManager _lineManager;
        private bool _draggingLine;
        private ConcreteJoint _lineStartJoint;

        public JointManager(MouseCursor mouseCursor, LineManager lineManager)
        {
            _mouseCursor = mouseCursor;
            _lineManager = lineManager;
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 150), 40, 5, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 240), 30, 4, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 300), 10, 3, true));
            _templateJoints.Add(new TemplateJoint(new Vector2(50, 340), 10, 2, false));
        }

        private readonly List<TemplateJoint> _templateJoints = new List<TemplateJoint>();
        private List<ConcreteJoint> _concreteJoints = new List<ConcreteJoint>();

        public void Draw(Vector2 offset, bool drawFinal, Color color)
        {
            if (!drawFinal)
            {
                foreach (var joint in _templateJoints)
                {
                    joint.Draw(offset, false, color);
                }
            }

            foreach (var joint in _concreteJoints)
            {
                joint.Draw(offset, drawFinal, color);
            }

            if (_draggingLine && _lineStartJoint != null)
            {
                BeebappsGame.Current.SpriteBatch.DrawLine(_lineStartJoint.Position, _mouseCursor.Position, Color.Gray, 4f);
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
                return;
            }

            var templateJoint = _draggingJoint as TemplateJoint;
            var concreteJoint = _draggingJoint as ConcreteJoint;
            if (concreteJoint != null)
            {
                if (!IsWithinDrawArea(_mouseCursor))
                {
                    _concreteJoints.Remove(concreteJoint);
                    _lineManager.DeleteJoint(concreteJoint);
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

            if (MouseExtended.Current.CurrentState.RightButton == ButtonState.Pressed)
            {
                if (!_draggingLine)
                {
                    _lineStartJoint = GetFirstTouching(_mouseCursor) as ConcreteJoint;
                    if (_lineStartJoint != null)
                    {
                        _draggingLine = true;
                    }
                }
            }
            else
            {
                if (_draggingLine)
                {
                    var lineEndJoint = GetFirstTouching(_mouseCursor) as ConcreteJoint;
                    if (lineEndJoint != null)
                    {
                        _lineManager.AddConnection(_lineStartJoint, lineEndJoint);
                    }
                    _draggingLine = false;
                    _lineStartJoint = null;
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

        public JointFile ToFile()
        {
            return new JointFile {
                ConcreteJoints = _concreteJoints,
                Lines = _lineManager.Lines
            };
        }

        public void FromFile(JointFile file)
        {
            _concreteJoints = file.ConcreteJoints;
            var jointDict = _concreteJoints.ToDictionary(j => j.Id, j => j);
            var lines = new List<Line>();
            foreach (var line in file.Lines)
            {
                var start = jointDict.ContainsKey(line.Start.Id) ? jointDict[line.Start.Id] : null;
                var end = jointDict.ContainsKey(line.Finish.Id) ? jointDict[line.Finish.Id] : null;
                if(start == null || end == null)
                    continue;
                lines.Add(new Line(start,  end));
            }

            _lineManager.Lines = lines;
        }
    }
}
