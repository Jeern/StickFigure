using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class TimedEvent : GameComponent
    {
        private readonly Func<bool> _stopCondition;
        private readonly double _milliSecondsBetweenEvents;
        private readonly Action _eventToHappen;
        private readonly int _maxNumberOfEvents;
        private double _measuredTime;
        private int _numberOfEvents;
        private bool _restarted;

        public TimedEvent(double milliSecondsBetweenEvents, Action eventToHappen, int maxNumberOfEvents) 
            : this(milliSecondsBetweenEvents, eventToHappen, () => false, maxNumberOfEvents)
        {
        }

        public TimedEvent(double milliSecondsBetweenEvents, Action eventToHappen, Func<bool> stopCondition)
            : this(milliSecondsBetweenEvents, eventToHappen, stopCondition, int.MaxValue)
        {
        }

        public TimedEvent(double milliSecondsBetweenEvents, Action eventToHappen, Func<bool> stopCondition, int maxNumberOfEvents)
            : base(BeebappsGame.Current)
        {
            _milliSecondsBetweenEvents = milliSecondsBetweenEvents;
            _eventToHappen = eventToHappen;
            _stopCondition = stopCondition;
            _maxNumberOfEvents = maxNumberOfEvents;
            _measuredTime = double.MaxValue;
            _numberOfEvents = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if(_restarted)
            {
                _restarted = false;
                _numberOfEvents = 0;
                ResetTime(gameTime);
            }
            else if(_numberOfEvents < _maxNumberOfEvents 
                &&  _measuredTime + _milliSecondsBetweenEvents < gameTime.TotalGameTime.TotalMilliseconds
                && !_stopCondition())
            {
                _numberOfEvents++;
                ResetTime(gameTime);
                _eventToHappen();
            }
        }

        public void Restart()
        {
            _restarted = true;
        }

        public void ResetTime(GameTime gameTime)
        {
            _measuredTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
    }
}
