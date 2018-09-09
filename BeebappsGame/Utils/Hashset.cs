using System.Collections.Generic;

namespace Beebapps.Game.Utils
{
    public class Hashset<T> : Dictionary<T,T>, IEnumerable<T>
    {
        public bool Contains(T t)
        {
            return ContainsKey(t);
        }

        public void Add(T t)
        {
            Add(t, t);
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return Keys.GetEnumerator();
        }
    }
}
