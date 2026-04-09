using System;
using System.Collections.Generic;

namespace GameAssembly.ReplaySystem
{
    [Serializable]
    public class ReplayData
    {
        public ReplayHeader header = new();
        public List<ReplayCommand> commands = new();
        public int durationTicks;
    }
}
