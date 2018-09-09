using System;

namespace Beebapps.Game.Utils
{
    public class Resetable<TX> where TX : struct
    {
        private bool _isSet;

        public void Reset()
        {
            if (!_isSet)
                throw new ArgumentNullException("You can only reset the type if it was previously set");

            Value = _originalValue;
        }

        public void Set()
        {
            if (!_isSet)
            {
                _isSet = true;
                _originalValue = Value;
            }
        }

        private TX _originalValue;

        public TX Value; 

        public static implicit operator TX(Resetable<TX> resetable)
        {
            return resetable.Value;
        }

        public static implicit operator Resetable<TX>(TX x)
        {
            var resetable = new Resetable<TX> {Value = x};
            resetable.Set();
            return resetable;
        }

    }
}
