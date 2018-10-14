using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StickFigure.Input
{
    public class InputDeviceExtended<TS> where TS : struct
    {
        private readonly Queue<InputStateExtended<TS>> _recordedStates = new Queue<InputStateExtended<TS>>();

        public Queue<InputStateExtended<TS>> RecordedStates
        {
            get { return _recordedStates; }
        }

        private readonly Stack<InputStateExtended<TS>> _statesForReuse = new Stack<InputStateExtended<TS>>();

        protected void EnqueueNewState(GameTime time, TS state)
        {
            if (!state.Equals(_currentState))
            {
                _currentState = state;
                _recordedStates.Enqueue(CreateState(time, state));
            }
        }

        private TS _currentState;
        public TS CurrentState
        {
            get { return _currentState; }
        }

        protected void DequeueOldStates(GameTime currentTime)
        {
            InputStateExtended<TS> state = null;
            if (_recordedStates.Count > 0)
            {
                state = _recordedStates.Peek();
            }
            if (state != null && state.StateTime < currentTime.TotalRealTime().Subtract(new TimeSpan(0, 0, 0, 0, InputDeviceConstants.ClickCountTimeMS)))
            {
                _statesForReuse.Push(_recordedStates.Dequeue());
                DequeueOldStates(currentTime);
            }
        }

        private InputStateExtended<TS> CreateState(GameTime time, TS state)
        {
            if (_statesForReuse.Count > 0)
            {
                //Reuses the object to fight of the GC
                InputStateExtended<TS> stateExt = _statesForReuse.Pop();
                stateExt.StateTime = time.TotalRealTime();
                stateExt.State = state;
                return stateExt;
            }
            return new InputStateExtended<TS>(time, state);
        }

        /// <summary>
        /// Deletes all the states in the queue and adds them to the reuse stack.
        /// Used when a Click event or similar succeededs to stop another click event to occur immediately after.
        /// </summary>
        private void FlushAllStates()
        {
            while (_recordedStates.Count > 0)
            {
                _statesForReuse.Push(_recordedStates.Dequeue());
            }
        }

        public void Reset()
        {
            FlushAllStates();
        }

        public virtual bool NothingPressed
        {
            get { return true; }
        }

    }
}
