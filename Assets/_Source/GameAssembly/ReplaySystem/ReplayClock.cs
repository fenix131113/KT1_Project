using VContainer.Unity;

namespace GameAssembly.ReplaySystem
{
    public class ReplayClock : IFixedTickable
    {
        public const int TICK_RATE = 50;
        public int CurrentTick { get; private set; }

        public void FixedTick() => CurrentTick++;

        public void Reset() => CurrentTick = 0;
    }
}
