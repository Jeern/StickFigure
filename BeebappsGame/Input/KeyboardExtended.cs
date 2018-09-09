#if WINDOWS
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Input
{
    public class KeyboardExtended : InputDeviceExtended<KeyboardState>
    {
        private readonly bool _hasPlayerIndex;
        private readonly PlayerIndex _playerIndex = PlayerIndex.One;

        public KeyboardExtended()
        {

        }

        public KeyboardExtended(PlayerIndex playerIndex)
            : this()
        {
            _hasPlayerIndex = true;
            _playerIndex = playerIndex;
        }

        private static KeyboardExtended _current;
        private static Dictionary<PlayerIndex, KeyboardExtended> _currentDictionary;

        public static KeyboardExtended Current
        {
            get { return _current ?? (_current = new KeyboardExtended()); }
        }

        public static KeyboardExtended CurrentIndexed(PlayerIndex index)
        {
            if (_currentDictionary == null)
            {
                _currentDictionary = new Dictionary<PlayerIndex, KeyboardExtended>(4);
            }

            if (!_currentDictionary.ContainsKey(index))
            {
                _currentDictionary.Add(index, new KeyboardExtended(index));
            }
            return _currentDictionary[index];
        }

        public KeyboardState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            KeyboardState state;
            if (_hasPlayerIndex)
            {
                state = Keyboard.GetState(_playerIndex);
            }
            else
            {
                state = Keyboard.GetState();
            }
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(Keys checkKey, int requiredCount)
        {
            bool keyWasDown = false;
            int count = 0;
            foreach (InputStateExtended<KeyboardState> stateExt in RecordedStates)
            {
                if (keyWasDown && stateExt.State.IsKeyUp(checkKey))
                {
                    count++;
                    if (count >= requiredCount)
                    {
                        Reset();
                        return true;
                    }
                }
                keyWasDown = stateExt.State.IsKeyDown(checkKey);
            }
            return false;
        }

        public bool WasSingleClick(Keys checkKey)
        {
            return ClickCount(checkKey, 1);
        }

        public bool WasDoubleClick(Keys checkKey)
        {
            return ClickCount(checkKey, 2);
        }

        public override bool NothingPressed
        {
            get
            {
                return CurrentState.GetPressedKeys().Length == 0;
            }
        }
    }
}
#endif