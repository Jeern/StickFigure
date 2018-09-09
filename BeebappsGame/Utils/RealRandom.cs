using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Beebapps.Game.Utils
{
    public class RealRandom
    {
        private readonly Random _random;
        private readonly int _min;
        private readonly int _max;

        private static readonly Dictionary<string, RealRandom> Dict = new Dictionary<string, RealRandom>();

        private RealRandom(int min, int max)
        {
            _random = new Random(GetSeed());
            _min = min;
            _max = max;
        }

        public static RealRandom Create(int min, int max)
        {
            string key = Key(min, max);
            if (Dict.ContainsKey(key))
            {
                return Dict[key];
            }

            var r = new RealRandom(min, max);
            Dict.Add(key, r);
            return r;
        }

        private static string Key(int min, int max)
        {
            var sb = new StringBuilder();
            sb.Append(min);
            sb.Append(';');
            sb.Append(max);
            return sb.ToString();
        }

        public int Next()
        {
            return _random.Next(_min, _max);
        }

        private int GetSeed()
        {
            var bytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return bytes[0] * bytes[1] * bytes[2] * bytes[3];
        }
    }
}
