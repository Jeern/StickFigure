using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Beebapps.Game.Sequencers
{
    public class SequencedIterator<T> : IEnumerator<T>
    {
        private readonly List<T> _listOfItems;
        private readonly Sequencer _sequencer;

        public SequencedIterator(Sequencer sequencer, List<T> listOfItems)
        {
            _listOfItems = listOfItems;
            _sequencer = sequencer;
        }

        public SequencedIterator(Sequencer sequencer, params T[] items)
        {
            _listOfItems = items.ToList();
            _sequencer = sequencer;
        }

        #region IEnumerator<T> Members

        public T Current
        {
            get { return _listOfItems[_sequencer.Current]; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //Nothing to dispose.
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return _sequencer.MoveNext();
        }

        public void Reset()
        {
            _sequencer.Reset();
        }

        public int CurrentIndex
        {
            get {
                return _sequencer.Current; } 
        }

        #endregion
    }
}
