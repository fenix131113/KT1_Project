using System;

namespace GameAssembly.ScoresSystem
{
    public class Score
    {
        public int ScoresCount { get; private set; }

        public event Action OnScoresChanged;
        
        public void AddScore()
        {
            ScoresCount++;
            OnScoresChanged?.Invoke();
        }
    }
}