using System;
using System.Collections;

namespace Beebapps.Game.Sequencers
{
    public abstract class Sequencer : ISequencer
    {
        protected int MinValue { get; set; }
        protected int MaxValue { get; set; }

        protected Sequencer(int minValue, int maxValue)
        {
            if(minValue >= maxValue && maxValue > 0)
                throw new ArgumentException(SequencerErrorMessage(), "minValue");

            MinValue = minValue;
            MaxValue = maxValue;
            Reset();
        }

        protected Sequencer(int maxValue) : this(0, maxValue) { }


        #region ISequencer Members

        protected string SequencerErrorMessage()
        {
            return string.Format("minValue: {0} must be smaller than maxValue: {1} in {2}", MinValue, MaxValue, GetType().Name);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
           //Der er intet at dispose
        }

        #endregion

        #region IEnumerator Members

        public int Current
        {
            get;
            protected set;
        }

        public abstract bool MoveNext();


        public virtual void Reset()
        {
            Current = MinValue-1;
            MoveNext();
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }
}
