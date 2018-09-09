using System.Collections.Generic;

namespace Beebapps.Game.Utils
{
    public abstract class Animation<T>
    {
        protected int Frame { get; set; }
        public void Start()
        {
            IsStarted = true;
            IsFinished = false;
        }
        public void Reset()
        {
            IsStarted = false;
            IsFinished = false;
            Frame = -1;
        }
        public virtual void Update()
        {
            if (IsStarted)
            {
                Frame++;
            }
        }

        public bool IsStarted { get; private set; }
        public bool IsFinished { get; protected set; }

        public virtual T CurrentValue
        {
            get
            {
                if (Frame < 0)
                    return Elements[0];

                if (Frame > Elements.Count - 1)
                {
                    IsFinished = true;
                    return Elements[Elements.Count - 1];
                }

                return Elements[Frame];

            }
        }

        protected List<T> Elements { get; set; }
    }
}
