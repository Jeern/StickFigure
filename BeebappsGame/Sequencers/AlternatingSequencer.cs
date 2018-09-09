namespace Beebapps.Game.Sequencers
{
    /// <summary>
    /// Alternates sequence again and again. Don't use foreach
    /// </summary>
    public class AlternatingSequencer : Sequencer
    {
        private bool _goingForward = true;

        public AlternatingSequencer(int maxValue) : this(0, maxValue) {}

        public AlternatingSequencer(int minValue, int maxValue)
            : base(minValue, maxValue)
        {
        }

        public override bool MoveNext()
        {
            if (Current == MaxValue && _goingForward)
            {
                _goingForward = false;
            }
            else if (Current == MinValue && !_goingForward)
            {
                _goingForward = true;
            }

            if (Current < MaxValue && _goingForward)
            {
                Current++;
            }
            else if (Current > MinValue && !_goingForward)
            {
                Current--;
            }
            return true;
        }
    }
}
