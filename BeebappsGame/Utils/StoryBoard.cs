using System;

namespace Beebapps.Game.Utils
{
    public abstract class StoryBoard
    {
        private readonly Func<int> _getFrameSpeed;

        protected StoryBoard(Func<int> getFrameSpeed)
        {
            _getFrameSpeed = getFrameSpeed;
        }

        protected int Frame { get; set; }
        public virtual void Start()
        {
            IsStarted = true;
        }

        public virtual void Reset()
        {
            IsStarted = false;
            IsFinished = false;
            Frame = -1;
        }
        public virtual void Update()
        {
            if (IsStarted)
            {
                Frame += _getFrameSpeed();
            }
        }

        public bool IsStarted { get; private set; }
        public bool IsFinished { get; protected set; }
    }
}
