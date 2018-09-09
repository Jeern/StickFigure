using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Beebapps.Game.Input
{
    public class GamepadExtended : InputDeviceExtended<GamePadState>
    {
        private readonly bool _hasDeadZoneMode;
        private readonly PlayerIndex _playerIndex = PlayerIndex.One;
        private readonly GamePadDeadZone _gamePadDeadZone;

        public GamepadExtended(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
        }

        public GamepadExtended(PlayerIndex playerIndex, GamePadDeadZone gamePadDeadZone)
            : this(playerIndex)
        {
            _hasDeadZoneMode = true;
            _gamePadDeadZone = gamePadDeadZone;
        }

        private static Dictionary<PlayerIndex, GamepadExtended> _currentDictionary;
        private static Dictionary<string, GamepadExtended> _currentDictionaryWDeadZone;

        public static GamepadExtended Current(PlayerIndex index)
        {
            if (_currentDictionary == null)
            {
                _currentDictionary = new Dictionary<PlayerIndex, GamepadExtended>(4);
            }

            if (!_currentDictionary.ContainsKey(index))
            {
                _currentDictionary.Add(index, new GamepadExtended(index));
            }
            return _currentDictionary[index];
        }

        public static GamepadExtended Current(PlayerIndex index, GamePadDeadZone gamePadDeadZone)
        {
            if (_currentDictionaryWDeadZone == null)
            {
                _currentDictionaryWDeadZone = new Dictionary<string, GamepadExtended>(4);
            }

            string key = ComboKey(index, gamePadDeadZone);
            if (!_currentDictionaryWDeadZone.ContainsKey(key))
            {
                _currentDictionaryWDeadZone.Add(key, new GamepadExtended(index, gamePadDeadZone));
            }
            return _currentDictionaryWDeadZone[key];
        }

        private static string ComboKey(PlayerIndex index, GamePadDeadZone gamePadDeadZone)
        {
            return string.Format("{0};{1}", index.ToString(), gamePadDeadZone.ToString());
        }

        public GamePadState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            GamePadState state;
            if (_hasDeadZoneMode)
            {
                state = GamePad.GetState(_playerIndex, _gamePadDeadZone);
            }
            else
            {
                state = GamePad.GetState(_playerIndex);
            }
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(Buttons checkButton, int requiredCount)
        {
            bool buttonWasDown = false;
            int count = 0;
            foreach (InputStateExtended<GamePadState> stateExt in RecordedStates)
            {
                if (buttonWasDown && stateExt.State.IsButtonUp(checkButton))
                {
                    count++;
                    if (count >= requiredCount)
                    {
                        Reset();
                        return true;
                    }
                }
                buttonWasDown = stateExt.State.IsButtonDown(checkButton);
            }
            return false;
        }

        public bool WasSingleClick(Buttons checkButton)
        {
            return ClickCount(checkButton, 1);
        }

        public static bool WasSingleClickAny(Buttons checkButton)
        {
            return
                Current(PlayerIndex.One).WasSingleClick(checkButton) ||
                Current(PlayerIndex.Two).WasSingleClick(checkButton) ||
                Current(PlayerIndex.Three).WasSingleClick(checkButton) ||
                Current(PlayerIndex.Four).WasSingleClick(checkButton);
        }

        public bool WasDoubleClick(Buttons checkButton)
        {
            return ClickCount(checkButton, 2);
        }

        public override bool NothingPressed
        {
            get
            {
                return GetButtonValues().All(button => !CurrentState.IsButtonDown(button));
            }
        }

        private IEnumerable<Buttons> GetButtonValues()
        {
            foreach (Buttons button in (Buttons[])Enum.GetValues(typeof(Buttons)))
                yield return button;
        }
    }
}

