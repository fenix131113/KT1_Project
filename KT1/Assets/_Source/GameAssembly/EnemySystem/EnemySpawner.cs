using System.Collections;
using GameAssembly.PlayerSystem;
using UnityEngine;
using VContainer;

namespace GameAssembly.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [SerializeField] private float minTimeBetweenSpawn;
        [SerializeField] private float maxTimeBetweenSpawn;
        [SerializeField] private float spawnDistanceFromPlayer;

        [Inject] private Player _player;

        public void StartSpawn() => StartCoroutine(SpawnRoutine());

        private void SpawnEnemy()
        {
            Instantiate(enemyPrefab,
                new Vector3(_player.transform.position.x + spawnDistanceFromPlayer, Random.Range(minY, maxY), 0),
                Quaternion.identity);
        }

        // ReSharper disable once IteratorNeverReturns
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn));

                SpawnEnemy();
            }
        }
    }
}