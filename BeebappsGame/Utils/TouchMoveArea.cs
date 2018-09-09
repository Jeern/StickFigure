using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{
    /// <summary>
    /// Et TouchMoveArea kan returnerer en Delta vektor for i hvilken retning brugeren ønsker at bevæge skærmen
    /// </summary>
    public class TouchMoveArea : GameComponent
    {
        public FloatRectangle MoveArea { get; set; }

        private int _numberOfFingersAllowed;

        public Vector2 Delta
        {
            get; set;
        }
        private TouchableArea _touchableArea;
        private Vector2 _previousPosition;

        public TouchMoveArea(FloatRectangle moveArea, int numberOfFingersAllowed)
            : base(BeebappsGame.Current)
        {
            MoveArea = moveArea;
            _touchableArea = new TouchableArea(moveArea);
            _previousPosition = Misc.NonInitialized;
            _numberOfFingersAllowed = numberOfFingersAllowed;
        }

        private bool _touchCountWasTooBig;

        public override void Update(GameTime gameTime)
        {
            Delta = Vector2.Zero;
            if(!_touchCountWasTooBig && _touchableArea.IsPressed)
            {
                _previousPosition = _touchableArea.TouchPosition;
            }
            else if (!_touchCountWasTooBig && _touchableArea.IsMoving)
            {
                if (!_previousPosition.Equals(Misc.NonInitialized))
                {
                    Delta = (_touchableArea.TouchPosition - _previousPosition);
                }
                _previousPosition = _touchableArea.TouchPosition;
            }

            if (_touchableArea.TouchCount > _numberOfFingersAllowed)
            {
                _touchCountWasTooBig = true;
            }
            else if(_touchableArea.TouchCount == 0)
            {
                _touchCountWasTooBig = false;
            }
        }
    }
}
