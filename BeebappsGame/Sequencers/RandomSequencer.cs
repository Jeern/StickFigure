using Beebapps.Game.Utils;

namespace Beebapps.Game.Sequencers
{
    /// <summary>
    /// Don't use foreach the random sequence never stops
    /// </summary>
    public class RandomSequencer : Sequencer
    {
        private RealRandom _random;
        public RandomSequencer(int maxValue) : this(0, maxValue) {}

        public RandomSequencer(int minValue, int maxValue)
            : base(minValue, maxValue)
        {
        }

        public override bool MoveNext()
        {
            if(_random == null)
                _random = RealRandom.Create(MinValue, MaxValue);
            Current = _random.Next();
            return true;
        }

    }
}
