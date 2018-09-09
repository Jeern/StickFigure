using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class FloatEasing : GameComponent 
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Change { get; set; }
        private readonly double _easeTimeMS;
        private float _previous;
        private float _currentStart;
        
        public float Current
        {
            get { return _current; }
            private set 
            { 
                _current = Math.Min(value, Max);
                _current = Math.Max(_current, Min);
            }
        }
        
        private float _diff;
        private bool _start;
        private double _startTimeMS;
        private float _current;

        /// <summary>
        /// Min er det mindste det tillades at Current antager, max det højeste. change er hvor meget det tillades at ændre floaten
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="change"></param>
        /// <param name="easeTimeMS"></param>
        public FloatEasing(float min, float max, float change, double easeTimeMS)
            : base(BeebappsGame.Current)
        {
            Min = min;
            Max = max;
            Change = change;
            _easeTimeMS = easeTimeMS;
        }

        public void Set(float current)
        {
            _previous = _currentStart;
            _currentStart = current;
            _diff = _currentStart - _previous;
            _start = true;
        }

        public override void Update(GameTime gameTime)
        {
            double currentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;
            if(_start)
            {
                _start = false;
                _startTimeMS = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (_startTimeMS + _easeTimeMS < currentTimeMS)
            {
                _start = false;
            }
            else
            {
                Current = _currentStart + Math.Sign(_diff) * Change * (float)Math.Sin(0.5f * Math.PI * (float)((currentTimeMS - _startTimeMS) / _easeTimeMS));
            }
        }



    }
}
