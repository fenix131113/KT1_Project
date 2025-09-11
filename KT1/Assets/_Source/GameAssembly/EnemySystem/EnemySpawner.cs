using System.Collections;
using GameAssembly.Core;
using GameAssembly.Level.Data;
using GameAssembly.PlayerSystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private SpawnSettings spawnSettings;

        [Inject] private IPositionGetter _playerPosition;

        public void StartSpawn() => StartCoroutine(SpawnRoutine());

        private void SpawnEnemy()
        {
            Instantiate(enemyPrefab,
                new Vector3(_playerPosition.GetPosition().x + spawnSettings.SpawnDistanceFromPlayer, Random.Range(spawnSettings.MinY, spawnSettings.MaxY), 0),
                Quaternion.identity);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(spawnSettings.MinTimeBetweenSpawn, spawnSettings.MaxTimeBetweenSpawn));

                SpawnEnemy();
            }
        }
    }
}