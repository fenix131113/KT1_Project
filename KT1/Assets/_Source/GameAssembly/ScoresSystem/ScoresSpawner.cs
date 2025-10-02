using System.Collections;
using GameAssembly.Core;
using GameAssembly.Core.Data;
using GameAssembly.Level.Data;
using GameAssembly.PlayerSystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.ScoresSystem
{
    public class ScoresSpawner : MonoBehaviour
    {
        [SerializeField] private ScoreCollectable scorePrefab;
        [SerializeField] private SpawnSettings spawnSettings;

        [Inject] private IPositionGetter _player;
        [Inject] private LayersDataSO _layersDataSO;
        [Inject] private Score _score;

        public void StartSpawn() => StartCoroutine(SpawnRoutine());

        private void SpawnScore()
        {
            Instantiate(scorePrefab,
                new Vector3(_player.GetPosition().x + spawnSettings.SpawnDistanceFromPlayer, Random.Range(spawnSettings.MinY, spawnSettings.MaxY), 0),
                Quaternion.identity).Init(_score, _layersDataSO);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(spawnSettings.MinTimeBetweenSpawn, spawnSettings.MaxTimeBetweenSpawn));

                SpawnScore();
            }
        }
    }
}