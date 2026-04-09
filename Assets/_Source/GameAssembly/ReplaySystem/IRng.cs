namespace GameAssembly.ReplaySystem
{
    public interface IRng
    {
        int Seed { get; }
        void Reset(int seed);
        float Range(float minInclusive, float maxInclusive);
        int Range(int minInclusive, int maxExclusive);
    }
}
