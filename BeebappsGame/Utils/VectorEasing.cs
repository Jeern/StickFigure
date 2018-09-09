using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class VectorEasing : GameComponent 
    {
        public float Change { get; private set; }
        private readonly double _easeTimeMS;
        private Vector2 _previous;
        private Vector2 _currentStart;
        
        public Vector2 Current
        {
            get; private set; 
        }
        
        private Vector2 _diff;
        private bool _start;
        private double _startTimeMS;

        /// <summary>
        /// Change er hvor meget det tillades at ændre Vectoren
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="change"></param>
        /// <param name="easeTimeMS"></param>
        public VectorEasing(float change, double easeTimeMS)
            : base(BeebappsGame.Current)
        {
            Change = change;
            _easeTimeMS = easeTimeMS;
        }

        public void Set(Vector2 current)
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
                if (!_diff.Equals(Vector2.Zero))
                {
                    Current = _currentStart +
                              Vector2.Normalize(_diff) * Change *
                              (float) Math.Sin(0.5f*Math.PI*(float) ((currentTimeMS - _startTimeMS)/_easeTimeMS));
                }
                else
                {
                    Current = _currentStart;
                }
            }
        }



    }
}
