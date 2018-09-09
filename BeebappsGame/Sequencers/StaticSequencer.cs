namespace Beebapps.Game.Sequencers
{
    public class StaticSequencer : Sequencer
    {
        public StaticSequencer()
            : this(0)
        {
        }

        public StaticSequencer(int value)
            : base(value)
        {
        }

        public override bool MoveNext()
        {
            Current = MaxValue;
            return false;
        }
    }
}
