using System;

namespace GameAssembly.ReplaySystem
{
    [Serializable]
    public struct ReplayCommand
    {
        public int tick;
        public ReplayCommandType type;
        public bool boolValue;
        public float floatValue;
        public float floatValue2;
        public int intValue;

        public static ReplayCommand JumpHold(int tick, bool isPressed)
        {
            return new ReplayCommand
            {
                tick = tick,
                type = ReplayCommandType.JUMP_HOLD,
                boolValue = isPressed
            };
        }

        public static ReplayCommand EnemySpawn(int tick, float x, float y)
        {
            return new ReplayCommand
            {
                tick = tick,
                type = ReplayCommandType.ENEMY_SPAWN,
                floatValue = x,
                floatValue2 = y
            };
        }

        public static ReplayCommand ScoreSpawn(int tick, float x, float y)
        {
            return new ReplayCommand
            {
                tick = tick,
                type = ReplayCommandType.SCORE_SPAWN,
                floatValue = x,
                floatValue2 = y
            };
        }
    }
}
