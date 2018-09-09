using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class EasingUtil : GameComponent
    {
        public EasingUtil(double easeTimeMS)
            : base(BeebappsGame.Current)
        {
            _easeTimeMS = easeTimeMS;
            _prevPosition = NonInitialized;
        }

        private double _easeTimeMS;
        private double _inStartTimeMS;
        private double _outStartTimeMS;
        private static readonly Vector2 NonInitialized = new Vector2(float.MinValue);

        private bool _easeIn;
        private bool _easeOut;

        private Vector2 _position;
        private Vector2 _prevPosition;
        private Vector2 _nextPosition;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_easeIn && _prevPosition.Equals(NonInitialized))
                {
                    _prevPosition = value;
                }
                _nextPosition = value;
            }
        }

        public void StartEaseIn()
        {
            if (!_easeIn)
            {
                _easeIn = true;
                _easeOut = false;
                _inStartTimeMS = _currentTimeMS;
                _prevPosition = NonInitialized;
            }
        }

        public void StartEaseOut()
        {
            if (!_easeOut)
            {
                _easeIn = false;
                _easeOut = true;
                _outStartTimeMS = _currentTimeMS;
                _prevPosition = _position;
            }
        }

        private double _currentTimeMS;

        public override void Update(GameTime gameTime)
        {
            _currentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;
            if (_easeIn)
            {
                if (_inStartTimeMS + _easeTimeMS < _currentTimeMS)
                {
                    _easeIn = false;
                    _position = _nextPosition;
                }
                else if (!_nextPosition.Equals(_prevPosition))
                {
                    Vector2 add = _nextPosition - _prevPosition;
                    //Her sker egentlig EaseIn
                    _position = _prevPosition +
                                Vector2.Normalize(add) *
                                (float)Math.Sin(0.5f * Math.PI * (float)((_currentTimeMS - _inStartTimeMS) / _easeTimeMS));
                }
                else
                {
                    _position = _nextPosition;
                }
            }
            else if (_easeOut)
            {
                if (_outStartTimeMS + _easeTimeMS < _currentTimeMS)
                {
                    _easeOut = false;
                    _position = _nextPosition;
                }
                else if(!_nextPosition.Equals(_prevPosition))
                {
                    Vector2 add = _nextPosition - _prevPosition;
                    //Her sker egentlig EaseOut
                    _position = _prevPosition +
                                Vector2.Normalize(add) *
                                (float)Math.Sin(0.5f * Math.PI * (float)((_currentTimeMS - _outStartTimeMS) / _easeTimeMS));
                }
                else
                {
                    _position = _nextPosition;
                }
            }
            else
            {
                _position = _nextPosition;
            }
        }
    }
}
