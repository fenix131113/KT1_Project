using System;

namespace GameAssembly.ReplaySystem
{
    [Serializable]
    public class ReplayHeader
    {
        public int version;
        public int tickRate;
        public int seed;
        public string platform;
    }
}
