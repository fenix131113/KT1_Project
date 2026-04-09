using System;

namespace GameAssembly.ReplaySystem
{
    public class DeterministicRng : IRng
    {
        private Random _random = new();

        public int Seed { get; private set; }

        public DeterministicRng() => Reset(UnityEngine.Random.Range(int.MinValue, int.MaxValue));

        public void Reset(int seed)
        {
            Seed = seed;
            _random = new Random(seed);
        }

        public float Range(float minInclusive, float maxInclusive)
        {
            if (maxInclusive <= minInclusive)
                return minInclusive;

            return (float)(_random.NextDouble() * (maxInclusive - minInclusive) + minInclusive);
        }

        public int Range(int minInclusive, int maxExclusive) =>
            maxExclusive <= minInclusive ? minInclusive : _random.Next(minInclusive, maxExclusive);
    }
}