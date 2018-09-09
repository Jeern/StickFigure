namespace Beebapps.Game.Sequencers
{
    public class MinMaxIterator : SequencedIterator<int>
    {
        public MinMaxIterator(Sequencer sequencer, int min, int max) : base(sequencer, new SequenceCreator().GetMinMax(min, max))
        {

        }
    }
}
