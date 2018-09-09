using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    public class Timer : GameComponent
    {
        private event EventHandler<EventArgs<GameTime>> _timesUp = delegate { };

        public bool Repeat { get; set; }

        public float DelayInMilliseconds { get; set; }

        public event EventHandler<EventArgs<GameTime>> TimesUp
        {
            add { _timesUp += value; }
            remove { _timesUp -= value; }
        }

        private float _millisecondsLeft;


        public Timer(int delay) : base(BeebappsGame.Current)
        {
            DelayInMilliseconds = delay;
            _millisecondsLeft = DelayInMilliseconds;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _millisecondsLeft -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_millisecondsLeft <= 0)
            {
                OnTimesUp(gameTime);
            }
        }

        public void Reset()
        {
            _millisecondsLeft = DelayInMilliseconds;
        }

        protected void OnTimesUp(GameTime time)
        {
            _timesUp(this, new EventArgs<GameTime>(time));

            if (Repeat)
            {
                Enabled = true;
                Reset();
            }
        }
    }
}
