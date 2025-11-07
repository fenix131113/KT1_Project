using System;
using GameAssembly.GP;
using UnityEngine;
using VContainer.Unity;

namespace GameAssembly.ScoresSystem
{
    public class Score : IInitializable
    {
        public int ScoresCount { get; private set; }

        public event Action OnScoresChanged;

        public void Initialize() => CloudSaveService.LoadData(SetScores);

        private void SetScores(int score) => ScoresCount = score;

        public void AddScore()
        {
            ScoresCount++;
            PlayerPrefs.SetInt(CloudSaveService.SCORE_KEY, ScoresCount);
            CloudSaveService.SaveData(CloudSaveService.SCORE_KEY, ScoresCount);
            OnScoresChanged?.Invoke();
        }
    }
}